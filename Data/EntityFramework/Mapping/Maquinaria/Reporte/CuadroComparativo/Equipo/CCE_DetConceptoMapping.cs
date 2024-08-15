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
    public class CCE_DetConceptoMapping : EntityTypeConfiguration<tblM_CCE_DetConcepto>
    {
        public CCE_DetConceptoMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.IdEquipo).HasColumnName("IdEquipo");
            Property(x => x.IdConcepto).HasColumnName("IdConcepto");
            Property(x => x.Valor).HasColumnName("Valor");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            Property(x => x.idUsuarioRegistro).HasColumnName("idUsuarioRegistro");
            HasRequired(x => x.Equipo).WithMany().HasForeignKey(y => y.IdEquipo);
            HasRequired(x => x.Concepto).WithMany().HasForeignKey(y => y.IdConcepto);
            ToTable("tblM_CCE_DetConcepto");
        }
    }
}
