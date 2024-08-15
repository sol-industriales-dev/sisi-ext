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
    public class ResultadoProveedorRequisicionesDetMapping : EntityTypeConfiguration<tblEN_ResultadoProveedorRequisicionDet>
    {
        public ResultadoProveedorRequisicionesDetMapping()
        {

            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.centroCostos).HasColumnName("centroCostos"); ;
            Property(x => x.numeroRequisicion).HasColumnName("numeroRequisicion");
            Property(x => x.nombreProveedor).HasColumnName("nombreProveedor");
            Property(x => x.comentarios).HasColumnName("comentarios");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.evaluadorID).HasColumnName("evaluadorID");
            Property(x => x.numProveedor).HasColumnName("numProveedor");
            Property(x => x.calificacion).HasColumnName("calificacion");
            Property(x => x.fechaEvaluacion).HasColumnName("fechaEvaluacion");

            ToTable("tblEN_ResultadoProveedorRequisicionDet");

        }
    }
}
