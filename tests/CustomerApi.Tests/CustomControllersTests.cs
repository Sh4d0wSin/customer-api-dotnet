using System.Net;
using System.Net.Http.Json;
using CustomerApi.Models;


public class CustomersControllerTests : IClassFixture<CustomWebApplicationFactory>  {

    private readonly HttpClient _client;

    public CustomersControllerTests (CustomWebApplicationFactory factory) {
        _client = factory.CreateClient();
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

}