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
    public class CCF_AuthMapping : EntityTypeConfiguration<tblM_CCF_Auth>
    {
        public CCF_AuthMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.IdCuadro).HasColumnName("IdCuadro");
            Property(x => x.Orden).HasColumnName("Orden");
            Property(x => x.IdUsuario).HasColumnName("IdUsuario");
            Property(x => x.FechaFirma).HasColumnName("FechaFirma");
            Property(x => x.Firma).HasColumnName("Firma");
            Property(x => x.Estado).HasColumnName("Estado");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            Property(x => x.idUsuarioRegistro).HasColumnName("idUsuarioRegistro");
            HasRequired(x => x.Cuadro).WithMany().HasForeignKey(y => y.IdCuadro);
            ToTable("tblM_CCF_Auth");
        }
    }
}
