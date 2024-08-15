using Core.Entity.Administrativo.Contratistas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contratistas
{
    public class AtblC_EmpleadosContratistasMapping : EntityTypeConfiguration<tblS_IncidentesEmpleadoContratistas>
    {
        public AtblC_EmpleadosContratistasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idEmpresaContratista).HasColumnName("idEmpresaContratista");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.apePaterno).HasColumnName("apePaterno");
            Property(x => x.apeMaterno).HasColumnName("apeMaterno");
            Property(x => x.domicilio).HasColumnName("domicilio");
            Property(x => x.colonia).HasColumnName("colonia");
            Property(x => x.idPais).HasColumnName("idPais");
            Property(x => x.idEstado).HasColumnName("idEstado");
            Property(x => x.idCiudad).HasColumnName("idCiudad");
            Property(x => x.codigoPostal).HasColumnName("codigoPostal");
            Property(x => x.UMF).HasColumnName("UMF");
            Property(x => x.sexo).HasColumnName("sexo");
            Property(x => x.localidadNacimiento).HasColumnName("localidadNacimiento");
            Property(x => x.estadoNacimiento).HasColumnName("estadoNacimiento");
            Property(x => x.numeroSeguroSocial).HasColumnName("numeroSeguroSocial");
            Property(x => x.numeroDeIdentificacionOficial).HasColumnName("numeroDeIdentificacionOficial");
            Property(x => x.rfc).HasColumnName("rfc");
            Property(x => x.curp).HasColumnName("curp");
            Property(x => x.nombrePadre).HasColumnName("nombrePadre");
            Property(x => x.nombreMadre).HasColumnName("nombreMadre");
            Property(x => x.nombreEspo).HasColumnName("nombreEspo");
            Property(x => x.beneficiario).HasColumnName("beneficiario");
            Property(x => x.parentesco).HasColumnName("parentesco");
            Property(x => x.calzado).HasColumnName("calzado");
            Property(x => x.tipoSangre).HasColumnName("tipoSangre");
            Property(x => x.estadoCivil).HasColumnName("estadoCivil");
            Property(x => x.tallaCamisa).HasColumnName("tallaCamisa");
            Property(x => x.alergias).HasColumnName("alergias");
            Property(x => x.tipoVivienda).HasColumnName("tipoVivienda");
            Property(x => x.tallaPantalon).HasColumnName("tallaPantalon");
            Property(x => x.overoll).HasColumnName("overoll");
            Property(x => x.hijos).HasColumnName("hijos");
            Property(x => x.edades).HasColumnName("edades");
            Property(x => x.telefono).HasColumnName("telefono");
            Property(x => x.celular).HasColumnName("celular");
            Property(x => x.puesto).HasColumnName("puesto");
            Property(x => x.centroDeCostos).HasColumnName("centroDeCostos");
            Property(x => x.jefeInmediato).HasColumnName("jefeInmediato");
            Property(x => x.sueldoBase).HasColumnName("sueldoBase");
            Property(x => x.complento).HasColumnName("complento");
            Property(x => x.total).HasColumnName("total");
            Property(x => x.esActivo).HasColumnName("esActivo");


            ToTable("tblS_IncidentesEmpleadoContratistas");
        }
    }
}
