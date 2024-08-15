using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Sistemas
{
    public class tblP_DirArchivos
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string dirFisico { get; set; }
        public string dirVirtual { get; set; }
    }
}
