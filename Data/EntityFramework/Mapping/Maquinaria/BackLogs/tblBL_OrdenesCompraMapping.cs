using Core.Entity.Maquinaria.BackLogs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.BackLogs
{
    public class tblBL_OrdenesCompraMapping : EntityTypeConfiguration<tblBL_OrdenesCompra>
    {
        public tblBL_OrdenesCompraMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.numRequisicion).HasColumnName("numRequisicion");
            Property(x => x.numOC).HasColumnName("numOC");
            Property(x => x.idBackLog).HasColumnName("idBackLog");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaCreacionNumOC).HasColumnName("fechaCreacionNumOC");
            Property(x => x.fechaModificacionNumOC).HasColumnName("fechaModificacionNumOC");

            ToTable("tblBL_OrdenesCompra");
        }
    }
}
