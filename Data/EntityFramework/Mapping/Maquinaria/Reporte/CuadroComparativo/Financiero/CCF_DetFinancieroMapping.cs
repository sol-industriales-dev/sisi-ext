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
    public class CCF_DetFinancieroMapping : EntityTypeConfiguration<tblM_CCF_DetFinanciero>
    {
        public CCF_DetFinancieroMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.IdCuadro).HasColumnName("IdCuadro");
            Property(x => x.IdConcepto).HasColumnName("IdConcepto");
            Property(x => x.IdPlazo).HasColumnName("IdPlazo");
            Property(x => x.IdFinanciero).HasColumnName("IdFinanciero");
            Property(x => x.Valor).HasColumnName("Valor");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            Property(x => x.idUsuarioRegistro).HasColumnName("idUsuarioRegistro");
            HasRequired(x => x.Cuadro).WithMany().HasForeignKey(y => y.IdCuadro);
            HasRequired(x => x.Conceptos).WithMany().HasForeignKey(y => y.IdConcepto);
            ToTable("tblM_CCF_DetFinanciero");
        }
    }
}
