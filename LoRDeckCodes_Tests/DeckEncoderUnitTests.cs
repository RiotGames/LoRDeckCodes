using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using LoRDeckCodes;

namespace LoRDeckCodes_Tests
{
    [TestClass]
    public class DeckEncoderUnitTests
    {
        //Tests the encoding of a set of hard coded decks in DeckCodesTestData.txt
        [TestMethod]
        public void EncodeDecodeRecommendedDecks()
        {
            var codes = new List<string>();
            var decks = new List<List<CardCodeAndCount>>();

            //Load the test data from file.
            string line;
            using (var myReader = new StreamReader("..\\..\\DeckCodesTestData.txt"))
            {
                while ((line = myReader.ReadLine()) != null)
                {
                    codes.Add(line);
                    var newDeck = new List<CardCodeAndCount>();
                    while (!string.IsNullOrEmpty(line = myReader.ReadLine()))
                    {
                        string[] parts = line.Split(new char[] { ':' });
                        newDeck.Add(new CardCodeAndCount { Count = int.Parse(parts[0]), CardCode = parts[1] });
                    }
                    decks.Add(newDeck);
                }
            }

            //Encode each test deck and ensure it's equal to the correct string.
            //Then decode and ensure the deck is unchanged.
            for (int i = 0; i < decks.Count; i++)
            {
                string encoded = LoRDeckEncoder.GetCodeFromDeck(decks[i]);
                Assert.AreEqual(codes[i], encoded);

                var decoded = LoRDeckEncoder.GetDeckFromCode(encoded);
                Assert.IsTrue(VerifyRehydration(decks[i], decoded));

            }
        }

        [TestMethod]
        public void SmallDeck()
        {
            var deck = new List<CardCodeAndCount>
            {
                new CardCodeAndCount { CardCode = "01DE002", Count = 1 }
            };

            string code = LoRDeckEncoder.GetCodeFromDeck(deck);
            var decoded = LoRDeckEncoder.GetDeckFromCode(code);
            Assert.IsTrue(VerifyRehydration(deck, decoded));

        }

        [TestMethod]
        public void LargeDeck()
        {
            var deck = new List<CardCodeAndCount>
            {
                new CardCodeAndCount { CardCode = "01DE002", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE003", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE004", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE005", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE006", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE007", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE008", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE009", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE010", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE011", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE012", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE013", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE014", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE015", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE016", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE017", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE018", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE019", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE020", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE021", Count = 3 }
            };

            string code = LoRDeckEncoder.GetCodeFromDeck(deck);
            var decoded = LoRDeckEncoder.GetDeckFromCode(code);
            Assert.IsTrue(VerifyRehydration(deck, decoded));

        }

        [TestMethod]
        public void DeckWithCountsMoreThan3Small()
        {
            var deck = new List<CardCodeAndCount>
            {
                new CardCodeAndCount { CardCode = "01DE002", Count = 4 }
            };

            string code = LoRDeckEncoder.GetCodeFromDeck(deck);
            var decoded = LoRDeckEncoder.GetDeckFromCode(code);
            Assert.IsTrue(VerifyRehydration(deck, decoded));
        }

        [TestMethod]
        public void DeckWithCountsMoreThan3Large()
        {
            var deck = new List<CardCodeAndCount>
            {
                new CardCodeAndCount { CardCode = "01DE002", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE003", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE004", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE005", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE006", Count = 4 },
                new CardCodeAndCount { CardCode = "01DE007", Count = 5 },
                new CardCodeAndCount { CardCode = "01DE008", Count = 6 },
                new CardCodeAndCount { CardCode = "01DE009", Count = 7 },
                new CardCodeAndCount { CardCode = "01DE010", Count = 8 },
                new CardCodeAndCount { CardCode = "01DE011", Count = 9 },
                new CardCodeAndCount { CardCode = "01DE012", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE013", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE014", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE015", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE016", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE017", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE018", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE019", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE020", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE021", Count = 3 }
            };

            string code = LoRDeckEncoder.GetCodeFromDeck(deck);
            var decoded = LoRDeckEncoder.GetDeckFromCode(code);
            Assert.IsTrue(VerifyRehydration(deck, decoded));
        }

        [TestMethod]
        public void SingleCard40Times()
        {
            var deck = new List<CardCodeAndCount>
            {
                new CardCodeAndCount { CardCode = "01DE002", Count = 40 }
            };

            string code = LoRDeckEncoder.GetCodeFromDeck(deck);
            var decoded = LoRDeckEncoder.GetDeckFromCode(code);
            Assert.IsTrue(VerifyRehydration(deck, decoded));
        }

        [TestMethod]
        public void WorstCaseLength()
        {
            var deck = new List<CardCodeAndCount>
            {
                new CardCodeAndCount { CardCode = "01DE002", Count = 4 },
                new CardCodeAndCount { CardCode = "01DE003", Count = 4 },
                new CardCodeAndCount { CardCode = "01DE004", Count = 4 },
                new CardCodeAndCount { CardCode = "01DE005", Count = 4 },
                new CardCodeAndCount { CardCode = "01DE006", Count = 4 },
                new CardCodeAndCount { CardCode = "01DE007", Count = 5 },
                new CardCodeAndCount { CardCode = "01DE008", Count = 6 },
                new CardCodeAndCount { CardCode = "01DE009", Count = 7 },
                new CardCodeAndCount { CardCode = "01DE010", Count = 8 },
                new CardCodeAndCount { CardCode = "01DE011", Count = 9 },
                new CardCodeAndCount { CardCode = "01DE012", Count = 4 },
                new CardCodeAndCount { CardCode = "01DE013", Count = 4 },
                new CardCodeAndCount { CardCode = "01DE014", Count = 4 },
                new CardCodeAndCount { CardCode = "01DE015", Count = 4 },
                new CardCodeAndCount { CardCode = "01DE016", Count = 4 },
                new CardCodeAndCount { CardCode = "01DE017", Count = 4 },
                new CardCodeAndCount { CardCode = "01DE018", Count = 4 },
                new CardCodeAndCount { CardCode = "01DE019", Count = 4 },
                new CardCodeAndCount { CardCode = "01DE020", Count = 4 },
                new CardCodeAndCount { CardCode = "01DE021", Count = 4 }
            };

            string code = LoRDeckEncoder.GetCodeFromDeck(deck);
            var decoded = LoRDeckEncoder.GetDeckFromCode(code);
            Assert.IsTrue(VerifyRehydration(deck, decoded));
        }

        [TestMethod]
        public void OrderShouldNotMatter1()
        {
            var deck1 = new List<CardCodeAndCount>
            {
                new CardCodeAndCount { CardCode = "01DE002", Count = 1 },
                new CardCodeAndCount { CardCode = "01DE003", Count = 2 },
                new CardCodeAndCount { CardCode = "02DE003", Count = 3 }
            };

            var deck2 = new List<CardCodeAndCount>
            {
                new CardCodeAndCount { CardCode = "01DE003", Count = 2 },
                new CardCodeAndCount { CardCode = "02DE003", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE002", Count = 1 }
            };

            string code1 = LoRDeckEncoder.GetCodeFromDeck(deck1);
            string code2 = LoRDeckEncoder.GetCodeFromDeck(deck2);

            Assert.AreEqual(code1, code2);

            var deck3 = new List<CardCodeAndCount>
            {
                new CardCodeAndCount { CardCode = "01DE002", Count = 4 },
                new CardCodeAndCount { CardCode = "01DE003", Count = 2 },
                new CardCodeAndCount { CardCode = "02DE003", Count = 3 }
            };

            var deck4 = new List<CardCodeAndCount>
            {
                new CardCodeAndCount { CardCode = "01DE003", Count = 2 },
                new CardCodeAndCount { CardCode = "02DE003", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE002", Count = 4 }
            };

            string code3 = LoRDeckEncoder.GetCodeFromDeck(deck1);
            string code4 = LoRDeckEncoder.GetCodeFromDeck(deck2);

            Assert.AreEqual(code3, code4);
        }

        [TestMethod]
        public void OrderShouldNotMatter2()
        {
            //importantly this order test includes more than 1 card with counts >3, which are sorted by card code and appending to the <=3 encodings.
            var deck1 = new List<CardCodeAndCount>
            {
                new CardCodeAndCount { CardCode = "01DE002", Count = 4 },
                new CardCodeAndCount { CardCode = "01DE003", Count = 2 },
                new CardCodeAndCount { CardCode = "02DE003", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE004", Count = 5 }
            };

            var deck2 = new List<CardCodeAndCount>
            {
                new CardCodeAndCount { CardCode = "01DE004", Count = 5 },
                new CardCodeAndCount { CardCode = "01DE003", Count = 2 },
                new CardCodeAndCount { CardCode = "02DE003", Count = 3 },
                new CardCodeAndCount { CardCode = "01DE002", Count = 4 }
            };

            string code1 = LoRDeckEncoder.GetCodeFromDeck(deck1);
            string code2 = LoRDeckEncoder.GetCodeFromDeck(deck2);

            Assert.AreEqual(code1, code2);
        }

        [TestMethod]
        public void BadCardCodes()
        {
            var decks = new[]
            {
                new List<CardCodeAndCount>
                {
                    new CardCodeAndCount { CardCode = "01DE02", Count = 1 }
                },
                new List<CardCodeAndCount>
                {
                    new CardCodeAndCount { CardCode = "01XX002", Count = 1 }
                },
                new List<CardCodeAndCount>
                {
                    new CardCodeAndCount { CardCode = "01DE002", Count = 0 }
                }
            };

            foreach (var deck in decks)
                Assert.ThrowsException<ArgumentException>(() => LoRDeckEncoder.GetCodeFromDeck(deck));
        }

        [TestMethod]
        public void BadCount()
        {
            var decks = new[]
            {
                new List<CardCodeAndCount>
                {
                    new CardCodeAndCount { CardCode = "01DE002", Count = 0 }
                },
                new List<CardCodeAndCount>
                {
                    new CardCodeAndCount { CardCode = "01DE002", Count = -1 }
                }
            };
            foreach (var deck in decks)
                Assert.ThrowsException<ArgumentException>(() => LoRDeckEncoder.GetCodeFromDeck(deck));
        }


        [TestMethod]
        public void GarbageDecoding()
        {
            var badEncodingNotBase32 = "I'm no card code!";
            var badEncoding32 = "ABCDEFG";
            var badEncodingEmpty = "";

            Assert.ThrowsException<ArgumentException>(() => LoRDeckEncoder.GetDeckFromCode(badEncodingNotBase32));
            Assert.ThrowsException<ArgumentException>(() => LoRDeckEncoder.GetDeckFromCode(badEncoding32));
            Assert.ThrowsException<ArgumentException>(() => LoRDeckEncoder.GetDeckFromCode(badEncodingEmpty));
        }

        public bool VerifyRehydration(IReadOnlyList<CardCodeAndCount> d, IReadOnlyList<CardCodeAndCount> rehydrated)
        {
            return d.Count == rehydrated.Count && rehydrated.All(cd => d.Any(cc => cc.CardCode == cd.CardCode && cc.Count == cd.Count));
        }
    }
}
