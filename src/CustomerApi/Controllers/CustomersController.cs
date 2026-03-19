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



}