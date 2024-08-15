using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_CapStandBy
    {
        public int id { get; set; }
        public int usuarioID { get; set; }
        public DateTime FechaCaptura { get; set; }
        public string CC { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int UsuarioElabora { get; set; }
        public int UsuarioGerente { get; set; }
        public int estatus { get; set; }
        public string folio { get; set; }
    }
}
