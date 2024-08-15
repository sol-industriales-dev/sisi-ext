using Core.Entity.Principal.Configuraciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.CuadroComparativo.Financiero
{
    public class tblM_CCF_DetFinanciero : RegistroDTO
    {
        public int Id { get; set; }
        public int IdCuadro { get; set; }
        public int IdConcepto { get; set; }
        public int IdPlazo { get; set; }
        public int IdFinanciero { get; set; }
        public string Valor { get; set; }
        [ForeignKey("IdConcepto")]
        public virtual tblM_CCF_EncFinanciero Cuadro { get; set; }
        [ForeignKey("IdConcepto")]
        public virtual tblM_CCF_CatConcepto Conceptos { get; set; }
    }
}
