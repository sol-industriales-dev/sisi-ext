using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Inventario
{
    public class CapPlazoMapping : EntityTypeConfiguration<tblM_Comp_CapPlazo> 
    {
        public CapPlazoMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.financieroID).HasColumnName("financieroID");
            Property(x => x.tipoOperacion).HasColumnName("tipoOperacion");
            Property(x => x.opcionCompra).HasColumnName("opcionCompra");
            Property(x => x.enganche).HasColumnName("enganche");
            Property(x => x.depositoPorcentaje).HasColumnName("depositoPorcentaje");
            Property(x => x.depositoMoneda).HasColumnName("depositoMoneda");
            Property(x => x.moneda).HasColumnName("moneda");
            Property(x => x.plazo).HasColumnName("plazo");
            Property(x => x.tasaInteres).HasColumnName("tasaInteres").HasPrecision(18, 13);
            Property(x => x.gastosFijos).HasColumnName("gastosFijos");
            Property(x => x.comision).HasColumnName("comision");
            Property(x => x.rentasGarantia).HasColumnName("rentasGarantia");
            Property(x => x.crecimientoPagos).HasColumnName("crecimientoPagos");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            Property(x => x.usuarioRegistra).HasColumnName("usuarioRegistra");
            Property(x => x.estado).HasColumnName("estado");

            HasRequired(x => x.financiero).WithMany().HasForeignKey(y => y.financieroID);

            ToTable("tblM_Comp_CapPlazo");
        }
    }
}
