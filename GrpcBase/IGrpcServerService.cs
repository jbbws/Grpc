using Grpc.Core;
namespace GrpcBase{
    public interface IGrpcServerService
    {
        ServerServiceDefinition ToServerServiceDefinition();
    }
}