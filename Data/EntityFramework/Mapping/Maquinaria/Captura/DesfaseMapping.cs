using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura
{
    public class DesfaseMapping : EntityTypeConfiguration<tblM_CapDesfase>
    {
        public DesfaseMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.CC).HasColumnName("CC");
            Property(x => x.Economico).HasColumnName("Economico");
            Property(x => x.horasDesfase).HasColumnName("horasDesfase");
            Property(x => x.horasDesfaseAcumulado).HasColumnName("horasDesfaseAcumulado");
            Property(x => x.estado).HasColumnName("estado");
            Property(x => x.fecha).HasColumnName("fecha");
            ToTable("tblM_CapDesfase");
        }
    }
}
