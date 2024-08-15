using Core.DAO.Enkontrol.General.CC;
using Core.DAO.Facturacion.Enkontrol;
using Core.Entity.Administrativo.Contabilidad.Facturas;
using Core.Entity.Facturacion.Prefacturacion;
using Data.EntityFramework;
using Data.EntityFramework.Generic;
using Data.Factory.Enkontrol.General.CC;
using Data.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Multiempresa;
using Core.DTO.Utils.Data;
using Core.DTO;
using System.Data.Entity;
using Core.DTO.Enkontrol.Tablas.Poliza;
using Core.DAO.Contabilidad.Poliza;
using Core.Service.Contabilidad.Poliza;
using Data.Factory.Contabilidad.Poliza;
using Core.Enum.Principal;

namespace Data.DAO.Facturacion.Enkontrol
{
    public class FacturasEKDAO : GenericDAO<tblF_EK_Facturas>, IFacturasSPDAO
    {
        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        private string NombreControlador = "FacturasEKController";

        #region TRANSAC

        private OdbcTransaction _transaccion;
        private OdbcConnection _ctx;

        public void SetContext(object context)
        {
            _ctx = context as OdbcConnection;
        }

        public void SetTransaccion(object transaccion)
        {
            _transaccion = transaccion as OdbcTransaction;
        }

        public object GetTransaccion()
        {
            return _transaccion;
        }
        #endregion

        ICCDAO _ccFS_SP = new CCFactoryService().getCCServiceSP();
        IPolizaSPDAO polizaSPService = new PolizaSPFactoryService().GetPolizaSPService();

        #region RUTAS CARPETAS

        #region ARCHIVOS | CONTRATOS FIRMADOS
        //private readonly string RutaContratosFirmados = @"\\REPOSITORIO\Proyecto\SIGOPLAN\CAPITAL_HUMANO\RECLUTAMIENTOS\CONTRATOS_FIRMADOS";
        //private const string RutaLocalContratosFirmados = @"C:\Proyecto\SIGOPLAN\CONTRATOS_FIRMADOS";
        //private readonly string RutaExpedientesDigitales = @"\\REPOSITORIO\Proyecto\SIGOPLAN\CAPITAL_HUMANO\RECLUTAMIENTOS\EXPEDIENTES_DIGITALES";
        //private readonly string RutaExpedientesDigitalesLocal = @"C:\Proyecto\SIGOPLAN\CAPITAL_HUMANO\RECLUTAMIENTOS\EXPEDIENTES_DIGITALES";
        //private readonly string RutaFotosEmpleadosLocal = @"C:\Proyecto\SIGOPLAN\CAPITAL_HUMANO\RECLUTAMIENTOS\FOTOS_EMPLEADOS";
        //private readonly string RutaFotosEmpleados = @"\\REPOSITORIO\Proyecto\SIGOPLAN\CAPITAL_HUMANO\RECLUTAMIENTOS\FOTOS_EMPLEADOS";
        #endregion

        #endregion

        #region RUTAS PARA SUBIR ARCHIVOS
        private readonly string RutaServidor;
        public FacturasEKDAO()
        {
            resultado.Clear();

#if DEBUG
            RutaServidor = @"C:\Proyectos\SIGOPLANv2\FACTURAS";
#else
            RutaServidor = @"\\10.1.0.112\Proyecto\SIGOPLAN\FACTURAS";
#endif
        }
        #endregion

        #region GENERALES

        //METODO MAIN PARA EL GUARDADO DE PREFACTURA Y SUS COMPLEMENTOS EN ENKONTROL
        public Dictionary<string, object> GuardarPrefactura(tblF_RepPrefactura obj, List<tblF_CapPrefactura> lst, List<tblF_CapImporte> lstImpuesto)
        {
            resultado.Clear();

            using (var dbTransac = _context.Database.BeginTransaction())
            {
                using (var con = new Conexion().ConexionEKAdm())
                {
                    using (var trans = con.BeginTransaction())
                    {
                        try
                        {

                            SetTransaccion(trans);

                            decimal cambioDls = GetTipoCambioNow();

                            //TIPO CAMBIO PARA TESTING
                            obj.TipoCambio = obj.TipoMoneda == "MX" ? 1 : cambioDls;

                            var dictEKPedido = GuardarPedidos(obj, lst, lstImpuesto, 0);
                            int? idEKPedido = dictEKPedido["items"] as int?;
                            var dictEKRemision = GuardarRemision(obj, lst, lstImpuesto, idEKPedido.Value, 0);
                            int? idEKRemision = dictEKRemision["items"] as int?;
                            var dictEKFactura = GuardarFactura(obj, lst, lstImpuesto, idEKPedido.Value, idEKRemision.Value, 0);
                            int? idEKFactura = dictEKFactura["items"] as int?;
                            int? idCfdFolio = dictEKFactura["cfd_folio"] as int?;

                            int idUsrEnkontrol = 0;
                            var usrEnkontrol = _context.tblP_Usuario_Enkontrol.FirstOrDefault(e => e.idUsuario == vSesiones.sesionUsuarioDTO.id);

                            if (usrEnkontrol != null)
                            {
                                idUsrEnkontrol = usrEnkontrol.empleado;
                            }
                            else
                            {
                                throw new Exception("EL usuario no tiene clave en enkontrol");
                            }

                            var metodoPago = _context.tblF_EK_MetodoPagoSat.FirstOrDefault(e => e.esActivo && e.id == obj.MetodoPagoSAT.Value);
                            string descMetodoPago = "";

                            if (metodoPago != null)
                            {
                                descMetodoPago = metodoPago.descripcion;
                            }

                            #region POLIZA

                            

                            decimal subFactSubTotal = Convert.ToDecimal(lstImpuesto[0].Valor.Substring(1, lstImpuesto[0].Valor.Length - 1));
                            decimal subFactIVA = Convert.ToDecimal(lstImpuesto[1].Valor.Substring(1, lstImpuesto[1].Valor.Length - 1));
                            decimal subFactTotal = Convert.ToDecimal(lstImpuesto[2].Valor.Substring(1, lstImpuesto[2].Valor.Length - 1));

                            decimal DLSsubFactSubTotal = Convert.ToDecimal(lstImpuesto[0].Valor.Substring(1, lstImpuesto[0].Valor.Length - 1)) * obj.TipoCambio.Value;
                            decimal DLSsubFactIVA = Convert.ToDecimal(lstImpuesto[1].Valor.Substring(1, lstImpuesto[1].Valor.Length - 1)) * obj.TipoCambio.Value;
                            decimal DLSsubFactTotal = Convert.ToDecimal(lstImpuesto[2].Valor.Substring(1, lstImpuesto[2].Valor.Length - 1)) * obj.TipoCambio.Value;

                            decimal polCargos = 0;
                            decimal polAbonos = 0;

                            if (obj.TipoMoneda == "MX")
	                        {
                                polCargos = subFactTotal;
                                polAbonos = -(subFactSubTotal) + -(subFactIVA);

	                        }else{
                                polCargos = DLSsubFactTotal;
                                polAbonos = -(DLSsubFactSubTotal) + -(DLSsubFactIVA);

                            }

                            var objPol = new sc_polizasDTO() 
                            { 
                                year = DateTime.Now.Year,
                                mes = DateTime.Now.Month,
                                tp = "08",
                                fechapol = DateTime.Now,
                                cargos = polCargos,
                                abonos = polAbonos,
                                generada = "F",
                                status = "A",
                                error = "",
                                status_lock = "N",
                                fec_hora_movto = DateTime.Now,
                                usuario_movto = idUsrEnkontrol,
                                fecha_hora_crea = DateTime.Now,
                                usuario_crea = idUsrEnkontrol,
                                socio_inversionista = null,
                                status_carga_pol = null,
                                concepto = null,
                            };

                            var objMovs = new List<Core.DTO.Enkontrol.Tablas.Poliza.sc_movpolDTO>();

                            //int[] arrCuentas = new int[4] { 0, 1125, 2117, 4000 };
                            //int[] DLSarrCuentas = new int[5] { 0, 1125, 2117, 4000 };}
                            List<List<int>> arrCuentas = new List<List<int>>() { 
                                new List<int>() { }
                                , new List<int>() { 1125, 1, 0, 0 } 
                                , new List<int>() { 2117, 2, 0, 0 } 
                                , new List<int>() { 4000, 1, 0, 8 } 
                            };

                            List<List<int>> DLSarrCuentas = new List<List<int>>() { 
                                new List<int>() { }
                                , new List<int>() { 1125, 2, 0, 0 } 
                                , new List<int>() { 1125, 200, 0, 5 } 
                                , new List<int>() { 2117, 1, 0, 8 } 
                                , new List<int>() { 4000, 1, 0, 8 } 
                            };

                            List<decimal> lstMontos = new List<decimal>();

                            int loopStart = 1;
                            int loopEnd = 1;

                            foreach (var item in lst)
                            {
                                decimal subFactIVADet = item.Importe * obj.IVA.Value;
                                decimal subFactSubTotalDet = item.Importe;
                                decimal subFactTotalDet = item.Importe + subFactIVADet;

                                decimal DLSsubFactSubTotalDet = item.Importe * obj.TipoCambio.Value; //Nacional
                                decimal DLSsubFactIVADet = DLSsubFactSubTotalDet * obj.IVA.Value; //Nacional
                                decimal DLSsubFactTotalDet = item.Importe; //EN DLS

                                switch (obj.TipoMoneda)
                                {
                                    case "MX":
                                        lstMontos = new List<decimal>() { 0, subFactTotalDet, -(subFactIVADet), -(subFactSubTotalDet) };

                                        if (objMovs.Count > 0)
                                        {
                                            loopStart += 3;
                                            loopEnd += 3;
                                        }
                                        else
                                        {
                                            loopStart = 1;
                                            loopEnd = 3;
                                        }

                                        for (int i = loopStart; i <= loopEnd; i++)
                                        {
                                            int modVal = i % 3;
                                            if (modVal == 0)
                                            {
                                                modVal = 3;
                                            }
                                            objMovs.Add(new Core.DTO.Enkontrol.Tablas.Poliza.sc_movpolDTO
                                            {
                                                year = DateTime.Now.Year,
                                                mes = DateTime.Now.Month,
                                                tp = "08",
                                                linea = i,
                                                cta = arrCuentas[modVal][0],
                                                scta = arrCuentas[modVal][1],
                                                sscta = arrCuentas[modVal][2],
                                                digito = arrCuentas[modVal][3],
                                                tm = modVal > 1 ? 2 : 1,
                                                referencia = idEKFactura.ToString(),
                                                cc = item.cc,
                                                concepto = modVal != 2 ? item.Unidad : obj.Nombre.Replace("- ", string.Empty),
                                                monto = lstMontos[modVal],
                                                iclave = 0,
                                                itm = obj.TipoMoneda == "MX" ? 1 : 0,
                                                st_par = "",
                                                orden_compra = obj.ReqOC,
                                                numpro = null,
                                                socio_inversionista = null,
                                                istm = null,
                                                folio_imp = null,
                                                linea_imp = null,
                                                num_emp = null,
                                                folio_gxc = null,
                                                cfd_ruta_pdf = null,
                                                cfd_ruta_xml = null,
                                                UUID = null,
                                                cfd_rfc = obj.RFC,
                                                cfd_tipocambio = null,
                                                cfd_total = null,
                                                cfd_moneda = obj.TipoMoneda == "MX" ? "1" : "2",
                                                metodo_pago_sat = null,
                                                ruta_comp_ext = null,
                                                taxid = null,
                                                forma_pago = null,
                                                cfd_fecha_expedicion = null,
                                                cfd_tipocomprobante = null,
                                                cfd_metodo_pago_sat = null,
                                                area = null,
                                                cuenta_oc = null,
                                            });
                                        }
                                        break;
                                    default:

                                        if (objMovs.Count > 0)
                                        {
                                            loopStart += 4;
                                            loopEnd += 4;
                                        }
                                        else
                                        {
                                            loopStart = 1;
                                            loopEnd = 4;
                                        }

                                        for (int i = loopStart; i <= loopEnd; i++)
                                        {
                                            int modVal = i % 4;
                                            if (modVal == 0)
                                            {
                                                modVal = 4;
                                            }

                                            decimal complementarioNac = (DLSsubFactSubTotalDet + DLSsubFactIVADet) - DLSsubFactTotalDet;

                                            lstMontos = new List<decimal>() { 0, DLSsubFactTotalDet, complementarioNac, -(DLSsubFactIVADet), -(DLSsubFactSubTotalDet) };

                                            objMovs.Add(new Core.DTO.Enkontrol.Tablas.Poliza.sc_movpolDTO
                                            {
                                                year = DateTime.Now.Year,
                                                mes = DateTime.Now.Month,
                                                tp = "08",
                                                linea = i,
                                                cta = DLSarrCuentas[i % 4][0],
                                                scta = DLSarrCuentas[i % 4][1],
                                                sscta = DLSarrCuentas[i % 4][2],
                                                digito = DLSarrCuentas[i % 4][3],
                                                tm = i % 4 > 2 ? 1 : 2,
                                                referencia = idEKFactura.ToString(),
                                                cc = item.cc,
                                                concepto = i % 4 != 3 ? item.Unidad : obj.Nombre.Replace("- ", string.Empty),
                                                monto = lstMontos[i % 4],
                                                iclave = 0,
                                                itm = obj.TipoMoneda == "MX" ? 1 : 0,
                                                st_par = "",
                                                orden_compra = obj.ReqOC,
                                                numpro = null,
                                                socio_inversionista = null,
                                                istm = null,
                                                folio_imp = null,
                                                linea_imp = null,
                                                num_emp = null,
                                                folio_gxc = null,
                                                cfd_ruta_pdf = null,
                                                cfd_ruta_xml = null,
                                                UUID = null,
                                                cfd_rfc = obj.RFC,
                                                cfd_tipocambio = null,
                                                cfd_total = null,
                                                cfd_moneda = obj.TipoMoneda == "MX" ? "1" : "2",
                                                metodo_pago_sat = null,
                                                ruta_comp_ext = null,
                                                taxid = null,
                                                forma_pago = null,
                                                cfd_fecha_expedicion = null,
                                                cfd_tipocomprobante = null,
                                                cfd_metodo_pago_sat = null,
                                                area = null,
                                                cuenta_oc = null,
                                            });
                                        }
                                        break;
                                }
                            }

                            int idPolizaEK = GuardarPoliza(objPol, objMovs);

                            polizaSPService.SetContext(_context);
                            polizaSPService.SetTransaccion(dbTransac);
                            string idPolizaSP = polizaSPService.GuardarPoliza(objPol, objMovs);
                            #endregion

                            #region MOVCLTES
                            GuardarMovCltes(obj, lst, lstImpuesto, idEKPedido.Value, idEKRemision.Value, idEKFactura.Value, idPolizaEK, idCfdFolio.Value);
                            #endregion

                            #region ACTUALIZAR DATOS SP
                            var objPedido = _context.tblF_EK_Pedidos.FirstOrDefault(e => e.esActivo && e.folioPrefactura == obj.Folio);
                            objPedido.pedido = idEKPedido.Value;

                            _context.SaveChanges();

                            var lstPedidoDet = _context.tblF_EK_Pedido_Det.Where(e => e.esActivo && e.folioPrefactura == obj.Folio).ToList();

                            foreach (var item in lstPedidoDet)
                            {
                                item.pedido = idEKPedido.Value;
                            }

                            _context.SaveChanges();

                            var objRemision = _context.tblF_EK_Remision.FirstOrDefault(e => e.esActivo && e.folioPrefactura == obj.Folio);
                            objRemision.pedido = idEKPedido.Value;
                            objRemision.remision = idEKRemision.Value;
                            objRemision.factura = idEKFactura.Value;

                            _context.SaveChanges();


                            var lstRemisionDet = _context.tblF_EK_Remision_Det.Where(e => e.esActivo && e.folioPrefactura == obj.Folio).ToList();

                            foreach (var item in lstRemisionDet)
                            {
                                item.pedido = idEKPedido.Value;
                                item.remision = idEKRemision.Value;
                            }

                            _context.SaveChanges();


                            var objFactura = _context.tblF_EK_Facturas.FirstOrDefault(e => e.esActivo && e.folioPrefactura == obj.Folio);
                            objFactura.pedido = idEKPedido.Value;
                            objFactura.remision = idEKRemision.Value;
                            objFactura.factura = idEKFactura.Value;

                            _context.SaveChanges();


                            var lstFacturaDet = _context.tblF_EK_Facturas_Det.Where(e => e.esActivo && e.folioPrefactura == obj.Folio).ToList();

                            foreach (var item in lstFacturaDet)
                            {
                                item.pedido = idEKPedido.Value;
                                item.remision = idEKRemision.Value;
                                item.factura = idEKFactura.Value;
                            }

                            _context.SaveChanges();

                            #endregion

                            #region CONSECUTIVO FACTURA CFD

                            UpdateConsecutivoFacturaCFD(obj,lst,lstImpuesto,idEKFactura.Value,idUsrEnkontrol,descMetodoPago);

                            #endregion

                            #region CONSECUTIVO FACTURA SUCURSAL
                            UpdateConsecutivoFacturaSucursal(idEKFactura.Value);
                            #endregion

                            resultado.Clear();

                            trans.Commit();
                            dbTransac.Commit();

                            resultado.Add(SUCCESS, true);
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            dbTransac.Rollback();

                            resultado.Add(MESSAGE, e.Message);
                            resultado.Add(SUCCESS, false);
                            //throw e;
                        }
                    }
                }
            }

            return resultado;
        }
        public decimal GetTipoCambioNow()
        {
            try
            {

                var ultimoRegistro = new OdbcConsultaDTO();

                ultimoRegistro.consulta =
                    @"SELECT TOP 1 tipo_cambio FROM tipo_cambio ORDER BY fecha DESC";

                var ultimoValue = _contextEnkontrol.Select<dynamic>(vSesiones.sesionAmbienteEnkontrolAdm, ultimoRegistro).FirstOrDefault();

                return Convert.ToDecimal(ultimoValue.tipo_cambio);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region PEDIDOS
        public int GetLastPedido()
        {
            try
            {

                var ultimoRegistro = new OdbcConsultaDTO();

                ultimoRegistro.consulta =
                    @"SELECT TOP 1 pedido FROM sf_pedidos ORDER BY pedido DESC";

                var ultimoValue = _contextEnkontrol.Select<dynamic>(vSesiones.sesionAmbienteEnkontrolAdm, ultimoRegistro).FirstOrDefault();

                return Convert.ToInt32(ultimoValue.pedido);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Dictionary<string, object> GuardarPedidos(tblF_RepPrefactura obj, List<tblF_CapPrefactura> lst, List<tblF_CapImporte> lstImpuesto, int idSPPedido)
        {
            resultado.Clear();
            int idPedido = 0;

            int idUsrEnkontrol = 0;
            var usrEnkontrol = _context.tblP_Usuario_Enkontrol.FirstOrDefault(e => e.idUsuario == vSesiones.sesionUsuarioDTO.id);

            if (usrEnkontrol != null)
            {
                idUsrEnkontrol = usrEnkontrol.empleado;
            }
            else
            {
                throw new Exception("EL usuario no tiene clave en enkontrol");
            }

            try
            {
                //if (obj.id > 0)
                if (false) 
                {
                    #region EDITAR

//                            var count = 0;
//                            idPedido = idSPPedido;

//                            #region V1
//                            //                            var consulta = @"UPDATE sf_pedidos 
//                            //                                                pedido = ?,
//                            //                                                numcte = ?,
//                            //                                                sucursal = ?,
//                            //                                                fecha = ?,
//                            //                                                requisicion = ?,
//                            //                                                vendedor = ?,
//                            //                                                cond_pago = ?,
//                            //                                                moneda = ?,
//                            //                                                tipo_cambio = ?,
//                            //                                                sub_total = ?,
//                            //                                                iva = ?,
//                            //                                                total = ?,
//                            //                                                porcent_iva = ?,
//                            //                                                tipo = ?,
//                            //                                                porcent_descto = ?,
//                            //                                                descuento = ?,
//                            //                                                estatus = ?,
//                            //                                                condicion_entrega = ?,
//                            //                                                tipo_flete = ?,
//                            //                                                lista_precios = ?,
//                            //                                                status_autorizado = ?,
//                            //                                                zona = ?,
//                            //                                                obs = ?,
//                            //                                                otros_cond_pago = ?,
//                            //                                                usario = ?,
//                            //                                                fecha_hora = ?,
//                            //                                                tipo_pedido = ?,
//                            //                                                cc = ?,
//                            //                                                tm = ?,
//                            //                                                elaboro = ?,
//                            //                                                cia_sucursal = ?,
//                            //                                                tipo_credito = ?,
//                            //                                                retencion = ?,
//                            //                                                aplica_total_antes_iva = ?,
//                            //                                                total_dec = ?,
//                            //                                                iva_partida = 
//                            //                                            WHERE pedido = ?";
//                            #endregion

//                            #region SF_PEDIDO
//                            var consulta = @"UPDATE sf_pedidos 
//                                                fecha = ?,
//                                                requisicion = ?,
//                                                vendedor = ?,
//                                                cond_pago = ?,
//                                                moneda = ?,
//                                                tipo_cambio = ?,
//                                                sub_total = ?,
//                                                iva = ?,
//                                                total = ?,
//                                                porcent_iva = ?,
//                                                estatus = ?,
//                                                condicion_entrega = ?,
//                                                tipo_flete = ?,
//                                                obs = ?,
//                                                fecha_hora = ?,
//                                                tipo_pedido = ?,
//                                                cc = ?,
//                                                tm = ?,
//                                                elaboro = ?
//                                            WHERE pedido = ?";

//                            using (var cmd = new OdbcCommand(consulta))
//                            {
//                                OdbcParameterCollection parameters = cmd.Parameters;

//                                parameters.Add("@fecha", OdbcType.DateTime).Value = DateTime.Now;
//                                parameters.Add("@requisicion", OdbcType.Numeric).Value = obj.ReqOC;
//                                parameters.Add("@vendedor", OdbcType.Numeric).Value = idUsrEnkontrol;
//                                parameters.Add("@cond_pago", OdbcType.Numeric).Value = obj.CondicionesPago;
//                                parameters.Add("@moneda", OdbcType.Numeric).Value = obj.TipoMoneda == "MX" ? 1 : 2;
//                                parameters.Add("@tipo_cambio", OdbcType.Numeric).Value = 0;
//                                parameters.Add("@subtotal", OdbcType.Numeric).Value = Convert.ToDecimal(lstImpuesto[1].Valor.Substring(1, lstImpuesto[1].Valor.Length - 1));
//                                parameters.Add("@iva", OdbcType.Numeric).Value = Convert.ToDecimal(lstImpuesto[2].Valor.Substring(1, lstImpuesto[2].Valor.Length - 1));
//                                parameters.Add("@total", OdbcType.Numeric).Value = Convert.ToDecimal(lstImpuesto[0].Valor.Substring(1, lstImpuesto[0].Valor.Length - 1));
//                                parameters.Add("@condicion_entrega", OdbcType.Char).Value = obj.CondEntrega;
//                                parameters.Add("@tipo_flete", OdbcType.Char).Value = obj.TipoFlete;
//                                parameters.Add("@obs", OdbcType.Char).Value = obj.Observaciones;
//                                parameters.Add("@fecha_hora", OdbcType.DateTime).Value = DateTime.Now;
//                                parameters.Add("@tipo_pedido", OdbcType.Char).Value = obj.TipoPedido;
//                                parameters.Add("@cc", OdbcType.Char).Value = obj.CC;
//                                parameters.Add("@tm", OdbcType.Numeric).Value = obj.TM;
//                                parameters.Add("@elaboro", OdbcType.Numeric).Value = idUsrEnkontrol;

//                                //WHERE
//                                parameters.Add("@pedido", OdbcType.Numeric).Value = idPedido;

//                                cmd.Connection = trans.Connection;
//                                cmd.Transaction = trans;

//                                count += cmd.ExecuteNonQuery();
//                            }
//                            #endregion

//                            #region SF_PEDIDOS_DET

//                            foreach (var item in lst)
//                            {
//                                var count2 = 0;
//                                var consulta2 = @"UPDATE sf_pedidos_det 
//                                                    insumo = ?,
//                                                    cantidad = ?,
//                                                    precio = ?,
//                                                    importe = ?,
//                                                    unidad = ?,
//                                                    usuario = ?,
//                                                    fecha_hora = ?,
//                                                    linea = ?,
//
//                                                    WHERE 
//                                                    pedido = ? AND partida = ?";

//                                using (var cmd = new OdbcCommand(consulta2))
//                                {
//                                    OdbcParameterCollection parameters = cmd.Parameters;


//                                    parameters.Add("@insumo", OdbcType.Numeric).Value = item.insumo;
//                                    parameters.Add("@cantidad", OdbcType.Numeric).Value = item.Cantidad;
//                                    parameters.Add("@precio", OdbcType.Numeric).Value = item.Precio;
//                                    parameters.Add("@importe", OdbcType.Numeric).Value = item.Importe;
//                                    parameters.Add("@unidad", OdbcType.Char).Value = ""; //LOTE PESOS USD NO APLICA
//                                    parameters.Add("@estatus", OdbcType.Char).Value = "T";
//                                    parameters.Add("@usuario", OdbcType.Char).Value = DBNull.Value;
//                                    parameters.Add("@fecha_hora", OdbcType.Char).Value = DateTime.Now;
//                                    parameters.Add("@linea", OdbcType.Char).Value = item.Concepto;

//                                    parameters.Add("@pedido", OdbcType.Numeric).Value = idPedido;
//                                    parameters.Add("@partida", OdbcType.Numeric).Value = item.Renglon;

//                                    cmd.Connection = trans.Connection;
//                                    cmd.Transaction = trans;

//                                    count2 += cmd.ExecuteNonQuery();
//                                }
//                            }
//                            #endregion

                    #endregion
                }
                else
                {
                    #region CREAR
                    idPedido = GetLastPedido();
                    idPedido++;
                    #region SF_PEDIDOS

                    var count = 0;
                    var consulta = @"INSERT INTO sf_pedidos 
                                    (pedido,numcte,sucursal,fecha,requisicion,vendedor,cond_pago, moneda, tipo_cambio, sub_total, iva, total, porcent_iva, tipo, porcent_descto, descuento, estatus, condicion_entrega, tipo_flete, lista_precios,
                                    status_autorizado, zona, obs, otros_cond_pago, usuario, fecha_hora, tipo_pedido, cc, tm,elaboro, cia_sucursal, tipo_credito, retencion, aplica_total_antes_iva, total_dec, iva_partida) 
                                    VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                    using (var cmd = new OdbcCommand(consulta))
                    {
                        OdbcParameterCollection parameters = cmd.Parameters;


                        parameters.Add("@pedido", OdbcType.Numeric).Value = idPedido;
                        parameters.Add("@numcte", OdbcType.Numeric).Value = obj.numcte.Value;
                        parameters.Add("@sucursal", OdbcType.Numeric).Value = 1;
                        parameters.Add("@fecha", OdbcType.DateTime).Value = DateTime.Now;
                        parameters.Add("@requisicion", OdbcType.Numeric).Value = obj.ReqOC.Value.ToString();
                        parameters.Add("@vendedor", OdbcType.Numeric).Value = idUsrEnkontrol;
                        parameters.Add("@cond_pago", OdbcType.Numeric).Value = obj.CondicionesPago;
                        parameters.Add("@moneda", OdbcType.Numeric).Value = obj.TipoMoneda == "MX" ? 1 : 2;
                        parameters.Add("@tipo_cambio", OdbcType.Numeric).Value = obj.TipoCambio;
                        parameters.Add("@sub_total", OdbcType.Numeric).Value = Convert.ToDecimal(lstImpuesto[0].Valor.Substring(1, lstImpuesto[0].Valor.Length - 1));
                        parameters.Add("@iva", OdbcType.Numeric).Value = Convert.ToDecimal(lstImpuesto[1].Valor.Substring(1, lstImpuesto[1].Valor.Length - 1));
                        parameters.Add("@total", OdbcType.Numeric).Value = Convert.ToDecimal(lstImpuesto[2].Valor.Substring(1, lstImpuesto[2].Valor.Length - 1));
                        parameters.Add("@porcent_iva", OdbcType.Numeric).Value = 16;
                        parameters.Add("@tipo", OdbcType.Char).Value = "0";
                        parameters.Add("@porcent_descto", OdbcType.Numeric).Value = 0;
                        parameters.Add("@descuento", OdbcType.Numeric).Value = 0;
                        parameters.Add("@estatus", OdbcType.Char).Value = "A";
                        parameters.Add("@condicion_entrega", OdbcType.Char).Value = obj.CondEntrega;
                        parameters.Add("@tipo_flete", OdbcType.Char).Value = obj.TipoFlete;
                        parameters.Add("@lista_precios", OdbcType.Numeric).Value = 0;
                        parameters.Add("@status_autorizado", OdbcType.Char).Value = "A"; //??
                        parameters.Add("@zona", OdbcType.Numeric).Value = 0;
                        parameters.Add("@obs", OdbcType.Char).Value = obj.Observaciones ?? "";
                        parameters.Add("@otros_cond_pago", OdbcType.Numeric).Value = DBNull.Value;
                        parameters.Add("@usuario", OdbcType.Numeric).Value = DBNull.Value;
                        parameters.Add("@fecha_hora", OdbcType.DateTime).Value = DateTime.Now;
                        parameters.Add("@tipo_pedido", OdbcType.Char).Value = obj.TipoPedido;
                        parameters.Add("@cc", OdbcType.Char).Value = obj.CC;
                        parameters.Add("@tm", OdbcType.Numeric).Value = obj.TM;
                        parameters.Add("@elaboro", OdbcType.Numeric).Value = idUsrEnkontrol;
                        parameters.Add("@cia_sucursal", OdbcType.Numeric).Value = 1;
                        parameters.Add("@tipo_credito", OdbcType.Char).Value = "C";
                        parameters.Add("@retencion", OdbcType.Numeric).Value = 0;
                        parameters.Add("@aplica_total_antes_iva", OdbcType.Numeric).Value = 0;
                        parameters.Add("@total_dec", OdbcType.Numeric).Value = 0;
                        parameters.Add("@iva_partida", OdbcType.Char).Value = "N"; // N/S

                        cmd.Connection = _transaccion.Connection;
                        cmd.Transaction = _transaccion;

                        count += cmd.ExecuteNonQuery();
                    }
                    #endregion

                    #region SF_PEDIDOS_DET

                    foreach (var item in lst)
                    {
                        var count2 = 0;
                        var consulta2 = @"INSERT INTO sf_pedidos_det 
                                    (pedido,partida,insumo,cantidad,precio,importe,unidad,estatus,porcent_descto,cant_pedido,precio_pedido,cant_facturada,factor_uni,fec_entrega,cant_cancelada,muestra,usuario,
                                    fecha_hora,cant_kg,cant_kg_cancelada,cant_kg_facturada,cant_kg_pedido,cant_x_surtir,cant_en_produc,cant_x_embarc,cant_produccion,cant_entregada,cant_remision,
                                    porcen_iva_partida,linea,cia_sucursal) 
                                    VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                        using (var cmd = new OdbcCommand(consulta2))
                        {
                            OdbcParameterCollection parameters = cmd.Parameters;


                            parameters.Add("@pedido", OdbcType.Numeric).Value = idPedido;
                            parameters.Add("@partida", OdbcType.Numeric).Value = item.Renglon;
                            parameters.Add("@insumo", OdbcType.Numeric).Value = item.TipoInsumo.Value;
                            parameters.Add("@cantidad", OdbcType.Numeric).Value = item.Cantidad;
                            parameters.Add("@precio", OdbcType.Numeric).Value = item.Precio;
                            parameters.Add("@importe", OdbcType.Numeric).Value = item.Importe;
                            parameters.Add("@unidad", OdbcType.Char).Value = item.Concepto; //LOTE PESOS USD NO APLICA
                            parameters.Add("@estatus", OdbcType.Char).Value = "T";
                            parameters.Add("@porcent_descto", OdbcType.Numeric).Value = 0M;
                            parameters.Add("@cant_pedido", OdbcType.Numeric).Value = 0M;
                            parameters.Add("@precio_pedido", OdbcType.Numeric).Value = DBNull.Value;
                            parameters.Add("@cant_factura", OdbcType.Numeric).Value = 0M;
                            parameters.Add("@factor_uni", OdbcType.Numeric).Value = 0M;
                            parameters.Add("@fec_entrega", OdbcType.DateTime).Value = DateTime.Now;
                            parameters.Add("@cant_cancelada", OdbcType.Numeric).Value = 0M;
                            parameters.Add("@muestra", OdbcType.Char).Value = "N";
                            parameters.Add("@usuario", OdbcType.Char).Value = DBNull.Value;
                            parameters.Add("@fecha_hora", OdbcType.DateTime).Value = DateTime.Now;
                            parameters.Add("@cant_kg", OdbcType.Char).Value = DBNull.Value;
                            parameters.Add("@cant_kg_cancelada", OdbcType.Numeric).Value = DBNull.Value;
                            parameters.Add("@cant_kg_facturada", OdbcType.Numeric).Value = DBNull.Value;
                            parameters.Add("@cant_kg_pedido", OdbcType.Numeric).Value = DBNull.Value;
                            parameters.Add("@cant_x_surtir", OdbcType.Char).Value = DBNull.Value;
                            parameters.Add("@cant_en_produc", OdbcType.Numeric).Value = DBNull.Value;
                            parameters.Add("@cant_x_embarc", OdbcType.Numeric).Value = DBNull.Value;
                            parameters.Add("@cant_produccion", OdbcType.Numeric).Value = DBNull.Value;
                            parameters.Add("@cant_entregada", OdbcType.Numeric).Value = DBNull.Value;
                            parameters.Add("@cant_remision", OdbcType.Numeric).Value = 0M;
                            parameters.Add("@porcen_iva_partida", OdbcType.Numeric).Value = 0M;
                            parameters.Add("@linea", OdbcType.Char).Value = item.Unidad;
                            parameters.Add("@cia_sucursal", OdbcType.Numeric).Value = 1;

                            cmd.Connection = _transaccion.Connection;
                            cmd.Transaction = _transaccion;

                            count2 += cmd.ExecuteNonQuery();
                        }
                    }
                    #endregion

                    #endregion
                }

                //trans.Commit();

                resultado.Add(ITEMS, idPedido);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {

                //trans.Rollback();

                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
            
        }
        #endregion

        #region REMISION
        public int GetLastRemision()
        {
            try
            {
                var ultimoRegistro = _contextEnkontrol.Select<dynamic>(vSesiones.sesionAmbienteEnkontrolAdm, new OdbcConsultaDTO()
                {
                    consulta = string.Format(@"SELECT TOP 1 remision FROM sf_remision ORDER BY remision DESC")
                }).ToList();
                return Convert.ToInt32(ultimoRegistro.FirstOrDefault().remision) + 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Dictionary<string, object> GuardarRemision(tblF_RepPrefactura obj, List<tblF_CapPrefactura> lst, List<tblF_CapImporte> lstImpuesto, int idSPPedido, int idSPRemision)
        {
            resultado.Clear();

            int idRemision = 0;

            int idUsrEnkontrol = 0;
            var usrEnkontrol = _context.tblP_Usuario_Enkontrol.FirstOrDefault(e => e.idUsuario == vSesiones.sesionUsuarioDTO.id);

            if (usrEnkontrol != null)
            {
                idUsrEnkontrol = usrEnkontrol.empleado;
            }
            else
            {
                throw new Exception("EL usuario no tiene clave en enkontrol");
            }

            try
            {

                //if (obj.id > 0)
                if (false)
                {
                    #region EDITAR

//                            idRemision = idSPRemision;

//                            #region SF_REMISION
//                            var count = 0;
//                            var consulta = @"UPDATE
//                                                sucursal = ?,
//                                                fecha = ?,
//                                                rfc = ?,
//                                                nombre = ?,
//                                                direccion = ?,
//                                                duicada = ?,
//                                                cp = ?,
//                                                telefono = ?,
//                                                transporte = ?,
//                                                talon = ?,
//                                                consignado = ?,
//                                                observaciones = ?,
//                                                moneda = ?,
//                                                tipo_cambio = ?,
//                                                sub_total = ?,
//                                                iva = ?,
//                                                total = ?,
//                                                porcent_iva = ?,
//                                                porcent_descto = ?,
//                                                elaboro = ?,
//                                                tipo_flete = ?,
//                                                descuento = ?,
//                                                factura = ?,
//                                                estatus = ?,
//                                                entregado = ?,
//                                                pedido = ?,
//                                                retencion = ? 
//                                                WHERE
//                                                remision = ?";

//                            using (var cmd = new OdbcCommand(consulta))
//                            {
//                                OdbcParameterCollection parameters = cmd.Parameters;


//                                parameters.Add("@sucursal", OdbcType.Numeric).Value = 1;
//                                parameters.Add("@remision", OdbcType.Numeric).Value = idRemision;
//                                parameters.Add("@fecha", OdbcType.DateTime).Value = DateTime.Now;
//                                parameters.Add("@rfc", OdbcType.Char).Value = obj.RFC;
//                                parameters.Add("@nombre", OdbcType.Char).Value = obj.Nombre;
//                                parameters.Add("@direccion", OdbcType.Char).Value = obj.Direccion;
//                                parameters.Add("@ciudad", OdbcType.Char).Value = "";
//                                parameters.Add("@cp", OdbcType.Numeric).Value = obj.CP;
//                                parameters.Add("@telefono", OdbcType.Numeric).Value = 0;
//                                parameters.Add("@transporte", OdbcType.Numeric).Value = 1;
//                                parameters.Add("@talon", OdbcType.Numeric).Value = 1;
//                                parameters.Add("@consignado", OdbcType.Numeric).Value = 1; //?????????
//                                parameters.Add("@observaciones", OdbcType.Char).Value = obj.Observaciones;
//                                parameters.Add("@moneda", OdbcType.Numeric).Value = obj.TipoMoneda == "MX" ? 1 : 2;
//                                parameters.Add("@tipo_cambio", OdbcType.Numeric).Value = 0;
//                                parameters.Add("@sub_total", OdbcType.Numeric).Value = Convert.ToDecimal(lstImpuesto[1].Valor.Substring(1, lstImpuesto[1].Valor.Length - 1));
//                                parameters.Add("@iva", OdbcType.Numeric).Value = Convert.ToDecimal(lstImpuesto[2].Valor.Substring(1, lstImpuesto[2].Valor.Length - 1));
//                                parameters.Add("@total", OdbcType.Numeric).Value = Convert.ToDecimal(lstImpuesto[0].Valor.Substring(1, lstImpuesto[0].Valor.Length - 1));
//                                parameters.Add("@porcent_iva", OdbcType.Numeric).Value = 16; //??????
//                                parameters.Add("@porcent_descto", OdbcType.Numeric).Value = 0;
//                                parameters.Add("@elaboro", OdbcType.Numeric).Value = idUsrEnkontrol;
//                                parameters.Add("@tipo_flete", OdbcType.Char).Value = obj.TipoFlete;
//                                parameters.Add("@descuento", OdbcType.Numeric).Value = 0;
//                                //parameters.Add("@factura", OdbcType.Numeric).Value = idFactura;
//                                parameters.Add("@estatus", OdbcType.Char).Value = "A";
//                                parameters.Add("@entrega", OdbcType.Char).Value = obj.CondEntrega;
//                                parameters.Add("@pedido", OdbcType.Numeric).Value = idSPPedido;
//                                parameters.Add("@retencion", OdbcType.Numeric).Value = 0;

//                                cmd.Connection = trans.Connection;
//                                cmd.Transaction = trans;

//                                count += cmd.ExecuteNonQuery();
//                            }
                    //#endregion

                    #endregion
                }
                else
                {
                    #region CREAR
                    idRemision = GetLastRemision();
                    int idFactura = GetLastFactura();

                    #region SF_REMISION

                    var count = 0;
                    var consulta = @"INSERT INTO sf_remision
                                    (sucursal,remision,fecha,rfc,nombre,direccion,ciudad,cp,telefono,transporte,talon,consignado,observaciones,moneda,tipo_cambio,sub_total,iva,total,porcent_iva,porcent_descto,elaboro,tipo_flete,descuento
                                    ,factura,estatus,entregado,pedido,retencion) 
                                    VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                    using (var cmd = new OdbcCommand(consulta))
                    {
                        OdbcParameterCollection parameters = cmd.Parameters;


                        parameters.Add("@sucursal", OdbcType.Numeric).Value = 1;
                        parameters.Add("@remision", OdbcType.Numeric).Value = idRemision;
                        parameters.Add("@fecha", OdbcType.DateTime).Value = DateTime.Now;
                        parameters.Add("@rfc", OdbcType.Char).Value = obj.RFC;
                        parameters.Add("@nombre", OdbcType.Char).Value = obj.Nombre;
                        parameters.Add("@direccion", OdbcType.Char).Value = obj.Direccion;
                        parameters.Add("@ciudad", OdbcType.Char).Value = "";
                        parameters.Add("@cp", OdbcType.Char).Value = obj.CP;
                        parameters.Add("@telefono", OdbcType.Numeric).Value = 0;
                        parameters.Add("@transporte", OdbcType.Numeric).Value = 1;
                        parameters.Add("@talon", OdbcType.Numeric).Value = 1;
                        parameters.Add("@consignado", OdbcType.Numeric).Value = 1; //?????????
                        parameters.Add("@observaciones", OdbcType.Char).Value = obj.Observaciones ?? "";
                        parameters.Add("@moneda", OdbcType.Numeric).Value = obj.TipoMoneda == "MX" ? 1 : 2;
                        parameters.Add("@tipo_cambio", OdbcType.Numeric).Value = obj.TipoCambio;
                        parameters.Add("@sub_total", OdbcType.Numeric).Value = Convert.ToDecimal(lstImpuesto[0].Valor.Substring(1, lstImpuesto[0].Valor.Length - 1));
                        parameters.Add("@iva", OdbcType.Numeric).Value = Convert.ToDecimal(lstImpuesto[1].Valor.Substring(1, lstImpuesto[1].Valor.Length - 1));
                        parameters.Add("@total", OdbcType.Numeric).Value = Convert.ToDecimal(lstImpuesto[2].Valor.Substring(1, lstImpuesto[2].Valor.Length - 1));
                        parameters.Add("@porcent_iva", OdbcType.Numeric).Value = obj.IVA*100; //??????
                        parameters.Add("@porcent_descto", OdbcType.Numeric).Value = 0;
                        parameters.Add("@elaboro", OdbcType.Numeric).Value = idUsrEnkontrol;
                        parameters.Add("@tipo_flete", OdbcType.Char).Value = obj.TipoFlete;
                        parameters.Add("@descuento", OdbcType.Numeric).Value = 0;
                        parameters.Add("@factura", OdbcType.Numeric).Value = idFactura;
                        parameters.Add("@estatus", OdbcType.Char).Value = "A";
                        parameters.Add("@entregado", OdbcType.Char).Value = "S";
                        parameters.Add("@pedido", OdbcType.Numeric).Value = idSPPedido;
                        parameters.Add("@retencion", OdbcType.Numeric).Value = 0;

                        cmd.Connection = _transaccion.Connection;
                        cmd.Transaction = _transaccion;

                        count += cmd.ExecuteNonQuery();
                    }
                    #endregion

                    #region SF_REMISION_DET

                    foreach (var item in lst)
                    {


                        var count2 = 0;
                        var consulta2 = @"INSERT INTO sf_remision_det
                                    (sucursal,remision,partida,insumo,cantidad,precio,importe,unidad,cant_remision,precio_remision,partida_factura,pedido,ped_part,cant_facturada,linea) 
                                    VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                        using (var cmd = new OdbcCommand(consulta2))
                        {
                            OdbcParameterCollection parameters = cmd.Parameters;


                            parameters.Add("@sucursal", OdbcType.Numeric).Value = 1;
                            parameters.Add("@remision", OdbcType.Numeric).Value = idRemision;
                            parameters.Add("@partida", OdbcType.Numeric).Value = item.Renglon;
                            parameters.Add("@insumo", OdbcType.Numeric).Value = item.TipoInsumo.Value;
                            parameters.Add("@cantidad", OdbcType.Numeric).Value = item.Cantidad;
                            parameters.Add("@precio", OdbcType.Numeric).Value = item.Cantidad;
                            parameters.Add("@importe", OdbcType.Numeric).Value = item.Importe;
                            parameters.Add("@unidad", OdbcType.Char).Value = item.Concepto;
                            parameters.Add("@cant_remision", OdbcType.Numeric).Value = 0;
                            parameters.Add("@precio_remision", OdbcType.Numeric).Value = 0;
                            parameters.Add("@partida_factura", OdbcType.Numeric).Value = DBNull.Value;
                            parameters.Add("@pedido", OdbcType.Numeric).Value = idSPPedido;
                            parameters.Add("@ped_part", OdbcType.Numeric).Value = 1;
                            parameters.Add("@cant_facturada", OdbcType.Numeric).Value = DBNull.Value;
                            parameters.Add("@linea", OdbcType.Char).Value = item.Unidad;

                            cmd.Connection = _transaccion.Connection;
                            cmd.Transaction = _transaccion;

                            count2 += cmd.ExecuteNonQuery();
                        }
                    }
                    #endregion

                    #endregion
                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, idRemision);

            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }


            return resultado;
        }
        #endregion

        #region FACTURA
        public int GetLastFactura()
        {
            try
            {
                var ultimoRegistro = _contextEnkontrol.Select<dynamic>(vSesiones.sesionAmbienteEnkontrolAdm, new OdbcConsultaDTO()
                {
                    consulta = string.Format(@"SELECT TOP 1 factura FROM sf_facturas ORDER BY factura DESC")
                }).ToList();
                return Convert.ToInt32(ultimoRegistro.FirstOrDefault().factura) + 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetLastFolio()
        {
            try 
	        {
                var ultimoRegistro = _contextEnkontrol.Select<dynamic>(vSesiones.sesionAmbienteEnkontrolAdm, new OdbcConsultaDTO()
                {
                    consulta = string.Format(@"SELECT TOP 1 cfd_folio FROM sf_facturas ORDER BY factura DESC")
                }).ToList();
                return Convert.ToInt32(ultimoRegistro.FirstOrDefault().cfd_folio) + 1;
	        }
	        catch (Exception e)
	        {
		        throw e;
	        }

        }

        public Dictionary<string, object> GuardarFactura(tblF_RepPrefactura obj, List<tblF_CapPrefactura> lst, List<tblF_CapImporte> lstImpuesto, int idSPPedido, int idSPRemision, int idSPFactura)
        {
            resultado.Clear();

            try
            {
                int idFactura = GetLastFactura();

                //Consecutivo cfd_folios
                int idCFDFolio = GetLastFolio();

                int idUsrEnkontrol = 0;
                var usrEnkontrol = _context.tblP_Usuario_Enkontrol.FirstOrDefault(e => e.idUsuario == vSesiones.sesionUsuarioDTO.id);

                if (usrEnkontrol != null)
                {
                    idUsrEnkontrol = usrEnkontrol.empleado;
                }
                else
                {
                    throw new Exception("EL usuario no tiene clave en enkontrol");
                }

                var metodoPago = _context.tblF_EK_MetodoPagoSat.FirstOrDefault(e => e.esActivo && e.id == obj.MetodoPagoSAT.Value);
                string descMetodoPago = "";

                if (metodoPago != null)
                {
                    descMetodoPago = metodoPago.descripcion;
                }

                var objFormaPago = _context.tblF_EK_FormaPagoSat.FirstOrDefault(e => e.clave_sat == obj.MetodoPago);
                int formaPagoEK = 0;

                if (objFormaPago != null)
                {
                    formaPagoEK = objFormaPago.id_enkontrol;
                }

                //COMPAÑIA EK
                string compañiaEK = "";
                switch ((int)(MainContextEnum)vSesiones.sesionEmpresaActual)
                {
                    case 1:
                        if (vSesiones.sesionAmbienteEnkontrolAdm == Core.Enum.Multiempresa.EnkontrolAmbienteEnum.Prod)
                        {
                            compañiaEK = "01";
                        }
                        else
                        {
                            compañiaEK = "98";

                        }

                        break;
                    case 2:
                        if (vSesiones.sesionAmbienteEnkontrolAdm == Core.Enum.Multiempresa.EnkontrolAmbienteEnum.Prod)
                        {
                            compañiaEK = "04";
                        }
                        else
                        {
                            compañiaEK = "99";

                        }

                        break;
                    default:

                        compañiaEK = "98";
                        break;
                }

                #region SF_FACTURA

                var count = 0;
                var consulta = @"INSERT INTO sf_facturas
                                    (factura,numcte,sucursal,fecha,pedido,remision,requisicion,vendedor,consignado,transporte,cond_pago,talon,moneda,tipo_cambio,sub_total,iva,total,porcent_iva,tipo,lista_precio,cc,tm,descuento,
                                    porcent_descto,nombre,rfc,direccion,ciudad,cp,estatus,obs,compania,talon_emb,camion,placas,pais,estado,tipo_clase,cia_sucursal,numero_nc,tipo_nc,status_notacredito,retencion,cfd_folio,
                                    cfd_fecha,cfd_no_certificado,cfd_ano_aprob,cfd_num_aprob,cfd_cadena_original,cfd_certificado_sello,cfd_sello_digital,cfd_serie,fecha_cancelacion,usuario_cancela,cfd_subtotal,cfd_descuento,
                                    cfd_iva,cfd_retenciones,cfd_total,enviado,tipo_documento,autoriza_factura,empleado_autoriza,fecha_autoriza,idPac,gsdb,cve_formulario,cfd_mn,cfd_enviada,cfd_ret_iva,id_metodo_pago,id_regimen_fiscal,cfd_num_cta_pago,
                                    comentarios_cfdi,bit_cfdi,uuid,fecha_timbrado,certificado_sat,sello_sat,asn,bit_parcialidad,copia_xml,copia_xml_pdf,cfd_tiporelacion,refid,usocfdi,cfd_forma_pago_sat,cfd_metodo_pago_sat,cfd_condicionespago,
                                    cfd_reg_fiscal_sat,cfd_version,bit_implocal,cfd_certificado,bit_exento_iva) 
                                    VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                using (var cmd = new OdbcCommand(consulta))
                {
                    OdbcParameterCollection parameters = cmd.Parameters;

                    parameters.Add("@factura", OdbcType.Numeric).Value = idFactura;
                    parameters.Add("@numcte", OdbcType.Numeric).Value = obj.numcte.Value;
                    parameters.Add("@sucursal", OdbcType.Numeric).Value = 1;
                    parameters.Add("@fecha", OdbcType.DateTime).Value = DateTime.Now;
                    parameters.Add("@pedido", OdbcType.Numeric).Value = idSPPedido;
                    parameters.Add("@remision", OdbcType.Numeric).Value = idSPRemision;
                    parameters.Add("@requisicion", OdbcType.Char).Value = obj.ReqOC;
                    parameters.Add("@vendedor", OdbcType.Numeric).Value = idUsrEnkontrol;
                    parameters.Add("@consignado", OdbcType.Char).Value = "1";
                    parameters.Add("@transporte", OdbcType.Char).Value = "1";
                    parameters.Add("@cond_pago", OdbcType.Numeric).Value = obj.CondicionesPago;
                    parameters.Add("@talon", OdbcType.Numeric).Value = obj.TipoMoneda == "MX" ? 1 : 2;
                    parameters.Add("@moneda", OdbcType.Char).Value = "1";
                    parameters.Add("@tipo_cambio", OdbcType.Numeric).Value = obj.TipoCambio;
                    parameters.Add("@sub_total", OdbcType.Numeric).Value = Convert.ToDecimal(lstImpuesto[0].Valor.Substring(1, lstImpuesto[0].Valor.Length - 1));
                    parameters.Add("@iva", OdbcType.Numeric).Value = Convert.ToDecimal(lstImpuesto[1].Valor.Substring(1, lstImpuesto[1].Valor.Length - 1));
                    parameters.Add("@total", OdbcType.Numeric).Value = Convert.ToDecimal(lstImpuesto[2].Valor.Substring(1, lstImpuesto[2].Valor.Length - 1));
                    parameters.Add("@porcent_iva", OdbcType.Numeric).Value = obj.IVA * 100;
                    parameters.Add("@tipo", OdbcType.Char).Value = "N";
                    parameters.Add("@lista_precios", OdbcType.Numeric).Value = 0;
                    parameters.Add("@cc", OdbcType.Char).Value = obj.CC;
                    parameters.Add("@tm", OdbcType.Numeric).Value = obj.TM;
                    parameters.Add("@descuento", OdbcType.Numeric).Value = 0;
                    parameters.Add("@porcent_descto", OdbcType.Numeric).Value = 0;
                    parameters.Add("@nombre", OdbcType.Char).Value = obj.Nombre.Split('-')[1].Trim();
                    parameters.Add("@rfc", OdbcType.Char).Value = obj.RFC;
                    parameters.Add("@direccion", OdbcType.Char).Value = obj.Direccion;
                    parameters.Add("@ciudad", OdbcType.Char).Value = "";
                    parameters.Add("@cp", OdbcType.Char).Value = obj.CP;
                    parameters.Add("@estatus", OdbcType.Char).Value = "I";
                    parameters.Add("@obs", OdbcType.Char).Value = obj.Observaciones ?? "";
                    parameters.Add("@compania", OdbcType.Char).Value = compañiaEK;
                    parameters.Add("@talon_emb", OdbcType.Char).Value = DBNull.Value;
                    parameters.Add("@camion", OdbcType.Char).Value = DBNull.Value;
                    parameters.Add("@placas", OdbcType.Char).Value = DBNull.Value;
                    parameters.Add("@pais", OdbcType.Char).Value = "MEXICO";
                    parameters.Add("@estado", OdbcType.Char).Value = "COAH";
                    parameters.Add("@tipo_clase", OdbcType.Char).Value = "S";
                    parameters.Add("@cia_sucursal", OdbcType.Numeric).Value = 1;
                    parameters.Add("@numero_nc", OdbcType.Numeric).Value = 0;
                    parameters.Add("@tipo_nc", OdbcType.Numeric).Value = 0;
                    parameters.Add("@status_notacredito", OdbcType.Char).Value = DBNull.Value;
                    parameters.Add("@retencion", OdbcType.Numeric).Value = 0;
                    parameters.Add("@cfd_folio", OdbcType.Numeric).Value = idCFDFolio;
                    parameters.Add("@cfd_fecha", OdbcType.DateTime).Value = DateTime.Now;
                    parameters.Add("@cfd_no_certificado", OdbcType.Char).Value = "00001000000413753782";
                    parameters.Add("@cfd_ano_aprob", OdbcType.Numeric).Value = 2011;
                    parameters.Add("@cfd_num_aprob", OdbcType.Numeric).Value = 1;
                    parameters.Add("@cfd_cadena_original", OdbcType.Char).Value = string.Format(
                        @"||3.2|{0}|ingreso|{1}|{2}|0.00|{3}|{4}|HERMOSILLO, Son|NO IDENTIFICADO|GCP800324FJ1|GRUPO CONSTRUCCIONES PLANIFICADAS SA DE CV|PERIFERICO|770|EMILIANO ZAPATA|HERMOSILLO|HERMOSILLO|Son|MEXICO|83280"
                        , DateTime.Now.ToString("s"), descMetodoPago, obj.CondicionesPago, Convert.ToDecimal(lstImpuesto[1].Valor.Substring(1, lstImpuesto[1].Valor.Length - 1)), obj.TipoMoneda == "MX" ? "MN" : "19|USD", obj.MetodoPago, obj.RFC
                    );
                    parameters.Add("@cfd_certificado_sello", OdbcType.Char).Value = "";
                    parameters.Add("@cfd_sello_digital", OdbcType.Char).Value = "";
                    parameters.Add("@cfd_serie", OdbcType.Char).Value = "A";
                    parameters.Add("@fecha_cancelacion", OdbcType.DateTime).Value = DBNull.Value;
                    parameters.Add("@usuario_cancela", OdbcType.Numeric).Value = DBNull.Value;
                    parameters.Add("@cfd_subtotal", OdbcType.Numeric).Value = DBNull.Value;
                    parameters.Add("@cfd_descuento", OdbcType.Numeric).Value = DBNull.Value;
                    parameters.Add("@cfd_iva", OdbcType.Numeric).Value = DBNull.Value;
                    parameters.Add("@cfd_retenciones", OdbcType.Numeric).Value = DBNull.Value;
                    parameters.Add("@cfd_total", OdbcType.Numeric).Value = DBNull.Value;
                    parameters.Add("@enviado", OdbcType.Numeric).Value = 0;
                    parameters.Add("@tipo_documento", OdbcType.Char).Value = "F";
                    parameters.Add("@autoriza_factura", OdbcType.Char).Value = "S";
                    parameters.Add("@empleado_autoriza", OdbcType.Numeric).Value = 0;
                    parameters.Add("@fecha_autoriza", OdbcType.DateTime).Value = DBNull.Value;
                    parameters.Add("@idPac", OdbcType.Numeric).Value = 0;
                    parameters.Add("@gsdb", OdbcType.Char).Value = "";
                    parameters.Add("@cve_formulario", OdbcType.Numeric).Value = 0;
                    parameters.Add("@cfd_mn", OdbcType.Char).Value = "S";
                    parameters.Add("@cfd_enviada", OdbcType.Char).Value = "N";
                    parameters.Add("@cfd_ret_iva", OdbcType.Char).Value = "";
                    parameters.Add("@id_metodo_pago", OdbcType.Numeric).Value = formaPagoEK;
                    parameters.Add("@id_regimen_fiscal", OdbcType.Numeric).Value = obj.RegimenFiscal;
                    parameters.Add("@cfd_num_cta_pago", OdbcType.Char).Value = '0';
                    parameters.Add("@comentarios_cfdi", OdbcType.Char).Value = "Documento procesado sin timbrado, Solo CFDI";
                    parameters.Add("@bit_cfdi", OdbcType.Numeric).Value = 1;
                    parameters.Add("@uuid", OdbcType.Char).Value = DBNull.Value;
                    parameters.Add("@fecha_timbrado", OdbcType.DateTime).Value = DBNull.Value;
                    parameters.Add("@certificado_sat", OdbcType.Char).Value = DBNull.Value;
                    parameters.Add("@sello_sat", OdbcType.Char).Value = DBNull.Value;
                    parameters.Add("@asn", OdbcType.Char).Value = "";
                    parameters.Add("@bit_parcialidad", OdbcType.Numeric).Value = 0;
                    parameters.Add("@copia_xml", OdbcType.Char).Value = DBNull.Value;
                    parameters.Add("@copia_xml_pdf", OdbcType.Char).Value = DBNull.Value;
                    parameters.Add("@cfd_tiporelacion", OdbcType.Char).Value = "";
                    parameters.Add("@refid", OdbcType.Char).Value = DBNull.Value;
                    parameters.Add("@usocfdi", OdbcType.Char).Value = DBNull.Value;
                    parameters.Add("@cfd_forma_pago_sat", OdbcType.Char).Value = DBNull.Value;
                    parameters.Add("@cfd_metodo_pago_sat", OdbcType.Char).Value = DBNull.Value;
                    parameters.Add("@cfd_condicionespago", OdbcType.Char).Value = DBNull.Value;
                    parameters.Add("@cfd_reg_fiscal_sat", OdbcType.Char).Value = DBNull.Value;
                    parameters.Add("@cfd_version", OdbcType.Char).Value = "32";
                    parameters.Add("@bit_implocal", OdbcType.Numeric).Value = 0;
                    parameters.Add("@cfd_certificado", OdbcType.Char).Value = DBNull.Value;
                    parameters.Add("@bit_excento_iva", OdbcType.Numeric).Value = 0;

                    cmd.Connection = _transaccion.Connection;
                    cmd.Transaction = _transaccion;

                    count += cmd.ExecuteNonQuery();
                }
                #endregion

                #region SF_FACTURA_DET

                foreach (var item in lst)
                {


                    var count2 = 0;
                    var consulta2 = @"INSERT INTO sf_facturas_det
                                    (factura,partida,insumo,cantidad,precio,importe,unidad,cant_factura,precio_factura,pedido,ped_part,porcent_descto,cant_entregada,cant_kg_factura,cant_kg,cia_sucursal,numero_nc,fac_part,remision,linea,linea_nc,
                                    porcen_iva_partida,iva) 
                                    VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                    using (var cmd = new OdbcCommand(consulta2))
                    {
                        OdbcParameterCollection parameters = cmd.Parameters;


                        parameters.Add("@factura", OdbcType.Numeric).Value = idFactura;
                        parameters.Add("@partida", OdbcType.Numeric).Value = item.Renglon;
                        parameters.Add("@insumo", OdbcType.Numeric).Value = item.TipoInsumo.Value;
                        parameters.Add("@cantidad", OdbcType.Numeric).Value = item.Cantidad;
                        parameters.Add("@precio", OdbcType.Numeric).Value = item.Precio;
                        parameters.Add("@importe", OdbcType.Numeric).Value = item.Importe;
                        parameters.Add("@unidad", OdbcType.Char).Value = item.Concepto;
                        parameters.Add("@cant_factura", OdbcType.Numeric).Value = 1M;
                        parameters.Add("@precio_factura", OdbcType.Numeric).Value = 0;
                        parameters.Add("@pedido", OdbcType.Numeric).Value = idSPPedido;
                        parameters.Add("@ped_part", OdbcType.Numeric).Value = 1;
                        parameters.Add("@porcent_descto", OdbcType.Numeric).Value = 0;
                        parameters.Add("@cantidad_entregada", OdbcType.Numeric).Value = 0;
                        parameters.Add("@cant_kg_factura", OdbcType.Numeric).Value = 0;
                        parameters.Add("@cant_kg", OdbcType.Numeric).Value = 0;
                        parameters.Add("@cia_sucursal", OdbcType.Numeric).Value = 1;
                        parameters.Add("@numero_nc", OdbcType.Numeric).Value = 0;
                        parameters.Add("@fac_part", OdbcType.Numeric).Value = DBNull.Value;
                        parameters.Add("@remision", OdbcType.Numeric).Value = idSPRemision;
                        parameters.Add("@linea", OdbcType.Char).Value = item.Unidad;
                        parameters.Add("@linea_nc", OdbcType.Char).Value = "";
                        parameters.Add("@porcen_iva_partida", OdbcType.Numeric).Value = DBNull.Value;
                        parameters.Add("@iva", OdbcType.Numeric).Value = DBNull.Value;

                        cmd.Connection = _transaccion.Connection;
                        cmd.Transaction = _transaccion;

                        count2 += cmd.ExecuteNonQuery();
                    }
                }
                #endregion

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, idFactura);
                resultado.Add("cfd_folio", idCFDFolio);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public int ActualizarConsecutivoCfdFolio(int ultimoCfd)
        {
            try
            {

                var count = 0;
                var consulta = @"UPDATE cfd_folios
                                SET ult_folio = ?, 
                                fecha_cambios_log = ?
                                WHERE linea_folios = 1";

                using (var cmd = new OdbcCommand(consulta))
                {
                    OdbcParameterCollection parameters = cmd.Parameters;


                    parameters.Add("@ult_folio", OdbcType.Numeric).Value = ultimoCfd;
                    parameters.Add("@fecha_cambios_log", OdbcType.Date).Value = DateTime.Now;

                    cmd.Connection = _transaccion.Connection;
                    cmd.Transaction = _transaccion;

                    count += cmd.ExecuteNonQuery();
                }

                return ultimoCfd;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion

        #region POLIZA
        public int GuardarPoliza<Tpoliza, Tmovimientos>(Tpoliza poliza, List<Tmovimientos> movimientos)
        {
            try
            {
                var polizaDTO = poliza as sc_polizasDTO;
                var movimientosDTO = movimientos as List<sc_movpolDTO>;

                var numeroPoliza = GetNumeroPolizaNueva(polizaDTO.year, polizaDTO.mes, polizaDTO.tp);

                var query_sc_polizas =
                    @"INSERT INTO
                    sc_polizas
                    (
                        year,
                        mes,
                        poliza,
                        tp,
                        fechapol,
                        cargos,
                        abonos,
                        generada,
                        status,
                        error,
                        status_lock,
                        fec_hora_movto,
                        usuario_movto,
                        fecha_hora_crea,
                        usuario_crea,
                        socio_inversionista,
                        status_carga_pol,
                        concepto
                    )
                    VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

                using (var cmd = new OdbcCommand(query_sc_polizas))
                {
                    var parametros = cmd.Parameters;

                    parametros.Add("@year", OdbcType.Numeric).Value = polizaDTO.year;
                    parametros.Add("@mes", OdbcType.Numeric).Value = polizaDTO.mes;
                    parametros.Add("@poliza", OdbcType.Numeric).Value = numeroPoliza;
                    parametros.Add("@tp", OdbcType.Char).Value = polizaDTO.tp;
                    parametros.Add("@fechapol", OdbcType.Date).Value = polizaDTO.fechapol;
                    parametros.Add("@cargos", OdbcType.Numeric).Value = polizaDTO.cargos;
                    parametros.Add("@abonos", OdbcType.Numeric).Value = polizaDTO.abonos;
                    parametros.Add("@generada", OdbcType.Char).Value = polizaDTO.generada ?? "C";
                    parametros.Add("@status", OdbcType.Char).Value = polizaDTO.status ?? "C";
                    parametros.Add("@error", OdbcType.VarChar).Value = polizaDTO.error ?? "";
                    parametros.Add("@status_lock", OdbcType.Char).Value = polizaDTO.status_lock ?? "N";
                    parametros.Add("@fec_hora_movto", OdbcType.DateTime).Value = polizaDTO.fec_hora_movto ?? (object)DBNull.Value;
                    parametros.Add("@usuario_movto", OdbcType.Char).Value = polizaDTO.usuario_movto ?? (object)DBNull.Value;
                    parametros.Add("@fecha_hora_crea", OdbcType.DateTime).Value = polizaDTO.fecha_hora_crea ?? (object)DBNull.Value;
                    parametros.Add("@usuario_crea", OdbcType.Char).Value = polizaDTO.usuario_crea ?? (object)DBNull.Value;
                    parametros.Add("@socio_inversionista", OdbcType.Numeric).Value = polizaDTO.socio_inversionista ?? (object)DBNull.Value;
                    parametros.Add("@status_carga_pol", OdbcType.VarChar).Value = polizaDTO.status_carga_pol ?? "";
                    parametros.Add("@concepto", OdbcType.VarChar).Value = polizaDTO.concepto ?? (object)DBNull.Value;

                    cmd.Connection = _transaccion.Connection;
                    cmd.Transaction = _transaccion;
                    cmd.ExecuteNonQuery();
                }

                foreach (var movimiento in movimientosDTO)
                {
                    var query_sc_movpol =
                    @"INSERT INTO
                        sc_movpol
                        (
                            year,
                            mes,
                            poliza,
                            tp,
                            linea,
                            cta,
                            scta,
                            sscta,
                            digito,
                            tm,
                            referencia,
                            cc,
                            concepto,
                            monto,
                            iclave,
                            itm,
                            st_par,
                            orden_compra,
                            numpro,
                            socio_inversionista,
                            istm,
                            folio_imp,
                            linea_imp,
                            num_emp,
                            folio_gxc,
                            cfd_ruta_pdf,
                            cfd_ruta_xml,
                            UUID,
                            cfd_rfc,
                            cfd_tipocambio,
                            cfd_total,
                            cfd_moneda,
                            metodo_pago_sat,
                            ruta_comp_ext,
                            factura_comp_ext,
                            taxid,
                            forma_pago,
                            cfd_fecha_expedicion,
                            cfd_tipocomprobante,
                            cfd_metodo_pago_sat,
                            area,
                            cuenta_oc
                        )
                        VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

                    using (var cmd = new OdbcCommand(query_sc_movpol))
                    {
                        OdbcParameterCollection parametros = cmd.Parameters;

                        parametros.Add("@year", OdbcType.Numeric).Value = movimiento.year;
                        parametros.Add("@mes", OdbcType.Numeric).Value = movimiento.mes;
                        parametros.Add("@poliza", OdbcType.Numeric).Value = numeroPoliza;
                        parametros.Add("@tp", OdbcType.Char).Value = movimiento.tp;
                        parametros.Add("@linea", OdbcType.Numeric).Value = movimiento.linea;
                        parametros.Add("@cta", OdbcType.Numeric).Value = movimiento.cta;
                        parametros.Add("@scta", OdbcType.Numeric).Value = movimiento.scta;
                        parametros.Add("@sscta", OdbcType.Numeric).Value = movimiento.sscta;
                        parametros.Add("@digito", OdbcType.Numeric).Value = movimiento.digito;
                        parametros.Add("@tm", OdbcType.Numeric).Value = movimiento.tm;
                        parametros.Add("@referencia", OdbcType.Char).Value = movimiento.referencia;
                        parametros.Add("@cc", OdbcType.Char).Value = movimiento.cc ?? (object)DBNull.Value;
                        parametros.Add("@concepto", OdbcType.Char).Value = movimiento.concepto;
                        parametros.Add("@monto", OdbcType.Numeric).Value = movimiento.monto;
                        parametros.Add("@iclave", OdbcType.Numeric).Value = movimiento.iclave;
                        parametros.Add("@itm", OdbcType.Numeric).Value = movimiento.itm;
                        parametros.Add("@st_par", OdbcType.Char).Value = movimiento.st_par ?? "";
                        parametros.Add("@orden_compra", OdbcType.Numeric).Value = movimiento.orden_compra ?? 0;
                        parametros.Add("@numpro", OdbcType.Numeric).Value = movimiento.numpro ?? (object)DBNull.Value;
                        parametros.Add("@socio_inversionista", OdbcType.Numeric).Value = movimiento.socio_inversionista ?? (object)DBNull.Value;
                        parametros.Add("@istm", OdbcType.Numeric).Value = movimiento.istm ?? (object)DBNull.Value;
                        parametros.Add("@folio_imp", OdbcType.Numeric).Value = movimiento.folio_imp ?? (object)DBNull.Value;
                        parametros.Add("@linea_imp", OdbcType.Numeric).Value = movimiento.linea_imp ?? (object)DBNull.Value;
                        parametros.Add("@num_emp", OdbcType.Int).Value = movimiento.num_emp ?? (object)DBNull.Value;
                        parametros.Add("@folio_gxc", OdbcType.Int).Value = movimiento.folio_gxc ?? (object)DBNull.Value;
                        parametros.Add("@cfd_ruta_pdf", OdbcType.VarChar).Value = movimiento.cfd_ruta_pdf ?? "";
                        parametros.Add("@cfd_ruta_xml", OdbcType.VarChar).Value = movimiento.cfd_ruta_xml ?? "";
                        parametros.Add("@UUID", OdbcType.VarChar).Value = movimiento.UUID ?? (object)DBNull.Value;
                        parametros.Add("@cfd_rfc", OdbcType.VarChar).Value = movimiento.cfd_rfc ?? (object)DBNull.Value;
                        parametros.Add("@cfd_tipocambio", OdbcType.Numeric).Value = movimiento.cfd_tipocambio ?? (object)DBNull.Value;
                        parametros.Add("@cfd_total", OdbcType.Numeric).Value = movimiento.cfd_total ?? (object)DBNull.Value;
                        parametros.Add("@cfd_moneda", OdbcType.Char).Value = movimiento.cfd_moneda ?? (object)DBNull.Value;
                        parametros.Add("@metodo_pago_sat", OdbcType.Numeric).Value = movimiento.metodo_pago_sat ?? (object)DBNull.Value;
                        parametros.Add("@ruta_comp_ext", OdbcType.Char).Value = movimiento.ruta_comp_ext ?? (object)DBNull.Value;
                        parametros.Add("@factura_comp_ext", OdbcType.Char).Value = movimiento.factura_comp_ext ?? (object)DBNull.Value;
                        parametros.Add("@taxid", OdbcType.Char).Value = movimiento.taxid ?? (object)DBNull.Value;
                        parametros.Add("@forma_pago", OdbcType.VarChar).Value = movimiento.forma_pago ?? (object)DBNull.Value;
                        parametros.Add("@cfd_fecha_expedicion", OdbcType.SmallDateTime).Value = movimiento.cfd_fecha_expedicion ?? (object)DBNull.Value;
                        parametros.Add("@cfd_tipocomprobante", OdbcType.VarChar).Value = movimiento.cfd_tipocomprobante ?? (object)DBNull.Value;
                        parametros.Add("@cfd_metodo_pago_sat", OdbcType.VarChar).Value = movimiento.cfd_metodo_pago_sat ?? (object)DBNull.Value;
                        parametros.Add("@area", OdbcType.Numeric).Value = movimiento.area ?? (object)DBNull.Value;
                        parametros.Add("@cuenta_oc", OdbcType.Numeric).Value = movimiento.cuenta_oc ?? (object)DBNull.Value;

                        cmd.Connection = _transaccion.Connection;
                        cmd.Transaction = _transaccion;
                        cmd.ExecuteNonQuery();
                    }
                }

                return numeroPoliza;
            }
            catch (Exception e)
            {
                
                throw e;
            }
            
        }

        public int GetNumeroPolizaNueva(int year, int mes, string tp)
        {
            var query_sc_polizas = new OdbcConsultaDTO();

            query_sc_polizas.consulta =
                @"SELECT TOP 1
                    poliza
                FROM
                    sc_polizas
                WHERE
                    year = ? AND
                    mes = ? AND
                    tp = ?
                ORDER BY
                    poliza DESC";

            query_sc_polizas.parametros.Add(new OdbcParameterDTO
            {
                nombre = "year",
                tipo = OdbcType.Int,
                valor = year
            });
            query_sc_polizas.parametros.Add(new OdbcParameterDTO
            {
                nombre = "mes",
                tipo = OdbcType.Int,
                valor = mes
            });
            query_sc_polizas.parametros.Add(new OdbcParameterDTO
            {
                nombre = "tp",
                tipo = OdbcType.Char,
                valor = tp
            });

            return _contextEnkontrol.Select<sc_polizasDTO>(vSesiones.sesionAmbienteEnkontrolAdm, query_sc_polizas).Select(m => m.poliza).FirstOrDefault() + 1;
        }

        #endregion

        #region MOVCLTES
        public Dictionary<string, object> GuardarMovCltes(tblF_RepPrefactura obj, List<tblF_CapPrefactura> lst, List<tblF_CapImporte> lstImpuesto, int idSPPedido, int idSPRemision, int idSPFactura, int idSPPoliza, int idSPCfdFolio)
        {
            resultado.Clear();

            
            try
            {
                var metodoPago = _context.tblF_EK_MetodoPagoSat.FirstOrDefault(e => e.esActivo && e.id == obj.MetodoPagoSAT.Value);
                string descMetodoPago = "";

                if (metodoPago != null)
                {
                    descMetodoPago = metodoPago.descripcion;
                }

                #region SP
                _context.tblF_EK_Movcltes.Add(new tblF_EK_Movcltes 
                {
                    numcte = obj.numcte.Value,
                    factura = idSPFactura,
                    fecha = DateTime.Now,
                    tm = obj.TM.Value,
                    fechavenc = DateTime.Now.AddDays(obj.CondicionesPago.Value),//????????
                    concepto = lst[0].Unidad, //e.UNIDAD == CONCEPTO ↔ e.CONCEPTO == UNIDAD
                    cc = obj.CC,
                    referenciaoc = obj.ReqOC.ToString(),
                    monto = Convert.ToDecimal(lstImpuesto[0].Valor.Substring(1, lstImpuesto[0].Valor.Length - 1)),
                    tipocambio = obj.TipoCambio.Value,
                    iva = Convert.ToDecimal(lstImpuesto[1].Valor.Substring(1, lstImpuesto[1].Valor.Length - 1)),
                    year = DateTime.Now.Year,
                    mes = DateTime.Now.Month,
                    poliza = idSPPoliza,
                    tp = "08",
                    linea = 1,
                    generado = "F",
                    es_factura = "S",
                    moneda = obj.TipoMoneda == "MX" ? "1" : "2",
                    autorizapago = "",
                    total = Convert.ToDecimal(lstImpuesto[2].Valor.Substring(1, lstImpuesto[2].Valor.Length - 1)),
                    bit_escrituracion = "N",
                    poliza_generada = "S",
                    acumula_ingresos = null,
                    tipo_clase = null,
                    num_factura_moratorio = null,
                    origen = null,
                    porcent_iva = null,
                    folio_imp = null,
                    linea_imp = null,
                    bit_orden_escr = null,
                    descuentos = null,
                    cfd_serie = "A",
                    cfd_folio = idSPCfdFolio,
                    cfd_fecha = DateTime.Now,
                    cfd_certificado = "00001000000413753782",
                    cfd_ano_aprob = 2011,
                    cfd_num_aprob = 1,
                    cfd_cadena_original = string.Format(
                        @"||3.2|{0}|ingreso|{1}|{2}|0.00|{3}|{4}|HERMOSILLO, Son|NO IDENTIFICADO|GCP800324FJ1|GRUPO CONSTRUCCIONES PLANIFICADAS SA DE CV|PERIFERICO|770|EMILIANO ZAPATA|HERMOSILLO|HERMOSILLO|Son|MEXICO|83280"
                        , DateTime.Now.ToString("s"), descMetodoPago, obj.CondicionesPago, Convert.ToDecimal(lstImpuesto[1].Valor.Substring(1, lstImpuesto[1].Valor.Length - 1)), obj.TipoMoneda == "MX" ? "MN" : "19|USD", obj.MetodoPago, obj.RFC
                    ),
                    cfd_certificado_sello = "",
                    cfd_sello_digital = "MIIGfDCCBGSgAwIBAgIUMDAwMDEwMDAwMDA0MTM3NTM3ODIwDQYJKoZIhvcNAQELBQAwggGyMTgwNgYDVQQDDC9BLkMuIGRlbCBTZXJ2aWNpbyBkZSBBZG1pbmlzdHJhY2nDs24gVHJpYnV0YXJpYTEvMC0GA1UECgwmU2VydmljaW8gZGUgQWRtaW5pc3RyYWNpw7NuIFRyaWJ1dGFyaWExODA2BgNVBAsML0FkbWluaXN0cmFjacOzbiBkZSBT",
                    fecha_cancelacion = null,
                    usuario_cancela = null,
                    total_canc = null,
                    iva_canc = null,
                    monto_canc = null,
                    cfd_forma_pago = null,
                    cfd_cond_pago = null,
                    cfd_porc_iva = null,
                    cfd_ruta_pdf = "",
                    cfd_ruta_xml = "",
                    cfd_no_certificado = "00001000000413753782",
                    cfd_monto = null,
                    cfd_iva = null,
                    cfd_total = null,
                    UUID = null,
                    cfd_mn = "S",
                    cfd_enviada = null,
                    cfd_regimen_fiscal = "1",
                    cfd_sucursal = "1",
                    cfd_metodo_pago = obj.MetodoPagoSAT.ToString(),
                    cfd_num_cta_pago = "0",
                    bit_cfdi = "0",
                    fecha_timbrado = null,
                    certificado_sat = null,
                    sello_sat = null,
                    comentarios_sat = "Documento procesado sin timbrado, Solo CFDI",
                    refid = null,
                    insumo = null,
                    folio_canc_stf = null,
                    numregidtrib = null,
                    fecha_registro = DateTime.Now,
                    concepto_CFDI = null,
                    observaciones_cfdi = null,
                    bit_stf = 0,
                    folio_parcial = null,
                    copia_xml = null,
                    copia_xml_pdf = null,
                    rfcemisorctaord = null,
                    RfcEmisorCtaBen = null,
                    nombancoordext = null,
                    CtaOrdenante = null,
                    CtaBeneficiario = null,
                    impsaldoant = null,
                    imppagado = null,
                    impsaldoinsoluto = null,
                    reg_fiscal_sat = null,
                    cfd_tiporelacion = null,
                    tipocadpago = null,
                    condicionespago = null,
                    usocfdi = null,
                    cfd_metodo_pago_sat = null,
                    cfd_forma_pago_sat = null,
                    cfd_condicionespago = null,
                    cfd_version = "32",
                    sello_digital_pago = null,
                    cadena_original_pago = null,
                    no_parcialidades = 0,
                    parcialidades = 0,
                    cia_global = null,
                    anticipo = "N",
                    pago_anticipo = "N",
                    cancelacion_anticipo = "N",
                    ClaveProdServ = null,
                    claveunidadsat = null,
                    insumo_EK = null,
                    desglosa_iva_anticipo = "N",
                    relacionado_escrituracion = "N",
                    numcte_rel_canc = null,
                    factura_rel_canc = null,
                    tm_rel_canc = null,
                    fecha_rel_canc = null,
                    fecha_registro_rel_canc = null,
                    numcte_rel = null,
                    factura_rel = null,
                    tm_rel = null,
                    fecha_rel = null,
                    fecha_registro_rel = null,
                    descuenta_anticipo = "N",
                    folio_canc_anticipo = null,
                    cfd_concepto_egreso = null,
                    pago_cfdi_escrituracion = "N",
                    cfdirel = 0,
                    tmcfdirel = 0,
                    cfd_predial_enc = null,
                    id_complemento = 0,
                    tipoagrupacion = null,
                    referenceidentificacion = null,
                    ordencompra_addenda = null,
                    determinante_addenda = null,
                    pedido = null,
                    contrarecibo = null,
                    idpac = null,
                    formato_addenda = null,
                    nomunidad_addenda = null,
                    noproyecto_addenda = null,
                    foliorecibo_addenda = null,
                    fecha_pago_Real = null,
                    iddocumento = null,
                    NumOperacion = null,
                    Bancoext = "",
                    CertPago = null,
                    CadPago = null,
                    SelloPago = null,
                    tipocambio_pago = null,
                    moneda_pago = null,
                    monto_pago = null,
                    activa_factoraje = "N",
                    id_factoraje = 0,
                    cfd_estatus_cancelacion = "",
                    cfd_estado = "",
                    cfd_movto_cancela = 0,
                    cfd_tipocomprobante = "",
                    cfd_cancelado = 0,
                });
                _context.SaveChanges();
                #endregion

                #region EK



                var query_sc_polizas =
                    @"INSERT INTO sx_movcltes
                        (
                            numcte,factura,fecha,tm,fechavenc,concepto,cc,referenciaoc,monto,tipocambio,iva,year,mes,poliza,tp,linea,generado,es_factura,moneda,autorizapago,
                            total,bit_escrituracion,poliza_generada,acumula_ingresos,tipo_clase,num_factura_moratorio,origen,porcent_iva,folio_imp,linea_imp,bit_orden_escr,
                            descuentos,cfd_serie,cfd_folio,cfd_fecha,cfd_certificado,cfd_ano_aprob,cfd_num_aprob,cfd_cadena_original,cfd_certificado_sello,cfd_sello_digital,
                            fecha_cancelacion,usuario_cancela,total_canc,iva_canc,monto_canc,cfd_forma_pago,cfd_cond_pago,cfd_porc_iva,cfd_ruta_pdf,cfd_ruta_xml,cfd_no_certificado,
                            cfd_monto,cfd_iva,cfd_total,UUID,cfd_mn,cfd_enviada,cfd_regimen_fiscal,cfd_sucursal,cfd_metodo_pago,cfd_num_cta_pago,bit_cfdi,fecha_timbrado,certificado_sat,sello_sat,
                            comentarios_cfdi,refid,insumo,folio_canc_stf,numregidtrib,fecha_registro,concepto_CFDI,observaciones_cfdi,bit_stf,folio_parcial,copia_xml,copia_xml_pdf,
                            rfcemisorctaord,RfcEmisorCtaBen,nombancoordext,CtaOrdenante,CtaBeneficiario,impsaldoant,imppagado,impsaldoinsoluto,reg_fiscal_sat,cfd_tiporelacion,tipocadpago,
                            condicionespago,usocfdi,cfd_metodo_pago_sat,cfd_forma_pago_sat,cfd_condicionespago,cfd_version,sello_digital_pago,cadena_original_pago,no_parcialidades,
                            parcialidades,cia_global,anticipo,pago_anticipo,cancelacion_anticipo,ClaveProdServ,claveunidadsat,insumo_EK,desglosa_iva_anticipo,relacionado_escrituracion,
                            numcte_rel_canc,factura_rel_canc,tm_rel_canc,fecha_rel_canc	,fecha_registro_rel_canc,numcte_rel,factura_rel,tm_rel,fecha_rel,fecha_registro_rel,
                            descuenta_anticipo,folio_canc_anticipo,cfd_concepto_egreso,pago_cfdi_escrituracion,cfdirel,tmcfdirel,cfd_predial_enc,tipoagrupacion,id_complemento,referenceidentificacion,
                            ordencompra_addenda,determinante_addenda,pedido,contrarecibo,idpac,formato_addenda,nomunidad_addenda,noproyecto_addenda,foliorecibo_addenda,fecha_pago_Real,
                            iddocumento,NumOperacion,Bancoext,CertPago,CadPago,SelloPago,tipocambio_pago,moneda_pago,monto_pago,activa_factoraje,id_factoraje,cfd_estatus_cancelacion,cfd_estado,
                            cfd_movto_cancela,cfd_tipocomprobante,cfd_cancelado
                        )
                    VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,
                            ?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                using (var cmd = new OdbcCommand(query_sc_polizas))
                {
                    var parametros = cmd.Parameters;

                    parametros.Add("@numcte", OdbcType.Numeric).Value = obj.numcte;
                    parametros.Add("@factura", OdbcType.Numeric).Value = idSPFactura;
                    parametros.Add("@fecha", OdbcType.DateTime).Value = DateTime.Now;
                    parametros.Add("@tm", OdbcType.Numeric).Value = obj.TM;
                    parametros.Add("@fechavenc", OdbcType.DateTime).Value = DateTime.Now.AddDays(obj.CondicionesPago.Value);//????????
                    parametros.Add("@concepto", OdbcType.Char).Value = lst[0].Unidad; //e.UNIDAD == CONCEPTO ↔ e.CONCEPTO == UNIDAD
                    parametros.Add("@cc", OdbcType.Char).Value = obj.CC;
                    parametros.Add("@referenciaoc", OdbcType.Numeric).Value = obj.ReqOC;
                    parametros.Add("@monto", OdbcType.Numeric).Value = Convert.ToDecimal(lstImpuesto[0].Valor.Substring(1, lstImpuesto[0].Valor.Length - 1));
                    parametros.Add("@tipocambio", OdbcType.Numeric).Value = obj.TipoCambio;
                    parametros.Add("@iva", OdbcType.Numeric).Value = Convert.ToDecimal(lstImpuesto[1].Valor.Substring(1, lstImpuesto[1].Valor.Length - 1));
                    parametros.Add("@year", OdbcType.Numeric).Value = DateTime.Now.Year;
                    parametros.Add("@mes", OdbcType.Numeric).Value = DateTime.Now.Month;
                    parametros.Add("@poliza", OdbcType.Numeric).Value = idSPPoliza;
                    parametros.Add("@tp", OdbcType.Char).Value = "08";
                    parametros.Add("@linea", OdbcType.Numeric).Value = 1;
                    parametros.Add("@generado", OdbcType.Char).Value = "F";
                    parametros.Add("@es_factura", OdbcType.Char).Value = "S";
                    parametros.Add("@moneda", OdbcType.Numeric).Value = obj.TipoMoneda == "MX" ? 1 : 2;
                    parametros.Add("@autorizapago", OdbcType.Char).Value = "";
                    parametros.Add("@total", OdbcType.Numeric).Value = Convert.ToDecimal(lstImpuesto[2].Valor.Substring(1, lstImpuesto[2].Valor.Length - 1));
                    parametros.Add("@bit_escrituracion", OdbcType.Char).Value = "N";
                    parametros.Add("@poliza_generada", OdbcType.Char).Value = "S";
                    parametros.Add("@acumula_ingresos", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@tipo_clase", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@num_factura_moratorio", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@origen", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@porcent_iva", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@folio_imp", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@linea_imp", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@bit_orden_escr", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@descuentos", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@cfd_serie", OdbcType.Char).Value = "A";
                    parametros.Add("@cfd_folio", OdbcType.Numeric).Value = idSPCfdFolio;
                    parametros.Add("@cfd_fecha", OdbcType.DateTime).Value = DateTime.Now;
                    parametros.Add("@cfd_certificado", OdbcType.Char).Value = "00001000000413753782";
                    parametros.Add("@cfd_ano_aprob", OdbcType.Numeric).Value = 2011;
                    parametros.Add("@cfd_num_aprob", OdbcType.Numeric).Value = 1;
                    parametros.Add("@cfd_cadena_original", OdbcType.Char).Value = string.Format(
                        @"||3.2|{0}|ingreso|{1}|{2}|0.00|{3}|{4}|HERMOSILLO, Son|NO IDENTIFICADO|GCP800324FJ1|GRUPO CONSTRUCCIONES PLANIFICADAS SA DE CV|PERIFERICO|770|EMILIANO ZAPATA|HERMOSILLO|HERMOSILLO|Son|MEXICO|83280"
                        , DateTime.Now.ToString("s"), descMetodoPago, obj.CondicionesPago, Convert.ToDecimal(lstImpuesto[1].Valor.Substring(1, lstImpuesto[1].Valor.Length - 1)), obj.TipoMoneda == "MX" ? "MN" : "19|USD", obj.MetodoPago, obj.RFC
                    );
                    parametros.Add("@cfd_certificado_sello", OdbcType.Char).Value = "";
                    parametros.Add("@cfd_sello_digital", OdbcType.Char).Value = "MIIGfDCCBGSgAwIBAgIUMDAwMDEwMDAwMDA0MTM3NTM3ODIwDQYJKoZIhvcNAQELBQAwggGyMTgwNgYDVQQDDC9BLkMuIGRlbCBTZXJ2aWNpbyBkZSBBZG1pbmlzdHJhY2nDs24gVHJpYnV0YXJpYTEvMC0GA1UECgwmU2VydmljaW8gZGUgQWRtaW5pc3RyYWNpw7NuIFRyaWJ1dGFyaWExODA2BgNVBAsML0FkbWluaXN0cmFjacOzbiBkZSBT";
                    parametros.Add("@fecha_cancelacion", OdbcType.DateTime).Value = DBNull.Value;
                    parametros.Add("@usuario_cancela", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@total_canc", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@iva_canc", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@monto_canc", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@cfd_forma_pago", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@cfd_cond_pago", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@cfd_porc_iva", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@cfd_ruta_pdf", OdbcType.Char).Value = "";
                    parametros.Add("@cfd_ruta_xml", OdbcType.Char).Value = "";
                    parametros.Add("@cfd_no_certificado", OdbcType.Char).Value = "00001000000413753782";
                    parametros.Add("@cfd_monto", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@cfd_iva", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@cfd_total", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@UUID", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@cfd_mn", OdbcType.Char).Value = "S";
                    parametros.Add("@cfd_enviada", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@cfd_regimen_fiscal", OdbcType.Numeric).Value = 1;
                    parametros.Add("@cfd_sucursal", OdbcType.Numeric).Value = 1;
                    parametros.Add("@cfd_metodo_pago", OdbcType.Numeric).Value = obj.MetodoPagoSAT;;
                    parametros.Add("@cfd_num_cta_pago", OdbcType.Char).Value = "0";
                    parametros.Add("@bit_cfdi", OdbcType.Numeric).Value = 0;
                    parametros.Add("@fecha_timbrado", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@certificado_sat", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@sello_sat", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@comentarios_cfdi", OdbcType.Char).Value = "Documento procesado sin timbrado, Solo CFDI";
                    parametros.Add("@refid", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@insumo", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@folio_canc_stf", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@numregidtrib", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@fecha_registro", OdbcType.DateTime).Value = DateTime.Now;
                    parametros.Add("@concepto_CFDI", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@observaciones_cfdi", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@bit_stf", OdbcType.Numeric).Value = 0;
                    parametros.Add("@folio_parcial", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@copia_xml", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@copia_xml_pdf", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@rfcemisorctaord", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@RfcEmisorCtaBen", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@nombancoordext", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@CtaOrdenante", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@CtaBeneficiario", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@impsaldoant", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@imppagado", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@impsaldoinsoluto", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@reg_fiscal_sat", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@cfd_tiporelacion", OdbcType.Char).Value = "";
                    parametros.Add("@tipocadpago", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@condicionespago", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@usocfdi", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@cfd_metodo_pago_sat", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@cfd_forma_pago_sat", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@cfd_condicionespago", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@cfd_version", OdbcType.Char).Value = "32";
                    parametros.Add("@sello_digital_pago", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@cadena_original_pago", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@no_parcialidades", OdbcType.Numeric).Value = 0;
                    parametros.Add("@parcialidades", OdbcType.Numeric).Value = 0;
                    parametros.Add("@cia_global", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@anticipo", OdbcType.Char).Value = "N";
                    parametros.Add("@pago_anticipo", OdbcType.Char).Value = "N";
                    parametros.Add("@cancelacion_anticipo", OdbcType.Char).Value = "N";
                    parametros.Add("@ClaveProdServ", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@claveunidadsat", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@insumo_EK", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@desglosa_iva_anticipo", OdbcType.Char).Value = "N";
                    parametros.Add("@relacionado_escrituracion", OdbcType.Char).Value = "N";
                    parametros.Add("@numcte_rel_canc", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@factura_rel_canc", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@tm_rel_canc", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@fecha_rel_canc", OdbcType.DateTime).Value = DBNull.Value;
                    parametros.Add("@fecha_registro_rel_canc", OdbcType.DateTime).Value = DBNull.Value;
                    parametros.Add("@numcte_rel", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@factura_rel", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@tm_rel", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@fecha_rel", OdbcType.DateTime).Value = DBNull.Value;
                    parametros.Add("@fecha_registro_rel", OdbcType.DateTime).Value = DBNull.Value;
                    parametros.Add("@descuenta_anticipo", OdbcType.Char).Value = "N";
                    parametros.Add("@folio_canc_anticipo", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@cfd_concepto_egreso", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@pago_cfdi_escrituracion", OdbcType.Char).Value = "N";
                    parametros.Add("@cfdirel", OdbcType.Numeric).Value = 0;
                    parametros.Add("@tmcfdirel", OdbcType.Numeric).Value = 0;
                    parametros.Add("@cfd_predial_enc", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@id_complemento", OdbcType.Numeric).Value = 0;
                    parametros.Add("@tipoagrupacion", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@referenceidentificacion", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@ordencompra_addenda", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@determinante_addenda", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@pedido", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@contrarecibo", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@idpac", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@formato_addenda", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@nomunidad_addenda", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@noproyecto_addenda", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@foliorecibo_addenda", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@fecha_pago_Real", OdbcType.DateTime).Value = DBNull.Value;
                    parametros.Add("@iddocumento", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@NumOperacion", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@Bancoext", OdbcType.Char).Value = "";
                    parametros.Add("@CertPago", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@CadPago", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@SelloPago", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@tipocambio_pago", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@moneda_pago", OdbcType.Char).Value = DBNull.Value;
                    parametros.Add("@monto_pago", OdbcType.Numeric).Value = DBNull.Value;
                    parametros.Add("@activa_factoraje", OdbcType.Char).Value = "N";
                    parametros.Add("@id_factoraje", OdbcType.Numeric).Value = 0;
                    parametros.Add("@cfd_estatus_cancelacion", OdbcType.Char).Value = "";
                    parametros.Add("@cfd_estado", OdbcType.Char).Value = "";
                    parametros.Add("@cfd_movto_cancela", OdbcType.Numeric).Value = 0;
                    parametros.Add("@cfd_tipocomprobante", OdbcType.Char).Value = "";
                    parametros.Add("@cfd_cancelado", OdbcType.Numeric).Value = 0;

                    cmd.Connection = _transaccion.Connection;
                    cmd.Transaction = _transaccion;
                    cmd.ExecuteNonQuery();
                }
                #endregion

            }
            catch (Exception e)
            {
                
                throw e;
            }

            return resultado;
        }
        #endregion

        #region CONSECUTIVO FACTURAS CFD
        public Dictionary<string, object> UpdateConsecutivoFacturaCFD(tblF_RepPrefactura obj, List<tblF_CapPrefactura> lst, List<tblF_CapImporte> lstImpuesto, int idFacturaSP, int usuarioEK, string descMetodoPago)
        {
            resultado.Clear();

            try
            {
                //int idUltimaFactura = 0;

                string companiaEK = "";
                switch ((int)(MainContextEnum)vSesiones.sesionEmpresaActual)
                {
                    case 1:
                        if (vSesiones.sesionAmbienteEnkontrolAdm == Core.Enum.Multiempresa.EnkontrolAmbienteEnum.Prod)
                        {
                            companiaEK = "01";
                        }
                        else
                        {
                            companiaEK = "98";

                        }

                        break;
                    case 2:
                        if (vSesiones.sesionAmbienteEnkontrolAdm == Core.Enum.Multiempresa.EnkontrolAmbienteEnum.Prod)
                        {
                            companiaEK = "04";
                        }
                        else
                        {
                            companiaEK = "99";

                        }

                        break;
                    default:

                        companiaEK = "98";
                        break;
                }

                //var ultimaFactura = _contextEnkontrol.Select<dynamic>(vSesiones.sesionAmbienteEnkontrolAdm, new OdbcConsultaDTO()
                //{
                //    consulta = string.Format(@"SELECT TOP 1 factura FROM sf_cfd WHERE numcia = '{0}' ORDER BY factura DESC", companiaEK)
                    
                //}).ToList();
                //idUltimaFactura = Convert.ToInt32(ultimaFactura.FirstOrDefault().factura) + 1;

                var count = 0;
                var consulta = @"UPDATE sf_cfd 
                                SET
                                    numcte = ?,
                                    factura = ?,
                                    fecha = ?,
                                    cadena_original = ?
                                WHERE usuario = 10";

                using (var cmd = new OdbcCommand(consulta))
                {
                    OdbcParameterCollection parameters = cmd.Parameters;


                    //parameters.Add("@numcia", OdbcType.Char).Value = companiaEK;
                    //parameters.Add("@cia_sucursal", OdbcType.Numeric).Value = 1;
                    parameters.Add("@numcte", OdbcType.Numeric).Value = obj.numcte;
                    parameters.Add("@factura", OdbcType.Numeric).Value = idFacturaSP;
                    //parameters.Add("@numero_nc", OdbcType.Numeric).Value = 0;
                    parameters.Add("@fecha", OdbcType.DateTime).Value = DateTime.Now;
                    //parameters.Add("@tm", OdbcType.Numeric).Value = 1;
                    //parameters.Add("@usuario", OdbcType.Numeric).Value = usuarioEK;
                    parameters.Add("@cadena_original", OdbcType.Char).Value = string.Format(
                        @"||3.2|{0}|ingreso|{1}|{2}|0.00|{3}|{4}|HERMOSILLO, Son|NO IDENTIFICADO|GCP800324FJ1|GRUPO CONSTRUCCIONES PLANIFICADAS SA DE CV|PERIFERICO|770|EMILIANO ZAPATA|HERMOSILLO|HERMOSILLO|Son|MEXICO|83280"
                        , DateTime.Now.ToString("s"), descMetodoPago, obj.CondicionesPago, Convert.ToDecimal(lstImpuesto[1].Valor.Substring(1, lstImpuesto[1].Valor.Length - 1)), obj.TipoMoneda == "MX" ? "MN" : "19|USD", obj.MetodoPago, obj.RFC
                    );
                    //parameters.Add("@ruta_key", OdbcType.Char).Value = "C:\\sellosn\\CSD_GRUPO_CONSTRUCCIONES_PLANIFICADAS_SA_DE_CV_GCP800324FJ1_20190305_123217.key";
                    //parameters.Add("@ruta_certificado", OdbcType.Char).Value = "C:\\sellosn\\CSD_GRUPO_CONSTRUCCIONES_PLANIFICADAS_SA_DE_CV_GCP800324FJ1_20190305_123217s.cer";
                    //parameters.Add("@clave_privada", OdbcType.Char).Value = "Construplan.1920";
                    //parameters.Add("@sello_digital", OdbcType.Char).Value = "";
                    //parameters.Add("@certificado", OdbcType.Char).Value = "MIIGfDCCBGSgAwIBAgIUMDAwMDEwMDAwMDA0MTM3NTM3ODIwDQYJKoZIhvcNAQELBQAwggGyMTgwNgYDVQQDDC9BLkMuIGRlbCBTZXJ2aWNpbyBkZSBBZG1pbmlzdHJhY2nDs24gVHJpYnV0YXJpYTEvMC0GA1UECgwmU2VydmljaW8gZGUgQWRtaW5pc3RyYWNpw7NuIFRyaWJ1dGFyaWExODA2BgNVBAsML0FkbWluaXN0cmFjacOzbiBkZSBT";

                    cmd.Connection = _transaccion.Connection;
                    cmd.Transaction = _transaccion;

                    count += cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                
                throw;
            }

            return resultado;
        }
        #endregion

        #region CONSECUTIVO FACTURAS SUCURSAL
        public void UpdateConsecutivoFacturaSucursal(int ultimaFactura)
        {

            try
            {

                var count = 0;
                var consulta = @"UPDATE cia_sucursales
                                SET folio_factura = ?
                                WHERE sucursal = 1";

                using (var cmd = new OdbcCommand(consulta))
                {
                    OdbcParameterCollection parameters = cmd.Parameters;


                    parameters.Add("@folio_factura", OdbcType.Numeric).Value = ultimaFactura;

                    cmd.Connection = _transaccion.Connection;
                    cmd.Transaction = _transaccion;

                    count += cmd.ExecuteNonQuery();
                }

                //return ultimoCfd;
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }
        #endregion
    }
}
