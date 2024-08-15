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
   public class tblCO_OC_GestionFirmasMapping: EntityTypeConfiguration<tblCO_OC_GestionFirmas>
    {
       public tblCO_OC_GestionFirmasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.idEmpleado).HasColumnName("idEmpleado");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.estatus).HasColumnName("estatus");
           

            ToTable("tblCO_OC_GestionFirmas");
        }
    }
}
