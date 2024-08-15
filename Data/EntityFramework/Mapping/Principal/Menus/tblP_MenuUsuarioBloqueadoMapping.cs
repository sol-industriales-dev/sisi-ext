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
    public class tblP_MenuUsuarioBloqueadoMapping : EntityTypeConfiguration<tblP_MenuUsuarioBloqueado>
    {
        public tblP_MenuUsuarioBloqueadoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            ToTable("tblP_MenuUsuarioBloqueado");
        }
    }
}
