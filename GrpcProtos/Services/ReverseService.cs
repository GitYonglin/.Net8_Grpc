using APPGrpc;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GrpcProtos.Services
{
  public class ReverseService : Reverse.ReverseBase
  {
    private static Dictionary<int, IServerStreamWriter<Reply>> _listContent = new Dictionary<int, IServerStreamWriter<Reply>>();
    // 双向流式处理
    public override async Task Bidirectional(IAsyncStreamReader<Request> requestStream, IServerStreamWriter<Reply> responseStream, ServerCallContext context)
    {

      AddClient();
      while (await requestStream.MoveNext())
      {
        Console.WriteLine($"{requestStream.Current.Message}");
        var marr = requestStream.Current.Message.Split(":");
        var Message = new { Neme = int.Parse(marr[0]), Message = marr[1] };
        _listContent.TryGetValue(Message.Neme, out var clientStream);
        if (clientStream != null)
        {
          await clientStream.WriteAsync(new Reply { Message = requestStream.Current.Message });
        }
      }

      void AddClient()
      {
        var hashCode = responseStream.GetHashCode();
        _listContent.Add(hashCode, responseStream);
        var Message = string.Join(",", _listContent.Select(s => s.Key));
        foreach (var item in _listContent)
        {
          item.Value.WriteAsync(new Reply { Message = Message });
        }
      }
    }

  }

  //public class ReverseService : Reverse.ReverseBase
  //{
  //  private readonly ILogger<ReverseService> _logger;

  //  public ReverseService(ILogger<ReverseService> logger)
  //  {
  //    Console.WriteLine("ReverseServiceReverseServiceReverseService");
  //    _logger = logger;
  //  }

  //  private static Reply CreateReplay(Request request)
  //  {
  //    return new Reply
  //    {
  //      Message = new string(request.Message.Reverse().ToArray())
  //    };
  //  }

  //  // 双向流式处理
  //  public override async Task Bidirectional(IAsyncStreamReader<Request> requestStream, IServerStreamWriter<Reply> responseStream, ServerCallContext context)
  //  {
  //    AddClient(responseStream, true);
  //    while (await requestStream.MoveNext())
  //    {
  //      AddClient(responseStream);
  //      //DisplayReceivedMessage(requestStream.Current);
  //      await responseStream.WriteAsync(CreateReplay(requestStream.Current));
  //    }

  //    void AddClient(IServerStreamWriter<Reply> responseStream, bool init = false)
  //    {
  //      if (init)
  //      {
  //        Console.WriteLine($"第一次第一次");
  //      }
  //      Console.WriteLine($"hashCode: {responseStream.GetHashCode()}");
  //    }
  //  }

  //}

}

