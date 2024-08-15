using Core.Entity.Maquinaria.Reporte.CuadroComparativo.Financiero;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Reporte.CuadroComparativo.Financiero
{
    public class CCF_CatConceptoMapping : EntityTypeConfiguration<tblM_CCF_CatConcepto>
    {
        public CCF_CatConceptoMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.Orden).HasColumnName("Orden");
            Property(x => x.Descripcion).HasColumnName("Descripcion");
            Property(x => x.TipoDato).HasColumnName("TipoDato");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            Property(x => x.idUsuarioRegistro).HasColumnName("idUsuarioRegistro");
            ToTable("tblM_CCF_CatConcepto");
        }
    }
}
