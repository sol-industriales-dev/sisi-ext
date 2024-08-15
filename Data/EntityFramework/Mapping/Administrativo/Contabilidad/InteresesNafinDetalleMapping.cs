using Core.Entity.Administrativo.Contabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad
{
    public class InteresesNafinDetalleMapping : EntityTypeConfiguration<tblC_InteresesNafinDetalle>
    {
        public InteresesNafinDetalleMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idInteres).HasColumnName("idInteres");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.interesFactoraje).HasColumnName("interesFactoraje");
            Property(x => x.divisa).HasColumnName("divisa");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            ToTable("tblC_InteresesNafinDetalle");
        }
    }
}
