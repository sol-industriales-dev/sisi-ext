using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Overhaul;

namespace Core.Entity.Maquinaria.Overhaul
{
    public class tblM_CalendarioPlaneacionOverhaul
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public int tipo { get; set; }
        public int estatus { get; set; }
        public DateTime fecha { get; set; }
        public int grupoMaquinaID { get; set; }
        public int modeloMaquinaID { get; set; }
        public string obraID { get; set; }
        public string subConjuntoID { get; set; }
        public int anio { get; set; }
    }
}

