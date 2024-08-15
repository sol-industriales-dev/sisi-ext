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
    public class tblCO_MR_TiposDeRepuestaRiesgoMapping : EntityTypeConfiguration<tblCO_MR_TiposDeRepuestaRiesgo>
    {
        public tblCO_MR_TiposDeRepuestaRiesgoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.id).HasColumnName("id");
            Property(x => x.idMatrizDeRiesgo).HasColumnName("idMatrizDeRiesgo");
            Property(x => x.tipoDeRepuesta).HasColumnName("tipoDeRepuesta");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.comentario).HasColumnName("comentario");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.idUsuario).HasColumnName("idUsuario");

           ToTable("tblCO_MR_TiposDeRepuestaRiesgo");
        }
    }
}
