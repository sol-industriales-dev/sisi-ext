using Core.Entity.Administrativo.Contabilidad.Nomina;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_PreNomina_DescuentoMapping : EntityTypeConfiguration<tblC_Nom_PreNomina_Descuento>
    {
        public tblC_Nom_PreNomina_DescuentoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.empleadoCve).HasColumnName("empleadoCve");
            Property(x => x.tipoDescuento).HasColumnName("tipoDescuento");
            Property(x => x.anio).HasColumnName("anio");
            Property(x => x.tipoNomina).HasColumnName("tipoNomina");
            Property(x => x.periodoInicial).HasColumnName("periodoInicial");
            Property(x => x.periodoFinal).HasColumnName("periodoFinal");
            Property(x => x.monto).HasColumnName("monto");
            Property(x => x.estatus).HasColumnName("estatus");
            ToTable("tblC_Nom_PreNomina_Descuento");
        }
    }
}
