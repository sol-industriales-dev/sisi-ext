using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Rentabilidad
{
    public class tblM_KBCorte
    {
        public int id { get; set; }
        public DateTime fechaCorte { get; set; }
        public DateTime fecha { get; set; }
        public int usuarioID { get; set; }
        public int tipo { get; set; }
        public bool guardadoConstruplan { get; set; }
        public bool guardadoArrendadora { get; set; }
        public bool costoEstCerrado { get; set; }
        public int anio { get; set; }
    }
}
