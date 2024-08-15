using Core.Entity.Administrativo.Contabilidad.Nomina;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_AFPtblRH_EK_EmpleadosMapping : EntityTypeConfiguration<tblC_Nom_AFPtblRH_EK_Empleados>
    {
        public tblC_Nom_AFPtblRH_EK_EmpleadosMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.afpID).HasColumnName("afpID");
            Property(x => x.clave_empleado).HasColumnName("clave_empleado");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            Property(x => x.usuarioRegistro).HasColumnName("usuarioRegistro");
            Property(x => x.fechaModifica).HasColumnName("fechaModifica");
            Property(x => x.usuarioModifica).HasColumnName("usuarioModifica");
            Property(x => x.aplicaFamiliar).HasColumnName("aplicaFamiliar");
            Property(x => x.registroActivo).HasColumnName("registroActivo");
            ToTable("tblC_Nom_AFPtblRH_EK_Empleados");
        }
    }
}