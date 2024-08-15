using Core.Entity.Encuestas.Proveedores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Encuestas.Proveedores
{
    public class tblEN_ProvCorreosNotificacionMapping : EntityTypeConfiguration<tblEN_ProvCorreosNotificacion>
    {
        public tblEN_ProvCorreosNotificacionMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Correo).HasColumnName("correo");
            Property(x => x.motivoNotificacion).HasColumnName("motivoNotificacion");
            Property(x => x.Estatus).HasColumnName("estatus");
            ToTable("tblEN_ProvCorreosNotificacion");
        }
    }
}
