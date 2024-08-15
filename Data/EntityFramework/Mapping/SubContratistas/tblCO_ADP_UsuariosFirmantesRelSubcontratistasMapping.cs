using Core.Entity.ControlObra.Evaluacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.SubContratistas
{
    public class tblCO_ADP_UsuariosFirmantesRelSubcontratistasMapping : EntityTypeConfiguration<tblCO_ADP_UsuariosFirmantesRelSubcontratistas>
    {
        public tblCO_ADP_UsuariosFirmantesRelSubcontratistasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            ToTable("tblCO_ADP_UsuariosFirmantesRelSubcontratistas");
        }
    }
}
