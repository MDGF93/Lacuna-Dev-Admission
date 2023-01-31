namespace Lacuna_Dev_Admission.Entities;

public class JobEntity
{
    public JobEntity(string id, string type, string? strand, string? strandEncoded, string? geneEncoded)
    {
        Id = id;
        Type = type;
        Strand = strand;
        StrandEncoded = strandEncoded;
        GeneEncoded = geneEncoded;
    }

    public JobEntity()
    {
    }

    public string Id { get; set; }

    public string Type { get; set; } // Operation types ['DecodeStrand', 'EncodeStrand', 'CheckGene']

    public string? Strand { get; set; } // Strand in String format. Non-null when operation type 'EncodeStrand'

    public string?
        StrandEncoded
    {
        get;
        set;
    } // Strand in the Binary format Base64 encoded. Non-null when operation types 'DecodeStrand' and 'CheckGene'

    public string?
        GeneEncoded
    {
        get;
        set;
    } // A gene segment in the Binary format Base64 encoded. Non-null when operation type 'CheckGene'

    public void PrintJob()
    {
        Console.WriteLine("Id: " + Id);
        Console.WriteLine("Type: " + Type);
        if (Strand != null) Console.WriteLine("Strand: " + Strand);
        if (StrandEncoded != null) Console.WriteLine("StrandEncoded: " + StrandEncoded);
        if (GeneEncoded != null) Console.WriteLine("GeneEncoded: " + GeneEncoded);
    }
}