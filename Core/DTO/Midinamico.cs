using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO
{
    public class Midinamico : DynamicObject
    {
        public Midinamico()
        {

        }
        public Object Value {  get; set; }
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = Value.GetType().InvokeMember(
                binder.Name,
                BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                Value,
                null);

            // Always return true, since InvokeMember would have thrown if something went wrong
            return true;
        }
    }
}
