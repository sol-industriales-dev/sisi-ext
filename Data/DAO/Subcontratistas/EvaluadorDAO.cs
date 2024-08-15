using Core.DAO.Subcontratistas;
using Core.DTO;
using Core.DTO.ControlObra;
using Core.DTO.ControlObra.EvaluacionSubcontratista;
using Core.DTO.Principal.Generales;
using Core.DTO.Subcontratistas.Evaluacion;
using Core.DTO.Utils.Firmas;
using Core.Entity.ControlObra;
using Core.Entity.ControlObra.Evaluacion;
using Core.Entity.Encuestas;
using Core.Entity.Principal.Alertas;
using Core.Entity.Principal.Usuarios;
using Core.Entity.SubContratistas;
using Core.Enum.ControlObra.EvaluacionSubcontratista;
using Dapper;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Principal.Alertas;
using Infrastructure.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Data.DAO.Subcontratistas
{
    public class EvaluadorDAO : GenericDAO<tblCO_ADP_EvalSubConAsignacion>, IEvaluadorDAO
    {

        AlertaFactoryServices alertaFactoryServices = new AlertaFactoryServices();
        contextSigoplan db = new contextSigoplan();
        DynamicParameters lstParametros = new DynamicParameters();
        List<tblPUsuarioDTO> lstUsuariosExpediente = new List<tblPUsuarioDTO>();
        tblPUsuarioDTO objUsuariosExpediente = new tblPUsuarioDTO();
        private readonly string RutaBase = @"\\10.1.0.49\Proyecto\SUBCONTRATISTAS\SUBCONTRATISTAS_EVALUACIONES";
        string NombreControlador = "EvaluacionSubcontratistaController";
        private const string RutaLocal = @"C:\Proyecto\SUBCONTRATISTAS\CONTROL_OBRA";
        private readonly string RutaControlObra;
        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        public EvaluadorDAO()
        {
            resultado.Clear();
#if DEBUG
            RutaControlObra = Path.Combine(RutaLocal, @"EVALUACION_DE_SUBCONTRATISTAS");
#else
            RutaControlObra = Path.Combine(RutaBase, @"EVALUACION_DE_SUBCONTRATISTAS");
#endif
        }
       
        #region SUB CONTRATISTA

        public List<ComboDTO> getProyecto(int idUsuario)
        {
            var lstProyecto = new List<ComboDTO>();
            try
            {
                var obj = db.tblCO_ADP_EvaluadorXcc.Where(r => r.evaluador == idUsuario && r.esActivo).FirstOrDefault();
                if (obj != null)
                {
                    var lst = obtenerTodolosCC(obj.cc);

                    lstProyecto = _context.tblP_CC.Where(r => r.estatus && lst.Contains(r.cc)).ToList().Select(y => new ComboDTO
                    {
                        Value = y.cc.ToString(),
                        Text = y.cc.ToString() + " " + y.descripcion
                    }).ToList();
                }
                else
                {
                    lstProyecto = _context.tblP_CC.Where(r => r.estatus).ToList().Select(y => new ComboDTO
                    {
                        Value = y.cc.ToString(),
                        Text = y.cc.ToString() + " " + y.descripcion
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return lstProyecto;
        }
        public List<ComboDTO> getSubContratistas(string AreaCuenta)
        {
            var lstProyecto = new List<ComboDTO>();
            try
            {
                if (AreaCuenta == null)
                {
                    AreaCuenta = "";
                }
                List<ContratoSubContratistaDTO> lstContrato = new List<ContratoSubContratistaDTO>();



                var lstSubContratistas = db.tblX_SubContratista.Where(r => r.estatus).ToList();
                var lstContratos = db.tblX_Contrato.Where(r => r.estatus
                    //&& (AreaCuenta == "" ? r.cc == r.cc : r.cc == AreaCuenta)
                                                                                        && (r.cc == AreaCuenta)
                                                                                        ).ToList().Select(y => y.subcontratistaID).ToList();
                lstContrato = lstSubContratistas.Where(y => lstContratos.Contains(y.id)).Select(y => new ContratoSubContratistaDTO
                {
                    id = y.id,
                    nombre = y.nombre,
                    direccion = y.direccion,
                }).ToList();

                lstProyecto = lstContrato.ToList().Select(y => new ComboDTO
                {
                    Value = y.id.ToString(),
                    Text = y.nombre
                }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return lstProyecto;
        }
        public SubContratistasDTO getTblSubContratista(SubContratistasDTO parametros)
        {
            return null;
        }
        public SubContratistasDTO addEditSubContratista(List<HttpPostedFileBase> Archivo, List<SubContratistasDTO> parametros, tblPUsuarioDTO ObjUsuario, bool subcontratista)
        {
            string urlArchivo = RutaControlObra;
            string FechaActual = Convert.ToString(parametros.Select(y => y.idSubContratista).FirstOrDefault().ToString() + "CO" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute);
            urlArchivo = Path.Combine(urlArchivo, FechaActual);
            if (Archivo != null)
            {
                foreach (var Arch in Archivo)
                {
                    foreach (var param in parametros)
                    {
                        if (Arch.FileName == param.Archivo)
                        {
                            param.File = Arch;
                        }
                    }
                }

            }

            var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string, string>>();
            tblCO_ADP_EvalSubContratistaDet objAdd = new tblCO_ADP_EvalSubContratistaDet();

            foreach (var item in parametros)
            {
                #region ELIMINADO
                //if (objAdd == null)
                //{
                //    objAdd = new tblCO_ADP_EvalSubContratista();
                //    objAdd.tipoEvaluacion = parametros.Select(y => y.tipoEvaluacion).FirstOrDefault();
                //    objAdd.idAreaCuenta = parametros.Select(y => y.AreaCuenta).FirstOrDefault();
                //    objAdd.idSubContratista = parametros.Select(y => y.idSubContratista).FirstOrDefault();
                //    objAdd.Calificacion = parametros.Select(y => y.Calificacion).FirstOrDefault();

                //    db.tblCO_ADP_EvalSubContratista.Add(objAdd);
                //    db.SaveChanges();


                //    foreach (var item in parametros)
                //    {
                //        tblCO_ADP_EvalSubContratistaDet objAddDet = new tblCO_ADP_EvalSubContratistaDet();
                //        objAddDet = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.id == item.id).FirstOrDefault();
                //        if (objAddDet == null)
                //        {
                //            objAddDet = new tblCO_ADP_EvalSubContratistaDet();
                //            objAddDet.idEvaluacion = objAdd.id;
                //            objAddDet.fechaDocumento = DateTime.Now;
                //            objAddDet.idRow = item.idRow;
                //            objAddDet.calificacion = 0;
                //            objAddDet.tipoEvaluacion = item.tipoEvaluacion;
                //            if (item.Archivo != null)
                //            {
                //                objAddDet.rutaArchivo = Path.Combine(urlArchivo, item.Archivo);
                //            }
                //            db.tblCO_ADP_EvalSubContratistaDet.Add(objAddDet);
                //            db.SaveChanges();

                //            if (item.File != null)
                //            {
                //                listaRutaArchivos.Add(Tuple.Create(item.File, Path.Combine(urlArchivo, item.Archivo), urlArchivo));

                //            }
                //        }
                //    }

                //}
                //else
                #endregion
                objAdd = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.idRow == item.idRow && r.id == item.idEvaluacion).FirstOrDefault();
                if (objAdd != null)
                {
                    objAdd.fechaDocumento = DateTime.Now;
                    if (item.Archivo != null)
                    {
                        objAdd.rutaArchivo = Path.Combine(urlArchivo, item.Archivo);
                    }
                    db.SaveChanges();

                    if (item.File != null)
                    {
                        listaRutaArchivos.Add(Tuple.Create(item.File, Path.Combine(urlArchivo, item.Archivo), urlArchivo));

                    }
                }
            }

            foreach (var arch in listaRutaArchivos)
            {
                if (SaveHTTPPostedFile(arch.Item1, arch.Item2, arch.Item3) == false)
                {
                    resultado.Clear();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                }
            }

            if (ObjUsuario.tipo == 3)
            {
                int idSubContratista = parametros.Select(y => y.idSubContratista).FirstOrDefault();
                int tipoEvaluacion = parametros.Select(y => y.tipoEvaluacion).FirstOrDefault();
                tblPUsuarioDTO objUsuario = new tblPUsuarioDTO();
                string sql = @"SELECT * FROM tblP_Usuario WHERE id="+idSubContratista;
                using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                {
                    ctx.Open();
                    objUsuario = ctx.Query<tblPUsuarioDTO>(sql, null, null, true, 300).FirstOrDefault();
                    ctx.Close();
                }
                var objTipoEvaluacion = db.tblCO_ADP_EvaluacionDiv.Where(r => r.id == tipoEvaluacion).FirstOrDefault();
                if (objUsuario != null)
                {
                    var objSubcon2 = db.tblCO_ADP_EvalSubContratista.Where(r => r.id == objAdd.idEvaluacion).FirstOrDefault();
                    var objAsignacion = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.id == objSubcon2.idSubConAsignacion).FirstOrDefault();
                    var lstCorreos = db.tblCO_ADP_EvaluadorXcc.Where(r => r.esActivo).ToList().Select(y => new
                    {
                        evaluador = y.evaluador,
                        cc = retornarStringComasSep(y.cc),
                        elementos = retornarStringComasSep(y.elementos),
                    }).ToList();

                    var lstEvaluador = lstCorreos.Where(r => r.cc.Contains(objAsignacion.cc) && r.elementos.Contains(tipoEvaluacion.ToString())).Select(y => y.evaluador).ToList();
                    List<tblPUsuarioDTO> lst = new List<tblPUsuarioDTO>();
                    string sql3 = @"SELECT * FROM tblP_Usuario WHERE tipo=10 AND id LIKE'%" + lstEvaluador + "%'";
                    using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                    {
                        ctx.Open();
                        lst = ctx.Query<tblPUsuarioDTO>(sql3, null, null, true, 300).ToList();
                        ctx.Close();
                    }

                    List<string> emails = new List<string>();
                    foreach (var item in lst)
                    {
                        emails.Add(item.correo);
                    }
                    if (subcontratista == false)
                    {
                        var objSubContra = db.tblX_SubContratista.Where(r => r.rfc == objUsuario._user).FirstOrDefault();
                        var objSubcon = db.tblCO_ADP_EvalSubContratista.Where(r => r.id == objAdd.idEvaluacion).FirstOrDefault();
                        var objAlerta = db.tblP_Alerta.Where(r => r.objID == objSubcon.idSubConAsignacion && r.sistemaID == 2 && r.tipoAlerta == 2).FirstOrDefault();
                        if (objAlerta != null)
                        {
                            alertaFactoryServices.getAlertaService().updateAlerta(objAlerta);
                        }
                        string Subject = "RETROALIMENTACION SUBCONTRATISTA " + objTipoEvaluacion.descripcion + ".";
                        string msg2 = CuerpoCorreoSinPass(objAsignacion, objUsuario);
                        List<FirmantesDTO> lstFirmantes = new List<FirmantesDTO>();
                        string msg = GlobalUtils.cuerpoDeCorreoFormato("Prestadores de servicio", "Evaluaciones", "Asginacion de la evaluacion para prestadores de servicio", msg2, "Gracias por su atencion.", "http://expediente.construplan.com.mx/", lstFirmantes);

                        GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), Subject), msg, emails);
                    }
                    else
                    {
                        var objSubContra = db.tblX_SubContratista.Where(r => r.rfc == objUsuario._user).FirstOrDefault();
                        var objSubcon = db.tblCO_ADP_EvalSubContratista.Where(r => r.id == objAdd.idEvaluacion).FirstOrDefault();

                        string Subject = "RETROALIMENTACION SUBCONTRATISTA " + objTipoEvaluacion.descripcion + ".";
                        string msg2 = CuerpoCorreoSinPass(objAsignacion, objUsuario);
                        List<FirmantesDTO> lstFirmantes = new List<FirmantesDTO>();
                        string msg = GlobalUtils.cuerpoDeCorreoFormato("Prestadores de servicio", "Evaluaciones", "Asginacion de la evaluacion para prestadores de servicio", msg2, "Gracias por su atencion.", "http://expediente.construplan.com.mx/", lstFirmantes);

                        GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), Subject), msg, emails);
                    }

                }
            }
            else if (ObjUsuario.tipo == 14)
            {

                tblPUsuarioDTO objUsuarioHijo = new tblPUsuarioDTO();
                string sql = @"SELECT * FROM tblP_Usuario WHERE tipo=10 AND estatus='true' AND id LIKE'%" + ObjUsuario.id + "%'";
                using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                {
                    ctx.Open();
                    objUsuarioHijo = ctx.Query<tblPUsuarioDTO>(sql, null, null, true, 300).FirstOrDefault();
                    ctx.Close();
                }

                tblPUsuarioDTO objUsuarioPadre = new tblPUsuarioDTO();
                string sql2 = @"SELECT * FROM tblP_Usuario WHERE tipo=10 AND estatus='true' AND idPadre LIKE'%" + objUsuarioHijo.idPadre+ "%'";
                using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                {
                    ctx.Open();
                    objUsuarioPadre = ctx.Query<tblPUsuarioDTO>(sql2, null, null, true, 300).FirstOrDefault();
                    ctx.Close();
                }

                var objSubcon2 = db.tblCO_ADP_EvalSubContratista.Where(r => r.id == objAdd.idEvaluacion).FirstOrDefault();
                var objAsignacion = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.id == objSubcon2.idSubConAsignacion).FirstOrDefault();
                int tipoEvaluacion = parametros.Select(y => y.tipoEvaluacion).FirstOrDefault();
                var objTipoEvaluacion = db.tblCO_ADP_EvaluacionDiv.Where(r => r.id == tipoEvaluacion).FirstOrDefault();

                var lstCorreos = db.tblCO_ADP_EvaluadorXcc.Where(r => r.esActivo).ToList().Select(y => new
                {
                    evaluador = y.evaluador,
                    cc = retornarStringComasSep(y.cc),
                    elementos = retornarStringComasSep(y.elementos),
                }).ToList();

                var lstEvaluador = lstCorreos.Where(r => r.cc.Contains(objAsignacion.cc) && r.elementos.Contains(tipoEvaluacion.ToString())).Select(y => y.evaluador).ToList();
                List<tblPUsuarioDTO> lst = new List<tblPUsuarioDTO>();
                string sql3 = @"SELECT * FROM tblP_Usuario WHERE tipo=10 AND id LIKE'%" + lstEvaluador + "%'";
                using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                {
                    ctx.Open();
                    lst = ctx.Query<tblPUsuarioDTO>(sql3, null, null, true, 300).ToList();
                    ctx.Close();
                }
                

                List<string> emails = new List<string>();
                foreach (var item in lst)
                {
                    emails.Add(item.correo);
                }


                if (objUsuarioPadre != null)
                {
                    if (subcontratista == false)
                    {
                        var objSubContra = db.tblX_SubContratista.Where(r => r.rfc == objUsuarioPadre._user).FirstOrDefault();
                        var objSubcon = db.tblCO_ADP_EvalSubContratista.Where(r => r.id == objAdd.idEvaluacion).FirstOrDefault();
                        var objAlerta = db.tblP_Alerta.Where(r => r.objID == objSubcon.idSubConAsignacion && r.sistemaID == 2 && r.tipoAlerta == 2).FirstOrDefault();
                        if (objAlerta != null)
                        {
                            alertaFactoryServices.getAlertaService().updateAlerta(objAlerta);
                        }
                        string Subject = "RETROALIMENTACION SUBCONTRATISTA " + objTipoEvaluacion.descripcion + ".";
                        string msg2 = CuerpoCorreoSinPass(objAsignacion, objUsuarioPadre);
                        List<FirmantesDTO> lstFirmantes = new List<FirmantesDTO>();
                        string msg = GlobalUtils.cuerpoDeCorreoFormato("Prestadores de servicio", "Evaluaciones", "Asginacion de la evaluacion para prestadores de servicio", msg2, "Gracias por su atencion.", "http://expediente.construplan.com.mx/", lstFirmantes);

                        GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), Subject), msg, emails);
                    }
                    else
                    {

                        var objSubContra = db.tblX_SubContratista.Where(r => r.rfc == objUsuarioPadre._user).FirstOrDefault();
                        var objSubcon = db.tblCO_ADP_EvalSubContratista.Where(r => r.id == objAdd.idEvaluacion).FirstOrDefault();
                        string Subject = "RETROALIMENTACION SUBCONTRATISTA " + objTipoEvaluacion.descripcion + ".";
                        string msg2 = CuerpoCorreoSinPass(objAsignacion, objUsuarioPadre);
                        List<FirmantesDTO> lstFirmantes = new List<FirmantesDTO>();
                        string msg = GlobalUtils.cuerpoDeCorreoFormato("Prestadores de servicio", "Evaluaciones", "Asginacion de la evaluacion para prestadores de servicio", msg2, "Gracias por su atencion.", "http://expediente.construplan.com.mx/", lstFirmantes);

                        GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), Subject), msg, emails);
                    }

                }
            }
            return null;
        }
        public List<string> retornarStringComasSep(string r)
        {
            var a = r.Split(',');
            List<string> b = new List<string>();
            if (a != null)
            {
                foreach (var item in a)
                {
                    if (item != "")
                    {
                        b.Add(item);
                    }
                }
            }
            return b;
        }
        public string CuerpoCorreo(tblCO_ADP_EvalSubConAsignacion objAsignacion, tblPUsuarioDTO objUsuario)
        {
            string textoContrato = objAsignacion.contrato == null ? "No cuentas con numero de contrato" : objAsignacion.contrato.numeroContrato;
            string textoProyecto = objAsignacion.contrato == null ? "No cuentas con un proyecto asignado" : objAsignacion.contrato.proyecto.nombre;
            string a = "Tiene una evaluacion pendiente por realizar <br>"
                                      + " La evaluacion con el nombre : " + objAsignacion.nombreEvaluacion + "<br>"
                                      + " El numero del contrato " + textoContrato + "<br>"
                                      + " El nombre del proyecto asignado " + textoProyecto + "<br>"
                                      + " Usuario : " + objUsuario._user + "<br>"
                                      + " Password : " + objUsuario._pass + "<br>"
                                      + " Gracias por su atencion";
            return a;
        }
        public string CuerpoCorreoSinPass(tblCO_ADP_EvalSubConAsignacion objAsignacion, tblPUsuarioDTO objUsuario)
        {
            string textoContrato = objAsignacion.contrato == null ? "No cuentas con numero de contrato" : objAsignacion.contrato.numeroContrato;
            string textoProyecto = objAsignacion.contrato == null ? "No cuentas con un proyecto asignado" : objAsignacion.contrato.proyecto.nombre;
            string a = "Tiene una evaluacion pendiente por realizar <br>"
                                      + " La evaluacion con el nombre : " + objAsignacion.nombreEvaluacion + "<br>"
                                      + " El numero del contrato " + textoContrato + "<br>"
                                      + " El nombre del proyecto asignado " + textoProyecto + "<br>"
                                      + " Gracias por su atencion";
            return a;
        }
        public static bool SaveHTTPPostedFile(HttpPostedFileBase archivo, string ruta, string Archivo)
        {
            try
            {
                byte[] data;

                if (!Directory.Exists(Archivo))
                {
                    Directory.CreateDirectory(Archivo);
                }

                using (Stream inputStream = archivo.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    data = memoryStream.ToArray();
                }

                File.WriteAllBytes(ruta, data);
            }
            catch (Exception)
            {
                return false;
            }

            return File.Exists(ruta);
        }
        public List<SubContratistasDTO> CargarArchivosXSubcontratista(SubContratistasDTO parametros)
        {
            List<SubContratistasDTO> lstDetalle = new List<SubContratistasDTO>();
            try
            {
                var icontextuttonElemento = db.tblCO_ADP_EvaluacionDiv.First(x => x.id == parametros.tipoEvaluacion).idbutton;
                var objSubAsig = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.idSubContratista == parametros.idSubContratista && r.cc == parametros.cc && r.esActivo == true && r.statusAutorizacion == 2).FirstOrDefault();
                if (objSubAsig != null)
                {
                    var objSubContratistas = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == objSubAsig.id && r.tipoEvaluacion == parametros.tipoEvaluacion).FirstOrDefault();
                    if (objSubContratistas != null)
                    {
                        var lstDatos = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.idEvaluacion == objSubContratistas.id && r.tipoEvaluacion == parametros.tipoEvaluacion).ToList();
                        if (lstDatos.Count() != 0)
                        {
                            lstDetalle = lstDatos.Select(y => new SubContratistasDTO
                            {
                                id = y.id,
                                idRow = y.idRow,
                                tipoEvaluacion = y.tipoEvaluacion,
                                idEvaluacion = y.idEvaluacion,
                                evaluacionPendiente = y.evaluacionPendiente,
                                rutaArchivo = obtenerNombreRuta(y.rutaArchivo),
                                Calificacion = db.tblCO_ADP_EvalSubContratista.Where(n => n.id == y.idEvaluacion).Select(P => P.Calificacion).FirstOrDefault(),
                                idSubContratista = db.tblCO_ADP_EvalSubContratista.Where(n => n.id == y.idEvaluacion).Select(P => P.idSubContratista).FirstOrDefault(),
                                estaEvaluado = y.calificacion != 0,
                                btnElemento = icontextuttonElemento
                            }).ToList();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return lstDetalle;
        }
        public List<SubContratistasDTO> CargarArchivosSubcontratista(SubContratistasDTO parametros, tblPUsuarioDTO ObjUsuario)
        {
            List<SubContratistasDTO> lstDetalle = new List<SubContratistasDTO>();
            try
            {
                if (ObjUsuario.tipo == 3)
                {
                    int idSubcontratista = db.tblX_SubContratista.Where(r => r.rfc == parametros.RFC).FirstOrDefault().id;
                    parametros.idSubContratista = idSubcontratista;

                    var objAsignacion = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.idSubContratista == parametros.idSubContratista && r.id == parametros.idAsignacion && r.esActivo).FirstOrDefault();

                    if (objAsignacion != null)
                    {
                        var objSubContratista = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubContratista == parametros.idSubContratista && r.idSubConAsignacion == objAsignacion.id && r.tipoEvaluacion == parametros.tipoEvaluacion).FirstOrDefault();

                        if (objSubContratista != null)
                        {
                            var lstDatos = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.tipoEvaluacion == parametros.tipoEvaluacion && r.idEvaluacion == objSubContratista.id).ToList();
                            if (lstDatos.Count() != 0)
                            {
                                lstDetalle = lstDatos.Select(y => new SubContratistasDTO
                                {
                                    id = y.id,
                                    idRow = y.idRow,
                                    tipoEvaluacion = y.tipoEvaluacion,
                                    idEvaluacion = y.idEvaluacion,
                                    evaluacionPendiente = y.evaluacionPendiente,
                                    rutaArchivo = obtenerNombreRuta(y.rutaArchivo),
                                    Calificacion = db.tblCO_ADP_EvalSubContratista.Where(n => n.id == y.idEvaluacion).Select(P => P.Calificacion).FirstOrDefault(),
                                    idSubContratista = db.tblCO_ADP_EvalSubContratista.Where(n => n.id == y.idEvaluacion).Select(P => P.idSubContratista).FirstOrDefault(),
                                    opcional = y.opcional
                                }).ToList();
                            }
                        }
                    }
                }
                else if (ObjUsuario.tipo == 14)
                {
                    tblPUsuarioDTO objUsuarioHijo = new tblPUsuarioDTO();
                    string sql = @"SELECT * FROM tblP_Usuario WHERE tipo=10 AND estatus='true' AND id LIKE'%" + ObjUsuario.id + "%'";
                    using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                    {
                        ctx.Open();
                        objUsuarioHijo = ctx.Query<tblPUsuarioDTO>(sql, null, null, true, 300).FirstOrDefault();
                        ctx.Close();
                    }


                    tblPUsuarioDTO objUsuarioPadre = new tblPUsuarioDTO();
                    string sql2 = @"SELECT * FROM tblP_Usuario WHERE tipo=10 AND estatus='true' AND id LIKE'%" + objUsuarioHijo.idPadre + "%'";
                    using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                    {
                        ctx.Open();
                        objUsuarioPadre = ctx.Query<tblPUsuarioDTO>(sql2, null, null, true, 300).FirstOrDefault();
                        ctx.Close();
                    }

                    int idSubcontratista = db.tblX_SubContratista.Where(r => r.rfc == objUsuarioPadre._user).FirstOrDefault().id;
                    parametros.idSubContratista = idSubcontratista;

                    var objAsignacion = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.idSubContratista == parametros.idSubContratista && r.id == parametros.idAsignacion && r.esActivo).FirstOrDefault();

                    if (objAsignacion != null)
                    {
                        var objSubContratista = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubContratista == parametros.idSubContratista && r.idSubConAsignacion == objAsignacion.id && r.tipoEvaluacion == parametros.tipoEvaluacion).FirstOrDefault();

                        if (objSubContratista != null)
                        {
                            var lstDatos = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.tipoEvaluacion == parametros.tipoEvaluacion && r.idEvaluacion == objSubContratista.id).ToList();
                            if (lstDatos.Count() != 0)
                            {
                                lstDetalle = lstDatos.Select(y => new SubContratistasDTO
                                {
                                    id = y.id,
                                    idRow = y.idRow,
                                    tipoEvaluacion = y.tipoEvaluacion,
                                    idEvaluacion = y.idEvaluacion,
                                    evaluacionPendiente = y.evaluacionPendiente,
                                    rutaArchivo = obtenerNombreRuta(y.rutaArchivo),
                                    Calificacion = db.tblCO_ADP_EvalSubContratista.Where(n => n.id == y.idEvaluacion).Select(P => P.Calificacion).FirstOrDefault(),
                                    idSubContratista = db.tblCO_ADP_EvalSubContratista.Where(n => n.id == y.idEvaluacion).Select(P => P.idSubContratista).FirstOrDefault(),
                                    opcional = y.opcional
                                }).ToList();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return lstDetalle;
        }
        public string obtenerNombreRuta(string rutaArchivo)
        {
            string r = "";

#if DEBUG
            r = rutaArchivo == null ? "Ningún archivo seleccionado" : rutaArchivo.Split('\\')[6];
#else
            r = rutaArchivo == null ? "Ningún archivo seleccionado" : rutaArchivo.Split('\\')[8];
#endif



            return r;
        }
        public List<SubContratistasDTO> CargarArchivosXSubcontratistaEvaluacion(SubContratistasDTO parametros)
        {
            List<SubContratistasDTO> lstDetalle = new List<SubContratistasDTO>();
            try
            {
                var objSubContratista = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubContratista == parametros.idSubContratista).FirstOrDefault();
                if (objSubContratista != null)
                {
                    var lstDatos = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.tipoEvaluacion == parametros.tipoEvaluacion).ToList();
                    if (lstDatos.Count() != 0)
                    {
                        lstDetalle = lstDatos.Select(y => new SubContratistasDTO
                        {
                            id = y.id,
                            idRow = y.idRow,
                            tipoEvaluacion = y.tipoEvaluacion,
                            idEvaluacion = y.idEvaluacion,
                            rutaArchivo = obtenerNombreRuta(y.rutaArchivo),
                            Calificacion = db.tblCO_ADP_EvalSubContratista.Where(n => n.id == y.idEvaluacion).Select(P => P.Calificacion).FirstOrDefault(),
                            idSubContratista = db.tblCO_ADP_EvalSubContratista.Where(n => n.id == y.idEvaluacion).Select(P => P.idSubContratista).FirstOrDefault(),
                        }).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return lstDetalle;
        }
        public List<SubContratistasDTO> ObtenerTblAutorizacion(SubContratistasDTO parametros)
        {
            List<SubContratistasDTO> lstDatos = new List<SubContratistasDTO>();
            try
            {
                lstDatos = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubContratista == parametros.idSubContratista).ToList().Select(y => new SubContratistasDTO
                {
                    id = y.id,
                    Calificacion = y.Calificacion,
                    fechaAutorizacion = y.fechaAutorizacion,
                    tipoEvaluacionDesc = obtenerTipoEvaluacion(db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.idEvaluacion == y.id).Select(n => n.tipoEvaluacion).FirstOrDefault())
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return lstDatos;
        }
        public string obtenerTipoEvaluacion(int tipoEvaluacion)
        {
            string Desc = "";
            switch (tipoEvaluacion)
            {
                case 1:
                    Desc = "Calidad ";
                    break;
                case 2:
                    Desc = "Planeacion/Programa";
                    break;
                case 3:
                    Desc = "Facturacion";
                    break;
                case 4:
                    Desc = "Seguridad";
                    break;
                case 5:
                    Desc = "Ambiental";
                    break;
                case 6:
                    Desc = "Efectivo";
                    break;
                case 7:
                    Desc = "Fuerza de trabajo y Atencion al cliente";
                    break;
            }
            return Desc;
        }

        public List<DivicionesMenuDTO> obtenerDiviciones(int idPlantilla, int idAsignacion)
        {
            List<DivicionesMenuDTO> lstDatos2 = new List<DivicionesMenuDTO>();
            try
            {
                var lstIdDiviciones = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == idAsignacion).Select(y => y.tipoEvaluacion).ToList();

                var lstDatos = db.tblCO_ADP_EvaluacionDiv.Where(r => r.esActivo && r.idPlantilla == idPlantilla && r.SubContratista == false && lstIdDiviciones.Contains(r.id)).ToList().Select(y => new DivicionesMenuDTO
                {
                    id = y.id,
                    idbutton = y.idbutton,
                    idsection = y.idsection,
                    toltips = y.toltips,
                    descripcion = y.descripcion,
                    esActivo = y.esActivo,
                    orden = y.orden,
                    important = y.important
                }).ToList();
                lstDatos2 = lstDatos.OrderBy(n => n.orden).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return lstDatos2;
        }
        public Dictionary<string, object> obtenerPlantillas()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                var lst = db.tblCO_ADP_EvaluacionPlantilla.ToList().Select(y => new
                {
                    id = y.id,
                    nombrePlantilla = y.nombrePlantilla,
                    lstId = obtenerNumeroContratosID(y.contratos),
                    contratos = obtenerNumeroContratos(y.contratos),
                    esActivo = y.esActivo,
                }).ToList();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lst);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                throw;
            }
            return resultado;
        }
        public List<string> obtenerNumeroContratosID(string rcon)
        {
            List<string> rt = new List<string>();
            if (rcon != "")
            {
                var r = rcon.Split(',');
                foreach (var item in r)
                {
                    if (item != "")
                    {
                        int id = Convert.ToInt32(item);
                        var contrato = Convert.ToString(db.tblX_Contrato.Where(f => f.id == id).FirstOrDefault().id);
                        rt.Add(contrato);
                    }
                }
            }
            return rt;
        }
        public string obtenerNumeroContratos(string rcon)
        {
            string rt = "";
            if (rcon != "")
            {
                var r = rcon.Split(',');
                foreach (var item in r)
                {
                    if (item != "")
                    {
                        int id = Convert.ToInt32(item);
                        var contrato = db.tblX_Contrato.Where(f => f.id == id).FirstOrDefault().numeroContrato;
                        rt += contrato + ",";
                    }
                }
            }
            return rt.TrimEnd(',');
        }
        public Dictionary<string, object> eliminarPlantilla(int id)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                var objPlantilla = db.tblCO_ADP_EvaluacionPlantilla.Where(r => r.id == id).FirstOrDefault();
                var lstDiv = db.tblCO_ADP_EvaluacionDiv.Where(r => r.idPlantilla == id).ToList();



                foreach (var item in lstDiv)
                {
                    var lstReq = db.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == item.id).ToList();

                    db.tblCO_ADP_EvaluacionReq.RemoveRange(lstReq);
                    db.SaveChanges();
                }
                db.tblCO_ADP_EvaluacionPlantilla.Remove(objPlantilla);
                db.SaveChanges();

                db.tblCO_ADP_EvaluacionDiv.RemoveRange(lstDiv);
                db.SaveChanges();

                resultado.Add(ITEMS, "Registro borrado con exito.");
                resultado.Add(SUCCESS, true);

            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                throw;
            }

            return resultado;
        }
        public DivicionesMenuDTO addEditPlantilla(DivicionesMenuDTO parametros)
        {
            DivicionesMenuDTO objDatos = new DivicionesMenuDTO();
            try
            {
                tblCO_ADP_EvaluacionPlantilla obj = new tblCO_ADP_EvaluacionPlantilla();
                if (parametros.idPlantilla == 0)
                {
                    obj.contratos = parametros.contratos;
                    obj.nombrePlantilla = parametros.nombrePlantilla;
                    obj.esActivo = true;
                    db.tblCO_ADP_EvaluacionPlantilla.Add(obj);
                    db.SaveChanges();
                }
                else
                {
                    obj = db.tblCO_ADP_EvaluacionPlantilla.Where(r => r.id == parametros.idPlantilla).FirstOrDefault();
                    obj.contratos = parametros.contratos;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return objDatos;
        }
        public List<ComboDTO> cboObtenerContratos()
        {
            List<ComboDTO> lstCombo = new List<ComboDTO>();
            try
            {
                var lstPlantillasContrato = db.tblCO_ADP_EvaluacionPlantilla.ToList();
                List<int> lstTodosLosContratos = new List<int>();
                foreach (var item in lstPlantillasContrato)
                {
                    var lst = obtenerListas(item.contratos);
                    lstTodosLosContratos.AddRange(lst);
                }

                lstCombo = db.tblX_Contrato.Where(r => !lstTodosLosContratos.Contains(r.id) && r.estatus).ToList().Select(y => new ComboDTO
                {
                    Value = y.id.ToString(),
                    Text = y.numeroContrato
                }).ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
            return lstCombo;

        }
        public List<ComboDTO> cboObtenerContratosInclu(int idPlantilla)
        {
            List<ComboDTO> lstCombo = new List<ComboDTO>();
            try
            {
                var lstPlantillasContrato = db.tblCO_ADP_EvaluacionPlantilla.Where(r => r.id != idPlantilla).ToList();
                List<int> lstTodosLosContratos = new List<int>();
                foreach (var item in lstPlantillasContrato)
                {
                    var lst = obtenerListas(item.contratos);
                    lstTodosLosContratos.AddRange(lst);
                }

                lstCombo = db.tblX_Contrato.Where(r => !lstTodosLosContratos.Contains(r.id) && r.estatus).ToList().Select(y => new ComboDTO
                {
                    Value = y.id.ToString(),
                    Text = y.numeroContrato
                }).ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
            return lstCombo;
        }
        public List<int> obtenerListas(string rt)
        {
            List<int> devolverLst = new List<int>();
            if (rt != "")
            {
                var arr = rt.Split(',');
                if (arr.Count() > 0)
                {
                    foreach (var item in arr)
                    {
                        if (item != "")
                        {
                            devolverLst.Add(Convert.ToInt32(item));

                        }
                    }
                }
            }
            return devolverLst;
        }

        public DivicionesMenuDTO addEditDiviciones(DivicionesMenuDTO parametros)
        {
            DivicionesMenuDTO objDatos = new DivicionesMenuDTO();
            try
            {
                var obj = db.tblCO_ADP_EvaluacionDiv.Where(y => y.id == parametros.id).FirstOrDefault();
                if (obj == null)
                {
                    int id = 0;
                    int orden = 0;
                    var objid = (from i in db.tblCO_ADP_EvaluacionDiv orderby i.id descending select i).FirstOrDefault();
                    if (objid == null)
                    {
                        id = 1;
                        orden = 1;
                    }
                    else
                    {
                        id = 1 + db.tblCO_ADP_EvaluacionDiv.OrderByDescending(n => n.id).FirstOrDefault().id;
                        orden = 1 + db.tblCO_ADP_EvaluacionDiv.OrderByDescending(n => n.orden).FirstOrDefault().id;
                    }
                    obj = new tblCO_ADP_EvaluacionDiv();
                    obj.idPlantilla = parametros.idPlantilla;
                    obj.esActivo = true;
                    obj.descripcion = parametros.descripcion;
                    obj.idEvaluador = parametros.idEvaluador;
                    obj.toltips = parametros.toltips;
                    obj.idbutton = "btnEvaluacion" + id;
                    obj.idsection = "sectionDiv" + id;
                    obj.orden = parametros.orden;
                    obj.important = parametros.important;
                    db.tblCO_ADP_EvaluacionDiv.Add(obj);
                    db.SaveChanges();
                    var objUltimoid = (from i in db.tblCO_ADP_EvaluacionDiv orderby i.id descending select i).FirstOrDefault();

                    if (parametros.lstRequerimientos != null)
                    {
                        foreach (var item in parametros.lstRequerimientos)
                        {
                            tblCO_ADP_EvaluacionReq objReq = new tblCO_ADP_EvaluacionReq();
                            int id2 = 0;
                            var objid2 = (from i in db.tblCO_ADP_EvaluacionReq orderby i.id descending select i).FirstOrDefault();
                            if (objid2 == null)
                            {
                                id2 = 1;
                            }
                            else
                            {
                                id2 = 1 + db.tblCO_ADP_EvaluacionReq.OrderByDescending(n => n.id).FirstOrDefault().id;
                            }
                            objReq.idDiv = objUltimoid.id;
                            objReq.texto = item.texto;
                            objReq.inputFile = "inputFile" + id2;
                            objReq.lblInput = "lblInput" + id2;
                            objReq.txtAComentario = "txtArea" + id2;
                            objReq.txtPlaneacion = "txtPlaneacion" + id2;
                            objReq.txtResponsable = "txtResponsable" + id2;
                            objReq.txtFechaCompromiso = "txtFechaCompromiso" + id2;
                            objReq.preguntarEvaluacion = item.important;
                            db.tblCO_ADP_EvaluacionReq.Add(objReq);
                            db.SaveChanges();
                        }
                    }

                }
                else
                {
                    obj.idPlantilla = parametros.idPlantilla;
                    obj.idEvaluador = parametros.idEvaluador;
                    obj.descripcion = parametros.descripcion;
                    obj.toltips = parametros.toltips;
                    obj.orden = parametros.orden;
                    obj.important = parametros.important;
                    db.SaveChanges();
                    var lstReq = db.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == parametros.id).ToList();
                    if (lstReq.Count() != 0)
                    {
                        db.tblCO_ADP_EvaluacionReq.RemoveRange(lstReq);
                        db.SaveChanges();
                    }
                    if (parametros.lstRequerimientos != null)
                    {
                        foreach (var item in parametros.lstRequerimientos)
                        {
                            tblCO_ADP_EvaluacionReq objReq = new tblCO_ADP_EvaluacionReq();
                            int id2 = 0;
                            var objid2 = (from i in db.tblCO_ADP_EvaluacionReq orderby i.id descending select i).FirstOrDefault();
                            if (objid2 == null)
                            {
                                id2 = 1;
                            }
                            else
                            {
                                id2 = 1 + db.tblCO_ADP_EvaluacionReq.OrderByDescending(n => n.id).FirstOrDefault().id;
                            }
                            objReq.idDiv = parametros.id;
                            objReq.texto = item.texto;
                            objReq.inputFile = "inputFile" + id2;
                            objReq.lblInput = "lblInput" + id2;
                            objReq.txtAComentario = "txtArea" + id2;
                            objReq.txtPlaneacion = "txtPlaneacion" + id2;
                            objReq.txtResponsable = "txtResponsable" + id2;
                            objReq.txtFechaCompromiso = "txtFechaCompromiso" + id2;
                            objReq.preguntarEvaluacion = item.important;
                            db.tblCO_ADP_EvaluacionReq.Add(objReq);
                            db.SaveChanges();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return objDatos;
        }
        public DivicionesMenuDTO eliminarDiviciones(int id)
        {
            DivicionesMenuDTO objDatos = new DivicionesMenuDTO();
            try
            {
                var obj = db.tblCO_ADP_EvaluacionDiv.Where(y => y.id == id).FirstOrDefault();
                if (obj != null)
                {
                    db.tblCO_ADP_EvaluacionDiv.Remove(obj);
                    db.SaveChanges();
                    var lstDatos = db.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == id).ToList();
                    if (lstDatos.Count() != 0)
                    {
                        db.tblCO_ADP_EvaluacionReq.RemoveRange(lstDatos);
                        db.SaveChanges();
                    }

                    objDatos.estatus = 1;
                    objDatos.mensaje = "Se ha eliminado con exito!";
                }
                else
                {
                    objDatos.estatus = 2;
                    objDatos.mensaje = "No se ha podido eliminar!";
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return objDatos;
        }
        public List<RequerimientosDTO> obtenerRequerimientos(int idDiv, int? idAsignacion)
        {
            List<RequerimientosDTO> objDatos = new List<RequerimientosDTO>();
            try
            {
                if (!idAsignacion.HasValue)
                {
                    idAsignacion = 0;
                }

                var requerimientos = new List<tblCO_ADP_EvalSubContratistaDet>();

                var asignacion = db.tblCO_ADP_EvalSubConAsignacion.FirstOrDefault(x => x.id == idAsignacion);
                if (asignacion != null)
                {
                    var evaluacion = db.tblCO_ADP_EvalSubContratista.FirstOrDefault(x => x.idSubConAsignacion == asignacion.id && x.tipoEvaluacion == idDiv);
                    if (evaluacion != null)
                    {
                        requerimientos = evaluacion.detalleEvaluaciones;
                    }
                    //requerimientos = db.tblCO_ADP_EvalSubContratistaDet.Where(x => x.idEvaluacion == asignacion.id && x.tipoEvaluacion == idDiv).ToList();
                }

                objDatos = db.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == idDiv).Select(y => new RequerimientosDTO
                {
                    id = y.id,
                    idDiv = y.idDiv,
                    texto = y.texto,
                    inputFile = y.inputFile,
                    lblInput = y.lblInput,
                    tipoFile = y.tipoFile,
                    txtAComentario = y.txtAComentario,
                    txtPlaneacion = y.txtPlaneacion,
                    txtResponsable = y.txtResponsable,
                    txtFechaCompromiso = y.txtFechaCompromiso,
                    important = y.preguntarEvaluacion,
                }).ToList();

                for (int i = 0; i < objDatos.Count; i++)
                {
                    objDatos[i].tieneRetroalimentacion = asignacion == null ? false : !string.IsNullOrEmpty(requerimientos[i].fechaCompromiso);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return objDatos;
        }
        public List<DivicionesMenuDTO> obtenerDivicionesEvaluador()
        {
            List<DivicionesMenuDTO> lstDatos = new List<DivicionesMenuDTO>();
            List<DivicionesMenuDTO> lstDatos2 = new List<DivicionesMenuDTO>();
            try
            {
                lstDatos = db.tblCO_ADP_EvaluacionDiv.Where(r => r.esActivo && r.SubContratista == true).ToList().Select(y => new DivicionesMenuDTO
                {
                    id = y.id,
                    idbutton = y.idbutton,
                    idsection = y.idsection,
                    toltips = y.toltips,
                    descripcion = y.descripcion,
                    esActivo = y.esActivo,
                    orden = y.orden,
                    important = y.important
                }).ToList();
                lstDatos2 = lstDatos.OrderBy(n => n.orden).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return lstDatos2;
        }
        public List<DivicionesMenuDTO> obtenerDivicionesEvaluadorArchivos(int idPlantilla, int idAsignacion)
        {
            List<DivicionesMenuDTO> lstDatos = new List<DivicionesMenuDTO>();
            List<DivicionesMenuDTO> lstDatos2 = new List<DivicionesMenuDTO>();
            try
            {
                var lstIdDiviciones = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == idAsignacion).Select(y => y.tipoEvaluacion).ToList();

                var elementoTieneEvaluacion = new List<ElementosEvaluadosDTO>();

                var asignacion = db.tblCO_ADP_EvalSubConAsignacion.FirstOrDefault(x => x.id == idAsignacion);
                if (asignacion != null)
                {
                    var evaluacion = db.tblCO_ADP_EvalSubContratista.Where(x => x.idSubConAsignacion == asignacion.id).ToList();
                    foreach (var item in evaluacion)
                    {
                        var elementoEvaluacion = new ElementosEvaluadosDTO();
                        elementoEvaluacion.idElemento = item.tipoEvaluacion;
                        elementoEvaluacion.tieneEvaluacion = item.detalleEvaluaciones.Any(x => x.calificacion != 0);
                        elementoTieneEvaluacion.Add(elementoEvaluacion);
                    }
                }

                lstDatos2 = db.tblCO_ADP_EvaluacionDiv.Where(r => r.esActivo && r.idPlantilla == idPlantilla && r.SubContratista == false && lstIdDiviciones.Contains(r.id)).ToList().Select(y => new DivicionesMenuDTO
                {
                    id = y.id,
                    idbutton = y.idbutton,
                    idsection = y.idsection,
                    toltips = y.toltips,
                    descripcion = y.descripcion,
                    esActivo = y.esActivo,
                    orden = y.orden,
                    important = y.important
                }).OrderBy(x => x.orden).ToList();
                //lstDatos2 = lstDatos.OrderBy(n => n.orden).ToList();

                foreach (var item in lstDatos2)
                {
                    var elemento = elementoTieneEvaluacion.FirstOrDefault(x => x.idElemento == item.id);
                    item.estaEvaluado = elemento == null ? false : elemento.tieneEvaluacion;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return lstDatos2;
        }
        public byte[] DescargarArchivos(long idDet)
        {
            var resultado = new Dictionary<string, object>();
            Stream fileStream;
            try
            {
                string pathExamen = db.tblCO_ADP_EvalSubContratistaDet.Where(x => x.id == idDet).FirstOrDefault().rutaArchivo;
                fileStream = GlobalUtils.GetFileAsStream(pathExamen);

            }
            catch (Exception e)
            {
                fileStream = null;
            }

            //resultado.Add("nombreDescarga", version.nombre);
            //resultado.Add(SUCCESS, true);
            return ReadFully(fileStream);
        }
        public string getFileName(long idDet)
        {
            string fileName = "";
            try
            {
                string pathExamen = db.tblCO_ADP_EvalSubContratistaDet.Where(x => x.id == idDet).FirstOrDefault().rutaArchivo;
                if (pathExamen != null)
                {
                    fileName = pathExamen.Split('\\').Last();
                }
            }
            catch (Exception e)
            {
                fileName = "";
            }

            return fileName;
        }
        public static byte[] ReadFully(Stream input)
        {
            MemoryStream ms = new MemoryStream();
            if (input != null)
            {
                byte[] buffer = new byte[16 * 1024];
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
            }
            return ms.ToArray();
        }
        public SubContratistasDTO obtenerEvaluacionxReq(SubContratistasDTO parametros)
        {
            var obj = new SubContratistasDTO();
            try
            {
                obj = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.id == parametros.id).Select(y => new SubContratistasDTO
                {
                    id = y.id,
                    Calificacion = y.calificacion,
                    Comentario = y.comentario,
                    fechaCompromiso = y.fechaCompromiso,
                    planesDeAccion = y.planesDeAccion,
                    responsable = y.responsable
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return obj;
        }
        public SubContratistasDTO GuardarEvaluacion(SubContratistasDTO parametros)
        {
            SubContratistasDTO objretu = new SubContratistasDTO();
            try
            {

                var obj = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.id == parametros.id).FirstOrDefault();
                if (obj != null)
                {
                    obj.calificacion = parametros.Calificacion;
                    obj.comentario = parametros.Comentario;
                    //obj.evaluacionPendiente = true;
                    db.SaveChanges();
                    objretu.mensaje = "Se ha calificado con exito";
                    objretu.status = 1;



                }
            }
            catch (Exception)
            {
                objretu.mensaje = "algo a ocurrido mal comuniquese con el departamento de TI";
                objretu.status = 2;
            }
            return objretu;
        }
        public SubContratistasDTO obtenerPromegioEvaluacion(SubContratistasDTO parametros)
        {
            try
            {
                SubContratistasDTO promedio = new SubContratistasDTO();
                int calculo = 0;

                var obj = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.cc == parametros.cc && r.idSubContratista == parametros.idSubContratista).FirstOrDefault();
                if (obj != null)
                {
                    var objInicio = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == obj.id && r.tipoEvaluacion == parametros.tipoEvaluacion).FirstOrDefault();

                    if (objInicio != null)
                    {
                        var lstDeCalificaciones = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.calificacion > 0 && r.tipoEvaluacion == parametros.tipoEvaluacion && r.idEvaluacion == objInicio.id).ToList();
                        var lstReq = db.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == parametros.tipoEvaluacion).ToList();
                        if (lstDeCalificaciones.Count() != 0)
                        {
                            foreach (var item in lstDeCalificaciones)
                            {
                                calculo += item.calificacion;
                            }
                            promedio.promedio = Convert.ToDouble(calculo / lstReq.Count());
                        }
                        else
                        {
                            promedio = new SubContratistasDTO();
                            promedio.promedio = 0;
                        }

                    }
                    else
                    {
                        promedio = new SubContratistasDTO();
                        promedio.promedio = 0;
                    }
                }
                return promedio;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string nombreCompleto(int idSubcontratista)
        {
            tblPUsuarioDTO objUsuarioHijo = new tblPUsuarioDTO();
            string sql = @"SELECT * FROM tblP_Usuario WHERE tipo=10 AND estatus='true' AND id LIKE'%" + idSubcontratista + "%'";
            using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
            {
                ctx.Open();
                objUsuarioHijo = ctx.Query<tblPUsuarioDTO>(sql, null, null, true, 300).FirstOrDefault();
                ctx.Close();
            }
            return objUsuarioHijo.nombre_completo;
        }
        public Dictionary<string, object> ObtenerGraficaDeBarras(SubContratistasDTO parametros)
        {
            Dictionary<string, object> lstDatos = new Dictionary<string, object>();
            List<SubContratistasDTO> lstSubContratistas = new List<SubContratistasDTO>();
            var lstGrafica = db.tblCO_ADP_EvalSubConAsignacion.Where(r => (r.statusAutorizacion == 2 || r.statusAutorizacion == 3)).ToList();
            //var lstGrafica = db.tblCO_ADP_EvalSubContratista.ToList().Select(y => y.idSubContratista).Distinct();
            //var lst = lstGrafica.Distinct();

            if (lstGrafica.Count() != 0)
            {
                lstSubContratistas = lstGrafica.Select(y => new SubContratistasDTO
                {
                    responsable = nombreCompleto(y.idSubContratista),
                    promedio = obtenerPromedio(y.id),
                    Indicador = IndicadorEvaluacion(obtenerPromedio(y.id)),
                }).ToList();
            }

            var lstResultado22 = lstSubContratistas.GroupBy(r => r.Indicador).ToList();
            List<object> lstResutlado = new List<object>();
            foreach (var item in lstResultado22)
            {
                if (item.Select(y => y.Indicador).FirstOrDefault() == (int)EstatusControlObraEnum.Pesimo)
                {
                    var obj = new
                    {
                        name = "PESIMO",
                        y = item.Count(),
                        color = "#FA0101",
                    };
                    lstResutlado.Add(obj);
                }
                if (item.Select(y => y.Indicador).FirstOrDefault() == (int)EstatusControlObraEnum.Malo)
                {
                    var obj = new
                    {
                        name = "MALO",
                        y = item.Count(),
                        color = "#FA8001",
                    };
                    lstResutlado.Add(obj);
                }
                if (item.Select(y => y.Indicador).FirstOrDefault() == (int)EstatusControlObraEnum.Regular)
                {
                    var obj = new
                    {
                        name = "REGULAR",
                        y = item.Count(),
                        color = "#FAFF01",
                    };
                    lstResutlado.Add(obj);
                }
                if (item.Select(y => y.Indicador).FirstOrDefault() == (int)EstatusControlObraEnum.Aceptable)
                {
                    var obj = new
                    {
                        name = "ACEPTABLE",
                        y = item.Count(),
                        color = "#018001",
                    };
                    lstResutlado.Add(obj);
                }
                if (item.Select(y => y.Indicador).FirstOrDefault() == (int)EstatusControlObraEnum.Excediendo)
                {
                    var obj = new
                    {
                        name = "EXCEDIENDO LAS EXPECTATIVAS",
                        y = item.Count(),
                        color = "#0180FF",
                    };
                    lstResutlado.Add(obj);
                }
            }



            var listado = new
            {
                name = "",
                colorByPoint = true,
                data = lstResutlado,
            };
            List<object> lstResult2 = new List<object>();
            lstResult2.Add(listado);

            lstDatos.Add("gpxGraficaDeBarras", lstResult2);

            return lstDatos;

        }
        public int IndicadorEvaluacion(double total)
        {
            int tipo = 0;
            if (total >= 0 && total <= 25)
            {
                tipo = (int)EstatusControlObraEnum.Pesimo;
            }
            if (total >= 26 && total <= 50)
            {
                tipo = (int)EstatusControlObraEnum.Malo;
            }
            if (total >= 51 && total <= 70)
            {
                tipo = (int)EstatusControlObraEnum.Regular;
            }
            if (total >= 71 && total <= 90)
            {
                tipo = (int)EstatusControlObraEnum.Aceptable;
            }
            if (total >= 91 && total <= 100)
            {
                tipo = (int)EstatusControlObraEnum.Excediendo;
            }
            return tipo;
        }
        public double obtenerPromedio(int idAsignacion)
        {

            double stTotal = 0;
            double promedio2 = 0;
            double total = 0;
            var Promedio = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == idAsignacion).ToList();
            var TotalEvaluaciones = db.tblCO_ADP_EvaluacionDiv.Where(r => r.SubContratista == false).ToList().Count();
            if (Promedio.Count() != 0)
            {
                foreach (var item in Promedio)
                {
                    var TotalRequerimientos = db.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == item.tipoEvaluacion).ToList().Count();
                    var lstObjetos = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.idEvaluacion == item.id && r.calificacion != 0).ToList();
                    if (lstObjetos.Count() != 0)
                    {
                        foreach (var item2 in lstObjetos)
                        {
                            total += item2.calificacion;
                        }
                    }
                    if (total == 0)
                    {
                        total = 0;
                    }
                    else
                    {
                        promedio2 += total / TotalRequerimientos;
                        stTotal += promedio2;
                    }
                    promedio2 = 0;
                    total = 0;
                }
                if (stTotal == 0)
                {
                    stTotal = 0;
                }
                else
                {
                    stTotal = stTotal / TotalEvaluaciones;
                }
            }
            return stTotal;
        }
        public List<SubContratistasDTO> obtenerPromediosxSubcontratista(SubContratistasDTO parametros)
        {
            List<SubContratistasDTO> lstSubContratistas = new List<SubContratistasDTO>();
            var lstGraficaAsig = db.tblCO_ADP_EvalSubConAsignacion.Where(r => (r.statusAutorizacion == 2 || r.statusAutorizacion == 3)).ToList();

            if (lstGraficaAsig.Count() != 0)
            {
                lstSubContratistas = lstGraficaAsig.Select(y => new SubContratistasDTO
                {
                    id = y.id,
                    idSubContratista = y.idSubContratista,
                    centroCostos = y.cc,
                    cc = y.cc + " " + _context.tblP_CC.Where(r => r.cc == y.cc).FirstOrDefault().descripcion,
                    responsable = db.tblX_SubContratista.Where(r => r.id == y.idSubContratista).Select(n => n.nombre).FirstOrDefault(),
                    promedio = obtenerPromedio(y.id),
                    Indicador = IndicadorEvaluacion(obtenerPromedio(y.id)),
                }).ToList();
            }
            return lstSubContratistas;
        }
        public List<ContratoSubContratistaDTO> obtenerTblSubContratista(string AreaCuenta)
        {
            List<ContratoSubContratistaDTO> lstContrato = new List<ContratoSubContratistaDTO>();
            if (AreaCuenta == null)
            {
                AreaCuenta = "";
            }
            var lstSubContratistas = db.tblX_SubContratista.Where(r => r.estatus).ToList();
            var lstContratos = db.tblX_Contrato.Where(r => r.estatus
                                                                                    && (AreaCuenta == "" ? r.cc == r.cc : r.cc == AreaCuenta)
                                                                                    ).ToList().Select(y => y.subcontratistaID).ToList();
            lstContrato = lstSubContratistas.Where(y => lstContratos.Contains(y.id)).Select(y => new ContratoSubContratistaDTO
            {
                id = y.id,
                nombre = y.nombre,
                direccion = y.direccion,
            }).ToList();



            return lstContrato;
        }
        public bool ObtenerEvaluacionPendiente(tblPUsuarioDTO ObjUsuario)
        {
            bool EvaluacionPendiente = false;
            try
            {
                if (ObjUsuario.tipo == 3)
                {
                    var objSubContratistaID = db.tblX_SubContratista.Where(r => r.rfc == ObjUsuario._user).FirstOrDefault();
                    int _idUsuario = objSubContratistaID.id;
                    var obj = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.idSubContratista == _idUsuario && r.esActivo == true && r.statusAutorizacion == 2).FirstOrDefault();
                    if (obj != null)
                    {
                        EvaluacionPendiente = true;
                    }
                }
                else
                {
                    var objUsuarioHijo = db.tblP_Usuario.Where(x => x.id == ObjUsuario.id).FirstOrDefault();
                    var objUsuarioPadre = db.tblP_Usuario.Where(x => x.id == objUsuarioHijo.idPadre).FirstOrDefault();
                    var objSubContratistaID = db.tblX_SubContratista.Where(r => r.rfc == objUsuarioPadre._user).FirstOrDefault();
                    int _idUsuario = objSubContratistaID.id;
                    var obj = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.idSubContratista == _idUsuario && r.esActivo == true && r.statusAutorizacion == 2).FirstOrDefault();
                    if (obj != null)
                    {
                        EvaluacionPendiente = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return EvaluacionPendiente;
        }
        public List<ContratoSubContratistaDTO> obtenerContratistasConContrato(string AreaCuenta, int subcontratista, int Estatus, tblPUsuarioDTO ObjUsuario)
        {
            List<ContratoSubContratistaDTO> lstRetornar = new List<ContratoSubContratistaDTO>();
            List<ContratoSubContratistaDTO> lstRetornarFiltrandoPasado = new List<ContratoSubContratistaDTO>();
            ObjUsuario.tipo = 10;
            //sigoplanGenericDAO _cont = new sigoplanGenericDAO();
            if (AreaCuenta == null)
            {
                AreaCuenta = "";
            }

            
                try
                {
                    if (ObjUsuario.tipo == 3)
                    {
                        if (Estatus == 0)
                        {
                            #region optimización
                            var lstEstatusNo2 = new List<int>
                        {
                            2, 5
                        };

                            var lstAsignaciones2 = db.tblCO_ADP_EvalSubConAsignacion.Where(x => lstEstatusNo2.Contains(x.statusAutorizacion) && x.esActivo).Select(x => x.id).ToList();

                            var lstContratos2 = db.tblX_Contrato
                                .Where(x =>
                                    (string.IsNullOrEmpty(AreaCuenta) ? true : x.cc == AreaCuenta) &&
                                    (subcontratista == 0 ? true : x.subcontratistaID == subcontratista) &&
                                    x.estatus)
                                .Select(x => new ContratoSubContratistaDTO
                                {
                                    numeroProveedor = x.subcontratista.numeroProveedor,
                                    nombre = x.subcontratista.nombre,
                                    direccion = x.subcontratista.direccion,
                                    nombreCorto = x.subcontratista.nombreCorto,
                                    codigoPostal = x.subcontratista.codigoPostal,
                                    cc = x.cc,
                                    idSubContratista = x.subcontratistaID,
                                    tipoUsuario = ObjUsuario.tipo,
                                    numeroContrato = x.numeroContrato,
                                    idContrato = x.id,
                                    evaluaciones = x.evaluaciones.Select(y => new EvaluacionesDTO
                                    {
                                        cc = y.cc,
                                        esActivo = y.esActivo,
                                        id = y.id,
                                        statusAutorizacion = y.statusAutorizacion,
                                        evaluacionAnteriorid = y.evaluacionAnteriorid,
                                        statusVobo = y.statusVobo,
                                        evaluacionesConAsignacion = y.evalSubContratista.Select(z => new EvaluacionConAsignacionDTO
                                        {
                                            id = z.id,
                                            evaluacionPendiente = z.evaluacionPendiente,
                                            evaluacionesPendiente = z.detalleEvaluaciones.Select(m => m.evaluacionPendiente).ToList()
                                        }).ToList()
                                    }).ToList()
                                }).ToList();

                            var ccContratos = lstContratos2.Select(x => x.cc).Distinct().ToList();
                            var _ccs = _context.tblP_CC.Where(x => ccContratos.Contains(x.cc)).ToList();

                            var lstPlantillas = db.tblCO_ADP_EvaluacionPlantilla.Where(x => !string.IsNullOrEmpty(x.contratos)).ToList();
                            var plantillaDefault = db.tblCO_ADP_EvaluacionPlantilla.First(x => x.nombrePlantilla == "default").id;

                            foreach (var contrato in lstContratos2)
                            {
                                var cc = _ccs.FirstOrDefault(x => x.cc == contrato.cc);

                                var evaluacion = contrato.evaluaciones.FirstOrDefault(x => x.esActivo);
                                var asignacion = contrato.evaluaciones.Where(x => x.esActivo).OrderByDescending(x => x.id).FirstOrDefault();
                                var estatusAutorizacion = contrato.evaluaciones.Where(x => !x.esActivo).ToList();
                                var status = evaluacion != null ? 2 : estatusAutorizacion.Count != 0 ? 3 : 0;

                                var idPlantilla = 0;
                                foreach (var plantilla in lstPlantillas)
                                {
                                    var contratosPlantilla = plantilla.contratos.Split(',');
                                    if (contratosPlantilla.Count() > 0)
                                    {
                                        var plantillaAsignada = contratosPlantilla.FirstOrDefault(x => !string.IsNullOrEmpty(x) && Convert.ToInt32(x) == contrato.idContrato);
                                        if (plantillaAsignada != null)
                                        {
                                            idPlantilla = plantilla.id;
                                            break;
                                        }
                                    }
                                }
                                if (idPlantilla == 0)
                                {
                                    idPlantilla = plantillaDefault;
                                }

                                int sta = 0;
                                if (evaluacion != null)
                                {
                                    var objSubCon = evaluacion.evaluacionesConAsignacion;
                                    if (objSubCon.Count != 0)
                                    {
                                        foreach (var item in objSubCon)
                                        {
                                            if (item.evaluacionesPendiente.Any(x => x))
                                            {
                                                sta = 1;
                                            }
                                            else
                                            {
                                                sta = 0;
                                                break;
                                            }
                                        }
                                    }
                                }

                                contrato.id = evaluacion != null ? evaluacion.id : 0;
                                contrato.idAsignacion = asignacion != null ? asignacion.id : 0;
                                contrato.descripcion = cc != null ? cc.descripcion : "";
                                contrato.descripcioncc = contrato.cc + (!string.IsNullOrEmpty(contrato.descripcion) ? " " + cc.descripcion : "");
                                contrato.estatusAutorizacion = estatusAutorizacion.Count > 0 ? estatusAutorizacion.First().statusAutorizacion : 0;
                                contrato.evaluacionAnteriorid = evaluacion != null ? evaluacion.evaluacionAnteriorid : 0;
                                contrato.evaluacionActual = sta;
                                contrato.existeEvaluacionAnterior = estatusAutorizacion.Count != 0 ? true : false;
                                contrato.existeEvaluacionPendiente = evaluacion == null ? false : true;
                                contrato.perdiodoFechas = obtenerPeriodoFechas(asignacion != null ? asignacion.id : 0);
                                contrato.status = status;
                                contrato.statusVobo = evaluacion == null ? false : evaluacion.statusVobo;
                                contrato.idPlantilla = idPlantilla;
                            }

                            lstRetornar = lstContratos2.Where(x => !lstAsignaciones2.Contains(x.id)).ToList();
                            #endregion
                        }
                        else if (Estatus == 2)
                        {
                            #region optimización
                            var lstContratos2 = db.tblX_Contrato
                                .Where(x =>
                                    (string.IsNullOrEmpty(AreaCuenta) ? true : x.cc == AreaCuenta) &&
                                    (subcontratista == 0 ? true : x.subcontratistaID == subcontratista) &&
                                    x.estatus)
                                .Select(x => new ContratoSubContratistaDTO
                                {
                                    numeroProveedor = x.subcontratista.numeroProveedor,
                                    nombre = x.subcontratista.nombre,
                                    direccion = x.subcontratista.direccion,
                                    nombreCorto = x.subcontratista.nombreCorto,
                                    codigoPostal = x.subcontratista.codigoPostal,
                                    cc = x.cc,
                                    idSubContratista = x.subcontratistaID,
                                    tipoUsuario = ObjUsuario.tipo,
                                    numeroContrato = x.numeroContrato,
                                    idContrato = x.id,
                                    evaluaciones = x.evaluaciones.Select(y => new EvaluacionesDTO
                                    {
                                        cc = y.cc,
                                        esActivo = y.esActivo,
                                        id = y.id,
                                        statusAutorizacion = y.statusAutorizacion,
                                        evaluacionAnteriorid = y.evaluacionAnteriorid,
                                        statusVobo = y.statusVobo,
                                        evaluacionesConAsignacion = y.evalSubContratista.Select(z => new EvaluacionConAsignacionDTO
                                        {
                                            id = z.id,
                                            evaluacionPendiente = z.evaluacionPendiente,
                                            evaluacionesPendiente = z.detalleEvaluaciones.Select(m => m.evaluacionPendiente).ToList()
                                        }).ToList()
                                    }).ToList()
                                }).ToList();

                            var ccContratos = lstContratos2.Select(x => x.cc).Distinct().ToList();
                            var _ccs = _context.tblP_CC.Where(x => ccContratos.Contains(x.cc)).ToList();

                            var lstPlantillas = db.tblCO_ADP_EvaluacionPlantilla.Where(x => !string.IsNullOrEmpty(x.contratos)).ToList();
                            var plantillaDefault = db.tblCO_ADP_EvaluacionPlantilla.First(x => x.nombrePlantilla == "default").id;

                            foreach (var contrato in lstContratos2)
                            {
                                var cc = _ccs.FirstOrDefault(x => x.cc == contrato.cc);

                                var evaluacion = contrato.evaluaciones.FirstOrDefault(x => x.esActivo);
                                var asignacion = contrato.evaluaciones.Where(x => x.esActivo).OrderByDescending(x => x.id).FirstOrDefault();
                                var estatusAutorizacion = contrato.evaluaciones.Where(x => x.esActivo).ToList();
                                var status = evaluacion != null ? 2 : contrato.evaluaciones.Where(x => !x.esActivo).Count() != 0 ? 3 : 0;

                                var idPlantilla = 0;
                                foreach (var plantilla in lstPlantillas)
                                {
                                    var contratosPlantilla = plantilla.contratos.Split(',');
                                    if (contratosPlantilla.Count() > 0)
                                    {
                                        var plantillaAsignada = contratosPlantilla.FirstOrDefault(x => !string.IsNullOrEmpty(x) && Convert.ToInt32(x) == contrato.idContrato);
                                        if (plantillaAsignada != null)
                                        {
                                            idPlantilla = plantilla.id;
                                            break;
                                        }
                                    }
                                }
                                if (idPlantilla == 0)
                                {
                                    idPlantilla = plantillaDefault;
                                }

                                int sta = 0;
                                if (evaluacion != null)
                                {
                                    var objSubCon = evaluacion.evaluacionesConAsignacion;
                                    if (objSubCon.Count != 0)
                                    {
                                        foreach (var item in objSubCon)
                                        {
                                            if (item.evaluacionesPendiente.Any(x => x))
                                            {
                                                sta = 1;
                                            }
                                            else
                                            {
                                                sta = 0;
                                                break;
                                            }
                                        }
                                    }
                                }

                                contrato.id = evaluacion != null ? evaluacion.id : 0;
                                contrato.idAsignacion = asignacion != null ? asignacion.id : 0;
                                contrato.descripcion = cc != null ? cc.descripcion : "";
                                contrato.descripcioncc = contrato.cc + (!string.IsNullOrEmpty(contrato.descripcion) ? " " + cc.descripcion : "");
                                contrato.estatusAutorizacion = estatusAutorizacion.Count > 0 ? estatusAutorizacion.First().statusAutorizacion : 0;
                                contrato.evaluacionAnteriorid = evaluacion != null ? evaluacion.evaluacionAnteriorid : 0;
                                contrato.evaluacionActual = sta;
                                contrato.existeEvaluacionAnterior = estatusAutorizacion.Count != 0 ? true : false;
                                contrato.existeEvaluacionPendiente = evaluacion == null ? false : true;
                                contrato.perdiodoFechas = obtenerPeriodoFechas(asignacion != null ? asignacion.id : 0);
                                contrato.status = status;
                                contrato.statusVobo = evaluacion == null ? false : evaluacion.statusVobo;
                                contrato.idPlantilla = idPlantilla;
                            }

                            lstRetornar = lstContratos2;
                            #endregion
                        }
                        else
                        {
                            #region optimización
                            lstRetornar = db.tblCO_ADP_EvalSubConAsignacion
                                .Where(x =>
                                    (string.IsNullOrEmpty(AreaCuenta) ? true : x.cc == AreaCuenta) &&
                                    (subcontratista == 0 ? true : x.idSubContratista == subcontratista) &&
                                    x.statusAutorizacion == Estatus)
                                .Select(x => new ContratoSubContratistaDTO
                                {
                                    id = x.id,
                                    numeroProveedor = x.subcontratista.numeroProveedor,
                                    nombre = x.subcontratista.nombre,
                                    direccion = x.subcontratista.direccion,
                                    nombreCorto = x.subcontratista.nombreCorto,
                                    codigoPostal = x.subcontratista.codigoPostal,
                                    idAsignacion = x.id,
                                    cc = x.cc,
                                    fechaCreacion = (DateTime)x.fechaCreacion,
                                    estatusAutorizacion = x.statusAutorizacion,
                                    idSubContratista = x.idSubContratista,
                                    status = 3,
                                    numeroContrato = "",
                                    tipoUsuario = ObjUsuario.tipo,
                                    statusVobo = x.statusVobo,
                                    idContrato = x.idContrato
                                }).ToList();

                            var ccContratos = lstRetornar.Select(x => x.cc).Distinct().ToList();
                            var _ccs = _context.tblP_CC.Where(x => ccContratos.Contains(x.cc)).ToList();

                            var lstPlantillas = db.tblCO_ADP_EvaluacionPlantilla.Where(x => !string.IsNullOrEmpty(x.contratos)).ToList();
                            var plantillaDefault = db.tblCO_ADP_EvaluacionPlantilla.First(x => x.nombrePlantilla == "default").id;

                            foreach (var asignacion in lstRetornar)
                            {
                                var cc = _ccs.FirstOrDefault(x => x.cc == asignacion.cc);

                                var evaluaciones = db.tblCO_ADP_EvalSubConAsignacion.Where(x => x.idContrato == asignacion.idContrato).ToList();
                                var evaluacionAnterior = evaluaciones.FirstOrDefault(x => x.esActivo);

                                int sta = 0;
                                foreach (var item in evaluaciones.Where(x => x.esActivo))
                                {
                                    if (item.evalSubContratista.Any(x => x.evaluacionPendiente))
                                    {
                                        sta = 1;
                                    }
                                    else
                                    {
                                        sta = 0;
                                    }
                                }

                                var idPlantilla = 0;
                                foreach (var plantilla in lstPlantillas)
                                {
                                    var contratosPlantilla = plantilla.contratos.Split(',');
                                    if (contratosPlantilla.Count() > 0)
                                    {
                                        var plantillaAsignada = contratosPlantilla.FirstOrDefault(x => !string.IsNullOrEmpty(x) && Convert.ToInt32(x) == asignacion.idContrato);
                                        if (plantillaAsignada != null)
                                        {
                                            idPlantilla = plantilla.id;
                                            break;
                                        }
                                    }
                                }
                                if (idPlantilla == 0)
                                {
                                    idPlantilla = plantillaDefault;
                                }

                                asignacion.descripcion = cc != null ? cc.descripcion : "";
                                asignacion.perdiodoFechas = obtenerPeriodoFechas(asignacion != null ? asignacion.id : 0);
                                asignacion.descripcioncc = asignacion.cc + (!string.IsNullOrEmpty(asignacion.descripcion) ? " " + cc.descripcion : "");
                                asignacion.evaluacionAnteriorid = evaluacionAnterior != null ? evaluacionAnterior.evaluacionAnteriorid : 0;
                                asignacion.evaluacionActual = sta;
                                asignacion.existeEvaluacionAnterior = evaluaciones.Where(x => x.esActivo).Count() != 0 ? true : false;
                                asignacion.existeEvaluacionPendiente = evaluacionAnterior == null ? false : true;
                                asignacion.idPlantilla = idPlantilla;
                            }
                            #endregion
                        }
                    }
                    else if (ObjUsuario.tipo == 14)
                    {
                        if (Estatus == 0)
                        {
                            #region optimización
                            var lstEstatusNo2 = new List<int>
                        {
                            2, 5
                        };

                            var lstAsignaciones2 = db.tblCO_ADP_EvalSubConAsignacion.Where(x => lstEstatusNo2.Contains(x.statusAutorizacion) && x.esActivo).Select(x => x.id).ToList();

                            var lstContratos2 = db.tblX_Contrato
                                .Where(x =>
                                    (string.IsNullOrEmpty(AreaCuenta) ? true : x.cc == AreaCuenta) &&
                                    x.estatus)
                                .Select(x => new ContratoSubContratistaDTO
                                {
                                    numeroProveedor = x.subcontratista.numeroProveedor,
                                    nombre = x.subcontratista.nombre,
                                    direccion = x.subcontratista.direccion,
                                    nombreCorto = x.subcontratista.nombreCorto,
                                    codigoPostal = x.subcontratista.codigoPostal,
                                    cc = x.cc,
                                    idSubContratista = x.subcontratistaID,
                                    tipoUsuario = ObjUsuario.tipo,
                                    numeroContrato = x.numeroContrato,
                                    idContrato = x.id,
                                    evaluaciones = x.evaluaciones.Select(y => new EvaluacionesDTO
                                    {
                                        cc = y.cc,
                                        esActivo = y.esActivo,
                                        id = y.id,
                                        statusAutorizacion = y.statusAutorizacion,
                                        evaluacionAnteriorid = y.evaluacionAnteriorid,
                                        statusVobo = y.statusVobo,
                                        evaluacionesConAsignacion = y.evalSubContratista.Select(z => new EvaluacionConAsignacionDTO
                                        {
                                            id = z.id,
                                            evaluacionPendiente = z.evaluacionPendiente,
                                            evaluacionesPendiente = z.detalleEvaluaciones.Select(m => m.evaluacionPendiente).ToList()
                                        }).ToList()
                                    }).ToList()
                                }).ToList();

                            var ccContratos = lstContratos2.Select(x => x.cc).Distinct().ToList();
                            var _ccs = _context.tblP_CC.Where(x => ccContratos.Contains(x.cc)).ToList();

                            var lstPlantillas = db.tblCO_ADP_EvaluacionPlantilla.Where(x => !string.IsNullOrEmpty(x.contratos)).ToList();
                            var plantillaDefault = db.tblCO_ADP_EvaluacionPlantilla.First(x => x.nombrePlantilla == "default").id;

                            foreach (var contrato in lstContratos2)
                            {
                                var cc = _ccs.FirstOrDefault(x => x.cc == contrato.cc);

                                var evaluacion = contrato.evaluaciones.FirstOrDefault(x => x.esActivo);
                                var asignacion = contrato.evaluaciones.Where(x => x.esActivo).OrderByDescending(x => x.id).FirstOrDefault();
                                var estatusAutorizacion = contrato.evaluaciones.Where(x => !x.esActivo).ToList();
                                var status = evaluacion != null ? 2 : estatusAutorizacion.Count != 0 ? 3 : 0;

                                var idPlantilla = 0;
                                foreach (var plantilla in lstPlantillas)
                                {
                                    var contratosPlantilla = plantilla.contratos.Split(',');
                                    if (contratosPlantilla.Count() > 0)
                                    {
                                        var plantillaAsignada = contratosPlantilla.FirstOrDefault(x => !string.IsNullOrEmpty(x) && Convert.ToInt32(x) == contrato.idContrato);
                                        if (plantillaAsignada != null)
                                        {
                                            idPlantilla = plantilla.id;
                                            break;
                                        }
                                    }
                                }
                                if (idPlantilla == 0)
                                {
                                    idPlantilla = plantillaDefault;
                                }

                                int sta = 0;
                                if (evaluacion != null)
                                {
                                    var objSubCon = evaluacion.evaluacionesConAsignacion;
                                    if (objSubCon.Count != 0)
                                    {
                                        foreach (var item in objSubCon)
                                        {
                                            if (item.evaluacionesPendiente.Any(x => x))
                                            {
                                                sta = 1;
                                            }
                                            else
                                            {
                                                sta = 0;
                                                break;
                                            }
                                        }
                                    }
                                }

                                contrato.id = evaluacion != null ? evaluacion.id : 0;
                                contrato.idAsignacion = asignacion != null ? asignacion.id : 0;
                                contrato.descripcion = cc != null ? cc.descripcion : "";
                                contrato.descripcioncc = contrato.cc + (!string.IsNullOrEmpty(contrato.descripcion) ? " " + cc.descripcion : "");
                                contrato.estatusAutorizacion = estatusAutorizacion.Count > 0 ? estatusAutorizacion.First().statusAutorizacion : 0;
                                contrato.evaluacionAnteriorid = evaluacion != null ? evaluacion.evaluacionAnteriorid : 0;
                                contrato.evaluacionActual = sta;
                                contrato.existeEvaluacionAnterior = estatusAutorizacion.Count != 0 ? true : false;
                                contrato.existeEvaluacionPendiente = evaluacion == null ? false : true;
                                contrato.perdiodoFechas = obtenerPeriodoFechas(asignacion != null ? asignacion.id : 0);
                                contrato.status = status;
                                contrato.statusVobo = evaluacion == null ? false : evaluacion.statusVobo;
                                contrato.idPlantilla = idPlantilla;
                            }

                            lstRetornar = lstContratos2.Where(x => !lstAsignaciones2.Contains(x.id)).ToList();
                            #endregion
                        }
                        else if (Estatus == 2)
                        {
                            #region optimización
                            var lstContratos2 = db.tblX_Contrato
                                .Where(x =>
                                    (string.IsNullOrEmpty(AreaCuenta) ? true : x.cc == AreaCuenta) &&
                                    x.estatus)
                                .Select(x => new ContratoSubContratistaDTO
                                {
                                    numeroProveedor = x.subcontratista.numeroProveedor,
                                    nombre = x.subcontratista.nombre,
                                    direccion = x.subcontratista.direccion,
                                    nombreCorto = x.subcontratista.nombreCorto,
                                    codigoPostal = x.subcontratista.codigoPostal,
                                    cc = x.cc,
                                    idSubContratista = x.subcontratistaID,
                                    tipoUsuario = ObjUsuario.tipo,
                                    numeroContrato = x.numeroContrato,
                                    idContrato = x.id,
                                    evaluaciones = x.evaluaciones.Select(y => new EvaluacionesDTO
                                    {
                                        cc = y.cc,
                                        esActivo = y.esActivo,
                                        id = y.id,
                                        statusAutorizacion = y.statusAutorizacion,
                                        evaluacionAnteriorid = y.evaluacionAnteriorid,
                                        statusVobo = y.statusVobo,
                                        evaluacionesConAsignacion = y.evalSubContratista.Select(z => new EvaluacionConAsignacionDTO
                                        {
                                            id = z.id,
                                            evaluacionPendiente = z.evaluacionPendiente,
                                            evaluacionesPendiente = z.detalleEvaluaciones.Select(m => m.evaluacionPendiente).ToList()
                                        }).ToList()
                                    }).ToList()
                                }).ToList();

                            var ccContratos = lstContratos2.Select(x => x.cc).Distinct().ToList();
                            var _ccs = _context.tblP_CC.Where(x => ccContratos.Contains(x.cc)).ToList();

                            var lstPlantillas = db.tblCO_ADP_EvaluacionPlantilla.Where(x => !string.IsNullOrEmpty(x.contratos)).ToList();
                            var plantillaDefault = db.tblCO_ADP_EvaluacionPlantilla.First(x => x.nombrePlantilla == "default").id;

                            foreach (var contrato in lstContratos2)
                            {
                                var cc = _ccs.FirstOrDefault(x => x.cc == contrato.cc);

                                var evaluacion = contrato.evaluaciones.FirstOrDefault(x => x.esActivo);
                                var asignacion = contrato.evaluaciones.Where(x => x.esActivo).OrderByDescending(x => x.id).FirstOrDefault();
                                var estatusAutorizacion = contrato.evaluaciones.Where(x => x.esActivo).ToList();
                                var status = evaluacion != null ? 2 : contrato.evaluaciones.Where(x => !x.esActivo).Count() != 0 ? 3 : 0;

                                var idPlantilla = 0;
                                foreach (var plantilla in lstPlantillas)
                                {
                                    var contratosPlantilla = plantilla.contratos.Split(',');
                                    if (contratosPlantilla.Count() > 0)
                                    {
                                        var plantillaAsignada = contratosPlantilla.FirstOrDefault(x => !string.IsNullOrEmpty(x) && Convert.ToInt32(x) == contrato.idContrato);
                                        if (plantillaAsignada != null)
                                        {
                                            idPlantilla = plantilla.id;
                                            break;
                                        }
                                    }
                                }
                                if (idPlantilla == 0)
                                {
                                    idPlantilla = plantillaDefault;
                                }

                                int sta = 0;
                                if (evaluacion != null)
                                {
                                    var objSubCon = evaluacion.evaluacionesConAsignacion;
                                    if (objSubCon.Count != 0)
                                    {
                                        foreach (var item in objSubCon)
                                        {
                                            if (item.evaluacionesPendiente.Any(x => x))
                                            {
                                                sta = 1;
                                            }
                                            else
                                            {
                                                sta = 0;
                                                break;
                                            }
                                        }
                                    }
                                }

                                contrato.id = evaluacion != null ? evaluacion.id : 0;
                                contrato.idAsignacion = asignacion != null ? asignacion.id : 0;
                                contrato.descripcion = cc != null ? cc.descripcion : "";
                                contrato.descripcioncc = contrato.cc + (!string.IsNullOrEmpty(contrato.descripcion) ? " " + cc.descripcion : "");
                                contrato.estatusAutorizacion = estatusAutorizacion.Count > 0 ? estatusAutorizacion.First().statusAutorizacion : 0;
                                contrato.evaluacionAnteriorid = evaluacion != null ? evaluacion.evaluacionAnteriorid : 0;
                                contrato.evaluacionActual = sta;
                                contrato.existeEvaluacionAnterior = estatusAutorizacion.Count != 0 ? true : false;
                                contrato.existeEvaluacionPendiente = evaluacion == null ? false : true;
                                contrato.perdiodoFechas = obtenerPeriodoFechas(asignacion != null ? asignacion.id : 0);
                                contrato.status = status;
                                contrato.statusVobo = evaluacion == null ? false : evaluacion.statusVobo;
                                contrato.idPlantilla = idPlantilla;
                            }

                            lstRetornar = lstContratos2;
                            #endregion
                        }
                        else
                        {
                            #region optimización
                            lstRetornar = db.tblCO_ADP_EvalSubConAsignacion
                                .Where(x =>
                                    (string.IsNullOrEmpty(AreaCuenta) ? true : x.cc == AreaCuenta) &&
                                    x.statusAutorizacion == Estatus)
                                .Select(x => new ContratoSubContratistaDTO
                                {
                                    id = x.id,
                                    numeroProveedor = x.subcontratista.numeroProveedor,
                                    nombre = x.subcontratista.nombre,
                                    direccion = x.subcontratista.direccion,
                                    nombreCorto = x.subcontratista.nombreCorto,
                                    codigoPostal = x.subcontratista.codigoPostal,
                                    idAsignacion = x.id,
                                    cc = x.cc,
                                    fechaCreacion = (DateTime)x.fechaCreacion,
                                    estatusAutorizacion = x.statusAutorizacion,
                                    idSubContratista = x.idSubContratista,
                                    status = 3,
                                    numeroContrato = "",
                                    tipoUsuario = ObjUsuario.tipo,
                                    statusVobo = x.statusVobo,
                                    idContrato = x.idContrato
                                }).ToList();

                            var ccContratos = lstRetornar.Select(x => x.cc).Distinct().ToList();
                            var _ccs = _context.tblP_CC.Where(x => ccContratos.Contains(x.cc)).ToList();

                            var lstPlantillas = db.tblCO_ADP_EvaluacionPlantilla.Where(x => !string.IsNullOrEmpty(x.contratos)).ToList();
                            var plantillaDefault = db.tblCO_ADP_EvaluacionPlantilla.First(x => x.nombrePlantilla == "default").id;

                            foreach (var asignacion in lstRetornar)
                            {
                                var cc = _ccs.FirstOrDefault(x => x.cc == asignacion.cc);

                                var evaluaciones = db.tblCO_ADP_EvalSubConAsignacion.Where(x => x.idContrato == asignacion.idContrato).ToList();
                                var evaluacionAnterior = evaluaciones.FirstOrDefault(x => x.esActivo);

                                int sta = 0;
                                foreach (var item in evaluaciones.Where(x => x.esActivo))
                                {
                                    if (item.evalSubContratista.Any(x => x.evaluacionPendiente))
                                    {
                                        sta = 1;
                                    }
                                    else
                                    {
                                        sta = 0;
                                    }
                                }

                                var idPlantilla = 0;
                                foreach (var plantilla in lstPlantillas)
                                {
                                    var contratosPlantilla = plantilla.contratos.Split(',');
                                    if (contratosPlantilla.Count() > 0)
                                    {
                                        var plantillaAsignada = contratosPlantilla.FirstOrDefault(x => !string.IsNullOrEmpty(x) && Convert.ToInt32(x) == asignacion.idContrato);
                                        if (plantillaAsignada != null)
                                        {
                                            idPlantilla = plantilla.id;
                                            break;
                                        }
                                    }
                                }
                                if (idPlantilla == 0)
                                {
                                    idPlantilla = plantillaDefault;
                                }

                                asignacion.descripcion = cc != null ? cc.descripcion : "";
                                asignacion.perdiodoFechas = obtenerPeriodoFechas(asignacion != null ? asignacion.id : 0);
                                asignacion.descripcioncc = asignacion.cc + (!string.IsNullOrEmpty(asignacion.descripcion) ? " " + cc.descripcion : "");
                                asignacion.evaluacionAnteriorid = evaluacionAnterior != null ? evaluacionAnterior.evaluacionAnteriorid : 0;
                                asignacion.evaluacionActual = sta;
                                asignacion.existeEvaluacionAnterior = evaluaciones.Where(x => x.esActivo).Count() != 0 ? true : false;
                                asignacion.existeEvaluacionPendiente = evaluacionAnterior == null ? false : true;
                                asignacion.idPlantilla = idPlantilla;
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        if (Estatus == 0)
                        {
                            #region optimización
                            var lstEstatusNo2 = new List<int>
                        {
                            2, 5
                        };

                            var lstAsignaciones2 = db.tblCO_ADP_EvalSubConAsignacion.Where(x => lstEstatusNo2.Contains(x.statusAutorizacion) && x.esActivo).Select(x => x.id).ToList();

                            var lstContratos2 = db.tblX_Contrato
                                .Where(x =>
                                    (string.IsNullOrEmpty(AreaCuenta) ? true : x.cc == AreaCuenta) &&
                                    (subcontratista == 0 ? true : x.subcontratistaID == subcontratista) &&
                                    x.estatus)
                                .Select(x => new ContratoSubContratistaDTO
                                {
                                    numeroProveedor = x.subcontratista.numeroProveedor,
                                    nombre = x.subcontratista.nombre,
                                    direccion = x.subcontratista.direccion,
                                    nombreCorto = x.subcontratista.nombreCorto,
                                    codigoPostal = x.subcontratista.codigoPostal,
                                    cc = x.cc,
                                    idSubContratista = x.subcontratistaID,
                                    tipoUsuario = ObjUsuario.tipo,
                                    numeroContrato = x.numeroContrato,
                                    idContrato = x.id,
                                    evaluaciones = x.evaluaciones.Select(y => new EvaluacionesDTO
                                    {
                                        cc = y.cc,
                                        esActivo = y.esActivo,
                                        id = y.id,
                                        statusAutorizacion = y.statusAutorizacion,
                                        evaluacionAnteriorid = y.evaluacionAnteriorid,
                                        statusVobo = y.statusVobo,
                                        evaluacionesConAsignacion = y.evalSubContratista.Select(z => new EvaluacionConAsignacionDTO
                                        {
                                            id = z.id,
                                            evaluacionPendiente = z.evaluacionPendiente,
                                            evaluacionesPendiente = z.detalleEvaluaciones.Select(m => m.evaluacionPendiente).ToList()
                                        }).ToList()
                                    }).ToList()
                                }).ToList();

                            var ccContratos = lstContratos2.Select(x => x.cc).Distinct().ToList();
                            var _ccs = _context.tblP_CC.Where(x => ccContratos.Contains(x.cc)).ToList();

                            var lstPlantillas = db.tblCO_ADP_EvaluacionPlantilla.Where(x => !string.IsNullOrEmpty(x.contratos)).ToList();
                            var plantillaDefault = db.tblCO_ADP_EvaluacionPlantilla.First(x => x.nombrePlantilla == "default").id;

                            foreach (var contrato in lstContratos2)
                            {
                                var cc = _ccs.FirstOrDefault(x => x.cc == contrato.cc);

                                var evaluacion = contrato.evaluaciones.FirstOrDefault(x => x.esActivo);
                                var asignacion = contrato.evaluaciones.Where(x => x.esActivo).OrderByDescending(x => x.id).FirstOrDefault();
                                var estatusAutorizacion = contrato.evaluaciones.Where(x => !x.esActivo).ToList();
                                var status = evaluacion != null ? 2 : estatusAutorizacion.Count != 0 ? 3 : 0;

                                var idPlantilla = 0;
                                foreach (var plantilla in lstPlantillas)
                                {
                                    var contratosPlantilla = plantilla.contratos.Split(',');
                                    if (contratosPlantilla.Count() > 0)
                                    {
                                        var plantillaAsignada = contratosPlantilla.FirstOrDefault(x => !string.IsNullOrEmpty(x) && Convert.ToInt32(x) == contrato.idContrato);
                                        if (plantillaAsignada != null)
                                        {
                                            idPlantilla = plantilla.id;
                                            break;
                                        }
                                    }
                                }
                                if (idPlantilla == 0)
                                {
                                    idPlantilla = plantillaDefault;
                                }

                                int sta = 0;
                                if (evaluacion != null)
                                {
                                    var objSubCon = evaluacion.evaluacionesConAsignacion;
                                    if (objSubCon.Count != 0)
                                    {
                                        foreach (var item in objSubCon)
                                        {
                                            if (item.evaluacionesPendiente.Any(x => x))
                                            {
                                                sta = 1;
                                            }
                                            else
                                            {
                                                sta = 0;
                                                break;
                                            }
                                        }
                                    }
                                }

                                contrato.id = evaluacion != null ? evaluacion.id : 0;
                                contrato.idAsignacion = asignacion != null ? asignacion.id : 0;
                                contrato.descripcion = cc != null ? cc.descripcion : "";
                                contrato.descripcioncc = contrato.cc + (!string.IsNullOrEmpty(contrato.descripcion) ? " " + cc.descripcion : "");
                                contrato.estatusAutorizacion = estatusAutorizacion.Count > 0 ? estatusAutorizacion.First().statusAutorizacion : 0;
                                contrato.evaluacionAnteriorid = evaluacion != null ? evaluacion.evaluacionAnteriorid : 0;
                                contrato.evaluacionActual = sta;
                                contrato.existeEvaluacionAnterior = estatusAutorizacion.Count != 0 ? true : false;
                                contrato.existeEvaluacionPendiente = evaluacion == null ? false : true;
                                contrato.perdiodoFechas = obtenerPeriodoFechas(asignacion != null ? asignacion.id : 0);
                                contrato.status = status;
                                contrato.statusVobo = evaluacion == null ? false : evaluacion.statusVobo;
                                contrato.idPlantilla = idPlantilla;
                            }

                            lstRetornar = lstContratos2.Where(x => !lstAsignaciones2.Contains(x.id)).ToList();
                            #endregion
                        }
                        else if (Estatus == 2)
                        {
                            #region optimización
                            var lstContratos2 = db.tblX_Contrato
                                .Where(x =>
                                    (string.IsNullOrEmpty(AreaCuenta) ? true : x.cc == AreaCuenta) &&
                                    (subcontratista == 0 ? true : x.subcontratistaID == subcontratista) &&
                                    x.estatus)
                                .Select(x => new ContratoSubContratistaDTO
                                {
                                    numeroProveedor = x.subcontratista.numeroProveedor,
                                    nombre = x.subcontratista.nombre,
                                    direccion = x.subcontratista.direccion,
                                    nombreCorto = x.subcontratista.nombreCorto,
                                    codigoPostal = x.subcontratista.codigoPostal,
                                    cc = x.cc,
                                    idSubContratista = x.subcontratistaID,
                                    tipoUsuario = ObjUsuario.tipo,
                                    numeroContrato = x.numeroContrato,
                                    idContrato = x.id,
                                    evaluaciones = x.evaluaciones.Select(y => new EvaluacionesDTO
                                    {
                                        cc = y.cc,
                                        esActivo = y.esActivo,
                                        id = y.id,
                                        statusAutorizacion = y.statusAutorizacion,
                                        evaluacionAnteriorid = y.evaluacionAnteriorid,
                                        statusVobo = y.statusVobo,
                                        evaluacionesConAsignacion = y.evalSubContratista.Select(z => new EvaluacionConAsignacionDTO
                                        {
                                            id = z.id,
                                            evaluacionPendiente = z.evaluacionPendiente,
                                            evaluacionesPendiente = z.detalleEvaluaciones.Select(m => m.evaluacionPendiente).ToList()
                                        }).ToList()
                                    }).ToList()
                                }).ToList();

                            var ccContratos = lstContratos2.Select(x => x.cc).Distinct().ToList();
                            var _ccs = _context.tblP_CC.Where(x => ccContratos.Contains(x.cc)).ToList();

                            var lstPlantillas = db.tblCO_ADP_EvaluacionPlantilla.Where(x => !string.IsNullOrEmpty(x.contratos)).ToList();
                            var plantillaDefault = db.tblCO_ADP_EvaluacionPlantilla.First(x => x.nombrePlantilla == "default").id;

                            foreach (var contrato in lstContratos2)
                            {
                                var cc = _ccs.FirstOrDefault(x => x.cc == contrato.cc);

                                var evaluacion = contrato.evaluaciones.FirstOrDefault(x => x.esActivo);
                                var asignacion = contrato.evaluaciones.Where(x => x.esActivo).OrderByDescending(x => x.id).FirstOrDefault();
                                var estatusAutorizacion = contrato.evaluaciones.Where(x => x.esActivo).ToList();
                                var status = evaluacion != null ? 2 : contrato.evaluaciones.Where(x => !x.esActivo).Count() != 0 ? 3 : 0;

                                var idPlantilla = 0;
                                foreach (var plantilla in lstPlantillas)
                                {
                                    var contratosPlantilla = plantilla.contratos.Split(',');
                                    if (contratosPlantilla.Count() > 0)
                                    {
                                        var plantillaAsignada = contratosPlantilla.FirstOrDefault(x => !string.IsNullOrEmpty(x) && Convert.ToInt32(x) == contrato.idContrato);
                                        if (plantillaAsignada != null)
                                        {
                                            idPlantilla = plantilla.id;
                                            break;
                                        }
                                    }
                                }
                                if (idPlantilla == 0)
                                {
                                    idPlantilla = plantillaDefault;
                                }

                                int sta = 0;
                                if (evaluacion != null)
                                {
                                    var objSubCon = evaluacion.evaluacionesConAsignacion;
                                    if (objSubCon.Count != 0)
                                    {
                                        foreach (var item in objSubCon)
                                        {
                                            if (item.evaluacionesPendiente.Any(x => x))
                                            {
                                                sta = 1;
                                            }
                                            else
                                            {
                                                sta = 0;
                                                break;
                                            }
                                        }
                                    }
                                }

                                contrato.id = evaluacion != null ? evaluacion.id : 0;
                                contrato.idAsignacion = asignacion != null ? asignacion.id : 0;
                                contrato.descripcion = cc != null ? cc.descripcion : "";
                                contrato.descripcioncc = contrato.cc + (!string.IsNullOrEmpty(contrato.descripcion) ? " " + cc.descripcion : "");
                                contrato.estatusAutorizacion = estatusAutorizacion.Count > 0 ? estatusAutorizacion.First().statusAutorizacion : 0;
                                contrato.evaluacionAnteriorid = evaluacion != null ? evaluacion.evaluacionAnteriorid : 0;
                                contrato.evaluacionActual = sta;
                                contrato.existeEvaluacionAnterior = estatusAutorizacion.Count != 0 ? true : false;
                                contrato.existeEvaluacionPendiente = evaluacion == null ? false : true;
                                contrato.perdiodoFechas = obtenerPeriodoFechas(asignacion != null ? asignacion.id : 0);
                                contrato.status = status;
                                contrato.statusVobo = evaluacion == null ? false : evaluacion.statusVobo;
                                contrato.idPlantilla = idPlantilla;
                            }

                            lstRetornar = lstContratos2;
                            #endregion
                        }
                        else
                        {
                            #region optimización
                            lstRetornar = db.tblCO_ADP_EvalSubConAsignacion
                                .Where(x =>
                                    (string.IsNullOrEmpty(AreaCuenta) ? true : x.cc == AreaCuenta) &&
                                    (subcontratista == 0 ? true : x.idSubContratista == subcontratista) &&
                                    x.statusAutorizacion == Estatus)
                                .Select(x => new ContratoSubContratistaDTO
                                {
                                    id = x.id,
                                    numeroProveedor = x.subcontratista.numeroProveedor,
                                    nombre = x.subcontratista.nombre,
                                    direccion = x.subcontratista.direccion,
                                    nombreCorto = x.subcontratista.nombreCorto,
                                    codigoPostal = x.subcontratista.codigoPostal,
                                    idAsignacion = x.id,
                                    cc = x.cc,
                                    fechaCreacion = (DateTime)x.fechaCreacion,
                                    estatusAutorizacion = x.statusAutorizacion,
                                    idSubContratista = x.idSubContratista,
                                    status = 3,
                                    numeroContrato = "",
                                    tipoUsuario = ObjUsuario.tipo,
                                    statusVobo = x.statusVobo,
                                    idContrato = x.idContrato
                                }).ToList();

                            var ccContratos = lstRetornar.Select(x => x.cc).Distinct().ToList();
                            var _ccs = _context.tblP_CC.Where(x => ccContratos.Contains(x.cc)).ToList();

                            var lstPlantillas = db.tblCO_ADP_EvaluacionPlantilla.Where(x => !string.IsNullOrEmpty(x.contratos)).ToList();
                            var plantillaDefault = db.tblCO_ADP_EvaluacionPlantilla.First(x => x.nombrePlantilla == "default").id;

                            foreach (var asignacion in lstRetornar)
                            {
                                var cc = _ccs.FirstOrDefault(x => x.cc == asignacion.cc);

                                var evaluaciones = db.tblCO_ADP_EvalSubConAsignacion.Where(x => x.idContrato == asignacion.idContrato).ToList();
                                var evaluacionAnterior = evaluaciones.FirstOrDefault(x => x.esActivo);

                                int sta = 0;
                                foreach (var item in evaluaciones.Where(x => x.esActivo))
                                {
                                    if (item.evalSubContratista.Any(x => x.evaluacionPendiente))
                                    {
                                        sta = 1;
                                    }
                                    else
                                    {
                                        sta = 0;
                                    }
                                }

                                var idPlantilla = 0;
                                foreach (var plantilla in lstPlantillas)
                                {
                                    var contratosPlantilla = plantilla.contratos.Split(',');
                                    if (contratosPlantilla.Count() > 0)
                                    {
                                        var plantillaAsignada = contratosPlantilla.FirstOrDefault(x => !string.IsNullOrEmpty(x) && Convert.ToInt32(x) == asignacion.idContrato);
                                        if (plantillaAsignada != null)
                                        {
                                            idPlantilla = plantilla.id;
                                            break;
                                        }
                                    }
                                }
                                if (idPlantilla == 0)
                                {
                                    idPlantilla = plantillaDefault;
                                }

                                asignacion.descripcion = cc != null ? cc.descripcion : "";
                                asignacion.perdiodoFechas = obtenerPeriodoFechas(asignacion != null ? asignacion.id : 0);
                                asignacion.descripcioncc = asignacion.cc + (!string.IsNullOrEmpty(asignacion.descripcion) ? " " + cc.descripcion : "");
                                asignacion.evaluacionAnteriorid = evaluacionAnterior != null ? evaluacionAnterior.evaluacionAnteriorid : 0;
                                asignacion.evaluacionActual = sta;
                                asignacion.existeEvaluacionAnterior = evaluaciones.Where(x => x.esActivo).Count() != 0 ? true : false;
                                asignacion.existeEvaluacionPendiente = evaluacionAnterior == null ? false : true;
                                asignacion.idPlantilla = idPlantilla;
                            }
                            #endregion
                        }
                    }

                    lstRetornarFiltrandoPasado = lstRetornar.Where(r => r.estatusAutorizacion == Estatus).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }

            return lstRetornarFiltrandoPasado;
        }
        public string obtenerPeriodoFechas(int idAsignacion)
        {
            string r = "";
            try
            {
                var objAsignacion = traermeDatosPrincipales(idAsignacion);
                var lstReqDivicion = obtenerLst(objAsignacion.idPlantilla, idAsignacion);
                var lstAsignacion = obtenerTodasLasASinaciones(idAsignacion);

                DateTime FechaInicialPeriodo = new DateTime();
                DateTime FechaInicialFinal = new DateTime();
                DateTime fechaActual = DateTime.Now;
                var sumaDeDias = (objAsignacion.numFreq * lstAsignacion.Count());
                var sumaDeDiasInicial = ((lstAsignacion.Count() - 1) * objAsignacion.numFreq);

                FechaInicialPeriodo = objAsignacion.fechaInicial.AddDays(sumaDeDiasInicial);
                FechaInicialFinal = objAsignacion.fechaInicial.AddDays(sumaDeDias);

                r = FechaInicialPeriodo.ToShortDateString() + " - " + FechaInicialFinal.ToShortDateString();
            }
            catch (Exception)
            {
                return r = "";
            }
            return r;
        }
        public int retornarEvaluacionActual(int id)
        {
            int sta = 0;
            var objSubCon = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == id).ToList();
            if (objSubCon.Count() != 0)
            {
                foreach (var item in objSubCon)
                {
                    var objdet = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.idEvaluacion == item.id).FirstOrDefault();
                    if (objdet != null)
                    {
                        if (objdet.evaluacionPendiente == true)
                        {
                            sta = 1;
                        }
                        else
                        {
                            return sta = 0;
                        }
                    }
                }
            }

            return sta;
        }
        public SubContratistasDTO GuardarEvaluacionSubContratista(SubContratistasDTO parametros)
        {
            SubContratistasDTO objretu = new SubContratistasDTO();
            try
            {
                var obj = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.id == parametros.id).FirstOrDefault();
                if (obj != null)
                {
                    obj.planesDeAccion = parametros.planesDeAccion;
                    obj.responsable = parametros.responsable;
                    obj.fechaCompromiso = parametros.fechaCompromiso;
                    obj.evaluacionPendiente = true;
                    db.SaveChanges();
                    objretu.mensaje = "Se ha calificado con exito";
                    objretu.status = 1;
                }
            }
            catch (Exception)
            {
                objretu.mensaje = "Ocurrio algun error comuniquese con el departamento de TI.";
                objretu.status = 1;
            }
            return objretu;
        }
        public SubContratistasDTO GuardarAsignacion(SubContratistasDTO parametros)
        {
            SubContratistasDTO objretu = new SubContratistasDTO();
            try
            {
                tblCO_ADP_EvalSubConAsignacion objAsignacion = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.id == parametros.idAsignacion).FirstOrDefault();
                if (objAsignacion != null)
                {
                    objAsignacion.statusAutorizacion = 2;
                    objAsignacion.statusVobo = true;
                    db.SaveChanges();
                    tblCO_ADP_EvaluacionRel objRel;
                    foreach (var item in parametros.lstRelacion)
                    {
                        objRel = new tblCO_ADP_EvaluacionRel();
                        objRel.idSubContratista = item.idSubContratista;
                        objRel.idReq = item.idReq;
                        objRel.Preguntar = item.Preguntar;
                        objRel.idAsignacion = objAsignacion.id;

                        db.tblCO_ADP_EvaluacionRel.Add(objRel);
                        db.SaveChanges();


                        var objDiv = db.tblCO_ADP_EvaluacionReq.Where(r => r.id == item.idReq).FirstOrDefault().idDiv;
                        var lstRequeriemientos = db.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == objDiv).ToList();

                        var Contratista = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == objAsignacion.id && r.tipoEvaluacion == objDiv).FirstOrDefault();
                        var lstDetalle = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.tipoEvaluacion == objDiv && r.idEvaluacion == Contratista.id).ToList();
                        int idRowReq = 0;
                        foreach (var item3 in lstRequeriemientos)
                        {
                            idRowReq++;
                            var objReq = db.tblCO_ADP_EvaluacionReq.Where(r => r.id == item3.id).FirstOrDefault();
                            if (objReq.id == item.idReq)
                            {
                                break;
                            }
                        }

                        var objDetalle = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.tipoEvaluacion == objDiv && r.idEvaluacion == Contratista.id && r.idRow == idRowReq).FirstOrDefault();
                        if (objDetalle != null)
                        {
                            objDetalle.opcional = item.Preguntar;
                            db.SaveChanges();
                        }
                    }

                }
                else
                {
                    objAsignacion = new tblCO_ADP_EvalSubConAsignacion();
                    objAsignacion.cc = parametros.cc;
                    objAsignacion.idSubContratista = parametros.idSubContratista;
                    objAsignacion.statusAutorizacion = 0;
                    objAsignacion.idPadre = 0;
                    objAsignacion.evaluacionActual = 0;
                    objAsignacion.fechaCreacion = DateTime.Now;
                    objAsignacion.evaluacionAnteriorid = 0;
                    objAsignacion.esActivo = true;
                    objAsignacion.fechaInicial = (DateTime)parametros.fechaInicial;
                    objAsignacion.fechaFinal = (DateTime)parametros.fechaFinal;
                    objAsignacion.numFreq = (int)parametros.freqEval;
                    objAsignacion.statusVobo = true;
                    db.tblCO_ADP_EvalSubConAsignacion.Add(objAsignacion);
                    db.SaveChanges();
                    objAsignacion = db.tblCO_ADP_EvalSubConAsignacion.OrderByDescending(r => r.id).FirstOrDefault();
                    tblCO_ADP_EvaluacionRel objRel;


                    int idSubContratista = db.tblCO_ADP_EvalSubConAsignacion.OrderByDescending(r => r.id).FirstOrDefault().id;

                    tblCO_ADP_EvalSubContratista objSubContratista = new tblCO_ADP_EvalSubContratista();
                    tblCO_ADP_EvalSubContratistaDet objSubContratistaDet = new tblCO_ADP_EvalSubContratistaDet();
                    var lstEvaluaciones = db.tblCO_ADP_EvaluacionDiv.Where(r => r.SubContratista == false && r.idPlantilla == parametros.idPlantilla).ToList();
                    foreach (var item in lstEvaluaciones)
                    {
                        objSubContratista = new tblCO_ADP_EvalSubContratista();
                        objSubContratista.idSubConAsignacion = idSubContratista;
                        objSubContratista.idAreaCuenta = parametros.cc;
                        objSubContratista.idSubContratista = parametros.idSubContratista;
                        objSubContratista.tipoEvaluacion = item.id;
                        objSubContratista.evaluacionPendiente = true;

                        db.tblCO_ADP_EvalSubContratista.Add(objSubContratista);
                        db.SaveChanges();
                        int idRow = 0;
                        var lstRequerimiento = db.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == item.id).ToList();
                        int idEvalu = db.tblCO_ADP_EvalSubContratista.OrderByDescending(r => r.id).FirstOrDefault().id;
                        foreach (var item2 in lstRequerimiento)
                        {
                            idRow++;
                            objSubContratistaDet = new tblCO_ADP_EvalSubContratistaDet();
                            objSubContratistaDet.tipoEvaluacion = item.id;
                            objSubContratistaDet.idRow = idRow;
                            objSubContratistaDet.idEvaluacion = idEvalu;
                            objSubContratistaDet.fechaDocumento = DateTime.Now;

                            db.tblCO_ADP_EvalSubContratistaDet.Add(objSubContratistaDet);
                            db.SaveChanges();
                        }
                        idRow = 0;
                    }

                    foreach (var item in parametros.lstRelacion)
                    {
                        objRel = new tblCO_ADP_EvaluacionRel();
                        objRel.idSubContratista = item.idSubContratista;
                        objRel.idReq = item.idReq;
                        objRel.Preguntar = item.Preguntar;
                        objRel.idAsignacion = objAsignacion.id;
                        db.tblCO_ADP_EvaluacionRel.Add(objRel);
                        db.SaveChanges();

                        var objDiv = db.tblCO_ADP_EvaluacionReq.Where(r => r.id == item.idReq).FirstOrDefault().idDiv;
                        var lstRequeriemientos = db.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == objDiv).ToList();

                        var Contratista = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == objAsignacion.id && r.tipoEvaluacion == objDiv).FirstOrDefault();
                        var lstDetalle = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.tipoEvaluacion == objDiv && r.idEvaluacion == Contratista.id).ToList();
                        int idRowReq = 0;
                        foreach (var item3 in lstRequeriemientos)
                        {
                            idRowReq++;
                            var objReq = db.tblCO_ADP_EvaluacionReq.Where(r => r.id == item3.id).FirstOrDefault();
                            if (objReq.id == item.idReq)
                            {
                                break;
                            }
                        }

                        var objDetalle = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.tipoEvaluacion == objDiv && r.idEvaluacion == Contratista.id && r.idRow == idRowReq).FirstOrDefault();
                        if (objDetalle != null)
                        {
                            objDetalle.opcional = item.Preguntar;
                            db.SaveChanges();
                        }
                    }


                    var objSubContra = db.tblX_SubContratista.Where(r => r.id == parametros.idSubContratista).FirstOrDefault();
                    if (objSubContra != null)
                    {
                        tblPUsuarioDTO objUsuario = new tblPUsuarioDTO();
                        string sql = @"SELECT * FROM tblP_Usuario WHERE estatus='true' AND _user LIKE'%" + objSubContra.rfc + "%'";
                        using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                        {
                            ctx.Open();
                            objUsuario = ctx.Query<tblPUsuarioDTO>(sql, null, null, true, 300).FirstOrDefault();
                            ctx.Close();
                        }
                        //var objUsuario = db.tblP_Usuario.Where(r => r._user == objSubContra.rfc).FirstOrDefault();
                        List<string> emails = new List<string>();
                        emails.Add(objUsuario.correo);
                        string Subject = "El nombre de la evaluacion : " + objAsignacion.nombreEvaluacion;
                        string msg2 = CuerpoCorreo(objAsignacion, objUsuario);
                        List<FirmantesDTO> lstFirmantes = new List<FirmantesDTO>();
                        string msg = GlobalUtils.cuerpoDeCorreoFormato("Prestadores de servicio", "Evaluaciones", "Asginacion de la evaluacion para prestadores de servicio", msg2, "Gracias por su atencion.", "http://expediente.construplan.com.mx/", lstFirmantes);

                        GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), Subject), msg, emails);
                    }
                }
            }
            catch (Exception ex)
            {
                objretu.mensaje = "Ocurrio algun error comuniquese con el departamento de TI.";
                objretu.status = 1;
            }
            return objretu;
        }
        public List<SubContratistasDTO> getUsuariosAutorizantes(string term)
        {
            List<SubContratistasDTO> lstUsuarios = new List<SubContratistasDTO>();
            try
            {
                lstUsuarios = db.tblP_Usuario.Where(r => r.tipo == 2).ToList().Select(y => new SubContratistasDTO
                {
                    id = y.id,
                    nombre = y.nombre_completo
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return lstUsuarios;
        }
        public SubContratistasDTO AutorizarEvaluacion(SubContratistasDTO parametros, int idUsuario)
        {
            SubContratistasDTO objReturn = new SubContratistasDTO();
            try
            {
                var obj = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.id == parametros.id).FirstOrDefault();
                if (obj != null)
                {
                    obj.firmaAutorizacion = GlobalUtils.CrearFirmaDigital(obj.id, Core.Enum.Principal.DocumentosEnum.AutorizacionEvaluacion, idUsuario);
                    obj.FechaAutorizacion = DateTime.Now;
                    obj.esActivo = false;
                    obj.statusAutorizacion = 3;
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return objReturn;
        }
        public SubContratistasDTO AutorizarAsignacion(SubContratistasDTO parametros, int idUsuario)
        {
            SubContratistasDTO objReturn = new SubContratistasDTO();
            try
            {
                var obj = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.id == parametros.id).FirstOrDefault();
                if (obj != null)
                {
                    obj.esActivo = false;
                    obj.statusAutorizacion = 5;
                    obj.statusVobo = false;
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return objReturn;
        }
        public List<ElementosDTO> obtenerTodosLosElementosConSuRequerimiento()
        {
            List<ElementosDTO> lstRequerimiento = new List<ElementosDTO>();
            try
            {
                var lstReq = db.tblCO_ADP_EvaluacionReq.Where(n => n.preguntarEvaluacion == true).ToList();
                if (lstReq.Count() != 0)
                {
                    lstRequerimiento = db.tblCO_ADP_EvaluacionDiv.Where(r => r.SubContratista == false).ToList().Select(y => new ElementosDTO
                    {
                        id = y.id,
                        idbutton = y.idbutton,
                        idsection = y.idsection,
                        orden = y.orden,
                        toltips = y.toltips,
                        descripcion = y.descripcion,
                        esActivo = y.esActivo,
                        SubContratista = y.SubContratista,
                        important = y.important,
                        idEvaluador = y.idEvaluador,
                        Aparece = db.tblCO_ADP_EvaluacionReq.Where(n => n.idDiv == y.id && n.preguntarEvaluacion == true).ToList().Count() >= 1 ? true : false,
                        lstRequerimientos = db.tblCO_ADP_EvaluacionReq.Where(n => n.idDiv == y.id && n.preguntarEvaluacion == true).ToList(),
                    }).ToList(); ;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return lstRequerimiento;
        }
        public ElementosDTO guardarRelacion(List<ElementosDTO> lstRelacion)
        {
            ElementosDTO obj = new ElementosDTO();
            try
            {
                tblCO_ADP_EvaluacionRel objRel;
                foreach (var item in lstRelacion)
                {
                    objRel = new tblCO_ADP_EvaluacionRel();
                    objRel.idSubContratista = item.idSubContratista;
                    objRel.idReq = item.idReq;
                    objRel.Preguntar = item.Preguntar;
                    db.tblCO_ADP_EvaluacionRel.Add(objRel);
                    db.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return obj;
        }
        public List<ContratoSubContratistaDTO> tblObtenerDashBoardSubContratista(tblPUsuarioDTO ObjUsuario)
        {
            #region CODIGO ANTERIOR
            //List<ContratoSubContratistaDTO> lstRetornar = new List<ContratoSubContratistaDTO>();
            //var lstContratos = db.tblX_Contrato.Where(r => r.estatus == true && r.subcontratistaID == objUsuario).Select(x => x.subcontratistaID).Distinct().ToList();
            #endregion

            List<ContratoSubContratistaDTO> lstRetornarFiltrandoPasado = new List<ContratoSubContratistaDTO>();
            try
            {
                if (ObjUsuario.tipo == 3)
                {
                    var objUsuario = db.tblX_SubContratista.Where(r => r.rfc == ObjUsuario._user).FirstOrDefault().id;

                    lstRetornarFiltrandoPasado = db.tblX_Contrato
                        .Where(x =>
                            x.subcontratista.rfc == ObjUsuario._user &&
                            x.estatus)
                        .Select(x => new ContratoSubContratistaDTO
                        {
                            id = x.subcontratistaID,
                            numeroProveedor = x.subcontratista.numeroProveedor,
                            nombre = x.subcontratista.nombre,
                            direccion = x.subcontratista.direccion,
                            nombreCorto = x.subcontratista.nombreCorto,
                            codigoPostal = x.subcontratista.codigoPostal,
                            numeroContrato = x.numeroContrato,
                            nombreEvaluacion = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.idContrato == x.id).FirstOrDefault() != null ? db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.idContrato == x.id).FirstOrDefault().nombreEvaluacion : "",
                            cc = x.cc,
                            idSubContratista = x.subcontratistaID,
                            idContrato = x.id,
                            evaluaciones = x.subcontratista.evaluaciones.Select(y => new EvaluacionesDTO
                            {
                                cc = y.cc,
                                esActivo = y.esActivo,
                                id = y.id,
                                statusAutorizacion = y.statusAutorizacion
                            }).ToList()
                        }).ToList();

                    var ccContratos = lstRetornarFiltrandoPasado.Select(x => x.cc).ToList();
                    var ccs = _context.tblP_CC.Where(x => ccContratos.Contains(x.cc)).ToList();

                    foreach (var item in lstRetornarFiltrandoPasado)
                    {
                        var idAsignacion = item.evaluaciones.Where(x => x.cc == item.cc && x.esActivo).OrderByDescending(x => x.id).FirstOrDefault();
                        var estatusAutorizacion = item.evaluaciones.Where(x => x.cc == item.cc && x.esActivo).FirstOrDefault();
                        var estatus = item.evaluaciones.Where(x => x.esActivo).Count() != 0 ? 2 : item.evaluaciones.Where(x => !x.esActivo).Count() != 0 ? 3 : 0;
                        item.numeroContrato = item.numeroContrato;
                        item.nombreEvaluacion = item.nombreEvaluacion;
                        item.descripcion = ccs.First(x => x.cc == item.cc).descripcion;
                        item.descripcioncc = item.cc + " " + item.descripcion;
                        item.idAsignacion = idAsignacion != null ? idAsignacion.id : 0;
                        item.estatusAutorizacion = estatusAutorizacion != null ? estatusAutorizacion.statusAutorizacion : 0;
                        item.existeEvaluacionAnterior = item.evaluaciones.Any(x => !x.esActivo);
                        item.existeEvaluacionPendiente = item.evaluaciones.Any(x => x.esActivo);
                        item.status = estatus;
                        item.evaluaciones = null;
                        item.idPlantilla = obtenerElIdDeLaPlantilla(item.idContrato);
                    }
                }
                else if (ObjUsuario.tipo == 14)
                {
                    var objUsuarioHijo = db.tblP_Usuario.Where(x => x.id == ObjUsuario.id).FirstOrDefault();
                    var objUsuarioPadre = db.tblP_Usuario.Where(x => x.id == objUsuarioHijo.idPadre).FirstOrDefault();

                    var objUsuario = db.tblX_SubContratista.Where(r => r.rfc == objUsuarioPadre._user).FirstOrDefault().id;

                    lstRetornarFiltrandoPasado = db.tblX_Contrato
                        .Where(x =>
                            x.subcontratista.rfc == objUsuarioPadre._user &&
                            x.estatus)
                        .Select(x => new ContratoSubContratistaDTO
                        {
                            id = x.subcontratistaID,
                            numeroProveedor = x.subcontratista.numeroProveedor,
                            nombre = x.subcontratista.nombre,
                            direccion = x.subcontratista.direccion,
                            nombreCorto = x.subcontratista.nombreCorto,
                            codigoPostal = x.subcontratista.codigoPostal,
                            numeroContrato = x.numeroContrato,
                            nombreEvaluacion = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.idContrato == x.id).FirstOrDefault() != null ? db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.idContrato == x.id).FirstOrDefault().nombreEvaluacion : "",
                            cc = x.cc,
                            idSubContratista = x.subcontratistaID,
                            idContrato = x.id,
                            evaluaciones = x.subcontratista.evaluaciones.Select(y => new EvaluacionesDTO
                            {
                                cc = y.cc,
                                esActivo = y.esActivo,
                                id = y.id,
                                statusAutorizacion = y.statusAutorizacion
                            }).ToList()
                        }).ToList();

                    var ccContratos = lstRetornarFiltrandoPasado.Select(x => x.cc).ToList();
                    var ccs = _context.tblP_CC.Where(x => ccContratos.Contains(x.cc)).ToList();

                    foreach (var item in lstRetornarFiltrandoPasado)
                    {
                        var idAsignacion = item.evaluaciones.Where(x => x.cc == item.cc && x.esActivo).OrderByDescending(x => x.id).FirstOrDefault();
                        var estatusAutorizacion = item.evaluaciones.Where(x => x.cc == item.cc && x.esActivo).FirstOrDefault();
                        var estatus = item.evaluaciones.Where(x => x.esActivo).Count() != 0 ? 2 : item.evaluaciones.Where(x => !x.esActivo).Count() != 0 ? 3 : 0;
                        item.numeroContrato = item.numeroContrato;
                        item.nombreEvaluacion = item.nombreEvaluacion;
                        item.descripcion = ccs.First(x => x.cc == item.cc).descripcion;
                        item.descripcioncc = item.cc + " " + item.descripcion;
                        item.idAsignacion = idAsignacion != null ? idAsignacion.id : 0;
                        item.estatusAutorizacion = estatusAutorizacion != null ? estatusAutorizacion.statusAutorizacion : 0;
                        item.existeEvaluacionAnterior = item.evaluaciones.Any(x => !x.esActivo);
                        item.existeEvaluacionPendiente = item.evaluaciones.Any(x => x.esActivo);
                        item.status = estatus;
                        item.evaluaciones = null;
                        item.idPlantilla = obtenerElIdDeLaPlantilla(item.idContrato);
                    }
                }

                lstRetornarFiltrandoPasado.RemoveAll(x => x.status != 2 || x.idAsignacion == 0);

                #region CODIGO ANTERIOR
                //var lstSubContratista = db.tblX_Contrato.Where(r => r.estatus == true).ToList().Select(y => new ContratoSubContratistaDTO
                //{
                //    id = db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault() == null ? 0 : db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault().id,
                //    numeroProveedor = db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault() == null ? 0 : db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault().numeroProveedor,
                //    nombre = db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault() == null ? "" : db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault().nombre,
                //    direccion = db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault() == null ? "" : db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault().direccion,
                //    nombreCorto = db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault() == null ? "" : db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault().nombreCorto,
                //    codigoPostal = db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault() == null ? "" : db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault().codigoPostal,
                //    cc = y.cc,
                //    descripcion = _cont.dbSigoplan.tblP_CC.Where(r => r.cc == y.cc).FirstOrDefault() == null ? "" : _cont.dbSigoplan.tblP_CC.Where(r => r.cc == y.cc).FirstOrDefault().descripcion

                //}).ToList();


                //lstRetornar = lstSubContratista.Where(y => lstContratos.Contains(y.id)).ToList().Select(y => new ContratoSubContratistaDTO
                //{
                //    id = y.id,
                //    numeroProveedor = y.numeroProveedor,
                //    nombre = y.nombre,
                //    direccion = y.direccion,
                //    nombreCorto = y.nombreCorto,
                //    codigoPostal = y.codigoPostal,
                //    idAsignacion = db.tblCO_ADP_EvalSubConAsignacion.Where(n => n.idSubContratista == y.id && n.cc == y.cc && n.esActivo).FirstOrDefault() == null ? 0 : db.tblCO_ADP_EvalSubConAsignacion.OrderByDescending(f => f.id).Where(n => n.idSubContratista == y.id && n.cc == y.cc && n.esActivo).FirstOrDefault().id,
                //    cc = y.cc,
                //    descripcioncc = y.cc + " " + y.descripcion,
                //    estatusAutorizacion = db.tblCO_ADP_EvalSubConAsignacion.Where(n => n.idSubContratista == y.id && n.esActivo && n.cc == y.cc).FirstOrDefault() == null ? 0 : db.tblCO_ADP_EvalSubConAsignacion.Where(n => n.idSubContratista == y.id && n.esActivo && n.cc == y.cc).FirstOrDefault().statusAutorizacion,
                //    idSubContratista = y.id,
                //    existeEvaluacionAnterior = db.tblCO_ADP_EvalSubConAsignacion.Where(n => n.idSubContratista == y.id && n.esActivo == false).ToList().Count() != 0 ? true : false,
                //    existeEvaluacionPendiente = db.tblCO_ADP_EvalSubConAsignacion.Where(n => n.idSubContratista == y.id && n.esActivo == true).FirstOrDefault() == null ? false : true,
                //    status = db.tblCO_ADP_EvalSubConAsignacion.Where(n => n.idSubContratista == y.id && n.esActivo == true).ToList().Count() != 0 ? 2 : db.tblCO_ADP_EvalSubConAsignacion.Where(n => n.idSubContratista == y.id && n.esActivo == false).ToList().Count() != 0 ? 3 : 0
                //}).ToList();

                //lstRetornarFiltrandoPasado = lstRetornar.Where(r => r.status == 2 && r.idAsignacion != 0).ToList();
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
            return lstRetornarFiltrandoPasado;
        }
        public int obtenerElIdDeLaPlantilla(int idContrato)
        {
            try
            {
                List<DivicionesMenuDTO> lstTodosLosContratos = new List<DivicionesMenuDTO>();
                var lstPlantillas = db.tblCO_ADP_EvaluacionPlantilla.ToList();
                foreach (var item in lstPlantillas)
                {
                    var lst = lstPlantillasPor(item.id, item.contratos);
                    lstTodosLosContratos.AddRange(lst);
                }

                return lstTodosLosContratos.Where(r => r.id == idContrato).FirstOrDefault().idPlantilla;

            }
            catch (Exception)
            {
                return db.tblCO_ADP_EvaluacionPlantilla.Where(r => r.nombrePlantilla == "default").FirstOrDefault().id;
            }
        }
        public List<DivicionesMenuDTO> lstPlantillasPor(int idPlantilla, string lstPlan)
        {
            List<DivicionesMenuDTO> lst = new List<DivicionesMenuDTO>();
            DivicionesMenuDTO obj = new DivicionesMenuDTO();
            if (lstPlan != "")
            {
                var arr = lstPlan.Split(',');
                if (arr.Count() > 0)
                {
                    foreach (var item in arr)
                    {
                        if (item != "")
                        {
                            obj = new DivicionesMenuDTO();
                            var a = Convert.ToInt32(item);
                            var objContrato = db.tblX_Contrato.Where(r => r.id == a).FirstOrDefault();
                            obj.id = objContrato.id;
                            obj.idPlantilla = idPlantilla;
                            lst.Add(obj);
                        }
                    }
                }
            }
            return lst;
        }
        public MemoryStream realizarExcel(int idAsignacion)
        {
            var bytes = new MemoryStream();
            try
            {
                if (idAsignacion != 0)
                {
                    using (ExcelPackage excel = new ExcelPackage())
                    {
                        Color colorDeNaranja = ColorTranslator.FromHtml("#fabf8f");
                        Color colorDeCelda = ColorTranslator.FromHtml("#fff");
                        Color ColorNegro = ColorTranslator.FromHtml("#000");

                        var hoja1 = excel.Workbook.Worksheets.Add("EvaluacionSubContratista");

                        string startupPath = AppDomain.CurrentDomain.BaseDirectory;
                        string targetPath = startupPath + "Content\\img\\logo\\logo0.png";

                        //C:\Proyecto\SIGOPLAN\SEGURIDAD_SALUD_OCUPACIONAL
                        //C:\Users\Programador\Documents\SUBPRODUCTIVO\SIGOPLAN\Content\img\logo\logo.jpg
                        #region ENCABEZADO

#if DEBUG
                        System.Drawing.Image image = System.Drawing.Image.FromFile((targetPath));
#else
                         System.Drawing.Image image = System.Drawing.Image.FromFile((targetPath));
#endif

                        var obASignacion = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.id == idAsignacion).FirstOrDefault();
                        var objContratista = db.tblX_SubContratista.Where(r => r.id == obASignacion.idSubContratista).FirstOrDefault();


                        var img = hoja1.Drawings.AddPicture("my logo", image);
                        img.From.Column = 1;
                        img.From.Row = 1;
                        img.SetSize(140, 70);

                        hoja1.Column(1).Width = 12;
                        hoja1.Column(2).Width = 38;
                        hoja1.Column(3).Width = 27;
                        hoja1.Column(4).Width = 32;
                        hoja1.Column(5).Width = 22;
                        hoja1.Column(6).Width = 20;
                        hoja1.Row(9).Height = 6;
                        hoja1.Row(11).Height = 6;
                        hoja1.Row(13).Height = 6;
                        hoja1.Row(15).Height = 6;

                        string TituloRangoConA = "A1:" + "A1";
                        hoja1.Cells[TituloRangoConA].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[TituloRangoConA].Merge = true;
                        hoja1.Cells[TituloRangoConA].AutoFitColumns(4);
                        hoja1.Cells[TituloRangoConA].Style.Font.Size = 4;
                        hoja1.Cells[TituloRangoConA].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[TituloRangoConA].Style.Fill.BackgroundColor.SetColor(colorDeCelda);

                        List<string[]> headerRow = new List<string[]>() { new string[] { 
                        "Sistema de Gestion de Proyectos"  ,
                    } };
                        string TituloRango = "B2:" + "F2";
                        hoja1.Cells[TituloRango].Merge = true;
                        hoja1.Cells[TituloRango].LoadFromArrays(headerRow);
                        hoja1.Cells[TituloRango].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[TituloRango].Style.Font.Color.SetColor(ColorNegro);
                        hoja1.Cells[TituloRango].Style.Font.Bold = true;
                        hoja1.Cells[TituloRango].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                        hoja1.Cells[TituloRango].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[TituloRango].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        List<string[]> headerRow2 = new List<string[]>() { new string[] { 
                        "Evaluación a Subcontratistas"  ,
                    } };
                        //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                        string TituloRango2 = "B4:" + "F4";
                        hoja1.Cells[TituloRango2].Merge = true;
                        hoja1.Cells[TituloRango2].LoadFromArrays(headerRow2);
                        hoja1.Cells[TituloRango2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[TituloRango2].Style.Font.Color.SetColor(ColorNegro);
                        hoja1.Cells[TituloRango2].Style.Font.Bold = true;
                        hoja1.Cells[TituloRango2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango2].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                        hoja1.Cells[TituloRango2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[TituloRango2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        List<string[]> headerRow3 = new List<string[]>() { new string[] { 
                        "Nombre del Subcontratista: "  ,
                        objContratista.nombre == null ?"":objContratista.nombre
                    } };
                        //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                        string TituloRango3 = "B6:" + "B6";
                        hoja1.Cells[TituloRango3].Merge = true;
                        hoja1.Cells[TituloRango3].LoadFromArrays(headerRow3);
                        hoja1.Cells[TituloRango3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[TituloRango3].Style.Font.Color.SetColor(ColorNegro);
                        hoja1.Cells[TituloRango3].Style.Font.Bold = true;
                        hoja1.Cells[TituloRango3].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango3].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango3].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango3].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                        hoja1.Cells[TituloRango3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        hoja1.Cells[TituloRango3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        List<string[]> headerRow4 = new List<string[]>() { new string[] { 
                        "Especialidad: "  ,
                    } };
                        //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                        string TituloRango4 = "B8:" + "B8";
                        hoja1.Cells[TituloRango4].Merge = true;
                        hoja1.Cells[TituloRango4].LoadFromArrays(headerRow4);
                        hoja1.Cells[TituloRango4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[TituloRango4].Style.Font.Color.SetColor(ColorNegro);
                        hoja1.Cells[TituloRango4].Style.Font.Bold = true;
                        hoja1.Cells[TituloRango4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango4].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                        hoja1.Cells[TituloRango4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        hoja1.Cells[TituloRango4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                        List<string[]> headerRow5 = new List<string[]>() { new string[] { 
                        "Proyecto: "  ,
                    } };
                        //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                        string TituloRango5 = "B10:" + "B10";
                        hoja1.Cells[TituloRango5].Merge = true;
                        hoja1.Cells[TituloRango5].LoadFromArrays(headerRow5);
                        hoja1.Cells[TituloRango5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[TituloRango5].Style.Font.Color.SetColor(ColorNegro);
                        hoja1.Cells[TituloRango5].Style.Font.Bold = true;
                        hoja1.Cells[TituloRango5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango5].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                        hoja1.Cells[TituloRango5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        hoja1.Cells[TituloRango5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        List<string[]> headerRow6 = new List<string[]>() { new string[] { 
                        "Fecha de evaluacion: "  ,
                        obASignacion.fechaCreacion == null ? "" :obASignacion.fechaCreacion.ToString()
                    } };
                        //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                        string TituloRango6 = "B12:" + "B12";
                        hoja1.Cells[TituloRango6].Merge = true;
                        hoja1.Cells[TituloRango6].LoadFromArrays(headerRow6);
                        hoja1.Cells[TituloRango6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[TituloRango6].Style.Font.Color.SetColor(ColorNegro);
                        hoja1.Cells[TituloRango6].Style.Font.Bold = true;
                        hoja1.Cells[TituloRango6].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango6].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango6].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                        hoja1.Cells[TituloRango6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        hoja1.Cells[TituloRango6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                        List<string[]> headerRow7 = new List<string[]>() { new string[] { 
                        "Periodo de evaluacion: "  ,
                    } };
                        //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                        string TituloRango7 = "D12:" + "D12";
                        hoja1.Cells[TituloRango7].Merge = true;
                        hoja1.Cells[TituloRango7].LoadFromArrays(headerRow7);
                        hoja1.Cells[TituloRango7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[TituloRango7].Style.Font.Color.SetColor(ColorNegro);
                        hoja1.Cells[TituloRango7].Style.Font.Bold = true;
                        hoja1.Cells[TituloRango7].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango7].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango7].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango7].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango7].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                        hoja1.Cells[TituloRango7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        hoja1.Cells[TituloRango7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        List<string[]> headerRow8 = new List<string[]>() { new string[] { 
                        "PROCESO DE RETROALIMENTACIÓN"  ,
                    } };
                        //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                        string TituloRango8 = "B14:" + "F14";
                        hoja1.Cells[TituloRango8].Merge = true;
                        hoja1.Cells[TituloRango8].LoadFromArrays(headerRow8);
                        hoja1.Cells[TituloRango8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[TituloRango8].Style.Font.Color.SetColor(ColorNegro);
                        hoja1.Cells[TituloRango8].Style.Font.Bold = true;
                        hoja1.Cells[TituloRango8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango8].Style.Fill.BackgroundColor.SetColor(colorDeNaranja);
                        hoja1.Cells[TituloRango8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[TituloRango8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        List<string[]> headerRow9 = new List<string[]>() { new string[] { 
                        "ELEMENTO CLAVE"  ,
                        "DESVIACIONES" ,
                        "PLANES DE ACCION" ,
                        "RESPONSABLE" ,
                        "FECHA COMPROMISO" ,
                    } };
                        //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                        string TituloRango9 = "B16:" + "F16";
                        hoja1.Cells[TituloRango9].LoadFromArrays(headerRow9);
                        hoja1.Cells[TituloRango9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[TituloRango9].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[TituloRango9].Style.Font.Bold = true;
                        hoja1.Cells[TituloRango9].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango9].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango9].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango9].Style.Fill.BackgroundColor.SetColor(ColorNegro);
                        hoja1.Cells[TituloRango9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[TituloRango9].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        #endregion


                        var obtenerListadoImprimible = obtenerListado(idAsignacion);


                        //string headerRange = "B18:" + "F18";



                        var cellData = new List<object[]>();
                        var cellDataC = new List<object[]>();

                        int cont = 17;
                        int contC = 17;
                        int contP = 17;
                        int contR = 17;
                        int contF = 17;
                        foreach (var item in obtenerListadoImprimible)
                        {
                            cellData = new List<object[]>();
                            cellData.Add(new object[]{
                                      item.Divicion.Trim(),
                                    });

                            cont++;
                            if (item.numeroMayor != 0)
                            {
                                string headAling = "B" + cont + ":" + "F" + cont;
                                int valid = item.numeroMayor == 0 ? 1 : item.numeroMayor;
                                string headA = "B" + cont + ":" + "B" + ((cont + valid) - 1).ToString();
                                hoja1.Cells[headAling].LoadFromArrays(cellData);
                                hoja1.Cells[headA].Merge = true;
                                hoja1.Cells[headA].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                hoja1.Cells[headA].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja1.Cells[headA].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja1.Cells[headA].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                                hoja1.Cells[headA].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headA].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headA].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headA].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAling].Style.Font.Bold = true;
                                hoja1.Cells[headAling].Style.WrapText = true;
                            }

                            foreach (var item2 in item.Desviaciones)
                            {
                                cellDataC = new List<object[]>();
                                cellDataC.Add(new object[]{
                                      item2.texto== null ?"":item2.texto.Trim() ,
                                    });
                                contC++;
                                string headAlingC = "C" + contC + ":" + "C" + contC;
                                hoja1.Cells[headAlingC].LoadFromArrays(cellDataC);
                                hoja1.Cells[headAlingC].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                hoja1.Cells[headAlingC].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja1.Cells[headAlingC].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja1.Cells[headAlingC].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                                hoja1.Cells[headAlingC].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAlingC].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAlingC].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAlingC].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            }

                            foreach (var item2 in item.PlanesDeAccion)
                            {
                                cellDataC = new List<object[]>();
                                cellDataC.Add(new object[]{
                                      item2.texto== null ?"":item2.texto.Trim() ,
                                    });
                                contP++;
                                string headAlingC = "D" + contP + ":" + "D" + contP;
                                hoja1.Cells[headAlingC].LoadFromArrays(cellDataC);
                                hoja1.Cells[headAlingC].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                hoja1.Cells[headAlingC].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja1.Cells[headAlingC].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja1.Cells[headAlingC].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                                hoja1.Cells[headAlingC].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAlingC].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAlingC].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAlingC].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            }
                            foreach (var item2 in item.Responsable)
                            {
                                cellDataC = new List<object[]>();
                                cellDataC.Add(new object[]{
                                      item2.texto== null ?"":item2.texto.Trim() ,
                                    });
                                contR++;
                                string headAlingC = "E" + contR + ":" + "E" + contR;
                                hoja1.Cells[headAlingC].LoadFromArrays(cellDataC);
                                hoja1.Cells[headAlingC].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                hoja1.Cells[headAlingC].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja1.Cells[headAlingC].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja1.Cells[headAlingC].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                                hoja1.Cells[headAlingC].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAlingC].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAlingC].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAlingC].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            }
                            foreach (var item2 in item.FechaCompromiso)
                            {
                                cellDataC = new List<object[]>();
                                cellDataC.Add(new object[]{
                                      item2.texto== null ?"":item2.texto.Trim() ,
                                    });
                                contF++;
                                string headAlingC = "F" + contF + ":" + "F" + contF;
                                hoja1.Cells[headAlingC].LoadFromArrays(cellDataC);
                                hoja1.Cells[headAlingC].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                hoja1.Cells[headAlingC].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja1.Cells[headAlingC].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja1.Cells[headAlingC].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                                hoja1.Cells[headAlingC].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAlingC].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAlingC].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAlingC].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            }



                            cont = (cont + item.numeroMayor) - 1;
                        }
                        for (int i = 18; i < cont; i++)
                        {
                            hoja1.Row(i).Height = 68;
                        }
                        cont = cont + 5;
                        cellDataC = new List<object[]>();
                        cellDataC.Add(new object[]{
                                      "EVALUACION GENERAL:" ,
                                       obtenerListadoImprimible.Sum(r=>r.Calificacion) / obtenerListadoImprimible.Count(),
                                    });

                        string headAlingD = "B" + cont + ":" + "B" + cont;
                        hoja1.Cells[headAlingD].LoadFromArrays(cellDataC);
                        hoja1.Cells[headAlingD].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[headAlingD].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        hoja1.Cells[headAlingD].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        hoja1.Cells[headAlingD].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                        hoja1.Cells[headAlingD].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[headAlingD].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[headAlingD].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[headAlingD].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        excel.Compression = OfficeOpenXml.CompressionLevel.BestSpeed;

                        List<byte[]> lista = new List<byte[]>();


                        using (var exportData = new MemoryStream())
                        {
                            excel.SaveAs(exportData);
                            bytes = exportData;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return bytes;
        }
        public List<excelDTO> obtenerListado(int idAsignacion)
        {
            List<excelDTO> obtenerListado = new List<excelDTO>();

            var lstSubContratista = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == idAsignacion).ToList();
            List<tblCO_ADP_EvalSubContratistaDet> lstSubContratistaDet = new List<tblCO_ADP_EvalSubContratistaDet>();
            foreach (var item in lstSubContratista)
            {
                var lst = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.idEvaluacion == item.id).ToList();
                lstSubContratistaDet.AddRange(lst);
            }

            obtenerListado = db.tblCO_ADP_EvaluacionDiv.Where(r => r.SubContratista == false).ToList().Select(y => new excelDTO
            {
                id = y.id,
                Divicion = y.descripcion,
                Desviaciones = lstSubContratistaDet.Where(r => r.tipoEvaluacion == y.id && r.opcional == false).ToList().Select(n => new resultDTO { texto = n.comentario }).ToList(),
                PlanesDeAccion = lstSubContratistaDet.Where(r => r.tipoEvaluacion == y.id && r.opcional == false).ToList().Select(n => new resultDTO { texto = n.planesDeAccion }).ToList(),
                Responsable = lstSubContratistaDet.Where(r => r.tipoEvaluacion == y.id && r.opcional == false).ToList().Select(n => new resultDTO { texto = n.responsable }).ToList(),
                FechaCompromiso = lstSubContratistaDet.Where(r => r.tipoEvaluacion == y.id && r.opcional == false).ToList().Select(n => new resultDTO { texto = n.fechaCompromiso }).ToList(),
                numeroMayor = lstSubContratistaDet.Where(r => r.tipoEvaluacion == y.id && r.opcional == false).ToList().Count(),
                Calificacion = lstSubContratistaDet.Where(r => r.tipoEvaluacion == y.id && r.opcional == false).ToList().Select(n => new resultDTO { calificacion = n.calificacion }).ToList().Sum(r => r.calificacion) == 0 ? 1 : lstSubContratistaDet.Where(r => r.tipoEvaluacion == y.id && r.opcional == false).ToList().Select(n => new resultDTO { calificacion = n.calificacion }).ToList().Sum(r => r.calificacion) / lstSubContratistaDet.Where(r => r.tipoEvaluacion == y.id && r.opcional == false).ToList().Count(),
            }).ToList();

            return obtenerListado;
        }
        public List<tblEN_Estrellas> getEstrellas()
        {
            return db.tblEN_Estrellas.ToList();
        }
        public ContratoSubContratistaDTO EvaluarDetalle(int id, List<SubContratistasDTO> parametros, int userEnvia)
        {
            ContratoSubContratistaDTO objre = new ContratoSubContratistaDTO();
            try
            {
                string Evaluacion = "";
                int idSubContratista = 0;
                foreach (var item in parametros)
                {
                    var obj = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.id == item.id).FirstOrDefault();
                    if (obj != null)
                    {
                        Evaluacion = db.tblCO_ADP_EvaluacionDiv.Where(r => r.id == obj.tipoEvaluacion).FirstOrDefault().descripcion;
                        idSubContratista = db.tblCO_ADP_EvalSubContratista.Where(r => r.id == obj.idEvaluacion).FirstOrDefault().idSubContratista;
                        obj.evaluacionPendiente = true;
                        db.SaveChanges();
                        objre.status = 2;
                        objre.mensaje = "Guardado con exito";
                    }
                }
                var objEval = db.tblCO_ADP_EvalSubContratista.Where(r => r.id == id).FirstOrDefault();
                tblP_Alerta objAlerta = new tblP_Alerta();
                objAlerta.userEnviaID = userEnvia;
                objAlerta.userRecibeID = objEval.idSubContratista;
                objAlerta.msj = "Tienes una evaluacion pendiente";
                objAlerta.visto = false;
                objAlerta.tipoAlerta = 2;
                objAlerta.sistemaID = 2;
                objAlerta.objID = id;
                objAlerta.url = "/ControlObra/ControlObra/DashboardSubContratista";
                db.tblP_Alerta.Add(objAlerta);
                db.SaveChanges();

                var objAsignacion = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.id == objEval.idSubConAsignacion).FirstOrDefault();
                var objSubContra = db.tblX_SubContratista.Where(r => r.id == idSubContratista).FirstOrDefault();
                if (objSubContra != null)
                {
                    tblPUsuarioDTO objUsuario = new tblPUsuarioDTO();
                    string sql = @"SELECT * FROM tblP_Usuario WHERE estatus='true' AND _user LIKE'%" + objSubContra.rfc + "%'";
                    using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                    {
                        ctx.Open();
                        objUsuario = ctx.Query<tblPUsuarioDTO>(sql, null, null, true, 300).FirstOrDefault();
                        ctx.Close();
                    }
                    //var objUsuario = db.tblP_Usuario.Where(r => r._user == objSubContra.rfc).FirstOrDefault();
                    List<string> emails = new List<string>();
                    emails.Add(objUsuario.correo);
                    string Subject = "El nombre de la evaluacion : " + objAsignacion.nombreEvaluacion + " El elemento a revisar es :" + Evaluacion;
                    string msg2 = CuerpoCorreo(objAsignacion, objUsuario);
                    List<FirmantesDTO> lstFirmantes = new List<FirmantesDTO>();
                    string msg = GlobalUtils.cuerpoDeCorreoFormato("Prestadores de servicio", "Evaluaciones", "Asginacion de la evaluacion para prestadores de servicio", msg2, "Gracias por su atencion.", "http://expediente.construplan.com.mx/", lstFirmantes);

                    GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), Subject), msg, emails);
                }

            }
            catch (Exception)
            {
                objre.status = 0;
                objre.mensaje = "Algo ocurrio mal comuniquese con el departamento de ti.";
            }
            return objre;
        }

        public List<SubContratistasDTO> obtenerPromedioxElemento(List<SubContratistasDTO> parametros)
        {
            try
            {
                List<SubContratistasDTO> lstProm = new List<SubContratistasDTO>();
                int calculo = 0;

                foreach (var item in parametros)
                {
                    SubContratistasDTO promedio = new SubContratistasDTO();

                    var obj = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.id == item.idAsignacion).FirstOrDefault();
                    if (obj != null)
                    {
                        var objInicio = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == obj.id && r.tipoEvaluacion == item.tipoEvaluacion).FirstOrDefault();

                        if (objInicio != null)
                        {
                            var lstDeCalificaciones = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.calificacion > 0 && r.tipoEvaluacion == item.tipoEvaluacion && r.idEvaluacion == objInicio.id).ToList();
                            var lstReq = db.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == item.tipoEvaluacion).ToList();
                            if (lstDeCalificaciones.Count() != 0)
                            {
                                foreach (var item2 in lstDeCalificaciones)
                                {
                                    calculo += item2.calificacion;
                                }
                                promedio.promedio = Convert.ToDouble(calculo / lstReq.Count());
                                promedio.tipoEvaluacion = item.tipoEvaluacion;
                            }
                            else
                            {
                                promedio = new SubContratistasDTO();
                                promedio.promedio = 0;
                                promedio.tipoEvaluacion = item.tipoEvaluacion;
                            }

                        }
                        else
                        {
                            promedio = new SubContratistasDTO();
                            promedio.promedio = 0;
                            promedio.tipoEvaluacion = item.tipoEvaluacion;
                        }

                        var tieneCargaDeArchivo = db.tblCO_ADP_EvalSubContratistaDet.Any(x => x.tipoEvaluacion == item.tipoEvaluacion && x.idEvaluacion == objInicio.id && !string.IsNullOrEmpty(x.fechaCompromiso));
                        promedio.tieneCargaDeArchivo = tieneCargaDeArchivo;
                    }
                    lstProm.Add(promedio);
                }
                return lstProm;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
        #region funciones Generales
        public List<evaluadorXccDTO> getEvaluadoresxCC()
        {
            List<evaluadorXccDTO> lst = new List<evaluadorXccDTO>();
            try
            {
                lst = db.tblCO_ADP_EvaluadorXcc.Where(r => r.esActivo).ToList().Select(y => new evaluadorXccDTO
                {
                    id = y.id,
                    evaluador = y.evaluador,
                    nombreEvaluador = db.tblP_Usuario.Where(r => r.id == y.evaluador).FirstOrDefault() == null ? "" : db.tblP_Usuario.Where(r => r.id == y.evaluador).FirstOrDefault().nombre_completo,
                    esActivo = y.esActivo,
                    cc = y.cc,
                    lstCC = obtenerlst(y.cc),
                    lstElem = y.elementos,
                    lstElementos = obtenerElementos(y.elementos),
                }).ToList();
            }
            catch (Exception ex)
            {

            }
            return lst;
        }
        public List<string> obtenerElementos(string cadena)
        {
            List<string> lst = new List<string>();
            if (cadena == null)
            {
                lst.Add("");
            }
            else
            {
                var Arr = cadena.Split(',');
                foreach (var item in Arr)
                {
                    if (item != "")
                    {
                        int id = Convert.ToInt32(item);
                        string nombreCC = db.tblCO_ADP_EvaluacionDiv.Where(r => r.id == id).FirstOrDefault() == null ? "" : db.tblCO_ADP_EvaluacionDiv.Where(r => r.id == id).FirstOrDefault().descripcion;
                        lst.Add(item + " - " + nombreCC);
                    }
                }
            }
            return lst;
        }
        public List<string> obtenerlst(string cadena)
        {
            List<string> lst = new List<string>();
            if (cadena == null)
            {
                lst.Add("");
            }
            else
            {
                var Arr = cadena.Split(',');
                foreach (var item in Arr)
                {
                    string nombreCC = _context.tblP_CC.Where(r => r.cc == item).FirstOrDefault() == null ? "" : _context.tblP_CC.Where(r => r.cc == item).FirstOrDefault().descripcion;
                    lst.Add(item + " - " + nombreCC);
                }
            }
            return lst;
        }
        public evaluadorXccDTO AgregarEditarEvaluadores(evaluadorXccDTO parametros)
        {
            evaluadorXccDTO obj = new evaluadorXccDTO();
            try
            {
                var agregar = db.tblCO_ADP_EvaluadorXcc.Where(r => r.id == parametros.id && r.esActivo).FirstOrDefault();
                if (agregar == null)
                {
                    var Existe = db.tblCO_ADP_EvaluadorXcc.Where(r => r.evaluador == parametros.evaluador && r.esActivo).FirstOrDefault();
                    if (Existe == null)
                    {
                        Existe = new tblCO_ADP_EvaluadorXcc();
                        Existe.evaluador = parametros.evaluador;
                        Existe.esActivo = true;
                        Existe.elementos = parametros.lstElem;
                        Existe.cc = parametros.cc.Remove(parametros.cc.Length - 1);
                        db.tblCO_ADP_EvaluadorXcc.Add(Existe);
                        db.SaveChanges();
                        obj.estatus = 1;
                        obj.mensaje = "Evaluador Agregado Con Exito";
                    }
                    else
                    {
                        obj.estatus = 2;
                        obj.mensaje = "Ya existe un evaluador revise porfabor";
                    }
                }
                else
                {
                    agregar.elementos = parametros.lstElem;
                    agregar.cc = parametros.cc;
                    db.SaveChanges();
                    obj.estatus = 1;
                    obj.mensaje = "Evaluador Modificado Con Exito";
                }

            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        public evaluadorXccDTO ActivarDesactivarEvaluadores(evaluadorXccDTO parametros)
        {
            evaluadorXccDTO obj = new evaluadorXccDTO();
            try
            {
                var Activar = db.tblCO_ADP_EvaluadorXcc.Where(r => r.id == parametros.id).FirstOrDefault();
                if (Activar != null)
                {
                    Activar.esActivo = false;
                    db.SaveChanges();
                    obj.estatus = 1;
                    obj.mensaje = "Evaluador Eliminado Con Exito";
                }
                else
                {
                    obj.estatus = 2;
                    obj.mensaje = "Algo ocurrio al quere eliminar el registro comuniquese con el departamento de TI.";
                }

            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        public List<ComboDTO> getProyectoRestantes(bool Agregar)
        {
            List<ComboDTO> obj = new List<ComboDTO>();
            try
            {
                List<string> todosLosCCActivos = new List<string>();
                List<string> todosLosCCActivosx = new List<string>();

                if (Agregar == true)
                {
                    var lst = db.tblCO_ADP_EvaluadorXcc.Where(r => r.esActivo).Select(y => y.cc).ToList();
                    var lst2 = db.tblCO_ADP_EvaluadorXcc.Where(r => r.esActivo).ToList();
                    if (lst2.Count() > 0)
                    {
                        foreach (var item in lst)
                        {
                            todosLosCCActivos = obtenerTodolosCC(item);
                            foreach (var item2 in todosLosCCActivos)
                            {
                                todosLosCCActivosx.Add(item2);
                            }
                        }
                    }

                    obj = _context.tblP_CC.Where(r => !todosLosCCActivosx.Contains(r.cc)).ToList().Select(y => new ComboDTO
                    {
                        Value = y.cc.ToString(),
                        Text = y.cc.ToString() + " - " + y.descripcion
                    }).ToList();
                }
                else
                {
                    var lst = db.tblCO_ADP_EvaluadorXcc.Where(r => r.esActivo).Select(y => y.cc).ToList();
                    var lst2 = db.tblCO_ADP_EvaluadorXcc.Where(r => r.esActivo).ToList();
                    if (lst2.Count() > 0)
                    {
                        foreach (var item in lst)
                        {
                            todosLosCCActivos = obtenerTodolosCC(item);
                            foreach (var item2 in todosLosCCActivos)
                            {
                                todosLosCCActivosx.Add(item2);
                            }
                        }
                    }

                    obj = _context.tblP_CC.ToList().Select(y => new ComboDTO
                    {
                        Value = y.cc.ToString(),
                        Text = y.cc.ToString() + " - " + y.descripcion
                    }).ToList();
                }


            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        public List<ComboDTO> obtenerTodolosElementos()
        {
            List<ComboDTO> lstobtenerElementos = new List<ComboDTO>();
            try
            {
                lstobtenerElementos = db.tblCO_ADP_EvaluacionDiv.Where(r => !r.SubContratista).Select(y => new ComboDTO
                {
                    Value = y.id.ToString(),
                    Text = y.descripcion
                }).ToList();
            }
            catch (Exception ex)
            {
            }
            return lstobtenerElementos;
        }
        public List<string> obtenerTodolosCC(string cc)
        {
            List<string> lst = new List<string>();
            if (cc == null)
            {
                lst.Add("");
            }
            else
            {
                var array = cc.Split(',');
                foreach (var item in array)
                {
                    lst.Add(item);
                }
            }
            return lst;
        }
        public List<ComboDTO> getSubContratistasRestantes()
        {
            List<ComboDTO> obj = new List<ComboDTO>();
            try
            {
                obj = db.tblP_Usuario.Where(r => r.estatus && r.tipo == 10).ToList().Select(y => new ComboDTO
                {
                    Value = y.id.ToString(),
                    Text = y.nombre_completo
                }).ToList();
            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        public string formatearStrign(List<int> datos)
        {
            string n = "";
            foreach (var item in datos)
            {
                n += item + ",";
            }
            return n;
        }
        public Dictionary<string, object> obtenerElementosEvaluar(int idUsuario, int idPlantilla)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                string nombreUsuario = vSesiones.sesionUsuarioDTO.nombreUsuario;
                tblPUsuarioDTO obtener = new tblPUsuarioDTO();
                string sql = @"SELECT * FROM tblP_Usuario WHERE tipo=10 AND estatus='true' AND _user LIKE'%" + nombreUsuario + "%'";
                using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                {
                    ctx.Open();
                    obtener = ctx.Query<tblPUsuarioDTO>(sql, null, null, true, 300).FirstOrDefault();
                    ctx.Close();
                }
                if (obtener != null)
                {
                    if (obtener.tipo == 1002)
                    {
                        var l = db.tblCO_ADP_EvaluacionDiv.Where(x => !x.SubContratista && x.idPlantilla == idPlantilla).ToList().Select(n => n.id).ToList();
                        var s = formatearStrign(l);
                        resultado.Add(ITEMS, s);
                        resultado.Add(SUCCESS, true);
                    }
                    else if (obtener.tipo == 10)
                    {
                        var n = db.tblCO_ADP_EvaluadorXcc.Where(r => r.evaluador == obtener.id && r.esActivo).FirstOrDefault();
                        var lst = obtenerId(n.elementos);
                        if (n != null)
                        {
                            var l = db.tblCO_ADP_EvaluacionDiv.Where(x => lst.Contains(x.id) && !x.SubContratista && x.idPlantilla == idPlantilla).ToList().Select(fn => fn.id).ToList();
                            var s = formatearStrign(l);
                            resultado.Add(ITEMS, s);
                            resultado.Add(SUCCESS, true);
                        }
                    }
                    else
                    {
                        var l = db.tblCO_ADP_EvaluacionDiv.Where(x => !x.SubContratista && x.idPlantilla == idPlantilla).ToList().Select(n => n.id).ToList();
                        var s = formatearStrign(l);
                        resultado.Add(ITEMS, s);
                        resultado.Add(SUCCESS, true);

                    }
                }

            }
            catch (Exception ex)
            {
                resultado.Add(ITEMS, "");
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public List<int> obtenerId(string n)
        {
            List<int> lst = new List<int>();
            var lstRe = n.Split(',');
            foreach (var item in lstRe)
            {
                if (item != "")
                {
                    lst.Add(Convert.ToInt32(item));
                }
            }
            return lst;
        }

        public double ObtenerPromedioPorElemento(tblCO_ADP_EvalSubContratista objContratista)
        {
            double total = 0;
            var lstDetalle = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.idEvaluacion == objContratista.id).ToList();
            foreach (var item in lstDetalle)
            {
                total += item.calificacion;
            }
            if (total == 0)
            {
                return total;
            }
            else
            {
                total = total / lstDetalle.Count();
            }
            return total;
        }
        #endregion
        #region GRAFICAS

        // ESTA ES LA GRAFICA DE CENTROS DE COSTOS
        public Dictionary<string, object> ObtenerGraficaDeEvaluacionPorCentroDeCosto(SubContratistasDTO parametros)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                //AQUI ESTOY TRABAJANDO
                var lstCentrosDeCostosAsignados = db.tblCO_ADP_EvalSubConAsignacion.Select(y => y.cc).ToList().Distinct();
                var lstCentrosDeCostos = _context.tblP_CC.Where(r => lstCentrosDeCostosAsignados.Contains(r.cc)).Select(y => new
                {
                    cc = y.cc,
                    descripcion = y.cc + " - " + y.descripcion
                }).ToList();
                var lstDatoPendientes = db.tblCO_ADP_EvalSubConAsignacion.Where(r => (r.statusAutorizacion == 2 || r.statusAutorizacion == 1) && r.esActivo).ToList();
                var lstDatoRealizadas = db.tblCO_ADP_EvalSubConAsignacion.Where(r => (r.statusAutorizacion == 3 || r.statusAutorizacion == 4) && !r.esActivo).ToList();
                var ListaDeDatosPendientesFiltrados = lstCentrosDeCostos.Select(y => new
                {
                    y = lstDatoPendientes.Where(r => r.cc == y.cc).ToList().Count(),
                    name = y.descripcion,
                }).ToList();
                var ListaDeDatosRealizadasFiltrados = lstCentrosDeCostos.Select(y => new
                {
                    y = lstDatoRealizadas.Where(r => r.cc == y.cc).ToList().Count(),
                    name = y.descripcion,
                }).ToList();

                var ObjDatoPendiente = new
                {
                    name = "Pendientes",
                    colorByPoint = true,
                    data = ListaDeDatosPendientesFiltrados,
                };
                var ObjDatoRealizado = new
                {
                    name = "Realizadas",
                    colorByPoint = true,
                    data = ListaDeDatosRealizadasFiltrados,
                };
                List<object> lstResultadoDeGRaficas = new List<object>();
                lstResultadoDeGRaficas.Add(ObjDatoPendiente);
                lstResultadoDeGRaficas.Add(ObjDatoRealizado);

                int lista1 = ListaDeDatosPendientesFiltrados.Select(y => y.y).Max();
                int lista2 = ListaDeDatosRealizadasFiltrados.Select(y => y.y).Max();
                int numeroMaximo = 0;
                if (lista1 > lista2)
                {
                    numeroMaximo = lista1 + 1;
                }
                else
                {
                    numeroMaximo = lista2 + 1;
                }

                resultado.Add("numMaximo", numeroMaximo);
                resultado.Add("gpxGrafica", lstResultadoDeGRaficas);
                resultado.Add(SUCCESS, true);

            }
            catch (Exception ex)
            {

            }
            return resultado;
        }
        // GRAFICA DE SUBCONTRATISTAS POR DIVISION/ELEMENTO
        public Dictionary<string, object> ObtenerGraficaDeEvaluacionPorDivisionElemento(SubContratistasDTO parametros)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                //AQUI ESTOY TRABAJANDO
                var LstElementos = db.tblCO_ADP_EvaluacionDiv.Where(r => !r.SubContratista).ToList();

                var lstContratista = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == parametros.id).ToList();
                if (lstContratista != null)
                {
                    var lstSubContratistas = LstElementos.Select(y => new
                    {
                        name = y.descripcion,
                        y = ObtenerPromedioPorElemento(lstContratista.Where(r => r.tipoEvaluacion == y.id).FirstOrDefault()),
                    }).ToList();


                    var ObjDatoRealizado = new
                    {
                        name = "Calificacion",
                        colorByPoint = true,
                        data = lstSubContratistas,
                    };
                    List<object> lstResultadoDeGRaficas = new List<object>();
                    lstResultadoDeGRaficas.Add(ObjDatoRealizado);

                    resultado.Add("gpxGrafica", lstResultadoDeGRaficas);
                    resultado.Add(SUCCESS, true);

                }

            }
            catch (Exception ex)
            {

            }
            return resultado;
        }
        //nnnn

        //POR CENTRO DE COSTO SUBCONTRATISTA MOSTRARIA SU AVANCE EN CUANTO A TRAYECTIRIA DE EVALUACION EN UNA GRAFICA DE TENDENCIA
        public Dictionary<string, object> ObtenerGraficaDeEvaluacionPorTendenciaMedianteEvaluaciones(SubContratistasDTO parametros)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                //AQUI ESTOY TRABAJANDO

                List<gpGraficaDTO> lstFormateada = traerResultadoMensual(parametros.cc, parametros.idSubContratista);
                var objResultado = lstFormateada.Select(y => new
                {
                    name = y.mes,
                    y = y.calificacion,
                }).ToList();
                var obj = new
                {
                    name = "Calificacion Mensual",
                    colorByPoint = true,
                    data = objResultado
                };
                List<object> lstResultado = new List<object>();
                lstResultado.Add(obj);

                resultado.Add("gpxGrafica", lstResultado);
                resultado.Add(SUCCESS, true);

            }
            catch (Exception ex)
            {

            }
            return resultado;
        }

        public List<gpGraficaDTO> traerResultadoMensual(string cc, int idSubcontratista)
        {
            List<gpGraficaDTO> lstRetornar = new List<gpGraficaDTO>();
            gpGraficaDTO objRetornar = new gpGraficaDTO();
            var Asignacion = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.cc == cc && r.idSubContratista == idSubcontratista).ToList();



            for (int i = 1; i <= 12; i++)
            {
                var obtenerMes = Asignacion.Where(r => r.fechaCreacion.Year == DateTime.Now.Year && r.fechaCreacion.Month == i).FirstOrDefault();
                if (obtenerMes != null)
                {
                    objRetornar = new gpGraficaDTO();
                    objRetornar.mes = retornarNombreMes(i);
                    objRetornar.calificacion = obtenerPromedio(obtenerMes.id);
                    lstRetornar.Add(objRetornar);
                }
                else
                {
                    objRetornar = new gpGraficaDTO();
                    objRetornar.mes = retornarNombreMes(i);
                    objRetornar.calificacion = 0;
                    lstRetornar.Add(objRetornar);
                }
            }
            return lstRetornar;
        }
        public string retornarNombreMes(int mes)
        {
            string rt = "";
            switch (mes)
            {
                case 1:
                    rt = "ENERO";
                    break;
                case 2:
                    rt = "FEBRERO";
                    break;
                case 3:
                    rt = "MARZO";
                    break;
                case 4:
                    rt = "ABRIL";
                    break;
                case 5:
                    rt = "MAYO";
                    break;
                case 6:
                    rt = "JUNIO";
                    break;
                case 7:
                    rt = "JULIO";
                    break;
                case 8:
                    rt = "AGOSTO";
                    break;
                case 9:
                    rt = "SEPTIEMBRE";
                    break;
                case 10:
                    rt = "OCTUBRE";
                    break;
                case 11:
                    rt = "NOVIEMBRE";
                    break;
                case 12:
                    rt = "DICIEMBRE";
                    break;
            }

            return rt;
        }



        //hiiii

        #endregion


//        #region Gestión
//        public Dictionary<string, object> ObtenerProyectosParaFiltro()
//        {
//            using (var _ctx = new contextSigoplan())
//            {
//                try
//                {
//                    var proyectos = _ctx.tblP_CC.Select(x => new ComboBoxDTO
//                    {
//                        valor = x.cc,
//                        texto = "[" + x.cc + "] " + x.descripcion.Trim(),
//                        datas = new List<ComboBoxDataDTO>
//                        {
//                            new ComboBoxDataDTO {
//                                data = "id",
//                                valor = x.id.ToString()
//                            }
//                        }
//                    }).OrderBy(x => x.valor).ToList();

//                    resultado.Add(SUCCESS, true);
//                    resultado.Add(ITEMS, proyectos);
//                }
//                catch (Exception ex)
//                {
//                    resultado.Add(SUCCESS, false);
//                    resultado.Add(MESSAGE, ex.Message);
//                }
//            }

//            return resultado;
//        }

//        public Dictionary<string, object> ObtenerSubcontratistasParaFiltro(string proyecto)
//        {
//            using (var _ctx = new MainContext())
//            {
//                try
//                {
//#if DEBUG
//                    var subcontratistas = _ctx.tblX_Contrato.Where(x =>
//                    x.cc == proyecto &&
//                    x.estatus).Select(x => new ComboBoxDTO
//                    {
//                        valor = x.subcontratistaID.ToString(),
//                        texto = x.subcontratista.nombre.Trim()
//                    }).Distinct().OrderBy(x => x.texto).ToList();
//#else
//                    var subcontratistas = _ctx.tblX_Contrato.Where(x =>
//                    x.cc == proyecto &&
//                    x.estatus &&
//                    x.subcontratista.estatus).Select(x => new ComboBoxDTO
//                    {
//                        valor = x.subcontratistaID.ToString(),
//                        texto = x.subcontratista.nombre.Trim()
//                    }).Distinct().OrderBy(x => x.texto).ToList();
//#endif

//                    resultado.Add(SUCCESS, true);
//                    resultado.Add(ITEMS, subcontratistas);
//                }
//                catch (Exception ex)
//                {
//                    resultado.Add(SUCCESS, false);
//                    resultado.Add(MESSAGE, ex.Message);
//                }
//            }

//            return resultado;
//        }

//        public Dictionary<string, object> ObtenerEvaluacionesSubcontratistas(string proyecto, int? subcontratistaId)
//        {
            
//                try
//                {
//                    var evaluaciones = db.tblCO_ADP_EvalSubConAsignacion.Where(x =>
//                        !x.esActivo &&
//                        x.statusAutorizacion == 3 &&
//                        (subcontratistaId.HasValue ? x.idSubContratista == subcontratistaId.Value : true) &&
//                        (!string.IsNullOrEmpty(proyecto) ? x.cc == proyecto : true)).Select(x => new EvaluacionParaFirmarDTO
//                        {
//                            evaluacionId = x.id,
//                            contratoId = x.idContrato,
//                            numeroContrato = x.contrato.numeroContrato,
//                            subcontratistaId = x.idSubContratista,
//                            nombreSubcontratista = x.subcontratista.nombre,
//                            proyecto = x.cc,
//                            nombreProyecto = x.contrato.proyecto.nombre,
//                            nombreEvaluacion = x.nombreEvaluacion,
//                            firma = x.firma.FirstOrDefault(y => y.esActivo)
//                        }).ToList();

//                    var ccEvaluaciones = evaluaciones.Select(x => x.proyecto).ToList();//

//                    var firmantes = db.tblX_Firmante.Where(x => x.esActivo && (x.orden == 0 || ccEvaluaciones.Contains(x.cc))).OrderBy(x => x.orden).ToList();//

//                    foreach (var evaluacion in evaluaciones)
//                    {
//                        if (evaluacion.firma == null)
//                        {
//                            var firmante = firmantes.First();
//                            if (firmante.esSubcontratista)
//                            {
//                                var subcontratista = _ctx.tblX_SubContratista.First(x => x.id == evaluacion.subcontratistaId);
//                                var usuario = db.tblP_Usuario.First(x => x._user == subcontratista.rfc);
//                                evaluacion.elUsuarioPuedeFirmar = usuario.id == vSesiones.sesionUsuarioDTO.id;
//                            }
//                            else
//                            {
//                                evaluacion.elUsuarioPuedeFirmar = false;
//                            }
//                        }
//                        else
//                        {
//                            evaluacion.firmado = evaluacion.firma.tieneTodasLasFirmas;
//                            evaluacion.elUsuarioPuedeFirmar = evaluacion.firmado ? false : evaluacion.firma.firmantePendiente.usuarioId == vSesiones.sesionUsuarioDTO.id;
//                        }

//                        evaluacion.firma = null;
//                    }

//                    resultado.Add(SUCCESS, true);
//                    resultado.Add(ITEMS, evaluaciones);
//                }
//                catch (Exception ex)
//                {
//                    resultado.Add(SUCCESS, false);
//                    resultado.Add(MESSAGE, ex.Message);
//                }
            

//            return resultado;
//        }

//        public Dictionary<string, object> ObtenerEstatusFirmantes(int evaluacionId)
//        {
           
//                try
//                {
//                    var asignacion = db.tblCO_ADP_EvalSubConAsignacion.FirstOrDefault(x => x.id == evaluacionId && !x.esActivo && x.statusAutorizacion == 3);
//                    if (asignacion != null)
//                    {
//                        var firmantes = db.tblX_Firmante.Where(x => x.esActivo && (x.orden == 0 || asignacion.cc == x.cc)).OrderBy(x => x.orden).ToList();

//                        var firmaEvaluacion = db.tblX_FirmaEvaluacion.FirstOrDefault(x => x.evaluacionId == evaluacionId && x.esActivo);

//                        var estatusFirmas = new List<EstatusFirmasEvaluacionDTO>();

//                        foreach (var firmante in firmantes)
//                        {
//                            var estatusFirma = new EstatusFirmasEvaluacionDTO();
//                            estatusFirma.puesto = firmante.puesto;

//                            if (firmante.esSubcontratista)
//                            {
//                                estatusFirma.nombreCompleto = asignacion.contrato.subcontratista.nombre.ToUpper();
//                            }
//                            else
//                            {
//                                estatusFirma.nombreCompleto = firmante.nombre + " " + firmante.apellidoPaterno +
//                                (firmante.apellidoMaterno != null ? " " + firmante.apellidoMaterno : "");
//                            }

//                            if (firmaEvaluacion == null)
//                            {
//                                estatusFirma.estatusFirma = false;
//                            }
//                            else if (firmaEvaluacion.tieneTodasLasFirmas)
//                            {
//                                estatusFirma.estatusFirma = true;
//                            }
//                            else
//                            {
//                                estatusFirma.estatusFirma = firmante.orden < firmaEvaluacion.numeroOrdenFirmantePendiente.Value;
//                            }

//                            if (firmaEvaluacion != null)
//                            {
//                                var fechaFirma = firmaEvaluacion.detalle.FirstOrDefault(x => x.firmanteId == firmante.id);
//                                if (fechaFirma != null)
//                                {
//                                    estatusFirma.fechaAutorizacion = fechaFirma.fechaCreacion;
//                                }
//                            }

//                            estatusFirmas.Add(estatusFirma);
//                        }

//                        resultado.Add(SUCCESS, true);
//                        resultado.Add(ITEMS, estatusFirmas);
//                    }
//                    else
//                    {
//                        throw new Exception("No existe la evaluación");
//                    }
//                }
//                catch (Exception ex)
//                {
//                    resultado.Add(SUCCESS, false);
//                    resultado.Add(MESSAGE, ex.Message);
//                }
            

//            return resultado;
//        }

//        public Dictionary<string, object> ObtenerFirmante(int evaluacionId)
//        {
//            using (var _ctx = new MainContext())
//            {
//                try
//                {
//                    var asignacion = _ctx.tblCO_ADP_EvalSubConAsignacion.FirstOrDefault(x => x.id == evaluacionId && !x.esActivo && x.statusAutorizacion == 3);
//                    if (asignacion != null)
//                    {
//                        var firmaPendiente = _ctx.tblX_FirmaEvaluacion.FirstOrDefault(x => x.evaluacionId == evaluacionId && x.esActivo);

//                        var infoParaFirmar = new InfoParaFirmarDTO();

//                        if (firmaPendiente != null)
//                        {
//                            if (firmaPendiente.tieneTodasLasFirmas)
//                            {
//                                throw new Exception("La evaluación ya tiene todas las firmas correspondientes");
//                            }
//                            else
//                            {
//                                if (firmaPendiente.firmantePendiente.esSubcontratista)
//                                {
//                                    var usuario = _ctx.tblP_Usuario.First(x => x._user == asignacion.contrato.subcontratista.rfc);
//                                    if (usuario.id == vSesiones.sesionUsuarioDTO.id)
//                                    {
//                                        infoParaFirmar.firmanteId = firmaPendiente.firmantePendienteId.Value;
//                                        infoParaFirmar.puestoDelFirmante = firmaPendiente.firmantePendiente.puesto.ToUpper();
//                                        infoParaFirmar.nombreCompletoFirmante = asignacion.contrato.subcontratista.nombre;
//                                        infoParaFirmar.evaluacionId = evaluacionId;
//                                    }
//                                    else
//                                    {
//                                        throw new Exception("Usted no es el subcontratista");
//                                    }
//                                }
//                                else
//                                {
//                                    throw new Exception("Para realizar las firmas pendientes debe ingresar al modulo desde sigoplan");
//                                }
//                            }
//                        }
//                        else
//                        {
//                            var firmantePendiente = _ctx.tblX_Firmante.Where(x => x.esActivo).OrderBy(x => x.orden).First();
//                            if (firmantePendiente.esSubcontratista)
//                            {
//                                var usuario = _ctx.tblP_Usuario.First(x => x._user == asignacion.contrato.subcontratista.rfc);
//                                if (usuario.id == vSesiones.sesionUsuarioDTO.id)
//                                {
//                                    infoParaFirmar.firmanteId = firmantePendiente.id;
//                                    infoParaFirmar.puestoDelFirmante = firmantePendiente.puesto.ToUpper();
//                                    infoParaFirmar.nombreCompletoFirmante = asignacion.contrato.subcontratista.nombre;
//                                    infoParaFirmar.evaluacionId = evaluacionId;
//                                }
//                                else
//                                {
//                                    throw new Exception("Usted no es el subcontratista");
//                                }
//                            }
//                            else
//                            {
//                                throw new Exception("Para realizar las firmas pendientes debe ingresar al modulo desde sigoplan");
//                            }
//                        }

//                        resultado.Add(SUCCESS, true);
//                        resultado.Add(ITEMS, infoParaFirmar);
//                    }
//                    else
//                    {
//                        throw new Exception("No existe la evaluación");
//                    }
//                }
//                catch (Exception ex)
//                {
//                    resultado.Add(SUCCESS, false);
//                    resultado.Add(MESSAGE, ex.Message);
//                }
//            }

//            return resultado;
//        }

//        public Dictionary<string, object> GuardarFirma(InformacionFirmaDigitalDTO firma)
//        {
//            using (var _ctx = new MainContext())
//            {
//                using (var transaccion = _ctx.Database.BeginTransaction())
//                {
//                    try
//                    {
//                        var evaluacion = _ctx.tblCO_ADP_EvalSubConAsignacion.FirstOrDefault(x => x.id == firma.evaluacionId && !x.esActivo && x.statusAutorizacion == 3);
//                        if (evaluacion != null)
//                        {
//                            var firmantes = _ctx.tblX_Firmante.Where(x => x.esActivo && (x.orden == 0 || evaluacion.cc == x.cc)).OrderBy(x => x.orden).ToList();

//                            if (firmantes.Count == 0)
//                            {
//                                throw new Exception("No existen usuarios para firmar");
//                            }
//                            else
//                            {
//                                var nuevaFirma = new tblX_FirmaEvaluacion();
//                                var nuevaFirmaDetalle = new tblX_FirmaEvaluacionDetalle();

//                                var firmaPendiente = _ctx.tblX_FirmaEvaluacion.FirstOrDefault(x => x.evaluacionId == firma.evaluacionId && x.esActivo);
//                                if (firmaPendiente != null)
//                                {
//                                    if (firmaPendiente.tieneTodasLasFirmas)
//                                    {
//                                        throw new Exception("La evaluación ya cuenta con todas las firmas correspondientes");
//                                    }
//                                    else
//                                    {
//                                        if (firmaPendiente.firmantePendiente.esSubcontratista)
//                                        {
//                                            var usuario = _ctx.tblP_Usuario.First(x => x._user == evaluacion.contrato.subcontratista.rfc);
//                                            if (usuario.id == vSesiones.sesionUsuarioDTO.id)
//                                            {

//                                                firmaPendiente.tieneTodasLasFirmas = firma.firmanteId == firmantes.Last().id;
//                                                firmaPendiente.firmantePendienteId = firmaPendiente.tieneTodasLasFirmas ? (int?)null :
//                                                firmantes.First(x => x.orden > firmantes.First(y => y.id == firma.firmanteId).orden).id;
//                                                firmaPendiente.numeroOrdenFirmantePendiente = firmaPendiente.tieneTodasLasFirmas ? (int?)null :
//                                                    firmantes.First(x => x.id == firmaPendiente.firmantePendienteId).orden;
//                                                firmaPendiente.esActivo = true;
//                                                firmaPendiente.fechaModificacion = DateTime.Now;
//                                                firmaPendiente.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
//                                                nuevaFirma = firmaPendiente;
//                                            }
//                                            else
//                                            {
//                                                throw new Exception("Usted no puede realizar la firma de subcontratista");
//                                            }
//                                        }
//                                        else
//                                        {
//                                            throw new Exception("Para realizar las firmas pendientes debe ingresar al modulo desde sigoplan");
//                                        }
//                                    }
//                                }
//                                else
//                                {
//                                    if (firmantes.First().esSubcontratista)
//                                    {
//                                        var usuario = _ctx.tblP_Usuario.First(x => x._user == evaluacion.contrato.subcontratista.rfc);
//                                        if (usuario.id == vSesiones.sesionUsuarioDTO.id)
//                                        {
//                                            nuevaFirma.evaluacionId = evaluacion.id;
//                                            nuevaFirma.evaluacion = evaluacion;
//                                            nuevaFirma.tieneTodasLasFirmas = firmantes.Count == 1;
//                                            if (firmantes.Count == 1)
//                                            {
//                                                nuevaFirma.firmantePendienteId = null;
//                                            }
//                                            else
//                                            {
//                                                var firmanteSiguiente = firmantes.Skip(1).First();
//                                                nuevaFirma.firmantePendienteId = firmanteSiguiente.id;
//                                                nuevaFirma.numeroOrdenFirmantePendiente = firmanteSiguiente.orden;
//                                            }

//                                            nuevaFirma.esActivo = true;
//                                            nuevaFirma.fechaCreacion = DateTime.Now;
//                                            nuevaFirma.fechaModificacion = nuevaFirma.fechaCreacion;
//                                            nuevaFirma.usuarioCreacionId = vSesiones.sesionUsuarioDTO.id;
//                                            nuevaFirma.usuarioModificacionId = nuevaFirma.usuarioCreacionId;

//                                            _ctx.tblX_FirmaEvaluacion.Add(nuevaFirma);
//                                        }
//                                        else
//                                        {
//                                            throw new Exception("Para las firmas que no son de subcontratistas favor de ingresar al modulo de sigoplan");
//                                        }
//                                    }
//                                    else
//                                    {
//                                        throw new Exception("Para realizar las firmas pendientes debe ingresar al modulo desde sigoplan");
//                                    }
//                                }
//                                _ctx.SaveChanges();

//                                nuevaFirmaDetalle.firmaEvaluacionId = nuevaFirma.id;
//                                nuevaFirmaDetalle.firmanteId = firma.firmanteId;
//                                nuevaFirmaDetalle.esActivo = true;
//                                nuevaFirmaDetalle.fechaCreacion = nuevaFirma.fechaModificacion;
//                                nuevaFirmaDetalle.usuarioCreacionId = nuevaFirma.usuarioCreacionId;
//                                nuevaFirmaDetalle.urlFirma = "";

//                                _ctx.tblX_FirmaEvaluacionDetalle.Add(nuevaFirmaDetalle);
//                                _ctx.SaveChanges();

//                                #region archivo firma
//                                var pathCompleto = "";
//                                byte[] imagenBytes = Convert.FromBase64String(firma.firmaDigitalBase64.Split(',')[1]);
//                                using (var ms = new MemoryStream(imagenBytes, 0, imagenBytes.Length))
//                                {
//                                    Image image = Image.FromStream(ms, true);

//                                    var bmpImage = (System.Drawing.Bitmap)image;
//                                    var bmp = new System.Drawing.Bitmap(bmpImage.Size.Width, bmpImage.Size.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
//                                    var gpx = System.Drawing.Graphics.FromImage(bmp);
//                                    gpx.Clear(System.Drawing.Color.White);
//                                    gpx.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
//                                    gpx.DrawImage(bmpImage, 0, 0);

//                                    Bitmap myBitmap;
//                                    ImageCodecInfo myImageCodecInfo;
//                                    System.Drawing.Imaging.Encoder myEncoder;
//                                    EncoderParameter myEncoderParameter;
//                                    EncoderParameters myEncoderParameters;

//                                    myBitmap = new Bitmap(bmp, 800, 800);
//                                    myImageCodecInfo = GetEncoderInfo("image/jpeg");
//                                    myEncoder = System.Drawing.Imaging.Encoder.Quality;
//                                    myEncoderParameters = new EncoderParameters(1);
//                                    myEncoderParameter = new EncoderParameter(myEncoder, 90L);
//                                    myEncoderParameters.Param[0] = myEncoderParameter;

//                                    using (var memStream = new MemoryStream())
//                                    {
//                                        myBitmap.Save(memStream, myImageCodecInfo, myEncoderParameters);
//                                        Image newImage = Image.FromStream(memStream);
//                                        ImageAttributes imageAttributes = new ImageAttributes();
//                                        using (Graphics g = Graphics.FromImage(newImage))
//                                        {
//                                            g.InterpolationMode =
//                                                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
//                                            g.DrawImage(newImage, new Rectangle(Point.Empty, newImage.Size), 0, 0,
//                                                newImage.Width, newImage.Height, GraphicsUnit.Pixel, imageAttributes);
//                                        }

//#if DEBUG
//                                        var folderFima = nuevaFirma.evaluacionId.ToString() + "_" + nuevaFirma.evaluacion.contrato.id.ToString();
//                                        var nombreArchivo = "firmaDigital_" + nuevaFirmaDetalle.firmanteId.ToString() + nuevaFirma.fechaModificacion.ToString("yyyyMMddTHHmmssfff") + ".jpeg";
//                                        pathCompleto = @"C:\Proyecto\SUBCONTRATISTAS\FIRMAS_EVALUACIONES\" + folderFima + @"\";
//#else
//                                        var folderFima = nuevaFirma.evaluacionId.ToString() + "_" + nuevaFirma.evaluacion.contrato.id.ToString();
//                                        var nombreArchivo = "firmaDigital_" + nuevaFirmaDetalle.firmanteId.ToString() + nuevaFirma.fechaModificacion.ToString("yyyyMMddTHHmmssfff") + ".jpeg";
//                                        pathCompleto = @"\\10.1.0.49\Proyecto\SUBCONTRATISTAS\FIRMAS_EVALUACIONES\" + folderFima + @"\";
//#endif
//                                        var directorio = new DirectoryInfo(pathCompleto);
//                                        if (!directorio.Exists)
//                                        {
//                                            directorio.Create();
//                                        }

//                                        pathCompleto = Path.Combine(pathCompleto, nombreArchivo);

//                                        newImage.Save(pathCompleto);
//                                    }
//                                }
//                                #endregion

//                                nuevaFirmaDetalle.urlFirma = pathCompleto;
//                                _ctx.SaveChanges();

//                                #region NOTIFICAR SIGUIENTE FIRMANTE
//                                if (!nuevaFirmaDetalle.firmaEvaluacion.tieneTodasLasFirmas)
//                                {
//                                    if (nuevaFirmaDetalle.firmaEvaluacion.firmantePendienteId.HasValue)
//                                    {
//                                        var correo = new Infrastructure.DTO.CorreoDTO();
//                                        correo.asunto = "Se ha realizado la evaluación " + nuevaFirmaDetalle.firmaEvaluacion.evaluacion.nombreEvaluacion + " y necesita su firma";
//                                        var firmanteSig = firmantes.Skip(1).First().id;
//                                        var objUsuario = db.tblP_Usuario.Where(r => r.id == firmanteSig).FirstOrDefault();
//                                        string msg2 = CuerpoCorreo(evaluacion, objUsuario);
//                                        List<FirmantesDTO> lstFirmantes = new List<FirmantesDTO>();
//                                        var firmanteSubObj = firmantes.Skip(0).First();
//                                        var firmanteSigObj = firmantes.Skip(1).First();
//                                        FirmantesDTO objFirmantes = new FirmantesDTO();

//                                        objFirmantes.puesto = firmanteSubObj.puesto;
//                                        objFirmantes.nombreCompleto = nombrecompletosubcon(evaluacion.idSubContratista);
//                                        objFirmantes.Fecha = DateTime.Now.ToShortDateString();
//                                        objFirmantes.estado = true;
//                                        objFirmantes.Firma = "FIRMADA";
//                                        lstFirmantes.Add(objFirmantes);
                                        
//                                        objFirmantes = new FirmantesDTO();
//                                        objFirmantes.puesto = firmanteSigObj.puesto;
//                                        objFirmantes.nombreCompleto = firmanteSigObj.nombre;
//                                        objFirmantes.Fecha = "";
//                                        objFirmantes.estado = false;
//                                        objFirmantes.Firma = "";
//                                        lstFirmantes.Add(objFirmantes);


//                                        string msg = GlobalUtils.cuerpoDeCorreoFormato("Prestadores de servicio", "Evaluaciones", "Asginacion de la evaluacion para prestadores de servicio", msg2, "Gracias por su atencion.", "http://expediente.construplan.com.mx/", lstFirmantes);
//                                        correo.cuerpo = msg;
//#if DEBUG
//                                        correo.correos = new List<string> { "adan.gonzalez@construplan.com.mx", "martin.zayas@construplan.com.mx" };
//#else
//                                        correo.correos = new List<string> { nuevaFirmaDetalle.firmaEvaluacion.firmantePendiente.correo };
//#endif
//                                        //correo.Enviar();

//                                        GlobalUtils.sendEmail(correo.asunto, correo.cuerpo, correo.correos);
//                                    }
//                                }
//                                #endregion

//                                transaccion.Commit();

//                                resultado.Add(SUCCESS, true);
//                            }
//                        }
//                        else
//                        {
//                            throw new Exception("No existe la evaluación");
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        transaccion.Rollback();

//                        resultado.Add(SUCCESS, false);
//                        resultado.Add(MESSAGE, ex.Message);
//                    }
//                }
//            }

//            return resultado;
//        }

//        public string nombrecompletosubcon(int idSubContratista)
//        {
//            return db.tblX_SubContratista.Where(d => d.id == idSubContratista).FirstOrDefault() == null ? "" : db.tblX_SubContratista.Where(d => d.id == idSubContratista).FirstOrDefault().nombre;
//        }
//        public tblX_FirmaEvaluacionDetalle obtenerObjDetalle(int usuarioId)
//        {
//            tblX_FirmaEvaluacionDetalle obj = db.tblX_FirmaEvaluacionDetalle.Where(r => r.firmanteId == usuarioId).FirstOrDefault();
//            return obj;
//        }

//        private static ImageCodecInfo GetEncoderInfo(String mimeType)
//        {
//            int j;
//            ImageCodecInfo[] encoders;
//            encoders = ImageCodecInfo.GetImageEncoders();
//            for (j = 0; j < encoders.Length; ++j)
//            {
//                if (encoders[j].MimeType == mimeType)
//                    return encoders[j];
//            }
//            return null;
//        }
//        #endregion


        public bool retornarValor(int idAsignacion, int tipoEvaluacion)
        {
            bool ns = false;
            var objAsignacion = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == idAsignacion && r.tipoEvaluacion == tipoEvaluacion).FirstOrDefault();
            var lst = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.idEvaluacion == objAsignacion.id && r.tipoEvaluacion == tipoEvaluacion).ToList();
            int contador = 0;
            var registros = lst.Count();
            foreach (var item in lst)
            {
                contador++;
                if (item.evaluacionPendiente == false && contador <= registros)
                {
                    return ns = false;
                }
                else
                {
                    ns = true;
                }
            }
            return ns;
        }
        public Dictionary<string, object> obtenerPromedioGeneral(int id)
        {
            resultado = new Dictionary<string, object>();
            resultado.Add(ITEMS, Math.Round(obtenerPromedio(id), 2));
            resultado.Add(SUCCESS, true);
            return resultado;
        }

        public Dictionary<string, object> cambiarDeColor(int idPlantilla, int idAsignacion)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                var lst = db.tblCO_ADP_EvaluacionDiv.Where(r => r.idPlantilla == idPlantilla).ToList().Select(y => new
                {
                    idbutton = y.idbutton,
                    classe = retornarValor(idAsignacion, y.id) == false ? "p-primary" : "p-success"
                }).ToList();

                resultado.Add(ITEMS, lst);
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "CONSULTA REALIZADA CON EXITO");
            }
            catch (Exception ex)
            {
                resultado.Add(ITEMS, "");
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, ex.Message.ToString());
                throw;
            }
            return resultado;
        }


        public List<DivReqDTO> obtenerLst(int idPlantilla, int idAsignacion)
        {
            List<DivReqDTO> lstResultado = new List<DivReqDTO>();
            DivReqDTO objDivReq = new DivReqDTO();
            try
            {
                var lstDiviciones = obtenerDivicionesEvaluadorArchivos(idPlantilla, idAsignacion);
                foreach (var item in lstDiviciones)
                {
                    var total = obtenerCalificacionPorDiv(idAsignacion, item.id);
                    var numero = Math.Round(total, 2);
                    objDivReq = new DivReqDTO();
                    objDivReq.DivicionesORequerimiento = item.descripcion;
                    objDivReq.TituloP = item.toltips;
                    objDivReq.Titulo = "";
                    if (total <= 25) { objDivReq.Pesimo = numero == 0 ? "" : numero.ToString(); } else { objDivReq.Pesimo = ""; }
                    if (total >= 26 && total <= 50) { objDivReq.Malo = numero == 0 ? "" : numero.ToString(); } else { objDivReq.Malo = ""; }
                    if (total >= 51 && total <= 70) { objDivReq.Regular = numero == 0 ? "" : numero.ToString(); } else { objDivReq.Regular = ""; }
                    if (total >= 71 && total <= 90) { objDivReq.Aceptable = numero == 0 ? "" : numero.ToString(); } else { objDivReq.Aceptable = ""; }
                    if (total >= 91 && total <= 100) { objDivReq.Excdediendo = numero == 0 ? "" : numero.ToString(); } else { objDivReq.Excdediendo = ""; }
                    objDivReq.Calificacion = numero == 0 ? "" : numero.ToString();
                    lstResultado.Add(objDivReq);

                    var lstReq = db.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == item.id).ToList();
                    foreach (var item2 in lstReq)
                    {
                        objDivReq = new DivReqDTO();
                        objDivReq.DivicionesORequerimiento = item.descripcion;
                        objDivReq.TituloP = item.toltips;
                        objDivReq.Titulo = item2.texto;
                        objDivReq.Pesimo = "";
                        objDivReq.Malo = "";
                        objDivReq.Regular = "";
                        objDivReq.Aceptable = "";
                        objDivReq.Excdediendo = "";
                        objDivReq.Calificacion = "";
                        lstResultado.Add(objDivReq);
                    }

                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return lstResultado;
        }
        public double obtenerCalificacionPorDiv(int idAsignacion, int idTipoDiv)
        {
            double total = 0;
            try
            {
                var lstDetalle = db.tblCO_ADP_EvalSubContratistaDet.Where(x => x.tipoEvaluacion == idTipoDiv && x.idEvaluacion == idAsignacion).ToList();
                foreach (var item in lstDetalle)
                {
                    total += item.calificacion;
                }
                if (total == 0)
                {
                    total = 0;
                }
                else
                {
                    total = (total / lstDetalle.Count());
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return total;
        }
        public List<tblX_FirmaEvaluacion> obtenerFirmas(int idAsignacion)
        {
            List<tblX_FirmaEvaluacion> lst = new List<tblX_FirmaEvaluacion>();


            return lst;
        }
        public List<excelDTO> obtenerListado2(int idAsignacion)
        {
            List<excelDTO> obtenerListado = new List<excelDTO>();

            var lstSubContratista = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == idAsignacion).ToList();
            List<tblCO_ADP_EvalSubContratistaDet> lstSubContratistaDet = new List<tblCO_ADP_EvalSubContratistaDet>();
            foreach (var item in lstSubContratista)
            {
                var lst = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.idEvaluacion == item.id).ToList();
                lstSubContratistaDet.AddRange(lst);
            }

            obtenerListado = db.tblCO_ADP_EvaluacionDiv.Where(r => r.SubContratista == false).ToList().Select(y => new excelDTO
            {
                id = y.id,
                Divicion = y.descripcion,
                Desviaciones = lstSubContratistaDet.Where(r => r.tipoEvaluacion == y.id && r.opcional == false && (r.comentario != null || r.fechaCompromiso != null || r.responsable != null || r.planesDeAccion != null)).ToList().Select(n => new resultDTO { texto = n.comentario }).ToList(),
                PlanesDeAccion = lstSubContratistaDet.Where(r => r.tipoEvaluacion == y.id && r.opcional == false && (r.comentario != null || r.fechaCompromiso != null || r.responsable != null || r.planesDeAccion != null)).ToList().Select(n => new resultDTO { texto = n.planesDeAccion }).ToList(),
                Responsable = lstSubContratistaDet.Where(r => r.tipoEvaluacion == y.id && r.opcional == false && (r.comentario != null || r.fechaCompromiso != null || r.responsable != null || r.planesDeAccion != null)).ToList().Select(n => new resultDTO { texto = n.responsable }).ToList(),
                FechaCompromiso = lstSubContratistaDet.Where(r => r.tipoEvaluacion == y.id && r.opcional == false && (r.comentario != null || r.fechaCompromiso != null || r.responsable != null || r.planesDeAccion != null)).ToList().Select(n => new resultDTO { texto = n.fechaCompromiso }).ToList(),
                numeroMayor = lstSubContratistaDet.Where(r => r.tipoEvaluacion == y.id && r.opcional == false && (r.comentario != null || r.fechaCompromiso != null || r.responsable != null || r.planesDeAccion != null)).ToList().Count(),
                Calificacion = lstSubContratistaDet.Where(r => r.tipoEvaluacion == y.id && r.opcional == false && (r.comentario != null || r.fechaCompromiso != null || r.responsable != null || r.planesDeAccion != null)).ToList().Select(n => new resultDTO { calificacion = n.calificacion }).ToList().Sum(r => r.calificacion) == 0 ? 1 : lstSubContratistaDet.Where(r => r.tipoEvaluacion == y.id && r.opcional == false).ToList().Select(n => new resultDTO { calificacion = n.calificacion }).ToList().Sum(r => r.calificacion) / lstSubContratistaDet.Where(r => r.tipoEvaluacion == y.id && r.opcional == false).ToList().Count(),
            }).ToList();

            return obtenerListado;
        }
        public tblCO_ADP_EvalSubConAsignacion traermeDatosPrincipales(int idAsignacion)
        {
            try
            {
                var obASignacion = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.id == idAsignacion).FirstOrDefault();
                return obASignacion;
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }
        }
        public List<tblCO_ADP_EvalSubConAsignacion> obtenerTodasLasASinaciones(int idAsignacion)
        {
            List<tblCO_ADP_EvalSubConAsignacion> lst = new List<tblCO_ADP_EvalSubConAsignacion>();
            try
            {
                var objAsignacion = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.id == idAsignacion).FirstOrDefault();
                lst = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.idContrato == objAsignacion.idContrato).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return lst;
        }



    }
}
