using Core.Entity.Administrativo.Contabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad
{
    public class LineaMapping : EntityTypeConfiguration<tblC_Linea>
    {
        public LineaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.banco).HasColumnName("banco");
            Property(x => x.factoraje).HasColumnName("factoraje");
            Property(x => x.moneda).HasColumnName("moneda");
            Property(x => x.linea).HasColumnName("linea");
            Property(x => x.fecha).HasColumnName("fecha");
            ToTable("tblC_Linea");
        }
    }
}
