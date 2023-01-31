namespace Lacuna_Dev_Admission.Entities;

public class UserEntity
{
    public UserEntity(string userName, string password, string email, AccessTokenEntity accessTokenObj,
        JobEntity[] assignedJobs)
    {
        UserName = userName;
        Password = password;
        Email = email;
        AccessTokenObj = accessTokenObj;
        AssignedJobs = assignedJobs;
    }

    public UserEntity(string userName, string password, string email)
    {
        UserName = userName;
        Password = password;
        Email = email;
    }

    public UserEntity()
    {
    }

    public UserEntity(string userName, string password)
    {
        UserName = userName;
        Password = password;
        Email = "not used";
    }

    public string UserName { get; set; }

    public string Password { get; set; }

    public string? Email { get; set; }
    public AccessTokenEntity? AccessTokenObj { get; set; }
    public JobEntity[]? AssignedJobs { get; set; }
}