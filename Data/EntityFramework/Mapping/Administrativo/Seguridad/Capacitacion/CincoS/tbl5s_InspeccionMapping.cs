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
    public class tbl5s_InspeccionMapping : EntityTypeConfiguration<tbl5s_Inspeccion>
    {
        public tbl5s_InspeccionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.checkList).WithMany().HasForeignKey(x => x.checkListId);
            HasRequired(x => x.subArea).WithMany().HasForeignKey(x => x.subAreaId);
            ToTable("tbl5s_Inspeccion");
        }
    }
}
