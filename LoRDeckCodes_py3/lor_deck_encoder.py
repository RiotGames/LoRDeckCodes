from base64 import b32encode, b32decode

from card_code_and_count import CardCodeAndCount
import varint_translator

CARD_CODE_LENGTH = 7
faction_to_int = {"DE": 0, "FR": 1, "IO": 2, "NX": 3, "PZ": 4, "SI": 5}
int_to_faction = {0: "DE", 1: "FR", 2: "IO", 3: "NX", 4: "PZ", 5: "SI"}
MAX_KNOWN_VERSION = 1


def get_deck_from_code(code):
    result = []

    try:
        while len(code) % 8:
            code += "="
        bytes_ = b32decode(code)
    except Exception:
        raise ValueError("Invalid deck code")

    byte_list = bytearray(bytes_)

    # grab format and version
    format_ = bytes_[0] >> 4
    version = bytes_[0] & 15
    byte_list.pop(0)

    if version > MAX_KNOWN_VERSION:
        raise ValueError("The provided code requires a higher version \
                         of this library; please update.")

    for i in range(3, 0, -1):
        byte_list, num_group_ofs = varint_translator.pop_varint(byte_list)

        for j in range(num_group_ofs):
            byte_list, num_ofs_in_this_group = varint_translator.pop_varint(
                                                                     byte_list)
            byte_list, set_ = varint_translator.pop_varint(byte_list)
            byte_list, faction = varint_translator.pop_varint(byte_list)

            for k in range(num_ofs_in_this_group):
                byte_list, card = varint_translator.pop_varint(byte_list)

                set_string = str(set_).zfill(2)
                faction_string = int_to_faction[faction]
                card_string = str(card).zfill(3)

                new_entry = CardCodeAndCount(set_string + faction_string
                                             + card_string, i)
                result.append(new_entry)

    # the remainder of the deck code is comprised
    # of entries for cards with counts >= 4.
    # this will only happen in Limited and special game modes.
    # the encoding is simply [count] [cardcode]
    while byte_list:
        byte_list, four_plus_count = varint_translator.pop_varint(byte_list)
        byte_list, four_plus_set = varint_translator.pop_varint(byte_list)
        byte_list, four_plus_faction = varint_translator.pop_varint(byte_list)
        byte_list, four_plus_number = varint_translator.pop_varint(byte_list)

        four_plus_set_string = str(four_plus_set).zfill(2)
        four_plus_faction_string = int_to_faction[four_plus_faction]
        four_plus_number_string = str(four_plus_number).zfill(3)

        new_entry = CardCodeAndCount(four_plus_set_string
                                     + four_plus_faction_string
                                     + four_plus_number_string,
                                     four_plus_count)
        result.append(new_entry)

    return result


def get_code_from_deck(deck):
    result = b32encode(get_deck_code_bytes(deck)).replace(b'=', b'')
    return result.decode()  # return a string and not a byte string


def get_deck_code_bytes(deck):
    result = bytearray()

    if not valid_card_codes_and_count(deck):
        raise ValueError("The provided deck contains invalid card codes.")

    format_and_version = bytes([17])  # i.e. 00010001
    result.extend(format_and_version)

    of_3 = []
    of_2 = []
    of_1 = []
    of_n = []

    for ccc in deck:
        if ccc.count == 3:
            of_3.append(ccc)
        elif ccc.count == 2:
            of_2.append(ccc)
        elif ccc.count == 1:
            of_1.append(ccc)
        elif ccc.count < 1:
            raise ValueError("Invalid count of " + ccc.count +
                             " for card " + ccc.card_code)
        else:
            of_n.append(ccc)

    # build the lists of set and faction combinations
    # within the groups of similar card counts
    grouped_of_3s = get_grouped_ofs(of_3)
    grouped_of_2s = get_grouped_ofs(of_2)
    grouped_of_1s = get_grouped_ofs(of_1)

    # to ensure that the same decklist in any order
    # produces the same code, do some sorting
    grouped_of_3s = sort_group_of(grouped_of_3s)
    grouped_of_2s = sort_group_of(grouped_of_2s)
    grouped_of_1s = sort_group_of(grouped_of_1s)

    # Nofs (since rare) are simply sorted by the card code
    # there's no optimiziation based upon the card count
    of_n = sorted(of_n, key=lambda c: c.card_code)

    # Encode
    encode_group_of(result, grouped_of_3s)
    encode_group_of(result, grouped_of_2s)
    encode_group_of(result, grouped_of_1s)

    # Cards with 4+ counts are handled differently:
    # simply [count] [card code] for each
    encode_n_ofs(result, of_n)

    return result


def encode_n_ofs(bytes_, n_ofs):
    for ccc in n_ofs:
        bytes_.extend(varint_translator.get_varint(ccc.count))

        set_number, faction_code, card_number = parse_card_code(ccc.card_code)
        faction_number = faction_to_int[faction_code]

        bytes_.extend(varint_translator.get_varint(set_number))
        bytes_.extend(varint_translator.get_varint(faction_number))
        bytes_.extend(varint_translator.get_varint(card_number))


# The sorting convention of this encoding scheme is
# First by the number of set/faction combinations in each top-level list
# Second by the alphanumeric order of the card codes within those lists.
def sort_group_of(group_of):
    group_of = sorted(group_of, key=lambda l: len(l))

    for i in range(len(group_of)):
        group_of[i] = sorted(group_of[i], key=lambda c: c.card_code)

    return group_of


def parse_card_code(code):
    set_, faction, number = int(code[0:2]), code[2:4], int(code[4:7])
    return set_, faction, number


def get_grouped_ofs(list_):
    result = []
    while list_:
        current_set = []

        # get info from first
        first_card_code = list_[0].card_code
        set_number, faction_code, card_number = parse_card_code(
                                                               first_card_code)

        # now add that to our new list, remove from old
        current_set.append(list_[0])
        list_.pop(0)

        # sweep through rest of list
        # and grab entries that should live with our first one.
        # matching means same set and faction
        # we are already assured the count matches from previous grouping.
        for i in range(len(list_)):
            current_card_code = list_[i].card_code
            current_set_number = int(current_card_code[0:2])
            current_faction_code = current_card_code[2:4]

            if current_set_number == set_number \
               and current_faction_code == faction_code:
                current_set.append(list_[i])

        for card in current_set[1:]:
            list_.remove(card)

        result.append(current_set)
    return result


def encode_group_of(bytes_, group_of):
    bytes_.extend(varint_translator.get_varint(len(group_of)))

    for current_list in group_of:
        # how many cards in current group?
        bytes_.extend(varint_translator.get_varint(len(current_list)))

        # what is this group, as identified by a set and faction pair
        current_card_code = current_list[0].card_code

        current_set_number, current_faction_code, _ = parse_card_code(
                                                             current_card_code)
        current_faction_number = faction_to_int[current_faction_code]

        bytes_.extend(varint_translator.get_varint(current_set_number))
        bytes_.extend(varint_translator.get_varint(current_faction_number))

        # what are the cards, as identified by the third section of card code
        # only now, within this group?
        for cd in current_list:
            code = cd.card_code
            sequence_number = int(code[4:7])
            bytes_.extend(varint_translator.get_varint(sequence_number))


def valid_card_codes_and_count(deck):
    for ccc in deck:
        if len(ccc.card_code) != CARD_CODE_LENGTH:
            return False

        try:
            int(ccc.card_code[0:2])
        except ValueError:
            return False

        if ccc.card_code[2:4] not in faction_to_int.keys():
            return False

        try:
            int(ccc.card_code[4:7])
        except ValueError:
            return False

        if ccc.count < 1:
            return False

    return True
