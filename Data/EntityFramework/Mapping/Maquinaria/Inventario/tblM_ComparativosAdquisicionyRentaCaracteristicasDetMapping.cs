using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Inventario
{
    class tblM_ComparativosAdquisicionyRentaCaracteristicasDetMapping : EntityTypeConfiguration<tblM_ComparativosAdquisicionyRentaCaracteristicasDet>
    {
        public tblM_ComparativosAdquisicionyRentaCaracteristicasDetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idComparativoDetalle).HasColumnName("idComparativoDetalle");
            Property(x => x.idRow).HasColumnName("idRow");
            Property(x => x.Descripcion).HasColumnName("Descripcion");


            ToTable("tblM_ComparativosAdquisicionyRentaCaracteristicasDet");
        }
    }
}
