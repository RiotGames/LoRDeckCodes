using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoRDeckCodes
{
    public class LoRDeckEncoder
    {
        private static readonly int CARD_CODE_LENGTH = 7;
        private static Dictionary<string, int> FactionCodeToIntIdentifier;
        private static Dictionary<int, string> IntIdentifierToFactionCode;
        private static readonly int MAX_KNOWN_VERSION = 3;

        static LoRDeckEncoder()
        {
            FactionCodeToIntIdentifier = new()
            {
                {"DE", 0},
                {"FR", 1},
                {"IO", 2},
                {"NX", 3},
                {"PZ", 4},
                {"SI", 5},
                {"BW", 6},
                {"SH", 7},
                {"MT", 9},
            };
            IntIdentifierToFactionCode = new()
            {
                {0, "DE"},
                {1, "FR"},
                {2, "IO"},
                {3, "NX"},
                {4, "PZ"},
                {5, "SI"},
                {6, "BW"},
                {7, "SH"},
                {9, "MT"}
            };
        }

        public static List<CardCodeAndCount> GetDeckFromCode(string code)
        {
            var result = new List<CardCodeAndCount>();

            byte[] bytes;
            try
            {
                bytes = Base32.Decode(code);
            }
            catch
            {
                throw new ArgumentException("Invalid deck code");
            }

            var byteList = bytes.ToList();

            //grab format and version
            var format = bytes[0] >> 4;
            var version = bytes[0] & 0xF;
            byteList.RemoveAt(0);

            if (version > MAX_KNOWN_VERSION)
            {
                throw new ArgumentException("The provided code requires a higher version of this library; please update.");
            }

            for (var i = 3; i > 0; i--)
            {
                var numGroupOfs = VarintTranslator.PopVarint(byteList);

                for (var j = 0; j < numGroupOfs; j++)
                {
                    var numOfsInThisGroup = VarintTranslator.PopVarint(byteList);
                    var set = VarintTranslator.PopVarint(byteList);
                    var faction = VarintTranslator.PopVarint(byteList);

                    for (var k = 0; k < numOfsInThisGroup; k++)
                    {
                        var card = VarintTranslator.PopVarint(byteList);

                        var setString = set.ToString().PadLeft(2, '0');
                        var factionString = IntIdentifierToFactionCode[faction];
                        var cardString = card.ToString().PadLeft(3, '0');

                        var newEntry = new CardCodeAndCount() { CardCode = setString + factionString + cardString, Count = i };
                        result.Add(newEntry);
                    }

                }
            }

            //the remainder of the deck code is comprised of entries for cards with counts >= 4
            //this will only happen in Limited and special game modes.
            //the encoding is simply [count] [cardcode]
            while (byteList.Count > 0)
            {
                var fourPlusCount = VarintTranslator.PopVarint(byteList);
                var fourPlusSet = VarintTranslator.PopVarint(byteList);
                var fourPlusFaction = VarintTranslator.PopVarint(byteList);
                var fourPlusNumber = VarintTranslator.PopVarint(byteList);

                var fourPlusSetString = fourPlusSet.ToString().PadLeft(2, '0');
                var fourPlusFactionString = IntIdentifierToFactionCode[fourPlusFaction];
                var fourPlusNumberString = fourPlusNumber.ToString().PadLeft(3, '0');

                var newEntry = new CardCodeAndCount() { CardCode = fourPlusSetString + fourPlusFactionString + fourPlusNumberString, Count = fourPlusCount };
                result.Add(newEntry);
            }

            return result;
        }

        public static string GetCodeFromDeck(List<CardCodeAndCount> deck) => Base32.Encode(GetDeckCodeBytes(deck));

        private static byte[] GetDeckCodeBytes(List<CardCodeAndCount> deck)
        {
            var result = new List<byte>();

            if (!ValidCardCodesAndCounts(deck))
                throw new ArgumentException("The provided deck contains invalid card codes.");

            var formatAndVersion = new byte[] { 19 }; //i.e. 00010011
            result.AddRange(formatAndVersion);

            var of3 = new List<CardCodeAndCount>();
            var of2 = new List<CardCodeAndCount>();
            var of1 = new List<CardCodeAndCount>();
            var ofN = new List<CardCodeAndCount>();

            foreach (var ccc in deck)
            {
                switch (ccc.Count)
                {
                    case 3:
                        of3.Add(ccc);
                        break;
                    case 2:
                        of2.Add(ccc);
                        break;
                    case 1:
                        of1.Add(ccc);
                        break;
                    case < 1:
                        throw new ArgumentException("Invalid count of " + ccc.Count + " for card " + ccc.CardCode);
                    default:
                        ofN.Add(ccc);
                        break;
                }
            }

            //build the lists of set and faction combinations within the groups of similar card counts
            var groupedOf3s = GetGroupedOfs(of3);
            var groupedOf2s = GetGroupedOfs(of2);
            var groupedOf1s = GetGroupedOfs(of1);

            //to ensure that the same decklist in any order produces the same code, do some sorting
            groupedOf3s = SortGroupOf(groupedOf3s);
            groupedOf2s = SortGroupOf(groupedOf2s);
            groupedOf1s = SortGroupOf(groupedOf1s);

            //Nofs (since rare) are simply sorted by the card code - there's no optimiziation based upon the card count
            ofN = ofN.OrderBy(c => c.CardCode).ToList();

            //Encode
            EncodeGroupOf(result, groupedOf3s);
            EncodeGroupOf(result, groupedOf2s);
            EncodeGroupOf(result, groupedOf1s);

            //Cards with 4+ counts are handled differently: simply [count] [card code] for each
            EncodeNOfs(result, ofN);

            return result.ToArray();
        }

        private static void EncodeNOfs(List<byte> bytes, List<CardCodeAndCount> nOfs)
        {
            foreach (var ccc in nOfs)
            {
                bytes.AddRange(VarintTranslator.GetVarint(ccc.Count));

                ParseCardCode(ccc.CardCode, out int setNumber, out string factionCode, out int cardNumber);
                var factionNumber = FactionCodeToIntIdentifier[factionCode];

                bytes.AddRange(VarintTranslator.GetVarint(setNumber));
                bytes.AddRange(VarintTranslator.GetVarint(factionNumber));
                bytes.AddRange(VarintTranslator.GetVarint(cardNumber));
            }
        }

        //The sorting convention of this encoding scheme is
        //First by the number of set/faction combinations in each top-level list
        //Second by the alphanumeric order of the card codes within those lists.
        private static List<List<CardCodeAndCount>> SortGroupOf(List<List<CardCodeAndCount>> groupOf)
        {
            return groupOf
                .OrderBy(g => g.Count)
                .ThenBy(c => c[0].CardCode)
                .Select(cardsByFact => cardsByFact
                    .OrderBy(c => c.CardCode).ToList())
                .ToList();
        }

        private static void ParseCardCode(string code, out int set, out string faction, out int number)
        {
            set = int.Parse(code.Substring(0, 2));
            faction = code.Substring(2, 2);
            number = int.Parse(code.Substring(4, 3));
        }

        private static List<List<CardCodeAndCount>> GetGroupedOfs(List<CardCodeAndCount> list)
        {
            var result = new List<List<CardCodeAndCount>>();
            while (list.Count > 0)
            {
                var currentSet = new List<CardCodeAndCount>();

                //get info from first
                var firstCardCode = list[0].CardCode;
                ParseCardCode(firstCardCode, out int setNumber, out string factionCode, out _);
                //now add that to our new list, remove from old
                currentSet.Add(list[0]);
                list.RemoveAt(0);

                //sweep through rest of list and grab entries that should live with our first one.
                //matching means same set and faction - we are already assured the count matches from previous grouping.
                for (var i = list.Count - 1; i >= 0; i--)
                {
                    var currentCardCode = list[i].CardCode;
                    ParseCardCode(currentCardCode, out var currentSetNumber, out var currentFactionCode, out _);

                    if (currentSetNumber == setNumber && currentFactionCode == factionCode)
                    {
                        currentSet.Add(list[i]);
                        list.RemoveAt(i);
                    }
                }
                result.Add(currentSet);
            }
            return result;
        }

        private static void EncodeGroupOf(List<byte> bytes, List<List<CardCodeAndCount>> groupOf)
        {
            bytes.AddRange(VarintTranslator.GetVarint(groupOf.Count));
            foreach (var currentList in groupOf)
            {
                //how many cards in current group?
                bytes.AddRange(VarintTranslator.GetVarint(currentList.Count));

                //what is this group, as identified by a set and faction pair
                var currentCardCode = currentList[0].CardCode;
                ParseCardCode(currentCardCode, out var currentSetNumber, out var currentFactionCode, out int _);
                var currentFactionNumber = FactionCodeToIntIdentifier[currentFactionCode];
                bytes.AddRange(VarintTranslator.GetVarint(currentSetNumber));
                bytes.AddRange(VarintTranslator.GetVarint(currentFactionNumber));

                //what are the cards, as identified by the third section of card code only now, within this group?
                foreach (var cd in currentList)
                {
                    var code = cd.CardCode;
                    var sequenceNumber = int.Parse(code.Substring(4, 3));
                    bytes.AddRange(VarintTranslator.GetVarint(sequenceNumber));
                }
            }
        }

        private static bool ValidCardCodesAndCounts(List<CardCodeAndCount> deck)
        {
            foreach (var ccc in deck)
            {
                if (ccc.CardCode.Length != CARD_CODE_LENGTH)
                    return false;
                
                if (!int.TryParse(ccc.CardCode.Substring(0, 2), out _))
                    return false;

                var faction = ccc.CardCode.Substring(2, 2);
                if (!FactionCodeToIntIdentifier.ContainsKey(faction))
                    return false;

                if (!int.TryParse(ccc.CardCode.Substring(4, 3), out _))
                    return false;

                if (ccc.Count < 1)
                    return false;
            }
            return true;
        }
    }
}
