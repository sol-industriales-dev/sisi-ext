using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.DocumentosXPagar
{
    public class tblAF_DxP_Divisiones_Proyecto
    {
        public int id { get; set; }
        public int divisionID { get; set; }
        public string cc { get; set; }
        public bool isAdmin { get; set; }
        public bool esActivo { get; set; }
    }
}
