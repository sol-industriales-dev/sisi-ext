using Core.Entity.SubContratistas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.SubContratistas
{
    public class ContratoMapping : EntityTypeConfiguration<tblX_Contrato>
    {
        public ContratoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.numeroContrato).HasColumnName("numeroContrato");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.fechaSuscripcion).HasColumnName("fechaSuscripcion");
            Property(x => x.fechaVigencia).HasColumnName("fechaVigencia");
            Property(x => x.montoContractual).HasColumnName("montoContractual");
            Property(x => x.anticipoAplica).HasColumnName("anticipoAplica");
            Property(x => x.anticipoPorcentaje).HasColumnName("anticipoPorcentaje");
            Property(x => x.penalizacion).HasColumnName("penalizacion");
            Property(x => x.area).HasColumnName("area");
            Property(x => x.proyectoID).HasColumnName("proyectoID");
            Property(x => x.subcontratistaID).HasColumnName("subcontratistaID");
            Property(x => x.estatusContratoId).HasColumnName("estatusContratoId");
            Property(x => x.estatus).HasColumnName("estatus");
            HasRequired(x => x.estatusContrato).WithMany().HasForeignKey(y => y.estatusContratoId);
            HasRequired(x => x.subcontratista).WithMany().HasForeignKey(x => x.subcontratistaID);
            HasRequired(x => x.proyecto).WithMany().HasForeignKey(x => x.proyectoID);

            ToTable("tblX_Contrato");
        }
    }
}
