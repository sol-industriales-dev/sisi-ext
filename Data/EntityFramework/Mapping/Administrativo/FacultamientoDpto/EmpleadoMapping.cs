using Core.Entity.Administrativo.FacultamientosDpto;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.Administrativo.FacultamientoDpto
{
    public class EmpleadoMapping : EntityTypeConfiguration<tblFA_Empleado>
    {
        public EmpleadoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombreEmpleado).HasColumnName("nombreEmpleado");
            Property(x => x.claveEmpleado).HasColumnName("claveEmpleado");
            Property(x => x.conceptoID).HasColumnName("conceptoID");
            HasRequired(x => x.concepto).WithMany(x => x.empleados).HasForeignKey(d => d.conceptoID);
            Property(x => x.facultamientoID).HasColumnName("facultamientoID");
            HasRequired(x => x.facultamiento).WithMany(x => x.empleados).HasForeignKey(d => d.facultamientoID);
            Property(x => x.editado).HasColumnName("editado");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.aplica).HasColumnName("aplica");
            ToTable("tblFA_Empleado");
        }
    }
}