using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiClient
{
    class Program
    {
        public static List<Organization> Organizations = new List<Organization>();

        static public async Task AwaitUserInput()
        {
            
            var response = GitHubTasks.PrintOrganizations();

            int i = response - '0';
            
            if(response == '0')
            {
                await GitHubTasks.AddOrganization();
            } else if (i <= Organizations.Count)
            {
                Console.WriteLine("-------------Calculating ------------");
                Console.WriteLine($"Found org {Organizations[i - 1].Name} - {Organizations[i-1].Repositories.Count} Repos");

                Console.WriteLine("Select from the options below");
                Console.WriteLine("P - Print Repos");

                var secondResponse = Console.ReadKey(true).KeyChar.ToString().ToLower();

                if(secondResponse == "p")
                {
                await GitHubTasks.PrintRepositories(Organizations[i - 1].Repositories);
                }                

                await AwaitUserInput();
            } else if(response != 'q'){
                await AwaitUserInput();
            } else if(response == 'q')
            {
                Console.WriteLine("Goodbye");
            }
        }
        static async Task Main(string[] args)
        {
            await AwaitUserInput();
        }

    }
}
