using System.Text;

namespace Lacuna_Dev_Admission;

public class Operations
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
        var encodedBytes = Convert.FromBase64String(encodedDna);

        // Initialize a string builder to hold the decoded DNA
        var decodedDna = new StringBuilder();

        // Iterate through the byte array and convert each group of 2 bits to a nucleobase
        for (var i = 0; i < encodedBytes.Length; i++)
        {
            var encodedByte = encodedBytes[i];
            for (var j = 6; j >= 0; j -= 2)
            {
                var nucleobaseCode = (byte)((encodedByte >> j) & 0b11);
                decodedDna.Append(nucleobaseCodes[nucleobaseCode]);
            }
        }

        // Return the decoded DNA
        return decodedDna.ToString();
    }

    public static string EncodeDna(string dna)
    {
        // Create a dictionary to store the binary codes for each nucleobase
        var nucleobaseCodes = new Dictionary<char, byte>
        {
            { 'A', 0b00 },
            { 'C', 0b01 },
            { 'T', 0b11 },
            { 'G', 0b10 }
        };

        // Convert the DNA string to an array of bytes
        var dnaBytes = dna
            .Select(c => nucleobaseCodes[c])
            .ToArray();
        Console.WriteLine("DNA encoded!");


        var binaryAsAscii = string.Join("", dnaBytes.Select(x => Convert.ToString(x, 2).PadLeft(2, '0')));
        var bytes = Enumerable.Range(0, binaryAsAscii.Length / 8)
            .Select(i => Convert.ToByte(binaryAsAscii.Substring(i * 8, 8), 2))
            .ToArray();

        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    ///     I'd like to take a moment to explain my thought process for this method.
    ///     I'll be using this call as an example: CheckGene(gene = "ATCGACAGGAC", strand = "CATCGTCAGGAC")
    ///     Inputs:
    ///     Strand = "CATCGTCAGGAC"
    ///     Gene   =│"ATCGACAGGAC"│
    ///     └──────┬──────┘
    ///     │
    ///     Gene length = 11
    ///     │
    ///     │
    ///     ▼
    ///     Activation threshold = Ceiling(11/2) = 6
    ///     So the gene could be considered active inside DNA strand
    ///     if there's a substring of length 6 that's shared between
    ///     the both of them.
    ///     The solution I came up with was to mimic a digital caliper.
    ///     Here's how the algorithm works, first we measure our activation
    ///     threshold, then we transverse our gene string using the length
    ///     we found on our activation threshold as a basis for our step.
    ///     Here's an illustration of how it works for the example above:
    ///     Original input:  ATCGACAGGAC
    ///     │
    ///     ▼
    ///     Measuring the threshold: │ATCGAC│AGGAC
    ///     └──┬───┘
    ///     │
    ///     └─► Threshold of length 6
    ///     ┌──────┐
    ///     Selected substring:  │ATCGAC│AGGAC
    ///     └──────┘
    ///     We now take our first substring: ATCGAC and check if it present
    ///     in our gene string, if it is, return true, if it's not we move
    ///     our digital caliper one step forward so the next substring it'll
    ///     check will be:
    ///     ┌──────┐
    ///     A│TCGACA│GGAC
    ///     └──────┘
    ///     Then, the algorithm will keep reapeating the above instructions
    ///     until we either find a substring that's acceptable or we reach
    ///     the end of our gene, meaning that this gene is not active inside
    ///     our DNA strand.
    /// </summary>
    /// <param name="gene"></param>
    /// <param name="dnaStrand"></param>
    /// <returns></returns>
    public static bool CheckGene(string gene, string strand)
    {
        var decodedGene = DecodeDna(gene);
        var decodedStrand = DecodeDna(strand);
        if (decodedStrand.Substring(0, 3) != "CAT") decodedStrand = GetTemplateStrand(decodedStrand);
        var activationThreshold = (int)Math.Ceiling((double)decodedGene.Length / 2);
        for (var i = 0; i < activationThreshold; i++)
        {
            var minimalGene = decodedGene.Substring(i, activationThreshold + i - i);
            if (decodedStrand.Contains(minimalGene)) return true;
        }

        return false;
    }

    private static string GetTemplateStrand(string input)
    {
        Console.WriteLine("Chamou o tmepalte");
        var mapping = new Dictionary<char, char>
        {
            { 'A', 'T' },
            { 'T', 'A' },
            { 'C', 'G' },
            { 'G', 'C' }
        };

        var chars = input.ToCharArray();
        for (var i = 0; i < chars.Length; i++)
            if (mapping.ContainsKey(chars[i]))
                chars[i] = mapping[chars[i]];
        return new string(chars);
    }
}