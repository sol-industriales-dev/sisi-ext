using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Captura
{
    public class tblRH_BN_Plantilla_Cuadrado_DetMapping : EntityTypeConfiguration<tblRH_BN_Plantilla_Cuadrado_Det>
    {
        public tblRH_BN_Plantilla_Cuadrado_DetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.plantillaID).HasColumnName("plantillaID");
            Property(x => x.puesto).HasColumnName("puesto");
            Property(x => x.puestoNombre).HasColumnName("puestoNombre");
            Property(x => x.periodicidad).HasColumnName("periodicidad");
            Property(x => x.monto).HasColumnName("monto");
            Property(x => x.tipoNominaCve).HasColumnName("tipoNominaCve");
            Property(x => x.depto).HasColumnName("depto");
            Property(x => x.deptoNombre).HasColumnName("deptoNombre");
            Property(x => x.empleado).HasColumnName("empleado");
            Property(x => x.empleadoNombre).HasColumnName("empleadoNombre");
            HasRequired(x => x.plantilla).WithMany().HasForeignKey(y => y.plantillaID);

            ToTable("tblRH_BN_Plantilla_Cuadrado_Det");
        }
    }

}
