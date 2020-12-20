using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace urlshortener
{
    public class Url
    {
        public string LongUrl { get; set; }
    }
    public class PersistUrl
    {
        public string LongUrl { get; set; }
        public string ShortUlr { get; set; }
        public string Identifier { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }
    public class Response
    {
        public string ShortUrl { get; set; }
        public string Identifier { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}
