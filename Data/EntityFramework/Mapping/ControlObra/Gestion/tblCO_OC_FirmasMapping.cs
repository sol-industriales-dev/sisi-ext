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
    public class tblCO_OC_FirmasMapping: EntityTypeConfiguration<tblCO_OC_Firmas>
    {
        public tblCO_OC_FirmasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.idOrdenDeCambio).HasColumnName("idOrdenDeCambio");
            Property(x => x.firma).HasColumnName("firma");
            Property(x => x.idFirma).HasColumnName("idFirma");
            Property(x => x.firmaDigital).HasColumnName("firmaDigital");
            Property(x => x.idRow).HasColumnName("idRow");
            Property(x => x.Autorizando).HasColumnName("Autorizando");
            Property(x => x.fechaAutorizacion).HasColumnName("fechaAutorizacion");
            Property(x => x.Estado).HasColumnName("Estado");
            

            ToTable("tblCO_OC_Firmas");
        }
    }
}
