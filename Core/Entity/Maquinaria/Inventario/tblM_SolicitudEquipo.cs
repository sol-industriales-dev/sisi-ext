using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_SolicitudEquipo
    {
        public int id { get; set; }
        public string folio { get; set; }
        public string CC { get; set; }
        public DateTime fechaElaboracion { get; set; }
        public int usuarioID { get; set; }
        public int cantidad { get; set; }
        public string descripcion { get; set; }
        public bool Estatus { get; set; }
        public decimal HorasTotales { get; set; }
        public bool ArranqueObra { get; set; }
        public bool EstatdoSolicitud { get; set; }
        public string condicionInicial { get; set; }
        public string condicionActual { get; set; }
        public string justificacion { get; set; }
        public string link { get; set; }
    }
}
