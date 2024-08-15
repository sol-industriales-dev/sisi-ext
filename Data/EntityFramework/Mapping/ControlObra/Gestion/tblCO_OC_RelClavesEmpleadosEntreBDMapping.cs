using Core.Entity.ControlObra.GestionDeCambio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.ControlObra.Gestion
{
    public class tblCO_OC_RelClavesEmpleadosEntreBDMapping : EntityTypeConfiguration<tblCO_OC_RelClavesEmpleadosEntreBD>
    {
        public tblCO_OC_RelClavesEmpleadosEntreBDMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.idUsuarioSigoplan).HasColumnName("idUsuarioSigoplan");
            Property(x => x.idUsuarioSubcontratista).HasColumnName("idUsuarioSubcontratista");
            Property(x => x.claveEmpleadoEncontrol).HasColumnName("claveEmpleadoEncontrol");

            ToTable("tblCO_OC_RelClavesEmpleadosEntreBD");
        }
    }
}
