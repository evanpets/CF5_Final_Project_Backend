# Final Project Backend
This backend server was developed by Evangelos Petsalis in C# using an ASP.NET Web API with the aim of serving as the backend of the final project of Coding Factory 5.

## How to run - Instructions
The server runs on `https://localhost:5001/`and `http://localhost:5000`. To run it (on Visual Studio) click on the green "HTTPS" icon in the toolbar or
press F5. On run, Swagger will also load.

Two SQL scripts are included within the folder provided. One creates the database and user, and the other inserts data. After running the script to create the database,
you will need to go to `View -> Other Windows -> Package Manager Console` and in the terminal that opens, run `Update-Database`.
(A migration folder and file should already exist within the application. If not, in the Package Manager Console terminal, run `Add-Migration <name of your choice>` before you run `Update-Database`).
This will create the tables of the database and will allow you to properly insert the values provided in the second script to the database.

## Brief description of the layout

## Data (Entities)
The approach adopted while developing this server was Model-First, so the entities and their relationships have been added to a DbContext class which is used to migrate the data to the chosen database.

The main entities are Events. They use other entitiies as properties, namely Performers and Venues (which in turn use a Venue Address entity as an property).
Users also exist in the data. Finallly, another entity found is an Event Save.
- Event: Refers to cultural events of various sorts.
- Venue: The area where an event is taking place.
- Venue address: The fields through which a venue can be located.
- Performer: An artist participating in an event. (Only name is used as a property, but I opted to make Performer into a class for future scalability purposes).
- User: A user of the application.

## Models
Contain useful classes that 
- Application User: Contains information relating to the active application user.
- Error: Contains information about an error to help with logging.
- Event Category: An enum that defines the category an event is listed under (Music, Cinema, Theater).
- User Role: An enum that defines the role a user has in the application (User, Admin).

## Controllers
Receive HTTP requests from the frontend and send them to the backend service. 
- Base Controller: Mainly sets the active user of the application.
- Event Controller: Handles requests related to events and their children entities.
- User Controller: Handles requests related to users.
- Admin Controller: Handles certain requests which can only be authorized for the 'Admin' user role.

## Service
Receive requests from the controllers and communicates with the repository layer to provide the necessary data to the controllers.
- Event Service: Handles operations related to events.
- User Service: Handles operations related to users.
- Venue Service: Handles operations related to venues and venue addresses.
- Performer Service: Handles operations related to performers.
- Application Service: Instances of the above services are injected here so that access to their methods is centrally administrated and easily managed.

## Repositories
Communicate with the database to perform CRUD methods and provide the related data to service methods.
- Event Repository: Handles CRUD methods related to events.
- User Repository: Handles CRUD methods related to users.
- Venue Repository: Handles CRUD methods related to venues and venue addresses.
- Performer Repository: Handles CRUD methods related to performers.
- Unit Of Work: Instances of the above repositories are injected here so that access to their methods is centrally administrated and easily managed.

## DTO
- Represents the data packages passed from the frontend to the backend for various requests.
- Event DTOs: Relays data related to events.
- User DTOs: Relays data related to users.
- Venue DTOs: Relays data related to venues and venue addresses.
- Performer DTOs: Relays data related to performers.
- EventSave DTO: Relays data related to the addition of a save status for an event by a certain user.
- JWTToken DTO: Relays the JWT required to authorize a user to access their account after their authentication.

## Configuration
Auto-mapping is found in a class here, which maps data from the DTOs to their respective data entities, and vice versa.

## Security
Contains the Encryption utility class, which helps encrypt password and validate them on request.

## Helpers
Contains the Authorize Operation Filter and the Error Handler Middleware. The former helps with propeer documentation and responses for unauthorized requests, while the latter helps with central handling of exceptions to fitting HTTP codes.

## Uploads
A folder used as local storage for images uploaded to the server.
