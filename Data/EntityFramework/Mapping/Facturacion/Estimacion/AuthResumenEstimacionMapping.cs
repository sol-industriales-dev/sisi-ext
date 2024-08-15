using Core.Entity.Facturacion.Estimacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Facturacion.Estimacion
{
    public class AuthResumenEstimacionMapping : EntityTypeConfiguration<tblF_AuthResumenEstimacion>
    {
        public AuthResumenEstimacionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.stAuth).HasColumnName("stAuth");
            Property(x => x.fechaResumen).HasColumnName("fechaResumen");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.firma).HasColumnName("firma");            
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.esActivo).HasColumnName("esActivo");
            ToTable("tblF_AuthResumenEstimacion");
        }
    }
}
