using Core.Entity.Principal.Menus;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Principal.Menus
{
    public class SistemaMapping : EntityTypeConfiguration<tblP_Sistema>
    {
        public SistemaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.icono).HasColumnName("icono");
            Property(x => x.url).HasColumnName("url");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.activo).HasColumnName("activo");
            Property(x => x.general).HasColumnName("general");
            Property(x => x.ext).HasColumnName("ext");
            Property(x => x.esColombia).HasColumnName("esColombia");
            Property(x => x.esVirtual).HasColumnName("esVirtual");
            ToTable("tblP_Sistema");
        }
    }
}
