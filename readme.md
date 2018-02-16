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

1. Create the following non-authenticated service endpoints
  a. `/signin` - *POST* - receiving an user name and a password
  b. `/signup` - *POST* - receiving an user full display name, an user name, a password and an e-mail address. Upon save time, add the current date and time to the database. An unique id must be created and used throughout the `/order/` POST endpoint

2. Create the following authenticate service endpoints
  a. `/users` - *GET* - return all inserted users considering the following search and filter parameters:
    * search and filter by user name 
    * search and filter by all or parts of the full display name
    * search and filter by an interval of creation dates
    * search and filter by e-mail address
    * all search criterias can be infinitely combined
    * all singular textual search criterias need to support either ascending and descending sorting

  b. `/products` - *GET* - return all inserted customers considering the same search requisites from previous task
  c. `/product/` - *POST* - insert a new product to the product table with the following fields: id, name, description, price, creation date
  d. `/product/` - *PUT* - update all passed fields in its appropriate record
  e. `/order/` - *POST* - inserts an order receiving an user id and a list of products id with the current price and quantity
  f. `/orders/ - *GET* - returns all orders from the database. In here you are free to determine what and how your endpoint should return the data.

    
