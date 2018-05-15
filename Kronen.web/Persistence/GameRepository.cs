using System.Collections.Generic;
using Kronen.web.Persistence.Domain;

namespace Kronen.web.Persistence
{
    public static class GameRepository
    {
        public static List<GameRoom> gameRooms {get;set;}
        public static List<Player> ActivePlayers {get;set;}
    }
}