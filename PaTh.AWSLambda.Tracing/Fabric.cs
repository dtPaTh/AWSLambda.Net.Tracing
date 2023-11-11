using Metalama.Framework.Fabrics;
using Metalama.Framework.Code;
using Amazon.Lambda.Core;

namespace PaTh.AWSLambda.Tracing
{

    internal class Fabric : TransitiveProjectFabric
    {
        public override void AmendProject(IProjectAmender amender)
        {
            amender.Outbound
                .SelectMany(compilation => compilation.AllTypes)
                .Where(type => type.Accessibility == Accessibility.Public)
                .SelectMany(type => type.Methods)
                .Where(method => method.Accessibility == Accessibility.Public && method.Name != "ToString" && true==method.Parameters.Any(p=>p.Type.Is(typeof(ILambdaContext))))
                .AddAspectIfEligible<TraceInvokationHandlerAttribute>();

        }
    }
}
