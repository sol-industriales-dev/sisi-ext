using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Catalogo;

namespace Core.Entity.Maquinaria
{
    public class tblM_RendimientoMaquina
    {
        public int id { get; set; }
        public int maquinaID { get; set; }        
        public string cc { get; set; }
        public DateTime fecha { get; set; }
        public decimal rendimiento { get; set; }
        public int modelo { get; set; }
        public int estatus { get; set; }        
        public virtual tblM_CatMaquina maquina { get; set; }
    }
}
