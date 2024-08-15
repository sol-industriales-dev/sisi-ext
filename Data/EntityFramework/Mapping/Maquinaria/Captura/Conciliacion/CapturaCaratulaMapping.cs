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
    public class CapturaCaratulaMapping : EntityTypeConfiguration<tblM_CapCaratula>
    {
        public CapturaCaratulaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idCaratula).HasColumnName("idCaratula");
            Property(x => x.idGrupo).HasColumnName("idGrupo");
            Property(x => x.idModelo).HasColumnName("idModelo");
            Property(x => x.equipo).HasColumnName("equipo");
            Property(x => x.costo).HasColumnName("costo");
            Property(x => x.unidad).HasColumnName("unidad");
            HasRequired(x => x.EncCaratula).WithMany().HasForeignKey(y => y.idCaratula);


            Property(x => x.cargoFijo).HasColumnName("cargoFijo");
            Property(x => x.cOverhaul).HasColumnName("cOverhaul");
            Property(x => x.cMttoCorrectivo).HasColumnName("cMttoCorrectivo");
            Property(x => x.cCombustible).HasColumnName("cCombustible");
            Property(x => x.cAceites).HasColumnName("cAceites");
            Property(x => x.cFiltros).HasColumnName("cFiltros");
            Property(x => x.cAnsul).HasColumnName("cAnsul");
            Property(x => x.cCarrileria).HasColumnName("cCarrileria");
            Property(x => x.cLlantas).HasColumnName("cLlantas");
            Property(x => x.cHerramientasDesgaste).HasColumnName("cHerramientasDesgaste");
            Property(x => x.cCargoOperador).HasColumnName("cCargoOperador");
            Property(x => x.cPersonalMtto).HasColumnName("cPersonalMtto");


            ToTable("tblM_CapCaratula");
        }
    }
}
