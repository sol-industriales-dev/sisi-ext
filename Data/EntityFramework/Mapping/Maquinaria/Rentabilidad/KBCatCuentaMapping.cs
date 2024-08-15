using Core.Entity.Maquinaria.Rentabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Rentabilidad
{
    public class KBCatCuentaMapping : EntityTypeConfiguration<tblM_KBCatCuenta>
    {
        KBCatCuentaMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cuenta).HasColumnName("cuenta");
            Property(x => x.descripcion).HasColumnName("descripcion");
            ToTable("tblM_KBCatCuenta");
        }
    }
}