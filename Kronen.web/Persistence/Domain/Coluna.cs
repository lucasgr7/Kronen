using System.Collections.Generic;

namespace Kronen.web.Persistence.Domain
{
    public class Coluna
    {
        public short Posicao {get;set;}
        public List<Linha> Linhas {get;set;}
    }
}