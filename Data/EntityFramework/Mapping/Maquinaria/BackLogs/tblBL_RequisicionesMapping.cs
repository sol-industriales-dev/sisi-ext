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
    public class tblBL_RequisicionesMapping : EntityTypeConfiguration<tblBL_Requisiciones>
    {
        public tblBL_RequisicionesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.numRequisicion).HasColumnName("numRequisicion");
            Property(x => x.idBackLog).HasColumnName("idBackLog");
            Property(x => x.fechaCreacionRequisicion).HasColumnName("fechaCreacionRequisicion");
            Property(x => x.fechaModificacionRequisicion).HasColumnName("fechaModificacionRequisicion");
            Property(x => x.esActivo).HasColumnName("esActivo");

            ToTable("tblBL_Requisiciones");
        }
    }
}
