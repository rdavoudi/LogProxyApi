using DNTCommon.Web.Core;
using LPA.DataLayer.Context;
using LPA.Model;
using LPA.Model.ThirdParty;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LPA.Service
{
    public interface IClientService
    {
        Task<SendMessageResponse> SendMessage(SendMessageModel model);

        Task<SendMessageResponse> SaveMessage(SendMessageModel message);

        bool CheckTocken(string token);

        Task<GetMessagesResponse> FetchMessages(int maxRecord);

        Task<GetMessagesResponse> GetMessages(int maxRecord);
    }

    public class ClientService : IClientService
    {
        private readonly ICommonHttpClientFactory _httpClientFactory;
        private readonly Uri thirdPartyHost;
        private readonly IOptionsSnapshot<ApiSettings> _apiOptions;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Message> _message;

        public ClientService(ICommonHttpClientFactory httpClientFactory,
            IOptionsSnapshot<ApiSettings> apiOptions,
            IUnitOfWork uow)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(_httpClientFactory));
            _apiOptions = apiOptions ?? throw new ArgumentNullException(nameof(_apiOptions));
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));

            _message = _uow.Set<Message>();
            thirdPartyHost = new Uri(_apiOptions.Value.ThirdPartyApiOptions.BaseURI);
        }

        private T DeserializeJson<T>(string json)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,

            };
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        private async Task<T> GetAsync<T>(string uri, int maxRecord)
        {
            IDictionary<string, string> headerParam = new Dictionary<string, string>();
            headerParam.Add("API_KEY", _apiOptions.Value.ThirdPartyApiOptions.ApiKey);

            var client = _httpClientFactory.GetOrCreate(thirdPartyHost, headerParam);

            var completeUri = string.Concat(uri, "?maxRecord=" + maxRecord);
            var responseMessage = await client.GetAsync(completeUri);
            return DeserializeJson<T>(await responseMessage.Content.ReadAsStringAsync());
        }

        public async Task<GetMessagesResponse> GetMessages(int maxRecord)
        {
            return await GetAsync<GetMessagesResponse>(_apiOptions.Value.ThirdPartyApiOptions.BaseMethod, maxRecord);
        }

        public async Task<SendMessageResponse> SendMessage(SendMessageModel model)
        {
            return await PostAsync<SendMessageModel>(_apiOptions.Value.ThirdPartyApiOptions.BaseMethod, model);
        }

        private async Task<SendMessageResponse> PostAsync<T>(string uri, T model)
        {
            IDictionary<string, string> headerParam = new Dictionary<string, string>();
            headerParam.Add("API_KEY", _apiOptions.Value.ThirdPartyApiOptions.ApiKey);
            var httpClient = _httpClientFactory.GetOrCreate(thirdPartyHost, headerParam);

            var myContent = JsonConvert.SerializeObject(model);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var responseMessage = await httpClient.PostAsync(_apiOptions.Value.ThirdPartyApiOptions.BaseMethod, byteContent);

            return DeserializeJson<SendMessageResponse>(await responseMessage.Content.ReadAsStringAsync());

        }

        public bool CheckTocken(string token)
        {
            return token == _apiOptions.Value.ThirdPartyApiOptions.ApiKey;
        }

        public async Task<GetMessagesResponse> FetchMessages(int maxRecord)
        {
            if (maxRecord > 0)
            {
                var query = await _message
                    .AsNoTracking()
                    .OrderByDescending(x=> x.Id)
                    .Take(maxRecord)
                    .Select(x => new MessageModel()
                    {
                        Id = "",
                        Fields = new Fields()
                        {
                            Id = x.Id,
                            Message = x.Title,
                            Summary = x.Text,
                            ReceivedAt = x.ReceivedAt.ToLocalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffZ", CultureInfo.InvariantCulture),
                        },

                    })
                    .ToListAsync();

                return new GetMessagesResponse(200, "Fetching messages successfully", query);
            }
            else
            {
                return new GetMessagesResponse("maxRecord must be greater than zero");
            }
        }

        public async Task<SendMessageResponse> SaveMessage(SendMessageModel message)
        {
            var newMessage = new Message()
            {
                Title = message.Title,
                Text = message.Text

            };

            _message.Add(newMessage);

            if (await _uow.SaveChangesAsync() > 0)
                return new SendMessageResponse(message);
            else
                return new SendMessageResponse("Sending Message Failed.");
        }
    }
}
