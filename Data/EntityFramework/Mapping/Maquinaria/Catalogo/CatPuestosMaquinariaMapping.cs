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
    public class CatPuestosMaquinariaMapping : EntityTypeConfiguration<tblM_CatPuestosMaquinaria>
    {
        CatPuestosMaquinariaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.puesto).HasColumnName("puesto");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblM_CatPuestosMaquinaria");
        }
    }

}
