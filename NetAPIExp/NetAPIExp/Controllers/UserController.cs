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
    [Route("/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserContext userContext;

        public UserController(UserContext context)
        {
            userContext = context;
        }

        /*[RequireHttps] */[HttpGet("userenum")]
        public ActionResult GetBoots(int userID = -1)
        {
            ActionResult result = userID >= 0f ? Ok() : NotFound();
            return result;
        }

        [HttpGet("TestDatabase")]
        public async Task<ActionResult<AuthenticationInfo>> TestDatabase(int userID = -1)
        {
            List<List<object>> results = await DatabaseHandler.Handler.ExecuteQuery($"SELECT * FROM AuthInfo WHERE ID = {userID};", new List<string>() { "ID", "username", "age" });
            AuthenticationInfo info = new AuthenticationInfo()
            {
                ID = (int)results[0][0],
                username = (string)results[0][1], 
                age = (int)results[0][2]
            };

            return new ActionResult<AuthenticationInfo>(info);
        }

        /*[HttpGet("users/")]
        public async Task<ActionResult<AuthenticationInfo>> GetAuthenticationInfo(int userID)
        {
            Random r = new Random();
            AuthenticationInfo userInfo = new AuthenticationInfo()
            {
                ID = userID,
                //username = "Testing;",
                age = r.Next(10, 50)
            };

            ActionResult<AuthenticationInfo> returnInfo = new ActionResult<AuthenticationInfo>(userInfo);

            return userInfo;
            //return Ok(userInfo);
        }*/

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

        [HttpGet("allUsers")]
        public async Task<ActionResult<List<AuthenticationInfo>>> GetAllUsers(int quantity)
        {
            Random r = new Random();
            List<AuthenticationInfo> users = new List<AuthenticationInfo>();

            for (int i = 0; i < quantity; i++)
            {
                AuthenticationInfo currentInfo = new AuthenticationInfo()
                {
                    ID = i,
                    username = $"User{i}", 
                    age = r.Next(10, 50)
                };

                users.Add(currentInfo);
            }
            
            return new ActionResult<List<AuthenticationInfo>>(users);
        }
        /*[HttpGet("users")]
        public ActionResult<AuthenticationInfo> GetAuthenticationInfo(int userID)
        {
            Random r = new Random();
            AuthenticationInfo userInfo = new AuthenticationInfo()
            {
                ID = userID,
                username = "Testing;",
                age = r.Next(10, 50)
            };
            
            ActionResult<AuthenticationInfo> returnInfo = new ActionResult<AuthenticationInfo>(userInfo);
            
            return returnInfo;
        }*/

        [HttpPost]
        public async Task<ActionResult> UploadImage([FromBody]UserController controller)
        {
            string users = await Task.Run(UserJSON);

            return NotFound();
        }

        private string UserJSON()
        {
            return "";
        }
    }
}