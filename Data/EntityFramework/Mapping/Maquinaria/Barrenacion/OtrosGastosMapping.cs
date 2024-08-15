
using Core.Entity.Maquinaria.Barrenacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Barrenacion
{
    public class OtrosGastosMapping : EntityTypeConfiguration<tblB_OtrosGastos>
    {
        public OtrosGastosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.areaCuenta).HasColumnName("areaCuenta");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.monto).HasColumnName("monto");
            ToTable("tblB_OtrosGastos");
        }
    }
}
