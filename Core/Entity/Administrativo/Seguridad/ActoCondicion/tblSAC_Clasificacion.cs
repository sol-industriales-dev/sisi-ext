using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Administracion.Seguridad.ActoCondicion;

namespace Core.Entity.Administrativo.Seguridad.ActoCondicion
{
    public class tblSAC_Clasificacion
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public TipoRiesgo tipoRiesgo { get; set; }
    }
}
