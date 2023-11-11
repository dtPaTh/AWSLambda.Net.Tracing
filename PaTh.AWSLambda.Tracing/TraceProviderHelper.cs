using Dynatrace.OpenTelemetry;
using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Instrumentation.AWSLambda;

using Dynatrace.OpenTelemetry.Instrumentation.AwsLambda;
using System.Diagnostics;

namespace PaTh.AWSLambda.Tracing
{

    public static class TraceProviderHelper
    {

        public static TracerProvider? BuildTraceProvider()
        {
            return Sdk.CreateTracerProviderBuilder()
                    // Configures to send traces to Dynatrace, automatically reading configuration from environment variables.
                    .AddDynatrace()
                    // Adds injection of Dynatrace-specific context information in certain SDK calls (e.g. Lambda Invoke).
                    // Can be omitted if there are no outgoing calls to other Lambdas via the AWS Lambda SDK.
                    .AddDynatraceAwsSdkInjection()
                    .AddAWSLambdaConfigurations(c =>
                    {
                        c.DisableAwsXRayContextExtraction = true;
                        c.SetParentFromBatch = true;
                    })
                    // Instrumentation used for tracing outgoing calls to AWS services via AWS SDK (including Amazon DynamoDB, SQS/SNS).
                    // Can be omitted if no outgoing AWS SDK calls expected.
                    .AddAWSInstrumentation(c => c.SuppressDownstreamInstrumentation = true)
                    // Avoid unexpected outgoing HTTP requests
                    .AddHttpClientInstrumentation(op =>
                    {
                        op.Filter = req => Activity.Current?.Parent?.IsAllDataRequested ?? false;
                    })
                    .Build();
        }

    }
}