using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ComLib.Extension
{
    public static class EntityFrameworkExtension
    {
        // TODO: Low efficiency.
        public static IEnumerable<T> Remove<T>(this DbSet<T> param, Func<T, bool> predicate) where T:class
        {
            IEnumerable<T> cands = param.Where(predicate);
            foreach(T c in cands)
            {
                param.Remove(c);
            }
            return cands;
        }

        public static IEnumerable<T> RemoveAll<T>(this DbSet<T> param) where T:class
        {
            param.Each(c=>param.Remove(c));
            return param;
        }
    }
}
