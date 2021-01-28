using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPA.Common;
using LPA.Model;
using LPA.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LPA.ThirdPartyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ILogger<MessagesController> _logger;
        private readonly IClientService _client;
        private readonly IHttpContextAccessor _contextAccessor;
        public MessagesController(
            ILogger<MessagesController> logger,
            IClientService client,
            IHttpContextAccessor contextAccessor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
            _client = client ?? throw new ArgumentNullException(nameof(_client));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(_contextAccessor));
        }

        public async Task<IActionResult> GetMessages(int maxRecord)
        {

            if (!_client.CheckTocken(_contextAccessor.HttpContext.Request.Headers["API_KEY"]))
            {
                return Ok(new GetMessagesResponse("The Token is wrong"));
            }

            var result = await _client.FetchMessages(maxRecord);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SendMessageModel model)
        {
            if (ModelState.IsValid)
            {
                if (!_client.CheckTocken(_contextAccessor.HttpContext.Request.Headers["API_KEY"]))
                {
                    return Ok(new SendMessageResponse("The Token is wrong"));
                }

                var result = await _client.SaveMessage(model);

                if (result.Code == 200)
                    return Ok(result);
            }

            return Ok(new SendMessageResponse(string.Join(",", ModelState.GetErrorMessages())));
        }
    }
}
