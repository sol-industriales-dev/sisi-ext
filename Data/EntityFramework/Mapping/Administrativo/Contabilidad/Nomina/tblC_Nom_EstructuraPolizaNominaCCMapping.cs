using Core.Entity.Administrativo.Contabilidad.Nomina;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_EstructuraPolizaNominaCCMapping : EntityTypeConfiguration<tblC_Nom_EstructuraPolizaNominaCC>
    {
        public tblC_Nom_EstructuraPolizaNominaCCMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasColumnName("id");
            Property(x => x.cuentaId).HasColumnName("cuentaId");
            Property(x => x.columnaRaya).HasColumnName("columnaRaya");
            Property(x => x.descuento).HasColumnName("descuento");
            Property(x => x.esPorEmpleado).HasColumnName("esPorEmpleado");
            Property(x => x.tipoRayaId).HasColumnName("tipoRayaId");
            Property(x => x.tipoNominaId).HasColumnName("tipoNominaId");
            Property(x => x.clasificacionCcId).HasColumnName("clasificacionCcId");
            Property(x => x.estatus).HasColumnName("estatus");
            HasRequired(x => x.cuenta).WithMany().HasForeignKey(y => y.cuentaId);
            HasRequired(x => x.tipoRaya).WithMany().HasForeignKey(y => y.tipoRayaId);
            HasRequired(x => x.tipoNomina).WithMany().HasForeignKey(y => y.tipoNominaId);
            HasRequired(x => x.clasificacionCC).WithMany().HasForeignKey(y => y.clasificacionCcId);
            ToTable("tblC_Nom_EstructuraPolizaNominaCC");
        }
    }
}
