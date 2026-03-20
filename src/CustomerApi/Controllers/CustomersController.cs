namespace CustomerApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerApi.Data;
using CustomerApi.Models;




[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase {
    
    private readonly AppDbContext _context;


    

    public CustomersController(AppDbContext context) {
        _context = context;

    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers() {
            return await _context.Customers.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomer(int id) {
        var customer = await _context.Customers.FindAsync(id);

        if(customer == null) return NotFound();
    
        return customer;
        
    }

     [HttpPost]
     public async Task<ActionResult<Customer>> CreateCustomer(Customer customer) {
        customer.CreatedAt = DateTime.UtcNow;
        _context.Customers.Add(customer);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCustomer), new {id = customer.Id }, customer);
     }

     [HttpPut("{id}")]
     public async Task<IActionResult> UpdateCustomer(int id, Customer customer) {
        if (id != customer.Id) return BadRequest();
        _context.Entry(customer).State = EntityState.Modified;
        try {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            if(!await _context.Customers.AnyAsync(c => c.Id == id)) return NotFound();
            throw;
        }
        return NoContent();
     }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id) {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            _context.Customers.Remove(customer);
            
            await _context.SaveChangesAsync();

            return NoContent();
        }
       



























}

