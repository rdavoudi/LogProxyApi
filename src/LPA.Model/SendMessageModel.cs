using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPA.Model
{
    public class SendMessageModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

    }
}
