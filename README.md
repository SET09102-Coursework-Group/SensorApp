# SET09102-coursework

This project was created for the SET09102 Software Engineering 3 Coursework. It focuses around the idea of an environmental agency, setting up sensors around Scotland to monitor the environment. 
Users can view sensors on a map, perform admin CRUD operations on users, analyze sensor measurements and create incident reports for sensor malfunctions or anomalies.

## Project setup:

1. Clone project locally
2. Create a local SQLite database in the location C:/sensorlistdb/sensorlist.db
3. Createa a new migration in the Package Manager Console with the command `Add-Migration InitialCreate` and then run the command `Update-Database` to create the initial database tables.
4. Run the SensorApp.Api project to populate initial Measurement data.
5. Download [ngrok](https://ngrok.com/) to host the API endpoints.
6. From a command prompt window, run the command `ngrok http 5158` to start listening for requests on port 5158.
7. Copy the URL provided by ngrok and add it to an `appsettings.Development.json` file in the SensorApp.Maui project, as the value of the `BaseAddress` property.
8. The SensorApp.Api project also requires a connection string to the SQLite database. This can be added to the `appsettings.Development.json` file in the SensorApp.Api project, as the value of the `ConnectionString` property. The connection string should be in the format `Data Source=C:/sensorlistdb/sensorlist.db;`.
9. This file also needs a `JwtSettings` section - contact a member of the development team for the required JWT secret key.
10. From the solution, navigate to the folder `SensorApp.Api` and run the command `dotnet run --launch-profile http` to start the APIs.
11. Start the project from the SensorApp.Maui folder and login using seeded credentials to use the application.

## Tests:

Tests are located in the SensorApp.Tests folder. 
These can be run by running the command `dotnet test` from the SensorApp.Tests root folder.