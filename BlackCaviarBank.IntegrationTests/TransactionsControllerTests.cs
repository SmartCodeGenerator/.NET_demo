using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.IntegrationTests.WebAppFactories;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlackCaviarBank.IntegrationTests
{
    public class TransactionsControllerTests : IClassFixture<BCBWebAppFactory1<Startup>>
    {
        private readonly HttpClient client;

        public TransactionsControllerTests(BCBWebAppFactory1<Startup> factory)
        {
            client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            "Test", options => { });
                    services.AddAuthorization(options =>
                    {
                        options.AddPolicy("<Existing Policy Name>", builder =>
                        {
                            builder.AuthenticationSchemes.Add("Test");
                            builder.RequireAuthenticatedUser();
                        });
                        options.DefaultPolicy = options.GetPolicy("<Existing Policy Name>");
                    });
                });
            }).CreateClient();
        }

        [Fact]
        public async Task GetTransactions_SuccessStatusCode_ReturnsPaginatedListWithCorrectReferences()
        {
            var response = await client.GetAsync("/api/Transactions?pageNumber=2&pageSize=2");

            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();

            Assert.DoesNotContain("D4F89BE4-07D6-4A70-AE35-08D809F70F4B".ToLower(), stringContent);
            Assert.DoesNotContain("0A7A4E01-3C81-47B7-AE37-08D809F70F4B".ToLower(), stringContent);
            Assert.Contains("91E0C512-5A0D-409F-AE38-08D809F70F4B".ToLower(), stringContent);
            Assert.DoesNotContain("35FDCDD7-DEA3-4732-E026-08D80A08BC78".ToLower(), stringContent);

            Assert.NotEmpty(response.Headers.GetValues("X-Pagination"));
        }

        [Fact]
        public async Task GetTransactions_SuccessStatusCode_ReturnsNoDataWhenNoPaginationParams()
        {
            var response = await client.GetAsync("/api/Transactions");

            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();

            Assert.Equal("[]", stringContent);
        }

        [Fact]
        public async Task GetTransaction_SuccessStatusCode_ReturnsTransactionObjectWhenRecordFound()
        {
            var response = await client.GetAsync("/api/Transactions/0A7A4E01-3C81-47B7-AE37-08D809F70F4B");

            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();

            Assert.Contains("2629275250356232", stringContent);
            Assert.Contains("5700396801463145", stringContent);
            Assert.Contains("\"amount\":700.0", stringContent);
        }

        [Fact]
        public async Task GetTransaction_SuccessStatusCode_ReturnsNoObjectWhenRecordNotFound()
        {
            var response = await client.GetAsync("/api/Transactions/735E586E-E37C-4C9A-88D6-8C0AFDA626C3");

            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();

            Assert.Equal("", stringContent);
        }

        [Fact]
        public async Task MakeTransaction_SuccessStatusCode_CreatedNewRecordWhenModelStateValid()
        {
            var response = await client.PostAsync("/api/Transactions/MakeTransaction",
                new StringContent(JsonConvert.SerializeObject(new TransactionDTO { From = "55722816549004828545",
                    To = "41618252404814018878", Amount = 100 }), Encoding.Default, "application/json"));

            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();

            Assert.Contains("55722816549004828545", stringContent);
            Assert.Contains("41618252404814018878", stringContent);
            Assert.Contains("\"amount\":100.0", stringContent);

            var getResponse = await client.GetAsync("/api/Transactions?pageNumber=1&pageSize=5");

            getResponse.EnsureSuccessStatusCode();

            var getStringContent = await getResponse.Content.ReadAsStringAsync();

            int index = getStringContent.LastIndexOf("transactionId");
            var guid = getStringContent.Substring(index + 16, 36);

            var deleteResponse = await client.DeleteAsync("/api/Transactions/" + guid.ToUpper());

            deleteResponse.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task MakeTransaction_BadRequestStatusCode_CreatedNothingWhenIncorrectParams()
        {
            var response = await client.PostAsync("/api/Transactions/MakeTransaction", new StringContent(JsonConvert.SerializeObject(new TransactionDTO { From = "", To = "", Amount = 100 }), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RollbackTransaction_SuccessStatusCode_DeletedObjectWhenRecordFound()
        {
            var response = await client.PostAsync("/api/Transactions/MakeTransaction", new StringContent(JsonConvert.SerializeObject(new TransactionDTO { From = "55722816549004828545", To = "41618252404814018878", Amount = 100 }), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            var getResponse = await client.GetAsync("/api/Transactions?pageNumber=1&pageSize=5");

            getResponse.EnsureSuccessStatusCode();

            var getStringContent = await getResponse.Content.ReadAsStringAsync();

            int index = getStringContent.LastIndexOf("transactionId");
            var guid = getStringContent.Substring(index + 16, 36);

            var deleteResponse = await client.DeleteAsync("/api/Transactions/" + guid.ToUpper());

            deleteResponse.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task RollbackTransaction_ThrowsNullReferenceException_DeletedNothingWhenRecordNotFound()
        {
            var response = await client.PostAsync("/api/Transactions/MakeTransaction", new StringContent(JsonConvert.SerializeObject(new TransactionDTO { From = "55722816549004828545", To = "41618252404814018878", Amount = 100 }), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            var getResponse = await client.GetAsync("/api/Transactions?pageNumber=1&pageSize=5");

            getResponse.EnsureSuccessStatusCode();

            var getStringContent = await getResponse.Content.ReadAsStringAsync();

            int index = getStringContent.LastIndexOf("transactionId");
            var guid = getStringContent.Substring(index + 16, 36);

            await Assert.ThrowsAsync(new NullReferenceException().GetType(), async () => await client.DeleteAsync("/api/Transactions/D79C0E54-0267-4CEA-8B6D-6B7A0AEA30FC"));

            var responseNoDelete = await client.GetAsync("/api/Transactions/" + guid.ToUpper());

            responseNoDelete.EnsureSuccessStatusCode();

            var stringContentNoDelete = await responseNoDelete.Content.ReadAsStringAsync();

            Assert.Contains("55722816549004828545", stringContentNoDelete);
            Assert.Contains("41618252404814018878", stringContentNoDelete);
            Assert.Contains("\"amount\":100.0", stringContentNoDelete);

            var deleteResponse = await client.DeleteAsync("/api/Transactions/" + guid.ToUpper());

            deleteResponse.EnsureSuccessStatusCode();
        }
    }
}
