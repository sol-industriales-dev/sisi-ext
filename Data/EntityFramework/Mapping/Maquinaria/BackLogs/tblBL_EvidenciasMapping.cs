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
    public class tblBL_EvidenciasMapping : EntityTypeConfiguration<tblBL_Evidencias>
    {
        public tblBL_EvidenciasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idBL).HasColumnName("idBL");
            Property(x => x.nombreArchivo).HasColumnName("nombreArchivo");
            Property(x => x.tipoEvidencia).HasColumnName("tipoEvidencia");
            Property(x => x.rutaArchivo).HasColumnName("rutaArchivo");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.esActivo).HasColumnName("esActivo");

            ToTable("tblBL_Evidencias");
        }
    }
}
