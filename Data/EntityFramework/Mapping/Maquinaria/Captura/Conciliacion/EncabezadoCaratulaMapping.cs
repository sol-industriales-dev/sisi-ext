using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria
{
    public class EncabezadoCaratulaMapping : EntityTypeConfiguration<tblM_EncCaratula>
    {
        public EncabezadoCaratulaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.ccID).HasColumnName("ccID");
            Property(x => x.creacion).HasColumnName("creacion");
            Property(x => x.idUsuario).HasColumnName("idUsuario");
            Property(x => x.isActivo).HasColumnName("isActivo");
            Property(x => x.fechaVigencia).HasColumnName("fechaVigencia");
            ToTable("tblM_EncCaratula");
        }
    }
}
