using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura
{
    public class CatAceitesLubricantes : EntityTypeConfiguration<tblM_CatAceitesLubricantes>
    {
        public CatAceitesLubricantes()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Descripcion).HasColumnName("Descripcion");
            Property(x => x.subConjuntoID).HasColumnName("subConjuntoID");
            Property(x => x.modeloID).HasColumnName("modeloID");

            ToTable("tblM_CatAceitesLubricantes");
        }
    }
}
