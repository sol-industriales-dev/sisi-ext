using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class rptTiemposEntreAutorizaciones
    {
        public string folio { get; set; }
        public string FechaAdministrador { get; set; }
        public string FechaGerente { get; set; }
        public string DirectorArea { get; set; }
        public string DirectorDivision { get; set; }
        public string DirectorGeneral { get; set; }
        public string Asigno { get; set; }
        public string TiempoTotal { get; set; }
    }
}
