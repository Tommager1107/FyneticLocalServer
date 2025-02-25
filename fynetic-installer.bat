@echo off
setlocal enabledelayedexpansion

:: Define variables
set "installDir=C:\fce"
set "zipUrl=https://github.com/Tommager1107/cdn-fynetic/raw/refs/heads/main/local-server/fce.zip"
set "zipFile=C:\fce.zip"

:: Create the installation directory C:\fce
echo Creating directory %installDir%...
mkdir "%installDir%"

:: Download fce.zip
echo Downloading fce.zip from GitHub...
powershell -Command "Invoke-WebRequest -Uri '%zipUrl%' -OutFile '%zipFile%'"

:: Check if the ZIP file was downloaded successfully
if not exist "%zipFile%" (
    echo Error: Failed to download fce.zip. Exiting.
    exit /b 1
)

:: Extract the contents of the ZIP file
echo Extracting files from fce.zip...
powershell -Command "Expand-Archive -Path '%zipFile%' -DestinationPath '%installDir%'"

:: Check if the extraction was successful
if not exist "%installDir%\fce.exe" (
    echo Error: Failed to extract files. Exiting.
    exit /b 1
)

:: Remove the downloaded ZIP file
echo Deleting the downloaded zip file...
del "%zipFile%"

:: Add C:\fce to the PATH environment variable
echo Adding C:\fce to the PATH environment variable...
setx PATH "%PATH%;C:\fce"

:: Confirm installation
echo Installation completed successfully.
echo You can now run the server by typing "fce server" in the command prompt.

pause

