using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_BN_Evaluacion_Det
    {
        public int id { get; set; }
        public int plantillaID { get; set; }
        public virtual tblRH_BN_Plantilla plantilla { get; set; }
        public int plantillaDetID { get; set; }
        public virtual tblRH_BN_Plantilla_Det plantillaDet { get; set; }
        public int evaluacionID { get; set; }
        public virtual tblRH_BN_Evaluacion evaluacion { get; set; }
        public int cve_Emp { get; set; }
	    public string nombre_Emp { get; set; }
	    public int puestoCve_Emp { get; set; }
        public string puesto_Emp { get; set; }
	    public decimal base_Emp { get; set; }
        public decimal complemento_Emp { get; set; }
        public decimal bono_FC { get; set; }
        public decimal bono_Emp { get; set; }
	    public int porcentaje_Asig { get; set; }
        public decimal monto_Asig { get; set; }
        public decimal total_Nom { get; set; }
        public string tipo_Nom { get; set; }
	    public int tipoCve_Nom { get; set; }
        public decimal total_Mensual { get; set; }
        public decimal con_Bono { get; set; }
        public int periodicidadCve { get; set; }
        [NotMapped] 
        public string aplicaSindicato { get; set; }
        [NotMapped] 
        public virtual string periodicidad { get; set; }
        [NotMapped] 
        public virtual DateTime fechaAlta { get; set; }
        [NotMapped] 
        public virtual string fechaAltaStr { get; set; }
        [NotMapped] 
        public virtual string fechaRe { get; set; }
        [NotMapped] 
        public virtual string fechaAltaRe { get; set; }
        [NotMapped] 
        public virtual Core.DTO.Utils.PeriodoDTO antiguedad { get; set; }
       
        public DateTime fechaAplicacion { get; set; }
        public int periodoNomina { get; set; }

    }
}
