using System;
using System.Linq;

//Keep As:RMC.Mini
namespace RMC.Mini
{
    public class Locator : IDisposable
    {
        public static Type GetLowestType(Type type)
        {
            return GetTypeHierarchy(type).LastOrDefault();
        }
        
        private static Type[] GetTypeHierarchy(Type type)
        {
            var hierarchy = new System.Collections.Generic.List<Type>();

            while (type != null)
            {
                hierarchy.Insert(0, type);  // Insert at the beginning to maintain order
                type = type.BaseType;
            }

            return hierarchy.ToArray();
        }

        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}