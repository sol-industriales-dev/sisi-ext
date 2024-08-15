using Core.Entity.RecursosHumanos.Desempeno;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Desempeno
{
    public class CatSemaforoMapping : EntityTypeConfiguration<tblRH_ED_CatSemaforo>
    {
        public CatSemaforoMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.minimo).HasColumnName("minimo");
            Property(x => x.maximo).HasColumnName("maximo");
            Property(x => x.color).HasColumnName("color");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            ToTable("tblRH_ED_CatSemaforo");
        }
    }
}
