using Core.Entity.Administrativo.Contabilidad.Sistema_Contable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.SistemaContable
{
    public class tblC_CP_CuentasProveedoresMapping : EntityTypeConfiguration<tblC_CP_CuentasProveedores>
    {
        public tblC_CP_CuentasProveedoresMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cta).HasColumnName("cta");
            Property(x => x.scta).HasColumnName("scta");
            Property(x => x.sscta).HasColumnName("sscta");
            Property(x => x.moneda).HasColumnName("moneda");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");
            Property(x => x.esActivo).HasColumnName("esActivo");
            ToTable("tblC_CP_CuentasProveedores");
        }
    }
}

