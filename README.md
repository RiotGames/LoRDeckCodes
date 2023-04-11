LoRDeckCodes
============

The LorDeckCodes library can be used to encode/decode Legends of Runeterra decks to/from simple strings. Below is an example code for a Ionia/Piltover & Zaun deck.
```
CEAAECABAIDASDASDISC2OIIAECBGGY4FAWTINZ3AICACAQXDUPCWBABAQGSOKRM
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
Decks are encoding via arranging VarInts ([little-endian](https://en.wikipedia.org/wiki/Endianness)) into an array and then base 32 encoding into a string.

All encodings begin with 4 bits for format and 4 bits for version.

| Format | Version | Date | Patch | About |
| ------ | ------- | ---- | ----- | ----- |
| 1 | 1 | Oct 18, 2019 | - | Closed alpha. Supports original set. |
| 1 | 2 | April 28, 2020 | [1.0](https://playruneterra.com/en-us/news/patch-1-0-notes/) | Launch. Supports second set with the Bilgewater faction. |
| 1 | 2 | August 17th, 2020 | [1.8](https://playruneterra.com/en-us/news/patch-1-8-notes-call-of-the-mountain/) | Supports third set with the Targon faction. |
| 1 | 3 | [March 3rd, 2021](https://twitter.com/PlayRuneterra/status/1362446783645945858) | [2.3](https://playruneterra.com/en-us/news/game-updates/patch-2-3-0-notes/) | Supports Empires of the Ascended expansion with Shurima faction. |
| 1 | 4 | [August 25, 2021](https://twitter.com/PlayRuneterra/status/1425487172589604865) | [2.14](https://playruneterra.com/en-us/news/game-updates/patch-2-14-0-notes/) | Supports Beyond the Bandlewood expansion with Bandle City faction and an update to the deck code library which will create the lowest version code required based on the cards in the deck. |
| 1 | 5 | [May 25th, 2022](https://twitter.com/PlayRuneterra/status/1525151384328454145) | [3.8](https://playruneterra.com/en-us/news/game-updates/patch-3-8-0-notes/) | Supports Worldwalker expansion with Runeterra faction. |

The list of cards are then encoded according to the following scheme:

1. Cards are grouped together based on how many copies of the card are in the deck (e.g., cards with three copies are grouped together, cards with two copies are grouped together, and cards with a single copy are grouped together).
1. Within those groups, lists of cards are created which share the same set AND faction.
1. The set/faction lists are ordered by increasing length. The contents of the set/faction lists are ordered alphanumerically.
1. Variable length integer ([varints](https://en.wikipedia.org/wiki/LEB128#Unsigned_LEB128)) ([little-endian](https://en.wikipedia.org/wiki/Endianness)) bytes for each ordered group of cards are written into the byte array according to the following convention:
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

| Version | Integer Identifier | Faction Identifier | Faction Name |
| ----------------- | ------------------ | ------------------ | ------------ |
| 1 | 0 | DE | Demacia |
| 1 | 1 | FR | Freljord |
| 1 | 2 | IO | Ionia |
| 1 | 3 | NX | Noxus |
| 1 | 4 | PZ | Piltover & Zaun |
| 1 | 5 | SI | Shadow Isles |
| 2 | 6 | BW | Bilgewater |
| 2 | 9 | MT | Mount Targon |
| 3 | 7 | SH | Shurima |
| 4 | 10 | BC | Bandle City |
| 5 | 12 | RU | Runeterra |

## Implementations
Members of the community have graciously created implementations of this library in various languages. The following is intended to assist in choosing the implementation that works best for you. If you're a developer and would like to include your implementation in this list, please create a [pull request](https://github.com/RiotGames/LoRDeckCodes/pulls) and add a row to the README.

### Current Version

| Name                  | Language | Version* | Maintainer |
| --------------------- | -------- | -------- | ---------- |
| [R4J](https://github.com/stelar7/R4J) | Java 8 | 5 | stelar7 |
| [ForDeckmacia](https://github.com/Billzabob/ForDeckmacia) | Scala | 5 | Billzabob |
| [LoRDeckCodesPython](https://github.com/Rafalonso/LoRDeckCodesPython) | Python 3 | 5 | Rafalonso |
| [runeterra](https://github.com/SwitchbladeBot/runeterra) | JavaScript | 5 | SwitchbladeBot |
| [runeterra_cards](https://github.com/zofrex/runeterra_cards) | Ruby | 5 | zofrex |
| [runeterra_decks](https://github.com/SolitudeSF/runeterra_decks) | Nim | 5 | SolitudeSF |
| [lor-deckcodes-ts](https://github.com/jcuker/lor-deckcode-ts) | TypeScript | 5 | jcuker |
| [lordecks](https://github.com/pholzmgit/lordecks) | R | 5 | pholzmgit |
| [riot_lor](https://github.com/ed-flanagan/riot_lor) | Elixir | 5 | ed-flanagan |
| [lor-deckcode-go](https://github.com/m0t0k1ch1/lor-deckcode-go) | Go | 5 | m0t0k1ch1 |
| [LorDeckCodeCpp](https://github.com/Suolumi/LorDeckCodeCpp) | C++ | 5 | Suolumi |

Leading up to the release of a new version of the library, we'll keep the original and new version in the **Current Version** section. A couple weeks after the release of a new version, any libraries that have not been updated to the latest version will be moved into the **Previous Versions** section. Any libraries in the **Previous Version** section that get updated to the latest version will get appended to the **Current Version** section.

### Previous Versions

| Name                  | Language | Version* | Maintainer |
| --------------------- | -------- | -------- | ---------- |
| [goterra](https://github.com/sousa-andre/goterra) | Go | 4 | sousa-andre |
| [lor-deckcodes](https://github.com/tomaszbak/lor-deckcodes) | Swift | 4 | tomaszbak |
| [runeterra-deck-code](https://github.com/Yutsa/runeterra-deck-code) | Java | 4 | Yutsa |
| [lordeckoder](https://github.com/MarekSalgovic/lordeckoder) | Go | 4 | MarekSalgovic |
| [RuneTerraPHP](https://github.com/mike-reinders/runeterra-php) | PHP 7.2 | 3 | Mike-Reinders |
| [lordeckcodes-rs](https://github.com/iulianR/lordeckcodes-rs) | Rust | 3 | iulianR |
| [LoRDeckCodes](https://github.com/Pole458/LoRDeckCodesAndroid) | Android | 3 | Pole |
| [lor_deck_codes_dart](https://github.com/edenizk/lor_deck_codes_dart) | Dart | 3 | edenizk |
| [LoRDeckCodes.jl](https://github.com/wookay/LoRDeckCodes.jl) | Julia | 3 | wookay |
| [twisted_fate](https://github.com/snowcola/twisted_fate) | Python 3 | 2** | snowcola |
| [lor_deckcodes_dart](https://github.com/exts/lor_deckcodes_dart) | Dart 2 | 2** | exts |
| [lor-deckcode](https://github.com/icepeng/lor-deckcode) | TypeScript | 2 | icepeng |
| [LoRDeck++](https://github.com/EvanKaraf/LoRDeckpp) | C++ | 2 | EvanKaraf |
| [LorElixir](https://github.com/petter-kaspersen/lor-deck-codes-elixir) | Elixir | 1 | petter-kaspersen |
| [CardGameFr-LoRDeckCode](https://github.com/Yohan-Frmt/CardGameFr-LoRDeckCode) | Ruby | 1 | Yohan-Frmt |
| [LoRDeckCoder](https://github.com/Paul1365972/LoRDeckCoder) | Java 8 | 1 | Paul1365972 |

*Version refers to the MAX_KNOWN_VERSION supported by the implementation.  
**Supports deck code version 2 with the Targon faction.

## License
Apache 2 (see [LICENSE](/LICENSE.txt) for details)
