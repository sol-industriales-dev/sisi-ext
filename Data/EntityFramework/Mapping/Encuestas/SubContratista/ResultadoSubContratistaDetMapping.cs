using Core.Entity.Encuestas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Encuestas.SubContratista
{
    public class ResultadoSubContratistaDetMapping : EntityTypeConfiguration<tblEN_ResultadoSubContratistasDet>
    {
        public ResultadoSubContratistaDetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombreSubContratista).HasColumnName("nombreSubContratista"); ;
            Property(x => x.numSubContratista).HasColumnName("numSubContratista");
            Property(x => x.noContrato).HasColumnName("noContrato");
            Property(x => x.descripcionServicio).HasColumnName("descripcionServicio");
            Property(x => x.nombreProyecto).HasColumnName("nombreProyecto");
            Property(x => x.evaluador).HasColumnName("evaluador");

            Property(x => x.centroCostos).HasColumnName("centroCostos");
            Property(x => x.centroCostosNombre).HasColumnName("centroCostosNombre");
            Property(x => x.comentarios).HasColumnName("comentarios");

            Property(x => x.estadoEncuesta).HasColumnName("estadoEncuesta");
            Property(x => x.encuestaID).HasColumnName("encuestaID");
            Property(x => x.convenioID).HasColumnName("convenioID");
            Property(x => x.calificacion).HasColumnName("calificacion");

            //HasRequired(x => x.encuesta).WithMany().HasForeignKey(y => y.encuestaID);

            ToTable("tblEN_ResultadoSubContratistasDet");
        }
    }
}
