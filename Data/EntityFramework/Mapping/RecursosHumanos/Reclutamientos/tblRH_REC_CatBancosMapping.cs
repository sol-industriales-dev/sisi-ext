using Core.Entity.RecursosHumanos.Reclutamientos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_CatBancosMapping : EntityTypeConfiguration<tblRH_REC_CatBancos>
    {
        public tblRH_REC_CatBancosMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.banco).HasColumnName("banco");
            Property(x => x.esActivo).HasColumnName("esActivo");

            ToTable("tblRH_REC_CatBancos");
        }
    }
}
