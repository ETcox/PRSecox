﻿using System;
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
    public class VendorsController : ControllerBase
    {
        private readonly PRSDbContext _context;

        public VendorsController(PRSDbContext context)
        {
            _context = context;
        }


        // GET: api/Vendors
        [HttpGet] // return all vendors and including their products
        public async Task<ActionResult<IEnumerable<Vendor>>> GetVendors()
        {
            if (_context.Vendors == null)
            {
                return NotFound();
            }
            return await _context.Vendors.Include(v => v.Products).ToListAsync();
        }

        // GET: api/Vendors/5
        [HttpGet("{id}")] // retrieve vendor by their id
        public async Task<ActionResult<Vendor>> GetVendor(int id)
        {
            if (_context.Vendors == null)
            {
                return NotFound();
            }
            var vendor = await _context.Vendors.Include(v => v.Products).FirstOrDefaultAsync(v => v.Id == id);

            if (vendor == null)
            {
                return NotFound();
            }

            return vendor;
        }

        // PUT: api/Vendors/5
        [HttpPut("{id}")] // updating a single/existing vendor on their id
        public async Task<IActionResult> PutVendor(int id, Vendor vendor)
        {
            if (id != vendor.Id)
            {
                return BadRequest();
            }

            _context.Entry(vendor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendorExists(id))
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

        
        // POST: api/Vendors
        [HttpPost]  //creating a new vendor
        public async Task<ActionResult<Vendor>> PostVendor(Vendor vendor)
        {
            if (_context.Vendors == null)
            {
                return Problem("Entity set 'PRSDbContext.Vendors'  is null.");
            }
            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVendor", new { id = vendor.Id }, vendor);
        }

        // DELETE: api/Vendors/5
        [HttpDelete("{id}")] //deletes a single vendor by id
        public async Task<IActionResult> DeleteVendor(int id)
        {
            if (_context.Vendors == null)
            {
                return NotFound();
            }
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }

            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VendorExists(int id)
        {
            return (_context.Vendors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
