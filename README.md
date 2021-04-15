# allocation-proration
---

## Prerequisites

- The .NET 5.0 SDK is required to build and run the application locally.
[Download the.Net 5.0 SDK here](https://dotnet.microsoft.com/download)
- Clone the repository 
 	-- [https://github.com/jcd03/allocation-proration](https://github.com/jcd03/allocation-proration)

## Running the application.
The root of the project contains a launch.bat file. Double click or run via command line. It will
open your browser to 'https://localhost:2000'. It will also build and launch the application.
* If port 2000 in unavailable, open up the launch.bat and change the port number.

### Overview of the application.
The application is a MVC Web project built in .NET 5.0. The front end uses Razor syntax to display the page and it is styled with bootstrap. The backend algorithm is written in C#. The overall application is in the standard MVC folder structure with the business logic separated into the 'Services' folder.

### In-Depth Decision Explanations.
##### Note: There are inline comments in the app to explain some decision making.
1. Front-End:  
 MVC Razor was chosen since this is a simple one page application. If this were a complex application I may have chosen a SPA like Angular.
 
2. Back-End:  
	The logic for the algorithm is separated into the ProrationService.cs class. This keeps the business logic separate from the responsibility of the controller. It iterates through the list to calculate the proration amount. Then does a check if the prorated amount is larger than the requested amount. If that item exists, it is moved to a temporary list and the totals are re-calculated.
	
3. ProrationServices using Dependency Injection:   
	Initially the ProrationService class was instantiated on the HomeController when needed. I moved it into the startup class to use Dependency Injection. For an application this small it wasn't necessary, but if this were a production application with future expansion I would use this design pattern too keep the classes loosely coupled and allow for expansion.
	
4. decimal? property:  
	The nullable decimal allowed for the UI to show the 'placeholder' attribute for `<input/>`.