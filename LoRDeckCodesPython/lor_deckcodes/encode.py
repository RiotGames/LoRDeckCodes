from itertools import product
from base64 import b32encode
from io import BytesIO
from typing import List

from lor_deckcodes.utils import write_varint
from lor_deckcodes.constants import FORMAT_VERSION, faction_mapping


def _encode_card_block(data_stream: BytesIO, cards: List[object]) -> None:
    set_faction_combinations = list(product(
        set([card.set for card in cards]),
        set([card.faction for card in cards])))
    write_varint(data_stream, len(set_faction_combinations))

    set_faction_combinations = sorted(set_faction_combinations,
                                      key=lambda l: len([card for card in cards if card.faction == l[1]]))
    for card_set, faction in set_faction_combinations:
        faction_cards = [card for card in cards if card.faction == faction]
        write_varint(data_stream, len(faction_cards))
        write_varint(data_stream, card_set)
        write_varint(data_stream, faction_mapping.get(faction))
        for faction_card in faction_cards:
            write_varint(data_stream, faction_card.card_id)


def encode_deck(cards: List[object]) -> str:
    data = BytesIO()
    write_varint(data, FORMAT_VERSION)

    # 3 card copies
    three_copies = list(filter(lambda x: x.count == 3, cards))
    _encode_card_block(data, three_copies)
    # 2 card copies
    two_copies = list(filter(lambda x: x.count == 2, cards))
    _encode_card_block(data, two_copies)
    # 1 card copies
    one_copies = list(filter(lambda x: x.count == 1, cards))
    _encode_card_block(data, one_copies)

    data.seek(0)
    return b32encode(data.read()).decode().replace('=', '')
