using Core.Entity.Administrativo.Seguridad.Capacitacion.CincoS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Capacitacion.CincoS
{
    public class tbl5s_LiderAreaMapping : EntityTypeConfiguration<tbl5s_LiderArea>
    {
        public tbl5s_LiderAreaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.checkList).WithMany().HasForeignKey(x => x.checkListId);
            HasRequired(x => x.usuario).WithMany().HasForeignKey(x => x.usuario5sId);
            ToTable("tbl5s_LiderArea");
        }
    }
}
