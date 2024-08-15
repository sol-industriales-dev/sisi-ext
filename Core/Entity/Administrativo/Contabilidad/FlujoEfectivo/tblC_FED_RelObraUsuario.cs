using Core.Enum.Administracion.FlujoEfectivo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.FlujoEfectivo
{
    public class tblC_FED_RelObraUsuario
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public string obra { get; set; }
        public tipoObraUsuarioEnum tipo { get; set; }
    }
}
