using LAB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Crypto.Generators;
using XapauServer.Helpers;

namespace LAB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                var users = _context.user.ToList();
                return Ok(users);

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                if (e.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {e.InnerException.Message}");
                }
                return BadRequest("An error occurred while fetching patient data.");
            }
        }


        [HttpPost("signup")]
        public async Task<ActionResult<CustomResponse<User>>> Signup(User Userrequest)
        {

            try
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(Userrequest.Password);

                Userrequest.Password = passwordHash;

                User newUser = new User
                {
                    Name = Userrequest.Name,
                    Password = passwordHash,
                    Email = Userrequest.Email,
                    Last_login = Userrequest.Last_login,
                    Permission = Userrequest.Permission,
                    Status = Userrequest.Status
                };

                await _context.user.AddAsync(newUser);
                await _context.SaveChangesAsync();

                var user = _context.user.Where(u => u.Email == Userrequest.Email).SingleOrDefault();

                if (user == null)
                {
                    return NotFound("User Not Found");
                }

                var response = new CustomResponse<User>
                {
                    status = true,
                    message = "signup successful",
                    data = user
                };

                return Ok(response);
            }
           
            catch (Exception e)
            {

                Console.WriteLine($"Error: {e.Message}");
                if (e.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {e.InnerException.Message}");
                }
                return BadRequest("An error occurred while saving the entity changes.");
            }
        }


        [HttpPost("signin")]
        public async Task<ActionResult<CustomResponse<Signin>>> Signin(Signin signinRequest)
        {

            try
            {
                var user = _context.user.Where(u => u.Email == signinRequest.Email).SingleOrDefault();
                if (user == null)
                {
                    return NotFound("User not found");
                }

                bool verified = BCrypt.Net.BCrypt.Verify(signinRequest.Password, user.Password);

                if (!verified)
                {
                    return NotFound("Email or password error");
                }

                var response = new CustomResponse<User>
                {
                    status = true,
                    message = "sign in success",
                    data = user
                };

                return Ok(response);
            }
            catch (Exception e)
            {

                Console.WriteLine($"Error: {e.Message}");
                if (e.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {e.InnerException.Message}");
                }
                return BadRequest("An error occurred while saving the entity changes.");
            }

        }



        }
}
