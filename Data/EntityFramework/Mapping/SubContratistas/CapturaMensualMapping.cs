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
    public class CapturaMensualMapping : EntityTypeConfiguration<tblX_CapturaMensual>
    {
        public CapturaMensualMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.archivoMensualID).HasColumnName("archivoMensualID");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.actualizacion).HasColumnName("actualizacion");
            Property(x => x.contratoPeriodoID).HasColumnName("contratoPeriodoID");
            Property(x => x.estatus).HasColumnName("estatus");
            HasRequired(x => x.periodo).WithMany().HasForeignKey(y => y.contratoPeriodoID);
            HasRequired(x => x.archivoMensual).WithMany().HasForeignKey(y => y.archivoMensualID);

            ToTable("tblX_CapturaMensual");
        }
    }
}
