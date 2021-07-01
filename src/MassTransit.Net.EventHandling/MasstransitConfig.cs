using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.EventHandling
{
    public class MasstransitConfig
    {
        public string Region { get; set; }
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Scope { get; set; }
        public int PrefetchCount { get; set; }
    }
}
