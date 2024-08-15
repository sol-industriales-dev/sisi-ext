using Core.DAO.Administracion.Facultamiento;
using Core.DTO;
using Core.DTO.Administracion.Facultamiento;
using Core.Entity.Administrativo.Facultamiento;
using Core.Entity.Principal.Alertas;
using Core.Entity.Principal.Usuarios;
using Core.Entity.RecursosHumanos.Catalogo;
using Core.Enum.Administracion.Facultamiento;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Principal.Usuarios;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using Core.Entity.FileManager;
using Core.DTO.Utils.Data;
using Core.DTO.Principal.Generales;

namespace Data.DAO.Administracion.Facultamiento
{
    public class FacultamientoDAO : GenericDAO<tblFa_CatFacultamiento>, IFacultamientoDAO
    {
        CentroCostosFactoryServices ccFS = new CentroCostosFactoryServices();
        UsuarioFactoryServices uffs = new UsuarioFactoryServices();
        #region Guardar
        public tblFa_CatAuth saveFacultamiento(tblFa_CatFacultamiento obj, List<tblFa_CatAutorizacion> lstAut, List<tblFa_CatMonto> lstMonto, List<tblFa_CatAuth> lstAuth, List<tblFa_CatPuesto> lstPuesto, int idUsuario)
        {
            try
            {
                obj.fechaRegistro = DateTime.Now;
                obj.usuarioID = vSesiones.sesionUsuarioDTO.id;
                obj.estatus = 1;
                SaveEntity(obj, (int)BitacoraEnum.Facultamiento);
                var _cMonto = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblFa_CatMonto>();
                var _cAut = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblFa_CatAutorizacion>();
                var _dbMonto = _cMonto.Where(w => obj.id == w.idFacultamiento);
                var _cAlert = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblP_Alerta>();
                var _cAuth = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblFa_CatAuth>();
                var _cPuesto = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblFa_CatPuesto>();

                lstMonto.ForEach(m =>
                {
                    var idMonto = m.id;
                    m.idFacultamiento = obj.id;
                    _cMonto.AddObject(m);
                    SaveChanges();
                    var _dbAut = _cAut.Where(w => w.idMonto == m.id);
                    lstAut.Where(w => w.renglon == m.renglon && w.idMonto == idMonto).ToList().ForEach(a =>
                    {
                        a.idMonto = m.id;
                        a.descPuesto = getPuestoFromNombreCompleto(a.nombre);
                        _cAut.AddObject(a);
                        SaveChanges();
                    });
                });
                lstPuesto.ForEach(p =>
                {
                    p.idFacultamiento = obj.id;
                    p.puesto = p.puesto ?? string.Empty;
                    _cPuesto.AddObject(p);
                });
                lstAuth.ForEach(a =>
                {
                    a.idFacultamiento = obj.id;
                    a.fechaFirma = new DateTime(1753, 1, 1);
                    _cAuth.AddObject(a);
                    SaveChanges();
                });
                var vobo = lstAuth.FirstOrDefault(w => w.orden.Equals(1));
                if (vobo == null || vobo.idUsuario == 0)
                    vobo = lstAuth.FirstOrDefault();
                _cAlert.AddObject(new tblP_Alerta()
                {
                    objID = obj.id,
                    userEnviaID = idUsuario,
                    userRecibeID = vobo.idUsuario,
                    sistemaID = (int)SistemasEnum.CONTROL_INTERNO,
                    tipoAlerta = 2,
                    url = "/Administrativo/Facultamiento/Autorizacion",
                    msj = string.Format("Facultamiento {0} Vobo 1", obj.cc)
                });
                SaveChanges();
                return vobo;
            }
            catch (Exception)
            {
                return new tblFa_CatAuth();
            }
        }
        public void deleteNoCompleto(string cc)
        {
            var _cCuadro = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblFa_CatFacultamiento>();
            var obj = _cCuadro.ToList().LastOrDefault(w => w.cc.Equals(cc));
            var _cMonto = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblFa_CatMonto>();
            var _cAut = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblFa_CatAutorizacion>();
            var _cAuth = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblFa_CatAuth>();
            var _cAlert = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblP_Alerta>();
            var _cPuesto = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblFa_CatPuesto>();
            var lstMonto = _cMonto.Where(w => obj.id == w.idFacultamiento).ToList();
            lstMonto.ForEach(m =>
            {
                var lstAut = _cAut.Where(e => e.idMonto.Equals(m.id)).ToList();
                lstAut.ForEach(e =>
                {
                    _cAut.Attach(e);
                    _cAut.DeleteObject(e);
                    SaveChanges();
                });
                _cMonto.Attach(m);
                _cMonto.DeleteObject(m);
                SaveChanges();
            });
            var lstPuesto = _cPuesto.Where(p => p.idFacultamiento.Equals(obj.id)).ToList();
            lstPuesto.ForEach(p =>
            {
                _cPuesto.Attach(p);
                _cPuesto.DeleteObject(p);
                SaveChanges();
            });
            var lstAuth = _cAuth.Where(a => a.idFacultamiento.Equals(obj.id)).ToList();
            lstAuth.ForEach(a =>
            {
                _cAuth.Attach(a);
                _cAuth.DeleteObject(a);
                SaveChanges();
            });
            var lstAlert = _cAlert.Where(a => a.url.Contains("Facultamiento") && a.objID.Equals(obj.id)).ToList();
            lstAlert.ForEach(a =>
            {
                _cAlert.Attach(a);
                _cAlert.DeleteObject(a);
                SaveChanges();
            });
            _cCuadro.Attach(obj);
            _cCuadro.DeleteObject(obj);
            SaveChanges();
        }
        public tblFa_CatAuth setAutorizacion(int id, int idUsuario)
        {
            try
            {
                var _cAuth = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblFa_CatAuth>();
                var _dbAuth = _cAuth.FirstOrDefault(w => w.id == id);
                _dbAuth.auth = true;
                _dbAuth.fechaFirma = DateTime.Now;
                _dbAuth.firma = string.Format("--{0}|{1}|{2}|{3}|{4}--", id, idUsuario, DateTime.Now.ToString("ddMMyyyy|HHmm"), (int)DocumentosEnum.Facultamiento_Orden_Compras, (int)StAutorizacionEnum.Autorizo);
                SaveChanges();
                var _cAlert = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblP_Alerta>();
                var _dbalerta = _cAlert.FirstOrDefault(w => w.userRecibeID.Equals(_dbAuth.idUsuario) && w.objID.Equals(_dbAuth.idFacultamiento) && w.url.Contains("Facultamiento"));
                if (_dbalerta != null)
                {
                    _dbalerta.visto = true;
                    SaveChanges();
                }
                var lstAuth = _cAuth.Where(w => w.idFacultamiento.Equals(_dbAuth.idFacultamiento) && !string.IsNullOrEmpty(w.nombre)).ToList();
                var vobo = lstAuth.FirstOrDefault(w => !w.orden.Equals(0) && !w.auth);
                if (vobo == null)
                    vobo = lstAuth.FirstOrDefault(w => w.orden.Equals(0) && !w.auth);
                if (vobo != null)
                    _cAlert.AddObject(new tblP_Alerta()
                    {
                        objID = _dbAuth.idFacultamiento,
                        userEnviaID = idUsuario,
                        userRecibeID = vobo.idUsuario,
                        sistemaID = (int)SistemasEnum.CONTROL_INTERNO,
                        tipoAlerta = 2,
                        url = "/Administrativo/Facultamiento/Autorizacion",
                        msj = string.Format("Facultamiento {0} {1}", getCuadro(_dbAuth.idFacultamiento).cc, vobo.Equals(0) ? "Autoriza" : "Vobo " + vobo.orden)
                    });
                SaveChanges();
                if (vobo == null)
                {
                    var auth = lstAuth.FirstOrDefault(a => a.orden.Equals(lstAuth.Min(m => m.orden)));
                    var _cFacul = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblFa_CatFacultamiento>();
                    var _dbCuadro = _cFacul.FirstOrDefault(f => f.id.Equals(auth.idFacultamiento));
                    _dbCuadro.estatus = 2;
                    SaveChanges();
                    var lst = new List<int>() { 11, 12 };
                    lst.ForEach(u =>
                    {
                        sendCorreo(u, auth.idUsuario, null, _dbCuadro.cc, auth.orden);
                    });
                    insertEnvioGestor(_dbCuadro.id);
                }
                return vobo ?? new tblFa_CatAuth();
            }
            catch (Exception)
            {
                return new tblFa_CatAuth();
            }
        }
        public tblFa_CatAuth setRechazo(int id, string comentario)
        {
            try
            {
                var _cFacul = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblFa_CatFacultamiento>();
                var _cAuth = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblFa_CatAuth>();
                var _cAlert = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblP_Alerta>();
                var _dbAuth = _cAuth.FirstOrDefault(w => w.id == id);
                _dbAuth.auth = false;
                _dbAuth.esRechazado = true;
                _dbAuth.motivoRechazo = comentario ?? string.Empty;
                _dbAuth.fechaFirma = DateTime.Now;
                _dbAuth.firma = string.Format("--{0}|{1}|{2}|{3}|{4}--", id, vSesiones.sesionUsuarioDTO.id, DateTime.Now.ToString("ddMMyyyy|HHmm"), (int)DocumentosEnum.Facultamiento_Orden_Compras, (int)StAutorizacionEnum.Rechazo);
                SaveChanges();
                var _dbCuadro = _cFacul.FirstOrDefault(f => f.id.Equals(_dbAuth.idFacultamiento));
                _dbCuadro.estatus = 3;
                SaveChanges();
                var lst = new List<int>() { 11, 12 };
                lst.ForEach(u =>
                {
                    sendCorreoRechazo(u, _dbAuth.idUsuario, _dbCuadro.cc, _dbAuth.orden, _dbAuth.motivoRechazo);
                });
                var lstAlert = _cAlert.Where(a => a.url.Contains("Facultamiento") && a.objID.Equals(_dbCuadro.id)).ToList();
                lstAlert.ForEach(a =>
                {
                    _cAlert.Attach(a);
                    _cAlert.DeleteObject(a);
                    SaveChanges();
                });
                SaveChanges();
                return _dbAuth ?? new tblFa_CatAuth();
            }
            catch (Exception)
            {
                return new tblFa_CatAuth();
            }
        }
        #endregion
        #region Eliminar
        public bool remMonto(int id)
        {
            try
            {
                var _ctx = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblFa_CatMonto>();
                var d = _ctx.FirstOrDefault(w => w.id == id);
                _ctx.DeleteObject(d);
                SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool remAuto(int id)
        {
            try
            {
                var _ctx = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblFa_CatAutorizacion>();
                var d = _ctx.FirstOrDefault(w => w.id == id);
                _ctx.DeleteObject(d);
                SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        #region Correo
        public bool sendCorreo(int iduserRecibe, int iduserEnvia, List<Byte[]> pdf, string cc, int vobo)
        {
            try
            {
                var usuarioEnvia = _context.tblP_Usuario.FirstOrDefault(w => w.id.Equals(iduserEnvia));
                var usuarioRecibe = _context.tblP_Usuario.FirstOrDefault(w => w.id.Equals(iduserRecibe));
                var asunto = string.Format("{0}-{1}", vSesiones.sesionEmpresaActual.Equals(2) ? "AC" : "CC", cc);
                List<string> CorreoEnviar = new List<string>() { usuarioRecibe.correo };
                if (vSesiones.sesionEmpresaActual == 1)
                {
                    CorreoEnviar.Add("valeria.gomez@construplan.com.mx");
                }
                string AsuntoCorreo = @"<html>
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
                                                        background-color: #dddddd;
                                                    }
                                                </style>
                                            </head>
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>
                                                <div class=WordSection1>
                                                    <p class=MsoNormal>
                                                        Buen día <o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>";
                AsuntoCorreo += @" <p class=MsoNormal>Se informa que se registro " + (vobo.Equals(1) ? "un nuevo cuadro" : "una autorización") + " de facultamiento por el empleado " + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + ".<o:p></o:p></p>";
                AsuntoCorreo += @"</tbody></table>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        Favor de ingresar al sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx/</a>), en el apartado de ADMINISTRACION, menú Facultamiento en la opción Autorización.<o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>" + string.Format("{0}: {1}", !vSesiones.sesionEmpresaActual.Equals(2) ? "Centro Costos" : "Area Cuenta", cc) + @"</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        PD. Se informa que esta es una notificación autogenerada por el sistema SIGOPLAN no es necesario dar una respuesta <o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        Gracias.<o:p></o:p>
                                                    </p>
                                                </div>
                                            </body>
                                        </html>";
                var tipoFormato = "Facultamiento.pdf";
                GlobalUtils.sendEmailAdjuntoInMemory2(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), "Alerta de Facultamiento " + asunto), AsuntoCorreo, CorreoEnviar.Distinct().ToList(), pdf, tipoFormato);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool sendCorreoRechazo(int iduserRecibe, int iduserEnvia, string cc, int vobo, string comentarios)
        {
            try
            {
                comentarios = WebUtility.HtmlEncode(comentarios.Trim());

                var usuarioEnvia = _context.tblP_Usuario.FirstOrDefault(w => w.id.Equals(iduserEnvia));
                var usuarioRecibe = _context.tblP_Usuario.FirstOrDefault(w => w.id.Equals(iduserRecibe));
                var asunto = string.Format("{0}-{1}", vSesiones.sesionEmpresaActual.Equals(2) ? "AC" : "CC", cc);
                List<string> CorreoEnviar = new List<string>() { usuarioRecibe.correo };
                string AsuntoCorreo = @"<html>
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
                                                        background-color: #dddddd;
                                                    }
                                                </style>
                                            </head>
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>
                                                <div class=WordSection1>
                                                    <p class=MsoNormal>
                                                        Buen día <o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>";
                AsuntoCorreo += @" <p class=MsoNormal>Se informa que se rechazó un cuadro de facultamiento por el empleado " + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + ". Por motivos de " + comentarios ?? string.Empty + "<o:p></o:p></p>";
                AsuntoCorreo += @"</tbody></table>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>" + string.Format("{0}: {1}", !vSesiones.sesionEmpresaActual.Equals(2) ? "Centro Costos" : "Area Cuenta", cc) + @"</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>El cuadro de facultamiento se reanudará hasta que se genere de nuevo.</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        PD. Se informa que esta es una notificación autogenerada por el sistema SIGOPLAN no es necesario dar una respuesta <o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        Gracias.<o:p></o:p>
                                                    </p>
                                                </div>
                                            </body>
                                        </html>";
                GlobalUtils.sendEmailAdjuntoInMemory2(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), "Alerta de Facultamiento " + asunto), AsuntoCorreo, CorreoEnviar.Distinct().ToList(), null, string.Empty);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        public List<tblFa_CatFacultamiento> getCuadro()
        {
            var lst = _context.tblFa_CatFacultamiento.ToList();
            return lst.GroupBy(g => g.cc).Select(g => g.OrderByDescending(o => o.id).FirstOrDefault()).ToList();
        }
        public List<tblFa_CatFacultamiento> getCuadroNoR()
        {
            var lst = _context.tblFa_CatFacultamiento.Where(x => x.estatus == 1).ToList();
            return lst.GroupBy(g => g.cc).Select(g => g.OrderByDescending(o => o.id).FirstOrDefault()).ToList();
        }
        public List<tblFa_CatFacultamiento> getCCCompleto(string cc)
        {
            return _context.tblFa_CatFacultamiento.Where(w => w.cc.Equals(cc)).ToList();
        }

        public string ObtenerMotivoRechazo(int facultamientoID)
        {
            try
            {
                var autorizacion = _context.tblFa_CatAuth.Where(x => x.idFacultamiento == facultamientoID).FirstOrDefault();
                if (autorizacion != null && autorizacion.motivoRechazo != null)
                {
                    return WebUtility.HtmlEncode(autorizacion.motivoRechazo.Trim().Replace("\n", " "));
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        public List<tblFa_CatFacultamiento> getCCCompleto(List<string> cc, DateTime fechaI, DateTime fechaF)
        {

            return _context.tblFa_CatFacultamiento.Where(w => cc.Contains(w.cc)).ToList().Where(y => (y.fecha.Date >= fechaI.Date && y.fecha.Date <= fechaF.Date)).ToList();
            //  return _context.tblFa_CatFacultamiento.Where(w => cc.Contains(w.cc)).ToList().Where(y => (fechaI.Date >= y.fecha.Date && fechaF <= y.fecha.Date)).ToList();
        }

        public tblFa_CatFacultamiento getCuadro(string cc, DateTime fecha)
        {
            var obra = getNombreCC(cc);
            try
            {
                var lst = _context.tblFa_CatFacultamiento.Where(w => w.cc.Equals(cc)).ToList();
                if (lst.Count == 0)
                    return new tblFa_CatFacultamiento() { id = 0, cc = cc, obra = obra, fecha = DateTime.Now };
                else
                    return lst.LastOrDefault();
            }
            catch (Exception)
            {
                return new tblFa_CatFacultamiento() { id = 0, cc = cc, obra = obra, fecha = DateTime.Now };
            }
        }
        public tblFa_CatFacultamiento getCuadro(int id)
        {
            try
            {
                return _context.tblFa_CatFacultamiento.FirstOrDefault(w => w.id == id);
            }
            catch (Exception)
            {
                return new tblFa_CatFacultamiento() { id = 0, cc = string.Empty, obra = string.Empty, fecha = DateTime.Now };
            }
        }
        public List<tblFa_CatAutorizacion> getAutorizacion(int id, int renglon)
        {
            var lst = _context.tblFa_CatAutorizacion.Where(w => w.idMonto == id).ToList();
            if (lst.Count == 0)
            {
                lst = new List<tblFa_CatAutorizacion>();
                lst.Add(new tblFa_CatAutorizacion() { descPuesto = string.Empty, idTipoAutorizacion = (int)TipoAutorizacionEnum.Autoriza, idMonto = id, nombre = string.Empty, renglon = renglon });
                lst.Add(new tblFa_CatAutorizacion() { descPuesto = string.Empty, idTipoAutorizacion = (int)TipoAutorizacionEnum.VoBo1, idMonto = id, nombre = string.Empty, renglon = renglon });
                lst.Add(new tblFa_CatAutorizacion() { descPuesto = string.Empty, idTipoAutorizacion = (int)TipoAutorizacionEnum.VoBo2, idMonto = id, nombre = string.Empty, renglon = renglon });

            }
            return lst;
        }
        public List<tblFa_CatMonto> getMonto(int id, string cc)
        {
            var lst = _context.tblFa_CatMonto.Where(w => w.idFacultamiento == id).ToList();
            if (lst.Count == 0)
            {
                lst = new List<tblFa_CatMonto>().ToList();
                if (vSesiones.sesionEmpresaActual.Equals((int)EmpresaEnum.Arrendadora) || cc.ParseInt(101) < 100)
                {
                    lst.Add(new tblFa_CatMonto() { id = -3, idTabla = (int)TipoTablaEnum.Administrativo, renglon = 1, min = 1, max = 5000 });
                    lst.Add(new tblFa_CatMonto() { id = -3, idTabla = (int)TipoTablaEnum.Administrativo, renglon = 2, min = 5001, max = 30000 });
                    lst.Add(new tblFa_CatMonto() { id = -3, idTabla = (int)TipoTablaEnum.Administrativo, renglon = 3, min = 30001, max = 250000 });
                    lst.Add(new tblFa_CatMonto() { id = -3, idTabla = (int)TipoTablaEnum.Administrativo, renglon = 4, min = 250001, max = 400000 });
                    lst.Add(new tblFa_CatMonto() { id = -3, idTabla = (int)TipoTablaEnum.Administrativo, renglon = 5, min = 400001, max = 0 });
                }
                else
                {
                    lst.Add(new tblFa_CatMonto() { id = -1, idTabla = (int)TipoTablaEnum.Refacciones, renglon = 1, min = 1, max = 5000 });
                    lst.Add(new tblFa_CatMonto() { id = -1, idTabla = (int)TipoTablaEnum.Refacciones, renglon = 2, min = 5001, max = 30000 });
                    lst.Add(new tblFa_CatMonto() { id = -1, idTabla = (int)TipoTablaEnum.Refacciones, renglon = 3, min = 30001, max = 250000 });
                    lst.Add(new tblFa_CatMonto() { id = -1, idTabla = (int)TipoTablaEnum.Refacciones, renglon = 4, min = 250001, max = 400000 });
                    lst.Add(new tblFa_CatMonto() { id = -1, idTabla = (int)TipoTablaEnum.Refacciones, renglon = 5, min = 400001, max = 0 });
                    lst.Add(new tblFa_CatMonto() { id = -2, idTabla = (int)TipoTablaEnum.Materiales, renglon = 1, min = 1, max = 5000 });
                    lst.Add(new tblFa_CatMonto() { id = -2, idTabla = (int)TipoTablaEnum.Materiales, renglon = 2, min = 5001, max = 30000 });
                    lst.Add(new tblFa_CatMonto() { id = -2, idTabla = (int)TipoTablaEnum.Materiales, renglon = 3, min = 30001, max = 250000 });
                    lst.Add(new tblFa_CatMonto() { id = -2, idTabla = (int)TipoTablaEnum.Materiales, renglon = 4, min = 250001, max = 400000 });
                    lst.Add(new tblFa_CatMonto() { id = -2, idTabla = (int)TipoTablaEnum.Materiales, renglon = 5, min = 400001, max = 0 });
                }
            }
            if (vSesiones.sesionEmpresaActual.Equals((int)EmpresaEnum.Arrendadora))
                lst.Where(w => w.idTabla.Equals((int)TipoTablaEnum.Refacciones)).ToList().ForEach(m =>
                {
                    m.idTabla = (int)TipoTablaEnum.Administrativo;
                });
            return lst;
        }
        public List<tblFa_CatAuth> getLstAuth(int idFacultamiento)
        {
            var lst = _context.tblFa_CatAuth.Where(w => w.idFacultamiento.Equals(idFacultamiento)).Take(4).ToList();
            if (lst.Count == 0)
            {
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                {
                    return new List<tblFa_CatAuth>() { 
                        new tblFa_CatAuth(){ orden = 0, nombre = "ADRIANA DEL CARMEN REINA CELAYA" , idUsuario = 1080},
                        new tblFa_CatAuth(){ orden = 1, nombre = "JOSE MANUEL GAYTAN LIZAMA" , idUsuario = 4},
                        new tblFa_CatAuth(){ orden = 2, nombre = "EDMUNDO FRAIJO VEGA" , idUsuario = 1063},
                        new tblFa_CatAuth(){ orden = 3, nombre = string.Empty },
                        //new tblFa_CatAuth(){ orden = 4, nombre = string.Empty },
                    };
                }
                else
                {
                    return new List<tblFa_CatAuth>() { 
                        new tblFa_CatAuth(){ orden = 0, nombre = "ADRIANA DEL CARMEN REINA CELAYA" , idUsuario = 1080},
                        new tblFa_CatAuth(){ orden = 1, nombre = string.Empty },
                        new tblFa_CatAuth(){ orden = 2, nombre = string.Empty },
                        new tblFa_CatAuth(){ orden = 3, nombre = string.Empty },
                        //new tblFa_CatAuth(){ orden = 4, nombre = string.Empty },
                    };
                }
            }
            else
                return lst;
        }
        public List<tblFa_CatPuesto> GetLstPuesto(int idFacultamiento)
        {
            var lst = _context.tblFa_CatPuesto.Where(w => w.idFacultamiento.Equals(idFacultamiento)).ToList();
            if (lst.Count.Equals(0))
            {
                var lstEmp = Enum.GetValues(typeof(TipoPuestoEnum)).Cast<TipoPuestoEnum>().ToList()
                    .Select(x => new
                    {
                        Puesto = x.GetDescription(),
                        Valor = x.GetHashCode()
                    }).ToList();
                var lstAuth = Enum.GetValues(typeof(TipoAutorizacionEnum)).Cast<TipoAutorizacionEnum>().ToList()
                    .Where(x => !x.GetHashCode().Equals(3))
                    .Select(x => new
                    {
                        Vobo = x.GetDescription(),
                        Valor = x.GetHashCode()
                    }).ToList();
                lstEmp.ForEach(e =>
                {
                    lstAuth.ForEach(a =>
                    {
                        lst.Add(
                            new tblFa_CatPuesto()
                            {
                                idTabla = e.Valor,
                                orden = a.Valor
                            });
                    });
                });
            }
            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
            {
                lst.FirstOrDefault(x => x.idTabla == (int)TipoPuestoEnum.ServAdmin && x.orden == (int)TipoAutorizacionEnum.Autoriza).puesto = "DIRECTOR GENERAL";
                lst.FirstOrDefault(x => x.idTabla == (int)TipoPuestoEnum.ServAdmin && x.orden == (int)TipoAutorizacionEnum.VoBo1).puesto = "DIRECTOR DE ADMINISTRACION Y FINANZAS O DIRECTOR DE CONTABILIDAD";
            }
            return lst;
        }
        public dynamic getLstAutorizacion(int id)
        {
            var lst = getLstAuth(id).Where(w => !string.IsNullOrEmpty(w.nombre)).ToList();
            return lst.Select(x => new
            {
                id = (int)x.id,
                idUsuario = (int)x.idUsuario,
                nombre = (string)x.nombre,
                orden = (int)x.orden,
                auth = x.auth ? "Autorizado" : x.esRechazado ? "Rechazado" : "En espera",
            }).ToList();
        }
        public dynamic getLstAutorizacion(int id, string nombre)
        {
            var lstAuth = getLstAuth(id).Where(w => !string.IsNullOrEmpty(w.nombre)).ToList();
            if (lstAuth.All(a => a.auth))
                return lstAuth.Select(a => new
                {
                    id = a.id,
                    idFacultamiento = a.idFacultamiento,
                    idUsuario = a.idUsuario,
                    nombre = a.nombre,
                    orden = a.orden,
                    auth = "A"
                });
            var orden = lstAuth.Where(w => !w.auth).Max(w => w.orden);
            if (orden > 0)
                orden = lstAuth.Where(w => !w.auth && w.orden > 0).Min(w => w.orden);
            var lst = lstAuth.Select(a => new
            {
                id = a.id,
                idFacultamiento = a.idFacultamiento,
                idUsuario = a.idUsuario,
                nombre = a.nombre,
                orden = a.orden,
                auth = (a.esRechazado ? "R" : a.auth ? "A" : a.idUsuario == vSesiones.sesionUsuarioDTO.id && orden.Equals(a.orden) ? "U" : "E"),
            });
            return lst;
        }
        public bool isUsuarioAutorisable(int id, string nombre)
        {
            var lstAuth = getLstAuth(id).Where(w => !string.IsNullOrEmpty(w.nombre) && !w.auth).ToList();
            if (lstAuth.Count.Equals(0))
                return false;
            var orden = lstAuth.Max(w => w.orden);
            if (orden > 0)
                orden = lstAuth.Where(w => w.orden > 0).Min(w => w.orden);
            var cve = vSesiones.sesionUsuarioDTO.id;
            return lstAuth.FirstOrDefault(w => w.orden.Equals(orden)).idUsuario == cve;
        }
        public CuadroDTO getCuadroFromAutorizado(int idAutorizado)
        {
            var allAutorizados = _context.tblFa_CatAutorizacion.ToList();
            var allMontos = _context.tblFa_CatMonto.ToList();
            var allFacultamientos = _context.tblFa_CatFacultamiento.ToList();
            var allUsuarios = _context.tblP_Usuario.ToList();
            var reffAutorizado = allAutorizados.FirstOrDefault(w => w.id == idAutorizado);
            var reffMontos = allMontos.FirstOrDefault(w => reffAutorizado.idMonto == w.id);
            var Faculamiento = allFacultamientos.FirstOrDefault(w => w.id == reffMontos.idFacultamiento);
            return new CuadroDTO()
            {
                facultamiento = Faculamiento,
                montos = allMontos.Where(w => w.idFacultamiento == Faculamiento.id).ToList().Select(m => new MontoDTO
                {
                    monto = m,
                    autorizaciones = allAutorizados.Where(a => a.idMonto == m.id && !string.IsNullOrEmpty(a.nombre)).Select(a => new AutorizacionesDTO
                    {
                        autorizaciones = a,
                        usuario = allUsuarios.FirstOrDefault(u => u.nombre.ToUpper().Replace(System.Environment.NewLine, string.Empty).Equals(a.nombre.ToUpper().Replace(System.Environment.NewLine, string.Empty)))
                    }).ToList()
                }).ToList()
            };
        }
        public bool getAutorizacion(int id)
        {
            var lstAuth = _context.tblFa_CatAuth.Where(w => w.idFacultamiento == id && !string.IsNullOrEmpty(w.nombre)).ToList();
            var isAuth = lstAuth.All(a => a.auth);
            return isAuth;
        }
        public string geSTtAuth(int id)
        {
            var lstAuth = _context.tblFa_CatAuth.Where(w => w.idFacultamiento == id && !string.IsNullOrEmpty(w.nombre)).ToList();
            var isRechazo = lstAuth.Any(a => a.esRechazado);
            var isAuth = lstAuth.All(a => a.auth);
            var st = isRechazo ? "Rechazado" : isAuth ? "Autorizado" : "En espera";
            return st;
        }
        public string getPuestoFromNombreCompleto(string completo)
        {
            try
            {
                if (string.IsNullOrEmpty(completo))
                    return string.Empty;
//                var consulta = string.Format(@"SELECT descripcion FROM si_puestos p
//                                    INNER JOIN sn_empleados e ON e.puesto = p.puesto AND e.tipo_nomina = p.tipo_nomina
//                                    WHERE REPLACE(e.nombre + e.ape_paterno + e.ape_materno, ' ', '') like REPLACE('{0}%', ' ', '')", completo);
//                var puesto = (List<PuestoDTO>)ContextEnKontrolNomina.Where(consulta).ToObject<List<PuestoDTO>>();

                var puesto = _context.Select<PuestoDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = string.Format(@"SELECT descripcion FROM tblRH_EK_Puestos p
                                    INNER JOIN tblRH_EK_Empleados e ON e.puesto = p.puesto AND e.tipo_nomina = p.FK_TipoNomina
                                    WHERE REPLACE(e.nombre + e.ape_paterno + e.ape_materno, ' ', '') like REPLACE('{0}%', ' ', '')", completo)
                }).ToList();

                if ((EmpresaEnum) vSesiones.sesionEmpresaActual == EmpresaEnum.Construplan) {
                    var puestoGCPLAN = _context.Select<PuestoDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.GCPLAN,
                        consulta = string.Format(@"SELECT descripcion FROM tblRH_EK_Puestos p
                                    INNER JOIN tblRH_EK_Empleados e ON e.puesto = p.puesto AND e.tipo_nomina = p.FK_TipoNomina
                                    WHERE REPLACE(e.nombre + e.ape_paterno + e.ape_materno, ' ', '') like REPLACE('{0}%', ' ', '')", completo)
                    }).ToList();
                    puesto.AddRange(puestoGCPLAN);
                }


                return puesto.First().descripcion ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string getNombreCC(string cc)
        {
            try
            {
                if (cc.Equals("1015"))
                    return "PATIO DE MAQUINARIA";
                if (cc.Equals("1010"))
                    return "TALLER DE MAQUINARIA";
                if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 2 || vSesiones.sesionEmpresaActual == 3 || vSesiones.sesionEmpresaActual == 6)
                {
                    var res = (List<dynamic>)_contextEnkontrol.Where(string.Format("SELECT descripcion FROM cc WHERE cc = '{0}'", cc)).ToObject<List<dynamic>>();
                    return res[0].descripcion;
                }
                else
                {
                    var res = _context.tblP_CC.Where(x => x.cc == cc).ToList();
                    return res[0].descripcion;
                }

            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public List<object> getComboCC()
        {
            var lst = _context.tblFa_CatFacultamiento.ToList();
            switch (vSesiones.sesionEmpresaActual)
            {
                case (int)EmpresaEnum.Construplan:
                    return lst.OrderBy(o => o.cc).GroupBy(g => g.cc).Select(s => new
                    {
                        Text = string.Format("{0} - {1}", s.FirstOrDefault().cc, ccFS.getCentroCostosService().getNombreCCFix(s.FirstOrDefault().cc)),
                        Value = s.FirstOrDefault().cc,
                        Prefijo = ""
                    }).Cast<object>().ToList();
                case (int)EmpresaEnum.Arrendadora:
                    return lst.OrderBy(o => o.cc).GroupBy(g => g.cc).Select(s => new
                    {
                        Text = string.Format("{0} - {1}", s.FirstOrDefault().cc, s.FirstOrDefault().cc.ParseInt() < 101 ? ccFS.getCentroCostosService().getNombreCCFix(s.FirstOrDefault().cc) : s.FirstOrDefault().obra),
                        Value = s.FirstOrDefault().cc,
                        Prefijo = ""
                    }).Cast<object>().ToList();
                default:
                    return lst.OrderBy(o => o.cc).GroupBy(g => g.cc).Select(s => new
                    {
                        Text = string.Format("{0} - {1}", s.FirstOrDefault().cc, ccFS.getCentroCostosService().getNombreCCFix(s.FirstOrDefault().cc)),
                        Value = s.FirstOrDefault().cc,
                        Prefijo = ""
                    }).Cast<object>().ToList();
            }

        }
        public dynamic getComboCCEnkontrol()
        {
            switch ( (EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                case EmpresaEnum.Peru:
                    return _context.tblP_CC.Select(x => new
                    {
                        Text = x.cc + " " + x.descripcion.Trim(),
                        Value = x.cc,
                        Prefijo = ""
                    }).ToList();
                    break;
                case EmpresaEnum.Colombia:
                    return _context.tblP_CC.Select(x => new
                    {
                        Text = x.cc + " " + x.descripcion.Trim(),
                        Value = x.cc,
                        Prefijo = ""
                    }).ToList();
                    break;
                default: var lst = _contextEnkontrol.Select<dynamic>(EnkontrolAmbienteEnum.Prod, string.Format("SELECT cc, descripcion FROM {0}cc WHERE st_ppto <> 'B' ORDER BY cc", vSesiones.sesionEmpresaDBPregijo));
                    return lst.Select(s => new
                    {
                        Text = string.Format("{0}-{1}", s.cc, s.descripcion),
                        Value = (string)s.cc,
                        Prefijo = ""
                    }).ToList();
                    break;
            }

            //var lst = _contextEnkontrol.Select<dynamic>(EnkontrolAmbienteEnum.Prod, string.Format("SELECT cc, descripcion FROM {0}cc WHERE st_ppto <> 'B' ORDER BY cc", vSesiones.sesionEmpresaDBPregijo));
            //return lst.Select(s => new
            //{
            //    Text = string.Format("{0}-{1}", s.cc, s.descripcion),
            //    Value = (string)s.cc,
            //    Prefijo = ""
            //}).ToList();
        }

        public dynamic getEmpleadosSigoplan(string term)
        {
           
                return _context.tblP_Usuario.Where(u => (u.nombre + u.apellidoPaterno + u.apellidoMaterno).ToUpper().Contains(term.ToUpper())).Select(u => new
                {
                    id = u.id,
                    label = u.nombre + " " + u.apellidoPaterno + " " + u.apellidoMaterno
                }).ToList();
        
        }
        public dynamic getEmpleadosSigoplanNOAG(string term)
        {
            return _context.tblP_Usuario.Where(u => (u.id!=1080 && u.id!=1164) && (u.nombre + u.apellidoPaterno + u.apellidoMaterno).ToUpper().Contains(term.ToUpper())).Select(u => new
            {
                id = u.id,
                label = u.nombre + " " + u.apellidoPaterno + " " + u.apellidoMaterno
            }).ToList();
        }
        public List<string> geDesctPuesto(string term)
        {
            try
            {
                //var lst = (List<dynamic>)ContextEnKontrolNomina.Where(string.Format("SELECT TOP 10 descripcion FROM si_puestos WHERE NOT descripcion like '(NOUS%' AND NOT descripcion like '(NO US%' AND descripcion like '%{0}%'", term)).ToObject<List<dynamic>>();
                var lst = _context.tblRH_EK_Puestos.Where(x => !x.descripcion.Contains("NOUS") && !x.descripcion.Contains("NO US") && x.descripcion.Contains(term)).Take(10).ToList();
                return lst.Select(s => (string)s.descripcion).ToList();
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }
        void setCveEmpleado()
        {
            //var getCatEmpleado = string.Format(@"SELECT clave_empleado, (nombre+ape_paterno+ape_materno) as Nombre FROM sn_empleados");
            //var lst = (List<tblRH_CatEmpleados>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 1).ToObject<List<tblRH_CatEmpleados>>();
            //lst.AddRange((List<tblRH_CatEmpleados>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 2).ToObject<List<tblRH_CatEmpleados>>());

            var lst = _context.Select<tblRH_CatEmpleados>(new DapperDTO
            {
                baseDatos = MainContextEnum.Construplan,
                consulta = string.Format(@"SELECT clave_empleado, (nombre+ape_paterno+ape_materno) as Nombre FROM tblRH_EK_Empleados")
            }).ToList();

            lst.AddRange(_context.Select<tblRH_CatEmpleados>(new DapperDTO
            {
                baseDatos = MainContextEnum.GCPLAN,
                consulta = string.Format(@"SELECT clave_empleado, (nombre+ape_paterno+ape_materno) as Nombre FROM tblRH_EK_Empleados")
            }).ToList());

            lst.AddRange(_context.Select<tblRH_CatEmpleados>(new DapperDTO
            {
                baseDatos = MainContextEnum.Arrendadora,
                consulta = string.Format(@"SELECT clave_empleado, (nombre+ape_paterno+ape_materno) as Nombre FROM tblRH_EK_Empleados")
            }).ToList());
            
            var _cAut = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblFa_CatAutorizacion>();
            _cAut.Where(w => !string.IsNullOrEmpty(w.nombre) && w.cve.Equals(0)).ToList().ForEach(a =>
            {
                var nombre = a.nombre.ToUpper().Replace(" ", string.Empty);
                var ekEmp = lst.FirstOrDefault(w => w.Nombre.ToUpper().Replace(" ", string.Empty).Equals(nombre));
                var cve = ekEmp is tblRH_CatEmpleados ? ekEmp.clave_empleado : 0;
                a.cve = cve;
                SaveChanges();
            });
            var lstUsuario = _context.tblP_Usuario.ToList();
            var _cAuth = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblFa_CatAuth>();
            _cAuth.Where(w => !string.IsNullOrEmpty(w.nombre) && w.idUsuario.Equals(0)).ToList().ForEach(a =>
            {
                var nombre = a.nombre.ToUpper().Replace(" ", string.Empty);
                var user = lstUsuario.FirstOrDefault(u => (u.nombre + u.apellidoPaterno + u.apellidoMaterno).ToUpper().Replace(" ", string.Empty).Contains(nombre));
                a.idUsuario = user is tblP_Usuario ? user.id : 0;
                SaveChanges();
            });
        }
        private bool insertEnvioGestor(int cuadroID)
        {
            try
            {
                var empresa = vSesiones.sesionEmpresaActual;
                var cuadro = _context.tblFa_CatFacultamiento.FirstOrDefault(x => x.id == cuadroID);
                var CC = _context.tblP_CC.FirstOrDefault(x => x.cc == cuadro.cc);
                using (var ctx = new MainContext((int)EmpresaEnum.Construplan))
                {
                    tblFM_EnvioDocumento documento = new tblFM_EnvioDocumento();
                    documento.id = 0;
                    documento.documentoID = cuadroID;
                    documento.descripcion = empresa == 2 ? cuadro.cc : cuadro.cc + "-" + CC.descripcion;
                    documento.tipoDocumento = 19;
                    documento.usuarioID = cuadro.usuarioID;
                    documento.estatus = 0;
                    documento.empresa = empresa;
                    documento.fecha = DateTime.Today;
                    ctx.tblFM_EnvioDocumento.Add(documento);
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public tblFa_CatFacultamiento getCuadro(int id, int empresa = 0)
        {
            if (empresa == 0) { empresa = vSesiones.sesionEmpresaActual; }
            try
            {
                using (var ctx = new MainContext(empresa)) {
                    return ctx.tblFa_CatFacultamiento.FirstOrDefault(w => w.id == id);
                }                
            }
            catch (Exception)
            {
                return new tblFa_CatFacultamiento() { id = 0, cc = string.Empty, obra = string.Empty, fecha = DateTime.Now };
            }
        }

        public List<tblFa_CatMonto> getMonto(int id, string cc, int empresa = 0)
        {
            if (empresa == 0) { empresa = vSesiones.sesionEmpresaActual; }
            using (var ctx = new MainContext(empresa))
            {
                var lst = ctx.tblFa_CatMonto.Where(w => w.idFacultamiento == id).ToList();
                if (lst.Count == 0)
                {
                    lst = new List<tblFa_CatMonto>().ToList();
                    if (vSesiones.sesionEmpresaActual.Equals((int)EmpresaEnum.Arrendadora) || cc.ParseInt(101) < 100)
                    {
                        lst.Add(new tblFa_CatMonto() { id = -3, idTabla = (int)TipoTablaEnum.Administrativo, renglon = 1, min = 1, max = 5000 });
                        lst.Add(new tblFa_CatMonto() { id = -3, idTabla = (int)TipoTablaEnum.Administrativo, renglon = 2, min = 5001, max = 30000 });
                        lst.Add(new tblFa_CatMonto() { id = -3, idTabla = (int)TipoTablaEnum.Administrativo, renglon = 3, min = 30001, max = 250000 });
                        lst.Add(new tblFa_CatMonto() { id = -3, idTabla = (int)TipoTablaEnum.Administrativo, renglon = 4, min = 250001, max = 400000 });
                        lst.Add(new tblFa_CatMonto() { id = -3, idTabla = (int)TipoTablaEnum.Administrativo, renglon = 5, min = 400001, max = 0 });
                    }
                    else
                    {
                        lst.Add(new tblFa_CatMonto() { id = -1, idTabla = (int)TipoTablaEnum.Refacciones, renglon = 1, min = 1, max = 5000 });
                        lst.Add(new tblFa_CatMonto() { id = -1, idTabla = (int)TipoTablaEnum.Refacciones, renglon = 2, min = 5001, max = 30000 });
                        lst.Add(new tblFa_CatMonto() { id = -1, idTabla = (int)TipoTablaEnum.Refacciones, renglon = 3, min = 30001, max = 250000 });
                        lst.Add(new tblFa_CatMonto() { id = -1, idTabla = (int)TipoTablaEnum.Refacciones, renglon = 4, min = 250001, max = 400000 });
                        lst.Add(new tblFa_CatMonto() { id = -1, idTabla = (int)TipoTablaEnum.Refacciones, renglon = 5, min = 400001, max = 0 });
                        lst.Add(new tblFa_CatMonto() { id = -2, idTabla = (int)TipoTablaEnum.Materiales, renglon = 1, min = 1, max = 5000 });
                        lst.Add(new tblFa_CatMonto() { id = -2, idTabla = (int)TipoTablaEnum.Materiales, renglon = 2, min = 5001, max = 30000 });
                        lst.Add(new tblFa_CatMonto() { id = -2, idTabla = (int)TipoTablaEnum.Materiales, renglon = 3, min = 30001, max = 250000 });
                        lst.Add(new tblFa_CatMonto() { id = -2, idTabla = (int)TipoTablaEnum.Materiales, renglon = 4, min = 250001, max = 400000 });
                        lst.Add(new tblFa_CatMonto() { id = -2, idTabla = (int)TipoTablaEnum.Materiales, renglon = 5, min = 400001, max = 0 });
                    }
                }
                if (vSesiones.sesionEmpresaActual.Equals((int)EmpresaEnum.Arrendadora))
                    lst.Where(w => w.idTabla.Equals((int)TipoTablaEnum.Refacciones)).ToList().ForEach(m =>
                    {
                        m.idTabla = (int)TipoTablaEnum.Administrativo;
                    });
                return lst;
            }
        }

        public List<tblFa_CatPuesto> GetLstPuesto(int idFacultamiento, int empresa = 0)
        {
            if (empresa == 0) { empresa = vSesiones.sesionEmpresaActual; }
            using (var ctx = new MainContext(empresa))
            {
                var lst = ctx.tblFa_CatPuesto.Where(w => w.idFacultamiento.Equals(idFacultamiento)).ToList();
                if (lst.Count.Equals(0))
                {
                    var lstEmp = Enum.GetValues(typeof(TipoPuestoEnum)).Cast<TipoPuestoEnum>().ToList()
                        .Select(x => new
                        {
                            Puesto = x.GetDescription(),
                            Valor = x.GetHashCode()
                        }).ToList();
                    var lstAuth = Enum.GetValues(typeof(TipoAutorizacionEnum)).Cast<TipoAutorizacionEnum>().ToList()
                        .Where(x => !x.GetHashCode().Equals(3))
                        .Select(x => new
                        {
                            Vobo = x.GetDescription(),
                            Valor = x.GetHashCode()
                        }).ToList();
                    lstEmp.ForEach(e =>
                    {
                        lstAuth.ForEach(a =>
                        {
                            lst.Add(
                                new tblFa_CatPuesto()
                                {
                                    idTabla = e.Valor,
                                    orden = a.Valor
                                });
                        });
                    });
                }
                return lst;
            }
        }

        public List<tblFa_CatAutorizacion> getAutorizacion(int id, int renglon, int empresa = 0)
        {
            if (empresa == 0) { empresa = vSesiones.sesionEmpresaActual; }
            using (var ctx = new MainContext(empresa))
            {
                var lst = ctx.tblFa_CatAutorizacion.Where(w => w.idMonto == id).ToList();
                if (lst.Count == 0)
                {
                    lst = new List<tblFa_CatAutorizacion>();
                    lst.Add(new tblFa_CatAutorizacion() { descPuesto = string.Empty, idTipoAutorizacion = (int)TipoAutorizacionEnum.Autoriza, idMonto = id, nombre = string.Empty, renglon = renglon });
                    lst.Add(new tblFa_CatAutorizacion() { descPuesto = string.Empty, idTipoAutorizacion = (int)TipoAutorizacionEnum.VoBo1, idMonto = id, nombre = string.Empty, renglon = renglon });
                    lst.Add(new tblFa_CatAutorizacion() { descPuesto = string.Empty, idTipoAutorizacion = (int)TipoAutorizacionEnum.VoBo2, idMonto = id, nombre = string.Empty, renglon = renglon });

                }
                return lst;
            }
        }

    }
}
