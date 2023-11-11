using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using OpenTelemetry.Trace;
using System.Diagnostics;
using OpenTelemetry.Instrumentation.AWSLambda;
using Amazon.Lambda.Core;
using Dynatrace.OpenTelemetry;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.ApplicationLoadBalancerEvents;

namespace PaTh.AWSLambda.Tracing
{


    /// <summary>
    /// Supports context propagation from following trigger events
    /// - APIGatewayProxyRequest 
    /// - APIGatewayHttpApiV2ProxyRequest
    /// - SQSEvent 
    /// - SQSEvent.SQSMessage
    /// - SNSEvent 
    /// - SNSEvent.SNSRecord
    /// See also, https://github.com/open-telemetry/opentelemetry-dotnet-contrib/blob/4db5ac689e2a5e5cf761d3df65d049829358af21/src/OpenTelemetry.Instrumentation.AWSLambda/Implementation/AWSLambdaUtils.cs#L68
    /// </summary>
    public class TraceInvokationHandlerAttribute : MethodAspect
    {
        [Introduce(WhenExists = OverrideStrategy.Ignore)]
        private static TracerProvider? _tracerProvider = null;

        public override void BuildAspect(IAspectBuilder<IMethod> builder)
        {

            if (builder.Target.Parameters.Any(p => (p.Type.Is(typeof(APIGatewayProxyRequest)) || p.Type.Is(typeof(APIGatewayHttpApiV2ProxyRequest))|| p.Type.Is(typeof(ApplicationLoadBalancerRequest)))))
                builder.Advice.Override(builder.Target, nameof(EventResponseTemplate));
            else if (builder.Target.ReturnType.Is(typeof(Task)))
                builder.Advice.Override(builder.Target, nameof(AsyncEventResponseTemplate));
            else
                builder.Advice.Override(builder.Target, nameof(EventTemplate));

        }

        [Template]
        public async Task AsyncEventResponseTemplate()
        {
            async Task wrapper(object obj, ILambdaContext context)
            {
                await meta.Proceed();
            }

            if (_tracerProvider == null)
            {
                DynatraceSetup.InitializeLogging();
                _tracerProvider = TraceProviderHelper.BuildTraceProvider();
            }

            ILambdaLogger logger = meta.Target.Parameters.Where(p => p.Type.Is(typeof(ILambdaContext))).FirstOrDefault().Value.Logger;
            
            ActivityContext? traceCtx = Activity.Current?.Context;

            if (_tracerProvider != null)
            {
                Func<object, ILambdaContext, Task> lambdaHandler = wrapper;

                await AWSLambdaWrapper.TraceAsync(_tracerProvider, lambdaHandler, meta.Target.Parameters.FirstOrDefault().Value, meta.Target.Parameters.Where(p => p.Type.Is(typeof(ILambdaContext))).FirstOrDefault().Value, traceCtx.GetValueOrDefault());
            }
            else
                await meta.Proceed();
            
        }

        [Template]
        public dynamic? EventResponseTemplate()
        {
            object wrapper(object obj, ILambdaContext context)
            {
                return meta.Proceed();
            }

            if (_tracerProvider == null)
            {
                DynatraceSetup.InitializeLogging();
                _tracerProvider = TraceProviderHelper.BuildTraceProvider();
            }

            ILambdaLogger logger = meta.Target.Parameters.Where(p => p.Type.Is(typeof(ILambdaContext))).FirstOrDefault().Value.Logger;
            
            ActivityContext? traceCtx = Activity.Current?.Context;

            if (_tracerProvider != null)
            {
                Func<object, ILambdaContext, object> lambdaHandler = wrapper;

                return AWSLambdaWrapper.Trace(_tracerProvider, lambdaHandler, meta.Target.Parameters.FirstOrDefault().Value, meta.Target.Parameters.Where(p => p.Type.Is(typeof(ILambdaContext))).FirstOrDefault().Value, traceCtx.GetValueOrDefault());
            }
            else
                return meta.Proceed();

        }


        [Template]
        public void EventTemplate()
        {
            void wrapper(object obj, ILambdaContext context)
            {
                meta.Proceed();
            }

            if (_tracerProvider == null)
            {
                DynatraceSetup.InitializeLogging();
                _tracerProvider = TraceProviderHelper.BuildTraceProvider();
            }

            ILambdaLogger logger = meta.Target.Parameters.Where(p => p.Type.Is(typeof(ILambdaContext))).FirstOrDefault().Value.Logger;

            ActivityContext? traceCtx = Activity.Current?.Context;

            if (_tracerProvider != null)
            {
                Action<object, ILambdaContext> lambdaHandler = wrapper;

                AWSLambdaWrapper.Trace(_tracerProvider, lambdaHandler, meta.Target.Parameters.FirstOrDefault().Value, meta.Target.Parameters.Where(p => p.Type.Is(typeof(ILambdaContext))).FirstOrDefault().Value, traceCtx.GetValueOrDefault());
            }
            else
                meta.Proceed();
        }
    }
}