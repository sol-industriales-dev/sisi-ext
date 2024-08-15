using Core.Entity.Administrativo.Contabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad
{
    public class CatNumNafinMapping : EntityTypeConfiguration<tblC_CatNumNafin>
    {
        public CatNumNafinMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.NumProveedor).HasColumnName("NumProveedor");
            Property(x => x.NumNafin).HasColumnName("NumNafin");
            Property(x => x.TipoMoneda).HasColumnName("TipoMoneda");
            Property(x => x.RFC).HasColumnName("RFC");
            Property(x => x.RazonSocial).HasColumnName("RazonSocial");
            Property(x => x.correo).HasColumnName("correo");
            Property(x => x.idTipoPropuesta).HasColumnName("idTipoPropuesta");
            Property(x => x.estatus).HasColumnName("estatus");
            ToTable("tblC_CatNumNafin");
        }
    }
}
