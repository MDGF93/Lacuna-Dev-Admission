namespace Lacuna_Dev_Admission.Responses;

public class CreateUserResponse
{
    public string Code { get; set; } // 'Success' or 'Error'
    public string? Message { get; set; }
}