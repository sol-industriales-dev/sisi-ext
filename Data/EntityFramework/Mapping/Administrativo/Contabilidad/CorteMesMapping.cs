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
    class CorteMesMapping : EntityTypeConfiguration<tblEF_CorteMes>
    {
        public CorteMesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.anio).HasColumnName("anio");
            Property(x => x.mes).HasColumnName("mes");
            Property(x => x.usuarioCapturaID).HasColumnName("usuarioCapturaID");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblEF_CorteMes");
        }
    }
}
