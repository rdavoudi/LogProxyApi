using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPA.Model
{
    public class SendMessageResponse : BaseResponse
    {
        
        public SendMessageModel SendMessage { get; private set; }

        [JsonConstructor]
        public SendMessageResponse(int code, string message, SendMessageModel sendMessage) : base(code, message)
        {
            SendMessage = sendMessage;
        }

        public SendMessageResponse(SendMessageModel sendMessage) : this(200, "Sending Message Successfully", sendMessage)
        {

        }

        public SendMessageResponse(string message) : this(500, message, null)
        {

        }
    }
}
