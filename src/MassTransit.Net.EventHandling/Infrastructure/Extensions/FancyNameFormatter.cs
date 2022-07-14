using MassTransit.Transports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.EventHandling.Infrastructure.Extensions
{
    internal class FancyNameFormatter<T> : IMessageEntityNameFormatter<T> where T : class
    {
        public string FormatEntityName()
        {
            // seriously, please don't do this, like, ever.
            return typeof(T).Name.ToString();
        }
    }

    internal class FancyNameFormatter :
        IEntityNameFormatter
    {
        readonly IEntityNameFormatter _original;
        public FancyNameFormatter(IEntityNameFormatter original)
        {
            _original = original;
        }

        public string FormatEntityName<T>()
        {
            return GetMessageName(typeof(T));
            //return _original.FormatEntityName<T>();
        }
        protected virtual string SanitizeName(string name)
        {
            return name;
            //return name.ToLowerInvariant();
        }

        string GetMessageName(Type type)
        {
            if (type.IsGenericType)
                return SanitizeName(type.GetGenericArguments()[0].Name);

            var messageName = type.Name;

            return SanitizeName(messageName);
        }
    }

    public class MessageNameFormatterEntityNameFormatter :
        IEntityNameFormatter
    {
        readonly IMessageNameFormatter _formatter;

        public MessageNameFormatterEntityNameFormatter(IMessageNameFormatter formatter)
        {
            _formatter = formatter;
        }

        string IEntityNameFormatter.FormatEntityName<T>()
        {
            return _formatter.GetMessageName(typeof(T)).ToString();
        }
    }
}
