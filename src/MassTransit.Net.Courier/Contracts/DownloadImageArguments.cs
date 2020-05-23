using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.Courier.Contracts
{
    public class DownloadImageArguments
    {
        public string WorkPath { get; set; }
        public string ImageUri { get; set; }
    }
}
