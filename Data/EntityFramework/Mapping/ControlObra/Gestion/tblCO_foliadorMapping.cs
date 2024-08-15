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
    public class tblCO_foliadorMapping : EntityTypeConfiguration<tblCO_foliador>
    {
        public tblCO_foliadorMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.descripcion).HasColumnName("descripcion");


            ToTable("tblCO_foliador");
        }
    }
}
