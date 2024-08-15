using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_BN_Evaluacion
    {
        public int id { get; set; }
        public int plantillaID { get; set; }
        public virtual tblRH_BN_Plantilla plantilla { get; set; }
        public DateTime fecha { get; set; }
        public int estatus { get; set; }
        public int usuarioEvaluoID { get; set; }
        public virtual tblP_Usuario usuarioEvaluo { get; set; }
        public string cc { get; set; }
        public int tipoNomina { get; set; }
        public int anio { get; set; }
        public int periodo { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public virtual ICollection<tblRH_BN_Evaluacion_Det> listDetalle { get; set; }
        public virtual ICollection<tblRH_BN_Evaluacion_Aut> listAutorizadores { get; set; }
        public bool aplicado { get; set; }
        public DateTime fechaAplicacion { get; set; }
        public int? idIncidencia { get; set; }
    }
}
