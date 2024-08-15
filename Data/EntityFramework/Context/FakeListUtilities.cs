using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.EntityFramework.Context
{
    public class FakeListUtilities<T> : List<T>
    {
        public ICollection<T> lst { get; set; }
        /// <summary>
        /// Funcion de escape para los contextos where().ToObject()
        /// </summary>
        /// <returns>El resultado de la consulta sin cambios</returns>
        public dynamic ToObject<T>() where T : class
        {
            var tipo = typeof(T).GetProperty("Item").PropertyType.BaseType;
            var res = this.lst.Select(s => ((IDictionary<string, object>)s).Values)
                .Select(s => (s as object)).Cast<T>();
            return res;
        }
    }
}
