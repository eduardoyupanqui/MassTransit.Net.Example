﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.EventHandling
{
    public class MasstransitConfig
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int PrefetchCount { get; set; }
    }
}
