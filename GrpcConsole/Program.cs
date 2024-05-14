using System;
using System.IO;
using System.Net.Http;
using System.Threading.Channels;
using System.Threading.Tasks;
using APPGrpc;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcProtos.Clients;
using static APPGrpc.Reverse;

GrpcClient grpcClien = new GrpcClient("https://localhost:7119");
var cl = new GrpcProtos.Clients.ReverseClient(grpcClien);
// 监听 Bidirectional 返回
Task.Run(async () =>
{
  while (await cl.stream.ResponseStream.MoveNext(default))
  {
    var reply = cl.stream.ResponseStream.Current.Message;
    Console.WriteLine(reply);
  }
});

//using var channel = GrpcChannel.ForAddress("https://localhost:7119");
//var client = new Reverse.ReverseClient(channel);
//var stream = client.Bidirectional();
//Task.Run(async () =>
//{
//  while (await stream.ResponseStream.MoveNext(default))
//  {
//    var reply = stream.ResponseStream.Current.Message;
//    Console.WriteLine(reply);
//  }
//});
Console.ReadKey();

// The port number must match the port of the gRPC server.
//using var channel = GrpcChannel.ForAddress("https://localhost:7119");
//var client = new Greeter.GreeterClient(channel);
//var reply = await client.SayHelloAsync(
//                  new HelloRequest { Name = "GreeterClient" });
//Console.WriteLine("Greeting: " + reply.Message);
//Console.WriteLine("Press any key to exit...");
//Console.ReadKey();

//var client2 = new Reverse.ReverseClient(channel);
//var stream = client2.Bidirectional();

//// 监听 Bidirectional 返回
//Task.Run(async () =>
//{
//  while (await stream.ResponseStream.MoveNext(default))
//  {
//    Console.WriteLine($"{stream.ResponseStream.Current}");
//  }
//});
//// 客户端发送 Bidirectional
//while (true)
//{
//  Console.WriteLine("请输入内容：");
//  var i = Console.ReadLine() ?? "ABC";
//  await stream.RequestStream.WriteAsync(new() { Message = $"{i}" });
//  //await stream.RequestStream.CompleteAsync();
//}


//while (true)
//{
//  Console.WriteLine("Enter a number:");

//  var i = Console.ReadLine() ?? "ABC";
//  await ClientSide(client2, i); 
//}
// 服务器监听
static async Task ClientSide(Reverse.ReverseClient client, string i)
{
  var stream = client.ClientSide();

  //for (int i = 0; i < 5; i++)
  //{
  //  await stream.RequestStream.WriteAsync(new() { Message = $"{nameof(ClientSide)}-{i}" });
  //}
  await stream.RequestStream.WriteAsync(new() { Message = $"{i}" });
  await stream.RequestStream.CompleteAsync();

  var reply = await stream.ResponseAsync;
  Console.WriteLine($"{nameof(ClientSide)}-{reply.Message}");
  //DisplayReceivedMessage(reply);
}
