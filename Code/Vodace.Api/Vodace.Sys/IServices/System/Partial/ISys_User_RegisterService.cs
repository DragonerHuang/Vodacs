
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface ISys_User_RegisterService
    {
        WebResponseContent AddUserRegister(UserBaseDto register);
        WebResponseContent RegisterUserNew(UserRegisterNewDto register);
        //WebResponseContent RegisterUser(UserRegisterDto register);
        //WebResponseContent GetUserRegisterDto();
        Task<WebResponseContent> CheckExist(string idNo);

        Task<WebResponseContent> CheckCompanyNo(string com_no);
        Task<WebResponseContent> CheckUserExist(string com_no, string userName);

    }
 }
