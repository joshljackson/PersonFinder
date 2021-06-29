# Sample Person Finder application
Set up for this project is pretty simple:
* Clone the repository
* [Set up the database connection](#Setting-up-and-connecting-to-the-database)
* [Run the application](#running-the-api)
* [Make API calls!](#make-api-calls)

# Setting up and connecting to the database

This application is built to use SQL Server, which can be hosted somewhere or running on your machine. The provided script **PersonDatabaseSetup.sql** can be used to create the Person database, a Person table, and to populate the Person table with some initial records (using Star Wars characters of course). 

Once you have the database set up, grab your connection string and put it in *PersonDatabaseConnection* section of the file **src/PersonFinder.WebApi/appsettings.json**, and then you'll be ready to run the app. Make sure that the Initial Catalog is set to Person in the connection string.


# Running the API

The Web Api can be run on the command line by navigating to **src/PersonFinder.WebApi** and running `dotnet run` on the command line. This will build and start the api and run it at *http://localhost:5000* by default.

You can also run it by opening **PersonFinder.sln** in Visual Studio or Visual Studio Code, setting the **PersonFinder.WebApi** project as the startup project and then run with the debugger.

# Make API calls

With the Api running, can make simple http calls to any of the three endpoints using any tool allowing you to make http requests ([Postman](https://www.postman.com/), [Insomnia](https://insomnia.rest/), [VS Code](https://marketplace.visualstudio.com/items?itemName=humao.rest-client), or from the command line, etc). Here are sample http requests for each endpoint to get you started (these can also be found in the included file *PersonFinderApiRequstSamples.http*).

## Get a person by Id

```
GET http://localhost:5000/api/person/3
```
returns...
```
{
  "id": 3,
  "name": "Zaalbar",
  "age": 35,
  "address": "Kaashyyk",
  "interests": "Crossbow"
}
```


## Search for a person by name

This endpoint takes a `PersonSearchQuery` object and searches for a person who's name contains the string in `SearchString` property. An exception is thrown if the `PersonSearchQuery` object is null.
```
POST http://localhost:5000/api/person/search
Content-Type: application/json

{"SearchString":"m"}
```
returns...
```
[
  {
    "id": 4,
    "name": "Mission Vao",
    "age": 20,
    "address": "Taris",
    "interests": null
  }
]
```


## Save a new person record
This endpoint takes a `Person` object (with no Id) and saves it to the database. An exception will be thrown if the Name is null or blank.
```
POST http://localhost:5000/api/person
Content-Type: application/json

{"Name":"Darth Revan", "Age":25, "Address":"Korriban", "Interests": "Domination"}
```
