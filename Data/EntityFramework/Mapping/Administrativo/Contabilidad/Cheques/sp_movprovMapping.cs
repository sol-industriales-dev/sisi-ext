using Core.Entity.Administrativo.Contabilidad.Cheques;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Cheques
{
    public class sp_movprovMapping : EntityTypeConfiguration<tblC_sp_movprov>
    {
        public sp_movprovMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.afectacompra).HasColumnName("afectacompra");
            Property(x => x.autoincremento).HasColumnName("autoincremento");
            Property(x => x.autorizapago).HasColumnName("autorizapago");
            Property(x => x.bit_autoriza).HasColumnName("bit_autoriza");
            Property(x => x.bit_factoraje).HasColumnName("bit_factoraje");
            Property(x => x.bit_pagada).HasColumnName("bit_pagada");
            Property(x => x.bit_trasnferida).HasColumnName("bit_trasnferida");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.cfd_ano_aprob).HasColumnName("cfd_ano_aprob");
            Property(x => x.cfd_certificado).HasColumnName("cfd_certificado");
            Property(x => x.cfd_fecha).HasColumnName("cfd_fecha");
            Property(x => x.cfd_folio).HasColumnName("cfd_folio");
            Property(x => x.cfd_metodo_pago).HasColumnName("cfd_metodo_pago");
            Property(x => x.cfd_num_aprob).HasColumnName("cfd_num_aprob");
            Property(x => x.cfd_serie).HasColumnName("cfd_serie");
            Property(x => x.concepto).HasColumnName("concepto");
            Property(x => x.empleado).HasColumnName("empleado");
            Property(x => x.empleado_autorizo).HasColumnName("empleado_autorizo");
            Property(x => x.empleado_modifica).HasColumnName("empleado_modifica");
            Property(x => x.empleado_vobo).HasColumnName("empleado_vobo");
            Property(x => x.es_factura).HasColumnName("es_factura");
            Property(x => x.factura).HasColumnName("factura");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.fecha_autoriza).HasColumnName("fecha_autoriza");
            Property(x => x.fecha_modifica).HasColumnName("fecha_modifica");
            Property(x => x.fecha_vobo).HasColumnName("fecha_vobo");
            Property(x => x.fechaven).HasColumnName("fechaven");
            Property(x => x.folio).HasColumnName("folio");
            Property(x => x.folio_gxc).HasColumnName("folio_gxc");
            Property(x => x.folio_retencion).HasColumnName("folio_retencion");
            Property(x => x.generado).HasColumnName("generado");
            Property(x => x.inst_factoraje).HasColumnName("inst_factoraje");
            Property(x => x.iva).HasColumnName("iva");
            Property(x => x.linea).HasColumnName("linea");
            Property(x => x.mes).HasColumnName("mes");
            Property(x => x.moneda).HasColumnName("moneda");
            Property(x => x.moneda_oc).HasColumnName("moneda_oc");
            Property(x => x.moneda_oc_nom).HasColumnName("moneda_oc_nom");
            Property(x => x.monto).HasColumnName("monto");
            Property(x => x.numpro).HasColumnName("numpro");
            Property(x => x.pago_factoraje).HasColumnName("pago_factoraje");
            Property(x => x.pide_iva).HasColumnName("pide_iva");
            Property(x => x.poliza).HasColumnName("poliza");

            Property(x => x.ruta_rec_pdf).HasColumnName("ruta_rec_pdf");
            Property(x => x.ruta_rec_xml).HasColumnName("ruta_rec_xml");
            Property(x => x.socio_inversionista).HasColumnName("socio_inversionista");
            Property(x => x.st_factoraje).HasColumnName("st_factoraje");
            Property(x => x.st_pago).HasColumnName("st_pago");
            Property(x => x.suma_o_resta).HasColumnName("suma_o_resta");
            Property(x => x.tipo_factoraje).HasColumnName("tipo_factoraje");
            Property(x => x.tipocambio).HasColumnName("tipocambio");

            Property(x => x.tipocambio_oc).HasColumnName("tipocambio_oc");
            Property(x => x.tm).HasColumnName("tm");
            Property(x => x.total).HasColumnName("total");
            Property(x => x.tp).HasColumnName("tp");

            Property(x => x.UUID).HasColumnName("UUID");
            Property(x => x.val_ref).HasColumnName("val_ref");
            Property(x => x.valida_almacen).HasColumnName("valida_almacen");
            Property(x => x.valida_recibido).HasColumnName("valida_recibido");
            Property(x => x.valida_recibido_autorizar).HasColumnName("valida_recibido_autorizar");
            Property(x => x.year).HasColumnName("year");

            ToTable("tblC_sp_movprov");
        }
    }
}
