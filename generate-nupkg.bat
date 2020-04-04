dotnet build
rm *.nupkg
dotnet pack ToolsPack.Config -o ./
dotnet pack ToolsPack.Displayer -o ./
dotnet pack ToolsPack.Log4net -o ./
dotnet pack ToolsPack.NLog -o ./
dotnet pack ToolsPack.Samba -o ./
dotnet pack ToolsPack.Sql -o ./
dotnet pack ToolsPack.String -o ./
dotnet pack ToolsPack.Thread -o ./
dotnet pack ToolsPack.Webservice -o ./
