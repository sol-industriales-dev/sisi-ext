using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.MedioAmbiente
{
    public class AspectosAmbientalesToDestinoFinalDTO
    {
        public int id { get; set; }
        public int idAgrupacion { get; set; }
        public DateTime fechaDestinoFinal { get; set; }
        public int idTransportistaDestinoFinal { get; set; }
        public int idArchivoDestinoFinal { get; set; }
        public string codigoContenedor { get; set; }
        public int idAspectoAmbiental { get; set; }
        public string aspectoAmbiental { get; set; }
        public bool sePesaAspectoAmbiental { get; set; }
        public string agrupacionRP { get; set; }
        public bool esSolido { get; set; }
        public decimal cantidad { get; set; }
        public string idRP { get; set; }
        public bool quitarDelRow { get; set; }
        public List<string> lstAspectosAmbientales { get; set; }
        public List<decimal> lstCantidad { get; set; }
        public string aaID { get; set; }
        public int clasificacion { get; set; }
        public int idCaptura { get; set; }
    }
}
