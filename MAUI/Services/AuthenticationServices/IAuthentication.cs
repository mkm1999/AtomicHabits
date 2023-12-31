using MAUI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI.Services.AuthenticationServices
{
    public interface IAuthentication
    {
        public Task<bool> IsAuthenticated();
        public Task<ResultDto<string>> login(string username, string password);
        public Task<ResultDto> SignUp(string name,string lastName,string number,string userName,string password);
    }
}
