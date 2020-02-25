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
    public class UnitTest1
    {
        //Tests the encoding of a set of hard coded decks in DeckCodesTestData.txt
        [TestMethod]
        public void EncodeDecodeRecommendedDecks()
        {
            List<string> codes = new List<string>();
            List<List<CardCodeAndCount>> decks = new List<List<CardCodeAndCount>>();

            //Load the test data from file.
            string line;
            using (StreamReader myReader = new StreamReader("..\\..\\DeckCodesTestData.txt"))
            {
                while( (line = myReader.ReadLine()) != null)
                {
                    codes.Add(line);
                    List<CardCodeAndCount> newDeck = new List<CardCodeAndCount>();
                    while (!string.IsNullOrEmpty(line = myReader.ReadLine()))
                    {
                        string[] parts = line.Split(new char[] { ':' });
                        newDeck.Add(new CardCodeAndCount() { Count = int.Parse(parts[0]), CardCode = parts[1] });
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

                List<CardCodeAndCount> decoded = LoRDeckEncoder.GetDeckFromCode(encoded);
                Assert.IsTrue(VerifyRehydration(decks[i], decoded));

            }
        }

        [TestMethod]
        public void SmallDeck()
        {
            List<CardCodeAndCount> deck = new List<CardCodeAndCount>();
            deck.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 1 });

            string code = LoRDeckEncoder.GetCodeFromDeck(deck);
            List<CardCodeAndCount> decoded = LoRDeckEncoder.GetDeckFromCode(code);
            Assert.IsTrue(VerifyRehydration(deck, decoded));

        }

        [TestMethod]
        public void LargeDeck()
        {
            List<CardCodeAndCount> deck = new List<CardCodeAndCount>();
            deck.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE003", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE004", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE005", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE006", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE007", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE008", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE009", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE010", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE011", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE012", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE013", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE014", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE015", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE016", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE017", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE018", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE019", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE020", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE021", Count = 3 });

            string code = LoRDeckEncoder.GetCodeFromDeck(deck);
            List<CardCodeAndCount> decoded = LoRDeckEncoder.GetDeckFromCode(code);
            Assert.IsTrue(VerifyRehydration(deck, decoded));

        }

        [TestMethod]
        public void DeckWithCountsMoreThan3Small()
        {
            List<CardCodeAndCount> deck = new List<CardCodeAndCount>();
            deck.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 4 });

            string code = LoRDeckEncoder.GetCodeFromDeck(deck);
            List<CardCodeAndCount> decoded = LoRDeckEncoder.GetDeckFromCode(code);
            Assert.IsTrue(VerifyRehydration(deck, decoded));
        }

        [TestMethod]
        public void DeckWithCountsMoreThan3Large()
        {
            List<CardCodeAndCount> deck = new List<CardCodeAndCount>();
            deck.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE003", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE004", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE005", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE006", Count = 4 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE007", Count = 5 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE008", Count = 6 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE009", Count = 7 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE010", Count = 8 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE011", Count = 9 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE012", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE013", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE014", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE015", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE016", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE017", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE018", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE019", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE020", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE021", Count = 3 });

            string code = LoRDeckEncoder.GetCodeFromDeck(deck);
            List<CardCodeAndCount> decoded = LoRDeckEncoder.GetDeckFromCode(code);
            Assert.IsTrue(VerifyRehydration(deck, decoded));
        }

        [TestMethod]
        public void SingleCard40Times()
        {
            List<CardCodeAndCount> deck = new List<CardCodeAndCount>();
            deck.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 40 });

            string code = LoRDeckEncoder.GetCodeFromDeck(deck);
            List<CardCodeAndCount> decoded = LoRDeckEncoder.GetDeckFromCode(code);
            Assert.IsTrue(VerifyRehydration(deck, decoded));
        }

        [TestMethod]
        public void WorstCaseLength()
        {
            List<CardCodeAndCount> deck = new List<CardCodeAndCount>();
            deck.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 4 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE003", Count = 4 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE004", Count = 4 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE005", Count = 4 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE006", Count = 4 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE007", Count = 5 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE008", Count = 6 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE009", Count = 7 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE010", Count = 8 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE011", Count = 9 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE012", Count = 4 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE013", Count = 4 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE014", Count = 4 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE015", Count = 4 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE016", Count = 4 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE017", Count = 4 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE018", Count = 4 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE019", Count = 4 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE020", Count = 4 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE021", Count = 4 });

            string code = LoRDeckEncoder.GetCodeFromDeck(deck);
            List<CardCodeAndCount> decoded = LoRDeckEncoder.GetDeckFromCode(code);
            Assert.IsTrue(VerifyRehydration(deck, decoded));
        }

        [TestMethod]
        public void OrderShouldNotMatter1()
        {
            List<CardCodeAndCount> deck1 = new List<CardCodeAndCount>();
            deck1.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 1 });
            deck1.Add(new CardCodeAndCount() { CardCode = "01DE003", Count = 2 });
            deck1.Add(new CardCodeAndCount() { CardCode = "02DE003", Count = 3 });

            List<CardCodeAndCount> deck2 = new List<CardCodeAndCount>();
            deck2.Add(new CardCodeAndCount() { CardCode = "01DE003", Count = 2 });
            deck2.Add(new CardCodeAndCount() { CardCode = "02DE003", Count = 3 });
            deck2.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 1 });

            string code1 = LoRDeckEncoder.GetCodeFromDeck(deck1);
            string code2 = LoRDeckEncoder.GetCodeFromDeck(deck2);

            Assert.AreEqual(code1, code2);

            List<CardCodeAndCount> deck3 = new List<CardCodeAndCount>();
            deck3.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 4 });
            deck3.Add(new CardCodeAndCount() { CardCode = "01DE003", Count = 2 });
            deck3.Add(new CardCodeAndCount() { CardCode = "02DE003", Count = 3 });

            List<CardCodeAndCount> deck4 = new List<CardCodeAndCount>();
            deck4.Add(new CardCodeAndCount() { CardCode = "01DE003", Count = 2 });
            deck4.Add(new CardCodeAndCount() { CardCode = "02DE003", Count = 3 });
            deck4.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 4 });

            string code3 = LoRDeckEncoder.GetCodeFromDeck(deck3);
            string code4 = LoRDeckEncoder.GetCodeFromDeck(deck4);

            Assert.AreEqual(code3, code4);
        }

        [TestMethod]
        public void OrderShouldNotMatter2()
        {
            //importantly this order test includes more than 1 card with counts >3, which are sorted by card code and appending to the <=3 encodings.
            List<CardCodeAndCount> deck1 = new List<CardCodeAndCount>();
            deck1.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 4 });
            deck1.Add(new CardCodeAndCount() { CardCode = "01DE003", Count = 2 });
            deck1.Add(new CardCodeAndCount() { CardCode = "02DE003", Count = 3 });
            deck1.Add(new CardCodeAndCount() { CardCode = "01DE004", Count = 5 });

            List<CardCodeAndCount> deck2 = new List<CardCodeAndCount>();
            deck2.Add(new CardCodeAndCount() { CardCode = "01DE004", Count = 5 });
            deck2.Add(new CardCodeAndCount() { CardCode = "01DE003", Count = 2 });
            deck2.Add(new CardCodeAndCount() { CardCode = "02DE003", Count = 3 });
            deck2.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 4 });

            string code1 = LoRDeckEncoder.GetCodeFromDeck(deck1);
            string code2 = LoRDeckEncoder.GetCodeFromDeck(deck2);

            Assert.AreEqual(code1, code2);
        }

        [TestMethod]
        public void BadCardCodes()
        {
            List<CardCodeAndCount> deck = new List<CardCodeAndCount>();
            deck.Add(new CardCodeAndCount() { CardCode = "01DE02", Count = 1 });

            try
            {
                string code = LoRDeckEncoder.GetCodeFromDeck(deck);
                Assert.Fail();
            }
            catch (ArgumentException)
            {

            }
            catch
            {
                Assert.Fail();
            }

            deck = new List<CardCodeAndCount>();
            deck.Add(new CardCodeAndCount() { CardCode = "01XX002", Count = 1 });

            try
            {
                string code = LoRDeckEncoder.GetCodeFromDeck(deck);
                Assert.Fail();
            }
            catch (ArgumentException)
            {

            }
            catch
            {
                Assert.Fail();
            }

            deck = new List<CardCodeAndCount>();
            deck.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 0 });

            try
            {
                string code = LoRDeckEncoder.GetCodeFromDeck(deck);
                Assert.Fail();
            }
            catch (ArgumentException)
            {

            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void BadCount()
        {
            List<CardCodeAndCount> deck = new List<CardCodeAndCount>();
            deck.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 0 });
            try
            {
                string code = LoRDeckEncoder.GetCodeFromDeck(deck);
                Assert.Fail();
            }
            catch (ArgumentException)
            {

            }
            catch
            {
                Assert.Fail();
            }

            deck = new List<CardCodeAndCount>();
            deck.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = -1 });
            try
            {
                string code = LoRDeckEncoder.GetCodeFromDeck(deck);
                Assert.Fail();
            }
            catch (ArgumentException)
            {

            }
            catch
            {
                Assert.Fail();
            }
        }


        [TestMethod]
        public void GarbageDecoding()
        {
            string badEncodingNotBase32 = "I'm no card code!";
            string badEncoding32 = "ABCDEFG";
            string badEncodingEmpty = "";
            
            try
            {
                List<CardCodeAndCount> deck = LoRDeckEncoder.GetDeckFromCode(badEncodingNotBase32);
                Assert.Fail();
            }
            catch (ArgumentException)
            {

            }
            catch
            {
                Assert.Fail();
            }

            try
            {
                List<CardCodeAndCount> deck = LoRDeckEncoder.GetDeckFromCode(badEncoding32);
                Assert.Fail();
            }
            catch (ArgumentException)
            {

            }
            catch
            {
                Assert.Fail();
            }

            try
            {
                List<CardCodeAndCount> deck = LoRDeckEncoder.GetDeckFromCode(badEncodingEmpty);
                Assert.Fail();
            }
            catch
            {

            }

        }

        public bool VerifyRehydration(List<CardCodeAndCount> d, List<CardCodeAndCount> rehydratedList)
        {
            if (d.Count != rehydratedList.Count)
                return false;

            foreach (CardCodeAndCount cd in rehydratedList)
            {
                bool found = false;
                foreach (CardCodeAndCount cc in d)
                {
                    if (cc.CardCode == cd.CardCode && cc.Count == cd.Count)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    return false;

            }
            return true;
        }
    }
}
