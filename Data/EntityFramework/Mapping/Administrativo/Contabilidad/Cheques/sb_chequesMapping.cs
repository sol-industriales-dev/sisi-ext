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
    public class sb_chequesMapping : EntityTypeConfiguration<tblC_sb_cheques>
    {
        public sb_chequesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.clave_sub_tm).HasColumnName("clave_sub_tm");
            Property(x => x.cpto1).HasColumnName("cpto1");
            Property(x => x.cpto2).HasColumnName("cpto2");
            Property(x => x.cpto3).HasColumnName("cpto3");
            Property(x => x.cuenta).HasColumnName("cuenta");
            Property(x => x.desc1).HasColumnName("desc1");
            Property(x => x.status_transf_cash).HasColumnName("status_transf_cash");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.fecha_firma1).HasColumnName("fecha_firma1");
            Property(x => x.fecha_firma2).HasColumnName("fecha_firma2");
            Property(x => x.fecha_firma3).HasColumnName("fecha_firma3");
            Property(x => x.fecha_mov).HasColumnName("fecha_mov");
            Property(x => x.fecha_reten).HasColumnName("fecha_reten");
            Property(x => x.fecha_reten_fin).HasColumnName("fecha_reten_fin");
            Property(x => x.firma1).HasColumnName("firma1");
            Property(x => x.firma2).HasColumnName("firma2");
            Property(x => x.firma3).HasColumnName("firma3");
            Property(x => x.hecha_por).HasColumnName("hecha_por");
            Property(x => x.id_empleado_firma).HasColumnName("id_empleado_firma");
            Property(x => x.id_empleado_firma2).HasColumnName("id_empleado_firma2");
            Property(x => x.ilinea).HasColumnName("ilinea");
            Property(x => x.imes).HasColumnName("imes");
            Property(x => x.ipoliza).HasColumnName("ipoliza");
            Property(x => x.itp).HasColumnName("itp");
            Property(x => x.iyear).HasColumnName("iyear");
            Property(x => x.monto).HasColumnName("monto");
            Property(x => x.num_pro_emp).HasColumnName("num_pro_emp");
            Property(x => x.numero).HasColumnName("numero");
            Property(x => x.ruta_comprobantebco_pdf).HasColumnName("ruta_comprobantebco_pdf");
            Property(x => x.status_bco).HasColumnName("status_bco");
            Property(x => x.status_lp).HasColumnName("status_lp");
            Property(x => x.tipocheque).HasColumnName("tipocheque");
            Property(x => x.tm).HasColumnName("tm");
            Property(x => x.tp).HasColumnName("tp");
            Property(x => x.usuariocapturaID).HasColumnName("usuariocapturaID");
            ToTable("tblC_sb_cheques");
        }
    }
}
