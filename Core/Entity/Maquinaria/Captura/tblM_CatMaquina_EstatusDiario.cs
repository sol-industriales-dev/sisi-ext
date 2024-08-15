using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_CatMaquina_EstatusDiario
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int estatus { get; set; }
        public DateTime fecha { get; set; }
        public int usuario { get; set; }
        public int cantActivos { get; set; }
        public int cantInactivos { get; set; }
        public decimal porActivos { get; set; }
        public decimal porInactivos { get; set; }
        
    }
}
