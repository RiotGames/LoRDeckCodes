# Python Implementation

## Install

python >=3.5 required
pip >= 19.0.0
```
pip install git+https://github.com/RiotGames/LoRDeckCodes/#subdirectory=LoRDeckCodesPython
```
todo:
Add package to pypi

## Usage

Ever expanding rich API with method to display cards conveniently

```python
from lor_deckcodes import LoRDeck, CardCodeAndCount


# Decoding
deck = LoRDeck.from_deckcode('CEBAIAIFB4WDANQIAEAQGDAUDAQSIJZUAIAQCBIFAEAQCBAA')

# list all cards with card format 3:01SI001
list(deck)

card = deck.cards[0] # instance of CardCodeAndCount
card.faction # SI/FR...
card.card_id # 111
card.set # 01


# Encoding
# These are equivalent
deck = LoRDeck(['3:01SI015', '3:01SI044'])
deck = LoRDeck([
    CardCodeAndCount.from_card_string('3:01SI015'),
    CardCodeAndCount('01SI015', 3)]
)
# returns encoded string
deck.encode()
```
