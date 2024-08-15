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
    class PMComponenteModeloMapping : EntityTypeConfiguration<tblM_PMComponenteModelo>
    {
        public PMComponenteModeloMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.componenteID).HasColumnName("componenteID");
            Property(x => x.modeloID).HasColumnName("modeloID");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.usuariosCaptura).HasColumnName("usuariosCaptura");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");

            HasRequired(x => x.Componente).WithMany().HasForeignKey(y => y.componenteID);
            HasRequired(x => x.Modelo).WithMany().HasForeignKey(y => y.modeloID);


            ToTable("tblM_PMComponenteModelo");

        }
    }
}
