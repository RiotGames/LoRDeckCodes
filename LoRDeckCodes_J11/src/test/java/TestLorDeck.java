import no.stelar7.lor.*;
import org.junit.*;

import java.io.InputStream;
import java.util.*;

public class TestLorDeck
{
    
    @Test
    public void testEncodingRecommendedDecks()
    {
        InputStream file = TestLorDeck.class.getClassLoader().getResourceAsStream("testdata.txt");
        if (file == null)
        {
            throw new RuntimeException("Unable to load test file");
        }
        
        List<String>             codes = new ArrayList<>();
        List<List<LoRCardCount>> decks = new ArrayList<>();
        
        try (Scanner scanner = new Scanner(file))
        {
            while (scanner.hasNextLine())
            {
                String line = scanner.nextLine();
                codes.add(line);
                
                List<LoRCardCount> deck = new ArrayList<>();
                while (scanner.hasNextLine() && !(line = scanner.nextLine()).equalsIgnoreCase(""))
                {
                    String[] parts = line.split(":");
                    deck.add(new LoRCardCount(parts[1], Integer.parseInt(parts[0])));
                }
                decks.add(deck);
            }
        }
        
        for (int i = 0; i < decks.size(); i++)
        {
            String encoded = LoRDeck.encode(decks.get(i));
            Assert.assertEquals("Decks are not equal", codes.get(i), encoded);
            
            List<LoRCardCount> decoded = LoRDeck.decode(encoded);
            Assert.assertTrue("Did not produce same deck when re-coded", checkSameDeck(decks.get(i), decoded));
        }
    }
    
    @Test
    public void testDecodeEncode()
    {
        String DECK_CODE = "CEBAIAIFB4WDANQIAEAQGDAUDAQSIJZUAIAQCAIEAEAQKBIA";
        
        List<LoRCardCount> deck   = LoRDeck.decode(DECK_CODE);
        String             result = LoRDeck.encode(deck);
        
        Assert.assertEquals("Did not transform code to match!", DECK_CODE, result);
    }
    
    
    @Test
    public void testSmallDeck()
    {
        List<LoRCardCount> deck = new ArrayList<>()
        {{
            add(new LoRCardCount("01DE002", 3));
        }};
        
        String             code    = LoRDeck.encode(deck);
        List<LoRCardCount> decoded = LoRDeck.decode(code);
        
        Assert.assertTrue("Did not produce same deck when re-coded", checkSameDeck(deck, decoded));
    }
    
    @Test
    public void testLargeDeck()
    {
        List<LoRCardCount> deck = new ArrayList<>()
        {{
            add(new LoRCardCount("01DE002", 3));
            add(new LoRCardCount("01DE003", 3));
            add(new LoRCardCount("01DE004", 3));
            add(new LoRCardCount("01DE005", 3));
            add(new LoRCardCount("01DE006", 3));
            add(new LoRCardCount("01DE007", 3));
            add(new LoRCardCount("01DE008", 3));
            add(new LoRCardCount("01DE009", 3));
            add(new LoRCardCount("01DE010", 3));
            add(new LoRCardCount("01DE011", 3));
            add(new LoRCardCount("01DE012", 3));
            add(new LoRCardCount("01DE013", 3));
            add(new LoRCardCount("01DE014", 3));
            add(new LoRCardCount("01DE015", 3));
            add(new LoRCardCount("01DE016", 3));
            add(new LoRCardCount("01DE017", 3));
            add(new LoRCardCount("01DE018", 3));
            add(new LoRCardCount("01DE019", 3));
            add(new LoRCardCount("01DE020", 3));
            add(new LoRCardCount("01DE021", 3));
        }};
        
        String             code    = LoRDeck.encode(deck);
        List<LoRCardCount> decoded = LoRDeck.decode(code);
        
        Assert.assertTrue("Did not produce same deck when re-coded", checkSameDeck(deck, decoded));
    }
    
    
    @Test
    public void testMoreThan3Small()
    {
        List<LoRCardCount> deck = new ArrayList<>()
        {{
            add(new LoRCardCount("01DE002", 4));
        }};
        
        String             code    = LoRDeck.encode(deck);
        List<LoRCardCount> decoded = LoRDeck.decode(code);
        
        Assert.assertTrue("Did not produce same deck when re-coded", checkSameDeck(deck, decoded));
    }
    
    @Test
    public void testMoreThan3Large()
    {
        List<LoRCardCount> deck = new ArrayList<>()
        {{
            add(new LoRCardCount("01DE002", 3));
            add(new LoRCardCount("01DE003", 3));
            add(new LoRCardCount("01DE004", 3));
            add(new LoRCardCount("01DE005", 3));
            add(new LoRCardCount("01DE006", 4));
            add(new LoRCardCount("01DE007", 5));
            add(new LoRCardCount("01DE008", 6));
            add(new LoRCardCount("01DE009", 7));
            add(new LoRCardCount("01DE010", 8));
            add(new LoRCardCount("01DE011", 9));
            add(new LoRCardCount("01DE012", 3));
            add(new LoRCardCount("01DE013", 3));
            add(new LoRCardCount("01DE014", 3));
            add(new LoRCardCount("01DE015", 3));
            add(new LoRCardCount("01DE016", 3));
            add(new LoRCardCount("01DE017", 3));
            add(new LoRCardCount("01DE018", 3));
            add(new LoRCardCount("01DE019", 3));
            add(new LoRCardCount("01DE020", 3));
            add(new LoRCardCount("01DE021", 3));
        }};
        
        String             code    = LoRDeck.encode(deck);
        List<LoRCardCount> decoded = LoRDeck.decode(code);
        
        Assert.assertTrue("Did not produce same deck when re-coded", checkSameDeck(deck, decoded));
    }
    
    @Test
    public void testSingleCard40()
    {
        List<LoRCardCount> deck = new ArrayList<>()
        {{
            add(new LoRCardCount("01DE002", 40));
        }};
        
        String             code    = LoRDeck.encode(deck);
        List<LoRCardCount> decoded = LoRDeck.decode(code);
        
        Assert.assertTrue("Did not produce same deck when re-coded", checkSameDeck(deck, decoded));
    }
    
    @Test
    public void testWorstCaseLength()
    {
        List<LoRCardCount> deck = new ArrayList<>()
        {{
            add(new LoRCardCount("01DE002", 4));
            add(new LoRCardCount("01DE003", 4));
            add(new LoRCardCount("01DE004", 4));
            add(new LoRCardCount("01DE005", 4));
            add(new LoRCardCount("01DE006", 4));
            add(new LoRCardCount("01DE007", 5));
            add(new LoRCardCount("01DE008", 6));
            add(new LoRCardCount("01DE009", 7));
            add(new LoRCardCount("01DE010", 8));
            add(new LoRCardCount("01DE011", 9));
            add(new LoRCardCount("01DE012", 4));
            add(new LoRCardCount("01DE013", 4));
            add(new LoRCardCount("01DE014", 4));
            add(new LoRCardCount("01DE015", 4));
            add(new LoRCardCount("01DE016", 4));
            add(new LoRCardCount("01DE017", 4));
            add(new LoRCardCount("01DE018", 4));
            add(new LoRCardCount("01DE019", 4));
            add(new LoRCardCount("01DE020", 4));
            add(new LoRCardCount("01DE021", 4));
        }};
        
        String             code    = LoRDeck.encode(deck);
        List<LoRCardCount> decoded = LoRDeck.decode(code);
        
        Assert.assertTrue("Did not produce same deck when re-coded", checkSameDeck(deck, decoded));
    }
    
    @Test
    public void testOrderDoesNotMatter()
    {
        List<LoRCardCount> deck = new ArrayList<>()
        {{
            add(new LoRCardCount("01DE002", 1));
            add(new LoRCardCount("01DE003", 2));
            add(new LoRCardCount("02DE003", 3));
        }};
        
        List<LoRCardCount> deck2 = new ArrayList<>()
        {{
            add(new LoRCardCount("01DE003", 2));
            add(new LoRCardCount("02DE003", 3));
            add(new LoRCardCount("01DE002", 1));
        }};
        
        String code  = LoRDeck.encode(deck);
        String code2 = LoRDeck.encode(deck2);
        
        Assert.assertEquals("Order matters?", code, code2);
    }
    
    @Test
    public void testOrderDoesNotMatterMoreThan3()
    {
        List<LoRCardCount> deck = new ArrayList<>()
        {{
            add(new LoRCardCount("01DE002", 4));
            add(new LoRCardCount("01DE003", 2));
            add(new LoRCardCount("02DE003", 3));
            add(new LoRCardCount("01DE004", 5));
        }};
        
        List<LoRCardCount> deck2 = new ArrayList<>()
        {{
            add(new LoRCardCount("01DE004", 5));
            add(new LoRCardCount("01DE003", 2));
            add(new LoRCardCount("02DE003", 3));
            add(new LoRCardCount("01DE002", 4));
        }};
        
        String code  = LoRDeck.encode(deck);
        String code2 = LoRDeck.encode(deck2);
        
        Assert.assertEquals("Order matters?", code, code2);
    }
    
    @Test
    public void testInvalidDecks()
    {
        List<LoRCardCount> deck = new ArrayList<>()
        {
            {
                add(new LoRCardCount("01DE02", 1));
            }
        };
        checkDeck(deck);
        
        deck = new ArrayList<>()
        {
            {
                add(new LoRCardCount("01XX202", 1));
            }
        };
        checkDeck(deck);
        
        deck = new ArrayList<>()
        {
            {
                add(new LoRCardCount("01DE002", 0));
            }
        };
        checkDeck(deck);
        
        deck = new ArrayList<>()
        {
            {
                add(new LoRCardCount("01DE002", -1));
            }
        };
        checkDeck(deck);
    }
    
    @Test
    public void testInvalidCodes()
    {
        String badNot32 = "This is not a card code. Dont @me";
        String bad32    = "ABCDEFG";
        String empty    = "";
        
        checkCode(badNot32);
        checkCode(bad32);
        checkCode(empty);
    }
    
    private void checkCode(String code)
    {
        try
        {
            List<LoRCardCount> deck = LoRDeck.decode(code);
            Assert.fail("Invalid code did not produce an error");
        } catch (IllegalArgumentException e)
        {
            // ok
        } catch (Exception e)
        {
            Assert.fail("Invalid code produced the wrong exception");
        }
    }
    
    
    private void checkDeck(List<LoRCardCount> deck)
    {
        try
        {
            LoRDeck.encode(deck);
            Assert.fail("Invalid deck did not produce an error");
        } catch (IllegalArgumentException e)
        {
            // ok
        } catch (Exception e)
        {
            Assert.fail("Invalid deck produced the wrong exception");
        }
    }
    
    private boolean checkSameDeck(List<LoRCardCount> a, List<LoRCardCount> b)
    {
        if (a.size() != b.size())
        {
            return false;
        }
        
        for (LoRCardCount bCard : b)
        {
            boolean found = false;
            for (LoRCardCount aCard : a)
            {
                if (aCard.equals(bCard))
                {
                    found = true;
                    break;
                }
            }
            
            if (!found)
            {
                return false;
            }
            
        }
        return true;
    }
}
