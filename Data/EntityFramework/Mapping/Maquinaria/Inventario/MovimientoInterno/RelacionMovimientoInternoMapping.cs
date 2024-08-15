using Core.Entity.Maquinaria.Inventario.Movimiento_Interno;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Inventario.MovimientoInterno
{
    public class RelacionMovimientoInternoMapping : EntityTypeConfiguration<tblM_CatRelacionMovimientoInterno>
    {
        public RelacionMovimientoInternoMapping()
        {
           HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Destino).HasColumnName("CC");
            Property(x => x.Estatus).HasColumnName("FechaCaptura");
            Property(x => x.Origen).HasColumnName("estatus");


            ToTable("tblM_CatRelacionMovimientoInterno");
        }
    }

}
