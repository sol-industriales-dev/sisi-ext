using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Data.EntityFramework.Mapping.Maquinaria.Catalogo
{
    class SubConjuntoMapping :EntityTypeConfiguration<tblM_CatSubConjunto>
    {
        SubConjuntoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.posicionID).HasColumnName("posicionID");
            Property(x => x.conjuntoID).HasColumnName("conjuntoID");
            Property(x => x.hasPosicion).HasColumnName("hasPosicion");
            Property(x => x.prefijo).HasColumnName("prefijo");
            HasRequired(x => x.conjunto).WithMany().HasForeignKey(y => y.conjuntoID);
            ToTable("tblM_CatSubConjunto");
        }
    }
}
