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
    public class tblEN_TipoPreguntaMapping : EntityTypeConfiguration<tblEN_TipoPregunta>
    {
        public tblEN_TipoPreguntaMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Descripcion).HasColumnName("descripcion");
            Property(x => x.Estatus).HasColumnName("estatus");
            ToTable("tblEN_TipoPregunta");
        }
    }
}
