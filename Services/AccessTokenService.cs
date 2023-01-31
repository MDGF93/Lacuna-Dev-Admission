using System.Text;
using Lacuna_Dev_Admission.Entities;
using Lacuna_Dev_Admission.Requests;
using Lacuna_Dev_Admission.Responses;
using Newtonsoft.Json;
using Serilog;

namespace Lacuna_Dev_Admission.Services;

public class AccessTokenService
{
    private readonly string _baseUrl = "https://gene.lacuna.cc/";
    private readonly Guid _g = Guid.NewGuid();

    private readonly HttpClient _httpClient;


    public AccessTokenService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<AccessTokenEntity> CreateNewAccessToken(CreateAccessTokenRequest createAccessTokenRequest)
    {
        var json = JsonConvert.SerializeObject(createAccessTokenRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_baseUrl + "api/users/login", content);
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var createAccessTokenResponse = JsonConvert.DeserializeObject<CreateAccessTokenResponse>(jsonResponse);
        Log.Information(createAccessTokenResponse?.Code == "Success"
            ? $"Access token created successfully for user {createAccessTokenRequest.UserName}."
            : $"Access token creation failed for {createAccessTokenRequest.UserName}, {createAccessTokenResponse!.Code}. Error id: {_g}");
        var accessTokenEntity = JsonConvert.DeserializeObject<AccessTokenEntity>(jsonResponse) ??
                                throw new InvalidOperationException();

        return accessTokenEntity;
    }
}