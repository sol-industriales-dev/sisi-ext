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
    public class tbl5s_CCMapping : EntityTypeConfiguration<tbl5s_CC>
    {
        public tbl5s_CCMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.checkListId).HasColumnName("checkListId");
            HasRequired(x => x.checkList).WithMany().HasForeignKey(x => x.checkListId);

            ToTable("tbl5s_CC");
        }
    }
}
