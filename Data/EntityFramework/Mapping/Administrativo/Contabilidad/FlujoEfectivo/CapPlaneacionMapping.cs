using Core.Entity.Administrativo.Contabilidad.FlujoEfectivo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.FlujoEfectivo
{
    public class CapPlaneacionMapping : EntityTypeConfiguration<tblC_FED_CapPlaneacion>
    {
        public CapPlaneacionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idConceptoDir).HasColumnName("idConceptoDir");
            Property(x => x.ac).HasColumnName("ac");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.anio).HasColumnName("anio");
            Property(x => x.semana).HasColumnName("semana");
            Property(x => x.planeado).HasColumnName("planeado").HasPrecision(22, 4);
            Property(x => x.corte).HasColumnName("corte").HasPrecision(22, 4);
            Property(x => x.strFlujoEfectivo).HasColumnName("strFlujoEfectivo");
            Property(x => x.strSaldoInicial).HasColumnName("strSaldoInicial");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            ToTable("tblC_FED_CapPlaneacion");
        }
    }
}
