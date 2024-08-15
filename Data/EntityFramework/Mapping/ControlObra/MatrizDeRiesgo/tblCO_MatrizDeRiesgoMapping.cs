using Core.Entity.ControlObra.MatrizDeRiesgo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.ControlObra.MatrizDeRiesgo
{
    public class tblCO_MatrizDeRiesgoMapping : EntityTypeConfiguration<tblCO_MatrizDeRiesgo>
    {
        public tblCO_MatrizDeRiesgoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.idPadre).HasColumnName("idPadre");
            Property(x => x.fechaElaboracion).HasColumnName("fechaElaboracion");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.nombreDelProyecto).HasColumnName("nombreDelProyecto");
            Property(x => x.personalElaboro).HasColumnName("personalElaboro");
            Property(x => x.faseDelProyecto).HasColumnName("faseDelProyecto");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblCO_MatrizDeRiesgo");
        }
    }
}
