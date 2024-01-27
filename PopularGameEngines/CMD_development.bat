@echo off

echo ----------------
echo 1 - call dotnet watch run
echo 2 - [ reset database / migrations ]
echo 3 - call dotnet test
echo 10 - call git reset --soft HEAD~1
echo ----------------

set /P input="ENTER: "

if %input% == 1 (
	call dotnet watch run
)

if %input% == 2 (
    call mysql -u johnsmith -pSecret!123 -e "DROP SCHEMA IF EXISTS gameengines;"

    call dotnet ef migrations add InitialMySQL

    call dotnet ef migrations add Identity

    call dotnet ef migrations remove

    call dotnet ef database update
)

if %input% == 3 (
    call dotnet test
)

if %input% == 10 (
	call call git reset --soft HEAD~1
)

echo ----------------

echo FINISHED

pause
