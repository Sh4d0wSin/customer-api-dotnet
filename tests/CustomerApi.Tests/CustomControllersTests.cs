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
}