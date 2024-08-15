using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Proyecciones
{
   public class tblPro_CierreObra
    {
        public int id { get; set; }

        public int  capturadeObrasID { get; set; }
        public int registroID { get; set; }
        public DateTime fecha { get; set; }
        public string DatosEconomicos { get; set; }
        public string CuantoCotizo { get; set; }
        public string MontoUtilidad { get; set; }
        public int CantidadPersonal { get; set; }
        public string Margen { get; set; }
        public string AnticipoMonto { get; set; }
        public string Porcentaje { get; set; }
        public string Retenciones { get; set; }
        public string Bien { get; set; }
        public string Mal { get; set; }
        public string Contactos { get; set; }
        public string Comentarios { get; set; }
        public string Cliente { get; set; }
   }
}
