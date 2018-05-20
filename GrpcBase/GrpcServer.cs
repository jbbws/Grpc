using System;
using System.Collections.Generic;
using Grpc.Core;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
namespace GrpcBase
{
    public class GrpcServer : IGrpcServer
    {
        private Server _server = new Server();
        private IOptions<GrpcServerOptions> _options;
        private IEnumerable<IGrpcServerService> _services;
        private ILogger<GrpcServer> _logger;
        public GrpcServer(IOptions<GrpcServerOptions> options,IEnumerable<IGrpcServerService> services,ILogger<GrpcServer> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }
        public void Stop() {
            _server.ShutdownAsync().Wait();
        }
        public void Start() {
            var host = _options.Value.ListenAddress.Host;
            var port = _options.Value.ListenAddress.Port;
            _server.Ports.Add(host, port, ServerCredentials.Insecure);

            foreach(var service in _services) {
                _server.Services.Add(service.ToServerServiceDefinition());
            }
            _server.Start();
        }

    }
}
