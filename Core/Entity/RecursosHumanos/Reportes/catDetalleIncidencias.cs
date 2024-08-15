using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reportes
{
    public class catDetalleIncidencias
    {
        public int clave { get; set; }
        public string Nombre { get; set; }
        public string cc { get; set; }
        public string descripcion { get; set; }
        public string incidencia { get; set; }
        public int anio { get; set; }
        public int periodo { get; set; }
        public int tipo_nomina { get; set; }
        public string strTipoNomina { get; set; }
        public int dia { get; set; }
        public string fecha { get; set; }
    }
}
