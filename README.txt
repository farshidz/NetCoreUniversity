Net Core University

The Net Core University sample web application demonstrates various concepts, 
techiques and technologies. The application is developed using ASP.NET Core 
and Entity Framework Core. 

Net Core University is a fictional university. This application's purpose is to
keep track of the university's students, classes and teachers.

- Current Version -
The application consists of mainly the back-end components, including a data 
layer that uses EF Core, as well ASP.NET Core MVC APIs that will provide
an interface to the front-end. The design of this application follows SOLID 
principles closely. For example, all classes receive their dependencies 
explicitely in order to facilitate testing and allow dependency injection to be
used.

The code conists of the following C# projects:

-- Data --
This is the DAL. Notable features: 

	* Unit of Work and Repository design patterns are used to decouple the rest 
	of the program from the ORM.
	* The current version of EF Core does not support many-to-many relationships
	directly. A common workaround is to have an entity that corresponds to the 
	join table. The code is written so that higher layers have no knowledge of
	this limitation and the workaround.
	* The current version of EF Core does not support lazy-loading. The 
	repository classes provide methods to perform explicit loading instead.
	* Data Annotations are used to help EF Core code-first approach define the
	database tables, as well as to allow validation of data where necessary,
	e.g., in APIs. Where database-specific constraints are necessary, Fluent API
	has been used in place of Data Annotations, in order to keep the entities 
	independent from any specific persistence mechanism.
	
-- WebApi --
ASP.NET Core MVC APIs (JSON) that providing basic operations such as adding a 
new class/student/teacher, listing all classes etc. ASP.NET Core's native 
dependency injection capabilities are demonstrated here.

-- WebApi.UnitTests --
Unit tests for WebApi project. For this version, one controller 
(ClassController) was developed using TDD. This is a simple, easy-to-understand
demonstration of how dependency inversion principle (DIP) faciliates unit 
testing. 
Since the Repository pattern has been used in the data layer, a mocking
framework can isolate the controllers from the data layer very easily. Here I 
have used Moq.

-- WebApi.IntegrationTests --
Integration tests for WebApi project. For this version, integration tests have
been written for ClassController.
Perhaps the most notable feature of this project is the use EF Core's in-memory
database. Through dependency injection, the data layer receives an instance
of DbContext class that uses a fast in-memory database instead of a physical 
database with slow I/O operations. This means integration tests can
run almost as fast as unit tests. At the same time, each test receives a fresh
database that is not modified by other tests.

- Future improvements -
	* Create an Angular user interface
	* Use proxies to implement lazy-loading (Castle DynamicProxy?)
	* Create unit and integration tests for other controllers
	* Create unit tests for data layer classes