namespace Lacuna_Dev_Admission;

public class GeneChecker
{

    /// <summary>
    /// I'd like to take a moment to explain my thought process for this method.
    /// I'll be using this call as an example: CheckGene(gene = "ATCGACAGGAC", strand = "CATCGTCAGGAC")
    /// Inputs:
    /// Strand = "CATCGTCAGGAC"
    /// Gene   =│"ATCGACAGGAC"│
    ///         └──────┬──────┘
    ///                │
    ///         Gene length = 11
    ///                │
    ///                │
    ///                ▼
    /// Activation threshold = Ceiling(11/2) = 6
    /// 
    /// So the gene could be considered active inside DNA strand
    /// if there's a substring of length 6 that's shared between
    /// the both of them.
    /// 
    /// The solution I came up with was to mimic a digital caliper.
    /// Here's how the algorithm works, first we measure our activation
    /// threshold, then we transverse our gene string using the length
    /// we found on our activation threshold as a basis for our step.
    /// 
    /// Here's an illustration of how it works for the example above:
    /// 
    ///          Original input:  ATCGACAGGAC
    ///                                │
    ///                                ▼
    /// Measuring the threshold: │ATCGAC│AGGAC
    ///                          └──┬───┘
    ///                             │
    ///                             └─► Threshold of length 6
    /// 
    ///                           ┌──────┐
    ///      Selected substring:  │ATCGAC│AGGAC
    ///                           └──────┘
    /// 
    /// We now take our first substring: ATCGAC and check if it present
    /// in our gene string, if it is, return true, if it's not we move
    /// our digital caliper one step forward so the next substring it'll
    /// check will be:
    ///                            ┌──────┐
    ///                           A│TCGACA│GGAC
    ///                            └──────┘
    /// 
    /// Then, the algorithm will keep reapeating the above instructions
    /// until we either find a substring that's acceptable or we reach
    /// the end of our gene, meaning that this gene is not active inside
    /// our DNA strand.
    /// </summary>
    /// <param name="gene"></param>
    /// <param name="dnaStrand"></param>
    /// <returns></returns>

    

    public static bool CheckGene(string gene, string strand) {
        
        //We create a string that's a copy of strand so we don't reassign the function argument.
        string templateStrand = strand;
        if (strand.Substring(0, 3) != "CAT")
        {
            templateStrand = GetTemplateStrand(strand);
        }
        int activationThreshold = (int) Math.Ceiling((double) gene.Length / 2);
        for (int i = 0; i < activationThreshold; i++) {
            string minimalGene = gene.Substring(i, activationThreshold + i - i);
            if (templateStrand.Contains(minimalGene)) {
                return true;
            }
        }

        return false;
    }

    private static string GetTemplateStrand(string input)
    {
        Dictionary<char, char> mapping = new Dictionary<char, char>()
        {
            { 'A', 'T' },
            { 'T', 'A' },
            { 'C', 'G' },
            { 'G', 'C' }
        };

        char[] chars = input.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            if (mapping.ContainsKey(chars[i]))
            {
                chars[i] = mapping[chars[i]];
            }
        }
        return new string(chars);
    }
    
}