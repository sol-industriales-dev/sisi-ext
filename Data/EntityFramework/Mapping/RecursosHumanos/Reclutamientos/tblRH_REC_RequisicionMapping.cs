using Core.Entity.RecursosHumanos.Reclutamientos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_RequisicionMapping : EntityTypeConfiguration<tblRH_REC_Requisicion>
    {
        public tblRH_REC_RequisicionMapping()
        {
            HasKey(x => x.idSigoplan);
            Property(x => x.idSigoplan).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("idSigoplan");
            HasRequired(x => x.virtualPuesto).WithMany().HasForeignKey(x => x.puesto);
            HasRequired(x => x.virtualTipoContrato).WithMany().HasForeignKey(x => x.tipo_contrato);
            //HasRequired(x => x.virtualEmpleadoSolicitante).WithMany().HasForeignKey(x => x.solicitante);
            //HasRequired(x => x.virtualEmpleadoAutoriza).WithMany().HasForeignKey(x => x.autoriza);
            //HasRequired(x => x.virtualEmpleadoJefeInmediato).WithMany().HasForeignKey(x => x.jefe_inmediato);
            HasRequired(x => x.virtualPlantilla).WithMany().HasForeignKey(x => x.id_plantilla);
            HasOptional(x => x.tabuladorDet).WithMany().HasForeignKey(x => x.idTabuladorDet);
            ToTable("tblRH_REC_Requisicion");
        }
    }
}
