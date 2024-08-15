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
    public class tblCO_OC_ArchivosFirmasMapping: EntityTypeConfiguration<tblCO_OC_ArchivosFirmas>
    {
        public tblCO_OC_ArchivosFirmasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            

            ToTable("tblCO_OC_ArchivosFirmas");
        }
    }
}
