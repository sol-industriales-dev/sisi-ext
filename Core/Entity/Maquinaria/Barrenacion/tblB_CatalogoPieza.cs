using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Maquinaria.Barrenacion;

namespace Core.Entity.Maquinaria.Barrenacion
{
    public class tblB_CatalogoPieza
    {
        public int id { get; set; }
        public TipoPiezaEnum tipoPieza { get; set; }
        public TipoBrocaEnum tipoBroca { get; set; }
        public int insumo { get; set; }
        public string descripcion { get; set; }
        public bool activo { get; set; }
        public DateTime fechaAlta { get; set; }
        public int usuarioCreadorID { get; set; }
        public int incremento { get; set; }
        public string areaCuenta { get; set; }
    }
}
