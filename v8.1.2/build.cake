var target = Argument("target", "Default");

Task("Build")
    .Does(() =>
{
    DotNetBuild("./Microsoft.Pfe.Xrm.Core.csproj", settings => 
        settings.SetConfiguration("Debug")
        .WithTarget("Clean;Rebuild"));
});

Task("Remove")
    .IsDependentOn("Build")
    .Does(() =>
{
    CleanDirectory("C:/Dev/Nuget");
});

Task("Default")
    .IsDependentOn("Remove")
    .Does(() => 
{
    var s = new NuGetPackSettings() {
        OutputDirectory = "C:/Dev/Nuget",
        Files = new [] {
            new NuSpecContent {
                Source = "bin/Debug/Microsoft.Pfe.Xrm.Core.dll",
                Target = "lib/net45"
            },
            new NuSpecContent {
                Source = "bin/Debug/Microsoft.Pfe.Xrm.Core.pdb",
                Target = "lib/net45"
            }
        }
    };
    NuGetPack("./Microsoft.Pfe.Xrm.Core.nuspec", s);
});

RunTarget(target);