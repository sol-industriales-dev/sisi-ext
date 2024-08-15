using Core.Entity.Administrativo.Contabilidad.Nomina;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_PrenominaMapping : EntityTypeConfiguration<tblC_Nom_Prenomina>
    {
        public tblC_Nom_PrenominaMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.year).HasColumnName("year");
            Property(x => x.periodo).HasColumnName("periodo");
            Property(x => x.tipoNomina).HasColumnName("tipoNomina");
            Property(x => x.CC).HasColumnName("CC");
            Property(x => x.nombreCC).HasColumnName("nombreCC");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.validada).HasColumnName("validada");
            Property(x => x.usuarioValidaID).HasColumnName("usuarioValidaID");
            Property(x => x.fechaValidacion).HasColumnName("fechaValidacion");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.usuarioCapturaID).HasColumnName("usuarioCapturaID");
            Property(x => x.fechaAutorizacion).HasColumnName("fechaAutorizacion");
            Property(x => x.notificadoOficina).HasColumnName("notificadoOficina");
            ToTable("tblC_Nom_Prenomina");
        }
    }
}