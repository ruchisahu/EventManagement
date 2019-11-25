# EventManagement
my attempt at a test problem 

## Features Implemented:

### EventAuth: Web applicationInclude basic login Page , 

This is implemented as a separate project
 - Username:evadmin
 - Password: EventManagement

### This is a 3 layer implementation, consisting of  UI, Backend and a data tier. 
 - UI Layer, This is thin layer using MVC architecture for views.
 - Business Logic, Backed Layer interaction between User Interface and Data layer.This layer contains all the domain logic of verifying that event has to be in 3 day duration and does not overlap with any other event , more such rules can be implemented in this layer.
 - Data tier, the system provided data layer.

### How to run this application:

The main starting point of this project is EventAuth but since all the layers are different projects we have to start all of them in parallel. We do this by ensuring the following 
 - Click Solution->Properties->”Startup Project”
 - Select “Multiple Projects “
 - Ensure Start is selected for all the executable layers. 
 - You would then be presented with Auth layer , where you can  enter user/pass( The password is currently statically coded as hash in the app settings.)
 - You would then be presented with  the Event Index , You can create modify events in this page. 

### Other details and considerations 
 - The Authentication mechanism can be made more robust by integrating it will OpenAuth framework using Identity server, but currently this is beyond the scope of this project 
 - We can enable docker containerization easily for the project converting it to micro services. 
 - We should ideally add unit-tests for each of the layers and should ideally also have a logging framework , but In the interest of time I have ignored those.
 - Finally, as of now I am working on some other changes that should make this code more modular , and object oriented. If all goes well I should integrate these in the next few hours. 
   - Update : Implemented an httpClient project to simplify calls to web apis. this client is currently integrated in domainController(middleLayer) but i have not yet added it to the UI layer.
 - The changes are mainly extracting out the repeated code to independent, reusable components for better readability , test-ability and ease of development. 


