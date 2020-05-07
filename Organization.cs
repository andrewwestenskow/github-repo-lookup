using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiClient
{
  class Organization
  {
    public string Name {get;}
    public int lastPage {get; set;}
    public static List<Repository> Repositories = new List<Repository>();

    public Organization(string name)
    {
      this.Name = name;
      this.lastPage = 1;
    }

    public Task AddRepositories(List<Repository> newRepos, int page)
    {
      foreach (var repo in newRepos)
      {
        Repositories.Add(repo);
      }

      lastPage = page;

      return Task.CompletedTask;
    }
  }
}