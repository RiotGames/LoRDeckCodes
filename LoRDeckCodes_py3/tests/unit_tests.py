import unittest
import sys


class TestDeckEncodeDecode(unittest.TestCase):
    # Tests the encoding of a set of hard coded decks in DeckCodesTestData.txt
    def test_encode_decode_recommended_decks(self):
        codes = []
        decks = []

        with open('../../LoRDeckCodes_Tests/DeckCodesTestData.txt',
                  encoding='utf-8-sig') as test_decks:
            current_line = test_decks.readline()  # read deck code
            while current_line:
                codes.append(current_line.rstrip('\n'))
                new_deck = []

                current_line = test_decks.readline()  # read card string
                while current_line not in ['\n', '']:
                    parts = current_line.split(':')
                    new_deck.append(CardCodeAndCount(parts[1].rstrip('\n'),
                                                     int(parts[0])))

                    # try and read card string
                    current_line = test_decks.readline()

                decks.append(new_deck)
                current_line = test_decks.readline()  # try and read deck code

        # Encode each test deck and ensure it's equal to the correct string.
        # Then decode and ensure the deck is unchanged.
        for i in range(len(decks)):
            encoded = lor_deck_encoder.get_code_from_deck(decks[i])
            self.assertEqual(codes[i], encoded)

            decoded = lor_deck_encoder.get_deck_from_code(encoded)
            self.assertTrue(verify_rehydration(decks[i], decoded))

    def test_small_deck(self):
        deck = []
        deck.append(CardCodeAndCount("01DE002", 1))

        code = lor_deck_encoder.get_code_from_deck(deck)
        decoded = lor_deck_encoder.get_deck_from_code(code)
        self.assertTrue(verify_rehydration(deck, decoded))

    def test_large_deck(self):
        deck = []
        deck.append(CardCodeAndCount("01DE002", 3))
        deck.append(CardCodeAndCount("01DE003", 3))
        deck.append(CardCodeAndCount("01DE004", 3))
        deck.append(CardCodeAndCount("01DE005", 3))
        deck.append(CardCodeAndCount("01DE006", 3))
        deck.append(CardCodeAndCount("01DE007", 3))
        deck.append(CardCodeAndCount("01DE008", 3))
        deck.append(CardCodeAndCount("01DE009", 3))
        deck.append(CardCodeAndCount("01DE010", 3))
        deck.append(CardCodeAndCount("01DE011", 3))
        deck.append(CardCodeAndCount("01DE012", 3))
        deck.append(CardCodeAndCount("01DE013", 3))
        deck.append(CardCodeAndCount("01DE014", 3))
        deck.append(CardCodeAndCount("01DE015", 3))
        deck.append(CardCodeAndCount("01DE016", 3))
        deck.append(CardCodeAndCount("01DE017", 3))
        deck.append(CardCodeAndCount("01DE018", 3))
        deck.append(CardCodeAndCount("01DE019", 3))
        deck.append(CardCodeAndCount("01DE020", 3))
        deck.append(CardCodeAndCount("01DE021", 3))

        code = lor_deck_encoder.get_code_from_deck(deck)
        decoded = lor_deck_encoder.get_deck_from_code(code)
        self.assertTrue(verify_rehydration(deck, decoded))

    def test_deck_with_counts_more_than_3_small(self):
        deck = []
        deck.append(CardCodeAndCount("01DE002", 4))

        code = lor_deck_encoder.get_code_from_deck(deck)
        decoded = lor_deck_encoder.get_deck_from_code(code)
        self.assertTrue(verify_rehydration(deck, decoded))

    def test_deck_with_counts_more_than_3_large(self):
        deck = []
        deck.append(CardCodeAndCount("01DE002", 3))
        deck.append(CardCodeAndCount("01DE003", 3))
        deck.append(CardCodeAndCount("01DE004", 3))
        deck.append(CardCodeAndCount("01DE005", 3))
        deck.append(CardCodeAndCount("01DE006", 4))
        deck.append(CardCodeAndCount("01DE007", 5))
        deck.append(CardCodeAndCount("01DE008", 6))
        deck.append(CardCodeAndCount("01DE009", 7))
        deck.append(CardCodeAndCount("01DE010", 8))
        deck.append(CardCodeAndCount("01DE011", 9))
        deck.append(CardCodeAndCount("01DE012", 3))
        deck.append(CardCodeAndCount("01DE013", 3))
        deck.append(CardCodeAndCount("01DE014", 3))
        deck.append(CardCodeAndCount("01DE015", 3))
        deck.append(CardCodeAndCount("01DE016", 3))
        deck.append(CardCodeAndCount("01DE017", 3))
        deck.append(CardCodeAndCount("01DE018", 3))
        deck.append(CardCodeAndCount("01DE019", 3))
        deck.append(CardCodeAndCount("01DE020", 3))
        deck.append(CardCodeAndCount("01DE021", 3))

        code = lor_deck_encoder.get_code_from_deck(deck)
        decoded = lor_deck_encoder.get_deck_from_code(code)
        self.assertTrue(verify_rehydration(deck, decoded))

    def test_single_card_40_times(self):
        deck = []
        deck.append(CardCodeAndCount("01DE002", 40))

        code = lor_deck_encoder.get_code_from_deck(deck)
        decoded = lor_deck_encoder.get_deck_from_code(code)
        self.assertTrue(verify_rehydration(deck, decoded))

    def test_worst_case_length(self):
        deck = []
        deck.append(CardCodeAndCount("01DE002", 4))
        deck.append(CardCodeAndCount("01DE003", 4))
        deck.append(CardCodeAndCount("01DE004", 4))
        deck.append(CardCodeAndCount("01DE005", 4))
        deck.append(CardCodeAndCount("01DE006", 4))
        deck.append(CardCodeAndCount("01DE007", 5))
        deck.append(CardCodeAndCount("01DE008", 6))
        deck.append(CardCodeAndCount("01DE009", 7))
        deck.append(CardCodeAndCount("01DE010", 8))
        deck.append(CardCodeAndCount("01DE011", 9))
        deck.append(CardCodeAndCount("01DE012", 4))
        deck.append(CardCodeAndCount("01DE013", 4))
        deck.append(CardCodeAndCount("01DE014", 4))
        deck.append(CardCodeAndCount("01DE015", 4))
        deck.append(CardCodeAndCount("01DE016", 4))
        deck.append(CardCodeAndCount("01DE017", 4))
        deck.append(CardCodeAndCount("01DE018", 4))
        deck.append(CardCodeAndCount("01DE019", 4))
        deck.append(CardCodeAndCount("01DE020", 4))
        deck.append(CardCodeAndCount("01DE021", 4))

        code = lor_deck_encoder.get_code_from_deck(deck)
        decoded = lor_deck_encoder.get_deck_from_code(code)
        self.assertTrue(verify_rehydration(deck, decoded))

    def test_order_should_not_matter_1(self):
        deck_1 = []
        deck_1.append(CardCodeAndCount("01DE002", 1))
        deck_1.append(CardCodeAndCount("01DE003", 2))
        deck_1.append(CardCodeAndCount("02DE003", 3))

        deck_2 = []
        deck_2.append(CardCodeAndCount("01DE003", 2))
        deck_2.append(CardCodeAndCount("02DE003", 3))
        deck_2.append(CardCodeAndCount("01DE002", 1))

        code_1 = lor_deck_encoder.get_code_from_deck(deck_1)
        code_2 = lor_deck_encoder.get_code_from_deck(deck_2)

        self.assertEqual(code_1, code_2)

        deck_3 = []
        deck_3.append(CardCodeAndCount("01DE002", 4))
        deck_3.append(CardCodeAndCount("01DE003", 2))
        deck_3.append(CardCodeAndCount("02DE003", 3))

        deck_4 = []
        deck_4.append(CardCodeAndCount("01DE003", 2))
        deck_4.append(CardCodeAndCount("02DE003", 3))
        deck_4.append(CardCodeAndCount("01DE002", 4))

        code_3 = lor_deck_encoder.get_code_from_deck(deck_3)
        code_4 = lor_deck_encoder.get_code_from_deck(deck_4)

        self.assertEqual(code_3, code_4)

    def test_order_should_not_matter_2(self):
        # importantly this order test includes more than 1 card with counts >3,
        # which are sorted by card code and appended to the <=3 encodings.
        deck_1 = []
        deck_1.append(CardCodeAndCount("01DE002", 4))
        deck_1.append(CardCodeAndCount("01DE003", 2))
        deck_1.append(CardCodeAndCount("02DE003", 3))
        deck_1.append(CardCodeAndCount("01DE004", 5))

        deck_2 = []
        deck_2.append(CardCodeAndCount("01DE004", 5))
        deck_2.append(CardCodeAndCount("01DE003", 2))
        deck_2.append(CardCodeAndCount("02DE003", 3))
        deck_2.append(CardCodeAndCount("01DE002", 4))

        code_1 = lor_deck_encoder.get_code_from_deck(deck_1)
        code_2 = lor_deck_encoder.get_code_from_deck(deck_2)

        self.assertEqual(code_1, code_2)

    def test_bad_card_codes(self):
        deck = []
        deck.append(CardCodeAndCount("01DE02", 1))

        try:
            code = lor_deck_encoder.get_code_from_deck(deck)
            self.fail()
        except ValueError:
            pass
        except:
            self.fail()

        deck = []
        deck.append(CardCodeAndCount("01XX002", 1))

        try:
            code = lor_deck_encoder.get_code_from_deck(deck)
            self.fail()
        except ValueError:
            pass
        except:
            self.fail()

        deck = []
        deck.append(CardCodeAndCount("0DE002", 1))

        try:
            code = lor_deck_encoder.get_code_from_deck(deck)
            self.fail()
        except ValueError:
            pass
        except:
            self.fail()

    def test_bad_count(self):
        deck = []
        deck.append(CardCodeAndCount("01DE002", 0))

        try:
            code = lor_deck_encoder.get_code_from_deck(deck)
            self.fail()
        except ValueError:
            pass
        except:
            self.fail()

        deck = []
        deck.append(CardCodeAndCount("01DE002", -1))

        try:
            code = lor_deck_encoder.get_code_from_deck(deck)
            self.fail()
        except ValueError:
            pass
        except:
            self.fail()

    def test_garbage_decoding(self):
        bad_encoding_not_base32 = "I'm no card code!"
        bad_encoding_32 = "ABCDEFG"
        bad_encoding_empty = ""

        try:
            deck = lor_deck_encoder.get_deck_from_code(bad_encoding_not_base32)
            self.fail()
        except ValueError:
            pass
        except:
            self.fail()

        try:
            deck = lor_deck_encoder.get_deck_from_code(bad_encoding_32)
            self.fail()
        except ValueError:
            pass
        except:
            self.fail()

        try:
            deck = lor_deck_encoder.get_deck_from_code(bad_encoding_empty)
            self.fail()
        except:
            pass


def verify_rehydration(d, rehydrated_list):
    if len(d) != len(rehydrated_list):
        return False

    for cd in rehydrated_list:
        found = False
        for cc in d:
            if cc.card_code == cd.card_code and cc.count == cd.count:
                found = True
                break
        if not found:
            return False

    return True


if __name__ == '__main__':
    sys.path.insert(0, '../')
    from card_code_and_count import CardCodeAndCount
    import lor_deck_encoder
    unittest.main()
