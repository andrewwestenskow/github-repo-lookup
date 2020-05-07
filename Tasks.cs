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

    public static async Task ProcessRepositories(string orgName, int page)
    {
      var repositories = await FetchRepositories(orgName, page);
      foreach (var repo in repositories)
      {
        Console.WriteLine($"id: {repo.repoId}; Name: {repo.repoName}; pushed: {repo.lastPush}");
      }

      Console.WriteLine("-------------------------------");
      Console.WriteLine("Press right arrow for more or any other key to exit");
      var keyPress = Console.ReadKey().Key.ToString();

      Console.WriteLine(keyPress);

      if(keyPress == "RightArrow")
      {
          await ProcessRepositories(orgName, page + 1);
      }
    }
  }
}