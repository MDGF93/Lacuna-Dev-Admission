using System.Net.Http.Headers;
using System.Text;
using Lacuna_Dev_Admission.Entities;
using Lacuna_Dev_Admission.Repositories;
using Lacuna_Dev_Admission.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

namespace Lacuna_Dev_Admission.Services;

public class JobService
{
    private readonly string _baseUrl = "https://gene.lacuna.cc/";
    private readonly JobRepository _jobRepository = new();
    private readonly HttpClient _httpClient;


    public JobService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<JobEntity> RequestJob(string accessToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await _httpClient.GetAsync(_baseUrl + "api/dna/jobs");
        var responseJson = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseJson);
        var jsonObject = JObject.Parse(responseJson);
        var jobEntity = jsonObject["job"]?.ToObject<JobEntity>();
        jobEntity?.PrintJob();
        return jobEntity ?? throw new InvalidOperationException();
    }

    public async Task<string> SubmitJob(JobEntity jobEntity, AllOperationsRequest answer, string jobId,
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
            Console.WriteLine(responseJson);
            var jsonObject = JObject.Parse(responseJson);
            Log.Logger.Information("Decoding completed successfully");
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            _jobRepository.Save(jobEntity, jsonObject["code"]?.ToString() ?? throw new InvalidOperationException());
        }

        else if (answer.strandEncoded != null)
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/dna/jobs/{jobId}/encode", content);
            var responseJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseJson);
            var jsonObject = JObject.Parse(responseJson);
            Log.Logger.Information("Encoding completed successfully");
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            _jobRepository.Save(jobEntity, jsonObject["code"]?.ToString() ?? throw new InvalidOperationException());
        }

        else if (answer.isActivated != null)
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/dna/jobs/{jobId}/gene", content);
            var responseJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseJson);
            var jsonObject = JObject.Parse(responseJson);
            Log.Logger.Information("Gene checking completed successfully");
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            _jobRepository.Save(jobEntity, jsonObject["code"]?.ToString() ?? throw new InvalidOperationException());
        }

        return jsonRequest;
    }
}