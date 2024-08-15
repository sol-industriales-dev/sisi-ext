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
    public class subConjuntoModeloMapping : EntityTypeConfiguration<tblM_SubConjuntoModelo>
    {
       public subConjuntoModeloMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.modeloID).HasColumnName("modeloID");
            Property(x => x.subConjuntoID).HasColumnName("subConjuntoID");

            HasRequired(x => x.modelo).WithMany().HasForeignKey(y => y.modeloID);
            HasRequired(x => x.SubConjunto).WithMany().HasForeignKey(y => y.subConjuntoID);

            ToTable("tblM_SubConjuntoModelo");
        }
    }
}
