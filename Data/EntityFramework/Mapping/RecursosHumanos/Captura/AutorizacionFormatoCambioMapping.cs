using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Captura
{
    public class AutorizacionFormatoCambioMapping : EntityTypeConfiguration<tblRH_AutorizacionFormatoCambio>
    {
        public AutorizacionFormatoCambioMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Id_FormatoCambio).HasColumnName("Id_FormatoCambio");
            Property(x => x.Clave_Aprobador).HasColumnName("Clave_Aprobador");
            Property(x => x.Nombre_Aprobador).HasColumnName("Nombre_Aprobador");
            Property(x => x.PuestoAprobador).HasColumnName("PuestoAprobador");
            Property(x => x.Responsable).HasColumnName("Responsable");
            Property(x => x.Estatus).HasColumnName("Estatus");
            Property(x => x.Firma).HasColumnName("Firma");
            Property(x => x.Autorizando).HasColumnName("Autorizando");
            Property(x => x.Rechazado).HasColumnName("Rechazado");
            Property(x => x.Orden).HasColumnName("Orden");
            Property(x => x.tipoAutoriza).HasColumnName("tipoAutoriza");
            Property(x => x.comentario).HasColumnName("comentario");
            ToTable("tblRH_AutorizacionFormatoCambio");
        }

    }
}
