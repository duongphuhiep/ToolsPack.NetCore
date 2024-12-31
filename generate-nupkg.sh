dotnet build
del *.nupkg
dotnet pack src/ToolsPack.Config -o ./
dotnet pack src/ToolsPack.Displayer -o ./
dotnet pack src/ToolsPack.Log4net -o ./
dotnet pack src/ToolsPack.Msql -o ./
dotnet pack src/ToolsPack.NLog -o ./
dotnet pack src/ToolsPack.Samba -o ./
dotnet pack src/ToolsPack.Sql -o ./
dotnet pack src/ToolsPack.String -o ./
dotnet pack src/ToolsPack.Thread -o ./
dotnet pack src/ToolsPack.Webservice -o ./
dotnet pack src/ToolsPack.Logging -o ./
dotnet pack src/ToolsPack.Logging.Otlp -o ./
