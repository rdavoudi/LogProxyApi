using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPA.Model
{
    public class GetMessagesResponse : BaseResponse
    {
        
        public ICollection<MessageModel> Records { get; private set; }

        [JsonConstructor]
        public GetMessagesResponse(int code, string message, ICollection<MessageModel> records) : base(code, message)
        {
            Records = records;
        }

        public GetMessagesResponse(ICollection<MessageModel> records) : this(200, "Sending Message Successfully", records)
        {

        }

        public GetMessagesResponse(string message) : this(500, message, null)
        {

        }
    }
}
