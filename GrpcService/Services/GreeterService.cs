using APPGrpc;
using Grpc.Core;

namespace GrpcService.Services
{
  public class GrpcTestService() : Greeter.GreeterBase
  {
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
      Console.WriteLine($"SayHello {request.Name}");
      return Task.FromResult(new HelloReply
      {
        Message = "Hello 9999999999999" + request.Name
      });
    }
  }
}
