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
    public class DocumentacionFijaMapping : EntityTypeConfiguration<tblX_DocumentacionFija>
    {
        public DocumentacionFijaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.clave).HasColumnName("clave");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.aplicaFechaVencimiento).HasColumnName("aplicaFechaVencimiento");
            Property(x => x.aplicaFechaSolicitud).HasColumnName("aplicaFechaSolicitud");
            Property(x => x.mesesVigenciaMinima).HasColumnName("mesesVigenciaMinima");
            Property(x => x.mesesNotificacion).HasColumnName("mesesNotificacion");
            Property(x => x.fisica).HasColumnName("fisica");
            Property(x => x.moral).HasColumnName("moral");
            Property(x => x.opcional).HasColumnName("opcional");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblX_DocumentacionFija");
        }
    }
}
