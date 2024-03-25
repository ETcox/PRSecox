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
    public class RequestsController : ControllerBase
    {
        private readonly PRSDbContext _context;

        const string statusRejected = "REJECTED";
        const string statusApproved = "APPROVED";
        const string statusNew = "NEW";
        const string statusReview = "REVIEW";




        public RequestsController(PRSDbContext context)
        {
            _context = context;
        }


        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            if (_context.Requests == null)
            {
                return NotFound();
            }

            //pulls all requests and showing the user it belongs to
            return await _context.Requests.Include(r => r.User).ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
           

            // getting a single request by its ID and showing the user
            var request = await _context.Requests.Include(r => r.User).FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        // PUT: api/Requests/5

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
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

        // POST: api/Requests

        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
            if (_context.Requests == null)
            {
                return Problem("Entity set 'PRSDbContext.Requests'  is null.");
            }

            //adds new request and saving to DB
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
           
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            // removing the request by Id and saving changes
            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPost("review/{id}")]
        public async Task<ActionResult<Request>> Review(int id)
        {
            try
            {
                var request = await _context.Requests.FindAsync(id);


                if (request == null)
                {
                    return NotFound();
                }


                //Sets the status of the request for the id provided to "REVIEW"
                //unless the total of the request is less than or equal to $50.
                //If so, it sets the status directly to "APPROVED".

                if (request.Total <= 50)
                {
                    request.Status = statusApproved;
                }
                else
                {
                    request.Status = statusReview;
                }


                await _context.SaveChangesAsync();

                return request;
            }




            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


        [HttpPost("reject/{id}")]                               //takes input from the body and implements
                                                                //into "reason for rejection"                    
        public async Task<ActionResult<Request>> Reject(int id, [FromBody] string reason)
        {
            try
            {
                var request = await _context.Requests.FindAsync(id);


                if (request == null)
                {
                    return NotFound();
                }



                request.ReasonForRejection = reason;
                request.Status = statusRejected;


                await _context.SaveChangesAsync();

                return request;
            }

            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


        [HttpPost("approve/{id}")]  // sets the request status to Approved
        public async Task<ActionResult<Request>> Approve(int id)
        {
            try
            {
                var request = await _context.Requests.FindAsync(id);


                if (request == null)
                {
                    return NotFound();
                }


                request.Status = statusApproved;


                await _context.SaveChangesAsync();

                return request;
            }

            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


        [HttpGet("reviews/{userid}")] //Gets list of requests with the "REVIEW" status and not owned
                                      //by the user with the primary key of id.
        public async Task<ActionResult<IEnumerable<Request>>> Reviews(int userid)
        {

            var request = await _context.Requests.Include(r => r.User).Where(r => r.UserId != userid && r.Status == statusReview)
                .ToListAsync();


            if (request == null)
            {
                return NotFound();
            }

            await _context.SaveChangesAsync();

            return request;


        }



        private bool RequestExists(int id)
        {
            return (_context.Requests?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
