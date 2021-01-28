using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPA.Model;
using LPA.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LPA.Common;


namespace LPA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ILogger<MessagesController> _logger;
        private readonly IClientService _client;
        public MessagesController(
            ILogger<MessagesController> logger,
            IClientService client
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
            _client = client ?? throw new ArgumentNullException(nameof(_client));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SendMessageModel model) 
        {
            if (ModelState.IsValid)
            {
                var result = await _client.SendMessage(model); 

                return Ok(result);
            }

            return Ok(new SendMessageResponse(string.Join(",", ModelState.GetErrorMessages())));
        }

        public async Task<IActionResult> Get(int maxRecord)
        {
            var result =await _client.GetMessages(maxRecord);
            return Ok(result);
        }
    }
}
