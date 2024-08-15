using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Overhaul
{
    public class CatLocacionesComponentesMapping : EntityTypeConfiguration<tblM_CatLocacionesComponentes>
    {
        CatLocacionesComponentesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.tipoLocacion).HasColumnName("tipoLocacion");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.areaCuenta).HasColumnName("areaCuenta");
            Property(x => x.JsonCorreos).HasColumnName("JsonCorreos");
            ToTable("tblM_CatLocacionesComponentes");
        }
    }
}