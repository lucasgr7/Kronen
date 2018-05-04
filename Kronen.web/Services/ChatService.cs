using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Kronen.web.Persistence.Domain;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Kronen.web.Services
{
    public static class ChatService
    {
        private static List<WebSocket> LobbySockets {get;set;}
        public static List<RoomWebSocket> RoomsSockets {get;set;}
        public static async Task Echo(HttpContext context, WebSocket webSocket)
        {
            if(LobbySockets==null)
                LobbySockets = new List<WebSocket>();
            LobbySockets.Add(webSocket);

            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                foreach(var socket in LobbySockets){
                    await socket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                }
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            LobbySockets.Remove(webSocket);
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
        public static async Task EchoRoom(HttpContext context, WebSocket webSocket, long id)
        {
            if(RoomsSockets==null)
                RoomsSockets = new List<RoomWebSocket>();
            var roomSocket = new RoomWebSocket(){
                socket = webSocket,
                RoomId = id
            };
            RoomsSockets.Add(roomSocket);

            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                var rooms = RoomsSockets.Where(x => x.RoomId == id).ToList();
                foreach(var room in rooms){
                    await room.socket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                }
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            //todo: mandar mensagem que saiu da sala
            RoomsSockets.Remove(roomSocket);
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
    public class RoomWebSocket{
        public WebSocket socket {get;set;}
        public long RoomId {get;set;}
    }
}