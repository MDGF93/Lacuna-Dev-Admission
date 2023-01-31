using System.Text;
using Lacuna_Dev_Admission.Repositories;
using Lacuna_Dev_Admission.Requests;
using Lacuna_Dev_Admission.Responses;
using Newtonsoft.Json;
using Serilog;

namespace Lacuna_Dev_Admission.Services;

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly string _baseUrl = "https://gene.lacuna.cc/";
    private readonly Guid _g = Guid.NewGuid();
    private readonly HttpClient _httpClient;

    public UserService()
    {
        _userRepository = new UserRepository();
        _httpClient = new HttpClient();
    }

    public async Task<CreateUserResponse> CreateNewUser(CreateUserRequest createUserRequest)
    {
        var json = JsonConvert.SerializeObject(createUserRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_baseUrl + "api/users/create", content);
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var createUserResponse = JsonConvert.DeserializeObject<CreateUserResponse>(jsonResponse);
        if (createUserResponse?.Code == "Success")
        {
            _userRepository.Save(createUserRequest);
            Log.Information("User created successfully.");
        }
        else
        {
            Log.Information($"User creation failed, {createUserResponse?.Code}. Error id: {_g}");
        }

        return createUserResponse ?? throw new InvalidOperationException();
    }
}