# C# Test for candidates


## Technical Requisites

The usage of the following are demanded
- Language C# Latest Version
- .Net Core Latest Version
- Auth0 Authentication and Authorization 
- Entity Framework
- Sql Server
- Docker


## Main Tasks 

> 1. Create the following non-authenticated service endpoints
>  a. `/signin` - *POST* - receiving an user name and a password
>  b. `/signup` - *POST* - receiving an user full display name, an user name, a password and an e-mail address. Upon save time, add the current date and time to the database. An unique id must be created and used throughout the `/order/` POST endpoint

> 2. Create the following authenticate service endpoints
>  a. `/users` - *GET* - return all inserted users considering the following search and filter parameters:
>    * search and filter by user name 
>    * search and filter by all or parts of the full display name
>    * search and filter by an interval of creation dates
>    * search and filter by e-mail address
>    * all search criterias can be infinitely combined
>    * all singular textual search criterias need to support either ascending and descending sorting

>  b. `/products` - *GET* - return all inserted customers considering the same search requisites from previous task
>  c. `/product/` - *POST* - insert a new product to the product table with the following fields: id, name, description, price, creation date
>  d. `/product/` - *PUT* - update all passed fields in its appropriate record
>  e. `/order/` - *POST* - inserts an order receiving an user id and a list of products id with the current price and quantity
>  f. `/orders/ - *GET* - returns all orders from the database. In here you are free to determine what and how your endpoint should return the data.


## Services Requisites
- Run all services in a single docker container
- All endpoints must have automated tests that will prove the requisites are implemented
- Use as much design patterns and best practices as you see fit


## Database Requisites
- You are the responsible the create the database structure as best as you can think of. Conciseness, coherence and best practices are going to be considered
- Microsoft Sql Server must be running in a separate container image from the services one


## Deployment Requisites
- Deployment is key to the sucess of this test
- Make it as easy as possible
- The ideal scenario will be to clone your repository and execute it through a single command such as `./INSTALL` or `./RUN`
- Considering this is a C# test, the deployment can be supported only at Windows SO running machines or virtual machines but it will be considered a plus if you can manage to make it work at linux and/or mac osx OS's as well


## Documentation Requisites
- It can be done in portuguese although being in english, french or spanish will also be considered a plus
- Should be easy to read and understand the usage (from a client developer's perspective) of your services
- Must include at least one example of `curl` or `wget` usage of all your endpoints


## Test Development Requisites
- Use your github to deliver this test
- You can fork from here to get started
- Try to keep your commits to a reasonable atomic capacity
- Use as much best practices you see fit to address the commits and/or branch naming 

Feel free to ask me (**Marco Santana** - *marco@portaltelemedicina.com.br*) any question.

You can keep me up to date with your test progress. Send me an e-mail as soon as you feel like start sharing your solution, even before being ready. 




Thanks for giving us this opportunity to get to know you and your work. 




