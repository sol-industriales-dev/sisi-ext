using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.RecursosHumanos.Captura;
using Core.Entity.RecursosHumanos.Catalogo;
using Core.Enum.RecursosHumanos;
using Core.DAO.RecursosHumanos.Captura;
using Data.EntityFramework.Context;
using Core.Enum.Principal.Bitacoras;
using Core.DTO;
using Data.Factory.Principal.Usuarios;
using Core.DTO.Utils.Data;
using System.Data.Odbc;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Data.DAO.Principal.Usuarios;
using Data.EntityFramework;
using Core.DAO.Enkontrol.General.CC;
using Data.Factory.Enkontrol.General.CC;
using Core.Entity.Administrativo.FacultamientosDpto;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using Core.Entity.Administrativo.RecursosHumanos.Tabuladores;
using Core.DTO.Maquinaria.Captura;

namespace Data.DAO.RecursosHumanos.Captura
{
    public class FormatoCambioDAO : GenericDAO<tblRH_FormatoCambio>, IFormatoCambio
    {
        ICCDAO _ccFS = new CCFactoryService().getCCService();
        ICCDAO _ccFS_SP = new CCFactoryService().getCCServiceSP();
        Dictionary<string, object> resultado = new Dictionary<string, object>();

        public tblRH_FormatoCambio getEmpleadoForId(int idEmpleado,bool activo)
        {

            tblRH_FormatoCambio resultado = new tblRH_FormatoCambio();

            List<tblRH_FormatoCambio> listaEmpleados = new List<tblRH_FormatoCambio>();
            if (activo)
            {
                listaEmpleados = InfEmpleado(idEmpleado);
            }
            else {
                listaEmpleados = InfEmpleadoNoneState(idEmpleado);
            }

            foreach (tblRH_FormatoCambio Emp in listaEmpleados)
            {
                var valid = listaEmpleados.Any(x => x.Clave_Empleado.Equals(idEmpleado));

                if (valid)
                {
                    resultado.Clave_Empleado = Emp.Clave_Empleado;
                    resultado.Nombre = Emp.Nombre;
                    resultado.Ape_Paterno = Emp.Ape_Paterno;
                    resultado.Ape_Materno = Emp.Ape_Materno;
                    resultado.Fecha_Alta = Convert.ToDateTime(Emp.Fecha_Alta);
                    resultado.PuestoID = Emp.PuestoID;
                    resultado.Puesto = Emp.Puesto;
                    resultado.TipoNominaID = Emp.TipoNominaID;
                    resultado.TipoNomina = Emp.TipoNomina;
                    resultado.CcID = Emp.CcID;
                    resultado.CC = Emp.CC;
                    resultado.RegistroPatronalID = Emp.RegistroPatronalID;
                    resultado.RegistroPatronal = Emp.RegistroPatronal;
                    resultado.Clave_Jefe_Inmediato = Emp.Clave_Jefe_Inmediato;
                    resultado.Nombre_Jefe_Inmediato = Emp.Nombre_Jefe_Inmediato;
                    resultado.Salario_Base = Emp.Salario_Base;
                    resultado.Complemento = Emp.Complemento;
                    resultado.Bono = Emp.Bono;
                    resultado.idCategoria = Emp.idCategoria;
                    resultado.descCategoria = Emp.descCategoria;
                    resultado.idLineaNegocios = Emp.idLineaNegocios;
                    resultado.descLineaNegocios = Emp.descLineaNegocios;
                    resultado.Departamento = Emp.Departamento;
                    resultado.ClaveDepartamento = Emp.ClaveDepartamento;
                    resultado.DepartamentoAnterior = Emp.DepartamentoAnterior;
                    resultado.ClaveDepartamentoAnterior = Emp.ClaveDepartamentoAnterior;
                    resultado.idCategoriaAnterior = Emp.idCategoriaAnterior;
                    resultado.descCategoriaAnterior = Emp.descCategoriaAnterior;
                    resultado.idLineaNegociosAnterior = Emp.idLineaNegociosAnterior;
                    resultado.descLineaNegociosAnterior = Emp.descLineaNegociosAnterior;
                    resultado.lowerBase = Emp.lowerBase;
                    resultado.lowerComplemento = Emp.lowerComplemento;
                    resultado.idTabuladorDet = Emp.idTabuladorDet;
                    resultado.esRango = Emp.esRango;
                }
                else
                {
                    resultado.Clave_Empleado = 0;
                    resultado.Nombre = "";
                    resultado.Ape_Paterno = "";
                    resultado.Ape_Materno = "";
                    resultado.Fecha_Alta = DateTime.Now;
                    resultado.PuestoID = 0;
                    resultado.Puesto = "";
                    resultado.TipoNominaID = 0;
                    resultado.TipoNomina = "";
                    resultado.CcID = "";
                    resultado.CC = "";
                    resultado.RegistroPatronalID = 0;
                    resultado.RegistroPatronal = "";
                    resultado.Clave_Jefe_Inmediato = 0;
                    resultado.Nombre_Jefe_Inmediato = "";
                    resultado.Salario_Base = 0;
                    resultado.Complemento = 0;
                    resultado.Bono = 0;
                    resultado.Departamento = "";
                    resultado.ClaveDepartamento = null;
                    resultado.DepartamentoAnterior = "";
                    resultado.ClaveDepartamentoAnterior = null;
                    resultado.lowerBase = 0;
                    resultado.lowerComplemento = 0;
                    resultado.esRango = false;
                }
            }

            return resultado;
        }
        public tblRH_FormatoCambio getFormatoCambioByID(int idFormatoCambio)
        {
            return _context.tblRH_FormatoCambio.FirstOrDefault(x => x.id.Equals(idFormatoCambio));
        }
        public tblRH_FormatoCambio SaveChangesEmpleado(tblRH_FormatoCambio objEmpleado)
        {
            if (objEmpleado.id == 0)
            {
                try
                {
                    SaveEntity(objEmpleado, (int)BitacoraEnum.FORMATOCAMBIORH);
                }
                catch (Exception ex)
                {
                    return new tblRH_FormatoCambio();
                }
            }
            else
            {
                using (var transaccionSP = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var temp = _context.tblRH_FormatoCambio.FirstOrDefault(x => x.id == objEmpleado.id);
                        temp.Clave_Empleado = objEmpleado.Clave_Empleado;
                        temp.Nombre = objEmpleado.Nombre;
                        temp.Ape_Paterno = objEmpleado.Ape_Paterno;
                        temp.Ape_Materno = objEmpleado.Ape_Materno;
                        temp.Fecha_Alta = objEmpleado.Fecha_Alta;
                        temp.PuestoID = objEmpleado.PuestoID;
                        temp.Puesto = objEmpleado.Puesto;
                        temp.TipoNominaID = objEmpleado.TipoNominaID;
                        temp.TipoNomina = objEmpleado.TipoNomina;
                        temp.CcID = objEmpleado.CcID;
                        temp.CC = objEmpleado.CC;
                        temp.RegistroPatronalID = objEmpleado.RegistroPatronalID;
                        temp.RegistroPatronal = objEmpleado.RegistroPatronal;
                        temp.Clave_Jefe_Inmediato = objEmpleado.Clave_Jefe_Inmediato;
                        temp.Nombre_Jefe_Inmediato = objEmpleado.Nombre_Jefe_Inmediato;
                        temp.Salario_Base = objEmpleado.Salario_Base;
                        temp.Complemento = objEmpleado.Complemento;
                        temp.folio = objEmpleado.folio;
                        temp.InicioNomina = objEmpleado.InicioNomina;
                        temp.FechaInicioCambio = objEmpleado.FechaInicioCambio;
                        temp.Justificacion = objEmpleado.Justificacion;
                        temp.CamposCambiados = objEmpleado.CamposCambiados;
                        temp.Aprobado = objEmpleado.Aprobado;
                        temp.Rechazado = objEmpleado.Rechazado;
                        temp.editable = objEmpleado.editable;
                        temp.Bono = objEmpleado.Bono;
                        temp.idCategoria = objEmpleado.idCategoria;
                        temp.descCategoria = objEmpleado.descCategoria;
                        temp.idLineaNegocios = objEmpleado.idLineaNegocios;
                        temp.descLineaNegocios = objEmpleado.descLineaNegocios;
                        temp.Departamento = objEmpleado.Departamento;
                        temp.ClaveDepartamento = objEmpleado.ClaveDepartamento;
                        temp.DepartamentoAnterior = objEmpleado.DepartamentoAnterior;
                        temp.ClaveDepartamentoAnterior = objEmpleado.ClaveDepartamentoAnterior;
                        temp.idCategoriaAnterior = objEmpleado.idCategoriaAnterior;
                        temp.descCategoriaAnterior = objEmpleado.descCategoriaAnterior;
                        temp.idLineaNegociosAnterior = objEmpleado.idLineaNegociosAnterior;
                        temp.descLineaNegociosAnterior = objEmpleado.descLineaNegociosAnterior;
                        _context.SaveChanges();

                        #region registroEnkontrol

                        //                                if (objEmpleado.Aprobado)
                        //                                {
                        //                                    if (!string.IsNullOrEmpty(temp.CamposCambiados))
                        //                                    {
                        //                                        var yaSeAgregoTabuladorNuevo = false;

                        //                                        var camposCambiados = temp.CamposCambiados.Split(',');
                        //                                        foreach (var campoCambiado in camposCambiados)
                        //                                        {
                        //                                            var cambio = campoCambiado;
                        //                                            if (cambio == "Reg Patronal")
                        //                                            {
                        //                                                cambio = "Registro Patronal";
                        //                                            }
                        //                                            if (cambio == "Bono")
                        //                                            {
                        //                                                cambio = "Sueldo";
                        //                                            }

                        //                                            switch (cambio)
                        //                                            {
                        //                                                #region PUESTO
                        //                                                case "Puesto":
                        //                                                    {
                        //                                                        var query_updatePuesto = string.Format(
                        //                                                            @"UPDATE sn_empleados SET
                        //                                                                puesto = ?
                        //                                                            WHERE clave_empleado = ?");
                        //                                                        using (var cmd = new OdbcCommand(query_updatePuesto))
                        //                                                        {
                        //                                                            cmd.Parameters.Clear();
                        //                                                            var parametros = cmd.Parameters;
                        //                                                            parametros.Add("@puesto", OdbcType.Int).Value = temp.PuestoID;
                        //                                                            parametros.Add("@clave_empleado", OdbcType.Int).Value = temp.Clave_Empleado;

                        //                                                            cmd.Connection = tranEK.Connection;
                        //                                                            cmd.Transaction = tranEK;
                        //                                                            cmd.ExecuteNonQuery();
                        //                                                        }

                        //                                                        var ultimoIdPuestoHistorial = _contextEnkontrol.Select<dynamic>(vSesiones.sesionAmbienteEnkontrolRh, new OdbcConsultaDTO
                        //                                                        {
                        //                                                            consulta = "SELECT TOP 1 id, (SELECT TOP 1 puesto FROM sn_empl_puestos_historial WHERE clave_empleado = ? ORDER BY id DESC) AS puesto FROM sn_empl_puestos_historial ORDER BY id DESC",
                        //                                                            parametros = new List<OdbcParameterDTO>
                        //                                                            {
                        //                                                                new OdbcParameterDTO { nombre = "clave_empleado", tipo = OdbcType.Int, valor = temp.Clave_Empleado }
                        //                                                            }
                        //                                                        }).FirstOrDefault();
                        //                                                        var siguienteIdPuestoHistorial = ultimoIdPuestoHistorial != null ? (int)ultimoIdPuestoHistorial.id + 1 : 1;

                        //                                                        var query_insertPuestoHistorial = string.Format(
                        //                                                            @"INSERT INTO sn_empl_puestos_historial (
                        //                                                                id,
                        //                                                                clave_empleado,
                        //                                                                puesto,
                        //                                                                puesto_anterior,
                        //                                                                fecha_cambio,
                        //                                                                hora
                        //                                                            ) VALUES (?, ?, ?, ?, ?, ?");
                        //                                                        using (var cmd = new OdbcCommand(query_insertPuestoHistorial))
                        //                                                        {
                        //                                                            cmd.Parameters.Clear();
                        //                                                            var parametros = cmd.Parameters;
                        //                                                            parametros.Add("@id", OdbcType.Int).Value = siguienteIdPuestoHistorial;
                        //                                                            parametros.Add("@clave_empleado", OdbcType.Int).Value = temp.Clave_Empleado;
                        //                                                            parametros.Add("@puesto", OdbcType.Int).Value = temp.PuestoID;
                        //                                                            parametros.Add("@puesto_anterior", OdbcType.Int).Value = ultimoIdPuestoHistorial != null && ultimoIdPuestoHistorial.puesto != null ? ultimoIdPuestoHistorial.puesto : 0;
                        //                                                            parametros.Add("@fecha_cambio", OdbcType.Date).Value = temp.FechaInicioCambio;
                        //                                                            parametros.Add("@hora", OdbcType.DateTime).Value = temp.FechaInicioCambio;

                        //                                                            cmd.Connection = tranEK.Connection;
                        //                                                            cmd.Transaction = tranEK;
                        //                                                            cmd.ExecuteNonQuery();
                        //                                                        }
                        //                                                    }
                        //                                                    break;
                        //                                                #endregion
                        //                                                #region CC
                        //                                                case "CC":
                        //                                                    {
                        //                                                        var query_updateCC = string.Format(
                        //                                                            @"UPDATE sn_empleados SET
                        //                                                                cc_contable = ?
                        //                                                            WHERE clave_empleado = ?");
                        //                                                        using (var cmd = new OdbcCommand(query_updateCC))
                        //                                                        {
                        //                                                            cmd.Parameters.Clear();
                        //                                                            var parametros = cmd.Parameters;
                        //                                                            parametros.Add("@cc_contable", OdbcType.NVarChar).Value = temp.CcID;
                        //                                                            parametros.Add("@clave_empleado", OdbcType.Int).Value = temp.Clave_Empleado;

                        //                                                            cmd.Connection = tranEK.Connection;
                        //                                                            cmd.Transaction = tranEK;
                        //                                                            cmd.ExecuteNonQuery();
                        //                                                        }

                        //                                                        var ultimoCCEmpleado = _contextEnkontrol.Select<dynamic>(vSesiones.sesionAmbienteEnkontrolRh, new OdbcConsultaDTO
                        //                                                        {
                        //                                                            consulta = "SELECT TOP 1 id, (SELECT TOP 1 cc FROM sn_empl_cc_historial WHERE clave_empleado = ? ORDER BY id DESC) AS cc FROM sn_empl_cc_historial ORDER BY id DESC",
                        //                                                            parametros = new List<OdbcParameterDTO>
                        //                                                            {
                        //                                                                new OdbcParameterDTO { nombre = "clave_empleado", tipo = OdbcType.Int, valor = temp.Clave_Empleado }
                        //                                                            }
                        //                                                        }).FirstOrDefault();
                        //                                                        var siguienteIdCCHistorial = ultimoCCEmpleado != null ? (int)ultimoCCEmpleado.id + 1 : 1;

                        //                                                        var query_insertCCHistorial = string.Format(
                        //                                                            @"INSERT INTO sn_empl_cc_historial (
                        //                                                                id,
                        //                                                                clave_empleado,
                        //                                                                cc,
                        //                                                                cc_anterior,
                        //                                                                fecha_cambio,
                        //                                                                hora,
                        //                                                                id_cambio
                        //                                                            ) VALUES (?, ?, ?, ?, ?, ?, ?");
                        //                                                        using (var cmd = new OdbcCommand(query_insertCCHistorial))
                        //                                                        {
                        //                                                            cmd.Parameters.Clear();
                        //                                                            var parametros = cmd.Parameters;
                        //                                                            parametros.Add("@id", OdbcType.Int).Value = siguienteIdCCHistorial;
                        //                                                            parametros.Add("@clave_empleado", OdbcType.Int).Value = temp.Clave_Empleado;
                        //                                                            parametros.Add("@cc", OdbcType.NVarChar).Value = temp.CcID;
                        //                                                            parametros.Add("@cc_anterior", OdbcType.NVarChar).Value = ultimoCCEmpleado != null && ultimoCCEmpleado.cc != null ? ultimoCCEmpleado.cc : "";
                        //                                                            parametros.Add("@fecha_cambio", OdbcType.Date).Value = temp.FechaInicioCambio;
                        //                                                            parametros.Add("@hora", OdbcType.DateTime).Value = temp.FechaInicioCambio;
                        //                                                            parametros.Add("@id_cambio", OdbcType.Int).Value = (object)DBNull.Value;

                        //                                                            cmd.Connection = tranEK.Connection;
                        //                                                            cmd.Transaction = tranEK;
                        //                                                            cmd.ExecuteNonQuery();
                        //                                                        }
                        //                                                    }
                        //                                                    break;
                        //                                                #endregion
                        //                                                #region JEFE INMEDIATO
                        //                                                case "Jefe Inmediato":
                        //                                                    {
                        //                                                        var query_JefeInmediato = string.Format(
                        //                                                            @"UPDATE sn_empleados SET
                        //                                                                jefe_inmediato = ?
                        //                                                            WHERE clave_empleado = ?");
                        //                                                        using (var cmd = new OdbcCommand(query_JefeInmediato))
                        //                                                        {
                        //                                                            cmd.Parameters.Clear();
                        //                                                            var parametros = cmd.Parameters;
                        //                                                            parametros.Add("@jefe_inmediato", OdbcType.Int).Value = temp.Clave_Jefe_Inmediato;
                        //                                                            parametros.Add("@clave_empleado", OdbcType.Int).Value = temp.Clave_Empleado;

                        //                                                            cmd.Connection = tranEK.Connection;
                        //                                                            cmd.Transaction = tranEK;
                        //                                                            cmd.ExecuteNonQuery();
                        //                                                        }
                        //                                                    }
                        //                                                    break;
                        //                                                #endregion
                        //                                                #region REGISTRO PATRONAL
                        //                                                case "Registro Patronal":
                        //                                                    {
                        //                                                        var query_registroPatronal = string.Format(
                        //                                                            @"UPDATE sn_empleados SET
                        //                                                                id_regpat = ?
                        //                                                            WHERE clave_empleado = ?");
                        //                                                        using (var cmd = new OdbcCommand(query_registroPatronal))
                        //                                                        {
                        //                                                            cmd.Parameters.Clear();
                        //                                                            var parametros = cmd.Parameters;
                        //                                                            parametros.Add("@id_regpat", OdbcType.Int).Value = temp.RegistroPatronalID;
                        //                                                            parametros.Add("@clave_empleado", OdbcType.Int).Value = temp.Clave_Empleado;

                        //                                                            cmd.Connection = tranEK.Connection;
                        //                                                            cmd.Transaction = tranEK;
                        //                                                            cmd.ExecuteNonQuery();
                        //                                                        }

                        //                                                        var ultimoRegPatronal = _contextEnkontrol.Select<dynamic>(vSesiones.sesionAmbienteEnkontrolRh, new OdbcConsultaDTO
                        //                                                        {
                        //                                                            consulta = "SELECT TOP 1 id, (SELECT TOP 1 reg_pat FROM sn_empl_reg_pat_historial WHERE clave_empleado = ? ORDER BY id DESC) AS regPat FROM sn_empl_reg_pat_historial ORDER BY id DESC",
                        //                                                            parametros = new List<OdbcParameterDTO>
                        //                                                            {
                        //                                                                new OdbcParameterDTO { nombre = "clave_empleado", tipo = OdbcType.Int, valor = temp.Clave_Empleado }
                        //                                                            }
                        //                                                        }).FirstOrDefault();
                        //                                                        var siguienteIdRegPatronal = ultimoRegPatronal != null ? (int)ultimoRegPatronal.id + 1 : 1;

                        //                                                        var query_insertRegistroPatronal = string.Format(
                        //                                                            @"INSERT INTO sn_empl_reg_pat_historial (
                        //                                                                id,
                        //                                                                clave_empleado,
                        //                                                                reg_pat,
                        //                                                                reg_pat_anterior,
                        //                                                                fecha_cambio,
                        //                                                                hora
                        //                                                            ) VALUES (?, ?, ?, ?, ?, ?");

                        //                                                        using (var cmd = new OdbcCommand(query_insertRegistroPatronal))
                        //                                                        {
                        //                                                            cmd.Parameters.Clear();
                        //                                                            var parametros = cmd.Parameters;
                        //                                                            parametros.Add("@id", OdbcType.Int).Value = siguienteIdRegPatronal;
                        //                                                            parametros.Add("@clave_empleado", OdbcType.Int).Value = temp.Clave_Empleado;
                        //                                                            parametros.Add("@reg_pat", OdbcType.Int).Value = temp.RegistroPatronalID;
                        //                                                            parametros.Add("@reg_pat_anterior", OdbcType.Int).Value = ultimoRegPatronal != null && ultimoRegPatronal.regPat ? (int)ultimoRegPatronal.regPat : 0;
                        //                                                            parametros.Add("@fecha_cambio", OdbcType.Date).Value = temp.FechaInicioCambio;
                        //                                                            parametros.Add("@hora", OdbcType.DateTime).Value = temp.FechaInicioCambio;

                        //                                                            cmd.Connection = tranEK.Connection;
                        //                                                            cmd.Transaction = tranEK;
                        //                                                            cmd.ExecuteNonQuery();
                        //                                                        }
                        //                                                    }
                        //                                                    break;
                        //                                                #endregion
                        //                                                #region SUELDO
                        //                                                case "Sueldo":
                        //                                                    {
                        //                                                        if (!yaSeAgregoTabuladorNuevo)
                        //                                                        {
                        //                                                            var query_updateSueldo = string.Format(
                        //                                                            @"UPDATE sn_empleados SET
                        //                                                                salario_base = ?,
                        //                                                                complemento = ?,
                        //                                                                bono_zona = ?,
                        //                                                                tabulador = 1
                        //                                                            WHERE clave_empleado = ?");
                        //                                                            using (var cmd = new OdbcCommand(query_updateSueldo))
                        //                                                            {
                        //                                                                cmd.Parameters.Clear();
                        //                                                                var parametros = cmd.Parameters;
                        //                                                                parametros.Add("@salario_base", OdbcType.Decimal).Value = temp.Salario_Base;
                        //                                                                parametros.Add("@complemento", OdbcType.Decimal).Value = temp.Complemento;
                        //                                                                parametros.Add("@bono_zona", OdbcType.Decimal).Value = temp.Bono;
                        //                                                                parametros.Add("@clave_empleado", OdbcType.Int).Value = temp.Clave_Empleado;

                        //                                                                cmd.Connection = tranEK.Connection;
                        //                                                                cmd.Transaction = tranEK;
                        //                                                                cmd.ExecuteNonQuery();
                        //                                                            }

                        //                                                            var ultimoTabulador = _contextEnkontrol.Select<dynamic>(vSesiones.sesionAmbienteEnkontrolRh, new OdbcConsultaDTO
                        //                                                            {
                        //                                                                consulta = "SELECT TOP 1 id, (SELECT TOP 1 tabulador FROM sn_tabulador_historial WHERE clave_empleado = ? ORDER BY id DESC) AS tabulador FROM sn_tabulador_historial ORDER BY id DESC",
                        //                                                                parametros = new List<OdbcParameterDTO>
                        //                                                            {
                        //                                                                new OdbcParameterDTO { nombre = "clave_empleado", tipo = OdbcType.Int, valor = temp.Clave_Empleado }
                        //                                                            }
                        //                                                            }).FirstOrDefault();
                        //                                                            var siguienteIdTabulador = ultimoTabulador != null ? (int)ultimoTabulador.id + 1 : 1;

                        //                                                            var query_insertTabulador = string.Format(
                        //                                                            @"INSERT INTO sn_tabulador_historial (
                        //                                                                id,
                        //                                                                clave_empleado,
                        //                                                                tabulador,
                        //                                                                tabulador_anterior,
                        //                                                                fecha_cambio,
                        //                                                                hora,
                        //                                                                suma,
                        //                                                                salario_base,
                        //                                                                complemento,
                        //                                                                bono_zona
                        //                                                            ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?");

                        //                                                            using (var cmd = new OdbcCommand(query_insertTabulador))
                        //                                                            {
                        //                                                                cmd.Parameters.Clear();
                        //                                                                var parametros = cmd.Parameters;
                        //                                                                parametros.Add("@id", OdbcType.Int).Value = siguienteIdTabulador;
                        //                                                                parametros.Add("@clave_empleado", OdbcType.Int).Value = temp.Clave_Empleado;
                        //                                                                parametros.Add("@tabulador", OdbcType.Int).Value = 1;
                        //                                                                parametros.Add("@tabulador_anterior", OdbcType.Int).Value = ultimoTabulador != null && ultimoTabulador.tabulador ? (int)ultimoTabulador.tabulador : 0;
                        //                                                                parametros.Add("@fecha_cambio", OdbcType.Date).Value = temp.FechaInicioCambio;
                        //                                                                parametros.Add("@hora", OdbcType.DateTime).Value = temp.FechaInicioCambio;
                        //                                                                parametros.Add("@suma", OdbcType.Decimal).Value = temp.Salario_Base + temp.Complemento + temp.Bono;
                        //                                                                parametros.Add("@salario_base", OdbcType.Decimal).Value = temp.Salario_Base;
                        //                                                                parametros.Add("@complemento", OdbcType.Decimal).Value = temp.Complemento;
                        //                                                                parametros.Add("@bono_zona", OdbcType.Decimal).Value = temp.Bono;

                        //                                                                cmd.Connection = tranEK.Connection;
                        //                                                                cmd.Transaction = tranEK;
                        //                                                                cmd.ExecuteNonQuery();
                        //                                                            }

                        //                                                            yaSeAgregoTabuladorNuevo = true;
                        //                                                        }
                        //                                                    }
                        //                                                    break;
                        //                                                #endregion
                        //                                                #region TIPO NOMINA
                        //                                                case "Tipo Nomina":
                        //                                                    {
                        //                                                        var query_updateTipoNomina = string.Format(
                        //                                                            @"UPDATE sn_empleados SET
                        //                                                                tipo_nomina = ?
                        //                                                            WHERE clave_empleado = ?");
                        //                                                        using (var cmd = new OdbcCommand(query_updateTipoNomina))
                        //                                                        {
                        //                                                            cmd.Parameters.Clear();
                        //                                                            var parametros = cmd.Parameters;
                        //                                                            parametros.Add("@tipo_nomina", OdbcType.Int).Value = temp.TipoNominaID;
                        //                                                            parametros.Add("@clave_empleado", OdbcType.Int).Value = temp.Clave_Empleado;

                        //                                                            cmd.Connection = tranEK.Connection;
                        //                                                            cmd.Transaction = tranEK;
                        //                                                            cmd.ExecuteNonQuery();
                        //                                                        }
                        //                                                    }
                        //                                                    break;
                        //                                                #endregion
                        //                                            }
                        //                                        }
                        //                                    }

                        //                                    tranEK.Commit();
                        //                                }
                        #endregion

                        transaccionSP.Commit();
                    }
                    catch (Exception ex)
                    {
                        //tranEK.Rollback();
                        transaccionSP.Rollback();

                        return new tblRH_FormatoCambio();
                    }
                }
            }

            return objEmpleado;
        }
        #region Llenado de catalogos
        private List<tblRH_FormatoCambio> InfEmpleado(int idEmpleado)
        {
            //string inf_Empleado = "SELECT Top 1 s.bono_zona As Bono, ";
            //inf_Empleado += "e.clave_empleado,e.nombre,ape_paterno,e.ape_materno,e.fecha_antiguedad as fecha_alta,e.puesto as 'puestoID',p.descripcion as 'puesto',";
            //inf_Empleado += "e.tipo_nomina as 'tipoNominaID',";
            //inf_Empleado += "tn.descripcion as 'tipoNomina',";
            //inf_Empleado += "e.cc_contable as 'ccID',";
            //inf_Empleado += "c.descripcion as 'cc',";
            //inf_Empleado += "e.id_regpat as 'registroPatronalID',";
            //inf_Empleado += "r.nombre_corto as 'registroPatronal',";
            //inf_Empleado += "e.jefe_inmediato as 'clave_jefe_inmediato',";
            //inf_Empleado += "(Select (e2.nombre+' '+e2.ape_paterno+' '+e2.ape_materno) from DBA.sn_empleados as e2 where e2.clave_empleado = e.jefe_inmediato)as 'nombre_jefe_inmediato',";
            //inf_Empleado += "s.salario_base,";
            //inf_Empleado += "s.complemento ";
            //inf_Empleado += "FROM DBA.sn_empleados as e";
            //inf_Empleado += " inner join DBA.si_puestos as p on e.puesto=p.puesto";
            //inf_Empleado += " inner join DBA.sn_tipos_nomina as tn on e.tipo_nomina=tn.tipo_nomina";
            //inf_Empleado += " inner join DBA.cc as c on e.cc_contable=c.cc";
            //inf_Empleado += " inner join DBA.sn_registros_patronales as r on e.id_regpat=r.clave_reg_pat";
            //inf_Empleado += " inner join DBA.sn_tabulador_historial as s on e.clave_empleado=s.clave_empleado";
            //inf_Empleado += " where e.clave_empleado=" + idEmpleado + "AND e.estatus_empleado ='A' ";
            //inf_Empleado += " order by s.id DESC";

            #region V1
            //string inf_Empleado = "SELECT Top 1 s.bono_zona As Bono, ";
            //inf_Empleado += "e.clave_empleado,e.nombre,ape_paterno,e.ape_materno,e.fecha_antiguedad as fecha_alta,e.puesto as 'puestoID',p.descripcion as 'puesto',";
            //inf_Empleado += "e.tipo_nomina as 'tipoNominaID',";
            //inf_Empleado += "tn.descripcion as 'tipoNomina',";
            //inf_Empleado += "e.cc_contable as 'ccID',";
            ////inf_Empleado += "c.descripcion as 'cc',";
            //inf_Empleado += "e.id_regpat as 'registroPatronalID',";
            //inf_Empleado += "r.nombre_corto as 'registroPatronal',";
            //inf_Empleado += "e.jefe_inmediato as 'clave_jefe_inmediato',";
            //inf_Empleado += "(Select (e2.nombre+' '+e2.ape_paterno+' '+e2.ape_materno) from tblRH_EK_Empleados as e2 where e2.clave_empleado = e.jefe_inmediato)as 'nombre_jefe_inmediato',";
            //inf_Empleado += "s.salario_base,";
            //inf_Empleado += "s.complemento, dep.desc_depto AS Departamento, dep.clave_depto AS ClaveDepartamento ";
            //inf_Empleado += "FROM tblRH_EK_Empleados as e";
            //inf_Empleado += " inner join tblRH_EK_Puestos as p on e.puesto=p.puesto";
            //inf_Empleado += " inner join tblRH_EK_Tipos_Nomina as tn on e.tipo_nomina=tn.tipo_nomina";
            ////inf_Empleado += " inner join DBA.cc as c on e.cc_contable=c.cc";
            //inf_Empleado += " inner join tblRH_EK_Registros_Patronales as r on e.id_regpat=r.clave_reg_pat";
            //inf_Empleado += " inner join tblRH_EK_Tabulador_Historial as s on e.clave_empleado=s.clave_empleado";
            //inf_Empleado += " left join tblRH_EK_Departamentos as dep on dep.clave_depto = e.clave_depto and dep.cc = e.cc_contable";
            //inf_Empleado += " where e.clave_empleado=" + idEmpleado + "AND e.estatus_empleado ='A' ";
            //inf_Empleado += " order by s.id DESC";
            #endregion

            string inf_Empleado = @"
            SELECT Top 1 s.bono_zona As Bono, 
                e.clave_empleado,e.nombre,ape_paterno,e.ape_materno,e.fecha_antiguedad as fecha_alta,e.puesto as 'puestoID',p.descripcion as 'puesto',
                e.tipo_nomina as 'tipoNominaID',
                tn.descripcion as 'tipoNomina',
                e.cc_contable as 'ccID',
                e.id_regpat as 'registroPatronalID',
                r.nombre_corto as 'registroPatronal',
                e.jefe_inmediato as 'clave_jefe_inmediato',
                (Select (e2.nombre+' '+e2.ape_paterno+' '+e2.ape_materno) from tblRH_EK_Empleados as e2 where e2.clave_empleado = e.jefe_inmediato)as 'nombre_jefe_inmediato',
                s.salario_base as salario_base,
                s.complemento, 
	            dep.desc_depto AS Departamento, dep.clave_depto AS ClaveDepartamento,
	            tabDet.FK_Tabulador as idTabulador,
	            tabDet.id as idTabuladorDet,
	            tabDet.FK_Categoria as idCategoria,
	            tabCat.concepto as descCategoria,
	            tabDet.FK_LineaNegocio as idLineaNegocios,
	            tabLinea.concepto as descLineaNegocios
            FROM tblRH_EK_Empleados as e
            inner join tblRH_EK_Puestos as p on e.puesto=p.puesto
            inner join tblRH_EK_Tipos_Nomina as tn on e.tipo_nomina=tn.tipo_nomina
            inner join tblRH_EK_Registros_Patronales as r on e.id_regpat=r.clave_reg_pat
            inner join tblRH_EK_Tabulador_Historial as s on e.clave_empleado=s.clave_empleado
            left join tblRH_REC_Requisicion as req on req.id = e.requisicion
			left join tblRH_TAB_TabuladoresDet as tabDet on tabDet.id = req.idTabuladorDet
            LEFT JOIN
	            tblRH_TAB_Tabuladores AS tab ON tab.id = tabDet.FK_Tabulador
            LEFT JOIN
	            tblRH_EK_Puestos AS tabP ON tabP.puesto = tab.FK_Puesto
            left join 
	            tblRH_EK_Departamentos as dep on dep.clave_depto = e.clave_depto and dep.cc = e.cc_contable
            LEFT JOIN 
	            tblRH_TAB_CatCategorias tabCat ON tabDet.FK_Categoria = tabCat.id
            LEFT JOIN 
	            tblRH_TAB_CatLineaNegocio tabLinea ON tabDet.FK_LineaNegocio = tabLinea.id
            where e.clave_empleado=" + idEmpleado + @"AND e.estatus_empleado ='A' 
            order by s.id DESC";

            try
            {
                //var resultado = (List<tblRH_FormatoCambio>)ContextEnKontrolNominaArrendadora.Where(inf_Empleado, vSesiones.sesionEmpresaActual).ToObject<List<tblRH_FormatoCambio>>();
                var resultado = _context.Select<tblRH_FormatoCambio>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = inf_Empleado
                }).ToList();

                if (resultado.Count > 0)
                {
                    var ccInfo = _ccFS_SP.GetCCNomina(resultado.First().CcID);
                    resultado.First().CC = ccInfo.descripcion;

                    var objInfoEmpleado = resultado.FirstOrDefault();

                    if (objInfoEmpleado.idTabuladorDet != null)
                    {
                        var lstTabuladoresDet = _context.tblRH_TAB_TabuladoresDet.Where(e => e.registroActivo &&
                                                e.tabuladorDetAutorizado == Core.Enum.RecursosHumanos.Tabuladores.EstatusGestionAutorizacionEnum.AUTORIZADO &&
                                                e.FK_Tabulador == objInfoEmpleado.idTabulador && e.FK_LineaNegocio == objInfoEmpleado.idLineaNegocios).OrderBy(E => E.FK_Categoria).ToList();
                        var lstIdsTabuladores = lstTabuladoresDet.Select(e => e.id).ToList();

                        //INDEX DEL SIGUIENTE TABULADOR
                        int indexOfTabuladorSelected = lstIdsTabuladores.IndexOf(objInfoEmpleado.idTabuladorDet.Value) + 1;

                        var objPermisoRangos = _context.tblP_AccionesVistatblP_Usuario.FirstOrDefault(e => e.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id && e.tblP_AccionesVista_id == 4040);

                        if (vSesiones.sesionUsuarioDTO.idPerfil == 1 || objPermisoRangos != null)
                        {
                            objInfoEmpleado.esRango = false;
                            objInfoEmpleado.lowerBase = 0;
                            objInfoEmpleado.lowerComplemento = 0;
                        }
                        else
                        {
                            if (indexOfTabuladorSelected == lstTabuladoresDet.Count())
                            {
                                objInfoEmpleado.lowerBase = 0;
                                objInfoEmpleado.lowerComplemento = 0;
                            }
                            else
                            {
                                var objTabDetAnterior = lstTabuladoresDet[indexOfTabuladorSelected];
                                objInfoEmpleado.lowerBase = objTabDetAnterior.sueldoBase;
                                objInfoEmpleado.lowerComplemento = objTabDetAnterior.complemento;

                            }

                            objInfoEmpleado.esRango = true;
                        }

                    }
                }
                return resultado;
            }
            catch (Exception o_O) {}
            //try
            //{
            //    var resultado = (List<tblRH_FormatoCambio>)ContextEnKontrolNominaArrendadora.Where(inf_Empleado, 2).ToObject<List<tblRH_FormatoCambio>>();
            //    return resultado;
            //}
            //catch (Exception o_O) { }
            return null;
        }
        private List<tblRH_FormatoCambio> InfEmpleadoNoneState(int idEmpleado)
        {
            //string inf_Empleado = "SELECT Top 1 s.bono_zona As Bono, ";
            //inf_Empleado += "e.clave_empleado,e.nombre,ape_paterno,e.ape_materno,e.fecha_antiguedad as fecha_alta,e.puesto as 'puestoID',p.descripcion as 'puesto',";
            //inf_Empleado += "e.tipo_nomina as 'tipoNominaID',";
            //inf_Empleado += "tn.descripcion as 'tipoNomina',";
            //inf_Empleado += "e.cc_contable as 'ccID',";
            //inf_Empleado += "c.descripcion as 'cc',";
            //inf_Empleado += "e.id_regpat as 'registroPatronalID',";
            //inf_Empleado += "r.nombre_corto as 'registroPatronal',";
            //inf_Empleado += "e.jefe_inmediato as 'clave_jefe_inmediato',";
            //inf_Empleado += "(Select (e2.nombre+' '+e2.ape_paterno+' '+e2.ape_materno) from DBA.sn_empleados as e2 where e2.clave_empleado = e.jefe_inmediato)as 'nombre_jefe_inmediato',";
            //inf_Empleado += "s.salario_base,";
            //inf_Empleado += "s.complemento ";
            //inf_Empleado += "FROM DBA.sn_empleados as e";
            //inf_Empleado += " inner join DBA.si_puestos as p on e.puesto=p.puesto";
            //inf_Empleado += " inner join DBA.sn_tipos_nomina as tn on e.tipo_nomina=tn.tipo_nomina";
            //inf_Empleado += " inner join DBA.cc as c on e.cc_contable=c.cc";
            //inf_Empleado += " inner join DBA.sn_registros_patronales as r on e.id_regpat=r.clave_reg_pat";
            //inf_Empleado += " inner join DBA.sn_tabulador_historial as s on e.clave_empleado=s.clave_empleado";
            //inf_Empleado += " where e.clave_empleado=" + idEmpleado + " ";
            //inf_Empleado += " order by s.id DESC";

            string inf_Empleado = "SELECT Top 1 s.bono_zona As Bono, ";
            inf_Empleado += "e.clave_empleado,e.nombre,ape_paterno,e.ape_materno,e.fecha_antiguedad as fecha_alta,e.puesto as 'puestoID',p.descripcion as 'puesto',";
            inf_Empleado += "e.tipo_nomina as 'tipoNominaID',";
            inf_Empleado += "tn.descripcion as 'tipoNomina',";
            inf_Empleado += "e.cc_contable as 'ccID',";
            //inf_Empleado += "c.descripcion as 'cc',";
            inf_Empleado += "e.id_regpat as 'registroPatronalID',";
            inf_Empleado += "r.nombre_corto as 'registroPatronal',";
            inf_Empleado += "e.jefe_inmediato as 'clave_jefe_inmediato',";
            inf_Empleado += "(Select (e2.nombre+' '+e2.ape_paterno+' '+e2.ape_materno) from tblRH_EK_Empleados as e2 where e2.clave_empleado = e.jefe_inmediato)as 'nombre_jefe_inmediato',";
            inf_Empleado += "s.salario_base,";
            inf_Empleado += "s.complemento, dep.desc_depto AS Departamento, dep.clave_depto AS ClaveDepartamento ";
            inf_Empleado += "FROM tblRH_EK_Empleados as e";
            inf_Empleado += " inner join tblRH_EK_Puestos as p on e.puesto=p.puesto";
            inf_Empleado += " inner join tblRH_EK_Tipos_Nomina as tn on e.tipo_nomina=tn.tipo_nomina";
            //inf_Empleado += " inner join DBA.cc as c on e.cc_contable=c.cc";
            inf_Empleado += " inner join tblRH_EK_Registros_Patronales as r on e.id_regpat=r.clave_reg_pat";
            inf_Empleado += " inner join tblRH_EK_Tabulador_Historial as s on e.clave_empleado=s.clave_empleado";
            inf_Empleado += " left join tblRH_EK_Departamentos as dep on dep.clave_depto = e.clave_depto and e.cc_contable = dep.cc";
            inf_Empleado += " where e.clave_empleado=" + idEmpleado + " ";
            inf_Empleado += " order by s.id DESC";
            try
            {
                //var resultado = (List<tblRH_FormatoCambio>)ContextEnKontrolNominaArrendadora.Where(inf_Empleado, vSesiones.sesionEmpresaActual).ToObject<List<tblRH_FormatoCambio>>();
                var resultado = _context.Select<tblRH_FormatoCambio>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = inf_Empleado
                }).ToList();

                if (resultado.Count > 0)
                {
                    var ccInfo = _ccFS_SP.GetCCNomina(resultado.First().CcID);
                    resultado.First().CC = ccInfo.descripcion;
                }

                return resultado;
            }
            catch (Exception o_O) { }
            //try
            //{
            //    var resultado = (List<tblRH_FormatoCambio>)ContextEnKontrolNominaArrendadora.Where(inf_Empleado, 2).ToObject<List<tblRH_FormatoCambio>>();
            //    return resultado;
            //}
            //catch (Exception o_O) { }
            return null;
        }

        public void EmpleadoEnkontrolToSigoplan()
        {
            var lst = new List<tblRH_FormatoCambio>();
            var lstBD = _context.tblRH_FormatoCambio.ToList();
            lstBD.ForEach(x =>
            {
                //string inf_Empleado = "SELECT s.bono_zona As Bono, ";
                //inf_Empleado += "e.clave_empleado,e.nombre,ape_paterno,e.ape_materno,e.fecha_antiguedad as fecha_alta,e.puesto as 'puestoID',p.descripcion as 'puesto',";
                //inf_Empleado += "e.tipo_nomina as 'tipoNominaID',";
                //inf_Empleado += "tn.descripcion as 'tipoNomina',";
                //inf_Empleado += "e.cc_contable as 'ccID',";
                //inf_Empleado += "c.descripcion as 'cc',";
                //inf_Empleado += "e.id_regpat as 'registroPatronalID',";
                //inf_Empleado += "r.nombre_corto as 'registroPatronal',";
                //inf_Empleado += "e.jefe_inmediato as 'clave_jefe_inmediato',";
                //inf_Empleado += "(Select (e2.nombre+' '+e2.ape_paterno+' '+e2.ape_materno) from DBA.sn_empleados as e2 where e2.clave_empleado = e.jefe_inmediato)as 'nombre_jefe_inmediato',";
                //inf_Empleado += "s.salario_base,";
                //inf_Empleado += "s.complemento ";
                //inf_Empleado += "FROM DBA.sn_empleados as e";
                //inf_Empleado += " inner join DBA.si_puestos as p on e.puesto=p.puesto";
                //inf_Empleado += " inner join DBA.sn_tipos_nomina as tn on e.tipo_nomina=tn.tipo_nomina";
                //inf_Empleado += " inner join DBA.cc as c on e.cc_contable=c.cc";
                //inf_Empleado += " inner join DBA.sn_registros_patronales as r on e.id_regpat=r.clave_reg_pat";
                //inf_Empleado += " inner join DBA.sn_tabulador_historial as s on e.clave_empleado=s.clave_empleado";
                //inf_Empleado += " where e.clave_empleado=" + x.Clave_Empleado + "AND e.estatus_empleado ='A' ";
                //inf_Empleado += " order by s.id DESC";

                string inf_Empleado = "SELECT s.bono_zona As Bono, ";
                inf_Empleado += "e.clave_empleado,e.nombre,ape_paterno,e.ape_materno,e.fecha_antiguedad as fecha_alta,e.puesto as 'puestoID',p.descripcion as 'puesto',";
                inf_Empleado += "e.tipo_nomina as 'tipoNominaID',";
                inf_Empleado += "tn.descripcion as 'tipoNomina',";
                inf_Empleado += "e.cc_contable as 'ccID',";
                //inf_Empleado += "c.descripcion as 'cc',";
                inf_Empleado += "e.id_regpat as 'registroPatronalID',";
                inf_Empleado += "r.nombre_corto as 'registroPatronal',";
                inf_Empleado += "e.jefe_inmediato as 'clave_jefe_inmediato',";
                inf_Empleado += "(Select (e2.nombre+' '+e2.ape_paterno+' '+e2.ape_materno) from tblRH_EK_Empleados as e2 where e2.clave_empleado = e.jefe_inmediato)as 'nombre_jefe_inmediato',";
                inf_Empleado += "s.salario_base,";
                inf_Empleado += "s.complemento ";
                inf_Empleado += "FROM tblRH_EK_Empleados as e";
                inf_Empleado += " inner join tblRH_EK_Puestos as p on e.puesto=p.puesto";
                inf_Empleado += " inner join tblRH_EK_Tipos_Nomina as tn on e.tipo_nomina=tn.tipo_nomina";
                //inf_Empleado += " inner join DBA.cc as c on e.cc_contable=c.cc";
                inf_Empleado += " inner join tblRH_EK_Registros_Patronales as r on e.id_regpat=r.clave_reg_pat";
                inf_Empleado += " inner join tblRH_EK_Tabulador_Historial as s on e.clave_empleado=s.clave_empleado";
                inf_Empleado += " where e.clave_empleado=" + x.Clave_Empleado + "AND e.estatus_empleado ='A' ";
                inf_Empleado += " order by s.id DESC";
                try
                {
                    //var resultado = (List<tblRH_FormatoCambio>)ContextEnKontrolNomina.Where(inf_Empleado).ToObject<List<tblRH_FormatoCambio>>();
                    var resultado = _context.Select<tblRH_FormatoCambio>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = inf_Empleado
                    }).ToList();

                    if (resultado.Count > 0)
                    {
                        var ccInfo = _ccFS_SP.GetCCNomina(resultado.First().CcID);
                        resultado.First().CC = ccInfo.descripcion;
                    }
                    lst.Add(resultado.FirstOrDefault());
                }
                catch (Exception o_O) { }
            });
            foreach (var ek in lstBD)
            {
                var bd = lst.FirstOrDefault(w => ek.Clave_Empleado == w.Clave_Empleado);
                var menor = new DateTime(1753, 1, 1);
                var mayor = new DateTime(9999, 12, 31);
                ek.Fecha_Alta = bd == null ? menor : bd.Fecha_Alta;
            }
            lstBD.ForEach(x =>
            {
                Update(x, x.id, (int)BitacoraEnum.FORMATOCAMBIORH);
            });
        }

        public List<tblRH_CatPuestos> getCatPuestos(string term)
        {
            var getPuestos = string.Empty;
            //if (!String.IsNullOrEmpty(term))
            //    getPuestos = "SELECT TOP 10 puesto, descripcion FROM DBA.si_puestos WHERE descripcion LIKE '%" + term + "%' and descripcion not like '%(NO USAR)%' order by descripcion";
            //else
            //    getPuestos = "SELECT TOP 10 puesto, descripcion FROM DBA.si_puestos";
            try
            {
                //var resultado = (List<tblRH_CatPuestos>)ContextEnKontrolNomina.Where(getPuestos).ToObject<List<tblRH_CatPuestos>>();
                var resultado = _context.Select<tblRH_CatPuestos>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = !string.IsNullOrEmpty(term) ? "SELECT TOP 10 puesto, descripcion FROM tblRH_EK_Puestos WHERE descripcion LIKE '%" + term + "%' and descripcion not like '%(NO USAR)%' and registroActivo = 1 order by descripcion" :
                                "SELECT TOP 10 puesto, descripcion FROM tblRH_EK_Puestos WHERE registroActivo = 1"
                }).ToList();
                return resultado;
            }
            catch (Exception o_O) { return new List<tblRH_CatPuestos>(); }
        }

        public List<tblRH_CatTipoNomina> getCatTipoNomina()
        {
            //string getTipoNomina = "SELECT tipo_nomina, descripcion FROM DBA.sn_tipos_nomina WHERE tipo_nomina=" + (int)Tipo_NominaEnum.SEMANAL + " OR tipo_nomina=" + (int)Tipo_NominaEnum.QUINCENAL;
            //var resultado = (List<tblRH_CatTipoNomina>)ContextEnKontrolNomina.Where(getTipoNomina).ToObject<List<tblRH_CatTipoNomina>>();
            var resultado = _context.Select<tblRH_CatTipoNomina>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = "SELECT tipo_nomina, descripcion FROM tblRH_EK_Tipos_Nomina WHERE tipo_nomina=" + (int)Tipo_NominaEnum.SEMANAL + " OR tipo_nomina=" + (int)Tipo_NominaEnum.QUINCENAL
            }).ToList();
            return resultado;
        }

        public List<tblRH_CatCentroCostos> getCC(string term)
        {
            try
            {
                //var odbc = new OdbcConsultaDTO()
                //    {
                //        consulta = string.Format("SELECT TOP 10 cc, descripcion FROM {0}\"cc\" WHERE descripcion LIKE ? AND st_ppto <> 'T'", vSesiones.sesionEmpresaDBPregijo),
                //        parametros = new List<OdbcParameterDTO>() { new OdbcParameterDTO() { nombre = "descripcion", tipo = OdbcType.Char, valor = string.Format("%{0}%", term.Trim()) } }
                //    };
                //var lst =  _contextEnkontrol.Select<tblRH_CatCentroCostos>(EnkontrolAmbienteEnum.Rh, odbc);
                //var query_cc = new OdbcConsultaDTO();

                //query_cc.consulta = string.Format("SELECT TOP 10 cc, descripcion FROM cc WHERE descripcion LIKE ? AND st_ppto <> 'T'");

                //var lst = _contextEnkontrol.Select<tblRH_CatCentroCostos>(vSesiones.sesionAmbienteEnkontrolAdm, query_cc);

                var lst = _context.tblC_Nom_CatalogoCC.Where(x => x.ccDescripcion.Contains(term)).Take(10).Select(x => new tblRH_CatCentroCostos
                {
                    cc = x.cc,
                    descripcion = x.ccDescripcion.Trim()
                }).ToList();

                return lst;
            }
            catch (Exception o_O)
            {
                return new List<tblRH_CatCentroCostos>();
            }
        }

        public List<tblRH_CatCentroCostos> getCCList()
        {
            try
            {
                //var odbc = new OdbcConsultaDTO()
                //{
                //    consulta = "SELECT cc, descripcion FROM DBA.cc where st_ppto <> 'T'"
                //};
                //var lst = _contextEnkontrol.Select<tblRH_CatCentroCostos>(EnkontrolAmbienteEnum.Rh, odbc);

                //var query_cc = new OdbcConsultaDTO();

                //query_cc.consulta = "SELECT cc, descripcion FROM cc where st_ppto <> 'T'";
                //query_cc.consulta = "SELECT cc, descripcion FROM cc";

                //var lst = _contextEnkontrol.Select<tblRH_CatCentroCostos>(vSesiones.sesionAmbienteEnkontrolAdm, query_cc);

                var lst = _context.tblC_Nom_CatalogoCC.Where(x => x.cc != "0").Select(x => new tblRH_CatCentroCostos
                {
                    cc = x.cc,
                    descripcion = x.ccDescripcion.Trim()
                }).ToList();

                return lst;
            }
            catch (Exception o_O)
            {
                return new List<tblRH_CatCentroCostos>();
            }
        }

        public List<tblRH_CatEmpleados> getCatEmpleados(string term)
        {
            //var getCatEmpleado = "SELECT TOP 10 clave_empleado, (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Nombre, puesto FROM DBA.sn_empleados WHERE replace((nombre+' '+ape_paterno+' '+ape_materno), ' ', '') LIKE '%" + term.Replace(" ", "") + "%' and estatus_empleado = 'A'";
            try
            {
                string strQuery = "SELECT clave_empleado, (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Nombre, puesto FROM tblRH_EK_Empleados WHERE replace((nombre+' '+ape_paterno+' '+ape_materno), ' ', '') LIKE '%" + term.Replace(" ", "") + "%' and estatus_empleado = 'A'";

                var resultado = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).ToList();

                var exclusion = _context.tblRH_FormatoCambioExclusion.Where(w => w.registroActivo).Select(x => x.empleadoCVE).ToList();
                UsuarioFactoryServices usuarioFactoryServices = new UsuarioFactoryServices();
                var permiso = usuarioFactoryServices.getUsuarioService().getViewAction(vSesiones.sesionCurrentView, "EmpleadoExclusion");
                if (permiso)
                {
                    return resultado;
                }
                else
                {
                    return resultado.Where(x => !exclusion.Contains(x.clave_empleado)).ToList();
                }
            }
            catch (Exception o_O)
            {
                return new List<tblRH_CatEmpleados>();
            }
        }

        public List<tblRH_CatEmpleados> getCatEmpleadosReclutamientos(string term)
        {
            //var getCatEmpleado = "SELECT TOP 10 clave_empleado, (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Nombre, puesto FROM DBA.sn_empleados WHERE replace((nombre+' '+ape_paterno+' '+ape_materno), ' ', '') LIKE '%" + term.Replace(" ", "") + "%' and estatus_empleado = 'A'";
            try
            {
                string strQuery = "SELECT clave_empleado, (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Nombre, puesto FROM tblRH_EK_Empleados WHERE replace((nombre+' '+ape_paterno+' '+ape_materno), ' ', '') LIKE '%" + term.Replace(" ", "") + "%' and estatus_empleado = 'A' and esActivo = 1";

                var resultado = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).ToList();

                return resultado;

            }
            catch (Exception o_O)
            {
                return new List<tblRH_CatEmpleados>();
            }
        }

        public List<tblRH_CatEmpleados> getCatEmpleadosGeneral(string term)
        {
            var palabraArr = term.Split(' ');
            var palabra = string.Join("%", palabraArr);
            //var getCatEmpleado = "SELECT TOP 10 clave_empleado, (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Nombre, puesto FROM DBA.sn_empleados WHERE  (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) LIKE '%" + palabra + "%'";
            try
            {
                //var resultado = (List<tblRH_CatEmpleados>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 1).ToObject<List<tblRH_CatEmpleados>>();
                var resultado = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = "SELECT TOP 10 clave_empleado, (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Nombre, puesto FROM tblRH_EK_Empleados WHERE  (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) LIKE '%" + palabra + "%'"
                }).ToList();
                return resultado;
            }
            catch (Exception o_O)
            {
                return new List<tblRH_CatEmpleados>();
            }
        }
        public List<tblRH_CatEmpleados> getCatEmpleadosTodos(string term)
        {
            //var getCatEmpleado = "SELECT TOP 10 clave_empleado, (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Nombre, puesto FROM DBA.sn_empleados WHERE replace((nombre+' '+ape_paterno+' '+ape_materno), ' ', '') LIKE '%" + term.Replace(" ", "") + "%' and estatus_empleado = 'A'";
            try
            {
                //var resultado = (List<tblRH_CatEmpleados>)ContextEnKontrolNomina.Where(getCatEmpleado).ToObject<List<tblRH_CatEmpleados>>();
                var resultado = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = "SELECT TOP 10 clave_empleado, (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Nombre, puesto FROM tblRH_EK_Empleados WHERE replace((nombre+' '+ape_paterno+' '+ape_materno), ' ', '') LIKE '%" + term.Replace(" ", "") + "%' and estatus_empleado = 'A'"
                }).ToList();
                var exclusion = _context.tblRH_FormatoCambioExclusion.Select(x => x.empleadoCVE).ToList();
                UsuarioFactoryServices usuarioFactoryServices = new UsuarioFactoryServices();
                var permiso = usuarioFactoryServices.getUsuarioService().getViewAction(vSesiones.sesionCurrentView, "EmpleadoExclusion");
                if (permiso)
                {
                    return resultado;
                }
                else
                {
                    return resultado.Where(x => !exclusion.Contains(x.clave_empleado)).ToList();

                }
            }
            catch (Exception o_O)
            {
                return new List<tblRH_CatEmpleados>();
            }
        }


        public List<tblRH_CatRegistroPatronales> getCatRegistroPatronales(string term)
        {
            //var getCatRegPatronal = "SELECT TOP 10 clave_reg_pat, nombre_corto as desc_reg_pat FROM DBA.sn_registros_patronales where desc_reg_pat LIKE '%" + term + "%'";
            try
            {
                //var resultado = (List<tblRH_CatRegistroPatronales>)ContextEnKontrolNomina.Where(getCatRegPatronal).ToObject<List<tblRH_CatRegistroPatronales>>();
                var resultado = _context.Select<tblRH_CatRegistroPatronales>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = "SELECT TOP 10 clave_reg_pat, nombre_corto as desc_reg_pat FROM tblRH_EK_Registros_Patronales where nombre_corto LIKE '%" + term + "%'"
                }).ToList();
                return resultado;
            }
            catch (Exception o_O)
            {
                return new List<tblRH_CatRegistroPatronales>();
            }
        }
        #endregion
        public int getFormatoCambioID(int id)
        {
            var a = _context.tblRH_AutorizacionFormatoCambio.FirstOrDefault(x => x.id == id);
            return a.Id_FormatoCambio;
        }

        public List<tblRH_FormatoCambio> getListFormatosCambioPendientes(int id, string cc, int claveEmp, int estado, string tipo, int numero)
        {
            try
            {
                var result = new List<tblRH_FormatoCambio>();
                var ud = new UsuarioDAO();
                var rh = ud.getViewAction(vSesiones.sesionCurrentView, "VerTodoFormato"); // DA FALSE CUANDO PORQUE EL SESIONCURRENTVIEW FALLA A CAMBIAR LA VISTA ALGUNAS VECES
                var exclusion = _context.tblRH_FormatoCambioExclusion.Select(x => x.empleadoCVE).ToList();
                var usuarioFactoryServices = new UsuarioFactoryServices();
                var lstAuth = _context.tblRH_AutorizacionFormatoCambio.Where(x => x.Clave_Aprobador == vSesiones.sesionUsuarioDTO.id && x.Autorizando).ToList();
                var autorizante = lstAuth.Select(x => x.Id_FormatoCambio).ToList();
                var permiso = usuarioFactoryServices.getUsuarioService().getViewAction(vSesiones.sesionCurrentView, "EmpleadoExclusion");
                var ccsPermiso = _context.tblRH_BN_Usuario_CC.Where(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id).Select(x => x.cc).ToList();
                var ccsAll = ccsPermiso.Contains("*");

                if (id != 0 && cc.Equals("") && claveEmp == 0 && estado == 1 && tipo.Equals("") && numero == 0) {
                    result = _context.tblRH_FormatoCambio.Where(x=>x.id==id).ToList();
                }
                else if (rh)
                {
                    if (estado == 5)
                    {
                        var lstFirmasCanceladas = _context.tblRH_AutorizacionFormatoCambio.Where(e => e.comentario == "RECHAZADO AUTOMATICAMENTE POR SISTEMA");

                        var lstFormatosCambioCancelados = lstFirmasCanceladas.Select(e => e.Id_FormatoCambio).Distinct();

                        result = _context.tblRH_FormatoCambio
                        .Where(x =>
                            (id == 0 ? true : x.id == id) &&
                                //(cc.Equals("") ? true : x.CcID.Contains(cc)) &&
                            (string.IsNullOrEmpty(cc) ? (ccsAll ? true : ccsPermiso.Contains(x.CcID)) : (ccsPermiso.Contains(cc) || ccsPermiso.Contains("*") ? x.CcID == cc : false)) &&
                            (claveEmp != 0 ? x.Clave_Empleado == claveEmp : true) &&
                            (lstFormatosCambioCancelados.Contains(x.id)) &&
                            (tipo.Equals("") ? true : (x.TipoNomina.Equals(tipo) && x.InicioNomina == numero)) &&
                            (permiso ? true : !exclusion.Contains(x.Clave_Empleado))
                        ).ToList();
                    }
                    else
                    {
                        result = _context.tblRH_FormatoCambio
                        .Where(x =>
                            (id == 0 ? true : x.id == id) &&
                                //(cc.Equals("") ? true : x.CcID.Contains(cc)) &&
                            (string.IsNullOrEmpty(cc) ? (ccsAll ? true : ccsPermiso.Contains(x.CcID)) : (ccsPermiso.Contains(cc) || ccsPermiso.Contains("*") ? x.CcID == cc : false)) &&
                            (claveEmp != 0 ? x.Clave_Empleado == claveEmp : true) &&
                            (estado == 2 ? (!x.Aprobado && !x.Rechazado) : (estado == 3 ? x.Aprobado : (estado == 4 ? x.Rechazado : (true)))) &&
                            (tipo.Equals("") ? true : (x.TipoNomina.Equals(tipo) && x.InicioNomina == numero)) &&
                            (permiso ? true : !exclusion.Contains(x.Clave_Empleado))
                        ).ToList();
                    }
                    
                }
                else
                {
                    if (estado == 5)
                    {
                        var lstFirmasCanceladas = _context.tblRH_AutorizacionFormatoCambio.Where(e => e.comentario == "RECHAZADO AUTOMATICAMENTE POR SISTEMA");

                        var lstFormatosCambioCancelados = lstFirmasCanceladas.Select(e => e.Id_FormatoCambio).Distinct();

                        result = _context.tblRH_FormatoCambio
                        .Where(x =>
                            (id == 0 ? true : x.id == id) &&
                                //(cc.Equals("") ? true : x.CcID.Contains(cc)) &&
                            (string.IsNullOrEmpty(cc) ? (ccsAll ? true : ccsPermiso.Contains(x.CcID)) : (ccsPermiso.Contains(cc) || ccsPermiso.Contains("*") ? x.CcID == cc : false)) &&
                            (claveEmp != 0 ? x.Clave_Empleado == claveEmp : true) &&
                            (lstFormatosCambioCancelados.Contains(x.id)) &&
                            (tipo.Equals("") ? true : (x.TipoNomina.Equals(tipo) && x.InicioNomina == numero)) &&
                            (permiso ? true : !exclusion.Contains(x.Clave_Empleado))
                        ).ToList();
                    }
                    else
                    {
                        result = _context.tblRH_FormatoCambio
                        .Where(x =>
                            (id == 0 ? true : x.id == id) &&
                            (string.IsNullOrEmpty(cc) ? (ccsAll ? true : ccsPermiso.Contains(x.CcID)) : (ccsPermiso.Contains(cc) || ccsPermiso.Contains("*") ? x.CcID == cc : false)) &&
                            (claveEmp != 0 ? x.Clave_Empleado == claveEmp : true) &&
                            (estado == 1 ? true : estado == 2 ? (!x.Aprobado && !x.Rechazado) : estado == 3 ? x.Aprobado : estado == 4 ? x.Rechazado : false) &&
                            (tipo.Equals("") ? true : (x.TipoNomina.Equals(tipo) && x.InicioNomina == numero)) &&
                            (
                                (permiso ? true : !exclusion.Contains(x.Clave_Empleado)) &&
                                ((autorizante.Contains(x.id) || x.usuarioCap == vSesiones.sesionUsuarioDTO.id))
                            ) /*&&
                            (string.IsNullOrEmpty(cc) ? (ccsAll ? true : ccsPermiso.Contains(x.CcID)) : (ccsPermiso.Contains(cc) ? x.CcID == cc : false))*/
                        ).ToList();
                        //result = result.Where(x => autorizante.Contains(x.id) ||
                        //        x.usuarioCap == vSesiones.sesionUsuarioDTO.id).ToList();
                    }

                }

                return result;
            }
            catch (Exception o_O)
            {
                return null;
            }

        }

        public tblRH_FormatoCambio getFormatoByID(int id)
        {
            return _context.tblRH_FormatoCambio.FirstOrDefault(x => x.id == id);
        }
        public void eliminarFormato(int formatoID)
        {

            

            var autorizantes = _context.tblRH_AutorizacionFormatoCambio.Where(x => x.Id_FormatoCambio == formatoID).ToList();
            _context.tblRH_AutorizacionFormatoCambio.RemoveRange(autorizantes);
            _context.SaveChanges();

            var formato = _context.tblRH_FormatoCambio.FirstOrDefault(x => x.id == formatoID);
            string folio = formato.folio;

            var alertas = _context.tblP_Alerta.Where(x => x.sistemaID == (int)SistemasEnum.RH && x.msj.Contains("Firma-Formato de Cambio " + folio)).ToList();
            _context.tblP_Alerta.RemoveRange(alertas);
            _context.SaveChanges();

            _context.tblRH_FormatoCambio.Remove(formato);
            _context.SaveChanges();

            
            
        }
        public bool getEmpleadoExclusion(int empleadoCVE)
        {
            UsuarioFactoryServices usuarioFactoryServices = new UsuarioFactoryServices();
            var permiso = usuarioFactoryServices.getUsuarioService().getViewAction(vSesiones.sesionCurrentView, "EmpleadoExclusion");
            if (permiso)
            {
                return true;
            }
            else
            {
                var empleado = _context.tblRH_EK_Empleados.FirstOrDefault(x => x.clave_empleado == empleadoCVE);
                if (empleado != null) 
                {
                    var puesto = _context.tblRH_EK_Puestos.FirstOrDefault(x => x.puesto == empleado.puesto);
                    if (puesto != null)
                    {
                        var puestoDesc = puesto.descripcion;
                        if (puestoDesc.Contains("DIRECTOR") || puestoDesc.Contains("DIRECCI") || puestoDesc.Contains("GERENCIA") || puestoDesc.Contains("GERENTE")) return false;
                        else return true;
                    }
                    else return false;
                }
                else return false;
                //return _context.tblRH_FormatoCambioExclusion.FirstOrDefault(x => x.empleadoCVE == empleadoCVE) == null ? true : false;
            }
        }
        public bool validReporteExclusion(int id)
        {
            UsuarioFactoryServices usuarioFactoryServices = new UsuarioFactoryServices();
            var permiso = usuarioFactoryServices.getUsuarioService().getViewAction(vSesiones.sesionCurrentView, "EmpleadoExclusion");
            var autorizante = _context.tblRH_AutorizacionFormatoCambio.FirstOrDefault(x => x.Id_FormatoCambio == id && x.Clave_Aprobador == vSesiones.sesionUsuarioDTO.id) == null ? false : true;
            if (permiso || autorizante)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Dictionary<string, object> getDepartamentosCC(string cc)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var departamentos = _context.tblRH_EK_Departamentos.Where(x => x.cc == cc && x.esActivo).Select( x => new
                    {
                        Value = x.clave_depto,
                        Text = x.desc_depto
                    }).OrderBy(o => o.Value).ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, departamentos);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, ex.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GetResponsableCC(string cc)
        {
            resultado.Clear();

            try
            {
                var usuarios = new List<ComboDTO>();

                var plantillasRequisiciones = new List<int>();

                tblFA_Paquete paquete = null;

                switch (vSesiones.sesionEmpresaActual)
                {
                    case (int)EmpresaEnum.Construplan:
                    case (int)EmpresaEnum.GCPLAN:
                        //plantillasRequisiciones = new List<int> { 111, 112 };
                        plantillasRequisiciones = new List<int> { 123 };
                        paquete = _context.tblFA_Paquete.Where(x => x.cc.cc == cc).OrderByDescending(x => x.fechaCreacion).FirstOrDefault();
                        break;
                    case (int)EmpresaEnum.Arrendadora:
                        //plantillasRequisiciones = new List<int> { 111, 112 };
                        //plantillasRequisiciones = new List<int> { 123 };
                        //paquete = _context.tblFA_Paquete.Where(x => x.cc.cc == cc).OrderByDescending(x => x.fechaCreacion).FirstOrDefault();
                        //break;

                        plantillasRequisiciones = new List<int> { 123 };
                        if (!string.IsNullOrEmpty(cc))
                        {
                            var ccPaquete = _context.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.cc == cc);
                            paquete = _context.tblFA_Paquete.Where(x => x.ccID == ccPaquete.id).OrderByDescending(x => x.fechaCreacion).FirstOrDefault();
                        }
                        break;
                    case (int)EmpresaEnum.Colombia:
                        //plantillasRequisiciones = new List<int> { 111, 112 };
                        plantillasRequisiciones = new List<int> { 123 };
                        if (!string.IsNullOrEmpty(cc))
                        {
                            var ccPaquete = _context.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.cc == cc);
                            paquete = _context.tblFA_Paquete.Where(x => x.ccID == ccPaquete.id).OrderByDescending(x => x.fechaCreacion).FirstOrDefault();
                        }
                        break;
                    case (int)EmpresaEnum.Peru:
                        //plantillasRequisiciones = new List<int> { 124 };
                        plantillasRequisiciones = new List<int> { 123 };
                        if (!string.IsNullOrEmpty(cc))
                        {
                            var ccPaquete = _context.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.cc == cc);
                            paquete = _context.tblFA_Paquete.Where(x => x.ccID == ccPaquete.id).OrderByDescending(x => x.fechaCreacion).FirstOrDefault();
                        }
                        break;
                }

                if (paquete != null)
                {
                    foreach (var facultamiento in paquete.facultamientos.Where(x => plantillasRequisiciones.Contains(x.plantillaID) && x.aplica))
                    {
                        foreach (var item in facultamiento.empleados.Where(x => x.esActivo && x.aplica))
                        {
                            var usuario = new ComboDTO();
                            usuario.Value = item.claveEmpleado.ToString();
                            usuario.Text = item.nombreEmpleado;
                            usuarios.Add(usuario);
                        }
                    }
                }

                var usuariosSinRepetir = new List<ComboDTO>();
                foreach (var item in usuarios.GroupBy(x => x.Value))
                {
                    usuariosSinRepetir.Add(item.First());
                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, usuariosSinRepetir);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, ex.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GetRegistroPatCC(string cc)
        {
            resultado.Clear();

            try
            {
                var lstRegPats = new List<tblRH_EK_Registros_Patronales>();
                var lstRelCCRegPat = _context.tblRH_REC_RelacionRegistroPatronalCC.Where(e => e.registroActivo && e.cc == cc).ToList();

                foreach (var item in lstRelCCRegPat)
                {
                    var objRegPat = _context.tblRH_EK_Registros_Patronales.FirstOrDefault(e => e.esActivo && e.clave_reg_pat == item.clave_reg_pat);

                    if (objRegPat != null)
                    {
                        lstRegPats.Add(objRegPat);
                        
                    }
                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstRegPats);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, ex.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GetTabuladorByEmpleado(int puesto, int lineaNegocios, int categoria)
        {
            resultado.Clear();

            try
            {
                var objTabulador = _context.tblRH_TAB_Tabuladores.FirstOrDefault(e => e.registroActivo && e.FK_Puesto == puesto);

                var objTabuladorDet = new tblRH_TAB_TabuladoresDet();
                var objLowerTabuladorDet = new tblRH_TAB_TabuladoresDet();

                bool esRango = true;
                
                if (objTabulador != null)
                {
                    var lstTabuladoresDet = _context.tblRH_TAB_TabuladoresDet.Where(e => e.registroActivo &&
                        e.tabuladorDetAutorizado == Core.Enum.RecursosHumanos.Tabuladores.EstatusGestionAutorizacionEnum.AUTORIZADO &&
                        e.FK_Tabulador == objTabulador.id && e.FK_LineaNegocio == lineaNegocios).OrderBy(E => E.FK_Categoria).ToList();

                    objTabuladorDet = lstTabuladoresDet.FirstOrDefault(e => e.FK_Categoria == categoria);

                    //INDEX DEL SIGUIENTE TABULADOR
                    int indexOfTabuladorSelected = lstTabuladoresDet.IndexOf(objTabuladorDet) + 1;

                    var objPermisoRangos = _context.tblP_AccionesVistatblP_Usuario.FirstOrDefault(e => e.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id && e.tblP_AccionesVista_id == 4040);

                    if (vSesiones.sesionUsuarioDTO.idPerfil == 1 || objPermisoRangos != null)
                    {
                        esRango = false;
                        objLowerTabuladorDet.id = 0;
                        objLowerTabuladorDet.totalNominal = 0;
                    }
                    else
                    {
                        if (indexOfTabuladorSelected == lstTabuladoresDet.Count())
                        {
                            objLowerTabuladorDet.id = 0;
                            objLowerTabuladorDet.totalNominal = 0;

                        }
                        else
                        {
                            objLowerTabuladorDet = lstTabuladoresDet[indexOfTabuladorSelected];

                        }
                    }

                }
                else
                {
                    objTabuladorDet = null;
                }

                resultado.Add(ITEMS, objTabuladorDet);
                resultado.Add("lowerTabulador", objLowerTabuladorDet);
                resultado.Add("esRango", esRango);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e) 
            {

                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        #region PERMISOS
        public bool CheckEsEditarPuestos()
        {
            bool esEditar = false;
            try
            {
                var objPermiso = _context.tblP_AccionesVistatblP_Usuario.FirstOrDefault(e => e.tblP_AccionesVista_id == 4039 && e.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id);

                if (objPermiso != null || vSesiones.sesionUsuarioDTO.idPerfil == 1)
                {
                    esEditar = true;
                }
            }
            catch (Exception e)
            {
                esEditar = false;

                throw e;
            }

            return esEditar;
        }

        public bool GetPermisoSueldos()
        {
            bool esVerSalarios = false;

            try
            {
                esVerSalarios = (_context.tblP_AccionesVistatblP_Usuario.Where(x => x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id && x.tblP_AccionesVista_id == 4056).Count()) > 0;

            }
            catch (Exception)
            {
                esVerSalarios = false;
            }

            return esVerSalarios;
        }

        #endregion

        public List<cboDTO> cboCentroCostos()
        {
            var lst = new List<cboDTO>();
            switch (vSesiones.sesionEmpresaActual)
            {
                case (int)EmpresaEnum.Construplan:
                    lst.AddRange(_contextEnkontrol.Select<cboDTO>(EnkontrolEnum.CplanProd, "SELECT cc AS Value, (cc+'-'+descripcion) AS Text FROM cc order by cc"));
                    break;
                case (int)EmpresaEnum.Arrendadora:
                    lst.AddRange(_context.Select<cboDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Colombia,
                        consulta = "SELECT cc as Value, (cc + ' - ' + ccDescripcion) as Text FROM tblC_Nom_CatalogoCC ORDER BY cc",
                    }));
                    break;
                case (int)EmpresaEnum.Colombia:
                    lst.AddRange(_context.Select<cboDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Colombia,
                        consulta = "SELECT cc as Value, (cc + ' - ' + ccDescripcion) as Text FROM tblC_Nom_CatalogoCC ORDER BY cc",
                    }));
                    //lst.AddRange(_contextEnkontrol.Select<cboDTO>(EnkontrolEnum.CplanProd, "SELECT cc AS Value, (cc+'-'+descripcion) AS Text FROM cc order by cc"));
                    break;
                case (int)EmpresaEnum.Peru:
                    lst.AddRange(_context.Select<cboDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.PERU,
                        consulta = "SELECT cc as Value, (cc + ' - ' + ccDescripcion) as Text FROM tblC_Nom_CatalogoCC ORDER BY cc",
                    }));
                    break;
                    //lst.AddRange(_contextEnkontrol.Select<cboDTO>(EnkontrolEnum.CplanProd, "SELECT cc AS Value, (cc+'-'+descripcion) AS Text FROM cc order by cc"));
                case (int)EmpresaEnum.GCPLAN:
                    lst.AddRange(_context.Select<cboDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.PERU,
                        consulta = "SELECT cc as Value, (cc + ' - ' + ccDescripcion) as Text FROM tblC_Nom_CatalogoCC ORDER BY cc",
                    }));
                    break;
            }
            return lst;
        }
    }
}
