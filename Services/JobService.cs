using System.Net.Http.Headers;
using System.Text;
using Lacuna_Dev_Admission.Entities;
using Lacuna_Dev_Admission.Repositories;
using Lacuna_Dev_Admission.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Events;

namespace Lacuna_Dev_Admission.Services;

public class JobService
{
    private readonly string _baseUrl = "https://gene.lacuna.cc/";
    private readonly JobRepository _jobRepository = new();
    private readonly HttpClient _httpClient;
    private readonly Guid _g = Guid.NewGuid();


    public JobService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<JobEntity> RequestJob(string accessToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await _httpClient.GetAsync(_baseUrl + "api/dna/jobs");
        var responseJson = await response.Content.ReadAsStringAsync();
        var jsonObject = JObject.Parse(responseJson);
        var jobEntity = jsonObject["job"]?.ToObject<JobEntity>();
        if (jobEntity != null)
        {
            Log.Logger.Information("Job requested successfully");
        }
        else
        {
            Log.Logger.Error("Job request failed. Error id: {Guid}", _g);
        }

        return jobEntity ?? throw new InvalidOperationException();
    }

    public async Task SubmitJob(JobEntity jobEntity, AllOperationsRequest answer, string jobId,
        string accessToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var jsonSerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        var jsonRequest = JsonConvert.SerializeObject(answer, jsonSerializerSettings);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        if (answer.strand != null)
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/dna/jobs/{jobId}/decode", content);
            var responseJson = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(responseJson);
            _jobRepository.Save(jobEntity, jsonObject["code"]?.ToString() ?? throw new InvalidOperationException());
            Log.Logger.Information("Job completed successfully");
        }

        else if (answer.strandEncoded != null)
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/dna/jobs/{jobId}/encode", content);
            var responseJson = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(responseJson);
            Log.Logger.Information("Encoding completed successfully");
            _jobRepository.Save(jobEntity, jsonObject["code"]?.ToString() ?? throw new InvalidOperationException());
            Log.Logger.Information("Job completed successfully");
        }

        else if (answer.isActivated != null)
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/dna/jobs/{jobId}/gene", content);
            var responseJson = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(responseJson);
            Log.Logger.Information("Gene checking completed successfully");
            _jobRepository.Save(jobEntity, jsonObject["code"]?.ToString() ?? throw new InvalidOperationException());
            if (jsonObject["code"].ToString() == "Success")
            Log.Logger.Information("Job completed successfully");
        }
    }
}