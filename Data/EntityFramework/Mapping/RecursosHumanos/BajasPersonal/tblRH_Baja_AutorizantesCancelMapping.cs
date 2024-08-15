using Core.Entity.RecursosHumanos.BajasPersonal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.BajasPersonal
{
    public class tblRH_Baja_AutorizantesCancelMapping : EntityTypeConfiguration<tblRH_Baja_AutorizantesCancel>
    {
        public tblRH_Baja_AutorizantesCancelMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idUsuario).HasColumnName("idUsuario");

            ToTable("tblRH_Baja_AutorizantesCancel");
        }
    }
}
