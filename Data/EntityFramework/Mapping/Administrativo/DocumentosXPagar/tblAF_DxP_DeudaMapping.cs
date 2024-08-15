using Core.Entity.Administrativo.DocumentosXPagar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.DocumentosXPagar
{
    public class tblAF_DxP_DeudaMapping : EntityTypeConfiguration<tblAF_DxP_Deuda>
    {
        public tblAF_DxP_DeudaMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.ContratoId).HasColumnName("ContratoId");
            Property(x => x.Cta).HasColumnName("Cta");
            Property(x => x.Scta).HasColumnName("Scta");
            Property(x => x.Sscta).HasColumnName("Sscta");
            Property(x => x.Digito).HasColumnName("Digito");
            Property(x => x.Descripcion).HasColumnName("Descripcion");
            Property(x => x.Debe).HasColumnName("Debe");
            Property(x => x.Haber).HasColumnName("Haber");
            Property(x => x.Estatus).HasColumnName("Estatus");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            HasRequired(x => x.Contrato).WithMany().HasForeignKey(y => y.ContratoId);
            ToTable("tblAF_DxP_Deuda");
        }
    }
}
