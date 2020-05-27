using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MassTransit.Net.Jobs.Client
{
    public static class StringExtensions
    {
        public static string ToUnderscoreCase(this string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }

        public static string ToConcatHost(this string queue, string host)
        {
            return string.IsNullOrEmpty(host) ?
                queue :
                host + "_" + queue;
        }
    }
}
