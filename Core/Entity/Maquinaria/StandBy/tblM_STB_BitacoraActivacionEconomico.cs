using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.StandBy
{
    public class tblM_STB_BitacoraActivacionEconomico
    {
        public int id { get; set; }
        public int economicoId { get; set; }
        public int motivoActivacionId { get; set; }
        public int usuarioAccionId { get; set; }
        public DateTime fechaAccion { get; set; }
        public string objeto { get; set; }
    }
}
