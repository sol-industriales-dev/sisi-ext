using Core.DTO;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria.Catalogo;
using Core.Enum.Multiempresa;
using Data.DAO.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Catalogos.Obra
{
    public class CatObraController : BaseController
    {


        #region Factory
        UsuarioFactoryServices usuarioFactoryServices;
        CentroCostosFactoryServices centroCostosFactoryServices;
        #endregion
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            usuarioFactoryServices = new UsuarioFactoryServices();
            centroCostosFactoryServices = new CentroCostosFactoryServices();
            base.OnActionExecuting(filterContext);
        }

        // GET: CatObra
        public ActionResult Index()
        {
            ViewBag.pagina = "catalogo";
            return View();
        }

        public ActionResult FillGridCentroCostos(tblM_CentroCostos obj)
        {

            var result = new Dictionary<string, object>();
            try
            {
                CentroCostosDAO cc = new CentroCostosDAO();
                var listResult = cc.FillGridCC(obj);

                result.Add("current", 1);
                result.Add("rowCount", 1);
                result.Add("total", listResult.Count());
                result.Add("rows", listResult);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cboCentroCostosSIGOPLAN()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioID = getUsuario().id;


                var listaCCusuario = usuarioFactoryServices.getUsuarioService().getCCsUsuario(usuarioID).Select(x => x.cc);


                var listaCentroCostosActuales = centroCostosFactoryServices.getCentroCostosService().getListaCCSIGOPLAN();
                if (base.getAction("AllCC"))
                {

                    result.Add(ITEMS, listaCentroCostosActuales.Select(y => new
                    {
                        Value = y.Value,
                        Text = y.Text
                    }).OrderBy(x => x.Value));
                }
                else
                {
                    List<ComboDTO> Resultado = listaCentroCostosActuales.Where(x => listaCCusuario.Contains(x.Prefijo)).Select(y => new ComboDTO
                    {
                        Value = y.Value,
                        Text = y.Text
                    }).ToList();

                    if (listaCCusuario.Contains("1010"))
                    {
                        Resultado.Add(new ComboDTO
                        {
                            Value = "1010",
                            Text = "1010-TALLER DE MAQUINARIA."
                        });
                    }
                    if (listaCCusuario.Contains("1015"))
                    {
                        Resultado.Add(new ComboDTO
                        {
                            Value = "1015",
                            Text = "1015-PATIO DE MAQUINARIA."
                        });
                    }

                    result.Add(ITEMS, Resultado.OrderBy(x => x.Value));
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

        public ActionResult cboCentroCostosConstruplanGeneral()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioID = getUsuario().id;

                var listaCCusuario = usuarioFactoryServices.getUsuarioService().getCCsUsuario(usuarioID).Select(x => x.cc);
                var listaCentroCostosActuales = centroCostosFactoryServices.getCentroCostosService().getListaCCSIGOPLAN();

                if (base.getAction("AllCC"))
                {

                    result.Add(ITEMS, listaCentroCostosActuales.Select(y => new
                    {
                        Value = y.Value,
                        Text = y.Text
                    }).OrderBy(x => x.Value));
                }
                else
                {
                    List<ComboDTO> Resultado = listaCentroCostosActuales.Where(x => listaCCusuario.Contains(x.Value)).Select(y => new ComboDTO
                    {
                        Value = y.Value,
                        Text = y.Text
                    }).ToList();

                    if (listaCCusuario.Contains("1010"))
                    {
                        Resultado.Add(new ComboDTO
                        {
                            Value = "1010",
                            Text = "1010-TALLER DE MAQUINARIA."
                        });
                    }
                    if (listaCCusuario.Contains("1015"))
                    {
                        Resultado.Add(new ComboDTO
                        {
                            Value = "1015",
                            Text = "1015-PATIO DE MAQUINARIA."
                        });
                    }



                    result.Add(ITEMS, Resultado.OrderBy(x => x.Value));
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

        public ActionResult cboCentroCostosUsuarios()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioID = getUsuario().id;


                var listaCCusuario = usuarioFactoryServices.getUsuarioService().getCCsByUsuario(usuarioID).OrderBy(x => x.area).ThenBy(x => x.cuenta).ToList();


                var listaCentroCostosActuales = centroCostosFactoryServices.getCentroCostosService().getListaCC();
                if (base.getAction("AllCC"))
                {

                    result.Add(ITEMS, listaCentroCostosActuales);
                }
                else
                {
                    List<ComboDTO> Resultado = new List<ComboDTO>();
                    if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual==3 )
                    {
                        Resultado = listaCCusuario.Select(y => new ComboDTO
                        {
                            Value = y.cc,
                            Text = y.cc + " - " + y.descripcion,
                            Prefijo = y.abreviacion

                        }).ToList();
                    }else if(vSesiones.sesionEmpresaActual==6)
                    {
                        Resultado = listaCCusuario.Select(y => new ComboDTO
                        {
                            Value = y.areaCuenta,
                            Text = y.cc + " - " + y.descripcion,
                            Prefijo = y.abreviacion

                        }).ToList();
                    }
                    else
                    {
                        Resultado = listaCCusuario.Select(y => new ComboDTO
                        {
                            Value = y.areaCuenta,
                            Text = y.areaCuenta + " - " + y.descripcion,
                            Prefijo = y.abreviacion

                        }).ToList();
                    }
                    result.Add(ITEMS, Resultado);
                }

                //result.Add(ITEMS, maquinaFactoryServices.getMaquinaServices().getCboMaquinaria(obj).Select(x => new { Value = x.noEconomico, Text = x.noEconomico }).OrderBy(x => x.Text));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult getNameCC(int obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = centroCostosFactoryServices.getCentroCostosService().getNombreCC(obj);

                result.Add(SUCCESS, true);
                result.Add("descripcionCC", res);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}