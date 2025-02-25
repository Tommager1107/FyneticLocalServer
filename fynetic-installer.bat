@echo off
setlocal enabledelayedexpansion


set "installDir=C:\fce"
set "zipUrl=https://github.com/Tommager1107/cdn-fynetic/raw/refs/heads/main/local-server/fce.zip"
set "zipFile=C:\fce.zip"


echo Creating directory %installDir%...
mkdir "%installDir%"


echo Downloading fce.zip from GitHub...
powershell -Command "Invoke-WebRequest -Uri '%zipUrl%' -OutFile '%zipFile%'"

if not exist "%zipFile%" (
    echo Error: Failed to download fce.zip. Exiting.
    exit /b 1
)

echo Extracting files from fce.zip...
powershell -Command "Expand-Archive -Path '%zipFile%' -DestinationPath '%installDir%'"

if not exist "%installDir%\fce.exe" (
    echo Error: Failed to extract files. Exiting.
    exit /b 1
)

echo Deleting the downloaded zip file...
del "%zipFile%"

echo Adding C:\fce to the PATH environment variable...
setx PATH "%PATH%;C:\fce"

echo Installation completed successfully.
echo You can now run the server by typing "fce server" in the command prompt.

pause

