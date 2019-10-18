from unittest import TestCase
from lor_deckcodes.models import LoRDeck
import os


class BaseTestDeckcode(TestCase):
    def setUp(self):
        with open(f'{os.path.dirname(os.path.abspath(__file__))}/test_data.txt', 'rb') as f:
            deck_blocks = f.read().decode().split('\n\n')

        self.decks = {}
        for deck_block in deck_blocks:
            deckcode = deck_block.split('\n')[0]
            card_list = deck_block.split('\n')[1:]
            self.decks[deckcode] = LoRDeck(card_list)


class TestDeckcodeDecode(BaseTestDeckcode):
    def test_data_decode(self):
        for deckcode, card_list in self.decks.items():
            self.assertListEqual(list(LoRDeck.from_deckcode(deckcode)), list(card_list))


class TestDeckcodeEncode(BaseTestDeckcode):
    def test_data_decode(self):
        for card_list in self.decks.values():
            encoded_string = card_list.encode()
            decoded_string = LoRDeck.from_deckcode(encoded_string)
            self.assertListEqual(sorted(list(decoded_string)), sorted(list(card_list)))
