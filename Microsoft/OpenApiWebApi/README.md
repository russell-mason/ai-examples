# Getting Started

The following assumes you're using Visual Studio, so adjust accordingly for other IDEs.

**N.B.** This is not intended to be a serious API, it's only intended to demonstate using an OpenAPI schema,
and calling the associated API, from within the context of Microsoft AI technologies.

There are only three APIS:

### GetUserExists
```http
GET /users/{UserName}/exists  

e.g. /users/jane_jones/exists
```

### GetUser

```http
GET /users/{UserName}  

e.g. /users/bob_smith
```

### CreateUser

```http
POST /users  
{
    "userName": "",
    "password": "",
    "email": "",
    "firstName": "",
    "lastName": ""
}
```
```http
e.g. /users
```
```json
{
    "userName": "mike_hall_001",
    "password": "A1234567890",
    "email": "mike_hallh@my-company.com",
    "firstName": "Mike",
    "lastName": "Hall"
}
```

You can find the OpenAPI spec at:

```http
/openapi/v1.json
```

and documentation at:

```http
/scalar/v1
```

If you run the project through Visual Studio, you can use the default base URL:

```http
https://localhost:7152/

e.g. https://localhost:7152/openapi/v1.json
     https://localhost:7152/scalar/v1
     https://localhost:7152/users/bob_smith_001
     etc.
```

Configuration for this is located in the **AIExamples.Shared** project, in the **appsettings.json** file.

See the **README.md** file in **AIExamples.Shared** for more details.

### User maintenance

- The list of users will be cleared on the next POST after 24 hours has elappsed
- A maximum of 50 records will be kept and the oldest evicted on the next POST when this is exceeded
- This is just so that if you publish this publically you won't run out of memory if the API is accessed fequently

