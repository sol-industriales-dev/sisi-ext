using Core.Entity.Administrativo.Contabilidad.Propuesta.Proveedores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Propuesta
{
    public class EstimacionProveedorMapping : EntityTypeConfiguration<tblC_EstimacionProveedor>
    {
        public EstimacionProveedorMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idGiro).HasColumnName("idGiro");
            Property(x => x.idEst).HasColumnName("idEst");
            Property(x => x.numpro).HasColumnName("numpro");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.comentarios).HasColumnName("comentarios");
            Property(x => x.total).HasColumnName("total").HasPrecision(22,4);
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            ToTable("tblC_EstimacionProveedor");
        }
    }
}
