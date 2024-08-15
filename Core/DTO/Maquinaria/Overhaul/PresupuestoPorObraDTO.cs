using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class PresupuestoPorObraDTO
    {
        public int presupuestoID { get; set; }
        public int modeloID { get; set; }
        public string modelo { get; set; }
        public string obraID { get; set; }
        public string obra { get; set; }
        public int anio { get; set; }
        public decimal costo { get; set; }
        public int estado { get; set; }
        public int numComponentes { get; set; }
        public decimal avance { get; set; }
        public int numCompAvance { get; set; }
        public decimal avanceNoPro { get; set; }
        public int numCompAvanceNoPro { get; set; }
        public string fecha { get; set; }
        public decimal presupuesto { get; set; }
        public int ComponenteID { get; set; }
        public string DescripcionModelo { get; set; }
        public string Descripcion { get; set; }
        public string cc { get; set; }
        public decimal avanceErogado { get; set; }
        public decimal bolsaRestante { get; set; }
    }
}

