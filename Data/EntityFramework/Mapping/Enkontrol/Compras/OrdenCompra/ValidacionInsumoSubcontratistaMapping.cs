using Core.Entity.Enkontrol.Compras.OrdenCompra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Enkontrol.Compras.OrdenCompra
{
    class ValidacionInsumoSubcontratistaMapping : EntityTypeConfiguration<tblCom_ValidacionInsumoSubcontratista>
    {
        public ValidacionInsumoSubcontratistaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.codigo).HasColumnName("codigo");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblCom_ValidacionInsumoSubcontratista");
        }
    }
}
