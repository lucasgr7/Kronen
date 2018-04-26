using System;
using Xunit;
using Kronen.web.Persistence;

namespace Kronen.test
{
    public class BaseTest
    {
        protected readonly Contexto contexto;

        public BaseTest(){
            contexto = getContexto();
            // var service = new RelatorioService(new RelatorioRepositorio(contexto), contexto, new SentryService("mock"));
            // controller = new CadastroRelatorioController(service, new SentryService("mock"));
        }

        protected Contexto getContexto(){
            // var options = new DbContextOptionsBuilder<RelatorioContexto>()
            //                     .UseInMemoryDatabase(new Guid().ToString())
            //                     .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            //                     .Options;

            // var contexto = new Contexto(options);
            return contexto;
        }
    }
}
