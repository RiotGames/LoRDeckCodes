import no.stelar7.lor.Base32;
import org.junit.*;

import java.nio.charset.StandardCharsets;

public class TestBase32
{
    @Test
    public void encodeDecodeReturnsSame()
    {
        String test = "THIS IS MY TEST";
        byte[] data = test.getBytes(StandardCharsets.UTF_8);
        
        String encoded = Base32.encode(data);
        byte[] decoded = Base32.decode(encoded);
        
        String original = new String(decoded, StandardCharsets.UTF_8);
        
        Assert.assertEquals("THe input is not the same as the output", test, original);
    }
}
