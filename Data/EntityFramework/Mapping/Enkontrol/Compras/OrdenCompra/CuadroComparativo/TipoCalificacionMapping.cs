using Core.Entity.Enkontrol.Compras.OrdenCompra.CuadroComparativo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Enkontrol.Compras.OrdenCompra.CuadroComparativo
{
    public class TipoCalificacionMappging : EntityTypeConfiguration<tblCom_CC_TipoCalificacion>
    {
        public TipoCalificacionMappging()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.Descripcion).HasColumnName("Descripcion");
            Property(x => x.Estatus).HasColumnName("Estatus");
            ToTable("tblCom_CC_TipoCalificacion");
        }
    }
}
