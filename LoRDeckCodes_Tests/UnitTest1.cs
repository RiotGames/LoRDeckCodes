using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Xunit;

using LoRDeckCodes;

namespace LoRDeckCodes_Tests
{
    public class UnitTest1
    {
        //Tests the encoding of a set of hard coded decks in DeckCodesTestData.txt
        [Fact]
        public void EncodeDecodeRecommendedDecks()
        {
            List<string> codes = new List<string>();
            List<List<CardCodeAndCount>> decks = new List<List<CardCodeAndCount>>();

            //Load the test data from file.
            string line;
            using (StreamReader myReader = new StreamReader(GetTestDataFilePath()))
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
                Assert.Equal(codes[i], encoded);

                List<CardCodeAndCount> decoded = LoRDeckEncoder.GetDeckFromCode(encoded);
                Assert.True(VerifyRehydration(decks[i], decoded));
            }
        }
        
        private static string GetTestDataFilePath()
        {
            string TestDataFileName = "DeckCodesTestData.txt";
            // first test local directory
            string testPath = TestDataFileName;
            if (File.Exists(testPath))
            {
                return testPath;
            }

            // then test in executable directory and walk backwards
            string testDir = AppDomain.CurrentDomain.BaseDirectory;
            while (true)
            {
                testPath = Path.Combine(testDir, TestDataFileName);
                if (File.Exists(testPath))
                {
                    return testPath;
                }

                DirectoryInfo parentDir = Directory.GetParent(testDir);
                if (parentDir == null)
                {
                    return null;
                }
                testDir = parentDir.FullName;
            }
        }

        [Fact]
        public void SmallDeck()
        {
            List<CardCodeAndCount> deck = new List<CardCodeAndCount>();
            deck.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 1 });

            string code = LoRDeckEncoder.GetCodeFromDeck(deck);
            List<CardCodeAndCount> decoded = LoRDeckEncoder.GetDeckFromCode(code);
            Assert.True(VerifyRehydration(deck, decoded));

        }

        [Fact]
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
            Assert.True(VerifyRehydration(deck, decoded));

        }

        [Fact]
        public void DeckWithCountsMoreThan3Small()
        {
            List<CardCodeAndCount> deck = new List<CardCodeAndCount>();
            deck.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 4 });

            string code = LoRDeckEncoder.GetCodeFromDeck(deck);
            List<CardCodeAndCount> decoded = LoRDeckEncoder.GetDeckFromCode(code);
            Assert.True(VerifyRehydration(deck, decoded));
        }

        [Fact]
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
            Assert.True(VerifyRehydration(deck, decoded));
        }

        [Fact]
        public void SingleCard40Times()
        {
            List<CardCodeAndCount> deck = new List<CardCodeAndCount>();
            deck.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 40 });

            string code = LoRDeckEncoder.GetCodeFromDeck(deck);
            List<CardCodeAndCount> decoded = LoRDeckEncoder.GetDeckFromCode(code);
            Assert.True(VerifyRehydration(deck, decoded));
        }

        [Fact]
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
            Assert.True(VerifyRehydration(deck, decoded));
        }

        [Fact]
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

            Assert.Equal(code1, code2);

            List<CardCodeAndCount> deck3 = new List<CardCodeAndCount>();
            deck3.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 4 });
            deck3.Add(new CardCodeAndCount() { CardCode = "01DE003", Count = 2 });
            deck3.Add(new CardCodeAndCount() { CardCode = "02DE003", Count = 3 });

            List<CardCodeAndCount> deck4 = new List<CardCodeAndCount>();
            deck4.Add(new CardCodeAndCount() { CardCode = "01DE003", Count = 2 });
            deck4.Add(new CardCodeAndCount() { CardCode = "02DE003", Count = 3 });
            deck4.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 4 });

            string code3 = LoRDeckEncoder.GetCodeFromDeck(deck1);
            string code4 = LoRDeckEncoder.GetCodeFromDeck(deck2);

            Assert.Equal(code3, code4);
        }

        [Fact]
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

            Assert.Equal(code1, code2);
        }

        [Fact]
        public void BilgewaterSet()
        {
            List<CardCodeAndCount> deck = new List<CardCodeAndCount>();
            deck.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 4 });
            deck.Add(new CardCodeAndCount() { CardCode = "02BW003", Count = 2 });
            deck.Add(new CardCodeAndCount() { CardCode = "02BW010", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE004", Count = 5 });

            string code = LoRDeckEncoder.GetCodeFromDeck(deck);
            List<CardCodeAndCount> decoded = LoRDeckEncoder.GetDeckFromCode(code);
            Assert.True(VerifyRehydration(deck, decoded));
        }

        [Fact]
         public void ShurimaSet()
         {
             List<CardCodeAndCount> deck = new List<CardCodeAndCount>();
             deck.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 4 });
             deck.Add(new CardCodeAndCount() { CardCode = "02BW003", Count = 2 });
             deck.Add(new CardCodeAndCount() { CardCode = "02BW010", Count = 3 });
             deck.Add(new CardCodeAndCount() { CardCode = "04SH047", Count = 5 });
        
             string code = LoRDeckEncoder.GetCodeFromDeck(deck);
             List<CardCodeAndCount> decoded = LoRDeckEncoder.GetDeckFromCode(code);
             Assert.True(VerifyRehydration(deck, decoded));
         }        

        [Fact]
        public void MtTargonSet()
        {
            List<CardCodeAndCount> deck = new List<CardCodeAndCount>();
            deck.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 4 });
            deck.Add(new CardCodeAndCount() { CardCode = "03MT003", Count = 2 });
            deck.Add(new CardCodeAndCount() { CardCode = "03MT010", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "02BW004", Count = 5 });

            string code = LoRDeckEncoder.GetCodeFromDeck(deck);
            List<CardCodeAndCount> decoded = LoRDeckEncoder.GetDeckFromCode(code);
            Assert.True(VerifyRehydration(deck, decoded));
        }

        [Fact]
        public void RuneterraSet()
        {
            List<CardCodeAndCount> deck = new List<CardCodeAndCount>();
            deck.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 4 });
            deck.Add(new CardCodeAndCount() { CardCode = "03MT003", Count = 2 });
            deck.Add(new CardCodeAndCount() { CardCode = "03MT010", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01RU001", Count = 5 });

            string code = LoRDeckEncoder.GetCodeFromDeck(deck);
            List<CardCodeAndCount> decoded = LoRDeckEncoder.GetDeckFromCode(code);
            Assert.True(VerifyRehydration(deck, decoded));
        }

        [Fact]
        public void BadVersion() {
            // make sure that a deck with an invalid version fails

            List<CardCodeAndCount> deck = new List<CardCodeAndCount>();
            deck.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 4 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE003", Count = 2 });
            deck.Add(new CardCodeAndCount() { CardCode = "02DE003", Count = 3 });
            deck.Add(new CardCodeAndCount() { CardCode = "01DE004", Count = 5 });

            List<byte> bytesFromDeck = Base32.Decode(LoRDeckEncoder.GetCodeFromDeck(deck)).ToList();

            List<byte> result = new List<byte>();
            byte[] formatAndVersion = new byte[] { 88 }; // arbitrary invalid format/version
            result.AddRange(formatAndVersion);

            bytesFromDeck.RemoveAt(0); // remove the actual format/version
            result.Concat(bytesFromDeck); // replace with invalid one

            try
            {
                string badVersionDeckCode = Base32.Encode(result.ToArray());
                List<CardCodeAndCount> deckBad = LoRDeckEncoder.GetDeckFromCode(badVersionDeckCode);
            }
            catch (ArgumentException e)
            {
                string expectedErrorMessage = "The provided code requires a higher version of this library; please update.";
                Console.WriteLine(e.Message);
                Assert.Equal(expectedErrorMessage, e.Message);
            }
        }

        [Fact]
        public void BadCardCodes()
        {
            List<CardCodeAndCount> deck = new List<CardCodeAndCount>();
            deck.Add(new CardCodeAndCount() { CardCode = "01DE02", Count = 1 });

            bool failed = false;
            try
            {
                string code = LoRDeckEncoder.GetCodeFromDeck(deck);
            }
            catch (ArgumentException)
            {
                failed = true;
            }
            catch (Exception e)
            {
                Assert.True(false, $"Expected to throw an ArgumentException, but it threw {e}.");
            }
            Assert.True(failed, "Expected to throw an ArgumentException, but it succeeded.");


            failed = false;
            deck = new List<CardCodeAndCount>();
            deck.Add(new CardCodeAndCount() { CardCode = "01XX002", Count = 1 });

            failed = false;
            try
            {
                string code = LoRDeckEncoder.GetCodeFromDeck(deck);
            }
            catch (ArgumentException)
            {
                failed = true;
            }
            catch (Exception e)
            {
                Assert.True(false, $"Expected to throw an ArgumentException, but it threw {e}.");
            }
            Assert.True(failed, "Expected to throw an ArgumentException, but it succeeded.");


            failed = false;
            deck = new List<CardCodeAndCount>();
            deck.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 0 });

            try
            {
                string code = LoRDeckEncoder.GetCodeFromDeck(deck);
            }
            catch (ArgumentException)
            {
                failed = true;
            }
            catch (Exception e)
            {
                Assert.True(false, $"Expected to throw an ArgumentException, but it threw {e}.");
            }
            Assert.True(failed, "Expected to throw an ArgumentException, but it succeeded.");
        }

        [Fact]
        public void BadCount()
        {
            List<CardCodeAndCount> deck = new List<CardCodeAndCount>();
            deck.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = 0 });
            bool failed = false;
            try
            {
                string code = LoRDeckEncoder.GetCodeFromDeck(deck);
            }
            catch (ArgumentException)
            {
                failed = true;
            }
            catch (Exception e)
            {
                Assert.True(false, $"Expected to throw an ArgumentException, but it threw {e}.");
            }
            Assert.True(failed, "Expected to throw an ArgumentException, but it succeeded.");

            failed = false;
            deck = new List<CardCodeAndCount>();
            deck.Add(new CardCodeAndCount() { CardCode = "01DE002", Count = -1 });
            try
            {
                string code = LoRDeckEncoder.GetCodeFromDeck(deck);
            }
            catch (ArgumentException)
            {
                failed = true;
            }
            catch (Exception e)
            {
                Assert.True(false, $"Expected to throw an ArgumentException, but it threw {e}.");
            }
            Assert.True(failed, "Expected to throw an ArgumentException, but it succeeded.");
        }


        [Fact]
        public void GarbageDecoding()
        {
            string badEncodingNotBase32 = "I'm no card code!";
            string badEncoding32 = "ABCDEFG";
            string badEncodingEmpty = "";
            
            bool failed = false;
            try
            {
                List<CardCodeAndCount> deck = LoRDeckEncoder.GetDeckFromCode(badEncodingNotBase32);
            }
            catch (ArgumentException)
            {
                failed = true;
            }
            catch (Exception e)
            {
                Assert.True(false, $"Expected to throw an ArgumentException, but it threw {e}.");
            }
            Assert.True(failed, "Expected to throw an ArgumentException, but it succeeded.");

            failed = false;
            try
            {
                List<CardCodeAndCount> deck = LoRDeckEncoder.GetDeckFromCode(badEncoding32);
            }
            catch (ArgumentException)
            {
                failed = true;
            }
            catch (Exception e)
            {
                Assert.True(false, $"Expected to throw an ArgumentException, but it threw {e}.");
            }
            Assert.True(failed, "Expected to throw an ArgumentException, but it succeeded.");

            failed = false;
            try
            {
                List<CardCodeAndCount> deck = LoRDeckEncoder.GetDeckFromCode(badEncodingEmpty);
            }
            catch
            {
                failed = true;
            }
            Assert.True(failed, "Expected to throw an ArgumentException, but it succeeded.");

        }

        [Theory]
        [InlineData("DE", 1)]
        [InlineData("FR", 1)]
        [InlineData("IO", 1)]
        [InlineData("NX", 1)]
        [InlineData("PZ", 1)]
        [InlineData("SI", 1)]
        [InlineData("BW", 2)]
        [InlineData("MT", 2)]
        [InlineData("SH", 3)]
        [InlineData("BC", 4)]
        [InlineData("RU", 5)]
        public void DeckVersionIsTheMinimumLibraryVersionThatSupportsTheContainedFactions(string faction, int expectedVersion)
        {
            List<CardCodeAndCount> deck = new List<CardCodeAndCount>();
            deck.Add(new CardCodeAndCount() { CardCode = "01DE001", Count = 1 });
            deck.Add(new CardCodeAndCount() { CardCode = $"01{faction}002", Count = 1 });
            deck.Add(new CardCodeAndCount() { CardCode = "01FR001", Count = 1 });
            string deckCode = LoRDeckEncoder.GetCodeFromDeck(deck);

            int minSupportedLibraryVersion = ExtractVersionFromDeckCode(deckCode);

            Assert.Equal(expectedVersion, minSupportedLibraryVersion);
        }

        [Fact]
        public void ArgumentExceptionOnFutureVersion()
        {
            const string deckCodeWithVersion10 = "DEAAABABAEFACAIBAAAQCAIFAEAQGCTP";
            Assert.Throws<ArgumentException>(() => LoRDeckEncoder.GetDeckFromCode(deckCodeWithVersion10));
        }

        private static int ExtractVersionFromDeckCode(string deckCode)
        {
            byte[] bytes = Base32.Decode(deckCode);
            return bytes[0] & 0xF;
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
