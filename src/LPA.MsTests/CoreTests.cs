using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;
using LPA.Model;
using LPA.Ioc;
using LPA.DataLayer.Context;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace LPA.MsTests
{
    [TestClass]
    public class CoreTests
    {
        
        [TestMethod]
        public async Task Test_Send_Message()
        {
            var client = TestsHttpClient.Instance;
            var message= await SendMessage(client, "API_KEY","title1","text1");

            message.Should().NotBeNull();
            message.Message.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public async Task Wrong_API_KEY()
        {
            var client = TestsHttpClient.Instance;
            var message = await SendMessage(client, "Wrong API_KEY", "title1", "text1");

            message.Should().NotBeNull();
            message.Message.Should().Be("The Token is wrong");
            message.Code.Should().Be(500);
        }


        [TestMethod]
        public async Task Test_Get_Messages()
        {
            var client = TestsHttpClient.Instance;
            var messages = await GetMessages(client,5);

            messages.Should().NotBeNull();
            messages.Records.Count.Should().Be(5);
        }

        [TestMethod]
        public async Task Test_Get_Messages_With_Zero_parameter()
        {
            var client = TestsHttpClient.Instance;
            var messages = await GetMessages(client, 0);

            messages.Should().NotBeNull();
            messages.Records.Should().BeNull();
            messages.Message.Should().Be("maxRecord must be greater than zero");
            messages.Code.Should().Be(500);
        }



        private static async Task<GetMessagesResponse> GetMessages(HttpClient client, int maxRecord)
        {
            const string loginUrl = "/api/messages";

            client.DefaultRequestHeaders.Add("API_KEY", "API_KEY");

            var uri = string.Concat(loginUrl, "?maxRecord=", maxRecord);
            var response = await client.GetAsync(uri);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().NotBeNullOrEmpty();
            return JsonConvert.DeserializeObject<GetMessagesResponse>(responseString);
        }

        private static async Task<SendMessageResponse> SendMessage(HttpClient client, string api_key, string title, string text)
        {
            const string loginUrl = "/api/messages";
            var message = new { Title = title, Text = text };
            
            client.DefaultRequestHeaders.Add("API_KEY", api_key);

            var content = JsonConvert.SerializeObject(message);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync(loginUrl,byteContent);
            
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().NotBeNullOrEmpty();
            return JsonConvert.DeserializeObject<SendMessageResponse>(responseString);
        }


    }
}