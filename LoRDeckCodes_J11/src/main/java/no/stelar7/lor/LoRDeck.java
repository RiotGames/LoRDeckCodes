package no.stelar7.lor;


import java.util.*;
import java.util.stream.Collectors;

public class LoRDeck
{
    public static List<LoRCardCount> decode(String code)
    {
        List<LoRCardCount> result = new ArrayList<>();
        List<Byte>         bytes  = new ArrayList<>(Arrays.asList(Base32.decodeBoxed(code)));
        
        int firstByte = bytes.remove(0);
        int format    = firstByte >>> 4;
        int version   = firstByte & 0xF;
        
        int MAX_KNOWN_VERSION = 1;
        if (version > MAX_KNOWN_VERSION)
        {
            throw new IllegalArgumentException("The provided code requires a higher version of this library; please update");
        }
        
        for (int i = 3; i > 0; i--)
        {
            int groupCount = VarInt.pop(bytes);
            
            for (int j = 0; j < groupCount; j++)
            {
                int itemCount = VarInt.pop(bytes);
                int set       = VarInt.pop(bytes);
                int faction   = VarInt.pop(bytes);
                
                for (int k = 0; k < itemCount; k++)
                {
                    int    card          = VarInt.pop(bytes);
                    String setString     = padLeft(String.valueOf(set), 2);
                    String factionString = LoRFaction.fromID(faction).getShortCode();
                    String cardString    = padLeft(String.valueOf(card), 3);
                    
                    result.add(new LoRCardCount(setString, factionString, cardString, i));
                }
            }
        }
        
        while (bytes.size() > 0)
        {
            int count   = VarInt.pop(bytes);
            int set     = VarInt.pop(bytes);
            int faction = VarInt.pop(bytes);
            int number  = VarInt.pop(bytes);
            
            String setString     = padLeft(String.valueOf(set), 2);
            String factionString = LoRFaction.fromID(faction).getShortCode();
            String numberString  = padLeft(String.valueOf(number), 3);
            
            result.add(new LoRCardCount(setString, factionString, numberString, count));
        }
        
        result.sort(Comparator.comparing(LoRCardCount::getCount).reversed().thenComparing(LoRCardCount::getCardCode));
        
        return result;
        
    }
    
    private static String padLeft(String input, int length)
    {
        return "0".repeat(length).substring(input.length()) + input;
    }
    
    public static String encode(List<LoRCardCount> cards)
    {
        List<Integer> result = new ArrayList<>();
        if (!isValidDeck(cards))
        {
            throw new IllegalArgumentException("The deck provided contains invalid card codes");
        }
        
        // format and version = 0b00010001 = 0x11 = 17
        result.add(0x11);
        
        Map<Integer, List<LoRCardCount>> counts   = cards.stream().collect(Collectors.groupingBy(LoRCardCount::getCount));
        List<List<LoRCardCount>>         grouped3 = groupByFactionAndSetSorted(counts.getOrDefault(3, new ArrayList<>()));
        List<List<LoRCardCount>>         grouped2 = groupByFactionAndSetSorted(counts.getOrDefault(2, new ArrayList<>()));
        List<List<LoRCardCount>>         grouped1 = groupByFactionAndSetSorted(counts.getOrDefault(1, new ArrayList<>()));
        List<LoRCardCount> nOfs = counts.entrySet().stream()
                                        .filter(e -> e.getKey() > 3)
                                        .flatMap(e -> e.getValue().stream())
                                        .collect(Collectors.toList());
        
        result.addAll(encodeGroup(grouped3));
        result.addAll(encodeGroup(grouped2));
        result.addAll(encodeGroup(grouped1));
        result.addAll(encodeNofs(nOfs));
        
        return Base32.encodeBoxed(result);
    }
    
    private static List<Integer> encodeNofs(List<LoRCardCount> nOfs)
    {
        List<Integer> result = new ArrayList<>();
        
        for (LoRCardCount card : nOfs)
        {
            result.addAll(VarInt.get(card.getCount()));
            result.addAll(VarInt.get(card.getSet()));
            result.addAll(VarInt.get(card.getFaction().getId()));
            result.addAll(VarInt.get(card.getId()));
        }
        
        return result;
    }
    
    private static List<Integer> encodeGroup(List<List<LoRCardCount>> group)
    {
        List<Integer> result = new ArrayList<>(VarInt.get(group.size()));
        
        for (List<LoRCardCount> list : group)
        {
            result.addAll(VarInt.get(list.size()));
            
            LoRCardCount first = list.get(0);
            result.addAll(VarInt.get(first.getSet()));
            result.addAll(VarInt.get(first.getFaction().getId()));
            
            for (LoRCardCount card : list)
            {
                result.addAll(VarInt.get(card.getId()));
            }
        }
        
        return result;
    }
    
    private static boolean isValidDeck(List<LoRCardCount> cards)
    {
        for (LoRCardCount card : cards)
        {
            if (card.getCardCode().length() != 7)
            {
                return false;
            }
            
            try
            {
                card.getId();
                card.getCount();
            } catch (NumberFormatException e)
            {
                return false;
            }
            
            if (card.getFaction() == null)
            {
                return false;
            }
            
            if (card.getCount() < 1)
            {
                return false;
            }
        }
        
        return true;
    }
    
    private static List<List<LoRCardCount>> groupByFactionAndSetSorted(List<LoRCardCount> cards)
    {
        List<List<LoRCardCount>> result = new ArrayList<>();
        
        while (cards.size() > 0)
        {
            List<LoRCardCount> set = new ArrayList<>();
            
            LoRCardCount first = cards.remove(0);
            set.add(first);
            
            for (int i = cards.size() - 1; i >= 0; i--)
            {
                LoRCardCount compare = cards.get(i);
                
                if (first.getSet() == compare.getSet() && first.getFaction() == compare.getFaction())
                {
                    set.add(compare);
                    cards.remove(i);
                }
            }
            
            result.add(set);
        }
        
        // sort outer list by size, then by inner list code, then sort inner list by code
        Comparator<List<LoRCardCount>> c  = Comparator.comparing(List::size);
        Comparator<List<LoRCardCount>> c2 = Comparator.comparing((List<LoRCardCount> a) -> a.get(0).getCardCode());
        result.sort(c.thenComparing(c2));
        for (List<LoRCardCount> group : result)
        {
            group.sort(Comparator.comparing(LoRCardCount::getCardCode));
        }
        
        return result;
    }
}
