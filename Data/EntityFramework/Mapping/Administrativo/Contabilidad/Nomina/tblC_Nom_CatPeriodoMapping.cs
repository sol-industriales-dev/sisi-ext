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
    public class tblC_Nom_CatPeriodoMapping : EntityTypeConfiguration<tblC_Nom_CatPeriodo>
    {
        public tblC_Nom_CatPeriodoMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.anio).HasColumnName("anio");
            Property(x => x.tipoNomina).HasColumnName("tipoNomina");
            Property(x => x.periodo).HasColumnName("periodo");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.fechaFin).HasColumnName("fechaFin");
            Property(x => x.fechaLimite).HasColumnName("fechaLimite");
            Property(x => x.usuarioCaptura).HasColumnName("usuarioCaptura");
            Property(x => x.estatus).HasColumnName("estatus");
            ToTable("tblC_Nom_CatPeriodo");
        }
    }
}
