using Core.Entity.Maquinaria.Mantenimiento2;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Mantenimiento.Agrupaciones
{
    public class PMComponenteLubricanteMapping : EntityTypeConfiguration<tblM_PMComponenteLubricante>
    {
        public PMComponenteLubricanteMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.componenteID).HasColumnName("componenteID");
            Property(x => x.lubricanteID).HasColumnName("lubricanteID");
            Property(x => x.modeloID).HasColumnName("modeloID");
            Property(x => x.vidaLubricante).HasColumnName("vidaLubricante");
            Property(x => x.cantidadLitros).HasColumnName("cantidadLitros");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblM_PMComponenteLubricante");

        }
    }
}
