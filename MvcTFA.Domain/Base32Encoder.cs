using System;
using System.Text;

namespace MvcTFA.Domain
{
    public static class Base32Encoder
    {
        private const string Base32Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        private const int InByteSize = 8;
        private const int OutByteSize = 5;

        /// <summary>
        /// Encode a byte array as a Base32 string
        /// </summary>
        public static string ToBase32String(byte[] data)
        {

            int i = 0, index = 0;
            var builder = new StringBuilder((data.Length + 7) * InByteSize / OutByteSize);

            while (i < data.Length)
            {
                int currentByte = data[i];
                int digit;

                //Is the current digit going to span a byte boundary?
                if (index > (InByteSize - OutByteSize))
                {
                    var nextByte = (i + 1) < data.Length ? data[i + 1] : 0;

                    digit = currentByte & (0xFF >> index);
                    index = (index + OutByteSize) % InByteSize;
                    digit <<= index;
                    digit |= nextByte >> (InByteSize - index);
                    i++;
                }
                else
                {
                    digit = (currentByte >> (InByteSize - (index + OutByteSize))) & 0x1F;
                    index = (index + OutByteSize) % InByteSize;

                    if (index == 0)
                    {
                        i++;
                    }
                }

                builder.Append(Base32Alphabet[digit]);
            }

            return builder.ToString();
        }

        public static byte[] FromBase32String(string input)
        {
            // Based on the code here: http://stackoverflow.com/a/7135008/465404
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException("input");
            }

            input = input.TrimEnd('='); //remove padding characters
            var byteCount = input.Length * OutByteSize / InByteSize; //this must be TRUNCATED
            var returnArray = new byte[byteCount];

            byte curByte = 0, bitsRemaining = 8;
            var arrayIndex = 0;

            foreach (var c in input)
            {
                var cValue = CharToValue(c);
                int mask;

                if (bitsRemaining > 5)
                {
                    mask = cValue << (bitsRemaining - 5);
                    curByte = (byte)(curByte | mask);
                    bitsRemaining -= 5;
                }
                else
                {
                    mask = cValue >> (5 - bitsRemaining);
                    curByte = (byte)(curByte | mask);
                    returnArray[arrayIndex++] = curByte;
                    curByte = (byte)(cValue << (3 + bitsRemaining));
                    bitsRemaining += 3;
                }
            }

            //if we didn't end with a full byte
            if (arrayIndex != byteCount)
            {
                returnArray[arrayIndex] = curByte;
            }

            return returnArray;
        }

        private static int CharToValue(char c)
        {
            var value = (int)c;

            //65-90 == uppercase letters
            if (value < 91 && value > 64)
            {
                return value - 65;
            }
            //50-55 == numbers 2-7
            if (value < 56 && value > 49)
            {
                return value - 24;
            }
            //97-122 == lowercase letters
            if (value < 123 && value > 96)
            {
                return value - 97;
            }

            throw new ArgumentException("Character is not a Base32 character.", "c");
        }
    }
}
