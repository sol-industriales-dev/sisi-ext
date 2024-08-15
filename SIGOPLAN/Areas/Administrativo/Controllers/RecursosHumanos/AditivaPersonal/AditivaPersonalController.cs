using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Factory.RecursosHumanos.Captura;
using Core.Entity.RecursosHumanos.Catalogo;
using Core.DTO.RecursosHumanos;
using Core.Entity.RecursosHumanos.Captura;
using SIGOPLAN.Controllers;
using Data.Factory.Principal.Alertas;
using Core.Enum.Principal.Alertas;
using Core.Entity.Principal.Alertas;
using Data.Factory.Principal.Usuarios;
using Core.DTO;
using Core.Enum.Principal;
using Infrastructure.Utils;
using Data.DAO.Principal.Usuarios;
using Core.Enum.Principal.Bitacoras;
using Core.DAO.RecursosHumanos.Captura;
using Core.DAO.Principal.Alertas;
using Core.DAO.Principal.Usuarios;
using System.IO;
using Data.Factory.Principal.Archivos;
using Data.EntityFramework.Context;
using Core.Entity.Administrativo.RecursosHumanos.Tabuladores;

namespace SIGOPLAN.Areas.Administrativo.Controllers.RecursosHumanos.AditivaPersonal
{
    public class AditivaPersonalController : BaseController
    {
        IAditivaDeductivaDAO AditivaDeductivaFS;
        IAutorizacionAditivaDeductivaDAO AuthAdivaDeductivaFS;
        IAditivaDeductivaDetDAO AditivaDeductivaDetFS;
        IAlertasDAO AlertasFS;
        IUsuarioDAO UsuarioFS;
        ArchivoFactoryServices ArchivoFS;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            AditivaDeductivaFS = new AditivaDeductivaFactoryService().getAditivaDeductivaService();
            AuthAdivaDeductivaFS = new AutorizacionAditivaDeductivaFactoryService().getAutAditivaDeductivaService();
            AditivaDeductivaDetFS = new AditivaDeductivaDetFactoryService().getAditivaDeductivaDetService();
            AlertasFS = new AlertaFactoryServices().getAlertaService();
            UsuarioFS = new UsuarioFactoryServices().getUsuarioService();
            ArchivoFS = new ArchivoFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        // GET: Administrativo/Index
        public ActionResult Index()
        {
            return View();
        }
        //GET: Administrativo/AditivaPersonal
        public ActionResult AltaAditiva()
        {
            return View();
        }
        public ActionResult FillComboCC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = AditivaDeductivaFS.getListaCC();
                result.Add(ITEMS, list.OrderBy(x => x.Value));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getPuestos(string cC)
        {
            tblRH_CatPuestos objCatPuestos = new tblRH_CatPuestos();
            List<tblRH_CatPuestos> listobjCatPuestos = new List<tblRH_CatPuestos>();
            var items = AditivaDeductivaFS.getCatPuestos(cC);
            var filteredItems = items.Select(x => new { id = x.puesto, label = x.descripcion });
            foreach (var objfilteredItems in filteredItems)
            {
                objCatPuestos = new tblRH_CatPuestos();
                //string Puesto = CargaPuesto(objfilteredItems.label);
                //objCatPuestos.descripcion = Puesto;
                objCatPuestos.descripcion = objfilteredItems.label;
                objCatPuestos.puesto = objfilteredItems.id;
                listobjCatPuestos.Add(objCatPuestos);
            }
            var ListaPuestos = listobjCatPuestos.GroupBy(a => a.descripcion).Select(b => b.First());
            return Json(ListaPuestos.OrderBy(x => x.descripcion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getAllPuestos(string cC)
        {
            tblRH_CatPuestos objCatPuestos = new tblRH_CatPuestos();
            List<tblRH_CatPuestos> listobjCatPuestos = new List<tblRH_CatPuestos>();
            var items = AditivaDeductivaFS.getAllCatPuestos(cC);
            var filteredItems = items.Select(x => new { id = x.puesto, label = x.descripcion, cc = x.cc });
            foreach (var objfilteredItems in filteredItems)
            {
                objCatPuestos = new tblRH_CatPuestos();
                //string Puesto = CargaPuesto(objfilteredItems.label);
                //objCatPuestos.descripcion = Puesto;
                objCatPuestos.descripcion = objfilteredItems.label;
                objCatPuestos.puesto = objfilteredItems.id;
                objCatPuestos.cc = objfilteredItems.cc;
                listobjCatPuestos.Add(objCatPuestos);
            }
            var ListaPuestos = listobjCatPuestos.GroupBy(a => a.descripcion).Select(b => b.First());
            return Json(ListaPuestos.OrderBy(x => x.descripcion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getCategorias(string Puesto, string cC)
        {
            //
            var lstObjAditivaPersonal = new List<Core.DTO.RecursosHumanos.AditivaPersonal>();
            using (var ctx = new MainContext())
            {
                var puestoEntity = ctx.tblRH_EK_Puestos.FirstOrDefault(x => x.registroActivo && x.descripcion.Trim() == Puesto.Trim());
                if (puestoEntity != null)
                {
                    var tabulador = ctx.tblRH_TAB_Tabuladores.FirstOrDefault(x => x.FK_Puesto == puestoEntity.puesto && x.registroActivo && x.tabuladorAutorizado == Core.Enum.RecursosHumanos.Tabuladores.EstatusGestionAutorizacionEnum.AUTORIZADO);
                    var lineaNegocio = ctx.tblRH_TAB_CatLineaNegocioDet.FirstOrDefault(x => x.cc == cC && x.registroActivo);
                    var categorias = new List<tblRH_TAB_TabuladoresDet>();
                    if (tabulador != null && lineaNegocio != null)
                    {
                        categorias = ctx.tblRH_TAB_TabuladoresDet.Where(x => x.registroActivo && x.FK_LineaNegocio == lineaNegocio.FK_LineaNegocio && x.FK_Tabulador == tabulador.id).ToList();
                    }
                    var plantilla = ctx.tblRH_TAB_PlantillasPersonal.FirstOrDefault(x => x.cc == cC && x.registroActivo);
                    var plantillaDet = plantilla != null ? ctx.tblRH_TAB_PlantillasPersonalDet.Where(x => x.FK_Plantilla == plantilla.id && x.registroActivo && x.FK_Puesto == puestoEntity.puesto).ToList() : new List<tblRH_TAB_PlantillasPersonalDet>();
                    var plantillaAditivas = ctx.tblRH_EK_Plantilla_Aditiva.Where(x => x.cc == cC && x.estatus == "A" && x.puesto == puestoEntity.puesto).ToList();

                    var empleadosEnElPuestoCC = ctx.tblRH_EK_Empleados.Where(x => x.estatus_empleado == "A" && x.puesto == puestoEntity.puesto && x.cc_contable == cC).ToList();

                    var cantidadEnCategorias = "";

                    foreach (var cat in categorias)
                    {
                        cantidadEnCategorias += "[" + cat.categoria.concepto + "]" + "(" + empleadosEnElPuestoCC
                            .Where(x =>
                                x.requisicionEntity != null &&
                                x.requisicionEntity.tabuladorDet != null &&
                                x.requisicionEntity.tabuladorDet.FK_Categoria == cat.FK_Categoria
                            ).Count() + ")";

                        if (cat != categorias.Last())
                        {
                            cantidadEnCategorias += ", ";
                        }
                    }

                    var objAditivaPersonal = new Core.DTO.RecursosHumanos.AditivaPersonal();
                    objAditivaPersonal.cantidad = plantillaDet.Sum(x => x.personalNecesario) + plantillaAditivas.Sum(x => x.cantidad);
                    objAditivaPersonal.altas = empleadosEnElPuestoCC.Count();
                    objAditivaPersonal.categoria = cantidadEnCategorias;
                    lstObjAditivaPersonal.Add(objAditivaPersonal);
                }

            }

            return Json(lstObjAditivaPersonal, JsonRequestBehavior.AllowGet);
            //

            //anterior
            //Puesto = CargaPuesto(Puesto);
            //List<string> lstPuestosRelacionados = new List<string>();
            //List<string> Categorias = new List<string>();
            //if (!string.IsNullOrEmpty(cC))
            //{
            //    lstPuestosRelacionados = PuestoRelacionado(Puesto, cC);
            //    Categorias = CargaCategoria(lstPuestosRelacionados, Puesto);
            //    Categorias = (Categorias.GroupBy(a => a).Select(b => b.First())).ToList();    
            //}
            
            //var FiltroCategorias = (dynamic)null;
            //if (Categorias.Count > 1)
            //{
            //    FiltroCategorias = Categorias.Select(x => new { categoria = x }).Where(x =>
            //        //x.categoria != ("N/A") &&
            //        Categorias.Count > (1)
            //    ).OrderBy(x => x.categoria);
            //}
            //else
            //{
            //    FiltroCategorias = Categorias.Select(x => new { categoria = x });
            //}
            //List<Core.DTO.RecursosHumanos.AditivaPersonal> lstobjAditivaPersonal = new List<Core.DTO.RecursosHumanos.AditivaPersonal>();
            //string puestoDescripcion = "";
            //foreach (var item in FiltroCategorias)
            //{
            //    string Converter = item.categoria;
            //    if (
            //        //item.categoria != "N/A" &&
            //        Converter.Length <= 1)
            //    {
            //        puestoDescripcion = Puesto + " " + item.categoria;
            //    }
            //    else
            //    {
            //        puestoDescripcion = Puesto;
            //    }
            //    Core.DTO.RecursosHumanos.AditivaPersonal objAditivaPersonal = new Core.DTO.RecursosHumanos.AditivaPersonal();
            //    if (!string.IsNullOrEmpty(cC))
            //    {
            //        var Prueba = AditivaDeductivaFS.getInfoAditiva(puestoDescripcion, cC);

            //        objAditivaPersonal.altas = Prueba.altas; //PERSONAL EXISTENTE
            //        objAditivaPersonal.cantidad = Prueba.cantidad; //PERSONAL REQUERIDO
            //        objAditivaPersonal.categoria = item.categoria;
            //        //objAditivaPersonal.idSolicitud = idSolicitud; //ID DE LA SOLICITUD DEL MODULO DE CH/RECLUTAMIENTOS/VACANTE/GESTION SOLICITUDES    
            //    }
                
            //    lstobjAditivaPersonal.Add(objAditivaPersonal);
            //}
            //return Json(lstobjAditivaPersonal, JsonRequestBehavior.AllowGet);
            //anterior fin
        }
        public List<string> PuestoRelacionado(string strPuesto, string cC)
        {
            //if (AditivaDeductivaFS == null)
            //    AditivaDeductivaFS = new AditivaDeductivaFactoryService().getAditivaDeductivaService();

            var items = AditivaDeductivaFS.getCatPuestos(cC);
            var filteredItems = items.Select(x => new { id = x.puesto, label = x.descripcion }).Where(x => x.label.Contains(strPuesto));
            List<string> lstPuestosRelacionados = new List<string>();
            foreach (var objfilteredItems in filteredItems)
            {
                lstPuestosRelacionados.Add(objfilteredItems.label);
            }
            return lstPuestosRelacionados;
        }
        public string vacantePuesto(string strPuesCat)
        {
            string puestoFinal = strPuesCat;
            string categoriaFinal = "";
            string Cadena = strPuesCat;
            int LongUltEspacio = Cadena.LastIndexOf(' ');
            int longitudPuesto = Cadena.Length;
            if (LongUltEspacio != -1)
            {
                int longUltPalabra = longitudPuesto - (LongUltEspacio);
                string ultPalabra = (Cadena.Substring(LongUltEspacio, longUltPalabra));
                Char delimiter = ' ';
                String[] substrings = ultPalabra.Split(delimiter);
                foreach (var palabra in substrings)
                {
                    ultPalabra = substrings[1];
                    break;

                }
                if (ultPalabra.Length == 1)
                {
                    puestoFinal = (Cadena.Substring(0, LongUltEspacio));
                    categoriaFinal = ultPalabra;
                }
                else
                {
                    puestoFinal = Cadena;
                    categoriaFinal = "N/A";
                }
            }
            strPuesCat = puestoFinal;
            return strPuesCat;
        }
        public string CargaPuesto(string strPuesCat)
        {
            string puestoFinal = strPuesCat;
            string categoriaFinal = "";
            string Cadena = strPuesCat;
            int LongUltEspacio = Cadena.LastIndexOf(' ');
            int longitudPuesto = Cadena.Length;
            if (LongUltEspacio != -1)
            {
                int longUltPalabra = longitudPuesto - (LongUltEspacio);
                string ultPalabra = (Cadena.Substring(LongUltEspacio, longUltPalabra));
                Char delimiter = ' ';
                String[] substrings = ultPalabra.Split(delimiter);
                foreach (var palabra in substrings)
                {
                    ultPalabra = substrings[1];
                    break;

                }
                if (ultPalabra.Length == 1)
                {
                    puestoFinal = (Cadena.Substring(0, LongUltEspacio));
                    categoriaFinal = ultPalabra;
                }
                else
                {
                    puestoFinal = Cadena;
                    categoriaFinal = "N/A";
                }
            }
            strPuesCat = puestoFinal;
            return strPuesCat;
        }
        public List<string> CargaCategoria(List<string> strPuestos, string PuestoPadre)
        {
            List<string> lstObjPuestos = new List<string>();
            foreach (var item in strPuestos)
            {
                if (!(PuestoPadre.Length + 2 < item.Length))
                {
                    lstObjPuestos.Add(item);
                }
            }

            List<string> Categoria = new List<string>();
            foreach (var Puesto in lstObjPuestos)
            {
                string puestoFinal = Puesto;
                string categoriaFinal = "";
                string Cadena = Puesto;
                int LongUltEspacio = Cadena.LastIndexOf(' ');
                int longitudPuesto = Cadena.Length;
                if (LongUltEspacio != -1)
                {
                    int longUltPalabra = longitudPuesto - (LongUltEspacio);
                    string ultPalabra = (Cadena.Substring(LongUltEspacio, longUltPalabra));
                    Char delimiter = ' ';
                    String[] substrings = ultPalabra.Split(delimiter);
                    foreach (var palabra in substrings)
                    {
                        ultPalabra = substrings[1];
                        break;
                    }
                    if (ultPalabra.Length == 1 || ultPalabra.Length == 2)
                    {
                        puestoFinal = (Cadena.Substring(0, LongUltEspacio));
                        categoriaFinal = ultPalabra;
                    }
                    else
                    {
                        puestoFinal = Cadena;
                        categoriaFinal = "N/A";
                    }
                }
                else
                {
                    categoriaFinal = "N/A";
                }
                Categoria.Add(categoriaFinal);
            }

            return Categoria;
        }
        BaseController objBaseController = new BaseController();
        public ActionResult getEnvioDetalle(List<tblRH_AditivaDeductivaDet> arrayEnvioObj, List<tblRH_AutorizacionAditivaDeductiva> lstAutorizacion, tblRH_AditivaDeductiva objAditivaDeduvtiva, List<tblRH_AditivaDeductivaDet> arraylstElimina)
        {
            var result = new Dictionary<string, object>();
            objAditivaDeduvtiva.fechaCaptura = DateTime.Now;
            objAditivaDeduvtiva.aprobado = false;
            CatFormatoAditivaDeductivaDTO objFormatoADitivaDTO = new CatFormatoAditivaDeductivaDTO();
            var AletaRaw = AlertasFS.getAlertasBySistema((int)SistemasEnum.RH);
            try
            {
                objAditivaDeduvtiva.usuarioCap = objBaseController.getUsuario().id;
                objAditivaDeduvtiva.nomUsuarioCap = objBaseController.getUsuario().nombre;

                if (objAditivaDeduvtiva.id == 0)
                {

                    objFormatoADitivaDTO.objAditivaDeductiva = AditivaDeductivaFS.GuardarAditivaDeduc(objAditivaDeduvtiva);
                    objFormatoADitivaDTO.objAditivaDeductiva.folio = objFormatoADitivaDTO.objAditivaDeductiva.cCid + "-" + objFormatoADitivaDTO.objAditivaDeductiva.id;
                    objFormatoADitivaDTO.objAditivaDeductiva = AditivaDeductivaFS.GuardarAditivaDeduc(objFormatoADitivaDTO.objAditivaDeductiva);
                }
                else
                {
                    var objTemp = AditivaDeductivaFS.getFormatoAditivaDeductivaByID(objAditivaDeduvtiva.id);
                    objAditivaDeduvtiva.folio = objTemp.folio;
                    objFormatoADitivaDTO.objAditivaDeductiva = AditivaDeductivaFS.GuardarAditivaDeduc(objAditivaDeduvtiva);
                    var usuariosFormatoCambios = AuthAdivaDeductivaFS.getAutorizacion(objAditivaDeduvtiva.id);
                    if (usuariosFormatoCambios.Count != lstAutorizacion.Count)
                    {
                        foreach (var autorizador in usuariosFormatoCambios)
                        {
                            var UsuarioEliminado = lstAutorizacion.Where(x => x.clave_Aprobador.Equals(autorizador.clave_Aprobador) && x.puestoAprobador.Equals(autorizador.puestoAprobador)).ToList();
                            if (UsuarioEliminado.Count <= 0)
                            {
                                var useObj = UsuarioEliminado.FirstOrDefault();
                                AuthAdivaDeductivaFS.EliminarAutorizador(autorizador);
                                tblP_Alerta objAlertaDis = new tblP_Alerta();
                                var AletaUpdate = AletaRaw.FirstOrDefault(x => x.userRecibeID == autorizador.clave_Aprobador && x.objID.Equals(autorizador.id_AditivaDeductiva));
                                if (AletaUpdate != null)
                                {
                                    AlertasFS.updateAlerta(AletaUpdate);
                                }
                            }
                        }
                    }
                }

                if (arraylstElimina != null)
                {
                    foreach (var drop in arraylstElimina)
                    {
                        AditivaDeductivaDetFS.eliminarDetalle(drop.id);
                    }
                }
                foreach (var objAditivaDeductivaDet in arrayEnvioObj)
                {

                    objFormatoADitivaDTO.objAditivaDeductivaDet = new List<tblRH_AditivaDeductivaDet>();
                    objAditivaDeductivaDet.id_AditivaDeductiva = objFormatoADitivaDTO.objAditivaDeductiva.id;
                    objAditivaDeductivaDet.puesto = objAditivaDeductivaDet.puesto;
                    objAditivaDeductivaDet.categoria = "N/A";//objAditivaDeductivaDet.categoria;
                    objAditivaDeductivaDet.personalNecesario = objAditivaDeductivaDet.personalNecesario;
                    objAditivaDeductivaDet.personalExistente = objAditivaDeductivaDet.personalExistente;
                    objAditivaDeductivaDet.personalFaltante = objAditivaDeductivaDet.personalFaltante;
                    objAditivaDeductivaDet.lugaresPlantilla = objAditivaDeductivaDet.lugaresPlantilla;
                    objAditivaDeductivaDet.numPersTotal = objAditivaDeductivaDet.numPersTotal;
                    objAditivaDeductivaDet.aditiva = objAditivaDeductivaDet.aditiva;
                    objAditivaDeductivaDet.deductiva = objAditivaDeductivaDet.deductiva;
                    objAditivaDeductivaDet.justificacion = objAditivaDeductivaDet.justificacion;
                    objAditivaDeductivaDet.estado = objAditivaDeductivaDet.estado = true;
                    objAditivaDeductivaDet.nuevo = objAditivaDeductivaDet.nuevo;
                    objAditivaDeductivaDet.idPuesto = objAditivaDeductivaDet.idPuesto;
                    AditivaDeductivaDetFS.GuardarAditivaDeducDet(objAditivaDeductivaDet);
                }
                objFormatoADitivaDTO.objAditivaDeducAut = new List<tblRH_AutorizacionAditivaDeductiva>();
                foreach (var objAutorizacion in lstAutorizacion)
                {
                    objAutorizacion.id_AditivaDeductiva = objAditivaDeduvtiva.id;
                    objAutorizacion.firma = "S/F";
                    if (objAutorizacion.orden == 1)
                    {
                        objAutorizacion.autorizando = true;
                    }
                    var aprobacion = AuthAdivaDeductivaFS.GuardarAutorizacion(objAutorizacion);
                    if (objAutorizacion.orden == 1)
                    {
                        var AletaUpdate = AletaRaw.FirstOrDefault(x => x.userRecibeID == objAutorizacion.clave_Aprobador && x.objID.Equals(objAutorizacion.id_AditivaDeductiva));
                        if (AletaUpdate != null)
                        {
                            AletaUpdate.msj = "Firma-Formato Aditiva-Deductiva " + objAditivaDeduvtiva.folio;
                            AletaUpdate.sistemaID = (int)SistemasEnum.RH;
                            AletaUpdate.documentoID = 0;
                            AletaUpdate.moduloID = (int)BitacoraEnum.AditivaPersonal;
                            AletaUpdate.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                            AletaUpdate.url = "/Administrativo/AditivaPersonal/Index?obj=" + objAditivaDeduvtiva.id + "";
                            AletaUpdate.userEnviaID = objBaseController.getUsuario().id;
                            AletaUpdate.userRecibeID = objAutorizacion.clave_Aprobador;
                            AletaUpdate.objID = objAditivaDeduvtiva.id;
                            AlertasFS.updateAlerta(AletaUpdate);
                        }
                        else
                        {
                            try
                            {
                                tblP_Alerta objAlerta = new tblP_Alerta();
                                objAlerta.msj = "Firma-Formato Aditiva-Deductiva " + objAditivaDeduvtiva.folio;
                                objAlerta.sistemaID = (int)SistemasEnum.RH;
                                objAlerta.documentoID = 0;
                                objAlerta.moduloID = (int)BitacoraEnum.AditivaPersonal;
                                objAlerta.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                                objAlerta.url = "/Administrativo/AditivaPersonal/Index?obj=" + objAditivaDeduvtiva.id + "";
                                objAlerta.userEnviaID = objBaseController.getUsuario().id;
                                objAlerta.userRecibeID = objAutorizacion.clave_Aprobador;
                                objAlerta.objID = objAditivaDeduvtiva.id;
                                AlertasFS.saveAlerta(objAlerta);
                            }
                            catch (Exception o_O) { }
                        }
                        var usuarioEnvia = UsuarioFS.ListUsersById(objBaseController.getUsuario().id).FirstOrDefault();
                        result.Add("idFormatoAditiva", objAditivaDeduvtiva.id);
                        result.Add("usuarioEnvia", objAutorizacion.clave_Aprobador);
                    }
                    objFormatoADitivaDTO.objAditivaDeducAut.Add(aprobacion);
                }
                result.Add("objFormatoAditivaDTO", objFormatoADitivaDTO);
            }
            catch
            {
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTblAdvPer(int id)
        {
            List<tblRH_AditivaDeductiva> lstobjAditivaDeductiva = new List<tblRH_AditivaDeductiva>();
            lstobjAditivaDeductiva = AditivaDeductivaFS.GetListAditivaDeducPersonal().Where(x => id == 0 ? true : x.id == id).ToList();
            lstobjAditivaDeductiva = ObtencionEditables(lstobjAditivaDeductiva);
            return Json(lstobjAditivaDeductiva, JsonRequestBehavior.AllowGet);
        }
        public bool getAction(string accion)
        {
            bool result = false;
            result = UsuarioFS.getViewAction(vSesiones.sesionCurrentView, accion);
            return result;
        }
        public ActionResult GetAutorizacion(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstobjAditivaDeductiva = AditivaDeductivaFS.getFormatoAditivaDeductivaByID(id);
                var userCapName = lstobjAditivaDeductiva.nomUsuarioCap;
                var listAp = AuthAdivaDeductivaFS.getAutorizacion(id);
                int usuarioLog = objBaseController.getUsuario().id;
                foreach (var Ap in listAp)
                {
                    if (Ap.clave_Aprobador != usuarioLog && !Ap.estatus && Ap.autorizando)
                    {
                        Ap.autorizando = false;
                    }
                }
                result.Add("CAPTURA", userCapName);
                result.Add(ITEMS, listAp);
                result.Add(SUCCESS, true);
                if (listAp.Any(x => x.comentario != null))
                {
                    string comentario = listAp.FirstOrDefault(x => x.comentario != null).comentario;
                    comentario = HttpUtility.HtmlEncode(comentario).Replace("\n", " ").Trim();
                    result.Add("comentario", comentario);
                }

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult eliminarFormato(int formatoID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                AditivaDeductivaFS.eliminarFormato(formatoID);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        DateTime FechaFirma;
        public ActionResult Aprobar(int id)
        {
            tblRH_AutorizacionAditivaDeductiva objAutorizacion = new tblRH_AutorizacionAditivaDeductiva();
            objAutorizacion = AuthAdivaDeductivaFS.getAutorizacionIndividual(id);
            int idAutorizado = objAutorizacion.id;
            objAutorizacion.estatus = true;
            objAutorizacion.firma = "--" + objAutorizacion.id_AditivaDeductiva.ToString() + "|" + DateTime.Now.ToString("ddMMyyyy|HHmm") + "|" + (int)DocumentosEnum.AditivaDeductiva + "|" + objAutorizacion.clave_Aprobador + "--";
            objAutorizacion.autorizando = false;
            objAutorizacion.fechafirma = (DateTime.Now).ToString();
            var aprobacion = AuthAdivaDeductivaFS.SaveChangesAutorizacionCambios(objAutorizacion);
            List<tblRH_AutorizacionAditivaDeductiva> listAp = AuthAdivaDeductivaFS.getAutorizacion(objAutorizacion.id_AditivaDeductiva);
            if (objAutorizacion.tipoAutoriza)
            {
                var autorizaTipo = listAp.Where(x => x.orden.Equals(objAutorizacion.orden));

                foreach (var item in autorizaTipo)
                {
                    if (item.firma == "S/F")
                    {
                        item.firma = objAutorizacion.firma;
                    }
                    item.autorizando = false;
                    item.estatus = true;
                    AuthAdivaDeductivaFS.SaveChangesAutorizacionCambios(item);
                }
            }

            bool todasAp = true;
            var result = new Dictionary<string, object>();
            var tempOrden = 0;
            foreach (var objAp in listAp)
            {
                if (!objAp.estatus)
                {
                    todasAp = false;
                    objAutorizacion = objAp;
                    if (objAp.tipoAutoriza)
                    {
                        if (tempOrden == objAp.orden || tempOrden == 0)
                        {
                            objAutorizacion.autorizando = true;
                            tempOrden = objAutorizacion.orden;
                            AuthAdivaDeductivaFS.SaveChangesAutorizacionCambios(objAutorizacion);
                        }
                        else { break; }
                    }
                    else
                    {
                        objAutorizacion.autorizando = true;
                        aprobacion = AuthAdivaDeductivaFS.SaveChangesAutorizacionCambios(objAutorizacion);
                        break;
                    }

                }
            }

            if (todasAp)
            {

                var lstObjAditivaDeu = AditivaDeductivaFS.getListAditivaDeductivaPendientes(objAutorizacion.id_AditivaDeductiva, "", "", 1);
                var folio = lstObjAditivaDeu.Select(x => x.folio).First();
                objAutorizacion.autorizando = true;
                if (objAutorizacion != null)
                {
                    tblP_Alerta objAlertaDis = new tblP_Alerta();
                    var AletaRaw = AlertasFS.getAlertasBySistema((int)SistemasEnum.RH);
                    var AditivaAprobada = AditivaDeductivaFS.getFormatoAditivaDeductivaByID(objAutorizacion.id_AditivaDeductiva);
                    AditivaAprobada.aprobado = true;
                    AditivaDeductivaFS.GuardarAditivaDeduc(AditivaAprobada);
                    objAutorizacion.autorizando = false;

                    try
                    {
                        var AletaUpdate = AletaRaw.FirstOrDefault(x => x.userRecibeID == objBaseController.getUsuario().id && x.objID.Equals(objAutorizacion.id_AditivaDeductiva));
                        AlertasFS.updateAlerta(AletaUpdate);
                    }
                    catch (Exception)
                    {


                    }
                }
                foreach (var objEmp in lstObjAditivaDeu)
                {

                    objEmp.aprobado = true;
                    AuthAdivaDeductivaFS.SaveChangesAutorizacionCambios(objAutorizacion);
                    result.Add(SUCCESS, true);
                    result.Add(ITEMS, objEmp.id);
                }
                result.Add("AprobadoTotal", true);
                List<string> CorreoEnviar = new List<string>();
              /*  var diana = UsuarioFS.ListUsersById(1019).FirstOrDefault();
                CorreoEnviar.Add(diana.correo);
                var aranza = UsuarioFS.ListUsersById(79552).FirstOrDefault();
                CorreoEnviar.Add(aranza.correo);*/

                result.Add("CorreoEnviar", CorreoEnviar);
                result.Add("Folio", folio);
            }
            else
            {
                var lstObjAditivaCambio = AditivaDeductivaFS.getListAditivaDeductivaPendientes(objAutorizacion.id_AditivaDeductiva, "", "", 1);
                var folio = lstObjAditivaCambio.Select(x => x.folio).First();

                objAutorizacion.autorizando = true;
                if (objAutorizacion != null)
                {
                    tblP_Alerta objAlertaDis = new tblP_Alerta();
                    var AletaRaw = AlertasFS.getAlertasBySistema((int)SistemasEnum.RH).Where(y => y.userRecibeID == vSesiones.sesionUsuarioDTO.id);
                    var AletaUpdate = AletaRaw.FirstOrDefault(x => x.userRecibeID == objBaseController.getUsuario().id && x.objID.Equals(objAutorizacion.id_AditivaDeductiva));
                    if (AletaUpdate != null)
                    {
                        AlertasFS.updateAlerta(AletaUpdate);
                    }                        
                }
                tblP_Alerta objAlerta = new tblP_Alerta();
                try
                {
                    objAlerta.msj = "Firma-Formato Aditiva-Deductiva " + folio;
                    objAlerta.sistemaID = (int)SistemasEnum.RH;
                    objAlerta.moduloID = (int)BitacoraEnum.AditivaPersonal;
                    objAlerta.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                    objAlerta.documentoID = 0;
                    objAlerta.url = "/Administrativo/AditivaPersonal/Index?obj=" + objAutorizacion.id_AditivaDeductiva + "";
                    objAlerta.objID = objAutorizacion.id_AditivaDeductiva;
                    objAlerta.userEnviaID = objBaseController.getUsuario().id;
                    objAlerta.userRecibeID = objAutorizacion.clave_Aprobador;
                    AlertasFS.saveAlerta(objAlerta);
                }
                catch (Exception o_O) { }
                result.Add("idFormatoAditivaDeductiva", objAutorizacion.id_AditivaDeductiva);
                result.Add("usuarioEnvia", objAutorizacion.clave_Aprobador);

                result.Add("AprobadoTotal", false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Rechazar(int id, string comentario)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (comentario == null || comentario.Trim().Length < 10)
                {
                    result.Add(MESSAGE, "No se rechazó la solicitud. El comentario viene vacío.");
                    result.Add(SUCCESS, false);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                tblRH_AutorizacionAditivaDeductiva objAutorizacion = new tblRH_AutorizacionAditivaDeductiva();
                objAutorizacion = AuthAdivaDeductivaFS.getAutorizacionIndividual(id);
                int idAutorizado = objAutorizacion.id;
                objAutorizacion.estatus = false;
                objAutorizacion.autorizando = false;
                objAutorizacion.rechazado = true;
                objAutorizacion.comentario = comentario.Trim();
                var aprobacion = AuthAdivaDeductivaFS.SaveChangesAutorizacionCambios(objAutorizacion);
                var AditivaAprobada = AditivaDeductivaFS.getFormatoAditivaDeductivaByID(objAutorizacion.id_AditivaDeductiva);
                AditivaAprobada.rechazado = true;
                AditivaDeductivaFS.GuardarAditivaDeduc(AditivaAprobada);
                result.Add("idFormatoAditivaDeductiva", objAutorizacion.id_AditivaDeductiva);
                result.Add("usuarioEnvia", objAutorizacion.clave_Aprobador);
                result.Add("aprobacion", aprobacion);
                result.Add("autorizacionID", objAutorizacion.id);
                result.Add(SUCCESS, true);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult enviarCorreos(int usuariorecibe, int formatoID, string tipo, int autorizacionID = 0)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioEnvia = UsuarioFS.ListUsersById(objBaseController.getUsuario().id).FirstOrDefault();
                var downloadPDF = (List<Byte[]>)Session["downloadPDF"];
                var usuariosFormatoCambios = AuthAdivaDeductivaFS.getAutorizacion(formatoID);
                List<string> CorreoEnviar = new List<string>();
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
                var folioID = usuariosFormatoCambios.FirstOrDefault().id_AditivaDeductiva;
                var folio = AditivaDeductivaFS.getFormatoAditivaDeductivaByID(folioID).folio;
                if (tipo.Equals("nuevo"))
                {
                    AsuntoCorreo += @" <p class=MsoNormal>
                                                    Se informa que se registro un nuevo Formato de Aditiva-Deductiva de Personal con Folio: &#8220;" + folio + @"&#8221 por el usuario (" + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + @").<o:p></o:p>
                                                </p>";
                }
                else if (tipo.Equals("cambio"))
                {
                    AsuntoCorreo += @" <p class=MsoNormal>
                                                    Se informa que fue realizada una modificación en el Formato de Aditiva-Deductiva De Personal con Folio: &#8220;" + folio + @"&#8221 por el usuario (" + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + @").<o:p></o:p>
                                                </p>";
                }
                else if (tipo.Equals("autoriza"))
                {
                    AsuntoCorreo += @" <p class=MsoNormal>
                                                    Se informa que fue realizada una autorización en el Formato de Aditiva-Deductiva De Personal con Folio: &#8220;" + folio + @"&#8221 por el usuario (" + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + @").<o:p></o:p>
                                                </p>";
                }
                else if (tipo.Equals("rechazar"))
                {
                    AsuntoCorreo += @" <p class=MsoNormal>
                                                    Se informa que el Formato de Aditiva-Deductiva De Personal con Folio: &#8220;" + folio + @"&#8221 fue rechazado por el usuario (" + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + @").<o:p></o:p>
                                                </p>";
                    var autorizacionAditiva = AuthAdivaDeductivaFS.getAutorizacionIndividual(autorizacionID);
                    if (autorizacionAditiva != null && autorizacionAditiva.comentario != null)
                    {
                        AsuntoCorreo += @" <p class=MsoNormal>
                                                <strong>La razón del rechazo fue: </strong> " + HttpUtility.HtmlEncode(autorizacionAditiva.comentario) + @"<o:p></o:p>
                                            </p>";
                    }
                }
                AsuntoCorreo += @" <p class=MsoNormal>
                                                    <o:p>&nbsp;</o:p>
                                                </p><br/><br/>
                                                    <table>
                                                        <thead>
                                                            <tr>
                                                            <th>Nombre Autorizador </th>
                                                            <th>Tipo</th>
                                                            <th>Autorizó</th>
                                                            </tr></thead>
                                                        <tbody>";

                var excepcionesCorreo = UsuarioFS.getPermisosAutorizaCorreo(1);

                List<int> excepcionesCorreoIDs = new List<int>();

                if (excepcionesCorreo.Count > 0)
                {
                    excepcionesCorreoIDs.AddRange(excepcionesCorreo.Select(x => x.usuarioID));
                }

                foreach (var i in usuariosFormatoCambios)
                {
                    AsuntoCorreo += @"<tr>
                                                            <td>" + i.nombre_Aprobador + "</td>" +
                                    "<td>" + i.puestoAprobador + "</td>" +
                                        getEstatus(i) +
                                    "</tr>";
                    var usuarioCorreo = UsuarioFS.ListUsersById(i.clave_Aprobador).FirstOrDefault();

                    if (i.autorizando)
                    {
                        CorreoEnviar.Add(usuarioCorreo.correo);
                    }
                    else
                    {
                        if (!excepcionesCorreoIDs.Contains(i.clave_Aprobador))
                        {
                            CorreoEnviar.Add(usuarioCorreo.correo);
                        }
                    }
                }

                AsuntoCorreo += @"</tbody>" +
                            @"</table>
                                                <p class=MsoNormal>
                                                    <o:p>&nbsp;</o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    Favor de ingresar al sistema SISI , en el apartado de CH en la opción Formato de Aditiva-Deductiva<o:p></o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    <o:p>&nbsp;</o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    <o:p>&nbsp;</o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    PD. Se informa que esta es una notificación autogenerada por el sistema SISI no es necesario dar una respuesta <o:p></o:p>
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
                CorreoEnviar.Add(usuarioEnvia.correo);
             /*   var diana = UsuarioFS.ListUsersById(1019).FirstOrDefault();
                CorreoEnviar.Add(diana.correo);
                var aranza = UsuarioFS.ListUsersById(79552).FirstOrDefault();
                CorreoEnviar.Add(aranza.correo);*/
                try
                {
                    var formato = AditivaDeductivaFS.getFormatoAditivaDeductivaByID(formatoID);
                    var cap = UsuarioFS.ListUsersById(formato.usuarioCap).FirstOrDefault();
                    CorreoEnviar.Add(cap.correo);
                }
                catch (Exception e) { }
                var tipoFormato = "FormatoDeAditivas.pdf";
                #region Remover_Gerardo Reina de seguimiento una ves autorizado
             /*   try
                {
                    if (CorreoEnviar.Contains("g.reina@construplan.com.mx"))
                    {
                        var autorizadores = AuthAdivaDeductivaFS.getAutorizacion(formatoID);
                        var greina = autorizadores.FirstOrDefault(x => x.clave_Aprobador == 1164);
                        if (greina != null)
                        {
                            if(greina.estatus || greina.rechazado)
                            {
                                CorreoEnviar.Remove("g.reina@construplan.com.mx");
                            }
                        }
                        else
                        {

                            CorreoEnviar.Remove("g.reina@construplan.com.mx");
                        }
                    }
                }
                catch{}*/
                #endregion
                GlobalUtils.sendEmailAdjuntoInMemory2("Alerta de Autorizaciones 'ADITIVAS/DEDUCTIVAS'", AsuntoCorreo, CorreoEnviar.Distinct().ToList(), downloadPDF, tipoFormato);
                Session["downloadPDF"] = null;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult enviarCorreofin(List<string> CorreoEnviar, string folio, int formatoID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioEnvia = UsuarioFS.ListUsersById(base.getUsuario().id).FirstOrDefault();
                var downloadPDF = (List<Byte[]>)Session["downloadPDF"];
                var usuariosFormatoCambios = AuthAdivaDeductivaFS.getAutorizacion(formatoID);
                List<string> CorreoEnviarF = new List<string>();
                string AsuntoCorreo = @"<html xmlns:v='urn:schemas-microsoft-com:vml' xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:word' xmlns:m='http://schemas.microsoft.com/office/2004/12/omml' xmlns='http://www.w3.org/TR/REC-html40'>
                                            <head>
                                                <meta http-equiv=Content-Type content='text/html; charset=iso-8859-1'>
                                                <meta name=Generator content='Microsoft Word 15 (filtered medium)'>
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
                                                        <span style='font-size:12.0pt;font-family:'Arial',sans-serif'>Buen día <o:p></o:p>
                                                        </span>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <span style='font-size:12.0pt;font-family:'Arial',sans-serif'><o:p>&nbsp;</o:p>
                                                        </span>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <span style='font-size:12.0pt;font-family:'Arial',sans-serif'>
                                                            Fue autorizada completamente la solicitud con el folio " + folio + @" del módulo &#8220;Formato de Aditiva/Deductiva&#8221;<o:p></o:p></span></p><p class=MsoNormal style='mso-margin-top-alt:auto;mso-margin-bottom-alt:auto'><span style='font-size:12.0pt;font-family:'Arial',sans-serif'>PD. Se informa que esta es una notificación autogenerada por el sistema SIGOPLAN no es necesario dar una respuesta <o:p></o:p>
                                                       </span>
                                                    </p><br/><br/>
                                                    <table>
                                                        <thead>
                                                          <tr>
                                                            <th>Nombre Autorizador </th>
                                                            <th>Tipo</th>
                                                            <th>Autorizó</th>
                                                          </tr></thead>
                                                        <tbody>";
                foreach (var i in usuariosFormatoCambios)
                {
                    AsuntoCorreo += @"<tr>
                                                            <td>" + i.nombre_Aprobador + "</td>" +
                                    "<td>" + i.puestoAprobador + "</td>" +
                                     getEstatus(i) +
                                  "</tr>";
                    var usuarioCorreo = UsuarioFS.ListUsersById(i.clave_Aprobador).FirstOrDefault();

                    var excepcionesCorreo = UsuarioFS.getPermisosAutorizaCorreo(1);

                    if (excepcionesCorreo.Count > 0)
                    {
                        var existUsuario = excepcionesCorreo.FirstOrDefault(x => x.usuarioID == usuarioCorreo.id);

                        if (existUsuario == null)
                        {
                            CorreoEnviarF.Add(usuarioCorreo.correo);
                        }
                    }
                    else
                    {
                        CorreoEnviarF.Add(usuarioCorreo.correo);
                    }
                }

                AsuntoCorreo += @"</tbody>" +
                            @"</table>
                                                    <p class=MsoNormal style='mso-margin-top-alt:auto;mso-margin-bottom-alt:auto'>
                                                        <span style='font-size:12.0pt;font-family:'Arial',sans-serif'>
                                                            Gracias.<o:p></o:p>
                                                        </span>
                                                    </p>
                                                </div>
                                           </body>
                                        </html>";
                CorreoEnviarF.Add(usuarioEnvia.correo);
                /*var diana = UsuarioFS.ListUsersById(1019).FirstOrDefault();
                CorreoEnviarF.Add(diana.correo);
                var aranza = UsuarioFS.ListUsersById(79552).FirstOrDefault();
                CorreoEnviar.Add(aranza.correo);*/
                try
                {
                    var formato = AditivaDeductivaFS.getFormatoAditivaDeductivaByID(formatoID);
                    var cap = UsuarioFS.ListUsersById(formato.usuarioCap).FirstOrDefault();
                    CorreoEnviar.Add(cap.correo);
                }
                catch (Exception e) { }
                var tipoFormato = "FormatoDeAditivas.pdf";
                #region Remover_Gerardo Reina de seguimiento una ves autorizado
             /*   try
                {
                    if (CorreoEnviar.Contains("g.reina@construplan.com.mx"))
                    {
                        var autorizadores = AuthAdivaDeductivaFS.getAutorizacion(formatoID);
                        var greina = autorizadores.FirstOrDefault(x => x.clave_Aprobador == 1164);
                        if (greina != null)
                        {
                            if(greina.estatus || greina.rechazado)
                            {
                                CorreoEnviar.Remove("g.reina@construplan.com.mx");
                            }
                        }
                        else
                        {

                            CorreoEnviar.Remove("g.reina@construplan.com.mx");
                        }
                    }
                }
                catch{}*/

                #endregion
                GlobalUtils.sendEmailAdjuntoInMemory2("Alerta de Autorizaciones", AsuntoCorreo, CorreoEnviarF.Distinct().ToList(), downloadPDF, tipoFormato);
                Session["downloadPDF"] = null;
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        private string getEstatus(tblRH_AutorizacionAditivaDeductiva o)
        {
            if (o.autorizando)
            {
                return "<td style='background-color: yellow;'>AUTORIZANDO</td>";
            }
            else if (o.estatus)
            {
                return "<td style='background-color: #82E0AA;'>AUTORIZADO</td>";
            }
            else
            {
                if (o.rechazado == true)
                {
                    return "<td style='background-color: #EC7063;'>RECHAZADO</td>";
                }
                else
                {
                    return "<td style='background-color: #FAE5D3;'>PENDIENTE</td>";
                }
            }
        }
        public ActionResult TablaFormatosPendientes(string CC, string folio, int id, int estado)
        {
            List<tblRH_AditivaDeductiva> lstObjAditivaDeductiva = new List<tblRH_AditivaDeductiva>();

            lstObjAditivaDeductiva = AditivaDeductivaFS.getListAditivaDeductivaPendientes(id, CC, folio, estado);
            lstObjAditivaDeductiva = ObtencionEditables(lstObjAditivaDeductiva);
            return Json(lstObjAditivaDeductiva, JsonRequestBehavior.AllowGet);
        }

        private delegate bool EsMismoUsuarioCC(int usuarioCapturoID);

        public List<tblRH_AditivaDeductiva> ObtencionEditables(List<tblRH_AditivaDeductiva> lstObjAditivaDeductiva)
        {
            int usuarioLog = objBaseController.getUsuario().id;
            List<tblRH_AditivaDeductiva> listaMostrar = new List<tblRH_AditivaDeductiva>();
            foreach (var objadv in lstObjAditivaDeductiva)
            {
                List<tblRH_AutorizacionAditivaDeductiva> listAp = AuthAdivaDeductivaFS.getAutorizacion(objadv.id);
                var ud = new UsuarioDAO();
                var rh = ud.getViewAction(vSesiones.sesionCurrentView, "VerTodoFormato");
                EsMismoUsuarioCC esMismoUsuarioCC = AuthAdivaDeductivaFS.EsUsuarioMismoCC;
                if (objadv.usuarioCap == usuarioLog || getAction("EditarAditiva") || rh || esMismoUsuarioCC(objadv.usuarioCap))
                {
                    var isEditarTodo = ud.getViewAction(vSesiones.sesionCurrentView, "EditarTodoAditiva");
                    if (isEditarTodo)
                    {
                        objadv.editable = true;
                    }
                    else
                    {
                        objadv.editable = true;
                        foreach (var objAp in listAp)
                        {
                            if (objAp.estatus)
                            {
                                objadv.editable = false;
                                break;
                            }
                            else if (objAp.rechazado)
                            {
                                objadv.editable = false;
                                break;
                            }
                            else {
                                objadv.editable = true;
                                break;
                            }
                        }
                    }
                    listaMostrar.Add(objadv);
                }
                else
                {
                    var isEditarTodo = ud.getViewAction(vSesiones.sesionCurrentView, "EditarTodoAditiva");
                    if (isEditarTodo)
                    {
                        objadv.editable = true;
                        foreach (var objAp in listAp)
                        {
                            if (objAp.clave_Aprobador == usuarioLog && objAp.autorizando)
                                listaMostrar.Add(objadv);
                        }
                    }
                    else
                    {
                        var objAp = listAp.FirstOrDefault(w => w.clave_Aprobador.Equals(usuarioLog));
                        if (objAp != null)
                        {
                            objadv.editable = false;
                            listaMostrar.Add(objadv);
                        }
                    }

                }

            }

            return listaMostrar;
        }
        public ActionResult getAditivadeductivaEditar(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {

                List<tblRH_AditivaDeductivaDet> lstObjAditivaDeductiva = new List<tblRH_AditivaDeductivaDet>();
                lstObjAditivaDeductiva = AditivaDeductivaDetFS.getAditivaDeductivaDet(id);
                lstObjAditivaDeductiva.Sort((p, q) => string.Compare(p.puesto, q.puesto));
                result.Add("Detalle", lstObjAditivaDeductiva);
                result.Add("Autorizacion", AuthAdivaDeductivaFS.getAutorizacion(id));
                result.Add("AditivaDeductiva", AditivaDeductivaFS.getListAditivaDeductivaPendientes(id, "", "", 1));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutorizarFormato(int plantillaID, int autorizacion, int estatus, string comentario)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var resultData = AditivaDeductivaFS.AutorizarPlantilla(plantillaID, autorizacion, estatus, comentario);

                result.Add(SUCCESS, resultData);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EnviarCorreoFormato(int plantillaID, int autorizacion, int estatus)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var resultData = AditivaDeductivaFS.EnviarCorreo(plantillaID, autorizacion, estatus);

                result.Add("enviado", resultData);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubirEvidenciaSolicitud()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var solicitudID = int.Parse(Request.Form["solicitudID"]);
                HttpPostedFileBase file = Request.Files["fuEvidencia"];

                string FileName = "";
                string ruta = "";
                bool pathExist = false;
                DateTime fecha = DateTime.Now;
                string f = fecha.ToString("ddMMyyyy") + fecha.Hour + "" + fecha.Minute;
                FileName = file.FileName;

                ruta = ArchivoFS.getArchivo().getUrlDelServidor(1018) + f + FileName;
                pathExist = GuardarDocumentos(file, ruta);

                if (pathExist)
                {
                    AditivaDeductivaFS.GuardarSolicitudEvidencia(solicitudID, ruta);
                }

                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getFileRuta(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var Archivo = AditivaDeductivaFS.getFormatoAditivaDeductivaByID(id).link;
                var esSuccess = Archivo.Length > 0;
                if (esSuccess)
                {
                    var ruta = Archivo.Replace("C:\\", "\\\\REPOSITORIO\\");
                    result.Add("ruta", ruta);
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public FileResult getFileDownload()
        {
            try
            {
                int id = Convert.ToInt32(Request.QueryString["id"]);
                var Archivo = AditivaDeductivaFS.getFormatoAditivaDeductivaByID(id).link;
                var ruta = Archivo.Replace("C:\\", "\\\\REPOSITORIO\\");
                return File(ruta, "multipart/form-data", Archivo);
            }
            catch (Exception)
            {

                return null;
            }

        }

        private bool GuardarDocumentos(HttpPostedFileBase archivo, string ruta)
        {
            bool result = false;
            byte[] data;
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
            ruta = ruta.Replace("C:\\", "\\REPOSITORIO\\");
            System.IO.File.WriteAllBytes(ruta, data);
            result = System.IO.File.Exists(ruta);
            return result;
        }

    }
}
