using Core.Entity.RecursosHumanos.Reportes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Reportes
{
    public class tblRH_EK_PrestamosAutorizacionesMapping : EntityTypeConfiguration<tblRH_EK_PrestamosAutorizaciones>
    {
        public tblRH_EK_PrestamosAutorizacionesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblRH_EK_PrestamosAutorizaciones");
        }
    }
}
