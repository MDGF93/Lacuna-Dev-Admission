// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Lacuna_Dev_Admission.Entities;
using Lacuna_Dev_Admission.Requests;
using Lacuna_Dev_Admission.Services;
using Serilog;

namespace Lacuna_Dev_Admission;

public class Program
{
    private static async Task Main()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        var userService = new UserService();
        var accessTokenService = new AccessTokenService();
        var jobService = new JobService();
        var user = new UserEntity();
        var submittingCredentials = true;
        while (submittingCredentials)
        {
            Console.WriteLine("Please enter credentials:");
            Console.WriteLine("1. Create new user");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");

            Console.Write("Enter your choice: ");
            var choice = int.Parse(Console.ReadLine()!);

            switch (choice)
            {
                case 1:
                    Console.Write("Enter your username: ");
                    var username = Console.ReadLine()!;
                    Console.Write("Enter your password: ");
                    var password = Console.ReadLine()!;
                    Console.Write("Enter your email: ");
                    var email = Console.ReadLine()!;
                    var createUserRequest = new CreateUserRequest(username, password, email);
                    await userService.CreateNewUser(createUserRequest);
                    user.UserName = username;
                    user.Password = password;
                    user.Email = email;
                    break;
                case 2:
                    Console.Write("Enter your username: ");
                    var usernameLogin = Console.ReadLine()!;
                    Console.Write("Enter your password: ");
                    var passwordLogin = Console.ReadLine()!;
                    var createAccessTokenRequest = new CreateAccessTokenRequest(usernameLogin, passwordLogin);
                    var accessToken = await accessTokenService.CreateNewAccessToken(createAccessTokenRequest);
                    if (accessToken.Code == "Success")
                    {
                        user.UserName = usernameLogin;
                        user.Password = passwordLogin;
                        submittingCredentials = false;
                    }
                    else
                    {
                        Console.WriteLine("Login failed. Please try again.");
                    }

                    break;
                case 3:
                    Console.Write("Exiting...");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        Console.WriteLine($"Welcome {user.UserName}!");
        user.AssignedJobs = null;
        var accessTokenRequest = new CreateAccessTokenRequest(user.UserName, user.Password);
        user.AccessTokenObj = await accessTokenService.CreateNewAccessToken(accessTokenRequest);
        Console.WriteLine(user.AccessTokenObj.AccessToken);
        Console.WriteLine("Expired: " + user.AccessTokenObj.IsExpired());
        while (true)
        {
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Request job");
            Console.WriteLine("2. Exit");
            Console.Write("Enter your choice: ");
            var choice = int.Parse(Console.ReadLine()!);
            switch (choice)
            {
                case 1:
                    if (user.AccessTokenObj.IsExpired())
                        user.AccessTokenObj = await accessTokenService.CreateNewAccessToken(accessTokenRequest);
                    Debug.Assert(user.AccessTokenObj.AccessToken != null, "user.AccessTokenObj.AccessToken != null");
                    var job = await jobService.RequestJob(user.AccessTokenObj.AccessToken);
                    Console.WriteLine("Job received:");
                    job.PrintJob();
                    switch (job.Type)
                    {
                        case "DecodeStrand":
                            Debug.Assert(job.StrandEncoded != null, "job.StrandEncoded != null");
                            var decodeAnswer = new AllOperationsRequest(Operations.DecodeDna(job.StrandEncoded),
                                "DecodeStrand");
                            await jobService.SubmitJob(job, decodeAnswer, job.Id, user.AccessTokenObj.AccessToken);
                            break;
                        case "EncodeStrand":
                            Debug.Assert(job.Strand != null, "job.Strand != null");
                            var encodeAnswer =
                                new AllOperationsRequest(Operations.EncodeDna(job.Strand), "EncodeStrand");
                            await jobService.SubmitJob(job, encodeAnswer, job.Id, user.AccessTokenObj.AccessToken);
                            break;
                        case "CheckGene":
                            Debug.Assert(job.GeneEncoded != null, "job.GeneEncoded != null");
                            Debug.Assert(job.StrandEncoded != null, "job.StrandEncoded != null");
                            var checkGeneAnswer =
                                new AllOperationsRequest(Operations.CheckGene(job.GeneEncoded, job.StrandEncoded));
                            await jobService.SubmitJob(job, checkGeneAnswer, job.Id, user.AccessTokenObj.AccessToken);
                            break;
                    }

                    break;
                case 2:
                    Console.Write("Exiting...");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}