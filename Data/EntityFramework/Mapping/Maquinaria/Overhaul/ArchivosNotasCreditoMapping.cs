using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Overhaul
{
    public class ArchivosNotasCreditoMapping : EntityTypeConfiguration<tblM_ArchivosNotasCredito>
    {
        ArchivosNotasCreditoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.NotaCreditoID).HasColumnName("NotaCreditoID");
            Property(x => x.nombreArchivo).HasColumnName("nombreArchivo");
            Property(x => x.rutaArchivo).HasColumnName("rutaArchivo");
            Property(x => x.usuario).HasColumnName("usuario");
            Property(x => x.FechaSubida).HasColumnName("FechaSubida");
            Property(x => x.tipoArchivo).HasColumnName("tipoArchivo");
            ToTable("tblM_ArchivosNotasCredito");
        }
    }
}
