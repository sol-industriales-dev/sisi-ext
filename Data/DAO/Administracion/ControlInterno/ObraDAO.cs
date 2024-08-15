using Core.DAO.Administracion.ControlInterno;
using Core.DTO;
using Core.DTO.Administracion.ControlInterno.Obra;
using Core.Entity.Administrativo.ControlInterno.Obra;
using Core.Entity.Principal.Alertas;
using Core.Enum.Administracion.ControlInterno.Obra;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.DTO;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Administracion.ControlInterno
{
    public class ObraDAO : GenericDAO<tblM_O_CatCCAC>, IObraDAO
    {
        private string TODOS = "TODOS";
        private string nombreController = "Obra";
        private int idSistema = (int)SistemasEnum.ADMINISTRACION_FINANZAS;
        private int idModulo = (int)BitacoraEnum.AperuraObra;
        private List<int> lstIdUsuarioEnkontrol = new List<int>() { 11, 1184 };
        #region Gestion
        /// <summary>
        /// Obras de Sigoplan
        /// </summary>
        public List<tblM_O_CatCCAC> getAllObra()
        {
            return _context.tblM_O_CatCCAC.ToList();
        }
        public List<tblM_O_CatCCAC> getLstObra(BusqObraGestionDTO busq)
        {
            var lstObra = from cat in _context.tblM_O_CatCCAC
                          where busq.lstTipo.Contains(cat.tipo) && busq.lstAuth.Contains(cat.authEstado)
                              select cat;
            var lstCC = from cc in lstObra
                        where cc.tipo != tipoCatalogoEnum.AreaCuenta && (busq.lstCC.Contains(TODOS) ? true : busq.lstCC.Contains(cc.clave))
                        select cc;
            var lstAC = from ac in lstObra
                        where ac.tipo == tipoCatalogoEnum.AreaCuenta && (busq.lstAC.Contains(TODOS) ? true : busq.lstAC.Contains(ac.clave))
                        select ac;
            var lst = lstCC.Union(lstAC).Distinct();
            return lst.ToList();
        }
        #endregion
        #region _formObra
        public bool GuardarObra(List<tblM_O_CatCCAC> lst)
        {
            var ahora = DateTime.Now;
            var esGuardado = false;
            var esNuevo = lst.Count > 0 && lst.Any(a => a.id > 0);
            using(var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    var idUsuario = vSesiones.sesionUsuarioDTO.id;
                    lst.ForEach(catalogo =>
                    {
                        var usuario = (from user in _context.tblP_Usuario
                                       where user.id == idUsuario
                                       select user).FirstOrDefault();
                        var db = (from cat in _context.tblM_O_CatCCAC
                                  where cat.clave == catalogo.clave
                                  select cat).FirstOrDefault();
                        if(db == null)
                        {
                            db = new tblM_O_CatCCAC()
                            {
                                idUsuarioRegistro = idUsuario,
                                usuario = usuario,
                                fechaRegistro = ahora,
                                lstAuth = new List<tblM_O_CatCCAC_Auth>()
                            };
                        }
                        else
                        {
                            catalogo.id = db.id;
                        }
                        catalogo.idUsuarioRegistro = db.idUsuarioRegistro;
                        catalogo.usuario = db.usuario;
                        catalogo.fechaRegistro = db.fechaRegistro;
                        catalogo.lstAuth.ToList().ForEach(auth =>
                        {
                            var userAuth = (from user in _context.tblP_Usuario
                                            where user.id == auth.idUsuario
                                            select user).FirstOrDefault();
                            var dbAuth = (from aut in db.lstAuth
                                          where aut.id == auth.id
                                          select aut).FirstOrDefault();
                            auth.esActivo = true;
                            if(dbAuth == null)
                            {
                                dbAuth = new tblM_O_CatCCAC_Auth()
                                {
                                    usuario = userAuth,
                                    firma = string.Empty,
                                    motivoRechazo = string.Empty,
                                    fechaFirma = System.Data.SqlTypes.SqlDateTime.MinValue.Value,
                                    fechaRegistro = ahora
                                };
                            }
                            else
                            {
                                auth.id = dbAuth.id;
                            }
                            auth.usuario = dbAuth.usuario;
                            auth.fechaFirma = auth.authEstado == authEstadoEnum.Autorizado || auth.authEstado == authEstadoEnum.Rechazado ? ahora : System.Data.SqlTypes.SqlDateTime.MinValue.Value;
                            auth.firma = setFirma(auth);
                            auth.motivoRechazo = auth.authEstado == authEstadoEnum.Rechazado ? auth.motivoRechazo : string.Empty;
                            auth.fechaRegistro = db.fechaRegistro;
                            _context.tblM_O_CatCCAC_Auth.AddOrUpdate(aut => aut.id, auth);
                            _context.SaveChanges();
                        });
                        _context.tblM_O_CatCCAC.AddOrUpdate(cat => cat.id, catalogo);
                        _context.SaveChanges();
                    });
                    var esEnviado = enviarAlertaCorreo(lst);
                    esGuardado = lst.Count > 0 && lst.All(a => a.id > 0) && esEnviado;
                    if(esGuardado)
                    {
                        dbTransaction.Commit();
                    }
                }
                catch(Exception o_O)
                {
                    dbTransaction.Rollback();
                    var primero = lst.FirstOrDefault();
                    var accion = esNuevo ? AccionEnum.AGREGAR : AccionEnum.ACTUALIZAR;
                    LogError(idSistema, idModulo, nombreController, "GuardarObra", o_O, accion, primero.id, primero);
                }
            return esGuardado;
        }
        public tblM_O_CatCCAC getFormDesdeClave(string clave)
        {
            return (from catalogo in _context.tblM_O_CatCCAC
                    where catalogo.clave == clave
                    select catalogo).FirstOrDefault() ?? new tblM_O_CatCCAC();
        }
        public List<tblM_O_CatCCAC> getAutocompleteClave(string term)
        {
            var lst = (from catalogo in _context.tblM_O_CatCCAC
                       where catalogo.clave.Contains(term)
                       select catalogo)
                      .GroupBy(g => g.clave)
                      .Select(s =>
                          s.FirstOrDefault(p => p.esActivo)
                      ).Take(15);
            return lst.ToList();
        }
        #endregion
        #region combobox

        #endregion
        #region Auxiliares
        string setFirma(tblM_O_CatCCAC_Auth auth)
        {
            var firma = string.Empty;
            switch(auth.authEstado)
            {
                case authEstadoEnum.Autorizado:
                    firma = string.Format("{0}|{1}|{2}|{3}|{4}|A",
                        auth.id,
                        auth.idCatalogo,
                        auth.idUsuario,
                        auth.fechaFirma.ToString("ddMMyyyy"),
                        auth.fechaFirma.ToString("hhmm"));
                    break;
                case authEstadoEnum.Rechazado:
                    firma = string.Format("{0}|{1}|{2}|{3}|{4}|R",
                        auth.id,
                        auth.idCatalogo,
                        auth.idUsuario,
                        auth.fechaFirma.ToString("ddMMyyyy"),
                        auth.fechaFirma.ToString("hhmm"));
                    break;
                default:
                    break;
            }
            return firma;
        }
        bool enviarAlertaCorreo(List<tblM_O_CatCCAC> lst)
        {
            var esEnviado = false;
            var lstEnEspera = from catalogo in lst where catalogo.authEstado == authEstadoEnum.EnEspera select catalogo;
            var lstAutorizado = from catalogo in lst where catalogo.authEstado == authEstadoEnum.Autorizado && !catalogo.exiteEnkontrol select catalogo;
            var lstExisEnkontrol = from catalogo in lst where catalogo.authEstado == authEstadoEnum.Autorizado && catalogo.exiteEnkontrol select catalogo;
            var lstRechazado = from catalogo in lst where catalogo.authEstado == authEstadoEnum.Rechazado select catalogo;
            if(lstEnEspera.Count() > 0)
            {
                esEnviado = enviaAlertaASiguiente(lstEnEspera);
            }
            if(lstAutorizado.Count() > 0)
            {
                esEnviado = enviaCorreoParaEnkontrol(lstAutorizado);
                enviaAlertaASiguiente(lstAutorizado);
            }
            if(lstExisEnkontrol.Count() > 0)
            {
                esEnviado = enviaCorreoAutorizados(lstExisEnkontrol);
            }
            if(lstRechazado.Count() > 0)
            {
                esEnviado = enviaCorreoRechazados(lstRechazado);
                enviaAlertaASiguiente(lstRechazado);
            }
            return esEnviado;
        }
        bool enviaAlertaASiguiente(IEnumerable<tblM_O_CatCCAC> lst)
        {
            var lstAuthAplicarVisto = new List<authEstadoEnum>() 
            {
                authEstadoEnum.Autorizado,
                authEstadoEnum.Rechazado
            };
            var lstAlerta = from alerta in _context.tblP_Alerta
                            where !alerta.visto && alerta.sistemaID == idSistema && alerta.moduloID == idModulo
                            select alerta;
            lst.ToList().ForEach(catalogo =>
            {
                catalogo.lstAuth.Where(w => w.authEstado == authEstadoEnum.EnEspera).ToList().ForEach(auth =>
                {
                    var alerta = (from bdAlert in lstAlerta
                                  where bdAlert.userEnviaID == catalogo.idUsuarioRegistro && lstIdUsuarioEnkontrol.Contains(bdAlert.userRecibeID) && bdAlert.objID == auth.idCatalogo
                                  select bdAlert).FirstOrDefault();
                    if(alerta == null)
                    {
                        alerta = new tblP_Alerta()
                        {
                            sistemaID = idSistema,
                            moduloID = idModulo,
                            userEnviaID = auth.idUsuario,
                            userRecibeID = lstIdUsuarioEnkontrol.FirstOrDefault(),
                            tipoAlerta = 2,
                            url = "/Administrativo/Obra/Gestion",
                            msj = string.Format("Marcar enkontrol {0} {1}", catalogo.clave, catalogo.descripcion),
                            objID = catalogo.id,
                            documentoID = 0
                        };
                    }
                    alerta.visto = false;
                    _context.tblP_Alerta.AddOrUpdate(alerta);
                    _context.SaveChanges();
                });
                catalogo.lstAuth.Where(w => lstAuthAplicarVisto.Contains(w.authEstado)).ToList().ForEach(auth =>
                {
                    var alerta = (from bdAlert in lstAlerta
                                  where bdAlert.userRecibeID == auth.idUsuario && bdAlert.objID == auth.idCatalogo
                                  select bdAlert).FirstOrDefault();
                    if(alerta != null)
                    {
                        alerta.visto = true;
                        _context.tblP_Alerta.AddOrUpdate(alerta);
                        _context.SaveChanges();
                    }
                });
            });
            return true;
        }
        bool enviaCorreoAutorizados(IEnumerable<tblM_O_CatCCAC> lst)
        {
            var correo = new CorreoDTO();
            correo.asunto = "Autorización de Aperturas";
            correo.cuerpo +=
            @"<p class=MsoNormal>Buen día</p>
            <p class=MsoNormal>
                Se informa que se dieron de alta los siguientes catálogos en enkontrol.
            </p>
            <table>
                <thead>
                    <tr>
                        <th>Clave</th>
                        <th>Descripción</th>
                        <th>Tipo</th>
                    </tr>
                </thead>
            <tbody>";
            lst.ToList().ForEach(catalogo =>
            {
                correo.cuerpo += string.Format(
                @"<tr>
                    <th>{0}</th>
                    <th>{1}</th>
                    <th>{2}</th>
                 </tr>"
                , catalogo.clave
                , catalogo.descripcion
                , catalogo.tipo.GetDescription());
                correo.correos.AddRange(catalogo.lstAuth.Select(auth => auth.usuario.correo));
            });
            correo.cuerpo +=
            @"</tbody></table>
            <p class=MsoNormal>
                Favor de ingresar al sistema SIGOPLAN, en el apartado de CONTROL INTERNO, menú Apertura en la opción de gestión.
            </p>";
            var esEnviado = correo.Enviar();
            return esEnviado;
        }
        bool enviaCorreoParaEnkontrol(IEnumerable<tblM_O_CatCCAC> lst)
        {
            var correo = new CorreoDTO();
            var lstCorreo = from usuario in _context.tblP_Usuario
                            where lstIdUsuarioEnkontrol.Contains(usuario.id)
                            select usuario.correo;
            correo.correos = lstCorreo.ToList();
            correo.asunto = "Autorización de Aperturas";
            correo.cuerpo +=
            @"<p class=MsoNormal>Buen día</p>
            <p class=MsoNormal>
                Se informa que se finalizó correctamente el proceso de autorización de aperturas de la Arrendadora del siguiente catálogo.
            </p>
            <table>
                <thead>
                    <tr>
                        <th>Clave</th>
                        <th>Descripción</th>
                        <th>Tipo</th>
                    </tr>
                </thead>
            <tbody>";
            lst.ToList().ForEach(catalogo =>
            {
                correo.cuerpo += string.Format(
                @"<tr>
                    <th>{0}</th>
                    <th>{1}</th>
                    <th>{2}</th>
                 </tr>"
                , catalogo.clave
                , catalogo.descripcion
                , catalogo.tipo.GetDescription());
            });
            correo.cuerpo +=
            @"</tbody></table>
            <p class=MsoNormal>
                Favor de ingresar al sistema SIGOPLAN, en el apartado de CONTROL INTERNO, menú Apertura en la opción de gestión.
            </p>";
            var esEnviado = correo.Enviar();
            return esEnviado;
        }
        bool enviaCorreoRechazados(IEnumerable<tblM_O_CatCCAC> lst)
        {
            var correo = new CorreoDTO();
            correo.asunto = "Rechazo de Aperturas";
            correo.cuerpo +=
            @"<p class=MsoNormal>Buen día</p>
            <p class=MsoNormal>
                Se informa que se dieron de rechazaron los siguientes catálogos en enkontrol.
            </p>
            <table>
                <thead>
                    <tr>
                        <th>Clave</th>
                        <th>Descripción</th>
                        <th>Tipo</th>
                        <th>Motivo</th>
                    </tr>
                </thead>
            <tbody>";
            lst.ToList().ForEach(catalogo =>
            {
                correo.cuerpo += string.Format(
                @"<tr>
                    <th>{0}</th>
                    <th>{1}</th>
                    <th>{2}</th>
                    <th>{3}</th>
                 </tr>"
                , catalogo.clave
                , catalogo.descripcion
                , catalogo.tipo.GetDescription()
                , catalogo.lstAuth.Select(s => s.motivoRechazo).ToList().ToLine(".").Replace("'", string.Empty));
                correo.correos.AddRange(catalogo.lstAuth.Select(auth => auth.usuario.correo));
            });
            correo.cuerpo +=
            @"</tbody></table>";
            var esEnviado = correo.Enviar();
            return esEnviado;
        }
        #endregion
        #region Actualización
        /// <summary>
        /// TODO: actualizar
        /// </summary>
        /// <returns></returns>
        bool ActualizarEnkontrolASigoplan()
        {
            var esGuardado = false;
            //using(var _transaccion = _context.Database.BeginTransaction())
            //{
            var ahora = DateTime.Now;
            //var lst = _contextEnkontrol.Select<tblM_O_CatCCAC>(EnkontrolAmbienteEnum.Prod, queryAreaCuenta());
            var lst = _contextEnkontrol.Select<tblM_O_CatCCAC>(EnkontrolAmbienteEnum.Prod, queryCentroCosto());
            var gaytan = _context.tblP_Usuario.FirstOrDefault(u => u.id == 4);
            var sanchez = _context.tblP_Usuario.FirstOrDefault(u => u.id == 1073);
            var romero = _context.tblP_Usuario.FirstOrDefault(u => u.id == 1184);
            lst.ForEach(catalogo =>
            {
                catalogo.usuario = romero;
                if(catalogo.tipo != tipoCatalogoEnum.AreaCuenta)
                {
                    if(catalogo.clave.All(clave => char.IsNumber(clave)))
                    {
                        if(catalogo.descripcion.ToUpper().Contains("NOMINA"))
                        {
                            catalogo.tipo = tipoCatalogoEnum.DepartamentoDeNomina;
                        }
                        else
                        {
                            catalogo.tipo = tipoCatalogoEnum.DepartamentoAdministrativo;
                        }
                    }
                }
                catalogo.lstAuth = new List<tblM_O_CatCCAC_Auth>()
                {
                    new tblM_O_CatCCAC_Auth() {
                        idUsuario = gaytan.id,
                        orden = 1,
                        motivoRechazo = string.Empty,
                        authEstado = authEstadoEnum.Autorizado,
                        fechaFirma = ahora,
                        fechaRegistro = ahora,
                        esActivo = true,
                        usuario = gaytan,
                        firma = string.Empty,
                    },
                    new tblM_O_CatCCAC_Auth() {
                        idUsuario = sanchez.id,
                        orden = 2,
                        motivoRechazo = string.Empty,
                        authEstado = authEstadoEnum.Autorizado,
                        fechaFirma = ahora,
                        fechaRegistro = ahora,
                        esActivo = true,
                        usuario = sanchez,
                        firma = string.Empty,
                    },
                };
                var bd = _context.tblM_O_CatCCAC.FirstOrDefault(cat => cat.clave == catalogo.clave) ?? new tblM_O_CatCCAC() { lstAuth = new List<tblM_O_CatCCAC_Auth>() };
                catalogo.id = bd.id;
                _context.tblM_O_CatCCAC.AddOrUpdate(catalogo);
                _context.SaveChanges();
                catalogo.lstAuth.ToList().ForEach(auth =>
                {
                    var bdAuth1 = bd.lstAuth.FirstOrDefault(a => a.idUsuario == auth.idUsuario);
                    if(bdAuth1 != null)
                    {
                        auth.id = bdAuth1.id;
                    }
                    auth.idCatalogo = catalogo.id;
                    _context.tblM_O_CatCCAC_Auth.AddOrUpdate(x => x.id, auth);
                    _context.SaveChanges();
                    auth.firma = setFirma(auth);
                    _context.SaveChanges();
                });

            });
            //_transaccion.Commit();
            esGuardado = true;
            //}
            return esGuardado;
        }
        string queryAreaCuenta()
        {
            return @"SELECT CAST(ac.area AS varchar(10)) +' -'+ CAST(ac.cuenta AS varchar(10)) AS clave, MAX(ac.descripcion) AS descripcion, (MIN(pol.fechapol) )AS fechaArranque,
                        1 AS tipo, 1184 AS idUsuarioRegistro, 1 AS authEstado, 1 AS exiteEnkontrol, GETDATE() AS fechaRegistro, 1 AS       esActivo
                    FROM si_area_cuenta ac
                    INNER JOIN sc_movpol mov ON mov.area = ac.area AND mov.cuenta_oc = ac.cuenta
                    INNER JOIN sc_polizas pol ON pol.year = mov.year AND pol.mes = mov.mes AND pol.tp = mov.tp AND pol.poliza =         mov.poliza
                    GROUP BY ac.area, ac.cuenta";
        }
        string queryCentroCosto()
        {
            return @"SELECT cc.cc AS clave, MAX(cc.descripcion) AS descripcion, (MIN(pol.fechapol) )AS fechaArranque,
    2 AS tipo, 1184 AS idUsuarioRegistro, 1 AS authEstado, 1 AS exiteEnkontrol, GETDATE() AS fechaRegistro, 1 AS esActivo
                    FROM cc cc
                    INNER JOIN sc_movpol mov ON mov.cc = cc.cc
                    INNER JOIN sc_polizas pol ON pol.year = mov.year AND pol.mes = mov.mes AND pol.tp = mov.tp AND pol.poliza =         mov.poliza
                    GROUP BY cc.cc";
        }
        #endregion
    }
}
