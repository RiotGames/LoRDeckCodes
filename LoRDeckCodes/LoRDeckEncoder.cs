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
        private const int CARD_CODE_LENGTH = 7;
        private const int MAX_KNOWN_VERSION = 1;

        private static readonly Dictionary<string, int> FactionCodeToIntIdentifier = new Dictionary<string, int>
        {
            ["DE"] = 0,
            ["FR"] = 1,
            ["IO"] = 2,
            ["NX"] = 3,
            ["PZ"] = 4,
            ["SI"] = 5
        };
        private static readonly string[] IntIdentifierToFactionCode = FactionCodeToIntIdentifier.Keys.ToArray();

        public static IReadOnlyList<CardCodeAndCount> GetDeckFromCode(string code)
        {
            var result = new List<CardCodeAndCount>();

            void ThrowInvalidDeckCode() => throw new ArgumentException("Invalid deck code", nameof(code));

            byte[] bytes;
            try
            {
                bytes = Base32.Decode(code);
            }
            catch
            {
                ThrowInvalidDeckCode();
            }

            var byteList = bytes.Skip(1).ToList();

            if (byteList.Count == 0)
            {
                ThrowInvalidDeckCode();
            }

            //grab format and version
            int format = bytes[0] >> 4;
            int version = bytes[0] & 0xF;

            if (version > MAX_KNOWN_VERSION)
            {
                throw new ArgumentException("The provided code requires a higher version of this library; please update.", nameof(code));
            }

            for (int i = 3; i > 0; i--)
            {
                int numGroupOfs = VarintTranslator.PopVarint(byteList);

                for (int j = 0; j < numGroupOfs; j++)
                {
                    int numOfsInThisGroup = VarintTranslator.PopVarint(byteList);
                    int set = VarintTranslator.PopVarint(byteList);
                    int faction = VarintTranslator.PopVarint(byteList);

                    for (int k = 0; k < numOfsInThisGroup; k++)
                    {
                        int card = VarintTranslator.PopVarint(byteList);

                        string setString = set.ToString().PadLeft(2, '0');
                        string factionString = IntIdentifierToFactionCode[faction];
                        string cardString = card.ToString().PadLeft(3, '0');

                        var newEntry = new CardCodeAndCount { CardCode = setString + factionString + cardString, Count = i };
                        result.Add(newEntry);
                    }

                }
            }

            //the remainder of the deck code is comprised of entries for cards with counts >= 4
            //this will only happen in Limited and special game modes.
            //the encoding is simply [count] [cardcode]
            while (byteList.Count > 0)
            {
                int fourPlusCount = VarintTranslator.PopVarint(byteList);
                int fourPlusSet = VarintTranslator.PopVarint(byteList);
                int fourPlusFaction = VarintTranslator.PopVarint(byteList);
                int fourPlusNumber = VarintTranslator.PopVarint(byteList);

                string fourPlusSetString = fourPlusSet.ToString().PadLeft(2, '0');
                string fourPlusFactionString = IntIdentifierToFactionCode[fourPlusFaction];
                string fourPlusNumberString = fourPlusNumber.ToString().PadLeft(3, '0');

                var newEntry = new CardCodeAndCount { CardCode = fourPlusSetString + fourPlusFactionString + fourPlusNumberString, Count = fourPlusCount };
                result.Add(newEntry);
            }

            return result;
        }

        public static string GetCodeFromDeck(IEnumerable<CardCodeAndCount> deck)
        {
            return Base32.Encode(GetDeckCodeBytes(deck));
        }

        private static byte[] GetDeckCodeBytes(IEnumerable<CardCodeAndCount> deck)
        {
            var deckList = deck.ToList();
            if (!ValidCardCodesAndCounts(deckList))
                throw new ArgumentException("The provided deck contains invalid card codes.", nameof(deck));

            var formatAndVersion = new byte[] { 17 }; //i.e. 00010001

            var result = new List<byte>(formatAndVersion);

            var of3 = new List<CardCodeAndCount>();
            var of2 = new List<CardCodeAndCount>();
            var of1 = new List<CardCodeAndCount>();
            var ofN = new List<CardCodeAndCount>();

            foreach (var ccc in deckList)
            {
                if (ccc.Count == 3)
                    of3.Add(ccc);
                else if (ccc.Count == 2)
                    of2.Add(ccc);
                else if (ccc.Count == 1)
                    of1.Add(ccc);
                else if (ccc.Count < 1)
                    throw new ArgumentException("Invalid count of " + ccc.Count + " for card " + ccc.CardCode);
                else
                    ofN.Add(ccc);
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

        private static void EncodeNOfs(List<byte> bytes, IEnumerable<CardCodeAndCount> nOfs)
        {
            foreach (var ccc in nOfs)
            {
                bytes.AddRange(VarintTranslator.GetVarint(ccc.Count));

                ParseCardCode(ccc.CardCode, out int setNumber, out string factionCode, out int cardNumber);
                int factionNumber = FactionCodeToIntIdentifier[factionCode];

                bytes.AddRange(VarintTranslator.GetVarint(setNumber));
                bytes.AddRange(VarintTranslator.GetVarint(factionNumber));
                bytes.AddRange(VarintTranslator.GetVarint(cardNumber));
            }
        }

        //The sorting convention of this encoding scheme is
        //First by the number of set/faction combinations in each top-level list
        //Second by the alphanumeric order of the card codes within those lists.
        private static List<List<CardCodeAndCount>> SortGroupOf(IEnumerable<IList<CardCodeAndCount>> groupOf)
        {
            return groupOf.OrderBy(g => g.Count).Select(g => g.OrderBy(c => c.CardCode).ToList()).ToList();
        }

        private static void ParseCardCode(string code, out int set, out string faction, out int number)
        {
            set = int.Parse(code.Substring(0, 2));
            faction = code.Substring(2, 2);
            number = int.Parse(code.Substring(4, 3));
        }

        private static List<List<CardCodeAndCount>> GetGroupedOfs(IList<CardCodeAndCount> list)
        {
            var result = new List<List<CardCodeAndCount>>();
            while (list.Count > 0)
            {
                var currentSet = new List<CardCodeAndCount>();

                //get info from first
                string firstCardCode = list[0].CardCode;
                ParseCardCode(firstCardCode, out int setNumber, out string factionCode, out int cardNumber);

                //now add that to our new list, remove from old
                currentSet.Add(list[0]);
                list.RemoveAt(0);

                //sweep through rest of list and grab entries that should live with our first one.
                //matching means same set and faction - we are already assured the count matches from previous grouping.
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    string currentCardCode = list[i].CardCode;
                    int currentSetNumber = int.Parse(currentCardCode.Substring(0, 2));
                    string currentFactionCode = currentCardCode.Substring(2, 2);

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

        private static void EncodeGroupOf(List<byte> bytes, IReadOnlyCollection<List<CardCodeAndCount>> groupOf)
        {
            bytes.AddRange(VarintTranslator.GetVarint(groupOf.Count));
            foreach (var currentList in groupOf)
            {
                //how many cards in current group?
                bytes.AddRange(VarintTranslator.GetVarint(currentList.Count));

                //what is this group, as identified by a set and faction pair
                string currentCardCode = currentList[0].CardCode;
                ParseCardCode(currentCardCode, out int currentSetNumber, out string currentFactionCode, out int _);
                int currentFactionNumber = FactionCodeToIntIdentifier[currentFactionCode];
                bytes.AddRange(VarintTranslator.GetVarint(currentSetNumber));
                bytes.AddRange(VarintTranslator.GetVarint(currentFactionNumber));

                //what are the cards, as identified by the third section of card code only now, within this group?
                foreach (var cd in currentList)
                {
                    string code = cd.CardCode;
                    int sequenceNumber = int.Parse(code.Substring(4, 3));
                    bytes.AddRange(VarintTranslator.GetVarint(sequenceNumber));
                }
            }
        }

        public static bool ValidCardCodesAndCounts(IEnumerable<CardCodeAndCount> deck)
        {
            foreach (var ccc in deck)
            {
                if (ccc.CardCode.Length != CARD_CODE_LENGTH)
                    return false;

                int parsed;
                if (!int.TryParse(ccc.CardCode.Substring(0, 2), out parsed))
                    return false;

                string faction = ccc.CardCode.Substring(2, 2);
                if (!FactionCodeToIntIdentifier.ContainsKey(faction))
                    return false;

                if (!int.TryParse(ccc.CardCode.Substring(4, 3), out parsed))
                    return false;

                if (ccc.Count < 1)
                    return false;
            }
            return true;
        }
    }
}
