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
    public class tblM_CatMarcaEquipotblM_CatGrupoMaquinariaMapping : EntityTypeConfiguration<tblM_CatMarcaEquipotblM_CatGrupoMaquinaria>
    {
        public tblM_CatMarcaEquipotblM_CatGrupoMaquinariaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.tblM_CatMarcaEquipo_id).HasColumnName("tblM_CatMarcaEquipo_id");
            Property(x => x.tblM_CatGrupoMaquinaria_id).HasColumnName("tblM_CatGrupoMaquinaria_id");
            Property(x => x.isActivo).HasColumnName("isActivo");


            ToTable("tblM_CatMarcaEquipotblM_CatGrupoMaquinaria");
        }
    }
}
