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
    public class tblCO_OrdenDeCambioMapping : EntityTypeConfiguration<tblCO_OrdenDeCambio>
    {
        public tblCO_OrdenDeCambioMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.fechaEfectiva).HasColumnName("fechaEfectiva");
            Property(x => x.Proyecto).HasColumnName("Proyecto");
            Property(x => x.CLiente).HasColumnName("CLiente");
            Property(x => x.Contratista).HasColumnName("Contratista");
            Property(x => x.Direccion).HasColumnName("Direccion");
            Property(x => x.NoOrden).HasColumnName("NoOrden");
            Property(x => x.esCobrable).HasColumnName("esCobrable");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.Antecedentes).HasColumnName("Antecedentes");
            Property(x => x.idSubContratista).HasColumnName("idSubContratista");
            Property(x => x.status).HasColumnName("status");
            Property(x => x.voboPMO).HasColumnName("voboPMO");
            Property(x => x.idContrato).HasColumnName("idContrato");
            Property(x => x.ubicacionProyecto).HasColumnName("ubicacionProyecto");
            Property(x => x.otrasCondicioes).HasColumnName("otrasCondicioes");
            Property(x => x.fechaVobo1).HasColumnName("fechaVobo1");
            Property(x => x.fechaVobo2).HasColumnName("fechaVobo2");
            
            

            ToTable("tblCO_OrdenDeCambio");
        }
    }
}
