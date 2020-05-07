using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApiClient
{
  class GitHubTasks
  {
    private static readonly HttpClient client = new HttpClient();

    public static async Task AddOrganization()
    {
      Console.WriteLine("Enter the name of the github org you want to see.");
      var input = Console.ReadLine();

      var org = new Organization(input);

      var repositories = await FetchRepositories(input, 1);
    
      await org.AddRepositories(repositories, 1);

      Program.Organizations.Add(org);

      await PrintRepositories(org.Repositories);

      await Program.AwaitUserInput();

    }
    public static async Task<List<Repository>> FetchRepositories(string orgName, int page)
    {
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(
          new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
      client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

      var streamTask = client.GetStreamAsync($"https://api.github.com/orgs/{orgName}/repos?per_page=25&page={page}&sort=updated");
      var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(await streamTask);

      return repositories;
    }

    public static Task PrintRepositories(List<Repository> repositories)
    {
      foreach (var repo in repositories)
      {
        Console.WriteLine($"id: {repo.repoId}; Name: {repo.repoName}; pushed: {repo.lastPush}");
      }

      return Task.CompletedTask;
    }

    public static int PrintOrganizations()
    {
      Console.WriteLine("Select from the following organizations");
      Console.WriteLine("0 - Add New");


      int counter = 1;

      foreach(var org in Program.Organizations)
      {
          Console.WriteLine($"{counter} - {org.Name} - {org.Repositories.Count} Repositories found");

          counter++;
      }

      Console.WriteLine("q - Exit");

      var response = Console.ReadKey(true).KeyChar;

      return response;
    }
  }
}