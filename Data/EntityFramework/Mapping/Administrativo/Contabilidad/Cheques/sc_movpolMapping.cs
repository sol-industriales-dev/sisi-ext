using Core.Entity.Administrativo.Contabilidad.Cheque;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Cheques
{
    public class sc_movpolMapping : EntityTypeConfiguration<tblC_sc_movpol>
    {
        public sc_movpolMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.area).HasColumnName("area");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.cfd_fecha_expedicion).HasColumnName("cfd_fecha_expedicion");
            Property(x => x.cfd_metodo_pago_sat).HasColumnName("cfd_metodo_pago_sat");
            Property(x => x.cfd_moneda).HasColumnName("cfd_moneda");
            Property(x => x.cfd_rfc).HasColumnName("cfd_rfc");
            Property(x => x.cfd_ruta_pdf).HasColumnName("cfd_ruta_pdf");
            Property(x => x.cfd_ruta_xml).HasColumnName("cfd_ruta_xml");
            Property(x => x.cfd_tipocambio).HasColumnName("cfd_tipocambio");
            Property(x => x.cfd_tipocomprobante).HasColumnName("cfd_tipocomprobante");
            Property(x => x.cfd_total).HasColumnName("cfd_total");
            Property(x => x.concepto).HasColumnName("concepto");
            Property(x => x.cta).HasColumnName("cta");
            Property(x => x.cuenta_oc).HasColumnName("cuenta_oc");
            Property(x => x.digito).HasColumnName("digito");
            Property(x => x.factura_comp_ext).HasColumnName("factura_comp_ext");
            Property(x => x.folio_gxc).HasColumnName("folio_gxc");
            Property(x => x.folio_imp).HasColumnName("folio_imp");
            Property(x => x.forma_pago).HasColumnName("forma_pago");
            Property(x => x.iclave).HasColumnName("iclave");
            Property(x => x.istm).HasColumnName("istm");
            Property(x => x.itm).HasColumnName("itm");
            Property(x => x.linea).HasColumnName("linea");
            Property(x => x.linea_imp).HasColumnName("linea_imp");
            Property(x => x.mes).HasColumnName("mes");
            Property(x => x.metodo_pago_sat).HasColumnName("metodo_pago_sat");
            Property(x => x.monto).HasColumnName("monto");
            Property(x => x.num_emp).HasColumnName("num_emp");
            Property(x => x.numpro).HasColumnName("numpro");
            Property(x => x.orden_compra).HasColumnName("orden_compra");
            Property(x => x.poliza).HasColumnName("poliza");
            Property(x => x.referencia).HasColumnName("referencia");
            Property(x => x.ruta_comp_ext).HasColumnName("ruta_comp_ext");
            Property(x => x.scta).HasColumnName("scta");
            Property(x => x.socio_inversionista).HasColumnName("socio_inversionista");
            Property(x => x.sscta).HasColumnName("sscta");
            Property(x => x.st_par).HasColumnName("st_par");
            Property(x => x.taxid).HasColumnName("taxid");
            Property(x => x.tm).HasColumnName("tm");
            Property(x => x.tp).HasColumnName("tp");
            Property(x => x.UUID).HasColumnName("UUID");
            Property(x => x.year).HasColumnName("year");
            ToTable("tblC_sc_movpol");
        }
    }
}
