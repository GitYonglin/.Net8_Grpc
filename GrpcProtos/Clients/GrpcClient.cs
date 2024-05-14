using APPGrpc;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrpcProtos.Clients
{
  public class GrpcClient
  {
    private readonly GrpcChannel channel;
    public readonly Reverse.ReverseClient reverseClient;
    public GrpcClient(string url)
    {
      channel = GrpcChannel.ForAddress(url);
      reverseClient = new Reverse.ReverseClient(channel);
      
    }
  }
}
