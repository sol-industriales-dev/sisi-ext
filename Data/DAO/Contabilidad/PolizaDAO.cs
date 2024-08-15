using Core.DAO.Contabilidad.Poliza;
using Core.DTO.Contabilidad.Poliza;
using Data.EntityFramework.Context;
using Core.DTO.Principal.Generales;
using Infrastructure.DTO;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.EntityFramework.Generic;
using Core.Entity.Administrativo.Contabilidad.Poliza;
using Core.Enum.Principal.Bitacoras;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using Data.EntityFramework;
using System.Data.Odbc;
using System.Data;
using Data.Factory.Contabilidad.Reportes;
using Core.Entity.Administrativo.Contabilidad;
using Core.DTO.Principal.Usuarios;
using Core.DTO.Administracion.Facultamiento;
using Core.DTO;
using Core.Enum.Multiempresa;
using Core.DTO.Contabilidad.Propuesta;
using Core.Enum.Administracion.Propuesta;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Proveedores;
using Core.DTO.Subcontratistas.Bloqueo;
using Data.Factory.Contabilidad.Poliza;

namespace Data.DAO.Contabilidad
{
    public class PolizaDAO : GenericDAO<tblPo_Poliza>, IPolizaDAO
    {
        CadenaProductivaFactoryServices cadenaFS = new CadenaProductivaFactoryServices();
        PolizaSPFactoryService polizaFS = new PolizaSPFactoryService();

        #region Guardar
        public bool Pagar(List<tblC_CadenaProductiva> lstPagadas)
        {
            try
            {
                PagarSigoplan(lstPagadas);
                return true;
            }
            catch (Exception) { return false; }
        }
        void PagarSigoplan(List<tblC_CadenaProductiva> lstPagadas)
        {
            var _op = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblC_CadenaProductiva>();
            var idPal = lstPagadas.FirstOrDefault().idPrincipal;
            var savePrincipal = true;
            lstPagadas.ForEach(o =>
            {
                var c = _op.FirstOrDefault(w => w.id == o.id);
                c.factoraje = o.factoraje;
                c.cif = o.cif;
                c.banco = o.banco;
                c.fecha = o.fecha;
                c.fechaVencimiento = o.fechaVencimiento;
                c.pagado = true;
                Update(c, c.id, (int)BitacoraEnum.CADENAPRODUCTIVA);
            });
            _op.Where(w => w.idPrincipal == idPal).ToList().ForEach(e =>
            {
                if (!e.pagado)
                    savePrincipal = false;
            });
            if (savePrincipal)
            {
                var _opal = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblC_CadenaPrincipal>();
                var pal = _opal.FirstOrDefault(w => w.id == lstPagadas.FirstOrDefault().idPrincipal);
                pal.pagado = true;
                Update(pal, pal.id, (int)BitacoraEnum.CadenaPrincipal);
            }
            _context.SaveChanges();
        }
        public bool Guardar(VMPolizaDTO o)
        {
            try
            {
                o.objPol.poliza = polizaFS.GetPolizaEkService().GetNumeroPolizaNueva(o.objPol.fecha.Year, o.objPol.fecha.Month, o.objPol.tipoPoliza).ToString();
                o.objPol.concepto = string.IsNullOrEmpty(o.objPol.concepto) ? string.Empty : o.objPol.concepto;
                var cve = getCEmpleado();
                var p = GuardarEkPoliza(o.objPol, cve);
                if (p > 0)
                {
                    o.lstMov.ForEach(x =>
                    {
                        x.cc = string.IsNullOrEmpty(x.cc) ? string.Empty : x.cc;
                        x.orderCompraCliente = string.IsNullOrEmpty(x.orderCompraCliente) ? "0" : x.orderCompraCliente;
                        if (x.tipoMovimiento.Equals("2") || x.tipoMovimiento.Equals("4"))
                            x.Monto *= -1;
                        var movPol = GuardarEkPolizaMov(x, o.objPol.fecha, int.Parse(o.objPol.poliza), o.objPol.tipoPoliza);
                        x.Monto *= -1;
                        if (movPol > 0)
                        {
                            new SwitchClass<object> {
                                {"P", () => GuardarEkMovPolProv(o.objPol, x, cve.ParseInt()) },
                                {"B", () => GuardarEkMovPolBanc(o.objPol, x) },
                                {"X", () => GuardarEkMovPolClte(o.objPol, x) },
                            }.Execute(getInterfaceSistema(int.Parse(x.cta), int.Parse(x.scta), int.Parse(x.sscta)));
                        }
                    });
                }
                else
                {
                    return false;
                }
                if (o.objPol.tipoPoliza.Equals("03"))
                    setPagoCadena();
                Guardar(o.objPol, o.lstMov);
                return true;
            }
            catch { return false; }
        }
        void setPagoCadena()
        {
            var lst = (List<tblC_CadenaProductiva>)System.Web.HttpContext.Current.Session["lstPago"];
            var _cp = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblC_CadenaProductiva>();
            var _pal = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblC_CadenaPrincipal>();
            lst.GroupBy(g => g.idPrincipal).ToList().ForEach(c =>
            {
                c.ToList().ForEach(i =>
                {
                    var prod = _cp.FirstOrDefault(w => w.id.Equals(i.id));
                    prod.pagado = true;
                    _context.SaveChanges();
                });
                var pal = _pal.FirstOrDefault(w => w.id.Equals(c.Key));
                pal.pagado = true;
                _context.SaveChanges();
            });
            System.Web.HttpContext.Current.Session["lstPago"] = null;
        }
        public void Guardar(tblPo_Poliza obj, List<tblPo_MovimientoPoliza> lst)
        {
            try
            {
                var _os = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblPo_MovimientoPoliza>();
                if (obj.id == 0)
                {
                    SaveEntity(obj, (int)BitacoraEnum.Poliza);
                    lst.ForEach(o =>
                    {
                        o.idPoliza = obj.id;
                        _os.AddObject(o);
                        _context.SaveChanges();
                    });
                }
                else
                {
                    Update(obj, obj.id, (int)BitacoraEnum.Poliza);
                    lst.ForEach(o => { _os.AddObject(o); });
                    _context.SaveChanges();
                }
            }
            catch (Exception) { throw; }
        }
        int GuardarEkPoliza(tblPo_Poliza obj, string usuario)
        {
            var id = 0;
            try
            {
                var c = new Conexion();
                var con = c.Connect();
                if (con.State.Equals("Open"))
                    con.Open();
                if (con != null)
                {
                    string insertQuery;
                    var bandera = existEkPoliza(obj.fecha, int.Parse(obj.poliza), obj.tipoPoliza);
                    if (bandera)
                    {
                        id = 0;
                        throw new Exception("El número (" + obj.poliza + ") de la póliza ya existe");
                        //insertQuery = "UPDATE sc_polizas SET year = ? ,mes = ? ,poliza = ? ,tp = ? ,fechapol = ? ,cargos = ? ,abonos = ? ,generada = ? ,status = ? ,status_lock = ? ,fec_hora_movto = ? ,usuario_movto = ? ,fecha_hora_crea = ? ,usuario_crea = ? ,concepto = ? ,error = ? WHERE year = ? AND mes = ? AND poliza = ? AND tp = ?";
                    }
                    else
                    {
                        insertQuery = "INSERT INTO sc_polizas (year ,mes ,poliza ,tp ,fechapol ,cargos ,abonos ,generada ,status ,status_lock ,fec_hora_movto ,usuario_movto ,fecha_hora_crea ,usuario_crea ,concepto ,error) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                        OdbcCommand command = new OdbcCommand(insertQuery);
                        OdbcParameterCollection parameters = command.Parameters;
                        parameters.Clear();
                        parameters.Add("@year", OdbcType.Numeric).Value = obj.fecha.Year;
                        parameters.Add("@mes", OdbcType.Numeric).Value = obj.fecha.Month;
                        parameters.Add("@poliza", OdbcType.Numeric).Value = obj.poliza.ParseInt();
                        parameters.Add("@tp", OdbcType.Char).Value = obj.tipoPoliza;
                        parameters.Add("@fechapol", OdbcType.Date).Value = obj.fecha;
                        parameters.Add("@cargos", OdbcType.Numeric).Value = obj.cargo;
                        parameters.Add("@abonos", OdbcType.Numeric).Value = -obj.abono;
                        parameters.Add("@generada", OdbcType.Char).Value = obj.generada;
                        parameters.Add("@status", OdbcType.Char).Value = obj.estatus ?? string.Empty;
                        parameters.Add("@status_lock", OdbcType.Char).Value = 'N';
                        parameters.Add("@fec_hora_movto", OdbcType.DateTime).Value = DateTime.Now;
                        parameters.Add("@usuario_movto", OdbcType.Char).Value = usuario;
                        parameters.Add("@fecha_hora_crea", OdbcType.DateTime).Value = DateTime.Now;
                        parameters.Add("@usuario_crea", OdbcType.Char).Value = usuario;
                        parameters.Add("@concepto", OdbcType.VarChar).Value = obj.concepto;
                        parameters.Add("@error", OdbcType.VarChar).Value = string.Empty;
                        //if (bandera)
                        //{
                        //    parameters.Add("@year", OdbcType.Numeric).Value = obj.fecha.Year;
                        //    parameters.Add("@mes", OdbcType.Numeric).Value = obj.fecha.Month;
                        //    parameters.Add("@poliza", OdbcType.Numeric).Value = obj.poliza.ParseInt();
                        //    parameters.Add("@tp", OdbcType.Char).Value = obj.tipoPoliza;
                        //}
                        command.Connection = con;
                        id = command.ExecuteNonQuery();
                        c.Close(con);
                    }
                }
            }
            catch (Exception) { }
            return id;
        }
        int GuardarEkPolizaMov(tblPo_MovimientoPoliza obj, DateTime fecha, int poliza, string tp)
        {
            var id = 0;
            try
            {
                var c = new Conexion();
                var con = c.Connect();
                if (con.State.Equals("Open"))
                    con.Open();
                if (con != null)
                {
                    string insertQuery;
                    var bandera = existEkMovPoliza(fecha, poliza, tp, obj.linea);
                    if (bandera)
                    {
                        throw new Exception("El número (" + poliza + ") de la póliza ya existe");
                        return 0;
                        //                        insertQuery = @"UPDATE sc_movpol
                        //                                               SET year = ?
                        //                                                  ,mes = ?
                        //                                                  ,poliza = ?
                        //                                                  ,tp = ?
                        //                                                  ,linea = ?
                        //                                                  ,cta = ?
                        //                                                  ,scta = ?
                        //                                                  ,sscta = ?
                        //                                                  ,digito = ?
                        //                                                  ,tm = ?
                        //                                                  ,referencia = ?
                        //                                                  ,cc = ?
                        //                                                  ,concepto = ?
                        //                                                  ,monto = ?
                        //                                                  ,iclave = ?
                        //                                                  ,itm = ?
                        //                                                  ,st_par = ?
                        //                                                  ,orden_compra = ?
                        //                                                  ,numpro = ?
                        //                                                  ,area = ?
                        //                                                  ,cuenta_oc = ?
                        //                                                WHERE year = ?
                        //                                                   AND mes = ?
                        //                                                   AND poliza = ?
                        //                                                   AND tp = ?
                        //                                                   AND linea = ?";
                    }
                    else
                    {
                        insertQuery = @"INSERT INTO sc_movpol 
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
                        OdbcCommand command = new OdbcCommand(insertQuery);
                        OdbcParameterCollection parameters = command.Parameters;
                        parameters.Add("@year", OdbcType.Numeric).Value = fecha.Year;
                        parameters.Add("@mes", OdbcType.Numeric).Value = fecha.Month;
                        parameters.Add("@poliza", OdbcType.Numeric).Value = poliza;
                        parameters.Add("@tp", OdbcType.Char).Value = tp;
                        parameters.Add("@linea", OdbcType.Numeric).Value = obj.linea;
                        parameters.Add("@cta", OdbcType.Numeric).Value = obj.cta.ParseInt();
                        parameters.Add("@scta", OdbcType.Numeric).Value = obj.scta.ParseInt();
                        parameters.Add("@sscta", OdbcType.Numeric).Value = obj.sscta.ParseInt();
                        parameters.Add("@digito", OdbcType.Numeric).Value = obj.digito;
                        parameters.Add("@tm", OdbcType.Numeric).Value = obj.tipoMovimiento.ParseInt();
                        parameters.Add("@referencia", OdbcType.Char).Value = obj.referencia;
                        parameters.Add("@cc", OdbcType.Char).Value = obj.cc;
                        parameters.Add("@concepto", OdbcType.Char).Value = obj.concepto;
                        parameters.Add("@monto", OdbcType.Numeric).Value = obj.Monto;
                        parameters.Add("@iclave", OdbcType.Numeric).Value = 0;
                        parameters.Add("@itm", OdbcType.Numeric).Value = obj.movimiento.ParseInt();
                        parameters.Add("@st_par", OdbcType.Char).Value = string.Empty;
                        parameters.Add("@orden_compra", OdbcType.Numeric).Value = obj.orderCompraCliente.ParseInt();
                        parameters.Add("@numpro", OdbcType.Numeric).Value = obj.numProverdor.ParseInt();
                        parameters.Add("@area", OdbcType.Numeric).Value = obj.area;
                        parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = obj.cuenta;
                        //if (bandera)
                        //{
                        //    parameters.Add("@year", OdbcType.Numeric).Value = fecha.Year;
                        //    parameters.Add("@mes", OdbcType.Numeric).Value = fecha.Month;
                        //    parameters.Add("@poliza", OdbcType.Numeric).Value = poliza;
                        //    parameters.Add("@tp", OdbcType.Char).Value = tp;
                        //    parameters.Add("@linea", OdbcType.Numeric).Value = obj.linea;
                        //}
                        command.Connection = con;
                        id = command.ExecuteNonQuery();
                        c.Close(con);
                    }
                }
            }
            catch (Exception) { }
            return id;
        }
        int GuardarEkMovPolProv(tblPo_Poliza pol, tblPo_MovimientoPoliza mov, int usuario)
        {
            var id = 0;
            try
            {
                var c = new Conexion();
                var con = c.Connect();
                if (con.State.Equals("Open"))
                    con.Open();
                if (con != null)
                {
                    string insertQuery;
                    var lstMovPolProv = existEkMovPolProv(int.Parse(mov.referencia), int.Parse(mov.numProverdor), pol.tipoPoliza);
                    var prov = getProv(Convert.ToInt32(mov.numProverdor));
                    var bandera = lstMovPolProv.Exists(w => w.poliza.Equals(pol.poliza.ParseInt()));
                    if (!bandera)
                        insertQuery = @"INSERT INTO sp_movprov 
                                                        (numpro
                                                        ,factura
                                                        ,fecha
                                                        ,tm
                                                        ,fechavenc
                                                        ,concepto
                                                        ,cc
                                                        ,referenciaoc
                                                        ,monto
                                                        ,tipocambio
                                                        ,iva
                                                        ,year
                                                        ,mes
                                                        ,poliza
                                                        ,tp
                                                        ,linea
                                                        ,generado
                                                        ,es_factura
                                                        ,moneda
                                                        ,autorizapago
                                                        ,total
                                                        ,tipocambio_oc
                                                        ,empleado_modifica
                                                        ,fecha_modifica
                                                        ,hora_modifica
                                                        ,bit_factoraje
                                                        ,bit_autoriza
                                                        ,bit_transferida
                                                        ,bit_pagada
                                                        ,empleado
                                                        ,cfd_folio
                                                        ,afectacompra
                                                        ,val_ref
                                                        ,pide_iva
                                                        ,valida_recibido
                                                        ,valida_almacen
                                                        ,valida_recibido_autorizar)
                                                VALUES (?,?,?,?,?,?,?,?,?,?
                                                       ,?,?,?,?,?,?,?,?,?,?
                                                       ,?,?,?,?,?,?,?,?,?,?
                                                       ,?,?,?,?,?,?,?)";
                    else
                        insertQuery = @"UPDATE sp_movprov
                                           SET numpro = ?
                                              ,factura = ?
                                              ,fecha = ?
                                              ,tm = ?
                                              ,fechavenc = ?
                                              ,concepto = ?
                                              ,cc = ?
                                              ,referenciaoc = ?
                                              ,monto = ?
                                              ,tipocambio = ?
                                              ,iva = ?
                                              ,year = ?
                                              ,mes = ?
                                              ,poliza = ?
                                              ,tp = ?
                                              ,linea = ?
                                              ,generado = ?
                                              ,es_factura = ?
                                              ,moneda = ?
                                              ,autorizapago = ?
                                              ,total = ?
                                              ,tipocambio_oc = ?
                                              ,empleado_modifica = ?
                                              ,fecha_modifica = ?
                                              ,hora_modifica = ?
                                              ,bit_factoraje = ?
                                              ,bit_autoriza = ?
                                              ,bit_transferida = ?
                                              ,bit_pagada = ?
                                              ,empleado = ?
                                              ,cfd_folio = ?
                                              ,afectacompra = ?
                                              ,val_ref = ?
                                              ,pide_iva = ?
                                              ,valida_recibido = ?
                                              ,valida_almacen = ?
                                              ,valida_recibido_autorizar = ?

                                            WHERE year = ?
                                               AND mes = ?
                                               AND poliza = ?
                                               AND tp = ?
                                               AND linea = ?";

                    OdbcCommand command = new OdbcCommand(insertQuery);
                    OdbcParameterCollection parameters = command.Parameters;
                    parameters.Add("@numpro", OdbcType.Numeric).Value = mov.numProverdor.ParseInt();
                    parameters.Add("@factura", OdbcType.Numeric).Value = mov.referencia.ParseInt();
                    parameters.Add("@fecha", OdbcType.Date).Value = pol.fecha;
                    parameters.Add("@tm", OdbcType.Numeric).Value = mov.movimiento.ParseInt();
                    parameters.Add("@fechavenc", OdbcType.Date).Value = pol.fecha.AddDays(prov.condpago);
                    parameters.Add("@concepto", OdbcType.Char).Value = mov.concepto;
                    parameters.Add("@cc", OdbcType.Char).Value = mov.cc;
                    parameters.Add("@referenciaoc", OdbcType.Char).Value = mov.orderCompraCliente ?? string.Empty;
                    var ant = existEkMovPolProv(int.Parse(mov.referencia), int.Parse(mov.numProverdor), pol.tipoPoliza.Equals("04") ? "07" : pol.tipoPoliza.Equals("03") ? "04" : pol.tipoPoliza);
                    var dll = mov.numProverdor.ParseInt() > 9000 ? ant.Count == 0 ? cadenaFS.getCadenaProductivaService().getDolarDelDia(pol.fecha) : ant.FirstOrDefault().tipocambio : 1;
                    parameters.Add("@monto", OdbcType.Numeric).Value = mov.Monto;
                    parameters.Add("@tipocambio", OdbcType.Numeric).Value = dll;
                    parameters.Add("@iva", OdbcType.Numeric).Value = 0;
                    parameters.Add("@year", OdbcType.Numeric).Value = pol.fecha.Year;
                    parameters.Add("@mes", OdbcType.Numeric).Value = pol.fecha.Month;
                    parameters.Add("@poliza", OdbcType.Numeric).Value = pol.poliza.ParseInt();
                    parameters.Add("@tp", OdbcType.Char).Value = pol.tipoPoliza;
                    parameters.Add("@linea", OdbcType.Numeric).Value = mov.linea;
                    parameters.Add("@generado", OdbcType.Char).Value = pol.generada;
                    parameters.Add("@es_factura", OdbcType.Char).Value = ant.Count > 0 ? "N" : "S";
                    parameters.Add("@moneda", OdbcType.Char).Value = mov.numProverdor.ParseInt() > 9000 ? 2 : 1;
                    parameters.Add("@autorizapago", OdbcType.Char).Value = string.Empty;
                    parameters.Add("@total", OdbcType.Numeric).Value = mov.Monto;
                    parameters.Add("@tipocambio_oc", OdbcType.Numeric).Value = 1;
                    parameters.Add("@empleado_modifica", OdbcType.Numeric).Value = usuario;
                    parameters.Add("@fecha_modifica", OdbcType.Date).Value = DateTime.Now;
                    parameters.Add("@hora_modifica", OdbcType.DateTime).Value = DateTime.Now;
                    parameters.Add("@bit_factoraje", OdbcType.Char).Value = prov.bit_factoraje;
                    parameters.Add("@bit_autoriza", OdbcType.Char).Value = "N";
                    parameters.Add("@bit_transferida", OdbcType.Char).Value = "N";
                    parameters.Add("@bit_pagada", OdbcType.Char).Value = "N";
                    parameters.Add("@empleado", OdbcType.Numeric).Value = usuario;
                    parameters.Add("@cfd_folio", OdbcType.Numeric).Value = mov.referencia.ParseInt();
                    parameters.Add("@afectacompra", OdbcType.Char).Value = "S";
                    parameters.Add("@val_ref", OdbcType.Char).Value = "0";
                    parameters.Add("@pide_iva", OdbcType.Char).Value = "S";
                    parameters.Add("@valida_recibido", OdbcType.Char).Value = string.Empty;
                    parameters.Add("@valida_almacen", OdbcType.Char).Value = "N";
                    parameters.Add("@valida_recibido_autorizar", OdbcType.Char).Value = "N";
                    if (bandera)
                    {
                        parameters.Add("@year", OdbcType.Numeric).Value = pol.fecha.Year;
                        parameters.Add("@mes", OdbcType.Numeric).Value = pol.fecha.Month;
                        parameters.Add("@poliza", OdbcType.Numeric).Value = pol.poliza.ParseInt();
                        parameters.Add("@tp", OdbcType.Char).Value = pol.tipoPoliza;
                        parameters.Add("@linea", OdbcType.Numeric).Value = mov.linea;
                    }
                    command.Connection = con;
                    id = command.ExecuteNonQuery();
                    c.Close(con);
                }
            }
            catch (Exception) { }
            return id;
        }
        int GuardarEkMovPolBanc(tblPo_Poliza pol, tblPo_MovimientoPoliza mov)
        {
            var i = GuardarEkEdoCtaChequera(pol, mov);
            return i;
        }
        int GuardarEkEdoCtaChequera(tblPo_Poliza pol, tblPo_MovimientoPoliza mov)
        {
            try
            {
                var c = new Conexion();
                var con = c.Connect();
                if (con.State.Equals("Open"))
                    con.Open();
                if (con != null)
                {
                    string insertQuery;
                    var cc = mov.cc.Substring(0, 3);
                    var ctaBanco = getCuentaDesc(Convert.ToInt32(mov.cta), Convert.ToInt32(mov.scta), Convert.ToInt32(mov.sscta), pol.tipoPoliza);
                    var bandera = existEkMovPolBanc(ctaBanco.cuenta, mov.referencia.ParseInt(), cc, pol.tipoPoliza);
                    if (bandera)
                        insertQuery = @"UPDATE sb_edo_cta_chequera
                                           SET cuenta = ?
                                              ,fecha_mov = ?
                                              ,tm = ?
                                              ,numero = ?
                                              ,cc = ?
                                              ,descripcion = ?
                                              ,monto = ?
                                              ,tc = ?
                                              ,origen_mov = ?
                                              ,generada = ?
                                              ,st_consilia = ?
                                              ,num_consilia = ?
                                              ,st_che = ?
                                              ,ref_che_inverso = ?
                                              ,ref_tm_inverso = ?
                                              ,motivo_cancelado = ?
                                              ,iyear = ?
                                              ,imes = ?
                                              ,ipoliza = ?
                                              ,itp = ?
                                              ,ilinea = ?
                                              ,banco = ?
                                            WHERE cuenta = ?
                                               AND fecha_mov = ?
                                               AND tm = ?
                                               AND numero = ?
                                               AND cc = ?";
                    else
                        insertQuery = @"INSERT INTO sb_edo_cta_chequera 
                                                        (cuenta
                                                        ,fecha_mov
                                                        ,tm
                                                        ,numero
                                                        ,cc
                                                        ,descripcion
                                                        ,monto
                                                        ,tc
                                                        ,origen_mov
                                                        ,generada
                                                        ,st_consilia
                                                        ,num_consilia
                                                        ,st_che
                                                        ,ref_che_inverso
                                                        ,ref_tm_inverso
                                                        ,motivo_cancelado
                                                        ,iyear
                                                        ,imes
                                                        ,ipoliza
                                                        ,itp
                                                        ,ilinea
                                                        ,banco)
                                                VALUES (?,?,?,?,?,?,?,?,?,?
                                                        ,?,?,?,?,?,?,?,?,?,?
                                                        ,?,?)";
                    OdbcCommand command = new OdbcCommand(insertQuery);
                    OdbcParameterCollection parameters = command.Parameters;
                    parameters.Add("@cuenta", OdbcType.Numeric).Value = ctaBanco.cuenta;
                    parameters.Add("@fecha_mov", OdbcType.Date).Value = pol.fecha;
                    parameters.Add("@tm", OdbcType.Numeric).Value = mov.movimiento;
                    parameters.Add("@numero", OdbcType.Numeric).Value = mov.referencia;
                    parameters.Add("@cc", OdbcType.Char).Value = cc;
                    parameters.Add("@descripcion", OdbcType.Char).Value = mov.concepto;
                    parameters.Add("@monto", OdbcType.Numeric).Value = mov.Monto;
                    parameters.Add("@tc", OdbcType.Numeric).Value = ctaBanco.moneda == 2 ? cadenaFS.getCadenaProductivaService().getDolarDelDia(pol.fecha) : 1;
                    parameters.Add("@origen_mov", OdbcType.Char).Value = pol.estatus;
                    parameters.Add("@generada", OdbcType.Char).Value = pol.generada;
                    parameters.Add("@st_consilia", OdbcType.Char).Value = string.Empty;
                    parameters.Add("@num_consilia", OdbcType.Numeric).Value = 0;
                    parameters.Add("@st_che", OdbcType.Char).Value = string.Empty;
                    parameters.Add("@ref_che_inverso", OdbcType.Numeric).Value = 0;
                    parameters.Add("@ref_tm_inverso", OdbcType.Numeric).Value = 0;
                    parameters.Add("@motivo_cancelado", OdbcType.Char).Value = string.Empty;
                    parameters.Add("@iyear", OdbcType.Numeric).Value = pol.fecha.Year;
                    parameters.Add("@imes", OdbcType.Numeric).Value = pol.fecha.Month;
                    parameters.Add("@ipoliza", OdbcType.Numeric).Value = pol.poliza;
                    parameters.Add("@itp", OdbcType.Char).Value = pol.tipoPoliza;
                    parameters.Add("@ilinea", OdbcType.Numeric).Value = mov.linea;
                    parameters.Add("@banco", OdbcType.Numeric).Value = ctaBanco.banco;
                    if (bandera)
                    {
                        parameters.Add("@cuenta", OdbcType.Numeric).Value = ctaBanco.cuenta;
                        parameters.Add("@fecha_mov", OdbcType.Date).Value = pol.fecha;
                        parameters.Add("@tm", OdbcType.Numeric).Value = mov.movimiento;
                        parameters.Add("@numero", OdbcType.Numeric).Value = mov.referencia;
                        parameters.Add("@cc", OdbcType.Char).Value = cc;
                    }
                    command.Connection = con;
                    var id = command.ExecuteNonQuery();
                    c.Close(con);
                    return id;
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        int GuardarEkEdoCheques(tblPo_Poliza pol, tblPo_MovimientoPoliza mov)
        {
            try
            {
                var c = new Conexion();
                var con = c.Connect();
                if (con.State.Equals("Open"))
                    con.Open();
                if (con != null)
                {
                    string insertQuery;
                    var ctaBanco = getCuentaDesc(Convert.ToInt32(mov.cta), Convert.ToInt32(mov.scta), Convert.ToInt32(mov.sscta), pol.tipoPoliza);
                    var bandera = existEkMovPolBanc(ctaBanco.cuenta, mov.referencia.ParseInt(), mov.cc, pol.tipoPoliza);
                    if (bandera)
                        insertQuery = @"UPDATE sb_cheques
                                           SET cuenta = ?
                                              ,fecha_mov = ?
                                              ,tm = ?
                                              ,numero = ?
                                              ,cc = ?
                                              ,descripcion = ?
                                              ,monto = ?
                                              ,tc = ?
                                              ,origen_mov = ?
                                              ,generada = ?
                                              ,st_consilia = ?
                                              ,num_consilia = ?
                                              ,st_che = ?
                                              ,ref_che_inverso = ?
                                              ,ref_tm_inverso = ?
                                              ,motivo_cancelado = ?
                                              ,iyear = ?
                                              ,imes = ?
                                              ,ipoliza = ?
                                              ,itp = ?
                                              ,ilinea = ?
                                              ,banco = ?
                                            WHERE cuenta = ?
                                               AND fecha_mov = ?
                                               AND tm = ?
                                               AND cc = ?";
                    else
                        insertQuery = @"INSERT INTO sb_cheques 
                                                        (cuenta
                                                        ,fecha_mov
                                                        ,tm
                                                        ,numero
                                                        ,cc
                                                        ,descripcion
                                                        ,monto
                                                        ,tc
                                                        ,origen_mov
                                                        ,generada
                                                        ,st_consilia
                                                        ,num_consilia
                                                        ,st_che
                                                        ,ref_che_inverso
                                                        ,ref_tm_inverso
                                                        ,motivo_cancelado
                                                        ,iyear
                                                        ,imes
                                                        ,ipoliza
                                                        ,itp
                                                        ,ilinea
                                                        ,banco)
                                                VALUES (?,?,?,?,?,?,?,?,?,?
                                                        ,?,?,?,?,?,?,?,?,?,?
                                                        ,?,?)";
                    OdbcCommand command = new OdbcCommand(insertQuery);
                    OdbcParameterCollection parameters = command.Parameters;
                    parameters.Add("@cuenta", OdbcType.Numeric).Value = ctaBanco.cuenta;
                    parameters.Add("@fecha_mov", OdbcType.Date).Value = pol.fecha;
                    parameters.Add("@tm", OdbcType.Numeric).Value = mov.tipoMovimiento;
                    parameters.Add("@numero", OdbcType.Numeric).Value = pol.poliza;
                    parameters.Add("@descripcion", OdbcType.Char).Value = mov.concepto;
                    parameters.Add("@cc", OdbcType.Char).Value = mov.cc;
                    parameters.Add("@monto", OdbcType.Numeric).Value = mov.Monto;
                    parameters.Add("@hecha_por", OdbcType.Char).Value = "Sigoplan";
                    parameters.Add("@status_bco", OdbcType.Char).Value = string.Empty;
                    parameters.Add("@status_lp", OdbcType.Char).Value = "N";
                    parameters.Add("@tc", OdbcType.Numeric).Value = cadenaFS.getCadenaProductivaService().getDolarDelDia(pol.fecha);
                    parameters.Add("@origen_mov", OdbcType.Char).Value = pol.estatus;
                    parameters.Add("@generada", OdbcType.Char).Value = pol.generada;
                    parameters.Add("@st_consilia", OdbcType.Char).Value = string.Empty;
                    parameters.Add("@num_consilia", OdbcType.Numeric).Value = 0;
                    parameters.Add("@st_che", OdbcType.Char).Value = string.Empty;
                    parameters.Add("@ref_che_inverso", OdbcType.Numeric).Value = 0;
                    parameters.Add("@ref_tm_inverso", OdbcType.Numeric).Value = 0;
                    parameters.Add("@motivo_cancelado", OdbcType.Char).Value = string.Empty;
                    parameters.Add("@iyear", OdbcType.Numeric).Value = pol.fecha.Year;
                    parameters.Add("@imes", OdbcType.Numeric).Value = pol.fecha.Month;
                    parameters.Add("@ipoliza", OdbcType.Numeric).Value = pol.poliza;
                    parameters.Add("@itp", OdbcType.Char).Value = pol.tipoPoliza;
                    parameters.Add("@ilinea", OdbcType.Numeric).Value = mov.linea;
                    parameters.Add("@banco", OdbcType.Numeric).Value = ctaBanco.banco;
                    if (bandera)
                    {
                        parameters.Add("@cuenta", OdbcType.Numeric).Value = ctaBanco.cuenta;
                        parameters.Add("@fecha_mov", OdbcType.Date).Value = pol.fecha;
                        parameters.Add("@tm", OdbcType.Numeric).Value = mov.tipoMovimiento;
                        parameters.Add("@cc", OdbcType.Char).Value = mov.cc;
                    }
                    command.Connection = con;
                    var id = command.ExecuteNonQuery();
                    c.Close(con);
                    return id;
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        int GuardarEkMovPolClte(tblPo_Poliza pol, tblPo_MovimientoPoliza mov)
        {
            try
            {
                var c = new Conexion();
                var con = c.Connect();
                if (con.State.Equals("Open"))
                    con.Open();
                if (con != null)
                {
                    string insertQuery;
                    var bandera = existEkMovPolClte(pol.fecha, int.Parse(pol.poliza), pol.tipoPoliza, mov.linea);
                    var clte = getClie(int.Parse(mov.orderCompraCliente));
                    if (bandera)
                        insertQuery = @"UPDATE sx_movcltes
                                           SET numcte = ?
                                              ,factura = ?
                                              ,fecha = ?
                                              ,tm = ?
                                              ,fechavenc = ?
                                              ,concepto = ?
                                              ,cc = ?
                                              ,referenciaoc = ?
                                              ,monto = ?
                                              ,tipocambio = ?
                                              ,iva = ?
                                              ,year = ?
                                              ,mes = ?
                                              ,poliza = ?
                                              ,tp = ?
                                              ,linea = ?
                                              ,generado = ?
                                              ,es_factura = ?
                                              ,moneda = ?
                                              ,autorizapago = ?
                                              ,total = ?
                                              ,bit_escrituracion = ?
                                              ,poliza_generada = ?
                                              ,descuentos = ?
                                              ,cfd_serie = ?
                                              ,cfd_folio = ?
                                              ,cfd_fecha = ?
                                              ,cfd_certificado = ?
                                              ,cfd_ano_aprob = ?
                                              ,cfd_num_aprob = ?
                                              ,cfd_ruta_pdf = ?
                                              ,cfd_ruta_xml = ?
                                              ,cfd_monto = ?
                                              ,cfd_iva = ?
                                              ,cfd_total = ?
                                              ,cfd_mn = ?
                                              ,fecha_registro = ?
                                              ,no_parcialidades = ?
                                              ,parcialidades = ?
                                              ,anticipo = ?
                                              ,pago_anticipo = ?
                                              ,cancelacion_anticipo = ?
                                              ,desglosa_iva_anticipo = ?
                                            WHERE year = ?
                                               AND mes = ?
                                               AND poliza = ?
                                               AND tp = ?
                                               AND linea = ?";
                    else
                        insertQuery = @"INSERT INTO sx_movcltes 
                                                        (numcte
                                                        ,factura
                                                        ,fecha
                                                        ,tm
                                                        ,fechavenc
                                                        ,concepto
                                                        ,cc
                                                        ,referenciaoc
                                                        ,monto
                                                        ,tipocambio
                                                        ,iva
                                                        ,year
                                                        ,mes
                                                        ,poliza
                                                        ,tp
                                                        ,linea
                                                        ,generado
                                                        ,es_factura
                                                        ,moneda
                                                        ,autorizapago
                                                        ,total
                                                        ,bit_escrituracion
                                                        ,poliza_generada
                                                        ,descuentos
                                                        ,cfd_serie
                                                        ,cfd_folio
                                                        ,cfd_fecha
                                                        ,cfd_certificado
                                                        ,cfd_ano_aprob
                                                        ,cfd_num_aprob
                                                        ,cfd_ruta_pdf
                                                        ,cfd_ruta_xml
                                                        ,cfd_monto
                                                        ,cfd_iva
                                                        ,cfd_total
                                                        ,cfd_mn
                                                        ,fecha_registro
                                                        ,no_parcialidades
                                                        ,parcialidades
                                                        ,anticipo
                                                        ,pago_anticipo
                                                        ,cancelacion_anticipo
                                                        ,desglosa_iva_anticipo) 
                                                VALUES (?,?,?,?,?,?,?,?,?,?
                                                       ,?,?,?,?,?,?,?,?,?,?
                                                       ,?,?,?,?,?,?,?,?,?,?
                                                       ,?,?,?,?,?,?,?,?,?,?,?,?,?)";
                    OdbcCommand command = new OdbcCommand(insertQuery);
                    OdbcParameterCollection parameters = command.Parameters;
                    parameters.Add("@numcte", OdbcType.Numeric).Value = mov.orderCompraCliente;
                    parameters.Add("@factura", OdbcType.Numeric).Value = mov.referencia;
                    parameters.Add("@fecha", OdbcType.Date).Value = pol.fecha;
                    parameters.Add("@tm", OdbcType.Numeric).Value = mov.movimiento;
                    parameters.Add("@fechavenc", OdbcType.Date).Value = pol.fecha.AddDays(clte.condpago);
                    parameters.Add("@concepto", OdbcType.Char).Value = mov.concepto;
                    parameters.Add("@cc", OdbcType.Char).Value = mov.cc;
                    parameters.Add("@referenciaoc", OdbcType.Char).Value = string.Empty;
                    var iva = mov.Monto * .16m;
                    parameters.Add("@monto", OdbcType.Numeric).Value = mov.Monto - iva;
                    parameters.Add("@tipocambio", OdbcType.Numeric).Value = cadenaFS.getCadenaProductivaService().getDolarDelDia(pol.fecha);
                    parameters.Add("@iva", OdbcType.Numeric).Value = iva;
                    parameters.Add("@year", OdbcType.Numeric).Value = pol.fecha.Year;
                    parameters.Add("@mes", OdbcType.Numeric).Value = pol.fecha.Month;
                    parameters.Add("@poliza", OdbcType.Numeric).Value = pol.poliza;
                    parameters.Add("@tp", OdbcType.Char).Value = pol.tipoPoliza;
                    parameters.Add("@linea", OdbcType.Numeric).Value = mov.linea;
                    parameters.Add("@generado", OdbcType.Char).Value = pol.generada;
                    parameters.Add("@es_factura", OdbcType.Char).Value = string.Empty;
                    parameters.Add("@moneda", OdbcType.Char).Value = int.Parse(mov.orderCompraCliente) >= 9000 ? 1 : 2;
                    parameters.Add("@autorizapago", OdbcType.Char).Value = string.Empty;
                    parameters.Add("@total", OdbcType.Numeric).Value = mov.Monto;
                    parameters.Add("@bit_escrituracion", OdbcType.Char).Value = 'N';
                    parameters.Add("@poliza_generada", OdbcType.Char).Value = 'S';
                    parameters.Add("@descuentos", OdbcType.Decimal).Value = 0;
                    parameters.Add("@cfd_serie", OdbcType.VarChar).Value = String.Empty;
                    parameters.Add("@cfd_folio", OdbcType.Numeric).Value = 0;
                    parameters.Add("@cfd_fecha", OdbcType.DateTime).Value = new DateTime(1900, 1, 1);
                    parameters.Add("@cfd_certificado", OdbcType.Char).Value = string.Empty;
                    parameters.Add("@cfd_ano_aprob", OdbcType.Numeric).Value = 0;
                    parameters.Add("@cfd_num_aprob", OdbcType.Numeric).Value = 0;
                    parameters.Add("@cfd_ruta_pdf", OdbcType.VarChar).Value = string.Empty;
                    parameters.Add("@cfd_ruta_xml", OdbcType.VarChar).Value = string.Empty;
                    parameters.Add("@cfd_monto", OdbcType.Numeric).Value = 0;
                    parameters.Add("@cfd_iva", OdbcType.Numeric).Value = 0;
                    parameters.Add("@cfd_total", OdbcType.Numeric).Value = 0;
                    parameters.Add("@cfd_mn", OdbcType.Char).Value = 'N';
                    parameters.Add("@fecha_registro", OdbcType.Date).Value = DateTime.Now;
                    parameters.Add("@no_parcialidades", OdbcType.Numeric).Value = 0;
                    parameters.Add("@parcialidades", OdbcType.Numeric).Value = 0;
                    parameters.Add("@anticipo", OdbcType.VarChar).Value = 'N';
                    parameters.Add("@pago_anticipo", OdbcType.VarChar).Value = 'N';
                    parameters.Add("@cancelacion_anticipo", OdbcType.VarChar).Value = 'N';
                    parameters.Add("@desglosa_iva_anticipo", OdbcType.VarChar).Value = 'N';
                    if (bandera)
                    {
                        parameters.Add("@year", OdbcType.Numeric).Value = pol.fecha.Year;
                        parameters.Add("@mes", OdbcType.Numeric).Value = pol.fecha.Month;
                        parameters.Add("@poliza", OdbcType.Numeric).Value = pol.poliza;
                        parameters.Add("@tp", OdbcType.Char).Value = pol.tipoPoliza;
                        parameters.Add("@linea", OdbcType.Numeric).Value = mov.linea;
                    }
                    command.Connection = con;
                    var id = command.ExecuteNonQuery();
                    c.Close(con);
                    return id;
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        bool existEkPoliza(DateTime fecha, int poliza, string tp)
        {
            var lst = getPolizaEk(fecha, poliza, tp);
            return lst.Count > 0;
        }
        bool existEkPoliza(DateTime perIni, DateTime perFin, int poliza, string tp)
        {
            var lst = getPolizaEk(perIni, perFin, poliza, tp);
            return lst.Count > 0;
        }
        bool existEkMovPoliza(DateTime fecha, int poliza, string tp, int linea)
        {
            var lst = getMovPolizaEk(fecha, poliza, tp);
            return lst.Exists(p => p.linea == linea && p.cta != 0);
        }
        List<MovProDTO> existEkMovPolProv(int factura, int numProv, string tp)
        {
            try
            {
                string consulta = string.Format(@"SELECT * FROM sp_movprov
                                                    WHERE numpro = {0} 
                                                        AND factura = {1}
                                                        AND tp = '{2}'",
                                                       numProv, factura, tp);
                return (List<MovProDTO>)_contextEnkontrol.Where(consulta).ToObject<List<MovProDTO>>();
            }
            catch (Exception)
            {
                return new List<MovProDTO>();
            }
        }
        bool existEkMovPolBanc(int cuenta, int numero, string cc, string tp)
        {
            try
            {
                string consulta = string.Format(@"SELECT * FROM sb_edo_cta_chequera WHERE cuenta = {0} AND numero = {1} AND cc = '{2}' AND itp = '{3}'"
                    , cuenta, numero, cc, tp);
                var lst = (List<MovProDTO>)_contextEnkontrol.Where(consulta).ToObject<List<MovProDTO>>();
                return lst.Count > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        bool existEkMovPolClte(DateTime fecha, int poliza, string tp, int linea)
        {
            try
            {
                string consulta = string.Format(@"SELECT * FROM sx_movcltes
                                                            WHERE year = {0}
                                                                AND mes = {1}
                                                                AND poliza = {2} 
                                                                AND tp = {3}
                                                                AND linea = {4}", fecha.Year
                                                                                , fecha.Month
                                                                                , poliza
                                                                                 , tp
                                                                                 , linea);
                var lst = (List<MovProDTO>)_contextEnkontrol.Where(consulta).ToObject<List<MovProDTO>>();
                return lst.Count > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        int getUlimoIncrementoFactura()
        {
            try
            {
                string consulta = string.Format(@"SELECT TOP 1 autoincremento FROM sp_movprov
                                                    ORDER BY autoincremento DESC");
                var lst = (List<MovProDTO>)_contextEnkontrol.Where(consulta).ToObject<List<MovProDTO>>();
                return (lst.FirstOrDefault().autoincremento + 1);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public string getCEmpleado()
        {
            return _context.tblP_Usuario_Enkontrol.FirstOrDefault(e => e.idUsuario == vSesiones.sesionUsuarioDTO.id).sn_empleado.ToString();
        }
        #endregion
        #region Reporte
        public List<RepPolizaDTO> getPolizaEk(string Estatus, int iPol, int fPol, string iPer, string fPer, string iTp, string fTp)
        {
            try
            {
                string consulta = string.Format(@"SELECT poliza, tp, fechapol, cargos, abonos,(cargos + abonos) AS diferencia,  status  FROM sc_polizas
                                                    WHERE status = '{0}'
                                                        AND poliza >= {1}
                                                        AND poliza <= {2}
                                                        AND year(fechapol) >= {3}
                                                        AND year(fechapol) <= {4}
                                                        AND month(fechapol) >= {5}
                                                        AND month(fechapol) <= {6}
                                                        AND tp >= '{7}'
                                                        AND tp <= '{8}'", Estatus,
                                                                         iPol,
                                                                         fPol,
                                                                         iPer.Substring(3, 4),
                                                                         fPer.Substring(3, 4),
                                                                         int.Parse(iPer.Substring(0, 2)),
                                                                         int.Parse(fPer.Substring(0, 2)),
                                                                         iTp,
                                                                         fTp);
                var lstEkP = (List<RepPolizaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<RepPolizaDTO>>();
                return lstEkP.ToList();
            }
            catch (Exception) { return new List<RepPolizaDTO>(); }
        }

        public List<RepPolizaDTO> getPolizaEk(int year, int mes, int poliza, string tp)
        {
            try
            {
                string consulta = string.Format(@"SELECT poliza, tp, fechapol, cargos, abonos,(cargos + abonos) AS diferencia,  status  FROM sc_polizas
                                                    WHERE year = {0} AND year = {1} AND mes = {2} AND tp = '{3}'", year, mes, poliza, tp);
                var lstEkP = (List<RepPolizaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<RepPolizaDTO>>();
                return lstEkP.ToList();
            }
            catch (Exception) { return new List<RepPolizaDTO>(); }
        }
        public List<RepPolizaDTO> getPolizaEkPruebaArrendadora(string Estatus, int iPol, int fPol, string iPer, string fPer, string iTp, string fTp)
        {
            try
            {
                string consulta = string.Format(@"SELECT poliza, tp, fechapol, cargos, abonos,(cargos + abonos) AS diferencia,  status  FROM sc_polizas
                                                    WHERE status = '{0}'
                                                        AND poliza >= {1}
                                                        AND poliza <= {2}
                                                        AND year(fechapol) >= {3}
                                                        AND year(fechapol) <= {4}
                                                        AND month(fechapol) >= {5}
                                                        AND month(fechapol) <= {6}
                                                        AND tp >= '{7}'
                                                        AND tp <= '{8}'", Estatus,
                                                                         iPol,
                                                                         fPol,
                                                                         iPer.Substring(3, 4),
                                                                         fPer.Substring(3, 4),
                                                                         int.Parse(iPer.Substring(0, 2)),
                                                                         int.Parse(fPer.Substring(0, 2)),
                                                                         iTp,
                                                                         fTp);
                var lstEkP = _contextEnkontrol.Select<RepPolizaDTO>(EnkontrolEnum.PruebaCplanProd, consulta);
                return lstEkP.ToList();
            }
            catch (Exception) { return new List<RepPolizaDTO>(); }
        }
        public List<RepMovPoliza2DTO> getMovPolizaEk(string Estatus, int iPol, int fPol, string iPer, string fPer, string iTp, string fTp, string icc, string fcc)
        {
            try
            {
                string consulta = string.Format(@"SELECT p.fechapoL, p.poliza, p.tp, p.generada, p.status, p.usuario_movto, p.fec_hora_movto, p.usuario_crea, p.fecha_hora_crea,
                                                m.linea, m.cta, m.scta, m.sscta, m.digito,  m.cc, m.referencia, m.orden_compra, m.concepto, m.forma_pago, m.tm, m.itm, m.monto, p.error, m.area, m.cuenta_oc,
                                                (SELECT descripcion FROM catcta c WHERE c.cta = m.cta AND c.scta = m.scta AND c.sscta = m.sscta)
                                                FROM sc_polizas p   
                                                INNER JOIN sc_movpol m 
                                                WHERE p.status = '{0}'
                                                    AND m.poliza >= {1}
                                                    AND m.poliza <= {2}
                                                    AND year(p.fechapol) >= {3}
                                                    AND year(p.fechapol) <= {4}
                                                    AND month(p.fechapol) >= {5}
                                                    AND month(p.fechapol) <= {6}
                                                    AND m.tp >= '{7}'
                                                    AND m.tp <= '{8}'
                                                    AND m.cc >= '{9}'
                                                    AND m.cc <= '{10}'", Estatus,
                                                                         iPol,
                                                                         fPol,
                                                                         iPer.Substring(3, 4),
                                                                         fPer.Substring(3, 4),
                                                                         int.Parse(iPer.Substring(0, 2)),
                                                                         int.Parse(fPer.Substring(0, 2)),
                                                                         iTp,
                                                                         fTp,
                                                                         icc,
                                                                         fcc);
                var lstEkP = (List<RepMovPolizaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<RepMovPolizaDTO>>();
                var lst2 = lstEkP.Select(x => new RepMovPoliza2DTO()
                {
                    cargo = (x.tm == 1) || (x.tm == 3) ? x.monto : 0,
                    abono = (x.tm == 2) || (x.tm == 4) ? x.monto : 0,
                    usuario_movto = x.usuario_movto == null ? "" : Convert.ToString(x.usuario_movto.Value),
                    fec_hora_movto = x.fec_hora_movto == null ? "" : Convert.ToDateTime(x.fec_hora_movto.Value).ToShortDateString(),
                    forma_pago = x.forma_pago == null ? String.Empty : x.forma_pago,
                    error = string.IsNullOrEmpty(x.error) ? string.Empty : x.error,
                    status = setEstatus(x.status),
                    generada = setGenerada(x.generada),
                    cc = x.cc,
                    concepto = x.concepto,
                    cta = x.cta,
                    descripcion = x.descripcion,
                    digito = x.digito,
                    fecha_hora_crea = x.fecha_hora_crea,
                    fechapol = x.fechapol,
                    itm = x.itm,
                    linea = x.linea,
                    monto = x.monto,
                    orden_compra = x.orden_compra,
                    poliza = x.poliza,
                    referencia = x.referencia,
                    scta = x.scta,
                    sscta = x.sscta,
                    tm = x.tm,
                    tp = x.tp,
                    usuario_crea = x.usuario_crea.ToString()
                });
                return lst2.ToList();
            }
            catch (Exception ex) { return new List<RepMovPoliza2DTO>(); }
        }

        public List<RepMovPoliza2DTO> getMovPolizaEk(int year, int mes, int poliza, string tp)
        {
            try
            {
                string consulta = string.Format(@"SELECT p.fechapoL, p.poliza, p.tp, p.generada, p.status, p.usuario_movto, p.fec_hora_movto, p.usuario_crea, p.fecha_hora_crea,
                                                m.linea, m.cta, m.scta, m.sscta, m.digito,  m.cc, m.referencia, m.orden_compra, m.concepto, m.forma_pago, m.tm, m.itm, m.monto, p.error, m.area, m.cuenta_oc,
                                                (SELECT descripcion FROM catcta c WHERE c.cta = m.cta AND c.scta = m.scta AND c.sscta = m.sscta),
                                                e.descripcion as usuarioCreacion, ee.descripcion as usuarioModificacion
                                                FROM sc_polizas p   
                                                INNER JOIN sc_movpol m on m.year = p.year and m.mes = p.mes and p.poliza = m.poliza and m.tp = p.tp
                                                LEFT JOIN empleados as e on e.empleado = p.usuario_crea
                                                LEFT JOIN empleados as ee on ee.empleado = p.usuario_movto
                                                WHERE p.year = {0} AND p.mes = {1} AND p.poliza = {2} AND p.tp = '{3}'", year, mes, poliza, tp);
                var lstEkP = (List<RepMovPolizaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<RepMovPolizaDTO>>();
                var lst2 = lstEkP.Select(x => new RepMovPoliza2DTO()
                {
                    cargo = (x.tm == 1) || (x.tm == 3) ? x.monto : 0,
                    abono = (x.tm == 2) || (x.tm == 4) ? x.monto : 0,
                    usuario_movto = x.usuario_movto == null ? "" : Convert.ToString(x.usuario_movto.Value) + " " + x.usuarioModificacion,
                    fec_hora_movto = x.fec_hora_movto == null ? "" : Convert.ToDateTime(x.fec_hora_movto.Value).ToShortDateString(),
                    forma_pago = x.forma_pago == null ? String.Empty : x.forma_pago,
                    error = string.IsNullOrEmpty(x.error) ? string.Empty : x.error,
                    status = setEstatus(x.status),
                    generada = setGenerada(x.generada),
                    cc = x.cc,
                    concepto = x.concepto,
                    cta = x.cta,
                    descripcion = x.descripcion,
                    digito = x.digito,
                    fecha_hora_crea = x.fecha_hora_crea,
                    fechapol = x.fechapol,
                    itm = x.itm,
                    linea = x.linea,
                    monto = x.monto,
                    orden_compra = x.orden_compra,
                    poliza = x.poliza,
                    referencia = x.referencia,
                    scta = x.scta,
                    sscta = x.sscta,
                    tm = x.tm,
                    tp = x.tp,
                    usuario_crea = x.usuario_crea + " " + x.usuarioCreacion
                });
                return lst2.ToList();
            }
            catch (Exception ex) { return new List<RepMovPoliza2DTO>(); }
        }
        

        public List<RepMovPoliza2DTO> getMovPolizaEkPruebaArrendadora(string Estatus, int iPol, int fPol, string iPer, string fPer, string iTp, string fTp, string icc, string fcc)
        {
            try
            {
                string consulta = string.Format(@"SELECT p.fechapoL, p.poliza, p.tp, p.generada, p.status, p.usuario_movto, p.fec_hora_movto, p.usuario_crea, p.fecha_hora_crea,
                                                m.linea, m.cta, m.scta, m.sscta, m.digito,  m.cc, m.referencia, m.orden_compra, m.concepto, m.forma_pago, m.tm, m.itm, m.monto, p.error, m.area, m.cuenta_oc,
                                                (SELECT descripcion FROM catcta c WHERE c.cta = m.cta AND c.scta = m.scta AND c.sscta = m.sscta)
                                                FROM sc_polizas p   
                                                INNER JOIN sc_movpol m 
                                                WHERE p.status = '{0}'
                                                    AND m.poliza >= {1}
                                                    AND m.poliza <= {2}
                                                    AND year(p.fechapol) >= {3}
                                                    AND year(p.fechapol) <= {4}
                                                    AND month(p.fechapol) >= {5}
                                                    AND month(p.fechapol) <= {6}
                                                    AND m.tp >= '{7}'
                                                    AND m.tp <= '{8}'
                                                    AND m.cc >= '{9}'
                                                    AND m.cc <= '{10}'", Estatus,
                                                                         iPol,
                                                                         fPol,
                                                                         iPer.Substring(3, 4),
                                                                         fPer.Substring(3, 4),
                                                                         int.Parse(iPer.Substring(0, 2)),
                                                                         int.Parse(fPer.Substring(0, 2)),
                                                                         iTp,
                                                                         fTp,
                                                                         icc,
                                                                         fcc);
                //var lstEkP = (List<RepMovPolizaDTO>)_contextEnkontrol.WherePruebaArrendadora(consulta).ToObject<List<RepMovPolizaDTO>>();
                var lstEkP = _contextEnkontrol.Select<RepMovPolizaDTO>(EnkontrolEnum.PruebaCplanProd, consulta);
                var lst2 = lstEkP.Select(x => new RepMovPoliza2DTO()
                {
                    cargo = (x.tm == 1) || (x.tm == 3) ? x.monto : 0,
                    abono = (x.tm == 2) || (x.tm == 4) ? x.monto : 0,
                    usuario_movto = x.usuario_movto == null ? "" : Convert.ToString(x.usuario_movto.Value),
                    fec_hora_movto = x.fec_hora_movto == null ? "" : Convert.ToDateTime(x.fec_hora_movto.Value).ToShortDateString(),
                    forma_pago = x.forma_pago == null ? String.Empty : x.forma_pago,
                    error = string.IsNullOrEmpty(x.error) ? string.Empty : x.error,
                    status = setEstatus(x.status),
                    generada = setGenerada(x.generada),
                    cc = x.cc,
                    concepto = x.concepto,
                    cta = x.cta,
                    descripcion = x.descripcion,
                    digito = x.digito,
                    fecha_hora_crea = x.fecha_hora_crea,
                    fechapol = x.fechapol,
                    itm = x.itm,
                    linea = x.linea,
                    monto = x.monto,
                    orden_compra = x.orden_compra,
                    poliza = x.poliza,
                    referencia = x.referencia,
                    scta = x.scta,
                    sscta = x.sscta,
                    tm = x.tm,
                    tp = x.tp,
                    usuario_crea = x.usuario_crea.ToString()
                });
                return lst2.ToList();
            }
            catch (Exception ex) { return new List<RepMovPoliza2DTO>(); }
        }

        public List<MovpolDTO> getMovPolizaEk(DateTime fecha, int poliza, string tp)
        {
            try
            {
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = "SELECT linea, cta, scta, sscta, digito, itm, numpro, referencia, cc, orden_compra, concepto, tm, monto, area, cuenta_oc FROM sc_movpol WHERE year = ? AND mes = ?  AND poliza = ?  AND tp = ?"
                };
                odbc.parametros.Add(new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Numeric, valor = fecha.Year });
                odbc.parametros.Add(new OdbcParameterDTO() { nombre = "mes", tipo = OdbcType.Numeric, valor = fecha.Month });
                odbc.parametros.Add(new OdbcParameterDTO() { nombre = "poliza", tipo = OdbcType.Numeric, valor = poliza });
                odbc.parametros.Add(new OdbcParameterDTO() { nombre = "tp", tipo = OdbcType.NVarChar, valor = tp });
                var lstEkP = _contextEnkontrol.Select<MovpolDTO>(EnkontrolAmbienteEnum.Prod, odbc);
                foreach (var x in lstEkP)
                {
                    x.numpro = x.numpro == null ? 0 : x.numpro;
                    x.orden_compra = x.orden_compra == null ? string.Empty : x.orden_compra;
                }
                return lstEkP.ToList();
            }
            catch (Exception)
            {
                var lst = new List<MovpolDTO>();
                lst.Add(new MovpolDTO()
                {
                    linea = 1,
                    poliza = 0,
                    cta = 0,
                    scta = 0,
                    sscta = 0,
                    digito = 0,
                    numpro = 0,
                    referencia = string.Empty,
                    orden_compra = string.Empty,
                    concepto = string.Empty,
                    tm = 0,
                    monto = 0,
                    cc = string.Empty,
                    iclave = 0,
                    forma_pago = string.Empty,
                    mes = fecha.Day,
                    year = fecha.Year,
                    tp = string.Empty
                });
                return lst;
            }
        }
        public Task<List<MovpolDTO>> agetMovPolizaEk(DateTime fecha, int poliza, string tp)
        {
            var empresa = vSesiones.sesionEmpresaActual;
            return Task.Factory.StartNew(() =>
            {
                var objDault = new MovpolDTO() { linea = 1, poliza = 0, cta = 0, scta = 0, sscta = 0, digito = 0, numpro = 0, referencia = string.Empty, orden_compra = string.Empty, concepto = string.Empty, tm = 0, monto = 0, cc = string.Empty, iclave = 0, forma_pago = string.Empty, mes = fecha.Day, year = fecha.Year, tp = string.Empty, itm = 0 };
                try
                {
                    if (poliza == 0)
                    {
                        throw new Exception();
                    }
                    var odbc = new OdbcConsultaDTO()
                    {
                        consulta = "SELECT linea, cta, scta, sscta, digito, itm, numpro, referencia, cc, orden_compra, concepto, tm, monto, area, cuenta_oc FROM sc_movpol WHERE year = ?  AND mes = ?  AND poliza = ?  AND tp = ?"
                    };
                    odbc.parametros.Add(new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Numeric, valor = fecha.Year });
                    odbc.parametros.Add(new OdbcParameterDTO() { nombre = "mes", tipo = OdbcType.Numeric, valor = fecha.Month });
                    odbc.parametros.Add(new OdbcParameterDTO() { nombre = "poliza", tipo = OdbcType.Numeric, valor = poliza });
                    odbc.parametros.Add(new OdbcParameterDTO() { nombre = "tp", tipo = OdbcType.NVarChar, valor = tp });
                    var lstEkP = new List<MovpolDTO>();
                    switch (empresa)
                    {
                        case (int)EmpresaEnum.Construplan: lstEkP = _contextEnkontrol.Select<MovpolDTO>(EnkontrolEnum.CplanProd, odbc); break;
                        case (int)EmpresaEnum.Arrendadora: lstEkP = _contextEnkontrol.Select<MovpolDTO>(EnkontrolEnum.ArrenProd, odbc); break;
                        case (int)EmpresaEnum.Colombia: lstEkP = _contextEnkontrol.Select<MovpolDTO>(EnkontrolEnum.CplanProd, odbc); break;
                        default:
                            break;
                    }
                    if (lstEkP.Count == 0)
                    {
                        lstEkP.Add(objDault);
                    }
                    else
                    {
                        lstEkP.ForEach(x =>
                        {
                            x.numpro = x.numpro == null ? 0 : x.numpro;
                            x.orden_compra = x.orden_compra == null ? string.Empty : x.orden_compra;
                        });
                    }
                    return lstEkP.OrderBy(o => o.linea).ToList();
                }
                catch (Exception)
                {
                    var lst = new List<MovpolDTO>();
                    lst.Add(objDault);
                    return lst;
                }
            });
        }
        #endregion
        #region Reporte Empresa
        public List<RepPolizaDTO> getPolizaPorEmpresa(EmpresaEnum empresa, int iPol, int fPol, string iPer, string fPer, string iTp, string fTp)
        {
            try
            {
                string consulta = string.Format(@"SELECT poliza, tp, fechapol, cargos, abonos,(cargos + abonos) AS diferencia,  status  FROM ""DBA"".""sc_polizas""
                                                    WHERE poliza >= {0}
                                                        AND poliza <= {1}
                                                        AND year(fechapol) >= {2}
                                                        AND year(fechapol) <= {3}
                                                        AND month(fechapol) >= {4}
                                                        AND month(fechapol) <= {5}
                                                        AND tp >= '{6}'
                                                        AND tp <= '{7}'"
                                                        , iPol
                                                        , fPol
                                                        , iPer.Substring(3, 4)
                                                        , fPer.Substring(3, 4)
                                                        , int.Parse(iPer.Substring(0, 2))
                                                        , int.Parse(fPer.Substring(0, 2))
                                                        , iTp
                                                        , fTp);
                var conector = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(empresa);
                //var conector = empresa == EmpresaEnum.Construplan ? EnkontrolEnum.PruebaCplanProd : _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(empresa);
                var lstEkP = _contextEnkontrol.Select<RepPolizaDTO>(conector, consulta);
                return lstEkP.ToList();
            }
            catch (Exception) { return new List<RepPolizaDTO>(); }
        }
        public List<RepMovPoliza2DTO> getMovPolizaPorEmpresa(EmpresaEnum empresa, int iPol, int fPol, string iPer, string fPer, string iTp, string fTp, string icc, string fcc)
        {
            try
            {
                string consulta = string.Format(@"SELECT p.fechapol, p.poliza, p.tp, p.generada, p.status, p.usuario_movto, p.fec_hora_movto, p.usuario_crea, p.fecha_hora_crea,
                                                m.linea, m.cta, m.scta, m.sscta, m.digito,  m.cc, m.referencia, m.orden_compra, m.concepto, m.forma_pago, m.tm, m.itm, m.monto, p.error, m.area, m.cuenta_oc,
                                                (SELECT descripcion FROM ""DBA"".""catcta"" c WHERE c.cta = m.cta AND c.scta = m.scta AND c.sscta = m.sscta)
                                                FROM ""DBA"".""sc_polizas"" p
                                                INNER JOIN ""DBA"".""sc_movpol"" m
                                                WHERE m.poliza >= {0}
                                                    AND m.poliza <= {1}
                                                    AND year(p.fechapol) >= {2}
                                                    AND year(p.fechapol) <= {3}
                                                    AND month(p.fechapol) >= {4}
                                                    AND month(p.fechapol) <= {5}
                                                    AND m.tp >= '{6}'
                                                    AND m.tp <= '{7}'
                                                    AND m.cc >= '{8}'
                                                    AND m.cc <= '{9}'"
                                                    , iPol
                                                    , fPol
                                                    , iPer.Substring(3, 4)
                                                    , fPer.Substring(3, 4)
                                                    , int.Parse(iPer.Substring(0, 2))
                                                    , int.Parse(fPer.Substring(0, 2))
                                                    , iTp
                                                    , fTp
                                                    , icc
                                                    , fcc);
                var conector = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(empresa);
                //var conector = empresa == EmpresaEnum.Construplan ? EnkontrolEnum.PruebaCplanProd : _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(empresa);
                var lstEkP = _contextEnkontrol.Select<RepMovPolizaDTO>(conector, consulta);
                var lst2 = lstEkP.Select(x => new RepMovPoliza2DTO()
                {
                    cargo = (x.tm == 1) || (x.tm == 3) ? x.monto : 0,
                    abono = (x.tm == 2) || (x.tm == 4) ? x.monto : 0,
                    usuario_movto = x.usuario_movto == null ? "" : Convert.ToString(x.usuario_movto.Value),
                    fec_hora_movto = x.fec_hora_movto == null ? "" : Convert.ToDateTime(x.fec_hora_movto.Value).ToShortDateString(),
                    forma_pago = x.forma_pago == null ? String.Empty : x.forma_pago,
                    error = string.IsNullOrEmpty(x.error) ? string.Empty : x.error,
                    status = setEstatus(x.status),
                    generada = setGenerada(x.generada),
                    cc = x.cc,
                    concepto = x.concepto,
                    cta = x.cta,
                    descripcion = x.descripcion,
                    digito = x.digito,
                    fecha_hora_crea = x.fecha_hora_crea,
                    fechapol = x.fechapol,
                    itm = x.itm,
                    linea = x.linea,
                    monto = x.monto,
                    orden_compra = x.orden_compra,
                    poliza = x.poliza,
                    referencia = x.referencia,
                    scta = x.scta,
                    sscta = x.sscta,
                    tm = x.tm,
                    tp = x.tp,
                    usuario_crea = x.usuario_crea.ToString()
                });
                return lst2.ToList();
            }
            catch (Exception ex) { return new List<RepMovPoliza2DTO>(); }
        }
        #endregion
        #region CtaCadena
        public List<MovpolDTO> getCtaCadena(List<MovpolDTO> lstCadena, bool isIntereses, DateTime dllFecha)
        {
            try
            {
                var lstCta = new List<MovpolDTO>();
                var i = 0;
                var esProvDollar = lstCadena.FirstOrDefault(c => c.numpro > 0).numpro > 8999;
                if (isIntereses)
                {
                    var banc = new MovpolDTO()
                    {
                        cta = 1110,
                        scta = lstCadena.FirstOrDefault().scta == 6694788 ? 1 :      //Banamex 1110	1	1
                               lstCadena.FirstOrDefault().scta == 0157323241 ? 5 :   //Banorte 1110	5	1
                               lstCadena.FirstOrDefault().scta == 006621422 ? 8 :    //Scotianbank 1110	8	1
                               lstCadena.FirstOrDefault().scta == 9176028 ? 50 :     //BanamexDll 1110	50	2
                               lstCadena.FirstOrDefault().scta == 0155776535 ? 51 : 0,  //BanorteDll 1110	51	1
                        sscta = lstCadena.FirstOrDefault().scta == 6694788 ? 1 :      //Banamex
                                lstCadena.FirstOrDefault().scta == 0157323241 ? 1 :   //Banorte
                                lstCadena.FirstOrDefault().scta == 006621422 ? 1 :    //Scotianbank
                                lstCadena.FirstOrDefault().scta == 9176028 ? 2 :     //BanamexDll
                                lstCadena.FirstOrDefault().scta == 0155776535 ? 1 : 0,  //BanorteDll,
                    };
                    var lstReff = new List<MovpolDTO>();
                    System.Web.HttpContext.Current.Session["lstPago"] = null;
                    var lstSession = new List<tblC_CadenaProductiva>();
                    lstCadena.GroupBy(g => new { g.cc, g.numpro, g.sscta }).ToList().ForEach(c =>
                    {
                        var cadena = c.FirstOrDefault();
                        var sum = c.Sum(x => x.monto);
                        var cc = c.Key.cc;
                        var area = cadena.area;
                        var cuenta = cadena.cuenta_oc;
                        var banco = cadena.cta;
                        var prov = c.Key.numpro.ToString();
                        var lstThisReff = getLstReferenciaAbonoFactoraje(c.Select(x => x.referencia).ToList(), prov, banco);
                        lstThisReff.GroupBy(g => g.factura).ToList().ForEach(dir =>
                        {
                            lstReff.Add(new MovpolDTO()
                            {
                                cc = cc,
                                area = area,
                                cuenta_oc = cuenta,
                                monto = sum,
                                numpro = c.LastOrDefault().numpro,
                                referencia = dir.Key,
                                tp = dir.Sum(s => s.total).ToString()
                            });
                            c.ToList().ForEach(p =>
                            {
                                lstSession.Add(new tblC_CadenaProductiva
                                {
                                    id = p.digito,
                                    idPrincipal = c.Key.sscta,
                                    numProveedor = prov,
                                    factura = p.referencia,
                                    numNafin = dir.Key
                                });
                            });
                        });
                    });
                    System.Web.HttpContext.Current.Session["lstPago"] = lstSession;
                    var bancCompl = getCtacompl(banc.cta, banc.scta, banc.sscta);

                    foreach (var item in lstCadena.GroupBy(g => new { g.cc, g.numpro, g.sscta }))
                    {
                        var sum = item.Sum(s => s.monto);
                        var reff = lstReff.FirstOrDefault(w => w.cc.Equals(item.LastOrDefault().cc) && sum == w.monto);

                        var _movpol = new MovpolDTO();
                        _movpol.linea = ++i;
                        _movpol.cta = 2105;
                        _movpol.scta = esProvDollar ? 2 : 1;
                        _movpol.sscta = 0;
                        _movpol.digito = 0;
                        _movpol.tm = 1;
                        _movpol.numpro = item.LastOrDefault().cta;
                        _movpol.referencia = reff == null ? "0" : reff.referencia;
                        _movpol.cc = item.LastOrDefault().cc;
                        _movpol.area = item.LastOrDefault().area;
                        _movpol.cuenta_oc = item.LastOrDefault().cuenta_oc;
                        _movpol.orden_compra = item.LastOrDefault().orden_compra;
                        _movpol.concepto = item.LastOrDefault().concepto;
                        _movpol.itm = 51;
                        _movpol.monto = sum;

                        lstCta.Add(_movpol);

                        if (esProvDollar)
                        {
                            var _movpolDll = new MovpolDTO();
                            _movpolDll.linea = ++i;
                            _movpolDll.cta = 2105;
                            _movpolDll.scta = 200;
                            _movpolDll.sscta = 0;
                            _movpolDll.digito = 0;
                            _movpolDll.tm = 1;
                            _movpolDll.numpro = item.LastOrDefault().cta;
                            _movpolDll.referencia = reff == null ? "0" : reff.referencia;
                            _movpolDll.cc = item.LastOrDefault().cc;
                            _movpolDll.area = item.LastOrDefault().area;
                            _movpolDll.cuenta_oc = item.LastOrDefault().cuenta_oc;
                            _movpolDll.orden_compra = item.LastOrDefault().orden_compra;
                            _movpolDll.concepto = item.LastOrDefault().concepto;
                            _movpolDll.itm = 51;
                            _movpolDll.monto = reff == null ? item.LastOrDefault().tp.ParseDecimal() : reff.tp.ParseDecimal();

                            lstCta.Add(_movpolDll);
                        }
                    }
                    lstCadena.GroupBy(g => new { g.cc, g.numpro, g.sscta }).ToList().ForEach(c =>
                    {
                        var sum = c.Sum(s => s.monto);
                        var reff = lstReff.FirstOrDefault(w => w.cc.Equals(c.LastOrDefault().cc) && sum == w.monto);
                        lstCta.Add(new MovpolDTO()
                        {
                            linea = ++i,
                            cta = banc.cta,
                            scta = banc.scta,
                            sscta = banc.sscta,
                            digito = 0,
                            tm = 2,
                            numpro = 0,
                            referencia = reff == null ? "0" : reff.referencia,
                            cc = c.LastOrDefault().cc,
                            area = c.LastOrDefault().area,
                            cuenta_oc = c.LastOrDefault().cuenta_oc,
                            orden_compra = c.LastOrDefault().orden_compra,
                            concepto = c.LastOrDefault().concepto,
                            itm = 74,
                            monto = -c.Sum(s => s.monto)
                        });
                        if (esProvDollar)
                            lstCta.Add(new MovpolDTO()
                            {
                                linea = ++i,
                                cta = bancCompl.ctacom,
                                scta = bancCompl.sctacom,
                                sscta = bancCompl.ssctacom,
                                digito = bancCompl.digitocom,
                                tm = 2,
                                numpro = 0,
                                referencia = reff == null ? "0" : reff.referencia,
                                cc = c.LastOrDefault().cc,
                                area = c.LastOrDefault().area,
                                cuenta_oc = c.LastOrDefault().cuenta_oc,
                                orden_compra = c.LastOrDefault().orden_compra,
                                concepto = c.LastOrDefault().concepto,
                                itm = 74,
                                monto = reff == null ? c.LastOrDefault().tp.ParseDecimal() : reff.tp.ParseDecimal()
                            });
                    });
                }
                else
                {
                    var dllCompl = cadenaFS.getCadenaProductivaService().getDolarDelDia(dllFecha);
                    lstCadena.ForEach(c =>
                    {
                        lstCta.Add(new MovpolDTO()
                        {
                            linea = ++i,
                            cta = 2105,
                            scta = esProvDollar ? 2 : 1,
                            sscta = 0,
                            digito = 0,
                            tm = 1,
                            numpro = c.numpro,
                            referencia = c.referencia,
                            cc = c.cc,
                            area = c.area,
                            cuenta_oc = c.cuenta_oc,
                            orden_compra = c.orden_compra,
                            concepto = c.concepto,
                            itm = 51,
                            monto = c.monto
                        });
                        if (esProvDollar)
                            lstCta.Add(new MovpolDTO()
                            {
                                linea = ++i,
                                cta = 2105,
                                scta = 200,
                                sscta = 0,
                                digito = 0,
                                tm = 1,
                                numpro = c.numpro,
                                referencia = c.referencia,
                                cc = c.cc,
                                area = c.area,
                                cuenta_oc = c.cuenta_oc,
                                orden_compra = c.orden_compra,
                                concepto = c.concepto,
                                itm = 51,
                                monto = (c.monto * cadenaFS.getCadenaProductivaService().GetTipoCambioRegistro(c.referencia, c.numpro.ToString())) - c.monto
                            });
                    });
                    var r = getLastReferenciaFactoraje(lstCadena.FirstOrDefault().cta);
                    var ivaReff = string.Empty;
                    lstCadena.GroupBy(g => g.cc).ToList().ForEach(c =>
                    {

                        lstCta.Add(new MovpolDTO()
                        {
                            linea = ++i,
                            cta = 2105,
                            scta = esProvDollar ? 2 : 1,
                            sscta = 0,
                            digito = 0,
                            tm = 2,
                            numpro = c.LastOrDefault().cta,
                            referencia = (++r).ToString(),
                            cc = c.Key,
                            area = c.LastOrDefault().area,
                            cuenta_oc = c.LastOrDefault().cuenta_oc,
                            orden_compra = c.LastOrDefault().orden_compra,
                            concepto = c.LastOrDefault().concepto,
                            itm = 15,
                            monto = -c.Sum(s => s.monto)
                        });
                        if (esProvDollar)
                            lstCta.Add(new MovpolDTO()
                            {
                                linea = ++i,
                                cta = 2105,
                                scta = 200,
                                sscta = 0,
                                digito = 0,
                                tm = 2,
                                numpro = c.LastOrDefault().cta,
                                referencia = r.ToString(),
                                cc = c.Key,
                                area = c.LastOrDefault().area,
                                cuenta_oc = c.LastOrDefault().cuenta_oc,
                                orden_compra = c.LastOrDefault().orden_compra,
                                concepto = c.LastOrDefault().concepto,
                                itm = 15,
                                monto = -c.Sum(s => (s.monto * dllCompl) - s.monto
                                )
                            });
                        ivaReff = r.ToString();
                    });
                    if (esProvDollar)
                    {
                        var dllCta = lstCadena.Sum(s => (s.monto * cadenaFS.getCadenaProductivaService().GetTipoCambioRegistro(s.referencia, s.numpro.ToString())) - s.monto);
                        var sumMonto = lstCadena.Sum(s => (s.monto * dllCompl) - s.monto);
                        var diffCam = dllCta - sumMonto;
                        var ddlCam = diffCam / dllCompl;
                        var ctaDif = getCtaDif(lstCta.FirstOrDefault().cta, lstCta.FirstOrDefault().scta, lstCta.FirstOrDefault().sscta);
                        if (diffCam != 0)
                        {
                            lstCta.Add(new MovpolDTO()
                            {
                                linea = ++i,
                                cta = ddlCam > 0 ? ctaDif.cta_dif_cambiaria : ctaDif.cta_perdida - 1,
                                scta = ddlCam > 0 ? ctaDif.scta_dif_cambiaria : ctaDif.scta_perdida,
                                sscta = ddlCam > 0 ? ctaDif.sscta_dif_cambiaria : ctaDif.sscta_perdida,
                                digito = ddlCam > 0 ? ctaDif.digito_dif_cambiaria : ctaDif.digito_perdida,
                                tm = diffCam > 0 ? 2 : 1,
                                numpro = 0,
                                referencia = ivaReff,
                                cc = lstCadena.LastOrDefault().cc,
                                area = lstCadena.LastOrDefault().area,
                                cuenta_oc = lstCadena.LastOrDefault().cuenta_oc,
                                orden_compra = lstCadena.LastOrDefault().orden_compra,
                                concepto = lstCadena.LastOrDefault().concepto,
                                itm = ddlCam < 0 ? 51 : 15,
                                monto = diffCam
                            });
                        }
                    }
                    var cargoIva = (lstCta.Where(w => w.scta != 2 && (w.tm == 1 || w.tm == 3)).Sum(s => s.monto) / 1.16m) * 0.16m;
                    var abonoIva = (lstCta.Where(w => w.scta == 2 && (w.tm == 2 || w.tm == 4)).Sum(s => s.monto) / 1.16m) * 0.16m;
                    lstCta.Add(new MovpolDTO()
                    {
                        linea = ++i,
                        cta = 1147,
                        scta = 4,
                        sscta = 0,
                        digito = 0,
                        tm = 1,
                        numpro = 0,
                        referencia = ivaReff,
                        cc = lstCadena.LastOrDefault().cc,
                        area = lstCadena.LastOrDefault().area,
                        cuenta_oc = lstCadena.LastOrDefault().cuenta_oc,
                        orden_compra = lstCadena.LastOrDefault().orden_compra,
                        concepto = lstCadena.LastOrDefault().concepto,
                        itm = 0,
                        monto = cargoIva
                    });
                    lstCta.Add(new MovpolDTO()
                    {
                        linea = ++i,
                        cta = 1146,
                        scta = 4,
                        sscta = 0,
                        digito = 0,
                        tm = 2,
                        numpro = 0,
                        referencia = ivaReff,
                        cc = lstCadena.LastOrDefault().cc,
                        area = lstCadena.LastOrDefault().area,
                        cuenta_oc = lstCadena.LastOrDefault().cuenta_oc,
                        orden_compra = lstCadena.LastOrDefault().orden_compra,
                        concepto = lstCadena.LastOrDefault().concepto,
                        itm = 0,
                        monto = abonoIva
                    });
                }
                return lstCta.ToList();
            }
            catch (Exception o_O) { return new List<MovpolDTO>(); }
        }

        public Dictionary<string, object> GetCtaCadenaNuevo(List<MovpolDTO> lstCadena, bool isIntereses, DateTime dllFecha)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            try
            {
                var lstCta = new List<MovpolDTO>();
                var i = 0;
                var esProvDollar = lstCadena.FirstOrDefault(c => c.numpro > 0).numpro > 8999;

                if (isIntereses)
                {
                    #region Intereses
                    var banc = new MovpolDTO()
                    {
                        cta = 1110,
                        scta = lstCadena.FirstOrDefault().scta == 6694788 ? 1 :      //Banamex 1110	1	1
                               lstCadena.FirstOrDefault().scta == 0157323241 ? 5 :   //Banorte 1110	5	1
                               lstCadena.FirstOrDefault().scta == 006621422 ? 8 :    //Scotianbank 1110	8	1
                               lstCadena.FirstOrDefault().scta == 9176028 ? 50 :     //BanamexDll 1110	50	2
                               lstCadena.FirstOrDefault().scta == 0155776535 ? 51 : 0,  //BanorteDll 1110	51	1
                        sscta = lstCadena.FirstOrDefault().scta == 6694788 ? 1 :      //Banamex
                                lstCadena.FirstOrDefault().scta == 0157323241 ? 1 :   //Banorte
                                lstCadena.FirstOrDefault().scta == 006621422 ? 1 :    //Scotianbank
                                lstCadena.FirstOrDefault().scta == 9176028 ? 2 :     //BanamexDll
                                lstCadena.FirstOrDefault().scta == 0155776535 ? 1 : 0,  //BanorteDll,
                    };
                    var lstReff = new List<MovpolDTO>();
                    System.Web.HttpContext.Current.Session["lstPago"] = null;
                    var lstSession = new List<tblC_CadenaProductiva>();
                    lstCadena.GroupBy(g => new { g.cc, g.numpro, g.sscta }).ToList().ForEach(c =>
                    {
                        var cadena = c.FirstOrDefault();
                        var sum = c.Sum(x => x.monto);
                        var cc = c.Key.cc;
                        var area = cadena.area;
                        var cuenta = cadena.cuenta_oc;
                        var banco = cadena.cta;
                        var prov = c.Key.numpro.ToString();
                        var lstThisReff = getLstReferenciaAbonoFactoraje(c.Select(x => x.referencia).ToList(), prov, banco);
                        lstThisReff.GroupBy(g => g.factura).ToList().ForEach(dir =>
                        {
                            lstReff.Add(new MovpolDTO()
                            {
                                cc = cc,
                                area = area,
                                cuenta_oc = cuenta,
                                monto = sum,
                                numpro = c.LastOrDefault().numpro,
                                referencia = dir.Key,
                                tp = dir.Sum(s => s.total).ToString()
                            });
                            c.ToList().ForEach(p =>
                            {
                                lstSession.Add(new tblC_CadenaProductiva
                                {
                                    id = p.digito,
                                    idPrincipal = c.Key.sscta,
                                    numProveedor = prov,
                                    factura = p.referencia,
                                    numNafin = dir.Key
                                });
                            });
                        });
                    });
                    System.Web.HttpContext.Current.Session["lstPago"] = lstSession;
                    var bancCompl = getCtacompl(banc.cta, banc.scta, banc.sscta);

                    var reffCuenta = lstReff.Select(x => x).ToList();

                    foreach (var item in lstCadena.GroupBy(g => new { g.cc, g.numpro, g.sscta }))
                    {
                        var sum = item.Sum(x => x.monto);
                        var reff = reffCuenta.FirstOrDefault(w => w.cc.Equals(item.LastOrDefault().cc) && sum == w.monto);
                        reffCuenta.Remove(reff);
                        //var reff = lstReff.FirstOrDefault(w => w.referencia == item.Key.referencia);

                        var _movpol = new MovpolDTO();
                        _movpol.linea = ++i;
                        _movpol.cta = 2105;
                        _movpol.scta = esProvDollar ? 2 : 1;
                        _movpol.sscta = 0;
                        _movpol.digito = 0;
                        _movpol.tm = 1;
                        _movpol.numpro = item.LastOrDefault().cta;
                        _movpol.referencia = reff == null ? "0" : reff.referencia;
                        _movpol.cc = item.LastOrDefault().cc;
                        _movpol.area = item.LastOrDefault().area;
                        _movpol.cuenta_oc = item.LastOrDefault().cuenta_oc;
                        _movpol.orden_compra = item.LastOrDefault().orden_compra;
                        _movpol.concepto = item.LastOrDefault().concepto;
                        _movpol.itm = 51;
                        _movpol.monto = sum;

                        lstCta.Add(_movpol);

                        if (esProvDollar)
                        {
                            var _movpolDll = new MovpolDTO();
                            _movpolDll.linea = ++i;
                            _movpolDll.cta = 2105;
                            _movpolDll.scta = 200;
                            _movpolDll.sscta = 0;
                            _movpolDll.digito = 0;
                            _movpolDll.tm = 1;
                            _movpolDll.numpro = item.LastOrDefault().cta;
                            _movpolDll.referencia = reff == null ? "0" : reff.referencia;
                            _movpolDll.cc = item.LastOrDefault().cc;
                            _movpolDll.area = item.LastOrDefault().area;
                            _movpolDll.cuenta_oc = item.LastOrDefault().cuenta_oc;
                            _movpolDll.orden_compra = item.LastOrDefault().orden_compra;
                            _movpolDll.concepto = item.LastOrDefault().concepto;
                            _movpolDll.itm = 51;
                            _movpolDll.monto = reff.tp.ParseDecimal();

                            lstCta.Add(_movpolDll);
                        }
                    }
                    reffCuenta = lstReff.Select(x => x).ToList();
                    lstCadena.GroupBy(g => new { g.cc, g.numpro, g.sscta }).ToList().ForEach(c =>
                    {
                        var sum = c.Sum(x => x.monto);
                        var reff = reffCuenta.FirstOrDefault(w => w.cc.Equals(c.LastOrDefault().cc) && sum == w.monto);
                        reffCuenta.Remove(reff);
                        //var reff = lstReff.FirstOrDefault(w => w.referencia == c.Key.referencia);
                        lstCta.Add(new MovpolDTO()
                        {
                            linea = ++i,
                            cta = banc.cta,
                            scta = banc.scta,
                            sscta = banc.sscta,
                            digito = 0,
                            tm = 2,
                            numpro = 0,
                            referencia = reff == null ? "0" : reff.referencia,
                            cc = c.LastOrDefault().cc,
                            area = c.LastOrDefault().area,
                            cuenta_oc = c.LastOrDefault().cuenta_oc,
                            orden_compra = c.LastOrDefault().orden_compra,
                            concepto = c.LastOrDefault().concepto,
                            itm = 74,
                            monto = -c.Sum(s => s.monto)
                        });
                        if (esProvDollar)
                            lstCta.Add(new MovpolDTO()
                            {
                                linea = ++i,
                                cta = bancCompl.ctacom,
                                scta = bancCompl.sctacom,
                                sscta = bancCompl.ssctacom,
                                digito = bancCompl.digitocom,
                                tm = 2,
                                numpro = 0,
                                referencia = reff.referencia,
                                cc = c.LastOrDefault().cc,
                                area = c.LastOrDefault().area,
                                cuenta_oc = c.LastOrDefault().cuenta_oc,
                                orden_compra = c.LastOrDefault().orden_compra,
                                concepto = c.LastOrDefault().concepto,
                                itm = 74,
                                monto = reff.tp.ParseDecimal()
                            });
                    });
                    #endregion
                }
                else
                {
                    #region No Intereses
                    var dllCompl = cadenaFS.getCadenaProductivaService().getDolarDelDia(dllFecha);
                    lstCadena.ForEach(c =>
                    {
                        lstCta.Add(new MovpolDTO()
                        {
                            linea = ++i,
                            cta = 2105,
                            scta = esProvDollar ? 2 : 1,
                            sscta = 0,
                            digito = 0,
                            tm = 1,
                            numpro = c.numpro,
                            referencia = c.referencia,
                            cc = c.cc,
                            area = c.area,
                            cuenta_oc = c.cuenta_oc,
                            orden_compra = c.orden_compra,
                            concepto = c.concepto,
                            itm = 51,
                            monto = c.monto
                        });
                        if (esProvDollar)
                        {
                            lstCta.Add(new MovpolDTO()
                            {
                                linea = ++i,
                                cta = 2105,
                                scta = 200,
                                sscta = 0,
                                digito = 0,
                                tm = 1,
                                numpro = c.numpro,
                                referencia = c.referencia,
                                cc = c.cc,
                                area = c.area,
                                cuenta_oc = c.cuenta_oc,
                                orden_compra = c.orden_compra,
                                concepto = c.concepto,
                                itm = 51,
                                monto = (c.monto * cadenaFS.getCadenaProductivaService().GetTipoCambioRegistro(c.referencia, c.numpro.ToString())) - c.monto
                            });
                        }
                    });
                    var r = getLastReferenciaFactoraje(lstCadena.FirstOrDefault().cta);
                    var ivaReff = string.Empty;
                    lstCadena.GroupBy(g => new { g.cc }).ToList().ForEach(c =>
                    {
                        lstCta.Add(new MovpolDTO()
                        {
                            linea = ++i,
                            cta = 2105,
                            scta = esProvDollar ? 2 : 1,
                            sscta = 0,
                            digito = 0,
                            tm = 2,
                            numpro = c.LastOrDefault().cta,
                            referencia = (++r).ToString(),
                            cc = c.Key.cc,
                            area = c.LastOrDefault().area,
                            cuenta_oc = c.LastOrDefault().cuenta_oc,
                            orden_compra = c.LastOrDefault().orden_compra,
                            concepto = c.LastOrDefault().concepto,
                            itm = 15,
                            monto = -c.Sum(s => s.monto)
                        });
                        if (esProvDollar)
                        {
                            lstCta.Add(new MovpolDTO()
                            {
                                linea = ++i,
                                cta = 2105,
                                scta = 200,
                                sscta = 0,
                                digito = 0,
                                tm = 2,
                                numpro = c.LastOrDefault().cta,
                                referencia = r.ToString(),
                                cc = c.Key.cc,
                                area = c.LastOrDefault().area,
                                cuenta_oc = c.LastOrDefault().cuenta_oc,
                                orden_compra = c.LastOrDefault().orden_compra,
                                concepto = c.LastOrDefault().concepto,
                                itm = 15,
                                monto = -c.Sum(s => (s.monto * dllCompl) - s.monto
                                )
                            });
                        }
                        ivaReff = r.ToString();
                    });
                    if (esProvDollar)
                    {
                        var dllCta = lstCadena.Sum(s => (s.monto * cadenaFS.getCadenaProductivaService().GetTipoCambioRegistro(s.referencia, s.numpro.ToString())) - s.monto);
                        var sumMonto = lstCadena.Sum(s => (s.monto * dllCompl) - s.monto);
                        var diffCam = dllCta - sumMonto;
                        var ddlCam = diffCam / dllCompl;
                        var ctaDif = getCtaDif(lstCta.FirstOrDefault().cta, lstCta.FirstOrDefault().scta, lstCta.FirstOrDefault().sscta);
                        if (diffCam != 0)
                        {
                            lstCta.Add(new MovpolDTO()
                            {
                                linea = ++i,
                                cta = ddlCam > 0 ? ctaDif.cta_dif_cambiaria : ctaDif.cta_perdida - 1,
                                scta = ddlCam > 0 ? ctaDif.scta_dif_cambiaria : ctaDif.scta_perdida,
                                sscta = ddlCam > 0 ? ctaDif.sscta_dif_cambiaria : ctaDif.sscta_perdida,
                                digito = ddlCam > 0 ? ctaDif.digito_dif_cambiaria : ctaDif.digito_perdida,
                                tm = diffCam > 0 ? 2 : 1,
                                numpro = 0,
                                referencia = ivaReff,
                                cc = lstCadena.LastOrDefault().cc,
                                area = lstCadena.LastOrDefault().area,
                                cuenta_oc = lstCadena.LastOrDefault().cuenta_oc,
                                orden_compra = lstCadena.LastOrDefault().orden_compra,
                                concepto = lstCadena.LastOrDefault().concepto,
                                itm = ddlCam < 0 ? 51 : 15,
                                monto = diffCam
                            });
                        }
                    }
                    var cargoIva = (lstCta.Where(w => w.scta != 2 && (w.tm == 1 || w.tm == 3)).Sum(s => s.monto) / 1.16m) * 0.16m;
                    var abonoIva = (lstCta.Where(w => w.scta == 2 && (w.tm == 2 || w.tm == 4)).Sum(s => s.monto) / 1.16m) * 0.16m;
                    lstCta.Add(new MovpolDTO()
                    {
                        linea = ++i,
                        cta = 1147,
                        scta = 4,
                        sscta = 0,
                        digito = 0,
                        tm = 1,
                        numpro = 0,
                        referencia = ivaReff,
                        cc = lstCadena.LastOrDefault().cc,
                        area = lstCadena.LastOrDefault().area,
                        cuenta_oc = lstCadena.LastOrDefault().cuenta_oc,
                        orden_compra = lstCadena.LastOrDefault().orden_compra,
                        concepto = lstCadena.LastOrDefault().concepto,
                        itm = 0,
                        monto = cargoIva
                    });
                    lstCta.Add(new MovpolDTO()
                    {
                        linea = ++i,
                        cta = 1146,
                        scta = 4,
                        sscta = 0,
                        digito = 0,
                        tm = 2,
                        numpro = 0,
                        referencia = ivaReff,
                        cc = lstCadena.LastOrDefault().cc,
                        area = lstCadena.LastOrDefault().area,
                        cuenta_oc = lstCadena.LastOrDefault().cuenta_oc,
                        orden_compra = lstCadena.LastOrDefault().orden_compra,
                        concepto = lstCadena.LastOrDefault().concepto,
                        itm = 0,
                        monto = abonoIva
                    });
                    #endregion
                }

                var listaCuentasEK = _contextEnkontrol.Select<CatctaDTO>(EnkontrolAmbienteEnum.Prod, new OdbcConsultaDTO() { consulta = "SELECT * FROM catcta" });
                var listaCTAINT = _contextEnkontrol.Select<CuentaDTO>(EnkontrolAmbienteEnum.Prod, new OdbcConsultaDTO() { consulta = "SELECT cuenta, sistema AS tipo FROM ctaint" });
                
                resultado.Add("objMovPolizas", lstCta.FirstOrDefault());
                resultado.Add("lstCta", lstCta.Select(x => new
                {
                    No = x.linea,
                    Cuenta = x.cta,
                    SCta = x.scta,
                    SSCta = x.sscta,
                    D = x.digito,
                    Mov = x.itm,
                    Proveedor = x.numpro,
                    Referencia = x.referencia,
                    cc = x.cc,
                    ac = string.Format("{0}-{1}", x.area, x.cuenta_oc),
                    oc = x.orden_compra ?? string.Empty,
                    isOc =  listaCuentasEK.Where(y => y.cta == x.cta && y.scta == x.scta && y.sscta == x.sscta).Select(z => z.requiere_oc).FirstOrDefault() == "S",
                    iSistema = listaCTAINT.Where(y => y.cuenta == x.cta).Select(z => z.tipo).FirstOrDefault(),
                    isInterface = !string.IsNullOrWhiteSpace(listaCTAINT.Where(y => y.cuenta == x.cta).Select(z => z.tipo).FirstOrDefault()),
                    Concepto = x.concepto,
                    TipoMovimiento = x.tm,
                    Monto = x.monto.ToString("C")
                }).OrderBy(o => o.No));
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, "PolizaController", "GetCtaCadenaNuevo", e, AccionEnum.CONSULTA, 0, null);
            }

            return resultado;
        }

        int getLastReferenciaFactoraje(int prov)
        {
            try
            {
                string consulta = string.Format("SELECT TOP 1 factura FROM sp_movprov WHERE tp = '04' AND numpro = '{0}' ORDER BY factura DESC", prov);
                var lstEkP = (List<MovProDTO>)_contextEnkontrol.Where(consulta).ToObject<List<MovProDTO>>();
                return lstEkP.LastOrDefault().factura.ParseInt();
            }
            catch (Exception) { return 0; }
        }
        int getLastReferenciaProveedor(int prov)
        {
            try
            {
                string consulta = string.Format("SELECT TOP 1 factura FROM sp_movprov WHERE tp = '07' AND numpro = '{0}' ORDER BY factura DESC", prov);
                var lstEkP = (List<MovProDTO>)_contextEnkontrol.Where(consulta).ToObject<List<MovProDTO>>();
                return lstEkP.LastOrDefault().factura.ParseInt();
            }
            catch (Exception) { return 0; }
        }
        List<MovProDTO> getLstReferenciaAbonoFactoraje(List<string> factura, string prov, int banco)
        {
            try
            {
                var odbcMovPol = new OdbcConsultaDTO()
                {
                    consulta = string.Format(@"SELECT DISTINCT dir.*
                        FROM sp_movprov fac 
                        INNER JOIN sc_movpol pol ON pol.year = fac.year AND pol.mes = fac.mes AND pol.tp = fac.tp AND pol.poliza = fac.poliza AND pol.cc = fac.cc and pol.numpro = ? AND pol.scta IN (1,2)
                        INNER JOIN sp_movprov dir ON dir.es_factura = 'S' AND dir.tp = pol.tp AND dir.numpro = pol.numpro AND dir.factura = CAST(pol.referencia AS numeric) AND dir.cc = pol.cc
                        WHERE fac.tp = '04' AND fac.es_factura = 'N' and fac.numpro = ? AND fac.factura IN {0}"
                    , factura.ToParamInValue())
                };
                odbcMovPol.parametros.Add(new OdbcParameterDTO() { nombre = "pol.numpro", tipo = OdbcType.Numeric, valor = banco });
                odbcMovPol.parametros.Add(new OdbcParameterDTO() { nombre = "fac.numpro ", tipo = OdbcType.Numeric, valor = prov });
                //odbcMovPol.parametros.Add(new OdbcParameterDTO() { nombre = "fac.factura ", tipo = OdbcType.Numeric, valor = factura });
                odbcMovPol.parametros.AddRange(factura.Select(s => new OdbcParameterDTO() { nombre = "fac.factura ", tipo = OdbcType.Numeric, valor = s }));
                var lstEkP = _contextEnkontrol.Select<MovProDTO>(EnkontrolAmbienteEnum.Prod, odbcMovPol);
                return lstEkP;
            }
            catch (Exception) { return new List<MovProDTO>(); }
        }
        List<MovpolDTO> getLstReferenciaAbonoFactoraje(int banco, string cc, decimal monto)
        {
            try
            {
                string consulta = string.Format("SELECT * FROM sc_movpol where tp = '04' and tm = 2 and numpro = '{0}' and cc = '{1}' and monto = -{2}", banco, cc, monto);
                var lstEkP = (List<MovpolDTO>)_contextEnkontrol.Where(consulta).ToObject<List<MovpolDTO>>();
                return lstEkP;
            }
            catch (Exception) { return new List<MovpolDTO>(); }
        }
        string getReferenciaAbonoFactoraje(int banco, string cc, decimal monto)
        {
            try
            {
                string consulta = string.Format("SELECT * FROM sc_movpol where tp = '04' and tm = 2 and numpro = '{0}' and cc = '{1}' and monto = -{2}", banco, cc, monto);
                var lstEkP = (List<MovpolDTO>)_contextEnkontrol.Where(consulta).ToObject<List<MovpolDTO>>();
                return lstEkP.LastOrDefault().referencia;
            }
            catch (Exception) { return string.Empty; }
        }
        decimal getMontoFromFactoraje(int prov, int factura)
        {
            try
            {
                string consulta = string.Format("SELECT * FROM sp_movprov where tp = '04' and numpro = {0} and factura = {1}", prov, factura);
                var lstEkP = (List<MovProDTO>)_contextEnkontrol.Where(consulta).ToObject<List<MovProDTO>>();
                var o = lstEkP.FirstOrDefault();
                return o.monto * o.tipocambio;
            }
            catch (Exception) { return 0; }
        }
        #endregion
        public List<PolizasDTO> getPolizaEk(DateTime fecha, int poliza, string tp)
        {
            try
            {
                string consulta = string.Format(@"SELECT year, mes, poliza, tp, fechapol, cargos, abonos, generada, status, status_lock, concepto
                                                FROM sc_polizas WHERE year = {0} 
                                                                AND mes = {1}",
                                                                              fecha.Year,
                                                                              fecha.Month);
                var lstEkP = (List<PolizasDTO>)_contextEnkontrol.Where(consulta).ToObject<List<PolizasDTO>>();
                return lstEkP.Where(w => poliza == 0 ? true : poliza == w.poliza)
                    .Where(w => tp.Equals(string.Empty) ? true : tp.Equals(w.tp)).ToList();
            }
            catch (Exception) { return new List<PolizasDTO>(); }
        }
        public List<PolizasDTO> getPolizaEk(DateTime perIni, DateTime perFin, int poliza, string tp)
        {
            try
            {
                string consulta = string.Format(@"SELECT year, mes, poliza, tp, fechapol, cargos, abonos, generada, status, status_lock, concepto
                                                FROM sc_polizas WHERE year >= {0} 
                                                                AND mes >= {1} 
                                                                AND year <= {2} 
                                                                AND mes <= {3}",
                                                                               perIni.Year,
                                                                               perIni.Day,
                                                                               perFin.Year,
                                                                               perFin.Day);
                var lstEkP = (List<PolizasDTO>)_contextEnkontrol.Where(consulta).ToObject<List<PolizasDTO>>();
                return lstEkP.Where(w => poliza == 0 ? true : poliza == w.poliza)
                    .Where(w => tp.Equals(string.Empty) ? true : tp.Equals(w.tp)).ToList();
            }
            catch (Exception) { return new List<PolizasDTO>(); }
        }
        #region Concentrado
        public List<MovPolDiarioAcumDTO> getLstPolizaDiario(BusqConcentradoDTO busq)
        {
            try
            {
                var consulta = queryLstPolizaDiario(busq);
                var lstEkP = (List<MovPolDiarioAcumDTO>)_contextEnkontrol.Where(consulta).ToObject<List<MovPolDiarioAcumDTO>>();
                var lst = lstEkP.ToList().GroupBy(g => new { g.poliza, g.cc, g.referencia, g.naturaleza, g.moneda })
                    .Select(m => new MovPolDiarioAcumDTO()
                    {
                        fechapol = m.FirstOrDefault().fechapol,
                        poliza = m.Key.poliza,
                        cc = m.Key.cc,
                        referencia = m.Key.referencia,
                        concepto = m.FirstOrDefault().concepto,
                        descripcion = m.FirstOrDefault().descripcion,
                        itm = m.FirstOrDefault().itm,
                        moneda = m.Key.moneda,
                        monto = m.Sum(s => s.monto),
                        naturaleza = m.Key.naturaleza
                    });
                return lstEkP.ToList();
            }
            catch (Exception) { return new List<MovPolDiarioAcumDTO>(); }
        }
        string queryLstPolizaDiario(BusqConcentradoDTO busq)
        {
            var idPablo = 1069;
            var strCuentas = vSesiones.sesionUsuarioDTO.id.Equals(idPablo) ? Infrastructure.Utils.EnumExtensions.ToLineValue<CuentasBancariasIndustrialEnum>(",") : Infrastructure.Utils.EnumExtensions.ToLineValue<CuentasBancariasEnum>(",");
            return string.Format(@"SELECT pol.fechapol ,pol.poliza, mov.referencia, mov.cc, tm.naturaleza, mov.monto, cue.moneda, mov.itm, tm.descripcion, cta.descripcion AS concepto
                                    FROM sc_polizas pol, sc_movpol mov, sb_cuenta cue, sb_tm tm, sc_movpol pue, catcta cta
                                    WHERE pol.poliza = mov.poliza AND pol.tp = mov.tp AND pol.year = mov.year AND pol.mes = mov.mes
                                        AND cue.cta = mov.cta AND cue.scta = mov.scta AND cue.sscta = mov.sscta
                                        AND pue.poliza = mov.poliza AND pue.tp = mov.tp AND pue.year = mov.year AND pue.mes = mov.mes AND pue.monto = (mov.monto * -1)
                                        AND cta.cta = pue.cta AND cta.scta = pue.scta AND cta.sscta = pue.sscta AND cta.digito = pue.digito
                                        AND tm.clave = mov.itm
                                        AND pol.tp = '03' AND pol.status = 'A'
                                        AND pol.fechapol >= '{0}' 
                                        AND pol.fechapol <= '{1}'
                                        AND cue.cuenta IN ({2})
                                        AND mov.cc IN ({3})"
                , busq.min.ToString("yyyy-MM-dd")
                , busq.max.ToString("yyyy-MM-dd")
                , strCuentas
                , busq.lstCC.ToLine(","));
        }
        #endregion
        #region Propuesta
        /// <summary>
        /// Guarda los saldos condensado del proveedor
        /// </summary>
        /// <param name="lst">facturas</param>
        /// <returns>si se puede guardar</returns>
        public bool guardarLstSaldosCondensados(List<tblC_SaldosCondensados> lst)
        {
            var esGuardado = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                var ahora = DateTime.Now;
                var lstBd = getAllCondSaldosActivos();
                lst.ForEach(cond =>
                {
                    var bd = lstBd.FirstOrDefault(condbd => condbd.numpro.Equals(cond.numpro) && condbd.cc.Equals(cond.cc) && cond.factura.Equals(condbd.factura) && cond.fechaPropuesta.noSemana().Equals(condbd.fechaPropuesta.noSemana()));
                    if (bd != null)
                    {
                        cond.id = bd.id;
                    }
                    cond.esActivo = true;
                    cond.fechaCaptura = ahora;
                    _context.tblC_SaldosCondensados.AddOrUpdate(cond);
                    SaveChanges();
                });
                esGuardado = lst.All(prov => prov.id > 0);
                if (esGuardado)
                {
                    var entity = lst.FirstOrDefault();
                    SaveBitacora((int)BitacoraEnum.SaldoCondensado, (int)AccionEnum.AGREGAR, entity.id, JsonUtils.convertNetObjectToJson(entity));
                }
                dbTransaction.Commit();
            }
            return esGuardado;
        }
        /// <summary>
        /// Consulta todas las facturas de proveedores activas en sigoplan
        /// </summary>
        /// <returns>Facturas activas</returns>
        public List<tblC_SaldosCondensados> getAllCondSaldosActivos()
        {
            return _context.tblC_SaldosCondensados.ToList().Where(cond => cond.esActivo).ToList();
        }
        /// <summary>
        /// Consulta los registros del reporte de condensado de saldos de arrendadora en construplan
        /// </summary>
        /// <param name="busq">Busqueda de propuesta</param>
        /// <returns>Saldos de facutras</returns>
        public List<AnaliticoVencimiento6ColDTO> getLstCondSaldosCplan(BusqConcentradoDTO busq)
        {
            try
            {
                var obj = new OdbcConsultaDTO()
                {
                    consulta = queryLstCondSaldosCplan(),
                    parametros = new List<OdbcParameterDTO>() { 
                        new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.Date, valor = busq.max } 
                    }
                };
                var lst = _contextEnkontrol.Select<AnaliticoVencimiento6ColDTO>(EnkontrolEnum.CplanProd, obj);
                return lst;
            }
            catch (Exception) { return new List<AnaliticoVencimiento6ColDTO>(); }
        }
        string queryLstCondSaldosCplan()
        {
            return string.Format(@"SELECT * FROM (SELECT mov.cc
                                      ,SUM(CASE WHEN mov.fechavenc <= ? THEN mov.total * mov.tipocambio ELSE 0 END) AS porVencer 
                                      FROM sp_movprov mov
                                      WHERE mov.numpro IN (4835,9867)
                                      GROUP BY mov.cc ORDER BY mov.cc) x
                                WHERE x.porVencer >0");
        }
        /// <summary>
        /// Consulta los registros del reporte de condensado de saldos de todos los proveedores de la arrendadora
        /// </summary>
        /// <param name="busq">Busqueda de propuesta</param>
        /// <returns>Saldos de facutras</returns>
        public List<AnaliticoVencimiento6ColDTO> getLstAnaliticoVencimiento6ColArrendadora(BusqConcentradoDTO busq)
        {
            try
            {
                var obj = new OdbcConsultaDTO()
                {
                    consulta = queryLstAnaliticoVencimiento6ColArrendadora(),
                    parametros = new List<OdbcParameterDTO>() { 
                        new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.Date, valor = busq.max } 
                    }
                };
                var lstEkP = _contextEnkontrol.Select<AnaliticoVencimiento6ColDTO>(EnkontrolEnum.ArrenProd, obj);
                var lst = lstEkP.ToList();
                return lst;
            }
            catch (Exception) { return new List<AnaliticoVencimiento6ColDTO>(); }
        }
        string queryLstAnaliticoVencimiento6ColArrendadora()
        {
            return string.Format(@"SELECT *  FROM (SELECT ac.descripcion AS cc ,SUM(CASE WHEN mov.fechavenc <= ? THEN mov.total * mov.tipocambio ELSE 0 END) AS porVencer
                                      FROM sp_movprov mov
                                      INNER JOIN so_orden_compra_det oc ON mov.referenciaoc = oc.numero and mov.cc = oc.cc
                                      INNER JOIN (SELECT area ,cuenta ,descripcion FROM si_area_cuenta GROUP BY area ,cuenta ,descripcion) ac ON ac.area = oc.area AND ac.cuenta = oc.cuenta
                                      GROUP BY ac.descripcion) x
                                    WHERE x.porVencer > 0");
        }
        /// <summary>
        /// Consulta los registros del reporte de condensado de saldos de todos los proveedores
        /// </summary>
        /// <param name="busq">Busqueda de propuesta</param>
        /// <returns>Saldos de facutras</returns>
        public List<AnaliticoVencimiento6ColDTO> getLstAnaliticoVencimiento6Col(BusqAnaliticoDTO busq)
        {
            try
            {
                var obj = new OdbcConsultaDTO()
                {
                    consulta = queryAnaliticoVencimiento6Col(busq),
                    parametros = paramAnaliticoVencimiento6Col(busq)
                };
                var lstEkP = _contextEnkontrol.Select<AnaliticoVencimiento6ColDTO>(EnkontrolAmbienteEnum.Prod, obj);
                var lst = lstEkP.ToList();
                lst = asignaAnalitico(lst);
                return lst;
            }
            catch (Exception) { return new List<AnaliticoVencimiento6ColDTO>(); }
        }
        string queryAnaliticoVencimiento6Col(BusqAnaliticoDTO busq)
        {
            return string.Format(@"SELECT *  FROM (SELECT numpro ,cc ,factura ,MAX(fecha) fechaFactura,MAX(fechavenc) fechaVence ,MIN(tm) AS tm
                                      ,SUM(CASE WHEN fechavenc >= ? THEN total * tipocambio ELSE 0 END) AS porVencer
                                      ,SUM(CASE WHEN fechavenc >= ? THEN total * tipocambio ELSE 0 END) AS dias7
                                      ,SUM(CASE WHEN fechavenc >= ? THEN total * tipocambio ELSE 0 END) AS dias14
                                      ,SUM(CASE WHEN fechavenc >= ? THEN total * tipocambio ELSE 0 END) AS dias30
                                      ,SUM(CASE WHEN fechavenc >= ? THEN total * tipocambio ELSE 0 END) AS dias45
                                      ,SUM(CASE WHEN fechavenc > ? THEN total * tipocambio ELSE 0 END) AS dias60
                                      ,SUM(CASE WHEN fechavenc < ? THEN total * tipocambio ELSE 0 END) AS dias61
                                      FROM sp_movprov
                                      WHERE numpro BETWEEN ? AND ?
                                        AND cc IN {0}
                                      GROUP BY numpro ,cc ,factura ORDER BY numpro ,factura) x
                WHERE x.porVencer > 0 OR x.dias7 > 0 OR x.dias14 > 0 OR x.dias30 > 0 OR x.dias45 > 0 OR x.dias60 > 0 OR x.dias61 > 0 AND x.tm IN {1}"
                                , busq.lstCC.ToParamInValue()
                                , busq.lstTm.ToParamInValue());
        }
        List<OdbcParameterDTO> paramAnaliticoVencimiento6Col(BusqAnaliticoDTO busq)
        {
            var dias7 = busq.fecha.AddDays(-7);
            var dias14 = busq.fecha.AddDays(-14);
            var dias30 = busq.fecha.AddDays(-30);
            var dias45 = busq.fecha.AddDays(-45);
            var dias60 = busq.fecha.AddDays(-60);
            var lst = new List<OdbcParameterDTO>();
            lst.Add(new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.Date, valor = busq.fecha });
            lst.Add(new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.Date, valor = dias7 });
            lst.Add(new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.Date, valor = dias14 });
            lst.Add(new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.Date, valor = dias30 });
            lst.Add(new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.Date, valor = dias45 });
            lst.Add(new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.Date, valor = dias60 });
            lst.Add(new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.Date, valor = dias60 });
            lst.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.Numeric, valor = busq.provMin });
            lst.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.Numeric, valor = busq.provMax });
            busq.lstCC.ForEach(cc => lst.Add(new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = cc }));
            busq.lstTm.ForEach(tm => lst.Add(new OdbcParameterDTO() { nombre = "tm", tipo = OdbcType.Numeric, valor = tm }));
            return lst;

        }
        List<AnaliticoVencimiento6ColDTO> asignaAnalitico(List<AnaliticoVencimiento6ColDTO> lst)
        {
            lst.ForEach(factura =>
            {
                if (factura.porVencer > 0)
                {
                    factura.dias7 = 0;
                    factura.dias14 = 0;
                    factura.dias30 = 0;
                    factura.dias45 = 0;
                    factura.dias60 = 0;
                    factura.dias61 = 0;
                }
                else if (factura.dias7 > 0)
                {
                    factura.porVencer = 0;
                    factura.dias14 = 0;
                    factura.dias30 = 0;
                    factura.dias45 = 0;
                    factura.dias60 = 0;
                    factura.dias61 = 0;
                }
                else if (factura.dias14 > 0)
                {
                    factura.porVencer = 0;
                    factura.dias7 = 0;
                    factura.dias30 = 0;
                    factura.dias45 = 0;
                    factura.dias60 = 0;
                    factura.dias61 = 0;
                }
                else if (factura.dias30 > 0)
                {
                    factura.porVencer = 0;
                    factura.dias7 = 0;
                    factura.dias14 = 0;
                    factura.dias45 = 0;
                    factura.dias60 = 0;
                    factura.dias61 = 0;
                }
                else if (factura.dias45 > 0)
                {
                    factura.porVencer = 0;
                    factura.dias7 = 0;
                    factura.dias14 = 0;
                    factura.dias30 = 0;
                    factura.dias60 = 0;
                    factura.dias61 = 0;
                }
                else if (factura.dias60 > 0)
                {
                    factura.porVencer = 0;
                    factura.dias7 = 0;
                    factura.dias14 = 0;
                    factura.dias30 = 0;
                    factura.dias45 = 0;
                    factura.dias61 = 0;
                }
                else if (factura.dias61 > 0)
                {
                    factura.porVencer = 0;
                    factura.dias7 = 0;
                    factura.dias14 = 0;
                    factura.dias30 = 0;
                    factura.dias45 = 0;
                    factura.dias60 = 0;
                }

            });
            return lst;
        }
        /// <summary>
        /// Consulta los registros del reporte de condensado de saldos de los bancos
        /// </summary>
        /// <param name="busq">Busqueda de propuesta</param>
        /// <returns>Saldos de bancos</returns>
        public List<MovProDTO> getLstCondSaldosBancos(BusqConcentradoDTO busq)
        {
            try
            {
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = queryLstCondSaldosBancos(),
                    parametros = paramLstCondesadoBancos(busq)
                };
                var lstEkP = _contextEnkontrol.Select<MovProDTO>(EnkontrolAmbienteEnum.Prod, odbc);
                var lst = lstEkP.Where(w => w.saldo != 0 || w.vencido != 0).ToList();
                return lst;
            }
            catch (Exception) { return new List<MovProDTO>(); }
        }
        string queryLstCondSaldosBancos()
        {
            return string.Format(@"SELECT cc ,numpro ,factura ,MAX(fecha) fechaFactura,MAX(fechavenc) fechaVence ,SUM(total * tipocambio) -  SUM(CASE WHEN fechavenc <= ? THEN total * tipocambio ELSE 0 END)                      AS saldo
                                ,SUM(CASE WHEN fechavenc <= ? THEN total * tipocambio ELSE 0 END) AS vencido
                                FROM sp_movprov 
                                    WHERE fecha <= ? 
                                    GROUP BY cc ,numpro ,factura");
        }
        List<OdbcParameterDTO> paramLstCondesadoBancos(BusqConcentradoDTO busq)
        {
            var lst = new List<OdbcParameterDTO>();
            lst.Add(new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.Date, valor = busq.max });
            lst.Add(new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.Date, valor = busq.max });
            lst.Add(new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.Date, valor = busq.max });
            return lst;
        }
        /// <summary>
        /// Registros del reporte de Analítico de vencimientos de 2 columnas
        /// </summary>
        /// <returns>Saldos de facturas</returns>
        public List<tblC_SaldosCondensados> getLstAnaliticoVencimiento(DateTime fechaBusqueda, List<string> lstCC)
        {
            try
            {
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = queryAnaliticoVencimiento(lstCC),
                    parametros = paramAnaliticoVencimiento(fechaBusqueda, lstCC)
                };
                var lstEkP = _contextEnkontrol.Select<tblC_SaldosCondensados>(EnkontrolAmbienteEnum.Prod, odbc);
                //var lst = lstEkP.Where(w => w.saldo > 0 || w.vencido > 0).ToList();
                return lstEkP;
            }
            catch (Exception) { return new List<tblC_SaldosCondensados>(); }
        }
        string queryAnaliticoVencimiento(List<string> lstCC)
        {
            return string.Format(@"SELECT numpro ,cc ,factura ,MAX(fecha) fechaFactura,MAX(fechavenc) fechaVence
                            ,SUM(CASE WHEN fechavenc > ? THEN total * tipocambio ELSE 0 END) AS saldo
                            ,SUM(CASE WHEN fechavenc <= ? THEN total * tipocambio ELSE 0 END) AS vencido
                            FROM sp_movprov 
                                WHERE fecha <= ?
                                GROUP BY numpro ,cc ,factura");
        }
        List<OdbcParameterDTO> paramAnaliticoVencimiento(DateTime fechaBusqueda, List<string> lstCC)
        {
            var lst = new List<OdbcParameterDTO>();
            lst.Add(new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.Date, valor = fechaBusqueda });
            lst.Add(new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.Date, valor = fechaBusqueda });
            lst.Add(new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.Date, valor = fechaBusqueda });
            return lst;
        }
        #endregion
        public List<ProveedorDTO> getProveedor()
        {
            try
            {
                List<ProveedorDTO> lstProveedores = new List<ProveedorDTO>();
                switch (vSesiones.sesionEmpresaActual) 
                {
                    case 6: //Perú
                        lstProveedores = getProveedorPeru();
                        break;
                    default:
                        lstProveedores = _contextEnkontrol.Select<ProveedorDTO>(EnkontrolAmbienteEnum.Prod, "SELECT * FROM " + vSesiones.sesionEmpresaDBPregijo + "sp_proveedores");
                        break;
                }
                return lstProveedores;
            }
            catch (Exception) { return new List<ProveedorDTO>(); }
        }

        public List<ProveedorDTO> CatProveedor(EnkontrolEnum conector)
        {
            return _contextEnkontrol.Select<ProveedorDTO>(conector, "SELECT * FROM \"DBA\".\"sp_proveedores\"");
        }
        public List<CatctaDTO> getCuenta()
        {
            try
            {
                var lstEkP = (List<CatctaDTO>)_contextEnkontrol.Where("SELECT * FROM catcta").ToObject<List<CatctaDTO>>();
                return lstEkP;
            }
            catch (Exception) { return new List<CatctaDTO>(); }
        }
        public List<MovProDTO> getFactura(int numPro)
        {
            try
            {
                var consulta = string.Format(@"SELECT mp.factura, mp.fecha, mp.fechavenc, mp.cc, mp.referenciaoc, mp.concepto, mp.tm
                                             ,(select SUM(g.total) from sp_movprov as g where g.numpro= {0} and g.factura=mp.factura) as total
                                             , (SELECT tm.descripcion FROM sp_tm tm WHERE tm.tm = mp.tm) as tmDesc
                                                 FROM sp_movprov mp
                                                 WHERE mp.numpro = {0}
                                                     AND mp.es_factura = 'S'
                                                    AND total > 0
                                                 ORDER BY fechavenc desc", numPro);
                var lstEkP = (List<MovProDTO>)_contextEnkontrol.Where(consulta).ToObject<List<MovProDTO>>();
                lstEkP.ForEach(x =>
                {
                    x.tmDesc = string.Format("{0} {1}", x.tm.ToString(), x.tmDesc);
                });
                return lstEkP;
            }
            catch (Exception) { return new List<MovProDTO>(); }
        }
        public MovProDTO getFactura(int numPro, string factura)
        {
            try
            {
                var consulta = string.Format(@"SELECT mp.factura, mp.cc, mp.concepto, mp.referenciaoc, mp.tm, mp.fecha, mp.moneda, tipoCambio, (select SUM(g.total) from sp_movprov as g where g.numpro= {0} and g.factura=mp.factura) as total, fecha, poliza, tp
                                                FROM sp_movprov mp
                                                WHERE mp.es_factura = 'S'
                                                    AND mp.numpro = {0}
                                                    AND mp.factura = {1}", numPro, factura);
                var lstEkP = (List<MovProDTO>)_contextEnkontrol.Where(consulta).ToObject<List<MovProDTO>>();
                if (lstEkP.Count == 0)
                    return new MovProDTO()
                    {
                        tipocambio = 1
                    };
                else
                    return lstEkP.FirstOrDefault();
            }
            catch (Exception)
            {
                return new MovProDTO()
                {
                    tipocambio = 1
                };
            }
        }
        public List<MovpolDTO> getPolizaDesdeFactura(List<MovProDTO> lstConsulta)
        {
            var lstFactura = new List<MovProDTO>();
            var lstMovPol = new List<MovpolDTO>();
            lstConsulta.ForEach(x => lstFactura.Add(getFactura(x.numpro.ParseInt(), x.factura.ToString())));
            lstFactura.ForEach(x => lstMovPol.AddRange(getMovPolizaEk(x.fecha, x.poliza, x.tp)));
            return lstMovPol;
        }
        public object getObjReferencia(string referencia, int numpro, string iSistema)
        {
            var reff = new SwitchClass<object>
                {
                    {"B", () => new object() },
                    {"P", () => getFactura(numpro, referencia) },
                    {"X", () => new object() },
                }.Execute(iSistema);
            return reff;
        }
        public Task<object> agetObjReferencia(string referencia, int numpro, string iSistema)
        {
            return Task.Factory.StartNew(() =>
            {
                var reff = new SwitchClass<object>
                {
                    {"B", () => new object() },
                    {"P", () => getFactura(numpro, referencia) },
                    {"X", () => new object() },
                }.Execute(iSistema);
                return reff;
            });
        }
        public OrdenCompraPagoDTO getOrdenCompra(int oc, string cc)
        {
            try
            {
                var res = (List<OrdenCompraPagoDTO>)_contextEnkontrol.Where(string.Format(@"SELECT * FROM so_orden_compra_pago
                                                                                                WHERE numero = {0} 
                                                                                                AND cc = '{1}'", oc, cc)).ToObject<List<OrdenCompraPagoDTO>>();
                return res.FirstOrDefault();
            }
            catch (Exception) { return new OrdenCompraPagoDTO(); }
        }
        public OrdenCompraPagoDTO getOrdenCompraProv(int prov, string cc)
        {
            try
            {
                var res = (List<OrdenCompraPagoDTO>)_contextEnkontrol.Where(string.Format(@"SELECT * FROM so_orden_compra
                                                                                                WHERE proveedor = {0} 
                                                                                                AND cc = '{1}'", prov, cc)).ToObject<List<OrdenCompraPagoDTO>>();
                return res.FirstOrDefault();
            }
            catch (Exception) { return new OrdenCompraPagoDTO(); }
        }
        public string getTmDescipcion(int clave)
        {
            try
            {
                string consulta = string.Format(@"SELECT descripcion AS Text FROM sp_tm WHERE tm = {0}", clave);
                var lstEkP = (List<Core.DTO.Principal.Generales.ComboDTO>)_contextEnkontrol.Where(consulta).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                return lstEkP.FirstOrDefault().Text;
            }
            catch (Exception) { return string.Empty; }
        }
        public decimal GetTipoCambioRegistro(string tp, string factura, string numProv, int tm, string cc, decimal monto)
        {
            if (numProv.ParseInt() < 9000)
                return 1;
            else
            {
                try
                {
                    string consulta = string.Empty;
                    if (tp.Equals("03"))
                        consulta = string.Format("SELECT tipocambio FROM sp_movprov where tp = '04' and tm = 15 and numpro = {0} and cc = '{1}' and monto = {2}", numProv, cc, monto);
                    else
                        consulta = string.Format("SELECT tipocambio FROM sp_movprov where tp = '07' and factura = {0} and numpro = {1}", factura, numProv);
                    var res1 = (List<Core.DTO.Contabilidad.VencimientoDTO>)_contextEnkontrol.Where(consulta).ToObject<List<Core.DTO.Contabilidad.VencimientoDTO>>();
                    var dll = res1.FirstOrDefault().tipoCambio;
                    return dll;

                }
                catch (Exception)
                {
                    var obj = _context.tblC_CadenaProductiva.FirstOrDefault(w => w.factura.Equals(factura) && w.numProveedor.Equals(numProv));
                    if (obj != null)
                        return obj.tipoCambio;
                    else
                        return -1;
                }
            }

        }
        public CatctaDTO getCuentata(int cta, int scta, int sscta)
        {
            try
            {
                string consulta = string.Format(@"SELECT * FROM catcta
                                                    WHERE cta = {0}
                                                        AND scta = {1}
                                                        AND sscta = {2}", cta, scta, sscta);
                var lstEkP = (List<CatctaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<CatctaDTO>>();
                var o = lstEkP.FirstOrDefault();
                o.moneda = getCtacompl(o.cta, o.scta, o.sscta).moneda;
                o.lblError = string.Empty;
                return o;
            }
            catch (Exception)
            {
                return new CatctaDTO()
                {
                    cta = cta,
                    scta = scta,
                    sscta = sscta,
                    descripcion = string.Empty,
                    digito = 0,
                    requiere_oc = string.Empty,
                    moneda = 1,
                    lblError = "No hay cuenta en existencia."
                };
            }
        }
        public Task<CatctaDTO> agetCuentata(int cta, int scta, int sscta)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    string consulta = string.Format(@"SELECT * FROM catcta
                                                    WHERE cta = {0}
                                                        AND scta = {1}
                                                        AND sscta = {2}", cta, scta, sscta);
                    var lstEkP = (List<CatctaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<CatctaDTO>>();
                    var o = lstEkP.FirstOrDefault();
                    o.moneda = getCtacompl(o.cta, o.scta, o.sscta).moneda;
                    o.lblError = string.Empty;
                    return o;
                }
                catch (Exception)
                {
                    return new CatctaDTO()
                    {
                        cta = cta,
                        scta = scta,
                        sscta = sscta,
                        descripcion = string.Empty,
                        digito = 0,
                        requiere_oc = string.Empty,
                        moneda = 1,
                        lblError = "No hay cuenta en existencia."
                    };
                }
            });
        }
        public CtacomplDTO getCtacompl(int cta, int scta, int sscta)
        {
            var objDefault = new CtacomplDTO()
            {
                ctadlls = cta,
                sctadlls = scta,
                ssctadlls = sscta,
                ctacom = cta,
                sctacom = scta,
                ssctacom = sscta,
                moneda = 1,
                ctadlls_fin = 0,
                digitocom = 0,
                digitodlls = 0,
                digitodlls_fin = 0,
                sctadlls_fin = 0
            };
            try
            {
                string consulta = string.Format(@"SELECT * FROM ctacompl
                                                            WHERE ctadlls = {0}
                                                            AND sctadlls = {1}
                                                            AND ssctadlls = {2}", cta, scta, sscta);
                var lstEkP = (List<CtacomplDTO>)_contextEnkontrol.Where(consulta).ToObject<List<CtacomplDTO>>();
                if (lstEkP.Count == 0)
                    return objDefault;
                else
                    return lstEkP.FirstOrDefault();
            }
            catch (Exception)
            {
                return objDefault;
            }
        }
        public CtaDifCambiariaDTO getCtaDif(int cta, int scta, int sscta)
        {
            var objDefault = new CtaDifCambiariaDTO()
            {
                cta_dif_cambiaria = 0,
                scta_dif_cambiaria = 0,
                sscta_dif_cambiaria = 0,
                digito_dif_cambiaria = 0,
                cta_ini = cta,
                scta_ini = scta,
                sscta_ini = sscta,
                digito_ini = 0,
                cta_fin = 0,
                scta_fin = 0,
                sscta_fin = 0,
                digito_fin = 0,
                cta_perdida = 0,
                scta_perdida = 0,
                sscta_perdida = 0,
                digito_perdida = 0,
            };
            try
            {
                string consulta = string.Format(@"SELECT * FROM dif_cambiaria
                                                            WHERE cta_ini = {0}
                                                            AND scta_ini = {1}
                                                            AND sscta_ini = {2}", cta, scta, sscta);
                var lstEkP = (List<CtaDifCambiariaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<CtaDifCambiariaDTO>>();
                if (lstEkP.Count == 0)
                    return objDefault;
                else
                    return lstEkP.FirstOrDefault();
            }
            catch (Exception)
            {
                return objDefault;
            }
        }
        public CtaIvaDTO getCtaIva(string iSistema)
        {
            try
            {
                var lstEkP = (List<CtaIvaDTO>)_contextEnkontrol.Where(string.Format(@"SELECT * FROM sc_iva WHERE sistema = '{0}'", iSistema)).ToObject<List<CtaIvaDTO>>();
                var obj = lstEkP.FirstOrDefault();
                try
                {
                    var lstIva = (List<CtaIvaDTO>)_contextEnkontrol.Where(string.Format(@"SELECT porcentaje FROM sp_porcentajes_iva
                                                                                                    WHERE cta = {0}
                                                                                                        and scta = {1}
                                                                                                        and sscta = {2}", obj.cta_iva, obj.scta_iva, obj.sscta_iva)).ToObject<List<CtaIvaDTO>>();
                    obj.porcentaje = lstIva.FirstOrDefault().porcentaje * .01m;
                }
                catch (Exception)
                {
                    obj.porcentaje = .16m;
                    throw;
                }
                return obj;
            }
            catch (Exception)
            {
                return new CtaIvaDTO() { porcentaje = .16m };
            }
        }
        public DifCambiariaDTO getCtaDiffCambiaria(string iSistema)
        {
            try
            {
                var lstEkP = (List<CtaintDTO>)_contextEnkontrol.Where(string.Format(@"SELECT * FROM ctaint WHERE sistema = '{0}'", iSistema)).ToObject<List<CtaintDTO>>();
                var obj = lstEkP.FirstOrDefault();
                try
                {
                    var lstDiff = (List<DifCambiariaDTO>)_contextEnkontrol.Where(string.Format(@"SELECT * FROM dif_cambiaria WHERE cta_ini = {0}", obj.cuenta)).ToObject<List<DifCambiariaDTO>>();
                    return lstDiff.FirstOrDefault();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                return new DifCambiariaDTO();
            }
        }
        public CuentaDTO getInterfaceDescripcion(int cta, int scta, int sscta, string tp, int numprov)
        {
            try
            {
                var obj = new CuentaDTO() { tipo = getInterfaceSistema(cta, scta, sscta) };
                obj.descripcion = new SwitchClass<string>
                {
                    {string.Empty, () => getCuentaDesc(cta, scta, sscta, tp).descripcion },
                    {"B", () => getBancDesc(cta, scta, sscta).descripcion },
                    {"P", () => getProvDesc(numprov).descripcion },
                    {"X", () => "Vivienda" },
                }.Execute(obj.tipo);
                return obj;
            }
            catch (Exception)
            {
                return new CuentaDTO()
                {
                    descripcion = string.Empty,
                    tipo = getInterfaceSistema(cta, scta, sscta)
                };
            }
        }
        public Task<CuentaDTO> agetInterfaceDescripcion(int cta, int scta, int sscta, string tp, int numprov)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var obj = new CuentaDTO() { tipo = getInterfaceSistema(cta, scta, sscta) };
                    obj.descripcion = new SwitchClass<string>
                {
                    {string.Empty, () => getCuentaDesc(cta, scta, sscta, tp).descripcion },
                    {"B", () => getBancDesc(cta, scta, sscta).descripcion },
                    {"P", () => getProvDesc(numprov).descripcion },
                    {"X", () => "Vivienda" },
                }.Execute(obj.tipo);
                    return obj;
                }
                catch (Exception)
                {
                    return new CuentaDTO()
                    {
                        descripcion = string.Empty,
                        tipo = getInterfaceSistema(cta, scta, sscta)
                    };
                }
            });
        }
        public Task<string> agetInterfaceSistema(int cta, int scta, int sscta)
        {
            return Task.Factory.StartNew(() =>
            {
                return getInterfaceSistema(cta, scta, sscta);
            });
        }
        public string getInterfaceSistema(int cta, int scta, int sscta)
        {
            try
            {
                var odbc = new OdbcConsultaDTO() { consulta = "SELECT sistema AS tipo FROM ctaint WHERE cuenta = ?" };
                odbc.parametros.Add(new OdbcParameterDTO() { nombre = "cuenta", tipo = OdbcType.VarChar, valor = cta });
                var lstEkP = _contextEnkontrol.Select<CuentaDTO>(EnkontrolAmbienteEnum.Prod, odbc);
                return lstEkP.FirstOrDefault().tipo;
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }
        CuentaDTO getCuentaDesc(int cta, int scta, int sscta, string tp)
        {
            string consulta = string.Format(@"SELECT * FROM sb_cuenta WHERE cta = {0} AND scta = {1} AND sscta = {2}", cta, scta, sscta);
            var lstEkP = (List<CuentaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<CuentaDTO>>();
            //if(vSesiones.sesionEmpresaActual==6)
            //{
            //    lstEkP = (List<CuentaDTO>)_context.Where(consulta).ToObject<List<CuentaDTO>>();
            //}
            return lstEkP.FirstOrDefault();
        }
        CuentaDTO getCuentaDesc(int cta, int scta, int sscta)
        {
            string consulta = string.Format(@"SELECT * FROM sb_cuenta
                                                    WHERE cta = {0}
                                                        AND scta = {1}
                                                        AND sscta = {2}", cta, scta, sscta);
            var lstEkP = (List<CuentaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<CuentaDTO>>();
            return lstEkP.FirstOrDefault();
        }
        CuentaDTO getProvDesc(int numprov)
        {
            string consulta = string.Format(@"SELECT nombre as descripcion FROM sp_proveedores WHERE numpro ={0}", numprov);
            var lstEkP = (List<CuentaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<CuentaDTO>>();
            return lstEkP.FirstOrDefault();
        }
        CuentaDTO getBancDesc(int cta, int scta, int sscta)
        {
            string consulta = string.Format(@"SELECT * FROM sb_cuenta 
                                                    WHERE cta = {0}
                                                        AND scta = {1}
                                                        AND sscta = {2}", cta, scta, sscta);
            var lstEkP = (List<CuentaDTO>)_contextEnkontrol.Where(consulta).ToObject<List<CuentaDTO>>();
            return lstEkP.FirstOrDefault();
        }
        ProveedorDTO getProv(int numprov)
        {
            string consulta = string.Format(@"SELECT * FROM sp_proveedores WHERE numpro = {0}", numprov);
            var lstEkP = (List<ProveedorDTO>)_contextEnkontrol.Where(consulta).ToObject<List<ProveedorDTO>>();
            return lstEkP.FirstOrDefault();
        }
        ClienteDTO getClie(int numcte)
        {
            string consulta = string.Format(@"SELECT * FROM sx_clientes WHERE numcte = {0}", numcte);
            var lstEkP = (List<ClienteDTO>)_contextEnkontrol.Where(consulta).ToObject<List<ClienteDTO>>();
            return lstEkP.FirstOrDefault();
        }
        public int getNumPoliza(string tp, DateTime fecha)
        {
            try
            {
                string consulta = string.Format(@"SELECT top 1 (poliza + 1) As poliza FROM sc_polizas
                                                WHERE year = {0}
                                                    AND mes = {1}
                                                    AND tp = '{2}'
                                                    ORDER BY poliza DESC", fecha.Year, fecha.Month, tp);
                var lstEkP = (List<PolizasDTO>)_contextEnkontrol.Where(consulta).ToObject<List<PolizasDTO>>();
                return lstEkP.FirstOrDefault().poliza;
            }
            catch (Exception) { return 1; }
        }
        #region Combobox
        public List<Core.DTO.Principal.Generales.ComboDTO> lstObra()
        {
            try
            {
                var consulta = string.Empty;
                var lst = new List<Core.DTO.Principal.Generales.ComboDTO>();
                switch (vSesiones.sesionEmpresaActual)
                {
                    case (int)EmpresaEnum.Construplan:
                        {
                            var bajio = vSesiones.sesionUsuarioDTO.isBajio;
                            if (bajio)
                            {
                                lst = _context.tblP_CC.Where(x => x.isBajio).Select(x => new Core.DTO.Principal.Generales.ComboDTO { Value = x.cc, Text = x.cc + " " + x.descripcion }).ToList();
                            }
                            else
                            {
                                lst = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.CplanProd, "SELECT cc as Value,  (cc+'-' +descripcion) as Text FROM cc ORDER BY cc");
                            }
                            break;
                        }
                    case (int)EmpresaEnum.Arrendadora:
                        lst = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.ArrenProd, "SELECT DISTINCT (CAST(area  AS varchar(4))+'-'+CAST(cuenta AS varchar(4))) AS Value, (CAST(area  AS varchar(4))+'-'+CAST(cuenta AS varchar(4))+'-'+descripcion) AS Text, area, cuenta FROM si_area_cuenta WHERE cc_activo = 1 ORDER BY area, cuenta");
                        break;
                    case (int)EmpresaEnum.Colombia:
                        lst = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.ColombiaProductivo, "SELECT cc as Value,  (cc+'-' +descripcion) as Text FROM DBA.cc ORDER BY cc");
                        break;
                    case (int)EmpresaEnum.Peru:
                        lst = _context.tblP_CC.Select(x => new Core.DTO.Principal.Generales.ComboDTO { Value = x.cc, Text = x.cc + " " + x.descripcion }).ToList();
                        break;
                }
                var lstDiv = _context.tblC_CCDivision.Where(x => x.cc != "").OrderBy(o => o.cc).ToList();
                lst.ForEach(e =>
                {
                    e.Prefijo = e.Prefijo ?? "0";
                    e.Prefijo = lstDiv.Exists(d => e.Value.Equals(d.cc.Replace(System.Environment.NewLine, string.Empty))) ? lstDiv.FirstOrDefault(d => e.Value.Equals(d.cc.Replace(System.Environment.NewLine, string.Empty))).division.ToString() : e.Prefijo;
                });
                return lst.GroupBy(x => x.Value).Select(x => new Core.DTO.Principal.Generales.ComboDTO { Value = x.Key, Text = x.First().Text, Prefijo = x.First().Prefijo }).ToList();
            }
            catch (Exception)
            { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> getComboOc()
        {
            try
            {
                var lstEkTP = (List<Core.DTO.Principal.Generales.ComboDTO>)_contextEnkontrol.Where("SELECT numcte as Value, nomcorto as Text FROM sx_clientes").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                return lstEkTP.ToList();
            }
            catch (Exception) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> getComboCentroCostos()
        {
            try
            {
                var lst = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolAmbienteEnum.Prod, "SELECT cc as Value,  (cc+'-' +descripcion) as Text FROM cc ORDER BY cc");
                return lst;
            }
            catch (Exception o_O) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> getComboAreaCuenta()
        {
            try
            {
                var odbcCplan = new OdbcConsultaDTO();
                if (vSesiones.sesionEmpresaActual == 2)
                {
                    odbcCplan = new OdbcConsultaDTO()
                    {
                        consulta = @"SELECT DISTINCT CAST(area AS Varchar)+'-'+CAST(cuenta AS Varchar) AS Value, CAST(area AS Varchar)+'-'+CAST(cuenta AS Varchar)+' '+descripcion AS Text ,UPPER(centro_costo) AS Prefijo ,area,cuenta
                                            FROM si_area_cuenta
                                            ORDER BY area, cuenta"
                        //                        consulta = @"SELECT DISTINCT CAST(area AS Varchar)+'-'+CAST(cuenta AS Varchar) AS Value, CAST(area AS Varchar)+'-'+CAST(cuenta AS Varchar)+' '+descripcion AS Text ,UPPER(centro_costo) AS Prefijo ,area,cuenta
                        //                                            FROM si_area_cuenta 
                        //                                            WHERE cc_activo = 1
                        //                                            ORDER BY area, cuenta"
                    };
                }
                else
                {
                    odbcCplan = new OdbcConsultaDTO()
                    {
                        consulta = @"SELECT DISTINCT CAST(area AS Varchar)+'-'+CAST(cuenta AS Varchar) AS Value, CAST(area AS Varchar)+'-'+CAST(cuenta AS Varchar)+' '+descripcion AS Text ,centro_costo AS Prefijo ,area,cuenta
                                            FROM si_area_cuenta
                                            ORDER BY area, cuenta"
                        //                        consulta = @"SELECT DISTINCT CAST(area AS Varchar)+'-'+CAST(cuenta AS Varchar) AS Value, CAST(area AS Varchar)+'-'+CAST(cuenta AS Varchar)+' '+descripcion AS Text ,centro_costo AS Prefijo ,area,cuenta
                        //                                            FROM si_area_cuenta 
                        //                                            WHERE cc_activo = 1
                        //                                            ORDER BY area, cuenta"
                    };
                }
                var lst = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolAmbienteEnum.Prod, odbcCplan);
                return lst;
            }
            catch (Exception o_O) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> getComboTipoPoliza()
        {
            try
            {
                var lstEkTP = (List<Core.DTO.Principal.Generales.ComboDTO>)_contextEnkontrol.Where("SELECT tp as Value, descripcion as Text, cve_tp_sat as prefijo FROM tipos_poliza").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                lstEkTP.FirstOrDefault().Prefijo = "1";
                lstEkTP.Where(w => string.IsNullOrEmpty(w.Prefijo)).ToList().ForEach(x => x.Prefijo = string.Empty);
                return lstEkTP.ToList();
            }
            catch (Exception) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        public List<tblC_CCProrrateo> getLstCCProrrateo(string cc)
        {
            return _context.tblc_CCProrrateo.ToList().Where(p => p.cc.Equals(cc)).ToList();
        }
        public List<tblC_CCProrrateo> getLstCCProrrateo()
        {
            return _context.tblc_CCProrrateo.ToList();
        }
        public List<tblC_RelCCPropuesta> getRelCCPropuesta()
        {
            return _context.tblC_RelCCPropuesta.ToList();
        }
        public List<tblC_RelCCPropuesta> getRelCCPropuesta(string ccSecundario)
        {
            return _context.tblC_RelCCPropuesta.ToList().Where(c => c.ccSecundario.Equals(ccSecundario)).ToList();
        }
        public bool guardarLstCCProrrateo(List<tblC_CCProrrateo> lst)
        {
            var esGuardado = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                getLstCCProrrateo(lst.FirstOrDefault().cc).ForEach(bd =>
                {
                    _context.tblc_CCProrrateo.Remove(bd);
                    _context.SaveChanges();
                });
                lst.ForEach(p =>
                {
                    _context.tblc_CCProrrateo.AddOrUpdate(p);
                    _context.SaveChanges();
                });
                esGuardado = lst.All(p => p.id > 0);
                dbTransaction.Commit();
            }
            return esGuardado;
        }
        public bool guarderRelCCPropuesta(List<tblC_RelCCPropuesta> lst)
        {
            var esGuardado = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                lst.ForEach(p =>
                {
                    var bd = _context.tblC_RelCCPropuesta.ToList().FirstOrDefault(b => b.ccSecundario.Equals(p.ccSecundario));
                    if (bd == null)
                    {
                        bd = new tblC_RelCCPropuesta();
                        bd.ccSecundario = p.ccSecundario;
                    }
                    bd.ccPrincipal = p.ccPrincipal;
                    _context.tblC_RelCCPropuesta.AddOrUpdate(bd);
                    _context.SaveChanges();
                });
                esGuardado = lst.All(p => p.id > 0);
                dbTransaction.Commit();
            }
            return esGuardado;
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> getComboTipoMovimiento()
        {
            try
            {
                var consulta = new SwitchClass<string>
                {
                    {"B", () => "SELECT (CONVERT(char(2), clave) + '  ' + descripcion) as Text, clave as Value, naturaleza as prefijo FROM sb_tm"},
                    {"P", () => "SELECT (CONVERT(char(2), tm) + '  ' + descripcion) as Text, tm as Value, naturaleza as prefijo FROM sp_tm"},
                    {"X", () => "SELECT (CONVERT(char(2), tm) + '  ' + descripcion) as Text, tm as Value, naturaleza as prefijo FROM sx_tm"},
                }.Execute(string.Empty);
                var allTm = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolAmbienteEnum.Prod, "SELECT (CONVERT(char(2) ,clave) + '  ' + descripcion) as Text, clave as Value, (CONVERT(char(2) ,naturaleza)+'-B') as prefijo FROM sb_tm");
                allTm.AddRange(_contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolAmbienteEnum.Prod, "SELECT (CONVERT(char(2) ,tm) + '  ' + descripcion) as Text ,tm as Value ,(CONVERT(char(2) ,naturaleza)+'-P') as prefijo FROM sp_tm"));
                allTm.AddRange(_contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolAmbienteEnum.Prod, "SELECT (CONVERT(char(2) ,tm) + '  ' + descripcion) as Text ,tm as Value ,(CONVERT(char(2) ,naturaleza)+'-X') AS prefijo FROM sx_tm"));
                return allTm;
            }
            catch (Exception)
            {
                var lst = new List<Core.DTO.Principal.Generales.ComboDTO>();
                lst.Add(
                    new Core.DTO.Principal.Generales.ComboDTO()
                    {
                        Prefijo = "",
                        Text = "",
                        Value = ""
                    });
                return lst;

            }
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> getComboTipoMovimiento(string iSistema)
        {
            try
            {
                var lstEkTP = new List<Core.DTO.Principal.Generales.ComboDTO>();                
                var consulta = new SwitchClass<string>
                {
                    {"B", () => "SELECT (CONVERT(char(2), clave) + '  ' + descripcion) as Text, clave as Value, naturaleza as prefijo FROM " + vSesiones.sesionEmpresaDBPregijo + "sb_tm"},
                    {"P", () => "SELECT (CONVERT(char(2), tm) + '  ' + descripcion) as Text, tm as Value, naturaleza as prefijo FROM " + vSesiones.sesionEmpresaDBPregijo + "sp_tm"},
                    {"X", () => "SELECT (CONVERT(char(2), tm) + '  ' + descripcion) as Text, tm as Value, naturaleza as prefijo FROM " + vSesiones.sesionEmpresaDBPregijo + "sx_tm"},
                }.Execute(iSistema);
                lstEkTP = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolAmbienteEnum.Prod, consulta);    
                return lstEkTP;
            }
            catch (Exception)
            {
                var lst = new List<Core.DTO.Principal.Generales.ComboDTO>();
                lst.Add(
                    new Core.DTO.Principal.Generales.ComboDTO()
                    {
                        Prefijo = "",
                        Text = "",
                        Value = ""
                    });
                return lst;

            }
        }
        
        public Task<List<Core.DTO.Principal.Generales.ComboDTO>> agetComboTipoMovimiento(Task<string> iSistema)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var consulta = new SwitchClass<string>
                {
                    {"B", () => "SELECT (CONVERT(char(2), clave) + '  ' + descripcion) as Text, clave as Value, naturaleza as prefijo FROM sb_tm"},
                    {"P", () => "SELECT (CONVERT(char(2), tm) + '  ' + descripcion) as Text, tm as Value, naturaleza as prefijo FROM sp_tm"},
                    {"X", () => "SELECT (CONVERT(char(2), tm) + '  ' + descripcion) as Text, tm as Value, naturaleza as prefijo FROM sx_tm"},
                }.Execute(iSistema);
                    var lstEkTP = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolAmbienteEnum.Prod, consulta);
                    return lstEkTP;
                }
                catch (Exception)
                {
                    var lst = new List<Core.DTO.Principal.Generales.ComboDTO>();
                    lst.Add(
                        new Core.DTO.Principal.Generales.ComboDTO()
                        {
                            Prefijo = "",
                            Text = "",
                            Value = ""
                        });
                    return lst;

                }
            });
        }
        string setEstatus(string status)
        {
            switch (status)
            {
                case "A": return "Actualizado";
                case "B": return "Bloqueada";
                case "C": return "Capturada";
                case "E": return "Errónea";
                case "V": return "Validada";
                default: return status;
            }
        }
        string setGenerada(string Gen)
        {
            switch (Gen)
            {
                case "A": return "Administración";
                case "B": return "Bancos";
                case "C": return "Contabilidad";
                case "F": return "Facturación";
                case "I": return "Inventarios";
                case "P": return "Proveedores";
                case "X": return "Clientes";
                default: return Gen;
            }
        }
        #endregion

        public bool aplicarBloqueo()
        {
            return _context.tblP_ReglasSubcontratistasBloqueo.Any(x => x.estatus && x.aplicar && x.id == 2);
        }

        public List<SubcontratistaBloqueadoDTO> subcontratistasBloqueados()
        {
            var subcontratistaDB = _context.sp_Select<SubcontratistaBloqueadoDTO>(new StoreProcedureDTO
            {
                nombre = "spSUBCONTRATISTAS_PROVEEDOR"
            });

            return subcontratistaDB;
        }


        #region Peru
        public List<ProveedorDTO> getProveedorPeru()
        {
            List<ProveedorDTO> lstProveedores = new List<ProveedorDTO>();
            using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
            {
                using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                {
                    try
                    {
                        var formasPago = _starsoft.FORMA_PAGO.ToList();
                        lstProveedores = _starsoft.MAEPROV.ToList().Select(x =>
                        {
                            var diasPago = formasPago.FirstOrDefault(y => y.COD_FP == x.PRVPAGO);
                            return new ProveedorDTO
                            {
                                numpro = 0,
                                nomcorto = x.PRVCNOMBRE,
                                nombre = x.PRVCNOMBRE,
                                condpago = diasPago == null ? 0 : diasPago.DIA_FP,
                                numproPeru = x.PRVCCODIGO
                                //moneda
                                //bit_factoraje
                            };
                        }).ToList();
                    }
                    catch (Exception e)
                    {
                        dbSigoplanTransaction.Rollback();
                    }
                }
            }
            return lstProveedores;
        }

        #endregion
    }
}
