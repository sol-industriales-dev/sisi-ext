using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_Eficiencia
    {
        public int id { get; set; }
        public DateTime Fecha { get; set; }
        public int IdEquipo { get; set; }
        public int IdGrupo { get; set; }
        public int IdTipoEquipo { get; set; }
        public string IdObra { get; set; }
        public int Turno { get; set; }
        public decimal HrsInicial { get; set; }
        public decimal HrsFinal { get; set; }
        public decimal HrsTrabajada { get; set; }
        public decimal FallaTrenRodaje { get; set; }
        public decimal FallaElectrica { get; set; }
        public decimal FallaHidraulica { get; set; }
        public decimal FallaOtros { get; set; }
        public decimal FallaOperacion { get; set; }
        public decimal FaltaOperador { get; set; }
        public decimal TramoFMat { get; set; }
        public decimal TramoFDat { get; set; }
        public decimal TramoFAvan { get; set; }
        public decimal TramoIClie { get; set; }
        public decimal HrsTotal { get; set; }
        public decimal HrsTotalReparacion { get; set; }
        public decimal Paro { get; set; }
        public string Comentarios { get; set; }
        public int Semana { get; set; }
        public decimal HrsBase { get; set; }
        public decimal HrsDiferencia { get; set; }
        public string Economico { get; set; }
    }
}
