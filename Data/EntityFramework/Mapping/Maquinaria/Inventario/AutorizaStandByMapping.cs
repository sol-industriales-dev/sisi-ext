using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Inventario
{
    public class AutorizaStandByMapping : EntityTypeConfiguration<tblM_AutorizaStandby>
    {
        AutorizaStandByMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.CadenaElabora).HasColumnName("CadenaElabora");
            Property(x => x.CadenaGerente).HasColumnName("CadenaGerente");
            Property(x => x.FechaElaboro).HasColumnName("FechaElaboro");
            Property(x => x.FechaValida).HasColumnName("FechaValida");
            Property(x => x.FirmaElabora).HasColumnName("FirmaElabora");
            Property(x => x.FirmaGerente).HasColumnName("FirmaGerente");
            Property(x => x.idElabora).HasColumnName("idElabora");
            Property(x => x.idGerente).HasColumnName("idGerente");
            Property(x => x.standByID).HasColumnName("standByID");
            ToTable("tblM_AutorizaStandby");

        }
    }
}
