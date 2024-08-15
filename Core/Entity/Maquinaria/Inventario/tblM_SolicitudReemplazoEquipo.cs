using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_SolicitudReemplazoEquipo
    {
        public int id { get; set; }
        public string folio { get; set; }
        public string CC { get; set; }
        public DateTime fechaElaboracion { get; set; }
        public int usuarioID { get; set; }
        public string descripcion { get; set; }
        public bool Estatus { get; set; }

       
    }
}
