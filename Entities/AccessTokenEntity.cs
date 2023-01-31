namespace Lacuna_Dev_Admission.Entities;

public class AccessTokenEntity
{
    public AccessTokenEntity(string code, string? accessToken, DateTime? createdDateTime, DateTime? expirationDateTime,
        string? message)
    {
        Code = code;
        AccessToken = accessToken;
        CreatedDateTime = createdDateTime;
        ExpirationDateTime = expirationDateTime;
        Message = message;
    }

    public AccessTokenEntity(string code, string? accessToken, string? message)
    {
        Code = code;
        if (code == "Success")
        {
            AccessToken = accessToken;
            CreatedDateTime = DateTime.Now;
            ExpirationDateTime = DateTime.Now.AddMinutes(2);
        }
        else
        {
            AccessToken = null;
            CreatedDateTime = null;
            ExpirationDateTime = null;
        }
    }

    public AccessTokenEntity()
    {
    }

    public string Code { get; set; } //Code can be one of these values: "Success" or "Error"
    public string? AccessToken { get; set; }
    public DateTime? CreatedDateTime { get; set; } //Will be set to DateTime.Now, if token is created
    public DateTime? ExpirationDateTime { get; set; } //the token expires after 2 minutes
    public string? Message { get; set; } //Message can be one of these values: "Success" or "Error"


    public bool IsExpired()
    {
        return DateTime.Now > ExpirationDateTime;
    }
}