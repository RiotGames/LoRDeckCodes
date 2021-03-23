LoRDeckCodes
============

The LorDeckCodes library can be used to encode/decode Legends of Runeterra decks to/from simple strings. Below is an example code for a Ionia/Piltover & Zaun deck.
```
CEAAECABAQJRWHBIFU2DOOYIAEBAMCIMCINCILJZAICACBANE4VCYBABAILR2HRL
```
These strings can be used to share decks across Legends of Runeterra clients. Just remember: you can't netdeck skill.

## Cards & Decks

Every Legends of Runeterra card has a corresponding card code. Card codes are seven character strings comprised of two characters for card set, two characters for faction identifier, and three characters for card number. 

```
01DE123
│ │ └ card number - 123
│ └ faction - DE
└ set - 01
```

The deck code library accepts a Legends of Runeterra deck as a list of `CardCodeAndCount` objects. This is simply the code and an associated integer for the number of occurences of the card in the deck.

## Process
Decks are encoding via arranging VarInts (big endian) into an array and then base 32 encoding into a string.

All encodings begin with 4 bits for format and 4 bits for version.

| Format | Version | Date | Patch | About |
| ------ | ------- | ---- | ----- | ----- |
| 1 | 1 | Oct 18, 2019 | - | Closed alpha. Supports original set. |
| 1 | 2 | April 28, 2020 | [1.0](https://playruneterra.com/en-us/news/patch-1-0-notes/) | Launch. Supports second set with the Bilgewater faction. |
| 1 | 2 | August 17th, 2020 | [1.8](https://playruneterra.com/en-us/news/patch-1-8-notes-call-of-the-mountain/) | Supports third set with the Targon faction. |
| 1 | 3 | [March 3rd, 2021](https://twitter.com/PlayRuneterra/status/1362446783645945858) | [2.3](https://playruneterra.com/en-us/news/game-updates/patch-2-3-0-notes/) | Supports Empires of the Ascended expansion with Shurima faction. |

The list of cards are then encoded according to the following scheme:

1. Cards are grouped together based on how many copies of the card are in the deck (e.g., cards with three copies are grouped together, cards with two copies are grouped together, and cards with a single copy are grouped together).
1. Within those groups, lists of cards are created which share the same set AND faction.
1. The set/faction lists are ordered by increasing length. The contents of the set/faction lists are ordered alphanumerically.
1. Variable length integer ([varints](https://en.wikipedia.org/wiki/Variable-length_quantity)) (big endian) bytes for each ordered group of cards are written into the byte array according to the following convention:
    * [how many lists of set/faction combination have three copies of a card]
      * [how many cards within this set/faction combination follow]
      * [set]
      * [faction]
        * [card number]
        * [card number]
        * ...
      * [how many cards in this next set/faction combination follow]
      * [set]
      * [faction]
        * [card number]
        * [card number]
        * ...
    * [repeat for the groups of two copies of a card]
    * [repeat for the groups of a single copy of a card]
1. The resulting byte array is base32 encoded into a string.


### Faction Identifiers
Factions are mapped as follows:

| Integer Identifier | Faction Identifier | Faction Name | Version |
| ------------------ | ------------------ | ------------ | ------- |
| 0 | DE | Demacia | 1 |
| 1 | FR | Freljord | 1 |
| 2 | IO | Ionia | 1 |
| 3 | NX | Noxus | 1 |
| 4 | PZ | Piltover & Zaun | 1 |
| 5 | SI | Shadow Isles | 1 |
| 6 | BW | Bilgewater | 2 |
| 7 | SH | Shurima | 3 |
| 9 | MT | Mount Targon | 2** |

**The deck code library remained on version 2 for two set launches; Bilgewater and Mount Targon.

## Implementations
Members of the community have graciously created implementations of this library in various languages. The following is intended to assist in choosing the implementation that works best for you. If you're a developer and would like to include your implementation in this list, please create a [pull request](https://github.com/RiotGames/LoRDeckCodes/pulls) and add a row to the README.

| Name                  | Language | Version* | Maintainer |
| --------------------- | -------- | -------- | ---------- |
| [R4J](https://github.com/stelar7/R4J) | Java 8 | 3 | stelar7 |
| [LoRDeckCodesPython](https://github.com/Rafalonso/LoRDeckCodesPython) | Python 3 | 3 | Rafalonso |
| [runeterra](https://github.com/SwitchbladeBot/runeterra) | JavaScript | 2** | SwitchbladeBot |
| [lordeckoder](https://github.com/MarekSalgovic/lordeckoder) | Golang | 2 | MarekSalgovic |
| [RuneTerraPHP](https://github.com/mike-reinders/runeterra-php) | PHP 7.2 | 3 | Mike-Reinders |
| [LoRDeckCodes.jl](https://github.com/wookay/LoRDeckCodes.jl) | Julia | 1 | wookay |
| [LorElixir](https://github.com/petter-kaspersen/lor-deck-codes-elixir) | Elixir | 1 | petter-kaspersen |
| [lordeckcodes-rs](https://github.com/iulianR/lordeckcodes-rs) | Rust | 3 | iulianR |
| [twisted_fate](https://github.com/snowcola/twisted_fate) | Python 3 | 2** | snowcola |
| [LoRDeckCodes](https://github.com/Pole458/LoRDeckCodesAndroid) | Android | 3 | Pole |
| [lor-deckcode](https://github.com/icepeng/lor-deckcode) | TypeScript | 2 | icepeng |
| [CardGameFr-LoRDeckCode](https://github.com/Yohan-Frmt/CardGameFr-LoRDeckCode) | Ruby | 1 | Yohan-Frmt |
| [LoRDeckCoder](https://github.com/Paul1365972/LoRDeckCoder) | Java 8 | 1 | Paul1365972 |
| [lor_deck_codes_dart](https://github.com/edenizk/lor_deck_codes_dart) | Dart | 2** | edenizk |
| [lor_deckcodes_dart](https://github.com/exts/lor_deckcodes_dart) | Dart 2 | 2** | exts |
| [lor-deckcodes](https://github.com/tomaszbak/lor-deckcodes) | Swift | 2** | tomaszbak |
| [ForDeckmacia](https://github.com/Billzabob/ForDeckmacia) | Scala | 3 | Billzabob |
| [LoRDeck++](https://github.com/EvanKaraf/LoRDeckpp) | C++ | 2 | EvanKaraf |
| [runeterra_cards](https://github.com/zofrex/runeterra_cards) | Ruby | 2** | zofrex |
| [runeterra_decks](https://github.com/SolitudeSF/runeterra_decks) | Nim | 2** | SolitudeSF |

*Version refers to the MAX_KNOWN_VERSION supported by the implementation.  
**Supports deck code version 2 with the Targon faction.

## License
Apache 2 (see [LICENSE](/LICENSE.txt) for details)
