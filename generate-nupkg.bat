dotnet build
rm *.nupkg
dotnet pack ToolsPack.Log4net\ToolsPack.Log4net.csproj -o ../
dotnet pack ToolsPack.Displayer\ToolsPack.Displayer.csproj -o ../
dotnet pack ToolsPack.Sql\ToolsPack.Sql.csproj -o ../
dotnet pack ToolsPack.Thread\ToolsPack.Thread.csproj -o ../
dotnet pack ToolsPack.Config\ToolsPack.Config.csproj -o ../
dotnet pack ToolsPack.Config\ToolsPack.String.csproj -o ../