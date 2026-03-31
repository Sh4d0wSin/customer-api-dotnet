namespace CustomerApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerApi.Data;
using CustomerApi.Models;
using CustomerApi.Dtos;




[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase {
    
    private readonly AppDbContext _context;


    

    public CustomersController(AppDbContext context) {
        _context = context;

    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetCustomers() {
            return await _context.Customers.Select(c => new CustomerResponseDto {Id = c.Id, Name = c.Name, Email = c.Email, Phone = c.Phone, Company = c.Company, CreatedAt = c.CreatedAt}).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResponseDto>> GetCustomer(int id) {
        var customer = await _context.Customers.FindAsync(id);

        if(customer == null) return NotFound();

        CustomerResponseDto customer_response = new CustomerResponseDto {Id = customer.Id, Name = customer.Name, Email = customer.Email, Phone = customer.Phone, Company = customer.Company, CreatedAt = customer.CreatedAt};
    
        return customer_response;
        
    }

     [HttpPost]
     public async Task<ActionResult<CustomerResponseDto>> CreateCustomer(CustomerCreateDto customerTransfer) {
        Customer customer = new Customer {Name = customerTransfer.Name, Email = customerTransfer.Email, Phone = customerTransfer.Phone, Company = customerTransfer.Company};
        customer.CreatedAt = DateTime.UtcNow;
        _context.Customers.Add(customer);

        await _context.SaveChangesAsync();

        CustomerResponseDto customer_response = new CustomerResponseDto {Id = customer.Id, Name = customer.Name, Email = customer.Email, Phone = customer.Phone, Company = customer.Company, CreatedAt = customer.CreatedAt};


        return CreatedAtAction(nameof(GetCustomer), new {id = customer.Id }, customer_response);
     }

     [HttpPut("{id}")]
     public async Task<IActionResult> UpdateCustomer(int id, CustomerCreateDto customCreateDto) {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null) return NotFound();

        customer.Name = customCreateDto.Name;
        customer.Email = customCreateDto.Email;
        customer.Phone = customCreateDto.Phone;
        customer.Company = customCreateDto.Company;

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

