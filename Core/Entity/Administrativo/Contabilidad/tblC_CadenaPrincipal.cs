using Core.Enum.Administracion.CadenaProductiva;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad
{
    public class tblC_CadenaPrincipal
    {
        public int id { get; set; }
        public string centro_costos { get; set; }
        public string nombCC { get; set; }
        public string numProveedor { get; set; }
        public string proveedor { get; set; }
        public decimal total { get; set; }
        public bool estatus { get; set; }
        public string numNafin { get; set; }
        public string factoraje { get; set; }
        public string banco { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public bool pagado { get; set; }
        public EstadoAutorizacionCadenaEnum estadoAutorizacion { get; set; }
        public string comentarioRechazo { get; set; }
        public string firma { get; set; }
        public string firmaVoBo { get; set; }
        public string firmaValidado { get; set; }

    }
}
