using Asp.Versioning.Builder;

namespace Taller_Challenge_Backend.API.Common
{
    public interface IEndpoints
    {
        static abstract IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder builder, ApiVersionSet versions);
    }

    public static class EndpointsRegister
    {
        public static IEndpointRouteBuilder Map<T>(this IEndpointRouteBuilder builder, ApiVersionSet versions) where T : IEndpoints
        {
            T.MapEndpoints(builder, versions);
            return builder;
        }
    }
}
