using Core.DAO.Administracion.DocumentosXPagar;
using Core.DTO;
using Core.DTO.Administracion.DocumentosXPagar;
using Core.DTO.Contabilidad;
using Core.DTO.Contabilidad.Bancos;
using Core.DTO.Contabilidad.DocumentosXPagar;
using Core.DTO.Contabilidad.DocumentosXPagar.Reporte_Adeudo;
using Core.DTO.Contabilidad.DocumentosXPagar.Reportes;
using Core.DTO.Contabilidad.FlujoEfectivo;
using Core.DTO.Enkontrol;
using Core.DTO.Maquinaria.Reporte.ActivoFijo;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contabilidad.Cheque;
using Core.Entity.Administrativo.Contabilidad.FlujoEfectivo;
using Core.Entity.Administrativo.DocumentosXPagar;
using Core.Entity.Administrativo.DocumentosXPagar.PQ;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Multiempresa;
using Core.Enum.Administracion.DocumentosXPagar;
using Core.Enum.Administracion.FlujoEfectivo;
using Core.Enum.Generico.Fecha;
using Core.Enum.Multiempresa;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.EntityFramework.Mapping.Administrativo.DocumentosXPagar;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ws_validacionCheques.DTO;
using System.Data.Entity.Migrations;
using Core.DTO.Contabilidad.DocumentosXPagar.PQ;
using Data.Factory.Contabilidad.Poliza;
using Core.DTO.Enkontrol.Tablas.Poliza;
using Core.DAO.Contabilidad.Poliza;
using Core.Entity.Administrativo.Contabilidad.Poliza;
using Core.DAO.Contabilidad.Cuenta;
using Data.Factory.Contabilidad.Cuenta;
using Core.DTO.Enkontrol.Tablas.Cuenta;
using Core.Enum.Contabilidad.Poliza;
using Core.DTO.Contabilidad.Poliza;
using Core.DAO.Principal.Archivos;
using Data.Factory.Principal.Archivos;
using System.IO;
using Data.Factory.Contabilidad.Banco;
using Core.DAO.Contabilidad.Banco;
using Core.DAO.Enkontrol.General.CC;
using Data.Factory.Enkontrol.General.CC;
using System.Web.Mvc;
using Core.DTO.Administracion.DocumentosXPagar.Poliza_revaluacion;
using Core.Enum.Contabilidad.Moneda;
using Core.Enum.Principal;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.Drawing;
using Core.Entity.SubContratistas.Usuarios;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Principal.Usuario;
using Core.Entity.Maquinaria.Inventario;


namespace Data.DAO.Administracion.DocumentosXPagar
{
    public class DocumentosXPagarDAO : GenericDAO<tblAF_DxP_Contrato>, IContratosDAO
    {
        private bool productivo = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["enkontrolProductivo"]) == "1";

        private EnkontrolEnum conexionEK = getConexion();

        private List<string> acVirtuales = new List<string>
        {
            "0",
            "1010",
            "1018",
            "1015",
            "997"
        };

        IDirArchivosDAO archivoFS = new ArchivoFactoryServices().getArchivo();

        ICCDAO _ccFS = new CCFactoryService().getCCService();
        IBancoDAO _bancoFS = new BancoFactoryService().GetBancoEkService();
        ICuentaDAO cuentaFS = new CuentaFactoryService().GetCuentaEkService();

        IPolizaSPDAO _polizaEkFS = new PolizaSPFactoryService().GetPolizaEkService();
        IPolizaSPDAO _polizaSPFS = new PolizaSPFactoryService().GetPolizaSPService();

        List<tblM_CatMaquina> _maquinas = new List<tblM_CatMaquina>();
        List<tblP_CC> _ccs = new List<tblP_CC>();

        /// <summary> 
        /// Constructor
        /// </summary>
        private static EnkontrolEnum getConexion()
        {
            int tipoAmbiente = vSesiones.sesionEmpresaActual;
            //  tipoAmbiente = 3;
            switch (tipoAmbiente)
            {
                case 1:
                    return EnkontrolEnum.CplanProd;
                case 2:
                    return EnkontrolEnum.ArrenProd;
                case 3:
                    return EnkontrolEnum.PruebaCplanProd;
                default:
                    return 0;
            }
        }
        #region Documentos por pagar.

        public Respuesta ObtenerContratos(FiltroContratosDTO filtro)
        {
            var r = new Respuesta();
            try
            {
                List<tblAF_DxP_ContratoDetalle> contratosConDetalle = null;

                if (filtro == null)
                {
                    contratosConDetalle = _context.tblAF_DxP_ContratosDetalle.Include("Contrato").Where(x => x.Contrato.Estatus && x.Estatus && x.Contrato.empresa == filtro.empresa).ToList();
                }
                else
                {
                    contratosConDetalle = _context.tblAF_DxP_ContratosDetalle.Include("Contrato").Where
                        (x => x.Contrato.empresa == filtro.empresa &&
                            x.Contrato.Estatus && x.Estatus &&
                            x.Contrato.arrendamientoPuro == filtro.arrendamiento &&
                            (
                                (!string.IsNullOrEmpty(filtro.Folio)) && (x.Contrato.Folio == filtro.Folio) ||
                                (string.IsNullOrEmpty(filtro.Folio))
                            ) &&
                            (
                                (!string.IsNullOrEmpty(filtro.Descripcion)) && (x.Contrato.Descripcion == filtro.Descripcion) ||
                                (string.IsNullOrEmpty(filtro.Descripcion))
                            ) &&
                            (
                                (filtro.Fecha != null) && (x.Contrato.FechaInicio == filtro.Fecha.Value) ||
                                (filtro.Fecha == null)
                            ) &&
                             (
                                (filtro.financiera != 0 ? filtro.financiera == x.Contrato.InstitucionId : true)

                            )
                        ).ToList();
                }

                List<ContratosDTO> contratosDTO = new List<ContratosDTO>();

                foreach (var contratoConDetalle in contratosConDetalle.GroupBy(g => g.Contrato.Id))
                {
                    ContratosDTO contratoDTO = new ContratosDTO();

                    var contrato = contratoConDetalle.First().Contrato;
                    var contratoPeriodosALaFecha = contratoConDetalle.Where(x => x.FechaVencimiento <= DateTime.Now || (x.FechaVencimiento.Year == DateTime.Now.Year && x.FechaVencimiento.Month == DateTime.Now.Month));

                    contratoDTO.Id = contrato.Id;
                    contratoDTO.Folio = contrato.Folio;
                    contratoDTO.Descripcion = contrato.Descripcion;
                    contratoDTO.Institucion = contrato.Institucion.Nombre;
                    contratoDTO.Plazo = contrato.Plazo;
                    contratoDTO.InteresMoratorio = contrato.InteresMoratorio;
                    contratoDTO.Domiciliado = contrato.Domiciliado;

                    var diaInicio = contrato.FechaInicio.Day;
                    var fechaActual = DateTime.Now;
                    int mesesFaltantes = 0;

                    mesesFaltantes = ((fechaActual.Year - contrato.FechaInicio.Year) * 12) + fechaActual.Month - contrato.FechaInicio.Month;

                    switch (contrato.FechaVencimientoTipoId)
                    {
                        case (int)DiasVencimientoEnum.UltimoDiaMes:
                            if (fechaActual.Day >= DateTime.DaysInMonth(fechaActual.Year, fechaActual.Month))
                            {
                                mesesFaltantes++;
                            }
                            break;
                        case (int)DiasVencimientoEnum.Dia15:
                            if (fechaActual.Day > 15)
                            {
                                mesesFaltantes++;
                            }
                            break;
                        case (int)DiasVencimientoEnum.Seleccion:
                            if (fechaActual.Day >= contrato.FechaVencimiento.Value.Day)
                            {
                                mesesFaltantes++;
                            }
                            break;
                    }
                    contratoDTO.ParcialidadActual = mesesFaltantes > contrato.Plazo ? contrato.Plazo : mesesFaltantes;

                    contratoDTO.FechaInicio = contrato.FechaInicio;
                    contratoDTO.FechaVencimiento = contratoPeriodosALaFecha.Count() > 0 ? contratoPeriodosALaFecha.Last().FechaVencimiento : (DateTime)contrato.FechaVencimiento;
                    contratoDTO.DiaInhabil = (int)contratoDTO.FechaVencimiento.DayOfWeek == 0 && contratoDTO.Domiciliado ? true : false;
                    contratoDTO.Credito = contrato.Credito;
                    contratoDTO.AmortizacionCapital = contrato.AmortizacionCapital;
                    contratoDTO.Intereses = contrato.TasaInteres;
                    contratoDTO.TipoCambio = contrato.TipoCambio;
                    contratoDTO.PagosVencidos = contratoPeriodosALaFecha.Count() > 0 ? contratoPeriodosALaFecha.Where(x => !x.Pagado).Count() : 0;

                    contratoDTO.ArchivoContrato = string.IsNullOrEmpty(contrato.FileContrato) ? false : true;
                    contratoDTO.ArchivoPagare = string.IsNullOrEmpty(contrato.FilePagare) ? false : true;
                    contratoDTO.arrendamientoPuro = contrato.arrendamientoPuro ? 1 : 2;
                    contratosDTO.Add(contratoDTO);
                }

                //Jessica Galdean, Lili Lavandera
                List<int> lstAdminID = new List<int>() { 3978, 6587, 6278, 79888, 1073 };

                //tblP_Usuario objUsr = _context.tblP_Usuario.Where(e => e.cveEmpleado == vSesiones.sesionUsuarioDTO.id.ToString()).FirstOrDefault();

                if (contratosDTO.Count()>0)
                {
                    if (vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.ADMINISTRADOR)
                    {
                        contratosDTO[0].esAdmin = true;
                    }
                    else
                    {

                        if (lstAdminID.Contains(vSesiones.sesionUsuarioDTO.id))
                        {
                            contratosDTO[0].esAdmin = true;

                        }
                        else
                        {
                            contratosDTO[0].esAdmin = false;

                        }
                        
                    }
                }

                r.Success = true;
                r.Message = "Contratos obtenidos: " + contratosDTO.Count;
                r.Value = contratosDTO;
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }
            return r;
        }
        public List<autoCompleteCta> autoComplCatCtas(string term)
        {
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"SELECT TOP 15  cta,scta,sscta,digito,descripcion,
                                (CAST(cta AS varchar) + '-' + CAST(scta AS varchar) + '-' + CAST(sscta AS varchar) + '-' + CAST(digito AS varchar)) AS id,
                                (CAST(cta AS varchar) + '-' + CAST(scta AS varchar) + '-' + CAST(sscta AS varchar) + '-' + CAST(digito AS varchar) + ' ' + descripcion) AS label
                            FROM catcta
                            WHERE label like ?";
            odbc.parametros.Add(new OdbcParameterDTO() { nombre = "label", tipo = OdbcType.VarChar, valor = string.Format("%{0}%", term) });
            var lstCtas = _contextEnkontrol.Select<autoCompleteCta>(productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, odbc);
            return lstCtas;
        }
        #region SE OBTIENE LOS DATOS DE ctaIA
        public List<autoCompleteCta> autoCompleteCtasIA(string term)
        {
            List<autoCompleteCta> lstData = new List<autoCompleteCta>();
            string strQuery = @"SELECT TOP 15  cta,scta,sscta,digito,descripcion,
                                (CAST(cta AS varchar) + '-' + CAST(scta AS varchar) + '-' + CAST(sscta AS varchar) + '-' + CAST(digito AS varchar)) AS id,
                                (CAST(cta AS varchar) + '-' + CAST(scta AS varchar) + '-' + CAST(sscta AS varchar) + '-' + CAST(digito AS varchar) + ' ' + descripcion) AS label
                            FROM catcta
                            WHERE label LIKE '%{0}%'";
            var odbc = new OdbcConsultaDTO() { consulta = strQuery };
            odbc.consulta = String.Format(strQuery, term);

            lstData = _contextEnkontrol.Select<autoCompleteCta>(productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, odbc);
            return lstData;
        }
        #endregion
        public Dictionary<string, object> ObtenerContratosNotificaciones()
        {
            Dictionary<string, object> resultados = new Dictionary<string, object>();
            try
            {
                List<tblAF_DxP_ContratoDetalle> contratosConDetalle = null;

                contratosConDetalle = _context.tblAF_DxP_ContratosDetalle.Include("Contrato").Where(x => x.Contrato.Estatus && x.Estatus).ToList();

                List<ContratosDTO> contratosDTO = new List<ContratosDTO>();

                foreach (var contratoConDetalle in contratosConDetalle.GroupBy(g => g.Contrato.Id))
                {
                    ContratosDTO contratoDTO = new ContratosDTO();

                    var contrato = contratoConDetalle.First().Contrato;
                    var contratoPeriodosALaFecha = contratoConDetalle.Where(x => x.FechaVencimiento <= DateTime.Now || (x.FechaVencimiento.Year == DateTime.Now.Year && x.FechaVencimiento.Month == DateTime.Now.Month));

                    contratoDTO.Id = contrato.Id;
                    contratoDTO.Folio = contrato.Folio;
                    contratoDTO.Descripcion = contrato.Descripcion;
                    contratoDTO.Institucion = contrato.Institucion.Nombre;
                    contratoDTO.Plazo = contrato.Plazo;
                    contratoDTO.InteresMoratorio = contrato.InteresMoratorio;
                    contratoDTO.Domiciliado = contrato.Domiciliado;

                    var diaInicio = contrato.FechaInicio.Day;
                    var fechaActual = DateTime.Now;
                    int mesesFaltantes = 0;

                    mesesFaltantes = ((fechaActual.Year - contrato.FechaInicio.Year) * 12) + fechaActual.Month - contrato.FechaInicio.Month;

                    switch (contrato.FechaVencimientoTipoId)
                    {
                        case (int)DiasVencimientoEnum.UltimoDiaMes:
                            if (fechaActual.Day >= DateTime.DaysInMonth(fechaActual.Year, fechaActual.Month))
                            {
                                mesesFaltantes++;
                            }
                            break;
                        case (int)DiasVencimientoEnum.Dia15:
                            if (fechaActual.Day > 15)
                            {
                                mesesFaltantes++;
                            }
                            break;
                        case (int)DiasVencimientoEnum.Seleccion:
                            if (fechaActual.Day >= contrato.FechaVencimiento.Value.Day)
                            {
                                mesesFaltantes++;
                            }
                            break;
                    }
                    contratoDTO.ParcialidadActual = mesesFaltantes > contrato.Plazo ? contrato.Plazo : mesesFaltantes;

                    contratoDTO.FechaInicio = contrato.FechaInicio;
                    contratoDTO.FechaVencimiento = contratoPeriodosALaFecha.Last().FechaVencimiento;
                    contratoDTO.DiaInhabil = (int)contratoDTO.FechaVencimiento.DayOfWeek == 0 && contratoDTO.Domiciliado ? true : false;
                    contratoDTO.Credito = contrato.Credito;
                    contratoDTO.AmortizacionCapital = contrato.AmortizacionCapital;
                    contratoDTO.Intereses = contrato.TasaInteres;
                    contratoDTO.TipoCambio = contrato.TipoCambio;
                    contratoDTO.PagosVencidos = contratoPeriodosALaFecha.Where(x => !x.Pagado).Count();
                    contratoDTO.ArchivoContrato = string.IsNullOrEmpty(contrato.FileContrato) ? false : true;
                    contratoDTO.ArchivoPagare = string.IsNullOrEmpty(contrato.FilePagare) ? false : true;

                    contratosDTO.Add(contratoDTO);
                }

                resultados.Add(SUCCESS, true);
                resultados.Add("value", contratosDTO.Where(n => n.DiaInhabil));

            }
            catch (Exception ex)
            {
                resultados.Add(SUCCESS, false);
            }

            return resultados;
        }
        public Dictionary<string, object> ObtenerContratoByID(int id)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();

            try
            {
                var contrato = _context.tblAF_DxP_Contratos.FirstOrDefault(i => i.Id == id);
                var maquinaria = _context.tblAF_DxP_ContratoMaquinas.Where(i => i.Contrato.Id == id).Select(i => new
                {
                    id = i.Id,
                    noEconomico = i.Maquina.noEconomico,
                    noSerie = i.Maquina.noSerie,
                    credito = i.Credito,
                    porcentaje = i.porcentaje,
                    economicoID = i.MaquinaId
                });

                ContratosDTO objResult = new ContratosDTO();
                objResult.Id = contrato.Id;
                objResult.Folio = contrato.Folio;
                objResult.Descripcion = contrato.Descripcion;
                objResult.Plazo = contrato.Plazo;
                objResult.InstitucionId = contrato.InstitucionId;
                objResult.FechaInicio = contrato.FechaInicio;
                objResult.FechaVencimientoTipoId = contrato.FechaVencimientoTipoId;
                objResult.Credito = contrato.Credito;
                objResult.AmortizacionCapital = contrato.AmortizacionCapital;
                objResult.TasaInteres = contrato.TasaInteres;
                objResult.InteresMoratorio = contrato.InteresMoratorio;
                objResult.TipoCambio = contrato.TipoCambio;
                objResult.Domiciliado = contrato.Domiciliado;
                objResult.rfc = contrato.rfc;
                objResult.montoOpcioncompra = contrato.montoOpcioncompra;
                objResult.penaConvencional = contrato.penaConvencional;
                objResult.fechaFirma = ((DateTime)contrato.fechaFirma).ToShortDateString();
                objResult.fechaVencimiento = ((DateTime)contrato.FechaVencimiento).ToShortDateString();
                objResult.monedaContrato = contrato.monedaContrato;
                objResult.nombreCorto = contrato.nombreCorto;
                objResult.empresa = contrato.empresa;

                objResult.pagoInterino = contrato.PagoInterino;
                objResult.pagoInterino2 = contrato.PagoInterino2;
                objResult.depGarantia = contrato.DepGarantia;

                objResult.cta = contrato.cta;
                objResult.scta = contrato.scta;
                objResult.sscta = contrato.sscta;
                objResult.digito = contrato.digito;
                objResult.ctaConcat = objResult.cta + "-" + objResult.scta + "-" + objResult.sscta + "-" + objResult.digito;
                objResult.aplicaInteres = contrato.aplicaInteres;
                objResult.aplicaContratoIntereses = contrato.aplicaContratoInteres;
                objResult.tasaFija = contrato.tasaFija;
                objResult.fileContrato = contrato.FileContrato.Split('\\').Last();
                objResult.arrendamientoPuro = contrato.arrendamientoPuro ? 1 : 2;

                r.Add(SUCCESS, true);
                r.Add(MESSAGE, "OK");
                r.Add("Value", objResult);
                r.Add("fecha", objResult.FechaInicio.ToShortDateString());
                r.Add("maquinaria", maquinaria);

            }
            catch (Exception e)
            {

                throw;
            }
            return r;

        }
        public Respuesta GuardarContrato(tblAF_DxP_Contrato contrato, List<AgregarMaquinaDTO> listaEconomicos, bool tasaFija)
        {
            var r = new Respuesta();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    contrato.IVA = 0.16M;
                    contrato.Terminado = false;
                    contrato.Estatus = true;
                    if (contrato.Id == 0)
                    {
                        contrato.FechaCreacion = DateTime.Now;
                        contrato.UsuarioCreacionId = vSesiones.sesionUsuarioDTO.id;
                    }

                    contrato.FechaModificacion = contrato.FechaCreacion;
                    contrato.UsuarioModificacionId = contrato.UsuarioCreacionId;

                    if (ObtenerContrato(contrato.Folio, contrato.Id) == null)
                    {

                        if (contrato.Id != 0)
                        {
                            var contratoDetalleMaquina = _context.tblAF_DxP_ContratoMaquinasDetalle.Where(c => c.ContratoDetalle.ContratoId == contrato.Id);
                            _context.tblAF_DxP_ContratoMaquinasDetalle.RemoveRange(contratoDetalleMaquina);

                            var contratoDetalle = _context.tblAF_DxP_ContratosDetalle.Where(c => c.ContratoId == contrato.Id);
                            _context.tblAF_DxP_ContratosDetalle.RemoveRange(contratoDetalle);

                            var contratoMaquina = _context.tblAF_DxP_ContratoMaquinas.Where(c => c.ContratoId == contrato.Id);
                            _context.tblAF_DxP_ContratoMaquinas.RemoveRange(contratoMaquina);
                            _context.SaveChanges();

                            var updateContrato = _context.tblAF_DxP_Contratos.FirstOrDefault(f => f.Id == contrato.Id);

                            updateContrato.AmortizacionCapital = contrato.AmortizacionCapital;
                            updateContrato.Credito = contrato.Credito;
                            updateContrato.cta = contrato.cta;
                            updateContrato.Descripcion = contrato.Descripcion;
                            updateContrato.digito = contrato.digito;
                            updateContrato.Domiciliado = contrato.Domiciliado;
                            updateContrato.fechaFirma = contrato.fechaFirma;
                            updateContrato.FechaInicio = contrato.FechaInicio;
                            updateContrato.FechaModificacion = DateTime.Now;
                            updateContrato.UsuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                            updateContrato.FechaVencimiento = contrato.FechaVencimiento;
                            updateContrato.FechaVencimientoTipoId = contrato.FechaVencimientoTipoId;
                            updateContrato.Folio = contrato.Folio;
                            updateContrato.InstitucionId = contrato.InstitucionId;
                            updateContrato.InteresMoratorio = contrato.InteresMoratorio;
                            updateContrato.IVA = contrato.IVA;
                            updateContrato.monedaContrato = contrato.monedaContrato;
                            updateContrato.montoOpcioncompra = contrato.montoOpcioncompra;
                            updateContrato.PagoInterino = contrato.PagoInterino;
                            updateContrato.PagoInterino2 = contrato.PagoInterino2;
                            updateContrato.DepGarantia = contrato.DepGarantia;
                            updateContrato.nombreCorto = contrato.nombreCorto;
                            updateContrato.penaConvencional = contrato.penaConvencional;
                            updateContrato.Plazo = contrato.Plazo;
                            updateContrato.rfc = contrato.rfc;
                            updateContrato.scta = contrato.scta;
                            updateContrato.sscta = contrato.sscta;
                            updateContrato.TasaInteres = contrato.TasaInteres;
                            updateContrato.Terminado = contrato.Terminado;
                            updateContrato.TipoCambio = contrato.TipoCambio;
                            updateContrato.TipoFechaVencimiento = contrato.TipoFechaVencimiento;
                            updateContrato.tasaFija = contrato.tasaFija;
                            updateContrato.aplicaInteres = contrato.aplicaInteres;
                            updateContrato.aplicaContratoInteres = contrato.aplicaContratoInteres;
                            updateContrato.empresa = contrato.empresa;
                            updateContrato.arrendamientoPuro = contrato.arrendamientoPuro;
                            updateContrato.FileContrato = contrato.FileContrato;
                            updateContrato.fechaFirma = contrato.fechaFirma;
                            updateContrato.ctaIA = contrato.ctaIA;
                            updateContrato.sctaIA = contrato.sctaIA;
                            updateContrato.ssctaIA = contrato.ssctaIA;
                            updateContrato.digitoIA = contrato.digitoIA;

                            _context.SaveChanges();

                            contrato.Institucion = _context.tblAF_DxP_Instituciones.First(f => f.Id == contrato.InstitucionId);
                        }
                        else
                        {
                            _context.tblAF_DxP_Contratos.Add(contrato);
                            _context.SaveChanges();

                            contrato.Institucion = _context.tblAF_DxP_Instituciones.First(f => f.Id == contrato.InstitucionId);

                            using (var _ctxArre = new MainContext(EmpresaEnum.Arrendadora))
                            {
                                using (var _traArre = _ctxArre.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        var stringSeparador = new string[] { "\\" };
                                        foreach (var item in listaEconomicos)
                                        {
                                            var contratoInventarioMaquinaria = new tblM_DocumentosMaquinaria();
                                            contratoInventarioMaquinaria.economicoID = item.MaquinaId;
                                            contratoInventarioMaquinaria.nombreRuta = contrato.FileContrato;
                                            contratoInventarioMaquinaria.nombreArchivo = contrato.FileContrato.Split(stringSeparador, StringSplitOptions.None).Last();
                                            contratoInventarioMaquinaria.tipoArchivo = 8;
                                            contratoInventarioMaquinaria.fechaCarga = DateTime.Now;
                                            contratoInventarioMaquinaria.usuarioSubeArchivo = vSesiones.sesionUsuarioDTO.id;

                                            _ctxArre.tblM_DocumentosMaquinaria.Add(contratoInventarioMaquinaria);
                                            _ctxArre.SaveChanges();
                                        }
                                        _traArre.Commit();
                                    }
                                    catch (Exception ex)
                                    {
                                        _traArre.Rollback();
                                        throw new Exception("Error al guardar el archivo en el inventario de maquinaria");
                                    }
                                }
                            }
                            
                        }
                        var detalles = new List<tblAF_DxP_ContratoDetalle>();

                        if (contrato.Institucion.Nombre == "ENGENCAP")
                        {
                            var pagoInterino = new tblAF_DxP_ContratoDetalle();
                            var depGarantia = new tblAF_DxP_ContratoDetalle();

                            if (contrato.PagoInterino2.HasValue)
                            {
                                pagoInterino.ContratoId = contrato.Id;
                                depGarantia.ContratoId = contrato.Id;
                                pagoInterino.Parcialidad = 1;
                                depGarantia.Parcialidad = 2;
                                pagoInterino.Intereses = 0;
                                depGarantia.Intereses = 0;
                                pagoInterino.AmortizacionCapital = contrato.PagoInterino.Value;
                                depGarantia.AmortizacionCapital = contrato.PagoInterino2.Value + contrato.AmortizacionCapital;
                                pagoInterino.IvaSCapital = contrato.aplicaInteres ? Math.Round(pagoInterino.AmortizacionCapital * contrato.IVA, 2, MidpointRounding.ToEven) : 0; ;
                                depGarantia.IvaSCapital = contrato.aplicaInteres ? Math.Round(contrato.PagoInterino2.Value * contrato.IVA, 2, MidpointRounding.ToEven) : 0; ;
                                pagoInterino.IvaIntereses = 0;
                                depGarantia.IvaIntereses = 0;
                                pagoInterino.Importe = contrato.PagoInterino.Value + pagoInterino.IvaSCapital;
                                depGarantia.Importe = contrato.PagoInterino2.Value + contrato.AmortizacionCapital + depGarantia.IvaSCapital;
                                pagoInterino.Saldo = contrato.Credito - contrato.PagoInterino.Value;
                                depGarantia.Saldo = pagoInterino.Saldo - depGarantia.AmortizacionCapital;
                                pagoInterino.Pagado = false;
                                depGarantia.Pagado = false;
                                pagoInterino.FechaPago = null;
                                depGarantia.FechaPago = null;
                                pagoInterino.GeneroInteresMoratorio = false;
                                depGarantia.GeneroInteresMoratorio = false;
                                pagoInterino.Estatus = true;
                                depGarantia.Estatus = true;

                                switch (contrato.FechaVencimientoTipoId)
                                {
                                    case (int)DiasVencimientoEnum.UltimoDiaMes:
                                        {
                                            pagoInterino.FechaVencimiento = new DateTime(contrato.FechaInicio.Year, contrato.FechaInicio.Month, DateTime.DaysInMonth(contrato.FechaInicio.Year, contrato.FechaInicio.Month));
                                            pagoInterino.FechaOriginal = pagoInterino.FechaVencimiento;

                                            var fechaActual = pagoInterino.FechaVencimiento;
                                            var fechaMasMes = fechaActual.AddMonths(1);
                                            var ultimoDiaMes = DateTime.DaysInMonth(fechaMasMes.Year, fechaMasMes.Month);
                                            depGarantia.FechaVencimiento = new DateTime(fechaMasMes.Year, fechaMasMes.Month, ultimoDiaMes);
                                            depGarantia.FechaOriginal = depGarantia.FechaVencimiento;
                                        }
                                        break;
                                    case (int)DiasVencimientoEnum.Dia15:
                                        {
                                            var fechaActual = contrato.FechaInicio;
                                            pagoInterino.FechaVencimiento = new DateTime(fechaActual.Year, fechaActual.Month, 15);
                                            pagoInterino.FechaOriginal = pagoInterino.FechaVencimiento;

                                            fechaActual = pagoInterino.FechaVencimiento;
                                            var fechaMasMes = fechaActual.AddMonths(1);
                                            depGarantia.FechaVencimiento = new DateTime(fechaMasMes.Year, fechaMasMes.Month, 15);
                                            depGarantia.FechaOriginal = depGarantia.FechaVencimiento;
                                        }
                                        break;
                                    case (int)DiasVencimientoEnum.Seleccion:
                                        {
                                            var fechaActual = contrato.FechaInicio;
                                            if (Math.Abs(contrato.FechaInicio.Day - contrato.FechaVencimiento.Value.Day) < 14)
                                            {
                                                fechaActual = contrato.FechaInicio;//.AddMonths(1);
                                            }
                                            try
                                            {
                                                pagoInterino.FechaVencimiento = new DateTime(fechaActual.Year, fechaActual.Month, contrato.FechaVencimiento.Value.Day);
                                                pagoInterino.FechaOriginal = pagoInterino.FechaVencimiento;
                                            }
                                            catch (Exception ex)
                                            {
                                                var ultimoDiasMes = DateTime.DaysInMonth(fechaActual.Year, fechaActual.Month);
                                                pagoInterino.FechaVencimiento = new DateTime(fechaActual.Year, fechaActual.Month, ultimoDiasMes);
                                                pagoInterino.FechaOriginal = pagoInterino.FechaVencimiento;
                                            }

                                            fechaActual = pagoInterino.FechaVencimiento;
                                            var fechaMasMes = fechaActual.AddMonths(1);
                                            try
                                            {
                                                depGarantia.FechaVencimiento = new DateTime(fechaMasMes.Year, fechaMasMes.Month, contrato.FechaVencimiento.Value.Day);
                                                depGarantia.FechaOriginal = depGarantia.FechaVencimiento;
                                            }
                                            catch (Exception ex)
                                            {
                                                var ultimoDiasMes = DateTime.DaysInMonth(fechaMasMes.Year, fechaMasMes.Month);
                                                depGarantia.FechaVencimiento = new DateTime(fechaMasMes.Year, fechaMasMes.Month, ultimoDiasMes);
                                                depGarantia.FechaOriginal = depGarantia.FechaVencimiento;
                                            }
                                        }
                                        break;
                                }

                                detalles.Add(pagoInterino);
                                detalles.Add(depGarantia);
                            }
                            else
                            {
                                pagoInterino.ContratoId = contrato.Id;
                                pagoInterino.Parcialidad = 1;
                                pagoInterino.Intereses = 0;
                                pagoInterino.AmortizacionCapital = contrato.PagoInterino.Value + contrato.AmortizacionCapital;
                                pagoInterino.IvaSCapital = contrato.aplicaInteres ? Math.Round(pagoInterino.AmortizacionCapital * contrato.IVA, 2, MidpointRounding.ToEven) : 0;
                                pagoInterino.IvaIntereses = 0;
                                pagoInterino.Importe = pagoInterino.AmortizacionCapital + pagoInterino.IvaSCapital;
                                pagoInterino.Saldo = contrato.Credito - pagoInterino.AmortizacionCapital;
                                pagoInterino.Pagado = false;
                                pagoInterino.FechaPago = null;
                                pagoInterino.GeneroInteresMoratorio = false;
                                pagoInterino.Estatus = true;

                                switch (contrato.FechaVencimientoTipoId)
                                {
                                    case (int)DiasVencimientoEnum.UltimoDiaMes:
                                        {
                                            pagoInterino.FechaVencimiento = new DateTime(contrato.FechaInicio.Year, contrato.FechaInicio.Month, DateTime.DaysInMonth(contrato.FechaInicio.Year, contrato.FechaInicio.Month));
                                            pagoInterino.FechaOriginal = pagoInterino.FechaVencimiento;

                                            var fechaActual = pagoInterino.FechaVencimiento;
                                            var fechaMasMes = fechaActual.AddMonths(1);
                                            var ultimoDiaMes = DateTime.DaysInMonth(fechaMasMes.Year, fechaMasMes.Month);
                                        }
                                        break;
                                    case (int)DiasVencimientoEnum.Dia15:
                                        {
                                            var fechaActual = contrato.FechaInicio;
                                            pagoInterino.FechaVencimiento = new DateTime(fechaActual.Year, fechaActual.Month, 15);
                                            pagoInterino.FechaOriginal = pagoInterino.FechaVencimiento;

                                            fechaActual = pagoInterino.FechaVencimiento;
                                            var fechaMasMes = fechaActual.AddMonths(1);
                                        }
                                        break;
                                    case (int)DiasVencimientoEnum.Seleccion:
                                        {
                                            var fechaActual = contrato.FechaInicio;
                                            if (Math.Abs(contrato.FechaInicio.Day - contrato.FechaVencimiento.Value.Day) < 14)
                                            {
                                                fechaActual = contrato.FechaInicio;//.AddMonths(1);
                                            }
                                            try
                                            {
                                                pagoInterino.FechaVencimiento = new DateTime(fechaActual.Year, fechaActual.Month, contrato.FechaVencimiento.Value.Day);
                                                pagoInterino.FechaOriginal = pagoInterino.FechaVencimiento;
                                            }
                                            catch (Exception ex)
                                            {
                                                var ultimoDiasMes = DateTime.DaysInMonth(fechaActual.Year, fechaActual.Month);
                                                pagoInterino.FechaVencimiento = new DateTime(fechaActual.Year, fechaActual.Month, ultimoDiasMes);
                                                pagoInterino.FechaOriginal = pagoInterino.FechaVencimiento;
                                            }
                                        }
                                        break;
                                }

                                detalles.Add(pagoInterino);
                            }
                        }

                        for (int periodo = contrato.Institucion.Nombre == "ENGENCAP" && contrato.PagoInterino2.HasValue ? 2 : 0; periodo < contrato.Plazo; periodo++)
                        {

                            if (contrato.Institucion.Nombre == "ENGENCAP" && !contrato.PagoInterino2.HasValue && periodo == 0)
                            {
                                periodo = 1;
                            }

                            var detalle = new tblAF_DxP_ContratoDetalle();
                            detalle.ContratoId = contrato.Id;
                            detalle.Parcialidad = periodo + 1;

                            if (!contrato.arrendamientoPuro)
                            {
                                if (contrato.aplicaContratoInteres.HasValue && !contrato.aplicaContratoInteres.Value)
                                {
                                    detalle.Intereses = 0;

                                }
                                else
                                {
                                    if (contrato.Institucion.Nombre == "TOYOTA S")
                                    {
                                        detalle.Intereses = periodo == 0 ? ((contrato.AmortizacionCapital * (contrato.TasaInteres / 100))) : ((detalles[periodo - 1].Saldo - detalles[periodo - 1].AmortizacionCapital) * (detalles[periodo - 1].Intereses)) / detalles[periodo - 1].Saldo;
                                    }
                                    else if (contrato.Institucion.Nombre == "TOYOTA")
                                    {
                                        detalle.Intereses = periodo == 0 ? ((contrato.Credito * (contrato.TasaInteres / 100)) / 360) * 30.4167m : ((detalles[periodo - 1].Saldo * (contrato.TasaInteres / 100)) / 360) * 30.4167m;
                                    }
                                    else
                                    {
                                        detalle.Intereses = Math.Round(periodo == 0 ? ((contrato.Credito * (contrato.TasaInteres / 100)) / (360)) * 30 : (detalles[periodo - 1].Saldo * (contrato.TasaInteres / 100)) / (360) * 30, 2, MidpointRounding.ToEven);
                                    }

                                }
                            }
                            else
                            {
                                detalle.Intereses = 0;
                            }

                            if (contrato.tasaFija)
                            {
                                detalle.AmortizacionCapital = Math.Round(contrato.AmortizacionCapital, 2, MidpointRounding.ToEven);
                            }
                            else
                            {
                                if (!contrato.arrendamientoPuro)
                                {
                                    if (contrato.Institucion.Nombre == "TOYOTA S")
                                    {
                                        detalle.AmortizacionCapital = periodo == 0 ? contrato.AmortizacionCapital : (detalles[periodo - 1].Importe - detalle.Intereses - (contrato.IVA * detalle.Intereses));
                                    }
                                    else if (contrato.Institucion.Nombre == "TOYOTA")
                                    {
                                        detalle.AmortizacionCapital = periodo == 0 ? contrato.AmortizacionCapital : detalles[periodo - 1].Importe - detalle.Intereses - (contrato.IVA * detalle.Intereses);
                                    }
                                    else
                                    {
                                        detalle.AmortizacionCapital = Math.Round(periodo == 0 ? contrato.AmortizacionCapital : detalles[periodo - 1].AmortizacionCapital + (detalles[periodo - 1].Intereses - detalle.Intereses), 2, MidpointRounding.ToEven);
                                    }
                                }
                                else
                                {
                                    detalle.AmortizacionCapital = Math.Round(contrato.AmortizacionCapital, 2, MidpointRounding.ToEven);
                                }
                            }

                            if (contrato.Institucion.Nombre != "TOYOTA" && contrato.Institucion.Nombre != "TOYOTA S")
                            {
                                detalle.IvaSCapital = contrato.aplicaInteres ? detalle.AmortizacionCapital * contrato.IVA : 0;
                            }
                            detalle.IvaIntereses = contrato.aplicaInteres ? Math.Round(detalle.Intereses * contrato.IVA, 2, MidpointRounding.ToEven) : 0;

                            if (contrato.arrendamientoPuro)
                            {
                                detalle.IvaIntereses = 0;
                            }
                            else
                            {
                                if (contrato.aplicaInteres)
                                {
                                    detalle.IvaIntereses = Math.Round(detalle.Intereses * contrato.IVA, 2, MidpointRounding.ToEven);

                                }
                                else
                                {
                                    detalle.IvaIntereses = 0;
                                }
                            }

                            detalle.Importe = Math.Round(detalle.AmortizacionCapital + detalle.IvaSCapital + detalle.Intereses + (contrato.arrendamientoPuro ? 0 : detalle.IvaIntereses), 2, MidpointRounding.ToEven);

                            if (contrato.Institucion.Nombre == "TOYOTA S")
                            {
                                detalle.Saldo = periodo == 0 ? contrato.Credito : detalles[periodo - 1].Saldo - detalles[periodo - 1].AmortizacionCapital;
                            }
                            else
                            {
                                detalle.Saldo = Math.Round(periodo == 0 ? contrato.Credito - detalle.AmortizacionCapital : detalles[periodo - 1].Saldo - detalle.AmortizacionCapital, 2, MidpointRounding.ToEven);
                            }
                            detalle.Pagado = false;
                            detalle.FechaPago = null;
                            detalle.GeneroInteresMoratorio = false;
                            detalle.Estatus = true;

                            if (detalle.Parcialidad == contrato.Plazo && detalle.Saldo != 0)
                            {
                                if (contrato.Institucion.Nombre == "ENGENCAP")
                                {
                                    detalle.AmortizacionCapital = contrato.Credito - detalles.Sum(s => s.AmortizacionCapital);
                                }
                                else
                                {
                                    if (contrato.Institucion.Nombre != "TOYOTA S" && contrato.Institucion.Nombre != "TOYOTA")
                                    {
                                        detalle.AmortizacionCapital = Math.Round(detalle.AmortizacionCapital + detalle.Saldo, 2, MidpointRounding.ToEven);
                                        detalle.Saldo = Math.Round(detalle.Saldo - detalle.Saldo, 2, MidpointRounding.ToEven);
                                    }
                                }
                            }

                            switch (contrato.FechaVencimientoTipoId)
                            {
                                case (int)DiasVencimientoEnum.UltimoDiaMes:
                                    if (periodo == 0)
                                    {
                                        detalle.FechaVencimiento = new DateTime(contrato.FechaInicio.Year, contrato.FechaInicio.Month, DateTime.DaysInMonth(contrato.FechaInicio.Year, contrato.FechaInicio.Month));
                                        detalle.FechaOriginal = detalle.FechaVencimiento;
                                    }
                                    else
                                    {
                                        var fechaActual = detalles[periodo - 1].FechaVencimiento;
                                        var fechaMasMes = fechaActual.AddMonths(1);
                                        var ultimoDiaMes = DateTime.DaysInMonth(fechaMasMes.Year, fechaMasMes.Month);
                                        detalle.FechaVencimiento = new DateTime(fechaMasMes.Year, fechaMasMes.Month, ultimoDiaMes);
                                        detalle.FechaOriginal = detalle.FechaVencimiento;
                                    }
                                    break;
                                case (int)DiasVencimientoEnum.Dia15:
                                    if (periodo == 0)
                                    {
                                        var fechaActual = contrato.FechaInicio;
                                        detalle.FechaVencimiento = new DateTime(fechaActual.Year, fechaActual.Month, 15);
                                        detalle.FechaOriginal = detalle.FechaVencimiento;
                                    }
                                    else
                                    {
                                        var fechaActual = detalles[periodo - 1].FechaVencimiento;
                                        var fechaMasMes = fechaActual.AddMonths(1);
                                        detalle.FechaVencimiento = new DateTime(fechaMasMes.Year, fechaMasMes.Month, 15);
                                        detalle.FechaOriginal = detalle.FechaVencimiento;
                                    }
                                    break;
                                case (int)DiasVencimientoEnum.Seleccion:
                                    if (periodo == 0)
                                    {
                                        var fechaActual = contrato.FechaInicio;
                                        if (Math.Abs(contrato.FechaInicio.Day - contrato.FechaVencimiento.Value.Day) < 14)
                                        {
                                            fechaActual = contrato.FechaInicio;//.AddMonths(1);
                                        }
                                        try
                                        {
                                            detalle.FechaVencimiento = new DateTime(fechaActual.Year, fechaActual.Month, contrato.FechaVencimiento.Value.Day);
                                            detalle.FechaOriginal = detalle.FechaVencimiento;
                                        }
                                        catch (Exception ex)
                                        {
                                            var ultimoDiasMes = DateTime.DaysInMonth(fechaActual.Year, fechaActual.Month);
                                            detalle.FechaVencimiento = new DateTime(fechaActual.Year, fechaActual.Month, ultimoDiasMes);
                                            detalle.FechaOriginal = detalle.FechaVencimiento;
                                        }
                                    }
                                    else
                                    {
                                        var fechaActual = detalles[periodo - 1].FechaVencimiento;
                                        var fechaMasMes = fechaActual.AddMonths(1);
                                        try
                                        {
                                            detalle.FechaVencimiento = new DateTime(fechaMasMes.Year, fechaMasMes.Month, contrato.FechaVencimiento.Value.Day);
                                            detalle.FechaOriginal = detalle.FechaVencimiento;
                                        }
                                        catch (Exception ex)
                                        {
                                            var ultimoDiasMes = DateTime.DaysInMonth(fechaMasMes.Year, fechaMasMes.Month);
                                            detalle.FechaVencimiento = new DateTime(fechaMasMes.Year, fechaMasMes.Month, ultimoDiasMes);
                                            detalle.FechaOriginal = detalle.FechaVencimiento;
                                        }
                                    }
                                    break;
                            }

                            detalles.Add(detalle);
                        }

                        _context.tblAF_DxP_ContratosDetalle.AddRange(detalles);
                        _context.SaveChanges();

                        foreach (var maquina in listaEconomicos)
                        {

                            var maquinasPorContrato = _context.tblAF_DxP_ContratoMaquinas.Where(x => x.ContratoId == maquina.ContratoId && x.Estatus).ToList();

                            var creditoDisponible = maquinasPorContrato.Count > 0 ? contrato.Credito - maquinasPorContrato.Sum(s => s.Credito) : contrato.Credito;

                            if (creditoDisponible >= maquina.Credito)
                            {
                                var contratoMaquina = new tblAF_DxP_ContratoMaquina();
                                contratoMaquina.ContratoId = contrato.Id;
                                contratoMaquina.MaquinaId = maquina.MaquinaId;
                                contratoMaquina.Credito = maquina.Credito;
                                contratoMaquina.Estatus = true;
                                contratoMaquina.FechaCreacion = DateTime.Now;
                                contratoMaquina.UsuarioCreacionId = vSesiones.sesionUsuarioDTO.id;
                                contratoMaquina.FechaModificacion = contratoMaquina.FechaCreacion;
                                contratoMaquina.UsuarioModificacionId = contratoMaquina.UsuarioCreacionId;
                                contratoMaquina.porcentaje = maquina.porcentaje;

                                _context.tblAF_DxP_ContratoMaquinas.Add(contratoMaquina);
                                _context.SaveChanges();

                                var maquinaDetalles = new List<tblAF_DxP_ContratoMaquinaDetalle>();

                                var porcentaje = maquina.porcentaje;

                                foreach (var detalle in detalles)
                                {
                                    var maquinaDetalle = new tblAF_DxP_ContratoMaquinaDetalle();
                                    maquinaDetalle.ContratoMaquinaId = contratoMaquina.Id;
                                    maquinaDetalle.ContratoDetalleId = detalle.Id;
                                    maquinaDetalle.Parcialidad = detalle.Parcialidad;
                                    maquinaDetalle.Intereses = (detalle.Intereses * porcentaje) / 100;// detalle.Parcialidad == 1 ? ((contratoMaquina.Credito * (contrato.TasaInteres / 100)) / (360)) * 30 : (maquinaDetalles[detalle.Parcialidad - 2].Saldo * (contrato.TasaInteres / 100)) / (360) * 30;
                                    maquinaDetalle.AmortizacionCapital = (detalle.AmortizacionCapital * porcentaje) / 100;// detalle.Parcialidad == 1 ? (contrato.AmortizacionCapital * porcentaje) / 100 : maquinaDetalles[detalle.Parcialidad - 2].AmortizacionCapital + (maquinaDetalles[detalle.Parcialidad - 2].Intereses - maquinaDetalle.Intereses);
                                    maquinaDetalle.IvaSCapital = (detalle.IvaSCapital * porcentaje) / 100; //maquinaDetalle.AmortizacionCapital * contrato.IVA;
                                    maquinaDetalle.IvaIntereses = (detalle.IvaIntereses * porcentaje) / 100;//maquinaDetalle.Intereses * contrato.IVA;
                                    maquinaDetalle.Importe = (detalle.Importe * porcentaje) / 100;//maquinaDetalle.AmortizacionCapital + maquinaDetalle.IvaSCapital + maquinaDetalle.Intereses + maquinaDetalle.IvaIntereses;
                                    maquinaDetalle.Saldo = detalle.Parcialidad == 1 ? contratoMaquina.Credito - maquinaDetalle.AmortizacionCapital : maquinaDetalles[detalle.Parcialidad - 2].Saldo - maquinaDetalle.AmortizacionCapital;
                                    maquinaDetalle.FechaVencimiento = detalle.FechaVencimiento;
                                    maquinaDetalle.Pagado = detalle.Pagado;
                                    maquinaDetalle.FechaPago = detalle.FechaPago;
                                    maquinaDetalle.GeneroInteresMoratorio = detalle.GeneroInteresMoratorio;// * porcentaje) / 100;
                                    maquinaDetalle.Estatus = true;

                                    maquinaDetalles.Add(maquinaDetalle);
                                }
                                _context.tblAF_DxP_ContratoMaquinasDetalle.AddRange(maquinaDetalles);
                                _context.SaveChanges();
                            }
                        }

                        transaction.Commit();

                        r.Success = true;
                        r.Message = "Contrato registrado";
                    }
                    else
                    {

                        r.Message = "Ya existe un contrato con el folio: " + contrato.Folio;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    r.Message += ex.Message;
                }
            }
            return r;
        }
        public Respuesta GuardarDeudas(List<tblAF_DxP_Deuda> objDeudas, tblC_sc_polizas poliza, List<tblC_sc_movpol> movPolList)
        {
            var r = new Respuesta();

            //Jessica Galdean, Lili Lavandera
            List<int> lstAdminID = new List<int>() { 3978, 6587, 6278, 79888, 1073 };

            //tblP_Usuario objUsr = _context.tblP_Usuario.Where(e => e.cveEmpleado == vSesiones.sesionUsuarioDTO.id.ToString()).FirstOrDefault();


            if (vSesiones.sesionUsuarioDTO.idPerfil != (int)PerfilUsuarioEnum.ADMINISTRADOR)
            {
                //contratosDTO[0].esAdmin = true;
                if (!lstAdminID.Contains(vSesiones.sesionUsuarioDTO.id))
                {
                    r.Message += "Usuario no permitido";
                    return r;
                }
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var contrato = objDeudas.Select(x => x.ContratoId).FirstOrDefault();

                    objDeudas.ForEach(f =>
                    {
                        f.fechaCreacion = DateTime.Now;
                        f.poliza = poliza.poliza;
                        f.year = poliza.year;
                        f.mes = poliza.mes;
                    });

                    var contratoDet = _context.tblAF_DxP_Contratos.First(c => c.Id == contrato);
                    //var maquinasContrato = _context.tblAF_DxP_ContratoMaquinas.Where(c => c.ContratoId == contrato).Select(c => c.Maquina.centro_costos).FirstOrDefault().Split('-');
                    //string area = maquinasContrato[0];
                    //string cuenta = maquinasContrato[1];

                    //string centro_costos = _context.tblP_CC.Where(c => c.areaCuenta == area + "-" + cuenta).FirstOrDefault().cc;
                    List<tblC_sc_movpol> addMovpol = new List<tblC_sc_movpol>();
                    int linea = 1;

                    poliza.fec_hora_movto = DateTime.Now;
                    poliza.fecha_hora_crea = DateTime.Now;
                    poliza.usuario_crea = vSesiones.sesionUsuarioDTO.id.ToString();
                    poliza.usuario_movto = vSesiones.sesionUsuarioDTO.id;
                    poliza.error = "";
                    poliza.status_carga_pol = "";
                    poliza.status_lock = "";

                    _context.tblAF_DxP_Deuda.AddRange(objDeudas);
                    _context.SaveChanges();
                    _context.tblC_sc_polizas.Add(poliza);

                    var _referenciaGeneral = CrearReferencia(poliza.fechapol, contratoDet);

                    foreach (var item in movPolList)
                    {
                        if (_context.tblAF_DxP_RelInstitucionCta.Any(a => a.activo && a.institucionID == contratoDet.InstitucionId && a.cta == item.cta))
                        {
                            _referenciaGeneral = ObtenerReferenciaDxP(poliza.fechapol, item.cta, Convert.ToInt32(_referenciaGeneral));
                            break;
                        }
                    }

                    foreach (var item in movPolList)
                    {
                        item.num_emp = vSesiones.sesionUsuarioDTO.id;
                        item.poliza = poliza.poliza;
                        item.referencia = _referenciaGeneral;
                        _context.tblC_sc_movpol.Add(item);
                    }
                    _context.SaveChanges();


                    using (var con = checkConexionProductivo())
                    {
                        using (var trans = con.BeginTransaction())
                        {
                            try
                            {
                                var count = 0;

                                var insertPoliza = "INSERT INTO sc_polizas (year ,mes ,poliza ,tp ,fechapol ,cargos ,abonos ,generada ,status ,status_lock ,fec_hora_movto ,usuario_movto ,fecha_hora_crea ,usuario_crea ,concepto ,error) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
                                using (var cmd = new OdbcCommand(insertPoliza))
                                {
                                    OdbcParameterCollection parameters = cmd.Parameters;
                                    parameters.Clear();

                                    decimal cargos = 0;
                                    decimal abonos = 0;

                                    foreach (var item in movPolList)
                                    {
                                        if (item.tm == 1)
                                        {
                                            cargos += item.monto;
                                        }
                                        if (item.tm == 2)
                                        {
                                            decimal montoT = 0;
                                            if (item.monto > 0)
                                                montoT = item.monto * (-1);
                                            else
                                                montoT = item.monto;
                                            abonos += montoT;
                                        }
                                    }

                                    parameters.Add("@year", OdbcType.Numeric).Value = poliza.year;
                                    parameters.Add("@mes", OdbcType.Numeric).Value = poliza.mes;
                                    parameters.Add("@poliza", OdbcType.Numeric).Value = poliza.poliza;
                                    parameters.Add("@tp", OdbcType.Char).Value = poliza.tp;
                                    parameters.Add("@fechapol", OdbcType.Date).Value = poliza.fechapol;
                                    parameters.Add("@cargos", OdbcType.Numeric).Value = cargos;
                                    parameters.Add("@abonos", OdbcType.Numeric).Value = abonos;
                                    parameters.Add("@generada", OdbcType.Char).Value = poliza.generada;
                                    parameters.Add("@status", OdbcType.Char).Value = poliza.status ?? string.Empty;
                                    parameters.Add("@status_lock", OdbcType.Char).Value = "";
                                    parameters.Add("@fec_hora_movto", OdbcType.DateTime).Value = DateTime.Now;
                                    parameters.Add("@usuario_movto", OdbcType.Char).Value = 'C';
                                    parameters.Add("@fecha_hora_crea", OdbcType.DateTime).Value = DateTime.Now;
                                    parameters.Add("@usuario_crea", OdbcType.Char).Value = '1';
                                    parameters.Add("@concepto", OdbcType.VarChar).Value = poliza.concepto;
                                    parameters.Add("@error", OdbcType.VarChar).Value = string.Empty;
                                    parameters.Add("@status_carga_pol", OdbcType.VarChar).Value = DBNull.Value;

                                    cmd.Connection = trans.Connection;
                                    cmd.Transaction = trans;
                                    count += cmd.ExecuteNonQuery();
                                }



                                foreach (var objMovPol1 in movPolList)
                                {
                                    var insertMovpol = @"INSERT INTO sc_movpol 
                                                                            (year
                                                                            ,mes
                                                                            ,poliza
                                                                            ,tp
                                                                            ,linea
                                                                            ,cta
                                                                            ,scta
                                                                            ,sscta
                                                                            ,digito
                                                                            ,tm
                                                                            ,referencia
                                                                            ,cc
                                                                            ,concepto
                                                                            ,monto
                                                                            ,iclave
                                                                            ,itm
                                                                            ,st_par
                                                                            ,orden_compra
                                                                            ,numpro
                                                                            ,area
                                                                            ,cuenta_oc)
                                                                    VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                                    using (var cmd = new OdbcCommand(insertMovpol))
                                    {
                                        OdbcParameterCollection parameters = cmd.Parameters;
                                        parameters.Clear();
                                        decimal monto = 0;
                                        if (objMovPol1.tm == 1)
                                            monto = objMovPol1.monto;
                                        else
                                        {

                                            if (objMovPol1.monto > 0)
                                                monto = objMovPol1.monto * (-1);
                                            else
                                                monto = objMovPol1.monto;
                                        }

                                        var consultaCC = "";

                                        if (!getCCValid(objMovPol1.cc))
                                        {
                                            transaction.Rollback();
                                            trans.Rollback();
                                            r.Success = false;
                                            r.Message = "No se encontro centro costos";
                                            return r;
                                        }

                                        parameters.Add("@year", OdbcType.Numeric).Value = objMovPol1.year;
                                        parameters.Add("@mes", OdbcType.Numeric).Value = objMovPol1.mes;
                                        parameters.Add("@poliza", OdbcType.Numeric).Value = poliza.poliza;
                                        parameters.Add("@tp", OdbcType.Char).Value = "03";
                                        parameters.Add("@linea", OdbcType.Numeric).Value = objMovPol1.linea;
                                        parameters.Add("@cta", OdbcType.Numeric).Value = objMovPol1.cta;
                                        parameters.Add("@scta", OdbcType.Numeric).Value = objMovPol1.scta;
                                        parameters.Add("@sscta", OdbcType.Numeric).Value = objMovPol1.sscta;
                                        parameters.Add("@digito", OdbcType.Numeric).Value = objMovPol1.digito;
                                        parameters.Add("@tm", OdbcType.Numeric).Value = objMovPol1.tm;
                                        parameters.Add("@referencia", OdbcType.Char).Value = objMovPol1.referencia;
                                        parameters.Add("@cc", OdbcType.Char).Value = objMovPol1.cc;
                                        parameters.Add("@concepto", OdbcType.Char).Value = objMovPol1.concepto;
                                        parameters.Add("@monto", OdbcType.Numeric).Value = monto;
                                        parameters.Add("@iclave", OdbcType.Numeric).Value = 0;
                                        parameters.Add("@itm", OdbcType.Numeric).Value = objMovPol1.itm;
                                        parameters.Add("@st_par", OdbcType.Char).Value = string.Empty;
                                        parameters.Add("@orden_compra", OdbcType.Numeric).Value = 0;
                                        parameters.Add("@numpro", OdbcType.Numeric).Value = objMovPol1.numpro;
                                        parameters.Add("@area", OdbcType.Numeric).Value = objMovPol1.area;
                                        parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = objMovPol1.cuenta_oc;
                                        cmd.Connection = trans.Connection;
                                        cmd.Transaction = trans;
                                        count += cmd.ExecuteNonQuery();
                                    }
                                }

                                _polizaEkFS.SetContext(con);
                                _polizaEkFS.SetTransaccion(trans);

                                var infoAConciliar = GenerarInfoConcilia(movPolList, poliza, contratoDet);

                                foreach (var concilia in infoAConciliar)
                                {
                                    _polizaEkFS.GuardarParaConciliar(concilia);
                                }

                                transaction.Commit();
                                trans.Commit();
                            }
                            catch (Exception e)
                            {
                                transaction.Rollback();
                                trans.Rollback();
                                r.Message += e.Message;
                            }
                        }
                    }
                    r.Success = true;
                    r.Message = "Ok";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    r.Message += ex.Message;
                }
            }
            return r;
        }

        private string CrearReferencia(DateTime fecha, tblAF_DxP_Contrato contrato)
        {
            string numReferencia = "";
            var numeros = string.Empty;

            for (int i = contrato.Folio.Length - 1; i > -1; i--)
            {
                if (Char.IsDigit(contrato.Folio[i]))
                {
                    numeros += contrato.Folio[i];
                }
            }

            if (numeros.Length > 0)
            {
                var numeroTemporal = "";
                for (int i = numeros.Length - 1; i > -1; i--)
                {
                    numeroTemporal += numeros[i];
                }

                numReferencia = numeroTemporal.Substring(numeroTemporal.Length > 6 ? numeroTemporal.Length - 6 : 0, numeroTemporal.Length < 6 ? numeroTemporal.Length : 6);
            }
            else
            {
                numReferencia = DateTime.Now.ToString("ffffff");
            }

            return numReferencia;
        }

        private string ObtenerReferenciaDxP(DateTime fecha, int ctaBanco, int numeroReferencia)
        {
            do
            {

            } while (!_polizaEkFS.ReferenciaDisponible(fecha, ctaBanco, numeroReferencia++));

            string referencia = (numeroReferencia - 1).ToString();

            return referencia;
        }

        private List<Core.DTO.Enkontrol.Tablas.Poliza.sb_edo_cta_chequeraDTO> GenerarInfoConcilia(List<tblC_sc_movpol> movimientos, tblC_sc_polizas poliza, tblAF_DxP_Contrato contrato)
        {
            var infoAConciliar = new List<Core.DTO.Enkontrol.Tablas.Poliza.sb_edo_cta_chequeraDTO>();

            var tipoCambio = getTipoCambioDLLs(poliza.fechapol);

            foreach (var mov in movimientos)
            {
                var cuentaInstitucion = _context.tblAF_DxP_RelInstitucionCta
                    .FirstOrDefault(f =>
                        f.institucionID == contrato.InstitucionId &&
                        f.activo &&
                        f.cta == mov.cta &&
                        f.scta == mov.scta &&
                        f.sscta == mov.sscta
                    );

                if (cuentaInstitucion != null)
                {
                    var cuentaBanco = _bancoFS.GetBanco(mov.cta, mov.scta, mov.sscta);

                    if (cuentaBanco != null)
                    {
                        var concilia = new Core.DTO.Enkontrol.Tablas.Poliza.sb_edo_cta_chequeraDTO();

                        concilia.cuenta = cuentaBanco.cuenta;
                        concilia.fecha_mov = poliza.fechapol;
                        concilia.tm = mov.itm;
                        concilia.numero = Convert.ToInt32(mov.referencia);
                        concilia.cc = mov.cc;
                        concilia.descripcion = mov.concepto;
                        concilia.monto = mov.monto;
                        if (contrato.monedaContrato == 1)
                        {
                            concilia.tc = 1M;
                        }
                        else
                        {
                            concilia.tc = !cuentaInstitucion.complementaria ? tipoCambio.tipo_cambio : 1M;
                        }
                        concilia.origen_mov = "C";
                        concilia.generada = "C";
                        concilia.iyear = mov.year;
                        concilia.imes = mov.mes;
                        concilia.ipoliza = poliza.poliza;
                        concilia.itp = mov.tp;
                        concilia.ilinea = mov.linea;
                        concilia.banco = cuentaBanco.banco;

                        infoAConciliar.Add(concilia);
                    }
                    else
                    {
                        throw new Exception("Error al obtener el número de cuenta banco");
                    }
                }
            }

            return infoAConciliar;
        }

        public bool getCCValid(string term)
        {
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"SELECT  cc  AS Text FROM cc
                            WHERE cc = '" + term + "'";

            var lstCtas = _contextEnkontrol.Select<ComboDTO>(productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, odbc);
            return lstCtas.Count > 0 ? true : false;
        }
        public Respuesta AgregarMaquina(AgregarMaquinaDTO maquina)
        {
            var r = new Respuesta();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var contratoDetalle = _context.tblAF_DxP_ContratosDetalle.Include("Contrato").Where(x => x.Contrato.Id == maquina.ContratoId && x.Contrato.Estatus && x.Estatus).ToList();
                    var contrato = contratoDetalle.First().Contrato;
                    var maquinasPorContrato = _context.tblAF_DxP_ContratoMaquinas.Where(x => x.ContratoId == maquina.ContratoId && x.Estatus).ToList();

                    var creditoDisponible = maquinasPorContrato.Count > 0 ? contrato.Credito - maquinasPorContrato.Sum(s => s.Credito) : contrato.Credito;

                    if (creditoDisponible >= maquina.Credito)
                    {
                        var contratoMaquina = new tblAF_DxP_ContratoMaquina();
                        contratoMaquina.ContratoId = contrato.Id;
                        contratoMaquina.MaquinaId = maquina.MaquinaId;
                        contratoMaquina.Credito = maquina.Credito;
                        contratoMaquina.Estatus = true;
                        contratoMaquina.FechaCreacion = DateTime.Now;
                        contratoMaquina.UsuarioCreacionId = vSesiones.sesionUsuarioDTO.id;
                        contratoMaquina.FechaModificacion = contratoMaquina.FechaCreacion;
                        contratoMaquina.UsuarioModificacionId = contratoMaquina.UsuarioCreacionId;

                        _context.tblAF_DxP_ContratoMaquinas.Add(contratoMaquina);
                        _context.SaveChanges();

                        var maquinaDetalles = new List<tblAF_DxP_ContratoMaquinaDetalle>();

                        var porcentaje = (maquina.Credito * 100) / contrato.Credito;

                        foreach (var detalle in contratoDetalle)
                        {
                            var maquinaDetalle = new tblAF_DxP_ContratoMaquinaDetalle();
                            maquinaDetalle.ContratoMaquinaId = contratoMaquina.Id;
                            maquinaDetalle.ContratoDetalleId = detalle.Id;
                            maquinaDetalle.Parcialidad = detalle.Parcialidad;
                            maquinaDetalle.Intereses = detalle.Parcialidad == 1 ? ((contratoMaquina.Credito * (contrato.TasaInteres / 100)) / (360)) * 30 : (maquinaDetalles[detalle.Parcialidad - 2].Saldo * (contrato.TasaInteres / 100)) / (360) * 30;
                            maquinaDetalle.AmortizacionCapital = detalle.Parcialidad == 1 ? (contrato.AmortizacionCapital * porcentaje) / 100 : maquinaDetalles[detalle.Parcialidad - 2].AmortizacionCapital + (maquinaDetalles[detalle.Parcialidad - 2].Intereses - maquinaDetalle.Intereses);
                            maquinaDetalle.IvaSCapital = maquinaDetalle.AmortizacionCapital * contrato.IVA;
                            maquinaDetalle.IvaIntereses = maquinaDetalle.Intereses * contrato.IVA;
                            maquinaDetalle.Importe = maquinaDetalle.AmortizacionCapital + maquinaDetalle.IvaSCapital + maquinaDetalle.Intereses + maquinaDetalle.IvaIntereses;
                            maquinaDetalle.Saldo = detalle.Parcialidad == 1 ? contratoMaquina.Credito - maquinaDetalle.AmortizacionCapital : maquinaDetalles[detalle.Parcialidad - 2].Saldo - maquinaDetalle.AmortizacionCapital;
                            maquinaDetalle.FechaVencimiento = detalle.FechaVencimiento;
                            maquinaDetalle.Pagado = detalle.Pagado;
                            maquinaDetalle.FechaPago = detalle.FechaPago;
                            maquinaDetalle.GeneroInteresMoratorio = detalle.GeneroInteresMoratorio;
                            maquinaDetalle.Estatus = true;
                            maquinaDetalles.Add(maquinaDetalle);
                        }

                        _context.tblAF_DxP_ContratoMaquinasDetalle.AddRange(maquinaDetalles);
                        _context.SaveChanges();

                        r.Success = true;
                        r.Message = "Ok";

                        transaction.Commit();
                    }
                    else
                    {
                        r.Message = "El crédito disponible para el contrato con folio: " + contrato.Folio + " es de: " + creditoDisponible;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    r.Message += ex.Message;
                }
            }

            return r;
        }
        public Respuesta ObtenerMaquinas(int idContrato)
        {
            var r = new Respuesta();

            try
            {
                var maquinas = _context.tblAF_DxP_ContratoMaquinas.Where(x => x.Estatus && x.ContratoId == idContrato).Select(m => new MaquinasDTO
                {
                    Id = m.Id,
                    ContratoId = m.ContratoId,
                    MaquinaId = m.MaquinaId,
                    NumeroEconomico = m.Maquina.noEconomico,
                    Credito = m.Credito
                }).ToList();

                r.Success = true;
                r.Message = "Ok";
                r.Value = maquinas;
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }
        public Respuesta ObtenerDesgloseGeneral(int idContrato)
        {
            var r = new Respuesta();

            //try
            //{
            int poliza = 0;
            var contrato = _context.tblAF_DxP_Contratos.FirstOrDefault(c => c.Id == idContrato);
            var contratoDisponibles = _context.tblAF_DxP_ContratoMaquinas.Where(m => m.ContratoId == idContrato && m.Estatus).ToList();
            var completo = contrato.Credito == Math.Round(contratoDisponibles.Sum(f => f.Credito), 2) ? true : false;

            var maquinasDeuda = _context.tblAF_DxP_ContratoMaquinas.Where(m => m.ContratoId == idContrato).ToList().Select(res => new
             {
                 ContratoId = res.ContratoId,
                 Descripcion = res.Maquina.noEconomico,
                 Debe = res.Credito,
                 area = getAreaCuenta(res.Maquina.centro_costos, 0),// Split('-')[0],
                 cuenta = getAreaCuenta(res.Maquina.centro_costos, 1)
             }).ToList();
            if (completo)
            {
                var desglose = _context.tblAF_DxP_ContratosDetalle.Where(x => x.Contrato.Estatus && x.Estatus && x.ContratoId == idContrato).Select(m => new DesgloseGeneralDTO
                {
                    Id = m.Id,
                    ContratoId = m.ContratoId,
                    Parcialidad = m.Parcialidad,
                    AmortizacionCapital = m.AmortizacionCapital,
                    IVASCapital = m.IvaSCapital,
                    Interes = m.Intereses,
                    IVAInteres = m.IvaIntereses,
                    Importe = m.Importe,
                    Saldo = m.Saldo,
                    FechaVencimiento = m.FechaVencimiento,
                    Pagado = m.Pagado,
                    FechaPago = m.Pagado ? m.FechaPago : null
                }).ToList();

                var guardarDeuda = true;
                var lstDeudas = (from dedua in _context.tblAF_DxP_Deuda
                                 where dedua.Estatus && dedua.ContratoId == idContrato
                                 select dedua).ToList();
                if (!lstDeudas.Any())
                {
                    var lstMaquina = (from maquina in contratoDisponibles
                                      group maquina by maquina.Maquina.noEconomico).ToList();
                    var primerAnio = contrato.FechaInicio.Year;
                    var tipoCambio = contrato.TipoCambio;
                    var docPagCP = desglose.Where(w => w.FechaVencimiento.Year == primerAnio).Sum(s => s.Importe) * tipoCambio;
                    var docPagLP = (desglose.Sum(s => s.Importe) * tipoCambio) - docPagCP;
                    var intAmortizar = desglose.Sum(s => s.Interes) * tipoCambio;
                    var ivaDif = desglose.Sum(s => s.IVASCapital + s.IVAInteres) * tipoCambio;
                    var prorrateoMaquina = (docPagCP + docPagLP - intAmortizar - ivaDif) / lstMaquina.Count();

                    lstDeudas.Add(new tblAF_DxP_Deuda
                    {
                        ContratoId = idContrato,
                        Descripcion = "DOC X PAG CP",
                        Haber = docPagCP,
                        cc = "200",
                        area = "14",
                        cuenta = "1"
                    });

                    lstDeudas.Add(new tblAF_DxP_Deuda
                    {
                        ContratoId = idContrato,
                        Descripcion = "DOC X PAG LP",
                        Haber = docPagLP,
                        cc = "191",
                        area = "16",
                        cuenta = "2"
                    });

                    foreach (var res in maquinasDeuda)
                    {
                        string acraw = res.area + "-" + res.cuenta;
                        string cc = "";
                        if (!string.IsNullOrEmpty(res.cuenta))
                        {
                            cc = _context.tblP_CC.FirstOrDefault(a => a.areaCuenta == acraw).cc;
                        }
                        else
                        {
                            cc = res.area;
                        }


                        lstDeudas.Add(new tblAF_DxP_Deuda
                        {
                            ContratoId = res.ContratoId,
                            Descripcion = res.Descripcion,
                            Debe = res.Debe * tipoCambio,
                            cc = cc,
                            area = res.area,
                            cuenta = res.cuenta
                        });
                    }

                    lstDeudas.Add(new tblAF_DxP_Deuda
                    {
                        ContratoId = idContrato,
                        Descripcion = "INT X AMORTIZAR",
                        Debe = intAmortizar,
                        cc = "191",
                        area = "16",
                        cuenta = "2"
                    });

                    lstDeudas.Add(new tblAF_DxP_Deuda
                    {
                        ContratoId = idContrato,
                        Descripcion = "IVA DIF",
                        Debe = ivaDif,
                        cc = "191",
                        area = "16",
                        cuenta = "2"
                    });
                }
                else
                {
                    foreach (var item in lstDeudas)
                    {
                        item.Contrato = null;
                        guardarDeuda = false;
                    }
                }
                r.Success = true;
                r.Message = "Ok";

                poliza = getPoliza();
                r.Value = new
                {
                    desglose = desglose,
                    deuda = lstDeudas,
                    guardarDeuda = guardarDeuda,
                    poliza = poliza
                };
            }
            else
            {
                r.Success = false;
                r.Message = "El contrato aun no tiene el monto total del credito capturado";
                r.Value = null;
            }

            //}
            //catch(Exception ex)
            //{
            //    r.Message += ex.Message;
            //}
            return r;
        }
        public string getAreaCuenta(string ac, int pos)
        {
            try
            {
                return ac.Split('-')[pos];
            }
            catch (Exception)
            {

                return "";
            }

        }
        public ReporteDTO DesgloseGeneral(int idContrato)
        {
            var r = new Respuesta();
            ReporteDTO rptData = new ReporteDTO();
            try
            {
                var contrato = _context.tblAF_DxP_Contratos.FirstOrDefault(c => c.Id == idContrato);
                var contratoDisponibles = _context.tblAF_DxP_ContratoMaquinas.Where(m => m.ContratoId == idContrato && m.Estatus).ToList();

                var desglose = _context.tblAF_DxP_ContratosDetalle.Where(x => x.Contrato.Estatus && x.Estatus && x.ContratoId == idContrato).Select(m => new DesgloseGeneralDTO
                {
                    Id = m.Id,
                    ContratoId = m.ContratoId,
                    Parcialidad = m.Parcialidad,
                    AmortizacionCapital = m.AmortizacionCapital,
                    IVASCapital = m.IvaSCapital,
                    Interes = m.Intereses,
                    IVAInteres = m.IvaIntereses,
                    Importe = m.Importe,
                    Saldo = m.Saldo,
                    FechaVencimiento = m.FechaVencimiento,
                    Pagado = m.Pagado,
                    FechaPago = m.Pagado ? m.FechaPago : null

                }).ToList();

                rptData.detalle = desglose;
                rptData.contrato = contrato;
                return rptData;
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
                return null;
            }
        }
        public Respuesta ObtenerDesglosePorMaquina(int idContratoMaquina)
        {
            var r = new Respuesta();
            try
            {
                var desglose = _context.tblAF_DxP_ContratoMaquinasDetalle.Where(x => x.ContratoMaquina.Estatus && x.Estatus && x.ContratoMaquinaId == idContratoMaquina).Select(m => new DesglosePorMaquinaDTO
                {
                    Id = m.Id,
                    ContratoId = m.ContratoMaquina.ContratoId,
                    ContratoMaquinaId = m.ContratoMaquinaId,
                    Parcialidad = m.Parcialidad,
                    AmortizacionCapital = m.AmortizacionCapital,
                    IVASCapital = m.IvaSCapital,
                    Interes = m.Intereses,
                    IVAInteres = m.IvaIntereses,
                    Importe = m.Importe,
                    Saldo = m.Saldo,
                    FechaVencimiento = m.FechaVencimiento
                }).ToList();

                r.Success = true;
                r.Message = "Ok";
                r.Value = desglose;
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }
        public Dictionary<string, object> ObtenerInstituciones()
        {
            var r = new Dictionary<string, object>();

            try
            {
                var instituciones = _context.tblAF_DxP_Instituciones.Where(x => x.Estatus && !x.esPQ).Select(m => new ComboDTO
                {
                    Text = m.Nombre,
                    Value = m.Id.ToString(),
                    Prefijo = m.Id.ToString()
                }).ToList();

                r.Add(ITEMS, instituciones);
                r.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, "Error: " + ex.Message);
            }

            return r;
        }
        public Dictionary<string, object> ObtenerMaquinas()
        {
            var r = new Dictionary<string, object>();

            try
            {
                var maquinas = _context.tblM_CatMaquina.Select(m => new ComboDTO
                {
                    Text = m.noEconomico,
                    Value = m.id.ToString(),
                    Prefijo = m.id.ToString()
                }).OrderBy(m => m.Text).ToList();

                r.Add(ITEMS, maquinas);
                r.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, "Error: " + ex.Message);
            }

            return r;
        }
        private tblAF_DxP_Contrato ObtenerContrato(string folio, int contratoID)
        {
            if (contratoID == 0)
                return _context.tblAF_DxP_Contratos.FirstOrDefault(p => p.Folio == folio && p.Estatus);
            else
                return null;
        }
        public Dictionary<string, object> ObtenerPagos(int contratoDetID, int parcialidad)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                var contratoDet = _context.tblAF_DxP_ContratosDetalle.Where(r => r.Id == contratoDetID).Select(x => new
                {
                    x.AmortizacionCapital,
                    x.ContratoId,
                    x.Estatus,
                    x.FechaPago,
                    x.FechaVencimiento,
                    x.GeneroInteresMoratorio,
                    x.Id,
                    x.Importe,
                    x.Intereses,
                    x.IvaIntereses,
                    x.IvaSCapital,
                    x.Pagado,
                    x.Parcialidad,
                    x.Saldo
                }).FirstOrDefault();

                int contratoID = 0;
                if (contratoDet != null)
                {
                    contratoID = contratoDet.ContratoId;

                    var validarPagoActual = _context.tblAF_DxP_ContratosDetalle.Where(r => r.ContratoId == contratoID && r.Pagado == false).FirstOrDefault();

                    bool esPagoActual = validarPagoActual.Id == contratoDetID ? true : false;

                    var contrato = _context.tblAF_DxP_Contratos.Where(r => r.Id == contratoID).Select(r => new
                    {
                        r.AmortizacionCapital,
                        r.Credito,
                        r.Descripcion,
                        r.Domiciliado,
                        r.Estatus,
                        r.FechaCreacion,
                        r.FechaInicio,
                        r.FechaModificacion,
                        r.FechaVencimiento,
                        r.FechaVencimientoTipoId,
                        r.FileContrato,
                        r.FilePagare,
                        r.Folio,
                        r.Id,
                        r.InstitucionId,
                        r.InteresMoratorio,
                        r.IVA,
                        r.Plazo,
                        r.TasaInteres,
                        r.Terminado,
                        r.TipoCambio,
                        r.TipoFechaVencimiento,
                        r.UsuarioCreacionId,
                        r.UsuarioModificacionId
                    }).FirstOrDefault();

                    var pagos = _context.tblAF_DxP_Pagos.Where(r => r.PeriodoId == contratoDet.Id).Select(r => new
                    {
                        r.ArchivoPago,
                        r.Estatus,
                        r.FechaCreacion,
                        r.FechaModificacion,
                        r.FechaPago,
                        r.Id,
                        r.Monto,
                        pagoParcial = r.PagoParcial,
                        r.PeriodoId,
                        r.UsuarioCreacionId,
                        r.UsuarioModificacionId
                    }).FirstOrDefault();

                    if (pagos != null)
                    {
                        var pagosDet = _context.tblAF_DxP_PagosMaquina.Where(r => r.PagoId == pagos.Id).ToList();
                        var tempPagos = pagosDet.Select(r => r.ContratoMaquinaId).ToList();
                        var contratoMaquinaDet = _context.tblAF_DxP_ContratoMaquinasDetalle.Where(m => tempPagos.Contains(m.Id))
                                     .Select(n => new
                                     {
                                         n.AmortizacionCapital,
                                         n.ContratoDetalleId,
                                         n.ContratoMaquinaId,
                                         n.Estatus,
                                         n.FechaPago,
                                         n.FechaVencimiento,
                                         n.GeneroInteresMoratorio,
                                         n.Id,
                                         n.Importe,
                                         n.Intereses,
                                         n.IvaIntereses,
                                         n.IvaSCapital,
                                         n.Pagado,
                                         n.Parcialidad,
                                         n.Saldo,
                                         Maquina = n.ContratoMaquina.Maquina.noEconomico
                                     }).ToList();

                        result.Add("contratoMaquinaDet", contratoMaquinaDet);
                    }
                    else
                    {
                        var contratoMaquina = _context.tblAF_DxP_ContratoMaquinas.Where(m => m.ContratoId == contrato.Id).ToList();
                        var contratoMaquinaDet = _context.tblAF_DxP_ContratoMaquinasDetalle.Where(m => m.ContratoDetalleId == contratoDet.Id && m.Parcialidad == parcialidad)
                                     .Select(n => new
                                             {
                                                 n.AmortizacionCapital,
                                                 n.ContratoDetalleId,
                                                 n.ContratoMaquinaId,
                                                 n.Estatus,
                                                 n.FechaPago,
                                                 n.FechaVencimiento,
                                                 n.GeneroInteresMoratorio,
                                                 n.Id,
                                                 n.Importe,
                                                 n.Intereses,
                                                 n.IvaIntereses,
                                                 n.IvaSCapital,
                                                 n.Pagado,
                                                 n.Parcialidad,
                                                 n.Saldo,
                                                 Maquina = n.ContratoMaquina.Maquina.noEconomico
                                             }).ToList();
                        result.Add("contratoMaquinaDet", contratoMaquinaDet);
                    }
                    result.Add("contrato", contrato);
                    result.Add("contratoDet", contratoDet);
                    result.Add("pagoActual", esPagoActual);
                    result.Add(SUCCESS, true);
                }

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return result;
        }
        public Dictionary<string, object> SaveOrupdatepagos(tblAF_DxP_Pago dxpPago, List<tblAF_DxP_PagoMaquina> dxpPagoMaquina, List<tblAF_DxP_ContratoMaquinaDetalle> dxpContratoMaquinaDetalle)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();


            //Jessica Galdean, Lili Lavandera
            List<int> lstAdminID = new List<int>() { 3978, 6587, 6278, 79888, 1073 };

            //tblP_Usuario objUsr = _context.tblP_Usuario.Where(e => e.cveEmpleado == vSesiones.sesionUsuarioDTO.id.ToString()).FirstOrDefault();


            if (vSesiones.sesionUsuarioDTO.idPerfil != (int)PerfilUsuarioEnum.ADMINISTRADOR)
            {
                //contratosDTO[0].esAdmin = true;
                if (!lstAdminID.Contains(vSesiones.sesionUsuarioDTO.id))
                {
                    resultado.Add(MESSAGE, "Usuario no permitido");
                    resultado.Add(SUCCESS, false);
                    return resultado;
                }
            }

            resultado.Clear();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    int contratoDetTempID = dxpContratoMaquinaDetalle.FirstOrDefault().ContratoDetalleId;
                    dxpPago.PeriodoId = contratoDetTempID;
                    if (dxpPago.Id != 0)
                    {

                        var dxpPagoTemp = _context.tblAF_DxP_Pagos.First(r => r.Id == dxpPago.Id);

                        decimal pagoPacial = dxpPagoTemp.PagoParcial + dxpPagoTemp.PagoParcial;

                        if (pagoPacial == dxpPagoTemp.Monto)
                        {
                            dxpPagoTemp.Estatus = false;
                            _context.SaveChanges();
                        }
                        _context.tblAF_DxP_PagosMaquina.AddRange(dxpPagoMaquina);
                        _context.SaveChanges();

                    }
                    else
                    {
                        _context.tblAF_DxP_Pagos.Add(dxpPago);
                        _context.SaveChanges();
                        dxpPagoMaquina.ForEach(r =>
                        {
                            r.PagoId = dxpPago.Id;
                        });

                        _context.tblAF_DxP_PagosMaquina.AddRange(dxpPagoMaquina);
                        _context.SaveChanges();
                    }

                    foreach (var item in dxpContratoMaquinaDetalle)
                    {
                        var tmpContratoMaquinaDet = _context.tblAF_DxP_ContratoMaquinasDetalle.FirstOrDefault(r => r.Id == item.Id);
                        var tmpContrato = _context.tblAF_DxP_ContratosDetalle.FirstOrDefault(r => r.Id == item.ContratoDetalleId);
                        if (tmpContratoMaquinaDet != null)
                        {
                            tmpContratoMaquinaDet.Estatus = true;
                            tmpContratoMaquinaDet.Pagado = true;
                            tmpContratoMaquinaDet.FechaPago = dxpPago.FechaPago;
                            _context.SaveChanges();
                        }
                        tmpContrato.Estatus = true;

                        tmpContrato.FechaPago = dxpPago.FechaPago;
                        _context.SaveChanges();
                    }

                    transaction.Commit();
                    transaction.Dispose();
                    resultado.Add(SUCCESS, true);

                }
                catch (Exception)
                {
                    transaction.Rollback();
                    resultado.Add(SUCCESS, false);
                }
            }

            return resultado;
        }
        public Dictionary<string, object> GuardarFechaNuevoPeriodo(int contratoID, DateTime nuevaFecha, int parcialidad)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    var contratoDetList = _context.tblAF_DxP_ContratosDetalle.Where(r => r.ContratoId == contratoID && r.Parcialidad >= parcialidad);

                    DateTime tmpFecha = new DateTime();
                    tmpFecha = nuevaFecha;
                    foreach (var contratoDet in contratoDetList)
                    {
                        contratoDet.FechaVencimiento = tmpFecha;
                        tmpFecha = tmpFecha.AddMonths(1);
                    }
                    _context.SaveChanges();
                    var contratoDetMaquinaList = _context.tblAF_DxP_ContratoMaquinasDetalle.Where(r => r.ContratoMaquina.ContratoId == contratoID && r.Parcialidad >= parcialidad);

                    tmpFecha = nuevaFecha;
                    foreach (var contratoMaquinaDet in contratoDetMaquinaList)
                    {
                        contratoMaquinaDet.FechaVencimiento = tmpFecha;
                        tmpFecha = tmpFecha.AddMonths(1);
                    }
                    _context.SaveChanges();

                    trans.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "La Modificación de la información de la fecha no fué efectuada, favor de volver a intentar");
                }
            }

            return resultado;
        }
        public Dictionary<string, object> guardarInstitucion(string descripcion)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            tblAF_DxP_Institucion institucion = new tblAF_DxP_Institucion();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (_context.tblAF_DxP_Instituciones.FirstOrDefault(r => r.Nombre == descripcion && !r.esPQ) == null)
                    {
                        institucion.Nombre = descripcion;
                        institucion.FechaCreacion = DateTime.Now;
                        institucion.FechaModificacion = DateTime.Now;
                        institucion.UsuarioCreacionId = vSesiones.sesionUsuarioDTO.id;
                        institucion.UsuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                        institucion.Estatus = true;
                        institucion.esPQ = false;
                        _context.tblAF_DxP_Instituciones.Add(institucion);
                        _context.SaveChanges();
                        transaction.Commit();
                        resultado.Add(SUCCESS, true);
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    resultado.Add(SUCCESS, false);
                }
            }

            return resultado;
        }
        public Dictionary<string, object> GetTipoCambioFecha(DateTime fecha)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {

                result.Add("tipoCambio", getTipoCambioDLLs(fecha));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "No sé pudo encontrar el tipo de cambio para la fecha seleccionada");
            }

            return result;
        }
        private TipoCambioDllDTO getTipoCambioDLLs(DateTime fecha)
        {
            string consulta = @"SELECT moneda,fecha,tipo_cambio,empleado_modifica,fecha_modifica,hora_modifica, 0 as tc_anterior FROM tipo_cambio
                                    WHERE fecha BETWEEN '" + fecha.ToString("yyyyMMdd") + "' AND '" + fecha.ToString("yyyyMMdd") + "';";
            var res1 = (IList<TipoCambioDllDTO>)_contextEnkontrol.Where(consulta).ToObject<IList<TipoCambioDllDTO>>();
            return res1.FirstOrDefault();

        }
        #endregion
        #region Seccion de Programacion de pagos.
        public Dictionary<string, object> CargarPropuesta(DateTime pFechaInicio, DateTime pFechaFinal, int estatus, int institucion)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                List<dtProgramacionPagosDTO> lstProgramacionPagos = new List<dtProgramacionPagosDTO>();
                switch (estatus)
                {
                    case 1:
                    case 2:
                        {
                            resultado.Add("result", lstProgramacionPagos);
                        }
                        break;
                    case 0:
                        {
                            resultado.Add("result", lstProgramacionPagos);
                        }
                        break;
                    default:
                        break;
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public Dictionary<string, object> GuardarProgramacion(List<tblAF_DxP_ProgramacionPagos> list)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in list)
                    {

                        var contratosMaquina = _context.tblAF_DxP_ContratoMaquinas.FirstOrDefault(r => r.ContratoId == item.contratoid && r.Maquina.noEconomico == item.noEconomico);
                        var fechaVencimiento = _context.tblAF_DxP_ContratoMaquinasDetalle.Where(r => r.ContratoMaquina.ContratoId == item.contratoid && r.Parcialidad == item.parcialidad).FirstOrDefault();
                        tblAF_DxP_ProgramacionPagos programacionPagos = new tblAF_DxP_ProgramacionPagos();

                        programacionPagos.ac = item.areaCuenta;
                        programacionPagos.aplicado = 0;
                        programacionPagos.capital = item.capital;
                        programacionPagos.cc = item.cc;
                        programacionPagos.contrato = item.contrato;
                        programacionPagos.contratoid = item.contratoid;
                        programacionPagos.fechaCaptura = item.fechaCaptura;
                        programacionPagos.fechaVencimiento = fechaVencimiento.FechaVencimiento; //_context.tblAF_DxP_ContratoMaquinasDetalle.Where(r=>r.Parcialidad == item.parcialidad && r.ContratoMaquinaId == r. );
                        programacionPagos.financiamiento = item.financiamiento;
                        programacionPagos.importe = item.importe;
                        programacionPagos.importeDLLS = item.importeDLLS;
                        programacionPagos.intereses = item.intereses;
                        programacionPagos.iva = item.iva;
                        programacionPagos.mensualidad = item.mensualidad;
                        programacionPagos.noEconomico = item.noEconomico;
                        programacionPagos.parcialidad = item.parcialidad;
                        programacionPagos.porcentaje = item.porcentaje;
                        programacionPagos.rfc = contratosMaquina.Contrato.rfc;
                        programacionPagos.tipoCambio = item.tipoCambio;
                        programacionPagos.importeDLLS = item.importeDLLS;
                        programacionPagos.total = item.total; //contrato.Contrato.monedaContrato == 1 ? (item.importe * contrato.porcentaje) / 100 : (programacionPagos.importeDLLS * contrato.Contrato.TipoCambio);
                        programacionPagos.ivaInteres = item.ivaInteres;
                        programacionPagos.usuarioCaptura = vSesiones.sesionUsuarioDTO.id;
                        programacionPagos.empresa = item.empresa;
                        programacionPagos.moneda = item.moneda;
                        programacionPagos.liquidar = item.liquidar;
                        programacionPagos.penaConvencional = item.penaConvencional;
                        programacionPagos.opcionCompra = item.opcionCompra;
                        programacionPagos.montoOpcionCompra = item.montoOpcionCompra;
                        programacionPagos.maquinaId = item.maquinaId;

                        _context.tblAF_DxP_ProgramacionPagos.Add(programacionPagos);
                        _context.SaveChanges();

                    }
                    resultado.Add(SUCCESS, true);
                    trans.Commit();
                }
                catch (Exception e)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    trans.Rollback();
                }
            }
            return resultado;
        }
        private int getPoliza()
        {
            try
            {
                var odbcPoliza = new OdbcConsultaDTO();
                odbcPoliza.consulta = @"SELECT MAX(poliza)+1 as maxVal FROM sc_polizas WHERE year=" + DateTime.Now.Year + " AND mes=" + DateTime.Now.Month + " AND tp='03'";
                var maxPoliza = _contextEnkontrol.Select<Autoincremento>(productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, odbcPoliza).FirstOrDefault().maxVal;
                return maxPoliza;
            }
            catch (Exception)
            {

                return 0;
            }
        }
        public Dictionary<string, object> getPolizaByFecha(DateTime fecha)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                var odbcPoliza = new OdbcConsultaDTO();
                odbcPoliza.consulta = @"SELECT MAX(poliza)+1 as maxVal FROM sc_polizas WHERE year=" + fecha.Year + " AND mes=" + fecha.Month + " AND tp='03'";
                if (vSesiones.sesionEmpresaActual == 1)
                {
                    var maxPoliza = _contextEnkontrol.Select<Autoincremento>(productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, odbcPoliza).FirstOrDefault().maxVal;
                    if (maxPoliza == 0)
                    {
                        maxPoliza = 1;
                    }
                    resultado.Add("maxPoliza", maxPoliza);
                    return resultado;
                }
                else
                {
                    var maxPoliza = _contextEnkontrol.Select<Autoincremento>(productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, odbcPoliza).FirstOrDefault().maxVal;
                    if (maxPoliza == 0)
                    {
                        maxPoliza = 1;
                    }
                    resultado.Add("maxPoliza", maxPoliza);
                    return resultado;
                }

            }
            catch (Exception)
            {

                resultado.Add("maxPoliza", 0);
                return resultado;
            }

        }
        public Dictionary<string, object> loadPropuestas(int? idInstitucion, int empresa, int moneda)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {

                var contratoLista2 = _context.tblAF_DxP_ProgramacionPagos.Where(r => r.aplicado == 0 && r.empresa == empresa && r.moneda == moneda)
                                    .GroupBy(n => new { n.contratoid, n.parcialidad }).ToList()
                                    .Select(f => new
                                    {
                                        contratoID = f.Key.contratoid,
                                        folioContrato = f.Where(p => p.contratoid == f.Key.contratoid).Select(g => g.contrato).FirstOrDefault(),
                                        parcialidad = f.Where(p => p.contratoid == f.Key.contratoid).Select(g => g.parcialidad).FirstOrDefault(),
                                        importeProgramado = f.First().tipoCambio != 1 ? f.Where(p => p.contratoid == f.Key.contratoid).Sum(g => g.importe) : f.Where(p => p.contratoid == f.Key.contratoid).Sum(g => g.total),//Math.Round(f.Where(p => p.contratoid == f.Key.contratoid).Sum(g => g.total), 2),
                                        fechaVencimiento = f.Where(p => p.contratoid == f.Key.contratoid).Select(g => g.fechaVencimiento).FirstOrDefault().ToShortDateString(),
                                        tipoCambio = f.Where(p => p.contratoid == f.Key.contratoid).Select(g => g.tipoCambio).FirstOrDefault(),
                                        aplicaPago = 1,
                                        plazos = _context.tblAF_DxP_Contratos.FirstOrDefault(c => c.Id == f.Key.contratoid).Plazo,
                                        idInstitucion = _context.tblAF_DxP_Contratos.FirstOrDefault(c => c.Id == f.Key.contratoid).InstitucionId
                                    }).ToList().Where(g => idInstitucion != null ? g.idInstitucion == idInstitucion.Value : false);

                resultado.Add("maxPoliza", getPoliza());
                resultado.Add("result", contratoLista2);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, ex.Message);
            }

            return resultado;
        }
        public Dictionary<string, object> CargarContrato(int contratoId, int parcialidad, DateTime fechaPol)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            try
            {
                string maquinasEconomicos = "";
                _context.tblAF_DxP_ContratoMaquinas.Where(r => r.ContratoId == contratoId).Select(r => r.Maquina.noEconomico).Distinct().ToList().ForEach(e =>
               {
                   maquinasEconomicos += "'" + e + "',";
               });

                var odbc = new OdbcConsultaDTO();
                odbc.consulta = @"SELECT A.cc AS Value,A.descripcion AS Text
                                  FROM cc A WHERE A.descripcion IN (" + maquinasEconomicos.TrimEnd(',') + ")";
                List<ComboDTO> listaEconomicosCC = _contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.ArrenProd, odbc);

                //DateTime mesAnterior = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                DateTime mesAnterior = new DateTime(fechaPol.Year, fechaPol.Month, 1).AddDays(-1);
                var tipoFecha = getTipoCambioDLLs(mesAnterior);

                var contrato = _context.tblAF_DxP_Contratos.FirstOrDefault(r => r.Id == contratoId);

                var contratoHistoricoDescripcion = _context.tblAF_DxP_ContratosDetalle
                                      .Where(r => r.ContratoId == contratoId && r.Parcialidad == parcialidad)
                                      .Select(n => n.Contrato.Folio).FirstOrDefault();

                int _numeros = 0;
                if (contratoHistoricoDescripcion != null)
                {
                    var soloNumeros = string.Empty;

                    for (int i = contratoHistoricoDescripcion.Length - 1; i > -1; i--)
                    {
                        if (Char.IsDigit(contratoHistoricoDescripcion[i]))
                        {
                            soloNumeros += contratoHistoricoDescripcion[i];
                        }
                    }

                    if (soloNumeros.Length > 0)
                    {
                        var _numTemp = "";
                        for (int i = soloNumeros.Length - 1; i > -1; i--)
                        {
                            _numTemp += soloNumeros[i];
                        }

                        _numeros = int.Parse(_numTemp.Substring(_numTemp.Length > 6 ? _numTemp.Length - 6 : 0, _numTemp.Length < 6 ? _numTemp.Length : 6));
                    }

                    if (_numeros == 0)
                    {
                        _numeros = int.Parse(DateTime.Now.ToString("ffffff"));
                    }
                }

                var ccEspecial = _context.tblAF_DxP_CCContrato.FirstOrDefault(x => x.contratoID == contratoId);

                var aplicaCC191 = false;
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                {
                    if (contrato.FechaInicio >= new DateTime(2020, 02, 01))
                    {
                        aplicaCC191 = true;
                    }

                    if (contrato.Institucion.Id == 9) //CATERPILLAR
                    {
                        if (contrato.Id <= 1189 && contrato.Id != 1165 && contrato.Id != 1171)
                        {
                            aplicaCC191 = false;
                        }
                    }
                }

                var programacionPagos = _context.tblAF_DxP_ProgramacionPagos.Where(r => r.parcialidad == parcialidad && r.contratoid == contratoId).ToList().Select(r => new ProgramacionDTO
                {
                    cc = vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora ? getCCInvalid(r.cc) : r.cc,
                    importe = r.tipoCambio != 1 ? r.importe : r.total,
                    importeDLLS = r.importeDLLS,
                    capital = r.capital,
                    intereses = r.intereses,
                    iva = r.iva,
                    ivaInteres = r.ivaInteres,
                    porcentaje = r.porcentaje,
                    total = r.total,
                    noEconomico = r.noEconomico,
                    idCatMaquina = r.maquinaId,
                    parcialidad = r.parcialidad,
                    tipoCambio = r.tipoCambio,
                    areaCuenta = r.ac,
                    liquidar = r.liquidar,
                    penaConvencional = r.liquidar ? r.penaConvencional.Value : 0M,
                    opcionCompra = r.opcionCompra,
                    montoOpcionCompra = r.opcionCompra ? r.montoOpcionCompra : 0M
                }).ToList();

                var economicos = programacionPagos.Select(x => x.idCatMaquina).ToList();
                var inventarioMaq = new List<tblM_CorteInventarioMaq_Detalle>();

                using (var _ctxArre = new MainContext(EmpresaEnum.Arrendadora))
                {
                    var corte = _ctxArre.tblM_CorteInventarioMaq.Where(x => x.FechaCorte <= fechaPol && x.Estatus).OrderByDescending(x => x.FechaCorte).FirstOrDefault();
                    if (corte != null)
                    {
                        inventarioMaq = _ctxArre.tblM_CorteInventarioMaq_Detalle.Where(x => x.IdCorteInvMaq == corte.Id).ToList();
                    }
                }

                var importeDeLaParcialidadDeLosEquipos = 0M;
                var interesesDeLaParcialidadesDeLosEquipos = 0M;
                var ivaDeLaParcialidadesDeLosEquipos = 0M;
                var ivaInteresDeLaParcialidadesDeLosEquipos = 0M;

                foreach (var item in programacionPagos)
                {
                    var acCargo = inventarioMaq.FirstOrDefault(x => x.IdEconomico == item.idCatMaquina);
                    if (acCargo != null)
                    {
                        switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                        {
                            case EmpresaEnum.Construplan:
                                item.cc = getCCInvalid(acCargo.cc.Trim());
                                item.areaCuenta = acCargo.ccCargoObra;
                                break;
                            case EmpresaEnum.Arrendadora:
                                item.areaCuenta = acCargo.ccCargoObra;
                                break;
                        }
                    }

                    if (item.liquidar)
                    {
                        var parcialidadesPendientes = _context.tblAF_DxP_ContratoMaquinasDetalle
                            .Where(x =>
                                x.ContratoMaquina.Maquina.noEconomico.ToUpper() == item.noEconomico.ToUpper() &&
                                !x.Pagado &&
                                x.Estatus).ToList();

                        importeDeLaParcialidadDeLosEquipos += parcialidadesPendientes.Sum(x => x.Importe);
                        interesesDeLaParcialidadesDeLosEquipos += parcialidadesPendientes.Sum(x => x.Intereses);
                        ivaDeLaParcialidadesDeLosEquipos += parcialidadesPendientes.Sum(x => x.IvaSCapital);
                        ivaInteresDeLaParcialidadesDeLosEquipos += parcialidadesPendientes.Sum(x => x.IvaIntereses);
                    }
                    else
                    {
                        var parcialidadPendiente = _context.tblAF_DxP_ContratoMaquinasDetalle
                            .First(x =>
                                x.ContratoMaquina.Maquina.noEconomico.ToUpper() == item.noEconomico.ToUpper() &&
                                x.Parcialidad == parcialidad &&
                                !x.Pagado &&
                                x.Estatus);

                        importeDeLaParcialidadDeLosEquipos += parcialidadPendiente.Importe;
                        interesesDeLaParcialidadesDeLosEquipos += parcialidadPendiente.Intereses;
                        ivaDeLaParcialidadesDeLosEquipos += parcialidadPendiente.IvaSCapital;
                        ivaInteresDeLaParcialidadesDeLosEquipos += parcialidadPendiente.IvaIntereses;
                    }
                }

                var contratoHistorico = _context.tblAF_DxP_ContratosDetalle
                                      .Where(r => r.ContratoId == contratoId && r.Parcialidad == parcialidad)
                                      .ToList()
                                      .Select(n =>
                                                new
                                                {
                                                    parcialidad = n.Parcialidad,
                                                    capital = n.AmortizacionCapital,
                                                    //ivaCapita = n.IvaSCapital,
                                                    ivaCapita = ivaDeLaParcialidadesDeLosEquipos,
                                                    //interesesPeriodo = n.Intereses,
                                                    interesesPeriodo = interesesDeLaParcialidadesDeLosEquipos,
                                                    //ivaIntereses = n.IvaIntereses,
                                                    ivaIntereses = ivaInteresDeLaParcialidadesDeLosEquipos,
                                                    //importePago = n.Importe,
                                                    importePago = importeDeLaParcialidadDeLosEquipos,
                                                    saldoCredito = n.Saldo,
                                                    contrato = n.ContratoId,
                                                    cta = n.Contrato.cta,
                                                    scta = n.Contrato.scta,
                                                    sscta = n.Contrato.sscta,
                                                    digito = n.Contrato.digito,
                                                    monedaContrato = n.Contrato.monedaContrato,
                                                    tipoCambioPeriodioAnterior = n.Contrato.monedaContrato == 1 ? 1 :
                                                        n.Contrato.fechaFirma.Value.Year == n.FechaVencimiento.Year && n.Contrato.fechaFirma.Value.Month == n.FechaVencimiento.Month ? n.Contrato.TipoCambio : tipoFecha.tipo_cambio,
                                                    descripcion = _numeros,
                                                    plazo = n.Contrato.Plazo,
                                                    contratoId = n.Contrato.Id,
                                                    folio = n.Contrato.Folio,
                                                    tipoCambioHistorico = n.Contrato.TipoCambio,
                                                    rfc = n.Contrato.rfc,
                                                    mostrarRFCenIVA = n.Contrato.Institucion.Nombre.Contains("SCOTIA") ? false : true,
                                                    arrendamientoPuro = n.Contrato.arrendamientoPuro,
                                                    calcularIVA = n.Contrato.cta != 5000,
                                                    ctaIA = n.Contrato.ctaIA,
                                                    sctaIA = n.Contrato.sctaIA,
                                                    ssctaIA = n.Contrato.ssctaIA,
                                                    digitoIA = n.Contrato.digitoIA,
                                                    cc191 = aplicaCC191,
                                                    tieneCCEspecial = ccEspecial != null ? true : false,
                                                    ccEspecial = ccEspecial != null ? ccEspecial.cc : ""
                                                }).FirstOrDefault();

                var ctasInstitucion = _context.tblAF_DxP_RelInstitucionCta.Where(r => r.institucionID == contrato.InstitucionId && r.activo && r.moneda == contrato.monedaContrato).ToList();

                resultado.Add("ctasInstitucion", ctasInstitucion);
                resultado.Add("contratoDetalle", programacionPagos);//contratoDetalle.ToList());
                resultado.Add("contratoHistorico", contratoHistorico);

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "No se encontro información del contrato, ");
            }
            return resultado;
        }
        public Dictionary<string, object> CargarDetallePago(int parcialidad, int contratoId)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            try
            {
                var contratoDetalle = _context.tblAF_DxP_ProgramacionPagos.Where(r => r.contratoid == contratoId && parcialidad == r.parcialidad).Select(r => new
                {
                    folio = r.contrato,
                    noEconomico = r.noEconomico,
                    ac = r.ac,
                    financiamiento = r.financiamiento,
                    fechaVencimiento = r.fechaVencimiento,
                    capital = r.capital,
                    intereses = r.intereses,
                    iva = r.iva,
                    importe = r.importe,
                    porcentaje = r.porcentaje,
                    importeDLLS = r.importeDLLS,
                    tipoCambio = r.tipoCambio,
                    total = r.total
                }).ToList();

                resultado.Add("contratoDetalle", contratoDetalle);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {

                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }
        private OdbcConnection checkConexionProductivo()
        {
            if (productivo)
            {
                return new Conexion().Connect();
            }
            else
            {
                return new Conexion().ConnectPrueba();
            }

        }
        public Dictionary<string, object> guardarPoliza(tblC_sc_polizas poliza, List<tblC_sc_movpol> movpol, List<listaContrato> contrato, decimal tipoCambio)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            var mensajeErrorExtra = "";
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var _numPoliza = 0;
                    var _contrato = new tblAF_DxP_Contrato();
                    var _tipoCambio = 1M;

                    if (poliza.id != 0)
                    {
                        var objEntity = _context.tblC_sc_polizas.FirstOrDefault(r => r.id == poliza.id);
                        objEntity.cargos = poliza.cargos;
                        objEntity.abonos = poliza.abonos;
                        _context.SaveChanges();
                    }
                    else
                    {
                        _numPoliza = (int)getPolizaByFecha(new DateTime(poliza.year, poliza.mes, 1))["maxPoliza"];

                        if (_numPoliza > 0)
                        {
                            poliza.poliza = _numPoliza;

                            foreach (var mov in movpol)
                            {
                                mov.poliza = _numPoliza;
                            }
                        }
                        else
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Error al obtener el número de póliza a capturar");

                            dbTransaction.Rollback();

                            return resultado;
                        }

                        _context.tblC_sc_polizas.Add(poliza);
                        _context.SaveChanges();
                    }

                    foreach (var obj in movpol)
                    {
                        if (obj.id != 0)
                        {
                            var objEntity = _context.tblC_sc_movpol.FirstOrDefault(r => r.id == obj.id);
                            objEntity.monto = obj.monto;
                            objEntity.cta = obj.cta;
                            objEntity.scta = obj.scta;
                            objEntity.sscta = obj.sscta;
                            _context.SaveChanges();
                        }
                        else
                        {
                            obj.year = poliza.year;
                            obj.mes = poliza.mes;
                            obj.tp = poliza.tp;
                            obj.poliza = poliza.poliza;

                            _context.tblC_sc_movpol.Add(obj);
                            _context.SaveChanges();
                        }
                    }

                    var _contratoId = contrato.First().contratoID;
                    _contrato = _context.tblAF_DxP_ContratosDetalle.First(r => r.ContratoId == _contratoId).Contrato;
                    _tipoCambio = tipoCambio;

                    foreach (var obj in movpol)
                    {
                        var relInsCuenta = _context.tblAF_DxP_RelInstitucionCta.FirstOrDefault
                            (f =>
                                f.institucionID == _contrato.InstitucionId &&
                                f.activo &&
                                f.cta == obj.cta &&
                                f.scta == obj.scta &&
                                f.sscta == obj.sscta
                            );

                        if (relInsCuenta != null)
                        {
                            var odbcCuentaReferencia = new OdbcConsultaDTO();
                            odbcCuentaReferencia.consulta =
                                @"
                                    SELECT TOP 1
                                        referencia
                                    FROM
                                        sc_movpol AS MOV
                                    INNER JOIN
                                        sc_polizas AS POL
                                            ON
                                                POL.year = MOV.year AND
                                                POL.mes = MOV.mes AND
                                                POL.poliza = MOV.poliza AND
                                                POL.tp = MOV.tp
                                    WHERE
                                        POL.fechapol = ? AND
                                        MOV.year = ? AND
                                        MOV.mes = ? AND
                                        MOV.cta = ? AND
                                        MOV.tm = ? AND
                                        MOV.referencia = ? AND
                                        MOV.cc = ?";

                            odbcCuentaReferencia.parametros.Add(new OdbcParameterDTO
                            {
                                nombre = "fechapol",
                                tipo = OdbcType.Date,
                                valor = poliza.fechapol
                            });
                            odbcCuentaReferencia.parametros.Add(new OdbcParameterDTO
                            {
                                nombre = "year",
                                tipo = OdbcType.Int,
                                valor = poliza.year
                            });
                            odbcCuentaReferencia.parametros.Add(new OdbcParameterDTO
                            {
                                nombre = "mes",
                                tipo = OdbcType.Int,
                                valor = poliza.mes
                            });
                            odbcCuentaReferencia.parametros.Add(new OdbcParameterDTO
                            {
                                nombre = "cta",
                                tipo = OdbcType.Int,
                                valor = relInsCuenta.cta
                            });
                            odbcCuentaReferencia.parametros.Add(new OdbcParameterDTO
                            {
                                nombre = "tm",
                                tipo = OdbcType.Int,
                                valor = obj.tm
                            });
                            odbcCuentaReferencia.parametros.Add(new OdbcParameterDTO
                            {
                                nombre = "referencia",
                                tipo = OdbcType.VarChar,
                                valor = obj.referencia
                            });
                            odbcCuentaReferencia.parametros.Add(new OdbcParameterDTO
                            {
                                nombre = "cc",
                                tipo = OdbcType.VarChar,
                                valor = obj.cc
                            });

                            var existeRef = _contextEnkontrol.Select<tblC_sc_movpol>(productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, odbcCuentaReferencia).FirstOrDefault();

                            if (existeRef != null)
                            {
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "Ya existe la referencia " + obj.referencia);

                                dbTransaction.Rollback();

                                return resultado;
                            }
                        }
                    }

                    foreach (var gbContratoID in contrato.GroupBy(g => g.contratoID).ToList())
                    {
                        bool esSoloUnEquipo = _context.tblAF_DxP_ContratoMaquinas.Where(x => x.ContratoId == gbContratoID.Key && x.Estatus).Count() == 1;

                        foreach (var gbParcialidad in gbContratoID.GroupBy(g => g.parcialidad))
                        {
                            var dato = _context.tblAF_DxP_ProgramacionPagos.Where(r => r.contratoid == gbContratoID.Key && gbParcialidad.Key == r.parcialidad).ToList();
                            var contratosDet = _context.tblAF_DxP_ContratosDetalle.Where(r => r.ContratoId == gbContratoID.Key && gbParcialidad.Key == r.Parcialidad).ToList();

                            foreach (var entidad in contratosDet)
                            {
                                var objEntity = _context.tblAF_DxP_ContratosDetalle.FirstOrDefault(r => r.Id == entidad.Id);

                                objEntity.Pagado = true;
                                objEntity.FechaPago = poliza.fechapol;
                                _context.SaveChanges();

                            }
                            var contratosDetM = _context.tblAF_DxP_ContratoMaquinasDetalle.Where(r => r.ContratoMaquina.ContratoId == gbContratoID.Key && gbParcialidad.Key == r.Parcialidad).ToList();

                            foreach (var entidad in contratosDetM)
                            {
                                var objEntity = _context.tblAF_DxP_ContratoMaquinasDetalle.FirstOrDefault(r => r.Id == entidad.Id);

                                objEntity.Pagado = true;
                                objEntity.FechaPago = poliza.fechapol;
                                _context.SaveChanges();
                            }

                            foreach (var entidad in dato)
                            {
                                var objEntity = _context.tblAF_DxP_ProgramacionPagos.FirstOrDefault(r => r.id == entidad.id);
                                objEntity.aplicado = 2;

                                if (entidad.liquidar)
                                {
                                    var maquinaDetalles = _context.tblAF_DxP_ContratoMaquinasDetalle
                                        .Where(x =>
                                            x.ContratoMaquina.ContratoId == entidad.contratoid &&
                                            x.ContratoMaquina.MaquinaId == entidad.maquinaId &&
                                            !x.Pagado).ToList();

                                    foreach (var item in maquinaDetalles)
                                    {
                                        item.Pagado = true;
                                        item.FechaPago = poliza.fechapol;
                                    }

                                    if (esSoloUnEquipo)
                                    {
                                        var detallesEquipo = _context.tblAF_DxP_ContratosDetalle.Where(x => x.ContratoId == gbContratoID.Key && x.Estatus && !x.Pagado).ToList();
                                        foreach (var item in detallesEquipo)
                                        {
                                            item.Pagado = true;
                                            item.FechaPago = poliza.fechapol;
                                        }

                                        var liquidarContrato = _context.tblAF_DxP_Contratos.First(x => x.Id == gbContratoID.Key);
                                        liquidarContrato.Terminado = true;

                                        _context.SaveChanges();
                                    }
                                    else
                                    {
                                        bool terminado = true;
                                        var equipos = _context.tblAF_DxP_ContratoMaquinas.Where(x => x.ContratoId == gbContratoID.Key && x.Estatus).ToList();
                                        foreach (var item in equipos)
                                        {
                                            terminado = !_context.tblAF_DxP_ContratoMaquinasDetalle.Any(x => x.ContratoMaquinaId == item.Id && !x.Pagado);

                                            if (!terminado)
                                            {
                                                break;
                                            }
                                        }

                                        if (terminado)
                                        {
                                            var liquidarContrato = _context.tblAF_DxP_Contratos.First(x => x.Id == gbContratoID.Key);
                                            liquidarContrato.Terminado = true;
                                            _context.SaveChanges();
                                        }
                                    }
                                }
                                
                                _context.SaveChanges();
                            }
                        }
                    }

                    using (var con = checkConexionProductivo())
                    {
                        using (var trans = con.BeginTransaction())
                        {
                            try
                            {
                                var count = 0;
                                var insertPoliza = "INSERT INTO sc_polizas (year ,mes ,poliza ,tp ,fechapol ,cargos ,abonos ,generada ,status ,status_lock ,fec_hora_movto ,usuario_movto ,fecha_hora_crea ,usuario_crea ,concepto ,error) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
                                using (var cmd = new OdbcCommand(insertPoliza))
                                {
                                    OdbcParameterCollection parameters = cmd.Parameters;
                                    parameters.Clear();

                                    decimal cargos = 0;
                                    decimal abonos = 0;

                                    //foreach (var item in movpol)
                                    //{
                                    //    if (item.tm == 1)
                                    //    {
                                    //        cargos += item.monto;
                                    //    }
                                    //    if (item.tm == 2)
                                    //    {
                                    //        decimal montoT = 0;
                                    //        if (item.monto > 0)
                                    //            montoT = item.monto * (-1);
                                    //        else
                                    //            montoT = item.monto;
                                    //        abonos += montoT;
                                    //    }
                                    //}
                                    //
                                    cargos = Math.Round(poliza.cargos, 2);
                                    abonos = Math.Round(poliza.abonos, 2);
                                    //

                                    var empleadoEko = _context.tblP_Usuario_Enkontrol.FirstOrDefault(r => r.idUsuario == vSesiones.sesionUsuarioDTO.id);

                                    mensajeErrorExtra = string.Format(" Al registrar poliza: {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}",
                                    poliza.year, poliza.mes, poliza.poliza, poliza.tp, poliza.fechapol, poliza.cargos, poliza.abonos, poliza.generada, empleadoEko.empleado, empleadoEko.sn_empleado);

                                    parameters.Add("@year", OdbcType.Numeric).Value = poliza.year;
                                    parameters.Add("@mes", OdbcType.Numeric).Value = poliza.mes;
                                    parameters.Add("@poliza", OdbcType.Numeric).Value = poliza.poliza;
                                    parameters.Add("@tp", OdbcType.Char).Value = poliza.tp;
                                    parameters.Add("@fechapol", OdbcType.Date).Value = poliza.fechapol;
                                    parameters.Add("@cargos", OdbcType.Numeric).Value = cargos;
                                    parameters.Add("@abonos", OdbcType.Numeric).Value = abonos;
                                    parameters.Add("@generada", OdbcType.Char).Value = poliza.generada;
                                    parameters.Add("@status", OdbcType.Char).Value = 'C';
                                    parameters.Add("@status_lock", OdbcType.Char).Value = 'N';
                                    parameters.Add("@fec_hora_movto", OdbcType.DateTime).Value = DateTime.Now;
                                    parameters.Add("@usuario_movto", OdbcType.Char).Value = empleadoEko.empleado;
                                    parameters.Add("@fecha_hora_crea", OdbcType.DateTime).Value = DateTime.Now;
                                    parameters.Add("@usuario_crea", OdbcType.Char).Value = empleadoEko.sn_empleado;
                                    parameters.Add("@concepto", OdbcType.VarChar).Value = poliza.concepto;
                                    parameters.Add("@error", OdbcType.VarChar).Value = string.Empty;
                                    parameters.Add("@status_carga_pol", OdbcType.VarChar).Value = DBNull.Value;

                                    cmd.Connection = trans.Connection;
                                    cmd.Transaction = trans;
                                    count += cmd.ExecuteNonQuery();
                                }

                                foreach (var objMovPol1 in movpol)
                                {
                                    var insertMovpol = @"INSERT INTO sc_movpol 
                                                        (year
                                                        ,mes
                                                        ,poliza
                                                        ,tp
                                                        ,linea
                                                        ,cta
                                                        ,scta
                                                        ,sscta
                                                        ,digito
                                                        ,tm
                                                        ,referencia
                                                        ,cc
                                                        ,concepto
                                                        ,monto
                                                        ,iclave
                                                        ,itm
                                                        ,st_par
                                                        ,orden_compra
                                                        ,numpro
                                                        ,area
                                                        ,cuenta_oc)
                                                VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                                    using (var cmd = new OdbcCommand(insertMovpol))
                                    {
                                        OdbcParameterCollection parameters = cmd.Parameters;
                                        parameters.Clear();
                                        decimal monto = 0;
                                        if (objMovPol1.tm == 1)
                                            monto = objMovPol1.monto;
                                        else
                                        {
                                            if (objMovPol1.monto > 0)
                                                monto = objMovPol1.monto * (-1);
                                            else
                                                monto = objMovPol1.monto;
                                        }

                                        mensajeErrorExtra = string.Format(" Al registrar poliza: {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}",
                                            objMovPol1.year, objMovPol1.mes, poliza.poliza, objMovPol1.linea, objMovPol1.cta, objMovPol1.scta, objMovPol1.sscta, objMovPol1.digito, objMovPol1.tm, objMovPol1.referencia,
                                            objMovPol1.cc, objMovPol1.concepto, monto, objMovPol1.itm, objMovPol1.numpro, objMovPol1.area, objMovPol1.cuenta_oc);

                                        parameters.Add("@year", OdbcType.Numeric).Value = objMovPol1.year;
                                        parameters.Add("@mes", OdbcType.Numeric).Value = objMovPol1.mes;
                                        parameters.Add("@poliza", OdbcType.Numeric).Value = poliza.poliza;
                                        parameters.Add("@tp", OdbcType.Char).Value = "03";
                                        parameters.Add("@linea", OdbcType.Numeric).Value = objMovPol1.linea;
                                        parameters.Add("@cta", OdbcType.Numeric).Value = objMovPol1.cta;
                                        parameters.Add("@scta", OdbcType.Numeric).Value = objMovPol1.scta;
                                        parameters.Add("@sscta", OdbcType.Numeric).Value = objMovPol1.sscta;
                                        parameters.Add("@digito", OdbcType.Numeric).Value = objMovPol1.digito;
                                        parameters.Add("@tm", OdbcType.Numeric).Value = objMovPol1.tm;
                                        parameters.Add("@referencia", OdbcType.Char).Value = objMovPol1.referencia;
                                        parameters.Add("@cc", OdbcType.Char).Value = objMovPol1.cc;
                                        parameters.Add("@concepto", OdbcType.Char).Value = objMovPol1.concepto;
                                        parameters.Add("@monto", OdbcType.Numeric).Value = Math.Round(monto, 2);
                                        parameters.Add("@iclave", OdbcType.Numeric).Value = 0;
                                        parameters.Add("@itm", OdbcType.Numeric).Value = objMovPol1.itm;
                                        parameters.Add("@st_par", OdbcType.Char).Value = string.Empty;
                                        parameters.Add("@orden_compra", OdbcType.Numeric).Value = 0;
                                        parameters.Add("@numpro", OdbcType.Numeric).Value = objMovPol1.numpro;
                                        parameters.Add("@area", OdbcType.Numeric).Value = objMovPol1.area;
                                        parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = objMovPol1.cuenta_oc;
                                        cmd.Connection = trans.Connection;
                                        cmd.Transaction = trans;
                                        count += cmd.ExecuteNonQuery();
                                    }
                                }

                                var num_concilia = 0;

                                foreach (var mov in movpol)
                                {
                                    var relInstCuenta = _context.tblAF_DxP_RelInstitucionCta.FirstOrDefault
                                        (f =>
                                            f.institucionID == _contrato.InstitucionId &&
                                            f.activo &&
                                            f.cta == mov.cta &&
                                            f.scta == mov.scta &&
                                            f.sscta == mov.sscta
                                        );
                                    var relInsCuentaScta = _context.tblAF_DxP_RelInstitucionCta.FirstOrDefault
                                        (f =>
                                            f.institucionID == _contrato.InstitucionId &&
                                            f.activo &&
                                            f.cta == mov.cta &&
                                            f.moneda == _contrato.monedaContrato &&
                                            !f.complementaria
                                        );

                                    if (relInstCuenta != null)
                                    {
                                        var odbcCuenta = new OdbcConsultaDTO();

                                        odbcCuenta.consulta = "SELECT cuenta, descripcion, banco, moneda FROM sb_cuenta WHERE cta = ? AND scta = ? AND sscta = ?";

                                        odbcCuenta.parametros.Add(new OdbcParameterDTO
                                        {
                                            nombre = "cta",
                                            tipo = OdbcType.Int,
                                            valor = relInstCuenta.cta
                                        });
                                        odbcCuenta.parametros.Add(new OdbcParameterDTO
                                        {
                                            nombre = "scta",
                                            tipo = OdbcType.Int,
                                            valor = relInstCuenta.scta
                                        });
                                        odbcCuenta.parametros.Add(new OdbcParameterDTO
                                        {
                                            nombre = "sscta",
                                            tipo = OdbcType.Int,
                                            valor = relInstCuenta.sscta
                                        });

                                        var sb_cuenta = _contextEnkontrol.Select<sb_cuentaDTO>(productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, odbcCuenta).FirstOrDefault();

                                        if (sb_cuenta != null)
                                        {
                                            if (num_concilia == 0)
                                                {
                                                    var odbcConcialia = new OdbcConsultaDTO();

                                                    odbcConcialia.consulta = "SELECT DISTINCT num_concilia from sb_movtran_chequera where cuenta = ? order by num_concilia desc";

                                                    odbcConcialia.parametros.Add(new OdbcParameterDTO
                                                    {
                                                        nombre = "cuenta",
                                                        tipo = OdbcType.Int,
                                                        valor = sb_cuenta.cuenta
                                                    });

                                                    var _numC = _contextEnkontrol.Select<Core.DTO.Administracion.DocumentosXPagar.sb_edo_cta_chequeraDTO>(productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, odbcConcialia).FirstOrDefault();

                                                    if (_numC != null)
                                                    {
                                                        num_concilia = _numC.num_concilia + 1;
                                                    }
                                                    else
                                                    {
                                                        num_concilia = 1;
                                                        //resultado.Add(SUCCESS, false);
                                                        //resultado.Add(MESSAGE, "Error al obtener el número de conciliación");

                                                        //dbTransaction.Rollback();
                                                        //trans.Rollback();

                                                        //return resultado;
                                                    }
                                                }


                                                var insert_edo_cta_chequera =
                                                    @"INSERT INTO sb_edo_cta_chequera
                                                    (
                                                        cuenta, fecha_mov, tm, numero, cc, descripcion, monto, tc, origen_mov, generada, iyear, imes, ipoliza, itp, ilinea, banco
                                                    )
                                                    VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

                                                mensajeErrorExtra = string.Format(" Al registrar chequera: {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}",
                                                    sb_cuenta.cuenta, poliza.fechapol, mov.itm, mov.referencia, mov.cc, mov.concepto, mov.monto, !relInstCuenta.complementaria ? _tipoCambio : 1M, mov.year, mov.mes,
                                                    mov.poliza, mov.tp, mov.linea, sb_cuenta.banco);

                                                using (var cmd = new OdbcCommand(insert_edo_cta_chequera))
                                                {
                                                    OdbcParameterCollection parameters = cmd.Parameters;
                                                    parameters.Clear();

                                                    parameters.Add("@cuenta", OdbcType.Numeric).Value = sb_cuenta.cuenta;
                                                    parameters.Add("@fecha_mov", OdbcType.DateTime).Value = poliza.fechapol;
                                                    parameters.Add("@tm", OdbcType.Numeric).Value = mov.itm;
                                                    parameters.Add("@numero", OdbcType.Numeric).Value = mov.referencia;
                                                    parameters.Add("@cc", OdbcType.VarChar).Value = mov.cc;
                                                    parameters.Add("@descripcion", OdbcType.VarChar).Value = mov.concepto;
                                                    parameters.Add("@monto", OdbcType.Decimal).Value = Math.Round(mov.monto, 2);
                                                    parameters.Add("@tc", OdbcType.Decimal).Value = !relInstCuenta.complementaria ? _tipoCambio : 1M;
                                                    parameters.Add("@origen_mov", OdbcType.Char).Value = 'C';
                                                    parameters.Add("@generada", OdbcType.Char).Value = 'C';
                                                    parameters.Add("@iyear", OdbcType.Numeric).Value = mov.year;
                                                    parameters.Add("@imes", OdbcType.Numeric).Value = mov.mes;
                                                    parameters.Add("@ipoliza", OdbcType.Numeric).Value = mov.poliza;
                                                    parameters.Add("@itp", OdbcType.VarChar).Value = mov.tp;
                                                    parameters.Add("@ilinea", OdbcType.Numeric).Value = mov.linea;
                                                    parameters.Add("@banco", OdbcType.Numeric).Value = sb_cuenta.banco;
                                                    parameters.Add("@num_consilia", OdbcType.Int).Value = 0;
                                                    parameters.Add("@ref_che_inverso", OdbcType.Int).Value = 0;
                                                    parameters.Add("@ref_tm_inverso", OdbcType.Int).Value = 0;

                                                    cmd.Connection = trans.Connection;
                                                    cmd.Transaction = trans;
                                                    cmd.ExecuteNonQuery();
                                                }

                                                if (!relInstCuenta.complementaria)
                                                {
                                                    //                                                var insert_movtran_chequera =
                                                    //                                                @"INSERT INTO sb_movtran_chequera
                                                    //                                                    (
                                                    //                                                        banco, cuenta, fecha_mov, tm, concepto, numero, monto, st_concilia, num_concilia
                                                    //                                                    )
                                                    //                                                    VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";

                                                    //                                                using (var cmd = new OdbcCommand(insert_movtran_chequera))
                                                    //                                                {
                                                    //                                                    OdbcParameterCollection parameters = cmd.Parameters;
                                                    //                                                    parameters.Clear();

                                                    //                                                    parameters.Add("@banco", OdbcType.Numeric).Value = sb_cuenta.banco;
                                                    //                                                    parameters.Add("@cuenta", OdbcType.Numeric).Value = sb_cuenta.cuenta;
                                                    //                                                    parameters.Add("@fecha_mov", OdbcType.DateTime).Value = poliza.fechapol;
                                                    //                                                    parameters.Add("@tm", OdbcType.Numeric).Value = relInsCuentaScta.scta;
                                                    //                                                    parameters.Add("@concepto", OdbcType.VarChar).Value = mov.concepto;
                                                    //                                                    parameters.Add("@numero", OdbcType.Numeric).Value = mov.referencia;
                                                    //                                                    parameters.Add("@monto", OdbcType.Decimal).Value = Math.Round(mov.monto, 2);
                                                    //                                                    parameters.Add("@st_concilia", OdbcType.VarChar).Value = "N";
                                                    //                                                    parameters.Add("@num_concilia", OdbcType.Numeric).Value = num_concilia;

                                                    //                                                    cmd.Connection = trans.Connection;
                                                    //                                                    cmd.Transaction = trans;
                                                    //                                                    cmd.ExecuteNonQuery();
                                                    //                                                }
                                                }
                                        }
                                        else
                                        {
                                            resultado.Add(SUCCESS, false);
                                            resultado.Add(MESSAGE, "Error al obtener el número de cuenta banco");

                                            dbTransaction.Rollback();
                                            trans.Rollback();

                                            return resultado;
                                        }
                                    }
                                }

                                if (_numPoliza > 0)
                                {
                                    resultado.Add("poliza", _numPoliza);
                                }

                                dbTransaction.Commit();
                                trans.Commit();
                            }
                            catch (Exception e)
                            {
                                trans.Rollback();
                                dbTransaction.Rollback();
                                
                                LogError(2, 0, "ContratosController", "guardarPoliza", e, AccionEnum.AGREGAR, 0, mensajeErrorExtra);

                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "Ocurrió al insertar poliza en enkontrol.");
                                return resultado;
                            }
                        }
                    }

                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    LogError(2, 0, "ContratosController", "guardarPoliza", e, AccionEnum.AGREGAR, 0, mensajeErrorExtra);

                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    dbTransaction.Rollback();
                }
                return resultado;
            }
        }
        public Dictionary<string, object> GetProveedores()
        {
            Dictionary<string, object> Resultado = new Dictionary<string, object>();
            try
            {

                var odbc = new OdbcConsultaDTO();
                odbc.consulta = @"SELECT  A.rfc AS Value,A.nombre AS Text, A.nomcorto AS Prefijo
                                    FROM sp_proveedores A
                                  UNION
                                  SELECT  B.rfc AS Value,B.nombre AS Text, B.nomcorto AS Prefijo
                                    FROM sx_clientes B";
                List<ComboDTO> listaProveedores = _contextEnkontrol.Select<ComboDTO>(productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, odbc);
                Resultado.Add("listaObj", listaProveedores);
                Resultado.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                Resultado.Add(SUCCESS, false);
                Resultado.Add(MESSAGE, "Ocurrió un error, al momento de cargar la información de proveedores en enkontrol.");
            }

            return Resultado;
        }
        #endregion
        #region What IF
        public List<tblC_FED_DetProyeccionCierre> getLstContratos(BusqProyeccionCierreDTO busq)
        {
            try
            {
                var anio = busq.max.Year;
                var semana = busq.max.noSemana();
                var lstContratoMaq = (from maq in _context.tblAF_DxP_ContratoMaquinas
                                      where maq.Contrato.Estatus && maq.Contrato.FechaVencimiento.HasValue
                                      select maq).ToList();
                var lstDetCierre = (from maq in lstContratoMaq
                                    select new tblC_FED_DetProyeccionCierre
                                    {
                                        anio = anio,
                                        semana = semana,
                                        idConceptoDir = 27,
                                        tipo = tipoProyeccionCierreEnum.DocPorPagar,
                                        ac = maq.Maquina.centro_costos,
                                        cc = "TODOS",
                                        descripcion = string.Format("{0} {1}", maq.Contrato.Folio, maq.Contrato.Descripcion),
                                        fechaFactura = maq.Contrato.FechaVencimiento.Value,
                                        fecha = maq.Contrato.FechaInicio,
                                        naturaleza = naturalezaEnum.Egreso,
                                        monto = maq.Contrato.Credito,
                                    }).ToList();
                return lstDetCierre;
            }
            catch (Exception o_O)
            {
                return new List<tblC_FED_DetProyeccionCierre>();
            }
        }

        public Dictionary<string, object> LoadContratosProgramados(DateTime fechaInicio, DateTime fechaFin, string cc)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {

                var contratoLista = _context.tblAF_DxP_ProgramacionPagos
                                           .Where(r => r.aplicado == 0 && !r.aplicaPropuesta).ToList()
                                           .Where(p => p.fechaCaptura.Date >= fechaInicio.Date && p.fechaCaptura.Date <= fechaFin.Date)
                                           .Where(r => (cc == "TODOS" ? true : r.ac == cc))
                                            .Select(c => new
                                            {
                                                id = c.id,
                                                contratoid = c.contratoid,
                                                parcialidad = c.parcialidad,
                                                rfc = c.rfc,
                                                noEconomico = c.noEconomico,
                                                cc = c.cc,
                                                mensualidad = c.mensualidad,
                                                financiamiento = c.financiamiento,
                                                fechaVencimiento = c.fechaVencimiento,
                                                contrato = c.contrato,
                                                ac = c.ac,
                                                capital = c.capital,
                                                intereses = c.intereses,
                                                iva = c.iva,
                                                importe = c.importe,
                                                porcentaje = c.porcentaje,
                                                importeDLLS = c.importeDLLS,
                                                tipoCambio = c.tipoCambio,
                                                ivaInteres = c.ivaInteres,
                                                total = c.total,
                                                programado = false
                                            });


                resultado.Add("contratoLista", contratoLista);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);

            }
            return resultado;
        }

        public Dictionary<string, object> LoadContratosProgramadosCplan(DateTime fechaInicio, DateTime fechaFin, string cc)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {

                var contratoLista = _context.tblAF_DxP_ProgramacionPagos
                                           .Where(r => r.aplicado == 0 && !r.aplicaPropuesta).ToList()
                                           .Where(p => p.fechaCaptura.Date >= fechaInicio.Date && p.fechaCaptura.Date <= fechaFin.Date)
                                           .Where(r => (cc == "TODOS" ? true : r.cc == cc))
                                            .Select(c => new
                                            {
                                                id = c.id,
                                                contratoid = c.contratoid,
                                                parcialidad = c.parcialidad,
                                                rfc = c.rfc,
                                                noEconomico = c.noEconomico,
                                                cc = c.cc,
                                                mensualidad = c.mensualidad,
                                                financiamiento = c.financiamiento,
                                                fechaVencimiento = c.fechaVencimiento,
                                                contrato = c.contrato,
                                                ac = c.ac,
                                                capital = c.capital,
                                                intereses = c.intereses,
                                                iva = c.iva,
                                                importe = c.importe,
                                                porcentaje = c.porcentaje,
                                                importeDLLS = c.importeDLLS,
                                                tipoCambio = c.tipoCambio,
                                                ivaInteres = c.ivaInteres,
                                                total = c.total,
                                                programado = false
                                            });


                resultado.Add("contratoLista", contratoLista);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);

            }
            return resultado;
        }
        #endregion
        #region Reporte Propuesta Arrendadora

        public List<PropuestaArrendadoraDTO> LoadReportePropuestaArrendadora(DateTime pfechaInicio, DateTime pfechaFin)
        {
            List<PropuestaArrendadoraDTO> objResult = new List<PropuestaArrendadoraDTO>();
            try
            {
                objResult = (from pp in _context.tblAF_DxP_ProgramacionPagos
                             join c in _context.tblAF_DxP_ContratoMaquinasDetalle
                             on pp.contratoid equals c.ContratoDetalle.ContratoId
                             where pp.parcialidad == c.Parcialidad
                             select new PropuestaArrendadoraDTO
                             {
                                 rfc = pp.rfc,
                                 noEconomico = pp.noEconomico,
                                 cc = pp.cc,
                                 mensualidad = pp.parcialidad + "/" + c.ContratoMaquina.Contrato.Plazo,
                                 financiamiento = pp.financiamiento,
                                 fechaVencimiento = pp.fechaVencimiento,
                                 noContrato = pp.contrato,
                                 areaCuenta = pp.ac,
                                 capital = pp.capital,
                                 ivaIntereses = c.ContratoDetalle.IvaIntereses,
                                 intereses = pp.intereses,
                                 iva = pp.iva,
                                 importe = pp.importe,
                                 importeDLLS = pp.importeDLLS,
                                 porcentaje = pp.porcentaje,
                                 total = pp.total,
                                 tipoCambio = pp.tipoCambio
                             }).Distinct().ToList();
            }
            catch (Exception)
            {

            }
            return objResult;
        }

        public Dictionary<string, object> ActualizarContratos(List<tblAF_DxP_ProgramacionPagos> arrayProgramacionID)
        {
            Dictionary<string, object> Resultado = new Dictionary<string, object>();
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {

                    foreach (var item in arrayProgramacionID)
                    {
                        var updateContratos = _context.tblAF_DxP_ProgramacionPagos.FirstOrDefault(r => item.id == r.id);
                        updateContratos.aplicaPropuesta = true;
                        _context.SaveChanges();
                    }

                    Resultado.Add(SUCCESS, true);
                    trans.Commit();
                }
                catch (Exception e)
                {
                    Resultado.Add(SUCCESS, false);
                    Resultado.Add(MESSAGE, e.Message);
                    trans.Rollback();

                }
            }
            return Resultado;
        }
        private string EconomicoCC(string economico)
        {
            try
            {

                var odbc = new OdbcConsultaDTO();
                odbc.consulta = @"SELECT A.cc AS Value,A.descripcion AS Text
                                  FROM cc A WHERE A.descripcion IN ('" + economico.TrimEnd(',') + "')";
                List<ComboDTO> listaEconomicosCC = _contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.ArrenProd, odbc);
                return listaEconomicosCC.FirstOrDefault().Value;

            }
            catch (Exception)
            {
                return "";
            }
        }

        public Dictionary<string, object> LoadProgramacionPagos(DateTime pInicio, DateTime pFinal, int pEstatus, List<int> institucion, int empresa, int moneda)
        {
            Dictionary<string, object> Resultado = new Dictionary<string, object>();
            try
            {

                switch (pEstatus)
                {
                    case 0:
                        {
                            var empresaContext = vSesiones.sesionEmpresaActual;

                            List<ComboDTO> listaEconomicosCC = new List<ComboDTO>();

                            var auxProgramacionPagos = _context.tblAF_DxP_ProgramacionPagos.Where(c => c.fechaVencimiento <= pFinal && c.aplicado == 0).ToList();

                            var programacionPagos = auxProgramacionPagos.Select(x => x.contratoid + "-" + x.parcialidad).ToList();

                            var pendientesPago = _context.tblAF_DxP_ContratoMaquinasDetalle
                                .Where(x =>
                                    institucion.Contains(x.ContratoMaquina.Contrato.InstitucionId) &&
                                    !x.Pagado &&
                                    (empresa == 0 ? true : x.ContratoMaquina.Contrato.empresa == empresa) &&
                                    !x.ContratoMaquina.Contrato.Terminado &&
                                    x.FechaVencimiento <= pFinal &&
                                    x.ContratoMaquina.Contrato.monedaContrato == moneda &&
                                    !programacionPagos.Contains(x.ContratoDetalle.ContratoId + "-" + x.Parcialidad)).ToList()
                                .Select(c =>
                                {
                                    var programacion = auxProgramacionPagos.FirstOrDefault(y => y.contratoid == c.Id);
                                    var opcionCompra = programacion == null ? false : programacion.opcionCompra;
                                    return new PagosPendientesDTO
                                        {
                                            contratoid = c.ContratoDetalle.ContratoId,
                                            parcialidad = c.Parcialidad,
                                            rfc = c.ContratoDetalle.Contrato.rfc,
                                            noEconomico = c.ContratoMaquina.Maquina.noEconomico,
                                            mensualidad = c.Parcialidad + "/" + c.ContratoMaquina.Contrato.Plazo,
                                            financiamiento = c.ContratoMaquina.Contrato.nombreCorto,
                                            fechaVencimiento = c.FechaVencimiento,
                                            contrato = c.ContratoMaquina.Contrato.Folio,
                                            capital = Math.Round(c.ContratoDetalle.AmortizacionCapital, 2),
                                            intereses = Math.Round(c.ContratoDetalle.Intereses, 2),
                                            iva = Math.Round(c.ContratoDetalle.IvaSCapital, 2),
                                            ivaInteres = Math.Round(c.ContratoDetalle.IvaIntereses, 2),
                                            tipoCambio = Math.Round(c.ContratoMaquina.Contrato.TipoCambio, 4),
                                            programado = 1,
                                            porcentaje = c.ContratoMaquina.porcentaje,
                                            acDescripcion = c.ContratoMaquina.Maquina.centro_costos,
                                            moneda = c.ContratoMaquina.Contrato.monedaContrato,
                                            empresa = c.ContratoMaquina.Contrato.empresa,
                                            maquinaId = c.ContratoMaquina.Maquina.id,
                                            maquinaCentroCostos = c.ContratoMaquina.Maquina.estatus != 1 ? "14-1" : c.ContratoMaquina.Maquina.centro_costos,
                                            detalle = c.ContratoDetalle,
                                            penaConvencional = 0M,
                                            montoOpcionCompra = opcionCompra ? c.ContratoDetalle.Contrato.montoOpcioncompra : 0
                                        };
                                }).ToList();

                            if (empresaContext == 1)
                            {
                                var maquinasId = pendientesPago.Select(x => x.maquinaId).ToList();

                                using (var ctx = new MainContext(EmpresaEnum.Arrendadora))
                                {
                                    _maquinas = ctx.tblM_CatMaquina
                                        .Where(x =>
                                            maquinasId.Contains(x.id) &&
                                            x.estatus == 1 &&
                                            !acVirtuales.Contains(x.centro_costos) &&
                                            !string.IsNullOrEmpty(x.centro_costos)).ToList();
                                }

                                var maquinasCC = _maquinas.Select(x => x.centro_costos).ToList();

                                _ccs = _context.tblP_CC.Where(x => maquinasCC.Contains(x.areaCuenta)).ToList();
                            }
                            else
                            {
                                var odbc = new OdbcConsultaDTO();
                                odbc.consulta = @"SELECT cc AS Value, descripcion AS Text FROM cc";
                                listaEconomicosCC = _contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.ArrenProd, odbc);
                            }

                            foreach (var item in pendientesPago)
                            {
                                item.importe = Math.Round(getImporte(item.detalle) * (item.porcentaje / 100), 2);
                                item.importeDLLS = Math.Round((item.moneda == 1 ? 0 : getImporte(item.detalle) * (item.porcentaje / 100)), 2);
                                item.total = item.moneda == 1 ? Math.Round((getImporte(item.detalle) * (item.porcentaje / 100)), 2) : Math.Round(getImporte(item.detalle) * (item.porcentaje / 100) * item.tipoCambio, 2);
                                item.cc = (empresaContext == 1 ? getCCForAC(item.maquinaId) : listaEconomicosCC.First(x => x.Text.ToUpper() == item.noEconomico.ToUpper()).Value);
                                item.areaCuenta = (empresaContext == 1 ? "0-0" : getACInvalid(item.maquinaCentroCostos));
                                item.detalle = null;
                            }

                            //foreach (var item in pendientesPago)
                            //{
                            //    decimal tipoCambio = item.tipoCambio > 0 ? item.tipoCambio : 0;
                            //    decimal importeDLLS = item.importeDLLS > 0 ? item.importeDLLS : 0;

                            //    if ((decimal)tipoCambio > 0 && (decimal)importeDLLS > 0)
                            //        item.importeDLLS = ((decimal)tipoCambio * (decimal)importeDLLS);
                            //}

                            HttpContext.Current.Session["rptContratoLista"] = pendientesPago;

                            // SANDBOX
                            //PagosPendientesDTO obj = new PagosPendientesDTO();
                            //obj = pendientesPago.FirstOrDefault();
                            //pendientesPago.Add(obj);
                            // END: SANDBOX

                            Resultado.Add("contratoLista", pendientesPago);
                            Resultado.Add(SUCCESS, true);
                        }
                        break;
                    case 1:
                        {

                            var listaInstituciones = _context.tblAF_DxP_Contratos.Where(r => institucion.Contains(r.InstitucionId) && !r.Terminado).Select(r => r.Id).ToList();

                            var contratoLista = _context.tblAF_DxP_ProgramacionPagos
                                                .Where(r => r.aplicado == 0 && (empresa == 0 ? true : r.empresa == empresa)).ToList()
                                                .Where(p => p.fechaCaptura.Date >= pInicio.Date && p.fechaCaptura.Date <= pFinal.Date)
                                                .Where(r => moneda == 1 ? r.tipoCambio == 1 : r.tipoCambio != 1)
                                                .Where(r => listaInstituciones.Contains(r.contratoid))
                                                 .Select(c => new
                                                 {
                                                     contratoid = c.contratoid,
                                                     parcialidad = c.parcialidad,
                                                     rfc = c.rfc,
                                                     noEconomico = c.noEconomico,
                                                     cc = vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora ? getCCInvalid(c.cc) : c.cc,
                                                     mensualidad = c.mensualidad,
                                                     financiamiento = c.financiamiento,
                                                     fechaVencimiento = c.fechaVencimiento,
                                                     contrato = c.contrato,
                                                     ac = vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora ? "0-0" : getACInvalid(c.ac),
                                                     capital = c.capital, //* (c.porcentaje / 100),
                                                     intereses = c.intereses,//* (c.porcentaje / 100),
                                                     iva = c.iva,//* (c.porcentaje / 100),
                                                     importe = c.importe,//* (c.porcentaje / 100),
                                                     porcentaje = c.porcentaje,
                                                     importeDLLS = c.importeDLLS,
                                                     tipoCambio = c.tipoCambio,
                                                     ivaInteres = c.ivaInteres,
                                                     total = c.total, // c.importe * c.tipoCambio * (c.porcentaje / 100),  //c.total,// c.ContratoMaquina.Contrato.monedaContrato == 1 ? (c.ContratoDetalle.AmortizacionCapital + c.ContratoDetalle.Intereses + c.ContratoDetalle.IvaSCapital) * c.ContratoMaquina.porcentaje : (c.ContratoDetalle.AmortizacionCapital + c.ContratoDetalle.Intereses + c.ContratoDetalle.IvaSCapital) * c.ContratoMaquina.porcentaje,
                                                     programado = 2
                                                 });

                            HttpContext.Current.Session["rptContratoLista"] = contratoLista;
                            Resultado.Add("contratoLista", contratoLista);
                            Resultado.Add(SUCCESS, true);
                        }
                        break;
                    case 2:
                        {
                            var contratoLista = _context.tblAF_DxP_ProgramacionPagos
                                               .Where(r => r.aplicado == 2 && (empresa == 0 ? true : r.empresa == empresa)).ToList()
                                               .Where(p => p.fechaCaptura.Date >= pInicio.Date && p.fechaCaptura.Date <= pFinal.Date)
                                               .Where(r => moneda == 1 ? r.tipoCambio == 1 : r.tipoCambio != 1)
                                                 .Select(c => new
                                                 {
                                                     contratoid = c.contratoid,
                                                     parcialidad = c.parcialidad,
                                                     rfc = c.rfc,
                                                     noEconomico = c.noEconomico,
                                                     cc = c.cc,
                                                     mensualidad = c.mensualidad,
                                                     financiamiento = c.financiamiento,
                                                     fechaVencimiento = c.fechaVencimiento,
                                                     contrato = c.contrato,
                                                     ac = vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora ? "0-0" : getACInvalid(c.ac),
                                                     capital = c.capital,
                                                     intereses = c.intereses,
                                                     iva = c.iva,
                                                     importe = c.importe,
                                                     porcentaje = c.porcentaje,
                                                     importeDLLS = c.importeDLLS,
                                                     tipoCambio = c.tipoCambio,
                                                     total = c.total,// c.ContratoMaquina.Contrato.monedaContrato == 1 ? (c.ContratoDetalle.AmortizacionCapital + c.ContratoDetalle.Intereses + c.ContratoDetalle.IvaSCapital) * c.ContratoMaquina.porcentaje : (c.ContratoDetalle.AmortizacionCapital + c.ContratoDetalle.Intereses + c.ContratoDetalle.IvaSCapital) * c.ContratoMaquina.porcentaje,
                                                     programado = 2
                                                 });


                            HttpContext.Current.Session["rptContratoLista"] = contratoLista;
                            Resultado.Add("contratoLista", contratoLista);
                            Resultado.Add(SUCCESS, true);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Resultado.Add(SUCCESS, false);
                Resultado.Add(MESSAGE, "Ocurrió un error, error al cargar la informacion de propuesta.");
            }
            return Resultado;
        }

        public Dictionary<string, object> GetInfoLiquidar(bool liquidar, int contratoId, int parcialidad)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var pagosPendientes = _context.tblAF_DxP_ContratosDetalle
                    .Where(x =>
                        x.ContratoId == contratoId &&
                        !x.Pagado).ToList();

                if (pagosPendientes.Count > 0)
                {
                    var info = new PagosPendientesDTO();

                    if (liquidar)
                    {
                        info.capital = Math.Round(pagosPendientes.Sum(x => x.AmortizacionCapital), 2);
                        info.iva = Math.Round(pagosPendientes.Sum(x => x.IvaSCapital), 2);
                        info.intereses = Math.Round(pagosPendientes.Sum(x => x.Intereses), 2);
                        info.ivaInteres = Math.Round(pagosPendientes.Sum(x => x.IvaIntereses), 2);
                        info.importe = Math.Round(info.capital + info.iva + info.intereses + info.ivaInteres, 2);
                        info.importeDLLS = pagosPendientes.First().Contrato.monedaContrato == 1 ? 0 : info.importe;
                    }
                    else
                    {
                        var pagoPendiente = pagosPendientes.FirstOrDefault(x => x.Parcialidad == parcialidad);

                        info.capital = Math.Round(pagoPendiente.AmortizacionCapital, 2);
                        info.iva = Math.Round(pagoPendiente.IvaSCapital, 2);
                        info.intereses = Math.Round(pagoPendiente.Intereses, 2);
                        info.ivaInteres = Math.Round(pagoPendiente.IvaIntereses, 2);
                        info.importe = Math.Round(info.capital + info.iva + info.intereses + info.ivaInteres, 2);
                        info.importeDLLS = pagoPendiente.Contrato.monedaContrato == 1 ? 0 : info.importe;
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, info);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "No se encontró información");
                }
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, ex.Message);
            }

            return resultado;
        }

        private string getCCInvalid(string cc)
        {
            if (!string.IsNullOrEmpty(cc))
            {
                switch (cc)
                {
                    case "0":
                        return "997";
                    default:
                        return cc;
                }
            }
            else
            {
                return "997";
            }
        }

        private string getACInvalid(string ac)
        {
            try
            {
                if (!string.IsNullOrEmpty(ac))
                {
                    switch (ac)
                    {
                        case "0":
                        case "1010":
                        case "1018":
                        case "1015":
                        case "997":
                            return "14-1";
                        default:
                            return ac;
                    }
                }
                else
                    return "14-1";
            }
            catch (Exception e)
            {

                return "14-1";
            }
        }

        private string getCCForAC(int economico)
        {
            try
            {
                //MainContext mc = new MainContext(EmpresaEnum.Arrendadora);
                //var maquina = mc.tblM_CatMaquina.FirstOrDefault(r => r.id == economico && r.estatus == 1 && !acVirtuales.Contains(r.centro_costos) && !string.IsNullOrEmpty(r.centro_costos));
                var maquina = _maquinas.FirstOrDefault(x => x.id == economico && x.estatus == 1);
                if (maquina != null)
                {
                    //var centroCostos = _context.tblP_CC.FirstOrDefault(r => r.areaCuenta == maquina.centro_costos);
                    var centroCostos = _ccs.FirstOrDefault(x => x.areaCuenta == maquina.centro_costos);
                    if (centroCostos != null)
                    {
                        return centroCostos.cc;
                    }
                    else
                    {

                    }
                    return "997";
                }
                else
                {
                    if (economico == 100)
                    {
                        return "990";
                    }
                    return "997";
                }
            }
            catch (Exception)
            {
                return "";
            }

        }

        private decimal getImporte(tblAF_DxP_ContratoDetalle obj)
        {
            return obj.AmortizacionCapital + obj.Intereses + obj.IvaSCapital + obj.IvaIntereses;
        }

        private string getEconomicosContrato(int p)
        {
            string economicos = "";

            _context.tblAF_DxP_ContratoMaquinas.Where(r => r.ContratoId == p).ToList().ForEach(r =>
            {
                economicos += r.Maquina.noEconomico + ",";
            });

            return economicos.TrimEnd(',');
        }
        #endregion
        public ComboDTO getArchivoDownLoad(int contratoID)
        {
            ComboDTO resultado = _context.tblAF_DxP_Contratos.Where(r => r.Id == contratoID).Select(r => new ComboDTO
            {
                Id = r.Folio,
                Text = r.FileContrato
            }).FirstOrDefault();

            return resultado;
        }
        #region Reporte de Adedudo

        public Dictionary<string, object> getRptAdeudosGeneral(List<int> tipoMoneda, List<int> anio, List<int> instituciones, bool tipoArrendamiento)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                List<MesesAdeudo> tablaAduedo = new List<MesesAdeudo>();
                int maxYear = anio.Max();
                int minYear = anio.Min();

                var rawDataCompleta = _context.tblAF_DxP_ContratosDetalle.Where(r =>

                    instituciones.Contains(r.Contrato.InstitucionId) &&
                    tipoMoneda.Contains(r.Contrato.monedaContrato) &&
                    r.Contrato.arrendamientoPuro == tipoArrendamiento

                    ).ToList();

                var rawDataFiltrada = rawDataCompleta.Where(r => r.FechaVencimiento.Year >= minYear && r.FechaVencimiento.Year <= maxYear).OrderBy(r => r.Contrato.InstitucionId).ThenBy(f => f.FechaVencimiento);
                var rawSaldosAnteriores = rawDataCompleta.Where(r => r.FechaVencimiento.Year < minYear && !r.Pagado);
                var listaGrupos = rawDataFiltrada.GroupBy(g => new { g.Contrato.Institucion.Nombre, g.Contrato.InstitucionId, g.Contrato.monedaContrato }).OrderBy(r => r.Key.InstitucionId);

                decimal saldoInicial = 0;
                decimal saldoInicialDlls = 0;

                for (int i = minYear; i <= maxYear; i++)
                {

                    foreach (var item in listaGrupos)
                    {
                        MesesAdeudo mesAdeudo = new MesesAdeudo();
                        mesAdeudo.idInstitucion = item.Key.InstitucionId;
                        mesAdeudo.descripcionInstitucion = item.Key.Nombre;
                        mesAdeudo.anio = i;

                        if (i == minYear)
                        {
                            saldoInicial = rawDataCompleta.Where(r => !r.Pagado && r.Contrato.InstitucionId == item.Key.InstitucionId && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe);
                            saldoInicialDlls = rawDataCompleta.Where(r => !r.Pagado && r.Contrato.InstitucionId == item.Key.InstitucionId && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe * r.Contrato.TipoCambio);
                        }
                        else
                        {
                            saldoInicial = tablaAduedo.FirstOrDefault(r => r.idInstitucion == item.Key.InstitucionId && r.anio == Convert.ToInt32(i - 1) && r.moneda == item.Key.monedaContrato).anioActual;
                            saldoInicialDlls = tablaAduedo.FirstOrDefault(r => r.idInstitucion == item.Key.InstitucionId && r.anio == Convert.ToInt32(i - 1) && r.moneda == item.Key.monedaContrato).anioActualDlls;
                        }


                        mesAdeudo.anioAnterior = saldoInicial;
                        mesAdeudo.enero = rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Month == 1 && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe);
                        mesAdeudo.febrero = rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Month == 2 && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe);
                        mesAdeudo.marzo = rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Month == 3 && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe);
                        mesAdeudo.abril = rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Month == 4 && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe);
                        mesAdeudo.mayo = rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Month == 5 && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe);
                        mesAdeudo.junio = rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Month == 6 && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe);
                        mesAdeudo.julio = rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Month == 7 && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe);
                        mesAdeudo.agosto = rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Month == 8 && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe);
                        mesAdeudo.septiembre = rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Month == 9 && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe);
                        mesAdeudo.octubre = rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Month == 10 && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe);
                        mesAdeudo.noviembre = rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Month == 11 && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe);
                        mesAdeudo.diciembre = rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Month == 12 && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe);
                        mesAdeudo.anioActual = saldoInicial - rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe);

                        mesAdeudo.anioAnteriorDlls = saldoInicialDlls;
                        mesAdeudo.eneroDlls = rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Month == 1 && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe * r.Contrato.TipoCambio);
                        mesAdeudo.febreroDlls = rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Month == 2 && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe * r.Contrato.TipoCambio);
                        mesAdeudo.marzoDlls = rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Month == 3 && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe * r.Contrato.TipoCambio);
                        mesAdeudo.abrilDlls = rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Month == 4 && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe * r.Contrato.TipoCambio);
                        mesAdeudo.mayoDlls = rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Month == 5 && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe * r.Contrato.TipoCambio);
                        mesAdeudo.junioDlls = rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Month == 6 && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe * r.Contrato.TipoCambio);
                        mesAdeudo.julioDlls = rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Month == 7 && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe * r.Contrato.TipoCambio);
                        mesAdeudo.agostoDlls = rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Month == 8 && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe * r.Contrato.TipoCambio);
                        mesAdeudo.septiembreDlls = rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Month == 9 && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe * r.Contrato.TipoCambio);
                        mesAdeudo.octubreDlls = rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Month == 10 && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe * r.Contrato.TipoCambio);
                        mesAdeudo.noviembreDlls = rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Month == 11 && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe * r.Contrato.TipoCambio);
                        mesAdeudo.diciembreDlls = rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Month == 12 && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe * r.Contrato.TipoCambio);
                        mesAdeudo.anioActualDlls = saldoInicialDlls - rawDataCompleta.Where(r => r.Contrato.InstitucionId == item.Key.InstitucionId && r.FechaVencimiento.Year == i && !r.Pagado && r.Contrato.monedaContrato == item.Key.monedaContrato).Sum(r => r.Importe * r.Contrato.TipoCambio);
                        mesAdeudo.moneda = item.Key.monedaContrato;
                        mesAdeudo.tipoCambio = item.Key.monedaContrato == 1 ? "Pesos" : "Dlls";
                        tablaAduedo.Add(mesAdeudo);
                    }
                }

                HttpContext.Current.Session["getRptAdeudosGeneral"] = tablaAduedo;
                result.Add("reporte", tablaAduedo.Where(r => r.moneda == 1));
                result.Add("adeudoDLLS", tablaAduedo.Where(r => r.moneda == 2));
                result.Add(SUCCESS, true);
                return result;

            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, e.Message);
                return result;
            }
        }

        public Dictionary<string, object> getRptAdeudosDetalle(int tipoMoneda, List<int> instituciones, DateTime fechaFin, int tipo, List<bool> tipoArre)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                using (var _ctxArre = new MainContext(EmpresaEnum.Arrendadora))
                {
                    //
                    var tipoCambioEK = getTipoCambioDLLs(fechaFin.Date);
                    //

                    List<tblAF_DxP_Divisiones> lstCatDivisiones = _context.tblAF_DxP_Divisiones.Where(x => x.esActivo).ToList(); //CATALOGO DE DIVISIONES
                    List<Core.Entity.Principal.Multiempresa.tblP_CC> lstCC = _ctxArre.tblP_CC.Where(x => x.estatus).ToList();
                    if (tipo == 1)
                    {
                        var reporte = _context.tblAF_DxP_ContratoMaquinas
                         .Where(r => instituciones.Contains(r.Contrato.InstitucionId) && r.Contrato.monedaContrato == tipoMoneda)
                         .Where(w => w.Contrato.fechaFirma <= fechaFin.Date)
                         //.Where(w => !w.Contrato.Terminado)
                         .Where(w => tipoArre.Count == 0 ? true : tipoArre.Contains(w.Contrato.arrendamientoPuro))
                         .ToList();

                        var _maquinasArre = _ctxArre.tblM_CatMaquina.ToList();

                        foreach (var item in reporte)
                        {
                            item.Maquina = _maquinasArre.FirstOrDefault(f => f.id == item.MaquinaId);
                        }

                        List<adeudosDTO> listaAdeudos = new List<adeudosDTO>();
                        foreach (var r in reporte)
                        {
                            ////
                            //if (!r.Contrato.detalles.Any(x => !x.Pagado))
                            //{
                            //    if (r.Contrato.detalles.Any(x => x.FechaPago.HasValue && x.FechaPago.Value.Year == fechaFin.Year))
                            //    {
                            //        r.Contrato.Terminado = true;
                            //    }
                            //    else
                            //    {
                            //        //continue;
                            //    }
                            //}
                            ////

                            if (r.MaquinaId == 6685)
                            {
                                int i = 0;
                            }

                            adeudosDTO objNew = new adeudosDTO(); //TODO
                            var contratoDetalleMaquina = _context.tblAF_DxP_ContratoMaquinasDetalle.Where(c => c.ContratoMaquinaId == r.Id).ToList();
                            var contrato = contratoDetalleMaquina.First().ContratoDetalle.Contrato;
                            var detalleSaldoInsoluto = contratoDetalleMaquina.Where(x => x.Pagado && x.FechaVencimiento <= fechaFin).OrderByDescending(x => x.Parcialidad).FirstOrDefault();

                            objNew.proveedor = r.Contrato.Institucion.Nombre;
                            objNew.tipoFinanciamiento = r.Contrato.arrendamientoPuro ? "AP" : "AF";
                            objNew.contrato = r.Contrato.Folio;
                            objNew.noEconomico = r.Maquina.noEconomico;
                            objNew.fechaInicio = r.Contrato.FechaInicio.ToShortDateString();
                            //objNew.fechaFin = r.Contrato.FechaInicio.AddMonths(r.Contrato.Plazo).ToShortDateString();
                            objNew.fechaFin = contratoDetalleMaquina.Last().FechaVencimiento.ToShortDateString();
                            objNew.tasaInteres = r.Contrato.TasaInteres; //Tasa Intereses
                            objNew.valorFinanciado = contratoDetalleMaquina.Where(f => f.ContratoMaquinaId == r.Id).Sum(f => f.Importe); //  Total Deuda
                            objNew.moneda = r.Contrato.monedaContrato == 1 ? "MN" : "  USD";//Moneda
                            objNew.pagoMensual = !r.Contrato.Institucion.Nombre.Contains("ENGENCAP") ? contratoDetalleMaquina.FirstOrDefault(f => f.ContratoMaquinaId == r.Id).Importe : contratoDetalleMaquina.Skip(2).First(f => f.ContratoMaquinaId == r.Id).Importe; //Pago Mensual
                            objNew.plazo = r.Contrato.Plazo;//Plazo
                            //
                            //objNew.intereses = contratoDetalleMaquina.FirstOrDefault(f => f.ContratoMaquinaId == r.Id).Intereses;//Intereses
                            //objNew.ivaSCapital = contratoDetalleMaquina.FirstOrDefault(f => f.ContratoMaquinaId == r.Id).IvaSCapital; //Iva SCapital
                            //objNew.ivaIntereses = contratoDetalleMaquina.FirstOrDefault(f => f.ContratoMaquinaId == r.Id).IvaIntereses; //Iva Intereses
                            //
                            objNew.fechaPago = getFechaPagoTipo(r.Contrato.TipoFechaVencimiento.Id, r.Contrato.FechaVencimiento);
                            decimal tipoCambio = r.Contrato.TipoCambio;
                            //
                            tipoCambio = tipoCambioEK.tipo_cambio;
                            //
                            objNew.tipoCambio = tipoCambio;
                            objNew.saldoInsoluto = detalleSaldoInsoluto == null || r.Contrato.Terminado ? 0 : contratoDetalleMaquina.Any(x => !x.Pagado || (x.Pagado && x.FechaPago.HasValue && x.FechaPago.Value > fechaFin)) ? detalleSaldoInsoluto.Saldo : 0;

                            int PagosRealizados = 0;
                            int PagosPendientes = 0;
                            decimal ImportePagado = 0;
                            decimal importePendiente = 0;

                            if (contrato.Terminado)
                            {
                                PagosRealizados = contratoDetalleMaquina.Where(w => w.Pagado).Count();
                                PagosPendientes = 0;
                                //ImportePagado = contratoDetalleMaquina.Where(w => w.Pagado && w.ContratoMaquina.Contrato.TipoCambio == 1).Sum(s => s.Importe);
                                ImportePagado = contratoDetalleMaquina.Where(w => w.Pagado).Sum(s => s.Importe);
                                importePendiente = 0;

                                objNew.pagoRealizados = PagosRealizados; //Pago Efe
                                objNew.importePagado = ImportePagado; //Importe Pagado
                                objNew.pagosPendientes = PagosPendientes; //Pagos Pendientes
                                objNew.saldoPendiente = 0; //Saldo Pendiente

                                objNew.saldoLP = 0;
                                objNew.saldoCP = 0;
                                //objNew.cargoObra = _context.tblP_CC.FirstOrDefault(f => f.areaCuenta == r.Maquina.centro_costos) != null ? _context.tblP_CC.FirstOrDefault(f => f.areaCuenta == r.Maquina.centro_costos).descripcion : "MAQUINARIA NO ASIGNADA A OBRA";
                                var _cc = lstCC.FirstOrDefault(f => f.areaCuenta == r.Maquina.centro_costos && r.Maquina.estatus == 1);
                                objNew.cargoObra = _cc != null && _cc.areaCuenta != "1010" && _cc.areaCuenta != "1015" && _cc.areaCuenta != "1018" && _cc.areaCuenta != "9-12" ? _cc.descripcion : "MAQUINARIA NO ASIGNADA A OBRA";
                            }
                            else
                            {
                                contratoDetalleMaquina.ForEach(s =>
                                {
                                    if ((s.FechaVencimiento.Date <= fechaFin.Date && s.Pagado) || (s.FechaPago.HasValue && s.FechaPago.Value <= fechaFin.Date && s.Pagado))
                                    {
                                        ImportePagado += s.Importe;
                                        PagosRealizados++;
                                    }
                                    else
                                    {
                                        importePendiente += s.Importe;
                                        PagosPendientes++;
                                        
                                        //
                                        objNew.intereses += s.Intereses;
                                        objNew.ivaSCapital += s.IvaSCapital;
                                        objNew.ivaIntereses += s.IvaIntereses;
                                        //
                                    }
                                });

                                //
                                decimal _cp = 0M;
                                decimal _lp = 0M;
                                contratoDetalleMaquina.ForEach(s =>
                                {
                                    if (s.FechaVencimiento.Date > fechaFin.Date && s.FechaVencimiento.Year == fechaFin.Year && (!s.FechaPago.HasValue || (s.FechaPago.HasValue && s.FechaPago.Value.Date > fechaFin.Date && s.FechaPago.Value.Year == fechaFin.Year)))
                                    {
                                        _cp += s.Importe;
                                    }
                                    if (s.FechaVencimiento.Date <= fechaFin.Date && !s.Pagado)
                                    {
                                        _cp += s.Importe;
                                    }
                                    if (s.FechaVencimiento.Year > fechaFin.Year && (!s.FechaPago.HasValue || (s.FechaPago.Value.Year > fechaFin.Year)))
                                    {
                                        _lp += s.Importe;
                                    }
                                });
                                //

                                objNew.pagoRealizados = PagosRealizados; //Pago Efe
                                objNew.importePagado = ImportePagado; //Importe Pagado
                                objNew.pagosPendientes = PagosPendientes; //Pagos Pendientes
                                objNew.saldoPendiente = objNew.valorFinanciado - ImportePagado; //Saldo Pendiente
                                objNew.saldoPendienteConversionDllsMxn = (objNew.valorFinanciado - ImportePagado) * tipoCambio;

                                //objNew.saldoLP = contratoDetalleMaquina.Where(f => f.FechaVencimiento.Year > DateTime.Now.Year).Sum(s => s.Importe);
                                //objNew.saldoCP = Math.Abs(objNew.saldoPendiente - objNew.saldoLP);
                                objNew.saldoCP = _cp;
                                objNew.saldoLP = _lp;
                                //objNew.cargoObra = _context.tblP_CC.FirstOrDefault(f => f.areaCuenta == r.Maquina.centro_costos) != null ? _context.tblP_CC.FirstOrDefault(f => f.areaCuenta == r.Maquina.centro_costos).descripcion : "MAQUINARIA NO ASIGNADA A OBRA";
                                var _cc = lstCC.FirstOrDefault(f => f.areaCuenta == r.Maquina.centro_costos && r.Maquina.estatus == 1);
                                objNew.cargoObra = _cc != null && _cc.areaCuenta != "1010" && _cc.areaCuenta != "1015" && _cc.areaCuenta != "1018" && _cc.areaCuenta != "9-12" ? _cc.descripcion : "MAQUINARIA NO ASIGNADA A OBRA";
                            }

                            #region SE OBTIENE LA DIVISIÓN QUE PERTENECE EL DETALLE
                            string areaCuenta = r.Maquina.centro_costos;
                            string cc = string.Empty;
                            int divisionID = 0;
                            string division = string.Empty;
                            bool isAdmin = false;
                            List<Core.Entity.Principal.Multiempresa.tblP_CC> objCC = lstCC.Where(x => x.areaCuenta == areaCuenta).ToList();
                            if (objCC.Count > 0)
                            {
                                cc = objCC.Select(x => x.cc).First();
                                List<tblAF_DxP_Divisiones_Proyecto> lstDivisionesProyectos = _context.tblAF_DxP_Divisiones_Proyecto.Where(x => x.cc == cc && x.esActivo).ToList();
                                if (lstDivisionesProyectos.Count() > 0)
                                {
                                    if (lstDivisionesProyectos.Count() > 0)
                                    {
                                        divisionID = lstDivisionesProyectos.Count > 0 ? lstDivisionesProyectos.Select(s => s.divisionID).First() : 0;
                                        division = lstCatDivisiones.Where(x => x.id == divisionID).Select(x => x.nombre).First();
                                    }
                                    isAdmin = lstDivisionesProyectos.Select(x => x.isAdmin).First();
                                    objNew.division = division;
                                    objNew.isAdmin = isAdmin ? "ADMIN" : "OBRA";
                                }
                            }
                            else if (areaCuenta == "997") //MAQUINARA NO ASIGNADA A OBRA
                            {
                                objNew.division = "ADMIN";
                                objNew.isAdmin = "OBRA";
                            }
                            #endregion

                            listaAdeudos.Add(objNew);
                        }

                        HttpContext.Current.Session["getRptAdeudosDetalle"] = listaAdeudos;
                        result.Add("reporte", listaAdeudos);
                    }
                    else
                    {

                        var reporte = _context.tblAF_DxP_Contratos
                       .Where(r => (instituciones.Count != 0 ? instituciones.Contains(r.InstitucionId) : true)
                                                 && r.monedaContrato == tipoMoneda)
                        .Where(w => tipoArre.Count == 0 ? true : tipoArre.Contains(w.arrendamientoPuro))
                        //.Where(w => !w.Terminado)
                       .Where(w => w.fechaFirma <= fechaFin.Date).ToList();
                        List<adeudosDTO> listaAdeudosGeneral = new List<adeudosDTO>();

                        foreach (var r in reporte)
                        {
                            adeudosDTO objNew = new adeudosDTO();
                            var contratoDetalle = _context.tblAF_DxP_ContratosDetalle.Where(f => f.ContratoId == r.Id).ToList();
                            var detalleSaldoInsoluto = contratoDetalle.Where(x => x.Pagado && x.FechaVencimiento <= fechaFin).OrderByDescending(x => x.Parcialidad).FirstOrDefault();

                            objNew.proveedor = r.Institucion.Nombre;
                            objNew.tipoFinanciamiento = r.arrendamientoPuro ? "AP" : "AF";
                            objNew.contrato = r.Folio;
                            objNew.noEconomico = r.Id.ToString();
                            objNew.fechaInicio = r.FechaInicio.ToShortDateString();
                            //objNew.fechaFin = r.FechaInicio.AddMonths(r.Plazo).ToShortDateString();
                            objNew.fechaFin = contratoDetalle.Last().FechaVencimiento.ToShortDateString();
                            objNew.tasaInteres = r.TasaInteres; //Tasa Intereses
                            objNew.valorFinanciado = contratoDetalle.Where(f => f.ContratoId == r.Id).Sum(f => f.Importe); //  Total Deuda
                            objNew.moneda = r.monedaContrato == 1 ? "MN" : "  USD";//Moneda
                            objNew.pagoMensual = !r.Institucion.Nombre.Contains("ENGENCAP") ? contratoDetalle.FirstOrDefault(f => f.ContratoId == r.Id).Importe : contratoDetalle.Skip(2).First(f => f.ContratoId == r.Id).Importe; //Pago Mensual
                            objNew.plazo = r.Plazo;//Plazo
                            objNew.intereses = contratoDetalle.FirstOrDefault(f => f.ContratoId == r.Id).Intereses;//Intereses
                            objNew.ivaSCapital = contratoDetalle.FirstOrDefault(f => f.ContratoId == r.Id).IvaSCapital; //Iva SCapital
                            objNew.ivaIntereses = contratoDetalle.FirstOrDefault(f => f.ContratoId == r.Id).IvaIntereses; //Iva Intereses
                            objNew.fechaPago = getFechaPagoTipo(r.TipoFechaVencimiento.Id, r.FechaVencimiento);
                            objNew.saldoInsoluto = detalleSaldoInsoluto == null || r.Terminado ? 0 : contratoDetalle.Any(x => !x.Pagado || (x.Pagado && x.FechaPago.HasValue && x.FechaPago.Value > fechaFin)) ? detalleSaldoInsoluto.Saldo : 0;

                            int PagosRealizados = 0;
                            int PagosPendientes = 0;
                            decimal ImportePagado = 0;
                            decimal importePendiente = 0;

                            if (r.Terminado)
                            {
                                PagosRealizados = contratoDetalle.Where(w => w.Pagado).Count();
                                PagosPendientes = 0;
                                ImportePagado = contratoDetalle.Where(w => w.Pagado).Sum(s => s.Importe);
                                importePendiente = 0;

                                objNew.pagoRealizados = PagosRealizados;
                                objNew.importePagado = ImportePagado;
                                objNew.pagosPendientes = PagosPendientes;
                                objNew.saldoPendiente = 0;

                                objNew.saldoLP = 0;
                                objNew.saldoCP = 0;
                                objNew.cargoObra = "";
                            }
                            else
                            {
                                contratoDetalle.ForEach(s =>
                                {
                                    if ((s.FechaVencimiento.Date <= fechaFin.Date && s.Pagado) || (s.FechaPago.HasValue && s.FechaPago.Value <= fechaFin.Date && s.Pagado))
                                    {
                                        ImportePagado += s.Importe;
                                        PagosRealizados++;
                                    }
                                    else
                                    {
                                        importePendiente += s.Importe;
                                        PagosPendientes++;
                                    }
                                });

                                //
                                decimal _cp = 0M;
                                decimal _lp = 0M;
                                contratoDetalle.ForEach(s =>
                                {
                                    if (s.FechaVencimiento.Date > fechaFin.Date && s.FechaVencimiento.Year == fechaFin.Year && (!s.FechaPago.HasValue || (s.FechaPago.HasValue && s.FechaPago.Value.Date > fechaFin.Date && s.FechaPago.Value.Year == fechaFin.Year)))
                                    {
                                        _cp += s.Importe;
                                    }
                                    if (s.FechaVencimiento.Date <= fechaFin.Date && !s.Pagado)
                                    {
                                        _cp += s.Importe;
                                    }
                                    if (s.FechaVencimiento.Year > fechaFin.Year && (!s.FechaPago.HasValue || (s.FechaPago.Value.Year > fechaFin.Year)))
                                    {
                                        _lp += s.Importe;
                                    }
                                });
                                //

                                objNew.pagoRealizados = PagosRealizados; //Pago Efe
                                objNew.importePagado = ImportePagado; //Importe Pagado
                                objNew.pagosPendientes = PagosPendientes; //Pagos Pendientes
                                objNew.saldoPendiente = objNew.valorFinanciado - ImportePagado; //Saldo Pendiente
                                //objNew.saldoLP = contratoDetalle.Where(f => f.FechaVencimiento.Year > DateTime.Now.Year).Sum(s => s.Importe);
                                //objNew.saldoCP = Math.Abs(objNew.saldoPendiente - objNew.saldoLP);
                                objNew.saldoCP = _cp;
                                objNew.saldoLP = _lp;
                                objNew.cargoObra = "";
                            }


                            #region SE OBTIENE LA DIVISIÓN QUE PERTENECE EL DETALLE
                            //string cc = r.Maquina.centro_costos;
                            //int divisionID = 0;
                            //string division = string.Empty;
                            //bool isAdmin = false;

                            //List<tblAF_DxP_Divisiones_Proyecto> lstDivisionesProyectos = _context.tblAF_DxP_Divisiones_Proyecto.Where(x => x.cc == cc).ToList();
                            //if (lstDivisionesProyectos.Count() > 0)
                            //{
                            //    if (lstDivisionesProyectos.Count() == 1)
                            //    {
                            //        divisionID = lstDivisionesProyectos.Count > 0 ? lstDivisionesProyectos.Select(s => s.divisionID).First() : 0;
                            //        division = lstCatDivisiones.Where(x => x.id == divisionID).Select(x => x.nombre).First();
                            //    }
                            //    else
                            //        division = lstCatDivisiones.Where(x => x.id == 1).Select(x => x.nombre).First(); //ADMIN POR DEFAULT EN EL CC MAQUINARIA NO ASIGNADA A OBRA

                            //    isAdmin = lstDivisionesProyectos.Select(x => x.isAdmin).First();
                            //}

                            //objNew.division = division;
                            //objNew.isAdmin = isAdmin ? "ADMIN" : "OBRA";
                            #endregion

                            listaAdeudosGeneral.Add(objNew);
                        }

                        result.Add("reporte", listaAdeudosGeneral);
                        listaAdeudosGeneral.ForEach(r => r.noEconomico = "");
                        HttpContext.Current.Session["getRptAdeudosDetalle"] = listaAdeudosGeneral;

                    }
                    result.Add(SUCCESS, true);
                    return result;
                }
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, e.Message);
                return result;

            }
        }

        private string nombreObra(string cc)
        {
            try
            {
                return _context.tblP_CC.FirstOrDefault(r => r.areaCuenta == cc).descripcion;
            }
            catch (Exception)
            {
                return "MAQUINARIA NO ASIGNADA A OBRA";
            }
        }

        private decimal getImporteFechaPagados(int noEconomico, int pagado, DateTime fecha)
        {
            try
            {

                switch (pagado)
                {
                    //Importe Pagado
                    case 1:
                        return _context.tblAF_DxP_ContratoMaquinasDetalle.Where(r => r.ContratoMaquina.MaquinaId == noEconomico && r.Pagado).ToList().Where(r => r.FechaVencimiento.Date <= fecha.Date).Sum(r => r.Importe);
                    case 2:
                        return _context.tblAF_DxP_ContratoMaquinasDetalle.Where(r => r.ContratoMaquina.MaquinaId == noEconomico).ToList().Where(r => r.FechaVencimiento.Date > fecha.Date).Sum(r => r.Importe);
                    default:
                        return 0;
                }

            }
            catch (Exception e)
            {
                return 0;
            }
        }

        private decimal getImportePagadoContrato(int contratoID)
        {
            try
            {
                return _context.tblAF_DxP_ContratosDetalle.Where(c => c.Pagado && c.ContratoId == contratoID).Sum(f => f.Importe);
            }
            catch (Exception)
            {
                return 0;
            }

        }

        private decimal getImportePagado(int noEconomico, bool? pagado)
        {
            try
            {
                return _context.tblAF_DxP_ContratoMaquinasDetalle.Where(c => (pagado == null ? true : c.Pagado == pagado) && c.ContratoMaquinaId == noEconomico).Sum(f => f.Importe);
            }
            catch (Exception)
            {
                return 0;
            }

        }

        private string getFechaPagoTipo(int tipoPago, DateTime? FechaTentativa)
        {
            switch (tipoPago)
            {
                case 1:
                    return "30 C/MES";
                case 2:
                    return "15 C/MES";
                case 3:
                    return FechaTentativa.Value.ToShortDateString();
                default:
                    return "N/A";
            }
        }

        public Dictionary<string, object> comboCbosData()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {

                string consulta = @"SELECT cc AS Value, descripcion AS Text FROM cc";
                var res1 = (IList<ComboDTO>)_contextEnkontrol.Where(consulta).ToObject<IList<ComboDTO>>();


                var comboAC = _context.tblP_CC.Select(z => new cboDocumentosDTO
                {
                    cc = z.cc,
                    area = z.area,
                    cuenta = z.cuenta,
                    descripcion = z.descripcion.Trim()
                }).ToList();

                foreach (var item in res1)
                {
                    cboDocumentosDTO obj = new cboDocumentosDTO();
                    obj.cc = item.Value;
                    obj.descripcion = item.Text;

                    comboAC.Add(obj);
                }

                result.Add("comboAC", comboAC);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }

            return result;
        }
        #endregion
        public Dictionary<string, object> GetListaCuentasInit(int iDisplayStart, int iDisplayLength)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                var odbc = new OdbcConsultaDTO();
                odbc.consulta = @"SELECT ROW_NUMBER() OVER(ORDER BY cta ASC) AS numRow, cta,scta,sscta,descripcion,digito, requiere_oc 
                                  FROM catcta ";
                List<ctaDTO> listaCuentas = _contextEnkontrol.Select<ctaDTO>(productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, odbc);

                result.Add("listaCta", listaCuentas.ToList());

                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error, al momento de cargar la información de proveedores en enkontrol.");
            }
            return result;
        }
        public List<ctaDTO> GetCtaList()
        {
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"SELECT ROW_NUMBER() OVER(ORDER BY cta ASC) AS numRow, cta,scta,sscta,descripcion,digito, requiere_oc 
                                  FROM catcta ";
            List<ctaDTO> listaCuentas = _contextEnkontrol.Select<ctaDTO>(productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, odbc);

            return listaCuentas.Take(10000).ToList();
        }
        public Dictionary<string, object> LoadCtas()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"SELECT ROW_NUMBER() OVER(ORDER BY cta ASC) AS numRow, cta,scta,sscta,descripcion,digito, requiere_oc 
                                  FROM catcta ";
            List<ctaDTO> listaCuentas = _contextEnkontrol.Select<ctaDTO>(productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, odbc);

            /* var data = listaCuentas.Skip(iDisplayStart)
             .Take(iDisplayLength).ToList();*/

            result.Add("listaCta", listaCuentas.Take(10000).ToList());
            result.Add(SUCCESS, true);
            return result;
        }

        public dtLoadCtaServerDTO LoadCtasServerSide()
        {
            dtLoadCtaServerDTO loadCtaServerDTO = new dtLoadCtaServerDTO();
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"SELECT ROW_NUMBER() OVER(ORDER BY cta ASC) AS numRow, cta,scta,sscta,descripcion,digito, requiere_oc 
                                  FROM catcta ";
            List<ctaDTO> listaCuentas = _contextEnkontrol.Select<ctaDTO>(productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, odbc);

            loadCtaServerDTO.data = listaCuentas.ToList();
            loadCtaServerDTO.draw = 1;
            loadCtaServerDTO.recordsFiltered = listaCuentas.ToList().Count();
            loadCtaServerDTO.recordsTotal = listaCuentas.ToList().Count();

            return loadCtaServerDTO;
        }

        public Dictionary<string, object> TerminarContrato(int contratoID)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    if (_context.tblAF_DxP_Contratos.FirstOrDefault(r => r.Id == contratoID) != null)
                    {

                        var contratos = _context.tblAF_DxP_Contratos.FirstOrDefault(r => r.Id == contratoID);

                        if (contratos.Terminado)
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "El contrato ya se encuentra terminado");
                        }
                        else
                        {
                            contratos.Terminado = true;
                            _context.SaveChanges();

                            var contratoDetalle = _context.tblAF_DxP_ContratosDetalle.Where(r => r.ContratoId == contratoID).ToList();
                            foreach (var item in contratoDetalle)
                            {
                                var updateData = _context.tblAF_DxP_ContratosDetalle.FirstOrDefault(r => r.Id == item.Id);
                                updateData.Pagado = true;
                                _context.SaveChanges();
                            }

                            var maquinaDetalle = _context.tblAF_DxP_ContratoMaquinasDetalle.Where(r => r.ContratoMaquina.ContratoId == contratoID).ToList();

                            foreach (var item in maquinaDetalle)
                            {
                                var itemContratoDetalle = _context.tblAF_DxP_ContratoMaquinasDetalle.FirstOrDefault(r => r.Id == item.Id);
                                itemContratoDetalle.Pagado = true;
                                _context.SaveChanges();
                            }

                            var contratoMaquina = _context.tblAF_DxP_ContratoMaquinas.Where(r => r.ContratoId == contratoID).ToList();

                            foreach (var item in contratoMaquina)
                            {
                                var itemCotratoMaquina = _context.tblAF_DxP_ContratoMaquinas.FirstOrDefault(r => r.Id == item.Id);
                                itemCotratoMaquina.Estatus = false;
                                _context.SaveChanges();
                            }
                            transaction.Commit();
                            resultado.Add(SUCCESS, true);
                        }

                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    resultado.Add(SUCCESS, false);
                }
                return resultado;
            }
        }
        public Dictionary<string, object> UpdateContratosDet(int contratoID)
        {
            Dictionary<string, object> Resultado = new Dictionary<string, object>();

            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var detalles = _context.tblAF_DxP_ContratoMaquinasDetalle.Where(r => r.ContratoMaquina.ContratoId == contratoID).ToList();
                    decimal saldo = 0;
                    foreach (var detalle in detalles)
                    {
                        var maquinaDetalle = _context.tblAF_DxP_ContratoMaquinasDetalle.FirstOrDefault(r => r.Id == detalle.Id);
                        var contratoDet = _context.tblAF_DxP_ContratosDetalle.FirstOrDefault(r => r.ContratoId == contratoID && detalle.Parcialidad == r.Parcialidad);

                        var tempContratoMaquina = _context.tblAF_DxP_ContratoMaquinas.FirstOrDefault(r => r.ContratoId == contratoID); //.porcentaje;

                        if (tempContratoMaquina != null)
                        {
                            decimal porcentaje = detalle.ContratoMaquina.porcentaje;
                            decimal contratoCredito = detalle.ContratoMaquina.Credito;
                            maquinaDetalle.Intereses = (contratoDet.Intereses * porcentaje) / 100;
                            maquinaDetalle.AmortizacionCapital = (contratoDet.AmortizacionCapital * porcentaje) / 100;
                            maquinaDetalle.IvaSCapital = (contratoDet.IvaSCapital * porcentaje) / 100;
                            maquinaDetalle.IvaIntereses = (contratoDet.IvaIntereses * porcentaje) / 100;
                            maquinaDetalle.Importe = (contratoDet.Importe * porcentaje) / 100;
                            maquinaDetalle.Saldo = detalle.Parcialidad == 1 ? contratoCredito - maquinaDetalle.AmortizacionCapital : saldo - maquinaDetalle.AmortizacionCapital;
                            maquinaDetalle.Estatus = true;
                            saldo = maquinaDetalle.Saldo;
                            _context.SaveChanges();
                        }
                    }

                    dbTransaction.Commit();
                    Resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    Resultado.Add(SUCCESS, false);
                    Resultado.Add(MESSAGE, "Ocurrió un error, al momento de cargar la información el contrato.");
                }
            }

            return Resultado;
        }

        public Dictionary<string, object> UpdateContratoArchivo(int contratoID, string fileContrato)
        {
            Dictionary<string, object> Resultado = new Dictionary<string, object>();
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var element = _context.tblAF_DxP_Contratos.FirstOrDefault(r => r.Id == contratoID);

                    element.FileContrato = fileContrato;
                    _context.SaveChanges();
                    dbTransaction.Commit();
                    Resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    Resultado.Add(SUCCESS, false);
                    Resultado.Add(MESSAGE, "Ocurrió un error, al momento de cargar la el archivo ocurrio un error.");
                }
            }
            return Resultado;
        }

        #region Cedula Mensual
        public Dictionary<string, object> GetCedula(DateTime fechaCorte)
        {
            var r = new Dictionary<string, object>();

            try
            {
                var tipoCambio = getTipoCambioDLLs(fechaCorte);

                var cedula = new List<CedulaMensualDTO>();
                var cedulaPQ = new List<CedulaMensualDTO>();
                var detalleCortoPlazo = new List<CedulaMensualDetalleInstitucionesDTO>();
                var detalleLargoPlazo = new List<CedulaMensualDetalleInstitucionesDTO>();

                var detalleContratos = _context.tblAF_DxP_ContratoMaquinasDetalle
                    .Where(w =>
                        w.ContratoMaquina.Contrato.fechaFirma <= fechaCorte.Date &&
                        !w.ContratoMaquina.Contrato.Terminado &&
                        !w.ContratoMaquina.Contrato.arrendamientoPuro
                    ).ToList();

                foreach (var gbInstitucion in detalleContratos.GroupBy(g => g.ContratoMaquina.Contrato.InstitucionId).OrderBy(o => o.Key))
                {
                    var infoCedula = new CedulaMensualDTO();
                    infoCedula.financiera = gbInstitucion.First().ContratoMaquina.Contrato.Institucion.Nombre;

                    foreach (var gbMoneda in gbInstitucion.GroupBy(g => g.ContratoMaquina.Contrato.monedaContrato))
                    {
                        foreach (var gbContrato in gbMoneda.GroupBy(g => g.ContratoMaquina.Contrato.Id))
                        {
                            var contrato = gbContrato.First().ContratoMaquina.Contrato;

                            var _cortoPlazoSigoplanDetalle = 0M;
                            var _cortoPlazoCargosContabilidad = 0M;
                            var _cortoPlazoAbonosContabilidad = 0M;
                            var _largoPlazoSigoplanDetalle = 0M;
                            var _largoPlazoSaldoInicial = 0M;
                            var _largoPlazoCargosContabilidad = 0M;
                            var _largoPlazoAbonosContabilidad = 0M;

                            var _interesesPagados = 0M;
                            var _interesesCP = 0M;
                            var _interesesLP = 0M;

                            if (!gbContrato.Any(x => !x.Pagado))
                            {
                                continue;
                            }

                            foreach (var detalle in gbContrato)
                            {
                                if (detalle.FechaVencimiento.Date > fechaCorte.Date && detalle.FechaVencimiento.Year == fechaCorte.Year)
                                {
                                    _cortoPlazoSigoplanDetalle += detalle.Importe;
                                    _interesesCP += detalle.Intereses;
                                }
                                if (detalle.FechaVencimiento.Date <= fechaCorte.Date && !detalle.Pagado)
                                {
                                    _cortoPlazoSigoplanDetalle += detalle.Importe;
                                    _interesesCP += detalle.Intereses;
                                }
                                if (detalle.FechaVencimiento.Year == fechaCorte.Year && detalle.FechaPago.HasValue && detalle.FechaPago.Value.Year == fechaCorte.Year && detalle.FechaPago.Value.Month > detalle.FechaVencimiento.Month && detalle.FechaPago.Value.Date > fechaCorte.Date)
                                {
                                    _cortoPlazoSigoplanDetalle += detalle.Importe;
                                    _interesesCP += detalle.Intereses;
                                }
                                if (detalle.FechaVencimiento.Year == fechaCorte.Year && detalle.FechaPago.HasValue && detalle.FechaPago.Value.Year == fechaCorte.Year && detalle.FechaPago.Value.Month < detalle.FechaVencimiento.Month)
                                {
                                    if (_cortoPlazoSigoplanDetalle != 0)
                                    {
                                        _cortoPlazoSigoplanDetalle -= detalle.Importe;
                                        _interesesCP -= detalle.Intereses;
                                    }
                                }
                                if (detalle.FechaVencimiento.Year > fechaCorte.Year)
                                {
                                    if (detalle.Pagado && detalle.FechaPago.HasValue && detalle.FechaPago.Value.Year <= fechaCorte.Year)
                                    {

                                    }
                                    else
                                    {
                                        _largoPlazoSigoplanDetalle += detalle.Importe;
                                        _interesesLP += detalle.Intereses;
                                    }
                                }
                                if (detalle.FechaVencimiento.Date <= fechaCorte.Date && detalle.Pagado && detalle.FechaPago.HasValue && detalle.FechaPago.Value.Year > fechaCorte.Date.Year)
                                {
                                    _cortoPlazoSigoplanDetalle += detalle.Importe;
                                    _interesesCP += detalle.Intereses;
                                }
                                if (detalle.FechaVencimiento.Date <= fechaCorte.Date && detalle.FechaVencimiento.Year == fechaCorte.Year && detalle.Pagado)
                                {
                                    _interesesPagados += detalle.Intereses;
                                }
                            }

                            #region cortoPlazoEnkontrol
                            var query_sc_salcont_cc = new OdbcConsultaDTO();

                            query_sc_salcont_cc.consulta = string.Format
                                (
                                    @"SELECT
                                        *
                                    FROM
                                        sc_salcont_cc
                                    WHERE
                                        cta = ? AND
                                        scta = ? AND
                                        sscta = ? AND
                                        year = ?"
                                );

                            query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                            {
                                nombre = "cta",
                                tipo = OdbcType.Int,
                                valor = contrato.cta
                            });
                            query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                            {
                                nombre = "scta",
                                tipo = OdbcType.Int,
                                valor = contrato.scta
                            });
                            query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                            {
                                nombre = "sscta",
                                tipo = OdbcType.Int,
                                valor = contrato.sscta
                            });
                            query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                            {
                                nombre = "year",
                                tipo = OdbcType.Int,
                                valor = fechaCorte.Year
                            });

                            var sc_salcont_cc = _contextEnkontrol.Select<ActivoFijoSaldosContablesDTO>(productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, query_sc_salcont_cc);

                            for (int mes = 1; mes <= fechaCorte.Month; mes++)
                            {
                                var mesCargo = Enum.GetName(typeof(Core.Enum.Generico.Fecha.MesCargoEnum), mes);
                                var mesAbono = Enum.GetName(typeof(Core.Enum.Generico.Fecha.MesAbonoEnum), mes);

                                _cortoPlazoCargosContabilidad += sc_salcont_cc.Sum(s => Convert.ToDecimal(s.GetType().GetProperty(mesCargo).GetValue(s, null)));
                                _cortoPlazoAbonosContabilidad += sc_salcont_cc.Sum(s => Convert.ToDecimal(s.GetType().GetProperty(mesAbono).GetValue(s, null)));
                            }

                            var infoDetalleCortoPlazo = new CedulaMensualDetalleInstitucionesDTO();
                            infoDetalleCortoPlazo.cuenta = contrato.cta;
                            infoDetalleCortoPlazo.scta = contrato.scta;
                            infoDetalleCortoPlazo.sscta = contrato.sscta;
                            infoDetalleCortoPlazo.descripcion = contrato.Descripcion;
                            infoDetalleCortoPlazo.saldoInicial = 0M;
                            infoDetalleCortoPlazo.cargos = _cortoPlazoCargosContabilidad + sc_salcont_cc.Sum(x => x.SalIni);
                            infoDetalleCortoPlazo.abonos = _cortoPlazoAbonosContabilidad;
                            infoDetalleCortoPlazo.saldoActual = infoDetalleCortoPlazo.cargos + infoDetalleCortoPlazo.abonos;
                            infoDetalleCortoPlazo.saldoActualSigoplan = contrato.monedaContrato == 1 ? _cortoPlazoSigoplanDetalle * -1 : (_cortoPlazoSigoplanDetalle * tipoCambio.tipo_cambio) * -1;
                            infoDetalleCortoPlazo.diferencia = Math.Abs(infoDetalleCortoPlazo.saldoActual - infoDetalleCortoPlazo.saldoActualSigoplan);

                            infoDetalleCortoPlazo.interesesPagados = _interesesPagados;
                            infoDetalleCortoPlazo.interesesCP = _interesesCP;
                            infoDetalleCortoPlazo.interesesLP = _interesesLP;

                            detalleCortoPlazo.Add(infoDetalleCortoPlazo);
                            #endregion

                            #region largoPlazoEnkontrol

                            CatCtaDTO infoCuentaLargoPlazo = null;

                            if (contrato.ctaLp.HasValue)
                            {
                                infoCuentaLargoPlazo = new CatCtaDTO();

                                infoCuentaLargoPlazo.cta = contrato.ctaLp.Value;
                                infoCuentaLargoPlazo.scta = contrato.sctaLp.Value;
                                infoCuentaLargoPlazo.sscta = contrato.ssctaLp.Value;
                            }
                            else
                            {
                                var query_catcta = new OdbcConsultaDTO();

                                query_catcta.consulta = string.Format
                                    (
                                        @"SELECT TOP 1
                                            cta,
                                            scta,
                                            sscta
                                        FROM
                                            catcta
                                        WHERE
                                            cta = 2135 AND
                                            descripcion like (SELECT TOP 1 descripcion FROM catcta WHERE cta = ? AND scta = ? AND sscta = ?)"
                                     );

                                query_catcta.parametros.Add(new OdbcParameterDTO()
                                {
                                    nombre = "cta",
                                    tipo = OdbcType.Int,
                                    valor = contrato.cta
                                });
                                query_catcta.parametros.Add(new OdbcParameterDTO()
                                {
                                    nombre = "scta",
                                    tipo = OdbcType.Int,
                                    valor = contrato.scta
                                });
                                query_catcta.parametros.Add(new OdbcParameterDTO()
                                {
                                    nombre = "sscta",
                                    tipo = OdbcType.Int,
                                    valor = contrato.sscta
                                });

                                infoCuentaLargoPlazo = _contextEnkontrol.Select<CatCtaDTO>(productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, query_catcta).FirstOrDefault();
                            }

                            if (infoCuentaLargoPlazo != null)
                            {
                                var query_lp_sc_salcont_cc = new OdbcConsultaDTO();

                                query_lp_sc_salcont_cc.consulta = string.Format
                                    (
                                        @"SELECT
                                            *
                                        FROM
                                            sc_salcont_cc
                                        WHERE
                                            cta = ? AND
                                            scta = ? AND
                                            sscta = ? AND
                                            year = ?"
                                    );

                                query_lp_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                                {
                                    nombre = "cta",
                                    tipo = OdbcType.Int,
                                    valor = infoCuentaLargoPlazo.cta
                                });
                                query_lp_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                                {
                                    nombre = "scta",
                                    tipo = OdbcType.Int,
                                    valor = infoCuentaLargoPlazo.scta
                                });
                                query_lp_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                                {
                                    nombre = "sscta",
                                    tipo = OdbcType.Int,
                                    valor = infoCuentaLargoPlazo.sscta
                                });
                                query_lp_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                                {
                                    nombre = "year",
                                    tipo = OdbcType.Int,
                                    valor = fechaCorte.Year
                                });

                                var lp_sc_salcont_cc = _contextEnkontrol.Select<ActivoFijoSaldosContablesDTO>(productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, query_lp_sc_salcont_cc);

                                for (int mes = 1; mes <= fechaCorte.Month; mes++)
                                {
                                    var mesCargo = Enum.GetName(typeof(Core.Enum.Generico.Fecha.MesCargoEnum), mes);
                                    var mesAbono = Enum.GetName(typeof(Core.Enum.Generico.Fecha.MesAbonoEnum), mes);

                                    _largoPlazoCargosContabilidad += lp_sc_salcont_cc.Sum(s => Convert.ToDecimal(s.GetType().GetProperty(mesCargo).GetValue(s, null)));
                                    _largoPlazoAbonosContabilidad += lp_sc_salcont_cc.Sum(s => Convert.ToDecimal(s.GetType().GetProperty(mesAbono).GetValue(s, null)));
                                }

                                _largoPlazoSaldoInicial = lp_sc_salcont_cc.Sum(s => s.SalIni);
                            }

                            var infoDetalleLargoPlazo = new CedulaMensualDetalleInstitucionesDTO();
                            infoDetalleLargoPlazo.cuenta = infoCuentaLargoPlazo != null ? infoCuentaLargoPlazo.cta : 0;
                            infoDetalleLargoPlazo.scta = infoCuentaLargoPlazo != null ? infoCuentaLargoPlazo.scta : 0;
                            infoDetalleLargoPlazo.sscta = infoCuentaLargoPlazo != null ? infoCuentaLargoPlazo.sscta : 0;
                            infoDetalleLargoPlazo.descripcion = contrato.Descripcion;
                            infoDetalleLargoPlazo.saldoInicial = _largoPlazoSaldoInicial;
                            infoDetalleLargoPlazo.cargos = _largoPlazoCargosContabilidad;
                            infoDetalleLargoPlazo.abonos = _largoPlazoAbonosContabilidad;
                            infoDetalleLargoPlazo.saldoActual = _largoPlazoSaldoInicial + _largoPlazoCargosContabilidad + _largoPlazoAbonosContabilidad;
                            infoDetalleLargoPlazo.saldoActualSigoplan = contrato.monedaContrato == 1 ? _largoPlazoSigoplanDetalle * -1 : (_largoPlazoSigoplanDetalle * tipoCambio.tipo_cambio) * -1;
                            infoDetalleLargoPlazo.diferencia = Math.Abs(infoDetalleLargoPlazo.saldoActual - infoDetalleLargoPlazo.saldoActualSigoplan);

                            detalleLargoPlazo.Add(infoDetalleLargoPlazo);
                            #endregion

                            infoCedula.nacionalCortoPlazo += contrato.monedaContrato == 1 ? _cortoPlazoSigoplanDetalle : 0;
                            infoCedula.dolaresCortoPlazo += contrato.monedaContrato == 2 ? (_cortoPlazoSigoplanDetalle * tipoCambio.tipo_cambio) : 0;
                            infoCedula.contabilidadCortoPlazo += Math.Abs(infoDetalleCortoPlazo.saldoActual);
                            infoCedula.sigoplanCortoPlazo += Math.Abs(infoDetalleCortoPlazo.saldoActualSigoplan);

                            infoCedula.nacionalLargoPlazo += contrato.monedaContrato == 1 ? _largoPlazoSigoplanDetalle : 0;
                            infoCedula.dolaresLargoPlazo += contrato.monedaContrato == 2 ? (_largoPlazoSigoplanDetalle * tipoCambio.tipo_cambio) : 0;
                            infoCedula.contabilidadLargoPlazo += Math.Abs(infoDetalleLargoPlazo.saldoActual);
                            infoCedula.sigoplanLargoPlazo += Math.Abs(infoDetalleLargoPlazo.saldoActualSigoplan);
                        }
                    }

                    infoCedula.diferenciaCortoPlazo = Math.Abs(infoCedula.contabilidadCortoPlazo - infoCedula.sigoplanCortoPlazo);
                    infoCedula.diferenciaLargoPlazo = Math.Abs(infoCedula.contabilidadLargoPlazo - infoCedula.sigoplanLargoPlazo);

                    cedula.Add(infoCedula);
                }

                #region PQ
                var movimientosActivosPQ = new List<int> { 1, 3, 5 };

                var pqs = _context.tblAF_DxP_PQ
                    .Where(w =>
                        w.estatus &&
                        w.fechaFirma <= fechaCorte.Date &&
                        (
                            movimientosActivosPQ.Contains(w.tipoMovimientoId)
                        ) ||
                        (
                            w.tipoMovimientoId == (int)PQTipoMovimientoEnum.BajaPorLiquidacion &&
                            w.fechaLiquidacion.HasValue &&
                            w.fechaLiquidacion.Value > fechaCorte.Date
                        )
                    ).ToList();

                foreach (var gbBanco in pqs.GroupBy(x => x.bancoId).OrderBy(x => x.Key))
                {
                    var _nacionalCP = 0M;
                    var _dolaresCP = 0M;
                    var _contabilidadCP = 0M;
                    var _sigoplanCP = 0M;

                    var ccPQenSigoplan = new List<string>();

                    foreach (var gbCC in gbBanco.GroupBy(x => x.cc))
                    {
                        ccPQenSigoplan.Add(gbCC.Key);

                        var _cargosContabilidadPQ = 0M;
                        var _abonosContabilidadPQ = 0M;

                        foreach (var gbMoneda in gbCC.GroupBy(x => x.monedaId))
                        {
                            var query_sc_salcont_cc = new OdbcConsultaDTO();

                            query_sc_salcont_cc.consulta = string.Format
                                (
                                    @"SELECT
                                        *
                                    FROM
                                        sc_salcont_cc
                                    WHERE
                                        cta = ? AND
                                        scta = ? AND
                                        sscta = ? AND
                                        cc = ?"
                                );

                            query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                            {
                                nombre = "cta",
                                tipo = OdbcType.Int,
                                valor = gbMoneda.First().ctaAbonoBanco
                            });
                            query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                            {
                                nombre = "scta",
                                tipo = OdbcType.Int,
                                valor = gbMoneda.First().sctaAbonoBanco
                            });
                            query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                            {
                                nombre = "sscta",
                                tipo = OdbcType.Int,
                                valor = gbMoneda.First().ssctaAbonoBanco
                            });
                            query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                            {
                                nombre = "cc",
                                tipo = OdbcType.NVarChar,
                                valor = gbCC.Key
                            });

                            var sc_salcont_cc = _contextEnkontrol.Select<ActivoFijoSaldosContablesDTO>(productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, query_sc_salcont_cc);

                            foreach (var item in sc_salcont_cc.Where(x => x.Year < fechaCorte.Year))
                            {
                                for (int mes = 1; mes <= 12; mes++)
                                {
                                    var mesCargo = Enum.GetName(typeof(Core.Enum.Generico.Fecha.MesCargoEnum), mes);
                                    var mesAbono = Enum.GetName(typeof(Core.Enum.Generico.Fecha.MesAbonoEnum), mes);

                                    _cargosContabilidadPQ += sc_salcont_cc.Where(x => x.Year == item.Year).Sum(x => Convert.ToDecimal(x.GetType().GetProperty(mesCargo).GetValue(x, null)));
                                    _abonosContabilidadPQ += sc_salcont_cc.Where(x => x.Year == item.Year).Sum(x => Convert.ToDecimal(x.GetType().GetProperty(mesAbono).GetValue(x, null)));
                                }
                            }
                            
                            for (int mes = 1; mes <= fechaCorte.Month; mes++)
                            {
                                var mesCargo = Enum.GetName(typeof(Core.Enum.Generico.Fecha.MesCargoEnum), mes);
                                var mesAbono = Enum.GetName(typeof(Core.Enum.Generico.Fecha.MesAbonoEnum), mes);

                                _cargosContabilidadPQ += sc_salcont_cc.Where(x => x.Year == fechaCorte.Year).Sum(x => Convert.ToDecimal(x.GetType().GetProperty(mesCargo).GetValue(x, null)));
                                _abonosContabilidadPQ += sc_salcont_cc.Where(x => x.Year == fechaCorte.Year).Sum(x => Convert.ToDecimal(x.GetType().GetProperty(mesAbono).GetValue(x, null)));
                            }

                            var cedulaPQDetalle = new CedulaMensualDetalleInstitucionesDTO();
                            cedulaPQDetalle.cuenta = gbCC.First().ctaAbonoBanco;
                            cedulaPQDetalle.scta = gbCC.First().sctaAbonoBanco;
                            cedulaPQDetalle.sscta = gbCC.First().ssctaAbonoBanco;
                            cedulaPQDetalle.descripcion = gbBanco.First().banco.Nombre + " [" + gbCC.Key + "]";
                            cedulaPQDetalle.saldoInicial = 0M;
                            cedulaPQDetalle.cargos = _cargosContabilidadPQ;
                            cedulaPQDetalle.abonos = _abonosContabilidadPQ;
                            cedulaPQDetalle.saldoActual = _cargosContabilidadPQ + _abonosContabilidadPQ;
                            cedulaPQDetalle.saldoActualSigoplan = gbMoneda.Key == (int)TipoMonedaEnum.MN ? (gbCC.Sum(x => x.importe) - gbCC.Sum(x => x.abonos.Sum(y => y.importe))) * -1 :
                                ((gbCC.Sum(x => x.importe) + gbCC.Sum(x => x.abonos.Sum(y => y.importe))) * tipoCambio.tipo_cambio) * -1;
                            cedulaPQDetalle.diferencia = cedulaPQDetalle.saldoActual - cedulaPQDetalle.saldoActualSigoplan;
                            detalleCortoPlazo.Add(cedulaPQDetalle);

                            _contabilidadCP += cedulaPQDetalle.saldoActual;
                            _nacionalCP += gbMoneda.Key == (int)TipoMonedaEnum.MN ? Math.Abs(cedulaPQDetalle.saldoActualSigoplan) + (gbCC.Sum(x => x.abonos.Sum(y => y.importe)) * 2) : 0M;
                            _dolaresCP += gbMoneda.Key == (int)TipoMonedaEnum.USD ? Math.Abs(cedulaPQDetalle.saldoActualSigoplan) + (gbCC.Sum(x => x.abonos.Sum(y => y.importe)) * 2): 0M;                            
                        }
                    }

                    #region saldos de contabilidad que no estan registrados en sigoplan PQ
                    var query_sc_salcont_ccs = new OdbcConsultaDTO();

                    query_sc_salcont_ccs.consulta = string.Format
                        (
                            @"SELECT
                                        *
                                    FROM
                                        sc_salcont_cc
                                    WHERE
                                        cta = ? AND
                                        scta = ? AND
                                        sscta = ? AND
                                        cc not in {0}",
                                    ccPQenSigoplan.ToParamInValue()
                        );

                    query_sc_salcont_ccs.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "cta",
                        tipo = OdbcType.Int,
                        valor = gbBanco.First().ctaAbonoBanco
                    });
                    query_sc_salcont_ccs.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "scta",
                        tipo = OdbcType.Int,
                        valor = gbBanco.First().sctaAbonoBanco
                    });
                    query_sc_salcont_ccs.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "sscta",
                        tipo = OdbcType.Int,
                        valor = gbBanco.First().ssctaAbonoBanco
                    });
                    query_sc_salcont_ccs.parametros.AddRange(ccPQenSigoplan.Select(cc => new OdbcParameterDTO
                    {
                        nombre = "cc",
                        tipo = OdbcType.NVarChar,
                        valor = cc
                    }));

                    var sc_salcont_ccs = _contextEnkontrol.Select<ActivoFijoSaldosContablesDTO>(productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, query_sc_salcont_ccs);
                    var _ccCargo = 0M;
                    var _ccAbono = 0M;

                    for (int mes = 1; mes <= 12; mes++)
                    {
                        var mesCargo = Enum.GetName(typeof(Core.Enum.Generico.Fecha.MesCargoEnum), mes);
                        var mesAbono = Enum.GetName(typeof(Core.Enum.Generico.Fecha.MesAbonoEnum), mes);

                        _ccCargo += sc_salcont_ccs.Where(x => x.Year < fechaCorte.Year).Sum(x => Convert.ToDecimal(x.GetType().GetProperty(mesCargo).GetValue(x, null)));
                        _ccAbono += sc_salcont_ccs.Where(x => x.Year < fechaCorte.Year).Sum(x => Convert.ToDecimal(x.GetType().GetProperty(mesAbono).GetValue(x, null)));
                    }

                    for (int mes = 1; mes <= fechaCorte.Month; mes++)
                    {
                        var mesCargo = Enum.GetName(typeof(Core.Enum.Generico.Fecha.MesCargoEnum), mes);
                        var mesAbono = Enum.GetName(typeof(Core.Enum.Generico.Fecha.MesAbonoEnum), mes);

                        _ccCargo += sc_salcont_ccs.Where(x => x.Year == fechaCorte.Year).Sum(x => Convert.ToDecimal(x.GetType().GetProperty(mesCargo).GetValue(x, null)));
                        _ccAbono += sc_salcont_ccs.Where(x => x.Year == fechaCorte.Year).Sum(x => Convert.ToDecimal(x.GetType().GetProperty(mesAbono).GetValue(x, null)));
                    }
                    #endregion

                    var cedulaMensualPQ = new CedulaMensualDTO();
                    cedulaMensualPQ.financiera = gbBanco.First().banco.Nombre;
                    //cedulaMensualPQ.nacionalCortoPlazo = gbBanco.Where(x => x.moneda.esMXN).Sum(x => x.importe);
                    cedulaMensualPQ.nacionalCortoPlazo = _nacionalCP;
                    //cedulaMensualPQ.dolaresCortoPlazo = gbBanco.Where(x => !x.moneda.esMXN).Sum(x => x.importe) * tipoCambio.tipo_cambio;
                    cedulaMensualPQ.dolaresCortoPlazo = _dolaresCP;
                    cedulaMensualPQ.contabilidadCortoPlazo = Math.Abs(_contabilidadCP) + Math.Abs(_ccCargo + _ccAbono);
                    cedulaMensualPQ.sigoplanCortoPlazo = _nacionalCP + _dolaresCP;
                    cedulaMensualPQ.diferenciaCortoPlazo = cedulaMensualPQ.contabilidadCortoPlazo - cedulaMensualPQ.sigoplanCortoPlazo;

                    cedulaPQ.Add(cedulaMensualPQ);
                }
                #endregion

                r.Add(SUCCESS, true);
                r.Add(ITEMS, cedula);
                r.Add("pqs", cedulaPQ);
                r.Add("detalleCortoPlazo", detalleCortoPlazo);
                r.Add("detalleLargoPlazo", detalleLargoPlazo);
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, ex.Message);
            }

            return r;
        }
        #endregion

        #region REPORTE SALDO PENDIENTE POR PROYECTO
        public Dictionary<string, object> ObtenerCboCC(List<int> lstDivisionID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                int idEmpresa = vSesiones.sesionEmpresaActual;

                List<int> lstDivisionesID = new List<int>();
                if (lstDivisionID != null)
                {
                    for (int i = 0; i < lstDivisionID.Count(); i++)
                    {
                        lstDivisionesID.Add(lstDivisionID[i]);
                    }
                }

                List<string> lstCC = _context.tblAF_DxP_Divisiones_Proyecto.Where(x => (lstDivisionesID.Count() > 0 ? lstDivisionesID.Contains(x.divisionID) : true) && x.esActivo).Select(x => x.cc).ToList();

                List<ComboDTO> lstCboCC = _context.tblP_CC.Where(x => lstCC.Count() > 0 ? lstCC.Contains(x.cc) : true).Select(x => new ComboDTO
                {
                    Value = x.cc,
                    Text = idEmpresa == 1 ? x.cc + " - " + x.descripcion : x.areaCuenta + " - " + x.descripcion
                }).ToList();
                result.Add(ITEMS, lstCboCC);
                result.Add(MESSAGE, true);
            }
            catch (Exception e)
            {
                LogError(2, 0, "ContratosController", "ObtenerCboDivisiones", e, AccionEnum.CONSULTA, 0, 0);
                result.Add(SUCCESS, false);
                return null;
            }
            return result;
        }

        public Dictionary<string, object> ObtenerCboDivisiones()
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstCboDivision = _context.tblAF_DxP_Divisiones.Where(x => x.esActivo).Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = !string.IsNullOrEmpty(x.abreviacion) ? x.abreviacion : x.nombre
                }).ToList();
                result.Add(ITEMS, lstCboDivision);
                result.Add(MESSAGE, true);
            }
            catch (Exception e)
            {
                LogError(2, 0, "ContratosController", "ObtenerCboDivisiones", e, AccionEnum.CONSULTA, 0, 0);
                result.Add(MESSAGE, false);
                return null;
            }
            return result;
        }

        //public List<adeudosDTO> ObtenerListadoDivisiones(adeudosDTO objDivision)
        //{
        //    try
        //    {
        //        //List<tblP_CC> objCC = _context.tblP_CC.Where(x => x.estatus).ToList(); //ACTIVO
        //        List<tblP_CC> objCC = _context.tblP_CC.ToList();

        //        //List<tblM_CatMaquina> objMaquinas = _context.tblM_CatMaquina.Where(x => x.estatus == 1).ToList(); // ACTIVO
        //        List<tblM_CatMaquina> objMaquinas = _context.tblM_CatMaquina.ToList();

        //        //List<tblAF_DxP_ContratoMaquina> objContratoMaquina = _context.tblAF_DxP_ContratoMaquinas.Where(x => x.Estatus).ToList(); //ACTIVO
        //        List<tblAF_DxP_ContratoMaquina> objContratoMaquina = _context.tblAF_DxP_ContratoMaquinas.ToList(); 

        //        //List<tblAF_DxP_ContratoMaquinaDetalle> objMaquinaDetalle = _context.tblAF_DxP_ContratoMaquinasDetalle.Where(x => x.Estatus).ToList(); //ACTIVO
        //        List<tblAF_DxP_ContratoMaquinaDetalle> objMaquinaDetalle = _context.tblAF_DxP_ContratoMaquinasDetalle.ToList();

        //        List<string> strCC = new List<string>();
        //        if (objDivision.lstCC != null)
        //        {
        //            for (int i = 0; i < objDivision.lstCC.Count(); i++)
        //            {
        //                strCC.Add(objDivision.lstCC[i]);
        //            }
        //        }

        //        List<tblAF_DxP_Divisiones> lstCatDivisiones = _context.tblAF_DxP_Divisiones.ToList();
        //        List<adeudosDTO> lstDivisiones = _context.tblAF_DxP_Divisiones_Proyecto
        //            .Where(x => strCC.Count() > 0 ? strCC.Contains(x.cc) : true).ToList()
        //            .Select(x =>
        //            {
        //                //SE OBTIENE LAS AREAS CUENTAS DE LOS CC SELECCIOADOS
        //                tblP_CC ac = objCC.FirstOrDefault(y => y.cc == x.cc);
        //                decimal importe = 0, importeNoPagado = 0;
        //                string areaCuenta = string.Empty;
        //                string _cc = string.Empty, _descripcion = string.Empty, _areaCuenta = string.Empty;
        //                if (ac != null)
        //                {
        //                    if (!string.IsNullOrEmpty(ac.areaCuenta))
        //                        areaCuenta = ac.areaCuenta;
        //                    else
        //                        throw new Exception("No se encuentra el area cuenta: " + ac.areaCuenta + ".");

        //                    //SE OBTIENE LOS ID DE LA MAQUINA
        //                    List<int> lstMaquinasID = new List<int>();
        //                    if (!string.IsNullOrEmpty(areaCuenta))
        //                    {
        //                        lstMaquinasID = objMaquinas.Where(s => s.centro_costos == areaCuenta).Select(s => s.id).ToList();
        //                    }

        //                    //SE OBTIENE LOS ID DE LOS CONTRATOS
        //                    List<int> lstContratosID = new List<int>();
        //                    if (lstMaquinasID.Count() > 0)
        //                    {
        //                        lstContratosID = objContratoMaquina.Where(k => lstMaquinasID.Contains(k.MaquinaId)).Select(s => s.Id).ToList();
        //                    }

        //                    //SE OBTIENE EL DETALLE DEL CONTRATO
        //                    if (lstContratosID.Count() > 0)
        //                    {
        //                        List<tblAF_DxP_ContratoMaquinaDetalle> lstMaquinaDetalle = objMaquinaDetalle.Where(s => lstContratosID.Contains(s.ContratoMaquinaId)).ToList();
        //                        if (lstMaquinaDetalle.Count() > 0)
        //                        {
        //                            //importe = lstMaquinaDetalle.Where(w => w.ContratoMaquina.Contrato.TipoCambio != 0).Sum(f => f.Importe * f.ContratoMaquina.Contrato.TipoCambio);
        //                            importeNoPagado = lstMaquinaDetalle.Where(w => w.ContratoMaquina.Contrato.TipoCambio != 0 && !w.Pagado).Sum(t => t.Importe * t.ContratoMaquina.Contrato.TipoCambio);
        //                        }
        //                    }
        //                    _cc = ac.cc != null ? ac.cc : string.Empty;
        //                    _descripcion = ac.descripcion != null ? ac.descripcion : string.Empty;
        //                    _areaCuenta = ac.areaCuenta != null ? ac.areaCuenta : string.Empty;
        //                }

        //                return new adeudosDTO
        //                {
        //                    division = GetDivision(x.divisionID, lstCatDivisiones),
        //                    cc = vSesiones.sesionEmpresaActual == 1 ? _cc + " - " + _descripcion : _areaCuenta + " - " + _descripcion,
        //                    saldoPendiente = importeNoPagado,
        //                    isAdmin = x.isAdmin ? "ADMIN" : "OBRA"
        //                };

        //            }).ToList();

        //        return lstDivisiones;
        //    }
        //    catch (Exception e)
        //    {
        //        LogError(2, 0, "ContratosController", "DocumentosXPagarDAO", e, AccionEnum.CONSULTA, 0, objDivision);
        //        return null;
        //    }
        //}

        public Dictionary<string, object> ObtenerListadoDivisiones(adeudosDTO objDivision)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                //SE OBTIENE LOS CC SELECCIONADOS DESDE EL FILTRO
                List<string> lstCC = new List<string>();
                if (objDivision.lstCC != null)
                {
                    for (int i = 0; i < objDivision.lstCC.Count(); i++)
                    {

                        lstCC.Add(objDivision.lstCC[i]);
                    }
                }

                //SE OBTIENE LOS ID'S DE LOS NUMERO ECONOMICOS QUE ESTEN DENTRO DE LOS CC SELECCIONADOS.
                List<string> lstAreaCuentas = new List<string>();
                lstAreaCuentas = _context.tblP_CC.Where(x => lstCC.Contains(x.cc)).Select(x => x.areaCuenta).ToList();
                if (lstAreaCuentas.Contains("14-1"))
                {
                    lstAreaCuentas.Add("997");
                    lstAreaCuentas.Remove("200");
                    lstAreaCuentas.Remove("14-1");
                }
                List<int> lstMaquinasID = new List<int>();
                using (var _contextArr = new MainContext((int)EmpresaEnum.Arrendadora))
                {
                    lstMaquinasID = _contextArr.tblM_CatMaquina.Where(x => lstAreaCuentas.Contains(x.centro_costos)).Select(x => x.id).ToList();
                }
                List<adeudosDTO> listaAdeudosMXN = new List<adeudosDTO>();
                List<adeudosDTO> listaAdeudosDOL = new List<adeudosDTO>();

                #region INSTITUCIONES
                List<int> lstInstituciones = new List<int>();
                if ((int)vSesiones.sesionEmpresaActual == 1)
                {
                    lstInstituciones.Add(1);
                    lstInstituciones.Add(2);
                    lstInstituciones.Add(1002);
                    lstInstituciones.Add(1003);
                }
                else
                {
                    lstInstituciones.Add(8);
                    lstInstituciones.Add(9);
                    lstInstituciones.Add(11);
                    lstInstituciones.Add(12);
                }
                #endregion

                #region PESOS MX
                var reporte = _context.tblAF_DxP_ContratoMaquinas.Where(r => lstInstituciones.Contains(r.Contrato.InstitucionId) && r.Contrato.monedaContrato == 1 && lstMaquinasID.Contains(r.MaquinaId)).ToList();

                foreach (var r in reporte)
                {
                    adeudosDTO objNew = new adeudosDTO();
                    var contratoDetalleMaquina = _context.tblAF_DxP_ContratoMaquinasDetalle.Where(c => c.ContratoMaquinaId == r.Id).ToList();
                    var contrato = contratoDetalleMaquina.First().ContratoDetalle.Contrato;

                    objNew.cc = r.Maquina.centro_costos;
                    objNew.proveedor = r.Contrato.Institucion.Nombre;
                    objNew.tipoFinanciamiento = r.Contrato.arrendamientoPuro ? "AP" : "AF";
                    objNew.contrato = r.Contrato.Folio;
                    objNew.noEconomico = r.Maquina.noEconomico;
                    objNew.fechaInicio = r.Contrato.FechaInicio.ToShortDateString();
                    objNew.fechaFin = r.Contrato.FechaInicio.AddMonths(r.Contrato.Plazo).ToShortDateString();
                    objNew.tasaInteres = r.Contrato.TasaInteres; //Tasa Intereses
                    objNew.valorFinanciado = contratoDetalleMaquina.Where(f => f.ContratoMaquinaId == r.Id).Sum(f => f.Importe); //  Total Deuda
                    objNew.moneda = r.Contrato.monedaContrato == 1 ? "MN" : "  USD";//Moneda
                    objNew.pagoMensual = contratoDetalleMaquina.FirstOrDefault(f => f.ContratoMaquinaId == r.Id).Importe; //Pago Mensual
                    objNew.plazo = r.Contrato.Plazo;//Plazo
                    objNew.intereses = contratoDetalleMaquina.FirstOrDefault(f => f.ContratoMaquinaId == r.Id).Intereses;//Intereses
                    objNew.ivaSCapital = contratoDetalleMaquina.FirstOrDefault(f => f.ContratoMaquinaId == r.Id).IvaSCapital; //Iva SCapital
                    objNew.ivaIntereses = contratoDetalleMaquina.FirstOrDefault(f => f.ContratoMaquinaId == r.Id).IvaIntereses; //Iva Intereses
                    objNew.fechaPago = getFechaPagoTipo(r.Contrato.TipoFechaVencimiento.Id, r.Contrato.FechaVencimiento);

                    int PagosRealizados = 0;
                    int PagosPendientes = 0;
                    decimal ImportePagado = 0;
                    decimal importePendiente = 0;

                    if (contrato.Terminado)
                    {
                        PagosRealizados = contratoDetalleMaquina.Where(w => w.Pagado).Count();
                        PagosPendientes = 0;
                        ImportePagado = contratoDetalleMaquina.Where(w => w.Pagado && w.ContratoMaquina.Contrato.TipoCambio == 1).Sum(s => s.Importe);
                        importePendiente = 0;

                        objNew.pagoRealizados = PagosRealizados; //Pago Efe
                        objNew.importePagado = ImportePagado; //Importe Pagado
                        objNew.pagosPendientes = PagosPendientes; //Pagos Pendientes
                        objNew.saldoPendiente = 0; //Saldo Pendiente

                        objNew.saldoLP = 0;
                        objNew.saldoCP = 0;
                        objNew.cargoObra = _context.tblP_CC.FirstOrDefault(f => f.areaCuenta == r.Maquina.centro_costos) != null ? _context.tblP_CC.FirstOrDefault(f => f.areaCuenta == r.Maquina.centro_costos).descripcion : "MAQUINARIA NO ASIGNADA A OBRA";
                    }
                    else
                    {
                        contratoDetalleMaquina.ForEach(s =>
                        {
                            if (s.Pagado)
                            {
                                ImportePagado += s.Importe;
                                PagosRealizados++;
                            }
                            else
                            {
                                importePendiente += s.Importe;
                                PagosPendientes++;
                            }
                        });

                        objNew.pagoRealizados = PagosRealizados; //Pago Efe
                        objNew.importePagado = ImportePagado; //Importe Pagado
                        objNew.pagosPendientes = PagosPendientes; //Pagos Pendientes
                        objNew.saldoPendiente = objNew.valorFinanciado - ImportePagado; //Saldo Pendiente

                        objNew.saldoLP = contratoDetalleMaquina.Where(f => f.FechaVencimiento.Year > DateTime.Now.Year).Sum(s => s.Importe);
                        objNew.saldoCP = Math.Abs(objNew.saldoPendiente - objNew.saldoLP);
                        objNew.cargoObra = _context.tblP_CC.FirstOrDefault(f => f.areaCuenta == r.Maquina.centro_costos) != null ? _context.tblP_CC.FirstOrDefault(f => f.areaCuenta == r.Maquina.centro_costos).descripcion : "MAQUINARIA NO ASIGNADA A OBRA";
                    }
                    listaAdeudosMXN.Add(objNew);
                }
                #endregion

                #region DOLARES
                //var reporteDol = _context.tblAF_DxP_ContratoMaquinas.Where(r => r.Contrato.InstitucionId > 7 && r.Contrato.InstitucionId < 13 && r.Contrato.monedaContrato == 2 && lstMaquinasID.Contains(r.MaquinaId) && r.MaquinaId == 6685).ToList();
                var reporteDol = _context.tblAF_DxP_ContratoMaquinas.Where(r => lstInstituciones.Contains(r.Contrato.InstitucionId) && r.Contrato.monedaContrato == 2 && lstMaquinasID.Contains(r.MaquinaId)).ToList();

                foreach (var r in reporteDol)
                {
                    if (r.MaquinaId == 6685)
                    {
                        int i = 0;
                    }

                    adeudosDTO objNew = new adeudosDTO();
                    var contratoDetalleMaquina = _context.tblAF_DxP_ContratoMaquinasDetalle.Where(c => c.ContratoMaquinaId == r.Id).ToList();
                    var contrato = contratoDetalleMaquina.First().ContratoDetalle.Contrato;

                    objNew.cc = r.Maquina.centro_costos;
                    objNew.proveedor = r.Contrato.Institucion.Nombre;
                    objNew.tipoFinanciamiento = r.Contrato.arrendamientoPuro ? "AP" : "AF";
                    objNew.contrato = r.Contrato.Folio;
                    objNew.noEconomico = r.Maquina.noEconomico;
                    objNew.fechaInicio = r.Contrato.FechaInicio.ToShortDateString();
                    objNew.fechaFin = r.Contrato.FechaInicio.AddMonths(r.Contrato.Plazo).ToShortDateString();
                    objNew.tasaInteres = r.Contrato.TasaInteres; //Tasa Intereses
                    objNew.valorFinanciado = contratoDetalleMaquina.Where(f => f.ContratoMaquinaId == r.Id).Sum(f => f.Importe); //  Total Deuda
                    objNew.moneda = r.Contrato.monedaContrato == 1 ? "MN" : "  USD";//Moneda
                    objNew.pagoMensual = contratoDetalleMaquina.FirstOrDefault(f => f.ContratoMaquinaId == r.Id).Importe; //Pago Mensual
                    objNew.plazo = r.Contrato.Plazo;//Plazo
                    objNew.intereses = contratoDetalleMaquina.FirstOrDefault(f => f.ContratoMaquinaId == r.Id).Intereses;//Intereses
                    objNew.ivaSCapital = contratoDetalleMaquina.FirstOrDefault(f => f.ContratoMaquinaId == r.Id).IvaSCapital; //Iva SCapital
                    objNew.ivaIntereses = contratoDetalleMaquina.FirstOrDefault(f => f.ContratoMaquinaId == r.Id).IvaIntereses; //Iva Intereses
                    objNew.fechaPago = getFechaPagoTipo(r.Contrato.TipoFechaVencimiento.Id, r.Contrato.FechaVencimiento);
                    decimal tipoCambio = r.Contrato.TipoCambio;

                    int PagosRealizados = 0;
                    int PagosPendientes = 0;
                    decimal ImportePagado = 0;
                    decimal importePendiente = 0;

                    if (contrato.Terminado)
                    {
                        PagosRealizados = contratoDetalleMaquina.Where(w => w.Pagado).Count();
                        PagosPendientes = 0;
                        ImportePagado = contratoDetalleMaquina.Where(w => w.Pagado && w.ContratoMaquina.Contrato.TipoCambio == 1).Sum(s => s.Importe);
                        importePendiente = 0;

                        objNew.pagoRealizados = PagosRealizados; //Pago Efe
                        objNew.importePagado = ImportePagado; //Importe Pagado
                        objNew.pagosPendientes = PagosPendientes; //Pagos Pendientes
                        objNew.saldoPendiente = 0; //Saldo Pendiente

                        objNew.saldoLP = 0;
                        objNew.saldoCP = 0;
                        objNew.cargoObra = _context.tblP_CC.FirstOrDefault(f => f.areaCuenta == r.Maquina.centro_costos) != null ? _context.tblP_CC.FirstOrDefault(f => f.areaCuenta == r.Maquina.centro_costos).descripcion : "MAQUINARIA NO ASIGNADA A OBRA";
                    }
                    else
                    {
                        contratoDetalleMaquina.ForEach(s =>
                        {
                            //if (s.Pagado)
                            if (s.FechaVencimiento.Date <= DateTime.Now && s.Pagado)
                            {
                                ImportePagado += s.Importe;
                                PagosRealizados++;
                            }
                            else
                            {
                                importePendiente += s.Importe;
                                PagosPendientes++;
                            }
                        });

                        objNew.pagoRealizados = PagosRealizados; //Pago Efe
                        objNew.importePagado = ImportePagado; //Importe Pagado
                        objNew.pagosPendientes = PagosPendientes; //Pagos Pendientes
                        objNew.saldoPendiente = (objNew.valorFinanciado - ImportePagado) * tipoCambio; //Saldo Pendiente CONVERSIÓN A PESOS MXN
                        //objNew.saldoPendiente = objNew.valorFinanciado - ImportePagado; //Saldo Pendiente

                        objNew.saldoLP = contratoDetalleMaquina.Where(f => f.FechaVencimiento.Year > DateTime.Now.Year).Sum(s => s.Importe);
                        objNew.saldoCP = Math.Abs(objNew.saldoPendiente - objNew.saldoLP);
                        objNew.cargoObra = _context.tblP_CC.FirstOrDefault(f => f.areaCuenta == r.Maquina.centro_costos) != null ? _context.tblP_CC.FirstOrDefault(f => f.areaCuenta == r.Maquina.centro_costos).descripcion : "MAQUINARIA NO ASIGNADA A OBRA";
                    }
                    listaAdeudosDOL.Add(objNew);
                }
                #endregion

                List<adeudosDTO> lista = new List<adeudosDTO>();
                lista.AddRange(listaAdeudosMXN);
                lista.AddRange(listaAdeudosDOL);

                List<tblAF_DxP_Divisiones> lstCatDivisiones = _context.tblAF_DxP_Divisiones.Where(x => x.esActivo).ToList();
                List<tblAF_DxP_Divisiones_Proyecto> lstDivisionesProyecto = _context.tblAF_DxP_Divisiones_Proyecto.Where(x => x.esActivo).ToList();

                var t = lista.GroupBy(x => x.cc).Select(x => new adeudosDTO
                //var t = listaAdeudos.Select(x => new adeudosDTO
                {
                    cc = GetDescripcionCC(x.Key),
                    division = GetDivision(x.Key, lstCatDivisiones.ToList(), lstDivisionesProyecto.ToList()),
                    saldoPendiente = x.Sum(y => y.saldoPendiente)
                }).ToList();

                result.Add("reporte", t);
                //result.Add("reporte", listaAdeudos);

                result.Add(SUCCESS, true);
                return result;
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, e.Message);
                return result;
            }
        }

        private string GetDivision(string areaCuenta, List<tblAF_DxP_Divisiones> lstCatDivisiones, List<tblAF_DxP_Divisiones_Proyecto> lstDivisionesProyecto)
        {
            try
            {
                string division = string.Empty;
                if (lstCatDivisiones.Count() > 0 && lstDivisionesProyecto.Count() > 0 && areaCuenta != "997")
                {
                    List<tblP_CC> _cc = _context.tblP_CC.Where(x => x.areaCuenta == areaCuenta).ToList();
                    string cc = string.Empty;
                    if (_cc.Count() > 0)
                        cc = _cc[0].cc;

                    int divisionID = lstDivisionesProyecto.Where(x => x.cc == cc).Select(x => x.divisionID).First();

                    List<tblAF_DxP_Divisiones> objDivision = lstCatDivisiones.Where(x => x.id == divisionID).ToList();
                    if (objDivision.Count() > 0)
                        division = !string.IsNullOrEmpty(objDivision[0].abreviacion) ? objDivision[0].abreviacion : objDivision[0].nombre;
                }
                else
                    division = "ADMIN";

                return !string.IsNullOrEmpty(division) ? division : "N/A";
            }
            catch (Exception)
            {
                return "N/A";
            }
        }

        private string GetDescripcionCC(string cc)
        {
            string _cc = string.Empty, _descripcion = string.Empty;
            if (!string.IsNullOrEmpty(cc))
            {
                List<tblP_CC> lstCC = _context.tblP_CC.Where(x => x.areaCuenta == cc).ToList();
                if (lstCC.Count() > 0)
                {
                    if ((int)vSesiones.sesionEmpresaActual == 1)
                        _cc = lstCC[0].cc;
                    else
                        _cc = lstCC[0].areaCuenta;

                    _descripcion = lstCC[0].descripcion;
                }
            }
            return cc == "997" ? "14-1 - MAQUINARIA NO ASIGNADA A OBRA" : _cc + " - " + _descripcion;
        }
        #endregion

        #region CATAGOLO DIVISION
        public List<CatDivisionesDTO> GetDivisiones()
        {
            try
            {
                List<CatDivisionesDTO> lstDivisiones = _context.tblAF_DxP_Divisiones.Where(x => x.esActivo).ToList().Select(x => new CatDivisionesDTO
                {
                    id = x.id,
                    nombre = x.nombre,
                    abreviacion = x.abreviacion
                }).ToList();
                return lstDivisiones;
            }
            catch (Exception e)
            {
                LogError(2, 0, "ContratosController", "GetDivisiones", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }


        public CatDivisionesDTO GuardarDivisiones(tblAF_DxP_Divisiones parametros)
        {
            CatDivisionesDTO objDivisiones = new CatDivisionesDTO();
            try
            {
                tblAF_DxP_Divisiones obj = _context.tblAF_DxP_Divisiones.Where(r => r.nombre == parametros.nombre && r.esActivo).FirstOrDefault();
                if (obj == null)
                {
                    obj = new tblAF_DxP_Divisiones();
                    obj.nombre = parametros.nombre.ToUpper().Trim();
                    if (parametros.abreviacion != null)
                    {
                        obj.abreviacion = parametros.abreviacion.ToUpper().Trim();
                    }
                    obj.esActivo = parametros.esActivo;
                    _context.tblAF_DxP_Divisiones.Add(obj);
                    _context.SaveChanges();
                    objDivisiones.estatus = 1;
                    objDivisiones.mensaje = "La division se a registrado con exito!!";
                }
                else
                {
                    objDivisiones.estatus = 2;
                    objDivisiones.mensaje = "Esta division ya se encuentra registrada!!!";
                }

                return objDivisiones;
            }
            catch (Exception e)
            {
                objDivisiones.estatus = 3;
                objDivisiones.mensaje = "Ha ocurrido un error, favor de reportarse al departamento de T.I.";
                LogError(2, 0, "ContratosController", "GetDivisiones", e, AccionEnum.CONSULTA, 0, 0);
                return objDivisiones;
            }
        }

        public bool EditarDivisiones(tblAF_DxP_Divisiones parametros)
        {
            try
            {
                tblAF_DxP_Divisiones obj = _context.tblAF_DxP_Divisiones.Where(r => r.id == parametros.id).FirstOrDefault();
                obj.nombre = parametros.nombre.ToUpper().Trim();
                if (parametros.abreviacion != null)
                {
                    obj.abreviacion = parametros.abreviacion.ToUpper().Trim();
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                LogError(2, 0, "ContratosController", "GetDivisiones", e, AccionEnum.CONSULTA, 0, 0);
                return false;
            }
        }

        public bool EliminarDivisiones(int id)
        {
            try
            {
                tblAF_DxP_Divisiones obj = _context.tblAF_DxP_Divisiones.Where(r => r.id == id).FirstOrDefault();
                obj.esActivo = false;
                _context.SaveChanges();
                //_context.tblAF_DxP_Divisiones.Remove(obj);
                //_context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                LogError(2, 0, "ContratosController", "GetDivisiones", e, AccionEnum.CONSULTA, 0, 0);
                return false;
            }
        }

        #endregion

        #region DIVISIONES_PROYECTOS
        public List<Divisiones_ProyectosDTO> GetDivisiones_Proyectos()
        {
            try
            {
                List<Divisiones_ProyectosDTO> lstDivisiones_Proyectos = _context.tblAF_DxP_Divisiones_Proyecto.Where(r => r.esActivo).ToList().Select(x => new Divisiones_ProyectosDTO
                {
                    
                    id = x.id,
                    cc = x.cc,
                    descripcionCC = x.cc + " - " + _context.tblP_CC.Where(c => c.cc == x.cc).ToList().Select(s => s.descripcion).FirstOrDefault(),
                    divisionId = x.divisionID,
                    nombreDivision = _context.tblAF_DxP_Divisiones.Where(r => r.id == x.divisionID).ToList().Select(s => s.nombre).FirstOrDefault(),
                    abreviacion = _context.tblAF_DxP_Divisiones.Where(r => r.id == x.divisionID).ToList().Select(s => s.abreviacion).FirstOrDefault(),
                    isadmin = x.isAdmin,
                    esAdmin = x.isAdmin ? "ADMIN":"OBRA"                  
                }).ToList();
               
                return lstDivisiones_Proyectos;
            }
            catch (Exception e)
            {
                LogError(2, 0, "ContratosController", "Divisiones_Proyectos", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }


        //(objFiltro.isAdmin != null ? r.isAdmin == objFiltro.isAdmin : (r.isAdmin && !r.isAdmin)) &&
        public List<Divisiones_ProyectosDTO> GetDivisiones_ProyectosFitro(tblAF_DxP_Divisiones_Proyecto objFiltro)
        {
            try
            {
               
                List<Divisiones_ProyectosDTO> lstDivisiones_Proyectos = _context.tblAF_DxP_Divisiones_Proyecto.
                    Where(r => (!string.IsNullOrEmpty(objFiltro.cc) ? r.cc == objFiltro.cc : true) &&
                        (objFiltro.divisionID > 0 ? r.divisionID == objFiltro.divisionID : true) &&                                  
                        r.esActivo).ToList().Select(x => new Divisiones_ProyectosDTO
                        {

                            id = x.id,
                            cc = x.cc,
                            descripcionCC = x.cc + " - " + _context.tblP_CC.Where(c => c.cc == x.cc).ToList().Select(s => s.descripcion).FirstOrDefault(),
                            divisionId = x.divisionID,
                            nombreDivision = _context.tblAF_DxP_Divisiones.Where(r => r.id == x.divisionID).ToList().Select(s => s.nombre).FirstOrDefault(),
                            abreviacion = _context.tblAF_DxP_Divisiones.Where(r => r.id == x.divisionID).ToList().Select(s => s.abreviacion).FirstOrDefault(),
                            isadmin = x.isAdmin,
                            esAdmin = x.isAdmin ? "ADMIN" : "OBRA"
                        }).ToList();

                return lstDivisiones_Proyectos;
            }
            catch (Exception e)
            {
                LogError(2, 0, "ContratosController", "Divisiones_Proyectos", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public List<ComboDTO> GetCC()
        {
            try
            {
                var lstComboCC = _context.tblP_CC.Where(r => r.estatus).ToList().Select(x => new ComboDTO
                {
                    Value = x.cc,
                    Text = x.cc + " - " + x.descripcion,
                }).ToList();
                return lstComboCC;
            }
            catch (Exception e)
            {
                LogError(2, 0, "ContratosController", "Divisiones_Proyectos", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }


        public List<ComboDTO> GetCmbDivision()
        {
            try
            {
                var lstComboDivision = _context.tblAF_DxP_Divisiones.Where(r => r.esActivo).ToList().Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.nombre,
                }).ToList();
                return lstComboDivision;
            }
            catch (Exception e)
            {
                LogError(2, 0, "ContratosController", "Divisiones_Proyectos", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }


        public Divisiones_ProyectosDTO GuardarDivisiones_Proyectos(tblAF_DxP_Divisiones_Proyecto parametros)
        {
            Divisiones_ProyectosDTO objDivisionesProyectos = new Divisiones_ProyectosDTO();
            try
            {
                tblAF_DxP_Divisiones_Proyecto obj = _context.tblAF_DxP_Divisiones_Proyecto.Where(r => r.cc == parametros.cc && r.esActivo).FirstOrDefault();
                if (obj == null)
                {
                    // ESTO VA AAGREGAR
                    obj = new tblAF_DxP_Divisiones_Proyecto();
                    obj.esActivo = true;
                    obj.cc = parametros.cc;
                    obj.divisionID = parametros.divisionID;
                    obj.isAdmin = parametros.isAdmin;
                    _context.tblAF_DxP_Divisiones_Proyecto.Add(obj);
                    _context.SaveChanges();

                    objDivisionesProyectos.estatus = 1;
                    objDivisionesProyectos.mensaje = "La division se a registrado con exito!!";
                }
                else
                {
                    objDivisionesProyectos.estatus = 2;
                    objDivisionesProyectos.mensaje = "Esta division ya se encuentra registrada!!!";
                }

                return objDivisionesProyectos;
            }
            catch (Exception e)
            {
                objDivisionesProyectos.estatus = 3;
                objDivisionesProyectos.mensaje = "Ha ocurrido un error, favor de reportarse al departamento de T.I.";
                LogError(2, 0, "ContratosController", "Divisiones_Proyectos", e, AccionEnum.CONSULTA, 0, 0);
                return objDivisionesProyectos;
            }
        }


        public bool EliminarDivisionesProyectos(int id)
        {
            try
            {
                tblAF_DxP_Divisiones_Proyecto obj = _context.tblAF_DxP_Divisiones_Proyecto.Where(r => r.id == id).FirstOrDefault();
                obj.esActivo = false;
                _context.SaveChanges();
                //_context.tblAF_DxP_Divisiones.Remove(obj);
                //_context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                LogError(2, 0, "ContratosController", "GetDivisiones_Proyectos", e, AccionEnum.CONSULTA, 0, 0);
                return false;
            }
        }

        public bool EditarDivisionesProyectos(tblAF_DxP_Divisiones_Proyecto parametros)
        {
            try
            {
                tblAF_DxP_Divisiones_Proyecto obj = _context.tblAF_DxP_Divisiones_Proyecto.Where(r => r.id == parametros.id).FirstOrDefault();
                obj.cc = parametros.cc;
                obj.divisionID = parametros.divisionID;
                obj.isAdmin = parametros.isAdmin;
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                LogError(2, 0, "ContratosController", "GetDivisiones", e, AccionEnum.CONSULTA, 0, 0);
                return false;
            }
        }
        
        #endregion

        #region PQ
        public Dictionary<string, object> GuardarPQ(tblAF_DxP_PQ pq, HttpPostedFileBase archivo)
        {
            var r = new Dictionary<string, object>();

            using (var transaccionSP = _context.Database.BeginTransaction())
            {
                using (var conexionEK = new Conexion().ConexionEKAdm())
                {
                    using (var transaccionEK = conexionEK.BeginTransaction())
                    {
                        try
                        {
                            if (archivo != null)
                            {
                                #region archivo
                                var folder = "DOCUMENTOS_PQ";

                                var fechaArchivo = DateTime.Now.ToString("yyyy-MM-ddTHHmmssfff");
                                var ruta = archivoFS.getUrlDelServidor(1019) + folder + @"\";

                                var nombreArchivo = archivo.FileName;
                                var nombreArchivoSinExtension = System.IO.Path.GetFileNameWithoutExtension(nombreArchivo);
                                var extension = System.IO.Path.GetExtension(nombreArchivo);

#if DEBUG
                                var directorio = new DirectoryInfo(@"c:\DOCUMENTOS_POR_PAGAR\" + folder + @"\");
#else
                                var directorio = new DirectoryInfo(ruta);
#endif
                                if (!directorio.Exists)
                                {
                                    directorio.Create();
                                }

                                var pathCompleto = System.IO.Path.Combine(directorio.ToString(), nombreArchivoSinExtension + "_" + fechaArchivo + extension);

                                archivo.SaveAs(pathCompleto);

                                var datoArchivo = new tblAF_DxP_PQ_Archivo();
                                
                                datoArchivo.estatus = true;
                                datoArchivo.fechaCarga = DateTime.Now;
                                datoArchivo.fechaCreacion = datoArchivo.fechaCarga;
                                datoArchivo.fechaModificacion = datoArchivo.fechaCreacion;
                                datoArchivo.nombreArchivo = nombreArchivo;
                                datoArchivo.ubicacionArchivo = pathCompleto;
                                datoArchivo.usuarioCreacionId = vSesiones.sesionUsuarioDTO.id;
                                datoArchivo.usuarioModificacionId = datoArchivo.usuarioCreacionId;

                                _context.tblAF_DxP_PQ_Archivo.Add(datoArchivo);
                                _context.SaveChanges();

                                pq.archivoId = datoArchivo.id;
                                pq.archivoPQ = datoArchivo;
                                #endregion

                                if (pq.id == 0)
                                {
                                    pq.estatus = true;
                                    pq.fechaCreacion = DateTime.Now;
                                    pq.fechaModificacion = pq.fechaCreacion;
                                    pq.usuarioCreacionId = vSesiones.sesionUsuarioDTO.id;
                                    pq.usuarioModificacionId = pq.usuarioCreacionId;
                                    pq.tipoMovimientoId = (int)PQTipoMovimientoEnum.AltaNuevoPQ;
                                    pq.moneda = _context.tblC_TipoMoneda.First(f => f.id == pq.monedaId);

                                    var poliza_movpol = generarPolizaPQ(pq);

                                    _polizaEkFS.SetContext(conexionEK);
                                    _polizaEkFS.SetTransaccion(transaccionEK);
                                    string numeroPoliza = _polizaEkFS.GuardarPoliza(poliza_movpol.poliza, poliza_movpol.movimientos);
                                    //string numeroPoliza = "";

                                    _polizaSPFS.SetContext(_context);
                                    _polizaSPFS.SetTransaccion(transaccionSP);
                                    var polizaPoliza = numeroPoliza.Split(new string[] { "-" }, StringSplitOptions.None)[2];
                                    poliza_movpol.poliza.poliza = Convert.ToInt32(polizaPoliza);
                                    var resultadoSP = _polizaSPFS.GuardarPoliza(poliza_movpol.poliza, poliza_movpol.movimientos);

                                    _context.tblAF_DxP_PQ.Add(pq);
                                    _context.SaveChanges();

                                    pq.folio = pq.id;
                                    _context.SaveChanges();

                                    var infoAConciliar = GenerarInfoConcilia(poliza_movpol.movimientos, pq, poliza_movpol.poliza);

                                    foreach (var concilia in infoAConciliar)
                                    {
                                        _polizaEkFS.GuardarParaConciliar(concilia);
                                    }

                                    r.Add(MESSAGE, "Se registró el PQ, póliza: " + numeroPoliza);
                                }
                                else
                                {
                                    var pqEditar = _context.tblAF_DxP_PQ.First(f => f.id == pq.id);

                                    pqEditar.fechaModificacion = DateTime.Now;
                                    pqEditar.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                                    pqEditar.estatus = pq.estatus;

                                    pqEditar.archivoPQ.estatus = false;

                                    if (pq.estatus)
                                    {
                                        pqEditar.bancoId = pq.bancoId;
                                        pqEditar.ctaCargoBanco = pq.ctaCargoBanco;
                                        pqEditar.sctaCargoBanco = pq.sctaCargoBanco;
                                        pqEditar.ssctaCargoBanco = pq.ssctaCargoBanco;
                                        pqEditar.digitoCargoBanco = pq.digitoCargoBanco;
                                        pqEditar.ctaAbonoBanco = pq.ctaAbonoBanco;
                                        pqEditar.sctaAbonoBanco = pq.sctaAbonoBanco;
                                        pqEditar.ssctaAbonoBanco = pq.ssctaAbonoBanco;
                                        pqEditar.digitoAbonoBanco = pq.digitoAbonoBanco;
                                        pqEditar.cc = pq.cc;
                                        pqEditar.fechaFirma = pq.fechaFirma;
                                        pqEditar.fechaVencimiento = pq.fechaVencimiento;
                                        pqEditar.importe = pq.importe;
                                        pqEditar.interes = pq.interes;
                                        pqEditar.monedaId = pq.monedaId;
                                        pqEditar.tipoCambio = pq.tipoCambio;
                                        pqEditar.archivoId = datoArchivo.id;
                                        pqEditar.archivoPQ = datoArchivo;

                                        r.Add(MESSAGE, "Se actualizó el PQ");
                                    }
                                    else
                                    {
                                        r.Add(MESSAGE, "Se eliminó el PQ");
                                    }

                                    pq = pqEditar;

                                    _context.SaveChanges();
                                }

                                transaccionEK.Commit();
                                transaccionSP.Commit();

                                r.Add(SUCCESS, true);
                            }
                            else
                            {
                                r.Add(SUCCESS, false);
                                r.Add(MESSAGE, "Necesita cargar un archivo de contrato");
                            }
                        }
                        catch (Exception ex)
                        {
                            transaccionEK.Rollback();
                            transaccionSP.Rollback();

                            r.Add(SUCCESS, false);

                            if (r.ContainsKey(MESSAGE))
                            {
                                r[MESSAGE] = ex.Message;
                            }
                            else
                            {
                                r.Add(MESSAGE, ex.Message);
                            }

                            LogError(2, 0, "ContratosController", "RegistrarPQ", ex, AccionEnum.ACTUALIZAR, 0, pq);
                        }
                    }
                }
            }

            return r;
        }

        public Dictionary<string, object> GetMonedas()
        {
            var r = new Dictionary<string, object>();

            try
            {
                var monedas = _context.tblC_TipoMoneda.Select(m => new
                {
                    Value = m.id,
                    Text = m.nombreCorto,
                    Prefijo = m.esMXN
                }).OrderBy(o => o.Text);

                r.Add(SUCCESS, true);
                r.Add(ITEMS, monedas);
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, ex.Message);

                LogError(2, 0, "ContratosController", "GetMonedas", ex, AccionEnum.CONSULTA, 0, null);
            }

            return r;
        }

        public Dictionary<string, object> GetPQs(bool estatus, string fechaCorte)
        {
            var r = new Dictionary<string, object>();

            var objError = new tblAF_DxP_PQ();

            try
            {
                var fecha = Convert.ToDateTime(fechaCorte);

                var movimientosActivos = new List<int> { 1, 3, 5 };
                var movimientosLiquidados = new List<int> { 6 };

                var pqs = _context.tblAF_DxP_PQ
                    .Where(w =>
                        w.estatus &&
                        (
                            estatus ?
                                movimientosActivos.Contains(w.tipoMovimientoId) ||
                                (
                                    movimientosLiquidados.Contains(w.tipoMovimientoId) &&
                                    w.fechaLiquidacion.HasValue &&
                                    w.fechaLiquidacion.Value >= fecha
                                )
                                :
                                movimientosLiquidados.Contains(w.tipoMovimientoId)) && w.fechaFirma <= fecha).ToList();

                var infoPQs = new List<tblPQsDTO>();

                DateTime mesAnterior = new DateTime(fecha.Year, fecha.Month, 1).AddDays(-1);
                var tipoCambioMesAnterior = getTipoCambioDLLs(mesAnterior);
                var tipoCambioMesActual = getTipoCambioDLLs(new DateTime(fecha.Year, fecha.Month, DateTime.DaysInMonth(fecha.Year, fecha.Month)));

                foreach (var pq in pqs.OrderByDescending(o => o.folio))
                {
                    objError = pq;

                    var infoPQ = new tblPQsDTO();

                    infoPQ.id = pq.id;
                    infoPQ.folio = pq.folio;
                    infoPQ.banco = pq.banco.Nombre;
                    infoPQ.fechaFirma = pq.fechaFirma;
                    infoPQ.fechaVencimiento = pq.fechaVencimiento;
                    infoPQ.cc = pq.cc;
                    infoPQ.moneda = pq.moneda.nombreCorto;
                    infoPQ.importe = pq.importe + pq.abonos.Where(x => x.estatus).Sum(x => x.importe);
                    if (!pq.moneda.esMXN)
                    {
                        if (fecha == new DateTime(fecha.Year, fecha.Month, DateTime.DaysInMonth(fecha.Year, fecha.Month)))
                        {
                            infoPQ.importeMN = infoPQ.importe * tipoCambioMesActual.tipo_cambio;
                        }
                        else
                        {
                            if (new DateTime(pq.fechaFirma.Year, pq.fechaFirma.Month, 1) < new DateTime(fecha.Year, fecha.Month, 1))
                            {
                                infoPQ.importeMN = infoPQ.importe * tipoCambioMesAnterior.tipo_cambio;
                            }
                            else
                            {
                                infoPQ.importeMN = infoPQ.importe * pq.tipoCambio.Value;
                            }
                        }
                    }
                    else
                    {
                        infoPQ.importeMN = infoPQ.importe;
                    }
                    infoPQ.interes = pq.interes;
                    infoPQ.interesDiario = (infoPQ.importeMN * (infoPQ.interes / 100)) / 360;
                    infoPQ.interesSemanal = ((infoPQ.importeMN * (infoPQ.interes / 100)) / 360) * 7;
                    infoPQ.fechaCorte = DateTime.Now;
                    infoPQ.interesAcumulado = (infoPQ.fechaCorte - infoPQ.fechaFirma).Days * infoPQ.interesDiario;
                    infoPQ.tipoMovimientoId = pq.tipoMovimientoId;
                    infoPQ.fechaLiquidacion = pq.fechaLiquidacion;
                    infoPQ.tieneAbono = pq.abonos.Count > 0;
                    infoPQ.poliza = pq.poliza;
                    infoPQ.folioInterno = pq.bancoId.ToString() + "-" + pq.folio.ToString();

                    infoPQs.Add(infoPQ);
                }

                infoPQs = infoPQs.Where(x => x.importe != 0 && x.interesAcumulado != 0 && x.importeMN != 0).ToList();

                r.Add(SUCCESS, true);
                r.Add(ITEMS, infoPQs);
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, ex.Message);

                LogError(2, 0, "ContratosController", "GetPQs", ex, AccionEnum.CONSULTA, 0, objError);
            }

            return r;
        }

        public Dictionary<string, object> GetPQ(int id)
        {
            var r = new Dictionary<string, object>();

            try
            {
                var contratoPQ = _context.tblAF_DxP_PQ.First(f => f.id == id);

                var pq = new PQDTO();

                var informacionCuentaAbono = cuentaFS.GetCuenta(contratoPQ.ctaAbonoBanco, contratoPQ.sctaAbonoBanco, contratoPQ.ssctaAbonoBanco) as catctaDTO;
                var informacionCuentaCargo = cuentaFS.GetCuenta(contratoPQ.ctaCargoBanco, contratoPQ.sctaCargoBanco, contratoPQ.ssctaCargoBanco) as catctaDTO;

                pq.id = contratoPQ.id;
                pq.ctaDescripcion = "[" + informacionCuentaAbono.cta + "-" + informacionCuentaAbono.scta + "-" + informacionCuentaAbono.sscta + "-" + informacionCuentaAbono.digito + "] " + informacionCuentaAbono.descripcion;
                pq.ctaRelacion = informacionCuentaAbono.cta + "-" + informacionCuentaAbono.scta + "-" + informacionCuentaAbono.sscta + "-" + informacionCuentaAbono.digito;
                pq.ctaCargoDescripcion = "[" + informacionCuentaCargo.cta + "-" + informacionCuentaCargo.scta + "-" + informacionCuentaCargo.sscta + "-" + informacionCuentaCargo.digito + "] " + informacionCuentaCargo.descripcion;
                pq.ctaCargoRelacion = informacionCuentaCargo.cta + "-" + informacionCuentaCargo.scta + "-" + informacionCuentaCargo.sscta + "-" + informacionCuentaCargo.digito;
                pq.bancoId = contratoPQ.bancoId;
                pq.fechaFirma = contratoPQ.fechaFirma;
                pq.fechaVencimiento = contratoPQ.fechaVencimiento;
                pq.cc = contratoPQ.cc;
                pq.monedaId = contratoPQ.monedaId;
                pq.importe = contratoPQ.importe;
                pq.interes = contratoPQ.interes;
                pq.tipoCambio = contratoPQ.tipoCambio;
                pq.importeMN = contratoPQ.tipoCambio.HasValue ? contratoPQ.tipoCambio.Value * contratoPQ.importe : contratoPQ.importe;

                r.Add(SUCCESS, true);
                r.Add(ITEMS, pq);
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, ex.Message);

                LogError(2, 0, "ContratosController", "GetPQ", ex, AccionEnum.CONSULTA, 0, new { id = id });
            }

            return r;
        }

        public Dictionary<string, object> ObtenerInstitucionesPQ()
        {
            var r = new Dictionary<string, object>();

            try
            {
                var instituciones = _context.tblAF_DxP_Instituciones.Where(x => x.Estatus && x.esPQ).Select(m => new ComboDTO
                {
                    Text = m.Nombre,
                    Value = m.Id.ToString(),
                    Prefijo = m.Id.ToString()
                }).ToList();

                r.Add(ITEMS, instituciones);
                r.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, "Error: " + ex.Message);
            }

            return r;
        }

        public Dictionary<string, object> GetPQLiquidar(int id)
        {
            var r = new Dictionary<string, object>();

            try
            {
                var movimientos = new List<PQPolizaDTO>();

                var pq = _context.tblAF_DxP_PQ.First(f => f.id == id);
                var infoCuentaAbono = cuentaFS.GetCuenta(pq.ctaAbonoBanco, pq.sctaAbonoBanco, pq.ssctaAbonoBanco) as catctaDTO;

                var movimiento1 = new PQPolizaDTO();
                movimiento1.cuenta = pq.ctaAbonoBanco + "-" + pq.sctaAbonoBanco + "-" + pq.ssctaAbonoBanco + "-" + pq.digitoAbonoBanco;
                movimiento1.cuentaDescripcion = "[" + movimiento1.cuenta + "] " + infoCuentaAbono.descripcion;
                movimiento1.tm = (int)TipoMovimientoEnum.Cargo;
                movimiento1.referencia = Convert.ToInt32(ObtenerReferencia(DateTime.Now, pq.ctaAbonoBanco));
                movimiento1.cc = pq.cc;

                string importeCorto = "";
                string escala = "";
                if (pq.importe >= 1000000)
                {
                    importeCorto = string.Format("{0:0.00}", pq.importe / 1000000);
                    escala = pq.moneda.esMXN ? "MDP" : "DLLS";
                }
                if (pq.importe >= 1000 && pq.importe < 1000000)
                {
                    importeCorto = string.Format("{0:0.00}", pq.importe / 1000);
                    escala = pq.moneda.esMXN ? "MIL PESOS" : "MIL DOLARES";
                }
                movimiento1.concepto = "LIQ DOC PQ " + importeCorto + " " + escala;

                movimiento1.monto = pq.importe;
                movimiento1.itm = 0;

                movimientos.Add(movimiento1);

                var infoCuentaProvisionInteres = cuentaFS.GetCuenta(2110, 1, 863) as catctaDTO;
                var cuentaInteres = _context.tblAF_DxP_InstitucionesCtasInteres.FirstOrDefault(x => x.institucionId == pq.bancoId);
                if (cuentaInteres != null)
                {
                    infoCuentaProvisionInteres.cta = cuentaInteres.cta;
                    infoCuentaProvisionInteres.descripcion = cuentaInteres.descripcion;
                    infoCuentaProvisionInteres.digito = cuentaInteres.digito;
                    infoCuentaProvisionInteres.scta = cuentaInteres.scta;
                    infoCuentaProvisionInteres.sscta = cuentaInteres.sscta;
                }

                var movimientoPI = new PQPolizaDTO();
                movimientoPI.cuenta = infoCuentaProvisionInteres.cta + "-" + infoCuentaProvisionInteres.scta + "-" + infoCuentaProvisionInteres.sscta + "-" + infoCuentaProvisionInteres.digito;
                movimientoPI.cuentaDescripcion = "[" + movimientoPI.cuenta + "] " + infoCuentaProvisionInteres.descripcion;
                movimientoPI.tm = (int)TipoMovimientoEnum.Cargo;
                movimientoPI.referencia = movimiento1.referencia;
                movimientoPI.cc = pq.cc;
                movimientoPI.concepto = movimiento1.concepto;
                movimientoPI.monto = 0M;
                movimientoPI.itm = 0;

                movimientos.Add(movimientoPI);

                var infoCuentaCargo = cuentaFS.GetCuenta(pq.ctaCargoBanco, pq.sctaCargoBanco, pq.ssctaCargoBanco) as catctaDTO;

                var movimiento2 = new PQPolizaDTO();
                movimiento2.cuenta = pq.ctaCargoBanco + "-" + pq.sctaCargoBanco + "-" + pq.ssctaCargoBanco + "-" + pq.digitoCargoBanco;
                movimiento2.cuentaDescripcion = "[" + movimiento2.cuenta + "] " + infoCuentaCargo.descripcion;
                movimiento2.tm = (int)TipoMovimientoEnum.Abono;
                movimiento2.referencia = movimientoPI.referencia;
                movimiento2.cc = pq.cc;
                movimiento2.concepto = movimiento1.concepto;
                movimiento2.monto = pq.importe * -1;
                movimiento2.itm = 69;

                movimientos.Add(movimiento2);

                if (!pq.moneda.esMXN)
                {
                    var ctaInstitucionComplementaria = _context.tblAF_DxP_RelInstitucionCta
                    .FirstOrDefault(f =>
                        f.institucionID == pq.bancoId &&
                        f.activo &&
                        f.complementaria &&
                        f.moneda == 2
                    );

                    var infoCuentaCargoComplementaria = cuentaFS.GetCuenta(ctaInstitucionComplementaria.cta, ctaInstitucionComplementaria.scta, ctaInstitucionComplementaria.sscta) as catctaDTO;

                    var movimiento22 = new PQPolizaDTO();
                    movimiento22.cuenta = infoCuentaCargoComplementaria.cta + "-" + infoCuentaCargoComplementaria.scta + "-" + infoCuentaCargoComplementaria.sscta + "-" + infoCuentaCargoComplementaria.digito;
                    movimiento22.cuentaDescripcion = "[" + movimiento22.cuenta + "] " + infoCuentaCargoComplementaria.descripcion;
                    movimiento22.tm = (int)TipoMovimientoEnum.Abono;
                    movimiento22.referencia = movimiento2.referencia;
                    movimiento22.cc = pq.cc;
                    movimiento22.concepto = movimiento1.concepto;
                    movimiento22.monto = pq.importe * -1;
                    movimiento22.itm = 69;

                    movimientos.Add(movimiento22);
                }

                var movimiento3 = new PQPolizaDTO();
                movimiento3.cuenta = movimiento2.cuenta;
                movimiento3.cuentaDescripcion = movimiento2.cuentaDescripcion;
                movimiento3.tm = (int)TipoMovimientoEnum.Abono;
                movimiento3.referencia = movimiento2.referencia + 1;
                movimiento3.cc = pq.cc;
                movimiento3.concepto = movimiento1.concepto;
                movimiento3.monto = -0M;
                movimiento3.itm = 68;

                movimientos.Add(movimiento3);

                if (!pq.moneda.esMXN)
                {
                    var ctaInstitucionComplementaria = _context.tblAF_DxP_RelInstitucionCta
                    .FirstOrDefault(f =>
                        f.institucionID == pq.bancoId &&
                        f.activo &&
                        f.complementaria &&
                        f.moneda == 2
                    );

                    var infoCuentaCargoComplementaria = cuentaFS.GetCuenta(ctaInstitucionComplementaria.cta, ctaInstitucionComplementaria.scta, ctaInstitucionComplementaria.sscta) as catctaDTO;

                    var movimiento33 = new PQPolizaDTO();
                    movimiento33.cuenta = infoCuentaCargoComplementaria.cta + "-" + infoCuentaCargoComplementaria.scta + "-" + infoCuentaCargoComplementaria.sscta + "-" + infoCuentaCargoComplementaria.digito;
                    movimiento33.cuentaDescripcion = "[" + movimiento33.cuenta + "] " + infoCuentaCargoComplementaria.descripcion;
                    movimiento33.tm = (int)TipoMovimientoEnum.Abono;
                    movimiento33.referencia = movimiento3.referencia;
                    movimiento33.cc = pq.cc;
                    movimiento33.concepto = movimiento1.concepto;
                    movimiento33.monto = -0M;
                    movimiento33.itm = 68;

                    movimientos.Add(movimiento33);
                }

                var infoCuenta5900 = cuentaFS.GetCuenta(5900, 5, 0) as catctaDTO;

                var movimiento4 = new PQPolizaDTO();
                movimiento4.tipoLinea = (int)PQLineaMovimientoEnum.PoderQuitarLinea;
                movimiento4.cuenta = 5900 + "-" + 5 + "-" + 0 + "-" + 9;
                movimiento4.cuentaDescripcion = "[" + movimiento4.cuenta + "] " + infoCuenta5900.descripcion;
                movimiento4.tm = (int)TipoMovimientoEnum.CargoRojo;
                movimiento4.referencia = movimiento1.referencia;
                movimiento4.cc = pq.cc;
                movimiento4.concepto = movimiento1.concepto;
                movimiento4.monto = -0M;
                movimiento4.itm = 0;

                movimientos.Add(movimiento4);

                r.Add(SUCCESS, true);
                r.Add(ITEMS, movimientos);
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, ex.Message);
                
                LogError(2, 0, "ContratosController", "GetPQLiquidar", ex, AccionEnum.CONSULTA, 0, id);
            }

            return r;
        }

        public Dictionary<string, object> GetPQRenovar(int id)
        {
            var r = new Dictionary<string, object>();

            try
            {
                var movimientos = new List<PQPolizaDTO>();

                var pq = _context.tblAF_DxP_PQ.First(f => f.id == id);
                var infoCuentaAbono = cuentaFS.GetCuenta(pq.ctaAbonoBanco, pq.sctaAbonoBanco, pq.ssctaAbonoBanco) as catctaDTO;

                var movimiento1 = new PQPolizaDTO();
                movimiento1.cuenta = pq.ctaAbonoBanco + "-" + pq.sctaAbonoBanco + "-" + pq.ssctaAbonoBanco + "-" + pq.digitoAbonoBanco;
                movimiento1.cuentaDescripcion = "[" + movimiento1.cuenta + "] " + infoCuentaAbono.descripcion;
                movimiento1.tm = (int)TipoMovimientoEnum.Cargo;
                movimiento1.referencia = Convert.ToInt32(ObtenerReferencia(DateTime.Now, pq.ctaAbonoBanco));
                movimiento1.cc = pq.cc;

                string importeCorto = "";
                string escala = "";
                if (pq.importe >= 1000000)
                {
                    importeCorto = string.Format("{0:0.00}", pq.importe / 1000000);
                    escala = pq.moneda.esMXN ? "MDP" : "DLLS";
                }
                if (pq.importe >= 1000 && pq.importe < 1000000)
                {
                    importeCorto = string.Format("{0:0.00}", pq.importe / 1000);
                    escala = pq.moneda.esMXN ? "MIL PESOS" : "MIL DOLARES";
                }
                movimiento1.concepto = "RENOVACION DOC PQ " + importeCorto + " " + escala;

                movimiento1.monto = pq.importe;
                movimiento1.itm = 0;

                movimientos.Add(movimiento1);

                var infoCuentaCargo = cuentaFS.GetCuenta(pq.ctaCargoBanco, pq.sctaCargoBanco, pq.ssctaCargoBanco) as catctaDTO;

                var movimiento2 = new PQPolizaDTO();
                movimiento2.cuenta = pq.ctaCargoBanco + "-" + pq.sctaCargoBanco + "-" + pq.ssctaCargoBanco + "-" + pq.digitoCargoBanco;
                movimiento2.cuentaDescripcion = "[" + movimiento2.cuenta + "] " + infoCuentaCargo.descripcion;
                movimiento2.tm = (int)TipoMovimientoEnum.Abono;
                movimiento2.referencia = movimiento1.referencia;
                movimiento2.cc = pq.cc;
                movimiento2.concepto = movimiento1.concepto;
                movimiento2.monto = pq.importe * -1;
                movimiento2.itm = 69;

                movimientos.Add(movimiento2);

                if (!pq.moneda.esMXN)
                {
                    var ctaInstitucionComplementaria = _context.tblAF_DxP_RelInstitucionCta
                    .FirstOrDefault(f =>
                        f.institucionID == pq.bancoId &&
                        f.activo &&
                        f.complementaria &&
                        f.moneda == 2
                    );

                    var infoCuentaCargoComplementaria = cuentaFS.GetCuenta(ctaInstitucionComplementaria.cta, ctaInstitucionComplementaria.scta, ctaInstitucionComplementaria.sscta) as catctaDTO;

                    var movimiento22 = new PQPolizaDTO();
                    movimiento22.cuenta = infoCuentaCargoComplementaria.cta + "-" + infoCuentaCargoComplementaria.scta + "-" + infoCuentaCargoComplementaria.sscta + "-" + infoCuentaCargoComplementaria.digito;
                    movimiento22.cuentaDescripcion = "[" + movimiento22.cuenta + "] " + infoCuentaCargoComplementaria.descripcion;
                    movimiento22.tm = (int)TipoMovimientoEnum.Abono;
                    movimiento22.referencia = movimiento2.referencia;
                    movimiento22.cc = pq.cc;
                    movimiento22.concepto = movimiento1.concepto;
                    movimiento22.monto = -0M;
                    movimiento22.itm = 69;

                    movimientos.Add(movimiento22);
                }

                var movimiento3 = new PQPolizaDTO();
                movimiento3.cuenta = movimiento1.cuenta;
                movimiento3.cuentaDescripcion = movimiento1.cuentaDescripcion;
                movimiento3.tm = (int)TipoMovimientoEnum.Abono;
                movimiento3.referencia = movimiento2.referencia + 1;
                movimiento3.cc = pq.cc;
                movimiento3.concepto = movimiento1.concepto;
                movimiento3.monto = pq.importe * -1;
                movimiento3.itm = 0;

                movimientos.Add(movimiento3);

                var movimiento4 = new PQPolizaDTO();
                movimiento4.tipoLinea = (int)PQLineaMovimientoEnum.Renovar_MontoNuevo;
                movimiento4.cuenta = movimiento2.cuenta;
                movimiento4.cuentaDescripcion = movimiento2.cuentaDescripcion;
                movimiento4.tm = (int)TipoMovimientoEnum.Cargo;
                movimiento4.referencia = movimiento2.referencia;
                movimiento4.cc = pq.cc;
                movimiento4.concepto = movimiento1.concepto;
                movimiento4.monto = pq.importe;
                movimiento4.itm = 5;

                movimientos.Add(movimiento4);

                if (!pq.moneda.esMXN)
                {
                    var ctaInstitucionComplementaria = _context.tblAF_DxP_RelInstitucionCta
                    .FirstOrDefault(f =>
                        f.institucionID == pq.bancoId &&
                        f.activo &&
                        f.complementaria &&
                        f.moneda == 2
                    );

                    var infoCuentaCargoComplementaria = cuentaFS.GetCuenta(ctaInstitucionComplementaria.cta, ctaInstitucionComplementaria.scta, ctaInstitucionComplementaria.sscta) as catctaDTO;

                    var movimiento44 = new PQPolizaDTO();
                    movimiento44.cuenta = infoCuentaCargoComplementaria.cta + "-" + infoCuentaCargoComplementaria.scta + "-" + infoCuentaCargoComplementaria.sscta + "-" + infoCuentaCargoComplementaria.digito;
                    movimiento44.cuentaDescripcion = "[" + movimiento44.cuenta + "] " + infoCuentaCargoComplementaria.descripcion;
                    movimiento44.tm = (int)TipoMovimientoEnum.Cargo;
                    movimiento44.referencia = movimiento4.referencia;
                    movimiento44.cc = pq.cc;
                    movimiento44.concepto = movimiento1.concepto;
                    movimiento44.monto = 0M;
                    movimiento44.itm = 5;

                    movimientos.Add(movimiento44);
                }

                var infoCuentaProvisionInteres = cuentaFS.GetCuenta(2110, 1, 863) as catctaDTO;
                var cuentaInteres = _context.tblAF_DxP_InstitucionesCtasInteres.FirstOrDefault(x => x.institucionId == pq.bancoId);
                if (cuentaInteres != null)
                {
                    infoCuentaProvisionInteres.cta = cuentaInteres.cta;
                    infoCuentaProvisionInteres.descripcion = cuentaInteres.descripcion;
                    infoCuentaProvisionInteres.digito = cuentaInteres.digito;
                    infoCuentaProvisionInteres.scta = cuentaInteres.scta;
                    infoCuentaProvisionInteres.sscta = cuentaInteres.sscta;
                }

                var movimientoPI = new PQPolizaDTO();
                movimientoPI.tipoLinea = (int)PQLineaMovimientoEnum.ProvisionInteres;
                movimientoPI.fechaFirma = pq.fechaFirma;
                movimientoPI.cuenta = infoCuentaProvisionInteres.cta + "-" + infoCuentaProvisionInteres.scta + "-" + infoCuentaProvisionInteres.sscta + "-" + infoCuentaProvisionInteres.digito;
                movimientoPI.cuentaDescripcion = "[" + movimientoPI.cuenta + "] " + infoCuentaProvisionInteres.descripcion;
                movimientoPI.tm = (int)TipoMovimientoEnum.Cargo;
                movimientoPI.referencia = movimiento4.referencia + 1;
                movimientoPI.cc = pq.cc;
                movimientoPI.concepto = movimiento1.concepto;
                movimientoPI.interesDiario = ((pq.importe * (pq.moneda.esMXN ? 1 : pq.tipoCambio.Value)) * (pq.interes / 100)) / 360;
                movimientoPI.monto = movimientoPI.interesDiario * ((DateTime.Now - pq.fechaFirma).Days + 1);
                movimientoPI.itm = 0;

                movimientos.Add(movimientoPI);

                var movimiento6 = new PQPolizaDTO();
                movimiento6.cuenta = movimiento2.cuenta;
                movimiento6.cuentaDescripcion = movimiento2.cuentaDescripcion;
                movimiento6.tm = (int)TipoMovimientoEnum.Abono;
                movimiento6.referencia = movimientoPI.referencia;
                movimiento6.cc = pq.cc;
                movimiento6.concepto = movimiento1.concepto;
                movimiento6.monto = -0;
                movimiento6.itm = 68;

                movimientos.Add(movimiento6);

                if (!pq.moneda.esMXN)
                {
                    var ctaInstitucionComplementaria = _context.tblAF_DxP_RelInstitucionCta
                    .FirstOrDefault(f =>
                        f.institucionID == pq.bancoId &&
                        f.activo &&
                        f.complementaria &&
                        f.moneda == 2
                    );

                    var infoCuentaCargoComplementaria = cuentaFS.GetCuenta(ctaInstitucionComplementaria.cta, ctaInstitucionComplementaria.scta, ctaInstitucionComplementaria.sscta) as catctaDTO;

                    var movimiento66 = new PQPolizaDTO();
                    movimiento66.cuenta = infoCuentaCargoComplementaria.cta + "-" + infoCuentaCargoComplementaria.scta + "-" + infoCuentaCargoComplementaria.sscta + "-" + infoCuentaCargoComplementaria.digito;
                    movimiento66.cuentaDescripcion = "[" + movimiento66.cuenta + "] " + infoCuentaCargoComplementaria.descripcion;
                    movimiento66.tm = (int)TipoMovimientoEnum.Abono;
                    movimiento66.referencia = movimiento6.referencia;
                    movimiento66.cc = pq.cc;
                    movimiento66.concepto = movimiento1.concepto;
                    movimiento66.monto = -0M;
                    movimiento66.itm = 68;

                    movimientos.Add(movimiento66);
                }

                var infoCuenta5900 = cuentaFS.GetCuenta(5900, 5, 0) as catctaDTO;

                var movimiento7 = new PQPolizaDTO();
                movimiento7.tipoLinea = (int)PQLineaMovimientoEnum.PoderQuitarLinea;
                movimiento7.cuenta = 5900 + "-" + 5 + "-" + 0 + "-" + 9;
                movimiento7.cuentaDescripcion = "[" + movimiento7.cuenta + "] " + infoCuenta5900.descripcion;
                movimiento7.tm = (int)TipoMovimientoEnum.Cargo;
                movimiento7.referencia = movimiento6.referencia + 1;
                movimiento7.cc = pq.cc;
                movimiento7.concepto = movimiento1.concepto;
                movimiento7.monto = 0M;
                movimiento7.itm = 0;

                movimientos.Add(movimiento7);

                r.Add(SUCCESS, true);
                r.Add(ITEMS, movimientos);
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, ex.Message);

                LogError(2, 0, "ContratosController", "GetPQLiquidar", ex, AccionEnum.CONSULTA, 0, id);
            }

            return r;
        }

        public Dictionary<string, object> GetPQAbono(int id)
        {
            var r = new Dictionary<string, object>();

            try
            {
                var movimientos = new List<PQPolizaDTO>();

                var pq = _context.tblAF_DxP_PQ.First(x => x.id == id);

                string importeCorto = "";
                string escala = "";
                if (pq.importe >= 1000000)
                {
                    importeCorto = string.Format("{0:0.00}", pq.importe / 1000000);
                    escala = pq.moneda.esMXN ? "MDP" : "DLLS";
                }
                if (pq.importe >= 1000 && pq.importe < 1000000)
                {
                    importeCorto = string.Format("{0:0.00}", pq.importe / 1000);
                    escala = pq.moneda.esMXN ? "MIL PESOS" : "MIL DOLARES";
                }

                var infoCuentaAbono = cuentaFS.GetCuenta(pq.ctaAbonoBanco, pq.sctaAbonoBanco, pq.ssctaAbonoBanco) as catctaDTO;

                var movimiento1 = new PQPolizaDTO();
                movimiento1.cuenta = pq.ctaAbonoBanco + "-" + pq.sctaAbonoBanco + "-" + pq.ssctaAbonoBanco + "-" + pq.digitoAbonoBanco;
                movimiento1.cuentaDescripcion = "[" + movimiento1.cuenta + "] " + infoCuentaAbono.descripcion;
                movimiento1.tm = (int)TipoMovimientoEnum.Cargo;
                movimiento1.referencia = Convert.ToInt32(ObtenerReferencia(DateTime.Now, pq.ctaAbonoBanco));
                movimiento1.cc = pq.cc;
                movimiento1.concepto = "ABONO DOC PQ " + importeCorto + " " + escala;
                movimiento1.monto = pq.importe;
                movimiento1.itm = 0;
                movimientos.Add(movimiento1);

                var infoCuentaCargo = cuentaFS.GetCuenta(pq.ctaCargoBanco, pq.sctaCargoBanco, pq.ssctaCargoBanco) as catctaDTO;

                var movimiento2 = new PQPolizaDTO();
                movimiento2.cuenta = pq.ctaCargoBanco + "-" + pq.sctaCargoBanco + "-" + pq.ssctaCargoBanco + "-" + pq.digitoCargoBanco;
                movimiento2.cuentaDescripcion = "[" + movimiento2.cuenta + "] " + infoCuentaCargo.descripcion;
                movimiento2.tm = (int)TipoMovimientoEnum.Abono;
                movimiento2.referencia = movimiento1.referencia;
                movimiento2.cc = pq.cc;
                movimiento2.concepto = movimiento1.concepto;
                movimiento2.monto = pq.importe * -1;
                movimiento2.itm = 69;
                movimiento2.tipoLinea = (int)PQLineaMovimientoEnum.Abono;
                movimientos.Add(movimiento2);

                if (!pq.moneda.esMXN)
                {
                    var ctaInstitucionComplementaria = _context.tblAF_DxP_RelInstitucionCta
                        .FirstOrDefault(x =>
                            x.institucionID == pq.bancoId &&
                            x.activo &&
                            x.complementaria &&
                            x.moneda == 2);

                    var infoCuentaCargoComplementaria = cuentaFS.GetCuenta(ctaInstitucionComplementaria.cta, ctaInstitucionComplementaria.scta, ctaInstitucionComplementaria.sscta) as catctaDTO;

                    var movimiento22 = new PQPolizaDTO();
                    movimiento22.cuenta = infoCuentaCargoComplementaria.cta + "-" + infoCuentaCargoComplementaria.scta + "-" + infoCuentaCargoComplementaria.sscta + "-" + infoCuentaCargoComplementaria.digito;
                    movimiento22.cuentaDescripcion = "[" + movimiento22.cuenta + "] " + infoCuentaCargoComplementaria.descripcion;
                    movimiento22.tm = (int)TipoMovimientoEnum.Abono;
                    movimiento22.referencia = movimiento2.referencia;
                    movimiento22.cc = pq.cc;
                    movimiento22.concepto = movimiento2.concepto;
                    movimiento22.monto = pq.importe * -1;
                    movimiento22.itm = 69;
                    movimientos.Add(movimiento22);
                }

                var infoCuentaProvisionInteres = cuentaFS.GetCuenta(2110, 1, 863) as catctaDTO;
                var cuentaInteres = _context.tblAF_DxP_InstitucionesCtasInteres.FirstOrDefault(x => x.institucionId == pq.bancoId);
                if (cuentaInteres != null)
                {
                    infoCuentaProvisionInteres.cta = cuentaInteres.cta;
                    infoCuentaProvisionInteres.descripcion = cuentaInteres.descripcion;
                    infoCuentaProvisionInteres.digito = cuentaInteres.digito;
                    infoCuentaProvisionInteres.scta = cuentaInteres.scta;
                    infoCuentaProvisionInteres.sscta = cuentaInteres.sscta;
                }

                var movimientoPI = new PQPolizaDTO();
                movimientoPI.tipoLinea = (int)PQLineaMovimientoEnum.ProvisionInteres;
                movimientoPI.fechaFirma = pq.fechaFirma;
                movimientoPI.cuenta = infoCuentaProvisionInteres.cta + "-" + infoCuentaProvisionInteres.scta + "-" + infoCuentaProvisionInteres.sscta + "-" + infoCuentaProvisionInteres.digito;
                movimientoPI.cuentaDescripcion = "[" + movimientoPI.cuenta + "] " + infoCuentaProvisionInteres.descripcion;
                movimientoPI.tm = (int)TipoMovimientoEnum.Cargo;
                movimientoPI.referencia = movimiento2.referencia + 1;
                movimientoPI.cc = pq.cc;
                movimientoPI.concepto = movimiento1.concepto;
                movimientoPI.interesDiario = ((pq.importe * (pq.moneda.esMXN ? 1 : pq.tipoCambio.Value)) * (pq.interes / 100)) / 360;
                movimientoPI.monto = movimientoPI.interesDiario * ((DateTime.Now - pq.fechaFirma).Days + 1);
                movimientoPI.itm = 0;

                movimientos.Add(movimientoPI);

                var movimiento6 = new PQPolizaDTO();
                movimiento6.cuenta = movimiento2.cuenta;
                movimiento6.cuentaDescripcion = movimiento2.cuentaDescripcion;
                movimiento6.tm = (int)TipoMovimientoEnum.Abono;
                movimiento6.referencia = movimientoPI.referencia;
                movimiento6.cc = pq.cc;
                movimiento6.concepto = movimiento1.concepto;
                movimiento6.monto = -0;
                movimiento6.itm = 68;

                movimientos.Add(movimiento6);

                if (!pq.moneda.esMXN)
                {
                    var ctaInstitucionComplementaria = _context.tblAF_DxP_RelInstitucionCta
                    .FirstOrDefault(f =>
                        f.institucionID == pq.bancoId &&
                        f.activo &&
                        f.complementaria &&
                        f.moneda == 2
                    );

                    var infoCuentaCargoComplementaria = cuentaFS.GetCuenta(ctaInstitucionComplementaria.cta, ctaInstitucionComplementaria.scta, ctaInstitucionComplementaria.sscta) as catctaDTO;

                    var movimiento66 = new PQPolizaDTO();
                    movimiento66.cuenta = infoCuentaCargoComplementaria.cta + "-" + infoCuentaCargoComplementaria.scta + "-" + infoCuentaCargoComplementaria.sscta + "-" + infoCuentaCargoComplementaria.digito;
                    movimiento66.cuentaDescripcion = "[" + movimiento66.cuenta + "] " + infoCuentaCargoComplementaria.descripcion;
                    movimiento66.tm = (int)TipoMovimientoEnum.Abono;
                    movimiento66.referencia = movimiento6.referencia;
                    movimiento66.cc = pq.cc;
                    movimiento66.concepto = movimiento1.concepto;
                    movimiento66.monto = -0M;
                    movimiento66.itm = 68;

                    movimientos.Add(movimiento66);
                }

                r.Add(SUCCESS, true);
                r.Add(ITEMS, movimientos);
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, ex.Message);

                LogError(2, 0, "ContratosController", "GetPQAbono", ex, AccionEnum.CONSULTA, 0, id);
            }

            return r;
        }

        public Dictionary<string, object> GetPQCambiarCC(int id)
        {
            var r = new Dictionary<string, object>();

            try
            {
                var movimientos = new List<PQPolizaDTO>();

                var pq = _context.tblAF_DxP_PQ.First(f => f.id == id);
                var infoCuentaAbono = cuentaFS.GetCuenta(pq.ctaAbonoBanco, pq.sctaAbonoBanco, pq.ssctaAbonoBanco) as catctaDTO;

                var movimiento1 = new PQPolizaDTO();
                movimiento1.tipoLinea = (int)PQLineaMovimientoEnum.CambioCC_MontoCCAnterior;
                movimiento1.cuenta = pq.ctaAbonoBanco + "-" + pq.sctaAbonoBanco + "-" + pq.ssctaAbonoBanco + "-" + pq.digitoAbonoBanco;
                movimiento1.cuentaDescripcion = "[" + movimiento1.cuenta + "] " + infoCuentaAbono.descripcion;
                movimiento1.tm = (int)TipoMovimientoEnum.Cargo;
                movimiento1.referencia = Convert.ToInt32(ObtenerReferencia(DateTime.Now, pq.ctaAbonoBanco));
                movimiento1.cc = pq.cc;

                string importeCorto = "";
                string escala = "";
                if (pq.importe >= 1000000)
                {
                    importeCorto = string.Format("{0:0.00}", pq.importe / 1000000);
                    escala = pq.moneda.esMXN ? "MDP" : "DLLS";
                }
                if (pq.importe >= 1000 && pq.importe < 1000000)
                {
                    importeCorto = string.Format("{0:0.00}", pq.importe / 1000);
                    escala = pq.moneda.esMXN ? "MIL PESOS" : "MIL DOLARES";
                }
                movimiento1.concepto = "CAMBIO CC DOC PQ " + importeCorto + " " + escala;

                movimiento1.monto = pq.importe;
                movimiento1.itm = 0;

                movimientos.Add(movimiento1);

                var infoCuentaCargo = cuentaFS.GetCuenta(pq.ctaCargoBanco, pq.sctaCargoBanco, pq.ssctaCargoBanco) as catctaDTO;

                var movimiento2 = new PQPolizaDTO();
                movimiento2.tipoLinea = movimiento1.tipoLinea;
                movimiento2.cuenta = pq.ctaCargoBanco + "-" + pq.sctaCargoBanco + "-" + pq.ssctaCargoBanco + "-" + pq.digitoCargoBanco;
                movimiento2.cuentaDescripcion = "[" + movimiento2.cuenta + "] " + infoCuentaCargo.descripcion;
                movimiento2.tm = (int)TipoMovimientoEnum.Abono;
                movimiento2.referencia = movimiento1.referencia;
                movimiento2.cc = pq.cc;
                movimiento2.concepto = movimiento1.concepto;
                movimiento2.monto = pq.importe * -1;
                movimiento2.itm = 69;

                movimientos.Add(movimiento2);

                if (!pq.moneda.esMXN)
                {
                    var ctaInstitucionComplementaria = _context.tblAF_DxP_RelInstitucionCta
                    .FirstOrDefault(f =>
                        f.institucionID == pq.bancoId &&
                        f.activo &&
                        f.complementaria &&
                        f.moneda == 2
                    );

                    var infoCuentaCargoComplementaria = cuentaFS.GetCuenta(ctaInstitucionComplementaria.cta, ctaInstitucionComplementaria.scta, ctaInstitucionComplementaria.sscta) as catctaDTO;

                    var movimiento22 = new PQPolizaDTO();
                    movimiento22.cuenta = infoCuentaCargoComplementaria.cta + "-" + infoCuentaCargoComplementaria.scta + "-" + infoCuentaCargoComplementaria.sscta + "-" + infoCuentaCargoComplementaria.digito;
                    movimiento22.cuentaDescripcion = "[" + movimiento22.cuenta + "] " + infoCuentaCargoComplementaria.descripcion;
                    movimiento22.tm = (int)TipoMovimientoEnum.Abono;
                    movimiento22.referencia = movimiento2.referencia;
                    movimiento22.cc = pq.cc;
                    movimiento22.concepto = movimiento1.concepto;
                    movimiento22.monto = -0M;
                    movimiento22.itm = 69;

                    movimientos.Add(movimiento22);
                }

                var movimiento3 = new PQPolizaDTO();
                movimiento3.tipoLinea = (int)PQLineaMovimientoEnum.CambioCC_MontoCCNuevo;
                movimiento3.cuenta = movimiento1.cuenta;
                movimiento3.cuentaDescripcion = movimiento1.cuentaDescripcion;
                movimiento3.tm = (int)TipoMovimientoEnum.Abono;
                movimiento3.referencia = movimiento1.referencia + 1;
                movimiento3.cc = pq.cc;
                movimiento3.concepto = movimiento1.concepto;
                movimiento3.monto = pq.importe * -1;
                movimiento3.itm = 0;

                movimientos.Add(movimiento3);

                var movimiento4 = new PQPolizaDTO();
                movimiento4.tipoLinea = movimiento3.tipoLinea;
                movimiento4.cuenta = movimiento2.cuenta;
                movimiento4.cuentaDescripcion = movimiento2.cuentaDescripcion;
                movimiento4.tm = (int)TipoMovimientoEnum.Cargo;
                movimiento4.referencia = movimiento1.referencia + 1;
                movimiento4.cc = pq.cc;
                movimiento4.concepto = movimiento1.concepto;
                movimiento4.monto = pq.importe;
                movimiento4.itm = 5;

                movimientos.Add(movimiento4);

                if (!pq.moneda.esMXN)
                {
                    var ctaInstitucionComplementaria = _context.tblAF_DxP_RelInstitucionCta
                    .FirstOrDefault(f =>
                        f.institucionID == pq.bancoId &&
                        f.activo &&
                        f.complementaria &&
                        f.moneda == 2
                    );

                    var infoCuentaCargoComplementaria = cuentaFS.GetCuenta(ctaInstitucionComplementaria.cta, ctaInstitucionComplementaria.scta, ctaInstitucionComplementaria.sscta) as catctaDTO;

                    var movimiento44 = new PQPolizaDTO();
                    movimiento44.cuenta = infoCuentaCargoComplementaria.cta + "-" + infoCuentaCargoComplementaria.scta + "-" + infoCuentaCargoComplementaria.sscta + "-" + infoCuentaCargoComplementaria.digito;
                    movimiento44.cuentaDescripcion = "[" + movimiento44.cuenta + "] " + infoCuentaCargoComplementaria.descripcion;
                    movimiento44.tm = (int)TipoMovimientoEnum.Cargo;
                    movimiento44.referencia = movimiento4.referencia;
                    movimiento44.cc = pq.cc;
                    movimiento44.concepto = movimiento1.concepto;
                    movimiento44.monto = 0M;
                    movimiento44.itm = 5;

                    movimientos.Add(movimiento44);
                }

                r.Add(SUCCESS, true);
                r.Add(ITEMS, movimientos);
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, ex.Message);

                LogError(2, 0, "ContratosController", "GetPQLiquidar", ex, AccionEnum.CONSULTA, 0, id);
            }

            return r;
        }

        public Dictionary<string, object> Liquidar(int idPq, DateTime fechaMovimiento, List<PQPolizaDTO> infoPol)
        {
            var r = new Dictionary<string, object>();

            using (var transaccionSP = _context.Database.BeginTransaction())
            {
                using (var conexionEK = new Conexion().ConexionEKAdm())
                {
                    using (var transaccionEK = conexionEK.BeginTransaction())
                    {
                        try
                        {
                            var pq = _context.tblAF_DxP_PQ.First(f => f.id == idPq);

                            pq.tipoMovimientoId = (int)PQTipoMovimientoEnum.BajaPorLiquidacion;
                            pq.fechaLiquidacion = fechaMovimiento;
                            pq.fechaModificacion = DateTime.Now;
                            pq.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;

                            _context.SaveChanges();

                            var poliza_movpol = generarPolizaAccion(fechaMovimiento, infoPol);

                            _polizaEkFS.SetContext(conexionEK);
                            _polizaEkFS.SetTransaccion(transaccionEK);
                            string numeroPoliza = _polizaEkFS.GuardarPoliza(poliza_movpol.poliza, poliza_movpol.movimientos);

                            pq.poliza = numeroPoliza;
                            _context.SaveChanges();

                            _polizaSPFS.SetContext(_context);
                            _polizaSPFS.SetTransaccion(transaccionSP);
                            var polizaPoliza = numeroPoliza.Split(new string[] { "-" }, StringSplitOptions.None)[2];
                            poliza_movpol.poliza.poliza = Convert.ToInt32(polizaPoliza);
                            var resultadoSP = _polizaSPFS.GuardarPoliza(poliza_movpol.poliza, poliza_movpol.movimientos);

                            var infoAConciliar = GenerarInfoConcilia(poliza_movpol.movimientos, pq, poliza_movpol.poliza);

                            foreach (var concilia in infoAConciliar)
                            {
                                _polizaEkFS.GuardarParaConciliar(concilia);
                            }

                            transaccionEK.Commit();
                            transaccionSP.Commit();

                            r.Add(SUCCESS, true);
                            r.Add(MESSAGE, "Registró correcto, póliza: " + numeroPoliza);
                        }
                        catch (Exception ex)
                        {
                            r.Add(SUCCESS, false);
                            r.Add(MESSAGE, ex.Message);

                            LogError(2, 0, "ContratosController", "Liquidar", ex, AccionEnum.ACTUALIZAR, 0, infoPol);
                        }
                    }
                }
            }

            return r;
        }

        public Dictionary<string, object> CambiarCC(int idPq, DateTime fechaMovimiento, List<PQPolizaDTO> infoPol)
        {
            var r = new Dictionary<string, object>();

            using (var transaccionSP = _context.Database.BeginTransaction())
            {
                using (var conexionEK = new Conexion().ConexionEKAdm())
                {
                    using (var transaccionEK = conexionEK.BeginTransaction())
                    {
                        try
                        {
                            var poliza_movpol = generarPolizaAccion(fechaMovimiento, infoPol);

                            _polizaEkFS.SetContext(conexionEK);
                            _polizaEkFS.SetTransaccion(transaccionEK);
                            string numeroPoliza = _polizaEkFS.GuardarPoliza(poliza_movpol.poliza, poliza_movpol.movimientos);

                            _polizaSPFS.SetContext(_context);
                            _polizaSPFS.SetTransaccion(transaccionSP);
                            var polizaPoliza = numeroPoliza.Split(new string[] { "-" }, StringSplitOptions.None)[2];
                            poliza_movpol.poliza.poliza = Convert.ToInt32(polizaPoliza);
                            var resultadoSP = _polizaSPFS.GuardarPoliza(poliza_movpol.poliza, poliza_movpol.movimientos);

                            var pq = _context.tblAF_DxP_PQ.First(f => f.id == idPq);

                            pq.tipoMovimientoId = (int)PQTipoMovimientoEnum.BajaPorCambioCC;
                            pq.fechaModificacion = DateTime.Now;
                            pq.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;

                            _context.SaveChanges();

                            if (infoPol.Any(a => a.tipoLinea.HasValue && a.tipoLinea.Value == (int)PQLineaMovimientoEnum.CambioCC_MontoCCAnterior && a.monto > 0))
                            {
                                if (infoPol.First(f => f.tipoLinea.HasValue && f.tipoLinea.Value == (int)PQLineaMovimientoEnum.CambioCC_MontoCCAnterior).monto !=
                                    infoPol.First(f => f.tipoLinea.HasValue && f.tipoLinea.Value == (int)PQLineaMovimientoEnum.CambioCC_MontoCCNuevo).monto)
                                {
                                    var pqNuevoAnterior = new tblAF_DxP_PQ();
                                    pqNuevoAnterior.archivoId = pq.archivoId;
                                    pqNuevoAnterior.bancoId = pq.bancoId;
                                    pqNuevoAnterior.cc = pq.cc;
                                    pqNuevoAnterior.ctaAbonoBanco = pq.ctaAbonoBanco;
                                    pqNuevoAnterior.ctaCargoBanco = pq.ctaCargoBanco;
                                    pqNuevoAnterior.digitoAbonoBanco = pq.digitoAbonoBanco;
                                    pqNuevoAnterior.digitoCargoBanco = pq.digitoCargoBanco;
                                    pqNuevoAnterior.estatus = pq.estatus;
                                    pqNuevoAnterior.fechaCreacion = DateTime.Now;
                                    pqNuevoAnterior.fechaFirma = pq.fechaFirma;
                                    pqNuevoAnterior.fechaModificacion = pqNuevoAnterior.fechaCreacion;
                                    pqNuevoAnterior.fechaVencimiento = pq.fechaVencimiento;
                                    pqNuevoAnterior.importe = pq.importe - infoPol.First(f => f.tipoLinea.HasValue && f.tipoLinea.Value == (int)PQLineaMovimientoEnum.CambioCC_MontoCCNuevo && f.tm == (int)TipoMovimientoEnum.Cargo).monto;
                                    pqNuevoAnterior.interes = pq.interes;
                                    pqNuevoAnterior.monedaId = pq.monedaId;
                                    pqNuevoAnterior.sctaAbonoBanco = pq.sctaAbonoBanco;
                                    pqNuevoAnterior.sctaCargoBanco = pq.sctaCargoBanco;
                                    pqNuevoAnterior.ssctaAbonoBanco = pq.ssctaAbonoBanco;
                                    pqNuevoAnterior.ssctaCargoBanco = pq.ssctaCargoBanco;
                                    pqNuevoAnterior.tipoCambio = pq.tipoCambio;
                                    pqNuevoAnterior.tipoMovimientoId = (int)PQTipoMovimientoEnum.AltaPorCambioCC;
                                    pqNuevoAnterior.poliza = pq.poliza + " / " + numeroPoliza;
                                    pqNuevoAnterior.usuarioCreacionId = vSesiones.sesionUsuarioDTO.id;
                                    pqNuevoAnterior.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;

                                    _context.tblAF_DxP_PQ.Add(pqNuevoAnterior);
                                    _context.SaveChanges();

                                    pqNuevoAnterior.folio = pq.folio;
                                    _context.SaveChanges();
                                }
                            }

                            var pqNuevo = new tblAF_DxP_PQ();
                            pqNuevo.archivoId = pq.archivoId;
                            pqNuevo.bancoId = pq.bancoId;
                            pqNuevo.cc = infoPol.First(f => f.tipoLinea.HasValue && f.tipoLinea.Value == (int)PQLineaMovimientoEnum.CambioCC_MontoCCNuevo).cc;
                            pqNuevo.ctaAbonoBanco = pq.ctaAbonoBanco;
                            pqNuevo.ctaCargoBanco = pq.ctaCargoBanco;
                            pqNuevo.digitoAbonoBanco = pq.digitoAbonoBanco;
                            pqNuevo.digitoCargoBanco = pq.digitoCargoBanco;
                            pqNuevo.estatus = pq.estatus;
                            pqNuevo.fechaCreacion = DateTime.Now;
                            pqNuevo.fechaFirma = pq.fechaFirma;
                            pqNuevo.fechaModificacion = pqNuevo.fechaCreacion;
                            pqNuevo.fechaVencimiento = pq.fechaVencimiento;
                            pqNuevo.importe = infoPol.First(f => f.tipoLinea.HasValue && f.tipoLinea.Value == (int)PQLineaMovimientoEnum.CambioCC_MontoCCNuevo && f.tm == (int)TipoMovimientoEnum.Cargo).monto;
                            pqNuevo.interes = pq.interes;
                            pqNuevo.monedaId = pq.monedaId;
                            pqNuevo.sctaAbonoBanco = pq.sctaAbonoBanco;
                            pqNuevo.sctaCargoBanco = pq.sctaCargoBanco;
                            pqNuevo.ssctaAbonoBanco = pq.ssctaAbonoBanco;
                            pqNuevo.ssctaCargoBanco = pq.ssctaCargoBanco;
                            pqNuevo.tipoCambio = pq.tipoCambio;
                            pqNuevo.tipoMovimientoId = (int)PQTipoMovimientoEnum.AltaPorCambioCC;
                            pqNuevo.poliza = numeroPoliza;
                            pqNuevo.usuarioCreacionId = vSesiones.sesionUsuarioDTO.id;
                            pqNuevo.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;

                            _context.tblAF_DxP_PQ.Add(pqNuevo);
                            _context.SaveChanges();

                            pqNuevo.folio = pq.folio;
                            _context.SaveChanges();

                            var infoAConciliar = GenerarInfoConcilia(poliza_movpol.movimientos, pq, poliza_movpol.poliza);

                            foreach (var concilia in infoAConciliar)
                            {
                                _polizaEkFS.GuardarParaConciliar(concilia);
                            }

                            transaccionEK.Commit();
                            transaccionSP.Commit();

                            r.Add(SUCCESS, true);
                            r.Add(MESSAGE, "Registró correcto, póliza: " + numeroPoliza);
                        }
                        catch (Exception ex)
                        {
                            transaccionEK.Rollback();
                            transaccionSP.Rollback();

                            r.Add(SUCCESS, false);
                            r.Add(MESSAGE, ex.Message);

                            LogError(2, 0, "ContratosController", "CambiarCC", ex, AccionEnum.ACTUALIZAR, 0, infoPol);
                        }
                    }
                }
            }

            return r;
        }

        public Dictionary<string, object> RenovarPQ(int idPq, DateTime fechaMovimiento, List<PQPolizaDTO> infoPol, DateTime fechaFirma, DateTime fechaVencimiento, decimal interes, HttpPostedFileBase archivo)
        {
            var r = new Dictionary<string, object>();

            using (var transaccionSP = _context.Database.BeginTransaction())
            {
                using (var conexionEK = new Conexion().ConexionEKAdm())
                {
                    using (var transaccionEK = conexionEK.BeginTransaction())
                    {
                        try
                        {
                            #region archivo
                            var folder = "DOCUMENTOS_PQ";

                            var fechaArchivo = DateTime.Now.ToString("yyyy-MM-ddTHHmmssfff");
                            var ruta = archivoFS.getUrlDelServidor(1019) + folder + @"\";

                            var nombreArchivo = archivo.FileName;
                            var nombreArchivoSinExtension = System.IO.Path.GetFileNameWithoutExtension(nombreArchivo);
                            var extension = System.IO.Path.GetExtension(nombreArchivo);

#if DEBUG
                            var directorio = new DirectoryInfo(@"c:\DOCUMENTOS_POR_PAGAR\" + folder + @"\");
#else
                                var directorio = new DirectoryInfo(ruta);
#endif
                            if (!directorio.Exists)
                            {
                                directorio.Create();
                            }

                            var pathCompleto = System.IO.Path.Combine(directorio.ToString(), nombreArchivoSinExtension + "_" + fechaArchivo + extension);

                            archivo.SaveAs(pathCompleto);

                            var datoArchivo = new tblAF_DxP_PQ_Archivo();

                            datoArchivo.estatus = true;
                            datoArchivo.fechaCarga = DateTime.Now;
                            datoArchivo.fechaCreacion = datoArchivo.fechaCarga;
                            datoArchivo.fechaModificacion = datoArchivo.fechaCreacion;
                            datoArchivo.nombreArchivo = nombreArchivo;
                            datoArchivo.ubicacionArchivo = pathCompleto;
                            datoArchivo.usuarioCreacionId = vSesiones.sesionUsuarioDTO.id;
                            datoArchivo.usuarioModificacionId = datoArchivo.usuarioCreacionId;

                            _context.tblAF_DxP_PQ_Archivo.Add(datoArchivo);
                            _context.SaveChanges();
                            #endregion


                            var tipoCambio = getTipoCambioDLLs(new DateTime(fechaMovimiento.Year, fechaMovimiento.Month, fechaMovimiento.Day));

                            var pq = _context.tblAF_DxP_PQ.First(f => f.id == idPq);

                            pq.tipoMovimientoId = (int)PQTipoMovimientoEnum.BajaPorRenovacion;
                            pq.fechaModificacion = DateTime.Now;
                            pq.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;

                            _context.SaveChanges();

                            var pqNuevo = new tblAF_DxP_PQ();
                            pqNuevo.archivoId = datoArchivo.id;
                            pqNuevo.bancoId = pq.bancoId;
                            pqNuevo.cc = infoPol.First(f => f.tipoLinea.HasValue && f.tipoLinea.Value == (int)PQLineaMovimientoEnum.Renovar_MontoNuevo).cc;
                            pqNuevo.ctaAbonoBanco = pq.ctaAbonoBanco;
                            pqNuevo.ctaCargoBanco = pq.ctaCargoBanco;
                            pqNuevo.digitoAbonoBanco = pq.digitoAbonoBanco;
                            pqNuevo.digitoCargoBanco = pq.digitoCargoBanco;
                            pqNuevo.estatus = pq.estatus;
                            pqNuevo.fechaCreacion = DateTime.Now;
                            pqNuevo.fechaFirma = fechaFirma;
                            pqNuevo.fechaModificacion = pqNuevo.fechaCreacion;
                            pqNuevo.fechaVencimiento = fechaVencimiento;
                            pqNuevo.importe = infoPol.First(f => f.tipoLinea.HasValue && f.tipoLinea.Value == (int)PQLineaMovimientoEnum.Renovar_MontoNuevo).monto;
                            pqNuevo.interes = interes;
                            pqNuevo.monedaId = pq.monedaId;
                            pqNuevo.sctaAbonoBanco = pq.sctaAbonoBanco;
                            pqNuevo.sctaCargoBanco = pq.sctaCargoBanco;
                            pqNuevo.ssctaAbonoBanco = pq.ssctaAbonoBanco;
                            pqNuevo.ssctaCargoBanco = pq.ssctaCargoBanco;
                            pqNuevo.tipoCambio = pq.moneda.esMXN ? 1 : tipoCambio.tipo_cambio;
                            pqNuevo.tipoMovimientoId = (int)PQTipoMovimientoEnum.AltaPorRenovacion;
                            pqNuevo.usuarioCreacionId = vSesiones.sesionUsuarioDTO.id;
                            pqNuevo.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;

                            _context.tblAF_DxP_PQ.Add(pqNuevo);
                            _context.SaveChanges();

                            pqNuevo.folio = pq.folio;
                            _context.SaveChanges();

                            var poliza_movpol = generarPolizaAccion(fechaMovimiento, infoPol);

                            _polizaEkFS.SetContext(conexionEK);
                            _polizaEkFS.SetTransaccion(transaccionEK);
                            string numeroPoliza = _polizaEkFS.GuardarPoliza(poliza_movpol.poliza, poliza_movpol.movimientos);

                            pqNuevo.poliza = numeroPoliza;
                            _context.SaveChanges();

                            _polizaSPFS.SetContext(_context);
                            _polizaSPFS.SetTransaccion(transaccionSP);
                            var polizaPoliza = numeroPoliza.Split(new string[] { "-" }, StringSplitOptions.None)[2];
                            poliza_movpol.poliza.poliza = Convert.ToInt32(polizaPoliza);
                            var resultadoSP = _polizaSPFS.GuardarPoliza(poliza_movpol.poliza, poliza_movpol.movimientos);

                            var infoAConciliar = GenerarInfoConcilia(poliza_movpol.movimientos, pq, poliza_movpol.poliza);

                            foreach (var concilia in infoAConciliar)
                            {
                                _polizaEkFS.GuardarParaConciliar(concilia);
                            }

                            transaccionEK.Commit();
                            transaccionSP.Commit();

                            r.Add(SUCCESS, true);
                            r.Add(MESSAGE, "Registró correcto, póliza: " + numeroPoliza);
                        }
                        catch (Exception ex)
                        {
                            r.Add(SUCCESS, false);
                            r.Add(MESSAGE, ex.Message);

                            LogError(2, 0, "ContratosController", "CambiarCC", ex, AccionEnum.ACTUALIZAR, 0, infoPol);
                        }
                    }
                }
            }

            return r;
        }

        public Dictionary<string, object> AbonarPQ(int idPq, DateTime fechaMovimiento, List<PQPolizaDTO> infoPol)
        {
            var r = new Dictionary<string, object>();

            using (var transaccionSP = _context.Database.BeginTransaction())
            {
                using (var conexionEK = new Conexion().ConexionEKAdm())
                {
                    using (var transaccionEK = conexionEK.BeginTransaction())
                    {
                        try
                        {
                            var poliza_movpol = generarPolizaAccion(fechaMovimiento, infoPol);

                            _polizaEkFS.SetContext(conexionEK);
                            _polizaEkFS.SetTransaccion(transaccionEK);
                            string numeroPoliza = _polizaEkFS.GuardarPoliza(poliza_movpol.poliza, poliza_movpol.movimientos);

                            _polizaSPFS.SetContext(_context);
                            _polizaSPFS.SetTransaccion(transaccionSP);
                            var polizaPoliza = numeroPoliza.Split(new string[] { "-" }, StringSplitOptions.None)[2];
                            poliza_movpol.poliza.poliza = Convert.ToInt32(polizaPoliza);
                            var resultadoSP = _polizaSPFS.GuardarPoliza(poliza_movpol.poliza, poliza_movpol.movimientos);

                            var pq = _context.tblAF_DxP_PQ.First(x => x.id == idPq);

                            var infoAConciliar = GenerarInfoConcilia(poliza_movpol.movimientos, pq, poliza_movpol.poliza);

                            foreach (var concilia in infoAConciliar)
                            {
                                _polizaEkFS.GuardarParaConciliar(concilia);
                            }

                            var montoAbono = infoPol.First(x => x.tm == (int)TipoMovimientoEnum.Abono && x.tipoLinea == (int)PQLineaMovimientoEnum.Abono).monto;

                            var abono = new tblAF_DxP_PQ_Abono();
                            abono.pqID = pq.id;
                            abono.importe = montoAbono;
                            abono.fecha = fechaMovimiento.Date;
                            abono.poliza = numeroPoliza;
                            abono.estatus = true;
                            abono.fechaCreacion = DateTime.Now;
                            abono.usuarioCreacionID = vSesiones.sesionUsuarioDTO.id;

                            _context.tblAF_DxP_PQ_Abono.Add(abono);
                            _context.SaveChanges();

                            transaccionEK.Commit();
                            transaccionSP.Commit();

                            r.Add(SUCCESS, true);
                            r.Add(MESSAGE, "Registró correcto, póliza: " + numeroPoliza);
                        }
                        catch (Exception ex)
                        {
                            transaccionEK.Rollback();
                            transaccionSP.Rollback();

                            r.Add(SUCCESS, false);
                            r.Add(MESSAGE, ex.Message);

                            LogError(2, 0, "ContratosContoller", "AbonarPQ", ex, AccionEnum.AGREGAR, 0, infoPol);
                        }
                    }
                }
            }

            return r;
        }

        public Dictionary<string, object> UrlArchivoPQ(int idPq)
        {
            var r = new Dictionary<string, object>();

            try
            {
                var pq = _context.tblAF_DxP_PQ.First(f => f.id == idPq);

                r.Add(SUCCESS, true);
                r.Add(ITEMS, new tblAF_DxP_PQ_Archivo { ubicacionArchivo = pq.archivoPQ.ubicacionArchivo, nombreArchivo = pq.archivoPQ.nombreArchivo });
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, ex);

                LogError(2, 0, "ContratosController", "DescargarArchivo", ex, AccionEnum.DESCARGAR, 0, idPq);
            }
            
            return r;
        }

        private PolizaMovPolEkDTO generarPolizaAccion(DateTime fechaMovimiento, List<PQPolizaDTO> polizaPQ)
        {
            var poliza_movpol = new PolizaMovPolEkDTO();

            var poliza = new sc_polizasDTO();
            var movimientos = new List<Core.DTO.Enkontrol.Tablas.Poliza.sc_movpolDTO>();

            var empleado = _context.tblP_Usuario_Enkontrol.FirstOrDefault(f => f.id == vSesiones.sesionUsuarioDTO.id);

            int linea = 1;

            foreach (var mov in polizaPQ)
            {
                var movpol = new Core.DTO.Enkontrol.Tablas.Poliza.sc_movpolDTO();

                var arrCuenta = mov.cuenta.Split('-');

                movpol.year = fechaMovimiento.Year;
                movpol.mes = fechaMovimiento.Month;
                movpol.tp = "03";
                movpol.linea = linea;
                movpol.cta = Convert.ToInt32(arrCuenta[0]);
                movpol.scta = Convert.ToInt32(arrCuenta[1]);
                movpol.sscta = Convert.ToInt32(arrCuenta[2]);
                movpol.digito = Convert.ToInt32(arrCuenta[3]);
                movpol.tm = mov.tm;
                movpol.referencia = mov.referencia.ToString();
                movpol.cc = mov.cc;
                movpol.concepto = mov.concepto;
                movpol.monto = mov.monto;
                movpol.itm = mov.itm;

                movimientos.Add(movpol);

                linea++;
            }

            poliza.year = fechaMovimiento.Year;
            poliza.mes = fechaMovimiento.Month;
            poliza.tp = "03";
            poliza.fechapol = fechaMovimiento;
            poliza.cargos = movimientos.Where(w => w.tm == (int)TipoMovimientoEnum.Cargo || w.tm == (int)TipoMovimientoEnum.CargoRojo).Sum(s => s.monto);
            poliza.abonos = movimientos.Where(w => w.tm == (int)TipoMovimientoEnum.Abono || w.tm == (int)TipoMovimientoEnum.AbonoRojo).Sum(s => s.monto);
            poliza.generada = "C";
            poliza.status = "C";
            poliza.status_lock = "N";
            poliza.fec_hora_movto = DateTime.Now;
            poliza.usuario_movto = null;
            poliza.fecha_hora_crea = poliza.fec_hora_movto;
            poliza.usuario_crea = empleado != null ? empleado.empleado : 1;
            poliza.concepto = "Póliza de DIARIO";

            poliza_movpol.poliza = poliza;
            poliza_movpol.movimientos = movimientos;

            return poliza_movpol;
        }

        private string ObtenerReferencia(DateTime fecha, int ctaBanco)
        {
            int referenciaInicial = 100000;
            int ultimaReferenciaPQ = _context.tblAF_DxP_PQ.Select(m => m.id).OrderByDescending(o => o).FirstOrDefault();
            referenciaInicial += ultimaReferenciaPQ;
            do
            {
            } while (!_polizaEkFS.ReferenciaDisponible(fecha, ctaBanco, referenciaInicial++));

            string referencia = (referenciaInicial - 1).ToString();

            return referencia;
        }

        private PolizaMovPolEkDTO generarPolizaPQ(tblAF_DxP_PQ pq)
        {
            var poliza_movpol = new PolizaMovPolEkDTO();

            var poliza = new sc_polizasDTO();
            var movimientos = new List<Core.DTO.Enkontrol.Tablas.Poliza.sc_movpolDTO>();

            var empleado = _context.tblP_Usuario_Enkontrol.FirstOrDefault(f => f.id == vSesiones.sesionUsuarioDTO.id);

            int linea = 1;

            string referencia = ObtenerReferencia(pq.fechaFirma, pq.ctaAbonoBanco);

            //linea 1
            var movimiento1 = new Core.DTO.Enkontrol.Tablas.Poliza.sc_movpolDTO();
            movimiento1.year = pq.fechaFirma.Year;
            movimiento1.mes = pq.fechaFirma.Month;
            movimiento1.tp = "03";
            movimiento1.linea = linea;
            movimiento1.cta = pq.ctaAbonoBanco;
            movimiento1.scta = pq.sctaAbonoBanco;
            movimiento1.sscta = pq.ssctaAbonoBanco;
            movimiento1.digito = pq.digitoAbonoBanco;
            movimiento1.tm = (int)TipoMovimientoEnum.Abono;
            movimiento1.referencia = referencia;
            movimiento1.cc = pq.cc;

            string importeCorto = "";
            string escala = "";
            if (pq.importe >= 1000000)
            {
                importeCorto = string.Format("{0:0.00}", pq.importe / 1000000);
                escala = pq.moneda.esMXN ? "MDP" : "DLLS";
            }
            if (pq.importe >= 1000 && pq.importe < 1000000)
            {
                importeCorto = string.Format("{0:0.00}", pq.importe / 1000);
                escala = pq.moneda.esMXN ? "MIL PESOS" : "MIL DOLARES";
            }
            movimiento1.concepto = "DOC PQ " + importeCorto + " " + escala + " TASA " + pq.interes + "%";

            movimiento1.monto = (pq.importe * pq.tipoCambio.Value) * -1;
            movimiento1.iclave = 0;
            movimiento1.itm = 0;
            movimientos.Add(movimiento1);

            linea++;

            //linea 2
            var movimiento2 = new Core.DTO.Enkontrol.Tablas.Poliza.sc_movpolDTO();
            movimiento2.year = movimiento1.year;
            movimiento2.mes = movimiento1.mes;
            movimiento2.tp = movimiento1.tp;
            movimiento2.linea = linea;
            movimiento2.cta = pq.ctaCargoBanco;
            movimiento2.scta = pq.sctaCargoBanco;
            movimiento2.sscta = pq.ssctaCargoBanco;
            movimiento2.digito = pq.digitoCargoBanco;
            movimiento2.tm = (int)TipoMovimientoEnum.Cargo;
            movimiento2.referencia = movimiento1.referencia;
            movimiento2.cc = movimiento1.cc;
            movimiento2.concepto = movimiento1.concepto;
            movimiento2.monto = pq.importe;
            movimiento2.iclave = 0;
            movimiento2.itm = 5;
            movimientos.Add(movimiento2);

            linea++;

            //linea 3
            var movimiento3 = new Core.DTO.Enkontrol.Tablas.Poliza.sc_movpolDTO();
            if (!pq.moneda.esMXN)
            {
                var infoComplementaria = _context.tblAF_DxP_RelInstitucionCta.FirstOrDefault(f => f.activo && f.institucionID == pq.bancoId && f.complementaria);
                movimiento3.year = movimiento1.year;
                movimiento3.mes = movimiento1.mes;
                movimiento3.tp = movimiento1.tp;
                movimiento3.linea = linea;
                movimiento3.cta = infoComplementaria.cta;
                movimiento3.scta = infoComplementaria.scta;
                movimiento3.sscta = infoComplementaria.sscta;
                movimiento3.digito = infoComplementaria.digito;
                movimiento3.tm = (int)TipoMovimientoEnum.Cargo;
                movimiento3.referencia = movimiento1.referencia;
                movimiento3.cc = movimiento1.cc;
                movimiento3.concepto = movimiento1.concepto;
                movimiento3.monto = (pq.importe * pq.tipoCambio.Value) - pq.importe;
                movimiento3.iclave = 0;
                movimiento3.itm = 5;
                movimientos.Add(movimiento3);
            }

            //poliza
            poliza.year = movimiento1.year;
            poliza.mes = movimiento1.mes;
            poliza.tp = "03";
            poliza.fechapol = pq.fechaFirma;
            poliza.cargos = movimiento2.monto + (pq.moneda.esMXN ? 0 : movimiento3.monto);
            poliza.abonos = movimiento1.monto;
            poliza.generada = "C";
            poliza.status = "C";
            poliza.status_lock = "N";
            poliza.fec_hora_movto = DateTime.Now;
            poliza.usuario_movto = null;
            poliza.fecha_hora_crea = poliza.fec_hora_movto;
            poliza.usuario_crea = empleado != null ? empleado.empleado : 1;
            poliza.concepto = "Póliza de DIARIO";

            poliza_movpol.poliza = poliza;
            poliza_movpol.movimientos = movimientos;

            return poliza_movpol;
        }

        private List<Core.DTO.Enkontrol.Tablas.Poliza.sb_edo_cta_chequeraDTO> GenerarInfoConcilia(List<Core.DTO.Enkontrol.Tablas.Poliza.sc_movpolDTO> movimientos, tblAF_DxP_PQ pq, sc_polizasDTO poliza)
        {
            var infoAConciliar = new List<Core.DTO.Enkontrol.Tablas.Poliza.sb_edo_cta_chequeraDTO>();

            var tipoCambio = getTipoCambioDLLs(poliza.fechapol);

            foreach (var mov in movimientos)
            {
                var cuentaInstitucion = _context.tblAF_DxP_RelInstitucionCta
                    .FirstOrDefault(f =>
                        f.institucionID == pq.bancoId &&
                        f.activo &&
                        f.cta == mov.cta &&
                        f.scta == mov.scta &&
                        f.sscta == mov.sscta
                    );

                if (cuentaInstitucion != null)
                {
                    var cuentaBanco = _bancoFS.GetBanco(mov.cta, mov.scta, mov.sscta);

                    if (cuentaBanco != null)
                    {
                        var concilia = new Core.DTO.Enkontrol.Tablas.Poliza.sb_edo_cta_chequeraDTO();

                        concilia.cuenta = cuentaBanco.cuenta;
                        concilia.fecha_mov = poliza.fechapol;
                        concilia.tm = mov.itm;
                        concilia.numero = Convert.ToInt32(mov.referencia);
                        concilia.cc = mov.cc;
                        concilia.descripcion = mov.concepto;
                        concilia.monto = mov.monto;
                        if (pq.moneda.esMXN)
                        {
                            concilia.tc = 1M;
                        }
                        else
                        {
                            concilia.tc = !cuentaInstitucion.complementaria ? tipoCambio.tipo_cambio : 1M;
                        }
                        concilia.origen_mov = "C";
                        concilia.generada = "C";
                        concilia.iyear = mov.year;
                        concilia.imes = mov.mes;
                        concilia.ipoliza = poliza.poliza;
                        concilia.itp = mov.tp;
                        concilia.ilinea = mov.linea;
                        concilia.banco = cuentaBanco.banco;

                        infoAConciliar.Add(concilia);
                    }
                    else
                    {
                        throw new Exception("Error al obtener el número de cuenta banco");
                    }
                }
            }

            return infoAConciliar;
        }
        #endregion

        #region POLIZA REVALUACION
        public Dictionary<string, object> GetInfoRevaluacion(DateTime fecha)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var tipoCambio = getTipoCambioDLLs(fecha);
                var fechaAnterior = fecha.AddMonths(-1);
                fechaAnterior = new DateTime(fechaAnterior.Year, fechaAnterior.Month, DateTime.DaysInMonth(fechaAnterior.Year, fechaAnterior.Month));
                var tipoCambioAnterior = getTipoCambioDLLs(fechaAnterior);

                var detalles = new List<PolizaRevaluacionDetalleDTO>();

                var contratos = _context.tblAF_DxP_ContratoMaquinasDetalle
                    .Where(w =>
                        w.ContratoMaquina.Contrato.fechaFirma <= fecha.Date &&
                        !w.ContratoMaquina.Contrato.Terminado &&
                        !w.ContratoMaquina.Contrato.arrendamientoPuro &&
                        w.ContratoMaquina.Contrato.monedaContrato == (int)TipoMonedaEnum.USD)
                    .ToList();

                var contratosPQs = new List<tblAF_DxP_PQ>();

                if ((int)EmpresaEnum.Construplan == vSesiones.sesionEmpresaActual)
                {
                    var movimientosActivos = new List<int> { 1, 3, 5 };

                    contratosPQs = _context.tblAF_DxP_PQ
                        .Where(x =>
                            x.estatus &&
                            movimientosActivos.Contains(x.tipoMovimientoId) &&
                            x.monedaId == (int)TipoMonedaEnum.USD &&
                            x.fechaFirma <= fecha.Date
                        ).ToList();

                    foreach (var item in contratosPQs)
                    {
                        #region cortoPlazoEkontrol
//                        var cortoPlazoContabilidadPQ = 0M;

//                        var query_sc_salcont_cc = new OdbcConsultaDTO();

//                        query_sc_salcont_cc.consulta =
//                            @"SELECT
//                                *
//                            FROM
//                                sc_salcont_cc
//                            WHERE
//                                cta = ? AND
//                                scta = ? AND
//                                sscta = ? AND
//                                year = ? AND
//                                cc = ?";

//                        query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
//                        {
//                            nombre = "cta",
//                            tipo = OdbcType.Int,
//                            valor = item.ctaAbonoBanco
//                            //valor = gbFolio.First().ContratoMaquina.Contrato.cta
//                        });
//                        query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
//                        {
//                            nombre = "scta",
//                            tipo = OdbcType.Int,
//                            valor = item.sctaAbonoBanco
//                            //valor = gbFolio.First().ContratoMaquina.Contrato.scta
//                        });
//                        query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
//                        {
//                            nombre = "sscta",
//                            tipo = OdbcType.Int,
//                            valor = item.ssctaAbonoBanco
//                            //valor = gbFolio.First().ContratoMaquina.Contrato.sscta
//                        });
//                        query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
//                        {
//                            nombre = "year",
//                            tipo = OdbcType.Int,
//                            valor = fecha.Year
//                        });
//                        query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
//                        {
//                            nombre = "cc",
//                            tipo = OdbcType.NVarChar,
//                            valor = item.cc
//                        });

//                        var sc_salcont_cc = _contextEnkontrol.Select<sc_salcont_ccDTO>(vSesiones.sesionAmbienteEnkontrolAdm, query_sc_salcont_cc);

//                        for (int mes = 1; mes <= fecha.Month; mes++)
//                        {
//                            var mesCargo = Enum.GetName(typeof(Core.Enum.Contabilidad.Poliza.MesCargoEnum), mes);
//                            var mesAbono = Enum.GetName(typeof(Core.Enum.Contabilidad.Poliza.MesAbonoEnum), mes);

//                            cortoPlazoContabilidadPQ += sc_salcont_cc.Sum(s => Convert.ToDecimal(s.GetType().GetProperty(mesCargo).GetValue(s, null)) + Convert.ToDecimal(s.GetType().GetProperty(mesAbono).GetValue(s, null)));
//                        }
                        #endregion

                        var cortoPlazoContabilidadPQ = 0M;
                        cortoPlazoContabilidadPQ = item.importe;

                        #region cortoPlazoSigoplan
                        var cortoPlazoPQ = 0M;

                        cortoPlazoPQ += item.importe;

                        foreach (var abono in item.abonos.Where(x => x.estatus && new DateTime(x.fecha.Year, x.fecha.Month, 1) <= new DateTime(fecha.Year, fecha.Month, 1)))
                        {
                            cortoPlazoPQ += abono.importe;
                        }
                        #endregion

                        var detallePQ = new PolizaRevaluacionDetalleDTO();
                        detallePQ.esPQ = true;
                        detallePQ.proveedor = item.banco.Nombre;
                        detallePQ.contrato = "PQ " + item.cc + " " + (int)item.importe;
                        detallePQ.activo = "";
                        detallePQ.cc = item.cc;
                        detallePQ.conceptoPQ = "REVALUACION POR " + (int)item.importe + " DLLS " + tipoCambio.tipo_cambio.ToString("0.0000##");
                        detallePQ.tipoCambio = tipoCambio.tipo_cambio;
                        detallePQ.deudaCP = cortoPlazoPQ * -1;
                        detallePQ.valuacionCP = detallePQ.deudaCP * tipoCambio.tipo_cambio;
                        if (new DateTime(item.fechaFirma.Year, item.fechaFirma.Month, 1) >= new DateTime(fecha.Year, fecha.Month, 1))
                        {
                            //detallePQ.contabilidadCP = (item.tipoCambio.Value * item.importe) * -1;
                            detallePQ.contabilidadCP = (item.tipoCambio.Value * cortoPlazoPQ) * -1;
                        }
                        else
                        {
                            //detallePQ.contabilidadCP = (item.importe * tipoCambioAnterior.tipo_cambio) * -1;
                            detallePQ.contabilidadCP = (cortoPlazoPQ * tipoCambioAnterior.tipo_cambio) * -1;
                        }
                        detallePQ.diferenciaCP = detallePQ.valuacionCP - detallePQ.contabilidadCP;
                        detallePQ.gananciaPerdidaCambiaria = (detallePQ.diferenciaCP) * -1;

                        detallePQ.ctaCP = item.ctaAbonoBanco;
                        detallePQ.sctaCP = item.sctaAbonoBanco;
                        detallePQ.ssctaCP = item.ssctaAbonoBanco;
                        detallePQ.digitoCP = item.digitoAbonoBanco;

                        if (detallePQ.deudaCP != 0)
                        {
                            detalles.Add(detallePQ);
                        }
                    }
                }

                foreach (var gbInstitucion in contratos.GroupBy(g => g.ContratoMaquina.Contrato.Institucion.Nombre).OrderBy(o => o.Key))
                {
                    foreach (var gbFolio in gbInstitucion.GroupBy(g => g.ContratoMaquina.Contrato.Folio).OrderBy(o => o.Key))
                    {
                        #region cortoPlazoEkontrol
                        var cortoPlazoContabilidad = 0M;

                        var query_sc_salcont_cc = new OdbcConsultaDTO();

                        query_sc_salcont_cc.consulta =
                            @"SELECT
                                *
                            FROM
                                sc_salcont_cc
                            WHERE
                                cta = ? AND
                                scta = ? AND
                                sscta = ? AND
                                year = ?";

                        query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                        {
                            nombre = "cta",
                            tipo = OdbcType.Int,
                            valor = gbFolio.First().ContratoMaquina.Contrato.cta
                        });
                        query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                        {
                            nombre = "scta",
                            tipo = OdbcType.Int,
                            valor = gbFolio.First().ContratoMaquina.Contrato.scta
                        });
                        query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                        {
                            nombre = "sscta",
                            tipo = OdbcType.Int,
                            valor = gbFolio.First().ContratoMaquina.Contrato.sscta
                        });
                        query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                        {
                            nombre = "year",
                            tipo = OdbcType.Int,
                            valor = fecha.Year
                        });

                        var sc_salcont_cc = _contextEnkontrol.Select<sc_salcont_ccDTO>(vSesiones.sesionAmbienteEnkontrolAdm, query_sc_salcont_cc);

                        for (int mes = 1; mes <= fecha.Month; mes++)
                        {
                            var mesCargo = Enum.GetName(typeof(Core.Enum.Contabilidad.Poliza.MesCargoEnum), mes);
                            var mesAbono = Enum.GetName(typeof(Core.Enum.Contabilidad.Poliza.MesAbonoEnum), mes);

                            cortoPlazoContabilidad += sc_salcont_cc.Sum(s => Convert.ToDecimal(s.GetType().GetProperty(mesCargo).GetValue(s, null)) + Convert.ToDecimal(s.GetType().GetProperty(mesAbono).GetValue(s, null)));
                        }
                        cortoPlazoContabilidad += sc_salcont_cc.Sum(x => x.salini);
                        #endregion

                        #region largoPlazoEnkontrol
                        var largoPlazoContabilidad = 0M;

                        catctaDTO infoCuentaLargoPlazo = null;

                        if (gbFolio.First().ContratoMaquina.Contrato.ctaLp.HasValue)
                        {
                            infoCuentaLargoPlazo = new catctaDTO();
                            infoCuentaLargoPlazo.cta = gbFolio.First().ContratoMaquina.Contrato.ctaLp.Value;
                            infoCuentaLargoPlazo.scta = gbFolio.First().ContratoMaquina.Contrato.sctaLp.Value;
                            infoCuentaLargoPlazo.sscta = gbFolio.First().ContratoMaquina.Contrato.ssctaLp.Value;
                        }
                        else
                        {
                            var query_catcta = new OdbcConsultaDTO();

                            query_catcta.consulta =
                                @"SELECT
                                    cta,
                                    scta,
                                    sscta
                                FROM
                                    catcta
                                WHERE
                                    cta = 2135 AND
                                    descripcion LIKE (SELECT TOP 1 descripcion FROM catcta WHERE cta = ? AND scta = ? AND sscta = ?)";

                            query_catcta.parametros.Add(new OdbcParameterDTO
                            {
                                nombre = "cta",
                                tipo = OdbcType.Int,
                                valor = gbFolio.First().ContratoMaquina.Contrato.cta
                            });
                            query_catcta.parametros.Add(new OdbcParameterDTO
                            {
                                nombre = "scta",
                                tipo = OdbcType.Int,
                                valor = gbFolio.First().ContratoMaquina.Contrato.scta
                            });
                            query_catcta.parametros.Add(new OdbcParameterDTO
                            {
                                nombre = "sscta",
                                tipo = OdbcType.Int,
                                valor = gbFolio.First().ContratoMaquina.Contrato.sscta
                            });

                            infoCuentaLargoPlazo = _contextEnkontrol.Select<catctaDTO>(vSesiones.sesionAmbienteEnkontrolAdm, query_catcta).FirstOrDefault();
                        }

                        if (infoCuentaLargoPlazo != null)
                        {
                            var query_lp_sc_salcont_cc = new OdbcConsultaDTO();

                            query_lp_sc_salcont_cc.consulta =
                                @"SELECT
                                    *
                                FROM
                                    sc_salcont_cc
                                WHERE
                                    cta = ? AND
                                    scta = ? AND
                                    sscta = ? AND
                                    year = ?";

                            query_lp_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                            {
                                nombre = "cta",
                                tipo = OdbcType.Int,
                                valor = infoCuentaLargoPlazo.cta
                            });
                            query_lp_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                            {
                                nombre = "scta",
                                tipo = OdbcType.Int,
                                valor = infoCuentaLargoPlazo.scta
                            });
                            query_lp_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                            {
                                nombre = "sscta",
                                tipo = OdbcType.Int,
                                valor = infoCuentaLargoPlazo.sscta
                            });
                            query_lp_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                            {
                                nombre = "year",
                                tipo = OdbcType.Int,
                                valor = fecha.Year
                            });

                            var lp_sc_salcont_cc = _contextEnkontrol.Select<sc_salcont_ccDTO>(vSesiones.sesionAmbienteEnkontrolAdm, query_lp_sc_salcont_cc);

                            for (int mes = 1; mes <= fecha.Month; mes++)
                            {
                                var mesCargo = Enum.GetName(typeof(Core.Enum.Contabilidad.Poliza.MesCargoEnum), mes);
                                var mesAbono = Enum.GetName(typeof(Core.Enum.Contabilidad.Poliza.MesAbonoEnum), mes);

                                largoPlazoContabilidad += lp_sc_salcont_cc.Sum(s => Convert.ToDecimal(s.GetType().GetProperty(mesCargo).GetValue(s, null)) + Convert.ToDecimal(s.GetType().GetProperty(mesAbono).GetValue(s, null)));
                            }

                            largoPlazoContabilidad += lp_sc_salcont_cc.Sum(s => s.salini);
                        }
                        #endregion

                        #region cortoPlazoSigoplan y largoPlazoSigoplan
                        var cortoPlazo = 0M;
                        var largoPlazo = 0M;

                        foreach (var folio in gbFolio)
                        {
                            if ((folio.FechaVencimiento.Date > fecha.Date && folio.FechaVencimiento.Year == fecha.Year && !folio.Pagado) || (folio.Pagado && folio.FechaPago.HasValue && folio.FechaPago.Value.Year == fecha.Year && folio.FechaPago.Value > fecha.Date))
                            {
                                cortoPlazo += folio.Importe;
                            }
                            if ((folio.FechaVencimiento.Date <= fecha.Date && !folio.Pagado))
                            {
                                cortoPlazo += folio.Importe;
                            }
                            if ((folio.FechaVencimiento.Year > fecha.Year && !folio.Pagado) || (folio.Pagado && folio.FechaPago.HasValue && folio.FechaPago.Value.Year > fecha.Year))
                            {
                                largoPlazo += folio.Importe;
                            }
                        }
                        #endregion

                        var detalle = new PolizaRevaluacionDetalleDTO();
                        detalle.proveedor = gbInstitucion.Key;
                        detalle.contrato = gbFolio.Key;
                        detalle.activo = gbFolio.First().ContratoMaquina.Contrato.Descripcion;
                        detalle.tipoCambio = tipoCambio.tipo_cambio;
                        detalle.deudaCP = cortoPlazo * -1;
                        detalle.valuacionCP = detalle.deudaCP * tipoCambio.tipo_cambio;
                        detalle.contabilidadCP = cortoPlazoContabilidad;
                        detalle.diferenciaCP = detalle.valuacionCP - detalle.contabilidadCP;
                        detalle.deudaLP = largoPlazo * -1;
                        detalle.valuacionLP = detalle.deudaLP * tipoCambio.tipo_cambio;
                        detalle.contabilidadLP = largoPlazoContabilidad;
                        detalle.diferenciaLP = detalle.valuacionLP - detalle.contabilidadLP;
                        detalle.gananciaPerdidaCambiaria = (detalle.diferenciaCP + detalle.diferenciaLP) * -1;

                        detalle.ctaCP = gbFolio.First().ContratoMaquina.Contrato.cta;
                        detalle.sctaCP = gbFolio.First().ContratoMaquina.Contrato.scta;
                        detalle.ssctaCP = gbFolio.First().ContratoMaquina.Contrato.sscta;
                        detalle.digitoCP = gbFolio.First().ContratoMaquina.Contrato.digito;

                        if (infoCuentaLargoPlazo != null)
                        {
                            detalle.ctaLP = infoCuentaLargoPlazo.cta;
                            detalle.sctaLP = infoCuentaLargoPlazo.scta;
                            detalle.ssctaLP = infoCuentaLargoPlazo.sscta;
                            detalle.digitoLP = infoCuentaLargoPlazo.digito;
                        }

                        if (detalle.deudaCP != 0 || detalle.deudaLP != 0)
                        {
                            detalles.Add(detalle);
                        }
                    }
                }

                var poliza = CrearPolizaRevaluacion(detalles, fecha);

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, detalles);
                resultado.Add("poliza", poliza);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, ex.Message);

                LogError(2, 0, "ContratosController", "GetInfoRevaluacion", ex, AccionEnum.CONSULTA, 0, fecha);
            }

            return resultado;
        }

        public Dictionary<string, object> RegistrarPolizaRevaluacion(PolizaMovPolEkDTO poliza)
        {
            var resultado = new Dictionary<string, object>();

            var permisoUsuario = new List<int> { 3978, 6571, 13, 6587, 79888, 1073 };

            using (var transaccionSP = _context.Database.BeginTransaction())
            {
                using (var conexionEK = new Conexion().ConexionEKAdm())
                {
                    using (var transaccionEK = conexionEK.BeginTransaction())
                    {
                        try
                        {
                            if (permisoUsuario.Contains(vSesiones.sesionUsuarioDTO.id))
                            {
                                _polizaEkFS.SetContext(conexionEK);
                                _polizaEkFS.SetTransaccion(transaccionEK);
                                string numeroPoliza = _polizaEkFS.GuardarPoliza(poliza.poliza, poliza.movimientos);

                                _polizaSPFS.SetContext(_context);
                                _polizaSPFS.SetTransaccion(transaccionSP);
                                var polizaPoliza = numeroPoliza.Split('-')[2];
                                poliza.poliza.poliza = Convert.ToInt32(polizaPoliza);
                                var resultadoSP = _polizaSPFS.GuardarPoliza(poliza.poliza, poliza.movimientos);

                                transaccionEK.Commit();
                                transaccionSP.Commit();

                                resultado.Add(SUCCESS, true);
                                resultado.Add("poliza", numeroPoliza);
                            }
                            else
                            {
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "No cuenta con permisos para realizar esta operación");
                            }
                        }
                        catch (Exception ex)
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, ex.Message);

                            LogError(2, 0, "ContratosController", "RegistrarPolizaRevaluacion", ex, AccionEnum.AGREGAR, 0, poliza.poliza);
                        }
                    }
                }
            }

            return resultado;
        }

        private PolizaMovPolEkDTO CrearPolizaRevaluacion(List<PolizaRevaluacionDetalleDTO> detalles, DateTime fecha)
        {
            var poliza_movpol = new PolizaMovPolEkDTO();

            var poliza = new sc_polizasDTO();
            var movimientos = new List<Core.DTO.Enkontrol.Tablas.Poliza.sc_movpolDTO>();

            var empleado = _context.tblP_Usuario_Enkontrol.FirstOrDefault(f => f.idUsuario == vSesiones.sesionUsuarioDTO.id);

            int linea = 1;

            foreach (var gbProveedor in detalles.Where(x => !x.esPQ).GroupBy(g => g.proveedor))
            {
                var diferenciaCambiaria = gbProveedor.Sum(s => s.gananciaPerdidaCambiaria);

                if (diferenciaCambiaria == 0)
                {
                    continue;
                }

                foreach (var detalle in gbProveedor)
                {
                    var tm = 0;
                    if (detalle.gananciaPerdidaCambiaria == 0)
                    {
                        continue;
                    }
                    if (detalle.gananciaPerdidaCambiaria > 0)
                    {
                        tm = 2;
                    }
                    if (detalle.gananciaPerdidaCambiaria < 0)
                    {
                        tm = 1;
                    }

                    for (int i = 0; i < 2; i++)
                    {
                        var mov = new Core.DTO.Enkontrol.Tablas.Poliza.sc_movpolDTO();
                        mov.year = fecha.Year;
                        mov.mes = fecha.Month;
                        mov.tp = "03";
                        mov.linea = linea;
                        mov.cta = i == 0 ? detalle.ctaCP : detalle.ctaLP;
                        mov.scta = i == 0 ? detalle.sctaCP : detalle.sctaLP;
                        mov.sscta = i == 0 ? detalle.ssctaCP : detalle.ssctaLP;
                        mov.digito = i == 0 ? detalle.digitoCP : detalle.digitoLP;
                        mov.tm = tm;
                        mov.referencia = "REVALU";
                        mov.cc = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? "191" : "990";
                        mov.concepto = "REVALUACION POR TC " + detalle.tipoCambio;
                        mov.monto = i == 0 ? detalle.diferenciaCP : detalle.diferenciaLP;
                        mov.area = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? (int?)16 : null;
                        mov.cuenta_oc = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? (int?)2 : null;
                        movimientos.Add(mov);

                        linea++;
                    }
                }

                //5900 costo
                //4900 ingreso

                var movCostoIngreso = new Core.DTO.Enkontrol.Tablas.Poliza.sc_movpolDTO();
                movCostoIngreso.year = fecha.Year;
                movCostoIngreso.mes = fecha.Month;
                movCostoIngreso.tp = "03";
                movCostoIngreso.linea = linea;
                movCostoIngreso.cta = diferenciaCambiaria > 0 ? 5900 : 4900;
                movCostoIngreso.scta = diferenciaCambiaria > 0 ? 6 : 4;
                movCostoIngreso.sscta = diferenciaCambiaria > 0 ? 0 : 0;
                movCostoIngreso.digito = diferenciaCambiaria > 0 ? 0 : 9;
                movCostoIngreso.tm = diferenciaCambiaria > 0 ? 1 : 2;
                movCostoIngreso.referencia = "DOCTOS";
                movCostoIngreso.cc = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? "191" : "990";
                movCostoIngreso.concepto = movimientos.First().concepto;
                movCostoIngreso.monto = diferenciaCambiaria;
                movCostoIngreso.area = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? (int?)16 : null;
                movCostoIngreso.cuenta_oc = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? (int?)2 : null;
                movimientos.Add(movCostoIngreso);

                linea++;
            }

            #region PQ
            foreach (var gbProveedor in detalles.Where(x => x.esPQ))
            {
                var diferenciaCambiaria = gbProveedor.gananciaPerdidaCambiaria;

                if (diferenciaCambiaria == 0)
                {
                    continue;
                }

                var tm = 0;
                if (gbProveedor.gananciaPerdidaCambiaria == 0)
                {
                    continue;
                }
                if (gbProveedor.gananciaPerdidaCambiaria > 0)
                {
                    tm = 2;
                }
                if (gbProveedor.gananciaPerdidaCambiaria < 0)
                {
                    tm = 1;
                }

                var mov = new Core.DTO.Enkontrol.Tablas.Poliza.sc_movpolDTO();
                mov.year = fecha.Year;
                mov.mes = fecha.Month;
                mov.tp = "03";
                mov.linea = linea;
                mov.cta = gbProveedor.ctaCP;
                mov.scta = gbProveedor.sctaCP;
                mov.sscta = gbProveedor.ssctaCP;
                mov.digito = gbProveedor.digitoCP;
                mov.tm = tm;
                mov.referencia = "REVALU";
                //mov.cc = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? "191" : "990";
                mov.cc = gbProveedor.cc;
                //mov.concepto = "REVALUACION POR TC " + gbProveedor.tipoCambio;
                mov.concepto = gbProveedor.conceptoPQ;
                mov.monto = gbProveedor.diferenciaCP;
                mov.area = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? (int?)16 : null;
                mov.cuenta_oc = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? (int?)2 : null;
                movimientos.Add(mov);

                linea++;

                //5900 costo
                //4900 ingreso

                var movCostoIngreso = new Core.DTO.Enkontrol.Tablas.Poliza.sc_movpolDTO();
                movCostoIngreso.year = fecha.Year;
                movCostoIngreso.mes = fecha.Month;
                movCostoIngreso.tp = "03";
                movCostoIngreso.linea = linea;
                movCostoIngreso.cta = diferenciaCambiaria > 0 ? 5900 : 4900;
                movCostoIngreso.scta = diferenciaCambiaria > 0 ? 6 : 4;
                movCostoIngreso.sscta = diferenciaCambiaria > 0 ? 0 : 0;
                movCostoIngreso.digito = diferenciaCambiaria > 0 ? 0 : 9;
                movCostoIngreso.tm = diferenciaCambiaria > 0 ? 1 : 2;
                movCostoIngreso.referencia = "DOCTOS";
                movCostoIngreso.cc = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? "191" : "990";
                movCostoIngreso.concepto = movimientos.First().concepto;
                movCostoIngreso.monto = diferenciaCambiaria;
                movCostoIngreso.area = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? (int?)16 : null;
                movCostoIngreso.cuenta_oc = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? (int?)2 : null;
                movimientos.Add(movCostoIngreso);

                linea++;
            }
            #endregion

            poliza.year = fecha.Year;
            poliza.mes = fecha.Month;
            poliza.tp = "03";
            poliza.fechapol = fecha.Date;
            poliza.cargos = movimientos.Where(w => w.tm == (int)TipoMovimientoEnum.Cargo || w.tm == (int)TipoMovimientoEnum.CargoRojo).Sum(s => s.monto);
            poliza.abonos = movimientos.Where(w => w.tm == (int)TipoMovimientoEnum.Abono || w.tm == (int)TipoMovimientoEnum.AbonoRojo).Sum(s => s.monto);
            poliza.fecha_hora_crea = DateTime.Now;
            poliza.usuario_crea = empleado.empleado;
            poliza.concepto = "VALUACION DE DOCUMENTOS POR PAGAR DE " + fecha.ToString("MMMM").ToUpper() + " " + fecha.Year;

            poliza_movpol.poliza = poliza;
            poliza_movpol.movimientos = movimientos;

            return poliza_movpol;
        }
        #endregion

        public Dictionary<string, object> CargarReporteInteresesPagados(DateTime fecha)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var anioSeleccionado = fecha.Year;
                var anioSiguiente = anioSeleccionado + 1;
                var primerDiaAnio = new DateTime(anioSeleccionado, 1, 1);
                var siguienteDiaFecha = fecha.AddDays(1);
                var ultimoDiaAnio = new DateTime(anioSeleccionado, 12, 31);
                var primerDiaSiguienteAnio = new DateTime(anioSiguiente, 1, 1);

                var data = _context.Select<ReporteInteresesPagadosDTO>(new DapperDTO
                {
                    baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                    consulta = string.Format(@"
                    SELECT * FROM (
                        SELECT
                            i.nombre,
                            CASE
		                        WHEN c.monedaContrato = 1 THEN 'MN'
		                        ELSE 'DLLS'
                            END AS 'Moneda',
	                        c.ctaIA AS Cta,
	                        c.sctaIA AS Scta,
	                        c.ssctaIA AS Sscta,
	                        c.descripcion AS Contrato,
	                        c.folio AS Folio,
	                        c.tipoCambio,
	                        (
		                        SELECT
			                        SUM(intereses) * c.tipoCambio
		                        FROM
			                        tblAF_DxP_ContratoDetalle AS d
		                        WHERE
                                    (
			                            d.estatus = 1 and
			                            d.fechaVencimiento >= '{0}' and
			                            d.fechaVencimiento <= '{1}' and
			                            d.pagado = 1 and
                                        d.fechaPago <= '{4}' and
			                            d.contratoId = c.id
                                    ) or
                                    (
                                        d.estatus = 1 and
                                        d.fechaVencimiento >= '{4}' and
                                        d.pagado = 1 and
                                        d.fechaPago is not null and
                                        d.fechaPago <= '{1}' and
                                        d.fechaPago >= '{0}' and
                                        d.contratoId = c.id
                                    )
	                        ) AS Pagado,
	                        (
		                        SELECT
			                        SUM(intereses) * c.tipoCambio
		                        FROM
			                        tblAF_DxP_ContratoDetalle AS d
		                        WHERE
			                        d.estatus = 1 and
			                        d.fechaPago >= '{2}' and
			                        d.fechaPago <= '{3}' and
			                        d.contratoId = c.id
	                        ) AS CP,
	                        (
		                        SELECT
			                        SUM(intereses) * tipoCambio
		                        FROM
			                        tblAF_DxP_ContratoDetalle AS d
		                        WHERE
			                        d.estatus = 1 and
			                        (d.fechaPago >= '{4}' or (d.fechaPago is null and d.fechaVencimiento >= '{4}')) and
			                        d.contratoId = c.id
	                        ) AS LP,
                            (
                                SELECT TOP 1
                                    d.pagado
                                FROM
                                    tblAF_DxP_ContratoDetalle AS d
                                WHERE
                                    d.estatus = 1 AND
                                    d.parcialidad = c.plazo AND
                                    d.contratoId = c.id AND
                                    d.pagado = 1
                            ) as yaEstaPagado
                        FROM
	                        tblAF_DxP_Contrato AS c
                        INNER JOIN
	                        tblAF_DxP_Institucion AS i
	                        ON
		                        i.id = c.institucionId
                        WHERE
	                        c.estatus = 1 and
	                        --c.monedaContrato = 2 and
	                        c.terminado = 0 and
	                        c.arrendamientoPuro = 0 and
                            c.fechaFirma <= '{5}'
                    ) AS resultado WHERE resultado.yaEstaPagado is NULL ORDER BY resultado.nombre, resultado.Moneda", primerDiaAnio.ToShortDateString(), fecha.ToShortDateString(), siguienteDiaFecha.ToShortDateString(), ultimoDiaAnio.ToShortDateString(), primerDiaSiguienteAnio.ToShortDateString(), fecha.ToShortDateString())
                });

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(2, 0, "ContratosController", "CargarReporteInteresesPagados", e, AccionEnum.CONSULTA, 0, fecha);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> DescargarExcelInteresesPagados(DateTime fecha)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var obtenerReporte = CargarReporteInteresesPagados(fecha);
                if ((bool)obtenerReporte[SUCCESS])
                {
                    var reporte = obtenerReporte["data"] as List<ReporteInteresesPagadosDTO>;

                    using (var excel = new ExcelPackage())
                    {
                        var hoja = excel.Workbook.Worksheets.Add("Intereses pagados");

                        var header = new List<string>
                        {
                            "BANCO",
                            "MONEDA",
                            "CTA",
                            "SCTA",
                            "SSCTA",
                            "CONTRATO",
                            "FOLIO",
                            "TIPO DE CAMBIO",
                            "PAGADO",
                            "CORTO PLAZO",
                            "LARGO PLAZO",
                            "TOTAL"
                        };

                        for (int i = 1; i <= header.Count; i++)
                        {
                            hoja.Cells[1, i].Value = header[i - 1];
                        }

                        var listaRenglonesTotales = new List<int>();

                        var cellData = new List<object[]>();

                        int renglon = 2;
                        foreach (var gbBanco in reporte.GroupBy(x => new { x.nombre, x.Moneda }))
                        {
                            foreach (var item in gbBanco)
                            {
                                cellData.Add(new object[] {
                                    item.nombre,
                                    item.Moneda,
                                    item.Cta,
                                    item.Scta,
                                    item.Sscta,
                                    item.Contrato,
                                    item.Folio,
                                    item.tipoCambio,
                                    item.Pagado,
                                    item.CP,
                                    item.LP,
                                    item.CP + item.LP
                                });

                                renglon++;
                            }

                            cellData.Add(new object[] {
                                gbBanco.First().nombre + " " + gbBanco.First().Moneda,
                                "",
                                "",
                                "",
                                "",
                                "",
                                "",
                                "",
                                gbBanco.Sum(x => x.Pagado),
                                gbBanco.Sum(x => x.CP),
                                gbBanco.Sum(x => x.LP),
                                gbBanco.Sum(x => x.CP + x.LP)
                            });

                            listaRenglonesTotales.Add(renglon);
                            renglon++;
                        }

                        cellData.Add(new object[] {
                            "TOTAL",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            reporte.Sum(x => x.Pagado),
                            reporte.Sum(x => x.CP),
                            reporte.Sum(x => x.LP),
                            reporte.Sum(x => x.CP + x.LP)
                        });

                        listaRenglonesTotales.Add(renglon);

                        hoja.Cells[2, 1].LoadFromArrays(cellData);

                        foreach (var item in listaRenglonesTotales)
                        {
                            var rangoTotal = hoja.Cells[item, 1, item, hoja.Dimension.End.Column];

                            rangoTotal.Style.Font.Bold = true;
                            rangoTotal.Style.Font.Size = 14;
                            rangoTotal.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            rangoTotal.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#808080"));

                            if (item == listaRenglonesTotales.Last())
                            {
                                rangoTotal.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ff9900"));
                            }
                        }

                        ExcelRange range = hoja.Cells[1, 1, hoja.Dimension.End.Row, hoja.Dimension.End.Column];

                        ExcelTable table = hoja.Tables.Add(range, "Tabla");

                        hoja.Cells[1, 8, hoja.Dimension.End.Row, 12].Style.Numberformat.Format = "$#,##0.00";

                        table.TableStyle = TableStyles.Medium17;

                        hoja.Cells[hoja.Dimension.Address].AutoFitColumns();

                        var bytes = new MemoryStream();
                        using (var stream = new MemoryStream())
                        {
                            excel.SaveAs(stream);
                            bytes = stream;
                        }

                        resultado.Add(SUCCESS, true);
                        resultado.Add(ITEMS, bytes);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(2, 0, "ContratosController", "CargarReporteInteresesPagados", ex, AccionEnum.CONSULTA, 0, fecha);

                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, ex.Message);
            }

            return resultado;
        }
    }
}