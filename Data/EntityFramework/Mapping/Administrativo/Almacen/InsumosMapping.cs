using Core.Entity.Administrativo.ControlInterno.Almacen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Almacen
{
    public class InsumosMapping : EntityTypeConfiguration<tblAlm_MergeInsumos>
    {
        public InsumosMapping()
        {
            
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.insumoA).HasColumnName("insumoA");
            Property(x => x.insumoC).HasColumnName("insumoC");
            ToTable("tblAlm_MergeInsumos");

        }
    }
}
