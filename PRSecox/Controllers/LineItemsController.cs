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
    public class LineItemsController : ControllerBase
    {
        private readonly PRSDbContext _context;

        public LineItemsController(PRSDbContext context)
        {
            _context = context;
        }

        // GET: api/LineItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LineItem>>> GetLineItems()
        {
            if (_context.LineItems == null)
            {
                return NotFound();
            }                               // includes the relevant request and products, then returning a list
                                            // of all lineitems
            return await _context.LineItems.Include(l => l.Request).Include(l => l.Product).ToListAsync();

        }


        [HttpGet("lines-for-pr/{id}")]
        public async Task<ActionResult<IEnumerable<LineItem>>> GetLineItemsByRequestId(int id)
        {
            // GET by requestid, returning lineitems/product
            var lineitem = await _context.LineItems.Where(l => l.RequestId == id).Include(p => p.Product).ToListAsync();


            if (lineitem == null)
            {
                return NotFound();
            }

            return lineitem;

        }

        // GET: api/LineItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LineItem>> GetLineItem(int id)
        {
            if (_context.LineItems == null)
            {
                return NotFound();
            } // includes the relevant request and products, then returning a list from a specific lineitem id
            var lineItem = await _context.LineItems.Include(l => l.Request).Include(l => l.Product).FirstOrDefaultAsync(l => l.Id == id);


            if (lineItem == null)
            {
                return NotFound();
            }

            return lineItem;
        }

        // PUT: api/LineItems/5

        [HttpPut("{id}")] //updating line item
        public async Task<IActionResult> PutLineItem(int id, LineItem lineItem)
        {
            if (id != lineItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(lineItem).State = EntityState.Modified;

            try
            {
                var requestid = lineItem.RequestId;
                await _context.SaveChangesAsync();
                await RecalculateRequestTotal(requestid);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LineItemExists(id))
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

        // POST: api/LineItems

        [HttpPost]
        public async Task<ActionResult<LineItem>> PostLineItem(LineItem lineItem)
        {
            if (_context.LineItems == null)
            {
                return Problem("Entity set 'PRSDbContext.LineItems'  is null.");
            }

            var requestid = lineItem.RequestId;
            _context.LineItems.Add(lineItem);
            //adding a new lineitem and then recalculating the total
            await _context.SaveChangesAsync();
            await RecalculateRequestTotal(requestid);

            return CreatedAtAction("GetLineItem", new { id = lineItem.Id }, lineItem);
        }

        // DELETE: api/LineItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLineItem(int id)
        {
            
            var lineItem = await _context.LineItems.FindAsync(id);

            if (lineItem == null)
            {
                return NotFound();
            }


            var requestid = lineItem.RequestId;
            _context.LineItems.Remove(lineItem);
            //deletes the lineitem and then recalculating the total
            await _context.SaveChangesAsync();
            await RecalculateRequestTotal(requestid);

            return NoContent();
        }

        private bool LineItemExists(int id)
        {
            return (_context.LineItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        private async Task RecalculateRequestTotal(int requestid)
        {


            var requesttotal = await _context.LineItems.Include(p => p.Product).Where(l => l.RequestId == requestid).SumAsync(p => p.Quantity * p.Product.Price);



            var request = await _context.Requests.FirstOrDefaultAsync(e => e.Id == requestid);


            //calculate the total
            // LINQ

            //update the request total
            // find the request, update the total and SaveChanges

            request.Total = requesttotal;

            _context.SaveChanges();
        }
    }
}
