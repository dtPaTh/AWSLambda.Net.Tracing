# Trace .NET AWS Lambda Functions with Dynatrace

This projects uses [Metalama](https://www.postsharp.net/metalama) to automatically weave distributed tracing to AWS Lambda functions utilizing the [Dynarace AWS Lambda Instrumentation Package](https://docs.dynatrace.com/docs/setup-and-configuration/setup-on-cloud-platforms/amazon-web-services/amazon-web-services-integrations/aws-lambda-integration/aws-lambda-otel-integration). 

## Features
The aspect weaves distributed tracing into async as well as syncronous event handlers and supports tracecontext propagation for: 
* APIGatewayProxyRequest 
* APIGatewayHttpApiV2ProxyRequest
* SQSEvent 
* SNSEvent 

The aspect configures the OpenTelemetry trace-provider as described [here](https://docs.dynatrace.com/docs/setup-and-configuration/setup-on-cloud-platforms/amazon-web-services/amazon-web-services-integrations/aws-lambda-integration/aws-lambda-otel-integration#initialization) and [here](https://docs.dynatrace.com/docs/setup-and-configuration/setup-on-cloud-platforms/amazon-web-services/amazon-web-services-integrations/aws-lambda-integration/aws-lambda-otel-integration#special-considerations-for-httpclient-instrumentation) to send the telemetry to Dynatrace and enabling built-in instrumentation for: 
* AWS SDK Instrumentation
* and HTTPClient

## ChangeLog
* v1.0.0 Initial release

## Contribute
This is an open source project, and we gladly accept new contributions and contributors.  

## Support
This project is not an offical release of Dynatrace. If you have questions or any problems, open a github issue.  

## License
Licensed under Apache 2.0 license. See [LICENSE](LICENSE) for details.
