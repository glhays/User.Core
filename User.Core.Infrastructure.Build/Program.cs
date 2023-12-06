// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using ADotNet.Clients;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks.SetupDotNetTaskV3s;

namespace User.Core.Infrastructure.Build
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var adotNetClient = new ADotNetClient();

            var githubPipeline = new GithubPipeline
            {
                Name = "User Core Build",

                OnEvents = new Events
                {
                    PullRequest = new PullRequestEvent
                    {
                        Branches = new string[] { "main" }
                    },

                    Push = new PushEvent
                    {
                        Branches = new string[] { "main" }
                    }
                },

                Jobs = new Dictionary<string, Job>
                {
                    {
                        "build",
                        new Job
                        {
                            RunsOn = BuildMachines.WindowsLatest,

                            Steps = new List<GithubTask>
                            {
                                new CheckoutTaskV3
                                {
                                    Name = "Checking Out Code"
                                },

                                new SetupDotNetTaskV3
                                {
                                    Name = "Installing .NET",
                                    With = new TargetDotNetVersionV3
                                    {
                                        DotNetVersion = "8.0.100-preview.7.23376.3",
                                    }
                                },

                                new RestoreTask
                                {
                                    Name = "Restoring NuGet Packages"
                                },

                                new DotNetBuildTask
                                {
                                    Name = "Building Project"
                                },

                                new TestTask
                                {
                                    Name = "Running Tests"
                                }
                            }
                        }
                    }
                }
            };
            string buildScriptPath = "../../../../.github/workflows/dotnet.yml";
            string directoryPath = Path.GetDirectoryName(buildScriptPath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            adotNetClient.SerializeAndWriteToFile(
                adoPipeline: githubPipeline,
                path: buildScriptPath);
        }
    }
}