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
    public class PMComponenteFiltroMapping : EntityTypeConfiguration<tblM_PMComponenteFiltro>
    {
        public PMComponenteFiltroMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.componenteID).HasColumnName("componenteID");
            Property(x => x.cantidad).HasColumnName("cantidad");
            Property(x => x.filtroID).HasColumnName("filtroID");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.modeloID).HasColumnName("modeloID");


            ToTable("tblM_PMComponenteFiltro");

        }
    }
}
