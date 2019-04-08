#addin nuget:?package=Cake.Sonar

#tool nuget:?package=xunit.runner.console
#tool nuget:?package=MSBuild.SonarQube.Runner.Tool

// ARGUMENTS
var target = Argument("target", "Default");
var sonarLabel = Argument("label", "");

var testsResultDirectory = MakeAbsolute(Directory("./TestResults"));
var sonarKey = EnvironmentVariable("SONAR_KEY");
var sonarUrl = EnvironmentVariable("SONAR_URL");

const string ApplicationName = "BoxrNet";
const string Configuration = "Release";


Task("Build").Does(() =>
{
    DotNetCoreBuild($"./{ApplicationName}.sln", 
        new DotNetCoreBuildSettings()
        {
            Configuration = Configuration
        });
});

	
Task("Tests").Does(() =>
{
    foreach(var project in GetFiles("./tests/**/*.csproj"))
    {
        DotNetCoreTest(
            project.GetDirectory().FullPath,
            new DotNetCoreTestSettings()
            {
                Configuration = Configuration
            });
    }
});
	  

Task("SonarBegin").WithCriteria(ShouldRunSonarQube()).Does(() => 
{
    SonarBegin(new SonarBeginSettings{
        Url = sonarUrl,
        Login = sonarKey,
        Key = ApplicationName,
        Version = sonarLabel,
        //OpenCoverReportsPath = openCovertFileName,
        Verbose = true
    });
});


Task("SonarEnd").WithCriteria(ShouldRunSonarQube()).Does(() => 
{
    SonarEnd(new SonarEndSettings{
        Login = sonarKey
    });
});


Task("Default")
	.IsDependentOn("SonarBegin")
	.IsDependentOn("Build")
	.IsDependentOn("Tests")
	.IsDependentOn("SonarEnd");

RunTarget(target);

private bool ShouldRunSonarQube() => !string.IsNullOrEmpty(sonarKey) && !string.IsNullOrEmpty(sonarUrl);