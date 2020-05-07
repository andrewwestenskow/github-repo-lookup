using System;
using System.Threading.Tasks;

namespace WebApiClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter the name of the github org you want to see.");
            var input = Console.ReadLine();

            await GitHubTasks.ProcessRepositories(input, 1);
        }

    }
}
