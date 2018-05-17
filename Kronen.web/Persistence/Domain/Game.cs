using System.Collections.Generic;

namespace Kronen.web.Persistence.Domain
{
    public class Game
    {
        public long GameId {get;set;}
        public long IndexCartas {get;set;}
        public List<Tabuleiro> Tabuleiros {get;set;}
        public List<Card> CartasVisiveis {get;set;}
        public List<Card> Baralho {get;set;}
        public string ActivePlayer {get;set;}
        
    }
}