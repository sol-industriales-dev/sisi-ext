using Core.DAO.Enkontrol.Compras;
using Core.DTO;
using Core.DTO.Enkontrol.OrdenCompra;
using Core.DTO.Enkontrol.OrdenCompra.CuadroComparativo;
using Core.DTO.Enkontrol.Requisicion;
using Core.DTO.Generales.Enkontrol.Proveedores;
using Core.DTO.Maquinaria.KPI.Dashboard;
using Core.DTO.Utils.Data;
using Core.Entity.Encuestas;
using Core.Entity.Enkontrol.Compras.OrdenCompra;
using Core.Entity.Enkontrol.Compras.OrdenCompra.CuadroComparativo;
using Core.Entity.Enkontrol.Compras.Requisicion;
using Core.Enum.Encuesta;
using Core.Enum.Enkontrol.Compras;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.DTO;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Enkontrol.Compras
{
    public class CuadroComparativoDAO : GenericDAO<tblCom_CC_Calificacion>, ICuadroComparativoDAO
    {
        private bool productivo = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["enkontrolProductivo"]) == "1";

        private const int tipoRequisicionReembolso = 4;

        private tblCom_CC_CatConfiabilidad confiabilidad;

        #region DASHBOARD
        public Dictionary<string, object> ConsultaDashboard(DateTime fechaInicio, DateTime fechaFin, List<int> proveedores, List<int> compradores)
        {
            var r = new Dictionary<string, object>();
            try
            {

                var comprasConCalificacion = _context.tblCom_CC_ProveedorNoOptimo.Where(w => (w.Fecha >= fechaInicio && w.Fecha <= fechaFin) && w.Estatus).ToList();
                var noOptimas = comprasConCalificacion.Where(w => w.IdTipoCalificacion == (int)TipoCalificacion.NoOptima).ToList();
                var optimas = comprasConCalificacion.Where(w => w.IdTipoCalificacion == (int)TipoCalificacion.Optima).ToList();
                var media = comprasConCalificacion.Where(w => w.IdTipoCalificacion == (int)TipoCalificacion.Media).ToList();

                var _infoProveedores = InfoProveedores(proveedores);
                var informacionProveedores = new List<InfoProveedorDTO>();
                var comprasTotales = comprasConCalificacion.Count > 0 ? comprasConCalificacion.Count : 1;

                List<tblCom_CC_ProveedorNoOptimo> lstProveedoresNoOptimos = _context.tblCom_CC_ProveedorNoOptimo.Where(x => x.Estatus).ToList();

                List<tblCom_CC_Calificacion> lstCalificaciones = _context.Select<tblCom_CC_Calificacion>(new DapperDTO
                {
                    baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                    consulta = @"SELECT * FROM tblCom_CC_Calificacion WHERE esActivo = 1"
                }).ToList();

                #region Grafica de pastel de provedores optimos vs no optimos
                var yNoOptimas = new SeriesGraficaDTO();
                var porcentajeNoOptimas = ((noOptimas.Count + media.Count) * 100) / comprasTotales;
                yNoOptimas.name = "No optimas";
                yNoOptimas.y = porcentajeNoOptimas;
                var yOptimas = new SeriesGraficaDTO();
                var porcentajeOptimas = (optimas.Count * 100) / comprasTotales;
                yOptimas.name = "Optimas";
                yOptimas.y = porcentajeOptimas;
                var series = new List<SeriesGraficaDTO>
                {
                    yNoOptimas, yOptimas
                };
                #endregion

                #region Grafica de pastel del top 10 de proveedores optimos
                var top10ProvOptimos = new List<SeriesGraficaDTO>();
                foreach (var proveedor in optimas.GroupBy(g => g.Calificacion.Proveedor).Take(10))
                {
                    var yProveedor = new SeriesGraficaDTO();

                    //var _prov = informacionProveedores.FirstOrDefault(f => f.Numero == proveedor.Key);
                    InfoProveedorDTO _prov = new InfoProveedorDTO();

                    _prov = informacionProveedores.FirstOrDefault(f => f.Numero == proveedor.Key);

                    if (_prov == null)
                    {
                        //_prov = InfoProveedor(proveedor.Key);
                        _prov = _infoProveedores.FirstOrDefault(f => f.Numero == proveedor.Key);
                        if (_prov != null)
                            informacionProveedores.Add(_prov);
                    }

                    yProveedor.name = _prov != null ? _prov.Nombre : proveedor.Key.ToString();
                    yProveedor.y = proveedor.Count();

                    top10ProvOptimos.Add(yProveedor);
                }
                #endregion

                #region Detalles grafica pastel Top 10 proveedores optimos
                //SE OBTIENE LAS CalificacionID
                var lstCalificacionesID = _context.tblCom_CC_ProveedorNoOptimo.Where(x => x.Estatus && x.IdTipoCalificacion == (int)TipoCalificacion.Optima && (x.Fecha >= fechaInicio && x.Fecha <= fechaFin)).Select(x => x.CalificacionId).ToList();

                var lstCalificacionesGrpID = lstCalificacionesID.GroupBy(x => x).ToList();

                //SE OBTIENE LOS ID DE LOS PROVEEDORES
                var lstProveedoresID = lstCalificaciones.Where(x => lstCalificacionesID.Contains(x.Id) && x.esActivo).GroupBy(x => x.Proveedor).ToList();

                var lstDetalleTop10ProveedoresOptimos = new List<DetalleTop10ProvDTO>();
                for (int i = 0; i < lstProveedoresID.Count(); i++)
                {
                    InfoProveedorDTO objProv = new InfoProveedorDTO();
                    string nombreProv = string.Empty;
                    if (_infoProveedores != null)
                    {
                        objProv = _infoProveedores.FirstOrDefault(f => f.Numero == lstProveedoresID[i].Key);
                        if (objProv != null)
                            nombreProv = objProv.Nombre;
                    }

                    var data = lstProveedoresID.Select(m => new DetalleTop10ProvDTO
                    {
                        //proveedor = InfoProveedor(lstProveedoresID[i].Key).Nombre,
                        //proveedor = _infoProveedores.FirstOrDefault(f => f.Numero == lstProveedoresID[i].Key).Nombre,
                        proveedor = nombreProv,
                        proveedorID = lstProveedoresID[i].Key,
                        //cantOC = CantOCProvOptimo(lstCalificacionesGrpID[i].Key, lstProveedoresNoOptimos)
                    }).ToList();
                    lstDetalleTop10ProveedoresOptimos.AddRange(data);
                }
                //r.Add("lstDetalleTop10ProveedoresOptimos", lstDetalleTop10ProveedoresOptimos.Take(10)); //CONSULTA APARTE
                #endregion

                #region Detalles grafica pastel Top 10 proveedores no optimos
                lstCalificacionesID = _context.tblCom_CC_ProveedorNoOptimo.Where(x => x.Estatus && x.IdTipoCalificacion == (int)TipoCalificacion.NoOptima && (x.Fecha >= fechaInicio && x.Fecha <= fechaFin))
                    .Select(x => x.CalificacionId).ToList();

                lstCalificacionesGrpID = lstCalificacionesID.GroupBy(x => x).ToList();

                //SE OBTIENE LOS ID DE LOS PROVEEDORES
                lstProveedoresID = lstCalificaciones.Where(x => lstCalificacionesID.Contains(x.Id) && x.esActivo).GroupBy(x => x.Proveedor).ToList();

                var lstDetalleTop10ProveedoresNoOptimos = new List<DetalleTop10ProvDTO>();
                for (int i = 0; i < lstProveedoresID.Count(); i++)
                {
                    InfoProveedorDTO objProv = new InfoProveedorDTO();
                    string nombreProv = string.Empty;
                    if (_infoProveedores != null) 
                    {
                        objProv = _infoProveedores.FirstOrDefault(f => f.Numero == lstProveedoresID[i].Key);
                        if (objProv != null)
                            nombreProv = objProv.Nombre;
                    }

                    var data = lstProveedoresID.Select(m => new DetalleTop10ProvDTO
                    {
                        //proveedor = InfoProveedor(lstProveedoresID[i].Key).Nombre,
                        proveedor = nombreProv,
                        proveedorID = lstProveedoresID[i].Key,
                        cantOC = CantOCProvOptimo(lstCalificacionesGrpID[i].Key, lstProveedoresNoOptimos)
                    }).ToList();
                    var t = data.First();
                    lstDetalleTop10ProveedoresNoOptimos.Add(t);
                }
                //r.Add("lstDetalleTop10ProveedoresNoOptimos", lstDetalleTop10ProveedoresNoOptimos.Take(10)); // CONSULTA APARTE
                #endregion

                #region Detalles grafica pastel Calificaciones
                //SE OBTIENE LAS CalificacionID
                lstCalificacionesID = _context.tblCom_CC_ProveedorNoOptimo.Where(x => x.Estatus && (x.Fecha >= fechaInicio && x.Fecha <= fechaFin)).Select(x => x.CalificacionId).ToList();
                lstCalificacionesGrpID = lstCalificacionesID.GroupBy(x => x).ToList();

                //SE OBTIENE LOS ID DE LOS PROVEEDOR
                var lstProveedores = lstCalificaciones.Where(x => lstCalificacionesID.Contains(x.Id) && x.esActivo).ToList();
                //var lstProveedores = _context.tblCom_CC_Calificacion.Where(x => x.esActivo).GroupBy(x => x.Proveedor).ToList();
                var lstOC = _context.tblCom_CC_ProveedorNoOptimo.Where(x => x.Estatus && (x.Fecha >= fechaInicio && x.Fecha <= fechaFin)).ToList();

                var lstDetalleCalificaciones = new List<DetalleCalificacionesDTO>();
                for (int i = 0; i < lstProveedores.Count(); i++)
                {
                    InfoProveedorDTO objProv = new InfoProveedorDTO();
                    string nombreProv = string.Empty;
                    if (_infoProveedores != null)
                    {
                        objProv = _infoProveedores.FirstOrDefault(f => f.Numero == lstProveedores[i].Proveedor);
                        if (objProv != null)
                            nombreProv = objProv.Nombre;
                    }

                    var data = lstProveedores.Select(m => new DetalleCalificacionesDTO
                    {
                        oc = lstOC[i].NumeroCompra,
                        requisicion = lstProveedores[i].Numero,
                        cc = lstProveedores[i].CC,
                        //proveedor = InfoProveedor(lstProveedores[i].Proveedor).Nombre,
                        proveedor = nombreProv,
                        fecha = lstOC[i].Fecha
                    }).ToList();
                    var t = data.First();
                    lstDetalleCalificaciones.Add(t);
                }
                //r.Add("lstDetalleCalificaciones", lstDetalleCalificaciones); //CONSULTA APARTE
                #endregion

                #region Grafica de pastel del top 10 de proveedores no optimos
                var top10ProvNoOptimos = new List<SeriesGraficaDTO>();
                foreach (var proveedor in noOptimas.GroupBy(g => g.Calificacion.Proveedor).Take(10))
                {
                    var yProveedor = new SeriesGraficaDTO();

                    var _prov = informacionProveedores.FirstOrDefault(f => f.Numero == proveedor.Key);

                    if (_prov == null)
                    {
                        //_prov = InfoProveedor(proveedor.Key);
                        _prov = _infoProveedores.FirstOrDefault(f => f.Numero == proveedor.Key);
                        if (_prov != null)
                            informacionProveedores.Add(_prov);
                    }

                    yProveedor.name = _prov != null ? _prov.Nombre : proveedor.Key.ToString();
                    yProveedor.y = proveedor.Count();

                    top10ProvNoOptimos.Add(yProveedor);
                }
                #endregion

                #region Grafica de barras por calificaciones
                var objCalificaciones = _context.tblCom_CC_ProveedorNoOptimo.Where(x => x.Estatus && (x.Fecha >= fechaInicio && x.Fecha <= fechaFin)).Select(x => x.IdTipoCalificacion).ToList();
                List<int> lstComprasOptimas = new List<int>();
                List<int> lstComprasMedias = new List<int>();
                List<int> lstComprasNoOptimas = new List<int>();
                int comprasOptimas = 0, comprasMedias = 0, comprasNoOptimas = 0;
                foreach (var tipoCalificacion in objCalificaciones)
                {
                    switch (tipoCalificacion)
                    {
                        case 1:
                            comprasOptimas++;
                            break;
                        case 2:
                            comprasMedias++;
                            break;
                        case 3:
                            comprasNoOptimas++;
                            break;
                        default:
                            break;
                    }
                }
                lstComprasOptimas.Add(comprasOptimas);
                lstComprasMedias.Add(comprasMedias);
                lstComprasNoOptimas.Add(comprasNoOptimas);

                r.Add("lstComprasOptimas", lstComprasOptimas);
                r.Add("lstComprasMedias", lstComprasMedias);
                r.Add("lstComprasNoOptimas", lstComprasNoOptimas);
                #endregion

                #region Grafica de barras por proveedores
                var gpx_Proveedores = new InfoGraficasDTO();
                List<InfoGraficasDTO> lstGpx_Proveedores = new List<InfoGraficasDTO>();
                Dictionary<int, int> lstProveedoresOrdBy = new Dictionary<int, int>();

                var objProveedores = lstCalificaciones.Where(x => proveedores.Contains(x.Proveedor) && x.esActivo).Select(x => x.Id).ToList();
                var objCalificacionesID = _context.tblCom_CC_ProveedorNoOptimo.Where(x => x.Estatus && (x.Fecha >= fechaInicio && x.Fecha <= fechaFin) && objProveedores.Contains(x.CalificacionId)).GroupBy(x => x.Calificacion.Proveedor).ToList();

                foreach (var itemProveedor in objCalificacionesID)
                {
                    comprasOptimas = 0;
                    comprasMedias = 0;
                    comprasNoOptimas = 0;
                    int idProveedor = itemProveedor.Key;
                    foreach (var itemCalificaciones in itemProveedor.Where(x => x.Estatus))
                    {
                        switch (itemCalificaciones.IdTipoCalificacion)
                        {
                            case 1:
                                comprasOptimas++;
                                break;
                            case 2:
                                comprasMedias++;
                                break;
                            case 3:
                                comprasNoOptimas++;
                                break;
                            default:
                                break;
                        }
                    }
                    int totalComprasOC = comprasOptimas + comprasMedias + comprasNoOptimas;
                    lstProveedoresOrdBy.Add(idProveedor, (int)totalComprasOC);
                }
                var dicProveedores = lstProveedoresOrdBy.OrderByDescending(x => x.Value);
                var lstProveedoresGpxProv = dicProveedores.Select(x => x.Key).ToList().Take(20);
                var lstCalificacionesGpxProveedoresID = lstCalificaciones.Where(x => lstProveedoresGpxProv.Contains(x.Proveedor) && x.esActivo).ToList();
                var lstTipoCalificaciones = _context.tblCom_CC_ProveedorNoOptimo.Where(x => x.Estatus && (x.Fecha >= fechaInicio && x.Fecha <= fechaFin)).ToList();

                List<string> lstNombreProveedores = new List<string>();
                foreach (var itemProveedores in dicProveedores.Take(20))
                {
                    lstGpx_Proveedores = new List<InfoGraficasDTO>();
                    string proveedor = string.Empty;
                    //proveedor = InfoProveedor(itemProveedores.Key).Nombre;
                    proveedor = _infoProveedores.FirstOrDefault(f => f.Numero == itemProveedores.Key).Nombre;
                    gpx_Proveedores.categorias.Add(proveedor);

                    comprasOptimas = 0;
                    comprasMedias = 0;
                    comprasNoOptimas = 0;

                    var calificacionID = lstCalificacionesGpxProveedoresID.Where(x => x.Proveedor == itemProveedores.Key).Select(s => s.Id).ToList();
                    var tipoCalificaciones = lstTipoCalificaciones.Where(x => calificacionID.Contains(x.CalificacionId)).Select(s => s.IdTipoCalificacion).ToList();

                    foreach (var itemCalificaciones in tipoCalificaciones)
                    {
                        switch (itemCalificaciones)
                        {
                            case 1:
                                comprasOptimas++;
                                break;
                            case 2:
                                comprasMedias++;
                                break;
                            case 3:
                                comprasNoOptimas++;
                                break;
                            default:
                                break;
                        }
                    }

                    gpx_Proveedores.serie1Descripcion = "Optimo";
                    gpx_Proveedores.serie1.Add(comprasOptimas);
                    gpx_Proveedores.serie2Descripcion = "Media";
                    gpx_Proveedores.serie2.Add(comprasMedias);
                    gpx_Proveedores.serie3Descripcion = "No optimo";
                    gpx_Proveedores.serie3.Add(comprasNoOptimas);

                    decimal totalComprasOC = comprasOptimas + comprasMedias + comprasNoOptimas;
                    gpx_Proveedores.totalComprasOC.Add(totalComprasOC);

                    lstGpx_Proveedores.Add(gpx_Proveedores);
                }

                r.Add("lstGpx_Proveedores", lstGpx_Proveedores);
                #endregion

                #region Grafica de barras por compradores
                var gpx_Compradores = new InfoGraficasDTO();
                List<InfoGraficasDTO> lstGpx_Compradores = new List<InfoGraficasDTO>();
                Dictionary<int, int> dicCompradores = new Dictionary<int, int>();

                var objCompradores = _context.tblCom_CC_ProveedorNoOptimo.Where(x => x.Estatus && (x.Fecha >= fechaInicio && x.Fecha <= fechaFin) && compradores.Contains(x.idUsuario)).ToList();
                List<int> lstCompradoresID = objCompradores.GroupBy(x => x.idUsuario).Select(x => x.Key).ToList();
                foreach (var itemCompradores in lstCompradoresID)
                {
                    int idUsuario = itemCompradores;
                    int cantCompras = objCompradores.Where(x => x.idUsuario == idUsuario).Count();
                    dicCompradores.Add(idUsuario, cantCompras);
                }
                var lstCompradoresOrdBy = dicCompradores.OrderByDescending(x => x.Value).Take(20);

                var lstUsuariosID = lstCompradoresOrdBy.Select(s => s.Key).ToList();
                var objNombreCompradores = _context.tblP_Usuario.Where(x => lstUsuariosID.Contains(x.id) && x.estatus).ToList();

                for (int i = 0; i < objNombreCompradores.Count(); i++)
                {
                    string nombreCompleto = string.Empty;
                    nombreCompleto = objNombreCompradores[i].nombre + " " + objNombreCompradores[i].apellidoPaterno;
                    gpx_Compradores.categorias.Add(nombreCompleto);

                    comprasOptimas = 0;
                    comprasMedias = 0;
                    comprasNoOptimas = 0;
                    int idUsuario = objNombreCompradores[i].id;
                    var calificacionesID = objCompradores.Where(x => x.idUsuario == idUsuario).Select(s => s.IdTipoCalificacion).ToList();
                    foreach (var item in calificacionesID)
                    {
                        switch (item)
                        {
                            case 1:
                                comprasOptimas++;
                                break;
                            case 2:
                                comprasMedias++;
                                break;
                            case 3:
                                comprasNoOptimas++;
                                break;
                            default:
                                break;
                        }
                    }
                    gpx_Compradores.serie1Descripcion = "Optimo";
                    gpx_Compradores.serie1.Add(comprasOptimas);
                    gpx_Compradores.serie2Descripcion = "Media";
                    gpx_Compradores.serie2.Add(comprasMedias);
                    gpx_Compradores.serie3Descripcion = "No optimo";
                    gpx_Compradores.serie3.Add(comprasNoOptimas);
                    lstGpx_Compradores.Add(gpx_Compradores);
                }
                List<InfoGraficasDTO> lstTop20Compradores = new List<InfoGraficasDTO>();
                lstTop20Compradores = lstGpx_Compradores.Take(1).ToList();
                r.Add("lstGpx_Compradores", lstTop20Compradores);
                #endregion

                r.Add("seriesOptimoVsNoOptimo", series);
                //r.Add("pastel_noOptimoDetalle", infoCC_Numero.OrderBy(o => o.CC).ThenBy(o => o.NumeroRequisicion).ThenBy(o => o.Folio)); //CONSULTA APARTE
                //r.Add("pastel_optimaDetalle", infoCC_Numero_Optima.OrderBy(o => o.CC).ThenBy(o => o.NumeroRequisicion).ThenBy(o => o.Folio)); //CONSULTA APARTE
                r.Add("seriesTop10ProvOptimos", top10ProvOptimos);
                r.Add("seriesTop10ProvNoOptimos", top10ProvNoOptimos);
            }
            catch (Exception e)
            {
                LogError(0, 0, "CuadroComparativoController", "ConsultaDashboard", e, AccionEnum.CONSULTA, 0, 0);
            }
            return r;
        }

        public Dictionary<string, object> GetDetallesProveedoresOptimosVsNoOptimos(DateTime fechaInicio, DateTime fechaFin, List<int> proveedores, List<int> compradores)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                #region detalles proveedores optimos vs no optimos
                var comprasConCalificacion = _context.tblCom_CC_ProveedorNoOptimo.Where(w => (w.Fecha >= fechaInicio && w.Fecha <= fechaFin) && w.Estatus).ToList();
                var noOptimas = comprasConCalificacion.Where(w => w.IdTipoCalificacion == (int)TipoCalificacion.NoOptima).ToList();
                var optimas = comprasConCalificacion.Where(w => w.IdTipoCalificacion == (int)TipoCalificacion.Optima).ToList();
                var media = comprasConCalificacion.Where(w => w.IdTipoCalificacion == (int)TipoCalificacion.Media).ToList();

                List<tblCom_CC_Calificacion> lstCalificaciones = _context.Select<tblCom_CC_Calificacion>(new DapperDTO
                {
                    baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                    consulta = @"SELECT * FROM tblCom_CC_Calificacion WHERE esActivo = 1"
                }).ToList();

                var requisNoOptimas = noOptimas.GroupBy(g => g.CalificacionId).ToList();
                var infoCC_Numero = new List<DetalleOptimoVsOptimoDTO>();
                foreach (var requis in requisNoOptimas)
                {
                    var _num = requis.First().Calificacion.Numero;
                    var _cc = requis.First().Calificacion.CC;
                    var _numCompra = requis.First().NumeroCompra;
                    var calificaciones = lstCalificaciones.Where
                        (w =>
                            w.esActivo &&
                            w.Numero == _num &&
                            w.CC == _cc
                        ).Select(m => new DetalleOptimoVsOptimoDTO
                        {
                            Id = requis.Key,
                            CC = m.CC,
                            Folio = m.Folio,
                            NumeroRequisicion = m.Numero,
                            NumeroProveedor = m.Proveedor,
                            NumeroOrdenCompra = m.Id == requis.Key ? _numCompra : 0,
                            Calificacion = m
                        }).ToList();
                    infoCC_Numero.AddRange(calificaciones);
                }
                var requisMedia = media.GroupBy(g => g.CalificacionId).ToList();
                foreach (var requis in requisMedia)
                {
                    var _num = requis.First().Calificacion.Numero;
                    var _cc = requis.First().Calificacion.CC;
                    var _numCompra = requis.First().NumeroCompra;
                    var calificaciones = lstCalificaciones.Where
                        (w =>
                            w.esActivo &&
                            w.Numero == _num &&
                            w.CC == _cc
                        ).Select(m => new DetalleOptimoVsOptimoDTO
                        {
                            Id = requis.Key,
                            CC = m.CC,
                            Folio = m.Folio,
                            NumeroRequisicion = m.Numero,
                            NumeroProveedor = m.Proveedor,
                            NumeroOrdenCompra = m.Id == requis.Key ? _numCompra : 0,
                            Calificacion = m
                        }).ToList();
                    infoCC_Numero.AddRange(calificaciones);
                }
                var requisOptima = optimas.GroupBy(g => g.CalificacionId).ToList();
                var infoCC_Numero_Optima = new List<DetalleOptimoVsOptimoDTO>();
                foreach (var requis in requisOptima)
                {
                    var _num = requis.First().Calificacion.Numero;
                    var _cc = requis.First().Calificacion.CC;
                    var _numCompra = requis.First().NumeroCompra;
                    var calificaciones = lstCalificaciones.Where
                        (w =>
                            w.esActivo &&
                            w.Numero == _num &&
                            w.CC == _cc
                        ).Select(m => new DetalleOptimoVsOptimoDTO
                        {
                            Id = requis.Key,
                            CC = m.CC,
                            Folio = m.Folio,
                            NumeroRequisicion = m.Numero,
                            NumeroProveedor = m.Proveedor,
                            NumeroOrdenCompra = m.Id == requis.Key ? _numCompra : 0,
                            Calificacion = m
                        }).ToList();
                    infoCC_Numero_Optima.AddRange(calificaciones);
                }
                resultado.Add("pastel_optimaDetalle", infoCC_Numero_Optima.OrderBy(o => o.CC).ThenBy(o => o.NumeroRequisicion).ThenBy(o => o.Folio)); //CONSULTA APARTE
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, "OrdenCompraController", "GetDetallesProveedoresOptimosVsNoOptimos", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetDetallesTop10ProvNoOptimos(DateTime fechaInicio, DateTime fechaFin, List<int> proveedores, List<int> compradores)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                #region Detalles grafica pastel Top 10 proveedores no optimos
                List<tblCom_CC_Calificacion> lstCalificaciones = _context.Select<tblCom_CC_Calificacion>(new DapperDTO
                {
                    baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                    consulta = @"SELECT * FROM tblCom_CC_Calificacion WHERE esActivo = 1"
                }).ToList();

                var lstCalificacionesID = _context.tblCom_CC_ProveedorNoOptimo.Where(x => x.Estatus && x.IdTipoCalificacion == (int)TipoCalificacion.Optima && (x.Fecha >= fechaInicio && x.Fecha <= fechaFin)).Select(x => x.CalificacionId).ToList();
                var lstCalificacionesGrpID = lstCalificacionesID.GroupBy(x => x).ToList();
                var lstProveedoresID = lstCalificaciones.Where(x => lstCalificacionesID.Contains(x.Id) && x.esActivo).GroupBy(x => x.Proveedor).ToList();
                var _infoProveedores = InfoProveedores(proveedores);
                List<tblCom_CC_ProveedorNoOptimo> lstProveedoresNoOptimos = _context.tblCom_CC_ProveedorNoOptimo.Where(x => x.Estatus).ToList();

                lstCalificacionesID = _context.tblCom_CC_ProveedorNoOptimo.Where(x => x.Estatus && x.IdTipoCalificacion == (int)TipoCalificacion.NoOptima && (x.Fecha >= fechaInicio && x.Fecha <= fechaFin)).Select(x => x.CalificacionId).ToList();
                lstCalificacionesGrpID = lstCalificacionesID.GroupBy(x => x).ToList();
                lstProveedoresID = lstCalificaciones.Where(x => lstCalificacionesID.Contains(x.Id) && x.esActivo).GroupBy(x => x.Proveedor).ToList();

                var lstDetalleTop10ProveedoresNoOptimos = new List<DetalleTop10ProvDTO>();
                for (int i = 0; i < lstProveedoresID.Count(); i++)
                {
                    InfoProveedorDTO objProv = new InfoProveedorDTO();
                    string nombreProv = string.Empty;
                    if (_infoProveedores != null)
                    {
                        objProv = _infoProveedores.FirstOrDefault(f => f.Numero == lstProveedoresID[i].Key);
                        if (objProv != null)
                            nombreProv = objProv.Nombre;
                    }

                    var data = lstProveedoresID.Select(m => new DetalleTop10ProvDTO
                    {
                        //proveedor = InfoProveedor(lstProveedoresID[i].Key).Nombre,
                        proveedor = nombreProv,
                        proveedorID = lstProveedoresID[i].Key,
                        cantOC = CantOCProvOptimo(lstCalificacionesGrpID[i].Key, lstProveedoresNoOptimos)
                    }).ToList();
                    var t = data.First();
                    lstDetalleTop10ProveedoresNoOptimos.Add(t);
                }
                resultado.Add("lstDetalleTop10ProveedoresNoOptimos", lstDetalleTop10ProveedoresNoOptimos.Take(10)); // CONSULTA APARTE
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, "OrdenCompraController", "GetDetallesTop10ProvNoOptimos", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetDetallesTop10ProvOptimos(DateTime fechaInicio, DateTime fechaFin, List<int> proveedores, List<int> compradores)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                //SE OBTIENE LAS CalificacionID
                List<tblCom_CC_Calificacion> lstCalificaciones = _context.Select<tblCom_CC_Calificacion>(new DapperDTO
                {
                    baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                    consulta = @"SELECT * FROM tblCom_CC_Calificacion WHERE esActivo = 1"
                }).ToList();

                var _infoProveedores = InfoProveedores(proveedores);
                var lstCalificacionesID = _context.tblCom_CC_ProveedorNoOptimo.Where(x => x.Estatus && x.IdTipoCalificacion == (int)TipoCalificacion.Optima && (x.Fecha >= fechaInicio && x.Fecha <= fechaFin)).Select(x => x.CalificacionId).ToList();
                var lstCalificacionesGrpID = lstCalificacionesID.GroupBy(x => x).ToList();
                var lstProveedoresID = lstCalificaciones.Where(x => lstCalificacionesID.Contains(x.Id) && x.esActivo).GroupBy(x => x.Proveedor).ToList();
                List<tblCom_CC_ProveedorNoOptimo> lstProveedoresOptimos = _context.tblCom_CC_ProveedorNoOptimo.Where(x => x.Estatus).ToList();

                var lstDetalleTop10ProveedoresOptimos = new List<DetalleTop10ProvDTO>();
                for (int i = 0; i < lstProveedoresID.Count(); i++)
                {
                    InfoProveedorDTO objProv = new InfoProveedorDTO();
                    string nombreProv = string.Empty;
                    if (_infoProveedores != null)
                    {
                        objProv = _infoProveedores.FirstOrDefault(f => f.Numero == lstProveedoresID[i].Key);
                        if (objProv != null)
                            nombreProv = objProv.Nombre;
                    }

                    var data = lstProveedoresID.Select(m => new DetalleTop10ProvDTO
                    {
                        //proveedor = InfoProveedor(lstProveedoresID[i].Key).Nombre,
                        //proveedor = _infoProveedores.FirstOrDefault(f => f.Numero == lstProveedoresID[i].Key).Nombre,
                        proveedor = nombreProv,
                        proveedorID = lstProveedoresID[i].Key,
                        cantOC = CantOCProvOptimo(lstCalificacionesGrpID[i].Key, lstProveedoresOptimos)
                    }).ToList();
                    lstDetalleTop10ProveedoresOptimos.AddRange(data);
                }
                resultado.Add("lstDetalleTop10ProveedoresOptimos", lstDetalleTop10ProveedoresOptimos.Take(10)); //CONSULTA APARTE
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "OrdenCompraController", "GetDetallesTop10ProvOptimos", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetDetallesCalificaciones(DateTime fechaInicio, DateTime fechaFin, List<int> proveedores, List<int> compradores)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                #region Detalles grafica pastel Calificaciones
                List<int> lstCalificacionesID = _context.tblCom_CC_ProveedorNoOptimo.Where(x => x.Estatus && (x.Fecha >= fechaInicio && x.Fecha <= fechaFin)).Select(x => x.CalificacionId).ToList();
                var lstCalificacionesGrpID = lstCalificacionesID.GroupBy(x => x).ToList();

                List<tblCom_CC_Calificacion> lstCalificaciones = _context.Select<tblCom_CC_Calificacion>(new DapperDTO
                {
                    baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                    consulta = @"SELECT * FROM tblCom_CC_Calificacion WHERE esActivo = 1"
                }).ToList();
                var lstProveedores = lstCalificaciones.Where(x => lstCalificacionesID.Contains(x.Id) && x.esActivo).ToList();
                //var lstProveedores = _context.tblCom_CC_Calificacion.Where(x => x.esActivo).GroupBy(x => x.Proveedor).ToList();
                var lstOC = _context.tblCom_CC_ProveedorNoOptimo.Where(x => x.Estatus && (x.Fecha >= fechaInicio && x.Fecha <= fechaFin)).ToList();
                var _infoProveedores = InfoProveedores(proveedores);

                var lstDetalleCalificaciones = new List<DetalleCalificacionesDTO>();
                for (int i = 0; i < lstProveedores.Count(); i++)
                {
                    InfoProveedorDTO objProv = new InfoProveedorDTO();
                    string nombreProv = string.Empty;
                    if (_infoProveedores != null)
                    {
                        objProv = _infoProveedores.FirstOrDefault(f => f.Numero == lstProveedores[i].Proveedor);
                        if (objProv != null)
                            nombreProv = objProv.Nombre;
                    }

                    var data = lstProveedores.Select(m => new DetalleCalificacionesDTO
                    {
                        oc = lstOC[i].NumeroCompra,
                        requisicion = lstProveedores[i].Numero,
                        cc = lstProveedores[i].CC,
                        //proveedor = InfoProveedor(lstProveedores[i].Proveedor).Nombre,
                        proveedor = nombreProv,
                        fecha = lstOC[i].Fecha
                    }).ToList();
                    var t = data.First();
                    lstDetalleCalificaciones.Add(t);
                }
                resultado.Add("lstDetalleCalificaciones", lstDetalleCalificaciones); //CONSULTA APARTE
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, "OrdenCompraController", "GetDetallesCalificaciones", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        private int CantOCProvOptimo(int calificacionID, List<tblCom_CC_ProveedorNoOptimo> lstProveedoresNoOptimos)
        {
            int numOC = 0;
            if (lstProveedoresNoOptimos != null)
                numOC = lstProveedoresNoOptimos.Where(x => x.CalificacionId == calificacionID && x.Estatus).Count();
            else
                numOC = _context.tblCom_CC_ProveedorNoOptimo.Where(x => x.CalificacionId == calificacionID && x.Estatus).Count();

            return numOC;
        }

        public Dictionary<string, object> FillCboProveedores()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                //SE OBTIENE LOS PROVEEDORES CON LOS QUE SE HAN REALIZADO COMPRAS
                var objCalificacionesID = _context.tblCom_CC_ProveedorNoOptimo.Where(x => x.Estatus).Select(x => x.CalificacionId).ToList();
                var lstProveedores = _context.tblCom_CC_Calificacion.Where(x => x.esActivo && objCalificacionesID.Contains(x.Id)).GroupBy(x => x.Proveedor).ToList();

                var cboProveedores = new List<ComboDTO>();
                var cbo = new ComboDTO();

                for (int i = 0; i < lstProveedores.Count(); i++)
                {
                    cbo = new ComboDTO();
                    cbo.Value = lstProveedores[i].Key;
                    cbo.Text = lstProveedores[i].Key + " - " + InfoProveedor(lstProveedores[i].Key).Nombre;
                    cboProveedores.Add(cbo);
                }

                if (lstProveedores.Count() > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", cboProveedores);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboCompradores()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                //SE OBTIENE LOS PROVEEDORES QUE ESTAN REGISTRADOS EN EL CUADRO COMPARATIVO
                var lstUsuariosID = _context.tblCom_CC_ProveedorNoOptimo.Where(x => x.Estatus).Select(x => x.idUsuario).ToList();
                var lstCompradores = _context.tblP_Usuario.Where(x => lstUsuariosID.Contains(x.id)).Select(x => new
                {
                    Value = x.id,
                    Text = x.nombre + " " + x.apellidoPaterno
                }).ToList();

                if (lstCompradores.Count() > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", lstCompradores);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion

        public Respuesta darVoBoProvNoOptimo(List<CheckProvNoOptimoDTO> idNoOptimo)
        {
            var r = new Respuesta();

            var usuariosVoboNoOptimo = _context.tblCom_CC_PermisoVoboProvNoOptimo.Where(w => w.Estatus);

            if (idNoOptimo != null)
            {
                if (usuariosVoboNoOptimo.Select(m => m.UsuarioId).Contains(vSesiones.sesionUsuarioDTO.id))
                {
                    List<int> idsNoOptimos = idNoOptimo.Select(m => m.idVoboProvNoOptimo).ToList();
                    var noOptimos = _context.tblCom_CC_ProveedorNoOptimo.Where(w => idsNoOptimos.Contains(w.Id)).ToList();
                    foreach (var item in noOptimos)
                    {
                        var todosDeLaMismaOrden = _context.tblCom_CC_ProveedorNoOptimo.Where(w => w.Calificacion.CC == item.Calificacion.CC && w.Calificacion.Numero == item.Calificacion.Numero && w.NumeroCompra == item.NumeroCompra).ToList();
                        foreach (var noOptimo in todosDeLaMismaOrden)
                        {
                            noOptimo.VoBo = idNoOptimo.First(f => f.idVoboProvNoOptimo == item.Id).check;
                        }
                        _context.SaveChanges();
                    }

                    r.Success = true;
                }
                else
                {
                    r.Message = "No cuenta con permisos para aceptar esta orden";
                }
            }

            return r;
        }

        public bool GuardarConfiabilidad(CuadroComparativoDTO cuadro)
        {
            var cuadroGuardadoSigoplan = _context.tblCom_CC_Calificacion.Where(x => x.Numero == cuadro.numero && x.CC.ToUpper() == cuadro.cc.ToUpper() && x.Folio == cuadro.folio && x.esActivo).ToList();

            if (cuadroGuardadoSigoplan.Count > 0)
            {
                foreach (var item in cuadroGuardadoSigoplan)
                {
                    item.esActivo = false;

                    foreach (var partida in item.partidas)
                    {
                        partida.esActivo = false;
                        partida.fechaModificacion = DateTime.Now;
                    }
                }

                _context.SaveChanges();
            }

            var calificaciones = CalificarConfiabilidad(cuadro);

            foreach (var item in calificaciones)
            {
                item.Calificacion = item.Precio + item.TiempoEntrega + item.CondicionPago + item.LAB + item.ConfiabilidadProveedor + item.Calidad + item.ServicioPostVenta;
                item.registrar();

                var partidas = item.partidas;

                item.partidas = null;

                _context.tblCom_CC_Calificacion.Add(item);
                _context.SaveChanges();

                foreach (var partida in partidas)
                {
                    partida.idCalificacion = item.Id;
                }

                _context.tblCom_CC_CalificacionPartida.AddRange(partidas);
                _context.SaveChanges();
            }

            return true;
        }

        #region Calificadores
        public Dictionary<string, object> CalificarConfiabilidad(CuadroComparativoReporteDTO reporte, int numero)
        {
            var result = new Dictionary<string, object>();

            var cc = reporte.cc.Split('-')[0];
            var folio = (reporte.folioCuadroComparativo.Split('-')[2]).ParseInt();

            var calificacionesConfiabilidad = new List<tblCom_CC_Calificacion>();

            var infoConfiabilidad = ObtenerInfoConfiabilidad(numero, cc, folio);
            if (infoConfiabilidad.Count > 0)
            {
                calificacionesConfiabilidad = infoConfiabilidad;
            }
            else
            {
                var cuadro = ConsultarCuadro(cc, numero, folio);
                calificacionesConfiabilidad = CalificarConfiabilidad(cuadro);
            }

            var resultados = new decimal[3];
            var calificaciones = new List<CalificacionPartidaDTO>();

            var contador = 0;
            foreach (var item in calificacionesConfiabilidad)
            {
                resultados[contador] = Math.Round(item.Precio + item.TiempoEntrega + item.CondicionPago + item.LAB + item.ConfiabilidadProveedor + item.Calidad + item.ServicioPostVenta, 2);
                contador++;

                foreach (var partida in item.partidas)
                {
                    var _caliPartida = new CalificacionPartidaDTO();
                    _caliPartida.proveedor = item.Proveedor;
                    _caliPartida.partida = partida.numeroPartida;
                    _caliPartida.calificacion = Math.Round(partida.calificacion, 0);
                    calificaciones.Add(_caliPartida);
                }
            }

            result.Add("prov", resultados);
            result.Add("partidas", calificaciones);

            return result;
        }

        public List<tblCom_CC_Calificacion> CalificarConfiabilidad(CuadroComparativoDTO cuadro)
        {
            if ((MainContextEnum)vSesiones.sesionEmpresaActual != MainContextEnum.PERU)
            {
                #region RESTO EMPRESAS
                if (AsignarObjetoConfiabilidad(cuadro))
                {
                    var calificaciones = GenerarRegistroCalificacion(cuadro);

                    var resultado = new List<tblCom_CC_Calificacion>();

                    if (calificaciones.Count > 0)
                    {
                        CalificarPrecio(calificaciones);
                        CalificarTiempoEntrega(calificaciones, cuadro.fecha_requisicion);
                        CalificacionesEncuestasCompradorLogueado(calificaciones, (int)vSesiones.sesionUsuarioDTO.id);
                        //CalificacionesEncuestas(calificaciones);
                        CalificarCondicionPago(calificaciones);
                        CalificarLAB(calificaciones);

                        foreach (var item in calificaciones)
                        {
                            var calificacionProv = new tblCom_CC_Calificacion();

                            calificacionProv.Calidad = item.Calidad;
                            calificacionProv.TipoRequisicion = item.TipoRequisicion;
                            calificacionProv.CC = item.CC;
                            calificacionProv.CondicionPago = item.CondicionPago;
                            calificacionProv.ConfiabilidadProveedor = item.ConfiabilidadProveedor;
                            calificacionProv.Folio = item.Folio;
                            calificacionProv.Id = item.Id;
                            calificacionProv.LAB = item.LAB;
                            calificacionProv.Numero = item.Numero;
                            calificacionProv.PonderacionCalidad = item.PonderacionCalidad;
                            calificacionProv.PonderacionCondicionPago = item.PonderacionCondicionPago;
                            calificacionProv.PonderacionConfiabilidadProveedor = item.PonderacionConfiabilidadProveedor;
                            calificacionProv.PonderacionLAB = item.PonderacionLAB;
                            calificacionProv.PonderacionPrecio = item.PonderacionPrecio;
                            calificacionProv.PonderacionServicioPostVenta = item.PonderacionServicioPostVenta;
                            calificacionProv.PonderacionTiempoEntrega = item.PonderacionTiempoEntrega;
                            calificacionProv.Precio = item.partidas.Where(w => w.ValorPrecio != 0).Count() > 0 ? item.partidas.Sum(s => s.precio) / item.partidas.Where(w => w.ValorPrecio != 0).Count() : 0;
                            calificacionProv.Proveedor = item.Proveedor;
                            calificacionProv.ServicioPostVenta = item.ServicioPostVenta;
                            calificacionProv.TiempoEntrega = item.TiempoEntrega;
                            calificacionProv.TipoRequisicion = item.TipoRequisicion;
                            calificacionProv.registrar();

                            calificacionProv.partidas = new List<tblCom_CC_CalificacionPartida>();
                            foreach (var partida in item.partidas)
                            {
                                var _par = new tblCom_CC_CalificacionPartida();
                                _par.idCalificacion = calificacionProv.Id;
                                _par.numeroPartida = partida.partida;
                                _par.precio = partida.precio;

                                _par.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                _par.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                _par.fechaCreacion = DateTime.Now;
                                _par.fechaModificacion = _par.fechaCreacion;
                                _par.esActivo = true;

                                if (partida.ValorPrecio != 0)
                                {
                                    _par.tiempoEntrega = partida.tiempoEntrega;
                                    _par.condicionPago = partida.condicionPago;
                                    _par.LAB = partida.LAB;
                                    _par.confiabilidadProveedor = partida.confiabilidadProveedor;
                                    _par.calidad = partida.calidad;
                                    _par.servicioPostVenta = partida.servicioPostVenta;
                                }

                                _par.calificacion = _par.precio + _par.tiempoEntrega + _par.condicionPago + _par.LAB + _par.confiabilidadProveedor + _par.calidad + _par.servicioPostVenta;
                                calificacionProv.partidas.Add(_par);
                            }

                            resultado.Add(calificacionProv);
                        }

                        var partidas = new List<tblCom_CC_CalificacionPartida>();
                        foreach (var item in resultado)
                        {
                            partidas.AddRange(item.partidas);
                        }

                        foreach (var partida in partidas.GroupBy(g => g.numeroPartida))
                        {
                            var optimo = partida.Max(m => m.calificacion);
                            var bajo = partida.Min(m => m.calificacion);

                            foreach (var item in partida)
                            {
                                item.idTipoCalificacionPartida = item.calificacion == optimo ? (int)TipoCalificacion.Optima : item.calificacion == bajo ? (int)TipoCalificacion.NoOptima : (int)TipoCalificacion.Media;
                            }
                        }
                    }
                    else
                    {
                        return new List<tblCom_CC_Calificacion>();
                    }

                    return resultado;
                }
                else
                {
                    return new List<tblCom_CC_Calificacion>();
                }
                #endregion
            }
            else
            {
                #region PERU
                return new List<tblCom_CC_Calificacion>();
                
                #endregion
            }

        }

        private List<tblCom_CC_Calificacion> ObtenerInfoConfiabilidad(int numeroRequisicion, string cc, int folio)
        {
            var calificacion = _context.tblCom_CC_Calificacion.Where
                (w =>
                    w.Numero == numeroRequisicion &&
                    w.CC.ToUpper() == cc.ToUpper() &&
                    w.Folio == folio &&
                    w.esActivo
                ).ToList();

            return calificacion;
        }

        private void CalificarPrecio(List<CalificacionConfiabilidadDTO> calificaciones)
        {
            var partidas = new List<CalificacionConfiabilidadPartidaDTO>();
            foreach (var prov in calificaciones.Where(w => w.Proveedor != 0))
            {

                partidas.AddRange(prov.partidas);
            }

            foreach (var gbPartida in partidas.GroupBy(g => g.partida))
            {
                var menor = gbPartida.Where(w => w.ValorPrecio != 0).Count() > 0 ? gbPartida.Where(w => w.ValorPrecio != 0).Min(precio => precio.ValorPrecio) : 0M;

                menor = menor == 0 ? 1 : menor;

                foreach (var prov in calificaciones)
                {
                    foreach (var partida in prov.partidas.Where(w => w.partida == gbPartida.Key))
                    {
                        if (partida.ValorPrecio == 0)
                        {
                            partida.precio = 0;
                        }
                        else
                        {
                            var promedio = menor / partida.ValorPrecio;
                            partida.precio = promedio * confiabilidad.Precio;
                            partida.precio = decimal.Round(partida.precio, 6);
                        }
                    }

                    var partidaProv = prov.partidas.Where(w => w.partida == gbPartida.Key).ToList();
                    if (partidaProv.Count > 0)
                    {
                        prov.Precio = partidaProv.Sum(s => s.precio) / partidaProv.Count;
                    }
                    else
                    {
                        prov.Precio = 0;
                    }
                }
            }

            ////var menor = calificaciones.Where(w => w.Proveedor != 0 && w.ValorPrecio > 0).Min(precio => precio.ValorPrecio);
            //menor = menor == 0 ? 1 : menor;
            //calificaciones.ForEach(precio =>
            //{
            //    if (precio.ValorPrecio == 0)
            //    {
            //        precio.Precio = 0;
            //    }
            //    else
            //    {
            //        var promedio = menor / precio.ValorPrecio;
            //        precio.Precio = promedio * confiabilidad.Precio;
            //        precio.Precio = decimal.Round(precio.Precio, 6);
            //    }
            //});
        }

        private void CalificarTiempoEntrega(List<CalificacionConfiabilidadDTO> calificaciones, DateTime fechaRequisicion)
        {
            var menor = calificaciones.Where(w => w.Proveedor != 0 && w.ValorPrecio > 0).OrderBy(o => o.ValorTiempoEntrega).First().ValorTiempoEntrega;
            var valorMaximo = Infrastructure.Utils.DatetimeUtils.DiasDiferencia(menor, calificaciones.Where(w => w.Proveedor != 0 && w.ValorPrecio > 0).OrderBy(o => o.ValorTiempoEntrega).First().ValorTiempoEntrega);

            calificaciones.ForEach(entrega =>
            {
                if (entrega.ValorPrecio == 0)
                {
                    entrega.TiempoEntrega = 0;

                    foreach (var partida in entrega.partidas)
                    {
                        partida.tiempoEntrega = 0;
                    }
                }
                else
                {
                    var promedio = Infrastructure.Utils.DatetimeUtils.DiasDiferencia(menor, entrega.ValorTiempoEntrega);
                    entrega.TiempoEntrega = ((decimal)valorMaximo / (decimal)promedio) * confiabilidad.TiempoEntrega;
                    entrega.TiempoEntrega = decimal.Round(entrega.TiempoEntrega, 6);

                    foreach (var item in entrega.partidas)
                    {
                        item.tiempoEntrega = entrega.TiempoEntrega;
                    }
                }
            });
        }

        private void CalificarCondicionPago(List<CalificacionConfiabilidadDTO> calificaciones)
        {
            var maximo = calificaciones.OrderBy(o => o.ValorCondicionPago).Last().ValorCondicionPago;
            maximo = maximo == 0 ? 1 : maximo;
            calificaciones.ForEach(condicionPago =>
            {
                if (maximo == 0)
                {
                    condicionPago.CondicionPago = 0;

                    foreach (var partida in condicionPago.partidas)
                    {
                        partida.condicionPago = 0;
                    }
                }
                else
                {
                    var promedio = condicionPago.ValorCondicionPago / maximo;
                    condicionPago.CondicionPago = promedio * confiabilidad.CondicionPago;
                    condicionPago.CondicionPago = decimal.Round(condicionPago.CondicionPago, 6);

                    foreach (var partida in condicionPago.partidas)
                    {
                        partida.condicionPago = condicionPago.CondicionPago;
                    }
                }
            });
        }

        private void CalificarLAB(List<CalificacionConfiabilidadDTO> calificaciones)
        {
            calificaciones.ForEach(lab =>
            {
                lab.LAB = lab.ValorLAB != 1 && lab.ValorLAB != 2 ? 0M : confiabilidad.LAB;

                foreach (var partida in lab.partidas)
                {
                    partida.LAB = lab.LAB;
                }
            });
        }

        private void CalificacionesEncuestas(List<CalificacionConfiabilidadDTO> calificaciones)
        {
            using (var ctxCplan = new MainContext(EmpresaEnum.Construplan))
            {
                using (var ctxArre = new MainContext(EmpresaEnum.Arrendadora))
                {
                    var preguntasCalidadConstruplan = ctxCplan.tblEN_PreguntasProveedores.Where
                        (w =>
                            w.tipo == (int)TiposPreguntasEnum.CALIDAD &&
                            w.pregunta.ToUpper().Contains("PRODUCTOS/SERVICIOS CUMPLEN CON LA CALIDAD")
                        ).ToList();
                    var preguntasCalidadArre = ctxArre.tblEN_PreguntasProveedores.Where
                        (w =>
                            w.tipo == (int)TiposPreguntasEnum.CALIDAD &&
                            w.pregunta.ToUpper().Contains("PRODUCTOS/SERVICIOS CUMPLEN CON LA CALIDAD")
                        ).ToList();

                    var preguntasPostVentaConstruplan = ctxCplan.tblEN_PreguntasProveedores.Where
                        (w =>
                            w.tipo == (int)TiposPreguntasEnum.ATENCION &&
                            w.pregunta.ToUpper().Contains("POST-VENTA")
                        ).ToList();
                    var preguntasPostVentaArre = ctxArre.tblEN_PreguntasProveedores.Where
                        (w =>
                            w.tipo == (int)TiposPreguntasEnum.ATENCION &&
                            w.pregunta.ToUpper().Contains("POST-VENTA")
                        ).ToList();

                    foreach (var item in calificaciones)
                    {
                        if (item.ValorPrecio == 0 || item.Proveedor == 0)
                        {
                            item.ConfiabilidadProveedor = 0;
                            item.Calidad = 0;
                            item.ServicioPostVenta = 0;
                        }
                        else
                        {
                            var numeroProveedor = item.Proveedor;

                            var encuestasCplan = ctxCplan.tblEN_ResultadoProveedoresDet.Where
                                (w =>
                                    w.estadoEncuesta &&
                                    w.calificacion != null &&
                                    w.calificacion <= 100 &&
                                    w.numProveedor != 0 &&
                                    w.numProveedor == numeroProveedor
                                ).ToList();
                            var encuestasCplan_requi = ctxCplan.tblEN_ResultadoProveedorRequisicionDet.Where
                                (w =>
                                    w.estatus &&
                                    w.calificacion != null &&
                                    w.calificacion <= 100 &&
                                    w.numProveedor != 0 &&
                                    w.numProveedor == numeroProveedor
                                ).ToList();
                            var encuestasArre = ctxArre.tblEN_ResultadoProveedoresDet.Where
                                (w =>
                                    w.estadoEncuesta &&
                                    w.calificacion != null &&
                                    w.calificacion <= 100 &&
                                    w.numProveedor != 0 &&
                                    w.numProveedor == numeroProveedor
                                ).ToList();
                            var encuestasArre_requi = ctxArre.tblEN_ResultadoProveedorRequisicionDet.Where
                                (w =>
                                    w.estatus &&
                                    w.calificacion != null &&
                                    w.calificacion <= 100 &&
                                    w.numProveedor != 0 &&
                                    w.numProveedor == numeroProveedor
                                ).ToList();

                            var _encuestasTotales =
                                    encuestasCplan.Count +
                                    encuestasCplan_requi.Count +
                                    encuestasArre.Count +
                                    encuestasArre_requi.Count;

                            //RESULTADO CONFIABILIDAD
                            var calificacionConfiabilidad = _encuestasTotales == 0 ? 0 :
                                (
                                    encuestasCplan.Sum(s => s.calificacion.Value) +
                                    encuestasCplan_requi.Sum(s => s.calificacion.Value) +
                                    encuestasArre.Sum(s => s.calificacion.Value) +
                                    encuestasArre_requi.Sum(s => s.calificacion.Value)
                                ) /
                                (
                                    encuestasCplan.Count +
                                    encuestasCplan_requi.Count +
                                    encuestasArre.Count +
                                    encuestasArre_requi.Count
                                );
                            item.ConfiabilidadProveedor = (calificacionConfiabilidad * confiabilidad.ConfiabilidadProveedor) / 100;

                            var totalPonderacionCalidad = 0M;
                            var cantidadEncuestasCalidad = 0;
                            foreach (var encuesta in encuestasCplan)
                            {
                                var cantidadPreguntasEncuesta = 0;
                                var _cali = 0.0M;
                                foreach (var pregunta in encuesta.detalles.Where(w => w.pregunta.tipo == (int)TiposPreguntasEnum.CALIDAD && preguntasCalidadConstruplan.Select(m => m.id).Contains(w.preguntaID)))
                                {
                                    _cali += pregunta.calificacionPonderacion.Value / pregunta.pregunta.ponderacion;
                                    cantidadPreguntasEncuesta++;
                                }

                                if (cantidadPreguntasEncuesta > 0)
                                {
                                    totalPonderacionCalidad += (_cali / cantidadPreguntasEncuesta);
                                    cantidadEncuestasCalidad++;
                                }
                            }

                            foreach (var encuesta in encuestasCplan_requi)
                            {
                                var cantidadPreguntasEncuesta = 0;
                                var _cali = 0.0M;
                                foreach (var pregunta in encuesta.detalles.Where(w => w.pregunta.tipo == (int)TiposPreguntasEnum.CALIDAD && preguntasCalidadConstruplan.Select(m => m.id).Contains(w.preguntaID)))
                                {
                                    _cali += pregunta.calificacionPonderacion.Value / pregunta.pregunta.ponderacion;
                                    cantidadPreguntasEncuesta++;
                                }

                                if (cantidadPreguntasEncuesta > 0)
                                {
                                    totalPonderacionCalidad += (_cali / cantidadPreguntasEncuesta);
                                    cantidadEncuestasCalidad++;
                                }
                            }

                            foreach (var encuesta in encuestasArre)
                            {
                                var cantidadPreguntasEncuesta = 0;
                                var _cali = 0.0M;
                                foreach (var pregunta in encuesta.detalles.Where(w => w.pregunta.tipo == (int)TiposPreguntasEnum.CALIDAD && preguntasCalidadArre.Select(m => m.id).Contains(w.preguntaID)))
                                {
                                    _cali += pregunta.calificacionPonderacion.Value / pregunta.pregunta.ponderacion;
                                    cantidadPreguntasEncuesta++;
                                }

                                if (cantidadPreguntasEncuesta > 0)
                                {
                                    totalPonderacionCalidad += (_cali / cantidadPreguntasEncuesta);
                                    cantidadEncuestasCalidad++;
                                }
                            }

                            foreach (var encuesta in encuestasArre_requi)
                            {
                                var cantidadPreguntasEncuesta = 0;
                                var _cali = 0.0M;
                                foreach (var pregunta in encuesta.detalles.Where(w => w.pregunta.tipo == (int)TiposPreguntasEnum.CALIDAD && preguntasCalidadArre.Select(m => m.id).Contains(w.preguntaID)))
                                {
                                    _cali += pregunta.calificacionPonderacion.Value / pregunta.pregunta.ponderacion;
                                    cantidadPreguntasEncuesta++;
                                }

                                if (cantidadPreguntasEncuesta > 0)
                                {
                                    totalPonderacionCalidad += (_cali / cantidadPreguntasEncuesta);
                                    cantidadEncuestasCalidad++;
                                }
                            }

                            //RESULTADO CALIDAD
                            var calificacionCalidad = cantidadEncuestasCalidad > 0 ? totalPonderacionCalidad / cantidadEncuestasCalidad : 0.0M;
                            item.Calidad = (calificacionConfiabilidad * confiabilidad.Calidad) / 100;

                            var totalPonderacionPostVenta = 0M;
                            var cantidadEncuestasPostVenta = 0;
                            foreach (var encuesta in encuestasCplan)
                            {
                                var cantidadPreguntasEncuesta = 0;
                                var _cali = 0.0M;
                                foreach (var pregunta in encuesta.detalles.Where(w => w.pregunta.tipo == (int)TiposPreguntasEnum.ATENCION && preguntasPostVentaConstruplan.Select(m => m.id).Contains(w.preguntaID)))
                                {
                                    _cali += pregunta.calificacionPonderacion.Value / pregunta.pregunta.ponderacion;
                                    cantidadPreguntasEncuesta++;
                                }

                                if (cantidadPreguntasEncuesta > 0)
                                {
                                    totalPonderacionPostVenta += (_cali / cantidadPreguntasEncuesta);
                                    cantidadEncuestasPostVenta++;
                                }
                            }

                            foreach (var encuesta in encuestasCplan_requi)
                            {
                                var cantidadPreguntasEncuesta = 0;
                                var _cali = 0.0M;
                                foreach (var pregunta in encuesta.detalles.Where(w => w.pregunta.tipo == (int)TiposPreguntasEnum.ATENCION && preguntasPostVentaConstruplan.Select(m => m.id).Contains(w.preguntaID)))
                                {
                                    _cali += pregunta.calificacionPonderacion.Value / pregunta.pregunta.ponderacion;
                                    cantidadPreguntasEncuesta++;
                                }

                                if (cantidadPreguntasEncuesta > 0)
                                {
                                    totalPonderacionPostVenta += (_cali / cantidadPreguntasEncuesta);
                                    cantidadEncuestasPostVenta++;
                                }
                            }

                            foreach (var encuesta in encuestasArre)
                            {
                                var cantidadPreguntasEncuesta = 0;
                                var _cali = 0.0M;
                                foreach (var pregunta in encuesta.detalles.Where(w => w.pregunta.tipo == (int)TiposPreguntasEnum.ATENCION && preguntasPostVentaConstruplan.Select(m => m.id).Contains(w.preguntaID)))
                                {
                                    _cali += pregunta.calificacionPonderacion.Value / pregunta.pregunta.ponderacion;
                                    cantidadPreguntasEncuesta++;
                                }

                                if (cantidadPreguntasEncuesta > 0)
                                {
                                    totalPonderacionPostVenta += (_cali / cantidadPreguntasEncuesta);
                                    cantidadEncuestasPostVenta++;
                                }
                            }

                            foreach (var encuesta in encuestasArre_requi)
                            {
                                var cantidadPreguntasEncuesta = 0;
                                var _cali = 0.0M;
                                foreach (var pregunta in encuesta.detalles.Where(w => w.pregunta.tipo == (int)TiposPreguntasEnum.ATENCION && preguntasPostVentaConstruplan.Select(m => m.id).Contains(w.preguntaID)))
                                {
                                    _cali += pregunta.calificacionPonderacion.Value / pregunta.pregunta.ponderacion;
                                    cantidadPreguntasEncuesta++;
                                }

                                if (cantidadPreguntasEncuesta > 0)
                                {
                                    totalPonderacionPostVenta += (_cali / cantidadPreguntasEncuesta);
                                    cantidadEncuestasPostVenta++;
                                }
                            }

                            //RESULTADO POST-VENTA
                            var calificacionPostVenta = cantidadEncuestasPostVenta > 0 ? totalPonderacionPostVenta / cantidadEncuestasPostVenta : 0.0M;
                            item.ServicioPostVenta = (calificacionPostVenta * confiabilidad.ServicioPostVenta) / 100;
                        }
                    }
                }
            }
        }

        private void CalificacionesEncuestasCompradorLogueado(List<CalificacionConfiabilidadDTO> calificaciones, int idComprador) // OMAR
        {
            using (var ctxCplan = new MainContext(EmpresaEnum.Construplan))
            {
                using (var ctxArre = new MainContext(EmpresaEnum.Arrendadora))
                {
                    var preguntasCalidadConstruplan = ctxCplan.tblEN_PreguntasProveedores.Where
                        (w =>
                            w.tipo == (int)TiposPreguntasEnum.CALIDAD &&
                            w.pregunta.ToUpper().Contains("PRODUCTOS/SERVICIOS CUMPLEN CON LA CALIDAD")
                        ).ToList();
                    var preguntasCalidadArre = ctxArre.tblEN_PreguntasProveedores.Where
                        (w =>
                            w.tipo == (int)TiposPreguntasEnum.CALIDAD &&
                            w.pregunta.ToUpper().Contains("PRODUCTOS/SERVICIOS CUMPLEN CON LA CALIDAD")
                        ).ToList();

                    var preguntasPostVentaConstruplan = ctxCplan.tblEN_PreguntasProveedores.Where
                        (w =>
                            w.tipo == (int)TiposPreguntasEnum.ATENCION &&
                            w.pregunta.ToUpper().Contains("POST-VENTA")
                        ).ToList();
                    var preguntasPostVentaArre = ctxArre.tblEN_PreguntasProveedores.Where
                        (w =>
                            w.tipo == (int)TiposPreguntasEnum.ATENCION &&
                            w.pregunta.ToUpper().Contains("POST-VENTA")
                        ).ToList();

                    foreach (var item in calificaciones)
                    {
                        if (item.ValorPrecio == 0 || item.Proveedor == 0)
                        {
                            item.ConfiabilidadProveedor = 0;
                            item.Calidad = 0;
                            item.ServicioPostVenta = 0;

                            foreach (var partida in item.partidas)
                            {
                                partida.confiabilidadProveedor = 0;
                                partida.calidad = 0;
                                partida.servicioPostVenta = 0;
                            }
                        }
                        else
                        {
                            var numeroProveedor = item.Proveedor;

                            var encuestasCplan = ctxCplan.tblEN_ResultadoProveedoresDet.Where
                                (w =>
                                    w.estadoEncuesta &&
                                    w.calificacion != null &&
                                    w.calificacion <= 100 &&
                                    w.numProveedor != 0 &&
                                    w.numProveedor == numeroProveedor &&
                                    w.evaluadorID == idComprador
                                ).ToList();
                            var encuestasCplan_requi = ctxCplan.tblEN_ResultadoProveedorRequisicionDet.Where
                                (w =>
                                    w.estatus &&
                                    w.calificacion != null &&
                                    w.calificacion <= 100 &&
                                    w.numProveedor != 0 &&
                                    w.numProveedor == numeroProveedor &&
                                    w.evaluadorID == idComprador
                                ).ToList();
                            var encuestasArre = ctxArre.tblEN_ResultadoProveedoresDet.Where
                                (w =>
                                    w.estadoEncuesta &&
                                    w.calificacion != null &&
                                    w.calificacion <= 100 &&
                                    w.numProveedor != 0 &&
                                    w.numProveedor == numeroProveedor &&
                                    w.evaluadorID == idComprador
                                ).ToList();
                            var encuestasArre_requi = ctxArre.tblEN_ResultadoProveedorRequisicionDet.Where
                                (w =>
                                    w.estatus &&
                                    w.calificacion != null &&
                                    w.calificacion <= 100 &&
                                    w.numProveedor != 0 &&
                                    w.numProveedor == numeroProveedor &&
                                    w.evaluadorID == idComprador
                                ).ToList();

                            var _encuestasTotales =
                                    encuestasCplan.Count +
                                    encuestasCplan_requi.Count +
                                    encuestasArre.Count +
                                    encuestasArre_requi.Count;

                            //RESULTADO CONFIABILIDAD
                            var calificacionConfiabilidad = _encuestasTotales == 0 ? 0 :
                                (
                                    encuestasCplan.Sum(s => s.calificacion.Value) +
                                    encuestasCplan_requi.Sum(s => s.calificacion.Value) +
                                    encuestasArre.Sum(s => s.calificacion.Value) +
                                    encuestasArre_requi.Sum(s => s.calificacion.Value)
                                ) /
                                (
                                    encuestasCplan.Count +
                                    encuestasCplan_requi.Count +
                                    encuestasArre.Count +
                                    encuestasArre_requi.Count
                                );
                            item.ConfiabilidadProveedor = (calificacionConfiabilidad * confiabilidad.ConfiabilidadProveedor) / 100;

                            var totalPonderacionCalidad = 0M;
                            var cantidadEncuestasCalidad = 0;
                            foreach (var encuesta in encuestasCplan)
                            {
                                var cantidadPreguntasEncuesta = 0;
                                var _cali = 0.0M;
                                foreach (var pregunta in encuesta.detalles.Where(w => w.pregunta.tipo == (int)TiposPreguntasEnum.CALIDAD && preguntasCalidadConstruplan.Select(m => m.id).Contains(w.preguntaID)))
                                {
                                    _cali += pregunta.calificacionPonderacion.Value / pregunta.pregunta.ponderacion;
                                    cantidadPreguntasEncuesta++;
                                }

                                if (cantidadPreguntasEncuesta > 0)
                                {
                                    totalPonderacionCalidad += (_cali / cantidadPreguntasEncuesta);
                                    cantidadEncuestasCalidad++;
                                }
                            }

                            foreach (var encuesta in encuestasCplan_requi)
                            {
                                var cantidadPreguntasEncuesta = 0;
                                var _cali = 0.0M;
                                foreach (var pregunta in encuesta.detalles.Where(w => w.pregunta.tipo == (int)TiposPreguntasEnum.CALIDAD && preguntasCalidadConstruplan.Select(m => m.id).Contains(w.preguntaID)))
                                {
                                    _cali += pregunta.calificacionPonderacion.Value / pregunta.pregunta.ponderacion;
                                    cantidadPreguntasEncuesta++;
                                }

                                if (cantidadPreguntasEncuesta > 0)
                                {
                                    totalPonderacionCalidad += (_cali / cantidadPreguntasEncuesta);
                                    cantidadEncuestasCalidad++;
                                }
                            }

                            foreach (var encuesta in encuestasArre)
                            {
                                var cantidadPreguntasEncuesta = 0;
                                var _cali = 0.0M;
                                foreach (var pregunta in encuesta.detalles.Where(w => w.pregunta.tipo == (int)TiposPreguntasEnum.CALIDAD && preguntasCalidadArre.Select(m => m.id).Contains(w.preguntaID)))
                                {
                                    _cali += pregunta.calificacionPonderacion.Value / pregunta.pregunta.ponderacion;
                                    cantidadPreguntasEncuesta++;
                                }

                                if (cantidadPreguntasEncuesta > 0)
                                {
                                    totalPonderacionCalidad += (_cali / cantidadPreguntasEncuesta);
                                    cantidadEncuestasCalidad++;
                                }
                            }

                            foreach (var encuesta in encuestasArre_requi)
                            {
                                var cantidadPreguntasEncuesta = 0;
                                var _cali = 0.0M;
                                foreach (var pregunta in encuesta.detalles.Where(w => w.pregunta.tipo == (int)TiposPreguntasEnum.CALIDAD && preguntasCalidadArre.Select(m => m.id).Contains(w.preguntaID)))
                                {
                                    _cali += pregunta.calificacionPonderacion.Value / pregunta.pregunta.ponderacion;
                                    cantidadPreguntasEncuesta++;
                                }

                                if (cantidadPreguntasEncuesta > 0)
                                {
                                    totalPonderacionCalidad += (_cali / cantidadPreguntasEncuesta);
                                    cantidadEncuestasCalidad++;
                                }
                            }

                            //RESULTADO CALIDAD
                            var calificacionCalidad = cantidadEncuestasCalidad > 0 ? totalPonderacionCalidad / cantidadEncuestasCalidad : 0.0M;
                            item.Calidad = (calificacionConfiabilidad * confiabilidad.Calidad) / 100;

                            var totalPonderacionPostVenta = 0M;
                            var cantidadEncuestasPostVenta = 0;
                            foreach (var encuesta in encuestasCplan)
                            {
                                var cantidadPreguntasEncuesta = 0;
                                var _cali = 0.0M;
                                foreach (var pregunta in encuesta.detalles.Where(w => w.pregunta.tipo == (int)TiposPreguntasEnum.ATENCION && preguntasPostVentaConstruplan.Select(m => m.id).Contains(w.preguntaID)))
                                {
                                    _cali += pregunta.calificacionPonderacion.Value / pregunta.pregunta.ponderacion;
                                    cantidadPreguntasEncuesta++;
                                }

                                if (cantidadPreguntasEncuesta > 0)
                                {
                                    totalPonderacionPostVenta += (_cali / cantidadPreguntasEncuesta);
                                    cantidadEncuestasPostVenta++;
                                }
                            }

                            foreach (var encuesta in encuestasCplan_requi)
                            {
                                var cantidadPreguntasEncuesta = 0;
                                var _cali = 0.0M;
                                foreach (var pregunta in encuesta.detalles.Where(w => w.pregunta.tipo == (int)TiposPreguntasEnum.ATENCION && preguntasPostVentaConstruplan.Select(m => m.id).Contains(w.preguntaID)))
                                {
                                    _cali += pregunta.calificacionPonderacion.Value / pregunta.pregunta.ponderacion;
                                    cantidadPreguntasEncuesta++;
                                }

                                if (cantidadPreguntasEncuesta > 0)
                                {
                                    totalPonderacionPostVenta += (_cali / cantidadPreguntasEncuesta);
                                    cantidadEncuestasPostVenta++;
                                }
                            }

                            foreach (var encuesta in encuestasArre)
                            {
                                var cantidadPreguntasEncuesta = 0;
                                var _cali = 0.0M;
                                foreach (var pregunta in encuesta.detalles.Where(w => w.pregunta.tipo == (int)TiposPreguntasEnum.ATENCION && preguntasPostVentaConstruplan.Select(m => m.id).Contains(w.preguntaID)))
                                {
                                    _cali += pregunta.calificacionPonderacion.Value / pregunta.pregunta.ponderacion;
                                    cantidadPreguntasEncuesta++;
                                }

                                if (cantidadPreguntasEncuesta > 0)
                                {
                                    totalPonderacionPostVenta += (_cali / cantidadPreguntasEncuesta);
                                    cantidadEncuestasPostVenta++;
                                }
                            }

                            foreach (var encuesta in encuestasArre_requi)
                            {
                                var cantidadPreguntasEncuesta = 0;
                                var _cali = 0.0M;
                                foreach (var pregunta in encuesta.detalles.Where(w => w.pregunta.tipo == (int)TiposPreguntasEnum.ATENCION && preguntasPostVentaConstruplan.Select(m => m.id).Contains(w.preguntaID)))
                                {
                                    _cali += pregunta.calificacionPonderacion.Value / pregunta.pregunta.ponderacion;
                                    cantidadPreguntasEncuesta++;
                                }

                                if (cantidadPreguntasEncuesta > 0)
                                {
                                    totalPonderacionPostVenta += (_cali / cantidadPreguntasEncuesta);
                                    cantidadEncuestasPostVenta++;
                                }
                            }

                            //RESULTADO POST-VENTA
                            var calificacionPostVenta = cantidadEncuestasPostVenta > 0 ? totalPonderacionPostVenta / cantidadEncuestasPostVenta : 0.0M;
                            item.ServicioPostVenta = (calificacionPostVenta * confiabilidad.ServicioPostVenta) / 100;

                            foreach (var partida in item.partidas)
                            {
                                partida.confiabilidadProveedor = item.ConfiabilidadProveedor;
                                partida.calidad = item.Calidad;
                                partida.servicioPostVenta = item.ServicioPostVenta;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        private List<CalificacionConfiabilidadDTO> GenerarRegistroCalificacion(CuadroComparativoDTO cuadro)
        {
            var lst = new List<CalificacionConfiabilidadDTO>();
            if (cuadro.prov1.HasValue && cuadro.prov1.Value > 0 && cuadro.total1 > 0)
            {
                var cal = new CalificacionConfiabilidadDTO();
                cal.TipoRequisicion = confiabilidad.TipoRequisicion;
                cal.Numero = cuadro.numero;
                cal.CC = cuadro.cc;
                cal.Folio = cuadro.folio;
                cal.Proveedor = cuadro.prov1.GetValueOrDefault();
                cal.ValorPrecio = cuadro.total1;
                cal.ValorTiempoEntrega = cuadro.fecha_entrega1;
                cal.ValorCondicionPago = cuadro.dias_pago1.GetValueOrDefault();
                cal.ValorLAB = cuadro.lab1.GetValueOrDefault();
                cal.PonderacionPrecio = confiabilidad.Precio;
                cal.PonderacionTiempoEntrega = confiabilidad.TiempoEntrega;
                cal.PonderacionCondicionPago = confiabilidad.CondicionPago;
                cal.PonderacionLAB = confiabilidad.LAB;
                cal.PonderacionConfiabilidadProveedor = confiabilidad.ConfiabilidadProveedor;
                cal.PonderacionCalidad = confiabilidad.Calidad;
                cal.PonderacionServicioPostVenta = confiabilidad.ServicioPostVenta;
                cal.partidas = new List<CalificacionConfiabilidadPartidaDTO>();

                foreach (var detalle in cuadro.detalleCuadro)
                {
                    var calDetalle = new CalificacionConfiabilidadPartidaDTO();
                    calDetalle.partida = detalle.partida;
                    calDetalle.ValorPrecio = detalle.precio1;

                    cal.partidas.Add(calDetalle);
                }

                lst.Add(cal);
            }
            else
            {
                var cal = new CalificacionConfiabilidadDTO();
                cal.TipoRequisicion = confiabilidad.TipoRequisicion;
                cal.Numero = cuadro.numero;
                cal.CC = cuadro.cc;
                cal.Folio = cuadro.folio;
                cal.partidas = new List<CalificacionConfiabilidadPartidaDTO>();

                lst.Add(cal);
            }

            if (cuadro.prov2.HasValue && cuadro.prov2.Value > 0 && cuadro.total2 > 0)
            {
                var cal = new CalificacionConfiabilidadDTO();
                cal.TipoRequisicion = confiabilidad.TipoRequisicion;
                cal.Numero = cuadro.numero;
                cal.CC = cuadro.cc;
                cal.Folio = cuadro.folio;
                cal.Proveedor = cuadro.prov2.GetValueOrDefault();
                cal.ValorPrecio = cuadro.total2;
                cal.ValorTiempoEntrega = cuadro.fecha_entrega2;
                cal.ValorCondicionPago = cuadro.dias_pago2.GetValueOrDefault();
                cal.ValorLAB = cuadro.lab2.GetValueOrDefault();
                cal.PonderacionPrecio = confiabilidad.Precio;
                cal.PonderacionTiempoEntrega = confiabilidad.TiempoEntrega;
                cal.PonderacionCondicionPago = confiabilidad.CondicionPago;
                cal.PonderacionLAB = confiabilidad.LAB;
                cal.PonderacionConfiabilidadProveedor = confiabilidad.ConfiabilidadProveedor;
                cal.PonderacionCalidad = confiabilidad.Calidad;
                cal.PonderacionServicioPostVenta = confiabilidad.ServicioPostVenta;
                cal.partidas = new List<CalificacionConfiabilidadPartidaDTO>();

                foreach (var detalle in cuadro.detalleCuadro)
                {
                    var calDetalle = new CalificacionConfiabilidadPartidaDTO();
                    calDetalle.partida = detalle.partida;
                    calDetalle.ValorPrecio = detalle.precio2;

                    cal.partidas.Add(calDetalle);
                }

                lst.Add(cal);
            }
            else
            {
                var cal = new CalificacionConfiabilidadDTO();
                cal.TipoRequisicion = confiabilidad.TipoRequisicion;
                cal.Numero = cuadro.numero;
                cal.CC = cuadro.cc;
                cal.Folio = cuadro.folio;
                cal.partidas = new List<CalificacionConfiabilidadPartidaDTO>();

                lst.Add(cal);
            }
            if (cuadro.prov3.HasValue && cuadro.prov3.Value > 0 && cuadro.total3 > 0)
            {
                var cal = new CalificacionConfiabilidadDTO();
                cal.TipoRequisicion = confiabilidad.TipoRequisicion;
                cal.Numero = cuadro.numero;
                cal.CC = cuadro.cc;
                cal.Folio = cuadro.folio;
                cal.Proveedor = cuadro.prov3.GetValueOrDefault();
                cal.ValorPrecio = cuadro.total3;
                cal.ValorTiempoEntrega = cuadro.fecha_entrega3;
                cal.ValorCondicionPago = cuadro.dias_pago3.GetValueOrDefault();
                cal.ValorLAB = cuadro.lab3.GetValueOrDefault();
                cal.PonderacionPrecio = confiabilidad.Precio;
                cal.PonderacionTiempoEntrega = confiabilidad.TiempoEntrega;
                cal.PonderacionCondicionPago = confiabilidad.CondicionPago;
                cal.PonderacionLAB = confiabilidad.LAB;
                cal.PonderacionConfiabilidadProveedor = confiabilidad.ConfiabilidadProveedor;
                cal.PonderacionCalidad = confiabilidad.Calidad;
                cal.PonderacionServicioPostVenta = confiabilidad.ServicioPostVenta;
                cal.partidas = new List<CalificacionConfiabilidadPartidaDTO>();

                foreach (var detalle in cuadro.detalleCuadro)
                {
                    var calDetalle = new CalificacionConfiabilidadPartidaDTO();
                    calDetalle.partida = detalle.partida;
                    calDetalle.ValorPrecio = detalle.precio3;

                    cal.partidas.Add(calDetalle);
                }

                lst.Add(cal);
            }
            else
            {
                var cal = new CalificacionConfiabilidadDTO();
                cal.TipoRequisicion = confiabilidad.TipoRequisicion;
                cal.Numero = cuadro.numero;
                cal.CC = cuadro.cc;
                cal.Folio = cuadro.folio;
                cal.partidas = new List<CalificacionConfiabilidadPartidaDTO>();

                lst.Add(cal);
            }

            return lst;
        }

        private bool AsignarObjetoConfiabilidad(CuadroComparativoDTO cuadro)
        {
            var requisicion = ConsultarRequisicion(cuadro.cc, cuadro.numero);
            var tipoRequisicion = requisicion.tipo_req_oc.ParseInt();
            if (tipoRequisicion != tipoRequisicionReembolso)
            {
                confiabilidad = ConsultarConfiabilidad(tipoRequisicion);
                return confiabilidad != null;
            }
            else
            {
                return false;
            }
        }

        private RequisicionDTO ConsultarRequisicion(string cc, int numero)
        {
            var consulta = new OdbcConsultaDTO
            {
                consulta = @"SELECT cc, numero, fecha, libre_abordo, tipo_req_oc, solicito, vobo, autorizo, comentarios, st_estatus, st_impresa, st_autoriza, emp_autoriza, fecha_autoriza, tmc, autoriza_activos, num_vobo
                            FROM so_requisicion WHERE cc = ? AND numero = ?",
                parametros = new List<OdbcParameterDTO>
                {
                    new OdbcParameterDTO { nombre = "cc", tipo = OdbcType.VarChar, valor = cc },
                    new OdbcParameterDTO { nombre = "numero", tipo = OdbcType.Numeric, valor = numero }
                }
            };
            return _contextEnkontrol.Select<RequisicionDTO>(productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, consulta).FirstOrDefault();
        }

        private tblCom_CC_CatConfiabilidad ConsultarConfiabilidad(int tipoRequisicion)
        {
            return (from cat in _context.tblCom_CC_CatConfiabilidad
                    where cat.TipoRequisicion == tipoRequisicion
                    select cat).FirstOrDefault();
        }

        private CuadroComparativoDTO ConsultarCuadro(string cc, int numero, int folio)
        {
            var consulta = new OdbcConsultaDTO
            {
                consulta = "SELECT * FROM so_cuadro_comparativo WHERE cc = ? AND numero = ? AND folio = ?",
                parametros = new List<OdbcParameterDTO>
                {
                    new OdbcParameterDTO { nombre = "cc", tipo = OdbcType.Char, valor = cc },
                    new OdbcParameterDTO { nombre = "numero", tipo = OdbcType.Numeric, valor = numero },
                    new OdbcParameterDTO { nombre = "folio", tipo = OdbcType.Numeric, valor = folio }
                }
            };
            var cuadro = _contextEnkontrol.Select<CuadroComparativoDTO>(productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, consulta).FirstOrDefault();
            return cuadro;
        }

        private InfoProveedorDTO InfoProveedor(int numeroProveedor)
        {
            var consultaProvODBC = new OdbcConsultaDTO();

            consultaProvODBC.consulta = "SELECT numpro as Numero, nomcorto as NombreCorto, nombre FROM sp_proveedores WHERE numpro = ?";
            //consultaProvODBC.consulta = "SELECT nombre FROM sp_proveedores WHERE numpro = ?";
            consultaProvODBC.parametros.Add(new OdbcParameterDTO
            {
                nombre = "numpro",
                tipo = OdbcType.Int,
                valor = numeroProveedor
            });
            var _prov = _contextEnkontrol.Select<InfoProveedorDTO>(EnkontrolAmbienteEnum.Prod, consultaProvODBC).FirstOrDefault();

            return _prov;
        }

        private List<InfoProveedorDTO> InfoProveedores(List<int> numeroProveedores)
        {
            var consultaProvODBC = new OdbcConsultaDTO();

            consultaProvODBC.consulta = string.Format("SELECT numpro as Numero, nomcorto as NombreCorto, nombre FROM sp_proveedores WHERE numpro in {0}", numeroProveedores.ToParamInValue());
            //consultaProvODBC.consulta = "SELECT nombre FROM sp_proveedores WHERE numpro = ?";
            foreach (var proNum in numeroProveedores)
            {
                consultaProvODBC.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "numpro",
                    tipo = OdbcType.Int,
                    valor = proNum
                });
            }
            var _prov = _contextEnkontrol.Select<InfoProveedorDTO>(EnkontrolAmbienteEnum.Prod, consultaProvODBC);

            return _prov;
        }
    }
}
