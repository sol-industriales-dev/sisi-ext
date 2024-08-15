using Core.Entity.Administrativo.Seguridad.CatHorasHombre;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.CatHorasHombre
{
    public class CatHorasHombreMapping : EntityTypeConfiguration<tblS_CatHorasHombre>
    {
        public CatHorasHombreMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.idEmpresa).HasColumnName("idEmpresa");
            Property(x => x.idGrupo).HasColumnName("idGrupo");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.horas).HasColumnName("horas");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.esActivo).HasColumnName("esActivo");


            //HasRequired(x => x.catCC).WithMany().HasForeignKey(y => y.cc);

            ToTable("tblS_CatHorasHombre");
        }
    }
}