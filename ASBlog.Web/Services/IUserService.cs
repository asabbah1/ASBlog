using ASBlog.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASBlog.Web.Services
{
    public interface IUserService
    {
        Task<List<UserRs>> GetAllUsersAsync();
        Task<UserRs> GetUserAsync();
        Task<LoginRs> LoginAsync(LoginRq loginRq);
        Task<GeneralRs> RegisterAsync(RegisterRq registerRq);
    }
}
