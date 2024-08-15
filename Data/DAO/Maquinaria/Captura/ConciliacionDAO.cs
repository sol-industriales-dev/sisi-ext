using Core.DAO.Maquinaria;
using Core.DTO;
using Core.DTO.Maquinaria.Captura.conciliacion;
using Core.DTO.Maquinaria.Catalogos;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Catalogo.Cararatulas;
using Core.Entity.Principal.Alertas;
using Core.Entity.Principal.Multiempresa;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Multiempresa;
using Core.Enum.Principal.Bitacoras;
using Data.DAO.Contabilidad.Reportes;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using MoreLinq.Extensions;
using Core.DTO.Utils.Data;
using System.Data.Odbc;
using Newtonsoft.Json;
using Data.DAO.Enkontrol.Almacen;
using Core.Enum.Enkontrol.Compras;
using Core.Entity.Maquinaria._Caratulas;

namespace Data.DAO.Maquinaria.Captura
{
    public class ConciliacionDAO : GenericDAO<tblM_EncCaratula>, IConciliacionDAO
    {
        public string GetNameObra(int id)
        {
            return _context.tblP_CC.FirstOrDefault(x => x.id == id).descripcion;
        }
        public List<tblAutorizaConciliacionDTO> getAutorizacionesPendientes(int CCID)
        {
            List<tblAutorizaConciliacionDTO> resultReturn = new List<tblAutorizaConciliacionDTO>();

            var usuarioID = vSesiones.sesionUsuarioDTO.id;

            var listaCC = _context.tblP_CC_Usuario.Where(x => x.usuarioID == usuarioID).Select(x => x.cc).ToList();

            var ccObj = _context.tblP_CC.Where(x => listaCC.Contains(x.areaCuenta)).Select(x => x.id);
            var result = _context.tblM_CapEncConciliacionHorometros.Where(c => (CCID != 0 ? c.centroCostosID == CCID : true) && c.estatus == 0).ToList().Where(y => ccObj.Contains(y.centroCostosID)).ToList();

            foreach (var item in result)
            {
                tblAutorizaConciliacionDTO obj = new tblAutorizaConciliacionDTO();
                var objAreaCuenta = _context.tblP_CC.Where(x => x.id == item.centroCostosID).FirstOrDefault();
                var autorizacion = _context.tblM_AutorizaConciliacionHorometros.FirstOrDefault(x => x.conciliacionID == item.id);
                if (autorizacion != null)
                {
                    obj.areaCuenta = objAreaCuenta.areaCuenta;
                    obj.descripcion = objAreaCuenta.descripcion;
                    obj.estatus = autorizacion.estatus;
                    //DateTime fecha = DateTime.Now;
                    //DateTime FechaSend = new DateTime(item.anio, 01, 01);
                    //var Data = item.esQuincena ? GlobalUtils.GetQuincenas(item.anio).FirstOrDefault(x => x.Value == item.fechaID) : GetFechas(FechaSend).FirstOrDefault(x => x.Value == item.fechaID);
                    //if (Data != null)
                    //    obj.periodo = Data.Text;
                    obj.periodo = item.fechaInicio.ToShortDateString() + " - " + item.fechaFin.ToShortDateString();
                    obj.btnRpt = item.id;
                    obj.btnValidar = autorizacion.id;
                    obj.isUsuarioAutorisable = getUsuarioValidando(usuarioID, autorizacion);
                    resultReturn.Add(obj);
                }
            }

            return resultReturn;
        }

        private bool getUsuarioValidando(int usuarioID, tblM_AutorizaConciliacionHorometros autorizacion)
        {
            switch (autorizacion.autorizando)
            {
                case 1:
                    if (autorizacion.autorizaAdmin.Equals(usuarioID))
                        return true;
                    else
                        return false;
                case 2:
                    if (autorizacion.autorizaGerenteID.Equals(usuarioID))
                        return true;
                    else
                        return false;
                case 3:
                    if (autorizacion.autorizaDirector.Equals(usuarioID))
                        return true;
                    else
                        return false;
                default:
                    return false;
            }
        }



        public tblM_CapEncConciliacionHorometros getCapEncConciliacion(int id)
        {
            return _context.tblM_CapEncConciliacionHorometros.FirstOrDefault(x => x.id == id);
        }
        public List<tblM_CapConciliacionHorometros> getConciliaciones(int id)
        {
            return _context.tblM_CapConciliacionHorometros.Where(x => x.idEncCaratula == id).ToList();
        }

        public int getConciliacionesExiste(int fechaID, int centroCostosID, DateTime fechaInicio, DateTime fechaFinal)
        {
            var lstConci = _context.tblM_CapEncConciliacionHorometros.Where(x => x.fechaInicio == fechaInicio && x.fechaFin == fechaFinal && x.centroCostosID == centroCostosID && x.estatus != 2 && x.esQuincena).ToList();
            var ban = 0;
            if (lstConci.Count != 0)
            {
                var caratula = getEncabezado(centroCostosID);
                if (caratula == null)
                    ban = 2;
                else
                    ban = 1;
            }
            else
            {
                ban = 2;
            }
            return ban;
        }
        public bool GuardarCaratula(tblM_EncCaratula enc, List<tblM_CapCaratula> lst, List<tblM_EncCaratula_Concideracion> lstCon)
        {
            try
            {
                var anterior = getEncabezado(enc.ccID);
                var nuevo = new tblM_EncCaratula()
                {
                    ccID = enc.ccID,
                    creacion = DateTime.Now,
                    idUsuario = vSesiones.sesionUsuarioDTO.id,
                    isActivo = false,
                    moneda = enc.moneda,
                    fechaVigencia = enc.fechaVigencia
                };
                _context.tblM_EncCaratula.AddOrUpdate(nuevo);
                SaveChanges();
                var lstCap = _context.tblM_CapCaratula.Where(w => w.idCaratula.Equals(nuevo.id)).ToList();
                lst.ForEach(p =>
                {
                    p.EncCaratula = nuevo;
                    p.idCaratula = nuevo.id;
                    p.equipo = string.Format("{0} {1}",
                        _context.tblM_CatGrupoMaquinaria.FirstOrDefault(g => g.id == p.idGrupo).descripcion,
                        _context.tblM_CatModeloEquipo.FirstOrDefault(m => m.id == p.idModelo).descripcion);
                    if (lstCap.Any(w => w.idGrupo.Equals(p.idGrupo) && w.idModelo.Equals(p.idModelo)))
                        p.id = lstCap.FirstOrDefault(w => w.idCaratula.Equals(w.idCaratula) && w.idGrupo.Equals(p.idGrupo) && w.idModelo.Equals(p.idModelo)).id;
                    _context.tblM_CapCaratula.AddOrUpdate(p);

                    // Update(p, p.id, (int)BitacoraEnum.CAPCARATULA);
                    SaveChanges();
                });
                foreach (var c in lstCon)
                {
                    c.EncCaratula = nuevo.id;
                    _context.tblM_EncCaratulatblM_CatConsideracionCostoHora.AddOrUpdate(c);
                    SaveChanges();

                    // Update(c, c.id, (int)BitacoraEnum.CAPCARATULADET);
                }
                setAutorizaciones(nuevo);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void setAutorizaciones(tblM_EncCaratula obj)
        {
            tblM_AutorizacionCaratulaPreciosU objResult = new tblM_AutorizacionCaratulaPreciosU();

            objResult.usuarioElaboraID = vSesiones.sesionUsuarioDTO.id;

            DateTime fecha = DateTime.Now;
            string f = fecha.ToString("ddMMyyyy") + fecha.Hour + "" + fecha.Minute;
            objResult.cadenaElabora = obj.id + f + "" + objResult.usuarioElaboraID + "A";

            objResult.cadenaAutoriza = "";
            objResult.cadenaVobo1 = "";
            objResult.cadenaVobo2 = "";
            objResult.caratulaID = obj.id;
            objResult.estadoCaratula = 0;
            objResult.fechaAutoriza = null;
            objResult.fechaElaboracion = DateTime.Now;
            objResult.fechaVobo1 = null;
            objResult.fechaVobo2 = null;
            objResult.firmaAutoriza = 0;
            objResult.firmaElabora = 1;
            objResult.firmaVobo1 = 0;
            objResult.firmaVobo2 = 0;

            objResult.id = 0;
            objResult.obraID = obj.ccID;
            objResult.usuarioAutoriza = 1164;
            objResult.usuarioFirma = 2;
            objResult.usuarioVobo1 = 1064;
            objResult.usuarioVobo2 = 4;
            _context.tblM_AutorizacionCaratulaPreciosU.Add(objResult);
            _context.SaveChanges();
            sendCorreo(objResult, 1, null);
        }

        public List<ComboDTO> fillCboCentrosCosto(int usuarioID)
        {
            var usuarioCC = from cc in _context.tblP_CC.ToList()
                            join ccA in _context.tblM_EncCaratula.ToList() on cc.id equals ccA.ccID
                            select new ComboDTO { Text = cc.descripcion, Value = cc.id.ToString(), Prefijo = cc.areaCuenta };

            return usuarioCC.Distinct().ToList();
        }
        public tblM_EncCaratula getEncabezado(int ccID)
        {
            var enc = _context.tblM_EncCaratula.Where(e => e.ccID.Equals(ccID) && e.isActivo).OrderByDescending(x => x.id).FirstOrDefault();
            if (enc is tblM_EncCaratula)
                return enc;
            else
                return new tblM_EncCaratula() { ccID = ccID, creacion = new DateTime().Date };
        }
        public tblM_Caratula getEncabezado_nuevo()
        {
            var enc = _context.tblM_Caratula.Where(e => e.autorizada == 1 ).OrderByDescending(x => x.id).FirstOrDefault();
            return enc;
        }
        public tblM_EncCaratula getEncCartulaFromIdCaratula(int idEnc)
        {
            return _context.tblM_EncCaratula.FirstOrDefault(e => e.id.Equals(idEnc) && e.isActivo);
        }
        public List<tblM_CatConsideracionCostoHora> getLstFullConsiceracion()
        {
            try
            {
                return _context.tblM_CatConsideracionCostoHora.ToList();
            }
            catch (Exception)
            {
                return new List<tblM_CatConsideracionCostoHora>();
            }

        }
        public List<tblM_EncCaratula_Concideracion> getLstConsiceracionWhereEnc(int enc)
        {
            try
            {
                return _context.tblM_EncCaratulatblM_CatConsideracionCostoHora.Where(w => w.EncCaratula.Equals(enc)).ToList();
            }
            catch (Exception)
            {
                return new List<tblM_EncCaratula_Concideracion>();
            }

        }
        public int getLengthConsideraciones()
        {
            return _context.tblM_CatConsideracionCostoHora.Count();
        }
        public List<tblM_CapCaratula> getLstPrecios(int idEnc)
        {
            try
            {

                List<tblM_CapCaratula> tblM_CapCaratulaLista = new List<tblM_CapCaratula>();
                var cc = _context.tblM_EncCaratula.FirstOrDefault(x => x.id == idEnc);
                string areaCuenta = "";
                if (cc != null)
                {
                    areaCuenta = _context.tblP_CC.FirstOrDefault(x => x.id == cc.ccID).areaCuenta;
                }
                var consulta = (from c in _context.tblM_CapCaratula

                                where c.idCaratula == idEnc
                                select c).ToList();
                if (consulta.Count >= 0)
                {
                    tblM_CapCaratulaLista.AddRange(consulta);

                    //var ret = _context.tblM_CatMaquina.ToList()
                    //    .Where(x => x.centro_costos.Equals(areaCuenta))
                    //    .Where(x => !x.estatus.Equals(0))
                    //    .Select(n => new tblM_CapCaratula
                    //    {
                    //        id = 0,
                    //        idGrupo = n.grupoMaquinariaID,
                    //        idModelo = n.modeloEquipoID,
                    //        activo = false,
                    //        costo = 0,
                    //        EncCaratula = new tblM_EncCaratula(),
                    //        equipo = n.grupoMaquinaria.descripcion + " " + n.modeloEquipo.descripcion,
                    //    })
                    //.ToList();
                    //var modelosPrecio = _context.tblM_CatMaquina.Where(x => x.estatus != 0 && x.centro_costos == areaCuenta && x.CargoEmpresa == 1).ToList();

                    //foreach (var n in modelosPrecio)
                    //{

                    //    if (consulta.Where(x => x.idModelo == n.modeloEquipoID).Count() == 0)
                    //    {
                    //        tblM_CapCaratula objCaratula = new tblM_CapCaratula();

                    //        objCaratula.id = 0;
                    //        objCaratula.idGrupo = n.grupoMaquinariaID;

                    //        objCaratula.idModelo = n.modeloEquipoID;
                    //        objCaratula.activo = false;
                    //        objCaratula.costo = 0;
                    //        objCaratula.EncCaratula = new tblM_EncCaratula();
                    //        objCaratula.equipo = n.grupoMaquinaria.descripcion + " " + n.modeloEquipo.descripcion;

                    //        tblM_CapCaratulaLista.Add(objCaratula);
                    //    }

                    //}

                    return tblM_CapCaratulaLista;
                }
                else
                {
                    return tblM_CapCaratulaLista;
                }
            }
            catch (Exception)
            {
                return new List<tblM_CapCaratula>();
            }
        }


        public List<caratulaPreciosDTO> getCaratulaByID(int idEnc)
        {
            try
            {
                var setMoneda = _context.tblM_EncCaratula.FirstOrDefault(x => x.id == idEnc);
                string tipoMoneda = "MXN";
                if (setMoneda != null)
                {
                    tipoMoneda = setMoneda.moneda == 1 ? "MXN" : "USD";
                }
                return (from
                         a in _context.tblM_CapCaratula
                        join b in _context.tblM_CatModeloEquipo on a.idModelo equals b.id
                        join c in _context.tblM_CatGrupoMaquinaria on a.idGrupo equals c.id
                        where a.idCaratula == idEnc
                        select new caratulaPreciosDTO
                        {
                            ID = a.id,
                            Cantidad = 1,
                            Costo = a.costo,
                            CostoTotal = a.costo,
                            Equipo = c.descripcion,
                            Unidad = a.unidad == 1 ? "HORAS" : "DIA",
                            tipoCambio = tipoMoneda,
                            Modelo = b.descripcion
                        }).ToList();
            }
            catch (Exception)
            {
                return new List<caratulaPreciosDTO>();
            }
        }
        public List<tblM_CapCaratula> getNewLstPrecios(string cc)
        {
            var lstgrupo = (from h in _context.tblM_CapHorometro
                            join m in _context.tblM_CatMaquina on h.Economico equals m.noEconomico
                            join g in _context.tblM_CatGrupoMaquinaria on m.grupoMaquinariaID equals g.id
                            join e in _context.tblM_CatModeloEquipo on m.modeloEquipoID equals e.id
                            where h.CC.Equals(cc)
                            select new
                            {
                                idGrupo = g.id,
                                idModelo = e.id,
                                grupo = g.descripcion,
                                modelo = e.descripcion
                            }).ToList();
            var lstPrecio = lstgrupo.GroupBy(g => new { g.idGrupo, g.idModelo, g.grupo, g.modelo }).Select(s => new tblM_CapCaratula()
            {
                idGrupo = s.Key.idGrupo,
                idModelo = s.Key.idModelo,
                equipo = string.Format("{0} {1}", s.Key.grupo, s.Key.modelo)
            }).OrderBy(o => o.equipo).ToList();
            return lstPrecio;
        }
        public dynamic getCboGrupo()
        {
            return _context.tblM_CatGrupoMaquinaria.Select(g => new
            {
                Value = (int)g.id,
                Text = (string)g.descripcion
            });
        }
        public dynamic getCboModelo(int idGrupo)
        {
            var lst = _context.tblM_CatModeloEquipo.Where(w => w.idGrupo == idGrupo).ToList();
            var lst2 = _context.tblM_CatMaquina.Where(x => x.grupoMaquinariaID == idGrupo && x.estatus == 1).Select(x => x.modeloEquipo).ToList();
            var lstMerge = new List<tblM_CatModeloEquipo>();
            lstMerge.AddRange(lst);
            lstMerge.AddRange(lst2);
            return lstMerge.Select(m => new
            {
                Value = (int)m.id,
                Text = (string)m.descripcion
            }).Distinct();
        }
        public dynamic getCboCC()
        {
            switch (vSesiones.sesionEmpresaActual)
            {
                case (int)EmpresaEnum.Construplan:
                    return _context.tblP_CC.Where(x => x.estatus).ToList().Select(c => new
                    {
                        Text = c.cc + " - " + c.descripcion,
                        Value = c.id,
                        Prefijo = c.cc
                    }).OrderBy(o => o.Prefijo);
                case (int)EmpresaEnum.Arrendadora:
                    return _context.tblP_CC.Where(x => x.estatus).ToList().Select(c => new
                    {
                        Text = c.areaCuenta + " - " + c.descripcion,
                        Value = c.id,
                        Prefijo = c.areaCuenta,
                        area = c.area,
                        cuenta = c.cuenta
                    }).OrderBy(o => o.area).ThenBy(o => o.cuenta).ToList();
                case (int)EmpresaEnum.Colombia:
                    return _context.tblC_Nom_CatalogoCC.Where(x => x.estatus).ToList().Select(c => new
                    {
                        Text = c.cc + " - " + c.ccDescripcion,
                        Value = c.id,
                        Prefijo = c.cc
                    }).OrderBy(o => o.Prefijo);
                case (int)EmpresaEnum.Peru:
                    return _context.tblC_Nom_CatalogoCC.Where(x => x.estatus).ToList().Select(c => new
                    {
                        Text = c.cc + " - " + c.ccDescripcion,
                        Value = c.id,
                        Prefijo = c.cc
                    }).OrderBy(o => o.Prefijo);
                default:
                    return null;
            }
        }
        public int getMonedaCaratula(int ccID)
        {
            int moneda = 1;
            var cc = _context.tblM_IndicadoresCaratula.FirstOrDefault(x => x.idCC == ccID);
            if(cc!= null)
            {
                moneda = cc.moneda ? 2 : 1;
            }
            return moneda;
        }
        public List<ConciliacionHorometrosDTO> getTblConciliacion_old(tblM_EncCaratula enc, DateTime fechaInicio, DateTime fechaFinal)
        {
            try
            {
                var vFechaFinal = fechaFinal.ToShortDateString();
                List<ConciliacionHorometrosDTO> result = new List<ConciliacionHorometrosDTO>();
                var cc = _context.tblP_CC.FirstOrDefault(x => x.id == enc.ccID);

                var listaEconomicosHorometrosRaw = _context.tblM_CapHorometro.Where(x => x.CC == cc.areaCuenta && (x.Fecha >= fechaInicio && x.Fecha <= fechaFinal)).ToList();
                var listaEconomicos = _context.tblM_CatMaquina.Where(x => x.centro_costos == cc.areaCuenta && x.estatus > 0).ToList();
                var encCaratula = getEncabezado(cc.id);
                var listaCaratula = _context.tblM_CapCaratula.Where(x => encCaratula.id == x.idCaratula).ToList();
                var listaEconomicosAsignados = (from a in _context.tblM_AsignacionEquipos
                                                join m in _context.tblM_CatMaquina on a.noEconomicoID equals m.id
                                                where a.cc == cc.areaCuenta && a.estatus >= 3 && a.estatus < 5
                                                select new { a, m }).ToList();

                List<string> economicosUnicos = new List<string>();
                economicosUnicos.AddRange(listaEconomicosHorometrosRaw.Select(x => x.Economico).Distinct().ToList());
                economicosUnicos.AddRange(listaEconomicos.Select(x => x.noEconomico).Distinct().ToList());
                economicosUnicos.AddRange(listaEconomicosAsignados.Select(x => x.m.noEconomico).ToList());

                foreach (var economico in economicosUnicos.Distinct())
                {
                    fechaFinal = DateTime.Parse(vFechaFinal);
  
                    ConciliacionHorometrosDTO objSend = new ConciliacionHorometrosDTO();
                    var InfoMaquinaria = _context.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico.Equals(economico));
                    var objHorometrosMaquinaria = listaEconomicosHorometrosRaw.Where(x => x.Economico.Equals(economico)).ToList();


                    if (InfoMaquinaria != null)
                    {
                        objSend.cargo = InfoMaquinaria.CargoEmpresa;
                        objSend.idMaquinaria = InfoMaquinaria.id;
                        objSend.economico = InfoMaquinaria.noEconomico;
                        objSend.modelo = InfoMaquinaria.modeloEquipo == null ? "" : InfoMaquinaria.modeloEquipo.descripcion;
                        objSend.descripcion = InfoMaquinaria.grupoMaquinaria == null ? "" : InfoMaquinaria.grupoMaquinaria.descripcion;
                        objSend.moneda = encCaratula.moneda;

                        var CaratulaModelo = listaCaratula.FirstOrDefault(x => x.idGrupo == InfoMaquinaria.grupoMaquinariaID && x.idModelo == InfoMaquinaria.modeloEquipoID);

                        decimal horometroInicial = 0;
                        decimal horometroFinal = 0;
                        decimal horometroEfectivo = 0;
                        if (CaratulaModelo != null)
                        {
                            if (CaratulaModelo.unidad == 1)
                            {
                                if (objHorometrosMaquinaria.Count > 0)
                                {
                                    //horometroInicial = objHorometrosMaquinaria.OrderBy(x => x.id).FirstOrDefault().Horometro - objHorometrosMaquinaria.OrderBy(x => x.id).FirstOrDefault().HorasTrabajo;
                                    horometroInicial = objHorometrosMaquinaria.OrderBy(x => x.id).FirstOrDefault().Horometro - objHorometrosMaquinaria.OrderBy(x => x.id).FirstOrDefault().HorasTrabajo;
                                    horometroFinal = objHorometrosMaquinaria.OrderByDescending(x => x.id).FirstOrDefault().Horometro;
                                    horometroEfectivo = horometroFinal - horometroInicial;
                                    objSend.HI = horometroInicial;
                                    objSend.HF = horometroFinal;
                                    objSend.HE = horometroEfectivo;

                                }
                                else
                                {
                                    objSend.HI = 0;
                                    objSend.HF = 0;
                                    objSend.HE = 0;
                                }
                            }
                        }

                        var dataDias = listaEconomicosAsignados.Where(x => x.a.noEconomicoID == InfoMaquinaria.id && x.a.fechaAsignacion >= fechaInicio && x.a.fechaAsignacion <= fechaFinal && x.a.cc == cc.areaCuenta).ToList();

                        if (listaCaratula != null)
                        {

                            if (CaratulaModelo != null)
                            {

                                objSend.unidad = CaratulaModelo.unidad;
                                objSend.costo = CaratulaModelo.costo.ToString();
                                objSend.overhaul = CaratulaModelo.cOverhaul;

                                if (objSend.unidad != 1)
                                {
                                    if (dataDias.Count() > 0)
                                    {
                                        var asignacion = dataDias.FirstOrDefault();

                                        var controlRecpecion = _context.tblM_CatControlCalidad.Where(x => x.IdAsignacion == asignacion.a.id && x.TipoControl == 2).OrderByDescending(x => x.id).FirstOrDefault();

                                        DateTime nFecha = new DateTime();
                                        if (controlRecpecion != null)
                                        {
                                            if (controlRecpecion.FechaCaptura > fechaFinal)
                                            {
                                                horometroEfectivo = Math.Round((decimal)(fechaFinal.AddHours(23) - fechaInicio).TotalDays);
                                            }
                                            else if (controlRecpecion.FechaCaptura < fechaInicio)
                                            {
                                                horometroEfectivo = Math.Round((decimal)(fechaFinal.AddHours(23) - fechaInicio).TotalDays);
                                            }
                                            else
                                            {
                                                nFecha = controlRecpecion.FechaCaptura;
                                                horometroEfectivo = Math.Round((decimal)(fechaFinal.AddHours(23) - nFecha).TotalDays);
                                            }

                                        }
                                        else
                                        {

                                            if (controlRecpecion != null)
                                            {
                                                DateTime TimeFechaRecepcion = controlRecpecion.FechaCaptura;
                                                fechaFinal = TimeFechaRecepcion;

                                                if (horometroEfectivo >= 13)
                                                {
                                                    horometroEfectivo = Math.Round((decimal)(fechaFinal.AddHours(23) - fechaInicio).TotalDays);
                                                }

                                            }
                                            else
                                            {
                                                if (objSend.unidad == 2)
                                                {
                                                    horometroEfectivo = Math.Round((decimal)(fechaFinal.AddHours(23) - fechaInicio).TotalDays);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        horometroEfectivo = Math.Round((decimal)(fechaFinal.AddHours(23) - fechaInicio).TotalDays);
                                    }
                                }

                                if (InfoMaquinaria.CargoEmpresa == 1)
                                {
                                    objSend.costoTotal = Math.Truncate((horometroEfectivo * CaratulaModelo.costo) * 100) / 100;
                                }
                            }
                        }

                        result.Add(objSend);
                    }
                }

                return result;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public List<ConciliacionHorometrosDTO> getTblConciliacion(tblM_EncCaratula enc, DateTime fechaInicio, DateTime fechaFinal)
        {
            try
            {
                var vFechaFinal = fechaFinal.ToShortDateString();
                List<ConciliacionHorometrosDTO> result = new List<ConciliacionHorometrosDTO>();
                var cc = _context.tblP_CC.FirstOrDefault(x => x.id == enc.ccID);

                var listaEconomicosHorometrosRaw = _context.tblM_CapHorometro.Where(x => x.CC == cc.areaCuenta && (x.Fecha >= fechaInicio && x.Fecha <= fechaFinal)).ToList();
                var listaEconomicos = _context.tblM_CatMaquina.Where(x => x.centro_costos == cc.areaCuenta && x.estatus > 0).ToList();
                var encCaratula = getEncabezado_nuevo();
                
                var detalleCC = _context.tblM_IndicadoresCaratula.FirstOrDefault(x => x.idCC == enc.ccID);
                var moneda = detalleCC.moneda ? 2 : 1;
                var listaCaratula = _context.tblM_CaratulaDet.Where(x => x.caratula == encCaratula.id).ToList();
                var listaEconomicosAsignados = (from a in _context.tblM_AsignacionEquipos
                                                join m in _context.tblM_CatMaquina on a.noEconomicoID equals m.id
                                                where a.cc == cc.areaCuenta && a.estatus >= 3 && a.estatus < 5
                                                select new { a, m }).ToList();

                List<string> economicosUnicos = new List<string>();
                economicosUnicos.AddRange(listaEconomicosHorometrosRaw.Select(x => x.Economico).Distinct().ToList());
                economicosUnicos.AddRange(listaEconomicos.Select(x => x.noEconomico).Distinct().ToList());
                economicosUnicos.AddRange(listaEconomicosAsignados.Select(x => x.m.noEconomico).ToList());

                foreach (var economico in economicosUnicos.Distinct().OrderBy(x=>x))
                {

                    fechaFinal = DateTime.Parse(vFechaFinal);

                    ConciliacionHorometrosDTO objSend = new ConciliacionHorometrosDTO();
                    var InfoMaquinaria = _context.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico.Equals(economico));
                    var objHorometrosMaquinaria = listaEconomicosHorometrosRaw.Where(x => x.Economico.Equals(economico)).ToList();


                    if (InfoMaquinaria != null)
                    {
                        objSend.cargo = InfoMaquinaria.CargoEmpresa;
                        objSend.idMaquinaria = InfoMaquinaria.id;
                        objSend.economico = InfoMaquinaria.noEconomico;
                        objSend.modelo = InfoMaquinaria.modeloEquipo == null ? "" : InfoMaquinaria.modeloEquipo.descripcion;
                        objSend.descripcion = InfoMaquinaria.grupoMaquinaria == null ? "" : InfoMaquinaria.grupoMaquinaria.descripcion;
                        objSend.moneda = moneda;

                        var CaratulaModelo = listaCaratula.FirstOrDefault(x => x.idGrupo == InfoMaquinaria.grupoMaquinariaID && x.idModelo == InfoMaquinaria.modeloEquipoID);

                        decimal horometroInicial = 0;
                        decimal horometroFinal = 0;
                        decimal horometroEfectivo = 0;
                        if (CaratulaModelo != null)
                        {
                            if (CaratulaModelo.tipoHoraDia == 1)
                            {
                                if (objHorometrosMaquinaria.Count > 0)
                                {
                                    //horometroInicial = objHorometrosMaquinaria.OrderBy(x => x.id).FirstOrDefault().Horometro - objHorometrosMaquinaria.OrderBy(x => x.id).FirstOrDefault().HorasTrabajo;
                                    horometroInicial = objHorometrosMaquinaria.OrderBy(x => x.id).FirstOrDefault().Horometro - objHorometrosMaquinaria.OrderBy(x => x.id).FirstOrDefault().HorasTrabajo;
                                    horometroFinal = objHorometrosMaquinaria.OrderByDescending(x => x.Fecha).ThenByDescending(x=>x.turno).FirstOrDefault().Horometro;
                                    horometroEfectivo = horometroFinal - horometroInicial;
                                    objSend.HI = horometroInicial;
                                    objSend.HF = horometroFinal;
                                    objSend.HE = horometroEfectivo;

                                }
                                else
                                {
                                    objSend.HI = 0;
                                    objSend.HF = 0;
                                    objSend.HE = 0;
                                }
                            }
                        }

                        var dataDias = listaEconomicosAsignados.Where(x => x.a.noEconomicoID == InfoMaquinaria.id && x.a.fechaAsignacion >= fechaInicio && x.a.fechaAsignacion <= fechaFinal && x.a.cc == cc.areaCuenta).ToList();

                        if (listaCaratula != null)
                        {

                            if (CaratulaModelo != null)
                            {
                                var costo = moneda == 1 ? CaratulaModelo.costoMXN : CaratulaModelo.costoDLLS;
                                objSend.unidad = CaratulaModelo.tipoHoraDia;
                                objSend.costo = moneda == 1 ? CaratulaModelo.costoMXN.ToString() : CaratulaModelo.costoDLLS.ToString();
                                objSend.overhaul = moneda == 1 ? CaratulaModelo.depreciacionOHMXN : CaratulaModelo.depreciacionOHDLLS;

                                if (objSend.unidad != 1)
                                {
                                    if (dataDias.Count() > 0)
                                    {
                                        var asignacion = dataDias.FirstOrDefault();

                                        var controlRecpecion = _context.tblM_CatControlCalidad.Where(x => x.IdAsignacion == asignacion.a.id && x.TipoControl == 2).OrderByDescending(x => x.id).FirstOrDefault();

                                        DateTime nFecha = new DateTime();
                                        if (controlRecpecion != null)
                                        {
                                            if (controlRecpecion.FechaCaptura > fechaFinal)
                                            {
                                                horometroEfectivo = Math.Round((decimal)(fechaFinal.AddHours(23) - fechaInicio).TotalDays);
                                            }
                                            else if (controlRecpecion.FechaCaptura < fechaInicio)
                                            {
                                                horometroEfectivo = Math.Round((decimal)(fechaFinal.AddHours(23) - fechaInicio).TotalDays);
                                            }
                                            else
                                            {
                                                nFecha = controlRecpecion.FechaCaptura;
                                                horometroEfectivo = Math.Round((decimal)(fechaFinal.AddHours(23) - nFecha).TotalDays);
                                            }

                                        }
                                        else
                                        {

                                            if (controlRecpecion != null)
                                            {
                                                DateTime TimeFechaRecepcion = controlRecpecion.FechaCaptura;
                                                fechaFinal = TimeFechaRecepcion;

                                                if (horometroEfectivo >= 13)
                                                {
                                                    horometroEfectivo = Math.Round((decimal)(fechaFinal.AddHours(23) - fechaInicio).TotalDays);
                                                }

                                            }
                                            else
                                            {
                                                if (objSend.unidad == 2)
                                                {
                                                    horometroEfectivo = Math.Round((decimal)(fechaFinal.AddHours(23) - fechaInicio).TotalDays);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        horometroEfectivo = Math.Round((decimal)(fechaFinal.AddHours(23) - fechaInicio).TotalDays);
                                    }
                                }

                                if (InfoMaquinaria.CargoEmpresa == 1)
                                {
                                    objSend.costoTotal = Math.Truncate((horometroEfectivo * costo) * 100) / 100;
                                }
                            }
                        }

                        result.Add(objSend);
                    }
                }

                return result;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public List<tblAutorizaConciliacionDTO> getAutorizaciones(int CCID, int fechaID, DateTime fechaInio, DateTime fechaFin, int estatus, bool esQuincena)
        {
            List<tblAutorizaConciliacionDTO> resultReturn = new List<tblAutorizaConciliacionDTO>();
            var result = _context.tblM_CapEncConciliacionHorometros
                .Where(c => CCID != 0 ? c.centroCostosID == CCID : true)
                .Where(c => c.esQuincena.Equals(esQuincena))
                .Where(c => fechaID != 0 ? c.fechaInicio == fechaInio && c.fechaFin == fechaFin : true)
                .Where(c => c.estatus == estatus).ToList();
            foreach (var item in result)
            {
                var usuario = vSesiones.sesionUsuarioDTO.id;
                tblAutorizaConciliacionDTO obj = new tblAutorizaConciliacionDTO();
                var objAreaCuenta = _context.tblP_CC.Where(x => x.id == item.centroCostosID).FirstOrDefault();
                var autorizacion = _context.tblM_AutorizaConciliacionHorometros.FirstOrDefault(x => x.conciliacionID == item.id);



                if (autorizacion != null)
                {
                    obj.areaCuenta = objAreaCuenta.areaCuenta;
                    obj.descripcion = objAreaCuenta.descripcion;
                    obj.estatus = autorizacion.estatus;
                    //DateTime fecha = DateTime.Now;
                    //DateTime FechaSend = new DateTime(item.anio, 01, 01);
                    //var Data = item.esQuincena ? GlobalUtils.GetQuincenas(item.anio).FirstOrDefault(x => x.Value == item.fechaID) : GetFechas(FechaSend).FirstOrDefault(x => x.Value == item.fechaID);

                    //if (item.fechaID == 19 && CCID == 55)
                    //{
                    //    obj.periodo = "18/09/2019 - 30/09/2019";
                    //}
                    //else if (Data != null)
                    //    obj.periodo = Data.Text;
                    obj.periodo = item.fechaInicio.ToShortDateString() + " - " + item.fechaFin.ToShortDateString();
                    obj.btnRpt = item.id;
                    obj.btnValidar = autorizacion.id;
                    obj.isUsuarioAutorisable = getUsuarioValidando(usuario, autorizacion);
                    obj.comentario = (autorizacion.comentario != null && autorizacion.comentario.Length > 0) ? WebUtility.HtmlEncode(autorizacion.comentario) : "";
                    resultReturn.Add(obj);
                }
            }
            return resultReturn;
        }
        public tblM_AutorizaConciliacionHorometros loadAutorizacion(int validaID)
        {
            return _context.tblM_AutorizaConciliacionHorometros.FirstOrDefault(a => a.id.Equals(validaID));
        }
        public tblM_AutorizaConciliacionHorometros loadAutorizacionFromConciliacacionId(int conciliacionId)
        {
            return _context.tblM_AutorizaConciliacionHorometros.FirstOrDefault(a => a.conciliacionID.Equals(conciliacionId));
        }
        public List<FechasDTO> GetFechas(DateTime fecha)
        {
            List<FechasDTO> ListaFechas = new List<FechasDTO>();
            DateTime FechaInicio = new DateTime();
            DateTime FechaFin = new DateTime();
            for (int i = 1; i <= 52; i++)
            {
                if (i == 1)
                {
                    var diaSemana = (int)fecha.DayOfWeek;
                    FechaInicio = fecha.AddDays(-(int)fecha.DayOfWeek - 4);
                    int diasViernes = ((int)DayOfWeek.Tuesday - (int)fecha.DayOfWeek + 7) % 7;
                    FechaFin = fecha.AddDays(diasViernes);
                    ListaFechas.Add(new FechasDTO
                    {
                        Value = i,
                        Text = FechaInicio.ToShortDateString() + " - " + FechaFin.ToShortDateString()
                    });
                }
                else
                {
                    var TempFecha = FechaFin.AddDays(1);
                    FechaInicio = TempFecha;
                    FechaFin = TempFecha.AddDays(6);
                    ListaFechas.Add(new FechasDTO
                    {
                        Value = i,
                        Text = FechaInicio.ToShortDateString() + " - " + FechaFin.ToShortDateString()
                    });
                }
            }
            return ListaFechas;
        }
        public tblP_Autoriza getAuth(int perfil, string ac)
        {
            tblP_Autoriza data = new tblP_Autoriza();
            
            var ccUsuario = _context.tblP_CC_Usuario.Where(x => x.cc == ac).Select(x => x.id).ToList();
            data = _context.tblP_Autoriza.FirstOrDefault(x => x.perfilAutorizaID == perfil && ccUsuario.Contains(x.cc_usuario_ID));            
            //var autoriza = (from cc in _context.tblP_CC_Usuario
            //        join a in _context.tblP_Autoriza on cc.id equals a.cc_usuario_ID
            //        where cc.cc == ac && a.perfilAutorizaID == perfil
            //        select a).FirstOrDefault();
            return data;
        }
        public bool saveOrUpdateConciliacion(List<tblM_CapConciliacionHorometros> obj, tblM_CapEncConciliacionHorometros objDet)
        {
            try
            {
                var CentroCostos = _context.tblP_CC.FirstOrDefault(x => x.id == objDet.centroCostosID);

                tblM_AutorizaConciliacionHorometros objAutoriza = new tblM_AutorizaConciliacionHorometros();

                var returnData = getAuth(1, CentroCostos.areaCuenta);
                var objAdminAutoriza = getAuth(5, CentroCostos.areaCuenta);
                var objDirectorAutoriza = getAuth(4, CentroCostos.areaCuenta);

                if (returnData != null && objAdminAutoriza != null && objDirectorAutoriza != null)
                {
                    IObjectSet<tblM_CapEncConciliacionHorometros> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CapEncConciliacionHorometros>();
                    if (objDet == null) { throw new ArgumentNullException("Entity"); }
                    _objectSet.AddObject(objDet);
                    _context.SaveChanges();
                    foreach (var item in obj)
                    {
                        item.idEncCaratula = objDet.id;
                        //if (!string.IsNullOrEmpty(item.observaciones))
                        //    item.total = 0;
                    }
                    _context.tblM_CapConciliacionHorometros.AddRange(obj);
                    _context.SaveChanges();


                    IObjectSet<tblM_AutorizaConciliacionHorometros> _objectSet2 = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_AutorizaConciliacionHorometros>();
                    objAutoriza.id = 0;
                    objAutoriza.autorizaAdmin = objAdminAutoriza.usuarioID;
                    objAutoriza.autorizaGerenteID = returnData.usuarioID;
                    objAutoriza.autorizaDirector = objDirectorAutoriza.usuarioID;

                    objAutoriza.estatus = 0;
                    objAutoriza.firmaAdmin = "";
                    objAutoriza.firmaGerente = "";
                    objAutoriza.firmaDirector = "";
                    objAutoriza.pendienteAdmin = 0;
                    objAutoriza.pendienteGerente = 0;
                    objAutoriza.conciliacionID = objDet.id;
                    objAutoriza.pendienteDirector = 0;

                    objAutoriza.folio = string.Format("{0:D10}", objAutoriza.conciliacionID);
                    objAutoriza.autorizando = 1;

                    //  objAutoriza = getAutoriza(CentroCostos.areaCuenta);

                    if (objAutoriza == null) { throw new ArgumentNullException("Entity"); }
                    _objectSet2.AddObject(objAutoriza);
                    _context.SaveChanges();

                    setOrUpdateAlerta(vSesiones.sesionUsuarioDTO.id, objAdminAutoriza.usuarioID, false, objDet.id, 1);

                    string ac = CentroCostos.areaCuenta;
                    string nombre = CentroCostos.descripcion;
                    string periodo = objDet.fechaInicio.ToShortDateString() + " - " + objDet.fechaFin.ToShortDateString();

                    List<string> correos = new List<string>();
                    try
                    {
                        var listaGerentes = (from a in _context.tblP_Autoriza
                                             join b in _context.tblP_CC_Usuario on
                                             a.cc_usuario_ID equals b.id
                                             where b.cc == CentroCostos.areaCuenta && (a.perfilAutorizaID == 1 || a.perfilAutorizaID == 5 || a.perfilAutorizaID == 4)
                                             select a.usuario.correo).ToList();


                        correos.AddRange(listaGerentes);

                    }
                    catch (Exception)
                    {

                        correos.Add("aaron.romero@construplan.com.mx");
                    }


                    //correos.Add("laura.rodriguez@construplan.com.mx");
                    correos.Add("e.encinas@construplan.com.mx");
                    correos.Remove("e.fraijo@construplan.com.mx");


                    string correo = @"Buen dia.<br>Se realizo la conciliacion del periodo <b> " + periodo + @"</b> de la Obra <b>" + nombre + @"</b> <br> ";

                    try
                    {
                        correos.Remove("d.laborin@construplan.com.mx");

                        if (objAutoriza.autorizaGerenteID == 3337)
                        {
                            correos.Add("d.laborin@construplan.com.mx");
                        }
                        else
                        {
                            correos.Remove("d.laborin@construplan.com.mx");
                        }
                    }
                    catch(Exception e){
            
                    }
                    // correo += "<br><br>Este es un mensaje autogenerado por el sistema SIGOPLAN favor de no responderlo.";
                    string asuntoCorreo = setCorreoAutorizadores(correo, objAutoriza);
                    GlobalUtils.sendEmailAdjuntoInMemory2(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), "Conciliacion de Horometros del periodo " + periodo), asuntoCorreo, correos, null);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        private tblM_AutorizaConciliacionHorometros getAutoriza(string cc)
        {
            tblM_AutorizaConciliacionHorometros objAutoriza = new tblM_AutorizaConciliacionHorometros();

            var objD = _context.tblM_catAutorizaciones.Where(x => x.areaCuenta == cc);


            if (objD != null)
            {


            }

            return objAutoriza;
        }

        public bool sendValidacion(int conciliacionID, int respuesta, int idUsuario, string comentario)
        {
            try
            {
                var _cAuth = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_AutorizaConciliacionHorometros>();
                var autorizaConciliacionHorometros = _cAuth.FirstOrDefault(w => w.conciliacionID.Equals(conciliacionID));
                var conciliacionEnc = _context.tblM_CapEncConciliacionHorometros.FirstOrDefault(x => x.id == conciliacionID);
                var AreaCuenta = _context.tblP_CC.FirstOrDefault(x => x.id == conciliacionEnc.centroCostosID);
                var catAutorizaciones = _context.tblM_catAutorizaciones.FirstOrDefault(x => x.areaCuenta == AreaCuenta.areaCuenta);
                var ahora = DateTime.Now;

                // Se agrega el comenatario de rechazo
                if (respuesta == 2)
                {
                    autorizaConciliacionHorometros.comentario = comentario.Trim();
                }



                if (autorizaConciliacionHorometros.autorizaAdmin.Equals(idUsuario))
                {
                    if (autorizaConciliacionHorometros.autorizaAdmin.Equals(idUsuario))
                    {
                        autorizaConciliacionHorometros.pendienteAdmin = respuesta;
                        autorizaConciliacionHorometros.firmaAdmin = respuesta.Equals(2) ? "RECHAZADO" : string.Format("--{0}|{1:MMddyyyy}|{2:HHmm}|{3}|{4}--", conciliacionID, ahora, ahora, respuesta, idUsuario);
                        autorizaConciliacionHorometros.autorizando = 2;
                        setOrUpdateAlerta(autorizaConciliacionHorometros.autorizaAdmin, autorizaConciliacionHorometros.autorizaGerenteID, false, conciliacionID, 1);
                        setOrUpdateAlerta(autorizaConciliacionHorometros.autorizaAdmin, autorizaConciliacionHorometros.autorizaAdmin, false, conciliacionID, 2);
                        if (respuesta == 2)
                        {
                            tblM_CapEncConciliacionHorometros existeConciliacion = _context.tblM_CapEncConciliacionHorometros.Find(autorizaConciliacionHorometros.conciliacionID);

                            existeConciliacion.estatus = respuesta;
                            _context.SaveChanges();
                        }
                    }
                    SaveChanges();
                    SendCorreo(autorizaConciliacionHorometros.conciliacionID, autorizaConciliacionHorometros);
                    return true;
                }
                else if (autorizaConciliacionHorometros.autorizaGerenteID.Equals(idUsuario) && autorizaConciliacionHorometros.autorizando == 2)
                {
                    if (autorizaConciliacionHorometros.autorizaGerenteID.Equals(idUsuario))
                    {
                        tblM_AutorizaConciliacionHorometros existe = _context.tblM_AutorizaConciliacionHorometros.Find(autorizaConciliacionHorometros.id);

                        existe.pendienteGerente = respuesta;
                        existe.firmaGerente = respuesta.Equals(2) ? "RECHAZADO" : string.Format("--{0}|{1:MMddyyyy}|{2:HHmm}|{3}|{4}--", conciliacionID, ahora, ahora, respuesta, idUsuario);
                        existe.estatus = autorizaConciliacionHorometros.pendienteGerente != 0 && autorizaConciliacionHorometros.pendienteAdmin != 0 && autorizaConciliacionHorometros.pendienteDirector != 0 ? 1 : 0;
                        autorizaConciliacionHorometros.autorizando = 3;
                        _context.SaveChanges();
                        if (catAutorizaciones != null)
                        {
                            if (!catAutorizaciones.autorizaDirectorDivision)
                            {
                                tblM_AutorizaConciliacionHorometros existe2 = _context.tblM_AutorizaConciliacionHorometros.Find(autorizaConciliacionHorometros.id);
                                existe.pendienteDirector = respuesta;
                                existe.firmaDirector = respuesta.Equals(2) ? "RECHAZADO" : string.Format("--{0}|{1:MMddyyyy}|{2:HHmm}|{3}|{4}--", conciliacionID, ahora, ahora, respuesta, idUsuario);
                                existe.estatus = autorizaConciliacionHorometros.pendienteGerente != 0 && autorizaConciliacionHorometros.pendienteAdmin != 0 && autorizaConciliacionHorometros.pendienteDirector != 0 ? 1 : 0;
                                autorizaConciliacionHorometros.autorizando = 4;
                                _context.SaveChanges();
                                tblM_CapEncConciliacionHorometros existeConciliacion = _context.tblM_CapEncConciliacionHorometros.Find(autorizaConciliacionHorometros.conciliacionID);
                                existeConciliacion.estatus = existe2.estatus;
                                _context.SaveChanges();

                            }
                        }
                        else
                        {
                            setOrUpdateAlerta(autorizaConciliacionHorometros.autorizaGerenteID, autorizaConciliacionHorometros.autorizaDirector, false, conciliacionID, 1);
                            SendCorreo(autorizaConciliacionHorometros.conciliacionID, autorizaConciliacionHorometros);
                        }

                        setOrUpdateAlerta(autorizaConciliacionHorometros.autorizaAdmin, autorizaConciliacionHorometros.autorizaGerenteID, false, conciliacionID, 2);
                        if (respuesta == 2)
                        {
                            tblM_CapEncConciliacionHorometros existeConciliacion = _context.tblM_CapEncConciliacionHorometros.Find(autorizaConciliacionHorometros.conciliacionID);

                            existeConciliacion.estatus = respuesta;
                            _context.SaveChanges();
                        }

                    }
                    return true;
                }
                else if (autorizaConciliacionHorometros.autorizaDirector.Equals(idUsuario) && autorizaConciliacionHorometros.autorizando == 3)
                {
                    if (autorizaConciliacionHorometros.autorizaDirector.Equals(idUsuario))
                    {
                        tblM_AutorizaConciliacionHorometros existe = _context.tblM_AutorizaConciliacionHorometros.Find(autorizaConciliacionHorometros.id);
                        existe.pendienteDirector = respuesta;
                        existe.firmaDirector = respuesta.Equals(2) ? "RECHAZADO" : string.Format("--{0}|{1:MMddyyyy}|{2:HHmm}|{3}|{4}--", conciliacionID, ahora, ahora, respuesta, idUsuario);
                        existe.estatus = autorizaConciliacionHorometros.pendienteGerente != 0 && autorizaConciliacionHorometros.pendienteAdmin != 0 && autorizaConciliacionHorometros.pendienteDirector != 0 ? 1 : 0;
                        autorizaConciliacionHorometros.autorizando = 4;
                        _context.SaveChanges();
                        tblM_CapEncConciliacionHorometros existeConciliacion = _context.tblM_CapEncConciliacionHorometros.Find(autorizaConciliacionHorometros.conciliacionID);
                        existeConciliacion.estatus = existe.estatus;
                        _context.SaveChanges();
                        setOrUpdateAlerta(autorizaConciliacionHorometros.autorizaGerenteID, autorizaConciliacionHorometros.autorizaDirector, false, conciliacionID, 2);

                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void setOrUpdateAlerta(int usuarioEnvia, int usuarioRecibe, bool visto, int conciliacionID, int tipoAlerta)
        {
            tblP_Alerta objAlerta = new tblP_Alerta();
            var _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblP_Alerta>();
            switch (tipoAlerta)
            {
                case 1:
                    objAlerta.moduloID = 103;
                    objAlerta.msj = "Conciliacion Pendiente de Autorizar folio " + conciliacionID;
                    objAlerta.url = "/conciliacion/Autoriza/";
                    objAlerta.objID = conciliacionID;
                    objAlerta.tipoAlerta = 2;
                    objAlerta.sistemaID = 1;
                    objAlerta.userEnviaID = usuarioEnvia;
                    objAlerta.userRecibeID = usuarioRecibe;
                    _objectSet.AddObject(objAlerta);
                    _context.SaveChanges();

                    break;
                case 2:
                    var objAlertaTemp = _context.tblP_Alerta.FirstOrDefault(x => x.objID == conciliacionID && x.objID == conciliacionID && x.moduloID == 103 && x.userRecibeID == usuarioRecibe);

                    if (objAlertaTemp != null)
                    {
                        tblP_Alerta existe = _context.tblP_Alerta.Find(objAlertaTemp.id);
                        existe.visto = true;
                        _context.SaveChanges();

                    }
                    break;
                case 3:
                    objAlerta.moduloID = 104;
                    objAlerta.msj = "Caratula Pendiente de Autorizar folio " + conciliacionID;
                    objAlerta.url = "/conciliacion/AutorizacionCaratula/";
                    objAlerta.objID = conciliacionID;
                    objAlerta.tipoAlerta = 2;
                    objAlerta.sistemaID = 1;
                    objAlerta.userEnviaID = usuarioEnvia;
                    objAlerta.userRecibeID = usuarioRecibe;

                    _objectSet.AddObject(objAlerta);
                    _context.SaveChanges();
                    break;
                case 4:
                    {
                        var objAlertaTemp2 = _context.tblP_Alerta.FirstOrDefault(x => x.objID == conciliacionID && x.objID == conciliacionID && x.moduloID == 104 && x.userRecibeID == usuarioRecibe);
                        if (objAlertaTemp2 != null)
                        {
                            tblP_Alerta existe = _context.tblP_Alerta.Find(objAlertaTemp2.id);
                            existe.visto = true;
                            _context.SaveChanges();
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        public void setCorreo(int conciliacionID)
        {

            List<adjuntoCorreoDTO> lstAdjuntoCorreo = new List<adjuntoCorreoDTO>();
            // List<byte[]> lista = new List<byte[]>();

            var objConciliacion = _context.tblM_CapEncConciliacionHorometros.FirstOrDefault(x => x.id == conciliacionID);
            var objCC = _context.tblP_CC.FirstOrDefault(x => x.id == objConciliacion.centroCostosID);
            tblM_AutorizaConciliacionHorometros getAutorizantes = _context.tblM_AutorizaConciliacionHorometros.FirstOrDefault(x => x.conciliacionID == conciliacionID);
            //DateTime dateNew = new DateTime();
            //dateNew = Convert.ToDateTime("01/01/" + objConciliacion.anio);
            //var objFechas = objConciliacion.esQuincena ? GlobalUtils.GetQuincenas(objConciliacion.anio).FirstOrDefault(x => x.Value == objConciliacion.fechaID) : GetFechas(dateNew).FirstOrDefault(x => x.Value == objConciliacion.fechaID);

            string ac = objCC.areaCuenta;
            string nombre = objCC.descripcion;
            string periodo = objConciliacion.fechaInicio.ToShortDateString() + " - " + objConciliacion.fechaFin.ToShortDateString();
            string fechaInicio = objConciliacion.fechaInicio.ToShortDateString();
            string fechaFin = objConciliacion.fechaFin.ToShortDateString();


            var ListaConciliacinoRaw = _context.tblM_CapConciliacionHorometros.Where(c => c.idEncCaratula == conciliacionID).ToList();

            var data = (List<byte[]>)HttpContext.Current.Session["downloadPDF"];

            lstAdjuntoCorreo.Add(
             GlobalUtils.setAdjunto(data.FirstOrDefault(), ".pdf", "Conciliacion de Horometros del periodo " + periodo + " " + nombre)
            );

            lstAdjuntoCorreo.Add(
            GlobalUtils.setAdjunto(getExcelConciliacion(ac, nombre, fechaInicio, fechaFin, conciliacionID, ListaConciliacinoRaw).ToArray(), ".xlsx", "Conciliacion de Horometros del periodo " + periodo + " " + nombre)
            );

            string observacion = (objCC.cc + " " + objCC.descripcion + " Periodo " + fechaInicio + " al " + fechaFin).ToString();

            var tempConciliacion = ListaConciliacinoRaw.Where(x => x.total > 0).ToList();

            while (tempConciliacion.Count > 0)
            {
                var TempFile = tempConciliacion.Take(20).ToList();

                lstAdjuntoCorreo.Add(
                GlobalUtils.setAdjunto(getLayautExcel(observacion, 0, 0, objCC.area, objCC.cuenta, TempFile).ToArray(), ".xlsx", observacion)
                );
                foreach (var item in TempFile)
                {
                    tempConciliacion.Remove(item);
                }
            }




            List<string> correos = new List<string>();
            //var listaUsuarios = _context.tblP_EnvioCorreos.Where(x => x.moduloID == 4218).ToList();
            //foreach (var item in listaUsuarios)
            //{
            //    if (item.centroCostosPermiso)
            //    {
            //        correos.Add(item.usuario.correo);
            //    }
            //    else
            //    {
            //        var addCorreo = _context.tblP_CC_Usuario.FirstOrDefault(x => x.usuarioID == item.usuarioID && x.cc == objCC.areaCuenta);
            //        if (addCorreo != null)
            //        {
            //            correos.Add(item.usuario.correo);
            //        }
            //    }
            //}

            try
            {
                var listaGerentes = (from a in _context.tblP_Autoriza
                                     join b in _context.tblP_CC_Usuario on
                                     a.cc_usuario_ID equals b.id
                                     where b.cc == objCC.areaCuenta && (a.perfilAutorizaID == 1 || a.perfilAutorizaID == 5)
                                     select a.usuario.correo).ToList();
                correos.AddRange(listaGerentes);

            }
            catch (Exception)
            {

            }
            //  correos = new List<string>();
            //correos.Add("c.coronado@construplan.com.mx");

            // correos.Add("e.encinas@construplan.com.mx");
            var correosAuxiliares = (from a in _context.tblP_Autoriza.ToList()
                                     join u in _context.tblP_Usuario.ToList()
                                     on a.usuarioID equals u.id
                                     join ccu in _context.tblP_CC_Usuario.ToList()
                                     on a.cc_usuario_ID equals ccu.id
                                     where (a.perfilAutorizaID == 8 || a.perfilAutorizaID == 9) && ccu.cc == objCC.areaCuenta
                                     select u.correo).ToList();
            correos.AddRange(correosAuxiliares);
            correos.Add("ana.mendez@construplan.com.mx");
            correos.Add("ruth.vargas@construplan.com.mx");
            correos.Add("valeria.gomez@construplan.com.mx");
            //correos.Add("laura.rodriguez@construplan.com.mx");
            correos.Add("e.encinas@construplan.com.mx");
            if (objCC != null && objCC.areaCuenta == "2-1") correos.Add("manolo.anton@construplan.com.mx");
            try
            {
                correos.Remove("mayra.soto@construplan.com.mx");
                correos.Remove("martha.cheno@construplan.com.mx");
                correos.Remove("b.valenzuela@construplan.com.mx");
                correos.Remove("juan.cecco@construplan.com.mx");
                correos.Remove("d.laborin@construplan.com.mx");

                //correos.Remove("e.encinas@construplan.com.mx");
            }
            catch (Exception)
            {
                
            }

            
            string adjuntoCorreo = @"
                Buen dia.<br>
                
                Se realizó la conciliacion del periodo <b>" + periodo + @"</b> de la Obra <b>" + nombre + "</b><br/><br/>";

            if (getAutorizantes.firmaDirector != "RECHAZADO")
            {
                AlmacenDAO alm = new AlmacenDAO();
                var tipocc = objCC.area == 9 ? TipoCentroCostoEnum.ADMINISTRATIVO : TipoCentroCostoEnum.OPERATIVO;
                var precio = ListaConciliacinoRaw.Sum(x => x.total);
                var moneda = ListaConciliacinoRaw.FirstOrDefault().moneda;
                var tc = moneda == 1 ? 1 : 0;

                if (objCC.cc != "166")
                {
                    var datos = alm.crearRequisicionCompraConciliacion(objCC.cc, "PERIODO " + periodo, tipocc, precio, moneda, tc, 16);
                    adjuntoCorreo += @"Requisición: " + datos.cc + "-" + datos.numeroRequisicion + "<br/>";
                    adjuntoCorreo += @"Compra: " + datos.cc + "-" + datos.numeroCompra + "<br/><br/>";
                }
            }
//            string adjuntoCorreo = @"
//                Buen dia.<br>
//                
//                Se realizó la conciliacion del periodo <b>" + periodo + @"</b> de la Obra <b>" + nombre + "</b>";

            correos.Remove("d.laborin@construplan.com.mx");

            if (getAutorizantes.autorizaGerenteID == 3337)
            {
                correos.Add("d.laborin@construplan.com.mx");
            }
            else
            {
                correos.Remove("d.laborin@construplan.com.mx");
            }
            string asuntoCorreo = setCorreoAutorizadores(adjuntoCorreo, getAutorizantes);
            GlobalUtils.sendMailWithFiles("Conciliacion de Horometros del periodo " + periodo + " " + nombre, asuntoCorreo, correos, lstAdjuntoCorreo);


        }
        public void setReenviarCorreo(int conciliacionID)
        {

            List<adjuntoCorreoDTO> lstAdjuntoCorreo = new List<adjuntoCorreoDTO>();
            // List<byte[]> lista = new List<byte[]>();

            var objConciliacion = _context.tblM_CapEncConciliacionHorometros.FirstOrDefault(x => x.id == conciliacionID);
            var objCC = _context.tblP_CC.FirstOrDefault(x => x.id == objConciliacion.centroCostosID);
            tblM_AutorizaConciliacionHorometros getAutorizantes = _context.tblM_AutorizaConciliacionHorometros.FirstOrDefault(x => x.conciliacionID == conciliacionID);
            //DateTime dateNew = new DateTime();
            //dateNew = Convert.ToDateTime("01/01/" + objConciliacion.anio);
            //var objFechas = objConciliacion.esQuincena ? GlobalUtils.GetQuincenas(objConciliacion.anio).FirstOrDefault(x => x.Value == objConciliacion.fechaID) : GetFechas(dateNew).FirstOrDefault(x => x.Value == objConciliacion.fechaID);

            string ac = objCC.areaCuenta;
            string nombre = objCC.descripcion;
            string periodo = objConciliacion.fechaID +" del "+objConciliacion.fechaInicio.ToShortDateString() + " - " + objConciliacion.fechaFin.ToShortDateString();
            string fechaInicio = objConciliacion.fechaInicio.ToShortDateString();
            string fechaFin = objConciliacion.fechaFin.ToShortDateString();


            var ListaConciliacinoRaw = _context.tblM_CapConciliacionHorometros.Where(c => c.idEncCaratula == conciliacionID).ToList();

            var data = (List<byte[]>)HttpContext.Current.Session["downloadPDF"];

            lstAdjuntoCorreo.Add(
             GlobalUtils.setAdjunto(data.FirstOrDefault(), ".pdf", "Conciliacion de Horometros del periodo " + periodo + " " + nombre)
            );

            lstAdjuntoCorreo.Add(
            GlobalUtils.setAdjunto(getExcelConciliacion(ac, nombre, fechaInicio, fechaFin, conciliacionID, ListaConciliacinoRaw).ToArray(), ".xlsx", "Conciliacion de Horometros del periodo " + periodo + " " + nombre)
            );

            string observacion = (objCC.cc + " " + objCC.descripcion + " Periodo " + fechaInicio + " al " + fechaFin).ToString();

            var tempConciliacion = ListaConciliacinoRaw.Where(x => x.total > 0).ToList();

            while (tempConciliacion.Count > 0)
            {
                var TempFile = tempConciliacion.Take(20).ToList();

                lstAdjuntoCorreo.Add(
                GlobalUtils.setAdjunto(getLayautExcel(observacion, 0, 0, objCC.area, objCC.cuenta, TempFile).ToArray(), ".xlsx", observacion)
                );
                foreach (var item in TempFile)
                {
                    tempConciliacion.Remove(item);
                }
            }




            List<string> correos = new List<string>();
            var listaUsuarios = _context.tblP_EnvioCorreos.Where(x => x.moduloID == 4218).ToList();
            foreach (var item in listaUsuarios)
            {
                if (item.centroCostosPermiso)
                {
                    correos.Add(item.usuario.correo);
                }
                else
                {
                    var addCorreo = _context.tblP_CC_Usuario.FirstOrDefault(x => x.usuarioID == item.usuarioID && x.cc == objCC.areaCuenta);
                    if (addCorreo != null)
                    {
                        correos.Add(item.usuario.correo);
                    }
                }
            }

            try
            {
                var listaGerentes = (from a in _context.tblP_Autoriza
                                     join b in _context.tblP_CC_Usuario on
                                     a.cc_usuario_ID equals b.id
                                     where b.cc == objCC.areaCuenta && (a.perfilAutorizaID == 1 || a.perfilAutorizaID == 5)
                                     select a.usuario.correo).ToList();
                correos.AddRange(listaGerentes);

            }
            catch (Exception)
            {
                correos.Add("aaron.romero@construplan.com.mx");
            }
            correos = new List<string>();
            correos.Add("c.coronado@construplan.com.mx");

            // correos.Add("e.encinas@construplan.com.mx");
            var correosAuxiliares = (from a in _context.tblP_Autoriza.ToList()
                                     join u in _context.tblP_Usuario.ToList()
                                     on a.usuarioID equals u.id
                                     join ccu in _context.tblP_CC_Usuario.ToList()
                                     on a.cc_usuario_ID equals ccu.id
                                     where (a.perfilAutorizaID == 8 || a.perfilAutorizaID == 9) && ccu.cc == objCC.areaCuenta
                                     select u.correo).ToList();
            correos.AddRange(correosAuxiliares);
            correos.Add("ana.mendez@construplan.com.mx");
            correos.Add("ruth.vargas@construplan.com.mx");
            correos.Add("valeria.gomez@construplan.com.mx");
            //correos.Add("laura.rodriguez@construplan.com.mx");
            correos.Add("e.encinas@construplan.com.mx");
            try
            {
                correos.Remove("mayra.soto@construplan.com.mx");
                correos.Remove("martha.cheno@construplan.com.mx");
                correos.Remove("b.valenzuela@construplan.com.mx");
                correos.Remove("juan.cecco@construplan.com.mx");
                correos.Remove("d.laborin@construplan.com.mx");

                correos.Remove("e.encinas@construplan.com.mx");
            }
            catch (Exception)
            {

            }

            if (getAutorizantes.autorizaGerenteID == 3337)
            {
                correos.Add("d.laborin@construplan.com.mx");
            }
            else
            {
                correos.Remove("d.laborin@construplan.com.mx");
            }

            if (objCC != null && objCC.areaCuenta == "2-1") correos.Add("manolo.anton@construplan.com.mx");

            List<string> correosTemp = new List<string>();
            //correosTemp.Add("martin.valle@construplan.com.mx");
            //correosTemp.Add("angel.devora@construplan.com.mx");
            //correosTemp.Add("e.salazar@construplan.com.mx");
            correosTemp.Add("ana.mendez@construplan.com.mx");
            //correosTemp.Add("e.encinas@construplan.com.mx");

            //AlmacenDAO alm = new AlmacenDAO();
            //var tipocc = objCC.area == 9 ? TipoCentroCostoEnum.ADMINISTRATIVO : TipoCentroCostoEnum.OPERATIVO;
            //var precio = ListaConciliacinoRaw.Sum(x => x.total);
            //var moneda = ListaConciliacinoRaw.FirstOrDefault().moneda;
            //var tc = moneda == 1 ? 1 : 0;
            //var datos = alm.crearRequisicionCompraConciliacion(objCC.cc, "PERIODO " + periodo, tipocc, precio, moneda, tc, 16);

            string adjuntoCorreo = @"
                Buen dia.<br>
                
                Se realizó la conciliacion del periodo <b>" + periodo + @"</b> de la Obra <b>" + nombre + "</b><br/><br/>";
            //adjuntoCorreo += @"Requisición: " + datos.cc + "-" + datos.numeroRequisicion + "<br/>";
            //adjuntoCorreo += @"Compra: " + datos.cc + "-" + datos.numeroCompra + "<br/><br/>";

            string asuntoCorreo = setCorreoAutorizadores(adjuntoCorreo, getAutorizantes);
            GlobalUtils.sendMailWithFiles("Conciliacion de Horometros del periodo " + periodo + " " + nombre, asuntoCorreo, correosTemp, lstAdjuntoCorreo);



        }

        public MemoryStream getLayautExcel(string observaciones, decimal tipoCambio, int moneda, int area, int cuenta, List<tblM_CapConciliacionHorometros> ListaConciliacinoRaw)
        {

            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Layaout");

                var tempobj = ListaConciliacinoRaw.FirstOrDefault();
                if (tempobj != null)
                {
                    moneda = tempobj.moneda;
                }


                CadenaProductivaDAO getD = new CadenaProductivaDAO();


                worksheet.Cells["A1"].Style.Font.SetFromFont(new Font("Arial Bold", 16));
                worksheet.Cells["A1"].Style.Font.Color.SetColor(Color.Black);
                worksheet.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["A1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(146, 208, 80));

                worksheet.Cells["A1"].Value = "Pedido";

                worksheet.Cells["A2"].Value = "Num Cliente";
                worksheet.Cells["A3"].Value = "Sucursal";
                worksheet.Cells["A4"].Value = "Requisicion";
                worksheet.Cells["A5"].Value = "Num Vendedor";
                worksheet.Cells["A6"].Value = "Sub Total";
                worksheet.Cells["A7"].Value = "IVA";
                worksheet.Cells["A8"].Value = "Total";
                worksheet.Cells["A9"].Value = "% Iva";
                worksheet.Cells["A10"].Value = "Descuento";
                worksheet.Cells["A11"].Value = "Condiciones Entrega";
                worksheet.Cells["A12"].Value = "Tipo de Flete";
                worksheet.Cells["A13"].Value = "observaciones";
                worksheet.Cells["A14"].Value = "Tipo de Pedido";
                worksheet.Cells["A15"].Value = "Centro de Costos";
                worksheet.Cells["A16"].Value = "Tipo de movimiento TM";
                worksheet.Cells["A17"].Value = "Compañía de sucursal (cia_Sucursal)";
                worksheet.Cells["A18"].Value = "Tipo Cambio";
                worksheet.Cells["A19"].Value = "Retención";
                worksheet.Cells["A20"].Value = "Moneda";



                worksheet.Cells["B3"].Value = 1;
                worksheet.Cells["B4"].Value = "reqoc";
                worksheet.Cells["B5"].Value = 1;
                worksheet.Cells["B6"].Value = 1500;
                worksheet.Cells["B7"].Value = 16;
                worksheet.Cells["B8"].Value = 0;
                worksheet.Cells["B9"].Value = 16;
                worksheet.Cells["B10"].Value = 0;
                worksheet.Cells["B11"].Value = 30;
                worksheet.Cells["B12"].Value = "L";
                worksheet.Cells["B13"].Value = observaciones;
                worksheet.Cells["B14"].Value = "M";
                worksheet.Cells["B15"].Value = "001'";
                worksheet.Cells["B16"].Value = 1;
                worksheet.Cells["B17"].Value = 1;

                worksheet.Cells["B19"].Value = 0;
                worksheet.Cells["B20"].Value = moneda;

                /*Titulo 2*/
                worksheet.Cells["A21"].Style.Font.SetFromFont(new Font("Arial Bold", 16));
                worksheet.Cells["A21"].Style.Font.Color.SetColor(Color.Black);
                worksheet.Cells["A21"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A21"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["A21"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(146, 208, 80));

                worksheet.Cells["A21"].Value = "Remision";
                worksheet.Cells["A22"].Value = "Transporte";
                worksheet.Cells["A23"].Value = "Talón";
                worksheet.Cells["A24"].Value = "Consignado";
                worksheet.Cells["A25"].Value = "Observaciones Remision";

                worksheet.Cells["B22"].Value = "CARRO";
                worksheet.Cells["B23"].Value = 1;
                worksheet.Cells["B24"].Value = 1;
                worksheet.Cells["B25"].Value = "NADA";

                /*Titulo 3*/
                worksheet.Cells["A26"].Style.Font.SetFromFont(new Font("Arial Bold", 16));
                worksheet.Cells["A26"].Style.Font.Color.SetColor(Color.Black);
                worksheet.Cells["A26"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A26"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["A26"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(146, 208, 80));
                worksheet.Cells["A26"].Value = "Factura";

                worksheet.Cells["A27"].Value = "CFD Serie";
                worksheet.Cells["A28"].Value = "Area";
                worksheet.Cells["A29"].Value = "Cuenta";
                worksheet.Cells["A30"].Value = "Metodo Pago";
                worksheet.Cells["A31"].Value = "Regimen Fiscal";
                worksheet.Cells["A32"].Value = "Tipo Clase";
                worksheet.Cells["A33"].Value = "gsdb";
                worksheet.Cells["A34"].Value = "asn";
                worksheet.Cells["A35"].Value = "Cuenta Pago";

                worksheet.Cells["B27"].Value = "A";
                worksheet.Cells["B28"].Value = area;
                worksheet.Cells["B29"].Value = cuenta;
                worksheet.Cells["B30"].Value = 4;
                worksheet.Cells["B31"].Value = 1;
                worksheet.Cells["B32"].Value = 5;
                worksheet.Cells["B33"].Value = "";
                worksheet.Cells["B34"].Value = "";
                worksheet.Cells["B35"].Value = cuenta;


                int inicio = 37;

                worksheet.Cells["A36"].Value = "Detalle del pedido";
                worksheet.Cells["B36"].Value = "insumo";
                worksheet.Cells["C36"].Value = "cantidad";
                worksheet.Cells["D36"].Value = "precio";
                worksheet.Cells["E36"].Value = "unidad";
                worksheet.Cells["F36"].Value = "porcent_descuento";
                worksheet.Cells["G36"].Value = "fec_entrega";
                worksheet.Cells["H36"].Value = "muestra";
                worksheet.Cells["I36"].Value = "porcen_iva_partida";
                worksheet.Cells["J36"].Value = "linea";
                worksheet.Cells["K36"].Value = "cc";

                var rawEconomicos = (from c in ListaConciliacinoRaw
                                     join m in _context.tblM_CatMaquina.ToList()
                                     on c.noEconomicoID equals m.id
                                     select m).ToList();

                string consultaWhere = "";

                foreach (var item in rawEconomicos.Select(x => x.noEconomico).ToList())
                {
                    consultaWhere += "'" + item + "'" + ",";

                }

                var consulta = @"SELECT cc as value, descripcion as text FROM DBA.cc where descripcion in (" + consultaWhere.TrimEnd(',') + ")";
                var ccEnkontrolLista = (List<ComboDTO>)ContextArrendadora.Where(consulta).ToObject<List<ComboDTO>>();



                foreach (var itemConciliacion in ListaConciliacinoRaw)
                {
                    var economico = rawEconomicos.Where(x => x.id == itemConciliacion.noEconomicoID).FirstOrDefault();

                    int cliente = 0;
                    var noEkontrol = ccEnkontrolLista.Where(x => x.Text == economico.noEconomico);
                    ///  var economicoEkontrol = 

                    int insumo = 0;
                    int insumoOverhaul = 0;

                    /*Si es nacional = 1*/
                    if (itemConciliacion.moneda == 1)
                    {
                        cliente = 504;
                        worksheet.Cells["B18"].Value = 1;
                        //if (economico.CargoEmpresa == 1)
                        //{
                        switch(economico.empresa)
                        {
                            case ((int)EmpresaEnum.Construplan):
                                insumo = 9010009;
                                break;
                            case ((int)EmpresaEnum.Arrendadora):
                                insumo = 9010005;
                                break;
                            default:
                                insumo = 9010009;
                                break;
                        }
                        insumoOverhaul = 9010017;
                    }
                    else
                    {
                        cliente = 9076;
                        worksheet.Cells["B18"].Value = getD.getDolarDelDia(DateTime.Now);
                        switch (economico.empresa)
                        {
                            case ((int)EmpresaEnum.Construplan):
                                insumo = 9010010;
                                break;
                            case ((int)EmpresaEnum.Arrendadora):
                                insumo = 9010008;
                                break;
                            default:
                                insumo = 9010009;
                                break;
                        }
                        //if (economico.CargoEmpresa == 1)
                        //{
                        //    insumo = 9010010;
                        //}
                        //else
                        //{
                        //    insumo = 9010008;
                        //}
                        insumoOverhaul = 9010018;
                    }
                    worksheet.Cells["B2"].Value = cliente;

                    decimal costoInfo = 0;
                    if (itemConciliacion.unidad == 1)
                    {
                        costoInfo = itemConciliacion.horometroEfectivo;
                    }
                    else
                    {
                        if (itemConciliacion.costo != 0)
                        {
                            costoInfo = (itemConciliacion.total / itemConciliacion.costo);
                        }
                        else
                        {
                            costoInfo = 0;// (itemConciliacion.total / itemConciliacion.costo);
                        }
                    }
                    if (costoInfo != 0)
                    {
                        if (itemConciliacion.overhaul == 0)
                        {
                            worksheet.Cells[inicio, 1].Value = 1;
                            worksheet.Cells[inicio, 2].Value = insumo;
                            worksheet.Cells[inicio, 3].Value = costoInfo;
                            worksheet.Cells[inicio, 4].Value = itemConciliacion.costo;
                            worksheet.Cells[inicio, 5].Value = itemConciliacion.moneda == 1 ? "PESOS" : "DOLARES";
                            worksheet.Cells[inicio, 6].Value = 0;
                            worksheet.Cells[inicio, 7].Value = "2018-08-08";
                            worksheet.Cells[inicio, 8].Value = "N";
                            worksheet.Cells[inicio, 9].Value = 0;
                            worksheet.Cells[inicio, 10].Value = economico.noEconomico + ":" + economico.modeloEquipo.descripcion + ":" + economico.grupoMaquinaria.descripcion + ":" + economico.noSerie;
                            worksheet.Cells[inicio, 11].Value = noEkontrol.FirstOrDefault() != null ? noEkontrol.FirstOrDefault().Value : "";
                            inicio++;

                        }
                        else
                        {


                            worksheet.Cells[inicio, 1].Value = 1;
                            worksheet.Cells[inicio, 2].Value = insumoOverhaul;
                            worksheet.Cells[inicio, 3].Value = costoInfo;
                            worksheet.Cells[inicio, 4].Value = itemConciliacion.overhaul;
                            worksheet.Cells[inicio, 5].Value = itemConciliacion.moneda == 1 ? "PESOS" : "DOLARES";
                            worksheet.Cells[inicio, 6].Value = 0;
                            worksheet.Cells[inicio, 7].Value = "2018-08-08";
                            worksheet.Cells[inicio, 8].Value = "N";
                            worksheet.Cells[inicio, 9].Value = 0;
                            worksheet.Cells[inicio, 10].Value = economico.noEconomico + ":" + economico.modeloEquipo.descripcion + ":" + economico.grupoMaquinaria.descripcion + ":" + economico.noSerie;
                            worksheet.Cells[inicio, 11].Value = noEkontrol.FirstOrDefault() != null ? noEkontrol.FirstOrDefault().Value : "";
                            inicio++;


                            worksheet.Cells[inicio, 1].Value = 1;
                            worksheet.Cells[inicio, 2].Value = insumo;
                            worksheet.Cells[inicio, 3].Value = costoInfo;
                            worksheet.Cells[inicio, 4].Value = itemConciliacion.costo - itemConciliacion.overhaul;
                            worksheet.Cells[inicio, 5].Value = itemConciliacion.moneda == 1 ? "PESOS" : "DOLARES";
                            worksheet.Cells[inicio, 6].Value = 0;
                            worksheet.Cells[inicio, 7].Value = "2018-08-08";
                            worksheet.Cells[inicio, 8].Value = "N";
                            worksheet.Cells[inicio, 9].Value = 0;
                            worksheet.Cells[inicio, 10].Value = economico.noEconomico + ":" + economico.modeloEquipo.descripcion + ":" + economico.grupoMaquinaria.descripcion + ":" + economico.noSerie;
                            worksheet.Cells[inicio, 11].Value = noEkontrol.FirstOrDefault() != null ? noEkontrol.FirstOrDefault().Value : "";
                            inicio++;


                        }
                    }


                }
                package.Compression = CompressionLevel.BestSpeed;

                using (var exportData = new MemoryStream())
                {
                    package.SaveAs(exportData);
                    //lista.Add(exportData.ToArray());

                    return exportData;
                }

            }

        }

        public MemoryStream getExcelConciliacion(string ac, string nombre, string fechaInicio, string fechaFin, int conciliacionID, List<tblM_CapConciliacionHorometros> ListaConciliacinoRaw)
        {
            List<byte[]> lista = new List<byte[]>();
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("CONCILIACION");
                using (ExcelRange r = worksheet.Cells["D2:G2"])
                {
                    r.Merge = true;
                    r.Style.Font.SetFromFont(new Font("Arial Bold", 16));
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    //  r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                }
                worksheet.Cells[2, 4].Value = "CONCILIACION HOROMETROS";
                worksheet.Cells[4, 4].Value = "PROYECTO";
                worksheet.Cells[4, 5].Value = ac + " - " + nombre;
                worksheet.Cells[3, 10].Value = "DEL:";
                worksheet.Cells[4, 10].Value = "AL:";

                worksheet.Cells[3, 11].Value = fechaInicio;
                worksheet.Cells[4, 11].Value = fechaFin;

                ExcelRange cols = worksheet.Cells["A6:N6"];
                cols.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cols.Style.Fill.BackgroundColor.SetColor(1, 230, 127, 34);

                // var ListaConciliacinoRaw = _context.tblM_CapConciliacionHorometros.Where(c => c.idEncCaratula == conciliacionID).ToList();
                int inicio = 7;
                for (int i = 0; i < ListaConciliacinoRaw.Count; i++)
                {

                    worksheet.Cells[inicio, 1].Value = ListaConciliacinoRaw[i].numero + 1;
                    worksheet.Cells[inicio, 2].Value = ListaConciliacinoRaw[i].economico;
                    worksheet.Cells[inicio, 3].Value = ListaConciliacinoRaw[i].descripcion;
                    worksheet.Cells[inicio, 4].Value = ListaConciliacinoRaw[i].modelo;
                    worksheet.Cells[inicio, 5].Value = ListaConciliacinoRaw[i].horometroInicial;
                    worksheet.Cells[inicio, 6].Value = ListaConciliacinoRaw[i].horometroFinal;
                    worksheet.Cells[inicio, 7].Value = ListaConciliacinoRaw[i].horometroEfectivo;
                    worksheet.Cells[inicio, 8].Value = ListaConciliacinoRaw[i].unidad == 1 ? "HORAS" : "DíA";
                    worksheet.Cells[inicio, 9].Value = ListaConciliacinoRaw[i].costo;
                    worksheet.Cells[inicio, 10].Value = ListaConciliacinoRaw[i].total;
                    worksheet.Cells[inicio, 11].Value = (ListaConciliacinoRaw[i].total == 0) ? 0 : ListaConciliacinoRaw[i].total - ListaConciliacinoRaw[i].overhaul;
                    worksheet.Cells[inicio, 12].Value = (ListaConciliacinoRaw[i].total == 0) ? 0 : ListaConciliacinoRaw[i].overhaul;
                    worksheet.Cells[inicio, 13].Value = ListaConciliacinoRaw[i].idEmpresa == 1 ? "CONSTRUPLAN" : "ARRENDADORA";
                    worksheet.Cells[inicio, 14].Value = ListaConciliacinoRaw[i].observaciones;

                    inicio++;
                }


                worksheet.Cells[inicio + 1, 10].Formula = string.Format("Sum({0})", new ExcelAddress(7, 10, inicio, 10).Address);
                worksheet.Cells[inicio + 1, 10].Style.Font.Bold = true;

                worksheet.Cells[ListaConciliacinoRaw.Count + 1, 9, ListaConciliacinoRaw.Count + 1, 10].Style.Numberformat.Format = "[$$-409]#,##0.##0";

                worksheet.Cells["A6"].Value = "No.";
                worksheet.Cells["B6"].Value = "Económico";
                worksheet.Cells["C6"].Value = "Descripción";
                worksheet.Cells["D6"].Value = "Modelo";
                worksheet.Cells["E6"].Value = "HI";
                worksheet.Cells["F6"].Value = "HF";
                worksheet.Cells["G6"].Value = "HE";
                worksheet.Cells["H6"].Value = "UNIDAD";
                worksheet.Cells["I6"].Value = "Costo/hora (M.N.)";
                worksheet.Cells["J6"].Value = "Costo total (M.N.)";
                worksheet.Cells["K6"].Value = "Cargo Renta";
                worksheet.Cells["L6"].Value = "Cargo Overhaul";
                worksheet.Cells["M6"].Value = "CARGO: CONSTRUPLAN / LA ARRENDADORA";
                worksheet.Cells["N6"].Value = "OBSERVACIONES";
                worksheet.View.FreezePanes(1, 1);
                worksheet.Cells["A6:N6"].AutoFilter = true;
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                package.Compression = CompressionLevel.BestSpeed;

                using (var exportData = new MemoryStream())
                {
                    package.SaveAs(exportData);
                    //lista.Add(exportData.ToArray());

                    return exportData;
                }

            }
            // return lista;
        }

        private void SendCorreo(int idConciliacon, tblM_AutorizaConciliacionHorometros objAutorizacion)
        {
            var objConciliacion = _context.tblM_CapEncConciliacionHorometros.FirstOrDefault(x => x.id == idConciliacon);
            //DateTime dateNew = new DateTime();
            var objCC = _context.tblP_CC.FirstOrDefault(x => x.id == objConciliacion.centroCostosID);
            //dateNew = Convert.ToDateTime("01/01/" + objConciliacion.anio);
            //var objFechas = objConciliacion.esQuincena ? GlobalUtils.GetQuincenas(objConciliacion.anio).FirstOrDefault(x => x.Value == objConciliacion.fechaID) : GetFechas(dateNew).FirstOrDefault(x => x.Value == objConciliacion.fechaID);
            string ac = objCC.areaCuenta;
            string nombre = objCC.descripcion;
            string periodo = objConciliacion.fechaInicio.ToShortDateString() + " - " + objConciliacion.fechaFin.ToShortDateString();

            List<string> correos = new List<string>();

            var correo1 = _context.tblP_Usuario.FirstOrDefault(x => x.id == objAutorizacion.autorizaAdmin);
            var correo2 = _context.tblP_Usuario.FirstOrDefault(x => x.id == objAutorizacion.autorizaGerenteID);
            var correo3 = _context.tblP_Usuario.FirstOrDefault(x => x.id == objAutorizacion.autorizaDirector);



            var correosAuxiliares = (from a in _context.tblP_Autoriza.ToList()
                                     join u in _context.tblP_Usuario.ToList()
                                     on a.usuarioID equals u.id
                                     join ccu in _context.tblP_CC_Usuario.ToList()
                                     on a.cc_usuario_ID equals ccu.id
                                     where (a.perfilAutorizaID == 8 || a.perfilAutorizaID == 9) && ccu.cc == objCC.areaCuenta
                                     select u.correo).ToList();

            correos.AddRange(correosAuxiliares);
            correos.Add(correo1.correo);
            correos.Add(correo2.correo);

            if (objAutorizacion.autorizando == 3)
            {
                var excepcionesCorreo = _context.tblP_PermisosAutorizaCorreo.Where(x => x.estatus && x.permiso == 3).ToList();

                List<int> excepcionesCorreoIDs = new List<int>();

                if (excepcionesCorreo.Count > 0)
                {
                    excepcionesCorreoIDs.AddRange(excepcionesCorreo.Select(x => x.usuarioID));

                    foreach (var item in excepcionesCorreoIDs)
                    {
                        var addCorreo = _context.tblP_Usuario.FirstOrDefault(x => x.id == objAutorizacion.autorizaDirector);
                        correos.Add(addCorreo.correo);

                    }
                }
                else
                {
                    correos.Add(correo3.correo);
                }
            }
            //correos.Add("e.encinas@construplan.com.mx");

            string adjuntoCorreo = @"
                Buen dia.<br>
                
                Se realizó la conciliacion del periodo <b>" + periodo  + @"</b> de la Obra <b>" + nombre + "</b>";

            // Si se rechazó
            if (objAutorizacion.comentario != null && objAutorizacion.comentario.Trim().Length >= 10)
            {
                adjuntoCorreo = @"
                Se rechazó la conciliacion del periodo <b>" + periodo  + @"</b> de la Obra <b>" + nombre + "</b><br>" +
                "Razón del rechazo: " + WebUtility.HtmlEncode(objAutorizacion.comentario.Trim()) + " <br>";
            }
            try
            {
                correos.Remove("d.laborin@construplan.com.mx");

                if (objAutorizacion.autorizaGerenteID == 3337)
                {
                    correos.Add("d.laborin@construplan.com.mx");
                }
                else
                {
                    correos.Remove("d.laborin@construplan.com.mx");
                }

                if (objCC != null && objCC.areaCuenta == "2-1") correos.Add("manolo.anton@construplan.com.mx");
            }
            catch(Exception e){
            
            }
            string asuntoCorreo = setCorreoAutorizadores(adjuntoCorreo, objAutorizacion);
            GlobalUtils.sendEmailAdjuntoInMemory2(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), "Conciliacion de Horometros del periodo " + periodo), asuntoCorreo, correos, null);


        }
        private string setCorreoAutorizadores(string adjunto, tblM_AutorizaConciliacionHorometros Usuario)
        {
            var AsuntoCorreo = @"<html>    <head>
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
                                <body>
                                <p>" + adjunto + "</p>" + @" 
                                <table>
                                <thead>
                                  <tr>
                                    <th>Nombre Autorizador </th>
                                    <th>Descripción Puesto</th>
                                    <th>Autorizó</th>
                                  </tr></thead>
                                <tbody>
                                  <tr>
                                    <td>" + getUsuarioNombre(Usuario.autorizaAdmin) + "</td>" +
                                "<td>Administrador de Maquinaria</td>" +
                                 getEstatus(Usuario.pendienteAdmin) +
                              "</tr>" +
                                                               " <tr>" +
                                "<td>" + getUsuarioNombre(Usuario.autorizaGerenteID) + "</td>" +
                                "<td>Gerente de Obra</td>" +
                               getEstatus(Usuario.pendienteGerente) +
                //"</tr>" +
                //                                  "<tr>" +
                //  "<td>" + getUsuarioNombre(Usuario.autorizaDirector) + "</td>" +
                //  "<td>Director de Area</td>" +
                //  getEstatus(Usuario.pendienteDirector) +
                //"</tr> </tbody>" +
                             "</table>" +

                            "<br> Los archivos adjuntos seran cargados al correo una vez que la conciliacion este autorizada por completo." +
                            "<br> Este es un mensaje autogenerado por el sistema SIGOPLAN favor de no responderlo." +
                            "</body>  </html>";

            return AsuntoCorreo;
        }

        private string getEstatus(int p)
        {
            switch (p)
            {
                case 1:
                    return "<td style='background-color: #82E0AA;'>AUTORIZADO</td>";
                case 2:
                    return "<td style='background-color: #EC7063;'>RECHAZADO</td>";
                default:
                    return "<td style='background-color: #FAE5D3;'>PENDIENTE</td>";
            }
        }

        private string getUsuarioNombre(int idUsuario)
        {
            var usuario = _context.tblP_Usuario.FirstOrDefault(x => x.id == idUsuario);

            return usuario.nombre + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno;

        }

        public List<tblM_AutorizacionCaratulaPreciosU> loadTlbAutorizacionesCaratula(int cc, int estatus)
        {
            var objResult = _context.tblM_AutorizacionCaratulaPreciosU.Where(ac => cc != 0 ? ac.obraID == cc : true && ac.estadoCaratula == estatus).
                ToList();
            return objResult;
        }

        public bool autorizacionUsuario(int obj, int Autoriza, int tipo, string comentario)
        {
            tblM_AutorizacionCaratulaPreciosU objResultados = _context.tblM_AutorizacionCaratulaPreciosU.FirstOrDefault(x => x.id == obj);

            if (objResultados != null)
            {
                try
                {
                    DateTime fecha = DateTime.Now;
                    string f = fecha.ToString("ddMMyyyy") + fecha.Hour + "" + fecha.Minute;

                    if (tipo == 2 && comentario != null)
                    {
                        objResultados.comentario = comentario.Trim();
                    }


                    switch (Autoriza)
                    {
                        case 2:
                            {
                                objResultados.cadenaVobo1 = objResultados.id + f + "" + objResultados.usuarioVobo1 + "A";
                                objResultados.firmaVobo1 = tipo;
                                objResultados.usuarioFirma = 3;
                                objResultados.fechaVobo1 = DateTime.Now;

                                if (tipo == 2)
                                {
                                    objResultados.estadoCaratula = 2;
                                    objResultados.usuarioFirma = 5;
                                }


                                setOrUpdateAlerta(objResultados.usuarioElaboraID, objResultados.usuarioVobo1, false, objResultados.caratulaID, 3);
                                Update(objResultados, objResultados.id, (int)BitacoraEnum.AUTORIZASOLICITUD);

                                sendCorreo(objResultados, 2, comentario);
                            }
                            break;
                        case 3:
                            {
                                objResultados.cadenaVobo2 = objResultados.id + f + "" + objResultados.usuarioVobo2 + "A";
                                objResultados.firmaVobo2 = tipo;
                                objResultados.usuarioFirma = 4;
                                objResultados.fechaVobo2 = DateTime.Now;

                                if (tipo == 2)
                                {
                                    objResultados.usuarioFirma = 5;
                                    objResultados.estadoCaratula = 2;
                                    setOrUpdateAlerta(objResultados.usuarioVobo2, objResultados.usuarioAutoriza, false, objResultados.caratulaID, 3);
                                }
                                else
                                {
                                    setOrUpdateAlerta(objResultados.usuarioVobo1, objResultados.usuarioVobo2, true, objResultados.caratulaID, 4);
                                    setOrUpdateAlerta(objResultados.usuarioVobo2, objResultados.usuarioAutoriza, false, objResultados.caratulaID, 3);
                                }

                                Update(objResultados, objResultados.id, (int)BitacoraEnum.AUTORIZASOLICITUD);

                                sendCorreo(objResultados, 2, comentario);
                            }
                            break;
                        case 4:
                            {
                                objResultados.cadenaAutoriza = objResultados.id + f + "" + objResultados.usuarioAutoriza + "A";
                                objResultados.firmaAutoriza = tipo;
                                objResultados.usuarioFirma = 5;
                                objResultados.estadoCaratula = tipo;
                                objResultados.fechaAutoriza = DateTime.Now;

                                tblM_EncCaratula existe = _context.tblM_EncCaratula.Find(objResultados.caratulaID);

                                setOrUpdateAlerta(objResultados.usuarioVobo2, objResultados.usuarioAutoriza, true, objResultados.caratulaID, 4);

                                if (existe != null && tipo == 1)
                                {
                                    existe.isActivo = true;
                                    _context.SaveChanges();

                                }
                                Update(objResultados, objResultados.id, (int)BitacoraEnum.AUTORIZASOLICITUD);

                                sendCorreo(objResultados, 2, comentario);
                            }
                            break;
                        default:
                            break;
                    }
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }


        private void sendCorreo(tblM_AutorizacionCaratulaPreciosU objResultados, int tipo, string comentario)
        {
            string tablaAsunto = "";

            var objCaratula = _context.tblM_EncCaratula.FirstOrDefault(x => x.id == objResultados.caratulaID);


            var ccID = objCaratula.ccID;
            var objCC = _context.tblP_CC.FirstOrDefault(x => x.id == ccID);

            string descripcion = "";
            if (objCC != null)
                descripcion = objCC.descripcion + " (" + objCC.areaCuenta + ")";

            if (tipo == 1)
                tablaAsunto += "Buen día. <br>  Se generó una nueva de carátula.";
            else
                tablaAsunto += "Buen día. <br> Se realizó una actualización de carátula.";

            if (comentario != null && comentario.Length > 0)
            {
                tablaAsunto += @"<p><strong>Razón del rechazo:</strong> " + WebUtility.HtmlEncode(comentario) + "</p>";
            }

            tablaAsunto += descripcion + "<br>";
            tablaAsunto += @"<br> <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width='99%' style='width:99.04%;margin-left:13.5pt;border-collapse:collapse'>
                                    <thead>
                                        <tr>
                                            <td style='border:solid #DDDDDD 1.0pt;padding:6.0pt 6.0pt 6.0pt 6.0pt'><p class=MsoNormal><b><span style='font-family:'Arial',sans-serif'>Nombre Autorizador <o:p></o:p></span></b></p></td>
                                            <td style='border:solid #DDDDDD 1.0pt;border-left:none;padding:6.0pt 6.0pt 6.0pt 6.0pt'><p class=MsoNormal><b><span style='font-family:'Arial',sans-serif'>Descripción Puesto</o:p></span></b></p></td>
                                            <td style='border:solid #DDDDDD 1.0pt;border-left:none;padding:6.0pt 6.0pt 6.0pt 6.0pt'><p class=MsoNormal><b><span style='font-family:'Arial',sans-serif'>Autorizó</o:p></span></b></p></td>
                                        </tr>
                                    </thead>
                                        <tr>
                                            <td style='border:solid #DDDDDD 1.0pt;border-top:none;padding:6.0pt 6.0pt 6.0pt 6.0pt'><p class=MsoNormal><span style='font-family:'Arial',sans-serif'>" + GETNOMBREUSUARIO(objResultados.usuarioElaboraID) + @"</span></p></td>
                                            <td style='border-top:none;border-left:none;border-bottom:solid #DDDDDD 1.0pt;border-right:solid #DDDDDD 1.0pt;padding:6.0pt 6.0pt 6.0pt 6.0pt'><p class=MsoNormal><span style='font-family:'Arial',sans-serif'>Auxiliar de Construcción<o:p></o:p></span></p></td>";
            tablaAsunto += getEstatus(objResultados.firmaElabora);
            tablaAsunto += @"<tr>
                                            <td style='border:solid #DDDDDD 1.0pt;border-top:none;padding:6.0pt 6.0pt 6.0pt 6.0pt'><p class=MsoNormal><span style='font-family:'Arial',sans-serif'>" + GETNOMBREUSUARIO(objResultados.usuarioVobo1) + @"<o:p></o:p></span></p></td>
                                            <td style='border-top:none;border-left:none;border-bottom:solid #DDDDDD 1.0pt;border-right:solid #DDDDDD 1.0pt;padding:6.0pt 6.0pt 6.0pt 6.0pt'><p class=MsoNormal><span style='font-family:'Arial',sans-serif'>Director de Area<o:p></o:p></span></p></td>";
            tablaAsunto += getEstatus(objResultados.firmaVobo1);
            tablaAsunto += @"<tr>
                                            <td style='border:solid #DDDDDD 1.0pt;border-top:none;padding:6.0pt 6.0pt 6.0pt 6.0pt'><p class=MsoNormal><span style='font-family:'Arial',sans-serif'>" + GETNOMBREUSUARIO(objResultados.usuarioVobo2) + @"<o:p></o:p></span></p></td>
                                            <td style='border-top:none;border-left:none;border-bottom:solid #DDDDDD 1.0pt;border-right:solid #DDDDDD 1.0pt;padding:6.0pt 6.0pt 6.0pt 6.0pt'><p class=MsoNormal><span style='font-family:'Arial',sans-serif'>Director de Maquinaria<o:p></o:p></span></p></td>";

            //  <td style='border-top:none;border-left:none;border-bottom:solid #DDDDDD 1.0pt;border-right:solid #DDDDDD 1.0pt;background:#FAE5D3;padding:6.0pt 6.0pt 6.0pt 6.0pt'><p class=MsoNormal><span style='font-family:'Arial',sans-serif'>" + GETESTATUS(objResultados.firmaVobo2) + "<o:p></o:p></span></p></td>";
            tablaAsunto += getEstatus(objResultados.firmaVobo2);
            tablaAsunto += @"<tr>
                                            <td style='border:solid #DDDDDD 1.0pt;border-top:none;padding:6.0pt 6.0pt 6.0pt 6.0pt'><p class=MsoNormal><span style='font-family:'Arial',sans-serif'>" + GETNOMBREUSUARIO(objResultados.usuarioAutoriza) + @"<o:p></o:p></span></p></td>
                                            <td style='border-top:none;border-left:none;border-bottom:solid #DDDDDD 1.0pt;border-right:solid #DDDDDD 1.0pt;padding:6.0pt 6.0pt 6.0pt 6.0pt'><p class=MsoNormal><span style='font-family:'Arial',sans-serif'>Alta Dirección<o:p></o:p></span></p></td>";
            tablaAsunto += getEstatus(objResultados.firmaAutoriza);


            tablaAsunto += "</table> <br>";

            tablaAsunto += "<br>ESTE ES UN MENSAJE GENERADO POR EL SISTEMA SIGOPLAN FAVOR DE NO CONTESTAR ESTE CORREO.";

            List<string> correos = new List<string>();
            // correos.Add(item.usuario.correo);
            correos.Add(_context.tblP_Usuario.FirstOrDefault(x => x.id == objResultados.usuarioElaboraID).correo);
            correos.Add(_context.tblP_Usuario.FirstOrDefault(x => x.id == objResultados.usuarioAutoriza).correo);
            correos.Add(_context.tblP_Usuario.FirstOrDefault(x => x.id == objResultados.usuarioVobo1).correo);
            correos.Add(_context.tblP_Usuario.FirstOrDefault(x => x.id == objResultados.usuarioVobo2).correo);

            //correos.Add("e.encinas@construplan.com.mx");
            //correos.Add("martha.cheno@construplan.com.mx");
            //           string obra = _context.tblP_cc.FirstOrDefault(x=>x.id ==  objResultados.obraID).descripcion;
            GlobalUtils.sendEmailAdjuntoInMemory2(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), "Se Generó una nueva Carátula "), tablaAsunto, correos, null);
        }

        private string GETESTATUS(int p)
        {
            switch (p)
            {
                case 1:
                    return "AUTORIZADO";
                case 0:
                    return "PENDIENTE";
                case 2:
                    return "RECHAZADO";
                default:
                    return "";
            }
        }

        private string GETNOMBREUSUARIO(int p)
        {
            var usuario = _context.tblP_Usuario.FirstOrDefault(x => x.id == p);
            return usuario.nombre + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno;
        }

        public bool getModeloExiste(int p, string CentroCostos)
        {
            var idCc = _context.tblP_CC.FirstOrDefault(c => c.areaCuenta.Equals(CentroCostos));

            if (idCc != null)
            {
                var modeloExiste = from c in _context.tblM_EncCaratula
                                   join b in _context.tblM_CapCaratula
                                   on c.id equals b.idCaratula
                                   where c.isActivo && c.ccID == idCc.id && b.idModelo == p
                                   select b;

                if (modeloExiste.FirstOrDefault() != null)
                    return false;
                else
                    return true;
            }
            else
                return true;
        }

        public tblAutorizaCaratulaDTO loadAutorizacionCaratula(int objID)
        {
            var item = _context.tblM_AutorizacionCaratulaPreciosU.FirstOrDefault(x => x.id == objID);

            tblAutorizaCaratulaDTO obj = new tblAutorizaCaratulaDTO();
            if (item != null)
            {
                var usuarioAutorizaObj = _context.tblP_Usuario.FirstOrDefault(u => u.id == item.usuarioAutoriza);
                var usuarioElaboraObj = _context.tblP_Usuario.FirstOrDefault(u => u.id == item.usuarioElaboraID);
                var usuarioVobo1Obj = _context.tblP_Usuario.FirstOrDefault(u => u.id == item.usuarioVobo1);
                var usuarioVobo2Obj = _context.tblP_Usuario.FirstOrDefault(u => u.id == item.usuarioVobo2);
                obj.id = item.id;
                obj.usuarioFirma = item.usuarioFirma;
                obj.estadoCaratula = item.estadoCaratula;
                obj.caratulaID = item.caratulaID;
                /*Firma de Autorizadores*/
                obj.cadenaAutoriza = item.cadenaAutoriza;
                obj.cadenaElabora = item.cadenaElabora;
                obj.cadenaVobo1 = item.cadenaVobo1;
                obj.cadenaVobo2 = item.cadenaVobo2;
                /*Estado Firmas*/
                obj.firmaAutoriza = item.firmaAutoriza;
                obj.firmaElabora = item.firmaElabora;
                obj.firmaVobo1 = item.firmaVobo1;
                obj.firmaVobo2 = item.firmaVobo2;
                /*Datos Autorizadores */
                obj.obraID = item.obraID;
                obj.usuarioAutoriza = item.usuarioAutoriza;
                obj.usuarioAutorizaNombre = usuarioAutorizaObj != null ? usuarioAutorizaObj.nombre + " " + usuarioAutorizaObj.apellidoPaterno + " " + usuarioAutorizaObj.apellidoMaterno : "";
                obj.usuarioElaboraID = item.firmaElabora;
                obj.usuarioElaboraNombre = usuarioElaboraObj != null ? usuarioElaboraObj.nombre + " " + usuarioElaboraObj.apellidoPaterno + " " + usuarioElaboraObj.apellidoMaterno : "";
                obj.usuarioVobo1 = item.firmaElabora;
                obj.usuarioVobo1Nombre = usuarioVobo1Obj != null ? usuarioVobo1Obj.nombre + " " + usuarioVobo1Obj.apellidoPaterno + " " + usuarioVobo1Obj.apellidoMaterno : "";
                obj.usuarioVobo2 = item.firmaElabora;
                obj.usuarioVobo2Nombre = usuarioVobo2Obj != null ? usuarioVobo2Obj.nombre + " " + usuarioVobo2Obj.apellidoPaterno + " " + usuarioVobo2Obj.apellidoMaterno : "";
                var actual = _context.tblM_AutorizacionCaratulaPreciosU.Where(x => x.obraID == obj.obraID && x.estadoCaratula == 1).OrderByDescending(x => x.fechaAutoriza).FirstOrDefault();
                if (actual == null)
                    obj.caratulaActualID = 0;
                else
                {
                    obj.caratulaActualID = actual.caratulaID;
                }

                return obj;
            }
            else
            {
                return obj;
            }

        }
        public tblP_CC getCC(int ccID)
        {
            var data = _context.tblP_CC.FirstOrDefault(x => x.id == ccID);
            return data;
        }
        bool migrarCaratulas()
        {
            if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora)
            {
                return false;
            }
            using (var dbTransaction = _context.Database.BeginTransaction())
            //try
            {
                var ahora = DateTime.Now;
                var bdCplan = new MainContext((int)EmpresaEnum.Arrendadora);
                var strAhora = ahora.ToString("ddMMyyyyhhmm");
                var lstCc = _context.tblP_CC.ToList()
                    .Where(w => new List<string>() { "6-5", "6-11", "9-12", "9-13", "9-22", "10-1", "10-3", "10-4", "10-17", "10-23", "10-24", "11-7", "11-9" }.Any(a => a == w.areaCuenta));
                var AllEnc = _context.tblM_EncCaratula.Where(w => w.isActivo).ToList();
                var AllCar = _context.tblM_CapCaratula.ToList();
                var AllAuth = _context.tblM_AutorizacionCaratulaPreciosU.ToList();
                var ALLexcel = _context.excelCaratula.ToList();
                var AllMaquina = _context.tblM_CatMaquina.ToList();

                //AllEnc.ForEach(e => e.isActivo = false);
                //_context.SaveChanges();

                //var lstEnc = AllEnc.GroupBy(g => new { g.ccID, g.moneda }).Select(s => s.FirstOrDefault()).ToList();
                //var lstAuth = AllAuth.Where(w => w.estadoCaratula == 1).GroupBy(g => g.obraID).Select(s => s.FirstOrDefault()).ToList();
                //var lstBDEnc = ALLexcel.GroupBy(g => g.ac)
                //    .Select(s => new
                //    {
                //        cc = lstCc.FirstOrDefault(c => s.Key == c.areaCuenta),
                //        enc = lstEnc.Any(e => lstCc.Any(c => c.id == e.ccID)) ? lstEnc.FirstOrDefault(e => lstCc.Any(c => c.id == e.ccID)) : new tblM_EncCaratula() { idUsuario = vSesiones.sesionUsuarioDTO.id, fechaVigencia = ahora },
                //        exe = s
                //    }).Select(s => new tblM_EncCaratula()
                //    {
                //        ccID = s.cc.id,
                //        creacion = ahora,
                //        idUsuario = s.enc.idUsuario,
                //        isActivo = true,
                //        moneda = s.exe.FirstOrDefault().moneda,
                //        fechaVigencia = s.enc.fechaVigencia
                //    }).ToList();
                var idUsuario = vSesiones.sesionUsuarioDTO.id;
                var lstBDEnc = lstCc.Select(cc => new tblM_EncCaratula
                {
                    ccID = cc.id,
                    creacion = ahora,
                    idUsuario = idUsuario,
                    isActivo = true,
                    moneda = 1,
                    fechaVigencia = ahora
                }
                );
                _context.tblM_EncCaratula.AddRange(lstBDEnc);
                _context.SaveChanges();

                var lstBDCar = new List<tblM_CapCaratula>();
                ALLexcel.ForEach(e =>
                {
                    switch (e.areaCuenta)
                    {
                        case "9-20": e.areaCuenta = "6-5"; break;
                        case "6-9": e.areaCuenta = "6-11"; break;
                        case "6-10": e.areaCuenta = "9-12"; break;
                        case "7-1": e.areaCuenta = "9-13"; break;
                        case "7-9": e.areaCuenta = "9-13"; break;
                        case "9-1": e.areaCuenta = "9-22"; break;
                        case "9-2": e.areaCuenta = "10-1"; break;
                        case "9-3": e.areaCuenta = "10-3"; break;
                        case "9-4": e.areaCuenta = "10-4"; break;
                        case "9-5": e.areaCuenta = "10-17"; break;
                        case "9-6": e.areaCuenta = "10-23"; break;
                        case "9-7": e.areaCuenta = "10-24"; break;
                        case "9-8": e.areaCuenta = "11-7"; break;
                        case "9-9": e.areaCuenta = "11-9"; break;
                        default:
                            e.areaCuenta = string.Empty;
                            break;
                    }
                });
                var gpoAc = ALLexcel.GroupBy(g => g.areaCuenta.Trim()).Where(w => w.Key != string.Empty).ToList();
                var gpoMaq = AllMaquina.GroupBy(g => new { modelo = g.modeloEquipo.descripcion, grupo = g.grupoMaquinaria.descripcion, g.modeloEquipoID, g.grupoMaquinariaID }).ToList();
                gpoAc.ForEach(ac =>
                {
                    var enc = lstBDEnc.FirstOrDefault(e => lstCc.Any(c => c.id == e.ccID && ac.Key == c.areaCuenta));
                    if (enc != null)
                        gpoMaq.ForEach(maquina =>
                        {
                            var lstExcel = ac.Where(w => w.modelo.Contains(maquina.Key.modelo)).ToList();
                            if (lstExcel.Count == 0)
                            {
                                var exe = ALLexcel.FirstOrDefault(w => w.moneda == enc.moneda && w.modelo.Contains(maquina.Key.modelo));
                                if (exe == null)
                                {
                                    exe = new excelCaratula()
                                    {
                                        unidad = 1
                                    };
                                }
                                lstExcel.Add(exe);
                            }
                            if (lstExcel.Count > 0)
                            {
                                lstBDCar.AddRange(lstExcel.Select(exe => new tblM_CapCaratula()
                                {
                                    idCaratula = enc.id,
                                    idGrupo = maquina.Key.grupoMaquinariaID,
                                    idModelo = maquina.Key.modeloEquipoID,
                                    equipo = string.Format("{0} {1}", maquina.Key.grupo.Trim(), maquina.Key.modelo.Trim()),
                                    costo = exe.costo,
                                    unidad = exe.unidad,
                                    activo = true,
                                    cargoFijo = exe.cargoFijo,
                                    cOverhaul = exe.overhaul,
                                    cMttoCorrectivo = exe.mtoCorrectivo,
                                    cCombustible = exe.combustible,
                                    cAceites = exe.aceites,
                                    cFiltros = exe.filtros,
                                    cAnsul = exe.ansul,
                                    cCarrileria = exe.carrileria,
                                    cLlantas = exe.llantas,
                                    cHerramientasDesgaste = exe.desgasteHerramientas,
                                    cCargoOperador = exe.cargoOperador,
                                    cPersonalMtto = exe.personalMto,
                                    EncCaratula = enc
                                }));
                            }
                        });
                });

                _context.tblM_CapCaratula.AddRange(lstBDCar);
                _context.SaveChanges();

                //var lstBdAuth = lstBDEnc
                //    .Where(s => lstAuth.Any(a => a.obraID == s.ccID))
                //    .Select(s => new
                //    {
                //        enc = s,
                //        auth = lstAuth.FirstOrDefault(a => a.obraID == s.ccID)
                //    }).Select(s => new tblM_AutorizacionCaratulaPreciosU()
                //    {
                //        usuarioElaboraID = s.auth.usuarioElaboraID,
                //        usuarioVobo1 = s.auth.usuarioVobo1,
                //        usuarioVobo2 = s.auth.usuarioVobo2,
                //        usuarioAutoriza = s.auth.usuarioAutoriza,
                //        cadenaElabora = string.Format("{0}{1}{2}A", s.enc.id, strAhora, s.auth.usuarioElaboraID),
                //        cadenaVobo1 = string.Format("{0}{1}{2}A", s.enc.id, strAhora, s.auth.usuarioVobo1),
                //        cadenaVobo2 = string.Format("{0}{1}{2}A", s.enc.id, strAhora, s.auth.usuarioVobo2),
                //        cadenaAutoriza = string.Format("{0}{1}{2}A", s.enc.id, strAhora, s.auth.usuarioAutoriza),
                //        firmaElabora = 1,
                //        firmaVobo1 = 1,
                //        firmaVobo2 = 1,
                //        firmaAutoriza = 1,
                //        fechaElaboracion = ahora,
                //        fechaVobo1 = ahora,
                //        fechaVobo2 = ahora,
                //        fechaAutoriza = ahora,
                //        estadoCaratula = 1,
                //        usuarioFirma = s.auth.usuarioFirma,
                //        caratulaID = s.enc.id,
                //        obraID = s.enc.ccID,
                //    }).ToList();
                var lstBdAuth = lstBDEnc.Select(s => new tblM_AutorizacionCaratulaPreciosU()
                {
                    usuarioElaboraID = idUsuario,
                    usuarioVobo1 = idUsuario,
                    usuarioVobo2 = idUsuario,
                    usuarioAutoriza = idUsuario,
                    cadenaElabora = string.Format("{0}{1}{2}A", s.id, strAhora, idUsuario),
                    cadenaVobo1 = string.Format("{0}{1}{2}A", s.id, strAhora, idUsuario),
                    cadenaVobo2 = string.Format("{0}{1}{2}A", s.id, strAhora, idUsuario),
                    cadenaAutoriza = string.Format("{0}{1}{2}A", s.id, strAhora, idUsuario),
                    firmaElabora = 1,
                    firmaVobo1 = 1,
                    firmaVobo2 = 1,
                    firmaAutoriza = 1,
                    fechaElaboracion = ahora,
                    fechaVobo1 = ahora,
                    fechaVobo2 = ahora,
                    fechaAutoriza = ahora,
                    estadoCaratula = 1,
                    usuarioFirma = idUsuario,
                    caratulaID = s.id,
                    obraID = s.ccID,
                });
                _context.tblM_AutorizacionCaratulaPreciosU.AddRange(lstBdAuth);
                _context.SaveChanges();
                dbTransaction.Commit();
                return true;
            }
            //catch (Exception o_O)
            //{
            //    dbTransaction.Rollback();
            //    return false;
            //}
        }

        private class caratulaDTO
        {
            public int grupoID { get; set; }
            public string grupoDesc { get; set; }
            public int modeloID { get; set; }
            public string modeloDesc { get; set; }
            public string areaCuenta { get; set; }
        }

        public void migrarCapCaratulaTest()
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var caratulasExcel = _context.excelCaratula.ToList();

                    var caratulasUnicas = caratulasExcel.Select(x => new
                        {
                            grupo = x.grupo.Trim(),
                            modelo = x.modelo.Trim()
                        })
                        .OrderBy(x => x.grupo).ThenBy(x => x.modelo)
                        .Distinct()
                        .ToList();

                    var acs = _context.tblP_CC.Where(x => x.estatus && x.areaCuenta != "2-1").ToList();

                    var listaGrupoModeloMaquinaria = _context.tblM_CatMaquina.Select(x => new caratulaDTO
                    {
                        grupoID = x.grupoMaquinariaID,
                        grupoDesc = x.grupoMaquinaria.descripcion.Trim(),
                        modeloID = x.modeloEquipoID,
                        modeloDesc = x.modeloEquipo.descripcion.Trim(),
                        areaCuenta = x.centro_costos.Trim()
                    }).Distinct().OrderBy(x => x.grupoDesc).ThenBy(x => x.modeloDesc).ToList();

                    var listaGrupoModelo = _context.tblM_CatModeloEquipo.ToList().Select(x => new caratulaDTO
                    {
                        grupoID = x.idGrupo.GetValueOrDefault(),
                        grupoDesc = _context.tblM_CatGrupoMaquinaria.First(y => y.id == x.idGrupo).descripcion.Trim(),
                        modeloID = x.id,
                        modeloDesc = x.descripcion.Trim()
                    }).Distinct().OrderBy(x => x.grupoDesc).ThenBy(x => x.modeloDesc).ToList();

                    foreach (var ac in acs)
                    {
                        var encCaratula = _context.tblM_EncCaratula.FirstOrDefault(x => x.ccID == ac.id && x.isActivo);
                        if (encCaratula != null)
                        {
                            encCaratula.isActivo = false;
                        }

                        var nuevoEncabezadoCaratula = new tblM_EncCaratula
                        {
                            ccID = ac.id,
                            creacion = encCaratula != null ? encCaratula.creacion : DateTime.Now,
                            fechaVigencia = encCaratula != null ? encCaratula.fechaVigencia : DateTime.Now.AddYears(1),
                            idUsuario = encCaratula != null ? encCaratula.idUsuario : vSesiones.sesionUsuarioDTO.id,
                            isActivo = true,
                            moneda = encCaratula != null ? encCaratula.moneda : 1
                        };
                        _context.tblM_EncCaratula.Add(nuevoEncabezadoCaratula);
                        _context.SaveChanges();

                        var listaCapCaratulas = new List<tblM_CapCaratula>();

                        foreach (var caratulaUnica in caratulasUnicas)
                        {
                            var caratulaAC = caratulasExcel.FirstOrDefault(x =>
                                x.areaCuenta == ac.areaCuenta &&
                                x.grupo == caratulaUnica.grupo &&
                                x.modelo == caratulaUnica.modelo);

                            var matches = listaGrupoModeloMaquinaria.Where(x =>
                                x.grupoDesc == caratulaUnica.grupo &&
                                x.modeloDesc == caratulaUnica.modelo
                                ).ToList();

                            if (matches.Count > 0)
                            {
                                matches.GroupBy(x => x.grupoID).ForEach(x =>
                                {
                                    x.GroupBy(z => z.modeloID).ForEach(z =>
                                    {
                                        var capCaratula = InitCapCaratula(caratulaAC);

                                        capCaratula.idGrupo = z.First().grupoID;
                                        capCaratula.idModelo = z.First().modeloID;
                                        capCaratula.equipo = z.First().grupoDesc + " " + z.First().modeloDesc;
                                        capCaratula.idCaratula = nuevoEncabezadoCaratula.id;
                                        listaCapCaratulas.Add(capCaratula);
                                    });
                                });
                            }
                            else
                            {
                                var matchesGrupoModelo = listaGrupoModelo.Where(x =>
                                x.grupoDesc == caratulaUnica.grupo &&
                                x.modeloDesc == caratulaUnica.modelo
                                );

                                matchesGrupoModelo.GroupBy(x => x.grupoID).ForEach(x =>
                                {
                                    x.GroupBy(z => z.modeloID).ForEach(z =>
                                    {
                                        var capCaratula = InitCapCaratula(caratulaAC);

                                        capCaratula.idGrupo = z.First().grupoID;
                                        capCaratula.idModelo = z.First().modeloID;
                                        capCaratula.equipo = z.First().grupoDesc + " " + z.First().modeloDesc;
                                        capCaratula.idCaratula = nuevoEncabezadoCaratula.id;
                                        listaCapCaratulas.Add(capCaratula);
                                    });
                                });
                            }
                        }

                        _context.tblM_CapCaratula.AddRange(listaCapCaratulas);
                        _context.SaveChanges();
                    }

                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    var b = 5;
                }
            }
        }

        private tblM_CapCaratula InitCapCaratula(excelCaratula caratulaAC)
        {
            var capCaratula = new tblM_CapCaratula { activo = true };

            if (caratulaAC == null)
            {
                capCaratula.unidad = 1;
            }
            else
            {
                capCaratula.unidad = caratulaAC.unidad;
                capCaratula.costo = caratulaAC.costo;
                capCaratula.cargoFijo = caratulaAC.cargoFijo;
                capCaratula.cOverhaul = caratulaAC.overhaul;
                capCaratula.cMttoCorrectivo = caratulaAC.mtoCorrectivo;
                capCaratula.cCombustible = caratulaAC.combustible;
                capCaratula.cAceites = caratulaAC.aceites;
                capCaratula.cFiltros = caratulaAC.filtros;
                capCaratula.cAnsul = caratulaAC.ansul;
                capCaratula.cCarrileria = caratulaAC.carrileria;
                capCaratula.cLlantas = caratulaAC.llantas;
                capCaratula.cHerramientasDesgaste = caratulaAC.desgasteHerramientas;
                capCaratula.cCargoOperador = caratulaAC.cargoOperador;
                capCaratula.cPersonalMtto = caratulaAC.personalMto;
            }

            return capCaratula;
        }

        private void agregarCombinacionesNoExistentes()
        {
            var caratulasExcel = _context.excelCaratula.ToList();

            var caratulasUnicas = caratulasExcel
                .Select(x => new
                {
                    grupo = x.grupo.Trim(),
                    modelo = x.modelo.Trim()
                })
                .OrderBy(x => x.grupo).ThenBy(x => x.modelo)
                .Distinct()
                .ToList();

            var acs = _context.tblP_CC.Where(x => x.estatus).ToList();

            var acTest = acs.First(x => x.areaCuenta == "4-2");

            var tipoMonedaAC = caratulasExcel.First(x => x.areaCuenta == acTest.areaCuenta).moneda;

            //var encCaratula = new EncCaratulaTest
            //{
            //    ccID = acTest.id,
            //    creacion = DateTime.Now,
            //    fechaVigencia = DateTime.Now.AddYears(1),
            //    idUsuario = vSesiones.sesionUsuarioDTO.id,
            //    isActivo = true,
            //    moneda = tipoMonedaAC
            //};

            //_context.EncCaratulaTest.Add(encCaratula);
            //_context.SaveChanges();

            var listaGrupoModeloMaquinaria = _context.tblM_CatMaquina.Select(x => new caratulaDTO
            {
                grupoID = x.grupoMaquinariaID,
                grupoDesc = x.grupoMaquinaria.descripcion.Trim(),
                modeloID = x.modeloEquipoID,
                modeloDesc = x.modeloEquipo.descripcion.Trim(),
                //areaCuenta = x.centro_costos.Trim()
            }).Distinct().OrderBy(x => x.grupoDesc).ThenBy(x => x.modeloDesc).ToList();


            var gruposModelosNoExistentes = listaGrupoModeloMaquinaria.Where(x =>
            {
                var grupoModeloDesc = x.grupoDesc + x.modeloDesc;

                var caratulaExistente = caratulasUnicas.FirstOrDefault(y => (y.grupo + y.modelo) == grupoModeloDesc);

                return caratulaExistente == null;
            }).ToList();

            var lista = new List<excelCaratula>();

            foreach (var grupoModeloNoExistente in gruposModelosNoExistentes)
            {
                lista.Add(new excelCaratula
                 {
                     areaCuenta = "4-2",
                     unidad = 1,
                     moneda = 1,
                     grupo = grupoModeloNoExistente.grupoDesc,
                     modelo = grupoModeloNoExistente.modeloDesc
                 });
            }

            _context.excelCaratula.AddRange(lista);

            var a = 5;
            _context.SaveChanges();

        }

        #region Facturado
        public List<ConciliacionFacturadoDTO> getConciliacionesAFacturar(bool estado, string folio, int cc, DateTime fechaInicio, DateTime fechaFin) 
        {
            
            int temp;
            if (Int32.TryParse(folio, out temp)) { folio = temp.ToString(); }

            var auxConciliacionesAutorizadasID = _context.tblM_AutorizaConciliacionHorometros.Where(x => x.pendienteAdmin == 1 && x.pendienteDirector == 1 && x.pendienteGerente == 1).Select(x => x.conciliacionID);

            var encConciliaciones = _context.tblM_CapEncConciliacionHorometros
            .Where(x =>
                x.id.ToString().Contains(folio)
                && (cc == -1 ? true : x.centroCostosID == cc)
                && x.fechaFin >= fechaInicio
                && x.fechaFin <= fechaFin
                && x.facturado == estado
                && auxConciliacionesAutorizadasID.Contains(x.id)
            ).ToList();
            List<DateTime> dias = new List<DateTime>();
            DateTime fechaMinima = encConciliaciones.Count() > 0 ? encConciliaciones.Min(x => x.fechaInicio) : DateTime.Today;
            while (fechaMinima.Date <= DateTime.Today) 
            {
                dias.Add(fechaMinima.Date);
                fechaMinima = fechaMinima.AddDays(1);
            }
            var diasDolar = getDolarDias(dias);

            var encConciliacionesCCIDs = encConciliaciones.Select(y => y.centroCostosID);
            var centrosCosto = _context.tblP_CC.Where(x => encConciliacionesCCIDs.Contains(x.id)).ToList();

            var data = encConciliaciones
            .Select(x => {
                var auxCC = centrosCosto.FirstOrDefault(y => y.id == x.centroCostosID);
                var dolar = diasDolar.FirstOrDefault(z => z.fecha.Date == x.fechaFin);
                decimal auxDolar = (decimal)18.66;
                if (dolar != null) { auxDolar = dolar.tipo_cambio; }
                var importe = _context.tblM_CapConciliacionHorometros.Where(y => y.idEncCaratula == x.id).ToList();
                return new ConciliacionFacturadoDTO()
                {
                    id = x.id,
                    folio = x.id.ToString().PadLeft(10, '0'),
                    ccID = x.centroCostosID,
                    cc = auxCC == null ? "INDEFINIDO" : auxCC.descripcion,
                    fechaInicioRaw = x.fechaInicio,
                    fechaInicio = x.fechaInicio.ToString("dd/MM/yyyy"),
                    fechaFinRaw = x.fechaFin,
                    fechaFin = x.fechaFin.ToString("dd/MM/yyyy"),
                    importe = importe.Count() > 0 ? importe.Sum(y => y.moneda == 2 ? y.total * auxDolar : y.total) : 0,
                    facturas = x.factura.Replace('[', '\0').Replace('"', '\0').Replace(']', '\0')
                };
            }).ToList();
            return data;
        }

        public bool indicarFacturacion(int conciliacionID, List<string> factura) 
        {
            try
            {
                var conciliacion = _context.tblM_CapEncConciliacionHorometros.FirstOrDefault(x => x.id == conciliacionID);
                if (conciliacion != null)
                {
                    conciliacion.factura = JsonConvert.SerializeObject(factura);
                    conciliacion.facturado = factura.Count() > 0;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e) 
            {
                return false;
            }
        }

        private List<Core.DTO.Contabilidad.TipoCambioDllDTO> getDolarDias(List<DateTime> dias)
        {
            try
            {
                var lst = new List<OdbcParameterDTO>();
                if (dias.Count() > 0)
                {
                    lst.AddRange(dias.Select(s => new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.DateTime, valor = s }));
                    var odbc = new OdbcConsultaDTO()
                    {
                        consulta = string.Format("SELECT tipo_cambio, fecha FROM tipo_cambio WHERE fecha in {0}", dias.ToParamInValue()),
                        parametros = lst
                    };
                    var tipoCambio = _contextEnkontrol.Select<Core.DTO.Contabilidad.TipoCambioDllDTO>(EnkontrolAmbienteEnum.Prod, odbc);
                    return tipoCambio;
                }
                else { return new List<Core.DTO.Contabilidad.TipoCambioDllDTO>(); }
                //var dolar = (List<Core.DTO.Contabilidad.TipoCambioDllDTO>)_contextEnkontrol.Where(consulta).ToObject<List<Core.DTO.Contabilidad.TipoCambioDllDTO>>();
                //return dolar.FirstOrDefault().tipo_cambio;
            }
            catch (Exception) { return new List<Core.DTO.Contabilidad.TipoCambioDllDTO>(); }
        }

        public List<string> getFacturasConciliacion(int conciliacionID) 
        {
            var conciliacion = _context.tblM_CapEncConciliacionHorometros.FirstOrDefault(x => x.id == conciliacionID);
            List<string> data = new List<string>();
            if (conciliacion != null) 
            {
                try
                {
                    data = JsonConvert.DeserializeObject<List<string>>(conciliacion.factura);
                    if (data == null) data = new List<string>();
                    return data;
                }
                catch (Exception) 
                {
                    return data;
                }
            }
            return data;
        }

        #endregion

    }
}