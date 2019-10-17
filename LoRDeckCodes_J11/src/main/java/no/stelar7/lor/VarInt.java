package no.stelar7.lor;

import java.util.*;

public class VarInt
{
    
    private static final int allButMSB = 0x7f;
    private static final int justMSB   = 0x80;
    
    public static int pop(List<Byte> bytes)
    {
        int result = 0;
        int shift  = 0;
        int popped = 0;
        
        for (int i = 0; i < bytes.size(); i++)
        {
            popped++;
            int current = bytes.get(i) & allButMSB;
            result |= current << shift;
            
            if ((bytes.get(i) & justMSB) != justMSB)
            {
                bytes.subList(0, popped).clear();
                return result;
            }
            
            shift += 7;
        }
        
        throw new IllegalArgumentException("Byte array did not contain valid VarInts");
    }
    
    public static List<Integer> get(int value)
    {
        Integer[] data = new Integer[10];
        Arrays.fill(data, 0);
        List<Integer> buffer = new ArrayList<>(Arrays.asList(data));
        
        int index = 0;
        
        if (value == 0)
        {
            return Collections.singletonList(0);
        }
        
        while (value != 0)
        {
            int byteVal = value & allButMSB;
            value >>>= 7;
            
            if (value != 0)
            {
                byteVal |= justMSB;
            }
            
            buffer.set(index++, byteVal);
        }
        
        return buffer.subList(0, index);
    }
}
