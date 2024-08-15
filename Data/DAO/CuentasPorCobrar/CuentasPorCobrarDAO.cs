using Core.DAO.Contabilidad.Reportes;
using Core.DAO.CuentasPorCobrar;
using Core.DAO.Maquinaria.Reporte;
using Core.DTO;
using Core.DTO.Contabilidad.Facturacion;
using Core.DTO.CuentasPorCobrar;
using Core.DTO.Maquinaria.Rentabilidad;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contabilidad.Facturas;
using Core.Entity.Administrativo.Contabilidad.FlujoEfectivo;
using Core.Entity.CuentasPorCobrar;
using Core.Entity.Principal.Alertas;
using Core.Enum.CuentasPorCobrar;
using Core.Enum.Principal;
using Core.Service.Contabilidad.Reportes;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Contabilidad.Reportes;
using Data.Factory.Maquinaria.Reporte;
using Infrastructure.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Data.DAO.CuentasPorCobrar
{
    public class CuentasPorCobrarDAO : GenericDAO<tblCXC_Convenios>, ICuentasPorCobrarDAO
    {
        Dictionary<string,object> resultado = new Dictionary<string,object>();

        IRentabilidadDAO rentabilidadFS = new RentabilidadFactoryServices().getRentabilidadDAO();
        IEncabezadoDAO encabezadoFD = new EncabezadoFactoryServices().getEncabezadoServices();
        IFlujoEfectivoDAO flujoEfectivoFS = new FlujoEfectivoFactoryServices().getFlujoEfectivoService();

        #region GESTION DE COBRANZA
        public Dictionary<string, object> GetConvenios(tblCXC_Convenios objFiltro)
        {
            resultado.Clear();

            try
            {
                //var lstConvenios = _context.tblCXC_Convenios.Where(e => e.esActivo).ToList();

                string consultaSQL = string.Format(@"
                     SELECT t1.*, t2.ccDescripcion, @usuarioActual as usuarioActual
                        FROM tblCXC_Convenios AS t1 
                        INNER JOIN tblC_Nom_CatalogoCC AS t2 ON t1.cc = t2.cc 
                        WHERE t1.esActivo = 1{0}"
                    , !string.IsNullOrEmpty(objFiltro.cc) ? " AND t2.cc = @cc " : "");

                var lstConvenios = _context.Select<CuentasPorCobrarDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = consultaSQL,
                    parametros = new { cc = objFiltro.cc, estatus = (int)objFiltro.estatus, usuarioActual = vSesiones.sesionUsuarioDTO.id},
                });

                foreach (var item in lstConvenios)
                {
                    var lstConvenioDet = _context.tblCXC_Convenios_Det.Where(e => e.esActivo && e.idAcuerdo == item.id).Select(e => new ConvenioDetDTO {
                        id = e.id,
                        idAcuerdo = e.idAcuerdo,
                        abonoDet = e.abonoDet,
                        fechaDet = e.fechaDet,
                        fechaCreacion = e.fechaCreacion,
                        esActivo = e.esActivo
                    }).ToList();

                    item.lstAbonos = lstConvenioDet;
                }

                resultado.Add(ITEMS, lstConvenios);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(ITEMS, null);
                resultado.Add(SUCCESS, true);
                //throw e;
            }

            return resultado;
        }

        public Dictionary<string, object> GetAcuerdoById(int idAcuerdo)
        {
            resultado.Clear();

            try
            {
                var objAcuerdo = _context.tblCXC_Convenios.FirstOrDefault(e => e.id == idAcuerdo);

                if (objAcuerdo == null)
                {
                    throw new Exception("Ocurrio algo mal con el acuerdo del la factura seleccionada");
                }

                var lstAcuerdoDet = _context.tblCXC_Convenios_Det.Where(e => e.esActivo && e.idAcuerdo == objAcuerdo.id).ToList();

                resultado.Add(ITEMS, objAcuerdo);
                resultado.Add("lstAcuerdoDet", lstAcuerdoDet);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e )
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> GetAcuerdoByFactura(string idFactura)
        {
            resultado.Clear();

            decimal pronosticoAMostrar = 0;

            try
            {
                var objAcuerdo = _context.tblCXC_Convenios.FirstOrDefault(e => e.idFactura == idFactura);

                if (objAcuerdo == null)
                {
                    throw new Exception("Ocurrio algo mal con el acuerdo del la factura seleccionada");
                }

                //var lstAcuerdoDet = _context.tblCXC_Convenios_Det.Where(e => e.esActivo && e.idAcuerdo == objAcuerdo.id).ToList();

                var objConvenioDet = _context.tblCXC_Convenios_Det.Where(e => e.esActivo && e.idAcuerdo == objAcuerdo.id).OrderBy(e => e.fechaDet).ToList().FirstOrDefault(e => e.fechaDet.Date >= DateTime.Now.Date);

                if (objConvenioDet != null)
                {
                    //if (objAcuerdo.esAutorizar)
                    //{
                    //    if (objAcuerdo.estatus == EstatusConvenioEnum.APROBADO)
                    //    {
                    //        pronosticoAMostrar = objConvenioDet.abonoDet;

                    //    }
                    //    else
                    //    {
                    //        pronosticoAMostrar = 0;

                    //    }

                    //}
                    //else
                    //{
                    //    pronosticoAMostrar = objConvenioDet.abonoDet;

                    //}

                    pronosticoAMostrar = objConvenioDet.abonoDet;
                }
                else
                {
                    pronosticoAMostrar = 0;

                }

                resultado.Add(ITEMS, objAcuerdo);
                resultado.Add("montoPronosticado", pronosticoAMostrar);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> RemoveAcuerdoDet(int idAcuerdoDet)
        {
            resultado.Clear();

            using (var dbTransac = _context.Database.BeginTransaction())
            {
                try
                {
                    var objAcuerdoDet = _context.tblCXC_Convenios_Det.FirstOrDefault(e => e.id == idAcuerdoDet);

                    if (objAcuerdoDet == null)
                    {
                        throw new Exception("Ocurrio algo mal con el abono a eliminar");
                    }

                    objAcuerdoDet.esActivo = false;
                    _context.SaveChanges();

                    resultado.Add(SUCCESS, true);

                    dbTransac.Commit();
                }
                catch (Exception e)
                {
                    dbTransac.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }

            }
            
            return resultado;
        }

        public Dictionary<string, object> CrearEditarConvenios(CuentasPorCobrarDTO objConvenio)
        {
            resultado.Clear();
            int nuevoConvenio = 0;
            decimal pronosticoAMostrar = 0;
            
            using (var dbTransac = _context.Database.BeginTransaction())
            {
                try
                {
                    if (objConvenio.id > 0)
                    {
                        #region EDITAR
                        var objCEConvenio = _context.tblCXC_Convenios.FirstOrDefault(e => e.esActivo && e.id == objConvenio.id);

                        if (objCEConvenio == null)
                        {
                            throw new Exception("Ocurrio algo mal");
                        }

                        objCEConvenio.esAutorizar = objConvenio.esAutorizar;

                        if (objCEConvenio.esAutorizar)
                        {
                            objCEConvenio.autoriza = objConvenio.autoriza;

                            #region ENVIAR CORREO AL AUTORIZADOR

//                            if (objCEConvenio.estatus == EstatusConvenioEnum.PENDIENTE)
//                            {
//                                var correos = new List<string>();

//                                var objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.id == objCEConvenio.autoriza);

//                                string asunto = "ACUERDO PENDIENTE DE AUTORIZAR EN CC " + objConvenio.ccDescripcion;
//                                string cuerpo = "Se le a asigando como autorizante en el acuerdo de cobranza en el cual hay un <b>abono con una fecha de vencimiento a un mes posterior</b><br><br>FACTURA: " + objConvenio.idFactura + " CLIENTE: " + objConvenio.nombreCliente +
//                                @"
//                                    <p class=MsoNormal>
//                                        <o:p>&nbsp;</o:p>
//                                    </p>
//                                    <p class=MsoNormal>
//                                        Favor de ingresar al sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx/</a>), en el apartado de ADMN/FINANZAS, menú CUENTAS POR COBRAR en la opción de Gestion de ACUERDOS<o:p></o:p>
//                                    </p>
//                                    <p class=MsoNormal>
//                                        <o:p>&nbsp;</o:p>
//                                    </p>
//                                    <p class=MsoNormal>
//                                        <o:p>&nbsp;</o:p>
//                                    </p>
//                                    <p class=MsoNormal>
//                                        PD. Se informa que esta es una notificación autogenerada por el sistema SIGOPLAN no es necesario dar una respuesta <o:p></o:p>
//                                    </p>
//                                    <p class=MsoNormal>
//                                        <o:p>&nbsp;</o:p>
//                                    </p>
//                                    <p class=MsoNormal>
//                                        Gracias.<o:p></o:p>
//                                    </p>";

//                                if (objUsuario != null)
//                                {
//                                    correos.Add(objUsuario.correo);
//                                }
//                                else
//                                {
//                                    correos.Add("miguel.buzani@construplan.com.mx");
//                                }

//#if DEBUG

//                                correos = new List<string>() { "miguel.buzani@construplan.com.mx" };

//#endif

//                                GlobalUtils.sendEmail(asunto, cuerpo, correos);   
                            //}
                            #endregion
                        }

                        objCEConvenio.comentarios = objConvenio.comentarios;
                        objCEConvenio.fechaModificacion = DateTime.Now;
                        objCEConvenio.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;

                        _context.SaveChanges();

                        if (objConvenio.lstAbonos != null && objConvenio.lstAbonos.Count() > 0)
                        {
                            foreach (var item in objConvenio.lstAbonos)
                            {
                                _context.tblCXC_Convenios_Det.Add(new tblCXC_Convenios_Det
                                {
                                    idAcuerdo = objCEConvenio.id,
                                    abonoDet = item.abonoDet,
                                    fechaDet = item.fechaDet,
                                    idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id,
                                    fechaCreacion = DateTime.Now,
                                    idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id,
                                    fechaModificacion = DateTime.Now,
                                    esActivo = true,
                                });
                                _context.SaveChanges();
                            }
                        }

                        var objConvenioDet = _context.tblCXC_Convenios_Det.Where(e => e.esActivo && e.idAcuerdo == objConvenio.id).OrderBy(e => e.fechaDet).ToList().FirstOrDefault(e => e.fechaDet.Date >= DateTime.Now.Date);

                        if (objConvenioDet != null)
                        {
                            //if (objCEConvenio.esAutorizar)
                            //{
                            //    if (objCEConvenio.estatus == EstatusConvenioEnum.APROBADO)
                            //    {
                            //        pronosticoAMostrar = objConvenioDet.abonoDet;

                            //    }
                            //    else
                            //    {
                            //        pronosticoAMostrar = 0;

                            //    }

                            //}
                            //else
                            //{
                            //    pronosticoAMostrar = objConvenioDet.abonoDet;

                            //}
                            pronosticoAMostrar = objConvenioDet.abonoDet;

                        }
                        else
                        {
                            pronosticoAMostrar = 0;

                        }

                        #endregion
                    }
                    else
                    {
                        #region CREAR
                        _context.tblCXC_Convenios.Add(new tblCXC_Convenios 
                        { 
                            numcte = objConvenio.numcte,
                            nombreCliente = objConvenio.nombreCliente,
                            cc = objConvenio.cc,
                            idFactura = objConvenio.idFactura,
                            monto = objConvenio.monto,
                            fechaOriginal = objConvenio.fechaOriginal,
                            comentarios = objConvenio.comentarios,
                            esAutorizar = objConvenio.esAutorizar,
                            esPagado = objConvenio.esPagado,
                            autoriza = objConvenio.autoriza,
                            estatus = EstatusConvenioEnum.PENDIENTE,
                            fechaCorte = objConvenio.fechaCorte,
                            idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id,
                            fechaCreacion = DateTime.Now,
                            idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id,
                            fechaModificacion = DateTime.Now,
                            esActivo = true,
                        });

                        _context.SaveChanges();

                        if (objConvenio.esAutorizar)
                        {

                            #region ENVIAR CORREO AL AUTORIZADOR

//                            var correos = new List<string>();

//                            var objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.id == objConvenio.autoriza);

//                            string asunto = "ACUERDO PENDIENTE DE AUTORIZAR EN CC " + objConvenio.ccDescripcion;
//                            string cuerpo = "Se le a asigando como autorizante en el acuerdo de cobranza en el cual hay un <b>abono con una fecha de vencimiento a un mes posterior</b><br><br>FACTURA: " + objConvenio.idFactura + " CLIENTE: " + objConvenio.nombreCliente +
//                                @"
//                                    <p class=MsoNormal>
//                                        <o:p>&nbsp;</o:p>
//                                    </p>
//                                    <p class=MsoNormal>
//                                        Favor de ingresar al sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx/</a>), en el apartado de ADMN/FINANZAS, menú CUENTAS POR COBRAR en la opción de Gestion de ACUERDOS<o:p></o:p>
//                                    </p>
//                                    <p class=MsoNormal>
//                                        <o:p>&nbsp;</o:p>
//                                    </p>
//                                    <p class=MsoNormal>
//                                        <o:p>&nbsp;</o:p>
//                                    </p>
//                                    <p class=MsoNormal>
//                                        PD. Se informa que esta es una notificación autogenerada por el sistema SIGOPLAN no es necesario dar una respuesta <o:p></o:p>
//                                    </p>
//                                    <p class=MsoNormal>
//                                        <o:p>&nbsp;</o:p>
//                                    </p>
//                                    <p class=MsoNormal>
//                                        Gracias.<o:p></o:p>
//                                    </p>";

//                            if (objUsuario != null)
//                            {
//                                correos.Add(objUsuario.correo);
//                            }
//                            else
//                            {
//                                correos.Add("miguel.buzani@construplan.com.mx");
//                            }

//#if DEBUG

//                            correos = new List<string>() { "miguel.buzani@construplan.com.mx" };

//#endif

//                            GlobalUtils.sendEmail(asunto, cuerpo, correos);
                            
                            #endregion
                        }

                        int idLastAcuerdo = _context.tblCXC_Convenios.Where(e => e.esActivo).OrderByDescending(e => e.fechaCreacion).FirstOrDefault().id;
                        nuevoConvenio = idLastAcuerdo;
                        
                        foreach (var item in objConvenio.lstAbonos)
                        {
                            _context.tblCXC_Convenios_Det.Add(new tblCXC_Convenios_Det
                            {
                                idAcuerdo = idLastAcuerdo,
                                abonoDet = item.abonoDet,
                                fechaDet = item.fechaDet,
                                idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id,
                                fechaCreacion = DateTime.Now,
                                idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id,
                                fechaModificacion = DateTime.Now,
                                esActivo = true,
                            });
                            _context.SaveChanges();
                        }

                        //var objConvenioDet = objConvenio.lstAbonos.FirstOrDefault(e => e.esActivo && e.fechaDet.Date >= objConvenio.fechaOriginal.Date && e.fechaDet.Date >= objConvenio.fechaCorte.Date);
                        var objConvenioDet = objConvenio.lstAbonos.OrderBy(e => e.fechaDet).FirstOrDefault(e => e.fechaDet.Date >= DateTime.Now.Date);

                        if (objConvenioDet != null)
                        {
                            //if (objConvenio.esAutorizar)
                            //{
                            //    if (objConvenio.estatus == EstatusConvenioEnum.APROBADO)
                            //    {
                            //        pronosticoAMostrar = objConvenioDet.abonoDet;

                            //    }
                            //    else
                            //    {
                            //        pronosticoAMostrar = 0;

                            //    }

                            //}
                            //else
                            //{
                            //    pronosticoAMostrar = objConvenioDet.abonoDet;

                            //}

                            pronosticoAMostrar = objConvenioDet.abonoDet;
                        }
                        else
                        {
                            pronosticoAMostrar = 0;

                        }
                        
                        #endregion
                    }

                    dbTransac.Commit();
                    resultado.Add(ITEMS, nuevoConvenio);
                    resultado.Add("montoProgramado", pronosticoAMostrar);
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbTransac.Rollback();
                    resultado.Add(SUCCESS, false);
                    throw e;
                }
            }

            return resultado;
        }
        
        public Dictionary<string, object> EliminarConvenio(int idConvenio)
        {
            resultado.Clear();

            using (var dbTransac = _context.Database.BeginTransaction())
            {
                try
                {
                    var objDelete = _context.tblCXC_Convenios.FirstOrDefault(e => e.esActivo && e.id == idConvenio);

                    if (objDelete != null)
                    {
                        objDelete.esActivo = false;
                        _context.SaveChanges();
                    }
                    dbTransac.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbTransac.Rollback();
                    resultado.Add(SUCCESS, false);
                }
            }
            
            return resultado;
        }

        public Dictionary<string, object> GetInfoFacturaById(string idFactura)
        {
            resultado.Clear();

            try
            {
                var ultimoRegistro = _contextEnkontrol.Select<CuentasPendientesDTO>(vSesiones.sesionAmbienteEnkontrolAdm, new OdbcConsultaDTO()
                {
                    consulta = string.Format(@"select 
                                                b.nombre as responsable, 
                                                x.factura as factura, 
                                                x.total as monto, 
                                                x.concepto as concepto, 
                                                x.fecha as fecha, 
                                                x.cc as areaCuenta, 
                                                (SELECT descripcion FROM cc tablaCC WHERE tablaCC.cc = x.cc) as areaCuentaDesc
                                            from
                                            (SELECT 
                                                a.numcte, a.factura, sum(a.total * a.tipocambio) as total, MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN a.concepto ELSE '' END) as concepto, 
                                                MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN a.cc ELSE '' END) as cc, MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN a.fechavenc ELSE '1900-01-01' END) as fecha,
                                                MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN CAST(a.referenciaoc AS numeric) ELSE -1 END) as numero, MAX(CASE WHEN (a.tp = '08') THEN a.year ELSE '' END) as year, 
                                                MAX(CASE WHEN (a.tp = '08') THEN a.mes ELSE '' END) as mes, MAX(CASE WHEN (a.tp = '08') THEN a.poliza ELSE '' END) as poliza
                                            FROM 
                                                (select * from sx_movcltes where factura = ?) a
                                            group by numcte, factura) x
                                            LEFT JOIN sx_clientes b ON x.numcte = b.numcte
                                            where x.total > 1"),
                    parametros = new List<OdbcParameterDTO>() {
                        new OdbcParameterDTO() { nombre = "factura", tipo = OdbcType.Numeric, valor = idFactura}
                    }
                }).FirstOrDefault();

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, ultimoRegistro);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GetFacturasByCliente(int idCliente)
        {
            resultado.Clear();

            try
            {
                var ultimoRegistro = _contextEnkontrol.Select<dynamic>(vSesiones.sesionAmbienteEnkontrolAdm, new OdbcConsultaDTO()
                {
                    consulta = string.Format(@"select 
                                                b.nombre as responsable, 
                                                x.numcte as numcte, 
                                                x.factura as factura, 
                                                x.total as monto, 
                                                x.concepto as concepto, 
                                                x.fecha as fecha, 
                                                x.cc as areaCuenta, 
                                                (SELECT descripcion FROM cc tablaCC WHERE tablaCC.cc = x.cc) as areaCuentaDesc
                                            from
                                            (SELECT 
                                                a.numcte, a.factura, sum(a.total * a.tipocambio) as total, MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN a.concepto ELSE '' END) as concepto, 
                                                MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN a.cc ELSE '' END) as cc, MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN a.fechavenc ELSE '1900-01-01' END) as fecha,
                                                MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN CAST(a.referenciaoc AS numeric) ELSE -1 END) as numero, MAX(CASE WHEN (a.tp = '08') THEN a.year ELSE '' END) as year, 
                                                MAX(CASE WHEN (a.tp = '08') THEN a.mes ELSE '' END) as mes, MAX(CASE WHEN (a.tp = '08') THEN a.poliza ELSE '' END) as poliza
                                            FROM 
                                                (select * from sx_movcltes where numcte = ?) a
                                            group by numcte, factura) x
                                            LEFT JOIN sx_clientes b ON x.numcte = b.numcte
                                            where x.total > 1

                                "),
                    parametros = new List<OdbcParameterDTO>() {
                        new OdbcParameterDTO() { nombre = "numcte", tipo = OdbcType.Numeric, valor = idCliente}
                    }
                }).Select(e => new ComboDTO 
                {
                    Value = e.factura.ToString(),
                    Text = e.factura.ToString(),
                });

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, ultimoRegistro);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string,object> GetAutorizantesCC(string cc)
        {
            resultado.Clear();

            try
            {
                var lstAutorizantes = _context.tblCXC_AutorizantesCC.Where(e => e.esActivo && (e.cc == cc || e.cc == "*") ).Select(e => e.idUsuario).ToList();

                var lstUsuarios = _context.tblP_Usuario.Where(e => lstAutorizantes.Contains(e.id)).Select(e => new ComboDTO 
                { 
                    Value = e.id.ToString(),
                    Text = (e.nombre ?? "") + (e.apellidoPaterno ?? "") + (e.apellidoMaterno ?? ""),
                }).ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstUsuarios);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> FillComboPeriodos()
        {
            resultado.Clear();

            try
            {
                var lstPeriodos = _context.tblRH_BN_EstatusPeriodos.Where(e => e.anio == DateTime.Now.Year).ToList();
                List<ComboDTO> lstComboPeriodos = new List<ComboDTO>();
                
                foreach (var e in lstPeriodos)
	            {
                    lstComboPeriodos.Add(new ComboDTO
                    {
                        Value = e.id.ToString(),
                        Text = (e.tipo_nomina == 1 ? "[S] " : "[Q] ")+ e.periodo + " : " + e.fecha_inicial.ToString("dd/MM/yyyy") + " - " + e.fecha_final.ToString("dd/MM/yyyy"),
                    });
	            }

                resultado.Add(ITEMS, lstComboPeriodos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> FillComboCC()
        {
            resultado.Clear();

            try
            {
                var lstCCs = _context.tblC_Nom_CatalogoCC.Select(e => new ComboDTO
                {
                    Value = e.cc,
                    Text = "[" + e.cc + "] " + (e.ccDescripcion.Trim())
                }).ToList();

                resultado.Add(ITEMS, lstCCs);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                
            }

            return resultado;
        }

        public Dictionary<string, object> ActualizarEstatusConvenio(int idConvenio, EstatusConvenioEnum estatus)
        {
            resultado.Clear();

            using (var dbTransac = _context.Database.BeginTransaction())    
            {
                try
                {
                    var objCovenio = _context.tblCXC_Convenios.FirstOrDefault(e => e.esActivo && e.id == idConvenio);

                    if (objCovenio == null)
                    {
                        throw new Exception("Ocurrio algo mal con el convenio");
                    }

                    objCovenio.estatus = estatus;
                    _context.SaveChanges();

                    dbTransac.Commit();
                    resultado.Add(SUCCESS, true);
                
                }
                catch (Exception e)
                {
                    dbTransac.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> CrearEditarCorte(DateTime fechaCorte, List<string> lstFacturas)
        {
            resultado.Clear();

            using (var dbTransac = _context.Database.BeginTransaction())
            {
                try
                {
                    var lstCorteFacturas = _context.tblCXC_Corte_Det.Where(e => e.esActivo).ToList();
                    var lstIdsCorteFacturas = lstCorteFacturas.Select(e => e.idFactura).ToList();

                    foreach (var item in lstFacturas)
                    {
                        if (!lstIdsCorteFacturas.Contains(item))
                        {
                            _context.tblCXC_Corte_Det.Add(new tblCXC_Corte_Det
                            {
                                idFactura = item,
                                fechaCorte = fechaCorte,
                                fechaCreacion = DateTime.Now,
                                fechaModificacion = DateTime.Now,
                                idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id,
                                idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id,
                                esActivo = true,
                                esRemoved = false,
                            });
                            _context.SaveChanges();
                        }
                        else
                        {
                            var objActualizar = _context.tblCXC_Corte_Det.FirstOrDefault(e => e.idFactura == item);

                            if (objActualizar != null )
                            {
                                if (objActualizar.esRemoved)
                                {
                                    objActualizar.esRemoved = false;
                                    
                                }
                                objActualizar.fechaCorte = fechaCorte;
                                objActualizar.fechaModificacion = DateTime.Now;
                                objActualizar.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                            }

                            _context.SaveChanges();
                        }
                    }
                    
                    dbTransac.Commit();

                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbTransac.Rollback();

                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> RemoveFactura(string idFactura, string comentarioRemove)
        {
            resultado.Clear();

            using (var dbTransac = _context.Database.BeginTransaction())
            {
                try
                {
                    var objFacturaCorte = _context.tblCXC_Corte_Det.FirstOrDefault(e => e.idFactura == idFactura);

                    if (objFacturaCorte == null)
                    {
                        throw new Exception("Ocurrio algo mal con la factura seleccionada");
                    }

                    //objFacturaCorte.esActivo = false;
                    objFacturaCorte.esRemoved = true;
                    objFacturaCorte.comentariosRemove = comentarioRemove;

                    _context.SaveChanges();
                    dbTransac.Commit();

                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbTransac.Rollback();

                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> CrearEditarComentarios(cxcComentariosDTO objFiltro)
        {
            resultado.Clear();

            using (var dbTransac = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.tblCXC_Comentarios.Add(new tblCXC_Comentarios {
                        comentario = objFiltro.comentario,
                        factura = objFiltro.factura,
                        clienteID = objFiltro.clienteID,
                        nomCliente = objFiltro.nomCliente,
                        cc = objFiltro.cc,
                        ccDesc = objFiltro.ccDesc,
                        tipoComentario = objFiltro.tipoComentario,
                        fechaCompromiso = objFiltro.fechaCompromiso,
                        nombreUsuarioCreacion  = vSesiones.sesionUsuarioDTO.apellidoPaterno + " " + vSesiones.sesionUsuarioDTO.apellidoMaterno + " " +  vSesiones.sesionUsuarioDTO.nombre,
                        fechaCreacion = DateTime.Now,
                        fechaModificacion = DateTime.Now,
                        idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id,
                        idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id,
                        esActivo = true,
                    });

                    _context.SaveChanges();

//                    var objLastComentario = _context.tblCXC_Comentarios.Select(e => e).OrderByDescending(e => e.id).FirstOrDefault();
//                    var lstTiposNoti = _context.tblCXC_Comentarios_Tipos.Where(e => e.esActivo).ToList();
//                    var lstIdsTiposNoti = lstTiposNoti.Select(e => e.id);

//                    var objTipoNoti = lstTiposNoti.FirstOrDefault(e => e.id == objFiltro.tipoComentario);

//                    if (objTipoNoti != null && objTipoNoti.esNoti)
//                    {
//                        string asunto = "";

//                        if (string.IsNullOrEmpty(objFiltro.cc))
//                        {
//                            asunto = "cliente: " + objFiltro.nomCliente;
//                        }
//                        else
//                        {
//                            asunto = "cc: " + objFiltro.ccDesc;

//                        }

//                        tblP_Alerta objNuevaAlerta = new tblP_Alerta();
//                        objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
//                        objNuevaAlerta.userRecibeID = (int)vSesiones.sesionUsuarioDTO.id;
//#if DEBUG
//                        //objNuevaAlerta.userRecibeID = 7939; //USUARIO ID: Omar Nuñez.
//                        objNuevaAlerta.userRecibeID = 13; //USUARIO ID: Martín Zayas.
//#endif
//                        objNuevaAlerta.tipoAlerta = 2;
//                        objNuevaAlerta.sistemaID = 11;
//                        objNuevaAlerta.visto = false;
//                        objNuevaAlerta.url = "/CuentasPorCobrar/CuentasPorCobrar/GestionCobranza";
//                        objNuevaAlerta.objID = objLastComentario.id;
//                        objNuevaAlerta.obj = "ComentarioCXC";
//                        objNuevaAlerta.msj = "CXC tipo: " + objTipoNoti.conceptoCorto + " " + asunto.Substring(0,20);
//                        objNuevaAlerta.documentoID = 0;
//                        objNuevaAlerta.moduloID = 0;
//                        _context.tblP_Alerta.Add(objNuevaAlerta);
//                        _context.SaveChanges();
//                    }

                    dbTransac.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbTransac.Rollback();
                    resultado.Add(SUCCESS, false);
                } 
            }

            return resultado;
        }

        public Dictionary<string, object> GetComentarios(cxcComentariosDTO objFiltro)
        {
            resultado.Clear();

            try
            {
                var lstComentarios = _context.tblCXC_Comentarios.Where(e => e.esActivo 
                    && e.factura == objFiltro.factura
                    ).ToList();

                resultado.Add(ITEMS, lstComentarios);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {

                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string,object> GetComentariosVencer()
        {
            resultado.Clear();

            try
            {
                DateTime oneWeek = DateTime.Now.Add(new TimeSpan(7, 0, 0, 0));

                var lstTiposComentariosNoti = _context.tblCXC_Comentarios_Tipos.Where(e => e.esActivo && e.esNoti).ToList();
                var lstIdsTiposComentariosNoti = lstTiposComentariosNoti.Select(e => e.id).ToList();
                var lstComentarios = _context.tblCXC_Comentarios.Where(e => e.esActivo).ToList().Where(e => lstIdsTiposComentariosNoti.Contains(e.tipoComentario) && e.fechaCompromiso.Value.Date >= DateTime.Now.Date.AddDays(-7) && e.fechaCompromiso.Value.Date <= oneWeek.Date).ToList();

                List<cxcComentariosDTO> lstComentariosVencerCliente = new List<cxcComentariosDTO>();
                List<cxcComentariosDTO> lstComentariosVencerCC = new List<cxcComentariosDTO>();

                foreach (var item in lstComentarios)
                {
                    bool venceMañana = false;
                    bool venceMañanaPasado = false;

                    if (item.fechaCompromiso.Value.Date == DateTime.Now.Date.AddDays(1))
                    {
                        venceMañana = true;
                    }

                    if (item.fechaCompromiso.Value.Date == DateTime.Now.Date.AddDays(2))
                    {
                        venceMañanaPasado = true;
                    }

                    var objTipoComentario = lstTiposComentariosNoti.FirstOrDefault(e => e.id == item.tipoComentario);

                    lstComentariosVencerCC.Add(new cxcComentariosDTO
                    {
                        id = item.id,
                        comentario = item.comentario,
                        factura = item.factura ?? 0,
                        cc = item.cc,
                        ccDesc = item.ccDesc,
                        tipoComentario = item.tipoComentario,
                        descTipoComentario = objTipoComentario.conceptoCorto,
                        fechaCompromiso = item.fechaCompromiso,
                        nombreUsuarioCreacion = item.nombreUsuarioCreacion,
                        esVenceMañana = venceMañana,
                        esVencePasado = venceMañanaPasado,
                    });

                    //if (!string.IsNullOrEmpty(item.cc))
                    //{
                    //    lstComentariosVencerCC.Add(new cxcComentariosDTO
                    //    {
                    //        id = item.id,
                    //        comentario = item.comentario,
                    //        factura = item.factura,
                    //        cc = item.cc,
                    //        ccDesc = item.ccDesc,
                    //        tipoComentario = item.tipoComentario,
                    //        descTipoComentario = objTipoComentario.conceptoCorto,
                    //        fechaCompromiso = item.fechaCompromiso,
                    //        nombreUsuarioCreacion = item.nombreUsuarioCreacion,
                    //        esVenceMañana = venceMañana,
                    //        esVencePasado = venceMañanaPasado,
                    //    });
                    //}
                    //else
                    //{
                    //    lstComentariosVencerCliente.Add(new cxcComentariosDTO
                    //    {
                    //        id = item.id,
                    //        factura = item.factura,
                    //        comentario = item.comentario,
                    //        clienteID = item.clienteID,
                    //        nomCliente = item.nomCliente,
                    //        tipoComentario = item.tipoComentario,
                    //        descTipoComentario = objTipoComentario.conceptoCorto,
                    //        fechaCompromiso = item.fechaCompromiso,
                    //        nombreUsuarioCreacion = item.nombreUsuarioCreacion,
                    //        esVenceMañana = venceMañana,
                    //        esVencePasado = venceMañanaPasado,
                    //    });
                    //}
                }

                //resultado.Add(ITEMS, lstComentarios);
                //resultado.Add("comentariosCliente", lstComentariosVencerCliente);
                resultado.Add("comentariosCC", lstComentariosVencerCC);
                resultado.Add(SUCCESS, true);
                
            }
            catch (Exception e)
            {

                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> FillComboTiposComentarios()
        {
            resultado.Clear();

            try
            {
                var lstTipos = _context.tblCXC_Comentarios_Tipos.Where(e => e.esActivo).ToList().Select(e => new ComboDTO 
                {
                    Text = e.conceptoCorto,
                    Value = e.id.ToString()
                });

                resultado.Add(ITEMS, lstTipos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {

                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> GetKardex(List<string> lstFiltroCC)
        {
            resultado.Clear();

            try
            {
                var consultaEK = new OdbcConsultaDTO();

                if (lstFiltroCC != null && lstFiltroCC.Count > 0)
                {
                    consultaEK.consulta =
                        string.Format(@"SELECT t1.*, t2.nombre as nombreCliente, t3.descripcion as ccDesc
                            FROM (SELECT  numcte, factura, cc,  sum(total) as total FROM sx_movcltes GROUP BY numcte, factura, cc ) as t1 
                            INNER JOIN sx_clientes as t2 ON t1.numcte = t2.numcte 
                            INNER JOIN cc as t3 ON t1.cc = t3.cc
                            WHERE total > 0 AND t1.cc IN ('{0}')
                            ", string.Join("', '", lstFiltroCC));
                    
                }else{
                    consultaEK.consulta =
                        @"SELECT t1.*, t2.nombre as nombreCliente, t3.descripcion as ccDesc
                            FROM (SELECT  numcte, factura, cc,  sum(total) as total FROM sx_movcltes GROUP BY numcte, factura, cc ) as t1 
                            INNER JOIN sx_clientes as t2 ON t1.numcte = t2.numcte 
                            INNER JOIN cc as t3 ON t1.cc = t3.cc
                            WHERE total > 0
                            ";
                }


                var lstFacturasConSaldo = _contextEnkontrol.Select<cxcKardexDTO>(vSesiones.sesionAmbienteEnkontrolAdm, consultaEK);

                resultado.Add(ITEMS, lstFacturasConSaldo);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> GetKardexDet(string cc)
        {
            resultado.Clear();

            try
            {
                var consultaEK = new OdbcConsultaDTO();

//                consultaEK.consulta =
//                            @"SELECT t1.*, t2.nombre as nombreCliente, t3.descripcion as ccDesc
//                FROM sx_movcltes as t1 
//                INNER JOIN sx_clientes as t2 ON t1.numcte = t2.numcte 
//                INNER JOIN cc as t3 ON t1.cc = t3.cc
//                WHERE t1.cc = ?
//                ORDER BY t1.factura, t1.fecha
//                ";

                consultaEK.consulta = @"
                    SELECT t4.* , t2.nombre as nombreCliente, t3.descripcion as ccDesc
                    FROM sx_movcltes as t4
                    INNER JOIN (
                        SELECT t1.*
                        FROM 
                            (SELECT  numcte as clienteSuma, factura as facturaSuma, cc as ccSuma, sum(total) as total FROM sx_movcltes GROUP BY numcte, factura, cc ) as t1
                        WHERE t1.total > 10
                        ) tSaldos ON t4.numcte = tSaldos.clienteSuma AND t4.factura = tSaldos.facturaSuma AND t4.cc = tSaldos.ccSuma
                    INNER JOIN sx_clientes as t2 ON t4.numcte = t2.numcte 
                    INNER JOIN cc as t3 ON t4.cc = t3.cc
                    WHERE t4.cc = ?
                    ORDER BY t4.factura, t4.fecha";

                consultaEK.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "cc",
                    tipo = OdbcType.Char,
                    valor = cc
                });

                var lstFacturasConSaldo = _contextEnkontrol.Select<cxcMovClientesDTO>(vSesiones.sesionAmbienteEnkontrolAdm, consultaEK);

                resultado.Add(ITEMS, lstFacturasConSaldo);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> VerificarCXC(DateTime fechaInicial, DateTime fechaFinal)
        {
            resultado.Clear();

            try
            {
                var lstFacturas = _context.tblCXC_CuentasPorCobrar.Where(e => e.esActivo).ToList().Where(e => e.fechaInicial.Date == fechaInicial.Date).ToList();

                if (lstFacturas != null && lstFacturas.Count() > 0)
                {

                    resultado.Add(ITEMS, lstFacturas);
                    resultado.Add("esAuth", true);
                }
                else
                {
                    resultado.Add("esAuth", false);

                }

                resultado.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarCXC(List<EstClieFacturaDTO> lstFacturas, DateTime fechaInicial, DateTime fechaFinal)
        {
            resultado.Clear();

            using (var dbTransac = _context.Database.BeginTransaction())
            {
                try
                {
                    //OBJ PERIODO PARA FLUJO
                    var objPeriodo = _context.tblRH_EK_Periodos.Where(e => e.tipo_nomina == 1 && e.year == fechaInicial.Year).ToList().FirstOrDefault(e =>
                        fechaInicial.Date >= e.fecha_inicial.Date && fechaInicial.Date < e.fecha_final);

                    //DTO PARA FLUJO EFECTIVO 
                    List<tblC_FED_PlaneacionDet> lstPlaneacionDet = new List<tblC_FED_PlaneacionDet>();

                    foreach (var item in lstFacturas)
                    {
                        int idCliente = Convert.ToInt32(item.numcte);
                        //int idFactura = Convert.ToInt32(item.factura);

                        _context.tblCXC_CuentasPorCobrar.Add(new tblCXC_CuentasPorCobrar {
                            fechaInicial = fechaInicial,
                            fechaFinal = fechaFinal,
                            cc = item.cc,
                            numcte = idCliente,
                            nombreCliente = item.nombreCliente,
                            fechaVencOrig = item.fecha,
                            fechaVenc = item.fechavenc,
                            factura = item.factura,
                            total = item.vencido,
                            pronosticado = item.pronostico,
                            aplicaPropuesta = false,
                            fechaCreacion = DateTime.Now,
                            fechaModificacion = DateTime.Now,
                            idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id,
                            idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id,
                            esActivo = true,
                        });

                        _context.SaveChanges();

                        lstPlaneacionDet.Add(new tblC_FED_PlaneacionDet() 
                        {
                            concepto = 17,
                            descripcion = "COBRANZA " +item.nombreCliente,
                            monto = item.vencido,
                            ac = "0-0",
                            cc = item.cc,
                            semana = objPeriodo.periodo,
                            año = fechaInicial.Year,
                            estatus = true,
                            fechaCaptura = DateTime.Now,
                            usuarioCaptura = vSesiones.sesionUsuarioDTO.id,
                            sp_gastos_provID = 0,
                            factura = item.factura,
                            nominaID = 0,
                            cadenaProductivaID = 0,
                            numcte = idCliente,
                            numprov = 0,
                            fechaFactura = item.fechaFlujo.ToString("dd/MM/yyyy"), //SET A MIERCOLES EN FRONT
                            idDetProyGemelo = 0,
                            categoriaTipo = 0,
                        });
                    }

                    #region GUARDAR FLUJO EFECTVIO


                    var lstPlaneacionGrpCC = lstPlaneacionDet.GroupBy(e => e.cc).Select(e => new {e.Key, lstData = e}).ToList();

                    foreach (var item in lstPlaneacionGrpCC)
	                {
                        flujoEfectivoFS.saveDetallesMasivos(item.lstData.ToList(), 17, fechaInicial.Year, objPeriodo.periodo, item.Key);
	                }

                    #endregion

                    string asunto = "CXC PRONOSTICO COBRANZA " + fechaInicial.ToString("dd/MM/yyyy") + " - " + fechaFinal.ToString("dd/MM/yyyy");
                    string cuerpo = @"
                        <html>
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
                                    </p>
                                    <p class=MsoNormal>
                                        <o:p>Se autorizado el siguiente Pronostico de cobranza de " + fechaInicial.ToString("dd/MM/yyyy") + " al " + fechaFinal.ToString("dd/MM/yyyy") + @"</o:p>
                                    </p>
                                    <br>
                                    <br>
                                    <p class=MsoNormal>
                                        <o:p>&nbsp;</o:p>
                                    </p>
                                    <p class=MsoNormal>
                                        
                                    </p>
                                    <p class=MsoNormal>
                                        <o:p>&nbsp;</o:p>
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
                            </body>
                        </html>";
                    var correos = new List<string>();
                    //Favor de ingresar al sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx/</a>), en el apartado de ADMN/FINANZAS, menú CUENTAS POR COBRAR en la opción de Gestion de ACUERDOS<o:p></o:p>

                    var lstNotificantes = _context.tblCXC_Permisos.Where(e => e.esActivo).ToList();

                    foreach (var item in lstNotificantes)
                    {
                        var objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.id == item.idUsuario);

                        if (objUsuario != null)
                        {
                            correos.Add(objUsuario.correo);
                        }
                    }

#if DEBUG
                    correos = new List<string>() { "miguel.buzani@construplan.com.mx"};
#endif

                    var lstAcuerdosAplazados = _context.tblCXC_Convenios.Where(e => e.esActivo && e.esAutorizar).ToList();
                    //var lstAcuerdosNotificar = new List<tblCXC_Convenios>();
                    var lstIdsFacturas = lstFacturas.Select(e => e.factura).ToList();
                    
                    bool esEnviar = false;
                    DateTime oneMonth = fechaInicial.AddMonths(1);
                    var lstCCs = _context.tblP_CC.ToList();

                    byte[] archive = exportEstimacionesResumenReporte();

                    var lstArchives = new List<adjuntoCorreoDTO>();

                    lstArchives.Add(new adjuntoCorreoDTO
                    {
                        archivo = archive,
                        nombreArchivo = "PronosticoCobranza",
                        extArchivo = ".xlsx"
                    });

                    GlobalUtils.sendMailWithFilesReclutamientos(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), cuerpo, correos, lstArchives);
                    //if (esEnviar)
                    //{
                    //    //GlobalUtils.sendEmail(asunto, cuerpo, correos);
                        
                    //}

                    resultado.Add(SUCCESS, true);

                    dbTransac.Commit();
                }
                catch (Exception e)
                {

                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);

                    dbTransac.Rollback();
                }
            }

            return resultado;
        }

        public byte[] exportEstimacionesResumenReporte()
        {
            try
            {
                #region Tab Resumen
                using (var package = new ExcelPackage())
                {
                    //HttpContext.Current.Session["EstfechaFinal"] as DateTime;
                    var lstEstRes = (HttpContext.Current.Session["lstEstimacionResumen"] as List<EstClieFacturaDTO>).ToArray();
                    var fecha = HttpContext.Current.Session["EstfechaFinal"] as DateTime?;
                    var i = 7;
                    var titulo = "PRONOSTICO DE COBRANZA AL " + fecha.Value.ToString("dd MMMM yyyy").ToUpper();
                    var estimacionesResumen = package.Workbook.Worksheets.Add(ExcelUtilities.NombreValidoArchivo(titulo));

                    estimacionesResumen.Cells["B2:H2"].Merge = true;
                    estimacionesResumen.Cells["B3:H3"].Merge = true;
                    estimacionesResumen.Cells["B4:H4"].Merge = true;
                    estimacionesResumen.Cells["B5:H5"].Merge = true;

                    estimacionesResumen.Cells["B2"].Value = encabezadoFD.getEncabezadoDatos().nombreEmpresa.ToUpper();
                    estimacionesResumen.Cells["B3"].Value = string.Format("PERIFÉRICO PONIENTE 770 COL. PALO VERDE C.P. 83280");
                    estimacionesResumen.Cells["B4"].Value = string.Format("HERMOSILLO, SON");
                    estimacionesResumen.Cells["B5"].Value = titulo;

                    estimacionesResumen.Cells["A6"].Value = string.Empty;
                    estimacionesResumen.Cells["B6"].Value = "CUENTAS POR COBRAR";
                    estimacionesResumen.Cells["E6"].Value = "TOTAL";
                    //estimacionesResumen.Cells["F6"].Value = string.Empty;
                    estimacionesResumen.Cells["G6"].Value = "PRONÓSTICOS COBRANZA";

                    estimacionesResumen.Cells["A7"].LoadFromCollection(lstEstRes.Select(s => s.no > 0 ? s.no.ToString() : string.Empty));
                    estimacionesResumen.Cells["B7"].LoadFromCollection(lstEstRes.Select(s => s.descripcion ?? string.Empty));
                    //lstEstRes.ToList().ForEach(s => estimacionesResumen.Cells[string.Format("C{0}", i++)].Value = s.estimacion);
                    //i = 7;
                    //lstEstRes.ToList().ForEach(s => estimacionesResumen.Cells[string.Format("D{0}", i++)].Value = s.anticipo);
                    //i = 7;
                    lstEstRes.ToList().ForEach(s => estimacionesResumen.Cells[string.Format("E{0}", i++)].Value = s.vencido);
                    i = 7;
                    //estimacionesResumen.Cells["F7"].LoadFromCollection(lstEstRes.Select(s => string.Empty));
                    lstEstRes.ToList().ForEach(s => estimacionesResumen.Cells[string.Format("G{0}", i++)].Value = s.pronostico);
                    i = 7;
                    //lstEstRes.ToList().ForEach(s => estimacionesResumen.Cells[string.Format("H{0}", i++)].Value = s.cobrado);
                    //i = 7;

                    using (var rng = estimacionesResumen.Cells["A1:H6"])
                    {
                        rng.Style.Font.Bold = true;
                        rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    using (var rng = estimacionesResumen.Cells["A6:H6"])
                    {
                        rng.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }

                    var index = 7;

                    Color gray2 = Color.FromArgb(230, 230, 230);

                    estimacionesResumen.Column(5).Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                    estimacionesResumen.Column(7).Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";

                    lstEstRes.ToList().ForEach(tipo =>
                    {
                        using (var rng = estimacionesResumen.Cells[string.Format("B{0}:H{0}", index)])
                        {
                            switch (tipo.clase)
                            {
                                case "suma":
                                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    rng.Style.Font.Bold = true;
                                    rng.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                                    rng.Style.Font.Color.SetColor(Color.Black);
                                    break;
                                case "encabezado":
                                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    rng.Style.Border.BorderAround(ExcelBorderStyle.Medium);
                                    rng.Style.Font.Bold = true;
                                    rng.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                                    rng.Style.Font.Color.SetColor(Color.Black);
                                    using (var abc = estimacionesResumen.Cells[string.Format("B{0}", index)])
                                    {
                                        abc.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    }
                                    //using(var abc = estimacionesResumen.Cells[string.Format("C{0}:H{0}", index)])
                                    //{
                                    //    abc.Value = string.Empty;
                                    //}
                                    break;
                                case "subtotal":
                                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    rng.Style.Font.Bold = true;
                                    rng.Style.Border.BorderAround(ExcelBorderStyle.Medium);
                                    rng.Style.Font.Color.SetColor(Color.Black);
                                    rng.Style.Fill.BackgroundColor.SetColor(Color.White);
                                    break;
                                case "normalCC":
                                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    rng.Style.Font.Bold = true;
                                    rng.Style.Border.BorderAround(ExcelBorderStyle.Medium);
                                    rng.Style.Font.Color.SetColor(Color.Black);
                                    rng.Style.Fill.BackgroundColor.SetColor(gray2);
                                    break;
                                case "normalCliente":
                                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    rng.Style.Font.Bold = true;
                                    rng.Style.Border.BorderAround(ExcelBorderStyle.Medium);
                                    rng.Style.Font.Color.SetColor(Color.Black);
                                    rng.Style.Fill.BackgroundColor.SetColor(Color.White);
                                    break;
                            }
                        }
                        index++;
                    });
                    using (var rng = estimacionesResumen.Cells["A6:H" + (index - 1)])
                    {
                        rng.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }
                    var index2 = 7;
                    lstEstRes.ToList().ForEach(tipo =>
                    {
                        using (var rng = estimacionesResumen.Cells[string.Format("C{0}:H{0}", index2)])
                        {
                            rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }
                        index2++;
                    });

                    estimacionesResumen.Cells[estimacionesResumen.Dimension.Address].AutoFitColumns();
                    package.Compression = CompressionLevel.BestSpeed;
                    List<byte[]> lista = new List<byte[]>();
                    //using (var exportData = new MemoryStream())
                    //{
                    //    this.Response.Clear();
                    //    package.SaveAs(exportData);
                    //    lista.Add(exportData.ToArray());
                    //    this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //    this.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", package.Workbook.Worksheets.FirstOrDefault().Name + ".xlsx"));
                    //    this.Response.BinaryWrite(exportData.ToArray());
                    //    this.Response.End();
                    //    return exportData;
                    //}
                    return package.GetAsByteArray();
                }
                #endregion
            }
            catch (Exception e)
            {
                
                throw e;
            }
            
        }

        public Dictionary<string, object> CancelarCXC(DateTime fechaInicial)
        {
            resultado.Clear();

            using (var dbTransac = _context.Database.BeginTransaction())
            {
                try
                {
                    var lstFacturas = _context.tblCXC_CuentasPorCobrar.Where(e => e.esActivo).ToList().Where(e => e.fechaInicial.Date == fechaInicial.Date).ToList();

                    foreach (var item in lstFacturas)
                    {
                        item.esActivo = false;
                        _context.SaveChanges();
                    }

                    resultado.Add(SUCCESS, true);
                    dbTransac.Commit();
                }
                catch (Exception e)
                {

                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    dbTransac.Rollback();
                }
            }

            return resultado;
        }

        public List<string> GetDivisionDetByDivision(int divisionID)
        {
            List<string> data = new List<string>();
            var detalles = _context.tblCXC_DivisionDetalle.Where(x => x.divisionID == divisionID && x.estatus && x.ac.areaCuenta != null).ToList();
            if (detalles.Count() > 0) data = detalles.Select(x => x.ac.areaCuenta).ToList();
            return data;
        }

        public Dictionary<string, object> GuardarFacturaMod(string factura, DateTime fechaVencimientoOG, DateTime fechaNueva)
        {
            resultado.Clear();

            using (var dbTransac = _context.Database.BeginTransaction())
            {
                
                try
                {

                    var objFacturaMod = _context.tblCXC_FacturasMod.FirstOrDefault(e => e.factura == factura);

                    if (objFacturaMod != null)
                    {
                        objFacturaMod.fechaModificacion = DateTime.Now;
                        objFacturaMod.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                        objFacturaMod.fechaNueva = fechaNueva;

                    }
                    else {

                        _context.tblCXC_FacturasMod.Add(new tblCXC_FacturasMod()
                        {
                            factura = factura,
                            fechaVencimientoOG = fechaVencimientoOG,
                            fechaNueva = fechaNueva,
                            fechaCreacion = DateTime.Now,
                            fechaModificacion = DateTime.Now,
                            idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id,
                            idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id,
                            esActivo = true,
                        });
                    }
                    

                    _context.SaveChanges();
                    dbTransac.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbTransac.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarFacturaMod(string factura)
        {
            resultado.Clear();

            using (var dbTransac = _context.Database.BeginTransaction())
            {
                try
                {
                    var objFacturaMod = _context.tblCXC_FacturasMod.FirstOrDefault(e => e.factura == factura);


                    if (objFacturaMod == null)
                    {
                        throw new Exception("Ocurrio algo mal con la factura seleccionada");
                    }

                    objFacturaMod.esActivo = false;
                    objFacturaMod.fechaModificacion = DateTime.Now;
                    objFacturaMod.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;

                    _context.SaveChanges();
                    dbTransac.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbTransac.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);

                }
            }

            return resultado;
        }
        #endregion

        #region FILLCOMBOS
        public Dictionary<string, object> fillComboDivision()
        {
            resultado.Clear();

            try
            {
                var lstDivisiones = _context.tblCXC_Division.Where(e => e.estatus).ToList().OrderBy(e => e.division).Select(e => new ComboDTO { Text = e.division, Value = e.id.ToString()});
                resultado.Add(ITEMS, lstDivisiones);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> FillComboCCGestion(int idDivision)
        {
            resultado.Clear();

            try
            {
                var lstCC = new List<ComboDTO>();

                if (idDivision > 0)
                {
                    var lstCCs = _context.tblP_CC.Where(e => e.estatus).ToList();
                    var lstRelCCs = _context.tblC_CCDivision.Where(e => e.division == idDivision).Select(e => e.cc).ToList();

                    lstCCs = lstCCs.Where(e => lstRelCCs.Contains(e.cc)).ToList();

                    foreach (var item in lstCCs)
                    {
                        lstCC.Add(new ComboDTO()
                        {
                            Text = item.cc + " " + item.descripcion,
                            Value = item.cc
                        });
                    }
                }
                else
                {
                    lstCC = _context.tblP_CC.Where(e => e.estatus).Select(e => new ComboDTO()
                    {
                        Text = e.cc + " " + e.descripcion,
                        Value = e.cc
                    }).ToList();
                }

                resultado.Add(ITEMS, lstCC);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        #endregion

        #region PERMISOS
        public bool esAutorizarCXC()
        {
            bool esAutorizar = false;

            try
            {
                var objPermisos = _context.tblCXC_Permisos.FirstOrDefault(e => e.esActivo && e.idUsuario == vSesiones.sesionUsuarioDTO.id);

                if (objPermisos != null && objPermisos.esAutorizar)
                {
                    esAutorizar = true;
                }
            }
            catch (Exception e )
            {
                throw e;
            }

            return esAutorizar;
        }
        #endregion
    }
}
