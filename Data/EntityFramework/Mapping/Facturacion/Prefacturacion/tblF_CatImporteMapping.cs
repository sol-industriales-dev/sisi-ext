using Core.Entity.Facturacion.Prefacturacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Facturacion.Prefacturacion
{
    public class tblF_CatImporteMapping : EntityTypeConfiguration<tblF_CatImporte>
    {
        public tblF_CatImporteMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Concepto).HasColumnName("Concepto");
            ToTable("tblF_CatImporte");
        }
    }
}
