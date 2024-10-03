set "SourcePath=%~dp0..\src\EndlessEscapade"
set "TargetPath=%USERPROFILE%\Documents\My Games\Terraria\tModLoader\ModSources\EndlessEscapade"

if not exist "%SourcePath%" (
    echo "ERROR: Source directory does not exist: %SourcePath%"
    pause
    exit /b 1
)

mklink /D "%TargetPath%" "%SourcePath%"

if %ERRORLEVEL% equ 1 (
    echo Failed to create symbolic link.
    pause
) 
