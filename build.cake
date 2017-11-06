#addin nuget:?package=Cake.Incubator&version=1.5.0
#addin nuget:?package=Cake.Git&version=0.16.1
#addin nuget:?package=Newtonsoft.Json

using Newtonsoft.Json;

var target = Argument<string>("Target");
var config = Argument<string>("Configuration");

//nuget
var nugetKey = EnvironmentVariable("NUGET_KEY");
var nugetSource = EnvironmentVariable("NUGET_SOURCE", "https://www.nuget.org/api/v2/package");

var version = GitDescribe(".", false, GitDescribeStrategy.Tags, 0)?.ToLower().TrimStart('v') ?? "debug";
Information("Building {0}...", $"v{version}");

//CI Environment vars
var isOnTravis = EnvironmentVariable<bool>("TRAVIS", false);
var prNumber = EnvironmentVariable("TRAVIS_PULL_REQUEST");
var branch = EnvironmentVariable("TRAVIS_BRANCH");
var isPR = int.TryParse(prNumber, out var _);

Task("tests")
    .Does(() =>
{
    var testProjects = GetFiles("./tests/**/*Tests.csproj");
    foreach(var file in testProjects)
    {
        DotNetCoreTest(file.FullPath);
    }
});

Task("pack")
    .Does(() =>
{
    var msBuildSettings = new DotNetCoreMSBuildSettings();
    msBuildSettings.SetVersion(version);

    var settings = new DotNetCorePackSettings
    {
        Configuration = config,
        OutputDirectory = "./artifacts/",
        MSBuildSettings = msBuildSettings,
        IncludeSource = true,
        ToolTimeout = TimeSpan.FromMinutes(5)
    };

    Information(
        "Packing version '{0}' with this settings: {1}{2}"
        , version
        , Environment.NewLine
        , JsonConvert.SerializeObject(settings, Formatting.Indented)
    );

    DotNetCorePack("./src/JsonFluentMap/JsonFluentMap.csproj", settings);
});

Task("nuget-push")
    .Does(() =>
{
    Information("Publishing package to nuget.org");
    var settings = new DotNetCoreNuGetPushSettings
     {
         Source = nugetSource,
         ApiKey = nugetKey
     };

     DotNetCoreNuGetPush($"./artifacts/JsonFluentMap.{version}.nupkg", settings);
});

Task("travis")
    .IsDependentOn("pack")
    .Does(() =>
{
    if (isOnTravis)
    {
        Information("Build running on travis! branch: {0}", branch);
        Information("Pull Request {0}", $"#{prNumber}");

        //if the branch equals to a version
        //it means that the script is building a git tag (release)
        if(branch.ToLower().TrimStart('v') == version && !isPR)
        {
            RunTarget("nuget-push");
        }
    }
    else
    {
        Information("Build is NOT running on travis!");
    }
});

RunTarget(target);
