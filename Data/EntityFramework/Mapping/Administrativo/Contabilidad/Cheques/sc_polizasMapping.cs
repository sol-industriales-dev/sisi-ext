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
    public class sc_polizasMapping : EntityTypeConfiguration<tblC_sc_polizas>
    {
        public sc_polizasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.abonos).HasColumnName("abonos");
            Property(x => x.cargos).HasColumnName("cargos");
            Property(x => x.concepto).HasColumnName("concepto");
            Property(x => x.error).HasColumnName("error");
            Property(x => x.fec_hora_movto).HasColumnName("fec_hora_movto");
            Property(x => x.fecha_hora_crea).HasColumnName("fecha_hora_crea");
            Property(x => x.fechapol).HasColumnName("fechapol");
            Property(x => x.generada).HasColumnName("generada");
            Property(x => x.mes).HasColumnName("mes");
            Property(x => x.poliza).HasColumnName("poliza");
            Property(x => x.socio_inversionista).HasColumnName("socio_inversionista");
            Property(x => x.status).HasColumnName("status");
            Property(x => x.status_carga_pol).HasColumnName("status_carga_pol");
            Property(x => x.status_lock).HasColumnName("status_lock");
            Property(x => x.tp).HasColumnName("tp");
            Property(x => x.usuario_crea).HasColumnName("usuario_crea");
            Property(x => x.usuario_movto).HasColumnName("usuario_movto");
            Property(x => x.year).HasColumnName("year");
            ToTable("tblC_sc_polizas");
        }
    }

}
