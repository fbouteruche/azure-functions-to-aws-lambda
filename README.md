# Azure Functions to AWS Lambda

This repository contains code samples to explain how to migrate .NET code from Azure Functions to AWS Lambda.

It contains three subfolders:
* [AzureFunctionsApi](./AzureFunctionsApi) contains the base solution with the Azure Function source code to migrate
* [AWSLambdaAspNetCoreApi](./AWSLambdaAspNetCoreApi) contains a migrated solution based on running an ASP.NET Core Api on AWS Lambda
* [AwsLambdaNetCoreApi](./AwsLambdaNetCoreApi) contains a migrated solution based on running raw Lambda Functions in .NET

## Azure Functions Api

The AzureFunctionsApi is a basic Azure Functions .NET solution. It defines three Azure Functions. 

The main one is named **GetClientList** and is defined
in the *ClientController.cs* file located in the Controllers folder. It uses a **ClientService** object to retrieve the a list of client through the call of 
the **GetClientsList** method. This method takes one parameter called name. The value of this parameter is retrieved 
through the *QueryString* parameter called *name* accessible through the **HttpRequest** object passed as a parameter of the Azure Function handler method. The method *GetClientsList* of the *ClientService* object returns an enumeration of *ClientDto* objects. This enumeration is returned to the caller through 
an **OkObjectResult** object. The Azure Functions runtime serializes this enumeration as a json object and returns it in the body of the HTTP response. 

The two other Azure Functions define endpoints to expose the Swagger json documentation of the Api and the Swagger UI. The nuget package 
***AzureExtensions.Swashbuckle*** is used to handle these two Azure Functions and to automatically generate the Swagger documentation from the source code 
and attributes like **QueryStringParameterAttribute** and **ProducesResponseTypeAttribute**.


## AWS Lambda ASP.NET Core Api

While Azure Functions manages both HTTP request handling and processing, API Gateway and AWS Lambda separate both concerns. API Gateway allows you to define your 
HTTP endpoints by defining your HTTP API resources and methods. It then routes the request to a backend service for processing, in this case, AWS Lambda. 
AWS Lambda is an event-based compute service. when a configured event occures, an AWS Lambda function is triggered to process the event. Here, the event is 
the invocation via API Gateway. API Gateway passes all the information about the HTTP request through the event (Headers, Body...).

The AWSLambdaASPNetCoreApi solution implements an AWS Serverless application composed of:
* an API Gateway handling HTTP request and invoking the AWS Lambda function
* an AWS Lambda function processing the HTTP request

The API Gateway is implicitly defined through the *serverless.template* file. This file is a AWS Serverless Application Model template file. To learn more about
the AWS Serverless Application Model, read [here](https://docs.aws.amazon.com/serverless-application-model/latest/developerguide/what-is-sam.html).

The AWSLambdaASPNetCoreApi is a standard ASP.NET Core API solution. You can run the API through IIS Express or kestrel for local
testing purpose. The solution leverages the ***Swashbuckle.AspNetCore*** Nuget package to automatically generate the Swagger json documentation endpoint and
the Swagger UI endpoint.

To integrate this ASP.NET Core Api into an AWS Lambda function, we integrate API Gateway with AWS Lambda through a generic proxy resource denoted *{proxy+}*.
For example, if you define a generic proxy resource under the path */api*, a request to the path */api/clients/list* will trigger the invocation of the associated 
AWS Lambda function. This integration is definied lines 21 to 26 of the *serverless.template* file.

To integrate AWS Lambda with the ASP.NET Core middleware, we use the ***Amazon.Lambda.AspNetCoreServer*** Nuget package  and we define 
the class **LambdaEntryPoint** which inherits from the class **Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction**. This base class handles the heavy 
lifting of integrating the received event from API Gateway into the ASP.NET Core middleware. To complete the integration, we define the 
**AwsLambdaAspNetCoreApi::AwsLambdaAspNetCoreApi.LambdaEntryPoint::FunctionHandlerAsync** method as the function handler at line 11 of the *serverless.template* file.

You can use the [AWS Toolkit for Visual Studio](https://docs.aws.amazon.com/en_us/toolkit-for-visual-studio/latest/user-guide/welcome.html) 
to quickly deploy this solution into your AWS developer account and test it. As we use the ***Swashbuckle.AspNetCore*** Nuget package to automaticaly 
generate the Swagger json documentation and the Swagger UI endpoints, you can browse the *https://<your_api_gateway_endpoint>/swagger* URL 
to access the Swagger UI.


## AWS Lambda .NET Core Api

While the previous solution is a very convenient way to build and deploy ASP.NET Core API on AWS Lambda, sometimes, you may need faster startup time of
your application to reduce the overall cold start of your Lambda function.

In this case, you may want to avoid the computational overhead of using the dependency injection mechanism of the ASP.NET Core middleware and rely explicity
instanciated objects. 

The AWSLambdaNetCore Api implements such a solution. It relies only on the **Amazon.Lambda.Core** and **Amazon.Lambda.APIGatewayEvents** Nuget packages . 
In the *serverless.template* file, you  define explicity for each path of your API which method to use as the handler like in lines 9 and 22.

With this solution, you cannot use any **Swashbuckle** package to automatically generate the Swagger json documentation.