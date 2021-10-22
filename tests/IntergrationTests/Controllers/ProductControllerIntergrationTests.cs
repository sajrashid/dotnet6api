
using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;

using MyAPI.Models;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Xunit;

namespace TestProject.IntergrationTests.Controllers
{
    public class ProductControllerIntergrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;
        public ProductControllerIntergrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var testData = new TestData();
            // delete data & recreate new for every test run
            testData.DeleteProducts();
            // creates 100 products
            testData.CreateProducts();
        }

        // Tests will execute in sequence as they are in the same test class 

        [Fact]
        public async Task Get_AllProducts_Returns_ListOfProducts_OK()
        {
            var response = await _client.GetAsync("/api/Products/");
            response.EnsureSuccessStatusCode();
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var res = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var productList = JsonSerializer.Deserialize<List<Product>>(res);
            productList.Should().NotBeNull();
            productList.Should().HaveCount(100);
            productList.Should().BeOfType<List<Product>>();
        }

        [Fact]
        public async Task Get_Products_By_Id_Returns_Single_Products_OK()
        {
            var response = await _client.GetAsync("/api/Products/1");

            response.EnsureSuccessStatusCode();
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var res = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var product = JsonSerializer.Deserialize<Product>(res);
            product.Should().NotBeNull();
            product.Should().BeOfType<Product>();
        }
        [Fact]
        public async Task Get_Products_By_Id_Returns_NotFound()
        {
            var response = await _client.GetAsync("/api/Products/1010");
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_New_Product_Returns_Single_Products_OK()
        {
            //Arrange 
            // create a new product
            var product = new Product { Company = "ABC CO", Phone = "111-222-333-444", Price = 55, InStock = true, StockCount = 5, NewStockDate = DateTime.Now, };
            var json = JsonSerializer.Serialize<Product>(product);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            // insert product
            var response = await _client.PostAsync("/api/Products", data).ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var res = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var newproduct = JsonSerializer.Deserialize<Product>(res, options);

            // set new productid so we can reference in the delete test to run next
            // Assert
            newproduct.Should().NotBeNull();
            newproduct.Should().BeOfType<Product>();
            newproduct.Company.Should().Be("ABC CO");
            newproduct.StockCount.Should().Be(5);
        }

        [Fact]
        public async Task Update_Product_Returns_Single_Products_OK()
        {
            //Arrange 
            // create a new product
            var product = new Product { Company = "XYZ CO", Phone = "111-222-333-444", Price = 55, InStock = true, StockCount = 5, NewStockDate = DateTime.Now, };
            var json = JsonSerializer.Serialize<Product>(product);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            // insert product
            var response = await _client.PostAsync("/api/Products", data).ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var res = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var newproduct = JsonSerializer.Deserialize<Product>(res, options);

            // Assert
            newproduct.Should().NotBeNull();
            newproduct.Should().BeOfType<Product>();
            newproduct.Company.Should().Be("XYZ CO");
            newproduct.StockCount.Should().Be(5);

            product = new Product { Id = newproduct.Id, Company = "Test CO", Phone = "11X-221-333-444", Price = 35, InStock = false, StockCount = 5, NewStockDate = DateTime.Now, };
            json = JsonSerializer.Serialize<Product>(product);
            data = new StringContent(json, Encoding.UTF8, "application/json");
            response = await _client.PutAsync($"/api/Products/{newproduct.Id}", data).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            res = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var upatedproduct = JsonSerializer.Deserialize<Product>(res, options);

            upatedproduct.Should().NotBeNull();
            upatedproduct.Should().BeOfType<Product>();
            upatedproduct.Company.Should().Be("Test CO");
            upatedproduct.Id.Should().Be(newproduct.Id);
        }

        [Fact]
        public async Task Update_Non_Exisiting_Product_Returns_NotFound()
        {
            // create a new product
            var product = new Product { Id = 101010, Company = "ABC CO", Phone = "111-222-333-444", Price = 55, InStock = true, StockCount = 5, NewStockDate = DateTime.Now, };
            var json = JsonSerializer.Serialize<Product>(product);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/api/Products/{product.Id}", data).ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }


        [Fact]
        public async Task Update_New_Product_Returns_Single_Products_OK()
        {
            // create a new product
            var product = new Product { Company = "ABC CO", Phone = "111-222-333-444", Price = 55, InStock = true, StockCount = 5, NewStockDate = DateTime.Now, };
            var json = JsonSerializer.Serialize<Product>(product);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // insert product
            var response = await _client.PostAsync("/api/Products", data).ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var res = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var newproduct = JsonSerializer.Deserialize<Product>(res, options);

            var newProductid = newproduct.Id;
            //Arrange 
            // create a new product
            var productToUpdate = new Product { Id = newProductid, Company = "ABC CO", Phone = "111-222-333-444", Price = 55, InStock = true, StockCount = 15, NewStockDate = DateTime.Now, };
            json = JsonSerializer.Serialize<Product>(productToUpdate);
            data = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            // insert product
            response = await _client.PutAsync($"/api/Products/{newProductid}", data).ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            res = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var Updatedproduct = JsonSerializer.Deserialize<Product>(res, options);

            // Assert
            Updatedproduct.Should().NotBeNull();
            Updatedproduct.Should().BeOfType<Product>();
            Updatedproduct.Company.Should().Be("ABC CO");
            Updatedproduct.StockCount.Should().Be(15);
        }
        [Fact]
        public async Task Deletes_New_Product_Returns_OK()
        {
            // create a new product
            var product = new Product { Company = "ABC CO", Phone = "111-222-333-444", Price = 55, InStock = true, StockCount = 5, NewStockDate = DateTime.Now, };
            var json = JsonSerializer.Serialize<Product>(product);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // insert product
            var response = await _client.PostAsync("/api/Products", data).ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var res = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var newproduct = JsonSerializer.Deserialize<Product>(res, options);

            // set new productid so we can reference in the delete test to run next
            var newProductid = newproduct.Id;

            // _newProductid is set in previous test Create_New_Product_Returns_Single_Products_OK
            response = await _client.DeleteAsync($"/api/Products/{newProductid}").ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}


