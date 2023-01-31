namespace Lacuna_Dev_Admission.Requests;

public class DecodeStrandRequest
{
    public string strand { get; set; }
    public string accessToken { get; set; }
    
    public DecodeStrandRequest(string strand, string accessToken)
    {
        this.strand = strand;
        this.accessToken = accessToken;
    }
}