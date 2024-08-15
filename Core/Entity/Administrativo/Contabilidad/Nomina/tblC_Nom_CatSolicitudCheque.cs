using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_CatSolicitudCheque
    {
        public int id { get; set; } 
        public int tipoSolicitudCheque { get; set; } 
        public string tipoSolicitudChequeDescripcion { get; set; } 
        public int banco { get; set; } 
        public string bancoDescripcion { get; set; } 
        public string cc { get; set; } 
        public int tipoNomina { get; set; } 
        public DateTime fechaRegistro { get; set; }
        public bool registroActivo { get; set; } 
    }
}
