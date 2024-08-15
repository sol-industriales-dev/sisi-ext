using Core.Entity.Enkontrol.Compras.OrdenCompra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Enkontrol.Compras.OrdenCompra
{
    class ResguardoMapping : EntityTypeConfiguration<tblAlm_Resguardo>
    {
        public ResguardoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.folio).HasColumnName("folio");
            Property(x => x.id_activo).HasColumnName("id_activo");
            Property(x => x.id_tipo_activo).HasColumnName("id_tipo_activo");
            Property(x => x.marca).HasColumnName("marca");
            Property(x => x.modelo).HasColumnName("modelo");
            Property(x => x.color).HasColumnName("color");
            Property(x => x.num_serie).HasColumnName("num_serie");
            Property(x => x.valor_activo).HasColumnName("valor_activo");
            Property(x => x.compania).HasColumnName("compania");
            Property(x => x.plan_desc).HasColumnName("plan_desc");
            Property(x => x.condiciones).HasColumnName("condiciones");
            Property(x => x.numpro).HasColumnName("numpro");
            Property(x => x.factura).HasColumnName("factura");
            Property(x => x.fec_factura).HasColumnName("fec_factura");
            Property(x => x.empleado).HasColumnName("empleado");
            Property(x => x.empleadoSIGOPLAN).HasColumnName("empleadoSIGOPLAN");
            Property(x => x.claveEmpleado).HasColumnName("claveEmpleado");
            Property(x => x.licencia).HasColumnName("licencia");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.fec_licencia).HasColumnName("fec_licencia");
            Property(x => x.observaciones).HasColumnName("observaciones");
            Property(x => x.fec_resguardo).HasColumnName("fec_resguardo");
            Property(x => x.foto).HasColumnName("foto");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.entrega).HasColumnName("entrega");
            Property(x => x.entregaSIGOPLAN).HasColumnName("entregaSIGOPLAN");
            Property(x => x.autoriza).HasColumnName("autoriza");
            Property(x => x.autorizaSIGOPLAN).HasColumnName("autorizaSIGOPLAN");
            Property(x => x.recibio).HasColumnName("recibio");
            Property(x => x.recibioSIGOPLAN).HasColumnName("recibioSIGOPLAN");
            Property(x => x.condiciones_ret).HasColumnName("condiciones_ret");
            Property(x => x.fec_devolucion).HasColumnName("fec_devolucion");
            Property(x => x.cantidad_resguardo).HasColumnName("cantidad_resguardo");
            Property(x => x.alm_salida).HasColumnName("alm_salida");
            Property(x => x.alm_entrada).HasColumnName("alm_entrada");
            Property(x => x.foto_2).HasColumnName("foto_2");
            Property(x => x.foto_3).HasColumnName("foto_3");
            Property(x => x.foto_4).HasColumnName("foto_4");
            Property(x => x.foto_5).HasColumnName("foto_5");
            Property(x => x.costo_promedio).HasColumnName("costo_promedio");
            Property(x => x.resguardo_parcial).HasColumnName("resguardo_parcial");
            Property(x => x.estatusRegistro).HasColumnName("estatusRegistro");

            ToTable("tblAlm_Resguardo");
        }
    }
}
