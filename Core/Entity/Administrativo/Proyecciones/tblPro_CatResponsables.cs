using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Proyecciones
{
    public class tblPro_CatResponsables
    {
        public int id { get; set; }
        public int responsableID { get; set; }
        public string Descripcion { get; set; }
        public string Color { get; set; }
        public string Abreviatura { get; set; }
    }
}
