# MockApi

MockApi is a simple web api mocking tool that allows the mocking of http web apis

# Running MockApi

You can build and run mockapi locally, or use it as a docker container. 

e.g.
``` Command Line
cd mockapi
docker build -t mockapi .
docker run --rm -d -p:5678:80 mockapi
```

# Setting up a response

To setup a response for a given path, post a request to the path with a body containing the required response. The POST request must have the following headers

```
MockApi-Action: Setup
MockApi-Method: GET|POST|PATCH|DELETE|HEAD|...
```

For example the following HTTP Request will setup the response for GET /myapi/myresource/1

```HTTP
POST /myapi/myresource/1 HTTP/1.1
Host: localhost:5678
MockApi-Action: Setup
MockApi-Method: GET
Cache-Control: no-cache

my-custom-response
```

Once set up calls to that path will respond with the designated payload

# Checking information about a call

You can retrieve information about the calls made to an endpoint you have setup by making a GET request to the path with the following http headers

```
MockApi-Action: Validate
MockApi-Method: GET|POST|PATCH|DELETE|HEAD|...
```

For example the following HTTP request will retieve the call details for the response setup in the step above

```HTTP
GET /myapi/myresource/1 HTTP/1.1
Host: localhost:5678
MockApi-Action: Validate
MockApi-Method: GET
```

The data will be returned as a block of JSON

e.g.

```JSON
{
    "count": 2,
    "requests": [
        {
            "path": "/myapi/myresource/1",
            "body": ""
        },
        {
            "path": "/myapi/myresource/1",
            "body": ""
        }
    ]
}
```

# Using placeholders 

An endpoint can use placeholders to match a group of endpoints based on variable parameters. Place holders are identified by curly braces. For example setting up a response for the following URI

* /myapi/contacts/{id}

Will match requests

* /myapi/contacts/12
* /myapi/contacts/alpha

This allows a single setup to setup a response to a web api route

Routes are matched by specificity, with a more specific match being preferred to a less specific one. This means that is two mock end point are setup as follows

* /myapi/contacts/{id}
* /myapi/contacts/details

Then calls to

/myapi/contacts/12 will be served by the first setup, and calls to /myapi/contacts/details will be served by the more specific second setup

# Placeholder values in responses
A placeholder value can be returned in a response by including the same placeholder (including curly braces) in the setup payload

e.g. After setting up a call with

```HTTP
POST /myapi/contacts/{id} HTTP/1.1
Host: localhost:5678
MockApi-Action: Setup
MockApi-Method: GET
Cache-Control: no-cache

{
    "userid": "{id}"
}
```

A GET to /myapi/contacts/12 will return

``` JSON
{
    "userid": "12"
}
```

# Using a setup file to configure responses

A configuration file can be provided to automatically register routes and responses when the mock api server starts.
The application will look for a setup.json file in the config folder.

The setup file contains an array of setup objects, each object defines an endpoint to mock by specifying 
Path - the patch to mock, can contain placeholders
Method - the method to respond to (e.g. GET, POST, etc.)
Status - the status code to respond with
Response - the data to respond with, can be any valid json value

```JSON
[
    {
        "Path": "/path/to/mock",
        "Method": "GET",
        "Status": 200,
        "Response": {
            "Type": "Example",
            "Description": "The response can be any object or array that you want to return" 
        }
    }
]
```

# Using docker
The mock api is available as a docker container

Docker CLI

```docker run -p 4000:80 -v ./my/config:/app/config secbyte/mockapi:v2.3```

Docker Compose snippet
```
mock-api:
    image: secbyte/mockapi:v2.3
    volumes:
      - "./my/config:/app/config"
    ports:
      - "4000:80"
```