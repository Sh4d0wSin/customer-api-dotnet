using System.Net;
using System.Net.Http.Json;
using CustomerApi.Models;
using Microsoft.Extensions.DependencyInjection;
using CustomerApi.Data;


public class CustomersControllerTests : IClassFixture<CustomWebApplicationFactory>,  IAsyncLifetime  {

    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public CustomersControllerTests (CustomWebApplicationFactory factory) {
        _factory = factory;
        _client = factory.CreateClient();

    }

    public async Task InitializeAsync() {
        using var scope = _factory.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Customers.RemoveRange(context.Customers);

        await context.SaveChangesAsync();
    }

    public Task DisposeAsync() {
        return Task.CompletedTask;
    }

    [Fact]
    public async Task Get_Customers_ReturnsEmptyList() {
        var response = await _client.GetAsync("/api/customers");
        response.EnsureSuccessStatusCode();


        var customers = await response.Content.ReadFromJsonAsync<List<Customer>>();


        Assert.NotNull(customers);

        Assert.Empty(customers);




    }

    [Fact]
    public async Task Post_Customers_ReturnsCreated() {
        Customer new_customer = new Customer {Name = "Michael", Email = "myers@testmail.de"};
        
        var response = await _client.PostAsJsonAsync("/api/customers", new_customer);

        

        var customer = await response.Content.ReadFromJsonAsync<Customer>();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        Assert.NotNull(customer);
        Assert.Equal("Michael", customer.Name);
        Assert.Equal("myers@testmail.de", customer.Email);
        Assert.True(customer.Id > 0);


    }

    [Fact]
    public async Task Get_HappyCustomers_ReturnsId() {
        Customer new_customer = new Customer {Name = "Happy", Email = "happy@testmail.de"};
        
        var response = await _client.PostAsJsonAsync("/api/customers", new_customer);

        var customer = await response.Content.ReadFromJsonAsync<Customer>();
        Assert.NotNull(customer);



        var createdCustomer = await _client.GetAsync($"/api/customers/{customer.Id}");

        var fetchedCustomer = await createdCustomer.Content.ReadFromJsonAsync<Customer>();

        Assert.NotNull(fetchedCustomer);
        Assert.Equal("Happy", fetchedCustomer.Name);
        Assert.Equal("happy@testmail.de", fetchedCustomer.Email);

    }

    [Fact]
    public async Task Get_NoCustomers_ReturnsId() {
        
        var response = await _client.GetAsync("/api/customers/9999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }

    [Fact]
    public async Task Put_Customers() {
        Customer new_customer = new Customer {Name = "Putter", Email = "put@testmail.de"};
        
        var response = await _client.PostAsJsonAsync("/api/customers", new_customer);

        var customer = await response.Content.ReadFromJsonAsync<Customer>();
        Assert.NotNull(customer);
        customer.Name = "Updated Putter";

        var put_response = await _client.PutAsJsonAsync($"/api/customers/{customer.Id}", customer);
        Assert.Equal(HttpStatusCode.NoContent, put_response.StatusCode);

    

    }

    [Fact]
    public async Task Delete_Customer_ById() {
        Customer new_customer = new Customer {Name = "Useless", Email = "useless@testmail.de"};

        var response = await _client.PostAsJsonAsync("/api/customers", new_customer);
        var customer = await response.Content.ReadFromJsonAsync<Customer>();
        Assert.NotNull(customer);
        var delete_response = await _client.DeleteAsync($"/api/customers/{customer.Id}");
        Assert.Equal(HttpStatusCode.NoContent, delete_response.StatusCode);

    }

}