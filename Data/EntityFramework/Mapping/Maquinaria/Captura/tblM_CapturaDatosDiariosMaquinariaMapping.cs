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
    class tblM_CapturaDatosDiariosMaquinariaMapping : EntityTypeConfiguration<tblM_CapturaDatosDiariosMaquinaria>
    {
        public tblM_CapturaDatosDiariosMaquinariaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.fechaCapturaMaquinaria).HasColumnName("fechaCapturaMaquinaria");
            Property(x => x.idCatMaquina).HasColumnName("idCatMaquina");
            Property(x => x.FechaPatioMaquinaria).HasColumnName("FechaPatioMaquinaria");
            Property(x => x.FechaTMC).HasColumnName("FechaTMC");
            Property(x => x.FechaMaquinaria).HasColumnName("FechaMaquinaria");
            Property(x => x.idEstatus).HasColumnName("idEstatus");
            Property(x => x.Observaciones).HasColumnName("Observaciones");
            Property(x => x.Enviado).HasColumnName("Enviado");
            

            //HasRequired(x => x.Maquina).WithMany().HasForeignKey(y => y.idCatMaquina);




            ToTable("tblM_CapturaDatosDiariosMaquinaria");
        }
    }
}
