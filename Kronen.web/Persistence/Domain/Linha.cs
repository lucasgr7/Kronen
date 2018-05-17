using System.Collections.Generic;

namespace Kronen.web.Persistence.Domain
{
    public class Linha
    {
        public short Posicao {get;set;}
        public long Number {get;set;}
        public List<Card> Data {get;set;}
    }
}