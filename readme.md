# Trace .NET AWS Lambda Functions with Dynatrace

## Distributed Tracing
[Dynatrace](https://www.dyntrace.com) offers an automatic instrumentation for AWS Lambda Python, Node.JS and Java via [Dynatrace AWS Lambda Layer]()

For functions using the .NET runtime, Dynatrace provides distributed tracing for these types based on [OpenTelemetry](https://opentelemetry.io/).

If you already use OpenTelemetry to instrument your functions you can ingest the telemetry either using the OpenTelemetry base functionality or using the Dynatrace (PurePath) exporter which adds additional benefits to fully leverage the automatic analysis capabilties of Dynatrace. 

To make using OpenTelemetry easier, Dynatrace provides an [enhanced library for AWS Lambda](https://docs.dynatrace.com/docs/setup-and-configuration/setup-on-cloud-platforms/amazon-web-services/amazon-web-services-integrations/aws-lambda-integration/aws-lambda-otel-integration) to reduce necessary OpenTelemetry boiler-plate code for trace-propagation, automatically applying resource attributes and initialization code as well to align with semantic conventions. 

## Eliminate the entry barrier
Whether you are new to OpenTelemetry or need to instrument thousands of functions or maybe just want to try out distributed tracing with minimal effort. The provided libary within this repository allows you to skip the need to fiddle around with OpenTelemetry or adding any additonal instrumentation code to your functions. 

### How-does it work?
The approach makes use of the [aspect oriented programming (AOP)](https://en.wikipedia.org/wiki/Aspect-oriented_programming) paradigm using [Metalama Framework](https://www.postsharp.net/metalama). 

#### What is Metalama?
Metalama is a modern Roslyn-based meta-programming framework from the creators of Postsharp, which helps to improve code quality and productivity in C#. Metalama comes with a license model that can be used for free, up to 3 aspect classes per project, which allows the use of this project for no additional charge as the projects includes 1 aspect class. For more details see [Metalama open source licensing](https://blog.postsharp.net/post/metalama-open-source-licensing.html)

## Features
The repository contains a ready-to-use nuget package, which includes an aspect to automatically 
instruments AWS Lambda handlers using the [Dynarace AWS Lambda Instrumentation Package](https://docs.dynatrace.com/docs/setup-and-configuration/setup-on-cloud-platforms/amazon-web-services/amazon-web-services-integrations/aws-lambda-integration/aws-lambda-otel-integration). 

The aspect weaves distributed tracing into async as well as syncronous event handlers and supports tracecontext propagation for: 
* APIGatewayProxyRequest 
* APIGatewayHttpApiV2ProxyRequest
* SQSEvent 
* SNSEvent 

The aspect configures the OpenTelemetry trace-provider as described [here](https://docs.dynatrace.com/docs/setup-and-configuration/setup-on-cloud-platforms/amazon-web-services/amazon-web-services-integrations/aws-lambda-integration/aws-lambda-otel-integration#initialization) and [here](https://docs.dynatrace.com/docs/setup-and-configuration/setup-on-cloud-platforms/amazon-web-services/amazon-web-services-integrations/aws-lambda-integration/aws-lambda-otel-integration#special-considerations-for-httpclient-instrumentation) to send the telemetry to Dynatrace and enabling built-in instrumentation for: 
* AWS SDK Instrumentation
* and HTTPClient

### How-To use

#### 1. Add the nuget package to your project e.g. via command line. 
[![NuGet](http://img.shields.io/nuget/v/Path.AWSLambda.Tracing.svg)](https://www.nuget.org/packages/Path.AWSLambda.Tracing/)

```
dotnet add package PaTh.AWSLambda.Tracing
``` 

#### 2. Apply Dynatrace configuration
The Dynatrace package automatically reads the necessary configuration such as connection endpoints and authentication tokens, either from environment variables or a custom config file. To read more about configuration see [Dynatrace Help](https://docs.dynatrace.com/docs/setup-and-configuration/setup-on-cloud-platforms/amazon-web-services/amazon-web-services-integrations/aws-lambda-integration/aws-lambda-otel-integration/aws-lambda-otel-setup)


## Examples / Testing / Debugging
In the [/Examples](/Examples/) folder you find various AWS Lambda sample functions, ready to test with e.g. [The AWS .NET Mock Lambda Test Tool](https://github.com/aws/aws-lambda-dotnet/blob/master/Tools/LambdaTestTool/README.md) 

### Running the examples
To run the examples, you have to apply the necessary connection parameters in dtconfig.json as described in previous **Step #2 - Apply Dynatrace configuration**

### Debugging transformed code
The provided Visual Studio solution is already prepared to debug the transformed code for the examples. For more details how to configure/debug your projects read [here](https://doc.metalama.net/conceptual/using/debugging-aspect-oriented-code). 

### Troubleshooting
To get extensive log output to troubleshoot potential configuration issues, add the environment variable:
```
DT_LOGGING_DESTINATION=stdout
```

## Contribute
This is an open source project, and we gladly accept new contributions and contributors.  

## Support
This project is not an offical release of Dynatrace. If you have questions or any problems, open a github issue.  

## License
Licensed under Apache 2.0 license. See [LICENSE](LICENSE) for details.
