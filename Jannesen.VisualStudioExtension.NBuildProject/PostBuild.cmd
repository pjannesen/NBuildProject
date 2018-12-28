
set target=%~dp0..
set source=%~dp0bin\Jannesen.VisualStudioExtension.NBuildProject.vsix

if not exist "%target%" mkdir "%target%"
copy "%source%" "%target%\NBuildProject.vsix"