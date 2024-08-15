using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Propuesta
{
    public class sp_gastos_prov_activofijoMapping : EntityTypeConfiguration<tblC_sp_gastos_prov_activofijo>
    {
        public sp_gastos_prov_activofijoMapping()
        {
            HasKey(x => x.idSigoplan);
            Property(x => x.idSigoplan).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("idSigoplan");
            Property(x => x.id);
            Property(x => x.numpro).HasColumnName("numpro");
            Property(x => x.cfd_serie).HasColumnName("cfd_serie");
            Property(x => x.cfd_folio).HasColumnName("cfd_folio");
            Property(x => x.referenciaoc).HasColumnName("referenciaoc");
            Property(x => x.tipo_prov).HasColumnName("tipo_prov");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.tm).HasColumnName("tm");
            Property(x => x.factura).HasColumnName("factura");
            Property(x => x.monto).HasColumnName("monto");
            Property(x => x.iva).HasColumnName("iva");
            Property(x => x.tipocambio).HasColumnName("tipocambio");
            Property(x => x.total).HasColumnName("total");
            Property(x => x.fecha_timbrado).HasColumnName("fecha_timbrado");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.idGiro).HasColumnName("idGiro");
            Property(x => x.concepto).HasColumnName("concepto");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.moneda).HasColumnName("moneda");
            Property(x => x.uuid).HasColumnName("uuid");
            Property(x => x.ruta_rec_xml).HasColumnName("ruta_rec_xml");
            Property(x => x.ruta_rec_pdf).HasColumnName("ruta_rec_pdf");
            Property(x => x.fecha_autoriza_portal).HasColumnName("fecha_autoriza_portal");
            Property(x => x.descuento).HasColumnName("descuento");
            Property(x => x.validacion).HasColumnName("validacion");
            Property(x => x.uuid_original).HasColumnName("uuid_original");
            Property(x => x.bit_nc).HasColumnName("bit_nc");
            Property(x => x.bit_compu).HasColumnName("bit_compu");
            Property(x => x.bit_carga).HasColumnName("bit_carga");
            Property(x => x.compuesta).HasColumnName("compuesta");
            Property(x => x.email_carga).HasColumnName("email_carga");
            Property(x => x.comentario_rechazo).HasColumnName("comentario_rechazo");
            Property(x => x.uuid_rechazo).HasColumnName("uuid_rechazo");
            Property(x => x.total_xml).HasColumnName("total_xml");
            Property(x => x.fecha_autoriza_factura).HasColumnName("fecha_autoriza_factura");
            Property(x => x.usuario_autoriza).HasColumnName("usuario_autoriza");
            Property(x => x.bit_antnc).HasColumnName("bit_antnc");
            Property(x => x.nivel_aut).HasColumnName("nivel_aut");
            Property(x => x.cerrado).HasColumnName("cerrado");
            Property(x => x.ruta_rec_xml_depura).HasColumnName("ruta_rec_xml_depura");
            Property(x => x.ruta_rec_pdf_depura).HasColumnName("ruta_rec_pdf_depura");
            Property(x => x.fechaPropuesta).HasColumnName("fechaPropuesta");
            Property(x => x.autorizada).HasColumnName("autorizada");
            Property(x => x.fechaAutorizacion).HasColumnName("fechaAutorizacion");
            Property(x => x.monto_plan).HasColumnName("monto_plan");
            Property(x => x.programo).HasColumnName("programo");
            Property(x => x.autorizo).HasColumnName("autorizo");
            Property(x => x.manual).HasColumnName("manual");

            ToTable("tblC_sp_gastos_prov_activofijo");
        }
    }
}
