using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura
{
    public class CapturaConciliacionHorometrosMapping : EntityTypeConfiguration<tblM_CapConciliacionHorometros>
    {
        public CapturaConciliacionHorometrosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idEncCaratula).HasColumnName("idEncCaratula");
            Property(x => x.idCapCaratula).HasColumnName("idCapCaratula");
            Property(x => x.noEconomicoID).HasColumnName("noEconomicoID");
            Property(x => x.economico).HasColumnName("economico");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.horometroInicial).HasColumnName("horometroInicial");
            Property(x => x.horometroFinal).HasColumnName("horometroFinal");
            Property(x => x.horometroEfectivo).HasColumnName("horometroEfectivo");
            Property(x => x.total).HasColumnName("total");
            Property(x => x.unidad).HasColumnName("unidad");
            Property(x => x.idEmpresa).HasColumnName("idEmpresa");
            Property(x => x.observaciones).HasColumnName("observaciones");

            Property(x => x.costo).HasColumnName("costo");
            Property(x => x.modelo).HasColumnName("modelo");
            Property(x => x.numero).HasColumnName("numero");
            Property(x => x.overhaul).HasColumnName("overhaul");

            ToTable("tblM_CapConciliacionHorometros");
        }
    }
}
