using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventFlow.Core;

namespace Vita.Domain.Infrastructure.EventFlow
{
    public static class VitaIdExtentions
    {
        public static Guid ToVitaGuid<T>(this Identity<T> id) where T : Identity<T>
        {
            if (string.IsNullOrEmpty(id?.Value)) return Guid.Empty;

            return id.Value.ToVitaGuid();
        }

        public static Guid ToVitaGuid(this string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException(id);

            var splits = id.Split(Convert.ToChar("-"));

            if (splits.Length > 2)
            {
                var str = $"{splits[1]}{splits[2]}{splits[3]}{splits[4]}{splits[5]}";
                Guid guid = Guid.Empty;
                if (Guid.TryParse(str, out guid))
                {
                    return guid;
                }
            }

            throw new ArgumentException(id);
        }
    }
}
