using Core.Entity.Maquinaria.BackLogs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.BackLogs
{
    public class tblBL_CatSubconjuntosMapping : EntityTypeConfiguration<tblBL_CatSubconjuntos>
    {
        public tblBL_CatSubconjuntosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.idConjunto).HasColumnName("idConjunto");
            Property(x => x.abreviacion).HasColumnName("abreviacion");

            Property(x => x.esActivo).HasColumnName("esActivo");

            //HasRequired(x => x.conjunto).WithMany().HasForeignKey(y => y.idConjunto);
            HasRequired(x => x.CatConjuntos).WithMany().HasForeignKey(y => y.idConjunto);

            ToTable("tblBL_CatSubconjuntos");
        }
    }
}
