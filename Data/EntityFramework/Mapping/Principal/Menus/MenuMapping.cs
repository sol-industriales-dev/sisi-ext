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
    class MenuMapping : EntityTypeConfiguration<tblP_Menu>
    {
        public MenuMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.sistemaID).HasColumnName("sistemaID");
            HasRequired(x => x.sistema).WithMany().HasForeignKey(y => y.sistemaID);
            Property(x => x.padre).HasColumnName("padre");
            Property(x => x.nivel).HasColumnName("nivel");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.url).HasColumnName("url");
            Property(x => x.icono).HasColumnName("icono");
            Property(x => x.orden).HasColumnName("orden");
            Property(x => x.visible).HasColumnName("visible");
            Property(x => x.iconoFont).HasColumnName("iconofont");
            Property(x => x.activo).HasColumnName("activo");
            Property(x => x.generalPorSistema).HasColumnName("generalPorSistema");
            Property(x => x.generalSIGOPLAN).HasColumnName("generalSIGOPLAN");
            Property(x => x.liberado).HasColumnName("liberado");
            Property(x => x.desarrollo).HasColumnName("desarrollo");
            Property(x => x.esColombia).HasColumnName("esColombia");
            ToTable("tblP_Menu");
        }
    }
}
