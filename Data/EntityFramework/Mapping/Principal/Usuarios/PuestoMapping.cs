using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Data.EntityFramework.Mapping.Principal.Usuarios
{
    public class PuestoMapping : EntityTypeConfiguration<tblP_Puesto>
    {
        public PuestoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.departamentoID).HasColumnName("departamentoID");
            HasRequired(x => x.departamento).WithMany(x => x.puestos).HasForeignKey(y => y.departamentoID);
            Property(x => x.nivel).HasColumnName("nivel");
            Property(x => x.puestoPadreID).HasColumnName("puestoPadreID");

            ToTable("tblP_Puesto");
        }
    }
}
