using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Requerimientos
{
    public class tblNOM_Asignacion
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int normaID { get; set; }
        public DateTime fechaAsignacion { get; set; }
        public DateTime fechaInicioEvaluacion { get; set; }
        public bool estatus { get; set; }
        public int idEmpresa { get; set; }
        public int idAgrupacion { get; set; }
    }
}
