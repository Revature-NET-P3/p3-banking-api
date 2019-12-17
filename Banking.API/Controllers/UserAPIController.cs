using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using Banking.API.Models;
using Banking.API.Repositories.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//post "api/UserAPI/Verify"

namespace Banking.API.Controllers
{
    [EnableCors("DefaultPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserAPIController : ControllerBase
    {

        private readonly IUserRepo _context;

        public UserAPIController(IUserRepo context)
        {
            _context = context;
        }
        /// <summary>
        /// This method will be work to create user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>New User</returns>
        // POST: api/Createuser
        [HttpPost("register")]
        public async Task<ActionResult<bool>> CreateUser(User user)
        {
            bool canCreate = true;
            user.Id = 0;
            var listOfUsers = await _context.GetUsersAsync();
            foreach (var registeredUser in listOfUsers)
            {
                if (registeredUser.Email == user.Email && registeredUser.Username == user.Username)
                {
                    canCreate = false;
                    break;
                }
            }
            if (canCreate)
            {
                bool result = await _context.CreateUser(user);
                return result;
            }
            else
            {
                return canCreate;
            }
        }

        /// <summary>
        /// This method wil return the user by Id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return User</returns>
        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.ViewById(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<User>> GetUser(string username)
        {
            var user = await _context.ViewByUsername(username);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        /// <summary>
        /// This method will work to update user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Updated User</returns>
        // PUT: api/Updateuser/5
        [HttpPut("Updateuser")]
        public async Task<ActionResult<bool>> UpdateUser(User user)
        {
             await _context.UpdateUser(user);
            return true;

            
        }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="username"></param>
      /// <param name="passhash"></param>
      /// <returns></returns>
        [HttpPost("Verify")]
        public async Task<ActionResult<bool>> VerifyLogin(LoginCredentials credentials)
        {
         
           var result =  await _context.VerifyLogin(credentials.Username, credentials.Passhash);
            return result;
        }

        public class UserName
        {
            public string Username { get; set; }
            public string Passhash { get; set; }
        }
    }
}
