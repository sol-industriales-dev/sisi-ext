using Core.Entity.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.CuentasPorCobrar
{
    public class tblCXC_Comentarios_TiposMapping : EntityTypeConfiguration<tblCXC_Comentarios_Tipos>
    {
        public tblCXC_Comentarios_TiposMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblCXC_Comentarios_Tipos");
        }
    }
}
