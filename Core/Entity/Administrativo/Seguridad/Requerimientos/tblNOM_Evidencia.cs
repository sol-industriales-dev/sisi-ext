using Core.Enum.Administracion.Seguridad.Requerimientos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Requerimientos
{
    public class tblNOM_Evidencia
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int normaID { get; set; }
        public DateTime fechaNorma { get; set; }
        public string rutaEvidencia { get; set; }
        public string comentariosCaptura { get; set; }
        public DateTime fechaCaptura { get; set; }
        public int usuarioCapturaID { get; set; }
        public int usuarioEvaluadorID { get; set; }
        public string comentariosEvaluador { get; set; }
        public bool aprobado { get; set; }
        public decimal calificacion { get; set; }
        public DateTime? fechaEvaluacion { get; set; }
        public bool estatus { get; set; }
    }
}
