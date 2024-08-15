using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Entity.ControlObra.GestionDeCambio
{
    public class tblCO_OC_Firmas
    {
        #region SQL
        public int id { get; set; }
        public int idOrdenDeCambio { get; set; }
        public string firma { get; set; }
        public int idFirma { get; set; }
        public string firmaDigital { get; set; }
        public int idRow { get; set; }
        public bool Autorizando { get; set; }
        public DateTime? fechaAutorizacion { get; set; }
        public bool Estado { get; set; }
        public int estatusFirma { get; set; }
        public string comentarioRechazo { get; set; }
        public string ubicacionArchivoFirmado { get; set; }
        public string nombreFirmante { get; set; }
        public string puestoFirmante { get; set; }
        #endregion

        [NotMapped]
        public string puesto { get; set; }
    }
}
