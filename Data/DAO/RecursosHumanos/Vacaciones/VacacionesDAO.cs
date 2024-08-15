using Core.DAO.Enkontrol.General.CC;
using Core.DAO.Maquinaria.Reporte;
using Core.DAO.RecursosHumanos.Vacaciones;
using Core.DTO;
using Core.DTO.Contabilidad.Propuesta.Nomina;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos.BajasPersonal;
using Core.DTO.RecursosHumanos.Vacaciones;
using Core.DTO.Utils;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.FacultamientosDpto;
using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using Core.Entity.Principal.Alertas;
using Core.Entity.Principal.Usuarios;
using Core.Entity.RecursosHumanos.Captura;
using Core.Entity.RecursosHumanos.Vacaciones;
using Core.Enum;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Core.Enum.Principal.Usuario;
using Core.Enum.RecursosHumanos.CatNotificantes;
using Core.Enum.RecursosHumanos.Vacaciones;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Enkontrol.General.CC;
using Data.Factory.Maquinaria.Reporte;
using Infrastructure.Utils;
using OfficeOpenXml;
using Reportes.Reports.RecursosHumanos.Vacaciones;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace Data.DAO.RecursosHumanos.Vacaciones
{
    public class VacacionesDAO : GenericDAO<tblP_Usuario>, IVacacionesDAO
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        private const string _NOMBRE_CONTROLADOR = "Vacaciones";
        private const int _SISTEMA = (int)SistemasEnum.RH;
        ICCDAO _ccFS_SP = new CCFactoryService().getCCServiceSP();
        IEncabezadoDAO encabezadoFactoryServices = new EncabezadoFactoryServices().getEncabezadoServices();

        Dictionary<string, object> resultado = new Dictionary<string, object>();

        #region RUTAS
        private readonly string RutaArchivos = @"\\REPOSITORIO\Proyecto\SIGOPLAN\CAPITAL_HUMANO\RECLUTAMIENTOS\VACACIONES\INCAPACIDADES";
        private readonly string RutaArchivosLocal = @"C:\Proyecto\SIGOPLAN\CAPITAL_HUMANO\RECLUTAMIENTOS\VACACIONES\INCAPACIDADES";

        #endregion

        #region FILL COMBOS

        public List<ComboDTO> FillComboPeriodos()
        {
            try
            {
                #region FILL COMBO MOTIVOS
                List<ComboDTO> lstMotivos = new List<ComboDTO>();
                lstMotivos = _context.tblRH_Vacaciones_Periodos.Where(w => w.registroActivo && w.estado).ToList().Select(s => new ComboDTO
                {
                    Value = s.id.ToString(),
                    Text = s.periodoDesc + ": " + (s.fechaInicio.Value.ToString("dd/MM/yyyy") ?? "") + " → " + (s.fechaFinal.Value.ToString("dd/MM/yyyy") ?? "")
                }).ToList();
                return lstMotivos;
                #endregion
            }
            catch (Exception e)
            {
                LogError(16, 16, "VacacionesController", "FillCboMotivos", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public List<ComboDTO> FillComboCC()
        {
            try
            {
                List<ComboDTO> ccs = new List<ComboDTO>();

                ccs = _ccFS_SP.GetCCsNomina(null).Select(x => new ComboDTO
                {
                    Value = x.cc,
                    Text = "[" + x.cc + "] " + x.descripcion.Trim()
                }).OrderBy(x => x.Value).ToList();

                if (vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.ADMINISTRADOR)
                {
                    //ccs = _ccFS.GetCCs().Select(x => new ComboDTO
                    //{
                    //    Value = x.cc,
                    //    Text = "[" + x.cc + "] " + x.descripcion.Trim()
                    //}).OrderBy(x => x.Value).ToList();

                    ccs = _ccFS_SP.GetCCsNomina(true).Select(x => new ComboDTO
                    {
                        Value = x.cc,
                        Text = "[" + x.cc + "] " + x.descripcion.Trim()
                    }).OrderBy(x => x.Value).ToList();

                }
                if (vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.USUARIO)
                {
                    var usuarioCCs = _context.tblRH_BN_Usuario_CC.Where(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id).Select(x => x.cc).ToList();
                    if (usuarioCCs.Count > 0)
                    {
                        if (usuarioCCs.Any(x => x == "*"))
                        {
                            //ccs = _ccFS.GetCCs().Select(x => new ComboDTO
                            //{
                            //    Value = x.cc,
                            //    Text = "[" + x.cc + "] " + x.descripcion.Trim()
                            //}).OrderBy(x => x.Value).ToList();

                            ccs = _ccFS_SP.GetCCsNomina(true).Select(x => new ComboDTO
                            {
                                Value = x.cc,
                                Text = "[" + x.cc + "] " + x.descripcion.Trim()
                            }).OrderBy(x => x.Value).ToList();
                        }
                        else
                        {
                            //ccs = _ccFS.GetCCs(usuarioCCs).Select(x => new ComboDTO
                            //{
                            //    Value = x.cc,
                            //    Text = "[" + x.cc + "] " + x.descripcion.Trim()
                            //}).OrderBy(x => x.Value).ToList();

                            ccs = _ccFS_SP.GetCCsNominaInactivos(usuarioCCs).Select(x => new ComboDTO
                            {
                                Value = x.cc,
                                Text = "[" + x.cc + "] " + x.descripcion.Trim()
                            }).OrderBy(x => x.Value).ToList();
                        }
                    }
                }

                if ((int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                {
                    ccs = ccs.Where(e => Regex.IsMatch(e.Value, @"^\d+$")).ToList();
                }

                return ccs;

            }
            catch (Exception e)
            {
                LogError(16, 16, "VacacionesController", "FillComboCC", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public Dictionary<string, object> FillComboAutorizantes(int clave_empleado)
        {
            resultado.Clear();
            var usuariosSinRepetir = new List<ComboDTO>();

            try
            {
                var objEmpleado = _context.tblRH_EK_Empleados.FirstOrDefault(e => e.clave_empleado == clave_empleado);
                var lstUsuarios = _context.tblP_Usuario.ToList();

                if (objEmpleado == null)
                {
                    throw new Exception("Ocurrio algo mal con el empleado ingresado");
                }

                var usuarios = new List<ComboDTO>();

                var plantillasRequisiciones = new List<int> { 125 };

                var paquete = new tblFA_Paquete();

                switch (vSesiones.sesionEmpresaActual)
                {
                    case (int)EmpresaEnum.Construplan:
                    case (int)EmpresaEnum.GCPLAN:
                        //plantillasRequisiciones = new List<int> { 111, 112 };
                        plantillasRequisiciones = new List<int> { 125 };
                        paquete = _context.tblFA_Paquete.Where(x => x.cc.cc == objEmpleado.cc_contable).OrderByDescending(x => x.fechaCreacion).FirstOrDefault();
                        break;
                    case (int)EmpresaEnum.Arrendadora:
                        //plantillasRequisiciones = new List<int> { 111, 112 };
                        //plantillasRequisiciones = new List<int> { 123 };
                        //paquete = _context.tblFA_Paquete.Where(x => x.cc.cc == cc).OrderByDescending(x => x.fechaCreacion).FirstOrDefault();
                        //break;

                        plantillasRequisiciones = new List<int> { 125 };
                        if (!string.IsNullOrEmpty(objEmpleado.cc_contable))
                        {
                            var ccPaquete = _context.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.cc == objEmpleado.cc_contable);
                            paquete = _context.tblFA_Paquete.Where(x => x.ccID == ccPaquete.id).OrderByDescending(x => x.fechaCreacion).FirstOrDefault();
                        }
                        break;
                    case (int)EmpresaEnum.Colombia:
                        //plantillasRequisiciones = new List<int> { 111, 112 };
                        plantillasRequisiciones = new List<int> { 125 };
                        if (!string.IsNullOrEmpty(objEmpleado.cc_contable))
                        {
                            var ccPaquete = _context.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.cc == objEmpleado.cc_contable);
                            paquete = _context.tblFA_Paquete.Where(x => x.ccID == ccPaquete.id).OrderByDescending(x => x.fechaCreacion).FirstOrDefault();
                        }
                        break;
                    case (int)EmpresaEnum.Peru:
                        //plantillasRequisiciones = new List<int> { 124 };
                        plantillasRequisiciones = new List<int> { 125 };
                        if (!string.IsNullOrEmpty(objEmpleado.cc_contable))
                        {
                            var ccPaquete = _context.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.cc == objEmpleado.cc_contable);
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
                            string usuarioClave = item.claveEmpleado.ToString();
                            var objUsuario = lstUsuarios.FirstOrDefault(e => e.cveEmpleado == usuarioClave);

                            var usuario = new ComboDTO();
                            usuario.Value = objUsuario != null ? objUsuario.id.ToString() : "0";
                            usuario.Text = item.nombreEmpleado;
                            usuarios.Add(usuario);
                        }
                    }
                }

                foreach (var item in usuarios.GroupBy(x => x.Value))
                {
                    usuariosSinRepetir.Add(item.First());
                }
                resultado.Add(ITEMS, usuariosSinRepetir);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> FillCboUsuarios()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL CBO
                List<tblP_Usuario> lstUsuarios = _context.tblP_Usuario.Where(w => w.estatus).OrderBy(o => o.nombre).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                string nombreCompleto = string.Empty;
                foreach (var item in lstUsuarios)
                {
                    nombreCompleto = string.Empty;
                    if (!string.IsNullOrEmpty(item.nombre))
                        nombreCompleto = item.nombre.Trim();
                    if (!string.IsNullOrEmpty(item.apellidoPaterno))
                        nombreCompleto += string.Format(" {0}", item.apellidoPaterno.Trim());
                    if (!string.IsNullOrEmpty(item.apellidoMaterno))
                        nombreCompleto += string.Format(" {0}", item.apellidoMaterno.Trim());

                    if (!string.IsNullOrEmpty(nombreCompleto))
                    {
                        objComboDTO = new ComboDTO();
                        objComboDTO.Value = item.id.ToString();
                        objComboDTO.Text = nombreCompleto;
                        lstComboDTO.Add(objComboDTO);
                    }
                }

                resultado.Add(ITEMS, lstComboDTO);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboUsuarios", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion

        #region FNC GRALES

        public Dictionary<string, object> GetDatosPersona(int? claveEmpleado, string nombre)
        {
            result = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE CC, PUESTO Y FECHA DE INGRESO EN BASE AL CLAVE DEL EMPLEADO

                var objDatosPersona = _context.Select<BajaPersonalDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"							
                                SELECT
                                    CONVERT(VARCHAR(200), t1.ape_paterno) + ' ' + CONVERT(VARCHAR(200), t1.ape_materno) + ' ' + CONVERT(VARCHAR(200), t1.nombre) AS nombreCompleto, 
                                    '[' + CONVERT(VARCHAR(200), t3.cc) + '] ' + CONVERT(VARCHAR(200), t3.ccDescripcion) AS cc, 
                                    t2.descripcion AS nombrePuesto,  t1.fecha_antiguedad AS fechaIngreso, t1.puesto, t3.cc AS numCC, t3.ccDescripcion AS descripcionCC,t1.curp,t1.rfc,t1.nss,
                                    t1.clave_ciudad_nac AS idCiudad, t1.clave_estado_nac AS idEstado, t1.fecha_alta as fechaAlta, t1.fecha_antiguedad as fechaAntiguedad,
                                    t1.*, t4.*
                                FROM tblRH_EK_Empleados AS t1
                                    INNER JOIN tblRH_EK_Puestos AS t2 ON t1.puesto = t2.puesto
                                    INNER JOIN tblC_Nom_CatalogoCC AS t3 ON t1.cc_contable = t3.cc
                                    INNER JOIN tblRH_EK_Empl_Grales as t4 ON t1.clave_empleado = t4.clave_empleado
                                WHERE t1.clave_empleado = @claveEmpleado AND t1.esActivo = 1",
                    parametros = new { claveEmpleado }
                }).FirstOrDefault();
                #endregion

                bool esAdmnCH = false;
                if (vSesiones.sesionUsuarioDTO.id == 1019 || vSesiones.sesionUsuarioDTO.id == 79552 || vSesiones.sesionUsuarioDTO.idPerfil == 1)
                {
                    esAdmnCH = true;
                }

                result.Add("objDatosPersona", objDatosPersona);
                result.Add("esAdmnCH", esAdmnCH);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetDatosPersona", e, AccionEnum.CONSULTA, 0, 0);
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return result;
        }

        public int? GetNumDias(string claveEmpleado)
        {

            int? dias = 0;
            int? años = 0;
            DateTime dateVal = new DateTime();

            try
            {
                //                var odbc = new OdbcConsultaDTO()
                //                {
                //                    consulta = @"SELECT t1.clave_empleado AS claveEmpleado, t1.fecha_antiguedad as fechaAntiguedad, t2.fecha_reingreso as fechaReingreso
                //                        FROM sn_empleados as t1 
                //                        LEFT JOIN sn_empl_recontratacion as t2 
                //                        ON t1.clave_empleado = t2.clave_empleado WHERE t1.clave_empleado = ? ORDER BY fecha_reingreso desc",
                //                    parametros = new List<OdbcParameterDTO>() { new OdbcParameterDTO { nombre = "clave_empleado", tipo = OdbcType.Numeric, valor = claveEmpleado } }
                //                }; //OMAR
                //                List<FechasUserDTO> objDatosPersona = new List<FechasUserDTO>();
                //                objDatosPersona = _contextEnkontrol.Select<FechasUserDTO>(EnkontrolEnum.CplanRh, odbc);

//                var objDatosPersona = _context.Select<FechasUserDTO>(new DapperDTO
//                {
//                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
//                    consulta = @"SELECT t1.clave_empleado AS claveEmpleado, t1.fecha_antiguedad as fechaAntiguedad, t2.fecha_reingreso as fechaReingreso
//                        FROM tblRH_EK_Empleados as t1 
//                        LEFT JOIN tblRH_EK_Empl_Recontratacion as t2 
//                        ON t1.clave_empleado = t2.clave_empleado WHERE t1.clave_empleado = @claveEmpleado ORDER BY fecha_reingreso desc",
//                    parametros = new { claveEmpleado }
//                }).ToList();

                //if (objDatosPersona.Count() > 1)
                //{

                //    dateVal = objDatosPersona.FirstOrDefault().fechaReingreso;

                //}
                //else if (objDatosPersona.Count() != 0)
                //{

                //    dateVal = objDatosPersona.FirstOrDefault().fechaAntiguedad;

                //}

                int numClaveEmpleado = Convert.ToInt32(claveEmpleado);

                var objDatosPersona = _context.tblRH_EK_Empleados.FirstOrDefault(e => e.clave_empleado == numClaveEmpleado);

                dateVal = objDatosPersona.fecha_antiguedad ?? objDatosPersona.fecha_alta.Value;

                años = (DateTime.Now - dateVal).Days / 365;

                #region V1
                #region SWITCH PARA TABLA DE DIAS DE VACACIONES DIPONIBLES SEGUN LOS AÑOS DE ANTIGUEDAD
                //switch (años)
                //{

                //    case 0:
                //        dias = 0;
                //        break;

                //    case 1:
                //        dias = 6;
                //        break;

                //    case 2:
                //        dias = 8;
                //        break;

                //    case 3:
                //        dias = 10;
                //        break;

                //    case 4:
                //        dias = 12;
                //        break;

                //    case 5:
                //    case 6:
                //    case 7:
                //    case 8:
                //    case 9:
                //        dias = 14;
                //        break;

                //    case 10:
                //    case 11:
                //    case 12:
                //    case 13:
                //    case 14:
                //        dias = 16;
                //        break;

                //    case 15:
                //    case 16:
                //    case 17:
                //    case 18:
                //    case 19:
                //        dias = 18;
                //        break;

                //    case 20:
                //    case 21:
                //    case 22:
                //    case 23:
                //    case 24:
                //        dias = 20;
                //        break;

                //    case 25:
                //    case 26:
                //    case 27:
                //    case 28:
                //    case 29:
                //        dias = 22;
                //        break;

                //    case 30:
                //    case 31:
                //    case 32:
                //    case 33:
                //    case 34:
                //        dias = 24;
                //        break;

                //    case 35:
                //    case 36:
                //    case 37:
                //    case 38:
                //    case 39:
                //        dias = 26;
                //        break;

                //    case 40:
                //    case 41:
                //    case 42:
                //    case 43:
                //    case 44:
                //        dias = 28;
                //        break;

                //    default:
                //        dias = 0;
                //        break;
                //}
                #endregion

                #endregion

                #region SWITCH PARA TABLA DE DIAS DE VACACIONES DIPONIBLES SEGUN LOS AÑOS DE ANTIGUEDAD Y EMPRESA
                switch (vSesiones.sesionEmpresaActual)
                {
                    case 1:
                    case 2:
                    case 8:
                        #region CONSTRUPLAN/ARRE MEXICO

                        DateTime fechaAniversario = new DateTime(2022, dateVal.Month, dateVal.Day);

                        int añosAniversario = (DateTime.Now - fechaAniversario).Days / 365;

                        if (añosAniversario > 0)
                        {
                            #region SWITCH FECHAS

                            switch (años)
                            {

                                case 0:
                                    dias = 0;
                                    break;

                                case 1:
                                    dias = 12;
                                    break;

                                case 2:
                                    dias = 14;
                                    break;

                                case 3:
                                    dias = 16;
                                    break;

                                case 4:
                                    dias = 18;
                                    break;

                                case 5:
                                    dias = 20;
                                    break;
                                case 6:
                                case 7:
                                case 8:
                                case 9:
                                case 10:
                                    dias = 22;
                                    break;
                                case 11:
                                case 12:
                                case 13:
                                case 14:
                                case 15:
                                    dias = 24;
                                    break;
                                case 16:
                                case 17:
                                case 18:
                                case 19:
                                case 20:
                                    dias = 26;
                                    break;
                                case 21:
                                case 22:
                                case 23:
                                case 24:
                                case 25:
                                    dias = 28;
                                    break;
                                case 26:
                                case 27:
                                case 28:
                                case 29:
                                case 30:
                                    dias = 30;
                                    break;
                                case 31:
                                case 32:
                                case 33:
                                case 34:
                                case 35:
                                    dias = 32;
                                    break;
                                case 36:
                                case 37:
                                case 38:
                                case 39:
                                case 40:
                                case 41:
                                case 42:
                                case 43:
                                case 44:
                                case 45:
                                case 46:
                                case 47:
                                case 48:
                                case 49:
                                case 50:
                                    dias = 34;
                                    break;

                                default:
                                    dias = 0;
                                    break;
                            }
                            #endregion
                        }
                        else
                        {
                            dias = 0;
                        }

                        #endregion

                        break;

                    case 3:
                        //COLOMBIA
                        if (años > 0)
                        {
                            dias = 15;
                        }
                        else
                        {
                            dias = 0;
                        }

                        break;

                    case 6:
                        if (años > 0)
                        {
                            dias = 30;
                        }
                        else
                        {
                            decimal equivalDias = (decimal)((DateTime.Now - dateVal).Days / 365m) * 30m;
                            dias = (int)Math.Floor(equivalDias);
                        }
                        break;
                    default:
                        dias = 0;
                        break;
                }

                #endregion
            }
            catch (Exception e)
            {
                dias = null;
                throw;
            }

            return dias;
        }

        public int? GetNumDias(string claveEmpleado, DateTime fecha)
        {

            int? dias = 0;
            int? años = 0;
            DateTime dateVal = new DateTime();

            try
            {
                int numClaveEmpleado = Convert.ToInt32(claveEmpleado);

                var objDatosPersona = _context.tblRH_EK_Empleados.FirstOrDefault(e => e.clave_empleado == numClaveEmpleado);

                dateVal = objDatosPersona.fecha_antiguedad ?? objDatosPersona.fecha_alta.Value;

                años = (fecha.Date - dateVal.Date).Days / 365;


                #region SWITCH PARA TABLA DE DIAS DE VACACIONES DIPONIBLES SEGUN LOS AÑOS DE ANTIGUEDAD Y EMPRESA
                switch (vSesiones.sesionEmpresaActual)
                {
                    case 1:
                    case 2:
                    case 8:
                        #region CONSTRUPLAN/ARRE MEXICO

                        DateTime fechaAniversario = new DateTime(2022, dateVal.Month, dateVal.Day);

                        int añosAniversario = (fecha - fechaAniversario).Days / 365;

                        if (añosAniversario > 0)
                        {
                            #region SWITCH FECHAS

                            switch (años)
                            {

                                case 0:
                                    dias = 0;
                                    break;

                                case 1:
                                    dias = 12;
                                    break;

                                case 2:
                                    dias = 14;
                                    break;

                                case 3:
                                    dias = 16;
                                    break;

                                case 4:
                                    dias = 18;
                                    break;

                                case 5:
                                    dias = 20;
                                    break;
                                case 6:
                                case 7:
                                case 8:
                                case 9:
                                case 10:
                                    dias = 22;
                                    break;
                                case 11:
                                case 12:
                                case 13:
                                case 14:
                                case 15:
                                    dias = 24;
                                    break;
                                case 16:
                                case 17:
                                case 18:
                                case 19:
                                case 20:
                                    dias = 26;
                                    break;
                                case 21:
                                case 22:
                                case 23:
                                case 24:
                                case 25:
                                    dias = 28;
                                    break;
                                case 26:
                                case 27:
                                case 28:
                                case 29:
                                case 30:
                                    dias = 30;
                                    break;
                                case 31:
                                case 32:
                                case 33:
                                case 34:
                                case 35:
                                    dias = 32;
                                    break;
                                case 36:
                                case 37:
                                case 38:
                                case 39:
                                case 40:
                                case 41:
                                case 42:
                                case 43:
                                case 44:
                                case 45:
                                case 46:
                                case 47:
                                case 48:
                                case 49:
                                case 50:
                                    dias = 34;
                                    break;

                                default:
                                    dias = 0;
                                    break;
                            }
                            #endregion
                        }
                        else
                        {
                            dias = 0;
                        }

                        #endregion

                        break;

                    case 3:
                        //COLOMBIA
                        if (años > 0)
                        {
                            dias = 15;
                        }
                        else
                        {
                            dias = 0;
                        }

                        break;

                    case 6:
                        if (años > 0)
                        {
                            dias = 30;
                        }
                        else
                        {
                            decimal equivalDias = (decimal)((fecha - dateVal).Days / 365m) * 30m;
                            dias = (int)Math.Floor(equivalDias);
                        }
                        break;
                    default:
                        dias = 0;
                        break;
                }

                #endregion
            }
            catch (Exception e)
            {
                dias = null;
                throw;
            }

            return dias;
        }

        public int? GetNumDiasPermisos(int tipoPermiso)
        {

            int? numDiasDisponibles = 0;
            int? años = 0;
            DateTime dateVal = new DateTime();

            try
            {
                #region SWITCH PARA TABLA DE DIAS DE VACACIONES DIPONIBLES SEGUN LOS AÑOS DE ANTIGUEDAD Y EMPRESA

                //MainContextEnum
                switch (vSesiones.sesionEmpresaActual)
                {
                    case 1:
                    case 2:
                    case 8:
                        #region GETDIAS
                        switch (tipoPermiso)
                        {
                            case 0:
                                numDiasDisponibles = 5;

                                break;
                            case 1:
                                numDiasDisponibles = 3;

                                break;
                            case 2:
                                //∞
                                numDiasDisponibles = 9999999;

                                break;
                            case 3:
                                numDiasDisponibles = 3;

                                break;
                            case 4:
                                //txtCalendarioNumDias.text("MESES").trigger("change");

                                break;
                            case 5:
                                numDiasDisponibles = 1;

                                break;
                            case 6:
                                numDiasDisponibles = 1;
                                break;
                            default:
                                numDiasDisponibles = 9999999;
                                break;
                        }
                        #endregion
                        break;

                    case 3:
                        //COLOMBIA
                        numDiasDisponibles = 0;
                        break;

                    case 6:
                        numDiasDisponibles = 0;
                        break;
                    default:
                        numDiasDisponibles = 0;
                        break;
                }

                #endregion
            }
            catch (Exception e)
            {
                numDiasDisponibles = null;
                throw;
            }

            return numDiasDisponibles;
        }

        public DateTime? GetFechaIngreso(int claveEmpleado)
        {

            int? dias = 0;
            int? años = 0;
            DateTime dateVal = new DateTime();

            try
            {
                //                var odbc = new OdbcConsultaDTO()
                //                {
                //                    consulta = @"SELECT t1.clave_empleado AS claveEmpleado, t1.fecha_antiguedad as fechaAntiguedad, t2.fecha_reingreso as fechaReingreso
                //                        FROM sn_empleados as t1 
                //                        LEFT JOIN sn_empl_recontratacion as t2 
                //                        ON t1.clave_empleado = t2.clave_empleado WHERE t1.clave_empleado = ? ORDER BY fecha_reingreso desc",
                //                    parametros = new List<OdbcParameterDTO>() { new OdbcParameterDTO { nombre = "clave_empleado", tipo = OdbcType.Numeric, valor = claveEmpleado } }
                //                };
                //                List<FechasUserDTO> objDatosPersona = new List<FechasUserDTO>();
                //                objDatosPersona = _contextEnkontrol.Select<FechasUserDTO>(EnkontrolEnum.CplanRh, odbc);
//                var objDatosPersona = _context.Select<FechasUserDTO>(new DapperDTO
//                {
//                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
//                    consulta = @"SELECT t1.clave_empleado AS claveEmpleado, t1.fecha_antiguedad as fechaAntiguedad, t2.fecha_reingreso as fechaReingreso
//                        FROM tblRH_EK_Empleados as t1 
//                        LEFT JOIN tblRH_EK_Empl_Recontratacion as t2 
//                        ON t1.clave_empleado = t2.clave_empleado WHERE t1.clave_empleado = @claveEmpleado ORDER BY fecha_reingreso desc",
//                    parametros = new { claveEmpleado }
//                }).ToList();

//                if (objDatosPersona.Count() > 1)
//                {

//                    dateVal = objDatosPersona.FirstOrDefault().fechaReingreso;

//                }
//                else if (objDatosPersona.Count() != 0)
//                {

//                    dateVal = objDatosPersona.FirstOrDefault().fechaAntiguedad;

//                }

                int numClaveEmpleado = Convert.ToInt32(claveEmpleado);

                var objDatosPersona = _context.tblRH_EK_Empleados.FirstOrDefault(e => e.clave_empleado == numClaveEmpleado);

                dateVal = objDatosPersona.fecha_antiguedad ?? objDatosPersona.fecha_alta.Value;

            }
            catch (Exception e)
            {
                dias = null;
                throw;
            }

            return dateVal;
        }

        public Dictionary<string, object> SetNotificada(int id)
        {
            result = new Dictionary<string, object>();

            try
            {

               
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "SetNotificada", e, AccionEnum.CONSULTA, 0, 0);

                result.Add(SUCCESS, false);
                result.Add(MESSAGE, e.Message);
            }
            


            return result;
        }

        public DataTable getInfoEnca(string nombreReporte, string area)
        {
            string RutaServidor = "";

#if DEBUG
            RutaServidor = @"C:\Proyectos\SIGOPLANv2\ENCABEZADOS";
#else
            RutaServidor = @"\\10.1.0.112\Proyecto\SIGOPLAN\ENCABEZADOS";
#endif

            DataTable tableEncabezado = new DataTable();

            tableEncabezado.Columns.Add("logo", System.Type.GetType("System.Byte[]"));
            tableEncabezado.Columns.Add("nombreEmpresa", System.Type.GetType("System.String"));
            tableEncabezado.Columns.Add("nombreReporte", System.Type.GetType("System.String"));
            tableEncabezado.Columns.Add("area", System.Type.GetType("System.String"));

            var data = encabezadoFactoryServices.getEncabezadoDatos();
            string path = data.logo;
            //string path = Path.Combine(RutaServidor, Path.GetFileName(data.logo));
            byte[] imgdata = System.IO.File.ReadAllBytes(HostingEnvironment.MapPath(path));
            string empresa = data.nombreEmpresa;

            tableEncabezado.Rows.Add(imgdata, empresa, nombreReporte, area);

            return tableEncabezado;
        }

        public Dictionary<string, object> GetResponsable(int clvEmpleado)
        {
            result = new Dictionary<string, object>();

            try
            {
                var objResponsable = _context.tblRH_Vacaciones_Responsables.FirstOrDefault(e => e.clave_empleado == clvEmpleado);
                var objEmpleadoResp = _context.tblRH_EK_Empleados.FirstOrDefault(e => e.clave_empleado == objResponsable.clave_responsable);

                if (objResponsable == null)
                    throw new Exception();


                result.Add(SUCCESS, true);
                result.Add(ITEMS, objResponsable);
                result.Add("nombre", objEmpleadoResp != null ? (objEmpleadoResp.ape_paterno + " " + objEmpleadoResp.ape_materno + " " + objEmpleadoResp.nombre) : "");

            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, e.Message);
            }

            return result;
        }

        private string getEstatus(int est, bool aut, bool esRechazada)
        {
            if ((int)GestionEstatusEnum.PENDIENTE == (est) && aut)
                if (esRechazada)
	            {
                    return "<td style='background-color: grey;'>GESTION TERMINADA</td>";
	            }
                else
                {
                    return "<td style='background-color: yellow;'>AUTORIZANDO</td>";
                }
            else if ((int)GestionEstatusEnum.AUTORIZADO == (est))
                return "<td style='background-color: #82E0AA;'>AUTORIZADO</td>";
            else
                if ((int)GestionEstatusEnum.RECHAZADO == (est))
                    return "<td style='background-color: #EC7063;'>RECHAZADO</td>";
                else
                    return "<td style='background-color: #FAE5D3;'>PENDIENTE</td>";
        }

        #endregion

        #region CRUD VACACIONES

        public Dictionary<string, object> GetVacaciones(VacacionesDTO objFiltro)
        {
            result = new Dictionary<string, object>();
            try
            {
                bool esConsulta = false;
                var lstCC = _context.tblC_Nom_CatalogoCC.ToList();

                var objEsCH = _context.tblRH_REC_Notificantes_Altas.FirstOrDefault(e => e.esActivo && e.idUsuario == vSesiones.sesionUsuarioDTO.id);
                var lstPermisoCC = _context.tblRH_BN_Usuario_CC.Where(e => e.usuarioID == vSesiones.sesionUsuarioDTO.id).Select(e => e.cc).ToList();
                var esPermisoConsultaVacaciones = _context.tblP_AccionesVistatblP_Usuario.FirstOrDefault(e => e.tblP_AccionesVista_id == 4044 && e.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id);
                var esPermisoConsultaPermisos = _context.tblP_AccionesVistatblP_Usuario.FirstOrDefault(e => e.tblP_AccionesVista_id == 4045 && e.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id);

                if (vSesiones.sesionUsuarioDTO.idPerfil != 1)
                {
                    if (objEsCH == null)
                    {
                        if (!string.IsNullOrEmpty(vSesiones.sesionUsuarioDTO.cveEmpleado))

                            objFiltro.claveEmpleado = vSesiones.sesionUsuarioDTO.cveEmpleado;
                        else
                            throw new Exception("Ocurrio algo mal al consultar las vacaciones de su usuario favor de contactarse con el depto de TI");
                    }
                }

                #region ES CONSULTA
                if (esPermisoConsultaVacaciones != null && objFiltro.tipoVacaciones == 7)
                {
                    lstPermisoCC = new List<string>() { "*" };
                    esConsulta = true;
                }

                if (esPermisoConsultaPermisos != null && objFiltro.tipoVacaciones != 7)
                {
                    lstPermisoCC = new List<string>() { "*" };
                    esConsulta = true;                    
                }
                #endregion

                List<VacacionesDTO> lstVacaciones = _context.tblRH_Vacaciones_Vacaciones.Where(e => e.registroActivo
                    && (objFiltro.estado != 0 ? e.estado == objFiltro.estado : true)
                    && (objFiltro.tipoVacaciones.HasValue ? e.tipoVacaciones == objFiltro.tipoVacaciones : e.tipoVacaciones != 7)
                    && (!string.IsNullOrEmpty(objFiltro.ccEmpleado) ? e.cc == objFiltro.ccEmpleado :
                        (!string.IsNullOrEmpty(objFiltro.claveEmpleado) || (lstPermisoCC.Contains("*") || vSesiones.sesionUsuarioDTO.idPerfil == 1) ? true : 
                            lstPermisoCC.Contains(e.cc)))
                    && ((!string.IsNullOrEmpty(objFiltro.claveEmpleado) && !esConsulta) ? e.claveEmpleado == objFiltro.claveEmpleado : true)
                    ).Select(e => new VacacionesDTO
                    {
                        id = e.id,
                        estado = e.estado,
                        nombreEmpleado = e.nombreEmpleado,
                        claveEmpleado = e.claveEmpleado,
                        comentarioRechazada = e.comentarioRechazada ?? "",
                        tipoVacaciones = e.tipoVacaciones,
                        cc = e.cc,
                        justificacion = e.justificacion,
                        idJefeInmediato = e.idJefeInmediato,
                        nombreJefeInmediato = e.nombreJefeInmediato,
                        rutaArchivoActa = e.rutaArchivoActa,
                        esPagadas = e.esPagadas.Value,
                        consecutivo = e.consecutivo,
                    }).ToList();

                List<VacacionesDTO> lstFiltrada = new List<VacacionesDTO>();

                //lstFiltrada.AddRange(lstVacaciones);

                foreach (var item in lstVacaciones)
                {

                    var objCC = lstCC.FirstOrDefault(e => e.cc == item.cc);

                    if (objCC != null)
                    {
                        item.ccDesc = "[" + objCC.cc + "] " + objCC.ccDescripcion;
                    }
                    else
                    {
                        item.ccDesc = "S/N";
                    }

                    #region LISTA AUTH
                    var lstAuthDTO = new List<VacacionesGestionDTO>();
                    var lstAuth = _context.tblRH_Vacaciones_Gestion.Where(e => e.registroActivo && e.idVacaciones == item.id).ToList();

                    foreach (var itemAuth in lstAuth)
                    {
                        var objAuth = new VacacionesGestionDTO();

                        var objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.id == itemAuth.idUsuario);
                        string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                        objAuth.id = itemAuth.id;
                        objAuth.idUsuario = itemAuth.idUsuario;
                        objAuth.estatus = itemAuth.estatus;
                        objAuth.orden = itemAuth.orden;
                        objAuth.nombreCompleto = nombreCompleto;
                        objAuth.firmaElect = itemAuth.firmaElect;

                        lstAuthDTO.Add(objAuth);
                    }

                    item.lstAutorizantes = lstAuthDTO;
                    #endregion

                    #region SALDO PAGADAS
                    if (item.tipoVacaciones == 7 && item.esPagadas)
                    {
                        var objSaldo = _context.tblRH_Vacaciones_Saldos.FirstOrDefault(e => e.registroActivo && e.idVacacionesPagadas == item.id);

                        if (objSaldo != null)
                        {
                            item.numDiasPagados = objSaldo.num_dias;

                        }
                        else
                        {
                            item.numDiasPagados = 0;

                        }

                    }
                    #endregion

                    if (objFiltro.fechaFiltroInicio != null && objFiltro.fechaFiltroFin != null)
                    {
                        var lstFechasVacaciones = _context.tblRH_Vacaciones_Fechas.Where(e => e.registroActivo && e.vacacionID == item.id).ToList().
                            Where(e => e.fecha.Value.Date >= objFiltro.fechaFiltroInicio.Value.Date && e.fecha.Value.Date <= objFiltro.fechaFiltroFin.Value.Date).ToList();

                        if (lstFechasVacaciones.Count() > 0)
                        {
                            lstFiltrada.Add(item);
                        }
                    }
                    else
                    {
                        lstFiltrada.Add(item);
                    }
                }

                bool esAdmnCH = false;
                if (vSesiones.sesionUsuarioDTO.id == 1019 || vSesiones.sesionUsuarioDTO.id == 79552 || vSesiones.sesionUsuarioDTO.idPerfil == 1)
	            {
		            esAdmnCH = true;
	            }

                result = new Dictionary<string, object>();

                result.Add(SUCCESS, true);
                result.Add(ITEMS, lstFiltrada);
                result.Add("claveEmpleado", objFiltro.claveEmpleado);
                result.Add("esAdmnCH", esAdmnCH);
                result.Add("esRegCH", objEsCH); // REGULAR
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }

        public Dictionary<string, object> GetVacacionesById(VacacionesDTO objFiltro)
        {
            result = new Dictionary<string, object>();
            try
            {
                var lstCC = _context.tblC_Nom_CatalogoCC.ToList();
                var lstPuestos = _context.tblRH_EK_Puestos.ToList();

                var objTblVacaciones = _context.tblRH_Vacaciones_Vacaciones.FirstOrDefault(e => e.registroActivo && e.id == objFiltro.id );

                var lstUsuarios = _context.tblP_Usuario.Where(e => e.estatus).ToList();
                var objUsuarioCapturo = lstUsuarios.FirstOrDefault(e => e.id == objTblVacaciones.idUsuarioCreacion);

                VacacionesDTO objVacaciones = new VacacionesDTO()
                    {

                        id = objTblVacaciones.id,
                        estado = objTblVacaciones.estado,
                        nombreEmpleado = objTblVacaciones.nombreEmpleado,
                        claveEmpleado = objTblVacaciones.claveEmpleado,
                        comentarioRechazada = objTblVacaciones.comentarioRechazada ?? "",
                        tipoVacaciones = objTblVacaciones.tipoVacaciones,
                        cc = objTblVacaciones.cc,
                        justificacion = objTblVacaciones.justificacion,
                        nombreCapturo = (objUsuarioCapturo != null ? (objUsuarioCapturo.apellidoPaterno + " " + objUsuarioCapturo.apellidoMaterno + " " + objUsuarioCapturo.nombre) : ""),
                        fechaCreacion = objTblVacaciones.fechaCreacion,
                        idJefeInmediato = objTblVacaciones.idJefeInmediato,
                        nombreJefeInmediato = objTblVacaciones.nombreJefeInmediato,
                        esPagadas = objTblVacaciones.esPagadas.Value,
                        consecutivo = objTblVacaciones.consecutivo,
                        numDiasDisponiblesAlDiaCaptura = objTblVacaciones.numDiasDisponiblesAlDiaCaptura
                    };

                int numClave_empleado = Convert.ToInt32(objTblVacaciones.claveEmpleado);
                var objEmpleado = _context.tblRH_EK_Empleados.FirstOrDefault(e => e.esActivo && e.clave_empleado == numClave_empleado);

                var objPuesto = lstPuestos.FirstOrDefault(e => e.puesto == objEmpleado.puesto);

                objVacaciones.descPuesto = objPuesto.descripcion;

                List<VacacionesDTO> lstFiltrada = new List<VacacionesDTO>();

                var objCC = lstCC.FirstOrDefault(e => e.cc == objVacaciones.cc);

                if (objCC != null)
                {
                    objVacaciones.ccDesc = "[" + objCC.cc + "] " + objCC.ccDescripcion;
                }
                else
                {
                    objVacaciones.ccDesc = "S/N";
                }

                #region LISTA AUTH
                var lstAuthDTO = new List<VacacionesGestionDTO>();
                var lstAuth = _context.tblRH_Vacaciones_Gestion.Where(e => e.registroActivo && e.idVacaciones == objVacaciones.id).ToList();

                foreach (var itemAuth in lstAuth)
                {
                    var objAuth = new VacacionesGestionDTO();

                    var objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.id == itemAuth.idUsuario);
                    string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                    objAuth.id = itemAuth.id;
                    objAuth.idUsuario = itemAuth.idUsuario;
                    objAuth.estatus = itemAuth.estatus;
                    objAuth.orden = itemAuth.orden;
                    objAuth.nombreCompleto = nombreCompleto;
                    objAuth.firmaElect = itemAuth.firmaElect;

                    lstAuthDTO.Add(objAuth);
                }

                objVacaciones.lstAutorizantes = lstAuthDTO;
                #endregion
                
                result = new Dictionary<string, object>();

                result.Add(SUCCESS, true);
                result.Add(ITEMS, objVacaciones);
                result.Add("claveEmpleado", objFiltro.claveEmpleado);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }

        public Dictionary<string, object> GetVacacionesIncidencias(VacacionesDTO objFiltro)
        {
            result = new Dictionary<string, object>();

            try
            {
                int erroresGuardado = 0;
                List<VacacionesDTO> lstVacaciones = _context.tblRH_Vacaciones_Vacaciones.Where(e => e.registroActivo
                    && (objFiltro.estado != 0 ? e.estado == objFiltro.estado : true)
                    ).Select(e => new VacacionesDTO
                    {

                        id = e.id,
                        estado = e.estado,
                        nombreEmpleado = e.nombreEmpleado,
                        claveEmpleado = e.claveEmpleado,
                        comentarioRechazada = e.comentarioRechazada ?? "",
                        tipoVacaciones = e.tipoVacaciones
                    }).ToList();

                List<VacacionesDTO> lstFiltrada = new List<VacacionesDTO>();

                lstFiltrada.AddRange(lstVacaciones);

                foreach (var item in lstFiltrada)
                {
                    var dicDatosPersona = GetDatosPersona(Convert.ToInt32(item.claveEmpleado), "");
                    BajaPersonalDTO bajaPersonalDTO = dicDatosPersona["objDatosPersona"] as BajaPersonalDTO;
                    if (objFiltro.ccEmpleado != null)
                    {
                        if (objFiltro.ccEmpleado != bajaPersonalDTO.numCC)
                        {
                            lstVacaciones.Remove(item);
                        }
                        else
                        {
                            item.ccEmpleado = bajaPersonalDTO.cc;
                        }
                    }
                    else
                    {
                        item.ccEmpleado = bajaPersonalDTO.cc;
                    }
                }

                var vacacionesIDs = lstFiltrada.Select(x => x.id).ToList();
                List<tblRH_Vacaciones_Fechas> lstFechas = _context.tblRH_Vacaciones_Fechas.Where(x => vacacionesIDs.Contains(x.vacacionID)
                    && (objFiltro.tipoVacaciones == 0 ? true : (x.tipoInsidencia == objFiltro.tipoVacaciones)) && (objFiltro.estado == 4 ? x.incidenciaAplicada : !x.incidenciaAplicada)).OrderBy(e => e.fecha.Value).ToList();
                var lstFechasIncidencias = lstFechas.Select(x =>
                {
                    var vacacionPpal = lstFiltrada.FirstOrDefault(y => y.id == x.vacacionID);
                    return new IncidenciasVacacionesDTO
                    {
                        id = x.id,
                        vacacionID = x.vacacionID,
                        claveEmpleado = vacacionPpal.claveEmpleado,
                        nombreEmpleado = vacacionPpal.nombreEmpleado,
                        ccDescripcion = vacacionPpal.ccEmpleado,
                        fecha = x.fecha ?? DateTime.Today,
                        tipoVacaciones = x.tipoInsidencia,
                        aplica = true,
                        cc = vacacionPpal.ccEmpleado.Split(' ')[0]
                    };
                }).OrderBy(x => x.claveEmpleado).ThenByDescending(x => x.fecha).ToList();

                if (lstFechasIncidencias.Count() > 0)
                {

                    List<Tuple<int, int>> periodos = new List<Tuple<int, int>>();

                    var fechaInicioIncidencias = lstFechasIncidencias.Min(x => x.fecha);
                    var fechaFinIncidencias = lstFechasIncidencias.Max(x => x.fecha);

                    var periodosIncidencias = _context.tblRH_BN_EstatusPeriodos.Where(x => x.fecha_inicial >= fechaInicioIncidencias && x.fecha_final <= fechaFinIncidencias).ToList();

                    foreach (var item in lstFechasIncidencias)
                    {
                        var auxPeriodoIncidencias = periodosIncidencias.Where(x => x.fecha_inicial <= item.fecha && x.fecha_final >= item.fecha).ToList();
                        if (auxPeriodoIncidencias.Count() > 0)
                        {
                            var checkExisteIncidencia = _context.tblRH_BN_Incidencia_det.Where(x => x.clave_empleado.ToString() == item.claveEmpleado).ToList();
                            if (checkExisteIncidencia.Count() > 0) item.aplica = true;
                            else item.aplica = false;
                        }
                        else { item.aplica = false; }
                    }
                }

                result = new Dictionary<string, object>();
                result.Add(SUCCESS, true);
                result.Add(ITEMS, lstFechasIncidencias);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }

        public Dictionary<string, object> GuardarVacacionesIncidencias(List<int> fechasIDs)
        {
            result = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var fechas = _context.tblRH_Vacaciones_Fechas.Where(x => fechasIDs.Contains(x.id)).OrderBy(e => e.fecha.Value).ToList();

                    foreach (var item in fechas)
                    {
                        var periodos = _context.tblRH_BN_EstatusPeriodos.Where(x => x.fecha_inicial <= item.fecha && x.fecha_final >= item.fecha).ToList();
                        var vacacion = _context.tblRH_Vacaciones_Vacaciones.FirstOrDefault(x => x.id == item.vacacionID);
                        foreach (var periodo in periodos)
                        {
                            var incidencias = _context.tblRH_BN_Incidencia.Where(x => x.anio == periodo.anio && x.periodo == periodo.periodo && x.tipo_nomina == periodo.tipo_nomina).ToList();
                            foreach (var incidencia in incidencias)
                            {
                                tblRH_BN_Incidencia auxIncidencia = new tblRH_BN_Incidencia();
                                bool flagAplicaHaciaAdelante = false;
                                if (incidencia.estatus != "P")
                                {
                                    auxIncidencia = _context.tblRH_BN_Incidencia.FirstOrDefault(x => x.anio >= incidencia.anio && x.periodo >= incidencia.periodo && x.tipo_nomina == incidencia.tipo_nomina && x.cc == incidencia.cc && x.estatus == "P");
                                    flagAplicaHaciaAdelante = true;
                                }
                                else { auxIncidencia = incidencia; }

                                if (auxIncidencia != null)
                                {
                                    var incidenciaDet = _context.tblRH_BN_Incidencia_det.FirstOrDefault(x => x.clave_empleado.ToString() == vacacion.claveEmpleado && x.incidenciaID == incidencia.id);
                                    if (incidenciaDet != null && ((!flagAplicaHaciaAdelante) || (flagAplicaHaciaAdelante && item.tipoInsidencia == 3)))
                                    {
                                        int diferenciaDias = ((item.fecha ?? periodo.fecha_inicial).Date - periodo.fecha_inicial.Date).TotalDays.ParseInt(0);
                                        switch (diferenciaDias)
                                        {
                                            case 0: incidenciaDet.dia1 = item.tipoInsidencia; break;
                                            case 1: incidenciaDet.dia2 = item.tipoInsidencia; break;
                                            case 2: incidenciaDet.dia3 = item.tipoInsidencia; break;
                                            case 3: incidenciaDet.dia4 = item.tipoInsidencia; break;
                                            case 4: incidenciaDet.dia5 = item.tipoInsidencia; break;
                                            case 5: incidenciaDet.dia6 = item.tipoInsidencia; break;
                                            case 6: incidenciaDet.dia7 = item.tipoInsidencia; break;
                                            case 7: incidenciaDet.dia8 = item.tipoInsidencia; break;
                                            case 8: incidenciaDet.dia9 = item.tipoInsidencia; break;
                                            case 9: incidenciaDet.dia10 = item.tipoInsidencia; break;
                                            case 10: incidenciaDet.dia11 = item.tipoInsidencia; break;
                                            case 11: incidenciaDet.dia12 = item.tipoInsidencia; break;
                                            case 12: incidenciaDet.dia13 = item.tipoInsidencia; break;
                                            case 13: incidenciaDet.dia14 = item.tipoInsidencia; break;
                                            case 14: incidenciaDet.dia15 = item.tipoInsidencia; break;
                                            case 15: incidenciaDet.dia16 = item.tipoInsidencia; break;
                                            default: incidenciaDet.dia1 = item.tipoInsidencia; break;
                                        }
                                        _context.SaveChanges();
                                        item.incidenciaAplicada = true;
                                        item.idIncidencia = incidenciaDet.incidenciaID;
                                        _context.SaveChanges();
                                    }

                                }
                            }
                        }
                    }

                    dbContextTransaction.Commit();
                    result.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Commit();
                    result.Add(SUCCESS, true);
                }
            }
            return result;
        }

        public Dictionary<string, object> CrearEditarVacaciones(VacacionesDTO objVacaciones)
        {
            result = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var objCEVacaciones = new tblRH_Vacaciones_Vacaciones();
                    var lstUsuarios = _context.tblP_Usuario.Where(e => e.estatus).ToList();

                    int clave_empleado = Convert.ToInt32(objVacaciones.claveEmpleado);
                    var objEmpleado = _context.tblRH_EK_Empleados.FirstOrDefault(e => e.esActivo && e.estatus_empleado == "A" && e.clave_empleado == clave_empleado);

                    if (objEmpleado == null)
                    {
                        throw new Exception("El usuario a capturar debe estar en estatus de activo");
                    }

                    if (objVacaciones.id > 0)
                    {
                        objCEVacaciones = _context.tblRH_Vacaciones_Vacaciones.Where(e => e.id == objVacaciones.id).FirstOrDefault();

                        if (objCEVacaciones == null)
                            throw new Exception("Ocurrio algo mal");

                        #region EDITAR
                        objCEVacaciones.nombreEmpleado = objVacaciones.nombreEmpleado;
                        objCEVacaciones.claveEmpleado = objVacaciones.claveEmpleado;
                        objCEVacaciones.cc = objEmpleado.cc_contable;
                        objCEVacaciones.esPagadas = objVacaciones.esPagadas;
                        objCEVacaciones.justificacion = objVacaciones.justificacion;
                        objCEVacaciones.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;

                        #region JEFE INMEDIATO

                        if (objCEVacaciones.idJefeInmediato != objVacaciones.idJefeInmediato)
                        {
                            var objUsuario = lstUsuarios.FirstOrDefault(e => e.id == objVacaciones.idJefeInmediato);

                            if (objUsuario != null)
	                        {
                               objCEVacaciones.nombreJefeInmediato = (objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno);
	                        }
                        }

                        #endregion

                        objCEVacaciones.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();

                        #region ACTUALIZAR AUTORIZANTES

                        var lstAuth = _context.tblRH_Vacaciones_Gestion.Where(e => e.registroActivo && e.idVacaciones == objVacaciones.id).ToList();
                        bool esReset = false;

                        #region RESPONSABLE CC

                        var objResponsable = lstAuth.FirstOrDefault(e => e.orden == OrdenGestionEnum.RESPONSABLE_CC);
                        var objNewResponsable = objVacaciones.lstAutorizantes.FirstOrDefault(e => e.orden == OrdenGestionEnum.RESPONSABLE_CC);

                        if (objResponsable != null && objResponsable.idUsuario != objNewResponsable.idUsuario)
                        {
                            objResponsable.idUsuario = objNewResponsable.idUsuario;
                            objResponsable.estatus = GestionEstatusEnum.PENDIENTE;
                            objResponsable.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                            objResponsable.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                            objResponsable.fechaCreacion = DateTime.Now;
                            objResponsable.fechaModificacion = DateTime.Now;
                            _context.SaveChanges();

                            esReset = true;
                        }
                        #endregion

                        var objPagadas1 = lstAuth.FirstOrDefault(e => e.orden == OrdenGestionEnum.AUTORIZANTE_PAGADAS_1);
                        var objPagadas2 = lstAuth.FirstOrDefault(e => e.orden == OrdenGestionEnum.AUTORIZANTE_PAGADAS_2);
                        if (objVacaciones.esPagadas)
                        {
                            #region AUTH PAGADAS 1
                            var objNewPagadas1 = objVacaciones.lstAutorizantes.FirstOrDefault(e => e.orden == OrdenGestionEnum.AUTORIZANTE_PAGADAS_1);

                            if (objPagadas1 != null && objPagadas1.idUsuario != objNewPagadas1.idUsuario)
                            {
                                objPagadas1.idUsuario = objNewPagadas1.idUsuario;
                                objPagadas1.estatus = GestionEstatusEnum.PENDIENTE;
                                objPagadas1.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                objPagadas1.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                objPagadas1.fechaCreacion = DateTime.Now;
                                objPagadas1.fechaModificacion = DateTime.Now;
                                _context.SaveChanges();

                                esReset = true;
                            }
                            #endregion

                            #region AUTH PAGADAS 2
                            var objNewPagadas2 = objVacaciones.lstAutorizantes.FirstOrDefault(e => e.orden == OrdenGestionEnum.AUTORIZANTE_PAGADAS_2);

                            if (objPagadas2 != null && objPagadas2.idUsuario != objNewPagadas2.idUsuario)
                            {
                                objPagadas2.idUsuario = objNewPagadas2.idUsuario;
                                objPagadas2.estatus = GestionEstatusEnum.PENDIENTE;
                                objPagadas2.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                objPagadas2.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                objPagadas2.fechaCreacion = DateTime.Now;
                                objPagadas2.fechaModificacion = DateTime.Now;
                                _context.SaveChanges();

                                esReset = true;
                            }
                            #endregion
                        }

                        //RESET GESTION
                        if (esReset)
                        {
                            objResponsable.estatus = GestionEstatusEnum.PENDIENTE;

                            if (objVacaciones.esPagadas)
                            {
                                objPagadas1.estatus = GestionEstatusEnum.PENDIENTE;
                                objPagadas2.estatus = GestionEstatusEnum.PENDIENTE;
                            }

                            //REMOVER ALERTAS VIEJAS
                            var alertasVacaciones = _context.tblP_Alerta.Where(e => e.sistemaID == 16 && e.objID == objCEVacaciones.id && e.obj == "AutorizacionVacaciones").ToList();

                            foreach (var item in alertasVacaciones)
                            {
                                item.visto = true;
                                _context.SaveChanges();
                            }

                            _context.SaveChanges();
                        }
                        #endregion

                        #region ACTUALIZAR SALDOS VACACIONES

                        if (objCEVacaciones.esPagadas.HasValue && objCEVacaciones.esPagadas.Value && objCEVacaciones.tipoVacaciones == 7)
                        {
                            var objSaldo = _context.tblRH_Vacaciones_Saldos.FirstOrDefault(e => e.registroActivo && e.idVacacionesPagadas == objCEVacaciones.id);

                            if (objSaldo != null)
                            {
                                objSaldo.num_dias = -objVacaciones.numDiasPagados;
                                objSaldo.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                objSaldo.fechaModificacion = DateTime.Now;
                                _context.SaveChanges();
                            }
                        }

                        #endregion

                        #endregion
                    }
                    else
                    {
                        #region CREAR
                        var vacacionesEmpleado = _context.tblRH_Vacaciones_Vacaciones.Where(x => x.claveEmpleado == objVacaciones.claveEmpleado).ToList();
                        int consecutivo = 0;
                        if (vacacionesEmpleado.Count() > 0) consecutivo = vacacionesEmpleado.Max(x => x.consecutivo);
                        objCEVacaciones.estado = 3;
                        objCEVacaciones.nombreEmpleado = objVacaciones.nombreEmpleado;
                        objCEVacaciones.claveEmpleado = objVacaciones.claveEmpleado;
                        //objCEVacaciones.idPeriodo = objVacaciones.idPeriodo;
                        objCEVacaciones.cc = objEmpleado.cc_contable;
                        objCEVacaciones.esPagadas = objVacaciones.esPagadas;
                        objCEVacaciones.justificacion = objVacaciones.justificacion;
                        objCEVacaciones.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCEVacaciones.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCEVacaciones.tipoVacaciones = objVacaciones.tipoVacaciones.Value;
                        objCEVacaciones.consecutivo = consecutivo + 1;
                        objCEVacaciones.idJefeInmediato = objVacaciones.idJefeInmediato;
                        objCEVacaciones.nombreJefeInmediato = objVacaciones.nombreJefeInmediato;
                        objCEVacaciones.fechaCreacion = DateTime.Now;
                        objCEVacaciones.fechaModificacion = DateTime.Now;
                        objCEVacaciones.registroActivo = true;
                        _context.tblRH_Vacaciones_Vacaciones.Add(objCEVacaciones);
                        _context.SaveChanges();

                        foreach (var item in objVacaciones.lstAutorizantes)
                        {
                            var objAuth = new tblRH_Vacaciones_Gestion();

                            objAuth.idVacaciones = objCEVacaciones.id;
                            objAuth.idUsuario = item.idUsuario;
                            objAuth.orden = item.orden;
                            objAuth.estatus = GestionEstatusEnum.PENDIENTE;
                            objAuth.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                            objAuth.idUsuarioModificacion = null;
                            objAuth.fechaCreacion = DateTime.Now;
                            objAuth.fechaModificacion = null;
                            objAuth.registroActivo = true;

                            _context.tblRH_Vacaciones_Gestion.Add(objAuth);
                            _context.SaveChanges();
                        }

                        #region PAGADAS

                        if (objVacaciones.esPagadas && objVacaciones.tipoVacaciones == 7)
                        {
                            var objSaldo = new tblRH_Vacaciones_Saldos()
                            {
                                clave_empleado = objEmpleado.clave_empleado,
                                idVacacionesPagadas = objCEVacaciones.id,
                                num_dias = -objVacaciones.numDiasPagados,
                                FK_UsuarioCreacion = vSesiones.sesionUsuarioDTO.id,
                                FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id,
                                fechaCreacion = DateTime.Now,
                                fechaModificacion = DateTime.Now,
                                registroActivo = true,
                            };

                            _context.tblRH_Vacaciones_Saldos.Add(objSaldo);
                            _context.SaveChanges();

                            #region CORREO

                            #region DOCUMENTO


                            ReportDocument rptCV = new rptFechasVacaciones();

                            //string path = Path.Combine(RutaServidor, "rptFechasVacaciones.rpt");
                            //rptCV.Load(path);

                            var meses = new List<string>() { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

                            var lstStrDias = new List<object>();

                            var objVacacionesReporte = GetVacacionesById(new VacacionesDTO() { id = objCEVacaciones.id })["items"] as VacacionesDTO;

                            //LIMPIAR RESULTADO
                            resultado.Clear();
                            result.Clear();

                            //var objResponsable = vacacionesService.GetResponsables(null, Convert.ToInt32(objVacacionesReporte.claveEmpleado))["data"] as List<Core.DTO.RecursosHumanos.Vacaciones.ResponsableDTO>;

                            var dicSaldos = GetFechasByClaveEmpleado(Convert.ToInt32(objVacacionesReporte.claveEmpleado), objCEVacaciones.tipoVacaciones);
                            var diasIniciales = dicSaldos["diasIniciales"] as int?;
                            var lstTotalDias = dicSaldos["items"] as List<tblRH_Vacaciones_Fechas>;
                            var lstSaldos = dicSaldos["lstSaldos"] as List<tblRH_Vacaciones_Saldos>;
                            //var lstDias = dicSaldos["items"] as List<tblRH_Vacaciones_Fechas>;
                            var objSaldoPagadas = lstSaldos.FirstOrDefault(e => e.idVacacionesPagadas == objVacacionesReporte.id);

                            //LIMPIAR RESULTADO
                            resultado.Clear();
                            result.Clear();

                            DateTime? fechaIngreso = GetFechaIngreso(Convert.ToInt32(objVacacionesReporte.claveEmpleado));

                            int años = (DateTime.Now - fechaIngreso.Value).Days / 365;

                            int añoVencer = fechaIngreso.Value.Year + años;

                            if (DateTime.Now.Day < fechaIngreso.Value.Day && DateTime.Now.Month == fechaIngreso.Value.Month)
                            {
                                añoVencer--;
                            }

                            DateTime nextDate = new DateTime(añoVencer, fechaIngreso.Value.Month, fechaIngreso.Value.Day);

                            var today = DateTime.Now;

                            string diasDisfrutar = "";
                            DateTime? currentDate = null;

                            var lstDias = _context.tblRH_Vacaciones_Fechas.Where(e => e.registroActivo && (objCEVacaciones.id > 0 ? e.vacacionID == objCEVacaciones.id : true)).OrderBy(e => e.fecha.Value).ToList();

                            var saldoDias = (diasIniciales ?? 0) - lstTotalDias.Count();
                            //saldoDias = saldoDias + lstDias.Count();
                            saldoDias = (objVacaciones.numDiasDisponiblesAlDiaCaptura ?? saldoDias) + lstDias.Count();

                            List<tblRH_Vacaciones_Fechas> lstFechasFiltro = new List<tblRH_Vacaciones_Fechas>();

                            var min = lstDias.Select(e => e.fecha).Min();
                            var max = lstDias.Select(e => e.fecha).Max();

                            string descMotivoVac = "";

                            switch (objVacacionesReporte.tipoVacaciones)
                            {
                                case 0:
                                    descMotivoVac = "Permiso paternidad (PGS)";
                                    break;
                                case 1:
                                    descMotivoVac = "Permiso de matrimonio (PGS)";
                                    break;
                                case 2:
                                    descMotivoVac = "Permiso sindical (PGS)";
                                    break;
                                case 3:
                                    descMotivoVac = "Permiso por fallecimiento (PGS)";
                                    break;
                                case 5:
                                    descMotivoVac = "Permiso médico (PGS)";
                                    break;
                                case 7:
                                    descMotivoVac = "Vacaciones (VAC)";
                                    break;
                                case 8:
                                    descMotivoVac = "Permiso SIN goce de sueldo (PS)";
                                    break;
                                case 9:
                                    descMotivoVac = "Permiso de comision de trabajo (CT)";
                                    break;
                                case 10:
                                    descMotivoVac = "Home office (PGS)";
                                    break;
                                case 11:
                                    descMotivoVac = "Tiempo x tiempo (PGS)";
                                    break;
                                case 13:
                                    descMotivoVac = "Suspención (SUSP)";
                                    break;
                                default:
                                    descMotivoVac = "S/N";
                                    break;
                            }

                            var objResponsableCC = objVacacionesReporte.lstAutorizantes.FirstOrDefault(e => e.orden == Core.Enum.RecursosHumanos.Vacaciones.OrdenGestionEnum.RESPONSABLE_CC);
                            var objRespPagadas1 = objVacacionesReporte.lstAutorizantes.FirstOrDefault(e => e.orden == Core.Enum.RecursosHumanos.Vacaciones.OrdenGestionEnum.AUTORIZANTE_PAGADAS_1);
                            //var objJefeInmediato = objVacacionesReporte.lstAutorizantes.FirstOrDefault(e => e.orden == Core.Enum.RecursosHumanos.Vacaciones.OrdenGestionEnum.JEFE_INMEDIATO);

                            var objStrVacaciones = new
                            {
                                nombreEmpleado = objVacacionesReporte.nombreEmpleado,
                                claveEmpleado = objVacacionesReporte.claveEmpleado,
                                ccEmpleado = objVacacionesReporte.cc,
                                //periodoDesc = lstPeriodos.FirstOrDefault(e => e.id == objVacacionesReporte.idPeriodo).periodoDesc,
                                fechaInicial = min != null ? min.Value.ToString("dd/MM/yyyy") : "",
                                fechaFinal = max != null ? max.Value.ToString("dd/MM/yyyy") : "",
                                nombreResponsable = objResponsableCC.nombreCompleto,
                                nombreResponsablePagadas = objRespPagadas1 != null ? objRespPagadas1.nombreCompleto : "  ",
                                //claveResponsable = objVacacionesReporte.claveResponsable,
                                //claveResponsablePagadas = objVacacionesReporte.claveResponsablePagadas ?? "",
                                folio = (objVacaciones.cc == null ? "" : objVacaciones.cc) + "-" + (objVacaciones.claveEmpleado == null ? "" : objVacaciones.claveEmpleado) + "-" + (objVacaciones.consecutivo == null ? "" : objVacaciones.consecutivo.ToString().PadLeft(3, '0'))

                            };

                            string strDias = "";

                            foreach (var item in lstDias)
                            {
                                #region NUEVA LISTA DIAS
                                strDias += "[" + item.fecha.Value.ToString("dd/MM/yyyy") + "] ";
                                #endregion

                                lstStrDias.Add(new
                                {

                                    fecha = item.fecha.Value.ToString("dddd", new CultureInfo("es-ES")) + ", " + item.fecha.Value.Day + " de " + meses[item.fecha.Value.Month - 1] +
                                    " de " + item.fecha.Value.Year,
                                    //fecha = item.fecha.Value.ToString("dd/MM/yyyy"),
                                    //periodo = objStrVacaciones.periodoDesc

                                });

                                if (currentDate != null)
                                {
                                    if (currentDate.Value.Month == item.fecha.Value.Month)
                                    {
                                        diasDisfrutar += item.fecha.Value.Day.ToString() + ", ";

                                    }
                                    else
                                    {

                                        diasDisfrutar += "de " + currentDate.Value.ToString("MMMM", new CultureInfo("es-ES")) + " ";
                                        if (currentDate.Value.Year != item.fecha.Value.Year)
                                        {
                                            diasDisfrutar += "del " + currentDate.Value.Year + " ";

                                        }
                                        diasDisfrutar += item.fecha.Value.Day.ToString() + ", ";

                                        if (item.fecha.Value == max)
                                        {
                                            diasDisfrutar += "de " + item.fecha.Value.ToString("MMMM", new CultureInfo("es-ES")) + " ";
                                            diasDisfrutar += "del " + item.fecha.Value.Year + " ";

                                        }
                                    }
                                }
                                else
                                {
                                    diasDisfrutar += item.fecha.Value.Day.ToString() + ", ";

                                }

                                currentDate = item.fecha.Value;

                            }

                            #region TOTAL DIASGetVacacionesById
                            int diasDisponibles = 0;
                            string strDiasDisponibles = "";
                            bool esSaldoMuchos = false;

                            if (objVacacionesReporte.tipoVacaciones == 7)
                            {
                                if (!objVacacionesReporte.esPagadas)
	                            {
                                    if (min.Value.Month == max.Value.Month)
                                    {
                                        diasDisfrutar += "de " + currentDate.Value.ToString("MMMM", new CultureInfo("es-ES")) + " ";

                                    }

                                    diasDisponibles = saldoDias - lstDias.Count();
                                    strDiasDisponibles = diasDisponibles.ToString();
	                            }
                                else
                                {
                                    strDias = (objSaldoPagadas.num_dias * -1) + " dias a Pagar";

                                    diasDisponibles = saldoDias;
                                    strDiasDisponibles = diasDisponibles.ToString();
                                }
                            }
                            else
                            {
                                int? diasTotales = GetNumDiasPermisos(objVacacionesReporte.tipoVacaciones.Value);

                                //true: Permisos con saldos finitos | false: permisos con saldos infinitos
                                if (objVacacionesReporte.tipoVacaciones != 2 &&
                                    objVacacionesReporte.tipoVacaciones != 4 &&
                                    objVacacionesReporte.tipoVacaciones != 8 &&
                                    objVacacionesReporte.tipoVacaciones != 9 &&
                                    objVacacionesReporte.tipoVacaciones != 10 &&
                                    objVacacionesReporte.tipoVacaciones != 11)
                                {
                                    diasDisponibles = diasTotales.Value - lstTotalDias.Count();
                                    strDiasDisponibles = diasDisponibles.ToString();
                                }
                                else
                                {
                                    esSaldoMuchos = true;
                                    strDiasDisponibles = "MUCHOS";

                                }

                                //if (min.Value.Month == max.Value.Month)
                                //{
                                //   //COMENTARIOS PERMISO

                                //}
                            }
                            #endregion

                            rptCV.Database.Tables[0].SetDataSource(new[] { objStrVacaciones });
                            rptCV.Database.Tables[1].SetDataSource(getInfoEnca("reporte", ""));
                            rptCV.Database.Tables[2].SetDataSource(lstStrDias);

                            rptCV.SetParameterValue("todayDate", objVacacionesReporte.fechaCreacion.Value.ToString("dd/MM/yyyy"));
                            rptCV.SetParameterValue("fechaIngreso", fechaIngreso.Value.ToString("dd/MM/yyyy"));
                            rptCV.SetParameterValue("diasDisponibles", (esSaldoMuchos ? strDiasDisponibles : saldoDias.ToString()));
                            rptCV.SetParameterValue("diasDisfrutar", diasDisfrutar);
                            rptCV.SetParameterValue("numDiasDisfrutar", (objVacacionesReporte.esPagadas ? (objSaldoPagadas.num_dias * -1).ToString() : lstDias.Count().ToString()));
                            rptCV.SetParameterValue("descMotivo", descMotivoVac);
                            rptCV.SetParameterValue("justificacion", objVacacionesReporte.justificacion ?? "  ");
                            rptCV.SetParameterValue("strDias", strDias);
                            rptCV.SetParameterValue("firmaElectJefeInmediato", (" "));
                            rptCV.SetParameterValue("firmaElectResponsableCC", (" "));
                            rptCV.SetParameterValue("descPuesto", objVacacionesReporte.descPuesto ?? " ");
                            rptCV.SetParameterValue("nombreJefeInmediato", objVacacionesReporte.nombreJefeInmediato);
                            rptCV.SetParameterValue("nombreResponsableCC", objResponsableCC.nombreCompleto);
                            rptCV.SetParameterValue("ccDesc", objVacacionesReporte.ccDesc.Trim());
                            rptCV.SetParameterValue("diasRestantes", (esSaldoMuchos ? strDiasDisponibles : diasDisponibles.ToString()));
                            rptCV.SetParameterValue("nombreCapturo", objVacacionesReporte.nombreCapturo);
                            rptCV.SetParameterValue("estatusResponsable", " ");
                            rptCV.SetParameterValue("tipoVacaciones", objVacacionesReporte.tipoVacaciones);
                            rptCV.SetParameterValue("tituloReporte", (objVacacionesReporte.tipoVacaciones == 7 ? "Control de vacaciones" : "Control de ausencias"));

                            Stream stream = rptCV.ExportToStream(ExportFormatType.PortableDocFormat);

                            #endregion

                            #region CUERPO CORREO

                            var lstCorreosNotificarTodos = new List<string>();

                            string cuerpo =
                                        @"<html>
                                            <head>
                                                <style>
                                                    table {
                                                        font-family: arial, sans-serif;
                                                        border-collapse: collapse;
                                                        width: 100%;
                                                    }

                                                    td, th {
                                                        border: 1px solid #dddddd;
                                                        text-align: left;
                                                        padding: 8px;
                                                    }

                                                    tr:nth-child(even) {
                                                        background-color: #ffcc5c;
                                                    }
                                                </style>
                                            </head>
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>

                                                    <p class=MsoNormal>
                                                        Buen día,<br><br>" +
                                                                "Se han capturado ausencias y estan listas para su gestion"
                                                                + @".<br><br>
                                                    </p>
                                                    <p class=MsoNormal style='font-weight:bold;'>" +
                                                                "Empleado: [" + objVacacionesReporte.claveEmpleado + "] " + objVacacionesReporte.nombreEmpleado + @".<o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal style='font-weight:bold;'>" +
                                                                "CC: " + objVacacionesReporte.ccDesc.Trim() + @".<o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal style='font-weight:bold;'>" +
                                                                "Puesto: " + objVacacionesReporte.descPuesto + @".<o:p></o:p>
                                                    </p>
                                                    <br><br><br>
                                            ";
                            #region TABLA AUTORIZANTES

                            cuerpo += @"
                                <table>
                                    <thead>
                                        <tr>
                                            <th>Nombre</th>
                                            <th>Tipo</th>
                                            <th>Autorizo</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    ";

                            bool esPrimero = true;
                            //EstatusGestionAutorizacionEnum P 0 A 1 R 2 
                            if (objVacacionesReporte.lstAutorizantes != null)
                            {
                                foreach (var itemDet in objVacacionesReporte.lstAutorizantes)
                                {
                                    tblP_Usuario objUsuario = lstUsuarios.FirstOrDefault(e => e.id == itemDet.idUsuario);
                                    string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                                    cuerpo += "<tr>" +
                                                "<td>" + nombreCompleto + "</td>" +
                                                "<td>" + EnumHelper.GetDescription(itemDet.orden) + "</td>" +
                                                getEstatus(0, esPrimero, false) +
                                            "</tr>";

                                    if (esPrimero)
                                    {
                                        lstCorreosNotificarTodos.Add(objUsuario.correo);
                                        esPrimero = false;
                                    }
                                }
                            }

                            cuerpo += "</tbody>" +
                                        "</table>" +
                                        "<br><br><br>";


                            #endregion

                            cuerpo += "<br><br><br>" +
                                  "Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>" +
                                  "Construplan > Capital Humano > Administración de Personal > Incidencias > Control de Ausencias > Gestion > Gestión de Ausencias.<br><br>" +
                                  "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                                  "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias." +
                                " </body>" +
                              "</html>";

                            #endregion

                            var objUsuarioLogged = lstUsuarios.FirstOrDefault(e => e.id == vSesiones.sesionUsuarioDTO.id);

                            if (objUsuarioLogged != null)
                            {
                                lstCorreosNotificarTodos.Add(objUsuarioLogged.correo);

                            }

                            var objJefeInmediato = lstUsuarios.FirstOrDefault(e => e.id == objVacacionesReporte.idJefeInmediato);

                            if (objJefeInmediato != null)
                            {
                                lstCorreosNotificarTodos.Add(objJefeInmediato.correo);
                                
                            }

                            lstCorreosNotificarTodos.Add("serviciosalpersonal@construplan.com.mx");

#if DEBUG
                            lstCorreosNotificarTodos = new List<string>();
                            //lstCorreosNotificarTodos.Add("omar.nunez@construplan.com.mx");
                            lstCorreosNotificarTodos.Add("miguel.buzani@construplan.com.mx");
#endif

                            string descTipoVacaciones = "";

                            if (objVacacionesReporte.tipoVacaciones == 7)
                            {
                                descTipoVacaciones = "VACACIONES LISTAS PARA SU GESTION. CC: {2} EMPLEADO: [{0}] {1}";
                            }
                            else
                            {
                                descTipoVacaciones = "AUSENCIAS LISTAS PARA SU GESTION. CC: {2} EMPLEADO: [{0}] {1}";
                            }

                            List<byte[]> downloadPDFs = new List<byte[]>();
                            using (var streamReader = new MemoryStream())
                            {
                                stream.CopyTo(streamReader);
                                downloadPDFs.Add(streamReader.ToArray());

                                GlobalUtils.sendEmailAdjuntoInMemory2(string.Format(PersonalUtilities.GetNombreEmpresa() + ": " + descTipoVacaciones, objVacacionesReporte.claveEmpleado, objVacacionesReporte.nombreEmpleado, objVacacionesReporte.cc),
                                    cuerpo, lstCorreosNotificarTodos, downloadPDFs, (objVacacionesReporte.tipoVacaciones == 7 ? "Vacaciones.pdf" : "ControldeAusencias.pdf"));

                            }
                            #endregion
                        }
                        #endregion

                        #region Alerta SIGOPLAN
                        //PRIMER AUTORIZANTE
                        string txtAlerta = (objVacaciones.tipoVacaciones == 7 ? "Vacaciones Num. Emp: {0}" : "Control de Ausencias Num. Emp: {0}");

                        var objNoti = objVacaciones.lstAutorizantes.FirstOrDefault();

                        tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                        objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                        objNuevaAlerta.userRecibeID = objNoti.idUsuario;
#if DEBUG
                        //objNuevaAlerta.userRecibeID = 7939; // OMAR NUÑEZ
#endif
                        objNuevaAlerta.tipoAlerta = 2;
                        objNuevaAlerta.sistemaID = 16;
                        objNuevaAlerta.visto = false;
                        objNuevaAlerta.url = "/Administrativo/Vacaciones/Gestion";
                        objNuevaAlerta.objID = objCEVacaciones.id;
                        objNuevaAlerta.obj = "AutorizacionVacaciones";
                        objNuevaAlerta.msj = string.Format(txtAlerta, objCEVacaciones.claveEmpleado);
                        objNuevaAlerta.documentoID = 0;
                        objNuevaAlerta.moduloID = 0;
                        _context.tblP_Alerta.Add(objNuevaAlerta);
                        _context.SaveChanges();
                        #endregion //ALERTA SIGPLAN

                        #endregion
                    }

                    dbContextTransaction.Commit();
                    result.Add(SUCCESS, true);
                    result.Add(ITEMS, objCEVacaciones);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "CrearEditarVacaciones", e, AccionEnum.CONSULTA, 0, 0);
                    result.Add(MESSAGE, e.Message);
                    result.Add(SUCCESS, false);
                }
            }
            return result;
        }

        public Dictionary<string, object> EliminarVacacion(int id)
        {
            result = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var objDelVacacion = _context.tblRH_Vacaciones_Vacaciones.Where(e => e.id == id).FirstOrDefault();
                    var lstPeriodos = _context.tblRH_EK_Periodos.Where(e => e.year == DateTime.Now.Year).ToList();
                    int numClaveEmpleado = Convert.ToInt32(objDelVacacion.claveEmpleado);
                    var objEmpleado = _context.tblRH_EK_Empleados.FirstOrDefault(e => e.clave_empleado == numClaveEmpleado && e.esActivo);

                    #region ELIMINAR FECHAS
                    var fechasExistentes = _context.tblRH_Vacaciones_Fechas.Where(e => e.registroActivo && e.vacacionID == id).OrderBy(e => e.fecha.Value).ToList();

                    if (fechasExistentes.Count() > 0)
                    {

                        #region ELIMINAR INCIDENCIAS
                        var lstIncidencias = _context.tblRH_BN_Incidencia.Where(e => 
                            e.cc == objDelVacacion.cc && e.estatus == "P" && e.tipo_nomina == objEmpleado.tipo_nomina && e.anio == DateTime.Now.Year).ToList();
                        var lstIdsIncidecias = lstIncidencias.Select(e => e.id).ToList();

                        var lstIncidenciaDet = _context.tblRH_BN_Incidencia_det.Where(e => lstIdsIncidecias.Contains(e.incidenciaID) && e.clave_empleado == numClaveEmpleado).ToList();

                        foreach (var incidencia in lstIncidencias)
	                    {
                            var objPeriodo = lstPeriodos.FirstOrDefault(e => e.periodo == incidencia.periodo);

                            foreach (var item in fechasExistentes)
                            {

                                if (item.fecha.Value.Date >= objPeriodo.fecha_inicial.Date && item.fecha.Value.Date <= objPeriodo.fecha_final.Date)
                                {
                                    var objIncidenciaDet = lstIncidenciaDet.FirstOrDefault(e => e.incidenciaID == incidencia.id && e.clave_empleado == objEmpleado.clave_empleado);

                                    TimeSpan difFechasVacaciones = (item.fecha ?? DateTime.Today) - objPeriodo.fecha_inicial;
                                    int diaVacaciones = difFechasVacaciones.Days + 1;

                                    switch (diaVacaciones)
                                    {
                                        case 1:
                                            objIncidenciaDet.dia1 = 0;
                                            break;
                                        case 2:
                                            objIncidenciaDet.dia2 = 0;
                                            break;
                                        case 3:
                                            objIncidenciaDet.dia3 = 0;
                                            break;
                                        case 4:
                                            objIncidenciaDet.dia4 = 0;
                                            break;
                                        case 5:
                                            objIncidenciaDet.dia5 = 0;
                                            break;
                                        case 6:
                                            objIncidenciaDet.dia6 = 0;
                                            break;
                                        case 7:
                                            objIncidenciaDet.dia7 = 0;
                                            break;
                                        case 8:
                                            objIncidenciaDet.dia8 = 0;
                                            break;
                                        case 9:
                                            objIncidenciaDet.dia9 = 0;
                                            break;
                                        case 10:
                                            objIncidenciaDet.dia10 = 0;
                                            break;
                                        case 11:
                                            objIncidenciaDet.dia11 = 0;
                                            break;
                                        case 12:
                                            objIncidenciaDet.dia12 = 0;
                                            break;
                                        case 13:
                                            objIncidenciaDet.dia13 = 0;
                                            break;
                                        case 14:
                                            objIncidenciaDet.dia14 = 0;
                                            break;
                                        case 15:
                                            objIncidenciaDet.dia15 = 0;
                                            break;
                                        case 16:
                                            objIncidenciaDet.dia16 = 0;
                                            break;
                                        default:
                                            break;
                                    }

                                    _context.SaveChanges();

                                }
                            }
	                    }

                        #endregion

                        foreach (var item in fechasExistentes)
                        {
                            item.registroActivo = false;
                            item.fechaModificacion = DateTime.Now;
                            item.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                            _context.SaveChanges();
                        }

                       
                    }

                    #endregion

                    #region ELIMINAR SALDO PAGADAS

                    var objSaldo = _context.tblRH_Vacaciones_Saldos.FirstOrDefault(e => e.registroActivo && e.idVacacionesPagadas == id);

                    if (objSaldo != null)
                    {
                        objSaldo.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                        objSaldo.fechaModificacion = DateTime.Now;
                        objSaldo.registroActivo = false;
                    }

                    #endregion

                    #region ALERTA
                    var alertasVacaciones = _context.tblP_Alerta.Where(e => e.sistemaID == 16 && e.objID == id && e.obj == "AutorizacionVacaciones").ToList();

                    foreach (var item in alertasVacaciones)
                    {
                        item.visto = true;
                        _context.SaveChanges();
                    }
                    
                    #endregion

                    if (objDelVacacion == null)
                        throw new Exception("Ocurrio algo mal");

                    objDelVacacion.registroActivo = false;
                    objDelVacacion.fechaModificacion = DateTime.Now;
                    objDelVacacion.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    result.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "EliminarVacacion", e, AccionEnum.CONSULTA, 0, 0);
                    result.Add(MESSAGE, e.Message);
                    result.Add(SUCCESS, false);
                }
            }
            return result;
        }

        public Dictionary<string, object> GuardarArchivoActa(int vacacion_id, HttpPostedFileBase archivoActa)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var RutaBase = "";

#if DEBUG
                    RutaBase = @"C:\Proyecto\SIGOPLAN\CAPITAL_HUMANO\VACACIONES\";
#else
                    RutaBase = @"\\REPOSITORIO\Proyecto\SIGOPLAN\CAPITAL_HUMANO\VACACIONES\";
#endif

                    string rutaArchivoActa = Path.Combine(RutaBase, ObtenerFormatoNombreArchivo("", archivoActa.FileName));

                    var registroVacaciones = _context.tblRH_Vacaciones_Vacaciones.FirstOrDefault(x => x.id == vacacion_id);

                    if (registroVacaciones == null)
                    {
                        throw new Exception("No se encuentra la información del registro de vacaciones.");
                    }

                    registroVacaciones.rutaArchivoActa = rutaArchivoActa;
                    _context.SaveChanges();

                    if (GlobalUtils.SaveHTTPPostedFile(archivoActa, rutaArchivoActa) == false)
                    {
                        dbContextTransaction.Rollback();
                        resultado.Clear();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                        return resultado;
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "GuardarArchivoActa", e, AccionEnum.AGREGAR, 0, vacacion_id);
                }
            }

            return resultado;
        }

        public Tuple<Stream, string> DescargarArchivoActa(int id)
        {
            try
            {
                var registroVacaciones = _context.tblRH_Vacaciones_Vacaciones.FirstOrDefault(x => x.id == id);

                var fileStream = GlobalUtils.GetFileAsStream(registroVacaciones.rutaArchivoActa);
                string name = Path.GetFileName(registroVacaciones.rutaArchivoActa);

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        #endregion

        #region VACACIONES GESTION
        public Dictionary<string, object> AutorizarVacacion(int id, int estado, string msg)
        {
            result = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var lstAuth = _context.tblRH_Vacaciones_Gestion.Where(e => e.registroActivo && e.idVacaciones == id).ToList();
                    var vacacionObj = _context.tblRH_Vacaciones_Vacaciones.FirstOrDefault(e => e.id == id);
                    var lstUsuarios = _context.tblP_Usuario.Where(e => e.estatus).ToList();

                    if (vacacionObj == null)
                        throw new Exception("Ocurrio algo mal");

                    var lstCorreosNotificarRestantes = new List<string>();
                    var lstCorreosNotificarTodos = new List<string>();
                    string cuerpo = "";

                    #region AUTORIZAR/RECHAZAR

                    int totalAuth = 0;
                    bool notifyNextAuth = false;
                    int totalAlertas = 0;

                    foreach (var item in lstAuth)
                    {
                        tblP_Usuario objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.id == item.idUsuario);
                        lstCorreosNotificarTodos.Add(objUsuario.correo);

                        #region AGREGAR ALERTA PARA EL SIGUIENTE AUTORIZANTE
                        if (notifyNextAuth && estado == (int)GestionEstatusEnum.AUTORIZADO && totalAlertas == 0)
                        {

                            string txtAlerta = (vacacionObj.tipoVacaciones == 7 ? "Vacaciones Num. Emp: {0}" : "Control de Ausencias Num. Emp: {0}");

                            #region Alerta SIGOPLAN
                            tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                            objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                            objNuevaAlerta.userRecibeID = item.idUsuario;
#if DEBUG
                            //objNuevaAlerta.userRecibeID = 7939;
#endif
                            objNuevaAlerta.tipoAlerta = 2;
                            objNuevaAlerta.sistemaID = 16;
                            objNuevaAlerta.visto = false;
                            objNuevaAlerta.url = "/Administrativo/Vacaciones/Gestion";
                            objNuevaAlerta.objID = id;
                            objNuevaAlerta.obj = "AutorizacionVacaciones";
                            objNuevaAlerta.msj = string.Format(txtAlerta, vacacionObj.claveEmpleado);
                            objNuevaAlerta.documentoID = 0;
                            objNuevaAlerta.moduloID = 0;
                            _context.tblP_Alerta.Add(objNuevaAlerta);
                            _context.SaveChanges();
                            #endregion //ALERTA SIGPLAN

                            //SIGUIENTE EN SER AUTORIZADO
                            lstCorreosNotificarRestantes.Add(objUsuario.correo);

                            //NOTIFICADA
                            notifyNextAuth = false;
                            totalAlertas++;
                        }

                        #endregion

                        if (item.estatus == GestionEstatusEnum.AUTORIZADO)
                            totalAuth++;
                        else
                        {
                            if (item.idUsuario == vSesiones.sesionUsuarioDTO.id)
                            {
                                if (estado == (int)GestionEstatusEnum.AUTORIZADO)
                                {
                                    totalAuth++;
                                    notifyNextAuth = true;
                                    tblP_Alerta objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == item.idUsuario && e.visto == false && e.objID == id && e.obj == "AutorizacionVacaciones");
                                    if (objAlerta != null)
                                    {
                                        //AGREGAR FIRMA ELECT
                                        item.firmaElect = GlobalUtils.CrearFirmaDigital(vacacionObj.id, DocumentosEnum.FirmaGestionVacaciones, Convert.ToInt32(item.idUsuario));

                                        objAlerta.visto = true;
                                        _context.SaveChanges();
                                    }
                                }
                                else
                                {
                                    tblP_Alerta objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == item.idUsuario && e.visto == false && e.objID == id && e.obj == "AutorizacionVacaciones");
                                    if (objAlerta != null)
                                    {
                                        objAlerta.visto = true;
                                        _context.SaveChanges();
                                    }
                                }
                                item.estatus = (GestionEstatusEnum)estado;
                                item.fechaModificacion = DateTime.Now;
                                item.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;

                                _context.SaveChanges();
                            }
                        }
                    }

                    #endregion

                    #region CUERPO CORREO

                    #region DOCUMENTO


                    ReportDocument rptCV = new rptFechasVacaciones();

                    //string path = Path.Combine(RutaServidor, "rptFechasVacaciones.rpt");
                    //rptCV.Load(path);

                    var meses = new List<string>() { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

                    var lstStrDias = new List<object>();

                    var objVacaciones = GetVacacionesById(new VacacionesDTO() { id = vacacionObj.id })["items"] as VacacionesDTO;

                    //LIMPIAR RESULTADO
                    resultado.Clear();
                    result.Clear();

                    //var objResponsable = vacacionesService.GetResponsables(null, Convert.ToInt32(objVacaciones.claveEmpleado))["data"] as List<Core.DTO.RecursosHumanos.Vacaciones.ResponsableDTO>;

                    var dicSaldos = GetFechasByClaveEmpleado(Convert.ToInt32(objVacaciones.claveEmpleado), vacacionObj.tipoVacaciones);
                    var diasIniciales = dicSaldos["diasIniciales"] as int?;
                    var lstTotalDias = dicSaldos["items"] as List<tblRH_Vacaciones_Fechas>;
                    var lstSaldos = dicSaldos["lstSaldos"] as List<tblRH_Vacaciones_Saldos>;

                    //var lstDias = dicSaldos["items"] as List<tblRH_Vacaciones_Fechas>;
                    var objSaldoPagadas = lstSaldos.FirstOrDefault(e => e.idVacacionesPagadas == objVacaciones.id);

                    //LIMPIAR RESULTADO
                    resultado.Clear();
                    result.Clear();

                    DateTime? fechaIngreso = GetFechaIngreso(Convert.ToInt32(objVacaciones.claveEmpleado));

                    int años = (DateTime.Now - fechaIngreso.Value).Days / 365;

                    int añoVencer = fechaIngreso.Value.Year + años;

                    if (DateTime.Now.Day < fechaIngreso.Value.Day && DateTime.Now.Month == fechaIngreso.Value.Month)
                    {
                        añoVencer--;
                    }

                    DateTime nextDate = new DateTime(añoVencer, fechaIngreso.Value.Month, fechaIngreso.Value.Day);

                    var today = DateTime.Now;

                    string diasDisfrutar = "";
                    DateTime? currentDate = null;

                    var lstDias = _context.tblRH_Vacaciones_Fechas.Where(e => e.registroActivo && (id > 0 ? e.vacacionID == vacacionObj.id : true)).OrderBy(e => e.fecha.Value).ToList();

                    var saldoDias = (diasIniciales ?? 0) - lstTotalDias.Count();
                    saldoDias = (objVacaciones.numDiasDisponiblesAlDiaCaptura ?? saldoDias) + lstDias.Count();
                    //saldoDias = saldoDias + lstDias.Count();

                    var min = lstDias.Select(e => e.fecha).Min();
                    var max = lstDias.Select(e => e.fecha).Max();

                    string descMotivoVac = "";

                    switch (objVacaciones.tipoVacaciones)
                    {
                        case 0:
                            descMotivoVac = "Permiso paternidad (PGS)";
                            break;
                        case 1:
                            descMotivoVac = "Permiso de matrimonio (PGS)";
                            break;
                        case 2:
                            descMotivoVac = "Permiso sindical (PGS)";
                            break;
                        case 3:
                            descMotivoVac = "Permiso por fallecimiento (PGS)";
                            break;
                        case 5:
                            descMotivoVac = "Permiso médico (PGS)";
                            break;
                        case 7:
                            descMotivoVac = "Vacaciones (VAC)";
                            break;
                        case 8:
                            descMotivoVac = "Permiso SIN goce de sueldo (PS)";
                            break;
                        case 9:
                            descMotivoVac = "Permiso de comision de trabajo (CT)";
                            break;
                        case 10:
                            descMotivoVac = "Home office (PGS)";
                            break;
                        case 11:
                            descMotivoVac = "Tiempo x tiempo (PGS)";
                            break;
                        case 13:
                            descMotivoVac = "Suspención (SUSP)";
                            break;
                        default:
                            descMotivoVac = "S/N";
                            break;
                    }

                    var objResponsableCC = objVacaciones.lstAutorizantes.FirstOrDefault(e => e.orden == Core.Enum.RecursosHumanos.Vacaciones.OrdenGestionEnum.RESPONSABLE_CC);
                    var objRespPagadas1 = objVacaciones.lstAutorizantes.FirstOrDefault(e => e.orden == Core.Enum.RecursosHumanos.Vacaciones.OrdenGestionEnum.AUTORIZANTE_PAGADAS_1);
                    //var objJefeInmediato = objVacaciones.lstAutorizantes.FirstOrDefault(e => e.orden == Core.Enum.RecursosHumanos.Vacaciones.OrdenGestionEnum.JEFE_INMEDIATO);

                    var objStrVacaciones = new
                    {
                        nombreEmpleado = objVacaciones.nombreEmpleado,
                        claveEmpleado = objVacaciones.claveEmpleado,
                        ccEmpleado = objVacaciones.cc,
                        //periodoDesc = lstPeriodos.FirstOrDefault(e => e.id == objVacaciones.idPeriodo).periodoDesc,
                        fechaInicial = min != null ? min.Value.ToString("dd/MM/yyyy") : "",
                        fechaFinal = max != null ? max.Value.ToString("dd/MM/yyyy") : "",
                        nombreResponsable = objResponsableCC.nombreCompleto,
                        nombreResponsablePagadas = objRespPagadas1 != null ? objRespPagadas1.nombreCompleto : "  ",
                        //claveResponsable = objVacaciones.claveResponsable,
                        //claveResponsablePagadas = objVacaciones.claveResponsablePagadas ?? "",
                        folio = (objVacaciones.cc == null ? "" : objVacaciones.cc) + "-" + (objVacaciones.claveEmpleado == null ? "" : objVacaciones.claveEmpleado) + "-" + (objVacaciones.consecutivo == null ? "" : objVacaciones.consecutivo.ToString().PadLeft(3, '0'))

                    };

                    string strDias = "";

                    foreach (var item in lstDias)
                    {
                        #region NUEVA LISTA DIAS
                        strDias += "[" + item.fecha.Value.ToString("dd/MM/yyyy") + "] ";
                        #endregion

                        lstStrDias.Add(new
                        {

                            fecha = item.fecha.Value.ToString("dddd", new CultureInfo("es-ES")) + ", " + item.fecha.Value.Day + " de " + meses[item.fecha.Value.Month - 1] +
                            " de " + item.fecha.Value.Year,
                            //fecha = item.fecha.Value.ToString("dd/MM/yyyy"),
                            //periodo = objStrVacaciones.periodoDesc

                        });

                        if (currentDate != null)
                        {
                            if (currentDate.Value.Month == item.fecha.Value.Month)
                            {
                                diasDisfrutar += item.fecha.Value.Day.ToString() + ", ";

                            }
                            else
                            {

                                diasDisfrutar += "de " + currentDate.Value.ToString("MMMM", new CultureInfo("es-ES")) + " ";
                                if (currentDate.Value.Year != item.fecha.Value.Year)
                                {
                                    diasDisfrutar += "del " + currentDate.Value.Year + " ";

                                }
                                diasDisfrutar += item.fecha.Value.Day.ToString() + ", ";

                                if (item.fecha.Value == max)
                                {
                                    diasDisfrutar += "de " + item.fecha.Value.ToString("MMMM", new CultureInfo("es-ES")) + " ";
                                    diasDisfrutar += "del " + item.fecha.Value.Year + " ";

                                }
                            }
                        }
                        else
                        {
                            diasDisfrutar += item.fecha.Value.Day.ToString() + ", ";

                        }

                        currentDate = item.fecha.Value;

                    }

                    #region TOTAL DIAS
                    int diasDisponibles = 0;
                    string strDiasDisponibles = "";
                    bool esSaldoMuchos = false;

                    if (objVacaciones.tipoVacaciones == 7)
                    {
                        if (!objVacaciones.esPagadas)
                        {
                            if (min.Value.Month == max.Value.Month)
                            {
                                diasDisfrutar += "de " + currentDate.Value.ToString("MMMM", new CultureInfo("es-ES")) + " ";

                            }

                            diasDisponibles = saldoDias - lstDias.Count();
                            strDiasDisponibles = diasDisponibles.ToString();
                        }
                        else
                        {
                            strDias = (objSaldoPagadas.num_dias * -1) + " dias a Pagar";

                            diasDisponibles = saldoDias;
                            strDiasDisponibles = diasDisponibles.ToString();
                        }
                        
                    }
                    else
                    {
                        int? diasTotales = GetNumDiasPermisos(objVacaciones.tipoVacaciones.Value);

                        //true: Permisos con saldos finitos | false: permisos con saldos infinitos
                        if (objVacaciones.tipoVacaciones != 2 &&
                            objVacaciones.tipoVacaciones != 4 &&
                            objVacaciones.tipoVacaciones != 8 &&
                            objVacaciones.tipoVacaciones != 9 &&
                            objVacaciones.tipoVacaciones != 10 &&
                            objVacaciones.tipoVacaciones != 11)
                        {
                            diasDisponibles = diasTotales.Value - lstTotalDias.Count();
                            strDiasDisponibles = diasDisponibles.ToString();

                            saldoDias = diasTotales.Value;

                        }
                        else
                        {
                            esSaldoMuchos = true;
                            strDiasDisponibles = "MUCHOS";

                        }

                        //if (min.Value.Month == max.Value.Month)
                        //{
                        //   //COMENTARIOS PERMISO

                        //}
                    }
                    #endregion

                    rptCV.Database.Tables[0].SetDataSource(new[] { objStrVacaciones });
                    rptCV.Database.Tables[1].SetDataSource(getInfoEnca("reporte", ""));
                    rptCV.Database.Tables[2].SetDataSource(lstStrDias);

                    rptCV.SetParameterValue("todayDate", objVacaciones.fechaCreacion.Value.ToString("dd/MM/yyyy"));
                    rptCV.SetParameterValue("fechaIngreso", fechaIngreso.Value.ToString("dd/MM/yyyy"));
                    rptCV.SetParameterValue("diasDisponibles", (esSaldoMuchos ? strDiasDisponibles : saldoDias.ToString()));
                    rptCV.SetParameterValue("diasDisfrutar", diasDisfrutar);
                    rptCV.SetParameterValue("numDiasDisfrutar", (objVacaciones.esPagadas ? (objSaldoPagadas.num_dias * -1).ToString() : lstDias.Count().ToString()));
                    rptCV.SetParameterValue("descMotivo", descMotivoVac);
                    rptCV.SetParameterValue("justificacion", objVacaciones.justificacion ?? "  ");
                    rptCV.SetParameterValue("strDias", strDias);
                    rptCV.SetParameterValue("firmaElectJefeInmediato", " ");
                    rptCV.SetParameterValue("firmaElectResponsableCC", (objResponsableCC != null ? (objResponsableCC.firmaElect ?? " ") : " "));
                    rptCV.SetParameterValue("descPuesto", objVacaciones.descPuesto ?? " ");
                    rptCV.SetParameterValue("nombreJefeInmediato", objVacaciones.nombreJefeInmediato);
                    rptCV.SetParameterValue("nombreResponsableCC", objResponsableCC.nombreCompleto);
                    rptCV.SetParameterValue("ccDesc", objVacaciones.ccDesc.Trim());
                    rptCV.SetParameterValue("diasRestantes", (esSaldoMuchos ? strDiasDisponibles : diasDisponibles.ToString()));
                    rptCV.SetParameterValue("nombreCapturo", objVacaciones.nombreCapturo);
                    rptCV.SetParameterValue("estatusResponsable", (objResponsableCC.estatus == GestionEstatusEnum.AUTORIZADO ? "AUTORIZADO" : " "));
                    rptCV.SetParameterValue("tipoVacaciones", objVacaciones.tipoVacaciones);
                    rptCV.SetParameterValue("tituloReporte", (objVacaciones.tipoVacaciones == 7 ? "Control de vacaciones" : "Control de ausencias"));

                    Stream stream = rptCV.ExportToStream(ExportFormatType.PortableDocFormat);

                    #endregion

                    cuerpo =
                                @"<html>
                                            <head>
                                                <style>
                                                    table {
                                                        font-family: arial, sans-serif;
                                                        border-collapse: collapse;
                                                        width: 100%;
                                                    }

                                                    td, th {
                                                        border: 1px solid #dddddd;
                                                        text-align: left;
                                                        padding: 8px;
                                                    }

                                                    tr:nth-child(even) {
                                                        background-color: #ffcc5c;
                                                    }
                                                </style>
                                            </head>
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>

                                                    <p class=MsoNormal>
                                                        Buen día,<br><br>" +
                                                                           (
                                                                            totalAuth == lstAuth.Count() ? "Se han autorizado las ausencias por todos los firmantes" : (
                                                                                estado == (int)GestionEstatusEnum.RECHAZADO 
                                                                                ? ("Se han rechazado " + (objVacaciones.tipoVacaciones == 7 ? "las vacaciones" : "las ausencias") + "<br>Motivo: " + msg)
                                                                                : ("Se han autorizado " + (objVacaciones.tipoVacaciones == 7 ? "las vacaciones" : "las ausencias")))
                                                                           )
                                                        + @".<br><br>
                                                    </p>
                                                    <p class=MsoNormal style='font-weight:bold;'>" +
                                                        "Empleado: [" + vacacionObj.claveEmpleado + "] " + vacacionObj.nombreEmpleado + @".<o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal style='font-weight:bold;'>" +
                                                                "CC: " + objVacaciones.ccDesc.Trim() + @".<o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal style='font-weight:bold;'>" +
                                                                "Puesto: " + objVacaciones.descPuesto + @".<o:p></o:p>
                                                    </p>
                                                    <br><br><br>
                                            ";
                    #region TABLA AUTORIZANTES

                    cuerpo += @"
                                <table>
                                    <thead>
                                        <tr>
                                            <th>Nombre</th>
                                            <th>Tipo</th>
                                            <th>Autorizo</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    ";

                    bool esAuth = false;
                    int totalSiguientes = 0;
                    bool esRechazada = false;
                    //EstatusGestionAutorizacionEnum P 0 A 1 R 2 
                    foreach (var itemDet in lstAuth)
                    {
                        tblP_Usuario objUsuario = lstUsuarios.FirstOrDefault(e => e.id == itemDet.idUsuario);
                        string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                        cuerpo += "<tr>" +
                                    "<td>" + nombreCompleto + "</td>" +
                                    "<td>" + EnumHelper.GetDescription(itemDet.orden) + "</td>" +
                                    getEstatus((int)itemDet.estatus, esAuth, esRechazada) +
                                "</tr>";

                        if (estado == (int)GestionEstatusEnum.RECHAZADO)
                        {
                            esRechazada = true;
                        }

                        if (vSesiones.sesionUsuarioDTO.id == itemDet.idUsuario && totalSiguientes == 0)
                        {
                            esAuth = true;
                            totalSiguientes++;
                        }
                        else
                        {
                            if (esAuth)
                            {
                                esAuth = false;

                                if (itemDet.estatus == GestionEstatusEnum.AUTORIZADO)
                                {
                                    esAuth = true;
                                    totalSiguientes = 0;
                                }
                            }
                        }
                    }

                    cuerpo += "</tbody>" +
                                "</table>" +
                                "<br><br><br>";


                    #endregion

                    cuerpo += "<br><br><br>" +
                          "Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>" +
                          "Construplan > Capital Humano > Administración de Personal > Incidencias > Control de Ausencias > Gestion > Gestión de Ausencias.<br><br>" +
                          "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                          "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias." +
                        " </body>" +
                      "</html>";

                    #endregion

                    if (totalAuth == lstAuth.Count())
                    {
                        vacacionObj.estado = estado;
                        vacacionObj.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        vacacionObj.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();

                        var objEmpVac = _context.tblP_Usuario.FirstOrDefault(e => e.cveEmpleado == vacacionObj.claveEmpleado);

                        if (objEmpVac != null && !string.IsNullOrEmpty(objEmpVac.correo))
                        {
                            lstCorreosNotificarTodos.Add(objEmpVac.correo);
                        }

                        //lstCorreosNotificarTodos.Add("despacho@construplan.com.mx");
                        lstCorreosNotificarTodos.Add("serviciosalpersonal@construplan.com.mx");

                        //List<int> listaUsuariosCorreos = _context.tblRH_REC_Notificantes_Altas.Where(w => (w.cc == vacacionObj.cc || w.cc == "*") && w.esActivo).Select(s => s.idUsuario).ToList();

                        //foreach (var item in listaUsuariosCorreos)
                        //{
                        //    lstCorreosNotificarTodos.Add(_context.tblP_Usuario.FirstOrDefault(x => x.id == item).correo);
                        //}

                        var objUsuarioLogged = lstUsuarios.FirstOrDefault(e => e.id == vSesiones.sesionUsuarioDTO.id);

                        if (objUsuarioLogged != null)
                        {
                            lstCorreosNotificarTodos.Add(objUsuarioLogged.correo);
                            
                        }

                        var objJefeInmediato = lstUsuarios.FirstOrDefault(e => e.id == objVacaciones.idJefeInmediato);

                        if (objJefeInmediato != null)
                        {
                            lstCorreosNotificarTodos.Add(objJefeInmediato.correo);

                        }

#if DEBUG
                        lstCorreosNotificarTodos = new List<string>();
                        //lstCorreosNotificarTodos.Add("omar.nunez@construplan.com.mx");
                        lstCorreosNotificarTodos.Add("miguel.buzani@construplan.com.mx");
#endif

                        string descTipoVacaciones = "";

                        if (objVacaciones.tipoVacaciones == 7)
                        {
                            descTipoVacaciones = "VACACIONES AUTORIZADAS POR TODOS LOS FIRMANTES. CC {2} EMPLEADO: [{0}] {1}";
                        }
                        else
                        {
                            descTipoVacaciones = "AUSENCIAS AUTORIZADAS POR TODOS LOS FIRMANTES. CC {2} EMPLEADO: [{0}] {1}";
                        }


                        List<byte[]> downloadPDFs = new List<byte[]>();
                        using (var streamReader = new MemoryStream())
                        {
                            stream.CopyTo(streamReader);
                            downloadPDFs.Add(streamReader.ToArray());

                            GlobalUtils.sendEmailAdjuntoInMemory2(string.Format(PersonalUtilities.GetNombreEmpresa() + ": " + descTipoVacaciones, vacacionObj.claveEmpleado, vacacionObj.nombreEmpleado, vacacionObj.cc),
                                cuerpo, lstCorreosNotificarTodos, downloadPDFs, (objVacaciones.tipoVacaciones == 7 ? "Vacaciones.pdf" : "ControldeAusencias.pdf"));

                        }
                    }
                    else
                    {
                        if (estado == (int)GestionEstatusEnum.RECHAZADO)
                        {
                            vacacionObj.estado = estado;
                            vacacionObj.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                            vacacionObj.fechaModificacion = DateTime.Now;
                            vacacionObj.comentarioRechazada = msg ?? "";
                            _context.SaveChanges();

                            #region CORREO ESTATUS
                            lstCorreosNotificarRestantes = new List<string>();
                            lstCorreosNotificarRestantes.Add("serviciosalpersonal@construplan.com.mx");

                            var objUsuarioLogged = lstUsuarios.FirstOrDefault(e => e.id == vSesiones.sesionUsuarioDTO.id);

                            if (objUsuarioLogged != null)
                            {
                                lstCorreosNotificarRestantes.Add(objUsuarioLogged.correo);

                            }

                            var objJefeInmediato = lstUsuarios.FirstOrDefault(e => e.id == objVacaciones.idJefeInmediato);

                            if (objJefeInmediato != null)
                            {
                                lstCorreosNotificarRestantes.Add(objJefeInmediato.correo);

                            }

#if DEBUG
                            lstCorreosNotificarRestantes = new List<string>();
                            //lstCorreosNotificarRestantes.Add("omar.nunez@construplan.com.mx");
                            lstCorreosNotificarRestantes.Add("miguel.buzani@construplan.com.mx");
#endif
                            //GlobalUtils.sendEmail(string.Format("AUSENCIAS RECHAZADAS. EMPLEADO: [{0}] {1}", vacacionObj.claveEmpleado, vacacionObj.nombreEmpleado),
                            //                        cuerpo, lstCorreosNotificarRestantes);

                            string descTipoVacaciones = "";

                            if (objVacaciones.tipoVacaciones == 7)
                            {
                                descTipoVacaciones = "VACACIONES RECHAZADAS. CC {2} EMPLEADO: [{0}] {1}";
                            }
                            else
                            {
                                descTipoVacaciones = "AUSENCIAS RECHAZADAS. CC {2} EMPLEADO: [{0}] {1}";
                            }

                            List<byte[]> downloadPDFs = new List<byte[]>();
                            using (var streamReader = new MemoryStream())
                            {
                                stream.CopyTo(streamReader);
                                downloadPDFs.Add(streamReader.ToArray());

                                GlobalUtils.sendEmailAdjuntoInMemory2(string.Format(PersonalUtilities.GetNombreEmpresa() + ": " + descTipoVacaciones, vacacionObj.claveEmpleado, vacacionObj.nombreEmpleado, vacacionObj.cc),
                                    cuerpo, lstCorreosNotificarRestantes, downloadPDFs, (objVacaciones.tipoVacaciones == 7 ? "Vacaciones.pdf" : "ControldeAusencias.pdf"));

                            }
                            #endregion
                        }

                        if (estado == (int)GestionEstatusEnum.AUTORIZADO)
                        {
                            #region CORREO ESTATUS
                            lstCorreosNotificarRestantes.Add("serviciosalpersonal@construplan.com.mx");

                            var objJefeInmediato = lstUsuarios.FirstOrDefault(e => e.id == objVacaciones.idJefeInmediato);

                            if (objJefeInmediato != null)
                            {
                                lstCorreosNotificarRestantes.Add(objJefeInmediato.correo);

                            }

#if DEBUG
                            lstCorreosNotificarRestantes = new List<string>();
                            //lstCorreosNotificarRestantes.Add("omar.nunez@construplan.com.mx");
                            lstCorreosNotificarRestantes.Add("miguel.buzani@construplan.com.mx");
#endif
                            //GlobalUtils.sendEmail(string.Format("AUSENCIAS AUTORIZADAS. EMPLEADO: [{0}] {1}", vacacionObj.claveEmpleado, vacacionObj.nombreEmpleado),
                            //                        cuerpo, lstCorreosNotificarRestantes);

                            string descTipoVacaciones = "";

                            if (objVacaciones.tipoVacaciones == 7)
                            {
                                descTipoVacaciones = "VACACIONES AUTORIZADAS. EMPLEADO: CC {2} [{0}] {1}";
                            }
                            else
                            {
                                descTipoVacaciones = "AUSENCIAS AUTORIZADAS. EMPLEADO: CC {2} [{0}] {1}";
                            }

                            List<byte[]> downloadPDFs = new List<byte[]>();
                            using (var streamReader = new MemoryStream())
                            {
                                stream.CopyTo(streamReader);
                                downloadPDFs.Add(streamReader.ToArray());

                                GlobalUtils.sendEmailAdjuntoInMemory2(string.Format(PersonalUtilities.GetNombreEmpresa() + ": " + descTipoVacaciones, vacacionObj.claveEmpleado, vacacionObj.nombreEmpleado, vacacionObj.cc),
                                    cuerpo, lstCorreosNotificarRestantes, downloadPDFs, (objVacaciones.tipoVacaciones == 7 ? "Vacaciones.pdf" : "ControldeAusencias.pdf"));

                            }
                            #endregion
                        }

                    }

                    dbContextTransaction.Commit();
                    result.Add(SUCCESS, true);

                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "AutorizarVacacion", e, AccionEnum.CONSULTA, 0, 0);
                    result.Add(MESSAGE, e.Message);
                    result.Add(SUCCESS, false);
                }
            }

            return result;
        }
        public Dictionary<string, object> GetVacacionesGestion(VacacionesDTO objFiltro)
        {
            result = new Dictionary<string, object>();
            try
            {
                var lstCC = _context.tblP_CC.ToList();
                var lstPermisoCC = _context.tblRH_BN_Usuario_CC.Where(e => e.usuarioID == vSesiones.sesionUsuarioDTO.id).Select(e => e.cc).ToList();

                List<VacacionesDTO> lstVacaciones = _context.tblRH_Vacaciones_Vacaciones.Where(e => e.registroActivo
                    && (objFiltro.estado != 0 ? e.estado == objFiltro.estado : true)
                    && (!string.IsNullOrEmpty(objFiltro.cc) ? e.cc == objFiltro.cc : (lstPermisoCC.Contains("*") || vSesiones.sesionUsuarioDTO.idPerfil == 1) ? true : 
                            lstPermisoCC.Contains(e.cc))
                    ).Select(e => new VacacionesDTO
                    {
                        id = e.id,
                        estado = e.estado,
                        nombreEmpleado = e.nombreEmpleado,
                        claveEmpleado = e.claveEmpleado,
                        comentarioRechazada = e.comentarioRechazada ?? "",
                        tipoVacaciones = e.tipoVacaciones,
                        cc = e.cc,
                        rutaArchivoActa = e.rutaArchivoActa,
                        consecutivo = e.consecutivo,
                    }).OrderBy(e => e.nombreEmpleado).ToList();

                List<VacacionesDTO> lstFiltrada = new List<VacacionesDTO>();

                foreach (var item in lstVacaciones)
                {
                    var lstAuth = _context.tblRH_Vacaciones_Gestion.Where(e => e.registroActivo && e.idVacaciones == item.id).ToList();
                    var lstIdsAuth = lstAuth.Select(e => e.idUsuario).ToList();;

                    //true: Estas en la lista de firmantes, eres admin, eres diana alvarez o keyla vasquez
                    if (lstIdsAuth.Contains(vSesiones.sesionUsuarioDTO.id) || vSesiones.sesionUsuarioDTO.idPerfil == 1 || vSesiones.sesionUsuarioDTO.id == 1019 || vSesiones.sesionUsuarioDTO.id == 79552)
                    {
                        var objCC = lstCC.FirstOrDefault(e => e.cc == item.cc);

                        if (objCC != null)
                        {
                            item.ccDesc = "[" + objCC.cc + "] " + objCC.descripcion;
                        }
                        else
                        {
                            item.ccDesc = "S/N";
                        }

                        #region LISTA AUTH
                        var lstAuthDTO = new List<VacacionesGestionDTO>();

                        int? sigAuth = null;
                        foreach (var itemAuth in lstAuth)
                        {
                            if (itemAuth.estatus == GestionEstatusEnum.AUTORIZADO || itemAuth.estatus == GestionEstatusEnum.RECHAZADO)
                                item.esFirmar = false;
                            else
                            {
                                if (sigAuth == null)
                                    sigAuth = itemAuth.idUsuario;

                                if (sigAuth.Value == vSesiones.sesionUsuarioDTO.id)
                                    item.esFirmar = true;
                                else
                                    item.esFirmar = false;
                            }

                            var objAuth = new VacacionesGestionDTO();

                            var objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.id == itemAuth.idUsuario);
                            string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                            objAuth.id = itemAuth.id;
                            objAuth.idUsuario = itemAuth.idUsuario;
                            objAuth.estatus = itemAuth.estatus;
                            objAuth.orden = itemAuth.orden;
                            objAuth.nombreCompleto = nombreCompleto;

                            lstAuthDTO.Add(objAuth);
                        }

                        item.lstAutorizantes = lstAuthDTO;

                        lstFiltrada.Add(item);
                        #endregion
                    }
                    
                }

                result = new Dictionary<string, object>();
                result.Add(SUCCESS, true);
                result.Add(ITEMS, lstFiltrada);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }
        #endregion

        #region CRUD FECHAS

        public Dictionary<string, object> GetFechas(int idReg, int tipoPermiso, int? clave_empleado, bool esGestion = false)
        {
            result = new Dictionary<string, object>();
            bool capturada = false;
            try
            {

                var objVacacion = _context.tblRH_Vacaciones_Vacaciones.FirstOrDefault(e => e.id == idReg && e.registroActivo && e.tipoVacaciones == tipoPermiso);

                int? diasIniciales = 0;
                int saldoAdicional = 0;
                int? saldoOriginal = 0;

                if (tipoPermiso == 7)
                {
                    #region VACACIONES
                    
                    if (objVacacion == null)
                    {

                        var lstFechas = new List<tblRH_Vacaciones_Fechas>() { };

                        if (clave_empleado != null)
                        {
                            var lstVacacionesTotales = _context.tblRH_Vacaciones_Vacaciones.Where(e => e.registroActivo && e.claveEmpleado == clave_empleado.Value.ToString() && e.tipoVacaciones == 7 && (e.estado == 1 || e.estado == 3)).ToList();
                            var lstIdsVacacionesTotales = lstVacacionesTotales.Select(e => e.id).ToList();

                            var lstFechasTotales = _context.tblRH_Vacaciones_Fechas.Where(e => e.registroActivo && lstIdsVacacionesTotales.Contains(e.vacacionID)).OrderBy(e => e.fecha.Value).ToList();

                            diasIniciales = GetNumDias(clave_empleado.Value.ToString());
                            saldoAdicional = 0;
                            saldoOriginal = diasIniciales;

                            DateTime? fechaIngreso = GetFechaIngreso(clave_empleado.Value);
                            int años = (DateTime.Now - fechaIngreso.Value).Days / 365;

                            int añoVencer = fechaIngreso.Value.Year + años;

                            if (DateTime.Now.Day < fechaIngreso.Value.Day && DateTime.Now.Month == fechaIngreso.Value.Month)
                            {
                                añoVencer--;
                            }

                            DateTime nextDate = new DateTime(añoVencer, fechaIngreso.Value.Month, fechaIngreso.Value.Day);
                            result.Add("fechaAniversario", nextDate);

                            if (esGestion) lstFechas = lstFechasTotales.ToList();
                            else lstFechas = lstFechasTotales.Where(e => nextDate <= e.fecha.Value).ToList();

                            var lstSaldos = _context.tblRH_Vacaciones_Saldos.Where(e => e.registroActivo && e.clave_empleado == clave_empleado.Value).ToList().Where(e => nextDate.Date < e.fechaCreacion.Date).ToList();

                            if (lstSaldos != null && lstSaldos.Count() > 0)
                            {
                                saldoAdicional = lstSaldos.Sum(e => e.num_dias);
                            }

                            diasIniciales += saldoAdicional;
                        }

                        result.Add("capturada", capturada);
                        result.Add("diasIniciales", diasIniciales);
                        result.Add("saldoAdicional", saldoAdicional);
                        result.Add("saldoOriginal", saldoOriginal);
                        result.Add(ITEMS, lstFechas);
                        result.Add(SUCCESS, true);
                    }
                    else
                    {
                        var lstVacacionesTotales = _context.tblRH_Vacaciones_Vacaciones.Where(e => e.registroActivo && e.claveEmpleado == objVacacion.claveEmpleado && e.tipoVacaciones == 7 && (e.estado == 1 || e.estado == 3)).ToList();
                        var lstIdsVacacionesTotales = lstVacacionesTotales.Select(e => e.id).ToList();

                        diasIniciales = GetNumDias(objVacacion.claveEmpleado);
                        saldoOriginal = diasIniciales;

                        int numClaveEmpeado = Convert.ToInt32(objVacacion.claveEmpleado);

                        DateTime? fechaIngreso = GetFechaIngreso(numClaveEmpeado);
                        int años = (DateTime.Now - fechaIngreso.Value).Days / 365;

                        int añoVencer = fechaIngreso.Value.Year + años;

                        if (DateTime.Now.Day < fechaIngreso.Value.Day && DateTime.Now.Month == fechaIngreso.Value.Month)
                        {
                            añoVencer--;
                        }

                        DateTime nextDate = new DateTime(añoVencer, fechaIngreso.Value.Month, fechaIngreso.Value.Day);
                        var lstFechasTotales = _context.tblRH_Vacaciones_Fechas.Where(e => e.registroActivo && (idReg > 0 ? (lstIdsVacacionesTotales.Contains(e.vacacionID)) : true)).OrderBy(e => e.fecha.Value).ToList();
                        var lstFechas = _context.tblRH_Vacaciones_Fechas.Where(e => e.registroActivo && (idReg > 0 ? e.vacacionID == objVacacion.id : true)).OrderBy(e => e.fecha.Value).ToList();

                        List<tblRH_Vacaciones_Fechas> lstFechasFiltroTotales = new List<tblRH_Vacaciones_Fechas>();
                        List<tblRH_Vacaciones_Fechas> lstFechasFiltroTotalesSinActuales = new List<tblRH_Vacaciones_Fechas>();
                        List<tblRH_Vacaciones_Fechas> lstFechasFiltro = new List<tblRH_Vacaciones_Fechas>();

                        var objPrimeraFecha = lstFechas.FirstOrDefault();

                        if (esGestion || (objPrimeraFecha != null && objPrimeraFecha.fecha < nextDate))
                        {
                            lstFechasFiltroTotales = lstFechasTotales;
                            lstFechasFiltro = lstFechas;
                        }
                        else { 
                            lstFechasFiltroTotales = lstFechasTotales.Where(e => nextDate <= e.fecha.Value).ToList();
                            lstFechasFiltro = lstFechas.Where(e => nextDate < e.fecha.Value).ToList();
                        }
                        
                        lstFechasFiltroTotalesSinActuales = lstFechasFiltroTotales.Except(lstFechasFiltro).ToList();

                        var lstSaldos = _context.tblRH_Vacaciones_Saldos.Where(e => e.registroActivo && e.clave_empleado.ToString() == objVacacion.claveEmpleado).ToList().Where(e => nextDate.Date < e.fechaCreacion.Date).ToList();

                        if (lstSaldos != null && lstSaldos.Count() > 0)
                        {
                            saldoAdicional = lstSaldos.Sum(e => e.num_dias);
                        }

                        diasIniciales += saldoAdicional;

                        result.Add("capturada", capturada);
                        result.Add("diasIniciales", diasIniciales);
                        result.Add("saldoAdicional", saldoAdicional);
                        result.Add("saldoOriginal", saldoOriginal);
                        result.Add("fechasTotales", lstFechasFiltroTotales);
                        result.Add("fechasTotalesSinActual", lstFechasFiltroTotalesSinActuales);
                        result.Add("fechaAniversario", nextDate);
                        result.Add(ITEMS, lstFechasFiltro);
                        result.Add(SUCCESS, true);
                    }

                    #endregion

                }
                else
                {
                    #region PERMISOS

                    if (objVacacion == null)
                    {
                        //var lstFechas = new List<tblRH_Vacaciones_Fechas>() { };

                        var lstVacacionesTotales = _context.tblRH_Vacaciones_Vacaciones.Where(e => e.registroActivo && e.claveEmpleado == clave_empleado.Value.ToString() && e.tipoVacaciones == tipoPermiso && (e.estado == 1 || e.estado == 3)).ToList();
                        var lstIdsVacacionesTotales = lstVacacionesTotales.Select(e => e.id).ToList();

                        var lstFechasTotales = _context.tblRH_Vacaciones_Fechas.Where(e => e.registroActivo && lstIdsVacacionesTotales.Contains(e.vacacionID)).OrderBy(e => e.fecha.Value).ToList();

                        result.Add("capturada", capturada);
                        result.Add("diasIniciales", diasIniciales);
                        result.Add("saldoAdicional", saldoAdicional);
                        result.Add("saldoOriginal", saldoOriginal);
                        result.Add(ITEMS, lstFechasTotales);
                        result.Add(SUCCESS, true);
                    }
                    else
                    {
                        var lstFechas = _context.tblRH_Vacaciones_Fechas.Where(e => e.registroActivo && (idReg > 0 ? e.vacacionID == objVacacion.id : true)).OrderBy(e => e.fecha.Value).ToList();
                        var lstVacacionesTotales = _context.tblRH_Vacaciones_Vacaciones.Where(e => e.registroActivo && e.claveEmpleado == objVacacion.claveEmpleado && e.tipoVacaciones == objVacacion.tipoVacaciones && (e.estado == 1 || e.estado == 3)).ToList();
                        var lstIdsVacacionesTotales = lstVacacionesTotales.Select(e => e.id).ToList();
                        var lstFechasTotales = _context.tblRH_Vacaciones_Fechas.Where(e => e.registroActivo && (idReg > 0 ? (lstIdsVacacionesTotales.Contains(e.vacacionID)) : true)).OrderBy(e => e.fecha.Value).ToList();

                        List<tblRH_Vacaciones_Fechas> lstFechasFiltroTotalesSinActuales = new List<tblRH_Vacaciones_Fechas>();

                        lstFechasFiltroTotalesSinActuales = lstFechasTotales.Except(lstFechas).ToList();

                        result.Add("capturada", capturada);
                        result.Add("diasIniciales", diasIniciales);
                        result.Add("saldoAdicional", saldoAdicional);
                        result.Add("saldoOriginal", saldoOriginal);
                        result.Add("fechasTotalesSinActual", lstFechasFiltroTotalesSinActuales);
                        result.Add(ITEMS, lstFechas);
                        result.Add(SUCCESS, true);
                    }

                    #endregion
                
                }

            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetFechas", e, AccionEnum.CONSULTA, 0, 0);
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }

        public Dictionary<string, object> GetFechasByClaveEmpleado(int clave_empleado, int tipoPermiso, bool esGestion = false)
        {
            result = new Dictionary<string, object>();
            bool capturada = false;
            try
            {

                var objVacacion = _context.tblRH_Vacaciones_Vacaciones.Where(e => e.claveEmpleado == clave_empleado.ToString() && e.registroActivo && e.tipoVacaciones == tipoPermiso && (e.estado == 1 || e.estado == 3)).ToList();
                var lstIdsVacaciones = objVacacion.Select(e => e.id).ToList();

                var diasIniciales = GetNumDias(clave_empleado.ToString());
                int saldoAdicional = 0;
                int? saldoOriginal = diasIniciales;
                var lstSaldos = new List<tblRH_Vacaciones_Saldos>() { };

                if (tipoPermiso == 7)
                {
                    #region VACACIONES

                    if (objVacacion == null || objVacacion.Count == 0)
                    {
                        var lstFechas = new List<tblRH_Vacaciones_Fechas>() { };

                        // VACACIONES

                        DateTime? fechaIngreso = GetFechaIngreso(clave_empleado);
                        int años = (DateTime.Now - fechaIngreso.Value).Days / 365;

                        int añoVencer = fechaIngreso.Value.Year + años;

                        if (DateTime.Now.Day < fechaIngreso.Value.Day && DateTime.Now.Month == fechaIngreso.Value.Month)
                        {
                            añoVencer--;
                        }

                        DateTime nextDate = new DateTime(añoVencer, fechaIngreso.Value.Month, fechaIngreso.Value.Day);

                        lstSaldos = _context.tblRH_Vacaciones_Saldos.Where(e => e.registroActivo && e.clave_empleado == clave_empleado).ToList().Where(e => nextDate.Date <= e.fechaCreacion.Date).ToList();

                        if (lstSaldos != null && lstSaldos.Count() > 0)
                        {
                            saldoAdicional = lstSaldos.Sum(e => e.num_dias);
                        }

                        diasIniciales += saldoAdicional;

                        result.Add("capturada", capturada);
                        result.Add("diasIniciales", diasIniciales);
                        result.Add("saldoAdicional", saldoAdicional);
                        result.Add("saldoOriginal", saldoOriginal);
                        result.Add("lstIdsVacacionesEmp", null);
                        result.Add("fechaAniversario", nextDate);
                        result.Add("fechaOriginal", fechaIngreso);
                        result.Add("lstSaldos", lstSaldos);
                        result.Add(ITEMS, lstFechas);
                        result.Add(SUCCESS, true);
                    }
                    else
                    {
                        DateTime? fechaIngreso = GetFechaIngreso(clave_empleado);
                        int años = (DateTime.Now - fechaIngreso.Value).Days / 365;

                        int añoVencer = fechaIngreso.Value.Year + años;

                        if (DateTime.Now.Day < fechaIngreso.Value.Day && DateTime.Now.Month == fechaIngreso.Value.Month)
                        {
                            añoVencer--;
                        }

                        DateTime nextDate = new DateTime(añoVencer, fechaIngreso.Value.Month, fechaIngreso.Value.Day);
                        var lstFechas = _context.tblRH_Vacaciones_Fechas.Where(e => e.registroActivo && (clave_empleado > 0 ? (lstIdsVacaciones.Contains(e.vacacionID)) : true)).OrderBy(e => e.fecha.Value).ToList();

                        List<tblRH_Vacaciones_Fechas> lstFechasFiltro = new List<tblRH_Vacaciones_Fechas>();

                        if (esGestion && lstFechas.Count() > 0) { 
                            var fechaInicial = lstFechas[0].fecha ?? DateTime.Today;
                            diasIniciales = GetNumDias(clave_empleado.ToString(), fechaInicial);
                            int anioFecha = fechaInicial.Year;
                            DateTime _aniversario = new DateTime(fechaInicial.Year, (fechaIngreso ?? fechaInicial).Month, (fechaIngreso ?? fechaInicial).Day);
                            if (fechaInicial < _aniversario) _aniversario = new DateTime(fechaInicial.Year - 1, (fechaIngreso ?? fechaInicial).Month, (fechaIngreso ?? fechaInicial).Day);
                            nextDate = _aniversario;
                        }

                        lstSaldos = _context.tblRH_Vacaciones_Saldos.Where(e => e.registroActivo && e.clave_empleado == clave_empleado).ToList().Where(e => nextDate.Date < e.fechaCreacion.Date).ToList();

                        var objPrimeraFecha = lstFechas.FirstOrDefault();

                        if (esGestion || (objPrimeraFecha != null && objPrimeraFecha.fecha < nextDate)) lstFechasFiltro = lstFechas;
                        else lstFechasFiltro = lstFechas.Where(e => nextDate <= e.fecha.Value).ToList();

                        if (lstSaldos != null && lstSaldos.Count() > 0)
                        {
                            saldoAdicional = lstSaldos.Sum(e => e.num_dias);
                        }

                        diasIniciales += saldoAdicional;

                        result.Add("capturada", capturada);
                        result.Add("diasIniciales", diasIniciales);
                        result.Add("saldoAdicional", saldoAdicional);
                        result.Add("saldoOriginal", saldoOriginal);
                        result.Add("lstIdsVacacionesEmp", lstIdsVacaciones);
                        result.Add("fechaAniversario", nextDate);
                        result.Add("fechaOriginal", fechaIngreso);
                        result.Add("lstSaldos", lstSaldos);
                        result.Add(ITEMS, lstFechasFiltro);
                        result.Add(SUCCESS, true);
                    }
                    #endregion

                }
                else
                {
                    //PERMISOS

                    if (objVacacion == null || objVacacion.Count == 0)
                    {
                        var lstFechas = new List<tblRH_Vacaciones_Fechas>() { };

                        result.Add("capturada", capturada);
                        result.Add("diasIniciales", diasIniciales);
                        result.Add("saldoAdicional", saldoAdicional);
                        result.Add("saldoOriginal", saldoOriginal);
                        result.Add("lstSaldos", lstSaldos);
                        result.Add(ITEMS, lstFechas);
                        result.Add(SUCCESS, true);
                    }
                    else
                    {
                        var lstFechas = _context.tblRH_Vacaciones_Fechas.Where(e => e.registroActivo && (clave_empleado > 0 ? (lstIdsVacaciones.Contains(e.vacacionID)) : true)).OrderBy(e => e.fecha.Value).ToList();

                        result.Add("capturada", capturada);
                        result.Add("diasIniciales", diasIniciales);
                        result.Add("saldoAdicional", saldoAdicional);
                        result.Add("saldoOriginal", saldoOriginal);
                        result.Add("lstSaldos", lstSaldos);
                        result.Add(ITEMS, lstFechas);
                        result.Add(SUCCESS, true);
                    }


                }
                

            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetFechasByClaveEmpleado", e, AccionEnum.CONSULTA, 0, 0);
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }

        public Dictionary<string, object> CrearEditarFechas(int idVacacion, List<DateTime> lstFechas, int tipoPermiso, bool esSobreEscribir, bool esEditar,  int diasPermitidos = 0)
        {
            result = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var objVacacion = _context.tblRH_Vacaciones_Vacaciones.FirstOrDefault(e => e.id == idVacacion && e.registroActivo && (e.estado == 1 || e.estado == 3));

                    var objEmpleado = _context.tblRH_EK_Empleados.FirstOrDefault(e => e.esActivo && e.clave_empleado.ToString() == objVacacion.claveEmpleado);

                    var lstAusencias = _context.tblRH_Vacaciones_Vacaciones.Where(e => e.registroActivo && e.claveEmpleado == objVacacion.claveEmpleado && (e.estado == 1 || e.estado == 3)).ToList();
                    var lstIdsAusencias = lstAusencias.Select(e => e.id).ToList();
                    var fechasEmpleado = _context.tblRH_Vacaciones_Fechas.Where(e => e.registroActivo && lstIdsAusencias.Contains(e.vacacionID)).OrderBy(e => e.fecha.Value).ToList();
                    var fechasExistentes = fechasEmpleado.Where(e => e.registroActivo && e.vacacionID == idVacacion).ToList();
                    var lstUsuarios = _context.tblP_Usuario.Where(e => e.estatus).ToList();

                    if (objVacacion.tipoVacaciones == 4)
                    {
                        #region CREAR FECHAS POR MES
                        if (fechasExistentes.Count() == 0)
                        {
                            //foreach (var item in lstFechas)
                            //{

                            //    var objFecha = new tblRH_Vacaciones_Fechas();

                            //    objFecha.fecha = item;
                            //    objFecha.vacacionID = idVacacion;
                            //    objFecha.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                            //    objFecha.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                            //    objFecha.fechaCreacion = DateTime.Now;
                            //    objFecha.fechaModificacion = DateTime.Now;
                            //    objFecha.registroActivo = true;
                            //    _context.tblRH_Vacaciones_Fechas.Add(objFecha);
                            //    _context.SaveChanges();

                            //}

                            var objFecha1 = new tblRH_Vacaciones_Fechas()
                            {
                                fecha = lstFechas[0],
                                vacacionID = idVacacion,
                                tipoInsidencia = tipoPermiso,
                                idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id,
                                idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id,
                                fechaCreacion = DateTime.Now,
                                registroActivo = true,
                            };
                            var objFecha2 = new tblRH_Vacaciones_Fechas()
                            {
                                fecha = lstFechas[1],
                                vacacionID = idVacacion,
                                tipoInsidencia = tipoPermiso,
                                idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id,
                                idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id,
                                fechaCreacion = DateTime.Now,
                                registroActivo = true,
                            };
                            _context.tblRH_Vacaciones_Fechas.Add(objFecha1);
                            _context.tblRH_Vacaciones_Fechas.Add(objFecha2);

                            _context.SaveChanges();
                        }
                        else
                        {
                            fechasExistentes[0].fecha = lstFechas[0];
                            fechasExistentes[0].fechaModificacion = DateTime.Now;
                            fechasExistentes[0].idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;

                            fechasExistentes[1].fecha = lstFechas[1];
                            fechasExistentes[1].fechaModificacion = DateTime.Now;
                            fechasExistentes[1].idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;

                            _context.SaveChanges();

                        }
                        #endregion
                    }
                    else
                    {
                        if (objVacacion.tipoVacaciones == 8 || objVacacion.tipoVacaciones == 9)
                        {
                            diasPermitidos = 1;
                        }

                        if (objVacacion.numDiasDisponiblesAlDiaCaptura != null)
                        {
                            SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, objVacacion.id, JsonUtils.convertNetObjectToJson(objVacacion));
                        }

                        objVacacion.numDiasDisponiblesAlDiaCaptura = diasPermitidos;
                        _context.SaveChanges();

                        #region CREAR FECHAS POR DIA
                        if (diasPermitidos < 0)
                            throw new Exception("Numero de vacaciones fuera del numero disponible");

                        if (fechasExistentes.Count() == 0)
                        {

                            foreach (var item in lstFechas)
                            {
                                var objAusenciaExistente = fechasEmpleado.FirstOrDefault(e => e.fecha.Value.Date == item.Date);

                                if (objAusenciaExistente != null)
                                {
                                    if (objAusenciaExistente.tipoInsidencia != tipoPermiso)
                                    {
                                        if (objAusenciaExistente != null)
                                        {
                                            var objVacacionExistente = lstAusencias.FirstOrDefault(e => e.id == objAusenciaExistente.vacacionID);

                                            #region DESCRIPCION MOTIVO

                                            string descMotivo = "";
                                            string descTipoVacacionesAlerta = "Ausencias";

                                            switch (objVacacionExistente.tipoVacaciones)
                                            {
                                                case 0:
                                                    descMotivo = "Permiso paternidad (PGS)";
                                                    break;
                                                case 1:
                                                    descMotivo = "Permiso de matrimonio (PGS)";
                                                    break;
                                                case 2:
                                                    descMotivo = "Permiso sindical (PGS)";
                                                    break;
                                                case 3:
                                                    descMotivo = "Permiso por fallecimiento (PGS)";
                                                    break;
                                                case 5:
                                                    descMotivo = "Permiso médico (PGS)";
                                                    break;
                                                case 7:
                                                    descMotivo = "Vacaciones (VAC)";
                                                    descTipoVacacionesAlerta = "Vacaciones";
                                                    break;
                                                case 8:
                                                    descMotivo = "Permiso SIN goce de sueldo (PS)";
                                                    break;
                                                case 9:
                                                    descMotivo = "Permiso de comision de trabajo (CT)";
                                                    break;
                                                case 10:
                                                    descMotivo = "Home office (PGS)";
                                                    break;
                                                case 11:
                                                    descMotivo = "Tiempo x tiempo (PGS)";
                                                    break;
                                                case 13:
                                                    descMotivo = "Suspención (SUSP)";
                                                    break;
                                                default:
                                                    descMotivo = "S/N";
                                                    break;
                                            }
                                            #endregion

                                            if (objAusenciaExistente.esAplicadaIncidencias)
                                            {
                                                result.Add("esRecursar", false);
                                                throw new Exception(("Ya existen unas " + descTipoVacacionesAlerta + " el dia: " + item.ToString("dd/MM/yyyy") + " de tipo: " + descMotivo + " y estan aplicadas en incidencias, favor de verificar los dias"));
                                            }
                                            else
                                            {
                                                if (esSobreEscribir)
                                                {
                                                    //ELIMINAR AUSENCIA A SOBREESCRIBIR
                                                    objAusenciaExistente.registroActivo = false;
                                                    objAusenciaExistente.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                                    objAusenciaExistente.fechaModificacion = DateTime.Now;
                                                    _context.SaveChanges();

                                                    //SAVE BITACORA
                                                    SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, objAusenciaExistente.id, JsonUtils.convertNetObjectToJson(objAusenciaExistente));

                                                    #region ACTUALIZAR INCIDENCIAS

                                                    var per = _context.tblRH_EK_Periodos.Where(x => (x.year == objAusenciaExistente.fecha.Value.Year) && x.tipo_nomina == objEmpleado.tipo_nomina).ToList()
                                                        .FirstOrDefault(e => e.fecha_inicial.Date <= objAusenciaExistente.fecha.Value.Date && e.fecha_final.Date >= objAusenciaExistente.fecha.Value.Date);

                                                    var objIncidencia = _context.tblRH_BN_Incidencia.FirstOrDefault(e => e.anio == objAusenciaExistente.fecha.Value.Year && e.periodo == per.id && e.cc == objVacacion.cc && e.tipo_nomina == objEmpleado.tipo_nomina);

                                                    if (objIncidencia != null)
                                                    {
                                                        var objIncidenciaDet = _context.tblRH_BN_Incidencia_det.FirstOrDefault(e => e.clave_empleado.ToString() == objVacacion.claveEmpleado && e.incidenciaID == objIncidencia.id);

                                                        if (objIncidenciaDet != null)
                                                        {
                                                            TimeSpan difFechasVacaciones = (objAusenciaExistente.fecha ?? DateTime.Today) - per.fecha_inicial;
                                                            int diaVacaciones = difFechasVacaciones.Days + 1;

                                                            //SAVE BITACORA ANTES
                                                            SaveBitacora(0, (int)AccionEnum.CONSULTA, objIncidenciaDet.id, JsonUtils.convertNetObjectToJson(objIncidenciaDet));

                                                            if (diaVacaciones == 1) objIncidenciaDet.dia1 = objAusenciaExistente.tipoInsidencia;
                                                            if (diaVacaciones == 2) objIncidenciaDet.dia2 = objAusenciaExistente.tipoInsidencia;
                                                            if (diaVacaciones == 3) objIncidenciaDet.dia3 = objAusenciaExistente.tipoInsidencia;
                                                            if (diaVacaciones == 4) objIncidenciaDet.dia4 = objAusenciaExistente.tipoInsidencia;
                                                            if (diaVacaciones == 5) objIncidenciaDet.dia5 = objAusenciaExistente.tipoInsidencia;
                                                            if (diaVacaciones == 6) objIncidenciaDet.dia6 = objAusenciaExistente.tipoInsidencia;
                                                            if (diaVacaciones == 7) objIncidenciaDet.dia7 = objAusenciaExistente.tipoInsidencia;
                                                            if (diaVacaciones == 8) objIncidenciaDet.dia8 = objAusenciaExistente.tipoInsidencia;
                                                            if (diaVacaciones == 9) objIncidenciaDet.dia9 = objAusenciaExistente.tipoInsidencia;
                                                            if (diaVacaciones == 10) objIncidenciaDet.dia10 = objAusenciaExistente.tipoInsidencia;
                                                            if (diaVacaciones == 11) objIncidenciaDet.dia11 = objAusenciaExistente.tipoInsidencia;
                                                            if (diaVacaciones == 12) objIncidenciaDet.dia12 = objAusenciaExistente.tipoInsidencia;
                                                            if (diaVacaciones == 13) objIncidenciaDet.dia13 = objAusenciaExistente.tipoInsidencia;
                                                            if (diaVacaciones == 14) objIncidenciaDet.dia14 = objAusenciaExistente.tipoInsidencia;
                                                            if (diaVacaciones == 15) objIncidenciaDet.dia15 = objAusenciaExistente.tipoInsidencia;
                                                            if (diaVacaciones == 16) objIncidenciaDet.dia16 = objAusenciaExistente.tipoInsidencia;

                                                            _context.SaveChanges();

                                                            //SAVE BITACORA DESPUES
                                                            SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, objIncidenciaDet.id, JsonUtils.convertNetObjectToJson(objIncidenciaDet));
                                                        }
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    result.Add("esRecursar", true);
                                                    throw new Exception(("Ya existen unas " + descTipoVacacionesAlerta + " el dia: " + item.ToString("dd/MM/yyyy") + " de tipo: " + descMotivo + ", desea sobreescribir el tipo de ausencia y las incidencias?"));
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {

                                        throw new Exception(("Ya existen unas " + (objVacacion.tipoVacaciones == 7 ? "Vacaciones" : "Ausencias") + " favor de verificar las fechas capturadas"));
                                    }
                                }
                                else
                                {
                                    var objFecha = new tblRH_Vacaciones_Fechas();

                                    objFecha.fecha = item.Date;
                                    objFecha.vacacionID = idVacacion;
                                    objFecha.tipoInsidencia = tipoPermiso;
                                    objFecha.esAplicadaIncidencias = false;
                                    objFecha.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                                    objFecha.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                                    objFecha.fechaCreacion = DateTime.Now;
                                    objFecha.fechaModificacion = DateTime.Now;
                                    objFecha.registroActivo = true;
                                    _context.tblRH_Vacaciones_Fechas.Add(objFecha);
                                    _context.SaveChanges();
                                }
    
                            }

                            #region CORREO

                            #region CUERPO CORREO

                            #region DOCUMENTO

                            ReportDocument rptCV = new rptFechasVacaciones();

                            //string path = Path.Combine(RutaServidor, "rptFechasVacaciones.rpt");
                            //rptCV.Load(path);

                            var meses = new List<string>() { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

                            var lstStrDias = new List<object>();

                            var objVacaciones = GetVacacionesById(new VacacionesDTO() { id = objVacacion.id })["items"] as VacacionesDTO;

                            //LIMPIAR RESULTADO
                            resultado.Clear();
                            result.Clear();

                            //var objResponsable = vacacionesService.GetResponsables(null, Convert.ToInt32(objVacaciones.claveEmpleado))["data"] as List<Core.DTO.RecursosHumanos.Vacaciones.ResponsableDTO>;

                            var dicSaldos = GetFechasByClaveEmpleado(Convert.ToInt32(objVacaciones.claveEmpleado), objVacacion.tipoVacaciones);
                            var diasIniciales = dicSaldos["diasIniciales"] as int?;
                            var lstTotalDias = dicSaldos["items"] as List<tblRH_Vacaciones_Fechas>;
                            var lstSaldos = dicSaldos["lstSaldos"] as List<tblRH_Vacaciones_Saldos>;
                            //var lstDias = dicSaldos["items"] as List<tblRH_Vacaciones_Fechas>;
                            var objSaldoPagadas = lstSaldos.FirstOrDefault(e => e.idVacacionesPagadas == objVacaciones.id);

                            //LIMPIAR RESULTADO
                            resultado.Clear();
                            result.Clear();

                            DateTime? fechaIngreso = GetFechaIngreso(Convert.ToInt32(objVacaciones.claveEmpleado));

                            int años = (DateTime.Now - fechaIngreso.Value).Days / 365;

                            int añoVencer = fechaIngreso.Value.Year + años;

                            if (DateTime.Now.Day < fechaIngreso.Value.Day && DateTime.Now.Month == fechaIngreso.Value.Month)
                            {
                                añoVencer--;
                            }

                            DateTime nextDate = new DateTime(añoVencer, fechaIngreso.Value.Month, fechaIngreso.Value.Day);

                            var today = DateTime.Now;

                            string diasDisfrutar = "";
                            DateTime? currentDate = null;

                            var lstDias = _context.tblRH_Vacaciones_Fechas.Where(e => e.registroActivo && (idVacacion > 0 ? e.vacacionID == objVacacion.id : true)).OrderBy(e => e.fecha.Value).ToList();

                            var saldoDias = (diasIniciales ?? 0) - lstTotalDias.Count();
                            saldoDias = (objVacaciones.numDiasDisponiblesAlDiaCaptura ?? diasPermitidos) + lstDias.Count();

                            List<tblRH_Vacaciones_Fechas> lstFechasFiltro = new List<tblRH_Vacaciones_Fechas>();

                            var min = lstDias.Select(e => e.fecha).Min();
                            var max = lstDias.Select(e => e.fecha).Max();

                            string descMotivoVac = "";

                            switch (objVacaciones.tipoVacaciones)
                            {
                                case 0:
                                    descMotivoVac = "Permiso paternidad (PGS)";
                                    break;
                                case 1:
                                    descMotivoVac = "Permiso de matrimonio (PGS)";
                                    break;
                                case 2:
                                    descMotivoVac = "Permiso sindical (PGS)";
                                    break;
                                case 3:
                                    descMotivoVac = "Permiso por fallecimiento (PGS)";
                                    break;
                                case 5:
                                    descMotivoVac = "Permiso médico (PGS)";
                                    break;
                                case 7:
                                    descMotivoVac = "Vacaciones (VAC)";
                                    break;
                                case 8:
                                    descMotivoVac = "Permiso SIN goce de sueldo (PS)";
                                    break;
                                case 9:
                                    descMotivoVac = "Permiso de comision de trabajo (CT)";
                                    break;
                                case 10:
                                    descMotivoVac = "Home office (PGS)";
                                    break;
                                case 11:
                                    descMotivoVac = "Tiempo x tiempo (PGS)";
                                    break;
                                case 13:
                                    descMotivoVac = "Suspención (SUSP)";
                                    break;
                                default:
                                    descMotivoVac = "S/N";
                                    break;
                            }

                            var objResponsableCC = objVacaciones.lstAutorizantes.FirstOrDefault(e => e.orden == Core.Enum.RecursosHumanos.Vacaciones.OrdenGestionEnum.RESPONSABLE_CC);
                            var objRespPagadas1 = objVacaciones.lstAutorizantes.FirstOrDefault(e => e.orden == Core.Enum.RecursosHumanos.Vacaciones.OrdenGestionEnum.AUTORIZANTE_PAGADAS_1);
                            //var objJefeInmediato = objVacaciones.lstAutorizantes.FirstOrDefault(e => e.orden == Core.Enum.RecursosHumanos.Vacaciones.OrdenGestionEnum.JEFE_INMEDIATO);

                            var objStrVacaciones = new
                            {
                                nombreEmpleado = objVacaciones.nombreEmpleado,
                                claveEmpleado = objVacaciones.claveEmpleado,
                                ccEmpleado = objVacaciones.cc,
                                //periodoDesc = lstPeriodos.FirstOrDefault(e => e.id == objVacaciones.idPeriodo).periodoDesc,
                                fechaInicial = min != null ? min.Value.ToString("dd/MM/yyyy") : "",
                                fechaFinal = max != null ? max.Value.ToString("dd/MM/yyyy") : "",
                                nombreResponsable = objResponsableCC.nombreCompleto,
                                nombreResponsablePagadas = objRespPagadas1 != null ? objRespPagadas1.nombreCompleto : "  ",
                                //claveResponsable = objVacaciones.claveResponsable,
                                //claveResponsablePagadas = objVacaciones.claveResponsablePagadas ?? "",
                                folio = (objVacaciones.cc == null ? "" : objVacaciones.cc) + "-" + (objVacaciones.claveEmpleado == null ? "" : objVacaciones.claveEmpleado) + "-" + (objVacaciones.consecutivo == null ? "" : objVacaciones.consecutivo.ToString().PadLeft(3, '0'))
                            };

                            string strDias = "";

                            foreach (var item in lstDias)
                            {
                                #region NUEVA LISTA DIAS
                                strDias += "[" + item.fecha.Value.ToString("dd/MM/yyyy") + "] ";
                                #endregion

                                lstStrDias.Add(new
                                {

                                    fecha = item.fecha.Value.ToString("dddd", new CultureInfo("es-ES")) + ", " + item.fecha.Value.Day + " de " + meses[item.fecha.Value.Month - 1] +
                                    " de " + item.fecha.Value.Year,
                                    //fecha = item.fecha.Value.ToString("dd/MM/yyyy"),
                                    //periodo = objStrVacaciones.periodoDesc

                                });

                                if (currentDate != null)
                                {
                                    if (currentDate.Value.Month == item.fecha.Value.Month)
                                    {
                                        diasDisfrutar += item.fecha.Value.Day.ToString() + ", ";

                                    }
                                    else
                                    {

                                        diasDisfrutar += "de " + currentDate.Value.ToString("MMMM", new CultureInfo("es-ES")) + " ";
                                        if (currentDate.Value.Year != item.fecha.Value.Year)
                                        {
                                            diasDisfrutar += "del " + currentDate.Value.Year + " ";

                                        }
                                        diasDisfrutar += item.fecha.Value.Day.ToString() + ", ";

                                        if (item.fecha.Value == max)
                                        {
                                            diasDisfrutar += "de " + item.fecha.Value.ToString("MMMM", new CultureInfo("es-ES")) + " ";
                                            diasDisfrutar += "del " + item.fecha.Value.Year + " ";

                                        }
                                    }
                                }
                                else
                                {
                                    diasDisfrutar += item.fecha.Value.Day.ToString() + ", ";

                                }

                                currentDate = item.fecha.Value;

                            }

                            #region TOTAL DIASGetVacacionesById
                            int diasDisponibles = 0;
                            string strDiasDisponibles = "";
                            bool esSaldoMuchos = false;

                            if (objVacaciones.tipoVacaciones == 7)
                            {
                                if (!objVacaciones.esPagadas)
	                            {
                                    if (min.Value.Month == max.Value.Month)
                                    {
                                        diasDisfrutar += "de " + currentDate.Value.ToString("MMMM", new CultureInfo("es-ES")) + " ";

                                    }

                                    diasDisponibles = saldoDias - lstDias.Count();
                                    strDiasDisponibles = diasDisponibles.ToString();
	                            }
                                else
                                {
                                    strDias = (objSaldoPagadas.num_dias * -1) + " dias a Pagar";

                                    diasDisponibles = saldoDias;
                                    strDiasDisponibles = diasDisponibles.ToString();
                                }
                            }
                            else
                            {
                                int? diasTotales = GetNumDiasPermisos(objVacaciones.tipoVacaciones.Value);

                                //true: Permisos con saldos finitos | false: permisos con saldos infinitos
                                if (objVacaciones.tipoVacaciones != 2 &&
                                    objVacaciones.tipoVacaciones != 4 &&
                                    objVacaciones.tipoVacaciones != 8 &&
                                    objVacaciones.tipoVacaciones != 9 &&
                                    objVacaciones.tipoVacaciones != 10 &&
                                    objVacaciones.tipoVacaciones != 11)
                                {
                                    diasDisponibles = diasTotales.Value - lstTotalDias.Count();
                                    strDiasDisponibles = diasDisponibles.ToString();
                                }
                                else
                                {
                                    esSaldoMuchos = true;
                                    strDiasDisponibles = "MUCHOS";

                                }

                                //if (min.Value.Month == max.Value.Month)
                                //{
                                //   //COMENTARIOS PERMISO

                                //}
                            }
                            #endregion

                            rptCV.Database.Tables[0].SetDataSource(new[] { objStrVacaciones });
                            rptCV.Database.Tables[1].SetDataSource(getInfoEnca("reporte", ""));
                            rptCV.Database.Tables[2].SetDataSource(lstStrDias);

                            rptCV.SetParameterValue("todayDate", objVacaciones.fechaCreacion.Value.ToString("dd/MM/yyyy"));
                            rptCV.SetParameterValue("fechaIngreso", fechaIngreso.Value.ToString("dd/MM/yyyy"));
                            rptCV.SetParameterValue("diasDisponibles", (esSaldoMuchos ? strDiasDisponibles : saldoDias.ToString()));
                            rptCV.SetParameterValue("diasDisfrutar", diasDisfrutar);
                            rptCV.SetParameterValue("numDiasDisfrutar", (objVacaciones.esPagadas ? (objSaldoPagadas.num_dias * -1).ToString() : lstDias.Count().ToString()));
                            rptCV.SetParameterValue("descMotivo", descMotivoVac);
                            rptCV.SetParameterValue("justificacion", objVacaciones.justificacion ?? "  ");
                            rptCV.SetParameterValue("strDias", strDias);
                            rptCV.SetParameterValue("firmaElectJefeInmediato", (" "));
                            rptCV.SetParameterValue("firmaElectResponsableCC", (" "));
                            rptCV.SetParameterValue("descPuesto", objVacaciones.descPuesto ?? " ");
                            rptCV.SetParameterValue("nombreJefeInmediato", objVacaciones.nombreJefeInmediato);
                            rptCV.SetParameterValue("nombreResponsableCC", objResponsableCC.nombreCompleto);
                            rptCV.SetParameterValue("ccDesc", objVacaciones.ccDesc.Trim());
                            rptCV.SetParameterValue("diasRestantes", (esSaldoMuchos ? strDiasDisponibles : diasDisponibles.ToString()));
                            rptCV.SetParameterValue("nombreCapturo", objVacaciones.nombreCapturo);
                            rptCV.SetParameterValue("estatusResponsable", " ");
                            rptCV.SetParameterValue("tipoVacaciones", objVacaciones.tipoVacaciones);
                            rptCV.SetParameterValue("tituloReporte", (objVacaciones.tipoVacaciones == 7 ? "Control de vacaciones" : "Control de ausencias"));

                            Stream stream = rptCV.ExportToStream(ExportFormatType.PortableDocFormat);

                            #endregion

                            var lstCorreosNotificarTodos = new List<string>();

                            string cuerpo =
                                        @"<html>
                                            <head>
                                                <style>
                                                    table {
                                                        font-family: arial, sans-serif;
                                                        border-collapse: collapse;
                                                        width: 100%;
                                                    }

                                                    td, th {
                                                        border: 1px solid #dddddd;
                                                        text-align: left;
                                                        padding: 8px;
                                                    }

                                                    tr:nth-child(even) {
                                                        background-color: #ffcc5c;
                                                    }
                                                </style>
                                            </head>
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>

                                                    <p class=MsoNormal>
                                                        Buen día,<br><br>" +
                                                                "Se han capturado ausencias y estan listas para su gestion"
                                                                + @".<br><br>
                                                    </p>
                                                    <p class=MsoNormal style='font-weight:bold;'>" +
                                                                "Empleado: [" + objVacaciones.claveEmpleado + "] " + objVacaciones.nombreEmpleado + @".<o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal style='font-weight:bold;'>" +
                                                                "CC: " + objVacaciones.ccDesc.Trim() + @".<o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal style='font-weight:bold;'>" +
                                                                "Puesto: " + objVacaciones.descPuesto + @".<o:p></o:p>
                                                    </p>
                                                    <br><br><br>
                                            ";
                            #region TABLA AUTORIZANTES

                            cuerpo += @"
                                <table>
                                    <thead>
                                        <tr>
                                            <th>Nombre</th>
                                            <th>Tipo</th>
                                            <th>Autorizo</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    ";

                            bool esPrimero = true;
                            //EstatusGestionAutorizacionEnum P 0 A 1 R 2 
                            if (objVacaciones.lstAutorizantes != null)
                            {
                                foreach (var itemDet in objVacaciones.lstAutorizantes)
                                {
                                    tblP_Usuario objUsuario = lstUsuarios.FirstOrDefault(e => e.id == itemDet.idUsuario);
                                    string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                                    cuerpo += "<tr>" +
                                                "<td>" + nombreCompleto + "</td>" +
                                                "<td>" + EnumHelper.GetDescription(itemDet.orden) + "</td>" +
                                                getEstatus(0, esPrimero, false) +
                                            "</tr>";

                                    if (esPrimero)
                                    {
                                        lstCorreosNotificarTodos.Add(objUsuario.correo);
                                        esPrimero = false;
                                    }
                                }
                            }

                            cuerpo += "</tbody>" +
                                        "</table>" +
                                        "<br><br><br>";


                            #endregion

                            cuerpo += "<br><br><br>" +
                                  "Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>" +
                                  "Construplan > Capital Humano > Administración de Personal > Incidencias > Control de Ausencias > Gestion > Gestión de Ausencias.<br><br>" +
                                  "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                                  "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias." +
                                " </body>" +
                              "</html>";

                            #endregion

                            var objUsuarioLogged = lstUsuarios.FirstOrDefault(e => e.id == vSesiones.sesionUsuarioDTO.id);

                            if (objUsuarioLogged != null)
                            {
                                lstCorreosNotificarTodos.Add(objUsuarioLogged.correo);

                            }

                            var objJefeInmediato = lstUsuarios.FirstOrDefault(e => e.id == objVacaciones.idJefeInmediato);

                            if (objJefeInmediato != null)
                            {
                                lstCorreosNotificarTodos.Add(objJefeInmediato.correo);
                                
                            }

                            lstCorreosNotificarTodos.Add("serviciosalpersonal@construplan.com.mx");

#if DEBUG
                            lstCorreosNotificarTodos = new List<string>();
                            //lstCorreosNotificarTodos.Add("omar.nunez@construplan.com.mx");
                            lstCorreosNotificarTodos.Add("miguel.buzani@construplan.com.mx");
#endif

                            string descTipoVacaciones = "";

                            if (objVacaciones.tipoVacaciones == 7)
                            {
                                descTipoVacaciones = "VACACIONES LISTAS PARA SU GESTION. CC: {2} EMPLEADO: [{0}] {1}";
                            }
                            else
                            {
                                descTipoVacaciones = "AUSENCIAS LISTAS PARA SU GESTION. CC: {2} EMPLEADO: [{0}] {1}";
                            }

                            List<byte[]> downloadPDFs = new List<byte[]>();
                            using (var streamReader = new MemoryStream())
                            {
                                stream.CopyTo(streamReader);
                                downloadPDFs.Add(streamReader.ToArray());

                                GlobalUtils.sendEmailAdjuntoInMemory2(string.Format(PersonalUtilities.GetNombreEmpresa() + ": " + descTipoVacaciones, objVacaciones.claveEmpleado, objVacaciones.nombreEmpleado, objVacacion.cc),
                                    cuerpo, lstCorreosNotificarTodos, downloadPDFs, (objVacaciones.tipoVacaciones == 7 ? "Vacaciones.pdf" : "ControldeAusencias.pdf"));

                            }
                            #endregion

                        }
                        else
                        {
                            var lstDates = fechasExistentes.Select(e => e.fecha).ToList();
                            var datesToDel = new List<DateTime?>();

                            datesToDel.AddRange(lstDates);

                            if (lstFechas != null)
                            {
                                foreach (var item in lstFechas)
                                {
                                    var objAusenciaExistente = fechasEmpleado.FirstOrDefault(e => e.fecha.Value.Date == item.Date && e.tipoInsidencia != tipoPermiso);
                                    if (objAusenciaExistente != null)
                                    {
                                        var objVacacionExistente = lstAusencias.FirstOrDefault(e => e.id == objAusenciaExistente.vacacionID);

                                        #region DESCRIPCION MOTIVO

                                        string descMotivo = "";

                                        switch (objVacacionExistente.tipoVacaciones)
                                        {
                                            case 0:
                                                descMotivo = "Permiso paternidad (PGS)";
                                                break;
                                            case 1:
                                                descMotivo = "Permiso de matrimonio (PGS)";
                                                break;
                                            case 2:
                                                descMotivo = "Permiso sindical (PGS)";
                                                break;
                                            case 3:
                                                descMotivo = "Permiso por fallecimiento (PGS)";
                                                break;
                                            case 5:
                                                descMotivo = "Permiso médico (PGS)";
                                                break;
                                            case 7:
                                                descMotivo = "Vacaciones (VAC)";
                                                break;
                                            case 8:
                                                descMotivo = "Permiso SIN goce de sueldo (PS)";
                                                break;
                                            case 9:
                                                descMotivo = "Permiso de comision de trabajo (CT)";
                                                break;
                                            case 10:
                                                descMotivo = "Home office (PGS)";
                                                break;
                                            case 11:
                                                descMotivo = "Tiempo x tiempo (PGS)";
                                                break;
                                            case 13:
                                                descMotivo = "Suspención (SUSP)";
                                                break;
                                            default:
                                                descMotivo = "S/N";
                                                break;
                                        }
                                        #endregion

                                        if (objAusenciaExistente.esAplicadaIncidencias)
                                        {
                                            result.Add("esRecursar", false);
                                            throw new Exception(("Ya existe una ausencia el dia: " + item.ToString("dd/MM/yyyy") + " de tipo: " + descMotivo + " y esta aplicada en incidencias, favor de verificar los dias"));
                                        }
                                        else
                                        {
                                            if (esSobreEscribir)
                                            {
                                                //ELIMINAR AUSENCIA A SOBREESCRIBIR
                                                objAusenciaExistente.registroActivo = false;
                                                objAusenciaExistente.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                                objAusenciaExistente.fechaModificacion = DateTime.Now;
                                                _context.SaveChanges();

                                                //SAVE BITACORA
                                                SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, objAusenciaExistente.id, JsonUtils.convertNetObjectToJson(objAusenciaExistente));

                                                #region ACTUALIZAR INCIDENCIAS

                                                var per = _context.tblRH_EK_Periodos.Where(x => (x.year == objAusenciaExistente.fecha.Value.Year) && x.tipo_nomina == objEmpleado.tipo_nomina).ToList()
                                                    .FirstOrDefault(e => e.fecha_inicial.Date <= objAusenciaExistente.fecha.Value.Date && e.fecha_final.Date >= objAusenciaExistente.fecha.Value.Date);

                                                var objIncidencia = _context.tblRH_BN_Incidencia.FirstOrDefault(e => e.anio == objAusenciaExistente.fecha.Value.Year && e.periodo == per.id && e.cc == objVacacion.cc && e.tipo_nomina == objEmpleado.tipo_nomina);

                                                if (objIncidencia != null)
                                                {
                                                    var objIncidenciaDet = _context.tblRH_BN_Incidencia_det.FirstOrDefault(e => e.clave_empleado.ToString() == objVacacion.claveEmpleado && e.incidenciaID == objIncidencia.id);

                                                    if (objIncidenciaDet != null)
                                                    {
                                                        TimeSpan difFechasVacaciones = (objAusenciaExistente.fecha ?? DateTime.Today) - per.fecha_inicial;
                                                        int diaVacaciones = difFechasVacaciones.Days + 1;

                                                        //SAVE BITACORA ANTES
                                                        SaveBitacora(0, (int)AccionEnum.CONSULTA, objIncidenciaDet.id, JsonUtils.convertNetObjectToJson(objIncidenciaDet));

                                                        if (diaVacaciones == 1) objIncidenciaDet.dia1 = objAusenciaExistente.tipoInsidencia;
                                                        if (diaVacaciones == 2) objIncidenciaDet.dia2 = objAusenciaExistente.tipoInsidencia;
                                                        if (diaVacaciones == 3) objIncidenciaDet.dia3 = objAusenciaExistente.tipoInsidencia;
                                                        if (diaVacaciones == 4) objIncidenciaDet.dia4 = objAusenciaExistente.tipoInsidencia;
                                                        if (diaVacaciones == 5) objIncidenciaDet.dia5 = objAusenciaExistente.tipoInsidencia;
                                                        if (diaVacaciones == 6) objIncidenciaDet.dia6 = objAusenciaExistente.tipoInsidencia;
                                                        if (diaVacaciones == 7) objIncidenciaDet.dia7 = objAusenciaExistente.tipoInsidencia;
                                                        if (diaVacaciones == 8) objIncidenciaDet.dia8 = objAusenciaExistente.tipoInsidencia;
                                                        if (diaVacaciones == 9) objIncidenciaDet.dia9 = objAusenciaExistente.tipoInsidencia;
                                                        if (diaVacaciones == 10) objIncidenciaDet.dia10 = objAusenciaExistente.tipoInsidencia;
                                                        if (diaVacaciones == 11) objIncidenciaDet.dia11 = objAusenciaExistente.tipoInsidencia;
                                                        if (diaVacaciones == 12) objIncidenciaDet.dia12 = objAusenciaExistente.tipoInsidencia;
                                                        if (diaVacaciones == 13) objIncidenciaDet.dia13 = objAusenciaExistente.tipoInsidencia;
                                                        if (diaVacaciones == 14) objIncidenciaDet.dia14 = objAusenciaExistente.tipoInsidencia;
                                                        if (diaVacaciones == 15) objIncidenciaDet.dia15 = objAusenciaExistente.tipoInsidencia;
                                                        if (diaVacaciones == 16) objIncidenciaDet.dia16 = objAusenciaExistente.tipoInsidencia;

                                                        _context.SaveChanges();

                                                        //SAVE BITACORA DESPUES
                                                        SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, objIncidenciaDet.id, JsonUtils.convertNetObjectToJson(objIncidenciaDet));
                                                    }
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                result.Add("esRecursar", true);
                                                throw new Exception(("Ya existe una ausencia el dia: " + item.ToString("dd/MM/yyyy") + " de tipo: " + descMotivo + ", desea sobreescribir el tipo de ausencia y las incidencias?"));
                                            }
                                        }
                                        
                                    }

                                    var objFecha = new tblRH_Vacaciones_Fechas();
                                    var temp = item;

                                    if (!lstDates.Contains(temp))
                                    {

                                        objFecha.fecha = item.Date;
                                        objFecha.vacacionID = idVacacion;
                                        objFecha.tipoInsidencia = tipoPermiso;
                                        objFecha.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                                        objFecha.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                                        objFecha.fechaCreacion = DateTime.Now;
                                        objFecha.fechaModificacion = DateTime.Now;
                                        objFecha.registroActivo = true;
                                        _context.tblRH_Vacaciones_Fechas.Add(objFecha);
                                        _context.SaveChanges();

                                    }
                                    else
                                    {
                                        datesToDel.Remove(item);

                                    }
                                }
                            }

                            if (datesToDel.Count() > 0)
                            {
                                foreach (var item in datesToDel)
                                {
                                    var objFecha = _context.tblRH_Vacaciones_Fechas.Where(e => e.fecha == item && e.vacacionID == idVacacion && e.registroActivo).OrderBy(e => e.fecha.Value).FirstOrDefault();
                                    objFecha.registroActivo = false;
                                    objFecha.fechaModificacion = DateTime.Now;
                                    objFecha.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                                    _context.SaveChanges();
                                }
                            }

                        }
                        #endregion

                    }

                    dbContextTransaction.Commit();
                    result.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "CrearEditarFechas", e, AccionEnum.CONSULTA, 0, 0);
                    result.Add(MESSAGE, e.Message);
                    result.Add(SUCCESS, false);
                }
            }

            return result;
        }

        #endregion

        #region CRUD PERIODOS

        public Dictionary<string, object> GetPeriodos(PeriodosDTO objFiltro)
        {
            result = new Dictionary<string, object>();

            try
            {
                var lstPeriodos = _context.tblRH_Vacaciones_Periodos.Where(e => e.registroActivo == true && (objFiltro.id > 0 ? e.id == objFiltro.id : true)).ToList();

                result.Add(ITEMS, lstPeriodos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }

        public Dictionary<string, object> CrearEditarPeriodo(PeriodosDTO objPeriodo)
        {
            result = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {

                try
                {
                    #region Validaciones

                    #endregion

                    var objCEPeriodo = new tblRH_Vacaciones_Periodos();

                    if (objPeriodo.id > 0)
                    {

                        objCEPeriodo = _context.tblRH_Vacaciones_Periodos.Where(e => e.id == objPeriodo.id).FirstOrDefault();

                        if (objCEPeriodo == null)
                            throw new Exception("Ocurrio algo mal");

                        #region EDITAR

                        objCEPeriodo.estado = objPeriodo.estado;
                        objCEPeriodo.periodoDesc = objPeriodo.periodoDesc;
                        objCEPeriodo.fechaInicio = objPeriodo.fechaInicio;
                        objCEPeriodo.fechaFinal = objPeriodo.fechaFinal;
                        objCEPeriodo.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCEPeriodo.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();

                        #endregion
                    }
                    else
                    {
                        #region CREAR

                        objCEPeriodo.estado = objPeriodo.estado;
                        objCEPeriodo.periodoDesc = objPeriodo.periodoDesc;
                        objCEPeriodo.fechaInicio = objPeriodo.fechaInicio;
                        objCEPeriodo.fechaFinal = objPeriodo.fechaFinal;
                        objCEPeriodo.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCEPeriodo.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCEPeriodo.fechaCreacion = DateTime.Now;
                        objCEPeriodo.fechaModificacion = DateTime.Now;
                        objCEPeriodo.registroActivo = true;
                        _context.tblRH_Vacaciones_Periodos.Add(objCEPeriodo);
                        _context.SaveChanges();

                        #endregion
                    }

                    dbContextTransaction.Commit();
                    result.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "CrearEditarPeriodo", e, AccionEnum.CONSULTA, 0, 0);
                    result.Add(MESSAGE, e.Message);
                    result.Add(SUCCESS, false);
                }

            }

            return result;
        }

        public Dictionary<string, object> EliminarPeriodo(int id)
        {
            result = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var objDelPeriodo = _context.tblRH_Vacaciones_Periodos.Where(e => e.id == id).FirstOrDefault();

                    if (objDelPeriodo == null)
                        throw new Exception("Ocurrio algo mal");

                    objDelPeriodo.registroActivo = false;
                    objDelPeriodo.fechaModificacion = DateTime.Now;
                    objDelPeriodo.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    result.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "EliminarPeriodo", e, AccionEnum.CONSULTA, 0, 0);
                    result.Add(MESSAGE, e.Message);
                    result.Add(SUCCESS, false);
                }
            }

            return result;
        }

        #endregion

        #region RESPONSABLES
        public Dictionary<string, object> GetResponsables(string cc, int claveEmpleado)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var listaResponsables = _context.tblRH_Vacaciones_Responsables.Where(x => x.registroActivo &&
                    (claveEmpleado != 0 ? claveEmpleado == x.clave_empleado : true)).Select(x => new ResponsableDTO
                    {
                        id = x.id,
                        clave_empleado = x.clave_empleado,
                        nombreEmpleado = "",
                        clave_responsable = x.clave_responsable,
                        nombreResponsable = "",
                        cc = "",
                        ccDesc = "",
                        diasPaternidad = 0,
                        diasMatrimonio = 0
                    }).ToList();

                foreach (var responsable in listaResponsables)
                {
                    var listaRegistrosDias = _context.tblRH_Vacaciones_Responsables_Dias.Where(x => x.registroActivo && x.responsable_id == responsable.id).ToList();
                    var empleadoDatos = GetDatosPersona(responsable.clave_empleado, "")["objDatosPersona"] as BajaPersonalDTO;
                    var responsableDatos = GetDatosPersona(responsable.clave_responsable, "")["objDatosPersona"] as BajaPersonalDTO;
                    //var ultimaFechaIngreso = 

                    var lstFechas = GetFechas(responsable.clave_empleado, 7, null)["items"] as List<tblRH_Vacaciones_Fechas>;

                    responsable.nombreEmpleado = empleadoDatos.nombreCompleto;
                    responsable.fecha_ingreso = GetFechaIngreso(responsable.clave_empleado);
                    responsable.nombreResponsable = string.Empty; //responsableDatos.nombreCompleto;
                    responsable.cc = empleadoDatos.numCC;
                    responsable.ccDesc = empleadoDatos.cc;
                    responsable.diasIniciales = listaRegistrosDias.Sum(x => x.dias);
                    responsable.diasTomados = GetNumDias(responsable.clave_empleado.ToString()) ?? 0;
                    responsable.diasPendientes = (responsable.diasIniciales + responsable.diasTomados) - lstFechas.Count();
                    responsable.esDiasIniciales = true;

                    // SE OBTIENE CANTIDAD DE DIAS DE VACACIONES DE PATERNIDAD Y MATRIMONIO DEL EMPLEADO

                    int idEmpresaSesion = (int)vSesiones.sesionEmpresaActual;
                    tblRH_Vacaciones_Responsables_Dias objCantDiasPaternindadMatrimonio = _context.Select<tblRH_Vacaciones_Responsables_Dias>(new DapperDTO
                    {
                        baseDatos = idEmpresaSesion == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan :
                                    idEmpresaSesion == (int)EmpresaEnum.Arrendadora ? MainContextEnum.Arrendadora :
                                    idEmpresaSesion == (int)EmpresaEnum.Colombia ? MainContextEnum.Colombia :
                                                                                      MainContextEnum.PERU,
                        consulta = @"SELECT SUM(diasPaternidad) AS diasPaternidad, SUM(diasMatrimonio) AS diasMatrimonio
	                                        FROM tblRH_Vacaciones_Responsables AS t1
	                                        INNER JOIN tblRH_Vacaciones_Responsables_Dias AS t2 ON t1.id = t2.responsable_id
		                                        WHERE t1.clave_empleado = @clave_empleado",
                        parametros = new { clave_empleado = responsable.clave_empleado }
                    }).FirstOrDefault();

                    //responsable.diasPaternidad = objCantDiasPaternindadMatrimonio.diasPaternidad != null ? objCantDiasPaternindadMatrimonio.diasPaternidad : 0;
                    //responsable.diasMatrimonio = objCantDiasPaternindadMatrimonio.diasMatrimonio != null ? objCantDiasPaternindadMatrimonio.diasMatrimonio : 0;

                    if (listaRegistrosDias.Count() > 0)
                    {
                        int años = (DateTime.Now - responsable.fecha_ingreso.Value).Days / 365;

                        int añoVencer = responsable.fecha_ingreso.Value.Year + años;

                        DateTime nextDate = new DateTime(añoVencer, responsable.fecha_ingreso.Value.Month, responsable.fecha_ingreso.Value.Day);

                        var min = listaRegistrosDias.Select(e => e.fechaCreacion).Min();

                        //int añosFin = (nextDate - min).Days / 365;

                        if (nextDate > min)
                        {
                            responsable.esDiasIniciales = false;
                            responsable.diasIniciales = 0;
                        }
                    }
                }

                if (cc != "" && cc != null)
                {
                    listaResponsables = listaResponsables.Where(x => x.cc == cc).ToList();
                }

                resultado.Add("data", listaResponsables);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetResponsables", e, AccionEnum.CONSULTA, 0, cc);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> VerificarAntiguedadEmpleado(int claveEmpleado)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                #region SE VERIFICA SI EL EMPLEADO YA CUENTA CON AL MENOS 1 AÑO DE ANTIGUEDAD
                tblRH_EK_Empleados objEmpleado = _context.tblRH_EK_Empleados.Where(w => w.clave_empleado == claveEmpleado && w.esActivo).FirstOrDefault();
                if (objEmpleado == null)
                    throw new Exception("Ocurrió un error al obtener la antiguedad del empleado.");

                DateTime fechaAlta = Convert.ToDateTime(objEmpleado.fecha_alta);
                DateTime fechaActual = DateTime.Now;
                TimeSpan difDias = fechaActual - fechaAlta;
                int cantDias = difDias.Days;
                if (cantDias >= 365)
                {
                    resultado.Add(MESSAGE, "El empleado cuenta con vacaciones");
                    resultado.Add("tieneVacaciones", true);
                }
                else
                    resultado.Add("tieneVacaciones", false);

                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "VerificarAntiguedadEmpleado", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> CrearEditarResponsable(VacacionesDTO objDTO)
        {
            var resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region COMENTADO
                    //if (responsable.id == 0)
                    //{
                    //    responsable.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                    //    responsable.fechaCreacion = DateTime.Now;
                    //    responsable.usuarioModificacion_id = 0;
                    //    responsable.fechaModificacion = null;
                    //    responsable.registroActivo = true;

                    //    _context.tblRH_Vacaciones_Responsables.Add(responsable);
                    //    _context.SaveChanges();

                    //    if (dias > 0)
                    //    {
                    //        #region Agregar registro de días
                    //        _context.tblRH_Vacaciones_Responsables_Dias.Add(new tblRH_Vacaciones_Responsables_Dias
                    //        {
                    //            responsable_id = responsable.id,
                    //            dias = dias,
                    //            usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id,
                    //            fechaCreacion = DateTime.Now,
                    //            usuarioModificacion_id = 0,
                    //            fechaModificacion = null,
                    //            registroActivo = true
                    //        });
                    //        _context.SaveChanges();
                    //        #endregion
                    //    }
                    //}
                    //else
                    //{
                    //    var responsableSIGOPLAN = _context.tblRH_Vacaciones_Responsables.FirstOrDefault(x => x.registroActivo && x.id == responsable.id);

                    //    if (responsableSIGOPLAN == null)
                    //    {
                    //        throw new Exception("No se encuentra la información del registro.");
                    //    }

                    //    responsableSIGOPLAN.clave_empleado = responsable.clave_empleado;
                    //    responsableSIGOPLAN.clave_responsable = responsable.clave_responsable;
                    //    responsableSIGOPLAN.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                    //    responsableSIGOPLAN.fechaModificacion = DateTime.Now;
                    //    _context.SaveChanges();

                    //    if (dias != 0)
                    //    {
                    //        #region Agregar registro de días
                    //        _context.tblRH_Vacaciones_Responsables_Dias.Add(new tblRH_Vacaciones_Responsables_Dias
                    //        {
                    //            responsable_id = responsable.id,
                    //            dias = dias,
                    //            usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id,
                    //            fechaCreacion = DateTime.Now,
                    //            usuarioModificacion_id = 0,
                    //            fechaModificacion = null,
                    //            registroActivo = true
                    //        });
                    //        _context.SaveChanges();
                    //        #endregion
                    //    }
                    //}
                    #endregion

                    #region VALIDACIONES
                    if (string.IsNullOrEmpty(objDTO.claveEmpleado)) { throw new Exception("Es necesario indicar la clave del empleado."); }
                    #endregion

                    #region SE REGISTRA LA CANTIDAD DE VACACIONES AL EMPLEADO

                    // SE CREA REGISTRO PRINCIPAL
                    tblRH_Vacaciones_Responsables objCEResponsable = new tblRH_Vacaciones_Responsables();
                    objCEResponsable.clave_empleado = Convert.ToInt32(objDTO.claveEmpleado);
                    objCEResponsable.clave_responsable = 0;
                    objCEResponsable.usuarioCreacion_id = (int)vSesiones.sesionUsuarioDTO.id;
                    objCEResponsable.fechaCreacion = DateTime.Now;
                    objCEResponsable.registroActivo = true;
                    _context.tblRH_Vacaciones_Responsables.Add(objCEResponsable);
                    _context.SaveChanges();

                    // SE OBTIENE ID DEL REGISTRO RECIEN HECHO
                    tblRH_Vacaciones_Responsables objResponsable = _context.tblRH_Vacaciones_Responsables.OrderByDescending(o => o.id).FirstOrDefault();

                    // SE CREA REGISTRO DETALLE
                    tblRH_Vacaciones_Responsables_Dias objCEResponsableDetalle = new tblRH_Vacaciones_Responsables_Dias();
                    objCEResponsableDetalle.responsable_id = objResponsable.id;
                    objCEResponsableDetalle.dias = objDTO.dias;
                    //objCEResponsableDetalle.diasPaternidad = objDTO.diasPaternidad;
                    //objCEResponsableDetalle.diasMatrimonio = objDTO.diasMatrimonio;
                    objCEResponsableDetalle.usuarioCreacion_id = (int)vSesiones.sesionUsuarioDTO.id;
                    objCEResponsableDetalle.fechaCreacion = DateTime.Now;
                    objCEResponsableDetalle.registroActivo = true;
                    _context.tblRH_Vacaciones_Responsables_Dias.Add(objCEResponsableDetalle);
                    _context.SaveChanges();
                    #endregion

                    dbContextTransaction.Commit();
                    SaveBitacora(0, (int)AccionEnum.AGREGAR, objDTO.id, JsonUtils.convertNetObjectToJson(objDTO));
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha registrado con éxito.");
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "CrearEditarResponsable", e, AccionEnum.AGREGAR, objDTO.id, objDTO);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }
        public Dictionary<string, object> EliminarResponsable(int id)
        {
            var resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var responsableSIGOPLAN = _context.tblRH_Vacaciones_Responsables.FirstOrDefault(x => x.id == id);

                    if (responsableSIGOPLAN != null)
                    {
                        responsableSIGOPLAN.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                        responsableSIGOPLAN.fechaModificacion = DateTime.Now;
                        responsableSIGOPLAN.registroActivo = false;
                        _context.SaveChanges();

                        var listaRegistrosDias = _context.tblRH_Vacaciones_Responsables_Dias.Where(x => x.registroActivo && x.responsable_id == id).ToList();

                        foreach (var registroDia in listaRegistrosDias)
                        {
                            registroDia.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                            registroDia.fechaModificacion = DateTime.Now;
                            registroDia.registroActivo = false;
                            _context.SaveChanges();
                        }
                    }

                    dbContextTransaction.Commit();
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, id, JsonUtils.convertNetObjectToJson(new { id = id }));
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "EliminarResponsable", e, AccionEnum.ELIMINAR, 0, id);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }

            return resultado;
        }
        public Dictionary<string, object> GetHistorialDias(int id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var listaUsuarios = _context.tblP_Usuario.Where(x => x.estatus).ToList();
                var objResponsable = _context.tblRH_Vacaciones_Responsables.FirstOrDefault(e => e.clave_empleado == id);

                var listaRegistrosDias = _context.tblRH_Vacaciones_Responsables_Dias.Where(x => x.registroActivo && x.responsable_id == objResponsable.id).ToList().Select(x => new
                {
                    id = x.id,
                    dias = x.dias,
                    nombreUsuario = listaUsuarios.Where(y => y.id == x.usuarioCreacion_id).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).FirstOrDefault(),
                    fecha = x.fechaCreacion,
                    fechaString = x.fechaCreacion.ToShortDateString()
                }).ToList();

                var lstFechas = GetFechas(id, 7, null)["items"] as List<tblRH_Vacaciones_Fechas>;

                var diasIniciales = GetNumDias(id.ToString());

                resultado.Add("data", listaRegistrosDias);
                resultado.Add("diasTomados", lstFechas.Count());
                resultado.Add("diasIniciales", diasIniciales);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetHistorialDias", e, AccionEnum.CONSULTA, 0, id);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public Dictionary<string, object> GetAutorizantes(string cc, int? clave_empleado, string nombre_empleado)
        {
            resultado.Clear();

            try
            {
                var usuarios = new List<ComboDTO>();

                var plantillasRequisiciones = new List<int> { 125 };

                var paquete = _context.tblFA_Paquete.Where(x => x.cc.cc == cc).OrderByDescending(x => x.fechaCreacion).FirstOrDefault();

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

                //if (clave_empleado != null)
                //{
                //    var objEmpleado = usuariosSinRepetir.FirstOrDefault(e => e.Value == clave_empleado.ToString());

                //    if (objEmpleado == null)
                //    {
                //        usuariosSinRepetir.Add(new ComboDTO
                //        {
                //            Value = clave_empleado.ToString(),
                //            Text = nombre_empleado ?? ""
                //        });
                //    }
                //}

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

        #endregion

        #region PENDIENTES

        public Dictionary<string, object> GetVacacionesPendientes(VacacionesDTO objFiltro)
        {
            result = new Dictionary<string, object>();

            try
            {
                var lstVacaciones = GetVacaciones(objFiltro)["items"] as List<VacacionesDTO>;
                List<VacacionesDTO> lstVacacionesPendientes = new List<VacacionesDTO>();
                //DateTime btmDate = DateTime.Now;

                TimeSpan twoWeeks = new TimeSpan(14, 0, 0, 0);

                //btmDate.Subtract(twoWeeks);

                //var lstVacacionesPendientes = lstVacaciones.Where(e => e.fechaInicial >= DateTime.Now && e.fechaInicial.Value.Subtract(twoWeeks) <= DateTime.Now).ToList();

                foreach (var item in lstVacaciones)
                {
                    DateTime tempTwoWeeks = item.fechaInicial.Value.Subtract(twoWeeks);
                    if (tempTwoWeeks <= DateTime.Now && DateTime.Now <= item.fechaInicial)
                    {
                        lstVacacionesPendientes.Add(item);
                    }
                }

                result = new Dictionary<string, object>();


                result.Add(SUCCESS, true);
                result.Add(ITEMS, lstVacacionesPendientes);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }

        #endregion

        #region PERMISOS
        public Dictionary<string, object> GetDiasDispPermisos(int cveUsuario, int tipoPermiso)
        {
            result = new Dictionary<string, object>();
            try
            {
                var dicDatosPersona = GetDatosPersona(Convert.ToInt32(cveUsuario), "");
                var datosPersona = dicDatosPersona["objDatosPersona"] as BajaPersonalDTO;

                DateTime btmFecha = new DateTime(DateTime.Now.Year, datosPersona.fechaIngreso.Month, datosPersona.fechaIngreso.Day);
                DateTime topFecha = new DateTime(DateTime.Now.Year, datosPersona.fechaIngreso.Month, datosPersona.fechaIngreso.Day);

                if (DateTime.Now.Month > datosPersona.fechaIngreso.Month && DateTime.Now.Day > datosPersona.fechaIngreso.Day)
                {
                    topFecha.AddYears(1);
                }
                else
                {
                    btmFecha.AddYears(-1);
                }

                var lstIncidencias = _context.tblRH_BN_Incidencia_det.Where(e => e.estatus && e.clave_empleado == cveUsuario && e.fecha < topFecha && e.fecha > btmFecha).ToList();
                var lstIncidenciasCGoce = lstIncidencias.Where(e =>
                    e.dia1 == 4
                    || e.dia2 == 4
                    || e.dia3 == 4
                    || e.dia4 == 4
                    || e.dia5 == 4
                    || e.dia6 == 4
                    || e.dia7 == 4
                    || e.dia8 == 4
                    || e.dia9 == 4
                    || e.dia10 == 4
                    || e.dia11 == 4
                    || e.dia12 == 4
                    || e.dia13 == 4
                    || e.dia14 == 4
                    || e.dia15 == 4
                    || e.dia16 == 4
                    ).ToList();

                result.Clear();
                var lstFechas = GetFechasByClaveEmpleado(cveUsuario, tipoPermiso)["items"] as List<tblRH_Vacaciones_Fechas>;

                result.Clear();
                result.Add(ITEMS, lstIncidenciasCGoce);
                result.Add("lstFechasCapturadas", lstFechas);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return result;
        }

        public bool GetEsPermisoSoloConsultaVacaciones()
        {
            bool esConsulta = false;

            try
            {
                var esPermisoConsultaVacaciones = _context.tblP_AccionesVistatblP_Usuario.FirstOrDefault(e => e.tblP_AccionesVista_id == 4044 && e.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id);

                if (esPermisoConsultaVacaciones != null)
                {
                    esConsulta = true;
                }
            }
            catch (Exception)
            {
            }

            return esConsulta;
        }

        public bool GetEsPermisoSoloConsultaPermisos()
        {
            bool esConsulta = false;

            try
            {
                var esPermisoConsultaPermisos = _context.tblP_AccionesVistatblP_Usuario.FirstOrDefault(e => e.tblP_AccionesVista_id == 4045 && e.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id);

                if (esPermisoConsultaPermisos != null)
                {
                    esConsulta = true;
                }
            }
            catch (Exception)
            {
            }

            return esConsulta;
        }
        #endregion

        #region GENERALES
        private MainContextEnum GetEmpresaMainContextEnum()
        {
            MainContextEnum objEmpresaEnum = new MainContextEnum();
            try
            {
                switch ((int)vSesiones.sesionEmpresaActual)
                {
                    case (int)EmpresaEnum.Construplan:
                        objEmpresaEnum = MainContextEnum.Construplan;
                        break;
                    case (int)EmpresaEnum.Arrendadora:
                        objEmpresaEnum = MainContextEnum.Arrendadora;
                        break;
                    case (int)EmpresaEnum.Colombia:
                        objEmpresaEnum = MainContextEnum.Colombia;
                        break;
                    case (int)EmpresaEnum.Peru:
                        objEmpresaEnum = MainContextEnum.PERU;
                        break;
                }
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
            }
            return objEmpresaEnum;
        }
        #endregion

        #region INCAPACIDADES

        public Dictionary<string, object> GetIncapacidades(int? estatus, List<string> ccs, DateTime? fechaInicio, DateTime? fechaTerminacion, int? claveEmpleado, string nombreEmpleado)
        {
            resultado.Clear();

            try
            {
                if (ccs == null || ccs.Count() == 0)
                {
                    if (vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.ADMINISTRADOR)
                        ccs = _ccFS_SP.GetCCs().Select(x => x.cc).ToList();

                    if (vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.USUARIO)
                    {
                        var usuarioCCs = _context.tblRH_BN_Usuario_CC.Where(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id).Select(x => x.cc).ToList();
                        if (usuarioCCs.Count > 0)
                        {
                            if (usuarioCCs.Any(x => x == "*"))
                                ccs = _ccFS_SP.GetCCs().Select(x => x.cc).ToList();
                            else
                                ccs = _ccFS_SP.GetCCs(usuarioCCs).Select(x => x.cc).ToList();
                        }
                    }
                }

                string textQuery = @"
                                SELECT 
                                    tIncap.*, (tEmpl.ape_paterno + ' ' + tEmpl.ape_materno  + ' ' + tEmpl.nombre) as nombreCompleto, tPuesto.descripcion as puestoDesc,
	                                tEmpl.nss, tRegPat.clave_reg_pat, tRegPat.nombre_corto, tCC.ccDescripcion, ('[' + tCC.cc + '] ' + tCC.ccDescripcion) as ccDesc,
									(
										SELECT TOP 1 id FROM tblRH_Vacaciones_Archivos as tArch WHERE tIncap.id = tArch.idIncapacidad AND tArch.esActivo = 1 ORDER BY fechaCreacion DESC
									) as evidenciaIncap,
                                    (u.apellidoPaterno + ' ' + u.apellidoMaterno + ' ' + u.nombre) as nombreUsuarioCapturo
                                FROM tblRH_Vacaciones_Incapacidades tIncap
                                INNER JOIN tblRH_EK_Empleados as tEmpl ON tIncap.clave_empleado = tEmpl.clave_empleado
                                INNER JOIN tblRH_EK_Registros_Patronales as tRegPat ON tEmpl.id_regpat = tRegPat.clave_reg_pat
                                INNER JOIN tblC_Nom_CatalogoCC as tCC ON tIncap.cc = tCC.cc
                                INNER JOIN tblRH_EK_Puestos as tPuesto ON tEmpl.puesto = tPuesto.puesto
                                LEFT JOIN tblP_Usuario AS u on u.id = tIncap.idUsuarioCreacion
                                WHERE tIncap.esActivo = 1 AND tIncap.cc IN @ccs" +
                                ((fechaInicio != null && fechaTerminacion != null) ? " AND tIncap.fechaInicio >= @fechaInicio AND tIncap.fechaTerminacion <= @fechaTerminacion" : "") +
                                //(estatus != null ? " AND tIncap.estatus = @estatus" : "") +
                                (claveEmpleado != null ? " AND tIncap.clave_empleado = @claveEmpleado" : "") +
                                (!string.IsNullOrEmpty(nombreEmpleado) ? " AND (tEmpl.ape_paterno + ' ' + tEmpl.ape_materno  + ' ' + tEmpl.nombre) LIKE @nombreEmpleado" : "") +

                                " ORDER BY tIncap.fechaInicio";

                var lstIncaps = _context.Select<IncapacidadesDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = textQuery,
                    parametros = new { estatus, ccs, fechaInicio, fechaTerminacion, claveEmpleado, nombreEmpleado = "%" + nombreEmpleado + "%" }
                });

                var lstFilteredIncaps = new List<IncapacidadesDTO>();

                foreach (var item in lstIncaps)
                {
                    int estatusPorFecha = 0;
                    item.descIncap = EnumExtensions.GetDescription((IncapacidadesEnum)item.tipoIncap);
                    item.descIncap2 = EnumExtensions.GetDescription((Incapacidades2Enum)item.tipoIncap2);

                    if (item.fechaTerminacion.Date < DateTime.Now.Date)
                    {
                        item.descEstatus = "VENCIDA";
                        estatusPorFecha = 0;
                    }
                    else
                    {
                        if (item.fechaTerminacion.Date >= DateTime.Now.Date)
                        {
                            item.descEstatus = "VIGENTE";
                            estatusPorFecha = 1;
                        }
                    }

                    if (estatus != null)
                    {
                        if (estatus == estatusPorFecha)
                        {
                            lstFilteredIncaps.Add(item);
                        }
                    }
                    else
                    {
                        lstFilteredIncaps.Add(item);

                    }
                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstFilteredIncaps);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetHistorialIncapacidades(int clave_empleado)
        {
            resultado.Clear();

            try
            {
                //var lstIncaps = _context.tblRH_Vacaciones_Incapacidades.Where(e => e.esActivo && e.clave_empleado == clave_empleado).OrderByDescending(e => e.fechaInicio).ToList();
                string textQuery = @"
                                SELECT 
                                    tIncap.*, (tEmpl.ape_paterno + ' ' + tEmpl.ape_materno  + ' ' + tEmpl.nombre) as nombreCompleto, tPuesto.descripcion as puestoDesc,
	                                tEmpl.nss, tRegPat.clave_reg_pat, tRegPat.nombre_corto, tCC.ccDescripcion, ('[' + tCC.cc + '] ' + tCC.ccDescripcion) as ccDesc,
									(
										SELECT TOP 1 id FROM tblRH_Vacaciones_Archivos as tArch WHERE tIncap.id = tArch.idIncapacidad AND tArch.esActivo = 1 ORDER BY fechaCreacion DESC
									) as evidenciaIncap
                                FROM tblRH_Vacaciones_Incapacidades tIncap
                                INNER JOIN tblRH_EK_Empleados as tEmpl ON tIncap.clave_empleado = tEmpl.clave_empleado
                                INNER JOIN tblRH_EK_Registros_Patronales as tRegPat ON tEmpl.id_regpat = tRegPat.clave_reg_pat
                                INNER JOIN tblC_Nom_CatalogoCC as tCC ON tIncap.cc = tCC.cc
                                INNER JOIN tblRH_EK_Puestos as tPuesto ON tEmpl.puesto = tPuesto.puesto
                                WHERE tIncap.esActivo = 1 AND tIncap.clave_empleado = @clave_empleado ORDER BY nombreCompleto";

                var lstIncaps = _context.Select<IncapacidadesDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = textQuery,
                    parametros = new { clave_empleado }
                });

                foreach (var item in lstIncaps)
                {
                    item.descIncap = EnumExtensions.GetDescription((IncapacidadesEnum)item.tipoIncap);
                    item.descEstatus = item.estatus == 1 ? "ACTIVA" : "NO ACTIVA";
                }

                resultado.Add(ITEMS, lstIncaps.OrderByDescending(e => e.fechaInicio).ToList());
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        private Dictionary<string, object> GetHistorialIncapacidadesbyID(int id_incapacidad)
        {
            resultado.Clear();

            try
            {
                //var lstIncaps = _context.tblRH_Vacaciones_Incapacidades.Where(e => e.esActivo && e.clave_empleado == clave_empleado).OrderByDescending(e => e.fechaInicio).ToList();
                string textQuery = @"
                                SELECT 
                                    tIncap.*, (tEmpl.ape_paterno + ' ' + tEmpl.ape_materno  + ' ' + tEmpl.nombre) as nombreCompleto, tPuesto.descripcion as puestoDesc,
	                                tEmpl.nss, tRegPat.clave_reg_pat, tRegPat.nombre_corto, tCC.ccDescripcion, ('[' + tCC.cc + '] ' + tCC.ccDescripcion) as ccDesc,
									(
										SELECT TOP 1 id FROM tblRH_Vacaciones_Archivos as tArch WHERE tIncap.id = tArch.idIncapacidad AND tArch.esActivo = 1 ORDER BY fechaCreacion DESC
									) as evidenciaIncap
                                FROM tblRH_Vacaciones_Incapacidades tIncap
                                INNER JOIN tblRH_EK_Empleados as tEmpl ON tIncap.clave_empleado = tEmpl.clave_empleado
                                INNER JOIN tblRH_EK_Registros_Patronales as tRegPat ON tEmpl.id_regpat = tRegPat.clave_reg_pat
                                INNER JOIN tblC_Nom_CatalogoCC as tCC ON tIncap.cc = tCC.cc
                                INNER JOIN tblRH_EK_Puestos as tPuesto ON tEmpl.puesto = tPuesto.puesto
                                WHERE tIncap.esActivo = 1 AND tIncap.id = @id_incapacidad ORDER BY nombreCompleto";

                var lstIncaps = _context.Select<IncapacidadesDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = textQuery,
                    parametros = new { id_incapacidad }
                });

                foreach (var item in lstIncaps)
                {
                    item.descIncap = EnumExtensions.GetDescription((IncapacidadesEnum)item.tipoIncap);
                    item.descEstatus = item.estatus == 1 ? "ACTIVA" : "NO ACTIVA";
                }

                resultado.Add(ITEMS, lstIncaps.OrderByDescending(e => e.fechaInicio).ToList());
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> CrearEditarIncapacidades(IncapacidadesDTO objIncaps)
        {
            using (var dbTransac = _context.Database.BeginTransaction())
            {
                resultado.Clear();

                try
                {
                    if (objIncaps.id > 0)
                    {
                        var objCEIncaps = _context.tblRH_Vacaciones_Incapacidades.FirstOrDefault(e => e.id == objIncaps.id);

                        if (objCEIncaps == null)
                        {
                            throw new Exception("Ocurrio un error actulizando el registro");
                        }

                        objCEIncaps.estatus = objIncaps.estatus;
                        objCEIncaps.codigoIncap = objIncaps.codigoIncap;
                        objCEIncaps.tipoIncap = objIncaps.tipoIncap;
                        objCEIncaps.tipoIncap2 = objIncaps.tipoIncap2;
                        objCEIncaps.totalDias = objIncaps.totalDias;
                        objCEIncaps.fechaInicio = objIncaps.fechaInicio;
                        objCEIncaps.fechaTerminacion = objIncaps.fechaTerminacion;
                        objCEIncaps.motivoIncap = objIncaps.motivoIncap;
                        objCEIncaps.esAplicadaIncidencias = false;
                        objCEIncaps.fechaModificacion = DateTime.Now;
                        objCEIncaps.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                        _context.SaveChanges();
                        SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, objCEIncaps.id, JsonUtils.convertNetObjectToJson(objCEIncaps));


                    }
                    else
                    {
                        //CHECAR CODIGO DE INCAPACIDAD PARA DUPLICADOS
                        var objUIncap = _context.tblRH_Vacaciones_Incapacidades.FirstOrDefault(e => e.esActivo && e.codigoIncap == objIncaps.codigoIncap);
                        if (objUIncap != null)
                            throw new Exception("La incapacidad con codigo: " + objUIncap.codigoIncap + " Ya existe.");

                        var objCrearIncap = new tblRH_Vacaciones_Incapacidades()
                        {
                            estatus = 1,
                            cc = objIncaps.cc,
                            clave_empleado = objIncaps.clave_empleado,
                            codigoIncap = objIncaps.codigoIncap,
                            tipoIncap = objIncaps.tipoIncap,
                            tipoIncap2 = objIncaps.tipoIncap2,
                            fechaInicio = objIncaps.fechaInicio,
                            totalDias = objIncaps.totalDias,
                            fechaTerminacion = objIncaps.fechaTerminacion,
                            motivoIncap = objIncaps.motivoIncap,
                            esNotificada = false,
                            fechaCreacion = DateTime.Now,
                            fechaModificacion = DateTime.Now,
                            idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id,
                            idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id,
                            esActivo = true,
                        };
                        _context.tblRH_Vacaciones_Incapacidades.Add(objCrearIncap);
                        _context.SaveChanges();
                        SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, objCrearIncap.id, JsonUtils.convertNetObjectToJson(objCrearIncap));

                    }

                    resultado.Add(SUCCESS, true);
                    dbTransac.Commit();
                }
                catch (Exception e)
                {
                    dbTransac.Rollback();
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);

                    LogError(0, 0, _NOMBRE_CONTROLADOR, "CrearEditarIncapacidad", e, AccionEnum.ACTUALIZAR, 0, objIncaps);
                }
                return resultado;
            }
        }

        public Dictionary<string, object> DeleteIncapacidades(int id_incap)
        {
            resultado.Clear();

            using (var dbTransac = _context.Database.BeginTransaction())
            {
                try
                {
                    var objDelIncap = _context.tblRH_Vacaciones_Incapacidades.FirstOrDefault(e => e.id == id_incap);
                    var lstPeriodos = _context.tblRH_EK_Periodos.Where(e => e.year == DateTime.Now.Year).ToList();
                    var objEmpleado = _context.tblRH_EK_Empleados.FirstOrDefault(e => e.clave_empleado == objDelIncap.clave_empleado && e.esActivo);

                    if (objDelIncap != null)
                    {
                        objDelIncap.fechaModificacion = DateTime.Now;
                        objDelIncap.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                        objDelIncap.esActivo = false;
                    }

                    #region ELIMINAR INCIDENCIAS
                    var lstIncidencias = _context.tblRH_BN_Incidencia.Where(e =>
                        e.cc == objDelIncap.cc && e.estatus == "P" && e.tipo_nomina == objEmpleado.tipo_nomina && e.anio == DateTime.Now.Year).ToList();
                    var lstIdsIncidecias = lstIncidencias.Select(e => e.id).ToList();

                    var lstIncidenciaDet = _context.tblRH_BN_Incidencia_det.Where(e => lstIdsIncidecias.Contains(e.incidenciaID) && e.clave_empleado == objDelIncap.clave_empleado).ToList();

                    foreach (var incidencia in lstIncidencias)
                    {
                        var objPeriodo = lstPeriodos.FirstOrDefault(e => e.periodo == incidencia.periodo);


                        List<int> diasIncapacidades = new List<int>();


                        TimeSpan difFechasIncapacidadInicio = objDelIncap.fechaInicio >= objPeriodo.fecha_inicial ? objDelIncap.fechaInicio - objPeriodo.fecha_inicial : objPeriodo.fecha_inicial - objPeriodo.fecha_inicial;
                        TimeSpan difFechasIncapacidadFin = objDelIncap.fechaTerminacion <= objPeriodo.fecha_final ? objDelIncap.fechaTerminacion - objPeriodo.fecha_inicial : objPeriodo.fecha_final - objPeriodo.fecha_inicial;
                        int diasIncapacidadInicio = difFechasIncapacidadInicio.Days + 1;
                        int diasIncapacidadFin = difFechasIncapacidadFin.Days + 1;
                        //int rango = (diasIncapacidadFin - diasIncapacidadInicio ) + 1;
                        for (int i = diasIncapacidadInicio; i <= diasIncapacidadFin; i++) diasIncapacidades.Add(i);

                        var objIncidenciaDet = lstIncidenciaDet.FirstOrDefault(e => e.incidenciaID == incidencia.id && e.clave_empleado == objEmpleado.clave_empleado);

                        if (diasIncapacidades.Contains(1)) objIncidenciaDet.dia1 = 0;
                        if (diasIncapacidades.Contains(2)) objIncidenciaDet.dia2 = 0;
                        if (diasIncapacidades.Contains(3)) objIncidenciaDet.dia3 = 0;
                        if (diasIncapacidades.Contains(4)) objIncidenciaDet.dia4 = 0;
                        if (diasIncapacidades.Contains(5)) objIncidenciaDet.dia5 = 0;
                        if (diasIncapacidades.Contains(6)) objIncidenciaDet.dia6 = 0;
                        if (diasIncapacidades.Contains(7)) objIncidenciaDet.dia7 = 0;
                        if (diasIncapacidades.Contains(8)) objIncidenciaDet.dia8 = 0;
                        if (diasIncapacidades.Contains(9)) objIncidenciaDet.dia9 = 0;
                        if (diasIncapacidades.Contains(10)) objIncidenciaDet.dia10 = 0;
                        if (diasIncapacidades.Contains(11)) objIncidenciaDet.dia11 = 0;
                        if (diasIncapacidades.Contains(12)) objIncidenciaDet.dia12 = 0;
                        if (diasIncapacidades.Contains(13)) objIncidenciaDet.dia13 = 0;
                        if (diasIncapacidades.Contains(14)) objIncidenciaDet.dia14 = 0;
                        if (diasIncapacidades.Contains(15)) objIncidenciaDet.dia15 = 0;
                        if (diasIncapacidades.Contains(16)) objIncidenciaDet.dia16 = 0;

                        _context.SaveChanges();

                        
                    }
                    #endregion

                    resultado.Add(SUCCESS, true);
                    dbTransac.Commit();
                    SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, 0, JsonUtils.convertNetObjectToJson(new { clave_incap = id_incap }));
                }
                catch (Exception e)
                {
                    dbTransac.Rollback();
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "CrearEditarIncapacidad", e, AccionEnum.ACTUALIZAR, 0, new { clave_incap = id_incap });
                }
                return resultado;
            }

        }

        public Dictionary<string, object> NotificarIncapacidades(int id_incapacidad)
        {
            using (var dbTransac = _context.Database.BeginTransaction())
            {
                resultado.Clear();

                try
                {
                    var dictIncap = GetHistorialIncapacidadesbyID(id_incapacidad);

                    //var dictIncap = GetHistorialIncapacidades(id_incapacidad);
                    var objIncap = dictIncap["items"] as List<IncapacidadesDTO>;

                    var lstArchivos = new List<adjuntoCorreoDTO>();

                    var lstArchivo = _context.tblRH_Vacaciones_Archivos.Where(e => e.esActivo).OrderByDescending(e => e.fechaCreacion).ToList();
                    var objArchivo = lstArchivo.FirstOrDefault(e => e.idIncapacidad == objIncap[0].id);

                    resultado.Clear();

                    //CAMBIAR ESTADO DE INCAPACIDAD
                    int idIncap = objIncap[0].id;
                    var objIncapacidad = _context.tblRH_Vacaciones_Incapacidades.FirstOrDefault(e => e.id == idIncap);
                    objIncapacidad.esNotificada = true;
                    _context.SaveChanges();

                    //SI TIENE EVIDENCIA ADJUNTARLO AL CORREO
                    if (objArchivo != null)
                    {
                        var ach = new adjuntoCorreoDTO();
                        ach.archivo = System.IO.File.ReadAllBytes(objArchivo.ubicacionArchivo);
                        ach.nombreArchivo = Path.GetFileNameWithoutExtension(objArchivo.ubicacionArchivo);
                        ach.extArchivo = Path.GetExtension(objArchivo.ubicacionArchivo);

                        lstArchivos.Add(ach);
                    }

                    var correos = new List<string>();
                    string ccIncap = objIncap[0].cc;

                    //CH EN OBRA
                    List<int> lstNotificantes = _context.tblRH_Notis_RelConceptoUsuario.
                        Where(e => e.cc == ccIncap && (e.idConcepto == (int)ConceptosNotificantesEnum.Altas
                             || e.idConcepto == (int)ConceptosNotificantesEnum.CH)).
                        Select(e => e.idUsuario).ToList();

                    foreach (var usu in lstNotificantes)
                    {
                        correos.Add(_context.tblP_Usuario.FirstOrDefault(x => x.id == usu).correo);
                    }

                    List<string> lstCorreoGenerales = _context.tblRH_Notis_RelConceptoCorreo.
                        Where(e => (e.cc == "*" || e.cc == ccIncap) && (e.idConcepto == (int)ConceptosNotificantesEnum.Altas
                            || e.idConcepto == (int)ConceptosNotificantesEnum.CH)).
                        Select(e => e.correo).ToList();

                    foreach (var correo in lstCorreoGenerales)
                    {
                        correos.Add(correo);
                    }

                    //USUARIO QUE NOTIFIÇAO ඞ
                    correos.Add(_context.tblP_Usuario.FirstOrDefault(e => e.id == vSesiones.sesionUsuarioDTO.id).correo);

                    //RESTO DE NOTIFICANTES
                    //correos.AddRange(new List<string> 
                    //    { 
                    //    "auxnominas.hmo@taxandlegal.com.mx",
                    //    "aux.seguridadsocialhmo@taxandlegal.com.mx",
                    //    "auxoperacionfiscal.hmo@taxandlegal.com.mx",
                    //    "seguridadsocial.hmo@taxandlegal.com.mx",
                    //    "seguridadsocialhmo.taxandlegal@gmail.com",
                    //    "nominas.hmo@taxandlegal.com.mx",
                    //    "despacho@construplan.com.mx",
                    //    "f.coronado@construplan.com.mx",
                    //    "emanuel.montes@construplan.com.mx",
                    //    "gloria.mariscal@construplan.com.mx"
                    //    }
                    //);

                    correos.AddRange(new List<string> 
                        {
                        //"pablo.clamont@construplan.com.mx",
                        "diego.cardenas@construplan.com.mx",
                        "m.cruz@construplan.com.mx",
                        "emanuel.montes@construplan.com.mx",
                        "wilfredo.willis@construplan.com.mx",
                        "aux.seguridadsocialhmo@taxandlegal.com.mx",
                        "auxseguridadsocial.hmo@taxandlegal.com.mx",
                        "seguridadsocial.hmo@taxandlegal.com.mx",
                        "seguridadsocialhmo.taxandlegal@gmail.com",
                        "despacho@construplan.com.mx",
                        }
                    );

#if DEBUG
                    correos = new List<string>() { "miguel.buzani@construplan.com.mx" };
#endif

                    var asunto = string.Format(@"INCAPACIDAD CC " + objIncap[0].ccDesc + " RP " + objIncap[0].nombre_corto);
                    var mensaje = string.Format(@"La siguiente persona ha presentado incapacidad:<br><br>

                    " + objIncap[0].clave_empleado + " " + objIncap[0].nombreCompleto + " del CC: " + objIncap[0].ccDesc + ". NSS:" + objIncap[0].nss + "<br><br>" +
                    "CODIGO: " + objIncap[0].codigoIncap + ". Fecha de Inicio:" + objIncap[0].fechaInicio.ToString("dd/MM/yyyy") + ". Fecha de Terminacion:" + objIncap[0].fechaTerminacion.ToString("dd/MM/yyyy") + ".");

                    if (string.IsNullOrEmpty(asunto))
                        asunto = "EMPTY";

                    if (string.IsNullOrEmpty(mensaje))
                        mensaje = "EMPTY";

                    GlobalUtils.sendMailWithFilesReclutamientos(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), mensaje, correos, lstArchivos);
                    dbTransac.Commit();

                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbTransac.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }


            return resultado;
        }

        public Dictionary<string, object> GuardarArchivo(int id_incap, int tipoArchivo, HttpPostedFileBase archivo)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var objTipoArchivo = _context.tblRH_Vacaciones_TipoArchivo.FirstOrDefault(e => e.id == tipoArchivo);
                    var objIncap = _context.tblRH_Vacaciones_Incapacidades.FirstOrDefault(e => e.id == id_incap);

                    var RutaBase = "";
#if DEBUG
                    RutaBase = RutaArchivosLocal;
#else
                    RutaBase = RutaArchivos;
#endif

                    var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
                    var rutaCarpetaEmpleado = Path.Combine(RutaBase, objIncap.clave_empleado.ToString());

                    verificarExisteCarpeta(rutaCarpetaEmpleado, true);

                    //archivoRegistroSIGOPLAN.descripcion = archivoRegistroSIGOPLAN.descripcion.Replace("*", string.Empty);
                    var rutaArchivo = ObtenerRutaArchivo(Path.Combine(rutaCarpetaEmpleado, ObtenerFormatoNombreArchivo(" ", archivo.FileName)));

                    var nuevoRegistroArchivo = new tblRH_Vacaciones_Archivos();

                    //nuevoRegistroArchivo.expediente_id = expediente_id;
                    nuevoRegistroArchivo.idIncapacidad = objIncap.id;
                    nuevoRegistroArchivo.nombreArchivo = archivo.FileName;
                    nuevoRegistroArchivo.descripcion = objTipoArchivo.descripcion;
                    nuevoRegistroArchivo.tipoArchivo = tipoArchivo;
                    nuevoRegistroArchivo.ubicacionArchivo = rutaArchivo;
                    nuevoRegistroArchivo.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                    nuevoRegistroArchivo.fechaCreacion = DateTime.Now;
                    nuevoRegistroArchivo.idUsuarioModificacion = 0;
                    nuevoRegistroArchivo.fechaModificacion = null;
                    nuevoRegistroArchivo.esActivo = true;

                    _context.tblRH_Vacaciones_Archivos.Add(nuevoRegistroArchivo);
                    _context.SaveChanges();

                    listaRutaArchivos.Add(Tuple.Create(archivo, rutaArchivo));

                    foreach (var arc in listaRutaArchivos)
                    {
                        var guardarArchivo = GlobalUtils.SaveHTTPPostedFileValidacion(arc.Item1, arc.Item2);

                        if (guardarArchivo.Item1 == false)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Clear();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                            LogError(0, 0, "VacacionesController", "GuardarArchivo", guardarArchivo.Item2, AccionEnum.AGREGAR, 0, new { nombreArchivo = arc.Item2 });
                            return resultado;
                        }
                    }

                    dbContextTransaction.Commit();
                    SaveBitacora(3, (int)AccionEnum.AGREGAR, nuevoRegistroArchivo.id, JsonUtils.convertNetObjectToJson(new { idIncapacidad = objIncap.id, tipoArchivo = tipoArchivo }));
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, "VacacionesController", "GuardarArchivo", e, AccionEnum.AGREGAR, 0, new { clave_empleado = id_incap, tipoArchivo = tipoArchivo });
                }
            }

            return resultado;
        }

        private static bool verificarExisteCarpeta(string path, bool crear = false)
        {
            bool existe = false;
            try
            {
                existe = Directory.Exists(path);
                if (!existe && crear)
                {
                    Directory.CreateDirectory(path);
                    existe = true;
                }
            }
            catch (Exception e)
            {
                existe = false;
            }

            return existe;
        }

        private string ObtenerRutaArchivo(string ruta)
        {
            if (File.Exists(ruta))
            {
                int count = 1;

                string fileNameOnly = Path.GetFileNameWithoutExtension(ruta);
                string extension = Path.GetExtension(ruta);
                string path = Path.GetDirectoryName(ruta);
                string newFullPath = ruta;

                while (File.Exists(newFullPath))
                {
                    string tempFileName = string.Format("{0} ({1})", fileNameOnly, count++);
                    newFullPath = Path.Combine(path, tempFileName + extension);
                }

                ruta = newFullPath;
            }

            return ruta;
        }

        private string ObtenerFormatoNombreArchivo(string nombreBase, string fileName)
        {
            return String.Format("{0} {1}{2}", Path.GetFileNameWithoutExtension(fileName).Replace("/", "-"), DateTime.Now.ToString("dd-MM-y HH:mm:ss").Replace(":", "-"), Path.GetExtension(fileName));
        }

        public tblRH_Vacaciones_Archivos GetArchivo(int id_archivo)
        {
            var objArchivo = _context.tblRH_Vacaciones_Archivos.FirstOrDefault(e => e.id == id_archivo);
            return objArchivo;
        }

        public Dictionary<string, object> GetIncapacidadesVencer()
        {
            resultado.Clear();
            try
            {
                var tresDiasAntes = DateTime.Now.AddDays(3);
                var today = DateTime.Now.Date;

                var lstIncapsVencer = GetIncapacidades(null, new List<string>(), null, null, null, null)["items"] as List<IncapacidadesDTO>;
                resultado.Clear();
                var lstFiltIncapsVencer = lstIncapsVencer.Where(e => e.fechaTerminacion >= today && e.fechaTerminacion <= tresDiasAntes).ToList();
                var lstIncapsVencidos = lstIncapsVencer.Where(e => e.fechaTerminacion >= today).ToList();

                var lstFiltIncapsVencidos = new List<IncapacidadesDTO>();

                foreach (var item in lstIncapsVencidos)
                {
                    if (item.fechaTerminacion.Date == today)
                    {
                        item.esVencida = true;
                        lstFiltIncapsVencidos.Add(item);
                    }
                    else
                    {
                        item.esVencida = false;
                        item.diasVencer = (item.fechaTerminacion - today).Days;
                    }
                }

                var objOfi = _context.tblRH_REC_Notificantes_Altas.FirstOrDefault(e => e.idUsuario == vSesiones.sesionUsuarioDTO.id);

                resultado.Add(ITEMS, lstFiltIncapsVencer);
                resultado.Add("lstVencidos", lstFiltIncapsVencidos);
                resultado.Add("esOfi", objOfi == null ? false : true);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        #endregion

        #region DASHBOARD INCAPS
        public Dictionary<string, object> GetDashboard(List<string> ccs, DateTime? fechaInicio, DateTime? fechaFin)
        {
            resultado.Clear();
            try
            {
                if (ccs == null)
                    ccs = FillComboCC().Select(e => e.Value).ToList();
                else
                {
                    if (ccs.Count() == 0)
                        ccs = FillComboCC().Select(e => e.Value).ToList();
                }

                //var lstIncapacidades = GetIncapacidades(null, ccs, fechaInicio, fechaFin, null, null)["items"] as List<IncapacidadesDTO>;
                //(⓿_⓿)(⓿_⓿)(⓿_⓿)(⓿_⓿)(⓿_⓿)(⓿_⓿)(⓿_⓿)(⓿_⓿)(⓿_⓿)(⓿_⓿)(⓿_⓿)(⓿_⓿)(⓿_⓿) DOBLE GROUP BY MANIACO

                string lstCC = string.Empty;
                foreach (var item in ccs)
                {
                    if (string.IsNullOrEmpty(lstCC))
                        lstCC = string.Format("'{0}'", item);
                    else
                        lstCC += string.Format(", '{0}'", item);
                }

                #region SE OBTIENE LISTADO DE INCAPACIDAD AGRUPADAS POR EMPLEADOS
                MainContextEnum objEmpresa = GetEmpresaLogueada();
                string strQuery = string.Format(@"SELECT tIncap.tipoIncap, tIncap.clave_empleado, tRegPat.nombre_corto, COUNT(tIncap.clave_empleado) AS CantEmpleadosIncapacidades, 
                                                         tIncap.fechaInicio, tIncap.fechaTerminacion
	                                                        FROM tblRH_Vacaciones_Incapacidades tIncap
	                                                        INNER JOIN tblRH_EK_Empleados as tEmpl ON tIncap.clave_empleado = tEmpl.clave_empleado
	                                                        INNER JOIN tblRH_EK_Registros_Patronales as tRegPat ON tEmpl.id_regpat = tRegPat.clave_reg_pat
	                                                        INNER JOIN tblC_Nom_CatalogoCC as tCC ON tIncap.cc = tCC.cc
	                                                        INNER JOIN tblRH_EK_Puestos as tPuesto ON tEmpl.puesto = tPuesto.puesto
		                                                        WHERE tIncap.esActivo = {0} AND tIncap.cc IN ({1})
			                                                       GROUP BY tIncap.tipoIncap, tIncap.clave_empleado, tRegPat.nombre_corto, tIncap.clave_empleado, tIncap.fechaInicio, tIncap.fechaTerminacion
						                                                ORDER BY clave_empleado", 1, lstCC);
                List<IncapacidadesDTO> lstIncapacidades = _context.Select<IncapacidadesDTO>(new DapperDTO
                {
                    baseDatos = objEmpresa,
                    consulta = strQuery
                }).ToList();

                if (fechaInicio != null)
                    lstIncapacidades = lstIncapacidades.Where(w => w.fechaInicio >= fechaInicio).ToList();

                if (fechaFin != null)
                    lstIncapacidades = lstIncapacidades.Where(w => w.fechaTerminacion <= fechaFin).ToList();

                foreach (var item in lstIncapacidades)
                {
                    if (item.tipoIncap >= 0)
                        item.descIncap = EnumHelper.GetDescription((IncapacidadesEnum)item.tipoIncap);
                }
                #endregion

                List<ReporteIncapacidadesDTO> lstGrpIncaps = lstIncapacidades.GroupBy(e => e.descIncap).Select(e =>
                    new ReporteIncapacidadesDTO
                    {
                        concepto = e.Key,
                        total = e.Count(),
                        lstIncapsDet = e.GroupBy(el => el.nombre_corto).Select(el => new ReporteIncapacidadesDetDTO
                        {
                            conceptoDet = el.Key.Replace(" ", string.Empty),
                            cantidadDet = el.Count(),
                        }).ToList(),
                    }).ToList();

                resultado = new Dictionary<string, object>();
                resultado.Add(ITEMS, lstGrpIncaps);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        private MainContextEnum GetEmpresaLogueada()
        {
            MainContextEnum objEmpresa = new MainContextEnum();
            try
            {
                #region SE OBTIENE MainContextEnum DE LA EMPRESA LOGUEADA
                switch ((int)vSesiones.sesionEmpresaActual)
                {
                    case (int)EmpresaEnum.Construplan:
                        objEmpresa = MainContextEnum.Construplan;
                        break;
                    case (int)EmpresaEnum.Arrendadora:
                        objEmpresa = MainContextEnum.Arrendadora;
                        break;
                    case (int)EmpresaEnum.Colombia:
                        objEmpresa = MainContextEnum.Colombia;
                        break;
                    case (int)EmpresaEnum.Peru:
                        objEmpresa = MainContextEnum.PERU;
                        break;
                    default:
                        break;
                }
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetEmpresaLogueada", e, AccionEnum.CONSULTA, (int)vSesiones.sesionEmpresaActual, new { idEmpresa = (int)vSesiones.sesionEmpresaActual });
                return objEmpresa;
            }
            return objEmpresa;
        }
        #endregion

        #region SALDOS
        public Dictionary<string, object> GetSaldos(FiltroSaldosDTO objFiltro)
        {
            resultado.Clear();

            try
            {
                //var objRow = new List<string[]>();
                var lstSaldosDTO = new List<SaldosAnualesDTO>();
                var lstCCs = _context.tblC_Nom_CatalogoCC.Where(e => e.estatus).ToList();

                var lstVacaciones = _context.tblRH_Vacaciones_Vacaciones.Where(e => e.registroActivo
                    && (!string.IsNullOrEmpty(objFiltro.cc) ? objFiltro.cc == e.cc : true)
                    && e.tipoVacaciones == 7
                    && e.estado == 1 // autorizadas
                    ).ToList();

                var lstEmpleados = _context.tblRH_EK_Empleados.
                    Where(e => e.esActivo && e.cc_contable != "000"
                        && (!string.IsNullOrEmpty(objFiltro.estatusEmpleado) ? e.estatus_empleado == objFiltro.estatusEmpleado : (e.estatus_empleado == "A" || e.estatus_empleado == "B"))
                        && (!string.IsNullOrEmpty(objFiltro.cc) ? e.cc_contable == objFiltro.cc : true)
                        && (objFiltro.clave_empleado > 0 ? (objFiltro.clave_empleado == e.clave_empleado) : true)
                        && (!string.IsNullOrEmpty(objFiltro.nombre_empleado) ? ((e.nombre + " " + e.ape_paterno + " " + e.ape_materno).Contains(objFiltro.nombre_empleado)) : true)
                    ).
                    OrderBy(e => e.cc_contable).ThenBy
                    (e => (e.ape_paterno + " " + e.ape_materno + " " + e.nombre)).ToList();

                var lstPuestos = _context.tblRH_EK_Puestos.Where(e => e.registroActivo);

                var lstFechasEmpleado = new List<DateTime>();
                foreach (var item in lstEmpleados)
                {
                    DateTime fechaIngreso = new DateTime();
                    int dias = 0;

                    var lstVacacionesEmpl = lstVacaciones.Where(e => e.claveEmpleado == item.clave_empleado.ToString()).ToList();
                    fechaIngreso = item.fecha_antiguedad ?? item.fecha_alta.Value;
                    var objPuesto = lstPuestos.FirstOrDefault(e => e.puesto == item.puesto);
                    string descPuesto = objPuesto != null ? objPuesto.descripcion : "";

                    int años = (DateTime.Now - fechaIngreso).Days / 365;

                    //CUMPLIO AÑO COMPLETO DESPUES DEL 2022 
                    DateTime fechaAniversario = new DateTime(2022, fechaIngreso.Month, fechaIngreso.Day);
                    DateTime fechaAniversarioActual = new DateTime(DateTime.Now.Year, fechaIngreso.Month, fechaIngreso.Day);
                    DateTime fechaAniversarioPost = new DateTime(DateTime.Now.Year+1, fechaIngreso.Month, fechaIngreso.Day);
                    int añosAniversario = (DateTime.Now - fechaAniversario).Days / 365;

                    // PERIODO ACTUAL
                    #region SWITCH 1
                    switch (vSesiones.sesionEmpresaActual)
                    {
                        case 1:
                        case 2:
                        case 8:
                            #region CONSTRUPLAN/ARRE MEXICO

                            if (añosAniversario > 0)
                            {
                                #region SWITCH FECHAS

                                switch (años)
                                {

                                    case 0:
                                        dias = 0;
                                        break;

                                    case 1:
                                        dias = 12;
                                        break;

                                    case 2:
                                        dias = 14;
                                        break;

                                    case 3:
                                        dias = 16;
                                        break;

                                    case 4:
                                        dias = 18;
                                        break;

                                    case 5:
                                        dias = 20;
                                        break;
                                    case 6:
                                    case 7:
                                    case 8:
                                    case 9:
                                    case 10:
                                        dias = 22;
                                        break;
                                    case 11:
                                    case 12:
                                    case 13:
                                    case 14:
                                    case 15:
                                        dias = 24;
                                        break;
                                    case 16:
                                    case 17:
                                    case 18:
                                    case 19:
                                    case 20:
                                        dias = 26;
                                        break;
                                    case 21:
                                    case 22:
                                    case 23:
                                    case 24:
                                    case 25:
                                        dias = 28;
                                        break;
                                    case 26:
                                    case 27:
                                    case 28:
                                    case 29:
                                    case 30:
                                        dias = 30;
                                        break;
                                    case 31:
                                    case 32:
                                    case 33:
                                    case 34:
                                    case 35:
                                        dias = 32;
                                        break;
                                    case 36:
                                    case 37:
                                    case 38:
                                    case 39:
                                    case 40:
                                    case 41:
                                    case 42:
                                    case 43:
                                    case 44:
                                        dias = 34;
                                        break;
                                    default:
                                        dias = 0;
                                        break;
                                }
                                #endregion
                            }
                            else
                            {
                                dias = 0;
                            }

                            #endregion

                            break;

                        case 3:
                            //COLOMBIA
                            if (años > 0)
                            {
                                dias = 15;
                            }
                            else
                            {
                                dias = 0;
                            }

                            break;

                        case 6:
                            if (años > 0)
                            {
                                dias = 30;
                            }
                            else
                            {
                                decimal equivalDias = (decimal)((DateTime.Now - fechaIngreso).Days / 365m) * 30m;
                                dias = (int)Math.Floor(equivalDias);
                            }
                            break;
                        default:
                            dias = 0;
                            break;
                    }
                    #endregion

                    //PERIODO POSTERIOR

                    int añosPost = (fechaAniversarioActual - fechaIngreso).Days / 365;
                    decimal diaPost = 0;

                    if (añosPost <= 0)
                    {
                        añosPost = (fechaAniversarioPost - fechaIngreso).Days / 365;
                    }

                    #region SWITCH 2
                    switch (vSesiones.sesionEmpresaActual)
                    {
                        case 1:
                        case 2:
                        case 8:
                            #region CONSTRUPLAN/ARRE MEXICO
                            #region SWITCH FECHAS

                            switch (añosPost)
                            {

                                case 0:
                                    diaPost = 0;
                                    break;

                                case 1:
                                    diaPost = 12;
                                    break;

                                case 2:
                                    diaPost = 14;
                                    break;

                                case 3:
                                    diaPost = 16;
                                    break;

                                case 4:
                                    diaPost = 18;
                                    break;

                                case 5:
                                    diaPost = 20;
                                    break;
                                case 6:
                                case 7:
                                case 8:
                                case 9:
                                case 10:
                                    diaPost = 22;
                                    break;
                                case 11:
                                case 12:
                                case 13:
                                case 14:
                                case 15:
                                    diaPost = 24;
                                    break;
                                case 16:
                                case 17:
                                case 18:
                                case 19:
                                case 20:
                                    diaPost = 26;
                                    break;
                                case 21:
                                case 22:
                                case 23:
                                case 24:
                                case 25:
                                    diaPost = 28;
                                    break;
                                case 26:
                                case 27:
                                case 28:
                                case 29:
                                case 30:
                                    diaPost = 30;
                                    break;
                                case 31:
                                case 32:
                                case 33:
                                case 34:
                                case 35:
                                case 36:
                                case 37:
                                case 38:
                                case 39:
                                case 40:
                                case 41:
                                case 42:
                                case 43:
                                case 44:
                                    dias = 32;
                                    break;

                                default:
                                    diaPost = 0;
                                    break;
                            }
                            #endregion


                            #endregion

                            break;

                        case 3:
                            //COLOMBIA
                            if (años > 0)
                            {
                                diaPost = 15;
                            }
                            else
                            {
                                diaPost = 0;
                            }

                            break;

                        case 6:
                            if (años > 0)
                            {
                                diaPost = 30;
                            }
                            else
                            {
                                decimal equivalDias = (decimal)((DateTime.Now - fechaIngreso).Days / 365m) * 30m;
                                diaPost = (int)Math.Floor(equivalDias);
                            }
                            break;
                        default:
                            diaPost = 0;
                            break;
                    }
                    #endregion

                    var lstSaldosPeriodoAnt = _context.tblRH_Vacaciones_Saldos.Where(e => e.registroActivo && e.clave_empleado == item.clave_empleado).ToList().Where(e => fechaAniversario.Date < e.fechaCreacion.Date && fechaAniversarioActual >= e.fechaCreacion.Date).ToList();
                    var lstSaldosPeriodoActual = _context.tblRH_Vacaciones_Saldos.Where(e => e.registroActivo && e.clave_empleado == item.clave_empleado).ToList().Where(e => fechaAniversarioActual.Date < e.fechaCreacion.Date).ToList();
                    int saldoAdicionalAnt = lstSaldosPeriodoAnt.Sum(e => e.num_dias);
                    int saldoAdicionalActual = lstSaldosPeriodoActual.Sum(e => e.num_dias);

                    var lstIdsVacacionesEmpleado = lstVacacionesEmpl.Select(e => e.id).ToList();

                    var lstDiasPeriodoAnt = _context.tblRH_Vacaciones_Fechas.Where(e => e.registroActivo && lstIdsVacacionesEmpleado.Contains(e.vacacionID)).ToList().Where(e =>
                        e.fecha.Value.Date >= fechaAniversario.Date && e.fecha.Value.Date < fechaAniversarioActual.Date).OrderBy(e => e.fecha.Value).ToList();
                    var lstDiasPeriodoActual = _context.tblRH_Vacaciones_Fechas.Where(e => e.registroActivo && lstIdsVacacionesEmpleado.Contains(e.vacacionID)).ToList().Where(e =>
                        e.fecha.Value.Date >= fechaAniversarioActual.Date && e.fecha.Value.Date < fechaAniversarioPost.Date).OrderBy(e => e.fecha.Value).ToList();

                    //SI LA PERSONA TODAVIA NO CUMPLE ANIVERSARIO(PASADO EL 2023) Y HACER EL SALDO EL NUMERO DE DIAS POR LEY DEL PERIODO PASADO
                    if (DateTime.Now.Date < fechaAniversarioActual.Date)
                    {
                        saldoAdicionalAnt = dias + saldoAdicionalAnt;                        
                    }

                    int diasPendientesGozarAnt = saldoAdicionalAnt - lstDiasPeriodoAnt.Count();
                    int diasPendientesGozarActual = lstDiasPeriodoActual.Count + (saldoAdicionalActual == 0 ? 0 : (saldoAdicionalActual * -1));

                    var objCC = lstCCs.FirstOrDefault(e => e.cc == item.cc_contable);

                    //HISTORIAL DE TABULADORES
                    var lstHistTabuladores = _context.tblRH_EK_Tabulador_Historial.Where(e => e.esActivo && e.clave_empleado == item.clave_empleado)
                        .ToList().Where(e => e.fecha_cambio.Date < fechaAniversarioPost.Date).OrderByDescending(e => e.fecha_cambio).ToList();
                    var objTabuladorPeriodo = lstHistTabuladores.FirstOrDefault();
                    decimal salarioDiario = 0M;

                    if (objTabuladorPeriodo != null)
                    {
                        if (item.tipo_nomina == 1)
                        {
                            //SEMANAL
                            salarioDiario = ((objTabuladorPeriodo.salario_base + objTabuladorPeriodo.complemento + objTabuladorPeriodo.bono_zona) / 7);
                        }
                        else
                        {
                            //QUINCEANAL
                            salarioDiario = ((objTabuladorPeriodo.salario_base + objTabuladorPeriodo.complemento + objTabuladorPeriodo.bono_zona) / 15);
                        }
                    }
                    else
                    {
                        if (item.salario_base != null)
                        {
                            if (item.tipo_nomina == 1)
                            {
                                //SEMANAL
                                salarioDiario = ((item.salario_base.Value + item.complemento.Value + item.bono_zona.Value) / 7);
                            }
                            else
                            {
                                //QUINCEANAL
                                salarioDiario = ((item.salario_base.Value + item.complemento.Value + item.bono_zona.Value) / 15);
                            }
                        }
                    }

                    //LAYOUT MESES 
                    var mesesActual = new List<int>();

                    var tempFechaIniSize = fechaAniversarioActual.Date;

                    for (DateTime i = tempFechaIniSize; i < fechaAniversarioPost.Date; i = i.AddMonths(1))
                    {
                        mesesActual.Add(i.Month);
                    }

                    //PROPORCIONAL MES
                    decimal ratioDiasPorMes = (dias / 12);
                    decimal ratioDiasPorMesPost = (diaPost / 12);

                    int indexOfMesActual = mesesActual.IndexOf(DateTime.Now.Month);

                    //if (indexOfMesActual > 1)
                    //{
                    //    indexOfMesActual--;
                    //}

                    decimal diasActual = (ratioDiasPorMes * indexOfMesActual);
                    decimal diasProximo = (ratioDiasPorMesPost * indexOfMesActual);

                    //int numDias = DateTime.IsLeapYear(DateTime.Now.Year) ? 366 : 365;
                    //int numDiaByYear = DateTime.Now.DayOfYear;

                    var objSaldo = new SaldosAnualesDTO();
                    objSaldo.clave_empleado = item.clave_empleado;
                    objSaldo.nombre_completo = (item.ape_paterno + " " + item.ape_materno + " " + item.nombre);
                    objSaldo.cc = objCC != null ? ("[" + objCC.cc + "] " + objCC.ccDescripcion) : "";
                    objSaldo.fecha_alta = (fechaIngreso);
                    objSaldo.años_servicio = (años);
                    objSaldo.dias_ganadosActual = dias;
                    objSaldo.dias_difrutadosActual = dias > 0 ? (diasPendientesGozarActual) : (lstDiasPeriodoAnt.Count());
                    objSaldo.dias_pendientesGozarActual = dias > 0 ? (dias - diasPendientesGozarActual) : (diasPendientesGozarAnt);
                    objSaldo.dias_proporcionalProximo = diasProximo;
                    objSaldo.dias_totalPendientesGozarProximo = objSaldo.dias_pendientesGozarActual + diasProximo;
                    objSaldo.salario_diario = salarioDiario;
                    objSaldo.vacaciones = objSaldo.dias_totalPendientesGozarProximo * salarioDiario;
                    objSaldo.prima_vacacionalProporcional = (diasProximo * salarioDiario) * .25M;
                    objSaldo.prima_vacacional = (diaPost * salarioDiario) * .25M;
                    objSaldo.estatusEmpleado = item.estatus_empleado;

                    objSaldo.puesto = item.puesto.Value;

                    lstSaldosDTO.Add(objSaldo);
                }

                resultado.Add(ITEMS, lstSaldosDTO);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> GetSaldosDet(int clave_empleado)
        {
            resultado.Clear();

            try
            {
                var lstSaldos = _context.tblRH_Vacaciones_Saldos.Where(e => e.registroActivo && e.clave_empleado == clave_empleado);

                resultado.Add(ITEMS, lstSaldos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> CrearEditarSaldo(SaldosDTO objFiltro)
        {
            resultado.Clear();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var objSaldo = _context.tblRH_Vacaciones_Saldos.Add(new tblRH_Vacaciones_Saldos()
                    {
                        clave_empleado = objFiltro.clave_empleado,
                        num_dias = objFiltro.num_dias,
                        FK_UsuarioCreacion = vSesiones.sesionUsuarioDTO.id,
                        FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id,
                        fechaCreacion = DateTime.Now,
                        fechaModificacion = DateTime.Now,
                        registroActivo = true,
                    });

                    _context.SaveChanges();
                    dbContextTransaction.Commit();

                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e )
                {
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> DeleteSaldoDet(int id)
        {
            resultado.Clear();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var objSaldo = _context.tblRH_Vacaciones_Saldos.FirstOrDefault(e => e.id == id);

                    objSaldo.registroActivo = false;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();

                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            
            return resultado;
        }

        public Dictionary<string, object> GetSaldosSinProporcional(FiltroSaldosDTO objFiltro)
        {
            resultado.Clear();

            try
            {
                //var objRow = new List<string[]>();
                var lstSaldosDTO = new List<SaldosExcelMensualDTO>();
                var lstCCs = _context.tblC_Nom_CatalogoCC.Where(e => e.estatus).ToList();

                var lstVacaciones = _context.tblRH_Vacaciones_Vacaciones.Where(e => e.registroActivo
                    && (!string.IsNullOrEmpty(objFiltro.cc) ? objFiltro.cc == e.cc : true)
                    && e.tipoVacaciones == 7
                    && (e.estado == 1 || e.estado == 3) // pendientes y autorizadas
                    ).ToList();

                var lstEmpleados = _context.tblRH_EK_Empleados.
                    Where(e => e.esActivo && e.estatus_empleado == "A" && (!string.IsNullOrEmpty(objFiltro.cc) ? e.cc_contable == objFiltro.cc : true)).
                    OrderBy(e => (e.ape_paterno + " " + e.ape_materno + " " + e.nombre)).ToList();

                var lstPuestos = _context.tblRH_EK_Puestos.Where(e => e.registroActivo);

                var lstFechasEmpleado = new List<DateTime>();
                foreach (var item in lstEmpleados)
                {
                    DateTime fechaIngreso = new DateTime();
                    int dias = 0;

                    var lstVacacionesEmpl = lstVacaciones.Where(e => e.claveEmpleado == item.clave_empleado.ToString()).ToList();
                    fechaIngreso = item.fecha_antiguedad ?? item.fecha_alta.Value;
                    var objPuesto = lstPuestos.FirstOrDefault(e => e.puesto == item.puesto);
                    string descPuesto = objPuesto != null ? objPuesto.descripcion : "";

                    int años = (DateTime.Now - fechaIngreso).Days / 365;

                    //CUMPLIO AÑO COMPLETO DESPUES DEL 2022
                    DateTime fechaAniversario = new DateTime(2022, fechaIngreso.Month, fechaIngreso.Day);
                    DateTime fechaAniversarioActual = new DateTime(DateTime.Now.Year, fechaIngreso.Month, fechaIngreso.Day);
                    DateTime fechaAniversarioPost = new DateTime(DateTime.Now.Year + 1, fechaIngreso.Month, fechaIngreso.Day);
                    int añosAniversario = (DateTime.Now - fechaAniversario).Days / 365;

                    // PERIODO ACTUAL
                    #region SWITCH 1
                    switch (vSesiones.sesionEmpresaActual)
                    {
                        case 1:
                        case 2:
                        case 8:
                            #region CONSTRUPLAN/ARRE MEXICO

                            if (añosAniversario > 0)
                            {
                                #region SWITCH FECHAS

                                switch (años)
                                {

                                    case 0:
                                        dias = 0;
                                        break;

                                    case 1:
                                        dias = 12;
                                        break;

                                    case 2:
                                        dias = 14;
                                        break;

                                    case 3:
                                        dias = 16;
                                        break;

                                    case 4:
                                        dias = 18;
                                        break;

                                    case 5:
                                        dias = 20;
                                        break;
                                    case 6:
                                    case 7:
                                    case 8:
                                    case 9:
                                    case 10:
                                        dias = 22;
                                        break;
                                    case 11:
                                    case 12:
                                    case 13:
                                    case 14:
                                    case 15:
                                        dias = 24;
                                        break;
                                    case 16:
                                    case 17:
                                    case 18:
                                    case 19:
                                    case 20:
                                        dias = 26;
                                        break;
                                    case 21:
                                    case 22:
                                    case 23:
                                    case 24:
                                    case 25:
                                        dias = 28;
                                        break;
                                    case 26:
                                    case 27:
                                    case 28:
                                    case 29:
                                    case 30:
                                        dias = 30;
                                        break;
                                    case 31:
                                    case 32:
                                    case 33:
                                    case 34:
                                    case 35:
                                        dias = 32;
                                        break;
                                    case 36:
                                    case 37:
                                    case 38:
                                    case 39:
                                    case 40:
                                    case 41:
                                    case 42:
                                    case 43:
                                    case 44:
                                        dias = 34;
                                        break;

                                    default:
                                        dias = 0;
                                        break;
                                }
                                #endregion
                            }
                            else
                            {
                                dias = 0;
                            }

                            #endregion

                            break;

                        case 3:
                            //COLOMBIA
                            if (años > 0)
                            {
                                dias = 15;
                            }
                            else
                            {
                                dias = 0;
                            }

                            break;

                        case 6:
                            if (años > 0)
                            {
                                dias = 30;
                            }
                            else
                            {
                                decimal equivalDias = (decimal)((DateTime.Now - fechaIngreso).Days / 365m) * 30m;
                                dias = (int)Math.Floor(equivalDias);
                            }
                            break;
                        default:
                            dias = 0;
                            break;
                    }
                    #endregion

                    //PERIODO POSTERIOR
                    int añosPost = (fechaAniversarioActual - fechaIngreso).Days / 365;
                    int diaPost = 0;

                    #region SWITCH 2
                    switch (vSesiones.sesionEmpresaActual)
                    {
                        case 1:
                        case 2:
                        case 8:
                            #region CONSTRUPLAN/ARRE MEXICO

                            if (DateTime.Now.Date < fechaAniversarioActual.Date)
                            {
                                #region SWITCH FECHAS

                                switch (añosPost)
                                {

                                    case 0:
                                        diaPost = 0;
                                        break;

                                    case 1:
                                        diaPost = 12;
                                        break;

                                    case 2:
                                        diaPost = 14;
                                        break;

                                    case 3:
                                        diaPost = 16;
                                        break;

                                    case 4:
                                        diaPost = 18;
                                        break;

                                    case 5:
                                        diaPost = 20;
                                        break;
                                    case 6:
                                    case 7:
                                    case 8:
                                    case 9:
                                    case 10:
                                        diaPost = 22;
                                        break;
                                    case 11:
                                    case 12:
                                    case 13:
                                    case 14:
                                    case 15:
                                        diaPost = 24;
                                        break;
                                    case 16:
                                    case 17:
                                    case 18:
                                    case 19:
                                    case 20:
                                        diaPost = 26;
                                        break;
                                    case 21:
                                    case 22:
                                    case 23:
                                    case 24:
                                    case 25:
                                        diaPost = 28;
                                        break;
                                    case 26:
                                    case 27:
                                    case 28:
                                    case 29:
                                    case 30:
                                        diaPost = 30;
                                        break;
                                    case 31:
                                    case 32:
                                    case 33:
                                    case 34:
                                    case 35:
                                    case 36:
                                    case 37:
                                    case 38:
                                    case 39:
                                    case 40:
                                    case 41:
                                    case 42:
                                    case 43:
                                    case 44:
                                        dias = 32;
                                        break;

                                    default:
                                        diaPost = 0;
                                        break;
                                }
                                #endregion
                            }
                            else
                            {
                                diaPost = 0;
                            }

                            #endregion

                            break;

                        case 3:
                            //COLOMBIA
                            if (años > 0)
                            {
                                diaPost = 15;
                            }
                            else
                            {
                                diaPost = 0;
                            }

                            break;

                        case 6:
                            if (años > 0)
                            {
                                diaPost = 30;
                            }
                            else
                            {
                                decimal equivalDias = (decimal)((DateTime.Now - fechaIngreso).Days / 365m) * 30m;
                                diaPost = (int)Math.Floor(equivalDias);
                            }
                            break;
                        default:
                            diaPost = 0;
                            break;
                    }
                    #endregion

                    var lstSaldosPeriodoAnt = _context.tblRH_Vacaciones_Saldos.Where(e => e.registroActivo && e.clave_empleado == item.clave_empleado).ToList().Where(e => fechaAniversario.Date < e.fechaCreacion.Date && fechaAniversarioActual >= e.fechaCreacion.Date).ToList();
                    var lstSaldosPeriodoActual = _context.tblRH_Vacaciones_Saldos.Where(e => e.registroActivo && e.clave_empleado == item.clave_empleado).ToList().Where(e => fechaAniversarioActual.Date < e.fechaCreacion.Date).ToList();
                    int saldoAdicionalAnt = lstSaldosPeriodoAnt.Sum(e => e.num_dias);
                    int saldoAdicionalActual = lstSaldosPeriodoActual.Sum(e => e.num_dias);

                    //SI LA PERSONA TODAVIA NO CUMPLE ANIVERSARIO(PASADO EL 2023) Y HACER EL SALDO EL NUMERO DE DIAS POR LEY DEL PERIODO PASADO
                    if (DateTime.Now.Date < fechaAniversarioActual.Date)
                    {
                        saldoAdicionalAnt = dias + saldoAdicionalAnt;
                    }

                    int diasPendientesGozarAnt = saldoAdicionalAnt ;
                    int diasPendientesGozarActual = (saldoAdicionalActual == 0 ? 0 : (saldoAdicionalActual * -1));

                    var objCC = lstCCs.FirstOrDefault(e => e.cc == item.cc_contable);

                    //HISTORIAL DE TABULADORES
                    var lstHistTabuladores = _context.tblRH_EK_Tabulador_Historial.Where(e => e.esActivo && e.clave_empleado == item.clave_empleado)
                        .ToList().Where(e => e.fecha_cambio.Date < fechaAniversarioPost.Date).OrderByDescending(e => e.fecha_cambio).ToList();
                    var objTabuladorPeriodo = lstHistTabuladores.FirstOrDefault();
                    decimal salarioDiario = 0M;

                    if (objTabuladorPeriodo != null)
                    {
                        if (item.tipo_nomina == 1)
                        {
                            //SEMANAL
                            salarioDiario = ((objTabuladorPeriodo.salario_base + objTabuladorPeriodo.complemento + objTabuladorPeriodo.bono_zona) / 7);
                        }
                        else
                        {
                            //QUINCEANAL
                            salarioDiario = ((objTabuladorPeriodo.salario_base + objTabuladorPeriodo.complemento + objTabuladorPeriodo.bono_zona) / 15);
                        }
                    }
                    else
                    {
                        if (item.salario_base != null)
                        {
                            if (item.tipo_nomina == 1)
                            {
                                //SEMANAL
                                salarioDiario = ((item.salario_base.Value + item.complemento.Value + item.bono_zona.Value) / 7);
                            }
                            else
                            {
                                //QUINCEANAL
                                salarioDiario = ((item.salario_base.Value + item.complemento.Value + item.bono_zona.Value) / 15);
                            }
                        }
                    }

                    var objSaldo = new SaldosExcelMensualDTO();

                    objSaldo.clave_empleado = item.clave_empleado;
                    objSaldo.nombre_completo = (item.ape_paterno + " " + item.ape_materno + " " + item.nombre);
                    objSaldo.cc = objCC != null ? ("[" + objCC.cc + "] " + objCC.ccDescripcion) : "";
                    objSaldo.fecha_alta = (fechaIngreso);
                    objSaldo.años_servicio = (años);
                    objSaldo.dias_pendientesGozarAnt = dias > 0 ? 0 : diasPendientesGozarAnt;
                    objSaldo.dias_periodoActual = dias;
                    objSaldo.dias_disfrutados = diasPendientesGozarActual;
                    objSaldo.dias_pendienteGozar = (dias - diasPendientesGozarActual);
                    objSaldo.dias_totalPendientesGozar = (objSaldo.dias_pendienteGozar + objSaldo.dias_pendientesGozarAnt);
                    objSaldo.salario_diario = salarioDiario;
                    objSaldo.vacaciones = (salarioDiario * (dias == 0 ? diaPost : objSaldo.dias_pendienteGozar));
                    objSaldo.prima_vacacional = (((dias == 0 ? diaPost : dias) * salarioDiario) * .25M);
                    objSaldo.puesto = item.puesto.Value;

                    lstSaldosDTO.Add(objSaldo);
                }

                resultado.Add(ITEMS, lstSaldosDTO);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        #endregion

        #region REPORTE DIAS
        public Dictionary<string, object> GetVacacionesReporte(VacacionesDTO objFiltro)
        {
            resultado.Clear();

            try
            {
                var lstEmpleados = _context.tblRH_EK_Empleados.Where(e => e.esActivo && e.estatus_empleado == "A" && e.cc_contable == objFiltro.cc).OrderBy(e => (e.ape_paterno + " " + e.ape_materno + " " + e.nombre)).ToList();
                var lstVacaciones = _context.tblRH_Vacaciones_Vacaciones.Where(e => e.registroActivo
                    && (!string.IsNullOrEmpty(objFiltro.cc) ? objFiltro.cc == e.cc : true)
                    && (objFiltro.tipoVacaciones > 0 ? objFiltro.tipoVacaciones == e.tipoVacaciones : true)
                    && e.estado == 1 // autorizadas
                    ).ToList();

                int numDias = (objFiltro.fechaFinal.Value.Date - objFiltro.fechaInicial.Value.Date).Days;

                var columnasBase = new List<DateTime>();
                var listaColumnas = new List<Tuple<string, string>>();

                for (DateTime fechaIni = objFiltro.fechaInicial.Value.Date; fechaIni <= objFiltro.fechaFinal.Value.Date; fechaIni = fechaIni.AddDays(1))
                {
                    columnasBase.Add(fechaIni);
                    listaColumnas.Add(new Tuple<string, string>(
                            "columnaDia" + fechaIni.ToString("ddMMyyyy"), //data
                            fechaIni.ToString("dd/MM/yyyy") //title
                        ));
                }

                var data = new List<Dictionary<string, object>>();
                foreach (var item in lstEmpleados)
                {
                    var lstVacacionesEmpleado = lstVacaciones.Where(e => e.claveEmpleado == item.clave_empleado.ToString()).Select(e => e.id).ToList();

                    var renglonDatatable = new Dictionary<string, object>();
                    var lstDias = _context.tblRH_Vacaciones_Fechas.Where(e => e.registroActivo && lstVacacionesEmpleado.Contains(e.vacacionID)).ToList().Where(e =>
                        e.fecha.Value.Date >= objFiltro.fechaInicial.Value.Date && e.fecha.Value.Date <= objFiltro.fechaFinal).OrderBy(e => e.fecha.Value).ToList();

                    if (lstDias.Count > 0)
                    {
                        renglonDatatable.Add("id", item.id);
                        renglonDatatable.Add("claveEmpleado", item.clave_empleado);
                        renglonDatatable.Add("empleadoDesc", (item.ape_paterno + " " + item.ape_materno + " " + item.nombre));

                        foreach (var day in columnasBase)
                        {
                            var objFechas = lstDias.FirstOrDefault(e => e.fecha.Value.Date == day);
                            if (objFechas != null)
                            {
                                renglonDatatable.Add("columnaDia" + day.ToString("ddMMyyyy"), new { estatus = true });
                            }
                            else
                            {
                                renglonDatatable.Add("columnaDia" + day.ToString("ddMMyyyy"), new { estatus = false });
                            }
                        }
                        data.Add(renglonDatatable);
                    }

                }

                resultado.Add("rows", data);
                resultado.Add("cols", listaColumnas);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
                
            }

            return resultado;
        }

        public List<string[]> GetExcelReporte(VacacionesDTO objFiltro)
        {
            try
            {
                var objRow = new List<string[]>();
                var lstPuestos = _context.tblRH_EK_Puestos.Where(e => e.registroActivo);

                var lstVacaciones = _context.tblRH_Vacaciones_Vacaciones.Where(e => e.registroActivo
                    && (!string.IsNullOrEmpty(objFiltro.cc) ? objFiltro.cc == e.cc : true)
                    && e.tipoVacaciones == 7
                    && e.estado == 1 // autorizadas
                    ).ToList();

                int numMonts = 0;

                var tempFechaIniSize = objFiltro.fechaInicial.Value;

                for (DateTime i = tempFechaIniSize; i < objFiltro.fechaFinal.Value; i = i.AddMonths(1))
                {
                    numMonts++;
                }

                int arrSize = 12 + numMonts;

                #region DATOS
                var dictRepSaldo = GetSaldosSinProporcional(new FiltroSaldosDTO() { cc = objFiltro.cc });
                var lstEmpleados = dictRepSaldo["items"] as List<SaldosExcelMensualDTO>;

                foreach (var item in lstEmpleados)
                {
                    var objPuesto = lstPuestos.FirstOrDefault(e => e.puesto == item.puesto);
                    string descPuesto = "";

                    if (objPuesto != null)
	                {
		                descPuesto = objPuesto.descripcion;
	                }

                    string[] rowArr = new string[arrSize];

                    rowArr[0] = (item.cc.ToString());
                    rowArr[1] = (item.clave_empleado.ToString());
                    rowArr[2] = (item.nombre_completo);
                    rowArr[3] = (descPuesto);
                    rowArr[4] = (item.fecha_alta.ToString("dd/MM/yyyy"));
                    rowArr[5] = (item.años_servicio.ToString());
                    rowArr[6] = (item.dias_pendientesGozarAnt.ToString());
                    rowArr[7] = item.dias_periodoActual.ToString();
                    rowArr[8] = (item.dias_totalPendientesGozar).ToString();
                    rowArr[9] = "FECHAS";

                    var lstVacacionesEmpl = lstVacaciones.Where(e => e.claveEmpleado == item.clave_empleado.ToString()).ToList();

                    var lstIdsVacacionesEmpleado = lstVacacionesEmpl.Select(e => e.id).ToList();
                    var lstDias = _context.tblRH_Vacaciones_Fechas.Where(e => e.registroActivo && lstIdsVacacionesEmpleado.Contains(e.vacacionID)).ToList().Where(e =>
                        e.fecha.Value.Date >= objFiltro.fechaInicial.Value.Date && e.fecha.Value.Date <= objFiltro.fechaFinal).OrderBy(e => e.fecha.Value).ToList();
                    decimal diasPendientesGozar = item.dias_totalPendientesGozar - lstDias.Count();

                    var tempFechaIniBody = objFiltro.fechaInicial.Value;
                    int arrRowIndx = 10;

                    for (DateTime i = tempFechaIniBody; i < objFiltro.fechaFinal.Value; i = i.AddMonths(1))
                    {
                        var lstFechasMonth = lstDias.Where(e => e.fecha.Value.Month == i.Month).OrderBy(e => e.fecha).ToList().Select(e => e.fecha.Value.ToString("dd")).ToList();

                        string descDiasMes = lstFechasMonth.Count > 0 ? string.Join(", ", lstFechasMonth) : "";

                        rowArr[arrRowIndx] = descDiasMes;
                        arrRowIndx++;
                    }

                    rowArr[arrSize - 2] = lstDias.Count().ToString();
                    rowArr[arrSize - 1] = (diasPendientesGozar).ToString();

                    objRow.Add(rowArr);
                }
                return objRow;
                #endregion
            }
            catch (Exception e)
            {
                return null;
            }
        }
        #endregion

        #region DASHBOARD
        public Dictionary<string, object> GetDashboardVacaciones(VacacionesDTO objFiltro)
        {
            resultado.Clear();

            try
            {
                var lstCCs = _context.tblP_CC.ToList();
                var lstVacacionesByCC = _context.tblRH_Vacaciones_Vacaciones.Where(e => e.registroActivo
                    && (objFiltro.tipoVacaciones > 0 ? objFiltro.tipoVacaciones == e.tipoVacaciones : true)
                    && e.estado == 1 // autorizadas
                    ).ToList().Where(e => (objFiltro.lstFiltroCC != null && objFiltro.lstFiltroCC.Count() > 0 ? objFiltro.lstFiltroCC.Contains(e.cc) : true)
                    ).GroupBy(e => e.cc).Select(e => new { e.Key, vacaciones = e});

                var columnasBase = new List<DateTime>();
                var lstFechasRango = new List<DateTime>();
                var listaColumnas = new List<Tuple<string, string>>();

                for (DateTime fechaIni = objFiltro.fechaInicial.Value.Date; fechaIni <= objFiltro.fechaFinal.Value.Date; fechaIni = fechaIni.AddMonths(1))
                {
                    columnasBase.Add(fechaIni);
                    listaColumnas.Add(new Tuple<string, string>(
                            "columnaMes" + fechaIni.ToString("MMyyyy"), //data
                            fechaIni.ToString("MM/yyyy") //title
                        ));
                }

                for (DateTime fechaIni = objFiltro.fechaInicial.Value.Date; fechaIni <= objFiltro.fechaFinal.Value.Date; fechaIni = fechaIni.AddDays(1))
                {
                    lstFechasRango.Add(fechaIni);
                }

                var data = new List<Dictionary<string, object>>();
                var lstFechasCalendario = new List<DashboardVacacionesDTO>();

                foreach (var item in lstVacacionesByCC)
                {
                    string ccDesc = lstCCs.FirstOrDefault(e => e.cc == item.Key).descripcion;

                    var renglonDatatable = new Dictionary<string, object>();
                    var lstIdsVacacionesCC = item.vacaciones.Select(e => e.id);
                    var lstDias = _context.tblRH_Vacaciones_Fechas.Where(e => e.registroActivo && lstIdsVacacionesCC.Contains(e.vacacionID)).ToList().Where(e =>
                        e.fecha.Value.Date >= objFiltro.fechaInicial.Value.Date && e.fecha.Value.Date <= objFiltro.fechaFinal).OrderBy(e => e.fecha.Value).ToList();

                    renglonDatatable.Add("ccDesc", ("[" + item.Key + "] " + ccDesc));

                    foreach (var day in columnasBase)
                    {
                        var lstFechas = lstDias.Where(e => e.fecha.Value.Month == day.Month && e.fecha.Value.Year == day.Year);
                        if (lstFechas != null && lstFechas.Count() > 0)
                        {
                            renglonDatatable.Add("columnaMes" + day.ToString("MMyyyy"), new { cantidad = lstFechas.Count() });
                        }
                        else
                        {
                            renglonDatatable.Add("columnaMes" + day.ToString("MMyyyy"), new { cantidad = 0 });
                        }
                    }

                    foreach (var day in lstFechasRango)
                    {
                        var lstFechas = lstDias.Where(e => e.fecha.Value.Date == day);
                        if (lstFechas != null && lstFechas.Count() > 0)
                        {
                            //renglonDatatable.Add("columnaDia" + day.ToString("ddMMyyyy"), new { estatus = true });
                            lstFechasCalendario.Add(new DashboardVacacionesDTO()
                            {
                                fecha = day,
                                cantidad = lstFechas.Count(),
                            });
                        }
                        else
                        {
                            //renglonDatatable.Add("columnaDia" + day.ToString("ddMMyyyy"), new { estatus = false });
                            lstFechasCalendario.Add(new DashboardVacacionesDTO()
                            {
                                fecha = day,
                                cantidad = lstFechas.Count(),
                            });
                        }
                    }

                    data.Add(renglonDatatable);
                }

                lstFechasCalendario = lstFechasCalendario.GroupBy(e => e.fecha).Select(e => new DashboardVacacionesDTO() { fecha = e.Key, cantidad = e.Sum(el => el.cantidad) }).ToList();

                resultado.Add("rows", data);
                resultado.Add("cols", listaColumnas);
                resultado.Add(ITEMS, lstFechasCalendario);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
                
            }

            return resultado;
        }
        #endregion

        #region RETARDOS O JUSTIFICACIONES

        public Dictionary<string, object> GetRetardos(RetardosDTO objFiltro)
        {
            resultado.Clear();

            using (var ctx = new MainContext())
            {
                try
                {
                    bool esConsulta = false;
                    var lstCC = ctx.tblC_Nom_CatalogoCC.ToList();

                    var objEsCH = ctx.tblRH_REC_Notificantes_Altas.FirstOrDefault(e => e.esActivo && e.idUsuario == vSesiones.sesionUsuarioDTO.id);
                    var lstPermisoCC = ctx.tblRH_BN_Usuario_CC.Where(e => e.usuarioID == vSesiones.sesionUsuarioDTO.id).Select(e => e.cc).ToList();
                    var esPermisoConsultaVacaciones = ctx.tblP_AccionesVistatblP_Usuario.FirstOrDefault(e => e.tblP_AccionesVista_id == 4044 && e.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id);
                    var esPermisoConsultaPermisos = ctx.tblP_AccionesVistatblP_Usuario.FirstOrDefault(e => e.tblP_AccionesVista_id == 4045 && e.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id);

                    if (vSesiones.sesionUsuarioDTO.idPerfil != 1)
                    {
                        if (objEsCH == null)
                        {
                            if (!string.IsNullOrEmpty(vSesiones.sesionUsuarioDTO.cveEmpleado))

                                objFiltro.claveEmpleado = vSesiones.sesionUsuarioDTO.cveEmpleado;
                            else
                                throw new Exception("Ocurrio algo mal al consultar las vacaciones de su usuario favor de contactarse con el depto de TI");
                        }
                    }

                    #region ES CONSULTA
                    if (esPermisoConsultaVacaciones != null)
                    {
                        lstPermisoCC = new List<string>() { "*" };
                        esConsulta = true;
                    }
                    #endregion

                    List<RetardosDTO> lstVacaciones = ctx.tblRH_Vacaciones_Retardos.Where(e => e.registroActivo
                    && (objFiltro.estado != 0 ? e.estado == objFiltro.estado : true)
                    && (objFiltro.tipoRetardo.HasValue ? e.tipoRetardo == objFiltro.tipoRetardo : true)
                    && (!string.IsNullOrEmpty(objFiltro.ccEmpleado) ? e.cc == objFiltro.ccEmpleado :
                        (!string.IsNullOrEmpty(objFiltro.claveEmpleado) || (lstPermisoCC.Contains("*") || vSesiones.sesionUsuarioDTO.idPerfil == 1) ? true :
                            lstPermisoCC.Contains(e.cc)))
                    && ((!string.IsNullOrEmpty(objFiltro.claveEmpleado) && !esConsulta) ? e.claveEmpleado == objFiltro.claveEmpleado : true)
                    ).Select(e => new RetardosDTO
                    {
                        id = e.id,
                        estado = e.estado,
                        nombreEmpleado = e.nombreEmpleado,
                        claveEmpleado = e.claveEmpleado,
                        comentarioRechazada = e.comentarioRechazada ?? "",
                        tipoRetardo = e.tipoRetardo,
                        motivoJustificacion = e.motivoJustificacion,
                        horario = e.horario,
                        horarioLower = e.horarioLower,
                        horarioUpper = e.horarioUpper,
                        tiempoRequeridoMin = e.tiempoRequeridoMin,
                        tiempoRequeridoHrs = e.tiempoRequeridoHrs,
                        cc = e.cc,
                        justificacion = e.justificacion,
                        idJefeInmediato = e.idJefeInmediato,
                        nombreJefeInmediato = e.nombreJefeInmediato,
                        rutaArchivoActa = e.rutaArchivoActa,
                        consecutivo = e.consecutivo,
                        diaTomado = e.diaTomado
                    }).ToList();

                    //public int horario { get; set; } //HACER ENUM
                    //public TimeSpan? horarioLower { get; set; }
                    //public TimeSpan? horarioUpper { get; set; } 
                    //public decimal tiempoRequeridoHrs { get; set; }
                    //public decimal tiempoRequeridoMin { get; set; }
                    //public DateTime diaTomado { get; set; }
                    //public string rutaArchivoActa { get; set; }

                    List<RetardosDTO> lstFiltrada = new List<RetardosDTO>();

                    foreach (var item in lstVacaciones)
                    {

                        var objCC = lstCC.FirstOrDefault(e => e.cc == item.cc);

                        if (objCC != null)
                        {
                            item.ccDesc = "[" + objCC.cc + "] " + objCC.ccDescripcion;
                        }
                        else
                        {
                            item.ccDesc = "S/N";
                        }

                        #region LISTA AUTH
                        var lstAuthDTO = new List<VacacionesGestionDTO>();
                        var lstAuth = _context.tblRH_Vacaciones_Retardos_Gestion.Where(e => e.registroActivo && e.idRetardo == item.id).ToList();

                        foreach (var itemAuth in lstAuth)
                        {
                            if (itemAuth.orden == OrdenGestionEnum.CAPITAL_HUMANO)
                            {
                                var objAuth = new VacacionesGestionDTO();

                                objAuth.id = itemAuth.id;
                                objAuth.idUsuario = itemAuth.idUsuario;
                                objAuth.estatus = itemAuth.estatus;
                                objAuth.orden = itemAuth.orden;
                                objAuth.nombreCompleto = "VOBO cumplimiento al reglamento";
                                objAuth.firmaElect = itemAuth.firmaElect;

                                lstAuthDTO.Add(objAuth);
                            }
                            else
                            {
                                var objAuth = new VacacionesGestionDTO();

                                var objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.id == itemAuth.idUsuario);
                                string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                                objAuth.id = itemAuth.id;
                                objAuth.idUsuario = itemAuth.idUsuario;
                                objAuth.estatus = itemAuth.estatus;
                                objAuth.orden = itemAuth.orden;
                                objAuth.nombreCompleto = nombreCompleto;
                                objAuth.firmaElect = itemAuth.firmaElect;

                                lstAuthDTO.Add(objAuth);
                            }
                        }

                        item.lstAutorizantes = lstAuthDTO;
                        #endregion

                        if (objFiltro.fechaFiltroInicio != null && objFiltro.fechaFiltroFin != null && item.diaTomado.Date >= objFiltro.fechaFiltroInicio.Value.Date && item.diaTomado.Date <= objFiltro.fechaFiltroFin.Value.Date)
                        {
                            var lstFechasVacaciones = _context.tblRH_Vacaciones_Fechas.Where(e => e.registroActivo && e.vacacionID == item.id).ToList().
                                Where(e => e.fecha.Value.Date >= objFiltro.fechaFiltroInicio.Value.Date && e.fecha.Value.Date <= objFiltro.fechaFiltroFin.Value.Date).ToList();

                            if (lstFechasVacaciones.Count() > 0)
                            {
                                lstFiltrada.Add(item);
                            }
                        }
                        else
                        {
                            lstFiltrada.Add(item);
                        }
                    }

                    bool esAdmnCH = false;
                    if (vSesiones.sesionUsuarioDTO.id == 1019 || vSesiones.sesionUsuarioDTO.id == 79552 || vSesiones.sesionUsuarioDTO.idPerfil == 1)
                    {
                        esAdmnCH = true;
                    }

                    resultado.Add(ITEMS, lstFiltrada);
                    resultado.Add(SUCCESS, true);
                    resultado.Add("claveEmpleado", objFiltro.claveEmpleado);
                    resultado.Add("esAdmnCH", esAdmnCH);
                    resultado.Add("esRegCH", objEsCH); // REGULAR
                }
                catch (Exception e)
                {
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                    
                }
            }

            return resultado;
        }

        public Dictionary<string, object> CrearEditarRetardo(RetardosDTO objRetardo)
        {
            resultado.Clear();

            using (var ctx = new MainContext())
            {
                using (var dbTransac = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        var objCERetardo = new tblRH_Vacaciones_Retardos();
                        var lstUsuarios = _context.tblP_Usuario.Where(e => e.estatus).ToList();

                        int clave_empleado = Convert.ToInt32(objRetardo.claveEmpleado);
                        var objEmpleado = _context.tblRH_EK_Empleados.FirstOrDefault(e => e.esActivo && e.estatus_empleado == "A" && e.clave_empleado == clave_empleado);

                        if (objEmpleado == null)
                        {
                            throw new Exception("El usuario a capturar debe estar en estatus de activo");
                        }

                        if (objRetardo.id > 0)
                        {
                            objCERetardo = _context.tblRH_Vacaciones_Retardos.Where(e => e.id == objRetardo.id).FirstOrDefault();

                            if (objCERetardo == null)
                                throw new Exception("Ocurrio algo mal");

                            #region EDITAR
                            objCERetardo.nombreEmpleado = objRetardo.nombreEmpleado;
                            objCERetardo.claveEmpleado = objRetardo.claveEmpleado;
                            objCERetardo.cc = objEmpleado.cc_contable;
                            objCERetardo.justificacion = objRetardo.justificacion;
                            objCERetardo.tipoRetardo = objRetardo.tipoRetardo.Value;
                            objCERetardo.motivoJustificacion = objRetardo.motivoJustificacion;
                            objCERetardo.justificacion = objRetardo.justificacion;
                            objCERetardo.horario = objRetardo.horario;
                            objCERetardo.horarioLower = objRetardo.horarioLower;
                            objCERetardo.horarioUpper = objRetardo.horarioUpper;
                            objCERetardo.tiempoRequeridoHrs = objRetardo.tiempoRequeridoHrs;
                            objCERetardo.tiempoRequeridoMin = objRetardo.tiempoRequeridoMin;
                            objCERetardo.diaTomado = objRetardo.diaTomado;
                            objCERetardo.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                            objCERetardo.fechaModificacion = DateTime.Now;


                            #region JEFE INMEDIATO

                            if (objCERetardo.idJefeInmediato != objRetardo.idJefeInmediato)
                            {
                                var objUsuario = lstUsuarios.FirstOrDefault(e => e.id == objRetardo.idJefeInmediato);

                                if (objUsuario != null)
                                {
                                    objCERetardo.nombreJefeInmediato = (objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno);
                                }
                            }

                            #endregion

                            _context.SaveChanges();

                            #region ACTUALIZAR AUTORIZANTES

                            var lstAuth = _context.tblRH_Vacaciones_Retardos_Gestion.Where(e => e.registroActivo && e.idRetardo == objRetardo.id).ToList();

                            #region RESPONSABLE CC

                            var objResponsable = lstAuth.FirstOrDefault(e => e.orden == OrdenGestionEnum.RESPONSABLE_CC);
                            var objNewResponsable = objRetardo.lstAutorizantes.FirstOrDefault(e => e.orden == OrdenGestionEnum.RESPONSABLE_CC);

                            if (objResponsable != null && objResponsable.idUsuario != objNewResponsable.idUsuario)
                            {
                                objResponsable.idUsuario = objNewResponsable.idUsuario;
                                objResponsable.estatus = GestionEstatusEnum.PENDIENTE;
                                objResponsable.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                objResponsable.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                objResponsable.fechaCreacion = DateTime.Now;
                                objResponsable.fechaModificacion = DateTime.Now;
                                _context.SaveChanges();
                            }
                            #endregion

                            #endregion

                            #endregion
                        }
                        else
                        {
                            #region CREAR
                            var vacacionesEmpleado = _context.tblRH_Vacaciones_Retardos.Where(x => x.claveEmpleado == objRetardo.claveEmpleado).ToList();
                            int consecutivo = 0;
                            if (vacacionesEmpleado.Count() > 0) consecutivo = vacacionesEmpleado.Max(x => x.consecutivo);
                            objCERetardo.consecutivo = consecutivo + 1;
                            objCERetardo.estado = 3;
                            objCERetardo.nombreEmpleado = objRetardo.nombreEmpleado;
                            objCERetardo.claveEmpleado = objRetardo.claveEmpleado;
                            //objCEVacaciones.idPeriodo = objVacaciones.idPeriodo;
                            objCERetardo.cc = objEmpleado.cc_contable;
                            objCERetardo.tipoRetardo = objRetardo.tipoRetardo.Value;
                            objCERetardo.motivoJustificacion = objRetardo.motivoJustificacion;
                            objCERetardo.justificacion = objRetardo.justificacion;
                            objCERetardo.horario = objRetardo.horario;
                            objCERetardo.horarioLower = objRetardo.horarioLower;
                            objCERetardo.horarioUpper = objRetardo.horarioUpper;
                            objCERetardo.tiempoRequeridoHrs = objRetardo.tiempoRequeridoHrs;
                            objCERetardo.tiempoRequeridoMin = objRetardo.tiempoRequeridoMin;
                            objCERetardo.diaTomado = objRetardo.diaTomado;
                            objCERetardo.rutaArchivoActa = objRetardo.rutaArchivoActa;
                            objCERetardo.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                            objCERetardo.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;

                            objCERetardo.idJefeInmediato = objRetardo.idJefeInmediato;
                            objCERetardo.nombreJefeInmediato = objRetardo.nombreJefeInmediato;
                            objCERetardo.fechaCreacion = DateTime.Now;
                            objCERetardo.fechaModificacion = DateTime.Now;
                            objCERetardo.registroActivo = true;
                            _context.tblRH_Vacaciones_Retardos.Add(objCERetardo);
                            _context.SaveChanges();

                            foreach (var item in objRetardo.lstAutorizantes)
                            {
                                var objAuth = new tblRH_Vacaciones_Retardos_Gestion();

                                objAuth.idRetardo = objCERetardo.id;
                                objAuth.idUsuario = item.idUsuario;
                                objAuth.orden = item.orden;
                                objAuth.estatus = GestionEstatusEnum.PENDIENTE;
                                objAuth.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                objAuth.idUsuarioModificacion = null;
                                objAuth.fechaCreacion = DateTime.Now;
                                objAuth.fechaModificacion = null;
                                objAuth.registroActivo = true;

                                _context.tblRH_Vacaciones_Retardos_Gestion.Add(objAuth);
                                _context.SaveChanges();
                            }

                            #region Alerta SIGOPLAN
                            //PRIMER AUTORIZANTE
                            string txtAlerta = ("Justific. Num. Emp: {0}");

                            var objNoti = objRetardo.lstAutorizantes.FirstOrDefault();

                            tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                            objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                            objNuevaAlerta.userRecibeID = objNoti.idUsuario;
#if DEBUG
                            objNuevaAlerta.userRecibeID = 0; // OMAR NUÑEZ
#endif
                            objNuevaAlerta.tipoAlerta = 2;
                            objNuevaAlerta.sistemaID = 16;
                            objNuevaAlerta.visto = false;
                            objNuevaAlerta.url = "/Administrativo/Vacaciones/RetardosGestion";
                            objNuevaAlerta.objID = objCERetardo.id;
                            objNuevaAlerta.obj = "AutorizacionRetardos";
                            objNuevaAlerta.msj = string.Format(txtAlerta, objCERetardo.claveEmpleado);
                            objNuevaAlerta.documentoID = 0;
                            objNuevaAlerta.moduloID = 0;
                            _context.tblP_Alerta.Add(objNuevaAlerta);
                            _context.SaveChanges();
                            #endregion //ALERTA SIGPLAN

                            #endregion

                            #region CORREO

                            #region DOCUMENTO

                            var meses = new List<string>() { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

                            rptFechasJustificaciones rptCV = new rptFechasJustificaciones();

                            int idVac = objCERetardo.id;

                            var objVacaciones = GetRetardoById(new RetardosDTO() { id = idVac })["items"] as RetardosDTO;
                            result.Clear();
                            resultado.Clear();

                            DateTime? fechaIngreso = GetFechaIngreso(Convert.ToInt32(objVacaciones.claveEmpleado));
                            result.Clear();
                            resultado.Clear();

                            var objResponsableCC = objVacaciones.lstAutorizantes.FirstOrDefault(e => e.orden == Core.Enum.RecursosHumanos.Vacaciones.OrdenGestionEnum.RESPONSABLE_CC);
                            var objRespPagadas1 = objVacaciones.lstAutorizantes.FirstOrDefault(e => e.orden == Core.Enum.RecursosHumanos.Vacaciones.OrdenGestionEnum.AUTORIZANTE_PAGADAS_1);

                            var objStrVacaciones = new
                            {
                                nombreEmpleado = objVacaciones.nombreEmpleado,
                                claveEmpleado = objVacaciones.claveEmpleado,
                                ccEmpleado = objVacaciones.cc,
                                nombreResponsable = objResponsableCC.nombreCompleto,
                                nombreResponsablePagadas = objRespPagadas1 != null ? objRespPagadas1.nombreCompleto : "  ",
                                folio = (objVacaciones.cc == null ? "" : objVacaciones.cc) + "-" + (objVacaciones.claveEmpleado == null ? "" : objVacaciones.claveEmpleado) + "-" + (objVacaciones.consecutivo == null ? "" : objVacaciones.consecutivo.ToString().PadLeft(3, '0'))
                            };

                            DateTime dateTiempoHorarioLower = new DateTime();
                            DateTime dateTiempoHorarioUpper = new DateTime();

                            if (objVacaciones.tipoRetardo == 1)
                            {
                                dateTiempoHorarioLower = DateTime.Today.Add(objVacaciones.horarioLower.Value);
                                dateTiempoHorarioUpper = DateTime.Today.Add(objVacaciones.horarioUpper.Value);
                            }

                            rptCV.Database.Tables[0].SetDataSource(new[] { objStrVacaciones });
                            rptCV.Database.Tables[1].SetDataSource(getInfoEnca("reporte", ""));
                            
                            rptCV.SetParameterValue("todayDate", objVacaciones.fechaCreacion.Value.ToString("dd/MM/yyyy"));
                            rptCV.SetParameterValue("fechaIngreso", fechaIngreso.Value.ToString("dd/MM/yyyy"));
                            rptCV.SetParameterValue("descMotivo", objVacaciones.descMotivo);
                            rptCV.SetParameterValue("justificacion", objVacaciones.justificacion ?? "  ");
                            rptCV.SetParameterValue("strDias", " ");
                            rptCV.SetParameterValue("firmaElectJefeInmediato", " ");
                            rptCV.SetParameterValue("firmaElectResponsableCC", (objResponsableCC != null ? (objResponsableCC.firmaElect ?? " ") : " "));
                            rptCV.SetParameterValue("descPuesto", objVacaciones.descPuesto ?? " ");
                            rptCV.SetParameterValue("nombreJefeInmediato", objVacaciones.nombreJefeInmediato);
                            rptCV.SetParameterValue("nombreResponsableCC", objResponsableCC.nombreCompleto);
                            rptCV.SetParameterValue("ccDesc", objVacaciones.ccDesc.Trim());
                            rptCV.SetParameterValue("nombreCapturo", objVacaciones.nombreCapturo);
                            rptCV.SetParameterValue("estatusResponsable", (objResponsableCC.estatus == GestionEstatusEnum.AUTORIZADO ? "AUTORIZADO" : " "));
                            rptCV.SetParameterValue("tituloReporte", "Justificaciones");
                            rptCV.SetParameterValue("tipoJust", EnumHelper.GetDescription((TipoRetrasoEnum)objVacaciones.tipoRetardo.Value));
                            rptCV.SetParameterValue("dia", objVacaciones.diaTomado.ToString("dd/MM/yyyy"));
                            rptCV.SetParameterValue("horario",
                                (objVacaciones.tipoRetardo == 0 ?
                                EnumHelper.GetDescription((RetardoHorarioEnum)objVacaciones.horario.Value) :
                                ("De: " + dateTiempoHorarioLower.ToString("hh:mm tt") + " A: " + dateTiempoHorarioUpper.ToString("hh:mm tt"))));
                            rptCV.SetParameterValue("tiempoReq", ("Hrs: " + objVacaciones.tiempoRequeridoHrs) + " Min: " + objVacaciones.tiempoRequeridoMin);

                            Stream stream = rptCV.ExportToStream(ExportFormatType.PortableDocFormat);

                            #endregion

                            var lstCorreosNotificarTodos = new List<string>();

                            #region CUERPO CORREO
                            var objCC = _context.tblC_Nom_CatalogoCC.FirstOrDefault(e => e.cc == objEmpleado.cc_contable);
                            var objPuesto = _context.tblRH_EK_Puestos.FirstOrDefault(e => e.puesto == objEmpleado.puesto);

                            string cuerpo =
                                        @"<html>
                                            <head>
                                                <style>
                                                    table {
                                                        font-family: arial, sans-serif;
                                                        border-collapse: collapse;
                                                        width: 100%;
                                                    }

                                                    td, th {
                                                        border: 1px solid #dddddd;
                                                        text-align: left;
                                                        padding: 8px;
                                                    }

                                                    tr:nth-child(even) {
                                                        background-color: #ffcc5c;
                                                    }
                                                </style>
                                            </head>
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>

                                                    <p class=MsoNormal>
                                                        Buen día,<br><br>" +
                                                                "Se ha capturado una justificacion y esta lista para su gestion"
                                                                + @".<br><br>
                                                    </p>
                                                    <p class=MsoNormal style='font-weight:bold;'>" +
                                                                "Empleado: [" + objCERetardo.claveEmpleado + "] " + objCERetardo.nombreEmpleado + @".<o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal style='font-weight:bold;'>" +
                                                                "CC: [" + objCC.cc + "] " + objCC.ccDescripcion + @".<o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal style='font-weight:bold;'>" +
                                                                "Puesto: " + objPuesto.descripcion + @".<o:p></o:p>
                                                    </p>
                                                    <br><br><br>
                                            ";
                            #region TABLA AUTORIZANTES

                            cuerpo += @"
                                <table>
                                    <thead>
                                        <tr>
                                            <th>Nombre</th>
                                            <th>Tipo</th>
                                            <th>Autorizo</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    ";

                            bool esPrimero = true;
                            //EstatusGestionAutorizacionEnum P 0 A 1 R 2 
                            if (objRetardo.lstAutorizantes != null)
                            {
                                foreach (var itemDet in objRetardo.lstAutorizantes)
                                {
                                    tblP_Usuario objUsuario = lstUsuarios.FirstOrDefault(e => e.id == itemDet.idUsuario);
                                    string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                                    if (itemDet.orden == OrdenGestionEnum.CAPITAL_HUMANO)
                                    {
                                        nombreCompleto = "VOBO cumplimiento al reglamento";
                                    }

                                    cuerpo += "<tr>" +
                                                "<td>" + nombreCompleto + "</td>" +
                                                "<td>" + EnumHelper.GetDescription(itemDet.orden) + "</td>" +
                                                getEstatus(0, esPrimero, false) +
                                            "</tr>";

                                    if (esPrimero)
                                    {
                                        lstCorreosNotificarTodos.Add(objUsuario.correo);
                                        esPrimero = false;
                                    }
                                }
                            }

                            cuerpo += "</tbody>" +
                                        "</table>" +
                                        "<br><br><br>";


                            #endregion

                            cuerpo += "<br><br><br>" +
                                  "Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>" +
                                  "Construplan > Capital Humano > Administración de Personal > Incidencias > Control de Ausencias > Gestion > Gestión de Ausencias.<br><br>" +
                                  "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                                  "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias." +
                                " </body>" +
                              "</html>";
                            #endregion

                            var objUsuarioLogged = lstUsuarios.FirstOrDefault(e => e.id == vSesiones.sesionUsuarioDTO.id);

                            if (objUsuarioLogged != null)
                            {
                                lstCorreosNotificarTodos.Add(objUsuarioLogged.correo);

                            }

                            var objUsuarioRetardo = lstUsuarios.FirstOrDefault(e => e.cveEmpleado == objRetardo.claveEmpleado.ToString());

                            if (objUsuarioRetardo != null)
                            {
                                lstCorreosNotificarTodos.Add(objUsuarioRetardo.correo);
                            }

                            var objJefeInmediato = lstUsuarios.FirstOrDefault(e => e.id == objRetardo.idJefeInmediato);

                            if (objJefeInmediato != null)
                            {
                                lstCorreosNotificarTodos.Add(objJefeInmediato.correo);

                            }

                            lstCorreosNotificarTodos.Add("serviciosalpersonal@construplan.com.mx");

#if DEBUG
                            lstCorreosNotificarTodos = new List<string>();
                            //lstCorreosNotificarTodos.Add("omar.nunez@construplan.com.mx");
                            lstCorreosNotificarTodos.Add("miguel.buzani@construplan.com.mx");
#endif

                            string descTipoVacaciones = "JUSTIFICACION LISTA PARA SU GESTION. CC {2} EMPLEADO: [{0}] {1}";

                            //GlobalUtils.sendEmail(string.Format(descTipoVacaciones, objRetardo.claveEmpleado, objRetardo.nombreEmpleado, objRetardo.cc),
                            //    cuerpo, lstCorreosNotificarTodos);

                            lstCorreosNotificarTodos = lstCorreosNotificarTodos.Distinct().ToList();

                            List<byte[]> downloadPDFs = new List<byte[]>();
                            using (var streamReader = new MemoryStream())
                            {
                                stream.CopyTo(streamReader);
                                downloadPDFs.Add(streamReader.ToArray());

                                GlobalUtils.sendEmailAdjuntoInMemory2(string.Format(PersonalUtilities.GetNombreEmpresa() + ": " + descTipoVacaciones, objRetardo.claveEmpleado, objRetardo.nombreEmpleado, objRetardo.cc),
                                    cuerpo, lstCorreosNotificarTodos, downloadPDFs, "Justificaciones.pdf");

                            }

                            #endregion
                        }

                        dbTransac.Commit();

                        resultado.Add(SUCCESS, true);
                        resultado.Add("idRetardo", objCERetardo.id);
                    }
                    catch (Exception e)
                    {
                        LogError(0, 0, _NOMBRE_CONTROLADOR, "CrearEditarRetardo", e, AccionEnum.AGREGAR, objRetardo.id, objRetardo);

                        dbTransac.Rollback();
                        resultado.Add(MESSAGE, e.Message);
                        resultado.Add(SUCCESS, false);
                        
                    }
                }
            }

            return resultado;
        }

        public Dictionary<string, object> RemoveRetardo(int idRetardo)
        {
            resultado.Clear();

            using (var ctx = new MainContext())
            {
                using (var dbTransac = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        var objRetardo = ctx.tblRH_Vacaciones_Retardos.FirstOrDefault(e => e.id == idRetardo);

                        objRetardo.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                        objRetardo.fechaModificacion = DateTime.Now;
                        objRetardo.registroActivo = false;
                        ctx.SaveChanges();

                        #region ALERTA
                        var alertasVacaciones = _context.tblP_Alerta.Where(e => e.sistemaID == 16 && e.objID == idRetardo && e.obj == "AutorizacionRetardos").ToList();

                        foreach (var item in alertasVacaciones)
                        {
                            item.visto = true;
                            _context.SaveChanges();
                        }
                    
                        #endregion

                        dbTransac.Commit();

                        resultado.Add(SUCCESS, true);
                    }
                    catch (Exception e)
                    {
                        dbTransac.Rollback();

                        resultado.Add(MESSAGE, e.Message);
                        resultado.Add(SUCCESS, false);

                    }
                }
            }

            return resultado;
        }

        public Dictionary<string, object> FillComboMotivosByTipo(int tipoRetardo)
        {
            resultado.Clear();

            using (var ctx = new MainContext())
            {
                try
                {
                    var lstMotivos = ctx.tblRH_Vacaciones_Retardos_Motivos.Where(e => e.esActivo && e.tipoRetardo == tipoRetardo).Select(e => new ComboDTO()
                    {
                        Value = e.id.ToString(),
                        Text = e.descripcion,
                    }).ToList();

                    resultado.Add(ITEMS, lstMotivos);
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);

                }
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarArchivoRetardo(int idRetardo, HttpPostedFileBase archivoActa)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var RutaBase = "";

#if DEBUG
                    RutaBase = @"C:\Proyecto\SIGOPLAN\CAPITAL_HUMANO\VACACIONES\RETARDOS";
#else
                    RutaBase = @"\\REPOSITORIO\Proyecto\SIGOPLAN\CAPITAL_HUMANO\VACACIONES\RETARDOS";
#endif

                    string rutaArchivoActa = Path.Combine(RutaBase, ObtenerFormatoNombreArchivo("", archivoActa.FileName));

                    var registroVacaciones = _context.tblRH_Vacaciones_Retardos.FirstOrDefault(x => x.id == idRetardo);

                    if (registroVacaciones == null)
                    {
                        throw new Exception("No se encuentra la información del registro del retardo.");
                    }

                    registroVacaciones.rutaArchivoActa = rutaArchivoActa;
                    _context.SaveChanges();

                    if (GlobalUtils.SaveHTTPPostedFile(archivoActa, rutaArchivoActa) == false)
                    {
                        dbContextTransaction.Rollback();
                        resultado.Clear();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                        return resultado;
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "GuardarArchivoActa", e, AccionEnum.AGREGAR, 0, idRetardo);
                }
            }

            return resultado;
        }

        public Tuple<Stream, string> DescargarArchivoRetardo(int id)
        {
            try
            {
                var registroVacaciones = _context.tblRH_Vacaciones_Retardos.FirstOrDefault(x => x.id == id);

                var fileStream = GlobalUtils.GetFileAsStream(registroVacaciones.rutaArchivoActa);
                string name = Path.GetFileName(registroVacaciones.rutaArchivoActa);

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Dictionary<string, object> GetRetardoById(RetardosDTO objFiltro)
        {
            result = new Dictionary<string, object>();
            try
            {
                var lstCC = _context.tblC_Nom_CatalogoCC.ToList();
                var lstPuestos = _context.tblRH_EK_Puestos.ToList();

                var objTblVacaciones = _context.tblRH_Vacaciones_Retardos.FirstOrDefault(e => e.registroActivo && e.id == objFiltro.id);

                var objMotivo = _context.tblRH_Vacaciones_Retardos_Motivos.FirstOrDefault(e => e.esActivo && e.id == objTblVacaciones.motivoJustificacion);

                var lstUsuarios = _context.tblP_Usuario.Where(e => e.estatus).ToList();
                var objUsuarioCapturo = lstUsuarios.FirstOrDefault(e => e.id == objTblVacaciones.idUsuarioCreacion);

                RetardosDTO objVacaciones = new RetardosDTO()
                {

                    id = objTblVacaciones.id,
                    estado = objTblVacaciones.estado,
                    nombreEmpleado = objTblVacaciones.nombreEmpleado,
                    claveEmpleado = objTblVacaciones.claveEmpleado,
                    comentarioRechazada = objTblVacaciones.comentarioRechazada ?? "",
                    tipoRetardo = objTblVacaciones.tipoRetardo,
                    motivoJustificacion = objTblVacaciones.motivoJustificacion,
                    horario = objTblVacaciones.horario,
                    horarioLower = objTblVacaciones.horarioLower,
                    horarioUpper = objTblVacaciones.horarioUpper,
                    tiempoRequeridoMin = objTblVacaciones.tiempoRequeridoMin,
                    tiempoRequeridoHrs = objTblVacaciones.tiempoRequeridoHrs,
                    cc = objTblVacaciones.cc,
                    justificacion = objTblVacaciones.justificacion,
                    idJefeInmediato = objTblVacaciones.idJefeInmediato,
                    nombreJefeInmediato = objTblVacaciones.nombreJefeInmediato,
                    rutaArchivoActa = objTblVacaciones.rutaArchivoActa,
                    consecutivo = objTblVacaciones.consecutivo,
                    diaTomado = objTblVacaciones.diaTomado,
                    descMotivo = objMotivo.descripcion,
                    fechaCreacion = objTblVacaciones.fechaCreacion,
                    nombreCapturo = (objUsuarioCapturo != null ? (objUsuarioCapturo.apellidoPaterno + " " + objUsuarioCapturo.apellidoMaterno + " " + objUsuarioCapturo.nombre) : ""),

                };

                int numClave_empleado = Convert.ToInt32(objTblVacaciones.claveEmpleado);
                var objEmpleado = _context.tblRH_EK_Empleados.FirstOrDefault(e => e.esActivo && e.clave_empleado == numClave_empleado);

                var objPuesto = lstPuestos.FirstOrDefault(e => e.puesto == objEmpleado.puesto);

                objVacaciones.descPuesto = objPuesto.descripcion;

                List<VacacionesDTO> lstFiltrada = new List<VacacionesDTO>();

                var objCC = lstCC.FirstOrDefault(e => e.cc == objVacaciones.cc);

                if (objCC != null)
                {
                    objVacaciones.ccDesc = "[" + objCC.cc + "] " + objCC.ccDescripcion;
                }
                else
                {
                    objVacaciones.ccDesc = "S/N";
                }

                #region LISTA AUTH
                var lstAuthDTO = new List<VacacionesGestionDTO>();
                var lstAuth = _context.tblRH_Vacaciones_Retardos_Gestion.Where(e => e.registroActivo && e.idRetardo == objVacaciones.id).ToList();

                foreach (var itemAuth in lstAuth)
                {
                    var objAuth = new VacacionesGestionDTO();
                
                    if (itemAuth.orden == OrdenGestionEnum.CAPITAL_HUMANO)
                    {
                        objAuth.id = itemAuth.id;
                        objAuth.idUsuario = itemAuth.idUsuario;
                        objAuth.estatus = itemAuth.estatus;
                        objAuth.orden = itemAuth.orden;
                        objAuth.nombreCompleto = "VOBO cumplimiento al reglamento";
                        objAuth.firmaElect = itemAuth.firmaElect;

                        lstAuthDTO.Add(objAuth);
                    }
                    else
                    {
                        var objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.id == itemAuth.idUsuario);
                        string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                        objAuth.id = itemAuth.id;
                        objAuth.idUsuario = itemAuth.idUsuario;
                        objAuth.estatus = itemAuth.estatus;
                        objAuth.orden = itemAuth.orden;
                        objAuth.nombreCompleto = nombreCompleto;
                        objAuth.firmaElect = itemAuth.firmaElect;

                        lstAuthDTO.Add(objAuth);
                    }
                }

                objVacaciones.lstAutorizantes = lstAuthDTO;
                #endregion

                result = new Dictionary<string, object>();

                result.Add(SUCCESS, true);
                result.Add(ITEMS, objVacaciones);
                result.Add("claveEmpleado", objFiltro.claveEmpleado);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }
        #endregion

        #region RETARDOS GESTION
        public Dictionary<string, object> AutorizarRetardo(int id, int estado, string msg)
        {
            result = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var lstAuth = _context.tblRH_Vacaciones_Retardos_Gestion.Where(e => e.registroActivo && e.idRetardo == id).ToList();
                    var vacacionObj = _context.tblRH_Vacaciones_Retardos.FirstOrDefault(e => e.id == id);
                    var lstUsuarios = _context.tblP_Usuario.Where(e => e.estatus).ToList();

                    if (vacacionObj == null)
                        throw new Exception("Ocurrio algo mal");

                    var lstCorreosNotificarRestantes = new List<string>();
                    var lstCorreosNotificarTodos = new List<string>();
                    string cuerpo = "";

                    #region AUTORIZAR/RECHAZAR

                    int totalAuth = 0;
                    bool notifyNextAuth = false;
                    int totalAlertas = 0;

                    foreach (var item in lstAuth)
                    {
                        tblP_Usuario objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.id == item.idUsuario);
                        lstCorreosNotificarTodos.Add(objUsuario.correo);

                        #region AGREGAR ALERTA PARA EL SIGUIENTE AUTORIZANTE
                        if (notifyNextAuth && estado == (int)GestionEstatusEnum.AUTORIZADO && totalAlertas == 0)
                        {

                            string txtAlerta = ("Justific. Num. Emp: {0}");

                            #region Alerta SIGOPLAN
                            tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                            objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                            objNuevaAlerta.userRecibeID = item.idUsuario;
#if DEBUG
                            //objNuevaAlerta.userRecibeID = 7939;
#endif
                            objNuevaAlerta.tipoAlerta = 2;
                            objNuevaAlerta.sistemaID = 16;
                            objNuevaAlerta.visto = false;
                            objNuevaAlerta.url = "/Administrativo/Vacaciones/RetardosGestion";
                            objNuevaAlerta.objID = id;
                            objNuevaAlerta.obj = "AutorizacionRetardos";
                            objNuevaAlerta.msj = string.Format(txtAlerta, vacacionObj.claveEmpleado);
                            objNuevaAlerta.documentoID = 0;
                            objNuevaAlerta.moduloID = 0;
                            _context.tblP_Alerta.Add(objNuevaAlerta);
                            _context.SaveChanges();
                            #endregion //ALERTA SIGPLAN

                            //SIGUIENTE EN SER AUTORIZADO
                            lstCorreosNotificarRestantes.Add(objUsuario.correo);

                            //NOTIFICADA
                            notifyNextAuth = false;
                            totalAlertas++;
                        }

                        #endregion

                        if (item.estatus == GestionEstatusEnum.AUTORIZADO)
                            totalAuth++;
                        else
                        {
                            if (item.idUsuario == vSesiones.sesionUsuarioDTO.id)
                            {
                                if (estado == (int)GestionEstatusEnum.AUTORIZADO)
                                {
                                    totalAuth++;
                                    notifyNextAuth = true;
                                    tblP_Alerta objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == item.idUsuario && e.visto == false && e.objID == id && e.obj == "AutorizacionRetardos");
                                    if (objAlerta != null)
                                    {
                                        //AGREGAR FIRMA ELECT
                                        item.firmaElect = GlobalUtils.CrearFirmaDigital(vacacionObj.id, DocumentosEnum.FirmaGestionVacaciones, Convert.ToInt32(item.idUsuario));

                                        objAlerta.visto = true;
                                        _context.SaveChanges();
                                    }
                                }
                                else
                                {
                                    tblP_Alerta objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == item.idUsuario && e.visto == false && e.objID == id && e.obj == "AutorizacionRetardos");
                                    if (objAlerta != null)
                                    {
                                        objAlerta.visto = true;
                                        _context.SaveChanges();
                                    }
                                }
                                item.estatus = (GestionEstatusEnum)estado;
                                item.fechaModificacion = DateTime.Now;
                                item.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;

                                _context.SaveChanges();
                            }
                        }
                    }

                    #endregion

                    var objVacaciones = GetRetardoById(new RetardosDTO() { id = vacacionObj.id })["items"] as RetardosDTO;
                    ////LIMPIAR RESULTADO
                    resultado.Clear();
                    result.Clear();

                    #region CUERPO CORREO

                    #region DOCUMENTO

                    var meses = new List<string>() { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

                    rptFechasJustificaciones rptCV = new rptFechasJustificaciones();

                    int idVac = id;

                    DateTime? fechaIngreso = GetFechaIngreso(Convert.ToInt32(objVacaciones.claveEmpleado));
                    result.Clear();
                    resultado.Clear();

                    var objResponsableCC = objVacaciones.lstAutorizantes.FirstOrDefault(e => e.orden == Core.Enum.RecursosHumanos.Vacaciones.OrdenGestionEnum.RESPONSABLE_CC);
                    var objRespPagadas1 = objVacaciones.lstAutorizantes.FirstOrDefault(e => e.orden == Core.Enum.RecursosHumanos.Vacaciones.OrdenGestionEnum.AUTORIZANTE_PAGADAS_1);

                    var objStrVacaciones = new
                    {
                        nombreEmpleado = objVacaciones.nombreEmpleado,
                        claveEmpleado = objVacaciones.claveEmpleado,
                        ccEmpleado = objVacaciones.cc,
                        nombreResponsable = objResponsableCC.nombreCompleto,
                        nombreResponsablePagadas = objRespPagadas1 != null ? objRespPagadas1.nombreCompleto : "  ",
                        folio = (objVacaciones.cc == null ? "" : objVacaciones.cc) + "-" + (objVacaciones.claveEmpleado == null ? "" : objVacaciones.claveEmpleado) + "-" + (objVacaciones.consecutivo == null ? "" : objVacaciones.consecutivo.ToString().PadLeft(3, '0'))
                    };

                    DateTime dateTiempoHorarioLower = new DateTime();
                    DateTime dateTiempoHorarioUpper = new DateTime();

                    if (objVacaciones.tipoRetardo == 1)
                    {
                        dateTiempoHorarioLower = DateTime.Today.Add( objVacaciones.horarioLower.Value);
                        dateTiempoHorarioUpper = DateTime.Today.Add(objVacaciones.horarioUpper.Value);
                    }

                    rptCV.Database.Tables[0].SetDataSource(new[] { objStrVacaciones });
                    rptCV.Database.Tables[1].SetDataSource(getInfoEnca("reporte", ""));

                    rptCV.SetParameterValue("todayDate", objVacaciones.fechaCreacion.Value.ToString("dd/MM/yyyy"));
                    rptCV.SetParameterValue("fechaIngreso", fechaIngreso.Value.ToString("dd/MM/yyyy"));
                    rptCV.SetParameterValue("descMotivo", objVacaciones.descMotivo);
                    rptCV.SetParameterValue("justificacion", objVacaciones.justificacion ?? "  ");
                    rptCV.SetParameterValue("strDias", " ");
                    rptCV.SetParameterValue("firmaElectJefeInmediato", " ");
                    rptCV.SetParameterValue("firmaElectResponsableCC", (objResponsableCC != null ? (objResponsableCC.firmaElect ?? " ") : " "));
                    rptCV.SetParameterValue("descPuesto", objVacaciones.descPuesto ?? " ");
                    rptCV.SetParameterValue("nombreJefeInmediato", objVacaciones.nombreJefeInmediato);
                    rptCV.SetParameterValue("nombreResponsableCC", objResponsableCC.nombreCompleto);
                    rptCV.SetParameterValue("ccDesc", objVacaciones.ccDesc.Trim());
                    rptCV.SetParameterValue("nombreCapturo", objVacaciones.nombreCapturo);
                    rptCV.SetParameterValue("estatusResponsable", (objResponsableCC.estatus == GestionEstatusEnum.AUTORIZADO ? "AUTORIZADO" : " "));
                    rptCV.SetParameterValue("tituloReporte", "Justificaciones");
                    rptCV.SetParameterValue("tipoJust", EnumHelper.GetDescription((TipoRetrasoEnum)objVacaciones.tipoRetardo.Value));
                    rptCV.SetParameterValue("dia", objVacaciones.diaTomado.ToString("dd/MM/yyyy"));
                    rptCV.SetParameterValue("horario",
                        (objVacaciones.tipoRetardo == 0 ?
                        EnumHelper.GetDescription((RetardoHorarioEnum)objVacaciones.horario.Value) :
                        ("De: " + dateTiempoHorarioLower.ToString("hh:mm tt") + " A: " + dateTiempoHorarioUpper.ToString("hh:mm tt"))));
                    rptCV.SetParameterValue("tiempoReq", ("Hrs: " + objVacaciones.tiempoRequeridoHrs) + " Min: " + objVacaciones.tiempoRequeridoMin);

                    Stream stream = rptCV.ExportToStream(ExportFormatType.PortableDocFormat);

                    #endregion

                    cuerpo =
                                @"<html>
                                            <head>
                                                <style>
                                                    table {
                                                        font-family: arial, sans-serif;
                                                        border-collapse: collapse;
                                                        width: 100%;
                                                    }

                                                    td, th {
                                                        border: 1px solid #dddddd;
                                                        text-align: left;
                                                        padding: 8px;
                                                    }

                                                    tr:nth-child(even) {
                                                        background-color: #ffcc5c;
                                                    }
                                                </style>
                                            </head>
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>

                                                    <p class=MsoNormal>
                                                        Buen día,<br><br>" +
                                                                           (
                                                                            totalAuth == lstAuth.Count() ? "Se ha autorizado la justificacion por todos los firmantes" : (
                                                                                estado == (int)GestionEstatusEnum.RECHAZADO ? ("Se ha rechazado la justificacion <br>Motivo: " + msg) 
                                                                                : "Se ha autorizado la justificacion")
                                                                           )
                                                        + @".<br><br>
                                                    </p>
                                                    <p class=MsoNormal style='font-weight:bold;'>" +
                                                        "Empleado: [" + vacacionObj.claveEmpleado + "] " + vacacionObj.nombreEmpleado + @".<o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal style='font-weight:bold;'>" +
                                                                "CC: " + objVacaciones.ccDesc.Trim() + @".<o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal style='font-weight:bold;'>" +
                                                                "Puesto: " + objVacaciones.descPuesto + @".<o:p></o:p>
                                                    </p>
                                                    <br><br><br>
                                            ";
                    #region TABLA AUTORIZANTES

                    cuerpo += @"
                                <table>
                                    <thead>
                                        <tr>
                                            <th>Nombre</th>
                                            <th>Tipo</th>
                                            <th>Autorizo</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    ";

                    bool esAuth = false;
                    int totalSiguientes = 0;
                    bool esRechazada = false;
                    //EstatusGestionAutorizacionEnum P 0 A 1 R 2 
                    foreach (var itemDet in lstAuth)
                    {
                        tblP_Usuario objUsuario = lstUsuarios.FirstOrDefault(e => e.id == itemDet.idUsuario);

                        string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                        if (itemDet.orden == OrdenGestionEnum.CAPITAL_HUMANO)
                        {
                            switch (itemDet.estatus)
                            {
                                case GestionEstatusEnum.PENDIENTE:
                                    nombreCompleto = "VOBO cumplimiento al reglamento";
                                    break;
                                case GestionEstatusEnum.AUTORIZADO:
                                    nombreCompleto = "VOBO cumplimiento al reglamento";
                                    break;
                                case GestionEstatusEnum.RECHAZADO:
                                    nombreCompleto = "Rechazo por incumplimiento al reglamento";
                                    break;
                            }
                        }

                        cuerpo += "<tr>" +
                                    "<td>" + nombreCompleto + "</td>" +
                                    "<td>" + EnumHelper.GetDescription(itemDet.orden) + "</td>" +
                                    getEstatus((int)itemDet.estatus, esAuth, esRechazada) +
                                "</tr>";

                        if (estado == (int)GestionEstatusEnum.RECHAZADO)
                        {
                            esRechazada = true;
                        }

                        if (vSesiones.sesionUsuarioDTO.id == itemDet.idUsuario && totalSiguientes == 0)
                        {
                            esAuth = true;
                            totalSiguientes++;
                        }
                        else
                        {
                            if (esAuth)
                            {
                                esAuth = false;

                                if (itemDet.estatus == GestionEstatusEnum.AUTORIZADO)
                                {
                                    esAuth = true;
                                    totalSiguientes = 0;
                                }
                            }
                        }
                    }

                    cuerpo += "</tbody>" +
                                "</table>" +
                                "<br><br><br>";


                    #endregion

                    cuerpo += "<br><br><br>" +
                          "Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>" +
                          "Construplan > Capital Humano > Administración de Personal > Incidencias > Control de Ausencias > Gestion > Justificaciones.<br><br>" +
                          "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                          "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias." +
                        " </body>" +
                      "</html>";

                    #endregion

                    if (totalAuth == lstAuth.Count())
                    {
                        vacacionObj.estado = estado;
                        vacacionObj.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        vacacionObj.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();

                        var objEmpVac = _context.tblP_Usuario.FirstOrDefault(e => e.cveEmpleado == vacacionObj.claveEmpleado);

                        if (objEmpVac != null && !string.IsNullOrEmpty(objEmpVac.correo))
                        {
                            lstCorreosNotificarTodos.Add(objEmpVac.correo);
                        }

                        //lstCorreosNotificarTodos.Add("despacho@construplan.com.mx");
                        lstCorreosNotificarTodos.Add("serviciosalpersonal@construplan.com.mx");

                        //List<int> listaUsuariosCorreos = _context.tblRH_REC_Notificantes_Altas.Where(w => (w.cc == vacacionObj.cc || w.cc == "*") && w.esActivo).Select(s => s.idUsuario).ToList();

                        //foreach (var item in listaUsuariosCorreos)
                        //{
                        //    lstCorreosNotificarTodos.Add(_context.tblP_Usuario.FirstOrDefault(x => x.id == item).correo);
                        //}

                        var objUsuarioLogged = lstUsuarios.FirstOrDefault(e => e.id == vSesiones.sesionUsuarioDTO.id);

                        if (objUsuarioLogged != null)
                        {
                            lstCorreosNotificarTodos.Add(objUsuarioLogged.correo);
                            
                        }

                        var objUsuarioRetardo = lstUsuarios.FirstOrDefault(e => e.cveEmpleado == vacacionObj.claveEmpleado.ToString());

                        if (objUsuarioRetardo != null)
                        {
                            lstCorreosNotificarTodos.Add(objUsuarioRetardo.correo);
                        }

                        var objJefeInmediato = lstUsuarios.FirstOrDefault(e => e.id == objVacaciones.idJefeInmediato);

                        if (objJefeInmediato != null)
                        {
                            lstCorreosNotificarTodos.Add(objJefeInmediato.correo);

                        }

#if DEBUG
                        lstCorreosNotificarTodos = new List<string>();
                        //lstCorreosNotificarTodos.Add("omar.nunez@construplan.com.mx");
                        lstCorreosNotificarTodos.Add("miguel.buzani@construplan.com.mx");
#endif

                        string descTipoVacaciones = "";
                        descTipoVacaciones = "JUSTIFICACIONES AUTORIZADAS POR TODOS LOS FIRMANTES. CC {2} EMPLEADO: [{0}] {1}";

                        //GlobalUtils.sendEmail(string.Format(descTipoVacaciones, vacacionObj.claveEmpleado, vacacionObj.nombreEmpleado, vacacionObj.cc),
                        //    cuerpo, lstCorreosNotificarTodos);

                        lstCorreosNotificarTodos = lstCorreosNotificarTodos.Distinct().ToList();

                        List<byte[]> downloadPDFs = new List<byte[]>();
                        using (var streamReader = new MemoryStream())
                        {
                            stream.CopyTo(streamReader);
                            downloadPDFs.Add(streamReader.ToArray());

                            GlobalUtils.sendEmailAdjuntoInMemory2(string.Format(PersonalUtilities.GetNombreEmpresa() + ": " + descTipoVacaciones, vacacionObj.claveEmpleado, vacacionObj.nombreEmpleado, vacacionObj.cc),
                                cuerpo, lstCorreosNotificarTodos, downloadPDFs, "Justificaciones.pdf");

                        }
                    }
                    else
                    {
                        if (estado == (int)GestionEstatusEnum.RECHAZADO)
                        {
                            vacacionObj.estado = estado;
                            vacacionObj.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                            vacacionObj.fechaModificacion = DateTime.Now;
                            vacacionObj.comentarioRechazada = msg ?? "";
                            _context.SaveChanges();

                            #region CORREO ESTATUS
                            lstCorreosNotificarRestantes = new List<string>();
                            lstCorreosNotificarRestantes.Add("serviciosalpersonal@construplan.com.mx");

                            var objUsuarioLogged = lstUsuarios.FirstOrDefault(e => e.id == vSesiones.sesionUsuarioDTO.id);

                            if (objUsuarioLogged != null)
                            {
                                lstCorreosNotificarRestantes.Add(objUsuarioLogged.correo);

                            }

                            var objUsuarioRetardo = lstUsuarios.FirstOrDefault(e => e.cveEmpleado == vacacionObj.claveEmpleado.ToString());

                            if (objUsuarioRetardo != null)
                            {
                                lstCorreosNotificarRestantes.Add(objUsuarioRetardo.correo);
                            }

                            var objJefeInmediato = lstUsuarios.FirstOrDefault(e => e.id == objVacaciones.idJefeInmediato);

                            if (objJefeInmediato != null)
                            {
                                lstCorreosNotificarRestantes.Add(objJefeInmediato.correo);

                            }

#if DEBUG
                            lstCorreosNotificarRestantes = new List<string>();
                            //lstCorreosNotificarRestantes.Add("omar.nunez@construplan.com.mx");
                            lstCorreosNotificarRestantes.Add("miguel.buzani@construplan.com.mx");
#endif
                            //GlobalUtils.sendEmail(string.Format("AUSENCIAS RECHAZADAS. EMPLEADO: [{0}] {1}", vacacionObj.claveEmpleado, vacacionObj.nombreEmpleado),
                            //                        cuerpo, lstCorreosNotificarRestantes);

                            string descTipoVacaciones = "";
                            descTipoVacaciones = "JUSTIFICACIONES RECHAZADAS. CC {2} EMPLEADO: [{0}] {1}";

                            //GlobalUtils.sendEmail(string.Format(descTipoVacaciones, vacacionObj.claveEmpleado, vacacionObj.nombreEmpleado, vacacionObj.cc),
                            //    cuerpo, lstCorreosNotificarTodos);

                            lstCorreosNotificarRestantes = lstCorreosNotificarRestantes.Distinct().ToList();

                            List<byte[]> downloadPDFs = new List<byte[]>();
                            using (var streamReader = new MemoryStream())
                            {
                                stream.CopyTo(streamReader);
                                downloadPDFs.Add(streamReader.ToArray());

                                GlobalUtils.sendEmailAdjuntoInMemory2(string.Format(PersonalUtilities.GetNombreEmpresa() + ": " + descTipoVacaciones, vacacionObj.claveEmpleado, vacacionObj.nombreEmpleado, vacacionObj.cc),
                                    cuerpo, lstCorreosNotificarRestantes, downloadPDFs, "Justificaciones.pdf");

                            }
                            #endregion
                        }

                        if (estado == (int)GestionEstatusEnum.AUTORIZADO)
                        {
                            #region CORREO ESTATUS
                            lstCorreosNotificarTodos.Add("serviciosalpersonal@construplan.com.mx");

                            var objJefeInmediato = lstUsuarios.FirstOrDefault(e => e.id == objVacaciones.idJefeInmediato);

                            if (objJefeInmediato != null)
                            {
                                lstCorreosNotificarTodos.Add(objJefeInmediato.correo);

                            }

                            var objUsuarioRetardo = lstUsuarios.FirstOrDefault(e => e.cveEmpleado == vacacionObj.claveEmpleado.ToString());

                            if (objUsuarioRetardo != null)
                            {
                                lstCorreosNotificarTodos.Add(objUsuarioRetardo.correo);
                            }

#if DEBUG
                            lstCorreosNotificarTodos = new List<string>();
                            //lstCorreosNotificarRestantes.Add("omar.nunez@construplan.com.mx");
                            lstCorreosNotificarTodos.Add("miguel.buzani@construplan.com.mx");
#endif
                            //GlobalUtils.sendEmail(string.Format("AUSENCIAS AUTORIZADAS. EMPLEADO: [{0}] {1}", vacacionObj.claveEmpleado, vacacionObj.nombreEmpleado),
                            //                        cuerpo, lstCorreosNotificarRestantes);

                            string descTipoVacaciones = "";
                            descTipoVacaciones = "JUSTIFICACIONES AUTORIZADAS. EMPLEADO: CC {2} [{0}] {1}";

                            //GlobalUtils.sendEmail(string.Format(descTipoVacaciones, vacacionObj.claveEmpleado, vacacionObj.nombreEmpleado, vacacionObj.cc),
                            //    cuerpo, lstCorreosNotificarTodos);

                            lstCorreosNotificarTodos = lstCorreosNotificarTodos.Distinct().ToList();

                            List<byte[]> downloadPDFs = new List<byte[]>();
                            using (var streamReader = new MemoryStream())
                            {
                                stream.CopyTo(streamReader);
                                downloadPDFs.Add(streamReader.ToArray());

                                GlobalUtils.sendEmailAdjuntoInMemory2(string.Format(PersonalUtilities.GetNombreEmpresa() + ": " + descTipoVacaciones, vacacionObj.claveEmpleado, vacacionObj.nombreEmpleado, vacacionObj.cc),
                                    cuerpo, lstCorreosNotificarTodos, downloadPDFs, "Justificaciones.pdf");

                            }
                            #endregion
                        }

                    }

                    dbContextTransaction.Commit();
                    result.Add(SUCCESS, true);

                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "AutorizarVacacion", e, AccionEnum.CONSULTA, 0, 0);
                    result.Add(MESSAGE, e.Message);
                    result.Add(SUCCESS, false);
                }
            }

            return result;
        }
        public Dictionary<string, object> GetRetardosGestion(RetardosDTO objFiltro)
        {
            result = new Dictionary<string, object>();
            try
            {
                var lstCC = _context.tblP_CC.ToList();
                var lstPermisoCC = _context.tblRH_BN_Usuario_CC.Where(e => e.usuarioID == vSesiones.sesionUsuarioDTO.id).Select(e => e.cc).ToList();

                List<RetardosDTO> lstVacaciones = _context.tblRH_Vacaciones_Retardos.Where(e => e.registroActivo
                    && (objFiltro.estado != 0 ? e.estado == objFiltro.estado : true)
                    && (!string.IsNullOrEmpty(objFiltro.cc) ? e.cc == objFiltro.cc : (lstPermisoCC.Contains("*") || vSesiones.sesionUsuarioDTO.idPerfil == 1) ? true : 
                            lstPermisoCC.Contains(e.cc))
                    ).Select(e => new RetardosDTO
                    {
                        id = e.id,
                        estado = e.estado,
                        nombreEmpleado = e.nombreEmpleado,
                        claveEmpleado = e.claveEmpleado,
                        comentarioRechazada = e.comentarioRechazada ?? "",
                        tipoRetardo = e.tipoRetardo,
                        cc = e.cc,
                        rutaArchivoActa = e.rutaArchivoActa,
                        consecutivo = e.consecutivo,
                        diaTomado = e.diaTomado
                    }).OrderBy(e => e.nombreEmpleado).ToList();

                List<RetardosDTO> lstFiltrada = new List<RetardosDTO>();

                foreach (var item in lstVacaciones)
                {
                    var lstAuth = _context.tblRH_Vacaciones_Retardos_Gestion.Where(e => e.registroActivo && e.idRetardo == item.id).ToList();
                    var lstIdsAuth = lstAuth.Select(e => e.idUsuario).ToList();;

                    //true: Estas en la lista de firmantes, eres admin, eres diana alvarez o keyla vasquez
                    if (lstIdsAuth.Contains(vSesiones.sesionUsuarioDTO.id) || vSesiones.sesionUsuarioDTO.idPerfil == 1 || vSesiones.sesionUsuarioDTO.id == 1019 || vSesiones.sesionUsuarioDTO.id == 79552)
                    {
                        var objCC = lstCC.FirstOrDefault(e => e.cc == item.cc);

                        if (objCC != null)
                        {
                            item.ccDesc = "[" + objCC.cc + "] " + objCC.descripcion;
                        }
                        else
                        {
                            item.ccDesc = "S/N";
                        }

                        #region LISTA AUTH
                        var lstAuthDTO = new List<VacacionesGestionDTO>();

                        int? sigAuth = null;
                        foreach (var itemAuth in lstAuth)
                        {
                            if (itemAuth.estatus == GestionEstatusEnum.AUTORIZADO || itemAuth.estatus == GestionEstatusEnum.RECHAZADO)
                                item.esFirmar = false;
                            else
                            {
                                if (sigAuth == null)
                                    sigAuth = itemAuth.idUsuario;

                                if (sigAuth.Value == vSesiones.sesionUsuarioDTO.id)
                                    item.esFirmar = true;
                                else
                                    item.esFirmar = false;
                            }

                            var objAuth = new VacacionesGestionDTO();

                            var objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.id == itemAuth.idUsuario);
                            string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                            if (itemAuth.orden == OrdenGestionEnum.CAPITAL_HUMANO)
                            {
                                nombreCompleto = "VOBO cumplimiento al reglamento";
                            }

                            objAuth.id = itemAuth.id;
                            objAuth.idUsuario = itemAuth.idUsuario;
                            objAuth.estatus = itemAuth.estatus;
                            objAuth.orden = itemAuth.orden;
                            objAuth.nombreCompleto = nombreCompleto;

                            lstAuthDTO.Add(objAuth);
                        }

                        item.lstAutorizantes = lstAuthDTO;

                        lstFiltrada.Add(item);
                        #endregion
                    }
                    
                }

                result = new Dictionary<string, object>();
                result.Add(SUCCESS, true);
                result.Add(ITEMS, lstFiltrada);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }
        #endregion
    }
}