using Core.Entity.Maquinaria.Reporte.CuadroComparativo.Equipo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Reporte.CuadroComparativo.Equipo
{
    public class CCE_CatConceptoMapping : EntityTypeConfiguration<tblM_CCE_CatConcepto>
    {
        public CCE_CatConceptoMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.Orden).HasColumnName("Orden");
            Property(x => x.TipoDato).HasColumnName("TipoDato");
            Property(x => x.Descripcion).HasColumnName("Descripcion");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            Property(x => x.idUsuarioRegistro).HasColumnName("idUsuarioRegistro");
            HasRequired(x => x.Valores).WithMany().HasForeignKey(y => y.Id);
            ToTable("tblM_CCE_CatConcepto");
        }
    }
}
