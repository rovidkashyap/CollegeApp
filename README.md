This is a CollegeApp Web API project Created using
.Net Core 7.0
MS SQL Server
Web API
Repository Pattern
DTO Models
TOOLS -> Visual Studio 2022 Community Edition
Microsoft SQL Server

1. You just need to copy the code and go to the appsettings.json file, changing the connection string as per your SQL server connection string.
2. Apply the migration by running the command in Package Manager Console (Tools -> NuGet Package Manager -> Package Manager Console -> type the command "Update-Database") to seed the records.
3. Build the solution and run the application.
4. All the APIs are displayed using the swaggerUI.
5. If you are unable to access the swagger page not found to display simply type the URL (https://yourwebsiteurl/localhosturl:portnumber/swagger/index.html)
