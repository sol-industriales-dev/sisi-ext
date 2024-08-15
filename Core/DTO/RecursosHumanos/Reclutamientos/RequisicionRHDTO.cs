using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class RequisicionRHDTO
    {
        public int id { get; set; }
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public int puesto { get; set; }
        public string puestoDesc { get; set; }
        public int jefe_inmediato { get; set; }
        public string jefe_inmediatoDesc { get; set; }
        public DateTime fecha_contratacion { get; set; }
        public int tipo_contrato { get; set; }
        public string tipo_contratoDesc { get; set; }
        public int id_plantilla { get; set; }
        public bool puesto_sindicalizado { get; set; }
        public int solicitados { get; set; }
        public int faltantes { get; set; }
        public int puestoTipoNom { get; set; }
        public string comentarioRechazo { get; set; }
        public string estatus { get; set; }
        public int idTabuladorDet { get; set; }
        public int? idCategoria { get; set; }
        public string descCategoria { get; set; }
        public List<int> lstID_Requisiciones { get; set; }
    }
}
