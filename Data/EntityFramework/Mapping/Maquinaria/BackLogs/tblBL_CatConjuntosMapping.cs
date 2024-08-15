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
    public class tblBL_CatConjuntosMapping : EntityTypeConfiguration<tblBL_CatConjuntos>
    {
        public tblBL_CatConjuntosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.abreviacion).HasColumnName("abreviacion");
            Property(x => x.esActivo).HasColumnName("esActivo");

            ToTable("tblBL_CatConjuntos");
        }
    }
}
