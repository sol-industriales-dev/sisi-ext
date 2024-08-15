using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.Moneda;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.SistemaContable.Moneda
{
    public class CatMonedaMapping : EntityTypeConfiguration<tblC_SC_CatMoneda>
    {
        public CatMonedaMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.Moneda).HasColumnName("Moneda");
            Property(x => x.Clave).HasColumnName("Clave");
            Property(x => x.Denominacion).HasColumnName("Denominacion");
            Property(x => x.Codigo).HasColumnName("Codigo");
            Property(x => x.Idioma).HasColumnName("Idioma");
            Property(x => x.idAccion).HasColumnName("idAccion");
            ToTable("tblC_SC_CatMoneda");
        }
    }
}
