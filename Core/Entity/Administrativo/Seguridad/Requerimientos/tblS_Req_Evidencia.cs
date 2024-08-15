using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Requerimientos
{
    public class tblS_Req_Evidencia
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int requerimientoID { get; set; }
        public int puntoID { get; set; }
        public DateTime fechaPunto { get; set; }
        public string rutaEvidencia { get; set; }
        public string comentariosCaptura { get; set; }
        public DateTime fechaCaptura { get; set; }
        public int usuarioCapturaID { get; set; }
        public int usuarioEvaluadorID { get; set; }
        public string comentariosEvaluador { get; set; }
        public bool aprobado { get; set; }
        public decimal calificacion { get; set; }
        public DateTime? fechaEvaluacion { get; set; }
        public int division { get; set; }
        public bool estatus { get; set; }
        public int idEmpresa { get; set; }
        public int? idAgrupacion { get; set; }
    }
}
