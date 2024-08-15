using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.DocumentosXPagar
{
    public class tblAF_DxP_Deuda
    {
        public int Id { get; set; }
        public int ContratoId { get; set; }
        public int Cta { get; set; }
        public int Scta { get; set; }
        public int Sscta { get; set; }
        public int Digito { get; set; }
        public string Descripcion { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
        public bool Estatus { get; set; }
        public DateTime fechaCreacion { get; set; }
        [ForeignKey("ContratoId")]
        public virtual tblAF_DxP_Contrato Contrato { get; set; }
        public string cc { get; set; }
        public string area { get; set; }
        public string cuenta { get; set; }
        public int year { get; set; }
        public int mes { get; set; }
        public int poliza { get; set; }
    }
}