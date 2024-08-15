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
    public class tblAF_DxP_PQ_TipoMovimientoMapping : EntityTypeConfiguration<tblAF_DxP_PQ_TipoMovimiento>
    {
        public tblAF_DxP_PQ_TipoMovimientoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.estatus).HasColumnName("estatus");
            ToTable("tblAF_DxP_PQ_TipoMovimiento");
        }
    }
}
