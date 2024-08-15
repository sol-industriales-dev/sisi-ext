using Core.DAO.Maquinaria.Captura;
using Core.DAO.Maquinaria.Catalogos;
using Core.DAO.Maquinaria.Inventario;
using Core.DAO.Maquinaria.Reporte.CuadroComparativo;
using Core.Enum.Maquinaria.Reportes.CuadroComparativo.Equipo;
using Data.Factory.Maquinaria.Captura;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Inventario;
using Data.Factory.Maquinaria.Reporte.CuadroComparativo;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
using System.Web.Mvc;
using Core.DTO.Principal.Generales;

namespace SIGOPLAN.Controllers.Maquinaria.Reportes
{
    public class CuadroComparativoController : BaseController
    {
        #region init
        private Dictionary<string, object> Resultado;
        private ICCEquipoDAO CCEquipoFS;
        private ICCFinanieroDAO CCFinancieroFS;
        private IMaquinariaRentada RentaFS;
        private IMaquinaDAO MaquinaFS;
        private IMarcaEquipoDAO MarcaFS;
        private IModeloEquipoDAO ModeloFS;
        private IAsignacionEquiposDAO AsignacionFS;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Resultado = new Dictionary<string, object>();
            CCEquipoFS = new CCEquipoFactoryServices().getEquipoServices();
            CCFinancieroFS = new CCFinancieroFactoryServices().getFinancieroServices();
            RentaFS = new MaquinariaRentadaFactoryServices().getMaquinariaRentadaServices();
            MaquinaFS = new MaquinaFactoryServices().getMaquinaServices();
            MarcaFS = new MarcaEquipoFactoryServices().getMarcaEquipoService();
            ModeloFS = new ModeloEquipoFactoryServices().getModeloEquipoService();
            AsignacionFS = new AsignacionEquiposFactoryServices().getAsignacionEquiposFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        #endregion
        #region _formCCEquipo
        public ActionResult _formCCEquipo()
        {
            return PartialView();
        }
        public ActionResult initFormEquipo()
        {
            try
            {
                var OptProveedores = from prov in RentaFS.FillCboProveedor()
                                     select new
                                     {
                                         Text = string.Format("{0} - {1}", prov.NUMPROVEEDOR, prov.NOMBRE2),
                                         Value = prov.NUMPROVEEDOR,
                                         Prefijo = prov.MONEDA
                                     };
                var OptMarcas = MarcaFS.GetLstMarcaActivas().Select(x =>
                {
                    var grupos = MarcaFS.GetGruposByMarca(x.id);
                    return new {
                        Text = x.descripcion,
                        Value = x.id.ToString(),
                        Prefijo = (grupos != null ? grupos.Select(s => s.id.ToString()) : Enumerable.Empty<string>()).ToArray()
                    };
                }).ToList();
                    
                    
                    //from marca in MarcaFS.GetLstMarcaActivas()
                    //            select new ComboDTO
                    //            {
                    //                Text = marca.descripcion,
                    //                Value = marca.id.ToString(),
                    //                Prefijo = (marca.grupo != null ? marca.grupo.Select(s => s.id.ToString()) : Enumerable.Empty<string>()).ToArray()
                    //            };
                var OptModelos = from modelo in ModeloFS.GetLstModeloActivos()
                                 select new
                                 {
                                     Text = modelo.descripcion,
                                     Value = modelo.id,
                                     Prefijo = modelo.marcaEquipoID.ToString()
                                 };
                var Catalogos = CCEquipoFS.LstCatalogoActivo().OrderBy(o => o.Orden).ToList();
                var Adquisiciones = EnumExtensions.ToCombo<AdquisicionEnum>();
                Resultado.Add("lstAdquisicion", Adquisiciones);
                Resultado.Add("optProveedor", OptProveedores);
                Resultado.Add("optMarca", OptMarcas);
                Resultado.Add("optModelo", OptModelos);
                Resultado.Add("lstCatalogo", Catalogos);
                Resultado.Add(SUCCESS, OptProveedores.Any() && OptMarcas.Any() && OptModelos.Any() && Catalogos.Any());
            }
            catch(Exception o_O)
            {
                Resultado.Add(SUCCESS, false);
                Resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(Resultado, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult CuadroComparativoEquipo(int idAsignacion)
        {
            try
            {
                var cuadro = CCEquipoFS.GetCuadroEquipo(idAsignacion);
                Resultado.Add("cuadro", cuadro);
                Resultado.Add(SUCCESS, true);
            }
            catch(Exception o_O)
            {
                Resultado.Add(SUCCESS, false);
                Resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(Resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Auxiliares
        
        #endregion
    }
}