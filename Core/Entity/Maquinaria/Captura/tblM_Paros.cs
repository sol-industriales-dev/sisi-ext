using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_Paros
    {
        public tblM_Paros()
        {
            id = 0;
            id_maquina = 0;
            fecha_paro = DateTime.Now;
            descripcion = "";
        }
        public int id { get; set; }
        public int id_maquina { get; set; }
        public DateTime fecha_paro { get; set; }
        public string descripcion { get; set; }
    }
}
