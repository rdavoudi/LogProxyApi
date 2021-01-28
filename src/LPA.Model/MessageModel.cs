using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPA.Model
{
    public class MessageModel
    {

        public MessageModel()
        {
            CreatedTime = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss.fffZ", CultureInfo.InvariantCulture);
        }

        public string Id { get; set; }

        public Fields Fields { get; set; }

        public string CreatedTime { get; set; }

    }

    public class Fields
    {
        public int Id { get; set; }
        public string Summary { get; set; }

        public string Message { get; set; }

        public string ReceivedAt { get; set; }

    }
}
