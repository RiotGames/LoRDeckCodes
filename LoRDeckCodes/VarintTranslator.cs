using System;
using System.Collections.Generic;

//THIS CODE ADAPTED FROM
/*
VarintBitConverter: https://github.com/topas/VarintBitConverter 
Copyright (c) 2011 Tomas Pastorek, Ixone.cz. All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions
are met:

 1. Redistributions of source code must retain the above copyright
    notice, this list of conditions and the following disclaimer.

 2. Redistributions in binary form must reproduce the above
    copyright notice, this list of conditions and the following
    disclaimer in the documentation and/or other materials provided
    with the distribution.

THIS SOFTWARE IS PROVIDED BY TOMAS PASTOREK AND CONTRIBUTORS ``AS IS'' 
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL TOMAS PASTOREK OR CONTRIBUTORS 
BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF 
THE POSSIBILITY OF SUCH DAMAGE. 

*/


namespace LoRDeckCodes
{
    public class VarintTranslator
    {
        private const byte AllButMSB = 0x7f;
        private const byte JustMSB = 0x80;

        public static int PopVarint(List<byte> bytes)
        {
            ulong result = 0;
            int currentShift = 0;
            int bytesPopped = 0;

            for (int i = 0; i < bytes.Count; i++)
            {
                bytesPopped++;
                ulong current = (ulong)bytes[i] & AllButMSB;
                result |= current << currentShift;

                if ((bytes[i] & JustMSB) != JustMSB)
                {
                    bytes.RemoveRange(0, bytesPopped);
                    return (int)result;
                }

                currentShift += 7;

            }

            throw new ArgumentException("Byte array did not contain valid varints.");
        }

        public static byte[] GetVarint(ulong value)
        {
            var buff = new byte[10];
            var currentIndex = 0;

            if (value == 0)
                return new byte[1] { 0 };

            while (value != 0)
            {
                var byteVal = value & AllButMSB;
                value >>= 7;

                if (value != 0)
                    byteVal |= 0x80;

                buff[currentIndex++] = (byte)byteVal;

            } 

            byte[] result = new byte[currentIndex];
            Buffer.BlockCopy(buff, 0, result, 0, currentIndex);

            return result;
        }

        public static byte[] GetVarint(int value)
        {
            return GetVarint((ulong)value);
        }
    }
}
