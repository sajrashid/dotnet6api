using MyAPI.Models;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestProject
{
    public static class AuthSvc
    {

        public static async Task<string> Login(string email, string password, Uri baseUri)
        {

            HttpClient client = new();
            client.BaseAddress = baseUri;
            Auth user = new Auth { Email = "testUser@test.com", Password = "Password99" };

            var json = JsonSerializer.Serialize<Auth>(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var queryParameters = new Dictionary<string, string>
            {
                { "Email", "testUser@test.com" },
                { "Password", "Password99"}
            };
            var dictFormUrlEncoded = new FormUrlEncodedContent(queryParameters);
            var queryString = await dictFormUrlEncoded.ReadAsStringAsync();
            //Act
            var response = await client.GetAsync($"/api/Login/{queryString}");



            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return "";

        }
    }
}
