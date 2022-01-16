set FILES=*.nupkg

for %%f in (%FILES%) do (
	dotnet nuget push %%~f -k secret -s https://www.nuget.org/api/v2/package --skip-duplicate
)