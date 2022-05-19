@ECHO OFF
cd Carma2SaveEditor
dotnet publish -c Release -r win-x86 /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true --output "bin\Release\net6.0-windows"
pause