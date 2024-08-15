using Core.Entity.Principal.Alertas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Principal.Alertas
{
    class AlertasMapping : EntityTypeConfiguration<tblP_Alerta>
    {
        public AlertasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.userEnviaID).HasColumnName("userEnviaID");
            Property(x => x.userRecibeID).HasColumnName("userRecibeID");
            Property(x => x.tipoAlerta).HasColumnName("tipoAlerta");
            Property(x => x.sistemaID).HasColumnName("sistemaID");
            Property(x => x.visto).HasColumnName("visto");
            Property(x => x.url).HasColumnName("url");
            Property(x => x.objID).HasColumnName("objID");
            Property(x => x.obj).HasColumnName("obj");
            Property(x => x.msj).HasColumnName("msj");
            Property(x => x.documentoID).HasColumnName("documentoID");
            Property(x => x.moduloID).HasColumnName("moduloID");

            ToTable("tblP_Alerta");
        }
    }
}
