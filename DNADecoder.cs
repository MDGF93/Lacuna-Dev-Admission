using System.Text;

namespace Lacuna_Dev_Admission;

public class DnaDecoder
{
    public static string DecodeDna(string encodedDna)
    {
        // Create a dictionary to store the nucleobases for each binary code
        var nucleobaseCodes = new Dictionary<byte, char>
        {
            { 0b00, 'A' },
            { 0b01, 'C' },
            { 0b11, 'T' },
            { 0b10, 'G' }
        };

        // Convert the encoded DNA to a byte array
        byte[] encodedBytes = Convert.FromBase64String(encodedDna);

        // Initialize a string builder to hold the decoded DNA
        var decodedDna = new StringBuilder();

        // Iterate through the byte array and convert each group of 2 bits to a nucleobase
        for (int i = 0; i < encodedBytes.Length; i++)
        {
            byte encodedByte = encodedBytes[i];
            for (int j = 6; j >= 0; j -= 2)
            {
                byte nucleobaseCode = (byte)((encodedByte >> j) & 0b11);
                decodedDna.Append(nucleobaseCodes[nucleobaseCode]);
            }
        }

        // Return the decoded DNA
        return decodedDna.ToString();
    }
}
