using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad
{
    public class tblS_Vehiculo
    {
        public int id { get; set; }
        public string cc { get; set; }
        public DateTime fecha { get; set; }
        public string economico { get; set; }
        public string decripcion { get; set; }
        public string marcaModelo { get; set; }
        public string placas { get; set; }
        public int tipoEncierro { get; set; }
        public string responsable { get; set; }
        public string puesto { get; set; }
        public string tipoLicencia { get; set; }
        public string numLicencia { get; set; }
        public DateTime vigenciaLicencia { get; set; }
        public decimal kilometraje { get; set; }
        public bool preventivo { get; set; }
        public string requerimientos { get; set; }
        public string notas { get; set; }
    }
}
