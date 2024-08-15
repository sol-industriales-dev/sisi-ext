using Core.Entity.Administrativo.DocumentosXPagar.PQ;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.DocumentosXPagar.PQ
{
    public class tblAF_DxP_PQ_RelacionMovimientoMapping : EntityTypeConfiguration<tblAF_DxP_PQ_RelacionMovimiento>
    {
        public tblAF_DxP_PQ_RelacionMovimientoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.origenId).HasColumnName("origenId");
            Property(x => x.destinoId).HasColumnName("destinoId");
            HasRequired(x => x.pqOrigen).WithMany().HasForeignKey(y => y.origenId);
            HasRequired(x => x.pqDestino).WithMany().HasForeignKey(y => y.destinoId);
            ToTable("tblAF_DxP_PQ_RelacionMovimiento");
        }
    }
}
