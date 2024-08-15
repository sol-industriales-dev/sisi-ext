using Core.DAO.Enkontrol.General.CC;
using Core.DAO.Facturacion.Enkontrol;
using Core.Entity.Administrativo.Contabilidad.Facturas;
using Core.Entity.Facturacion.Prefacturacion;
using Data.EntityFramework.Generic;
using Data.Factory.Enkontrol.General.CC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO;
using System.Data.Odbc;
using Data.Factory.Facturacion.Prefacturacion;
using Core.Entity.Administrativo.Contabilidad.Poliza;
using Core.DTO.Enkontrol.Tablas.Poliza;
using Core.DTO.Utils.Data;

namespace Data.DAO.Facturacion.Enkontrol
{
    public class FacturasSPDAO : GenericDAO<tblF_EK_Facturas>, IFacturasSPDAO
    {
        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        private string NombreControlador = "FacturasSPController";
        private bool productivo = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["enkontrolProductivo"]) == "0";
        PrefacturacionFactoryServices PrefacturacionFactoryServices;
        RepPrefacturacionFactoryService RepPrefacturacionFactoryService;
        CapImporteFactoryService CapImporteFactoryService;

        ICCDAO _ccFS_SP = new CCFactoryService().getCCServiceSP();

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
        public FacturasSPDAO()
        {
            resultado.Clear();

            PrefacturacionFactoryServices = new PrefacturacionFactoryServices();
            RepPrefacturacionFactoryService = new RepPrefacturacionFactoryService();
            CapImporteFactoryService = new CapImporteFactoryService();

#if DEBUG
            RutaServidor = @"C:\Proyectos\SIGOPLANv2\FACTURAS";
#else
            RutaServidor = @"\\10.1.0.112\Proyecto\SIGOPLAN\FACTURAS";
#endif
        }
        #endregion

        #region GENERALES
        public Dictionary<string, object> GuardarPrefactura(tblF_RepPrefactura obj, List<tblF_CapPrefactura> lst, List<tblF_CapImporte> lstImpuesto)
        {
            resultado.Clear();

            using (var dbTransac = _context.Database.BeginTransaction())
            {
                try
                {
                    GuardarPedidos(obj, lst, lstImpuesto, 0);
                    GuardarRemision(obj, lst, lstImpuesto, 0, 0);
                    GuardarFactura(obj, lst, lstImpuesto, 0, 0, 0);

                    var reporte = RepPrefacturacionFactoryService.getRepPrefacturacionService().saveRepPrefactura(obj);
                    foreach (var item in lst)
                    {
                        item.idRepPrefactura = reporte.id;
                        var guardado = PrefacturacionFactoryServices.getPrefacturacionServices().savePrefactura(item);
                    }
                    foreach (var item in lstImpuesto)
                    {
                        item.idReporte = reporte.id;
                        var guardado = CapImporteFactoryService.getCapImporteFactoryService().saveRepPrefactura(item);
                    }

                    resultado.Clear();

                    dbTransac.Commit();

                    resultado.Add(ITEMS, reporte);
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbTransac.Rollback();

                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                    //throw e;
                }
            }


            return resultado;
        }

        #endregion
        

        #region PEDIDOS
        public Dictionary<string, object> GuardarPedidos(tblF_RepPrefactura obj, List<tblF_CapPrefactura> lst, List<tblF_CapImporte> lstImpuesto, int idSPPedido)
        {
            resultado.Clear();

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

                if (obj.id > 0)
                {
                    #region EDITAR
                    var objPedido = _context.tblF_EK_Pedidos.FirstOrDefault(e => e.esActivo && e.pedido == idSPPedido);

                    objPedido.fecha = DateTime.Now;
                    objPedido.requisicion = obj.ReqOC ?? 0;
                    objPedido.vendedor = idUsrEnkontrol;
                    objPedido.cond_pago = objPedido.cond_pago;
                    objPedido.moneda = obj.TipoMoneda == "MX" ? 1 : 2;
                    objPedido.tipo_cambio = 0; //PENDING
                    objPedido.sub_total = Convert.ToDecimal(lstImpuesto[1].Valor.Substring(1, lstImpuesto[1].Valor.Length - 1));
                    objPedido.iva = Convert.ToDecimal(lstImpuesto[2].Valor.Substring(1, lstImpuesto[2].Valor.Length - 1));
                    objPedido.total = Convert.ToDecimal(lstImpuesto[0].Valor.Substring(1, lstImpuesto[0].Valor.Length - 1));
                    objPedido.porcent_iva = 0;
                    //objPedido.estatus = 
                    objPedido.condicion_entrega = obj.CondEntrega;
                    objPedido.tipo_flete = obj.TipoFlete;
                    objPedido.obs = obj.Observaciones;
                    objPedido.fecha_hora = DateTime.Now;
                    objPedido.tipo_pedido = obj.TipoPedido; 
                    objPedido.cc = obj.CC;
                    objPedido.tm = obj.TM;
                    objPedido.elaboro = idUsrEnkontrol;
                    objPedido.fechaModificacion = DateTime.Now;
                    objPedido.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;

                    _context.SaveChanges();

                    foreach (var item in lst)
                    {
                        var objPartida = _context.tblF_EK_Pedido_Det.FirstOrDefault(e => e.esActivo && e.pedido == idSPPedido && e.partida == item.Renglon);

                        objPartida.insumo = item.TipoInsumo.Value;
                        objPartida.cantidad = item.Cantidad;
                        objPartida.precio = item.Precio;
                        objPartida.importe = item.Importe;
                        objPartida.unidad = item.Concepto;
                        objPartida.usuario = idUsrEnkontrol;
                        objPartida.fecha_hora = DateTime.Now;
                        objPartida.linea = item.Unidad;
                        objPedido.fechaModificacion = DateTime.Now;
                        objPedido.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                        _context.SaveChanges();

                    }

                    #endregion
                }
                else
                {
                    #region CREAR
                    _context.tblF_EK_Pedidos.Add(new tblF_EK_Pedidos
                    {
                        folioPrefactura = obj.Folio,
                        pedido = idSPPedido,
                        numcte = obj.numcte.Value,
                        sucursal = 1,
                        fecha = DateTime.Now,
                        requisicion = obj.ReqOC.Value,
                        vendedor = idUsrEnkontrol,
                        cond_pago = obj.CondicionesPago.Value,
                        moneda = obj.TipoMoneda == "MX" ? 1 : 2,
                        tipo_cambio = 0M,
                        sub_total = Convert.ToDecimal(lstImpuesto[0].Valor.Substring(1, lstImpuesto[0].Valor.Length - 1)),
                        iva = Convert.ToDecimal(lstImpuesto[1].Valor.Substring(1, lstImpuesto[1].Valor.Length - 1)),
                        total = Convert.ToDecimal(lstImpuesto[2].Valor.Substring(1, lstImpuesto[2].Valor.Length - 1)),
                        porcent_iva = 16M,
                        tipo = "N", //NACIONAL | EXTRANGERO
                        porcent_descto = 0M,
                        descuento = 0M,
                        estatus = "N",
                        condicion_entrega = obj.CondEntrega,
                        tipo_flete = obj.TipoFlete,
                        lista_precios = 0,
                        status_autorizado = "P",
                        zona = 1,
                        obs = obj.Observaciones,
                        usuario = idUsrEnkontrol,
                        fecha_hora = DateTime.Now,
                        tipo_pedido = obj.TipoPedido,
                        cc = obj.CC,
                        tm = obj.TM,
                        elaboro = idUsrEnkontrol,
                        cia_sucursal = 1,
                        tipo_credito = "C",
                        retencion = 0M,
                        aplica_total_antes_iva = 0M,
                        total_dec = 0M,
                        iva_partida = "N",
                        fechaCreacion = DateTime.Now,
                        fechaModificacion = DateTime.Now,
                        idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id,
                        idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id,
                        esActivo = true,
                    });
                    _context.SaveChanges();

                    foreach (var item in lst)
                    {
                        _context.tblF_EK_Pedido_Det.Add(new tblF_EK_Pedidos_Det
                        {
                            folioPrefactura = obj.Folio,
                            pedido = idSPPedido,
                            partida = item.Renglon,
                            insumo = item.TipoInsumo.Value,
                            cantidad = item.Cantidad,
                            precio = item.Precio,
                            importe = item.Importe,
                            unidad = item.Concepto,
                            estatus = "T",
                            porcent_descto = 0M,
                            cant_pedido = 0M,
                            precio_pedido = 0M,
                            cant_facturada = 0M,
                            muestra = "N",
                            usuario = idUsrEnkontrol,
                            fec_entrega = DateTime.Now,
                            fecha_hora = DateTime.Now,
                            cant_kg = 0M,
                            cant_kg_cancelada = 0M,
                            cant_kg_facturada = 0M,
                            cant_kg_pedido = 0M,
                            cant_x_embarc = 0M,
                            cant_produccion = 0M,
                            cant_entregada = 0M,
                            cant_remision = 0M,
                            porcen_iva_partida = 0M,
                            linea = item.Unidad,
                            fechaCreacion = DateTime.Now,
                            fechaModificacion = DateTime.Now,
                            idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id,
                            idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id,
                            esActivo = true,
                        });
                        _context.SaveChanges();

                    }
                    #endregion
                }

                //dbTransac.Commit();

                resultado.Add(ITEMS, null);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {

                //dbTransac.Rollback();

                //resultado.Add(MESSAGE, e.Message);
                //resultado.Add(SUCCESS, false);

                throw e;
            }

            

            return resultado;
            
        }
        #endregion

        #region REMISION
        public Dictionary<string, object> GuardarRemision(tblF_RepPrefactura obj, List<tblF_CapPrefactura> lst, List<tblF_CapImporte> lstImpuesto, int idSPPedido, int idSPRemision)
        {
            resultado.Clear();

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

                if (obj.id > 0)
                {
                    #region EDITAR
                    var objRemision = _context.tblF_EK_Remision.FirstOrDefault(e => e.esActivo && e.remision == idSPRemision);

                    objRemision.fecha = DateTime.Now;
                    objRemision.rfc = obj.RFC;
                    objRemision.nombre = obj.Nombre;
                    objRemision.direccion = obj.Direccion;
                    objRemision.ciudad = "";
                    objRemision.cp = obj.CP;
                    objRemision.telefono = 0;
                    objRemision.observaciones = obj.Observaciones;
                    objRemision.moneda = obj.TipoMoneda == "MX" ? 1 : 2;
                    objRemision.tipo_cambio = 1M;
                    objRemision.sub_total = Convert.ToDecimal(lstImpuesto[1].Valor.Substring(1, lstImpuesto[1].Valor.Length - 1));
                    objRemision.iva = Convert.ToDecimal(lstImpuesto[2].Valor.Substring(1, lstImpuesto[2].Valor.Length - 1));
                    objRemision.total = Convert.ToDecimal(lstImpuesto[0].Valor.Substring(1, lstImpuesto[0].Valor.Length - 1));
                    objRemision.porcent_iva = 16M;
                    objRemision.elaboro = idUsrEnkontrol;
                    objRemision.tipo_flete = obj.TipoFlete;
                    objRemision.entregado = obj.Entregado;
                    objRemision.fechaModificacion = DateTime.Now;
                    objRemision.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;

                    _context.SaveChanges();

                    foreach (var item in lst)
                    {
                        var objPartida = _context.tblF_EK_Remision_Det.FirstOrDefault(e => e.esActivo && e.remision == idSPRemision && e.partida == item.Renglon);

                        objPartida.insumo = item.TipoInsumo.Value;
                        objPartida.cantidad = item.Cantidad;
                        objPartida.precio = item.Precio;
                        objPartida.importe = item.Importe;
                        objPartida.unidad = item.Concepto;
                        objPartida.linea = item.Unidad;
                        objPartida.fechaModificacion = DateTime.Now;
                        objPartida.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;

                        _context.SaveChanges();
                    }

                        
                    #endregion
                }
                else
                {
                    #region CREAR
                    _context.tblF_EK_Remision.Add(new tblF_EK_Remision
                    {
                        folioPrefactura = obj.Folio,
                        sucursal = 1,
                        remision = idSPRemision,
                        fecha = DateTime.Now,
                        rfc = obj.RFC,
                        nombre = obj.Nombre,
                        direccion = obj.Direccion,
                        ciudad = "",
                        cp = obj.CP,
                        telefono = 0,
                        transporte = "1",
                        talon = 1,
                        consignado = "1",
                        observaciones = obj.Observaciones,
                        moneda = obj.TipoMoneda == "MX" ? 1 : 2,
                        tipo_cambio = 1M,
                        sub_total = Convert.ToDecimal(lstImpuesto[0].Valor.Substring(1, lstImpuesto[0].Valor.Length - 1)),
                        iva = Convert.ToDecimal(lstImpuesto[1].Valor.Substring(1, lstImpuesto[1].Valor.Length - 1)),
                        total = Convert.ToDecimal(lstImpuesto[2].Valor.Substring(1, lstImpuesto[2].Valor.Length - 1)),
                        porcent_iva = 16M,
                        porcent_descto = 0M,
                        elaboro = idUsrEnkontrol,
                        tipo_flete = obj.TipoFlete,
                        descuento = 0M,
                        factura = 0,
                        estatus = "P",
                        entregado = obj.Entregado,
                        pedido = idSPPedido,
                        retencion = 0,
                        fechaCreacion = DateTime.Now,
                        fechaModificacion = DateTime.Now,
                        idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id,
                        idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id,
                        esActivo = true,
                    });

                    _context.SaveChanges();

                    foreach (var item in lst)
                    {
                        _context.tblF_EK_Remision_Det.Add(new tblF_EK_Remision_Det
                        {
                            folioPrefactura = obj.Folio,
                            sucursal = 1,
                            remision = idSPRemision,
                            partida = item.Renglon,
                            insumo = item.TipoInsumo.Value,
                            cantidad = item.Cantidad,
                            precio = item.Precio,
                            importe = item.Importe,
                            unidad = item.Concepto,
                            cant_remision = 0M,
                            precio_remision = 0M,
                            pedido = idSPPedido,
                            ped_part = 1, //??
                            linea = item.Unidad,
                            fechaCreacion = DateTime.Now,
                            fechaModificacion = DateTime.Now,
                            idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id,
                            idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id,
                            esActivo = true,
                        });
                    }

                    _context.SaveChanges();

                    #endregion
                }
                    
                //dbTransac.Commit();

                resultado.Add(ITEMS, null);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                    
                //dbTransac.Rollback();

                //resultado.Add(MESSAGE, e.Message);
                //resultado.Add(SUCCESS, false);
                throw e;

            }
            
            return resultado;
        }
        #endregion

        #region FACTURA
        public Dictionary<string, object> GuardarFactura(tblF_RepPrefactura obj, List<tblF_CapPrefactura> lst, List<tblF_CapImporte> lstImpuesto, int idSPPedido, int idSPRemision, int idSPFactura)
        {
            resultado.Clear();

            int idUsrEnkontrol = 0;
            var usrEnkontrol = _context.tblP_Usuario_Enkontrol.FirstOrDefault(e => e.idUsuario == vSesiones.sesionUsuarioDTO.id);

            if (usrEnkontrol != null)
            {
                idUsrEnkontrol = usrEnkontrol.empleado;
            }
            else
            {
                throw new Exception("El usuario no tiene clave en enkontrol");
            }

            var metodoPago = _context.tblF_EK_MetodoPagoSat.FirstOrDefault(e => e.esActivo && e.id == obj.MetodoPagoSAT);
            string descMetodoPago = "";

            if (metodoPago != null)
            {
                descMetodoPago = metodoPago.descripcion;
            }

            try
            {

                if (obj.id > 0)
                {
                    #region EDITAR
                    var objFactura = _context.tblF_EK_Facturas.FirstOrDefault(e => e.esActivo && e.factura == idSPFactura);

                    objFactura.fecha = DateTime.Now;
                    objFactura.requisicion = obj.ReqOC.Value.ToString();
                    objFactura.vendedor = idUsrEnkontrol;
                    objFactura.cond_pago = obj.CondicionesPago.Value;
                    objFactura.moneda = obj.TipoMoneda == "MX" ? 1 : 2;
                    objFactura.tipo_cambio = 1M;
                    objFactura.sub_total = Convert.ToDecimal(lstImpuesto[1].Valor.Substring(1, lstImpuesto[1].Valor.Length - 1));
                    objFactura.iva = Convert.ToDecimal(lstImpuesto[2].Valor.Substring(1, lstImpuesto[2].Valor.Length - 1));
                    objFactura.total = Convert.ToDecimal(lstImpuesto[0].Valor.Substring(1, lstImpuesto[0].Valor.Length - 1));
                    objFactura.porcent_iva = 16M;
                    objFactura.tipo = obj.TipoMoneda == "MX" ? "N" : "E";
                    objFactura.cc = obj.CC;
                    objFactura.tm = obj.TM;
                    objFactura.nombre = obj.Nombre;
                    objFactura.rfc = obj.RFC;
                    objFactura.direccion = obj.Direccion;
                    objFactura.ciudad = "";
                    objFactura.cp = obj.CP;
                    objFactura.estatus = "P";
                    objFactura.obs = obj.Observaciones;
                    objFactura.compania = "01";
                    objFactura.pais = "";
                    objFactura.estado = "";
                    objFactura.cfd_cadena_original = string.Format(
                        @"||3.2|{0}|ingreso|{1}|{2}|0.00|{3}|{4}|HERMOSILLO, Son|NO IDENTIFICADO|GCP800324FJ1|GRUPO CONSTRUCCIONES PLANIFICADAS SA DE CV|PERIFERICO|770|EMILIANO ZAPATA|HERMOSILLO|HERMOSILLO|Son|MEXICO|83280"
                        , DateTime.Now.ToString("s"), descMetodoPago, obj.CondicionesPago, Convert.ToDecimal(lstImpuesto[1].Valor.Substring(1, lstImpuesto[1].Valor.Length - 1)), obj.TipoMoneda == "MX" ? "MN" : "19|USD", obj.MetodoPago, obj.RFC
                    );
                    //||3.2|2022-03-24T08:48:34|ingreso|Pago en una sola exhibición|90 días|32674.55|0.00|MN|37902.48|99|HERMOSILLO, Son|NO IDENTIFICADO|GCP800324FJ1|GRUPO CONSTRUCCIONES PLANIFICADAS SA DE CV|PERIFERICO|770|EMILIANO ZAPATA|HERMOSILLO|HERMOSILLO|Son|MEXICO|83280
                    objFactura.fechaModificacion = DateTime.Now;
                    objFactura.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;

                    _context.SaveChanges();

                    foreach (var item in lst)
                    {
                        var objPartida = _context.tblF_EK_Facturas_Det.FirstOrDefault(e => e.esActivo && e.factura == idSPFactura && e.partida == item.Renglon);

                        objPartida.insumo = item.TipoInsumo.Value;
                        objPartida.cantidad = item.Cantidad;
                        objPartida.precio = item.Precio;
                        objPartida.importe = item.Importe;
                        objPartida.unidad = item.Concepto;
                        objPartida.linea = item.Unidad;

                        _context.SaveChanges();
                    }
                    #endregion
                }
                else
                {
                    #region CREAR
                    _context.tblF_EK_Facturas.Add(new tblF_EK_Facturas
                    {
                        folioPrefactura = obj.Folio,
                        factura = idSPFactura,
                        numcte = obj.numcte.Value, //??
                        sucursal = 1,
                        fecha = DateTime.Now,
                        pedido = idSPPedido,
                        remision = idSPRemision,
                        requisicion = obj.ReqOC.Value.ToString(),
                        vendedor = idUsrEnkontrol,
                        consignado = "1",
                        transporte = "1",
                        cond_pago = obj.CondicionesPago.Value,
                        talon = 1,
                        moneda = obj.TipoMoneda == "MX" ? 1 : 2,
                        tipo_cambio = 1M,
                        sub_total = Convert.ToDecimal(lstImpuesto[0].Valor.Substring(1, lstImpuesto[0].Valor.Length - 1)),
                        iva = Convert.ToDecimal(lstImpuesto[1].Valor.Substring(1, lstImpuesto[1].Valor.Length - 1)),
                        total = Convert.ToDecimal(lstImpuesto[2].Valor.Substring(1, lstImpuesto[2].Valor.Length - 1)),
                        porcent_iva = 16M,
                        tipo = "N", //NACIONAL | EXTRANGERO
                        lista_precio = 0,
                        cc = obj.CC,
                        tm = obj.TM,
                        descuento = 0M,
                        porcent_descto = 0M,
                        nombre = obj.Nombre,
                        rfc = obj.RFC,
                        direccion = obj.Direccion,
                        ciudad = "",
                        cp = obj.CP,
                        estatus = "P",
                        obs = obj.Observaciones,
                        compania = "01",
                        pais = "",
                        estado = "",
                        tipo_clase = "S",
                        cia_sucursal = 1,
                        numero_nc = 0,
                        tipo_nc = 0,
                        retencion = 0M,
                        cfd_folio = 0, //??
                        cfd_fecha = DateTime.Now,
                        cfd_no_certificado = "",
                        cfd_ano_aprob = 0,
                        cfd_num_aprob = 1,
                        cfd_cadena_original = string.Format(
                            @"||3.2|{0}|ingreso|{1}|{2}|0.00|{3}|{4}|HERMOSILLO, Son|NO IDENTIFICADO|GCP800324FJ1|GRUPO CONSTRUCCIONES PLANIFICADAS SA DE CV|PERIFERICO|770|EMILIANO ZAPATA|HERMOSILLO|HERMOSILLO|Son|MEXICO|83280"
                            , DateTime.Now.ToString("s"), descMetodoPago, obj.CondicionesPago, Convert.ToDecimal(lstImpuesto[1].Valor.Substring(1, lstImpuesto[1].Valor.Length - 1)), obj.TipoMoneda == "MX" ? "MN" : "19|USD", obj.MetodoPago, obj.RFC
                        ),
                        //||3.2|2022-03-24T08:48:34|ingreso|Pago en una sola exhibición|90 días|32674.55|0.00|MN|37902.48|99|HERMOSILLO, Son|NO IDENTIFICADO|GCP800324FJ1|GRUPO CONSTRUCCIONES PLANIFICADAS SA DE CV|PERIFERICO|770|EMILIANO ZAPATA|HERMOSILLO|HERMOSILLO|Son|MEXICO|83280
                        cfd_certificado_sello = "",
                        cfd_sello_digital = "",
                        cfd_serie = obj.Serie,
                        fecha_cancelacion = DateTime.Now,
                        usuario_cancela = 0,
                        cfd_subtotal = 0M,
                        cfd_descuento = 0M,
                        cfd_iva = 0M,
                        cfd_retenciones = 0M,
                        cfd_total = 0M,
                        enviado = false,
                        tipo_documento = "F",
                        autoriza_factura = "N",
                        empleado_autoriza = 0,
                        fecha_autoriza = null,
                        idPac = 0,
                        gsdb = "",
                        cve_formulario = 0,
                        cfd_mn = "S",
                        cfd_enviada = "N",
                        cfd_ret_iva = 0M,
                        id_metodo_pago = Convert.ToInt32(obj.MetodoPago),
                        id_regimen_fiscal = obj.RegimenFiscal.Value,
                        cfd_num_cta_pago = "0",
                        comentarios_cfdi = "",
                        bit_cfdi = 1,
                        uuid = "",
                        fecha_timbrado = null,
                        certificado_sat = "",
                        sello_sat = "",
                        asn = "",
                        bit_parcialidad = false,
                        copia_xml = "",
                        copia_xml_pdf = "",
                        cfd_tiporelacion = "",
                        refid = "",
                        cfd_forma_pago_sat = "",
                        cfd_condicionespago = "",
                        cfd_reg_fiscal_sat = "",
                        cfd_version = 32,
                        bit_implocal = false,
                        cfd_certificado = "",
                        bit_excento_iva = false,
                        fechaCreacion = DateTime.Now,
                        fechaModificacion = DateTime.Now,
                        idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id,
                        idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id,
                        esActivo = true,
                    });

                    _context.SaveChanges();

                    foreach (var item in lst)
                    {
                        _context.tblF_EK_Facturas_Det.Add(new tblF_EK_Facturas_Det
                        {
                            folioPrefactura = obj.Folio,
                            factura = idSPFactura,
                            partida = item.Renglon,
                            insumo = item.TipoInsumo.Value,
                            cantidad = item.Cantidad,
                            precio = item.Precio,
                            importe = item.Importe,
                            unidad = item.Concepto,
                            cant_factura = 0M,
                            precio_factura = 0M,
                            pedido = idSPPedido,
                            ped_part = 1,
                            porcent_descto = 0M,
                            cant_entregada = 0M,
                            cant_kg_factura = 0M,
                            cant_kg = 0M,
                            cia_sucursal = 1,
                            numero_nc = 0,
                            fac_part = 0,
                            remision = idSPRemision,
                            linea = item.Unidad,
                            linea_nc = "",
                            porcen_iva_partida = 0M,
                            iva = 0M,
                            cc = item.cc ?? obj.CC,
                            fechaCreacion = DateTime.Now,
                            fechaModificacion = DateTime.Now,
                            idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id,
                            idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id,
                            esActivo = true,
                        });

                        _context.SaveChanges();
                    }
                    #endregion
                }    

                //dbTransac.Commit();

                resultado.Add(ITEMS, null);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                                        
                //dbTransac.Rollback();

                //resultado.Add(MESSAGE, e.Message);
                //resultado.Add(SUCCESS, false);
                throw e;

            }

            return resultado;
        }
        
        #endregion

    }
}
