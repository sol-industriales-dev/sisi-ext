using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_RespuestaResguardoVehiculos
    {
        public int id { get; set; }
        public int ResguardoID { get; set; }
        public int RespuestaID { get; set; }

        public int Bueno { get; set; }
        public int Malo { get; set; }
        public int NA { get; set; }
        public int Regular { get; set; }
        public string Observaciones { get; set; }
        public int HasDocumento { get; set; }

        public int TipoResguardo { get; set; }

    }
}
