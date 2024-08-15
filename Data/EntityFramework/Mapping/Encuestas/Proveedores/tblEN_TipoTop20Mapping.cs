using Core.Entity.Encuestas.Proveedores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Encuestas.Proveedores
{
    public class tblEN_TipoTop20Mapping : EntityTypeConfiguration<tblEN_TipoTop20>
    {
        public tblEN_TipoTop20Mapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Descripcion).HasColumnName("descripcion");
            ToTable("tblEN_TipoTop20");
        }
    }
}
