using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRSecox.Models;

namespace PRSecox.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly PRSDbContext _context;

        public UsersController(PRSDbContext context)
        {
            _context = context;
        }


        //todo: GET: /api/users/username/password
        //todo: POST: GET: /api/users/login ---- pass as body username and password
        //created a new class/"DTO" - data transfer object "LoginDTO" 
        //limits data returned


        [HttpPost("login")]  
        public async Task<ActionResult> GetLoginByUsernamePassword([FromBody] LoginDTO login)
        {
            
            var user = await _context.Users.Where(u => u.Username == login.Username && u.Password == login.Password).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
           
        }


        // GET: api/Users
        [HttpGet] //returns list of all users, set to include their requests
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            return await _context.Users.Include(u => u.Requests).ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")] //get single user by their id, includes their requests
        public async Task<ActionResult<User>> GetUser(int id)
        {
         
            //pulling first user by their id(should be unique and shouldn't matter), while also including their requests
            var user = await _context.Users.Include(u => u.Requests).FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        
        [HttpPut("{id}")] //updates a users information, done on their ID
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
       
        [HttpPost]//creates a new user
        public async Task<ActionResult<User>> PostUser(User user)
        {
          if (_context.Users == null)
          {
              return Problem("Entity set 'PRSDbContext.Users'  is null.");
          }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]// deletes user, done on their ID
        public async Task<IActionResult> DeleteUser(int id)
        {
            
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        



    }
}
