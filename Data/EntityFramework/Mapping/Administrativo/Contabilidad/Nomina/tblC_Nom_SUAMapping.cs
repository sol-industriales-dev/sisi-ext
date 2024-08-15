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
    public class tblC_Nom_SUAMapping : EntityTypeConfiguration<tblC_Nom_SUA>
    {
        public tblC_Nom_SUAMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.anio).HasColumnName("anio");
            Property(x => x.periodo).HasColumnName("periodo");
            Property(x => x.tipoNomina).HasColumnName("tipoNomina");
            Property(x => x.tipoDocumento).HasColumnName("tipoDocumento");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            Property(x => x.usuarioRegistro).HasColumnName("usuarioRegistro");
            Property(x => x.fechaModifica).HasColumnName("fechaModifica");
            Property(x => x.usuarioModifica).HasColumnName("usuarioModifica");
            Property(x => x.validado).HasColumnName("validado");
            Property(x => x.fechaValida).HasColumnName("fechaValida");
            Property(x => x.usuarioValida).HasColumnName("usuarioValida");
            Property(x => x.polizaGuardada).HasColumnName("polizaGuardada");
            Property(x => x.registroActivo).HasColumnName("registroActivo");
            ToTable("tblC_Nom_SUA");
        }
    }
}

