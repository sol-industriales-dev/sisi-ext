using Core.Entity.RecursosHumanos.Reclutamientos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_EntrevistasInicialesMapping : EntityTypeConfiguration<tblRH_REC_EntrevistasIniciales>
    {
        public tblRH_REC_EntrevistasInicialesMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idCandidato).HasColumnName("idCandidato");
            Property(x => x.idEscolaridad).HasColumnName("idEscolaridad");
            Property(x => x.estadoCivil).HasColumnName("estadoCivil");
            Property(x => x.lugarNacimiento).HasColumnName("lugarNacimiento");
            Property(x => x.expectativaSalarial).HasColumnName("expectativaSalarial");
            Property(x => x.puestoSolicitado).HasColumnName("puestoSolicitado");
            Property(x => x.expLaboral).HasColumnName("expLaboral");
            Property(x => x.sectorCiudad).HasColumnName("sectorCiudad");
            Property(x => x.tiempoEnLaCiudad).HasColumnName("tiempoEnLaCiudad");
            Property(x => x.entrevistasAnteriores).HasColumnName("entrevistasAnteriores");
            Property(x => x.idPlataforma).HasColumnName("idPlataforma");
            Property(x => x.documentacion).HasColumnName("documentacion");
            Property(x => x.familiarEnEmpresa).HasColumnName("familiarEnEmpresa");
            Property(x => x.telefono).HasColumnName("telefono");
            Property(x => x.familia).HasColumnName("familia");
            Property(x => x.empleos).HasColumnName("empleos");
            Property(x => x.caracteristicasCandidato).HasColumnName("caracteristicasCandidato");
            Property(x => x.comentarioEntrevistador).HasColumnName("comentarioEntrevistador");
            Property(x => x.fechaEntrevista).HasColumnName("fechaEntrevista");
            Property(x => x.disposicionHorario).HasColumnName("disposicionHorario");
            Property(x => x.avanza).HasColumnName("avanza");
            Property(x => x.idUsuarioEntrevisto).HasColumnName("idUsuarioEntrevisto");
            Property(x => x.resultado).HasColumnName("resultado");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");

            ToTable("tblRH_REC_EntrevistasIniciales");
        }
    }
}
