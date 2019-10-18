from lor_deckcodes.encode import encode_deck
from lor_deckcodes.decode import decode_deck


class CardCodeAndCount:
    @classmethod
    def from_card_string(cls, card_string: str):
        count, card_code = card_string.split(':')
        return cls(card_code, int(count))

    def __init__(self, card_code: str, count: int):
        self.card_code = card_code
        self.count = count

    def __str__(self):
        return f"{self.count}:{self.card_code}"

    @property
    def set(self) -> int:
        return int(self.card_code[:2])

    @property
    def faction(self) -> str:
        return self.card_code[2:4]

    @property
    def card_id(self) -> int:
        return int(self.card_code[4:])


class LoRDeck:
    @classmethod
    def from_deckcode(cls, deckcode: str):
        return cls(decode_deck(deckcode))

    def __init__(self, cards=None):
        if cards:
            self.cards = [card if isinstance(card, CardCodeAndCount) else CardCodeAndCount.from_card_string(card)
                          for card in cards]
        else:
            self.cards = []

    def __iter__(self):
        self._n = 0
        return self

    def __next__(self):
        try:
            c = self.cards[self._n]
        except IndexError:
            raise StopIteration
        self._n += 1
        return str(c)

    def encode(self) -> str:
        return encode_deck(self.cards)