using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Encuestas
{
    public class tblEN_ResultadoSubContratistasDet
    {
        public int id { get; set; }
        public string nombreSubContratista { get; set; }
        public int numSubContratista { get; set; }
        public string noContrato { get; set; }
        public string descripcionServicio { get; set; }
        public string nombreProyecto { get; set; }
        public int evaluador { get; set; }
        public string centroCostos { get; set; }
        public string centroCostosNombre { get; set; }
        public string comentarios { get; set; }
        public bool estadoEncuesta { get; set; }
        public int encuestaID { get; set; }
        public int convenioID { get; set; }
        public decimal? calificacion { get; set; }

        [ForeignKey("evaluador")]
        public virtual tblP_Usuario usuario { get; set; }
     //   public virtual tblEN_EncuestaSubContratista encuesta { get; set; }

        [ForeignKey("encuestaFolioID")]
        public virtual List<tblEN_ResultadoSubContratistas> detalles { get; set; }

        //public tblEN_ResultadoSubContratistasDet()
        //{
        //    detalles = new List<tblEN_ResultadoSubContratistas>();
        //}
    }
}
