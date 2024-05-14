using APPGrpc;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static APPGrpc.Reverse;

namespace GrpcProtos.Clients
{
  public class ReverseClient
  {
    //private readonly GrpcClient _grpcClient;
    private readonly Reverse.ReverseClient _client;
    public AsyncDuplexStreamingCall<Request, Reply>? stream;

    public ReverseClient(GrpcClient grpcClient)
    {
      _client = grpcClient.reverseClient;
    }
    public void Init(Action<string> action)
    {
      stream = _client.Bidirectional();
      //监听 Bidirectional 返回
      Task.Run(async () =>
      {
        while (await stream.ResponseStream.MoveNext(default))
        {
          var reply = stream.ResponseStream.Current.Message;
          action(reply);
        }

      });
    }

    public async Task Send(string message)
    {
      if (stream != null)
        await stream.RequestStream.WriteAsync(new Request { Message = $"{message}" });
    }
  }
}
