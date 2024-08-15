using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.DocumentosXPagar.Especiales
{
    public class tblAF_DxP_CCContrato
    {
        public int id { get; set; }
        public int contratoID { get; set; }
        public string cc { get; set; }
    }
}
