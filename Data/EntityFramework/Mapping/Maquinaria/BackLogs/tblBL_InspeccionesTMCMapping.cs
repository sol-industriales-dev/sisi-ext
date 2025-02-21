﻿using Core.Entity.Maquinaria.BackLogs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.BackLogs
{
    public class tblBL_InspeccionesTMCMapping : EntityTypeConfiguration<tblBL_InspeccionesTMC>
    {
        public tblBL_InspeccionesTMCMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.areaCuenta).HasColumnName("areaCuenta");
            Property(x => x.periodo).HasColumnName("periodo");
            Property(x => x.noEconomico).HasColumnName("noEconomico");
            Property(x => x.horas).HasColumnName("horas");
            Property(x => x.idCatMaquina).HasColumnName("idCatMaquina");
            Property(x => x.esRehabilitar).HasColumnName("esRehabilitar");
            Property(x => x.idMotivo).HasColumnName("idMotivo");
            Property(x => x.fechaRequerido).HasColumnName("fechaRequerido");
            Property(x => x.fechaCreacionInsp).HasColumnName("fechaCreacionInsp");
            Property(x => x.fechaModificacionInsp).HasColumnName("fechaModificacionInsp");
            Property(x => x.esActivo).HasColumnName("esActivo");

            HasRequired(x => x.lstCatMaquinas).WithMany().HasForeignKey(y => y.idCatMaquina);

            ToTable("tblBL_InspeccionesTMC");
        }
    }
}
