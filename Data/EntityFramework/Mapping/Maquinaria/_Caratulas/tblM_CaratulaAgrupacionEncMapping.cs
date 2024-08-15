using Core.Entity.Maquinaria._Caratulas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria._Caratulas
{
    public class tblM_CaratulaAgrupacionEncMapping: EntityTypeConfiguration<tblM_CaratulaAgrupacionEnc>
    {
        public tblM_CaratulaAgrupacionEncMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.AgrupacionCaratula).HasColumnName("AgrupacionCaratula");
            Property(x => x.esActivo).HasColumnName("esActivo");

            ToTable("tblM_CaratulaAgrupacionEnc");
        }
    }
}
