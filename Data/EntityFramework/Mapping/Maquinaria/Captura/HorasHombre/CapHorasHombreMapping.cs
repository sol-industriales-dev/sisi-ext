using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura.HorasHombre
{
    public class CapHorasHombreMapping : EntityTypeConfiguration<tblM_CapHorasHombre>
    {
        public CapHorasHombreMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombreEmpleado).HasColumnName("nombreEmpleado");
            Property(x => x.numEmpleado).HasColumnName("numEmpleado");
            Property(x => x.categoriaTrabajo).HasColumnName("categoriaTrabajo");
            Property(x => x.subCategoria).HasColumnName("subCategoria");
            Property(x => x.tiempo).HasColumnName("tiempo");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.usuarioCapturaID).HasColumnName("usuarioCapturaID");
            Property(x => x.centroCostos).HasColumnName("centroCostos");
            Property(x => x.turno).HasColumnName("turno");
            Property(x => x.puestoID).HasColumnName("puestoID");

            ToTable("tblM_CapHorasHombre");
        }
    }
}
