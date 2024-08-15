using Core.Entity.ControlObra.GestionDeCambio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.ControlObra.Gestion
{
    public class tblCO_OC_NotificantesMapping: EntityTypeConfiguration<tblCO_OC_Notificantes>
    {
        public tblCO_OC_NotificantesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.idOrdenDeCambio).HasColumnName("idOrdenDeCambio");
            Property(x => x.idUsuario).HasColumnName("idUsuario");
            Property(x => x.cvEmpleados).HasColumnName("cvEmpleados");

            ToTable("tblCO_OC_Notificantes");
        }
    }
}
