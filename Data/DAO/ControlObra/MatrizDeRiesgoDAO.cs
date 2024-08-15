using Core.DAO.ControlObra;
using Core.DTO.ControlObra.MatrizDeRiesgo;
using Core.DTO.Principal.Generales;
using Core.Entity.ControlObra;
using Core.Entity.ControlObra.MatrizDeRiesgo;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using Core.Enum.ControlObra;
using Infrastructure.Utils;
using Core.Entity.Principal.Alertas;

namespace Data.DAO.ControlObra
{
    public class MatrizDeRiesgoDAO : GenericDAO<tblCO_MatrizDeRiesgo>, IMatrizDeRiesgoDAO
    {
        Dictionary<string, object> resultado;
        private readonly string RutaBase = @"\\REPOSITORIO\Proyecto\SUBCONTRATISTAS\GESTION";
        private const string RutaLocal = @"C:/Proyecto/SUBCONTRATISTAS/GESTION";

        public MatrizDeRiesgoDAO()
        {
            resultado = new Dictionary<string, object>();
#if DEBUG
            RutaBase = RutaLocal;
#endif

        }

        public Dictionary<string, object> obtenerMatrizesDeRiesgo(string variable)
        {
            resultado = new Dictionary<string, object>();
            try
            {

                var lstDetalle = _context.tblCO_MatrizDeRiesgo.Where(x => (variable == null ? x.cc == variable : x.cc == x.cc) && x.estatus == true).ToList().Select(n => new MatrizDetalleDTO
                {
                    id = n.id,
                    nombreDelProyecto = n.nombreDelProyecto,
                    faseDelProyecto = n.faseDelProyecto,
                    cc = n.cc,
                    fechaElaboracion = n.fechaElaboracion,
                    personalElaboro = n.personalElaboro,
                    estatus = n.estatus,
                    lstMatrizDeRiesgo = _context.tblCO_MatrizDeRiesgoDet.Where(y => y.idMatrizDeRiesgo == n.id).ToList().Count() > 0 ? _context.tblCO_MatrizDeRiesgoDet.Where(y => (y.idMatrizDeRiesgo == n.id)).ToList().Select(y => new MatrizDetDTO
                    {
                        id = y.id,
                        idMatrizDeRiesgo = y.idMatrizDeRiesgo,
                        Historial = y.Historial,
                        No = y.No,
                        chAmenzaOportunidad = y.chAmenzaOportunidad == 1 ? false : true,
                        amenazaOportunidad = y.amenazaOportunidad,
                        categoriaDelRiesgo = y.categoriaDelRiesgo,
                        descategoriaDelRiesgo = _context.tblCO_MR_CategoriaDeRiesgo.Where(r => r.id == y.categoriaDelRiesgo).FirstOrDefault() == null ? "" : _context.tblCO_MR_CategoriaDeRiesgo.Where(r => r.id == y.categoriaDelRiesgo).FirstOrDefault().descripcion,
                        causaBasica = y.causaBasica,
                        //descausaBasica = y.descausaBasica,
                        areaDelProyecto = y.areaDelProyecto,
                        costoTiempoCalidad = y.costoTiempoCalidad,
                        probabilidad = y.probabilidad,
                        impacto = y.impacto,
                        severidadInicial = y.severidadInicial,
                        severidadActual = y.severidadActual,
                        tipoDeRespuesta = y.tipoDeRespuesta,
                        desctipoDeRespuesta = _context.tblCO_MR_TipoDeRespuestas.Where(r => r.id == y.tipoDeRespuesta).FirstOrDefault() == null ? "" : _context.tblCO_MR_TipoDeRespuestas.Where(r => r.id == y.tipoDeRespuesta).FirstOrDefault().descripcion,
                        medidasATomar = y.medidasATomar,
                        dueñoDelRiesgo = y.dueñoDelRiesgo,
                        fechaDeCompromiso = y.fechaDeCompromiso,
                        abiertoProcesoCerrado = y.abiertoProcesoCerrado,
                    }).OrderBy(y => y.amenazaOportunidad).ToList() : null,
                    lstImpacto = _context.tblCO_MR_ImpractosSobreObjetivosDelProyecto.Where(y => y.idMatriz == n.id).ToList().Count() > 0 ? _context.tblCO_MR_ImpractosSobreObjetivosDelProyecto.Where(y => y.idMatriz == n.id).ToList() : null
                }).ToList();
                var lst = lstDetalle.OrderByDescending(n => n.id).ToList();
                resultado.Add(ITEMS, lst);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                resultado.Add(ITEMS, "");
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public MatrizPrinDTO obtenerMatrizesDeRiesgoxID(int idMatrizDeRiesgo, List<int> lstFiltro)
        {
            var r = new MatrizPrinDTO();
            try
            {

                var lstDetalle = _context.tblCO_MatrizDeRiesgo.Where(x => x.id == idMatrizDeRiesgo && x.estatus == true).ToList().Select(n => new MatrizPrinDTO
                {
                    id = n.id,
                    nombreDelProyecto = n.nombreDelProyecto,
                    faseDelProyecto = n.faseDelProyecto,
                    cc = n.cc,
                    fechaElaboracion = n.fechaElaboracion,
                    personalElaboro = n.personalElaboro,
                    estatus = n.estatus,
                    lstMatrizDeRiesgo = _context.tblCO_MatrizDeRiesgoDet.Where(y => y.idMatrizDeRiesgo == n.id && lstFiltro.Contains(y.abiertoProcesoCerrado)).ToList().Count() > 0 ? _context.tblCO_MatrizDeRiesgoDet.Where(y => y.idMatrizDeRiesgo == n.id).ToList().Select(y => new MatrizDetalDTO
                    {
                        id = y.id,
                        idMatrizDeRiesgo = y.idMatrizDeRiesgo,
                        Historial = y.Historial,
                        No = y.No,
                        amenazaOportunidad = y.amenazaOportunidad,
                        categoriaDelRiesgo = y.categoriaDelRiesgo,
                        descategoriaDelRiesgo = _context.tblCO_MR_CategoriaDeRiesgo.Where(f => f.id == y.categoriaDelRiesgo).FirstOrDefault() == null ? "" : _context.tblCO_MR_CategoriaDeRiesgo.Where(f => f.id == y.categoriaDelRiesgo).FirstOrDefault().descripcion,
                        causaBasica = y.causaBasica,
                        //descausaBasica = y.descausaBasica,
                        areaDelProyecto = y.areaDelProyecto,
                        costoTiempoCalidad = y.costoTiempoCalidad,
                        probabilidad = y.probabilidad,
                        impacto = y.impacto,
                        severidadInicial = y.severidadInicial,
                        severidadActual = y.severidadActual,
                        tipoDeRespuesta = y.tipoDeRespuesta,
                        desctipoDeRespuesta = _context.tblCO_MR_TipoDeRespuestas.Where(f => f.id == y.tipoDeRespuesta).FirstOrDefault() == null ? "" : _context.tblCO_MR_TipoDeRespuestas.Where(f => f.id == y.tipoDeRespuesta).FirstOrDefault().descripcion,
                        medidasATomar = y.medidasATomar,
                        dueñoDelRiesgo = y.dueñoDelRiesgo,
                        fechaDeCompromiso = y.fechaDeCompromiso,
                        abiertoProcesoCerrado = y.abiertoProcesoCerrado.ToString(),
                    }).ToList() : null,
                    lstImpacto = _context.tblCO_MR_ImpractosSobreObjetivosDelProyecto.Where(y => y.idMatriz == n.id).ToList().Count() > 0 ? _context.tblCO_MR_ImpractosSobreObjetivosDelProyecto.Where(y => y.idMatriz == n.id).ToList() : null
                }).FirstOrDefault();
                r = lstDetalle;
            }
            catch (Exception ex)
            {
                resultado.Add(ITEMS, "");
                resultado.Add(SUCCESS, false);
            }
            return r;
        }

        public Dictionary<string, object> GuardarEditarMatriz(MatrizDTO parametros, bool editar)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                if (editar == false)
                {
                    #region GUARDAR
                    tblCO_MatrizDeRiesgo objMatriz = _context.tblCO_MatrizDeRiesgo.Where(x => x.id == parametros.id).FirstOrDefault();
                    if (objMatriz == null)
                    {


                        objMatriz = new tblCO_MatrizDeRiesgo();
                        objMatriz.cc = parametros.cc;
                        objMatriz.idPadre = 0;
                        objMatriz.faseDelProyecto = parametros.faseDelProyecto;
                        objMatriz.fechaElaboracion = parametros.fechaElaboracion;
                        objMatriz.personalElaboro = parametros.personajeElaboro;
                        objMatriz.nombreDelProyecto = parametros.nombreDelProyecto;
                        objMatriz.estatus = true;
                        _context.tblCO_MatrizDeRiesgo.Add(objMatriz);
                        _context.SaveChanges();

                        var obtenerUltimoID = _context.tblCO_MatrizDeRiesgo.OrderByDescending(r => r.id).FirstOrDefault();
                        if (obtenerUltimoID != null)
                        {

                            var objImpac = new tblCO_MR_ImpractosSobreObjetivosDelProyecto();
                            objImpac.idMatriz = obtenerUltimoID.id;
                            objImpac.tiempo = parametros.tiempoBaja;
                            objImpac.tipo = 1;
                            objImpac.costo = parametros.costoBaja;
                            objImpac.calidad = parametros.calidadBaja;
                            objImpac.baja = parametros.baja;
                            objImpac.bajaFin = parametros.bajaFin;
                            _context.tblCO_MR_ImpractosSobreObjetivosDelProyecto.Add(objImpac);
                            _context.SaveChanges();

                            var objImpac2 = new tblCO_MR_ImpractosSobreObjetivosDelProyecto();
                            objImpac.idMatriz = obtenerUltimoID.id;
                            objImpac.tiempo = parametros.tiempoMedia;
                            objImpac.tipo = 2;
                            objImpac.costo = parametros.costoMedia;
                            objImpac.calidad = parametros.calidadMedia;
                            objImpac.baja = parametros.media;
                            objImpac.bajaFin = parametros.mediaFin;
                            _context.tblCO_MR_ImpractosSobreObjetivosDelProyecto.Add(objImpac);
                            _context.SaveChanges();

                            var objImpac3 = new tblCO_MR_ImpractosSobreObjetivosDelProyecto();
                            objImpac.idMatriz = obtenerUltimoID.id;
                            objImpac.tiempo = parametros.tiempoAlta;
                            objImpac.tipo = 3;
                            objImpac.costo = parametros.costoAlta;
                            objImpac.calidad = parametros.calidadAlta;
                            objImpac.baja = parametros.alta;
                            objImpac.bajaFin = parametros.altaFin;
                            _context.tblCO_MR_ImpractosSobreObjetivosDelProyecto.Add(objImpac);
                            _context.SaveChanges();
                        }

                        foreach (var item in parametros.lstDetalleGuardado)
                        {
                            var objDetalle = _context.tblCO_MatrizDeRiesgoDet.Where(r => r.id == item.id).FirstOrDefault();
                            if (objDetalle == null)
                            {
                                //AGREGAR
                                objDetalle = new tblCO_MatrizDeRiesgoDet();
                                objDetalle.idMatrizDeRiesgo = obtenerUltimoID.id;
                                objDetalle.impacto = item.impacto;
                                objDetalle.medidasATomar = item.medidasATomar;
                                objDetalle.amenazaOportunidad = item.amenazaOportunidad;
                                objDetalle.categoriaDelRiesgo = item.categoriaDelRiesgo;
                                objDetalle.causaBasica = item.causaBasica;
                                objDetalle.areaDelProyecto = item.areaDelProyecto;
                                objDetalle.costoTiempoCalidad = item.costoTiempoCalidad;
                                objDetalle.probabilidad = item.probabilidad;
                                objDetalle.impacto = item.impacto;
                                objDetalle.severidadInicial = item.severidadInicial;
                                objDetalle.severidadActual = item.severidadActual;
                                objDetalle.tipoDeRespuesta = item.tipoDeRespuesta;
                                objDetalle.medidasATomar = item.medidasATomar;
                                objDetalle.dueñoDelRiesgo = item.dueñoDelRiesgo;
                                objDetalle.fechaDeCompromiso = item.fechaDeCompromiso;
                                objDetalle.abiertoProcesoCerrado = item.abiertoProcesoCerrado;
                                _context.tblCO_MatrizDeRiesgoDet.Add(objDetalle);
                                _context.SaveChanges();
                            }

                        }



                    }
                    resultado.Add(ITEMS, "GUARDADO CON EXITO.");
                    resultado.Add(SUCCESS, true);
                    #endregion
                }
                else
                {
                    tblCO_MatrizDeRiesgo objMatriz = _context.tblCO_MatrizDeRiesgo.Where(x => x.id == parametros.id).FirstOrDefault();
                    if (objMatriz != null)
                    {

                        int idAnterior = objMatriz.id;
                        objMatriz.estatus = false;
                        _context.SaveChanges();

                        objMatriz = new tblCO_MatrizDeRiesgo();
                        objMatriz.idPadre = idAnterior;
                        objMatriz.cc = parametros.cc;
                        objMatriz.faseDelProyecto = parametros.faseDelProyecto;
                        objMatriz.fechaElaboracion = parametros.fechaElaboracion;
                        objMatriz.personalElaboro = parametros.personajeElaboro;
                        objMatriz.nombreDelProyecto = parametros.nombreDelProyecto;
                        objMatriz.estatus = true;
                        _context.tblCO_MatrizDeRiesgo.Add(objMatriz);
                        _context.SaveChanges();

                        var obtenerUltimoID = _context.tblCO_MatrizDeRiesgo.OrderByDescending(r => r.id).FirstOrDefault();
                        if (obtenerUltimoID != null)
                        {

                            var objImpac = _context.tblCO_MR_ImpractosSobreObjetivosDelProyecto.Where(r => r.idMatriz == obtenerUltimoID.id && r.tipo == 1).FirstOrDefault();
                            if (objImpac != null)
                            {
                                objImpac.idMatriz = obtenerUltimoID.id;
                                objImpac.tiempo = parametros.tiempoBaja;
                                objImpac.tipo = 1;
                                objImpac.costo = parametros.costoBaja;
                                objImpac.calidad = parametros.calidadBaja;
                                objImpac.baja = parametros.baja;
                                objImpac.bajaFin = parametros.bajaFin;
                                _context.SaveChanges();
                            }
                            else
                            {
                                objImpac = new tblCO_MR_ImpractosSobreObjetivosDelProyecto();
                                objImpac.idMatriz = obtenerUltimoID.id;
                                objImpac.tiempo = parametros.tiempoBaja;
                                objImpac.tipo = 1;
                                objImpac.costo = parametros.costoBaja;
                                objImpac.calidad = parametros.calidadBaja;
                                objImpac.baja = parametros.baja;
                                objImpac.bajaFin = parametros.bajaFin;
                                _context.tblCO_MR_ImpractosSobreObjetivosDelProyecto.Add(objImpac);
                                _context.SaveChanges();
                            }

                            var objImpac2 = _context.tblCO_MR_ImpractosSobreObjetivosDelProyecto.Where(r => r.idMatriz == obtenerUltimoID.id && r.tipo == 2).FirstOrDefault();
                            if (objImpac2 != null)
                            {
                                objImpac2.idMatriz = obtenerUltimoID.id;
                                objImpac2.tiempo = parametros.tiempoMedia;
                                objImpac2.tipo = 2;
                                objImpac2.costo = parametros.costoMedia;
                                objImpac2.calidad = parametros.calidadMedia;
                                objImpac2.baja = parametros.media;
                                objImpac2.bajaFin = parametros.mediaFin;
                                _context.SaveChanges();
                            }
                            else
                            {
                                objImpac2 = new tblCO_MR_ImpractosSobreObjetivosDelProyecto();
                                objImpac2.idMatriz = obtenerUltimoID.id;
                                objImpac2.tiempo = parametros.tiempoMedia;
                                objImpac2.tipo = 2;
                                objImpac2.costo = parametros.costoMedia;
                                objImpac2.calidad = parametros.calidadMedia;
                                objImpac2.baja = parametros.media;
                                objImpac2.bajaFin = parametros.mediaFin;
                                _context.tblCO_MR_ImpractosSobreObjetivosDelProyecto.Add(objImpac2);
                                _context.SaveChanges();
                            }
                            var objImpac3 = _context.tblCO_MR_ImpractosSobreObjetivosDelProyecto.Where(r => r.idMatriz == obtenerUltimoID.id && r.tipo == 3).FirstOrDefault();
                            if (objImpac3 != null)
                            {
                                objImpac3.idMatriz = obtenerUltimoID.id;
                                objImpac3.tiempo = parametros.tiempoAlta;
                                objImpac3.tipo = 3;
                                objImpac3.costo = parametros.costoAlta;
                                objImpac3.calidad = parametros.calidadAlta;
                                objImpac3.baja = parametros.alta;
                                objImpac3.bajaFin = parametros.altaFin;
                                _context.SaveChanges();
                            }
                            else
                            {
                                objImpac3 = new tblCO_MR_ImpractosSobreObjetivosDelProyecto();
                                objImpac3.idMatriz = obtenerUltimoID.id;
                                objImpac3.tiempo = parametros.tiempoAlta;
                                objImpac3.tipo = 3;
                                objImpac3.costo = parametros.costoAlta;
                                objImpac3.calidad = parametros.calidadAlta;
                                objImpac3.baja = parametros.alta;
                                objImpac3.bajaFin = parametros.altaFin;
                                _context.tblCO_MR_ImpractosSobreObjetivosDelProyecto.Add(objImpac3);
                                _context.SaveChanges();
                            }
                        }
                        int cont = 0;
                        foreach (var item in parametros.lstDetalleGuardado)
                        {
                            tblCO_MatrizDeRiesgoDet objDetalle = new tblCO_MatrizDeRiesgoDet();
                            cont++;
                            objDetalle.No = cont;
                            objDetalle.idMatrizDeRiesgo = obtenerUltimoID.id;
                            objDetalle.chAmenzaOportunidad = item.chAmenzaOportunidad == false ? 1 : 2;
                            objDetalle.impacto = item.impacto;
                            objDetalle.medidasATomar = item.medidasATomar;
                            objDetalle.amenazaOportunidad = item.amenazaOportunidad;
                            objDetalle.categoriaDelRiesgo = item.categoriaDelRiesgo;
                            objDetalle.causaBasica = item.causaBasica;
                            objDetalle.areaDelProyecto = item.areaDelProyecto;
                            objDetalle.costoTiempoCalidad = item.costoTiempoCalidad;
                            objDetalle.probabilidad = item.probabilidad;
                            objDetalle.impacto = item.impacto;
                            objDetalle.severidadInicial = item.severidadInicial;
                            objDetalle.severidadActual = item.severidadActual;
                            objDetalle.tipoDeRespuesta = item.tipoDeRespuesta;
                            objDetalle.medidasATomar = item.medidasATomar;
                            objDetalle.dueñoDelRiesgo = item.dueñoDelRiesgo;
                            objDetalle.fechaDeCompromiso = item.fechaDeCompromiso;
                            objDetalle.abiertoProcesoCerrado = item.abiertoProcesoCerrado;
                            _context.tblCO_MatrizDeRiesgoDet.Add(objDetalle);
                            _context.SaveChanges();
                        }

                        List<tblCO_MatrizDeRiesgoDet> lstMatrizDet = _context.tblCO_MatrizDeRiesgoDet.Where(r => r.idMatrizDeRiesgo == obtenerUltimoID.id).ToList();

                        if (lstMatrizDet.Count() != 0)
                        {
                            var lstIdUsuarios = retornar(lstMatrizDet.Select(y => y.dueñoDelRiesgo).ToList());
                            var lstCorreos = _context.tblP_Usuario.Where(r => lstIdUsuarios.Contains(r.id)).ToList().Select(y => y.correo).ToList();
                            var asunto = "Caratula Arrendadora";
                            string cuerpoCorreo = "";
                            cuerpoCorreo += "<html><head><style>table {font-family: arial, sans-serif;border-collapse: collapse;width: 100%;}td, th {border: 1px solid #dddddd;text-align: left;padding: 8px;}";
                            cuerpoCorreo += " tr:nth-child(even) {background-color: #dddddd;} .autorizado{background-color: #008f39 } .rechazado{background-color: #FF0000 } .enEspera{background-color: #FFFFFF }</style>";
                            cuerpoCorreo += " </head>";
                            cuerpoCorreo += " <body lang=ES-MX link='#0563C1' vlink='#954F72'>";
                            cuerpoCorreo += "<div class=WordSection1>";
                            cuerpoCorreo += "<p class=MsoNormal>NOTIFICACION DE MATRIZ DE RIESGO<o:p></o:p></p>";
                            cuerpoCorreo += " <p class=MsoNormal>";
                            cuerpoCorreo += "Favor de ingresar al sistema <a href='http://sigoplan.construplan.com.mx/'>SIGOPLAN</a>,";
                            cuerpoCorreo += " en el apartado de Administracion de proyectos, Control de obra, Matriz de riesgo, Captura<o:p></o:p>";
                            cuerpoCorreo += "</p>";
                            cuerpoCorreo += "<p class=MsoNormal>";
                            cuerpoCorreo += "También puede acceder ingresando normalmente al sistema y dando clic en la notificación correspondiente.<o:p></o:p>";
                            cuerpoCorreo += "</p>";
                            cuerpoCorreo += "<p class=MsoNormal>";
                            cuerpoCorreo += "PD. Se informa que esta notificación es autogenerada por el sistema SIGOPLAN y no es necesario dar una respuesta.<o:p></o:p>";
                            cuerpoCorreo += "</p>";
                            cuerpoCorreo += "<p class=MsoNormal>";
                            cuerpoCorreo += "Gracias.<o:p></o:p>";
                            cuerpoCorreo += "</p>";
                            cuerpoCorreo += "</div>";
                            cuerpoCorreo += "</body>";
                            cuerpoCorreo += "</html>";
                            GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), cuerpoCorreo, lstCorreos);

                            foreach (var item in lstMatrizDet)
                            {
                                tblP_Alerta objAlerta = new tblP_Alerta();

                                objAlerta.moduloID = 3;
                                objAlerta.msj = "Matriz de riesgo Generada";
                                objAlerta.userEnviaID = 1;
                                objAlerta.userRecibeID = Convert.ToInt32(item.dueñoDelRiesgo);
                                objAlerta.visto = false;
                                objAlerta.url = "/ControlObra/ControlObra/MatrizDeRiesgo";
                                objAlerta.tipoAlerta = 2;
                                objAlerta.objID = item.id;
                                objAlerta.documentoID = item.idMatrizDeRiesgo;

                                _context.tblP_Alerta.Add(objAlerta);
                                _context.SaveChanges();
                            }
                        }


                    }
                    resultado.Add(ITEMS, "EDITADO CON EXITO.");
                    resultado.Add(SUCCESS, true);
                }


            }
            catch (Exception ex)
            {
                resultado.Add(ITEMS, "ALGO A OCURRIDO");
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public List<int> retornar(List<string> n)
        {
            List<int> r = new List<int>();
            foreach (var item in n)
            {
                r.Add(Convert.ToInt32(item));
            }
            return r;
        }

        public List<ComboDTO> obtenerContratos()
        {
            List<ComboDTO> lst = new List<ComboDTO>();
            try
            {
                string sql = @"SELECT A.id AS Value,( A.cc + ' - ' + A.numeroContrato + ' - ' + B.nombre) AS Text, A.cc  AS TextoOpcional, B.nombre AS Prefijo
                                FROM 
                                tblX_Contrato AS A  
                                INNER JOIN tblX_SubContratista AS B ON A.subcontratistaID = B.id AND B.estatus=1
                                WHERE A.estatus = 1";
                
                using (var dbcon = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                {
                    dbcon.Open();
                    lst = dbcon.Query<ComboDTO>(sql, null, null, true, 300).ToList().Select(y => new ComboDTO
                    {
                        Value = y.Value,
                        Text = y.Text,
                        Prefijo = y.Prefijo,
                        TextoOpcional = y.TextoOpcional
                    }).ToList();
                    dbcon.Close();
                }
            }
            catch (Exception ex)
            {
                LogError(3, 100, "CONTROLOBRA", "OBTENERLISTADODECONTRATOS", ex, AccionEnum.CONSULTA, 10, lst);
            }
            return lst;
        }
        public List<ComboDTO> TraermeTodosLosCC()
        {
            List<ComboDTO> lst = new List<ComboDTO>();
            try
            {
                string sql = @"SELECT cc AS Value,(cc+' - ' +descripcion)AS Text FROM tblP_CC WHERE estatus=1";
                string conexion = "";
#if DEBUG
                conexion = ConextSigoDapper.conexionSIGOPLAN();
#else
                conexion = ConextSigoDapper.conexionSIGOPLAN();
#endif
                using (var dbcon = new SqlConnection(conexion))
                {
                    dbcon.Open();
                    lst = dbcon.Query<ComboDTO>(sql, null, null, true, 300).ToList().Select(y => new ComboDTO
                    {
                        Value = y.Value,
                        Text = y.Text
                    }).ToList();
                    dbcon.Close();
                }
            }
            catch (Exception ex)
            {
                LogError(3, 100, "CONTROLOBRA", "OBTENERLOSCC", ex, AccionEnum.CONSULTA, 10, lst);
            }
            return lst;
        }
        public List<ComboDTO> QuienElaboro(int idUsuario)
        {
            List<ComboDTO> lst = new List<ComboDTO>();
            try
            {
                string sql = @"SELECT id AS Value,(ISNULL(nombre,'')+' ' +ISNULL(apellidoPaterno,'')+' ' +ISNULL(apellidoMaterno,''))AS Text  FROM tblP_Usuario WHERE estatus=1 AND id=" + idUsuario;
                string conexion = "";
#if DEBUG
                conexion = ConextSigoDapper.conexionSIGOPLAN();
#else
                conexion = ConextSigoDapper.conexionSIGOPLAN();
#endif
                using (var dbcon = new SqlConnection(conexion))
                {
                    dbcon.Open();
                    lst = dbcon.Query<ComboDTO>(sql, null, null, true, 300).ToList().Select(y => new ComboDTO
                    {
                        Value = y.Value,
                        Text = y.Text
                    }).ToList();
                    dbcon.Close();
                }
            }
            catch (Exception ex)
            {
                LogError(3, 100, "CONTROLOBRA", "OBTENERLOSCC", ex, AccionEnum.CONSULTA, 10, lst);
            }
            return lst;
        }

        public List<tblCO_MR_CategoriaDeRiesgo> lstMrCategorias()
        {
            List<tblCO_MR_CategoriaDeRiesgo> lstDetalle = new List<tblCO_MR_CategoriaDeRiesgo>();
            try
            {
                lstDetalle = _context.tblCO_MR_CategoriaDeRiesgo.ToList();
            }
            catch (Exception ex)
            {
                LogError(3, 100, "CONTROLOBRA", "lstMrCategorias", ex, AccionEnum.CONSULTA, 10, lstDetalle);
            }
            return lstDetalle;
        }
        public tblCO_MR_CategoriaDeRiesgo AgregarEditarCategoria(tblCO_MR_CategoriaDeRiesgo parametros)
        {
            tblCO_MR_CategoriaDeRiesgo objDetalle = new tblCO_MR_CategoriaDeRiesgo();
            try
            {
                objDetalle = _context.tblCO_MR_CategoriaDeRiesgo.Where(r => r.id == parametros.id).FirstOrDefault();
                if (objDetalle != null)
                {
                    if (_context.tblCO_MR_CategoriaDeRiesgo.Where(r => r.descripcion == parametros.descripcion).FirstOrDefault() == null)
                    {
                        objDetalle.descripcion = parametros.descripcion;
                    }
                }
                else
                {
                    if (_context.tblCO_MR_CategoriaDeRiesgo.Where(r => r.descripcion == parametros.descripcion).FirstOrDefault() == null)
                    {
                        objDetalle = new tblCO_MR_CategoriaDeRiesgo();
                        objDetalle.descripcion = parametros.descripcion;
                        _context.tblCO_MR_CategoriaDeRiesgo.Add(objDetalle);
                    }
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                LogError(3, 100, "CONTROLOBRA", "AgregarEditarCategoria", ex, AccionEnum.CONSULTA, 10, objDetalle);
            }
            return objDetalle;
        }
        public tblCO_MR_CategoriaDeRiesgo EliminarCategoria(tblCO_MR_CategoriaDeRiesgo parametros)
        {
            tblCO_MR_CategoriaDeRiesgo objDetalle = new tblCO_MR_CategoriaDeRiesgo();
            try
            {
                objDetalle = _context.tblCO_MR_CategoriaDeRiesgo.Where(r => r.id == parametros.id).FirstOrDefault();
                _context.tblCO_MR_CategoriaDeRiesgo.Remove(objDetalle);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                LogError(3, 100, "CONTROLOBRA", "EliminarCategoria", ex, AccionEnum.CONSULTA, 10, objDetalle);
            }
            return objDetalle;
        }

        public List<ComboDTO> cbolstMrCategorias()
        {
            List<ComboDTO> lstDetalle = new List<ComboDTO>();
            try
            {
                lstDetalle = _context.tblCO_MR_CategoriaDeRiesgo.ToList().Select(y => new ComboDTO
                {
                    Value = y.id.ToString(),
                    Text = y.descripcion
                }).ToList();
            }
            catch (Exception ex)
            {
                LogError(3, 100, "CONTROLOBRA", "lstMrCategorias", ex, AccionEnum.CONSULTA, 10, lstDetalle);
            }
            return lstDetalle;
        }
        public List<ComboDTO> cboTiposDeRespuestas(int idTipo)
        {
            List<ComboDTO> lstDetalle = new List<ComboDTO>();
            try
            {
                if (idTipo != 0)
                {
                    lstDetalle = _context.tblCO_MR_TipoDeRespuestas.Where(e => e.tipoRespuesta == idTipo).Select(y => new ComboDTO
                    {
                        Value = y.id.ToString(),
                        Text = y.descripcion
                    }).ToList();
                }
                else
                {
                    lstDetalle = _context.tblCO_MR_TipoDeRespuestas.Select(y => new ComboDTO
                    {
                        Value = y.id.ToString(),
                        Text = y.descripcion
                    }).ToList();
                }
                
            }
            catch (Exception ex)
            {
                LogError(3, 100, "CONTROLOBRA", "lstMrCategorias", ex, AccionEnum.CONSULTA, 10, lstDetalle);
            }
            return lstDetalle;
        }
        public List<ComboDTO> cboResponsables()
        {
            List<ComboDTO> lstDetalle = new List<ComboDTO>();
            try
            {
                lstDetalle = _context.tblP_Usuario.Where(r => r.estatus).ToList().Select(y => new ComboDTO
                {
                    Value = y.id.ToString(),
                    Text = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno
                }).ToList();
            }
            catch (Exception ex)
            {
                LogError(3, 100, "CONTROLOBRA", "lstMrCategorias", ex, AccionEnum.CONSULTA, 10, lstDetalle);
            }
            return lstDetalle;
        }


        public List<TipoRespuestaDTO> lstMrTiposDeRespuestas()
        {
            List<TipoRespuestaDTO> lstDetalle = new List<TipoRespuestaDTO>();
            try
            {
                lstDetalle = _context.tblCO_MR_TipoDeRespuestas.Select(e => new TipoRespuestaDTO
                {
                    id = e.id,
                    descripcion = e.descripcion,
                    tipoRespuesta = e.tipoRespuesta == 1 ? "Amenaza" : e.tipoRespuesta == 2 ? "Oportunidad" : "",
                    respuestaDesc = e.respuestaDesc,
                }).ToList();
            }
            catch (Exception ex)
            {
                LogError(3, 100, "CONTROLOBRA", "lstMrCategorias", ex, AccionEnum.CONSULTA, 10, lstDetalle);
            }
            return lstDetalle;
        }
        public tblCO_MR_TipoDeRespuestas AgregarEditarTiposDeRespuestas(tblCO_MR_TipoDeRespuestas parametros)
        {
            tblCO_MR_TipoDeRespuestas objDetalle = new tblCO_MR_TipoDeRespuestas();
            try
            {
                objDetalle = _context.tblCO_MR_TipoDeRespuestas.Where(r => r.id == parametros.id).FirstOrDefault();
                if (objDetalle != null)
                {
                    //if (_context.tblCO_MR_TipoDeRespuestas.Where(r => r.descripcion == parametros.descripcion).FirstOrDefault() == null)
                    //{
                        objDetalle.descripcion = parametros.descripcion;
                        objDetalle.tipoRespuesta = parametros.tipoRespuesta;
                        objDetalle.respuestaDesc = parametros.respuestaDesc;

                    //}
                }
                else
                {
                    if (_context.tblCO_MR_TipoDeRespuestas.Where(r => r.descripcion == parametros.descripcion).FirstOrDefault() == null)
                    {
                        objDetalle = new tblCO_MR_TipoDeRespuestas();
                        objDetalle.descripcion = parametros.descripcion;
                        objDetalle.tipoRespuesta = parametros.tipoRespuesta;
                        objDetalle.respuestaDesc = parametros.respuestaDesc;

                        _context.tblCO_MR_TipoDeRespuestas.Add(objDetalle);
                    }
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                LogError(3, 100, "CONTROLOBRA", "AgregarEditarCategoria", ex, AccionEnum.CONSULTA, 10, objDetalle);
            }
            return objDetalle;
        }
        public tblCO_MR_TipoDeRespuestas EliminarTiposDeRespuestas(tblCO_MR_TipoDeRespuestas parametros)
        {
            tblCO_MR_TipoDeRespuestas objDetalle = new tblCO_MR_TipoDeRespuestas();
            try
            {
                objDetalle = _context.tblCO_MR_TipoDeRespuestas.Where(r => r.id == parametros.id).FirstOrDefault();
                _context.tblCO_MR_TipoDeRespuestas.Remove(objDetalle);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                LogError(3, 100, "CONTROLOBRA", "EliminarCategoria", ex, AccionEnum.CONSULTA, 10, objDetalle);
            }
            return objDetalle;
        }


       

    }
}
