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
    public class CCF_EncFinancieroMapping : EntityTypeConfiguration<tblM_CCF_EncFinanciero>
    {
        public CCF_EncFinancieroMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.IdAsignacion).HasColumnName("IdAsignacion");
            Property(x => x.Renta).HasColumnName("Renta");
            Property(x => x.IdMoneda).HasColumnName("IdMoneda");
            Property(x => x.RentaMes).HasColumnName("RentaMes");
            Property(x => x.FechaElaboracion).HasColumnName("FechaElaboracion");
            Property(x => x.Comentario).HasColumnName("Comentario");
            Property(x => x.IdFinanciero).HasColumnName("IdFinanciero");
            Property(x => x.Estado).HasColumnName("Estado");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            Property(x => x.idUsuarioRegistro).HasColumnName("idUsuarioRegistro");
            ToTable("tblM_CCF_EncFinanciero");
        }
    }
}
