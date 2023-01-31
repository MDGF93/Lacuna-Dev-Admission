namespace Lacuna_Dev_Admission.Requests;

public class AllOperationsRequest
{
    public AllOperationsRequest(string answer, string operation)
    {
        switch (operation)
        {
            case "DecodeStrand":
                strand = answer;
                break;
            case "EncodeStrand":
                strandEncoded = answer;
                break;
        }
    }


    public AllOperationsRequest(bool answer)
    {
        isActivated = answer;
    }

    public string? strand { get; set; }
    public string? strandEncoded { get; set; }
    public bool? isActivated { get; set; }
}