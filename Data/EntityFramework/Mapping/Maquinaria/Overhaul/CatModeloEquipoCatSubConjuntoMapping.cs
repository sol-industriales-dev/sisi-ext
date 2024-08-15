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
    public class CatModeloEquipoCatSubConjuntoMapping : EntityTypeConfiguration<tblM_CatModeloEquipotblM_CatSubConjunto>
    {
        public CatModeloEquipoCatSubConjuntoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.modeloID).HasColumnName("modeloID");
            Property(x => x.subconjuntoID).HasColumnName("subconjuntoID");
            Property(x => x.numParte).HasColumnName("numParte");
            HasRequired(x => x.subconjunto).WithMany().HasForeignKey(x => x.subconjuntoID);   
            ToTable("tblM_CatModeloEquipotblM_CatSubConjunto");
        }
    }
}
