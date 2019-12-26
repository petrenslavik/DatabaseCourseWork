using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class FileMessage:Message
    {
        public string FileName { get; set; }
        public string Size { get; set; }

        public string FileUri { get; set; }
    }
}
