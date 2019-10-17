package no.stelar7.lor;

import java.util.Arrays;

public enum LoRFaction
{
    DEMACIA("DE", 0),
    FRELJORD("FR", 1),
    IONIA("IO", 2),
    NOXUS("NX", 3),
    PILTOVER_AND_ZAUN("PZ", 4),
    SHADOW_ISLES("SI", 5);
    
    private String shortCode;
    private int    id;
    
    LoRFaction(String shortCode, int id)
    {
        this.shortCode = shortCode;
        this.id = id;
    }
    
    public String getShortCode()
    {
        return shortCode;
    }
    
    public int getId()
    {
        return id;
    }
    
    public static LoRFaction fromID(int id)
    {
        return Arrays.stream(values()).filter(a -> a.id == id).findFirst().orElse(null);
    }
    
    public static LoRFaction fromCode(String code)
    {
        return Arrays.stream(values()).filter(a -> a.shortCode.equalsIgnoreCase(code)).findFirst().orElse(null);
    }
}
