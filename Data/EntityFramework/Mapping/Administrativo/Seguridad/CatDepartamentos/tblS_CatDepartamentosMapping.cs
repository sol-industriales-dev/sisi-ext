using Core.Entity.Administrativo.Seguridad.Indicadores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Indicadores
{
    public class tblS_CatDepartamentosMapping : EntityTypeConfiguration<tblS_CatDepartamentos>
    {
        public tblS_CatDepartamentosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.clave_depto).HasColumnName("clave_depto");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.idAreaOperativa).HasColumnName("idAreaOperativa");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.idEmpresa).HasColumnName("idEmpresa");

            //HasRequired(x => x.catAreasOperativas).WithMany().HasForeignKey(y => y.idAreaOperativa);
            ToTable("tblS_CatDepartamentos");
        }
    }
}
