using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Threading.Tasks;
using NetAPIExp.Models;
using System.Text;
using Microsoft.Extensions.ObjectPool;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Http.HttpResults;
using NetAPIExp.Database;

namespace NetAPIExp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserContext userContext;

        public UserController(UserContext context)
        {
            userContext = context;
        }

        [HttpGet("getUser")]
        public async Task<ActionResult<AuthenticationInfo>> GetUser(int userID)
        {
            AuthenticationInfo user = userContext.AuthInfo.Where(a => a.ID == userID).Single();

            ActionResult<AuthenticationInfo> returnResult = user != null ? new ActionResult<AuthenticationInfo>(user) : NotFound();

            return returnResult;
        }

        [HttpGet("addUser")]
        public async Task<ActionResult<AuthenticationInfo>> AddUser(int userID, string name, int userAge)
        {
            AuthenticationInfo newUser = new AuthenticationInfo()
            {
                ID = userID, 
                username = name, 
                age = userAge
            };

            await userContext.AuthInfo.AddAsync(newUser);
            await userContext.SaveChangesAsync();

            return new ActionResult<AuthenticationInfo>(newUser);
        }
    }
}