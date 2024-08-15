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
    public class tblC_Nom_ColumnaRayaMapping : EntityTypeConfiguration<tblC_Nom_ColumnaRaya>
    {
        public tblC_Nom_ColumnaRayaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.clave).HasColumnName("clave");
            Property(x => x.concepto).HasColumnName("concepto");
            Property(x => x.conceptoGeneral).HasColumnName("conceptoGeneral");
            Property(x => x.nombreColumnaUnidades).HasColumnName("nombreColumnaUnidades");
            Property(x => x.nombreColumnaImportes).HasColumnName("nombreColumnaImportes");
            Property(x => x.nombreColumnaFechas).HasColumnName("nombreColumnaFechas");
            Property(x => x.tipoColumna).HasColumnName("tipoColumna");
            Property(x => x.estatus).HasColumnName("estatus");
            ToTable("tblC_Nom_ColumnaRaya");
        }
    }
}
