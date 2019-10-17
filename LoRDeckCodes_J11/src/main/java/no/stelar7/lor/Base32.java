package no.stelar7.lor;

import java.util.*;

public class Base32
{
    private static final String                  SEPARATOR = "-";
    private static final char[]                  CHARS     = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567".toCharArray();
    private static final int                     MASK      = CHARS.length - 1;
    private static final int                     SHIFT     = Integer.numberOfTrailingZeros(CHARS.length);
    private static final Map<Character, Integer> CHAR_MAP  = new HashMap<>()
    {{
        for (int i = 0; i < CHARS.length; i++)
        {
            put(CHARS[i], i);
        }
    }};
    
    public static Byte[] decodeBoxed(String code)
    {
        byte[] input  = decode(code);
        Byte[] result = new Byte[input.length];
        
        for (int i = 0; i < input.length; i++)
        {
            result[i] = input[i];
        }
        
        return result;
    }
    
    
    public static byte[] decode(String code)
    {
        String encoded = code.trim().replace(SEPARATOR, "");
        encoded = encoded.replaceAll("[=]*$", "");
        encoded = encoded.toUpperCase();
        
        if (encoded.length() == 0)
        {
            return new byte[]{0};
        }
        
        int    encodedLength = encoded.length();
        int    outLength     = encodedLength * SHIFT / 8;
        byte[] result        = new byte[outLength];
        
        int buffer   = 0;
        int next     = 0;
        int bitsLeft = 0;
        
        for (char c : encoded.toCharArray())
        {
            if (!CHAR_MAP.containsKey(c))
            {
                throw new IllegalArgumentException("Illegal character: " + c);
            }
            
            buffer <<= SHIFT;
            buffer |= CHAR_MAP.get(c) & MASK;
            bitsLeft += SHIFT;
            if (bitsLeft >= 8)
            {
                result[next++] = (byte) (buffer >>> (bitsLeft - 8));
                bitsLeft -= 8;
            }
            
        }
        
        return result;
    }
    
    public static String encode(byte[] data)
    {
        if (data.length == 0)
        {
            return "";
        }
        
        if (data.length >= (1 << 28))
        {
            throw new ArrayIndexOutOfBoundsException("Array is too long for this");
        }
        
        int           outputLength = (data.length * 8 + SHIFT - 1) / SHIFT;
        StringBuilder result       = new StringBuilder(outputLength);
        
        int buffer   = data[0];
        int next     = 1;
        int bitsLeft = 8;
        while (bitsLeft > 0 || next < data.length)
        {
            if (bitsLeft < SHIFT)
            {
                if (next < data.length)
                {
                    buffer <<= 8;
                    buffer |= (data[next++] & 0xff);
                    bitsLeft += 8;
                } else
                {
                    int pad = SHIFT - bitsLeft;
                    buffer <<= pad;
                    bitsLeft += pad;
                }
            }
            int index = MASK & (buffer >>> (bitsLeft - SHIFT));
            bitsLeft -= SHIFT;
            result.append(CHARS[index]);
        }
        
        if (false)
        {
            int padding = 8 - (result.length() % 8);
            if (padding > 0)
            {
                result.append("=".repeat(padding == 8 ? 0 : padding));
            }
        }
        
        return result.toString();
    }
    
    public static String encodeBoxed(List<Integer> result)
    {
        byte[] output = new byte[result.size()];
        
        for (int i = 0; i < result.size(); i++)
        {
            output[i] = result.get(i).byteValue();
        }
        
        return encode(output);
    }
}
