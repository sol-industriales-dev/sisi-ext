using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Captura
{
    public class FormatoCambioMapping : EntityTypeConfiguration<tblRH_FormatoCambio>
    {
        public FormatoCambioMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.folio).HasColumnName("Folio");
            Property(x => x.Clave_Empleado).HasColumnName("Clave_Empleado");
            Property(x => x.Nombre).HasColumnName("Nombre");
            Property(x => x.Ape_Paterno).HasColumnName("Ape_Paterno");
            Property(x => x.Ape_Materno).HasColumnName("Ape_Materno");
            Property(x => x.Fecha_Alta).HasColumnName("Fecha_Alta");
            Property(x => x.PuestoID).HasColumnName("PuestoID");
            Property(x => x.Puesto).HasColumnName("Puesto");
            Property(x => x.TipoNominaID).HasColumnName("TipoNominaID");
            Property(x => x.TipoNomina).HasColumnName("TipoNomina");
            Property(x => x.CcID).HasColumnName("CcID");
            Property(x => x.CC).HasColumnName("CC");
            Property(x => x.RegistroPatronalID).HasColumnName("RegistroPatronalID");
            Property(x => x.RegistroPatronal).HasColumnName("RegistroPatronal");
            Property(x => x.Clave_Jefe_Inmediato).HasColumnName("Clave_Jefe_Inmediato");
            Property(x => x.Nombre_Jefe_Inmediato).HasColumnName("Nombre_Jefe_Inmediato");
            Property(x => x.Salario_Base).HasColumnName("Salario_Base");
            Property(x => x.CamposCambiados).HasColumnName("CamposCambiados");
            Property(x => x.Bono).HasColumnName("Bono");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            //Property(x => x.Genero).HasColumnName("Genero");
            //Property(x => x.FechaNac).HasColumnName("FechaNac");
            //Property(x => x.ClaveCdNac).HasColumnName("ClaveCdNac");
            //Property(x => x.ClaveEstadoNac).HasColumnName("ClaveEstadoNac");
            //Property(x => x.ClavePaisNac).HasColumnName("ClavePaisNac");
            //Property(x => x.Rfc).HasColumnName("Rfc");
            //Property(x => x.Curp).HasColumnName("Curp");
            //Property(x => x.EstatusEmpleado).HasColumnName("EstatusEmpleado");
            //Property(x => x.ClaveDepto).HasColumnName("ClaveDepto");
            //Property(x => x.ClaveRegistro).HasColumnName("ClaveRegistro");
            //Property(x => x.Sindicato).HasColumnName("Sindicato");
            //Property(x => x.ClaveSalario).HasColumnName("ClaveSalario");
            //Property(x => x.RecibePtu).HasColumnName("RecibePtu");
            //Property(x => x.EstadoDeclara).HasColumnName("EstadoDeclara");
            //Property(x => x.HonorarioAsimilable).HasColumnName("HonorarioAsimilable");
            //Property(x => x.ClaveTurno).HasColumnName("ClaveTurno");
            //Property(x => x.Banco).HasColumnName("Banco");
            //Property(x => x.NumCtaPago).HasColumnName("NumCtaPago");
            //Property(x => x.NumCtaFondoAho).HasColumnName("NumCtaFondoAho");
            //Property(x => x.Nss).HasColumnName("Nss");
            //Property(x => x.UnidadMedica).HasColumnName("UnidadMedica");
            //Property(x => x.TipoFormulaImss).HasColumnName("TipoFormulaImss");
            //Property(x => x.DiasPeriodo).HasColumnName("DiasPeriodo");
            //Property(x => x.GrupoImss).HasColumnName("GrupoImss");
            //Property(x => x.SdiInfonavit).HasColumnName("SdiInfonavit");
            //Property(x => x.BaseVarImss).HasColumnName("BaseVarImss");
            //Property(x => x.BaseVarInf).HasColumnName("BaseVarInf");
            //Property(x => x.Codigo).HasColumnName("Codigo");
            //Property(x => x.IdseAltas).HasColumnName("IdseAltas");
            //Property(x => x.IdseBajas).HasColumnName("IdseBajas");
            //Property(x => x.IdseCambios).HasColumnName("IdseCambios");
            //Property(x => x.Numpro).HasColumnName("Numpro");
            //Property(x => x.SueldoNeto).HasColumnName("SueldoNeto");
            //Property(x => x.RecibeDespensa).HasColumnName("RecibeDespensa");
            //Property(x => x.Contratable).HasColumnName("Contratable");
            //Property(x => x.JefeCuadrilla).HasColumnName("JefeCuadrilla");
            //Property(x => x.AsistDiaria).HasColumnName("AsistDiaria");
            //Property(x => x.SubsidioEmpleo).HasColumnName("SubsidioEmpleo");
            //Property(x => x.TipoCuentaPago).HasColumnName("TipoCuentaPago");
            //Property(x => x.Tabulador).HasColumnName("Tabulador");
            //Property(x => x.DuracionContrato).HasColumnName("DuracionContrato");
            //Property(x => x.Solicitud).HasColumnName("Solicitud");
            //Property(x => x.TipoFirma).HasColumnName("TipoFirma");
            //Property(x => x.Requisicion).HasColumnName("Requisicion");
            //Property(x => x.ArchivoEnviado).HasColumnName("ArchivoEnviado");
            //Property(x => x.IdContratoEmpleado).HasColumnName("IdContratoEmpleado");
            //Property(x => x.LocalidadNacimiento).HasColumnName("LocalidadNacimiento");
            //Property(x => x.FormatoContrato).HasColumnName("FormatoContrato");
            //Property(x => x.DescPuesto).HasColumnName("DescPuesto");
            //Property(x => x.Autoriza).HasColumnName("Autoriza");
            //Property(x => x.VistoBueno).HasColumnName("VistoBueno");
            //Property(x => x.UsuarioCompras).HasColumnName("UsuarioCompras");
            //Property(x => x.ClaveInterbancaria).HasColumnName("ClaveInterbancaria");
            //Property(x => x.FechaContrato).HasColumnName("FechaContrato");
            //Property(x => x.IdExpediente).HasColumnName("IdExpediente");
            //Property(x => x.SolicitaTarjeta).HasColumnName("SolicitaTarjeta");
            Property(x => x.SalarioAnt).HasColumnName("SalarioAnt");
            Property(x => x.ComplementoAnt).HasColumnName("ComplementoAnt");
            Property(x => x.BonoAnt).HasColumnName("BonoAnt");
            Property(x => x.CCAntID).HasColumnName("CCAntID");
            Property(x => x.CCAnt).HasColumnName("CCAnt");
            Property(x => x.PuestoAnt).HasColumnName("PuestoAnt");
            Property(x => x.RegistroPatronalAnt).HasColumnName("RegistroPatronalAnt");
            Property(x => x.Nombre_Jefe_InmediatoAnt).HasColumnName("Nombre_Jefe_InmediatoAnt");
            Property(x => x.TipoNominaAnt).HasColumnName("TipoNominaAnt");
            ToTable("tblRH_FormatoCambio");
        }
    }
}
