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
    public class tblBL_InspeccionesMapping : EntityTypeConfiguration<tblBL_Inspecciones>
    {
        public tblBL_InspeccionesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.areaCuenta).HasColumnName("areaCuenta");
            Property(x => x.periodo).HasColumnName("periodo");
            Property(x => x.idGrupo).HasColumnName("idGrupo");
            Property(x => x.noEconomico).HasColumnName("noEconomico");
            Property(x => x.horometro).HasColumnName("horometro");
            Property(x => x.idCatMaquina).HasColumnName("idCatMaquina");
            Property(x => x.fechaInicioInsp).HasColumnName("fechaInicioInsp");
            Property(x => x.fechaFinalInsp).HasColumnName("fechaFinalInsp");
            Property(x => x.fechaInspRealizada).HasColumnName("fechaInspRealizada");
            Property(x => x.cantBackLogs).HasColumnName("cantBackLogs");
            Property(x => x.fechaCreacionInsp).HasColumnName("fechaCreacionInsp");
            Property(x => x.fechaModificacionInsp).HasColumnName("fechaModificacionInsp");

            ToTable("tblBL_Inspecciones");
        }
    }
}
