using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO
{
    public class SwitchClass<T> : Dictionary<object, Func<T>>
    {
        public T Execute(object key)
        {
            if (this.ContainsKey(key)) return this[key]();
            else return default(T);
        }
    }
}
