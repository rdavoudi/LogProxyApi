using System;
using System.Collections.Generic;
using System.Text;

namespace LPA.Model.ThirdParty
{
    public class Message
    {
        public Message()
        {
            ReceivedAt = DateTime.UtcNow;
        }
        public virtual int Id { get; set; }

        public virtual string Title { get; set; }

        public virtual string Text { get; set; }

        public virtual DateTimeOffset ReceivedAt { get; set; }
    }
}
