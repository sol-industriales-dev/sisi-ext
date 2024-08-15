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
    class tblM_ComparativoFinancieroDetMapping : EntityTypeConfiguration<tblM_ComparativoFinancieroDet>
    {
        public tblM_ComparativoFinancieroDetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idFinanciero).HasColumnName("idFinanciero");
            Property(x => x.idRow).HasColumnName("idRow");
            Property(x => x.banco).HasColumnName("banco");
            Property(x => x.plazo).HasColumnName("plazo");
            Property(x => x.precioDelEquipo).HasColumnName("precioDelEquipo");
            Property(x => x.tiempoRestanteProyecto).HasColumnName("tiempoRestanteProyecto");
            Property(x => x.iva).HasColumnName("iva");
            Property(x => x.total).HasColumnName("total");
            Property(x => x.montoFinanciar).HasColumnName("montoFinanciar");
            Property(x => x.tipoOperacion).HasColumnName("tipoOperacion");
            Property(x => x.opcionCompra).HasColumnName("opcionCompra");
            Property(x => x.valorResidual).HasColumnName("valorResidual");
            Property(x => x.depositoEfectivo).HasColumnName("depositoEfectivo");
            Property(x => x.moneda).HasColumnName("moneda");
            Property(x => x.plazoMeses).HasColumnName("plazoMeses");
            Property(x => x.tasaDeInteres).HasColumnName("tasaDeInteres");
            Property(x => x.gastosFijos).HasColumnName("gastosFijos");
            Property(x => x.comision).HasColumnName("comision");
            Property(x => x.montoComision).HasColumnName("montoComision");
            Property(x => x.rentasEnGarantia).HasColumnName("rentasEnGarantia");
            Property(x => x.crecimientoPagos).HasColumnName("crecimientoPagos");
            Property(x => x.pagoInicial).HasColumnName("pagoInicial");
            Property(x => x.pagoTotalIntereses).HasColumnName("pagoTotalIntereses");
            Property(x => x.tasaEfectiva).HasColumnName("tasaEfectiva");
            Property(x => x.mensualidad).HasColumnName("mensualidad");
            Property(x => x.mensualidadSinIVA).HasColumnName("mensualidadSinIVA");
            Property(x => x.pagoTotal).HasColumnName("pagoTotal");

            
            
            ToTable("tblM_ComparativoFinancieroDet");
        }
    }
}
