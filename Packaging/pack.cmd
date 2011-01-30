@echo off
if "%1"=="" GOTO :USAGE
if "%1"=="all" GOTO :ALL
GOTO :INVOKE

:ENSUREOUTPUTPATHEXISTS
if NOT EXIST Output md Output
GOTO :EOF

:INVOKE
..\tools\NuGet\nuget.exe pack %1 -o "Output"
GOTO :EOF

:ALL
CALL :ENSUREOUTPUTPATHEXISTS
for /f %%F in ('dir /b *.nuspec') DO CALL :INVOKE %%F
GOTO :EOF

:USAGE
echo Usage:
echo To create a single package:
echo 		pack nuspec-filename
echo ...
echo To create all packages:
echo pack all
GOTO :EOF

