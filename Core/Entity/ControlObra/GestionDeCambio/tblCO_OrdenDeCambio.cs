using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra.GestionDeCambio
{
    public class tblCO_OrdenDeCambio
    {
        public int id { get; set; }
        public DateTime fechaEfectiva { get; set; }
        public string Proyecto { get; set; }
        public string CLiente { get; set; }
        public string Contratista { get; set; }
        public string Direccion { get; set; }
        public string NoOrden { get; set; }
        public bool esCobrable { get; set; }
        public string cc { get; set; }
        public string Antecedentes { get; set; }
        public int idSubContratista { get; set; }
        public int status { get; set; }
        public bool voboPMO { get; set; }
        public int idContrato { get; set; }
        public string ubicacionProyecto { get; set; }
        public string otrasCondicioes { get; set; }
        public DateTime? fechaVobo1 { get; set; }
        public DateTime? fechaVobo2 { get; set; }
        public string nombreDelArchivo { get; set; }
        public string representanteLegal { get; set; }
        public DateTime? fechaAmpliacion { get; set; }
        public bool esValidada { get; set; }
        public bool? archivoValidado { get; set; }
    }
}
