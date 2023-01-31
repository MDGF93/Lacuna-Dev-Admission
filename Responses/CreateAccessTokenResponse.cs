namespace Lacuna_Dev_Admission.Responses;

public class CreateAccessTokenResponse
{
    public string? AccessToken { get; set; }
    public string Code { get; set; }
    public string? Message { get; set; }
}