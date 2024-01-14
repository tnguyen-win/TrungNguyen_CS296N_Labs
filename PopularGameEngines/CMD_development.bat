@echo off

echo ----------------
echo 1 - call dotnet watch run
echo 10 - call git reset --soft HEAD~1
echo ----------------

set /P input="ENTER: "

if %input% == 1 (
	call dotnet watch run
)

if %input% == 10 (
	call call git reset --soft HEAD~1
)

echo ----------------

echo FINISHED

pause
