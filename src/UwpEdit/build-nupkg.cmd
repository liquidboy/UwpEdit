WHERE /Q msbuild >NUL
IF %ERRORLEVEL% NEQ 0 ( 
    ECHO Error: It appears that 'msbuild' is not available in this environment. Try using the VS command prompt.
)

msbuild "%~dp0UwpEdit.csproj" /v:m /maxcpucount /nr:false  /p:RunCodeAnalysis=false /p:Configuration=Release /p:Platform=ARM
msbuild "%~dp0UwpEdit.csproj" /v:m /maxcpucount /nr:false  /p:RunCodeAnalysis=false /p:Configuration=Release /p:Platform=x86
msbuild "%~dp0UwpEdit.csproj" /v:m /maxcpucount /nr:false  /p:RunCodeAnalysis=false /p:Configuration=Release /p:Platform=x64


powershell -command "Start-BitsTransfer -Source 'http://dist.nuget.org/win-x86-commandline/latest/nuget.exe' -Destination '%~dp0build\nuget.exe'"

%~dp0build\nuget pack %~dp0UwpEdit.nuspec -OutputDirectory %~dp0build -BasePath %~dp0build\nuget