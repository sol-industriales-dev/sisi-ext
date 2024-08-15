using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Overhaul
{
    public class tblM_CatTipoServicioOverhaul
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public int grupoMaquinaID { get; set; }
        public int modeloMaquinaID { get; set; }
        public bool estatus { get; set; }
        public bool planeacion { get; set; }
    }
}
