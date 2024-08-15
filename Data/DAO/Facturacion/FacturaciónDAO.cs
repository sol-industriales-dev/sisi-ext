using Core.DAO.Facturacion;
using Core.DTO;
using Core.DTO.Facturacion;
using Core.Enum;
using Core.Enum.Facuración;
using Core.Enum.Multiempresa;
using Data.EntityFramework;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Odbc;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using CFDI;
using Core.DTO.Contabilidad.Propuesta;
using Newtonsoft.Json;
using System.Data;
using Core.DTO.Utils.Data;
using Core.DTO.Contabilidad.Facturacion;
using Core.Entity.Facturacion.Estimacion;
using Core.Enum.Administracion.Cotizaciones;
using Core.Enum.Principal.Bitacoras;
using System.Data.Entity;
using Core.Enum.Administracion.Propuesta;
using Core.DTO.Contabilidad.FlujoEfectivo;
using Core.Entity.Administrativo.Contabilidad.FlujoEfectivo;


namespace Data.DAO.Facturacion
{
    public class FacturaciónDAO : GenericDAO<Core.Entity.Facturacion.Prefacturacion.tblF_CapPrefactura>, IFacturaciónDAO
    {
        #region Facturación
        public List<InsumosDTO> lstInsumo(string insumo, string descripcion)
        {
            try
            {

                string condicion = string.Empty;
                if (!insumo.Equals(string.Empty) || !descripcion.Equals(string.Empty))
                {
                    condicion += " AND ";
                    condicion += insumo.Equals(string.Empty) ? string.Empty : " insumo LIKE " + Regex.Replace(insumo, @"\s+", "");
                    condicion += !insumo.Equals(string.Empty) && !descripcion.Equals(string.Empty) ? " AND " : string.Empty;
                    condicion += descripcion.Equals(string.Empty) ? string.Empty : "descripcion LIKE  '%" + descripcion + "%'";
                }
                string consulta = "SELECT insumo, descripcion, unidad, tipo, grupo FROM insumos WHERE tipo = 9 "
                                + condicion + " order by insumo ";
                var res = (List<InsumosDTO>)_contextEnkontrol.Where(consulta).ToObject<List<InsumosDTO>>();
                return res.ToList();

            }
            catch (Exception)
            {
                return new List<InsumosDTO>();
            }

        }
        public InsumosDTO objInsumo(int consecutivo)
        {
            string consulta = "SELECT i.insumo, i.descripcion, i.unidad, p.PRECIO_INSUMO, p.FE_PRECIO, p.OBRA, p.consecutivo_bit "
                              + " FROM \"DBA\".\"insumos\" i "
                              + " INNER JOIN \"DBA\".\"px_bit_SU_PRECIOS_INSUMO\" p ON  i.insumo = p.INSUMO "
                              + " WHERE p.consecutivo_bit = " + consecutivo;
            var res = (List<InsumosDTO>)_contextEnkontrol.Where(consulta).ToObject<List<InsumosDTO>>();
            return res.FirstOrDefault();
        }

        public InsumosDTO objRentencion(int insumo)
        {
            string consulta = "SELECT insumo, descripcion, unidad  FROM insumos WHERE insumo =" + insumo;
            var res = (List<InsumosDTO>)_contextEnkontrol.Where(consulta).ToObject<List<InsumosDTO>>();
            return res.FirstOrDefault();
        }

        string getDescripccionInsumo(int insumo)
        {
            string consulta = "SELECT * FROM insumos where insumo =" + insumo;
            var res = (List<InsumosDTO>)_contextEnkontrol.Where(consulta).ToObject<List<InsumosDTO>>();
            return res.FirstOrDefault().descripcion;
        }

        public List<object> GetlstInsumoFactura(int pedido)
        {
            try
            {
                string consulta = "SELECT partida, insumo, cantidad, precio, importe, unidad, porcent_descto FROM sf_facturas_det WHERE pedido = " + pedido;
                var lst = (List<FacturaDet>)_contextEnkontrol.Where(consulta).ToObject<List<FacturaDet>>();
                var res = lst.Select(x => new
                {
                    Partida = x.partida,
                    Insumo = x.insumo,
                    Descripcion = getDescripccionInsumo(x.insumo),
                    Unidad = x.unidad,
                    Cantidad = x.cantidad,
                    Precio = x.precio,
                    Descuento = x.porcent_descto,
                    DescuentoDinero = x.cantidad * x.precio * (x.porcent_descto / 100),
                    Importe = x.importe
                }).OrderBy(x => x.Partida).Cast<object>().ToList();
                return res;
            }
            catch (Exception)
            {
                return new List<object>();
            }
        }

        public List<object> GetlstInsumoRentencion(int pedido)
        {
            try
            {
                string consulta = "SELECT r.insumo, r.partida, r.cantidad, r.porc_ret, r.importe, r.ret_iva as calc_iva_factura, " +
                    "(select i.descripcion from insumos i where r.insumo = i.insumo), " +
                    "(select i.unidad from insumos i where r.insumo = i.insumo) " +
                    " FROM sf_factura_retencion r WHERE pedido = " + pedido;
                var lst = (List<FacturaDet>)_contextEnkontrol.Where(consulta).ToObject<List<FacturaDet>>();
                var res = lst.Select(x => new
                {
                    Partida = x.partida,
                    Insumo = x.insumo,
                    Descripcion = x.descripcion,
                    Unidad = x.unidad,
                    Cantidad = x.importe,
                    Descuento = x.porc_ret,
                    Importe = x.importe,
                    IvaRetenido = x.calc_iva_factura > 0 ? "S" : "N"
                }).OrderBy(x => x.Partida).Cast<object>().ToList();
                return res;
            }
            catch (Exception)
            {
                return new List<object>();
            }

        }

        public object objPedido(int pedido, out int numcte)
        {
            string consulta = "SELECT * FROM sf_pedidos where pedido = " + pedido;
            var objPedido = (List<PedidosDTO>)_contextEnkontrol.Where(consulta).ToObject<List<PedidosDTO>>();
            var res = objPedido.Select(x => new
            {
                pedido = x.pedido,
                numcte = x.numcte,
                sucursal = x.sucursal,
                fecha = x.fecha.ToString("d/M/yyyy"),
                requisicion = x.requisicion,
                vendedor = x.vendedor,
                cond_pago = x.cond_pago,
                moneda = x.moneda,
                tipo_cambio = x.tipo_cambio,
                sub_total = x.sub_total,
                iva = x.iva,
                total = x.total,
                porcent_iva = x.porcent_iva,
                tipo = x.tipo,
                porcent_descto = x.porcent_descto,
                descuento = x.descuento,
                estatus = x.estatus,
                condicion_entrega = x.condicion_entrega,
                tipo_flete = x.tipo_flete,
                lista_precios = x.lista_precios,
                status_autorizado = x.status_autorizado,
                zona = x.zona,
                obs = x.obs,
                otros_cond_pago = x.otros_cond_pago,
                usuario = x.usuario,
                fecha_hora = x.fecha_hora,
                tipo_pedido = x.tipo_pedido,
                cc = x.cc,
                tm = x.tm,
                elaboro = x.elaboro,
                cia_sucursal = x.cia_sucursal,
                tipo_credito = x.tipo_credito,
                retencion = x.retencion,
                aplica_total_antes_iva = x.aplica_total_antes_iva,
                total_dec = x.total_dec
            }).FirstOrDefault();

            numcte = res.numcte;
            return res;
        }

        public ClienteDTO objCliente(int cliente)
        {
            string consulta = "SELECT * FROM sx_clientes where numcte = " + cliente;
            var res = (List<ClienteDTO>)_contextEnkontrol.Where(consulta).ToObject<List<ClienteDTO>>();
            return res.FirstOrDefault();
        }
        public List<ClienteDTO> getLstCliente()
        {
            return _contextEnkontrol.Select<ClienteDTO>(EnkontrolAmbienteEnum.Prod,"SELECT * FROM sx_clientes");
        }

        public object objRemision(int pedido)
        {
            try
            {
                string consulta = "SELECT * FROM sf_remision WHERE  pedido = " + pedido;
                var objRemision = (List<RemisionDTO>)_contextEnkontrol.Where(consulta).ToObject<List<RemisionDTO>>();
                var res = objRemision.Select(x => new
                {
                    sucursal = x.sucursal,
                    remision = x.remision,
                    fecha = x.fecha.ToString("d/M/yyyy"),
                    rfc = x.rfc,
                    nombre = x.nombre,
                    direccion = x.direccion,
                    ciudad = x.ciudad,
                    cp = x.cp,
                    telefono = x.telefono,
                    transporte = x.transporte,
                    talon = x.talon,
                    consignado = x.consignado,
                    observaciones = x.observaciones,
                    moneda = x.moneda,
                    tipo_cambio = x.tipo_cambio,
                    sub_total = x.sub_total,
                    iva = x.iva,
                    total = x.total,
                    porcent_iva = x.porcent_iva,
                    porcent_descto = x.porcent_descto,
                    elaboro = x.elaboro,
                    tipo_flete = x.tipo_flete,
                    descuento = x.descuento,
                    factura = x.factura,
                    estatus = x.estatus,
                    entregado = x.entregado,
                    pedido = x.pedido,
                    retencion = x.retencion
                }).FirstOrDefault();
                return res;
            }
            catch (Exception)
            {
                return new RemisionDTO() { remision = 0 };
            }
        }

        public object objFactura(int pedido, out int cia_surcusal)
        {
            try
            {
                string consulta = "SELECT * FROM sf_facturas WHERE pedido = " + pedido;
                var objFactura = (List<FacturaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<FacturaDTO>>();
                var res = objFactura.Select(x => new
                {
                    factura = x.factura,
                    numcte = x.numcte,
                    sucursal = x.sucursal,
                    fecha = x.fecha.ToString("d/M/yyyy"),
                    pedido = x.pedido,
                    remision = x.remision,
                    requisicion = x.requisicion,
                    vendedor = x.vendedor,
                    consignado = x.consignado,
                    transporte = x.transporte,
                    cond_pago = x.cond_pago,
                    talon = x.talon,
                    moneda = x.moneda,
                    tipo_cambio = x.tipo_cambio,
                    sub_total = x.sub_total,
                    iva = x.iva,
                    total = x.total,
                    porcent_iva = x.porcent_iva,
                    tipo = x.tipo,
                    lista_precio = x.lista_precio,
                    cc = x.cc,
                    tm = x.tm,
                    descuento = x.descuento,
                    porcent_descto = x.porcent_descto,
                    nombre = x.nombre,
                    rfc = x.rfc,
                    direccion = x.direccion,
                    ciudad = x.ciudad,
                    cp = x.cp,
                    estatus = x.estatus,
                    obs = x.obs,
                    compania = x.compania,
                    talon_emb = x.talon_emb,
                    camion = x.camion,
                    placas = x.placas,
                    pais = x.pais,
                    estado = x.estado,
                    tipo_clase = x.tipo_clase,
                    cia_sucursal = x.cia_sucursal,
                    numero_nc = x.numero_nc,
                    tipo_nc = x.tipo_nc,
                    status_notacredito = x.status_notacredito,
                    retencion = x.retencion,
                    cfd_folio = x.cfd_folio,

                    cfd_no_certificado = x.cfd_no_certificado,
                    cfd_ano_aprob = x.cfd_ano_aprob,
                    cfd_num_aprob = x.cfd_num_aprob,
                    cfd_cadena_original = x.cfd_cadena_original,
                    cfd_certificado_sello = x.cfd_certificado_sello,
                    cfd_sello_digital = x.cfd_sello_digital,
                    cfd_serie = x.cfd_serie,
                    fecha_cancelacion = x.fecha_cancelacion,
                    usuario_cancela = x.usuario_cancela,
                    cfd_subtotal = x.cfd_subtotal,
                    cfd_descuento = x.cfd_descuento,
                    cfd_iva = x.cfd_iva,
                    cfd_retenciones = x.cfd_retenciones,
                    cfd_total = x.cfd_total,
                    enviado = x.enviado,
                    tipo_documento = x.tipo_documento,
                    autoriza_factura = x.autoriza_factura,
                    empleado_autoriza = x.empleado_autoriza,
                    fecha_autoriza = x.fecha_autoriza,
                    idPac = x.idPac,
                    gsdb = x.gsdb,
                    cve_formulario = x.cve_formulario,
                    cfd_mn = x.cfd_mn,
                    cfd_enviada = x.cfd_enviada,
                    cfd_ret_iva = x.cfd_ret_iva,
                    id_metodo_pago = x.id_metodo_pago,
                    id_regimen_fiscal = x.id_regimen_fiscal,
                    cfd_num_cta_pago = x.cfd_num_cta_pago,
                    comentarios_cfdi = x.comentarios_cfdi,
                    bit_cfdi = x.bit_cfdi,
                    uuid = x.uuid,
                    fecha_timbrado = x.fecha_timbrado,
                    certificado_sat = x.certificado_sat,
                    sello_sat = x.sello_sat,
                    asn = x.asn
                }).FirstOrDefault();
                cia_surcusal = res.cia_sucursal;
                return res;
            }
            catch (Exception)
            {
                cia_surcusal = 1;
                return new FacturaDTO() { factura = 0 };
            }

        }

        EstadoDTO getEstado(string ciudad)
        {
            string consulta = "SELECT c.ciudad, e.pais, e.estado FROM ciudades c INNER JOIN estados e on e.estado = c.estado  WHERE c.ciudad = '" + ciudad + "'";
            var res = (List<EstadoDTO>)_contextEnkontrol.Where(consulta).ToObject<List<EstadoDTO>>();
            return res.FirstOrDefault();
        }

        int getNewPedido()
        {
            string consulta = "SELECT TOP 1 * FROM sf_pedidos ORDER BY pedido DESC";
            var res = (List<pedidoDTO>)_contextEnkontrol.Where(consulta).ToObject<List<pedidoDTO>>();
            return res.FirstOrDefault().pedido + 1;
        }

        int getNewRemision()
        {
            string consulta = "SELECT TOP 1 * FROM sf_remision ORDER BY remision DESC";
            var res = (List<RemisionDTO>)_contextEnkontrol.Where(consulta).ToObject<List<RemisionDTO>>();
            return res.FirstOrDefault().remision + 1;
        }

        int getNewFactura()
        {
            string consulta = "SELECT TOP 1 * FROM sf_facturas ORDER BY factura DESC";
            var res = (List<FacturaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<FacturaDTO>>();
            return res.FirstOrDefault().factura + 1;
        }

        int getNewFolio()
        {
            string consulta = "SELECT TOP 1 * FROM sf_facturas ORDER BY factura DESC";
            var res = (List<FacturaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<FacturaDTO>>();
            return Convert.ToInt32(res.FirstOrDefault().cfd_folio) + 1;
        }

        public bool existPedido(int pedido)
        {
            try
            {
                string consulta = "SELECT * FROM sf_pedidos WHERE pedido = " + pedido;
                var res = (List<FacturaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<FacturaDTO>>();
                return res.Count > 0 ? true : false;
            }
            catch (Exception) { return false; }
        }

        public bool existRemision(int pedido)
        {
            try
            {
                string consulta = "SELECT * FROM sf_remision WHERE pedido = " + pedido;
                var res = (List<FacturaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<FacturaDTO>>();
                return res.Count > 0 ? true : false;
            }
            catch (Exception) { return false; }
        }

        public bool existFactura(int pedido)
        {
            try
            {
                string consulta = "SELECT * FROM sf_facturas WHERE pedido = " + pedido;
                var res = (List<FacturaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<FacturaDTO>>();
                return res.Count > 0 ? true : false;
            }
            catch (Exception) { return false; }
        }

        bool existPedidoRentencion(int insumo, int pedido)
        {
            try
            {
                string consulta = "SELECT * FROM sf_pedido_retencion WHERE pedido = " + pedido + " AND insumo = " + insumo;
                var res = (List<FacturaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<FacturaDTO>>();
                return res.Count > 0 ? true : false;
            }
            catch (Exception) { return false; }
        }

        bool existRemisionRentencion(int insumo, int pedido)
        {
            try
            {
                string consulta = "SELECT * FROM sf_remision_retencion WHERE pedido = " + pedido + " AND insumo = " + insumo;
                var res = (List<FacturaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<FacturaDTO>>();
                return res.Count > 0 ? true : false;
            }
            catch (Exception) { return false; }
        }

        bool existFacturaRentencion(int insumo, int pedido)
        {
            try
            {
                string consulta = "SELECT * FROM sf_factura_retencion WHERE pedido = " + pedido + " AND insumo = " + insumo;
                var res = (List<FacturaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<FacturaDTO>>();
                return res.Count > 0 ? true : false;
            }
            catch (Exception) { return false; }
        }

        bool existPartidaFactura(int insumo, int factura)
        {
            try
            {
                string consulta = "SELECT * FROM sf_facturas_det WHERE factura = " + factura + " AND insumo = " + insumo;
                var res = (List<FacturaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<FacturaDTO>>();
                return res.Count > 0 ? true : false;
            }
            catch (Exception) { return false; }
        }

        bool existPartidaRemision(int insumo, int remision)
        {
            try
            {
                string consulta = "SELECT * FROM sf_remision_det WHERE remision = " + remision + " AND insumo = " + insumo;
                var res = (List<PartidaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<PartidaDTO>>();
                return res.Count > 0 ? true : false;
            }
            catch (Exception) { return false; }
        }

        bool existPartidaDesc(int insumo, int pedido)
        {
            try
            {
                string consulta = "SELECT * FROM sf_pedidos_dcto WHERE pedido = " + pedido + " AND insumo = " + insumo;
                var res = (List<FacturaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<FacturaDTO>>();
                return res.Count > 0 ? true : false;
            }
            catch (Exception) { return false; }
        }

        bool existPartidaPedido(int insumo, int pedido)
        {
            try
            {
                string consulta = "SELECT * FROM sf_pedidos_det WHERE pedido = " + pedido + " AND insumo = " + insumo;
                var res = (List<FacturaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<FacturaDTO>>();
                return res.Count > 0 ? true : false;
            }
            catch (Exception) { return false; }
        }

        public FacturaDTO getNew()
        {
            return new FacturaDTO()
            {
                pedido = getNewPedido(),
                remision = getNewRemision(),
                factura = getNewFactura(),
                cfd_folio = getNewFolio(),
            };
        }

        int SaveTblPedido(BigFacturaDTO obj)
        {
            var c = new Conexion();
            var con = c.ConnectPrueba();
            if (con.State.Equals("Open"))
            {
                con.Open();
            }
            if (con != null)
            {
                string insertQuery = "INSERT INTO sf_pedidos (pedido, numcte, sucursal, fecha, requisicion, vendedor, cond_pago, moneda, tipo_cambio, sub_total, iva, total, porcent_iva, " +
                    "tipo, porcent_descto, descuento, estatus, condicion_entrega, tipo_flete, obs, tipo_pedido, cc, tm, elaboro, cia_sucursal, tipo_credito, retencion, aplica_total_antes_iva, total_dec, zona, lista_precios, usuario, fecha_hora, status_autorizado) " +
                                    " VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, 0, ?, ?, 'A');";

                OdbcCommand command = new OdbcCommand(insertQuery);
                OdbcParameterCollection parameters = command.Parameters;

                parameters.Add("@pedido", OdbcType.Numeric).Value = obj.pedido;
                parameters.Add("@numcte", OdbcType.Numeric).Value = obj.numcte;
                parameters.Add("@sucursal", OdbcType.Numeric).Value = obj.sucursal;
                parameters.Add("@fecha", OdbcType.Date).Value = obj.fecha;
                parameters.Add("@requisicion", OdbcType.Char).Value = obj.requisicion ?? "";
                parameters.Add("@vendedor", OdbcType.Numeric).Value = obj.vendedor;
                parameters.Add("@cond_pago", OdbcType.Numeric).Value = obj.cond_pago;
                parameters.Add("@moneda", OdbcType.Char).Value = obj.moneda;
                parameters.Add("@tipo_cambio", OdbcType.Numeric).Value = obj.tipo_cambio;
                parameters.Add("@sub_total", OdbcType.Numeric).Value = obj.sub_total;
                parameters.Add("@iva", OdbcType.Numeric).Value = obj.iva;
                parameters.Add("@total", OdbcType.Numeric).Value = obj.total;
                parameters.Add("@porcent_iva", OdbcType.Numeric).Value = obj.porcent_iva;
                parameters.Add("@tipo", OdbcType.Char).Value = obj.tipo;
                parameters.Add("@porcent_descto", OdbcType.Numeric).Value = obj.porcent_descto;
                parameters.Add("@descuento", OdbcType.Numeric).Value = obj.descuento;
                parameters.Add("@estatus", OdbcType.Char).Value = "T";
                parameters.Add("@condicion_entrega", OdbcType.Char).Value = obj.condicion_entrega.Equals("--Selecione--") ? "n/a" : obj.condicion_entrega;
                parameters.Add("@tipo_flete", OdbcType.Char).Value = obj.tipo_flete.Equals("--Selecione--") ? "n/a" : obj.tipo_flete;
                parameters.Add("@obs", OdbcType.Char).Value = obj.obs ?? "";
                parameters.Add("@tipo_pedido", OdbcType.Char).Value = obj.tipo_pedido;
                parameters.Add("@cc", OdbcType.Char).Value = obj.cc;
                parameters.Add("@tm", OdbcType.Numeric).Value = obj.tm;
                parameters.Add("@elaboro", OdbcType.Numeric).Value = obj.elaboro;
                parameters.Add("@cia_sucursal", OdbcType.Numeric).Value = obj.cia_sucursal;
                parameters.Add("@tipo_credito", OdbcType.Char).Value = obj.tipo_credito;
                parameters.Add("@retencion", OdbcType.Char).Value = obj.retencion;
                parameters.Add("@aplica_total_antes_iva", OdbcType.Numeric).Value = obj.aplica_total_antes_iva;
                parameters.Add("@total_dec", OdbcType.Char).Value = obj.total_dec;
                parameters.Add("@zona", OdbcType.Numeric).Value = obj.zona;
                parameters.Add("@usuario", OdbcType.Numeric).Value = obj.vendedor;
                parameters.Add("@fecha_hora", OdbcType.DateTime).Value = DateTime.Now;

                command.Connection = con;
                var id = command.ExecuteNonQuery();
                c.Close(con);
                return id;
            }
            return 0;
        }

        int SaveTblRemision(BigFacturaDTO obj)
        {
            var c = new Conexion();
            var con = c.ConnectPrueba();
            if (con != null)
            {
                string insertQuery = "INSERT INTO sf_remision (remision, sucursal, fecha, rfc, nombre, direccion, ciudad, cp, telefono, transporte, talon, consignado, observaciones, moneda, " +
                                     " tipo_cambio, sub_total, iva, total, porcent_iva, porcent_descto, elaboro, tipo_flete, descuento, estatus, entregado, retencion, pedido, factura) " +
                                     " VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

                OdbcCommand command = new OdbcCommand(insertQuery);
                OdbcParameterCollection parameters = command.Parameters;

                parameters.Add("@remision", OdbcType.Numeric).Value = obj.remision;
                parameters.Add("@sucursal", OdbcType.Numeric).Value = obj.sucursal;
                parameters.Add("@fecha", OdbcType.Date).Value = obj.fecha;
                parameters.Add("@rfc", OdbcType.Char).Value = obj.rfc;
                parameters.Add("@nombre", OdbcType.Char).Value = obj.nombre;
                parameters.Add("@direccion", OdbcType.Char).Value = obj.direccion;
                parameters.Add("@ciudad", OdbcType.Char).Value = obj.ciudad;
                parameters.Add("@cp", OdbcType.Char).Value = obj.cp ?? "1";
                parameters.Add("@telefono", OdbcType.Char).Value = obj.telefono1 ?? "1";
                parameters.Add("@transporte", OdbcType.Char).Value = obj.transporte;
                parameters.Add("@talon", OdbcType.Char).Value = obj.talon ?? "";
                parameters.Add("@consignado", OdbcType.Char).Value = obj.consignado ?? "";
                parameters.Add("@observaciones", OdbcType.Char).Value = obj.obs ?? "";
                parameters.Add("@moneda", OdbcType.Char).Value = obj.moneda;
                parameters.Add("@tipo_cambio", OdbcType.Numeric).Value = obj.tipo_cambio;
                parameters.Add("@sub_total", OdbcType.Numeric).Value = obj.sub_total;
                parameters.Add("@iva", OdbcType.Numeric).Value = obj.iva;
                parameters.Add("@total", OdbcType.Numeric).Value = obj.total;
                parameters.Add("@porcent_iva", OdbcType.Numeric).Value = obj.porcent_iva;
                parameters.Add("@porcent_descto", OdbcType.Numeric).Value = obj.porcent_descto;
                parameters.Add("@elaboro", OdbcType.Numeric).Value = obj.elaboro;
                parameters.Add("@tipo_flete", OdbcType.Char).Value = obj.tipo_flete.Equals("--Selecione--") ? "n/a" : obj.tipo_flete;
                parameters.Add("@descuento", OdbcType.Numeric).Value = obj.descuento;
                parameters.Add("@estatus", OdbcType.Char).Value = "I";
                parameters.Add("@entregado", OdbcType.Char).Value = obj.entregado;
                parameters.Add("@retencion", OdbcType.Char).Value = obj.retencion;
                parameters.Add("@pedido", OdbcType.Numeric).Value = obj.pedido;
                parameters.Add("@factura", OdbcType.Numeric).Value = obj.factura;

                command.Connection = con;
                var id = command.ExecuteNonQuery();
                c.Close(con);
                return id;
            }
            return 0;
        }

        int SaveTblFactura(BigFacturaDTO obj, List<PartidaDTO> lst)
        {
            var sello = "";
            var c = new Conexion();
            var con = c.ConnectPrueba();
            if (con != null)
            {
                string insertQuery = "INSERT INTO sf_facturas (factura, numcte, sucursal, fecha, requisicion, vendedor, consignado, transporte, cond_pago, talon, moneda, tipo_cambio, sub_total, " +
                                        " iva, total, porcent_iva, tipo, cc, tm, descuento, porcent_descto, nombre, rfc, direccion, ciudad, cp, estatus, obs, cia_sucursal, cfd_folio, " +
                                        " cfd_serie, gsdb, id_metodo_pago, id_regimen_fiscal, cfd_num_cta_pago, asn, pedido, remision, numero_nc, tipo_nc, lista_precio, tipo_clase, compania, pais, estado, autoriza_factura, cfd_no_certificado, cfd_certificado_sello, cfd_cadena_original, cfd_sello_digital) " +
                                        " VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, 0, 0, 0, ?, 01, ?, ?, ?, ?, ?, ?, ?)";

                OdbcCommand command = new OdbcCommand(insertQuery);
                OdbcParameterCollection parameters = command.Parameters;

                parameters.Add("@factura", OdbcType.Numeric).Value = obj.factura;
                parameters.Add("@numcte", OdbcType.Numeric).Value = obj.numcte;
                parameters.Add("@sucursal", OdbcType.Numeric).Value = obj.sucursal;
                parameters.Add("@fecha", OdbcType.Date).Value = obj.fecha;
                parameters.Add("@requisicion", OdbcType.Char).Value = obj.requisicion ?? "1";
                parameters.Add("@vendedor", OdbcType.Numeric).Value = obj.vendedor;
                parameters.Add("@consignado", OdbcType.Char).Value = obj.consignado ?? "1";
                parameters.Add("@transporte", OdbcType.Char).Value = obj.transporte ?? "1";
                parameters.Add("@cond_pago", OdbcType.Numeric).Value = obj.cond_pago;
                parameters.Add("@talon", OdbcType.Numeric).Value = obj.talon ?? "1";
                parameters.Add("@moneda", OdbcType.Char).Value = obj.moneda;
                parameters.Add("@tipo_cambio", OdbcType.Numeric).Value = obj.tipo_cambio;
                parameters.Add("@sub_total", OdbcType.Numeric).Value = obj.sub_total;
                parameters.Add("@iva", OdbcType.Numeric).Value = obj.iva;
                parameters.Add("@total", OdbcType.Numeric).Value = obj.total;
                parameters.Add("@porcent_iva", OdbcType.Numeric).Value = obj.porcent_iva;
                parameters.Add("@tipo", OdbcType.Char).Value = obj.tipo;
                parameters.Add("@cc", OdbcType.Char).Value = obj.cc;
                parameters.Add("@tm", OdbcType.Numeric).Value = obj.tm;
                parameters.Add("@descuento", OdbcType.Numeric).Value = obj.descuento;
                parameters.Add("@porcent_descto", OdbcType.Numeric).Value = obj.porcent_descto;
                parameters.Add("@nombre", OdbcType.Char).Value = obj.nombre;
                parameters.Add("@rfc", OdbcType.Char).Value = obj.rfc;
                parameters.Add("@direccion", OdbcType.Char).Value = obj.direccion;
                parameters.Add("@ciudad", OdbcType.Char).Value = obj.ciudad;
                parameters.Add("@cp", OdbcType.Char).Value = obj.cp ?? "";
                parameters.Add("@estatus", OdbcType.Char).Value = "I";
                parameters.Add("@obs", OdbcType.Char).Value = obj.obs ?? "";
                parameters.Add("@cia_sucursal", OdbcType.Numeric).Value = obj.cia_sucursal;
                parameters.Add("@cfd_folio", OdbcType.Numeric).Value = obj.cfd_folio ?? "";
                parameters.Add("@cfd_serie", OdbcType.VarChar).Value = obj.cfd_serie;
                parameters.Add("@gsdb", OdbcType.VarChar).Value = obj.gsdb ?? "";
                parameters.Add("@id_metodo_pago", OdbcType.Numeric).Value = obj.id_metodo_pago;
                parameters.Add("@id_regimen_fiscal", OdbcType.Numeric).Value = obj.id_regimen_fiscal;
                parameters.Add("@cfd_num_cta_pago", OdbcType.VarChar).Value = obj.cfd_num_cta_pago;
                parameters.Add("@asn", OdbcType.VarChar).Value = obj.asn ?? "";
                parameters.Add("@pedido", OdbcType.Numeric).Value = obj.pedido;
                parameters.Add("@remision", OdbcType.Numeric).Value = obj.remision;
                parameters.Add("@tipo_clase", OdbcType.Char).Value = obj.tipo_clase;
                parameters.Add("@pais", OdbcType.Char).Value = obj.pais;
                parameters.Add("@estado", OdbcType.Char).Value = obj.estado;
                parameters.Add("@autoriza_factura", OdbcType.Char).Value = obj.entregado;
                parameters.Add("@cfd_no_certificado", OdbcType.VarChar).Value = "00001000000400291734";
                parameters.Add("@cfd_certificado_sello", OdbcType.VarChar).Value = "MIIGXDCCBESgAwIBAgIUMDAwMDEwMDAwMDA0MDAyOTE3MzQwDQYJKoZIhvcNAQELBQAwggGyMTgwNgYDVQQDDC9BLkMuIGRlbCBTZXJ2aWNpbyBkZSBBZG1pbmlzdHJhY2nDs24gVHJpYnV0YXJpYTEvMC0GA1UECgwmU2VydmljaW8gZGUgQWRtaW5pc3RyYWNpw7NuIFRyaWJ1dGFyaWExODA2BgNVBAsML0FkbWluaXN0cmFjacOzbiBkZSBTZWd1cmlkYWQgZGUgbGEgSW5mb3JtYWNpw7NuMR8wHQYJKoZIhvcNAQkBFhBhY29kc0BzYXQuZ29iLm14MSYwJAYDVQQJDB1Bdi4gSGlkYWxnbyA3NywgQ29sLiBHdWVycmVybzEOMAwGA1UEEQwFMDYzMDAxCzAJBgNVBAYTAk1YMRkwFwYDVQQIDBBEaXN0cml0byBGZWRlcmFsMRQwEgYDVQQHDAtDdWF1aHTDqW1vYzEVMBMGA1UELRMMU0FUOTcwNzAxTk4zMV0wWwYJKoZIhvcNAQkCDE5SZXNwb25zYWJsZTogQWRtaW5pc3RyYWNpw7NuIENlbnRyYWwgZGUgU2VydmljaW9zIFRyaWJ1dGFyaW9zIGFsIENvbnRyaWJ1eWVudGUwHhcNMTUwOTAxMjEwNjM3WhcNMTkwOTAxMjEwNjM3WjCB/DEzMDEGA1UEAxMqR1JVUE8gQ09OU1RSVUNDSU9ORVMgUExBTklGSUNBREFTIFNBIERFIENWMTMwMQYDVQQpEypHUlVQTyBDT05TVFJVQ0NJT05FUyBQTEFOSUZJQ0FEQVMgU0EgREUgQ1YxMzAxBgNVBAoTKkdSVVBPIENPTlNUUlVDQ0lPTkVTIFBMQU5JRklDQURBUyBTQSBERSBDVjElMCMGA1UELRMcR0NQODAwMzI0RkoxIC8gU09MRjYyMDQxMVRIMjEeMBwGA1UEBRMVIC8gU09MRjYyMDQxMUhTUlROUjA2MRQwEgYDVQQLEwtDT05TVFJVUExBTjCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAIeP7DHPp2o1yfYyur9e5KFos51L4f7J0t4dDd5J3t5Q3xe2IsZVlDIvX11MFP3J0pH7125Co686ONiQX/WIyiZubXEi0lT0rLTw3Z7XffNEkD0HOYrL1Cvq12lel2fJwQs/ShCLEwYomTUGvSEpxhpVxQ+MkzLuo3hUpRHi5whEQUnfXPck7Pw1mNXkdZuLWoCjFEPcAnHpXuV3Q3LDlsr4CwIFGVO8zKCJwUNgpawsi9OOL5+pBvV5Gf0DFDVHOgRk5idsliP+ivMXvsJdWs0CC4zfsAWx+6HVHYiCHrh9Tg+khIGrif2v0To1IlXOohbw0VQ9vBqM0QkcG7FI+lsCAwEAAaMdMBswDAYDVR0TAQH/BAIwADALBgNVHQ8EBAMCBsAwDQYJKoZIhvcNAQELBQADggIBAHufyAd5kI324Cl66svdHVxg2+Ru3edKHRMgM7uIQplECS3z16EN4bWnphgnmIWboaGxeP8eWBNNWqSHNu9NCYMIdFETAG3zr6zpaKPIWEPfs8v20OEHHVJmXZ3xM0ysLlTYkyGcPvPhZoPXoBRkhNfEzRKLZZpTz5DJS/NEC8l9r90FVZDbMHSdtHtRGSyeupveB4GjxOaexS4+VSd4qPNmXfIhmPqNeedcIDk6EeP8gJaHU97LpMX5b5uGyWwLJFmysktQy6zjTTQd6LITmRBheUVfaY5S2HeJVTZjQR2gTEhbOLJpMoMmLzWrWOts1i4l2OOmIg83CEg5cD7XUI4NrWQ5taRvHbuLqWVjXLDB/AnuGau+Qnx0RNBUbvNo1V/0qJ5F49S3D9mJHjsTkzfRi22MOqBxz23RQr/SuJTQC34z1rZpN4ERJYP82aoNSKOU/lmUcZYznW8eGIeEAfXia79QNi29Jm8HAhiPzHS+Pb5ebwNweRoCixXpfFrYRbfLrTZeafcbGG9qIzuqTdg92mMBMij6GU8HgXyEXw3l6q0lxdUG0rhlxFz7Vog+XuZU4tDWg6tDND9PKUou6I52l9eQnqXxPc4MKoLJ4lWwhKZXJMZb+ZFe7860Ah7eDFhUbvvOoD+2aOycAoD+45+h23p4nkTFUCgo3ANob/XU";
                parameters.Add("@cfd_cadena_original", OdbcType.VarChar).Value = generarCadenaOriginal3_2(obj, lst, out sello);
                parameters.Add("@cfd_sello_digital", OdbcType.VarChar).Value = sello;

                int id = 1;
                if (obj.entregado.Equals("S"))
                {
                    command.Connection = con;
                    id = command.ExecuteNonQuery();
                }
                c.Close(con);
                return id;
            }
            return 0;
        }

        void SavePartidaPedido(PartidaDTO obj, int pedido, decimal iva, string obs)
        {
            var c = new Conexion();
            var con = c.ConnectPrueba();
            if (con != null)
            {
                string insertQuery = "INSERT INTO sf_pedidos_det (pedido, partida, insumo, cantidad, precio, importe, unidad, porcent_descto, cia_sucursal, estatus, cant_pedido, cant_facturada, factor_uni, fec_entrega, cant_cancelada, muestra, cant_remision, porcen_iva_partida, linea) " +
                                        "VALUES (?, ?, ?, ?, ?, ?, ?, ?, 1, 'T', ?, 0, 1, ?, 0, 'N', ?, ?, ?)";


                OdbcCommand command = new OdbcCommand(insertQuery);
                OdbcParameterCollection parameters = command.Parameters;

                parameters.Add("@pedido", OdbcType.Numeric).Value = pedido;
                parameters.Add("@partida", OdbcType.Numeric).Value = obj.Partida;
                parameters.Add("@insumo", OdbcType.Numeric).Value = obj.Insumo;
                parameters.Add("@cantidad", OdbcType.Numeric).Value = obj.Cantidad;
                parameters.Add("@precio", OdbcType.Numeric).Value = obj.Precio;
                parameters.Add("@importe", OdbcType.Numeric).Value = obj.Importe;
                parameters.Add("@unidad", OdbcType.Char).Value = obj.Unidad;
                parameters.Add("@porcent_descto", OdbcType.Numeric).Value = obj.Descuento;
                parameters.Add("@cant_pedido", OdbcType.Numeric).Value = obj.Cantidad;
                parameters.Add("@fec_entrega", OdbcType.Date).Value = DateTime.Now;
                parameters.Add("@cant_remision", OdbcType.Numeric).Value = obj.Cantidad;
                parameters.Add("@porcen_iva_partida", OdbcType.Numeric).Value = iva;
                parameters.Add("@linea", OdbcType.VarChar).Value = obs;

                command.Connection = con;
                var id = command.ExecuteNonQuery();
                c.Close(con);
            }
        }

        void SavePartidaRemision(PartidaDTO obj, int remision, int pedido, string obs)
        {
            var c = new Conexion();
            var con = c.ConnectPrueba();
            if (con != null)
            {
                string insertQuery = "INSERT INTO sf_remision_det (remision, partida, insumo, cantidad, precio, importe, unidad, cant_remision, precio_remision, sucursal, pedido, ped_part, linea) " +
                                        "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, 1, ?, ?, ?)";


                OdbcCommand command = new OdbcCommand(insertQuery);
                OdbcParameterCollection parameters = command.Parameters;

                parameters.Add("@remision", OdbcType.Numeric).Value = remision;
                parameters.Add("@partida", OdbcType.Numeric).Value = obj.Partida;
                parameters.Add("@insumo", OdbcType.Numeric).Value = obj.Insumo;
                parameters.Add("@cantidad", OdbcType.Numeric).Value = obj.Cantidad;
                parameters.Add("@precio", OdbcType.Numeric).Value = obj.Precio;
                parameters.Add("@importe", OdbcType.Numeric).Value = obj.Importe;
                parameters.Add("@unidad", OdbcType.Char).Value = obj.Unidad;
                parameters.Add("@cant_remision", OdbcType.Numeric).Value = obj.Cantidad;
                parameters.Add("@precio_remision", OdbcType.Numeric).Value = obj.Precio;
                parameters.Add("@pedido", OdbcType.Numeric).Value = pedido;
                parameters.Add("@ped_part", OdbcType.Numeric).Value = obj.Partida;
                parameters.Add("@linea", OdbcType.Char).Value = obs;

                command.Connection = con;
                var id = command.ExecuteNonQuery();
                c.Close(con);
            }
        }

        void SavePartidaFactura(PartidaDTO obj, int factura, int pedido, int remision, string obs, string entregado)
        {
            var c = new Conexion();
            var con = c.ConnectPrueba();
            if (con != null)
            {
                string insertQuery = "INSERT INTO sf_facturas_det (factura, pedido, partida, insumo, cantidad, precio, importe, unidad, cia_sucursal, numero_nc, porcent_descto, remision, linea) " +
                                        "VALUES (?, ?, ?, ?, ?, ?, ?, ?, 1, 0, ?, ? ,?)";


                OdbcCommand command = new OdbcCommand(insertQuery);
                OdbcParameterCollection parameters = command.Parameters;

                parameters.Add("@factura", OdbcType.Numeric).Value = factura;
                parameters.Add("@pedido", OdbcType.Numeric).Value = pedido;
                parameters.Add("@partida", OdbcType.Numeric).Value = obj.Partida;
                parameters.Add("@insumo", OdbcType.Numeric).Value = obj.Insumo;
                parameters.Add("@cantidad", OdbcType.Numeric).Value = obj.Cantidad;
                parameters.Add("@precio", OdbcType.Numeric).Value = obj.Precio;
                parameters.Add("@importe", OdbcType.Numeric).Value = obj.Importe;
                parameters.Add("@unidad", OdbcType.Char).Value = obj.Unidad;
                parameters.Add("@porcent_descto", OdbcType.Numeric).Value = obj.Descuento;
                parameters.Add("@remision", OdbcType.Numeric).Value = remision;
                parameters.Add("@linea", OdbcType.Char).Value = obs;

                int id = 1;
                if (entregado.Equals("S"))
                {
                    command.Connection = con;
                    id = command.ExecuteNonQuery();
                }
                c.Close(con);
            }
        }

        void SavePartidaDesc(PartidaDTO obj, int pedido, int usuario)
        {
            var c = new Conexion();
            var con = c.ConnectPrueba();
            if (con != null)
            {
                string insertQuery = "INSERT INTO sf_pedidos_dcto (pedido, partida, insumo, porcent_descto, cia_sucursal, usuario, descuento, fecha_hora) " +
                                        "VALUES (?, ?, ?, ?, 1, ?, ?, ?)";


                OdbcCommand command = new OdbcCommand(insertQuery);
                OdbcParameterCollection parameters = command.Parameters;

                parameters.Add("@pedido", OdbcType.Numeric).Value = pedido;
                parameters.Add("@partida", OdbcType.Numeric).Value = obj.Partida;
                parameters.Add("@insumo", OdbcType.Numeric).Value = obj.Insumo;
                parameters.Add("@porcent_descto", OdbcType.Numeric).Value = obj.Descuento;
                parameters.Add("@usuario", OdbcType.Numeric).Value = usuario;
                parameters.Add("@descuento", OdbcType.Numeric).Value = obj.DescuentoDinero;
                parameters.Add("@fecha_hora", OdbcType.DateTime).Value = DateTime.Now;

                command.Connection = con;
                var id = command.ExecuteNonQuery();
                c.Close(con);
            }
        }

        void SaveLstRentencio(PartidaDTO obj, int pedido)
        {
            var c = new Conexion();
            var con = c.ConnectPrueba();
            if (con != null)
            {
                string insertQuery = "INSERT INTO sf_pedido_retencion (pedido, partida, insumo, cantidad, importe, porc_ret, cia_sucursal, ret_iva) " +
                                        "VALUES (?, ?, ?, ?, ?, ?, 1 ,?)";


                OdbcCommand command = new OdbcCommand(insertQuery);
                OdbcParameterCollection parameters = command.Parameters;

                parameters.Add("@pedido", OdbcType.Numeric).Value = pedido;
                parameters.Add("@partida", OdbcType.Numeric).Value = obj.Partida;
                parameters.Add("@insumo", OdbcType.Numeric).Value = obj.Insumo;
                parameters.Add("@cantidad", OdbcType.Numeric).Value = obj.Importe;
                parameters.Add("@importe", OdbcType.Numeric).Value = obj.Importe;
                parameters.Add("@porc_ret", OdbcType.Numeric).Value = obj.Descuento;
                parameters.Add("@ret_iva", OdbcType.Char).Value = (obj.Importe * (obj.IvaRetenido.Equals("S") ? 16 : 0 / 100));

                command.Connection = con;
                var id = command.ExecuteNonQuery();
                c.Close(con);
            }
        }

        void SaveRemisionRentencio(PartidaDTO obj, int pedido, int remision)
        {
            var c = new Conexion();
            var con = c.ConnectPrueba();
            if (con != null)
            {
                string insertQuery = "INSERT INTO sf_remision_retencion (pedido, partida, insumo, cantidad, importe, porc_ret, cia_sucursal, ret_iva, remision) " +
                                        "VALUES (?, ?, ?, ?, ?, ?, 1 , ?, ?)";


                OdbcCommand command = new OdbcCommand(insertQuery);
                OdbcParameterCollection parameters = command.Parameters;

                parameters.Add("@pedido", OdbcType.Numeric).Value = pedido;
                parameters.Add("@partida", OdbcType.Numeric).Value = obj.Partida;
                parameters.Add("@insumo", OdbcType.Numeric).Value = obj.Insumo;
                parameters.Add("@cantidad", OdbcType.Numeric).Value = obj.Importe;
                parameters.Add("@importe", OdbcType.Numeric).Value = obj.Importe;
                parameters.Add("@porc_ret", OdbcType.Numeric).Value = obj.Descuento;
                parameters.Add("@ret_iva", OdbcType.Numeric).Value = (obj.Importe * (obj.IvaRetenido.Equals("S") ? 16 : 0 / 100));
                parameters.Add("@remision", OdbcType.Numeric).Value = remision;

                command.Connection = con;
                var id = command.ExecuteNonQuery();
                c.Close(con);
            }
        }

        void SaveFacturaRentencio(PartidaDTO obj, int pedido, int factura, int remision, string entregado)
        {
            var c = new Conexion();
            var con = c.ConnectPrueba();
            if (con != null)
            {
                string insertQuery = "INSERT INTO sf_factura_retencion (pedido, factura, partida, insumo, cantidad, importe, porc_ret, cia_sucursal, ret_iva, remision) " +
                                        "VALUES (?, ?, ?, ?, ?, ?, ?, 1 , ?, ?)";


                OdbcCommand command = new OdbcCommand(insertQuery);
                OdbcParameterCollection parameters = command.Parameters;

                parameters.Add("@pedido", OdbcType.Numeric).Value = pedido;
                parameters.Add("@factura", OdbcType.Numeric).Value = factura;
                parameters.Add("@partida", OdbcType.Numeric).Value = obj.Partida;
                parameters.Add("@insumo", OdbcType.Numeric).Value = obj.Insumo;
                parameters.Add("@cantidad", OdbcType.Numeric).Value = obj.Importe;
                parameters.Add("@importe", OdbcType.Numeric).Value = obj.Importe;
                parameters.Add("@porc_ret", OdbcType.Numeric).Value = obj.Descuento;
                parameters.Add("@ret_iva", OdbcType.Numeric).Value = (obj.Importe * (obj.IvaRetenido.Equals("S") ? 16 : 0 / 100));
                parameters.Add("@remision", OdbcType.Numeric).Value = remision;

                int id = 1;
                if (entregado.Equals("S"))
                {
                    command.Connection = con;
                    id = command.ExecuteNonQuery();
                }
                c.Close(con);
            }
        }

        public int UpdateFacura(BigFacturaDTO obj, List<PartidaDTO> lst, List<PartidaDTO> lstRentencion)
        {
            int response = 0;
            var estado = getEstado(obj.ciudad);
            obj.estado = estado.estado;
            obj.pais = estado.pais;
            try
            {
                var pedido = existPedido(obj.pedido) ? UpdateTblPedido(obj) : SaveTblPedido(obj);
                if (pedido == 1)
                {
                    var remision = existRemision(obj.pedido) ? UpdateTblRemision(obj) : SaveTblRemision(obj);
                    if (remision == 1)
                    {
                        var factura = existFactura(obj.pedido) ? UpdateTblFactura(obj) : SaveTblFactura(obj, lst);
                        if (factura == 1)
                        {
                            UpdateLstPartida(lst, lstRentencion, obj.factura, obj.pedido, obj.remision, obj.elaboro, obj.obs, obj.entregado, obj.iva);
                            response = 1;
                        }
                    }
                }
            }
            catch (Exception)
            {
                response = 0;
            }
            return response;
        }

        void UpdateLstPartida(List<PartidaDTO> lst, List<PartidaDTO> lstRentencion, int factura, int pedido, int remision, int usuario, string obs, string entregado, decimal iva)
        {
            foreach (var item in lst)
            {
                if (existPartidaPedido(item.Insumo, pedido))
                    UpdatePartidaPedido(item, pedido, iva, obs);
                else
                    SavePartidaPedido(item, pedido, iva, obs);

                if (item.Descuento > 0)
                {
                    if (existPartidaDesc(item.Insumo, pedido))
                        UpdatePartidaDesc(item, pedido, usuario);
                    else
                        SavePartidaDesc(item, pedido, usuario);
                }
                if (existPartidaRemision(item.Insumo, remision))
                    UpdatePartidaRemision(item, remision);
                else
                    SavePartidaRemision(item, remision, pedido, obs);
                if (existPartidaFactura(item.Insumo, factura))
                    UpdatePartidaFactura(item, factura, obs);
                else
                    SavePartidaFactura(item, factura, pedido, remision, obs, entregado);
            }
            if (lstRentencion != null)
            {
                foreach (var item in lstRentencion)
                {
                    if (existPedidoRentencion(item.Insumo, pedido))
                        UpdateRentencio(item, pedido);
                    else
                        SaveLstRentencio(item, pedido);
                    if (existRemisionRentencion(item.Insumo, pedido))
                        UpdateRemisionRentencio(item, pedido);
                    else
                        SaveRemisionRentencio(item, pedido, remision);
                    if (existFacturaRentencion(item.Insumo, pedido))
                        UpdateFacuraRentencio(item, pedido);
                    else
                        SaveFacturaRentencio(item, pedido, factura, remision, entregado);
                }
            }
        }

        int UpdateTblPedido(BigFacturaDTO obj)
        {
            var c = new Conexion();
            var con = c.ConnectPrueba();
            if (con.State.Equals("Open"))
            {
                con.Open();
            }
            if (con != null)
            {
                string insertQuery = "UPDATE sf_pedidos SET numcte = ?, sucursal = ?, fecha = ?, requisicion = ?,  vendedor = ?, " +
                    "cond_pago = ?, moneda = ?, tipo_cambio = ?, sub_total = ?, iva = ?, total = ?, porcent_iva = ?, tipo = ?, porcent_descto = ?, descuento = ?, estatus = ?, " +
                    "condicion_entrega = ?, tipo_flete = ?, obs = ?,  tipo_pedido = ?, cc = ?, tm = ?, elaboro = ?, cia_sucursal = ?, tipo_credito = ?, retencion = ?, aplica_total_antes_iva = ?, " +
                    "total_dec = ?, zona = ?, lista_precios = 0, usuario = ?, fecha_hora = ?, status_autorizado = 'A' WHERE pedido =" + obj.pedido;

                OdbcCommand command = new OdbcCommand(insertQuery);
                OdbcParameterCollection parameters = command.Parameters;

                parameters.Add("@numcte", OdbcType.Numeric).Value = obj.numcte;
                parameters.Add("@sucursal", OdbcType.Numeric).Value = obj.sucursal;
                parameters.Add("@fecha", OdbcType.Date).Value = obj.fecha;
                parameters.Add("@requisicion", OdbcType.Char).Value = obj.requisicion ?? "";
                parameters.Add("@vendedor", OdbcType.Numeric).Value = obj.vendedor;
                parameters.Add("@cond_pago", OdbcType.Numeric).Value = obj.cond_pago;
                parameters.Add("@moneda", OdbcType.Char).Value = obj.moneda;
                parameters.Add("@tipo_cambio", OdbcType.Numeric).Value = obj.tipo_cambio;
                parameters.Add("@sub_total", OdbcType.Numeric).Value = obj.sub_total;
                parameters.Add("@iva", OdbcType.Numeric).Value = obj.iva;
                parameters.Add("@total", OdbcType.Numeric).Value = obj.total;
                parameters.Add("@porcent_iva", OdbcType.Numeric).Value = obj.porcent_iva;
                parameters.Add("@tipo", OdbcType.Char).Value = obj.tipo;
                parameters.Add("@porcent_descto", OdbcType.Numeric).Value = obj.porcent_descto;
                parameters.Add("@descuento", OdbcType.Numeric).Value = obj.descuento;
                parameters.Add("@estatus", OdbcType.Char).Value = "T";
                parameters.Add("@condicion_entrega", OdbcType.Char).Value = obj.condicion_entrega.Equals("--Selecione--") ? "n/a" : obj.condicion_entrega;
                parameters.Add("@tipo_flete", OdbcType.Char).Value = obj.tipo_flete.Equals("--Selecione--") ? "n/a" : obj.tipo_flete;
                parameters.Add("@obs", OdbcType.Char).Value = obj.obs ?? "";
                parameters.Add("@tipo_pedido", OdbcType.Char).Value = obj.tipo_pedido;
                parameters.Add("@cc", OdbcType.Char).Value = obj.cc;
                parameters.Add("@tm", OdbcType.Numeric).Value = obj.tm;
                parameters.Add("@elaboro", OdbcType.Numeric).Value = obj.elaboro;
                parameters.Add("@cia_sucursal", OdbcType.Numeric).Value = obj.cia_sucursal;
                parameters.Add("@tipo_credito", OdbcType.Char).Value = obj.tipo_credito;
                parameters.Add("@retencion", OdbcType.Char).Value = obj.retencion;
                parameters.Add("@aplica_total_antes_iva", OdbcType.Numeric).Value = obj.aplica_total_antes_iva;
                parameters.Add("@total_dec", OdbcType.Char).Value = obj.total_dec;
                parameters.Add("@zona", OdbcType.Numeric).Value = obj.zona;
                parameters.Add("@usuario", OdbcType.Numeric).Value = obj.vendedor;
                parameters.Add("@fecha_hora", OdbcType.DateTime).Value = DateTime.Now;
                parameters.Add("@status_autorizado", OdbcType.Char).Value = "A";

                command.Connection = con;
                var id = command.ExecuteNonQuery();
                c.Close(con);
                return id;
            }
            return 0;
        }

        int UpdateTblRemision(BigFacturaDTO obj)
        {
            var c = new Conexion();
            var con = c.ConnectPrueba();
            if (con != null)
            {
                string insertQuery = "UPDATE sf_remision SET sucursal = ?, fecha = ?, rfc = ?, nombre = ?, direccion = ?, ciudad = ?, cp = ?, telefono = ?, transporte = ?, talon = ?, " +
                    "consignado = ?, observaciones = ?, moneda = ?, tipo_cambio  = ?, sub_total = ?, iva = ?, total = ?, porcent_iva = ?, porcent_descto = ?, elaboro = ?, tipo_flete = ?, " +
                    "descuento = ?, estatus = ?, entregado = ?, retencion = ? WHERE pedido = " + obj.pedido;

                OdbcCommand command = new OdbcCommand(insertQuery);
                OdbcParameterCollection parameters = command.Parameters;

                parameters.Add("@sucursal", OdbcType.Numeric).Value = obj.sucursal;
                parameters.Add("@fecha", OdbcType.Date).Value = obj.fecha;
                parameters.Add("@rfc", OdbcType.Char).Value = obj.rfc;
                parameters.Add("@nombre", OdbcType.Char).Value = obj.nombre;
                parameters.Add("@direccion", OdbcType.Char).Value = obj.direccion;
                parameters.Add("@ciudad", OdbcType.Char).Value = obj.ciudad;
                parameters.Add("@cp", OdbcType.Char).Value = obj.cp ?? "1";
                parameters.Add("@telefono", OdbcType.Char).Value = obj.telefono1 ?? "1";
                parameters.Add("@transporte", OdbcType.Char).Value = obj.transporte ?? "1";
                parameters.Add("@talon", OdbcType.Char).Value = obj.talon ?? "1";
                parameters.Add("@consignado", OdbcType.Char).Value = obj.consignado ?? "1";
                parameters.Add("@observaciones", OdbcType.Char).Value = obj.obs ?? "";
                parameters.Add("@moneda", OdbcType.Char).Value = obj.moneda;
                parameters.Add("@tipo_cambio", OdbcType.Numeric).Value = obj.tipo_cambio;
                parameters.Add("@sub_total", OdbcType.Numeric).Value = obj.sub_total;
                parameters.Add("@iva", OdbcType.Numeric).Value = obj.iva;
                parameters.Add("@total", OdbcType.Numeric).Value = obj.total;
                parameters.Add("@porcent_iva", OdbcType.Numeric).Value = obj.porcent_iva;
                parameters.Add("@porcent_descto", OdbcType.Numeric).Value = obj.porcent_descto;
                parameters.Add("@elaboro", OdbcType.Numeric).Value = obj.elaboro;
                parameters.Add("@tipo_flete", OdbcType.Char).Value = obj.tipo_flete.Equals("--Selecione--") ? "" : obj.tipo_flete;
                parameters.Add("@descuento", OdbcType.Numeric).Value = obj.descuento;
                parameters.Add("@estatus", OdbcType.Char).Value = "I";
                parameters.Add("@entregado", OdbcType.Char).Value = obj.entregado;
                parameters.Add("@retencion", OdbcType.Char).Value = obj.retencion;

                command.Connection = con;
                var id = command.ExecuteNonQuery();
                c.Close(con);
                return id;
            }
            return 0;
        }

        int UpdateTblFactura(BigFacturaDTO obj)
        {
            var c = new Conexion();
            var con = c.ConnectPrueba();
            if (con != null)
            {
                string insertQuery = "UPDATE sf_facturas SET numcte = ?, sucursal = ?, fecha = ?, requisicion = ?, vendedor = ?, consignado = ?, transporte = ?, cond_pago = ?, talon = ?, " +
                    "moneda = ?, tipo_cambio = ?, sub_total = ?, iva = ?, total = ?, porcent_iva = ?, tipo = ?, cc = ?, tm = ?, descuento = ?, porcent_descto = ?, nombre = ?, rfc = ?, " +
                    "direccion = ?, ciudad = ?, cp = ?, estatus = ?, obs = ?, cia_sucursal = ?, cfd_folio = ?, cfd_serie = ?, gsdb = ?, id_metodo_pago = ?, id_regimen_fiscal = ?, " +
                    "cfd_num_cta_pago = ?, asn = ?, numero_nc = 0, tipo_nc = 0, lista_precio = 0, tipo_clase = ?, autoriza_factura = ?, pais = ?, estado = ? " +
                                        " WHERE pedido = " + obj.pedido;

                OdbcCommand command = new OdbcCommand(insertQuery);
                OdbcParameterCollection parameters = command.Parameters;

                parameters.Add("@numcte", OdbcType.Numeric).Value = obj.numcte;
                parameters.Add("@sucursal", OdbcType.Numeric).Value = obj.sucursal;
                parameters.Add("@fecha", OdbcType.Date).Value = obj.fecha;
                parameters.Add("@requisicion", OdbcType.Char).Value = obj.requisicion ?? "";
                parameters.Add("@vendedor", OdbcType.Numeric).Value = obj.vendedor;
                parameters.Add("@consignado", OdbcType.Char).Value = obj.consignado ?? "1";
                parameters.Add("@transporte", OdbcType.Char).Value = obj.transporte ?? "1";
                parameters.Add("@cond_pago", OdbcType.Numeric).Value = obj.cond_pago;
                parameters.Add("@talon", OdbcType.Numeric).Value = obj.talon ?? "1";
                parameters.Add("@moneda", OdbcType.Char).Value = obj.moneda;
                parameters.Add("@tipo_cambio", OdbcType.Numeric).Value = obj.tipo_cambio;
                parameters.Add("@sub_total", OdbcType.Numeric).Value = obj.sub_total;
                parameters.Add("@iva", OdbcType.Numeric).Value = obj.iva;
                parameters.Add("@total", OdbcType.Numeric).Value = obj.total;
                parameters.Add("@porcent_iva", OdbcType.Numeric).Value = obj.porcent_iva;
                parameters.Add("@tipo", OdbcType.Char).Value = obj.tipo;
                parameters.Add("@cc", OdbcType.Char).Value = obj.cc;
                parameters.Add("@tm", OdbcType.Numeric).Value = obj.tm;
                parameters.Add("@descuento", OdbcType.Numeric).Value = obj.descuento;
                parameters.Add("@porcent_descto", OdbcType.Numeric).Value = obj.porcent_descto;
                parameters.Add("@nombre", OdbcType.Char).Value = obj.nombre;
                parameters.Add("@rfc", OdbcType.Char).Value = obj.rfc;
                parameters.Add("@direccion", OdbcType.Char).Value = obj.direccion;
                parameters.Add("@ciudad", OdbcType.Char).Value = obj.ciudad;
                parameters.Add("@cp", OdbcType.Char).Value = obj.cp ?? "";
                parameters.Add("@estatus", OdbcType.Char).Value = "I";
                parameters.Add("@obs", OdbcType.Char).Value = obj.obs ?? "";
                parameters.Add("@cia_sucursal", OdbcType.Numeric).Value = obj.cia_sucursal;
                parameters.Add("@cfd_folio", OdbcType.Numeric).Value = obj.cfd_folio ?? "";
                parameters.Add("@cfd_serie", OdbcType.VarChar).Value = obj.cfd_serie;
                parameters.Add("@gsdb", OdbcType.VarChar).Value = obj.gsdb ?? "";
                parameters.Add("@id_metodo_pago", OdbcType.Numeric).Value = obj.id_metodo_pago;
                parameters.Add("@id_regimen_fiscal", OdbcType.Numeric).Value = obj.id_regimen_fiscal;
                parameters.Add("@cfd_num_cta_pago", OdbcType.VarChar).Value = obj.cfd_num_cta_pago;
                parameters.Add("@asn", OdbcType.VarChar).Value = obj.asn ?? "";
                parameters.Add("@tipo_clase", OdbcType.Char).Value = obj.tipo_clase;
                parameters.Add("@autoriza_factura", OdbcType.Char).Value = obj.entregado;
                parameters.Add("@pais", OdbcType.Char).Value = obj.pais;
                parameters.Add("@estado", OdbcType.Char).Value = obj.estado;

                command.Connection = con;
                var id = command.ExecuteNonQuery();
                c.Close(con);
                return id;
            }
            return 0;
        }

        void UpdateRentencio(PartidaDTO obj, int pedido)
        {
            var c = new Conexion();
            var con = c.ConnectPrueba();
            if (con != null)
            {
                string insertQuery = "UPDATE sf_pedido_retencion SET cantidad = ?, importe = ?, porc_ret = ?, ret_iva = ?" +
                                        "WHERE insumo = " + obj.Insumo + " AND pedido  = " + pedido;


                OdbcCommand command = new OdbcCommand(insertQuery);
                OdbcParameterCollection parameters = command.Parameters;

                parameters.Add("@cantidad", OdbcType.Numeric).Value = obj.Importe;
                parameters.Add("@importe", OdbcType.Numeric).Value = obj.Importe;
                parameters.Add("@porc_ret", OdbcType.Numeric).Value = obj.Descuento;
                parameters.Add("@ret_iva", OdbcType.Char).Value = (obj.Importe * (obj.IvaRetenido.Equals("S") ? 16 : 0 / 100));

                command.Connection = con;
                var id = command.ExecuteNonQuery();
                c.Close(con);
            }
        }

        void UpdateRemisionRentencio(PartidaDTO obj, int pedido)
        {
            var c = new Conexion();
            var con = c.ConnectPrueba();
            if (con != null)
            {
                string insertQuery = "UPDATE sf_remision_retencion SET cantidad = ?, importe = ?, porc_ret = ?, ret_iva = ?" +
                                        "WHERE insumo = " + obj.Insumo + " AND pedido  = " + pedido;


                OdbcCommand command = new OdbcCommand(insertQuery);
                OdbcParameterCollection parameters = command.Parameters;

                parameters.Add("@cantidad", OdbcType.Numeric).Value = obj.Importe;
                parameters.Add("@importe", OdbcType.Numeric).Value = obj.Importe;
                parameters.Add("@porc_ret", OdbcType.Numeric).Value = obj.Descuento;
                parameters.Add("@ret_iva", OdbcType.Numeric).Value = (obj.Importe * (obj.IvaRetenido.Equals("S") ? 16 : 0 / 100));

                command.Connection = con;
                var id = command.ExecuteNonQuery();
                c.Close(con);
            }
        }

        void UpdateFacuraRentencio(PartidaDTO obj, int pedido)
        {
            var c = new Conexion();
            var con = c.ConnectPrueba();
            if (con != null)
            {
                string insertQuery = "UPDATE sf_factura_retencion SET cantidad = ?, importe = ?, porc_ret = ?, ret_iva = ?" +
                                        "WHERE insumo = " + obj.Insumo + " AND pedido  = " + pedido;


                OdbcCommand command = new OdbcCommand(insertQuery);
                OdbcParameterCollection parameters = command.Parameters;

                parameters.Add("@cantidad", OdbcType.Numeric).Value = obj.Importe;
                parameters.Add("@importe", OdbcType.Numeric).Value = obj.Importe;
                parameters.Add("@porc_ret", OdbcType.Numeric).Value = obj.Descuento;
                parameters.Add("@ret_iva", OdbcType.Numeric).Value = (obj.Importe * (obj.IvaRetenido.Equals("S") ? 16 : 0 / 100));

                command.Connection = con;
                var id = command.ExecuteNonQuery();
                c.Close(con);
            }
        }

        void UpdatePartidaFactura(PartidaDTO obj, int factura, string obs)
        {
            var c = new Conexion();
            var con = c.ConnectPrueba();
            if (con != null)
            {
                string insertQuery = "UPDATE sf_facturas_det SET cantidad = ?, precio = ?, importe = ?, unidad = ?, porcent_descto = ?, linea = ?, cant_factura = ? " +
                                        "WHERE factura = " + factura;


                OdbcCommand command = new OdbcCommand(insertQuery);
                OdbcParameterCollection parameters = command.Parameters;

                parameters.Add("@cantidad", OdbcType.Numeric).Value = obj.Cantidad;
                parameters.Add("@precio", OdbcType.Numeric).Value = obj.Precio;
                parameters.Add("@importe", OdbcType.Numeric).Value = obj.Importe;
                parameters.Add("@unidad", OdbcType.Char).Value = obj.Unidad;
                parameters.Add("@porcent_descto", OdbcType.Numeric).Value = obj.Descuento;
                parameters.Add("@linea", OdbcType.Char).Value = obs;
                parameters.Add("@cant_factura", OdbcType.Numeric).Value = obj.Cantidad;

                command.Connection = con;
                var id = command.ExecuteNonQuery();
                c.Close(con);
            }
        }

        void UpdatePartidaRemision(PartidaDTO obj, int remision)
        {
            var c = new Conexion();
            var con = c.ConnectPrueba();
            if (con != null)
            {
                string insertQuery = "UPDATE sf_remision_det SET cantidad = ?, precio = ?, importe = ?, unidad = ?, cant_remision = ?, precio_remision = ?" +
                                        "WHERE insumo = " + obj.Insumo + "AND remision = " + remision;


                OdbcCommand command = new OdbcCommand(insertQuery);
                OdbcParameterCollection parameters = command.Parameters;

                parameters.Add("@cantidad", OdbcType.Numeric).Value = obj.Cantidad;
                parameters.Add("@precio", OdbcType.Numeric).Value = obj.Precio;
                parameters.Add("@importe", OdbcType.Numeric).Value = obj.Importe;
                parameters.Add("@unidad", OdbcType.Char).Value = obj.Unidad;
                parameters.Add("@cant_remision", OdbcType.Numeric).Value = obj.Cantidad;
                parameters.Add("@precio_remision", OdbcType.Numeric).Value = obj.Precio;

                command.Connection = con;
                var id = command.ExecuteNonQuery();
                c.Close(con);
            }
        }

        void UpdatePartidaPedido(PartidaDTO obj, int pedido, decimal iva, string obs)
        {
            var c = new Conexion();
            var con = c.ConnectPrueba();
            if (con != null)
            {
                string insertQuery = "UPDATE sf_pedidos_det SET cantidad = ?, precio = ?, importe = ?, unidad = ?, porcent_descto = ?, cant_pedido = ?, cant_remision = ?, porcen_iva_partida = ?,  linea = ? " +
                                        "WHERE pedido = " + pedido + "AND insumo = " + obj.Insumo + " AND partida = " + obj.Partida;


                OdbcCommand command = new OdbcCommand(insertQuery);
                OdbcParameterCollection parameters = command.Parameters;

                parameters.Add("@cantidad", OdbcType.Numeric).Value = obj.Cantidad;
                parameters.Add("@precio", OdbcType.Numeric).Value = obj.Precio;
                parameters.Add("@importe", OdbcType.Numeric).Value = obj.Importe;
                parameters.Add("@unidad", OdbcType.Char).Value = obj.Unidad;
                parameters.Add("@porcent_descto", OdbcType.Numeric).Value = obj.Descuento;
                parameters.Add("@cant_pedido", OdbcType.Numeric).Value = obj.Cantidad;
                parameters.Add("@cant_remision", OdbcType.Numeric).Value = obj.Cantidad;
                parameters.Add("@porcen_iva_partida", OdbcType.Numeric).Value = iva;
                parameters.Add("@linea", OdbcType.VarChar).Value = obs;

                command.Connection = con;
                var id = command.ExecuteNonQuery();
                c.Close(con);
            }
        }

        void UpdatePartidaDesc(PartidaDTO obj, int pedido, int usuario)
        {
            var c = new Conexion();
            var con = c.ConnectPrueba();
            if (con != null)
            {
                string insertQuery = "UPDATE sf_pedidos_dcto SET  porcent_descto = ?, cia_sucursal = 1, usuario = ?, descuento = ?, fecha_hora = ? " +
                                        "WHERE pedido = " + pedido + "AND insumo = " + obj.Insumo + " AND partida = " + obj.Partida;

                OdbcCommand command = new OdbcCommand(insertQuery);
                OdbcParameterCollection parameters = command.Parameters;

                parameters.Add("@porcent_descto", OdbcType.Numeric).Value = obj.Descuento;
                parameters.Add("@usuario", OdbcType.Numeric).Value = usuario;
                parameters.Add("@descuento", OdbcType.Numeric).Value = obj.DescuentoDinero;
                parameters.Add("@fecha_hora", OdbcType.DateTime).Value = DateTime.Now;

                command.Connection = con;
                var id = command.ExecuteNonQuery();
                c.Close(con);
            }
        }

        public CiaParametrosDTO objCdfParametros(int cia_sucursal)
        {
            string consulta = "SELECT * FROM sf_cfd_parametros where cia_sucursal = " + cia_sucursal;
            var res = (List<CiaParametrosDTO>)_contextEnkontrol.Where(consulta).ToObject<List<CiaParametrosDTO>>();
            return res.FirstOrDefault();
        }
        string generarCadenaOriginal3_3(BigFacturaDTO obj, List<PartidaDTO> lst, out string selloDigital)
        {
            selloDigital = string.Empty;
            return null;
        }
        string generarCadenaOriginal3_2(BigFacturaDTO obj, List<PartidaDTO> lst, out string selloDigital)
        {
            try
            {
                var compania = (List<DatosCompaniaDTO>)_contextEnkontrol.Where("SELECT TOP 1 rfc FROM datos_compania").ToObject<List<DatosCompaniaDTO>>();
                var cia_sucursal = (List<CiaSucursalDTO>)_contextEnkontrol.Where("SELECT TOP 1 calle, ciudad, codigo_postal, nombre, no_exterior, no_interior, colonia  FROM cia_sucursales").ToObject<List<CiaSucursalDTO>>();
                var cliente = (List<ClienteDTO>)_contextEnkontrol.Where("SELECT * FROM sx_clientes where numcte = " + obj.numcte).ToObject<List<ClienteDTO>>();
                var certificado = (List<CertificadosDTO>)_contextEnkontrol.Where("SELECT TOP 1 certificado, ruta_certificado, ruta_llave_privada, llave_privada, clave FROM cfd_certificados where fecha_fin >='" + DateTime.Today.ToString("yyyy/MM/dd") + "'").ToObject<List<CertificadosDTO>>();

                var emisor = new Emisor();
                emisor.rfc = compania.FirstOrDefault().rfc;
                emisor.domicilioFiscal = new DomicilioFiscal();
                emisor.domicilioFiscal.calle = cia_sucursal.FirstOrDefault().calle ?? string.Empty;
                emisor.domicilioFiscal.municipio = cia_sucursal.FirstOrDefault().ciudad ?? string.Empty;
                emisor.domicilioFiscal.estado = "Sonora";
                emisor.domicilioFiscal.pais = "México";
                emisor.domicilioFiscal.codigoPostal = cia_sucursal.FirstOrDefault().codigo_postal ?? string.Empty;
                emisor.regimenFiscal = new RegimenFiscal();
                emisor.regimenFiscal.regimen = EnumHelper.GetDescription((RegimenFiscalEnum)obj.id_regimen_fiscal);

                var receptor = new Receptor();
                receptor.rfc = obj.rfc;
                receptor.domicilio = new Domicilio();
                receptor.domicilio.pais = obj.pais;

                var iva = new Traslado();
                iva.impuesto = "IVA";
                iva.importe = obj.iva.ToString();
                iva.tasa = (obj.iva * 100).ToString();

                var impuestos = new Impuestos();
                impuestos.traslados = new List<Traslado>();
                impuestos.traslados.Add(iva);

                var fecha = DateTime.Now;
                var comp = new Comprobante();
                comp.fecha = string.Format("{0}T{1}", fecha.ToString("yyyy-MM-dd"), fecha.ToString("HH:mm:ss"));
                comp.formaDePago = "PAGO EN UNA SOLA EXHIBICION";
                comp.metodoDePago = EnumHelper.GetDescription((MetodoPagoEnum)obj.id_metodo_pago);
                comp.tipoCambio = obj.tipo_cambio.ToString();
                comp.lugarExpedicion = cia_sucursal.FirstOrDefault().ciudad;
                comp.tipoDeComprobante = "ingreso";
                comp.subTotal = obj.sub_total.ToString();
                comp.total = obj.total.ToString();
                comp.emisor = emisor;
                comp.receptor = receptor;
                comp.condicionesDePago = obj.cond_pago + " Días";
                comp.conceptos = new List<Concepto>();
                foreach (var item in lst)
                {
                    var concepto = new Concepto();
                    concepto.cantidad = item.Cantidad.ToString();
                    concepto.unidad = item.Unidad;
                    concepto.descripcion = item.Descripcion;
                    concepto.importe = item.Importe.ToString();
                    concepto.valorUnitario = item.Precio.ToString();
                    comp.conceptos.Add(concepto);
                }
                comp.impuestos = impuestos;
                comp.terceros = true;
                comp.version = "3.2";

                //minimos (pero no mandatorios) para una impresion descente
                if (true)
                {
                    emisor.nombre = cia_sucursal.FirstOrDefault().nombre;
                    emisor.domicilioFiscal.noExterior = cia_sucursal.FirstOrDefault().no_exterior ?? string.Empty;
                    emisor.domicilioFiscal.noInterior = cia_sucursal.FirstOrDefault().no_interior ?? string.Empty;
                    emisor.domicilioFiscal.colonia = cia_sucursal.FirstOrDefault().colonia ?? string.Empty;
                    emisor.domicilioFiscal.localidad = cia_sucursal.FirstOrDefault().ciudad ?? string.Empty;
                    receptor.domicilio.calle = obj.direccion ?? string.Empty;
                    receptor.domicilio.codigoPostal = obj.cp ?? string.Empty;
                    receptor.domicilio.colonia = cliente.FirstOrDefault().colonia ?? string.Empty;
                    receptor.domicilio.noExterior = cliente.FirstOrDefault().no_exterior ?? string.Empty;
                    receptor.domicilio.noInterior = cliente.FirstOrDefault().no_interior ?? string.Empty;
                    receptor.domicilio.localidad = obj.ciudad ?? string.Empty;
                    receptor.domicilio.municipio = obj.ciudad ?? string.Empty;
                    receptor.domicilio.estado = obj.estado ?? string.Empty;
                    impuestos.totalImpuestosTrasladados = obj.iva.ToString();
                    impuestos.totalImpuestosRetenidos = obj.retencion.ToString();
                    comp.serie = obj.cfd_serie;
                    comp.folio = obj.cfd_folio;
                    comp.noCertificado = certificado.FirstOrDefault().certificado;
                }

                var cer = new X509Certificate2(certificado.FirstOrDefault().ruta_certificado, certificado.FirstOrDefault().clave, X509KeyStorageFlags.MachineKeySet);
                var xml = CFDIv32.Serializar(comp, false); //el xml sin sello
                var cadena = CFDIv32.CadenaOriginalSellado(xml); //cadena original para sello
                selloDigital = Sellar(certificado.FirstOrDefault().ruta_llave_privada, certificado.FirstOrDefault().llave_privada, cadena);
                comp.sello = selloDigital;
                var xmlSello = CFDIv32.Serializar(comp, true);
                return cadena;
            }
            catch (Exception ex)
            {
                selloDigital = string.Empty;
                return string.Empty;
            }
        }

        public string Sellar(string keyFile, string pass, string cadena)
        {
            string path = "C:\\sellosn\\openssl\\";

            // Escribir archivo UTF8 de la cadena
            var tempCadena = path + "\\temp\\cadena" + DateTime.Now.ToString("yyMMddhhmmss");
            System.IO.File.WriteAllText(tempCadena, cadena);

            // Digestion SHA1
            var tempSha = path + "temp\\sha" + DateTime.Now.ToString("yyMMddhhmmss");
            var opensslPath = path + "openssl.exe";
            Process process = new Process();
            process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            process.StartInfo.FileName = opensslPath;
            process.StartInfo.Arguments = "dgst -sha1 " + tempCadena;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.ErrorDialog = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            String codificado = "";
            codificado = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            String codificado2 = "";
            for (int i = 0; i < codificado.Length; i++)
            {
                if (codificado[i] == '=')
                {
                    codificado2 = codificado.Substring(i + 2);
                    break;
                }
            }
            System.IO.File.WriteAllText(tempSha, codificado2);

            // Crear .pem del .key
            var tempPem = path + "temp\\pem" + DateTime.Now.ToString("yyMMddhhmmss");
            Process process2 = new Process();
            process2.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            process2.StartInfo.FileName = opensslPath;
            process2.StartInfo.Arguments = "pkcs8 -inform DER -in " + keyFile + " -passin pass:" + pass + " -out " + tempPem;
            process2.StartInfo.UseShellExecute = false;
            process2.StartInfo.ErrorDialog = false;
            process2.StartInfo.RedirectStandardOutput = true;
            process2.Start();
            process2.WaitForExit();

            // Generar sello
            Process process3 = new Process();
            process3.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            process3.StartInfo.FileName = opensslPath;
            process3.StartInfo.Arguments = "dgst -sha1 -sign " + tempPem + " " + tempCadena;
            process3.StartInfo.UseShellExecute = false;
            process3.StartInfo.ErrorDialog = false;
            process3.StartInfo.RedirectStandardOutput = true;
            process3.Start();

            // Codificar en Base64
            String selloTxt = process3.StandardOutput.ReadToEnd();
            String b64 = Convert.ToBase64String(Encoding.Default.GetBytes(selloTxt));
            process3.WaitForExit();

            string[] filePaths = Directory.GetFiles(path + "temp");
            foreach (string filePath in filePaths)
                File.Delete(filePath);

            return b64;
        }

        public List<ComboDTO> getListaCC()
        {
            switch (vSesiones.sesionEmpresaActual)
            {
                case (int)EmpresaEnum.Construplan:
                    return (List<ComboDTO>)_contextEnkontrol.Where("SELECT descripcion as Value, cc as Text FROM cc where st_ppto!='T' ORDER BY Text").ToObject<List<ComboDTO>>();
                case (int)EmpresaEnum.Arrendadora:
                    return (List<ComboDTO>)ContextArrendadora.Where("SELECT DISTINCT (CAST(area  AS varchar(4)) + '-' + CAST(cuenta AS varchar(4))) AS Text, descripcion AS Value, area, cuenta FROM si_area_cuenta ORDER BY area, cuenta").ToObject<List<ComboDTO>>();
                default: return new List<ComboDTO>();
            }
        }

        public List<ComboDTO> getListaEmpleado()
        {
            return (List<ComboDTO>)_contextEnkontrol.Where("SELECT descripcion AS Value, empleado AS Text FROM empleados WHERE empleado > 1").ToObject<List<ComboDTO>>();
        }

        public List<ComboDTO> FillComboCiaSuc()
        {
            string consulta = "SELECT sucursal AS Value, nombre as Text FROM cia_sucursales";
            var res = (List<ComboDTO>)_contextEnkontrol.Where(consulta).ToObject<List<ComboDTO>>();
            return res.ToList();
        }

        public List<ComboDTO> FillComboSurcusal(int numcte)
        {
            string consulta = "SELECT nombre AS Value, sucursal as Text FROM sucursales WHERE numcte = " + numcte;
            var res = (List<ComboDTO>)_contextEnkontrol.Where(consulta).ToObject<List<ComboDTO>>();
            return res.ToList();
        }

        public List<ComboDTO> FillComboCliente()
        {
            return _contextEnkontrol.Select<ComboDTO>(EnkontrolAmbienteEnum.Prod, "SELECT numcte as Value, numcte as Text FROM \"DBA\".\"sx_clientes\"");
        }

        public List<ComboDTO> FillComboClienteNombre()
        {
            return _contextEnkontrol.Select<ComboDTO>(EnkontrolAmbienteEnum.Prod,"SELECT (CAST(numcte AS varchar(8))+'-'+nombre) AS Text, numcte AS Value FROM \"DBA\".\"sx_clientes\" ORDER BY Value");
        }
        public List<ComboDTO> ComboClienteNombre(EnkontrolEnum conector)
        {
            return _contextEnkontrol.Select<ComboDTO>(conector, "SELECT (CAST(numcte AS varchar(8))+'-'+nombre) AS Text, numcte AS Value FROM \"DBA\".\"sx_clientes\" ORDER BY Value");
        }
        public List<ComboDTO> FillComboClienteNombreMoneda(int moneda)
        {
            string where = moneda == 2 ? " where numcte > 9000" : " where numcte < 9000";
            string consulta = "SELECT nombre as Value, numcte as Text FROM sx_clientes" + where;
            var res = (List<ComboDTO>)_contextEnkontrol.Where(consulta).ToObject<List<ComboDTO>>();
            return res.ToList();
        }

        public List<ComboDTO> FillComboRegFiscal()
        {
            string consulta = "SELECT id_regimen_fiscal AS Value, desc_regimen_fiscal AS Text FROM cfd_regimen_fiscal";
            var res = (List<ComboDTO>)_contextEnkontrol.Where(consulta).ToObject<List<ComboDTO>>();
            return res.ToList();
        }

        public List<ComboDTO> FillComboClaveSat()
        {
            string consulta = "SELECT  clave_sat + '-' + CONVERT(CHAR (20), id_metodo_pago) AS Value, desc_metodo_pago AS Text FROM cfd_metodo_pago";
            var res = (List<ComboDTO>)_contextEnkontrol.Where(consulta).ToObject<List<ComboDTO>>();
            return res.ToList();
        }

        public List<ComboDTO> FillComboMetodoPago()
        {
            string consulta = "SELECT clave_sat  AS Value, clave_sat + ' - ' + descripcion_sat AS Text FROM cfd_metodo_pago_sat";
            var res = (List<ComboDTO>)_contextEnkontrol.Where(consulta).ToObject<List<ComboDTO>>();
            return res.ToList();
        }

        public List<ComboDTO> FillComboTipoCuenta()
        {
            string consulta = "SELECT clave_sat AS Value, descripcion_sat AS Text FROM cfd_metodo_pago_sat";
            var res = (List<ComboDTO>)_contextEnkontrol.Where(consulta).ToObject<List<ComboDTO>>();
            return res.ToList();
        }

        public List<ComboDTO> FillComboTm()
        {
            string consulta = "SELECT tm AS Value, descripcion AS Text FROM sx_tm";
            var res = (List<ComboDTO>)_contextEnkontrol.Where(consulta).ToObject<List<ComboDTO>>();
            return res.ToList();
        }

        public List<ComboDTO> FillComboZonas()
        {
            string consulta = "SELECT zona AS Value, descripcion AS Text FROM zonas";
            var res = (List<ComboDTO>)_contextEnkontrol.Where(consulta).ToObject<List<ComboDTO>>();
            return res.ToList();
        }
        #endregion
        #region Gestión
        public List<pedidoDTO> GetTblGestion(DateTime inicio, DateTime fin, string cliente)
        {
            string condicion = string.Empty;
            if (!cliente.Equals(string.Empty))
            {
                condicion += " AND numcte = " + cliente;
            }
            string consulta = "SELECT numcte, fecha, pedido FROM sf_pedidos" +
                                " WHERE fecha >= '" + inicio.ToString("yyyy-MM-dd") + "' AND fecha <= '" + fin.ToString("yyyy-MM-dd") + "'" +
                                condicion +
                                " ORDER BY fecha DESC;";
            try
            {
                var res = (List<pedidoDTO>)_contextEnkontrol.Where(consulta).ToObject<List<pedidoDTO>>();
                return res.ToList();
            }
            catch (Exception)
            {
                return new List<pedidoDTO>();
            }
        }

        public int GetRemisionFromPedido(int pedido)
        {
            string consulta = "SELECT * FROM sf_remision where entregado = 'S' AND pedido =" + pedido;
            try
            {
                var res = (List<RemisionDTO>)_contextEnkontrol.Where(consulta).ToObject<List<RemisionDTO>>();
                return res.FirstOrDefault().remision;
            }
            catch (Exception) { return 0; }

        }

        public int GetFacturaFromPedido(int pedido)
        {
            string consulta = "SELECT * FROM sf_facturas where pedido =" + pedido;
            try
            {
                var res = (List<FacturaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<FacturaDTO>>();
                return res.FirstOrDefault().factura;
            }
            catch (Exception) { return 0; }
        }
        public string GetClienteNombre(int numcte)
        {
            string consulta = "SELECT * FROM sx_clientes where numcte =" + numcte;
            var res = (List<ClienteDTO>)_contextEnkontrol.Where(consulta).ToObject<List<ClienteDTO>>();
            return res.FirstOrDefault().nombre;
        }
        #endregion
        #region Concentrado
        public List<MovcltesDTO> GetMovimientoFacturasClientes(BusqConcentradoDTO busq)
        {
            var odbc = new OdbcConsultaDTO()
            {
                consulta = queryGetMovimientoFacturasClientes(busq),
                parametros = paramGetMovimientoFacturasClientes(busq)
            };
            return _contextEnkontrol.Select<MovcltesDTO>(EnkontrolAmbienteEnum.Prod, odbc);
        }
        string queryGetMovimientoFacturasClientes(BusqConcentradoDTO busq)
        {
            return string.Format(@"SELECT mov.linea ,mov.factura ,(mov.cfd_serie + '-' + CONVERT(varchar(6), mov.cfd_folio)) AS folioDigital ,mov.fecha ,mov.numcte ,mov.total ,mov.moneda ,mov.tipocambio ,mov.cc ,mov.tm, mov.tp, mov.poliza
                                        ,(SELECT descripcion FROM sx_tm tm WHERE tm.tm = mov.tm) AS tipoMovimiento
                                        ,(SELECT nombre FROM sx_clientes sx WHERE sx.numcte = mov.numcte) AS cliente
                                    FROM sx_movcltes mov
                                    WHERE mov.es_factura = 'S' AND mov.fecha BETWEEN ? AND ? AND mov.cc IN {0}"
                , busq.lstCC.ToParamInValue());
        }
        List<OdbcParameterDTO> paramGetMovimientoFacturasClientes(BusqConcentradoDTO busq)
        {
            var parameters = new List<OdbcParameterDTO>();
            parameters.Add(new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.Date, valor = busq.min });
            parameters.Add(new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.Date, valor = busq.max });
            busq.lstCC.ForEach(c =>
            {
                parameters.Add(new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = c });
            });
            return parameters;
        }
        #endregion
        #region Estimaciones
        public bool guardarLstEstimacionResumen(List<tblF_EstimacionResumen> lst)
        {
            var esGuardado = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                var ahora = DateTime.Now;
                var fechaInicial = lst.Min(est => est.fechavenc);
                var fechaFinal = lst.Max(est => est.fechavenc);
                var lstBd = getlstEstimadoActivo(fechaInicial, fechaFinal);
                lst.ForEach(est => {
                    var bd = lstBd.FirstOrDefault(estBd => estBd.numcte.Equals(est.numcte) && estBd.cc.Equals(est.cc) && estBd.factura.Equals(est.factura));
                    if (bd == null)
                    {
                        est.esActivo = true;   
                    }
                    else
                    {
                        est.esActivo = bd.esActivo;   
                        est.id = bd.id;
                    }
                    est.fechaCaptura = ahora;
                    _context.tblF_EstimacionResumen.AddOrUpdate(est);
                    _context.SaveChanges();
                });
                esGuardado = lst.All(prov => prov.id > 0);
                if (esGuardado)
                {
                    var entity = lst.FirstOrDefault();
                    SaveBitacora((int)BitacoraEnum.EstimacionesResumen, (int)AccionEnum.AGREGAR, entity.id, JsonUtils.convertNetObjectToJson(entity));
                }
                dbTransaction.Commit();
            }
            return esGuardado;
        }
        public bool eliminarEstimacion(List<int> lstId)
        {
            var esGuardado = false;
            using (DbContextTransaction dbContextTransaction = _context.Database.BeginTransaction())
            {
                lstId.ForEach(id => {
                    var est = _context.tblF_EstimacionResumen.FirstOrDefault(w => w.id.Equals(id));
                    if (est != null)
                    {
                        est.esActivo = false;
                        est.fechaCaptura = DateTime.Now;
                        _context.tblF_EstimacionResumen.AddOrUpdate(est);
                        SaveChanges();
                        SaveBitacora((int)BitacoraEnum.EstimacionesResumen, (int)AccionEnum.ELIMINAR, est.id, JsonUtils.convertNetObjectToJson(est));
                    }
                });
                esGuardado = true;
                if (esGuardado)
                {
                    dbContextTransaction.Commit();
                }
                else { 
                    dbContextTransaction.Rollback(); 
                }
            }
            return esGuardado;
        }
        public bool authResumenEstimacion(DateTime fecha)
        {
            var esAuth = false;
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                var bd = _context.tblF_AuthResumenEstimacion.ToList().FirstOrDefault(a => a.esActivo && a.fechaResumen.Year.Equals(fecha.Year) && a.fechaResumen.noSemana().Equals(fecha.noSemana()));
                if (bd == null)
                {
                    var ahora = DateTime.Now;
                    var usuarioID = vSesiones.sesionUsuarioDTO.id;
                    var ultimoID = _context.tblF_AuthResumenEstimacion.ToList().Count();
                    bd = new tblF_AuthResumenEstimacion()
                    {
                        usuarioID = usuarioID,
                        fechaCaptura = ahora,
                        esActivo = true,
                        firma = string.Format("{0}{1:dd}{1:MM}{1:yyyy}{1:mm}{1:ss}{2}A"
                        , ++ultimoID
                        , ahora
                        , usuarioID)
                    };
                }
                else
                {
                    bd.stAuth = setStAuth(bd.stAuth);
                }
                bd.fechaResumen = fecha;
                _context.tblF_AuthResumenEstimacion.AddOrUpdate(bd);
                _context.SaveChanges();
                esAuth = bd.id > 0;
                if (esAuth)
                {
                    SaveBitacora((int)BitacoraEnum.AuthEstimacionesResumen, (int)AccionEnum.AGREGAR, bd.id, JsonUtils.convertNetObjectToJson(bd));
                    dbContextTransaction.Commit();
                }
                else
                {
                    dbContextTransaction.Rollback();
                } 
            }
            return esAuth;
        }
        int setStAuth(int stAuth)
        {
            if (stAuth.Equals(1))
            {
                stAuth = (int)stAuthEnum.Cancelado;
            }
            else
            {
                stAuth = (int)stAuthEnum.Aceptado;
            }
            return stAuth;
        }
        public tblF_AuthResumenEstimacion getAuthResumenEstimacion(DateTime fechaInicial, DateTime fechaFinal)
        {
            try
            {
                var auth = _context.tblF_AuthResumenEstimacion.FirstOrDefault(w => w.esActivo && w.fechaResumen > fechaInicial && w.fechaResumen <= fechaFinal );
                return auth == null ? new tblF_AuthResumenEstimacion() : auth;
            }
            catch (Exception o_O)
            {
                return new tblF_AuthResumenEstimacion();
            }
        }
        public List<tblF_EstimacionResumen> getlstEstimadoActivo(DateTime fechaInicial, DateTime fechaFinal)
        {
            return _context.tblF_EstimacionResumen.ToList()
                .Where(est => est.esActivo)
                .Where(est => est.fechaResumen >= fechaInicial && est.fechaResumen <= fechaFinal)
                .ToList();
        }
        public List<tblF_EstimacionResumen> getlstEstimadoActivo(DateTime fechaInicial, DateTime fechaFinal, List<string> lstCc)
        {
            return _context.tblF_EstimacionResumen.ToList()
                .Where(est => est.esActivo)
                .Where(est => lstCc.Any(c => c.Equals(est.cc)))
                .Where(est => est.fechaResumen >= fechaInicial && est.fechaResumen <= fechaFinal)
                .ToList();
        }
        public List<tblF_EstimacionResumen> getAlltEstimadoo(DateTime fechaInicial, DateTime fechaFinal)
        {
            return _context.tblF_EstimacionResumen.ToList()
                .Where(est => est.fechaResumen >= fechaInicial && est.fechaResumen <= fechaFinal)
                .ToList();
        }
        public List<tblF_EstimacionResumen> GetAnaliticoClientes(DateTime fecha_corte)
        {
            var odbc = new OdbcConsultaDTO()
            {
                consulta = queryAnaliticoClientes(),
                parametros = paramAnaliticoClientes(fecha_corte)
            };
            var lst = _contextEnkontrol.Select<tblF_EstimacionResumen>(EnkontrolAmbienteEnum.Prod, odbc);

            foreach (var item in lst)
            {
                item.Enkontrol = true;
            }

            return lst;
        }
        string queryAnaliticoClientes()
        {
            return string.Format(@"SELECT * FROM (SELECT mov.numcte, mov.factura, mov.cc, MAX(mov.fechavenc) AS fechavenc, MAX(det.linea) AS linea, 1 AS esActivo,
                                    SUM(CASE WHEN det.insumo IN (9010001 ,9010003 ,9010006 ,9010007) AND mov.fechavenc <= ? THEN mov.total ELSE 0 END) AS estimacion,
                                    SUM(CASE WHEN det.insumo IN (9010001 ,9010003 ,9010006 ,9010007) AND mov.fechavenc >= ? AND mov.tm <> 51 THEN mov.total ELSE 0 END) AS vencido,
                                    SUM(CASE WHEN det.insumo IN (9010001 ,9010003 ,9010006 ,9010007) AND mov.fechavenc BETWEEN ? AND ? AND mov.tm = 51 THEN mov.total ELSE 0 END) AS cobrado,
                                    SUM(CASE WHEN det.insumo IN (9010004 ,9030001) THEN mov.total ELSE 0 END) AS anticipo
                                        FROM sx_movcltes mov
                                        INNER JOIN sf_facturas fac ON fac.factura = mov.factura AND fac.numcte = mov.numcte AND fac.cc = mov.cc AND mov.referenciaoc = fac.numero_nc
                                        INNER JOIN sf_facturas_det det ON  det.factura = fac.factura AND fac.numero_nc = det.numero_nc AND det.cia_sucursal = fac.cia_sucursal
                                        GROUP BY  mov.numcte ,mov.factura ,mov.cc
                                        ORDER BY mov.cc ,mov.numcte ,mov.factura) x
                                    WHERE (x.estimacion + x.cobrado + x.anticipo + x.vencido) NOT BETWEEN -1 AND 1");
        }
        List<OdbcParameterDTO> paramAnaliticoClientes(DateTime fecha_corte)
        {
            var sabado = fecha_corte.AddDays(-6);
            var domingoAnt = sabado.AddDays(-1);
            var sabadoAnt = domingoAnt.AddDays(-6);
            var parameters = new List<OdbcParameterDTO>();
            parameters.Add(new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.Date, valor = fecha_corte });
            parameters.Add(new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.Date, valor = sabado });
            parameters.Add(new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.Date, valor = domingoAnt });
            parameters.Add(new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.Date, valor = sabadoAnt });
            return parameters;
        }
        #endregion
        #region ProyeccionDeCierre
        public List<tblC_FED_DetProyeccionCierre> getLstIngresosXCobrarCxCDTO(List<string> lstCC)
        {
            var empresa = (EmpresaEnum)vSesiones.sesionEmpresaActual;
            var consulta = string.Empty;
            switch(empresa)
            {
                case EmpresaEnum.Arrendadora:
                    consulta = queryIngresosXCobrarCxCACDTO(lstCC);
                    break;
                default:
                    consulta = queryIngresosXCobrarCxCDTO(lstCC);
                    break;
            }
            var odbc = new OdbcConsultaDTO()
            {
                consulta = consulta,
                parametros = paramIngresosXCobrarCxC(lstCC)
            };
            var lst = _contextEnkontrol.Select<tblC_FED_DetProyeccionCierre>(EnkontrolAmbienteEnum.Prod, odbc);
            return lst;
        }
        string queryIngresosXCobrarCxCDTO(List<string> lstCC)
        {
            return string.Format(@"SELECT * FROM (SELECT mov.numcte ,mov.factura ,mov.cc, SUM(mov.total * mov.tipocambio) AS monto, MAX(clte.nombre) AS descripcion, MAX(mov.fecha) AS fechaFactura, 1 AS esActivo, 1 AS tipo, 21 AS idConceptoDir, '0-0' AS ac
                                    FROM sx_movcltes mov
                                    INNER JOIN sx_clientes clte ON clte.numcte = mov.numcte
                                    WHERE mov.cc IN {0}
                                    GROUP BY  mov.numcte ,mov.factura ,mov.cc) x
                                    WHERE x.monto NOT BETWEEN -1 AND 1"
                , lstCC.ToParamInValue());
        }
        string queryIngresosXCobrarCxCACDTO(List<string> lstCC)
        {
            return string.Format(@"SELECT * FROM (SELECT mov.numcte ,mov.factura ,mov.cc, SUM(mov.total * mov.tipocambio) AS monto, MAX(clte.nombre) AS descripcion, MAX(mov.fecha) AS fechaFactura, 1 AS esActivo, 1 AS tipo, 21 AS idConceptoDir, (CAST(fac.area AS varchar)+'-'+ CAST(fac.cuenta AS varchar)) AS ac
                                    FROM sx_movcltes mov
                                    INNER JOIN sx_clientes clte ON clte.numcte = mov.numcte
                                    INNER JOIN sf_facturas fac ON fac.numcte = mov.numcte AND fac.factura = mov.factura
                                    WHERE mov.cc IN {0}
                                    GROUP BY  mov.numcte ,mov.factura ,mov.cc, ac) x
                                    WHERE x.monto NOT BETWEEN -1 AND 1"
                , lstCC.ToParamInValue());
        }
        List<OdbcParameterDTO> paramIngresosXCobrarCxC(List<string> lstCC)
        {
            return lstCC.Select(cc => new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.NVarChar, valor = cc }).ToList();
        }
        public List<tblC_FED_DetProyeccionCierre> getLstRetencionesClientes(BusqProyeccionCierreDTO busq/*List<string> lstCC*/)
        {
            var odbc = new OdbcConsultaDTO()
            {
                //consulta = queryRetencionesClientes(lstCC),
                //parametros = paramIngresosXCobrarCxC(lstCC)
                consulta = queryRetencionesClientes(busq),
                parametros = paramAnticipoClientes(busq)
            };
            var lst = _contextEnkontrol.Select<tblC_FED_DetProyeccionCierre>(EnkontrolAmbienteEnum.Prod, odbc);
            return lst;
        }
        string queryRetencionesClientes(BusqProyeccionCierreDTO busq/*List<string> lstCC*/)
        {
            return string.Format(@"SELECT 
                    *, 
                    1 AS esActivo, 
                    8 AS tipo, 
                    22 AS idConceptoDir
                FROM (
                    SELECT 
                        CAST((CASE WHEN ISNUMERIC(pol.referencia) = 1 THEN CAST(pol.referencia as numeric) ELSE 0 END) as int) as factura, 
                            pol.cc, 0 AS numcte, 
                            (CASE WHEN pol.cc in ('C79', 'D01', 'D02', 'D03', 'D04', 'D05', 'D06') THEN sum(pol.monto) ELSE sum(pol.monto) * 1.16 END) AS valida, 
                            (CASE WHEN pol.cc in ('C79', 'D01', 'D02', 'D03', 'D04', 'D05', 'D06') THEN sum(pol.monto) ELSE sum(pol.monto) * 1.16 END) AS monto,
                            --MAX(pol.concepto) AS descripcion, 
                            ISNULL((SELECT TOP 1 descripcion FROM catcta WHERE cta = pol.cta AND scta = pol.scta AND sscta = pol.sscta), MAX(pol.concepto)) as descripcion,
                            MAX(poliza.fechapol) AS fechaFactura,
                            CAST(MAX(CASE WHEN pol.area IS NULL THEN 0 ELSE pol.area END) AS varchar) +'-'+ CAST(MAX(CASE WHEN pol.cuenta_oc IS NULL THEN 0 ELSE pol.cuenta_oc END) AS varchar)    AS     ac
                        FROM sc_movpol pol
                        INNER JOIN sc_polizas poliza ON poliza.year = pol.year AND poliza.mes = pol.mes AND poliza.tp = pol.tp AND poliza.poliza = pol.poliza
                        WHERE pol.cta = 1140 AND pol.cc IN {0}
                        GROUP BY pol.referencia, pol.cc, pol.cta, pol.scta, pol.sscta
                    ) x
                WHERE x.valida <> 0"
                , busq.lstCC.ToParamInValue());

//            return string.Format(@"SELECT * FROM (SELECT mov.numcte, mov.factura, mov.cc, MAX(mov.fecha) AS fechaFactura, SUM(mov.total * mov.tipocambio) AS monto, MAX(clte.nombre) AS descripcion, 1 AS esActivo, 2 AS tipo, 22 AS idConceptoDir, '0-0' AS ac
//                                        FROM sx_movcltes mov
//                                        INNER JOIN sf_facturas fac ON fac.factura = mov.factura AND fac.numcte = mov.numcte AND fac.cc = mov.cc AND mov.referenciaoc = fac.numero_nc
//                                        INNER JOIN sf_facturas_det det ON  det.factura = fac.factura AND fac.numero_nc = det.numero_nc AND det.cia_sucursal = fac.cia_sucursal
//                                        INNER JOIN insumos ins ON ins.tipo = 9 AND ins.grupo = 99 AND ins.descripcion like '%retencio%' AND ins.insumo = det.insumo
//                                        INNER JOIN sx_clientes clte ON clte.numcte = mov.numcte
//                                        WHERE mov.cc IN {0}
//                                        GROUP BY  mov.numcte ,mov.factura ,mov.cc
//                                        ORDER BY mov.cc ,mov.numcte ,mov.factura) x
//                                    WHERE x.monto NOT BETWEEN -1 AND 1"
//                , lstCC.ToParamInValue());}
        }
        public List<tblC_FED_DetProyeccionCierre> getLstAmortizacionClientes(/*List<string> lstCC*/BusqProyeccionCierreDTO busq)
        {
            var odbc = new OdbcConsultaDTO()
            {
                consulta = queryAmortizacionClientes(/*lstCC*/busq),
                //parametros = paramIngresosXCobrarCxC(lstCC)
                parametros = paramAnticipoClientes(busq)
            };
            var lst = _contextEnkontrol.Select<tblC_FED_DetProyeccionCierre>(EnkontrolAmbienteEnum.Prod, odbc);
            return lst;
        }
        string queryAmortizacionClientes(/*List<string> lstCC*/BusqProyeccionCierreDTO busq)
        {
//            return string.Format(@"SELECT * FROM (SELECT mov.numcte, mov.factura, mov.cc, -SUM(mov.total * mov.tipocambio) AS monto, MAX(clte.nombre) AS descripcion, MAX(mov.fecha) AS fechaFactura, 1 AS esActivo, 5 AS tipo, 28 AS idConceptoDir, '0-0' AS ac
//                                        FROM sx_movcltes mov
//                                        INNER JOIN sf_facturas fac ON fac.factura = mov.factura AND fac.numcte = mov.numcte AND fac.cc = mov.cc AND mov.referenciaoc = fac.numero_nc
//                                        INNER JOIN sf_facturas_det det ON  det.factura = fac.factura AND fac.numero_nc = det.numero_nc AND det.cia_sucursal = fac.cia_sucursal
//                                        INNER JOIN insumos ins ON ins.grupo = 99 AND ins.descripcion like '%amort%' AND ins.insumo = det.insumo
//                                        INNER JOIN sx_clientes clte ON clte.numcte = mov.numcte
//                                        WHERE mov.cc IN {0}
//                                        GROUP BY  mov.numcte ,mov.factura ,mov.cc
//                                        ORDER BY mov.cc ,mov.numcte ,mov.factura) x
//                                    WHERE x.monto NOT BETWEEN -1 AND 1"
//                , lstCC.ToParamInValue());
            return string.Format(@"SELECT 
                                    *, 
                                    1 AS esActivo, 
                                    5 AS tipo, 
                                    28 AS idConceptoDir
                                FROM (
                                    SELECT 
                                        CAST((CASE WHEN ISNUMERIC(pol.referencia) = 1 THEN CAST(pol.referencia as numeric) ELSE 0 END) as int) as factura, 
                                            pol.cc, 0 AS numcte, 
                                            (CASE WHEN pol.cc in ('C79', 'D01', 'D02', 'D03', 'D04', 'D05', 'D06') THEN sum(pol.monto) ELSE sum(pol.monto) * 1.16 END) AS valida, 
                                            (CASE WHEN pol.cc in ('C79', 'D01', 'D02', 'D03', 'D04', 'D05', 'D06') THEN sum(pol.monto) ELSE sum(pol.monto) * 1.16 END) AS monto,
                                            ISNULL((SELECT TOP 1 descripcion FROM catcta WHERE cta = pol.cta AND scta = pol.scta AND sscta = pol.sscta), MAX(pol.concepto)) as descripcion,
                                            --MAX(pol.concepto) AS descripcion, 
                                            MAX(poliza.fechapol) AS fechaFactura,
                                            CAST(MAX(CASE WHEN pol.area IS NULL THEN 0 ELSE pol.area END) AS varchar) +'-'+ CAST(MAX(CASE WHEN pol.cuenta_oc IS NULL THEN 0 ELSE pol.cuenta_oc END) AS varchar)    AS     ac
                                        FROM sc_movpol pol
                                        INNER JOIN sc_polizas poliza ON poliza.year = pol.year AND poliza.mes = pol.mes AND poliza.tp = pol.tp AND poliza.poliza = pol.poliza
                                        WHERE pol.cta = 2125 AND pol.scta != 10 AND pol.cc IN {0}
                                        GROUP BY pol.referencia, pol.cc, pol.cta, pol.scta, pol.sscta
                                    ) x
                                WHERE x.valida <> 0"
            , busq.lstCC.ToParamInValue());
        }
        public List<tblC_FED_DetProyeccionCierre> getLstAnticipoClientes(BusqProyeccionCierreDTO busq)
        {
            var odbc = new OdbcConsultaDTO()
            {
                consulta = queryLstAnticipoClientes(busq),
                parametros = paramAnticipoClientes(busq)
            };
            var lst = _contextEnkontrol.Select<tblC_FED_DetProyeccionCierre>(EnkontrolAmbienteEnum.Prod, odbc);
            return lst;
        }
        string queryLstAnticipoClientes(BusqProyeccionCierreDTO busq)
        {
            return string.Format(@"SELECT 
                                    *, 
                                    1 AS esActivo, 
                                    5 AS tipo, 
                                    28 AS idConceptoDir
                                FROM (
                                    SELECT 
                                        CAST((CASE WHEN ISNUMERIC(pol.referencia) = 1 THEN CAST(pol.referencia as numeric) ELSE 0 END) as int) as factura, 
                                            pol.cc, 0 AS numcte, 
                                            (CASE WHEN pol.cc in ('C79', 'D01', 'D02', 'D03', 'D04', 'D05', 'D06') THEN sum(pol.monto) ELSE sum(pol.monto) * 1.16 END) AS valida, 
                                            (CASE WHEN pol.cc in ('C79', 'D01', 'D02', 'D03', 'D04', 'D05', 'D06') THEN sum(pol.monto) ELSE sum(pol.monto) * 1.16 END) AS monto,
                                            ISNULL((SELECT TOP 1 descripcion FROM catcta WHERE cta = pol.cta AND scta = pol.scta AND sscta = pol.sscta), MAX(pol.concepto)) as descripcion,
                                            --MAX(pol.concepto) AS descripcion, 
                                            MAX(poliza.fechapol) AS fechaFactura,
                                            CAST(MAX(CASE WHEN pol.area IS NULL THEN 0 ELSE pol.area END) AS varchar) +'-'+ CAST(MAX(CASE WHEN pol.cuenta_oc IS NULL THEN 0 ELSE pol.cuenta_oc END) AS varchar)    AS     ac
                                        FROM sc_movpol pol
                                        INNER JOIN sc_polizas poliza ON poliza.year = pol.year AND poliza.mes = pol.mes AND poliza.tp = pol.tp AND poliza.poliza = pol.poliza
                                        WHERE pol.cta = 2125 AND pol.scta != 10 AND pol.cc IN {0}
                                        GROUP BY pol.referencia, pol.cc, pol.cta, pol.scta, pol.sscta
                                    ) x
                                WHERE x.valida <> 0"
                , busq.lstCC.ToParamInValue());
        }
        List<OdbcParameterDTO> paramAnticipoClientes(BusqProyeccionCierreDTO busq)
        {
            var lst = new List<OdbcParameterDTO>();
            lst.AddRange(busq.lstCC.Select(cc => new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.NVarChar, valor = cc }).ToList());
            return lst;
        }
        public List<tblC_FED_DetProyeccionCierre> getLstAnticipoContratistas(BusqProyeccionCierreDTO busq)
        {
            var odbc = new OdbcConsultaDTO()
            {
                consulta = queryLstAnticipoContratistas(busq),
                parametros = paramAnticipoClientes(busq)
            };
            var lst = _contextEnkontrol.Select<tblC_FED_DetProyeccionCierre>(EnkontrolAmbienteEnum.Prod, odbc);
            return lst;
        }
        
        string queryLstAnticipoContratistas(BusqProyeccionCierreDTO busq)
        {
            return string.Format(@"SELECT 
                                    *, 
                                    1 AS esActivo, 
                                    8 AS tipo, 
                                    33 AS idConceptoDir
                                FROM (
                                    SELECT 
                                        CAST((CASE WHEN ISNUMERIC(pol.referencia) = 1 THEN CAST(pol.referencia as numeric) ELSE 0 END) as int) as factura, 
                                            pol.cc, 0 AS numcte, 
                                            (CASE WHEN pol.cc in ('C79', 'D01', 'D02', 'D03', 'D04', 'D05', 'D06') THEN sum(pol.monto) ELSE sum(pol.monto) * 1.16 END) AS valida, 
                                            (CASE WHEN pol.cc in ('C79', 'D01', 'D02', 'D03', 'D04', 'D05', 'D06') THEN sum(pol.monto) ELSE sum(pol.monto) * 1.16 END) AS monto,
                                            ISNULL((SELECT TOP 1 descripcion FROM catcta WHERE cta = pol.cta AND scta = pol.scta AND sscta = pol.sscta), MAX(pol.concepto)) as descripcion,
                                            --MAX(pol.concepto) AS descripcion, 
                                            MAX(poliza.fechapol) AS fechaFactura,
                                            CAST(MAX(CASE WHEN pol.area IS NULL THEN 0 ELSE pol.area END) AS varchar) +'-'+ CAST(MAX(CASE WHEN pol.cuenta_oc IS NULL THEN 0 ELSE pol.cuenta_oc END) AS varchar)    AS     ac
                                        FROM sc_movpol pol
                                        INNER JOIN sc_polizas poliza ON poliza.year = pol.year AND poliza.mes = pol.mes AND poliza.tp = pol.tp AND poliza.poliza = pol.poliza
                                        WHERE pol.cta = 1135 AND pol.scta <> 998 AND pol.cc IN {0}
                                        GROUP BY pol.referencia, pol.cc, pol.cta, pol.scta, pol.sscta
                                    ) x
                                WHERE x.valida <> 0"
                , busq.lstCC.ToParamInValue());
        }

        public List<tblC_FED_DetProyeccionCierre> getLstRetencionContratistas(BusqProyeccionCierreDTO busq)
        {
            var odbc = new OdbcConsultaDTO()
            {
                consulta = queryRetencionContratistas(busq),
                parametros = paramAnticipoClientes(busq)
            };
            var lst = _contextEnkontrol.Select<tblC_FED_DetProyeccionCierre>(EnkontrolAmbienteEnum.Prod, odbc);
            return lst;
        }


        string queryRetencionContratistas(/*List<string> lstCC*/BusqProyeccionCierreDTO busq)
        {
            //            return string.Format(@"SELECT * FROM (SELECT mov.numcte, mov.factura, mov.cc, -SUM(mov.total * mov.tipocambio) AS monto, MAX(clte.nombre) AS descripcion, MAX(mov.fecha) AS fechaFactura, 1 AS esActivo, 5 AS tipo, 28 AS idConceptoDir, '0-0' AS ac
            //                                        FROM sx_movcltes mov
            //                                        INNER JOIN sf_facturas fac ON fac.factura = mov.factura AND fac.numcte = mov.numcte AND fac.cc = mov.cc AND mov.referenciaoc = fac.numero_nc
            //                                        INNER JOIN sf_facturas_det det ON  det.factura = fac.factura AND fac.numero_nc = det.numero_nc AND det.cia_sucursal = fac.cia_sucursal
            //                                        INNER JOIN insumos ins ON ins.grupo = 99 AND ins.descripcion like '%amort%' AND ins.insumo = det.insumo
            //                                        INNER JOIN sx_clientes clte ON clte.numcte = mov.numcte
            //                                        WHERE mov.cc IN {0}
            //                                        GROUP BY  mov.numcte ,mov.factura ,mov.cc
            //                                        ORDER BY mov.cc ,mov.numcte ,mov.factura) x
            //                                    WHERE x.monto NOT BETWEEN -1 AND 1"
            //                , lstCC.ToParamInValue());
            return string.Format(@"SELECT 
                                    *, 
                                    1 AS esActivo, 
                                    12 AS tipo, 
                                    36 AS idConceptoDir
                                FROM (
                                    SELECT 
                                        CAST((CASE WHEN ISNUMERIC(pol.referencia) = 1 THEN CAST(pol.referencia as numeric) ELSE 0 END) as int) as factura, 
                                            pol.cc, 0 AS numcte, 
                                            (CASE WHEN pol.cc in ('C79', 'D01', 'D02', 'D03', 'D04', 'D05', 'D06') THEN sum(pol.monto) ELSE sum(pol.monto) * 1.16 END) AS valida, 
                                            (CASE WHEN pol.cc in ('C79', 'D01', 'D02', 'D03', 'D04', 'D05', 'D06') THEN sum(pol.monto) ELSE sum(pol.monto) * 1.16 END) AS monto,
                                            ISNULL((SELECT TOP 1 descripcion FROM catcta WHERE cta = pol.cta AND scta = pol.scta AND sscta = pol.sscta), MAX(pol.concepto)) as descripcion,
                                            --MAX(pol.concepto) AS descripcion, 
                                            MAX(poliza.fechapol) AS fechaFactura,
                                            CAST(MAX(CASE WHEN pol.area IS NULL THEN 0 ELSE pol.area END) AS varchar) +'-'+ CAST(MAX(CASE WHEN pol.cuenta_oc IS NULL THEN 0 ELSE pol.cuenta_oc END) AS varchar)    AS     ac
                                        FROM sc_movpol pol
                                        INNER JOIN sc_polizas poliza ON poliza.year = pol.year AND poliza.mes = pol.mes AND poliza.tp = pol.tp AND poliza.poliza = pol.poliza
                                        WHERE pol.cta = 2130 AND pol.cc IN {0}
                                        GROUP BY pol.referencia, pol.cc, pol.cta, pol.scta, pol.sscta
                                    ) x
                                WHERE x.valida <> 0"
            , busq.lstCC.ToParamInValue());
        }
        #endregion
    }
}