using Core.Entity.Sistemas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Principal.Menus
{
    class EmpresasMapping : EntityTypeConfiguration<tblP_Empresas>
    {
        public EmpresasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.icono).HasColumnName("icono");
            Property(x => x.url).HasColumnName("url");
            Property(x => x.urlInterna).HasColumnName("urlInterna");
            Property(x => x.urlLocal).HasColumnName("urlLocal");
            Property(x => x.urlPrueba).HasColumnName("urlPrueba");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.activo).HasColumnName("activo");
            Property(x => x.desarrollo).HasColumnName("desarrollo");
            Property(x => x.iconoRedireccion).HasColumnName("iconoRedireccion");
            ToTable("tblP_Empresas");
        }
    }
}
