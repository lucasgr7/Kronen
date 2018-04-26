using Kronen.web.Services.Contract.Dto;

namespace Kronen.web.Services.Contract
{
    public interface IUsersService
    {
         DtoLogonResponse Logon(DtoLogonRequest dto);
    }
}