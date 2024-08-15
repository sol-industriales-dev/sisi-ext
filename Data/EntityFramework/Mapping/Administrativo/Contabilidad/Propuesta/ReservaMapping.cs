using Core.Entity.Administrativo.Contabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Propuesta
{
    public class ReservaMapping : EntityTypeConfiguration<tblC_Reserva>
    {
        public ReservaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.cargo).HasColumnName("cargo").HasPrecision(22,4);
            Property(x => x.abono).HasColumnName("abono").HasPrecision(22, 4);
            Property(x => x.porcentaje).HasColumnName("porcentaje").HasPrecision(22, 4);
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.ultimoCambio).HasColumnName("ultimoCambio");
            ToTable("tblC_Reserva");
        }
    }
}
