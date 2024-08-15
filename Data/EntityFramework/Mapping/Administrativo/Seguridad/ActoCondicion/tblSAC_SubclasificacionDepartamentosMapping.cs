using Core.Entity.Administrativo.Seguridad.ActoCondicion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.ActoCondicion
{
    public class tblSAC_SubclasificacionDepartamentosMapping : EntityTypeConfiguration<tblSAC_SubclasificacionDepartamentos>
    {
        public tblSAC_SubclasificacionDepartamentosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.subclasificacionDep).HasColumnName("subclasificacionDep");
            Property(x => x.idDepartamento).HasColumnName("idDepartamento");
            Property(x => x.registroActivo).HasColumnName("registroActivo");
            ToTable("tblSAC_SubclasificacionDepartamentos");
        }
    }
}