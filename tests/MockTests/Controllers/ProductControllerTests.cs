using FakeItEasy;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

using MyAPI.Controllers;
using MyAPI.Models;
using MyAPI.Repository;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;
namespace TestProject.MockTests.Controllers
{
    public class ProductControllerTests
    {
        private readonly IProductRepository fakeProductRepository;
        public ProductControllerTests()
        {
            fakeProductRepository = A.Fake<IProductRepository>();
        }

        private ProductsController CreateProductController()
        {
            return new ProductsController(
                fakeProductRepository);
        }

        [Fact]
        [Trait("API", "Unit")]
        public async Task GetAllProductsReturnsAllProducts()
        {
            // Arrange
            var productController = CreateProductController();

            A.CallTo(() => fakeProductRepository.GetAllProducts()).Returns(new List<Product>
            {
                new Product {
                    Id = 0,
                    Company="abc",
                    Phone="123-456-789",
                    InStock=true,
                    NewStockDate=DateTime.Now,
                    Price=123,
                    StockCount=5
                },
                 new Product {
                    Id = 1,
                    Company="xyz",
                    Phone="231-333-789",
                    InStock=false,
                    NewStockDate=DateTime.Now,
                    Price=555,
                    StockCount=100
                },
                   new Product {
                    Id = 2,
                    Company="dummy",
                    Phone="231-333-789",
                    InStock=true,
                    NewStockDate=DateTime.Now,
                    Price=111,
                    StockCount=0
                }
            });

            // Act
            var response = await productController.Get().ConfigureAwait(false);

            // Assert
            // tests the return type
            var manager = Fake.GetFakeManager(fakeProductRepository);
            Assert.Equal(typeof(IProductRepository), manager.FakeObjectType);

            var result = response.Result as OkObjectResult;

            Assert.NotNull(result);
            // test for http 200

            Assert.IsType<OkObjectResult>(result);

            // test we return 3 products
            var listProducts = new List<Product>();
            listProducts = (List<Product>)result.Value;
            listProducts.Should().HaveCount(3);
            listProducts.Should().BeOfType<List<Product>>();
        }

        [Fact]
        [Trait("API", "Unit")]
        public async Task GetProductReturnsSingleProduct()
        {
            // Arrange
            A.CallTo(() => fakeProductRepository.GetProduct(1)).Returns(new Product
            {
                Id = 1,
                Company = "ONE",
                Phone = "111-111-111",
                InStock = true,
                NewStockDate = DateTime.Now,
                Price = 1,
            });

            const int id = 1;
            // Act
            var productController = CreateProductController();
            var response = await productController.Get(
                id).ConfigureAwait(false);
            // test for a not found 
            OkObjectResult result = response.Result as OkObjectResult; //may be null

            result.Should().BeOfType<OkObjectResult>();
            result.Should().NotBeNull();

            var product = new Product();
            product = (Product)result.Value;
            product.Id.Should().Be(1);
            product.Should().BeOfType<Product>();
        }

        [Fact]
        [Trait("API", "Unit")]
        public async Task DeleteProductReturnDeletedProductId()
        {
            // Arrange
            const int id = 1;
            A.CallTo(() => fakeProductRepository.DeleteProduct(id)).Returns(id);

            var productController = CreateProductController();
            // Act
            var response = await productController.Delete(id
                ).ConfigureAwait(false);

            // Assert
            OkObjectResult result = response.Result as OkObjectResult;

            result.Should().NotBeNull();
            result.Value.Should().Be(id);
            result.Value.Should().BeOfType<int>();
        }

        [Fact]
        [Trait("API", "Unit")]
        public async Task UpdateProductReturnsUpdatedProduct()
        {
            const int resourceId = 1;
            var productToUpDate = new Product()
            {
                Id = 1,
                Company = "ONE",
                Phone = "111-111-111",
                InStock = true,
                NewStockDate = DateTime.Now,
                Price = 1,
            };

            // Arrange
            A.CallTo(() => fakeProductRepository.UpdateProduct(1, productToUpDate)).Returns(1);

            var productController = CreateProductController();
            // Act
            var response = await productController.Put(resourceId, productToUpDate
                ).ConfigureAwait(false);

            // Assert
            OkObjectResult result = response.Result as OkObjectResult;

            result.Should().NotBeNull();

            var product = new Product();
            product = (Product)result.Value;
            product.Id.Should().Be(productToUpDate.Id);
            product.Should().BeOfType<Product>();
        }

        [Fact]
        [Trait("API", "Unit")]
        public async Task InsertProductReturnsInsertedProdcut()
        {
            const int resourceId = 1001;
            var productToInsert = new Product()
            {
                Id = 1001,
                Company = "ONE",
                Phone = "111-111-111",
                InStock = true,
                NewStockDate = DateTime.Now,
                Price = 1,
            };

            // Arrange
            A.CallTo(() => fakeProductRepository.UpdateProduct(1001, productToInsert)).Returns(1);

            var productController = CreateProductController();
            // Act
            var response = await productController.Put(resourceId, productToInsert
                ).ConfigureAwait(false);

            // Assert
            OkObjectResult result = response.Result as OkObjectResult;

            result.Should().NotBeNull();

            var product = new Product();
            product = (Product)result.Value;
            product.Id.Should().Be(productToInsert.Id);
            product.Should().BeOfType<Product>();
        }
    }
}