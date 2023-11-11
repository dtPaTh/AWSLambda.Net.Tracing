using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace TestApiGatewayProxyInvoke;

public class Function
{

    /// <summary>
    /// Lambda function handler to respond to events coming from an Application Load Balancer.
    /// 
    /// Note: If "Multi value headers" is disabled on the ELB Target Group then use the Headers and QueryStringParameters properties 
    /// on the ApplicationLoadBalancerRequest and ApplicationLoadBalancerResponse objects. If "Multi value headers" is enabled then
    /// use MultiValueHeaders and MultiValueQueryStringParameters properties.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public APIGatewayHttpApiV2ProxyResponse FunctionHandler(APIGatewayHttpApiV2ProxyRequest evt, ILambdaContext ctx)
    {
        ctx.Logger.LogInformation($"Execute ");

        // TODO: Do interesting work based on the new message
        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = 200,
            Body = "Example function result",
        };
       
    }
}