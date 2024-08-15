using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.BackLogs;

namespace Data.EntityFramework.Mapping.Maquinaria.BackLogs
{
    public class tblBL_SeguimientoPptosMapping : EntityTypeConfiguration<tblBL_SeguimientoPptos>
    {
        public tblBL_SeguimientoPptosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.areaCuenta).HasColumnName("areaCuenta");
            Property(x => x.folioPpto).HasColumnName("folioPpto");
            Property(x => x.consecutivoFolio).HasColumnName("consecutivoFolio");
            Property(x => x.fechaPpto).HasColumnName("fechaPpto");
            Property(x => x.noEconomico).HasColumnName("noEconomico");
            Property(x => x.idCatMaquina).HasColumnName("idCatMaquina");
            Property(x => x.horas).HasColumnName("horas");        
            Property(x => x.Ppto).HasColumnName("ppto");
            Property(x => x.idInspTMC).HasColumnName("idInspTMC");
            Property(x => x.esVobo1).HasColumnName("esVobo1");
            Property(x => x.fechaVobo1).HasColumnName("fechaVobo1");
            Property(x => x.comentRechaVobo1).HasColumnName("comentRechaVobo1");
            Property(x => x.esVobo2).HasColumnName("esVobo2");
            Property(x => x.fechaVobo2).HasColumnName("fechaVobo2");
            Property(x => x.comentRechaVobo2).HasColumnName("comentRechaVobo2");
            Property(x => x.esAutorizado).HasColumnName("esAutorizado");
            Property(x => x.fechaAutorizado).HasColumnName("fechaAutorizado");
            Property(x => x.comentRechaAutorizado).HasColumnName("comentRechaAutorizado");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.EstatusSegPpto).HasColumnName("EstatusSegPpto");
            Property(x => x.idFrente).HasColumnName("idFrentes");
            Property(x => x.firmaVobo1).HasColumnName("firmaVobo1");
            Property(x => x.idPuestoVobo1).HasColumnName("idPuestoVobo1");
            Property(x => x.idUserVobo1).HasColumnName("idUserVobo1");
            Property(x => x.firmaVobo2).HasColumnName("firmaVobo2");
            Property(x => x.idUserVobo2).HasColumnName("idUserVobo2");
            Property(x => x.firmaAutorizado).HasColumnName("firmaAutorizado");
            Property(x => x.idUserAutorizado).HasColumnName("idUserAutorizado");

            HasRequired(x => x.lstCatFrentes).WithMany().HasForeignKey(y => y.idFrente);
            HasRequired(x => x.lstInspeccionesTMC).WithMany().HasForeignKey(y => y.idInspTMC);
            HasRequired(x => x.lstCatMaquinas).WithMany().HasForeignKey(y => y.idCatMaquina);

            ToTable("tblBL_SeguimientoPptos");
        }
    }
}
