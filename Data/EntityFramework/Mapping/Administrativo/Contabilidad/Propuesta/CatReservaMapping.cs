using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Propuesta
{
    public class CatReservaMapping : EntityTypeConfiguration<tblC_CatReserva>
    {
        public CatReservaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.esAutomatico).HasColumnName("esAutomatico");
            Property(x => x.esSeleccionado).HasColumnName("esSeleccionado");
            Property(x => x.tipoReservaSaldoGlobal).HasColumnName("tipoReservaSaldoGlobal");
            Property(x => x.hexColor).HasColumnName("hexColor");
            Property(x => x.esPrincipal).HasColumnName("esPrincipal");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            ToTable("tblC_CatReserva");
        }
    }
}
