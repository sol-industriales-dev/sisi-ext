using Core.Entity.Administrativo.Seguridad.CatDepartamentos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.CatDepartamentos
{
    public class tblS_CatAreasOperativasMapping : EntityTypeConfiguration<tblS_CatAreasOperativas>
    {
        public tblS_CatAreasOperativasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.esActivo).HasColumnName("esActivo");
            ToTable("tblS_CatAreasOperativas");
        }
    }
}
