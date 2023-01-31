namespace Lacuna_Dev_Admission.Requests;

public class CreateUserRequest
{
    public CreateUserRequest(string userName, string password, string email)
    {
        UserName = userName;
        Password = password;
        Email = email;
    }

    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}