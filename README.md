# Application description
APS.NET WebAPI application written with .NET 6

There is a single controller to get data from Stripe API (https://stripe.com/docs/api/balance_transactions/list)
There are two endpoints:
- Endpoint to get all data
- Endpoint to be used fro pagination (params: pageNo, itensPerPage)

It is covered by UnitTests written with NUnit, FluentAssertions and Moq

# How to run the app locally
To run the app locally, you need to open the solution with VS and run the app with the F5 key.
No special settings are required.

# Calls Examples

## Endpoint: GET /Balance

### Request Parameters
No params

### Response
As a response, you will get the status code and the list of data objects in JSON format if everything went well.
If any errors appeared during the execution, the response will return the appropriate status.

### Example
Requests:
```
GET /api/Balance
```

## Endpoint: GET /Balance/GetPage

### Request Parameters
- pageNo : the number of page to get (starts from 0)
- itemsPerPage : the number of items per page

### Response
As a response, you will get the status code and the list of data objects filtered by input params in JSON format if everything went well.
If any errors appeared during the execution, the response will return the appropriate status.

### Example
Requests:
```
GET /api/Balance/GetPage?
GET /api/Balance/GetPage?pageNo=2
GET /api/Balance/GetPage?pageNo=2&itemsPerPage=3
```
