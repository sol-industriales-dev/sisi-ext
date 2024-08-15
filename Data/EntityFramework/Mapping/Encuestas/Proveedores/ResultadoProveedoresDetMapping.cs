using Core.Entity.Encuestas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Encuestas.Proveedores
{
    public class ResultadoProveedoresDetMapping : EntityTypeConfiguration<tblEN_ResultadoProveedoresDet>
    {
        public ResultadoProveedoresDetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.centrocostos).HasColumnName("centrocostos");
            Property(x => x.numeroOC).HasColumnName("numeroOC");
            Property(x => x.fechaOC).HasColumnName("fechaOC");
            Property(x => x.numProveedor).HasColumnName("numProveedor");
            Property(x => x.nombreProveedor).HasColumnName("nombreProveedor");
            Property(x => x.tipoProveedor).HasColumnName("tipoProveedor");
            Property(x => x.fechaAntiguedad).HasColumnName("fechaAntiguedad");
            Property(x => x.ubicacionProveedor).HasColumnName("ubicacionProveedor");
            Property(x => x.comentarios).HasColumnName("comentarios");
            Property(x => x.estadoEncuesta).HasColumnName("estadoEncuesta");
            Property(x => x.tipoMoneda).HasColumnName("tipoMoneda");
            Property(x => x.evaluadorID).HasColumnName("evaluadorID");
            Property(x => x.encuestaID).HasColumnName("encuestaID");
            Property(x => x.calificacion).HasColumnName("calificacion");
            Property(x => x.fechaEvaluacion).HasColumnName("fechaEvaluacion");

            ToTable("tblEN_ResultadoProveedoresDet");

        }
    }
}
