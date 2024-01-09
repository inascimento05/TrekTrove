using System.Diagnostics.CodeAnalysis;

namespace TrekTrove.Api.Bootstrap.Bootstrapers
{
    [ExcludeFromCodeCoverage]
    public static class HealthCheckBootstrap
    {
        private const string HEALTHCHECK_PING_PATHSTRING = $"/ping";
        private const string HEALTHCHECK_PONG_PATHSTRING = $"/pong";
    }
}
