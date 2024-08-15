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
    public class Cliente_CapturaMensualArchivoMapping : EntityTypeConfiguration<tblX_Cliente_CapturaMensualArchivo>
    {
        public Cliente_CapturaMensualArchivoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.rutaArchivo).HasColumnName("rutaArchivo");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.actualizacion).HasColumnName("actualizacion");
            Property(x => x.capturaMensualID).HasColumnName("capturaMensualID");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblX_Cliente_CapturaMensualArchivo");
        }
    }
}
