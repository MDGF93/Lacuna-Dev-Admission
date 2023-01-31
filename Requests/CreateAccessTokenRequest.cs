namespace Lacuna_Dev_Admission.Requests;

public class CreateAccessTokenRequest
{
    public CreateAccessTokenRequest(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }

    public string UserName { get; set; }
    public string Password { get; set; }
}