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
    class tblM_ComparativoAdiquisicionyRentaDetMapping : EntityTypeConfiguration<tblM_ComparativoAdquisicionyRentaDet>
    {
        public tblM_ComparativoAdiquisicionyRentaDetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idComparativo).HasColumnName("idComparativo");
            Property(x => x.proveedor).HasColumnName("proveedor");
            Property(x => x.precioDeVenta).HasColumnName("precioDeVenta");
            Property(x => x.tradeIn).HasColumnName("tradeIn");
            Property(x => x.valoresDeRecompra).HasColumnName("valoresDeRecompra");
            Property(x => x.precioDeRentaPura).HasColumnName("precioDeRentaPura");
            Property(x => x.precioDeRentaEnRoc).HasColumnName("precioDeRentaEnRoc");
            Property(x => x.baseHoras).HasColumnName("baseHoras");
            Property(x => x.tiempoDeEntrega).HasColumnName("tiempoDeEntrega");
            Property(x => x.ubicacion).HasColumnName("ubicacion");
            Property(x => x.horas).HasColumnName("horas");
            Property(x => x.seguro).HasColumnName("seguro");
            Property(x => x.garantia).HasColumnName("garantia");
            Property(x => x.serviciosPreventivos).HasColumnName("serviciosPreventivos");
            Property(x => x.capacitacion).HasColumnName("capacitacion");
            Property(x => x.depositoEnGarantia).HasColumnName("depositoEnGarantia");
            Property(x => x.lugarDeEntrega).HasColumnName("lugarDeEntrega");
            Property(x => x.flete).HasColumnName("flete");
            Property(x => x.condicionesDePagoEntrega).HasColumnName("condicionesDePagoEntrega");
            Property(x => x.caracteristicasDelEquipo1).HasColumnName("caracteristicasDelEquipo1");
            Property(x => x.caracteristicasDelEquipo2).HasColumnName("caracteristicasDelEquipo2");
            Property(x => x.caracteristicasDelEquipo3).HasColumnName("caracteristicasDelEquipo3");
            Property(x => x.caracteristicasDelEquipo4).HasColumnName("caracteristicasDelEquipo4");
            Property(x => x.caracteristicasDelEquipo5).HasColumnName("caracteristicasDelEquipo5");
            Property(x => x.caracteristicasDelEquipo6).HasColumnName("caracteristicasDelEquipo6");
            Property(x => x.caracteristicasDelEquipo7).HasColumnName("caracteristicasDelEquipo7");
            Property(x => x.rutaArchivo).HasColumnName("rutaArchivo");
            Property(x => x.comentarios).HasColumnName("comentarios");

            ToTable("tblM_ComparativoAdquisicionyRenta");
        }
    }
}
