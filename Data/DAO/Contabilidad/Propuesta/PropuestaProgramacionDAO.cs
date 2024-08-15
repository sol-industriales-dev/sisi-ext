using Core.DAO.Contabilidad.Propuesta;
using Core.DTO;
using Core.DTO.Contabilidad.Propuesta;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using Core.Enum.Multiempresa;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Administracion;
using Core.DTO.COMPRAS;
using Infrastructure.DTO;
using Core.DTO.Contabilidad.Poliza;
using Core.DTO.Contabilidad;
using System.Data.Entity;
using Core.DTO.RecursosHumanos;
using Newtonsoft.Json;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using Core.Entity.Administrativo.Contabilidad;
using Core.Entity.Portal;
using Core.DTO.Contabilidad.Proveedores;
//using Core.DTO.Contabilidad.Propuesta.Validacion;
//using Core.DTO.Enkontrol.Tablas.Poliza;
using System.IO;
using System.Xml.Linq;
using Core.DTO.Contabilidad.Propuesta.Validacion;
using Core.DAO.Contabilidad.Poliza;
using Data.Factory.Contabilidad.Poliza;
using Core.Enum.Contabilidad.Propuesta;

namespace Data.DAO.Contabilidad.Propuesta
{
    public class PropuestaProgramacionDAO : GenericDAO<tblC_sp_gastos_prov>, IPropuestaProgramacionDAO
    {
        private bool _productivo = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["enkontrolProductivo"]) == "1";

        IPolizaSPDAO _polizaEkFS = new PolizaSPFactoryService().GetPolizaEkService();
        IPolizaSPDAO _polizaSPFS = new PolizaSPFactoryService().GetPolizaSPService();

        #region Guardar
        public Dictionary<string, object> guardarGastosProv(List<tblC_sp_gastos_prov> lst, bool manual)
        {
            var result = new Dictionary<string, object>();

            using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
            {
                using (var con = checkConexionProductivo())
                {
                    using (var trans = con.BeginTransaction())
                    {
                        try
                        {
                            var listaNoActivoFijo = lst.Where(x => !((bool)x.activo_fijo)).ToList();
                            var listaPeru = lst.Where(x => x.numproPeru != null && x.numproPeru != "").ToList();
                            var listaActivoFijoTemporal = lst.Where(x => ((bool)x.activo_fijo)).ToList();

                            #region SIGOPLAN
                            if (listaActivoFijoTemporal.Count > 0)
                            {
                                var listaActivoFijo = new List<tblC_sp_gastos_prov_activofijo>();

                                foreach (var i in listaActivoFijoTemporal)
                                {
                                    listaActivoFijo.Add(new tblC_sp_gastos_prov_activofijo
                                    {
                                        id = i.id,
                                        idSigoplan = i.idSigoplan,
                                        numpro = (int)i.numpro,
                                        cfd_serie = i.cfd_serie,
                                        cfd_folio = i.cfd_folio,
                                        referenciaoc = i.referenciaoc,
                                        tipo_prov = i.tipo_prov,
                                        cc = i.cc,
                                        tm = i.tm,
                                        factura = i.factura,
                                        monto = i.monto,
                                        iva = i.iva,
                                        tipocambio = i.tipocambio,
                                        total = i.total,
                                        totalMN = i.totalMN,
                                        fecha_timbrado = i.fecha_timbrado,
                                        estatus = i.estatus,
                                        idGiro = i.idGiro,
                                        concepto = i.concepto,
                                        fecha = i.fecha,
                                        moneda = i.moneda,
                                        uuid = i.uuid,
                                        ruta_rec_xml = i.ruta_rec_xml,
                                        ruta_rec_pdf = i.ruta_rec_pdf,
                                        fecha_autoriza_portal = i.fecha_autoriza_portal,
                                        descuento = i.descuento,
                                        validacion = i.validacion,
                                        uuid_original = i.uuid_original,
                                        bit_nc = i.bit_nc,
                                        bit_compu = i.bit_compu,
                                        bit_carga = i.bit_carga,
                                        compuesta = i.compuesta,
                                        email_carga = i.email_carga,
                                        comentario_rechazo = i.comentario_rechazo,
                                        uuid_rechazo = i.uuid_rechazo,
                                        total_xml = i.total_xml,
                                        fecha_autoriza_factura = i.fecha_autoriza_factura,
                                        usuario_autoriza = i.usuario_autoriza,
                                        bit_antnc = i.bit_antnc,
                                        nivel_aut = i.nivel_aut,
                                        cerrado = i.cerrado,
                                        ruta_rec_xml_depura = i.ruta_rec_xml_depura,
                                        ruta_rec_pdf_depura = i.ruta_rec_pdf_depura,
                                        fechaPropuesta = i.fechaPropuesta,
                                        autorizada = i.autorizada,
                                        fechaAutorizacion = i.fechaAutorizacion,
                                        monto_plan = i.monto_plan,
                                        programo = i.programo,
                                        autorizo = i.autorizo,
                                        manual = i.manual,
                                        ac = i.ac,
                                        acDesc = i.acDesc,
                                        activo_fijo = i.activo_fijo
                                    });
                                }

                                _context.tblC_sp_gastos_prov_activofijo.AddRange(listaActivoFijo);
                                _context.SaveChanges();
                            }
                            #endregion

                            #region ENKONTROL
                            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Colombia) manual = true;
                            var listaEnkontrol = (manual ? getLstGastosProvManual(listaNoActivoFijo) : getLstGastosProv(listaNoActivoFijo));
                            var listaSIGOPLAN = getLstGastosProvSigoplan(listaNoActivoFijo);                            

                            foreach (var noActivoFijo in listaNoActivoFijo)
                            {
                                var gastosProveedorEnkontrol = listaEnkontrol.FirstOrDefault(ek => ek.cc.Equals(noActivoFijo.cc) && ek.numpro == ek.numpro && ek.factura.Equals(noActivoFijo.factura));

                                if (gastosProveedorEnkontrol != null)
                                {
                                    gastosProveedorEnkontrol.idGiro = noActivoFijo.idGiro;
                                    gastosProveedorEnkontrol.estatus = noActivoFijo.estatus;
                                    gastosProveedorEnkontrol.idSigoplan = listaSIGOPLAN.Any(w => gastosProveedorEnkontrol.id == w.id) ? listaSIGOPLAN.FirstOrDefault(w => gastosProveedorEnkontrol.id.Equals(w.id)).idSigoplan : 0;
                                    gastosProveedorEnkontrol.fechaPropuesta = DateTime.Now;
                                    gastosProveedorEnkontrol.concepto = noActivoFijo.concepto;
                                    gastosProveedorEnkontrol.ac = noActivoFijo.ac;
                                    gastosProveedorEnkontrol.programo = vSesiones.sesionUsuarioDTO.nombre;

                                    int queryExec = 0;

                                    List<OdbcParameterDTO> parametros = new List<OdbcParameterDTO>();
                                    parametros.Add(new OdbcParameterDTO() { nombre = "autorizapago", tipo = OdbcType.VarChar, valor = gastosProveedorEnkontrol.estatus.Equals("P") ? string.Empty : gastosProveedorEnkontrol.estatus });
                                    if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Colombia) parametros.Add(new OdbcParameterDTO() { nombre = "empleado_vobo", tipo = OdbcType.Numeric, valor = 1 });
                                    if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Colombia) parametros.Add(new OdbcParameterDTO() { nombre = "fecha_vobo", tipo = OdbcType.Date, valor = DateTime.Now });
                                    parametros.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.VarChar, valor = gastosProveedorEnkontrol.numpro });
                                    parametros.Add(new OdbcParameterDTO() { nombre = "factura", tipo = OdbcType.VarChar, valor = gastosProveedorEnkontrol.factura });
                                    parametros.Add(new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = gastosProveedorEnkontrol.cc });
                                    
                                    queryExec  = _contextEnkontrol.SaveT(trans, new OdbcConsultaDTO()
                                    {
                                        consulta = @"
                                            UPDATE " + vSesiones.sesionEmpresaDBPregijo + @"sp_movprov 
                                            SET autorizapago = ?" + (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Colombia ? @"" : @", empleado_vobo = ?, fecha_vobo = ?")
                                            + @" WHERE es_factura = 'S' AND numpro = ? AND factura = ? AND cc = ?",
                                        parametros = parametros
                                    });

                                    if (queryExec > 0)
                                    {
                                        #region Descuenta_Notas_Credito
                                        var data = _contextEnkontrol.Select<tblC_sp_gastos_prov>(getEnkontrolEnumADM(), new OdbcConsultaDTO()
                                        {
                                            consulta = @"
                                                SELECT
                                                    total
                                                FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_movprov
                                                WHERE numpro = ? AND factura = ? AND es_factura = 'N' AND tm IN (26,51) AND autorizapago NOT IN ('A','E')",
                                            parametros = new List<OdbcParameterDTO> {
                                                new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.Numeric, valor = gastosProveedorEnkontrol.numpro },
                                                new OdbcParameterDTO() { nombre = "factura", tipo = OdbcType.VarChar, valor = gastosProveedorEnkontrol.factura }
                                            }
                                        });
                                        var resta = data.Sum(x => Math.Abs(x.total));
                                        var total = gastosProveedorEnkontrol.total - resta;

                                        gastosProveedorEnkontrol.total = total;
                                        gastosProveedorEnkontrol.total_xml = total;
                                        gastosProveedorEnkontrol.monto_plan = noActivoFijo.monto_plan;

                                        _context.tblC_sp_gastos_prov.AddOrUpdate(gastosProveedorEnkontrol);
                                        SaveChanges();
                                        #endregion
                                    }
                                }

                            }
                            #region Peru

                            BusqPropEkDTO busq = new BusqPropEkDTO();
                            busq.lstCc = new List<string>();
                            busq.fechaCorte = new DateTime(DateTime.Today.Year, 12, 31);
                            busq.min = listaPeru.Min(x => x.numproPeru);
                            busq.max = listaPeru.Max(x => x.numproPeru);
                            var facturasTodas = getLstFacturasProvPeru(busq);

                            foreach (var facturaPeru in listaPeru)
                            {
                                var _factura = facturasTodas.FirstOrDefault(x => x.numproPeru == facturaPeru.numproPeru && x.factura == facturaPeru.factura);
                                if (_factura != null)
                                {
                                    facturaPeru.cfd_serie = "";
                                    facturaPeru.cfd_folio = 0;
                                    facturaPeru.tipo_prov = 0;
                                    facturaPeru.monto = _factura.monto_plan;
                                    facturaPeru.iva = 0;
                                    facturaPeru.total = _factura.monto_plan;
                                    facturaPeru.fecha = DateTime.Parse(_factura.vence);
                                    facturaPeru.uuid = "";
                                    facturaPeru.moneda = facturaPeru.moneda == "DLL" ? facturaPeru.moneda : "MXN";
                                    facturaPeru.ruta_rec_xml = "";
                                    facturaPeru.ruta_rec_pdf = "";
                                    facturaPeru.fecha_autoriza_portal = DateTime.Parse(_factura.vence);
                                    facturaPeru.validacion = "";
                                    facturaPeru.fecha_autoriza_factura = DateTime.Parse(_factura.vence);
                                    facturaPeru.nivel_aut = 1;
                                    facturaPeru.fechaPropuesta = DateTime.Now;
                                    facturaPeru.programo = vSesiones.sesionUsuarioDTO.nombre + " " + vSesiones.sesionUsuarioDTO.apellidoPaterno + " " + vSesiones.sesionUsuarioDTO.apellidoMaterno;
                                }
                            }

                            _context.tblC_sp_gastos_prov.AddRange(listaPeru);
                            SaveChanges();
                            #endregion

                            #endregion
                            trans.Commit();
                            dbSigoplanTransaction.Commit();
                            result.Add(SUCCESS, true);
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            dbSigoplanTransaction.Rollback();

                            LogError(11, 0, "PropuestaController", "guardarGastosProv", e, AccionEnum.AGREGAR, 0, new { lst = lst, manual = manual });

                            result.Add(MESSAGE, e.Message);
                            result.Add(SUCCESS, false);
                        }
                    }
                }
            }

            return result;
        }

        private EnkontrolEnum getEnkontrolEnumADM()
        {
            var baseDatos = new EnkontrolEnum();

            if (_productivo)
            {
                if (vSesiones.sesionEmpresaActual == 1)
                {
                    baseDatos = EnkontrolEnum.CplanProd;
                }
                else if (vSesiones.sesionEmpresaActual == 2)
                {
                    baseDatos = EnkontrolEnum.ArrenProd;
                }
                else if (vSesiones.sesionEmpresaActual == 3) 
                {
                    baseDatos = EnkontrolEnum.ColombiaProductivo;
                }
                else
                {
                    throw new Exception("Empresa distinta a Construplan, Arrendadora o Colombia");
                }
            }
            else
            {
                if (vSesiones.sesionEmpresaActual == 1)
                {
                    baseDatos = EnkontrolEnum.PruebaCplanProd;
                }
                else if (vSesiones.sesionEmpresaActual == 2)
                {
                    baseDatos = EnkontrolEnum.PruebaArrenADM;
                }
                else
                {
                    throw new Exception("Empresa distinta a Construplan y Arrendadora");
                }
            }

            return baseDatos;
        }

        private OdbcConnection checkConexionProductivo()
        {
            if (_productivo)
            {
                return new Conexion().Connect();
            }
            else
            {
                return new Conexion().ConnectPrueba();
            }
        }

        public bool guardarGastosProv_ActivoFijo(List<tblC_sp_gastos_prov> lst, bool manual)
        {
            var esGuardado = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                var lstEnk = (manual ? getLstGastosProvManual(lst) : getLstGastosProv(lst));
                var lstSigoplan = getLstGastosProvSigoplan(lst);
                foreach (var nuevo in lst)
                {
                    var objEnk = lstEnk
                        .Where(ek => ek.cc.Equals(nuevo.cc))
                        .Where(ek => ek.numpro == ek.numpro)
                        .FirstOrDefault(ek => ek.factura.Equals(nuevo.factura));
                    if (objEnk != null)
                    {
                        objEnk.idGiro = nuevo.idGiro;
                        objEnk.estatus = nuevo.estatus;
                        objEnk.idSigoplan = lstSigoplan.Any(w => objEnk.id == w.id) ? lstSigoplan.FirstOrDefault(w => objEnk.id.Equals(w.id)).idSigoplan : 0;
                        objEnk.fechaPropuesta = DateTime.Now;
                        objEnk.programo = vSesiones.sesionUsuarioDTO.nombre;
                        var guardoMov = updateMovProvAuth(objEnk);
                        if (guardoMov == 1)
                        {
                            try
                            {
                                #region Descuenta_Notas_Credito
                                var odbcS = new OdbcConsultaDTO()
                                {
                                    consulta = "select total from " + vSesiones.sesionEmpresaDBPregijo + @"sp_movprov where numpro= ? and factura= ? and es_factura='N' and tm in (26,51) and autorizapago not in ('A','E')",
                                    parametros = new List<OdbcParameterDTO>()
                                };
                                odbcS.parametros.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.Numeric, valor = objEnk.numpro });
                                odbcS.parametros.Add(new OdbcParameterDTO() { nombre = "factura", tipo = OdbcType.VarChar, valor = objEnk.factura });

                                var data = _contextEnkontrol.Select<tblC_sp_gastos_prov>(_productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, odbcS);
                                var resta = data.ToList().Sum(x => Math.Abs(x.total));
                                var total = objEnk.total - resta;
                                objEnk.total = total;
                                objEnk.total_xml = total;
                                objEnk.monto_plan = nuevo.monto_plan;
                            }
                            catch (Exception e2) { }
                                #endregion
                            _context.tblC_sp_gastos_prov.AddOrUpdate(objEnk);
                            SaveChanges();
                        }
                    }
                }
                esGuardado = true;
                dbTransaction.Commit();
            }
            return esGuardado;
        }
        #endregion
        public List<tblC_sp_gastos_prov> getLstGastosProv(BusqConcentradoDTO busq)
        {
            return _context.tblC_sp_gastos_prov.ToList()
                .Where(s => s.fecha >= busq.min && s.fecha <= busq.max).ToList();

        }
        public List<tblC_sp_gastos_prov> getLstGastosProv(BusqPropEkDTO busq)
        {
            var minimo = Int32.Parse(busq.min);
            var maximo = Int32.Parse(busq.max);
            if (vSesiones.sesionEmpresaActual == 1)
            {
                
                return _context.tblC_sp_gastos_prov.ToList()
                    .Where(s => busq.lstCc.Any(c => c.Equals(s.cc)))
                    .Where(s => minimo >= s.numpro && maximo <= s.numpro).ToList();
            }
            else
            {
                return _context.tblC_sp_gastos_prov.ToList()
                    .Where(s => minimo >= s.numpro && maximo <= s.numpro).ToList();
            }
        }
        public List<tblC_sp_gastos_prov> getLstGastosProvSigoplan(List<tblC_sp_gastos_prov> lst)
        {
            return _context.tblC_sp_gastos_prov.ToList().Where(s => lst.Any(a => a.cc.Equals(s.cc) && a.numpro.Equals(s.numpro) && a.factura.Equals(s.factura))).ToList();
        }
        public List<tblC_sp_gastos_prov> getLstFacturasProv(BusqPropEkDTO busq)
        {
            try
            {
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Colombia ? queryLstFacturasProvColombia(busq) :  queryLstFacturasProv(busq),
                    parametros = LstFacturasProv(busq)
                };
                var lst = _contextEnkontrol.Select<tblC_sp_gastos_prov>(getEnkontrolEnumADM(), odbc);
                var lstAF = _context.tblC_sp_gastos_prov_activofijo.ToList();
                lst = lst.Where(x => !lstAF.Any(y => y.cc == x.cc && y.numpro == x.numpro && y.factura == x.factura)).ToList();
                return lst.Where(p => busq.tipoProceso.Equals(3) ? true : busq.tipoProceso.Equals(2) ? (p.estatus.Equals("A") || p.estatus.Equals("E")) : (p.estatus.Equals(" ") || p.estatus.Equals("B") || p.estatus.Equals("P"))).ToList();
            }
            catch (Exception o_O) { return new List<tblC_sp_gastos_prov>(); }
        }
        public List<tblC_sp_gastos_prov_activofijo> getLstFacturasProv_activofijo(BusqPropEkDTO busq)
        {
            try
            {

                var lst = new List<tblC_sp_gastos_prov_activofijo>();
                lst = _context.tblC_sp_gastos_prov_activofijo.ToList().Where(x => !_context.tblC_sp_gastos_prov.Any(y => y.cc == x.cc && y.numpro == x.numpro && y.factura == x.factura)).ToList();

                return lst;
            }
            catch (Exception o_O) { return new List<tblC_sp_gastos_prov_activofijo>(); }
        }
        string queryLstFacturasProv(BusqPropEkDTO busq)
        {
            string query = @"
                SELECT
                    x.*,
                    ";
            if (vSesiones.sesionEmpresaActual == 2)
            {
                query += @"(select case when count(*) > 0 then 1 else 0 end from " + vSesiones.sesionEmpresaDBPregijo + @"so_orden_Compra_det comp where comp.cc=x.cc and comp.numero=x.referenciaoc and substring(comp.insumo,0,3) in ('701','702','703')) as activo_fijo";
            }
            else
            {
                query += @"0 as activo_fijo";
            }

            query += @" FROM (
                    SELECT
                        mov.numpro, mov.factura, mov.cc, MIN(mov.tm) AS tm, MIN(mov.fechavenc) AS fecha, MAX(mov.referenciaoc) AS referenciaoc, MAX(mov.concepto) AS concepto,
                        SUM(mov.total) AS total, mov.autorizapago AS estatus, mov.tipocambio, MIN(CAST(gas.fecha_timbrado AS DATE)) AS fechaTimbrado, MOV.fecha as fechaValidacion
                    FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_movprov mov ";

            if (!busq.tipo)
            {
                query += @" INNER JOIN " + vSesiones.sesionEmpresaDBPregijo + @"sp_gastos_prov gas ON (mov.numpro = gas.numpro AND mov.cc = gas.cc AND mov.factura = gas.factura AND gas.cerrado = 1) ";
            }
            else
            {
                query += @" INNER JOIN " + vSesiones.sesionEmpresaDBPregijo + @"sp_gastos_prov gas ON (mov.numpro = gas.numpro AND mov.cc = gas.cc AND mov.factura = gas.factura) ";
            }

            query += @"
                    WHERE (
                        (mov.autorizapago NOT IN ('A','E') AND mov.es_factura = 'S' AND mov.fechavenc <= ?) OR
                        (mov.es_factura='N' AND mov.autorizapago IN ('A','E'))
                    ) AND mov.numpro BETWEEN ? AND ? ";

            //            if (busq.tipo)
            //            {
            //                query += @" AND NOT EXISTS (
            //                    SELECT
            //                        *
            //                    FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_gastos_prov gas
            //                    WHERE (mov.numpro = gas.numpro AND mov.cc = gas.cc AND mov.factura = gas.factura)
            //                ) ";
            //            }

            if (vSesiones.sesionEmpresaActual != 2)
            {
                query += @" AND mov.cc IN {0} ";
            }

            query += @" AND mov.factura not in (select factura FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_movprov where (tm=51 or tm=26) and numpro=mov.numpro and cc=mov.cc group by factura having sum(abs(total))=mov.total)
                                   -- AND bit_factoraje='N'
                                group by mov.numpro ,mov.factura ,mov.cc ,mov.autorizapago, mov.tipocambio,mov.fecha order by mov.numpro,mov.factura) x where (x.total * x.tipocambio)>1";

            if (vSesiones.sesionEmpresaActual != 2)
            {
                return string.Format(query, busq.lstCc.ToParamInValue());
            }
            else
            {
                return string.Format(query);
            }
        }
        List<OdbcParameterDTO> LstFacturasProv(BusqPropEkDTO busq)
        {
            var param = new List<OdbcParameterDTO>();
            param.Add(new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.Date, valor = busq.fechaCorte });
            param.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.VarChar, valor = busq.min });
            param.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.VarChar, valor = busq.max });
            if (vSesiones.sesionEmpresaActual != 2)
            {
                param.AddRange(busq.lstCc.Select(s => new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = s }));
            }

            return param;
        }
        public List<tblC_sp_gastos_prov> getLstFacturasSaldosMenores(BusqPropEkDTO busq)
        {
            try
            {
                List<tblC_sp_gastos_prov> lst = new List<tblC_sp_gastos_prov>();

                var odbc = new OdbcConsultaDTO()
                {
                    consulta = queryLstFacturasSaldosMenores(busq),
                    parametros = LstFacturasSaldosMenores(busq)
                };
                lst = _contextEnkontrol.Select<tblC_sp_gastos_prov>(EnkontrolAmbienteEnum.Prod, odbc);

                return lst;
            }
            catch (Exception o_O) { return new List<tblC_sp_gastos_prov>(); }
        }
        string queryLstFacturasSaldosMenores(BusqPropEkDTO busq)
        {
            string query = @"";
            //Si es para complementaria
            if (busq.tipo)
            {
                query = @"
                SELECT
                      *,
                    (
                        SELECT 
                            TOP 1 STRING(area, '-', cuenta) AS ac 
                        FROM " + vSesiones.sesionEmpresaDBPregijo + @"si_area_cuenta 
                        WHERE centro_costo = x.cc
                    ) AS ac, 
                    (
                        SELECT 
                            TOP 1 descripcion 
                        FROM " + vSesiones.sesionEmpresaDBPregijo + @"si_area_cuenta 
                        WHERE centro_costo = x.cc
                    ) AS acDesc
                    FROM (
                         SELECT
                               mov.numpro, mov.factura, mov.cc, MIN(mov.tm) AS tm, MIN(mov.fechavenc) AS fecha, MAX(mov.referenciaoc) AS referenciaoc, MIN(mov.concepto) AS concepto, (select top 1 w.tipocambio from " + vSesiones.sesionEmpresaDBPregijo + @"sp_movprov w where w.numpro=mov.numpro and w.factura=mov.factura and w.es_factura='S' ) AS tipocambio, 
                               SUM(mov.total) AS total,cast(ROUND(SUM(mov.total*mov.tipocambio),2) as decimal(18,2)) as totalMN
                         FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_movprov mov 
                         WHERE (
                                mov.numpro BETWEEN ? AND ?
                          )
                         group by mov.numpro ,mov.factura ,mov.cc order by mov.numpro,mov.factura
                    ) x where x.total = 0 and x.totalMN != 0
                ";
            }
            else
            {
                query = @"
                SELECT
                      *,
                    (
                        SELECT 
                            TOP 1 STRING(area, '-', cuenta) AS ac 
                        FROM " + vSesiones.sesionEmpresaDBPregijo + @"si_area_cuenta 
                        WHERE centro_costo = x.cc
                    ) AS ac, 
                    (
                        SELECT 
                            TOP 1 descripcion 
                        FROM " + vSesiones.sesionEmpresaDBPregijo + @"si_area_cuenta 
                        WHERE centro_costo = x.cc
                    ) AS acDesc
                    FROM (
                         SELECT
                               mov.numpro, mov.factura, mov.cc, MIN(mov.tm) AS tm, MIN(mov.fechavenc) AS fecha, MAX(mov.referenciaoc) AS referenciaoc, MIN(mov.concepto) AS concepto,
                               SUM(mov.total) AS total,cast(ROUND(SUM(mov.total*mov.tipocambio),2) as decimal(18,2)) as totalMN
                         FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_movprov mov 
                         WHERE (
                                mov.numpro BETWEEN ? AND ?
                          )
                         group by mov.numpro ,mov.factura ,mov.cc order by mov.numpro,mov.factura
                    ) x where (x.total > 0 and x.total < 1 )
                ";
            }
            return string.Format(query);
        }
        List<OdbcParameterDTO> LstFacturasSaldosMenores(BusqPropEkDTO busq)
        {
            var param = new List<OdbcParameterDTO>();
            param.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.VarChar, valor = busq.min });
            param.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.VarChar, valor = busq.max });

            return param;
        }
        List<tblC_sp_gastos_prov> getLstGastosProv(List<tblC_sp_gastos_prov> lst)
        {
            try
            {
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = queryLstGastosProv(lst),
                    parametros = paramLstGastosProv(lst)
                };
                var lstRees = _contextEnkontrol.Select<tblC_sp_gastos_prov>(_productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, odbc);
                return lstRees.ToList();
            }
            catch (Exception o_O) { return new List<tblC_sp_gastos_prov>(); }
        }
        List<tblC_sp_gastos_prov> getLstGastosProvManual(List<tblC_sp_gastos_prov> lst)
        {
            try
            {
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = queryLstGastosProvManual(lst),
                    parametros = paramLstGastosProv(lst)
                };
                var lstRees = _contextEnkontrol.Select<tblC_sp_gastos_prov>(_productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, odbc);

                foreach (var r in lstRees)
                {
                    r.manual = true;
                }

                return lstRees.ToList();
            }
            catch (Exception o_O) { return new List<tblC_sp_gastos_prov>(); }
        }
        string queryLstGastosProv(List<tblC_sp_gastos_prov> lst)
        {
            return string.Format(@"SELECT * FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_gastos_prov 
                                    WHERE cc IN {0}
                                    AND numpro IN {1}
                                    AND factura IN {2} "
                , lst.GroupBy(g => g.cc).Select(s => s.Key).ToList().ToParamInValue()
                , lst.GroupBy(g => g.numpro).Select(s => s.Key.ToString()).ToList().ToParamInValue()
                , lst.GroupBy(g => g.factura).Select(s => s.Key).ToList().ToParamInValue());
        }
        string queryLstGastosProvManual(List<tblC_sp_gastos_prov> lst)
        {
            return string.Format(@"SELECT * FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_movprov 
                                    WHERE es_factura='S' 
                                    AND cc IN {0}
                                    AND numpro IN {1}
                                    AND factura IN {2} "
                , lst.GroupBy(g => g.cc).Select(s => s.Key).ToList().ToParamInValue()
                , lst.GroupBy(g => g.numpro).Select(s => s.Key.ToString()).ToList().ToParamInValue()
                , lst.GroupBy(g => g.factura).Select(s => s.Key).ToList().ToParamInValue());
        }
        List<OdbcParameterDTO> paramLstGastosProv(List<tblC_sp_gastos_prov> lst)
        {
            var res = lst.GroupBy(g => g.cc).Select(s => new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = s.Key }).ToList();
            res.AddRange(lst.GroupBy(g => g.numpro).Select(s => new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.Numeric, valor = s.Key.ToString() }).ToList());
            res.AddRange(lst.GroupBy(g => g.factura).Select(s => new OdbcParameterDTO() { nombre = "factura", tipo = OdbcType.VarChar, valor = s.Key }).ToList());
            return res;
        }
        int updateGastosProv(tblC_sp_gastos_prov guarda)
        {
            var guardado = 0;
            var odbcS = new OdbcConsultaDTO()
            {
                consulta = "select * from " + vSesiones.sesionEmpresaDBPregijo + @"sp_gastos_prov where numpro= ? and factura= ?",
                parametros = new List<OdbcParameterDTO>()
            };
            odbcS.parametros.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.Numeric, valor = guarda.numpro });
            odbcS.parametros.Add(new OdbcParameterDTO() { nombre = "factura", tipo = OdbcType.VarChar, valor = guarda.factura });

            var data = _contextEnkontrol.Select<tblC_sp_gastos_prov>(EnkontrolAmbienteEnum.Prod, odbcS);

            if (data.Count > 0)
            {
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = "UPDATE " + vSesiones.sesionEmpresaDBPregijo + @"sp_gastos_prov SET estatus = ? WHERE numpro = ? and factura= ?",
                    parametros = new List<OdbcParameterDTO>()
                };
                odbc.parametros.Add(new OdbcParameterDTO() { nombre = "estatus", tipo = OdbcType.VarChar, valor = guarda.estatus });
                odbc.parametros.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.Numeric, valor = guarda.numpro });
                odbc.parametros.Add(new OdbcParameterDTO() { nombre = "factura", tipo = OdbcType.VarChar, valor = guarda.factura });
                _contextEnkontrol.Save(EnkontrolAmbienteEnum.Prod, odbc);
                guardado = 0;
            }
            else
            {
                guardado = 1;
            }
            return guardado;
        }
        int updateMovProvAuth(tblC_sp_gastos_prov guarda)
        {
            var guardado = 1;
            //var cveEmp = _context.tblP_Usuario_Enkontrol.ToList().FirstOrDefault(u => u.idUsuario.Equals(vSesiones.sesionUsuarioDTO.id)).empleado;
            var cveEmp = 1;
            var odbc = new OdbcConsultaDTO()
            {
                consulta = @"UPDATE " + vSesiones.sesionEmpresaDBPregijo + @"sp_movprov 
                            SET autorizapago = ? 
                           ,empleado_vobo = ?
                           ,fecha_vobo = ?
                        WHERE es_factura='S' AND numpro = ? AND factura = ? AND cc = ? ",
                parametros = new List<OdbcParameterDTO>()
            };
            odbc.parametros.Add(new OdbcParameterDTO() { nombre = "autorizapago", tipo = OdbcType.VarChar, valor = guarda.estatus.Equals("P") ? string.Empty : guarda.estatus });
            odbc.parametros.Add(new OdbcParameterDTO() { nombre = "empleado_vobo", tipo = OdbcType.Numeric, valor = cveEmp });
            odbc.parametros.Add(new OdbcParameterDTO() { nombre = "fecha_vobo", tipo = OdbcType.Date, valor = DateTime.Now });
            odbc.parametros.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.VarChar, valor = guarda.numpro });
            odbc.parametros.Add(new OdbcParameterDTO() { nombre = "factura", tipo = OdbcType.VarChar, valor = guarda.factura });
            odbc.parametros.Add(new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = guarda.cc });
            guardado = _contextEnkontrol.Save(_productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, odbc);
            return guardado;
        }

        public bool GuardarMontosProgrPagos(List<MontoPropPagoDTO> lst, DateTime pago)
        {
            using (var context = new MainContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Peru)
                        {
                            var lstEk = new List<int>();
                            lst.ForEach(item =>
                            {
                                lstEk.Add(saveGeneraMovprov(item, pago));
                            });
                            if (lstEk.Any(i => i == 1))
                            {
                                context.SaveChanges();
                                dbContextTransaction.Commit();
                                return true;
                            }
                        }
                        else
                        {
                            foreach (var item in lst)
                            {
                                var obj = context.tblC_sp_gastos_prov.FirstOrDefault(x => x.numpro == item.numpro && x.factura.Equals(item.factura.ToString()));
                                obj.fechaAutorizacion = DateTime.Now;
                                obj.autorizada = true;
                                obj.monto_plan = obj.monto_plan;
                                obj.autorizo = vSesiones.sesionUsuarioDTO.nombre;                                
                            }
                            context.SaveChanges();
                            dbContextTransaction.Commit();
                            return true;
                        }
                        dbContextTransaction.Commit();
                        return false;
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        return false;
                    }
                }
            }
        }
        #region Saldos_Menores
        public int SaldosMenores_GenerarPolizas(DateTime pago, string moneda, decimal total, bool tipo)
        {
            int poliza = 0;
            int polizaID = getNextFolioPolizaID(pago);
            try
            {
                var obj1 = getPolizaToSave(new MontoPropPagoDTO(), pago, polizaID, moneda, total);
                _contextEnkontrol.Save(EnkontrolAmbienteEnum.Prod, obj1);
                poliza = polizaID;
            }
            catch (Exception e)
            {
                poliza = 0;
            }
            return poliza;
        }
        public string SaldosMenores_GenerarPolizas_det(List<MontoPropPagoDTO> lst, DateTime pago, int polizaID, bool tipo)
        {
            string poliza = "";
            bool result = true;
            try
            {
                int lineas = 1;
                foreach (var x in lst)
                {
                    var odbcDet = new List<OdbcConsultaDTO>();
                    if (tipo)
                    {
                        odbcDet.Add(getMovProv_ToUpdate(x));
                        _contextEnkontrol.Update(EnkontrolAmbienteEnum.Prod, odbcDet);
                    }
                    else
                    {
                        odbcDet.Add(getPoliza_Det_ToSave(x, pago, polizaID, 1, lineas++));
                        odbcDet.Add(getPoliza_Det_ToSave(x, pago, polizaID, 2, lineas++));
                        if (x.moneda != "MN")
                        {
                            odbcDet.Add(getPoliza_Det_ToSave(x, pago, polizaID, 3, lineas++));
                        }
                        odbcDet.Add(getMovProv_ToSave(x, pago, polizaID));
                        _contextEnkontrol.Save(EnkontrolAmbienteEnum.Prod, odbcDet);
                    }

                }
                if (tipo)
                {
                    poliza = "tipocambio";
                }
                else
                {
                    poliza = pago.Year + "-" + pago.Month + "-" + polizaID + "-03";
                }

            }
            catch (Exception e)
            {
                poliza = "false";
            }
            return poliza;
        }
        int getNextFolioPolizaID(DateTime pago)
        {
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"SELECT max(poliza) as id_incidencia FROM sc_polizas where year=" + pago.Year + @" and mes=" + pago.Month + @" and tp='03'";
            var res = _contextEnkontrol.Select<IncidenciaEmpDTO>(EnkontrolAmbienteEnum.Prod, odbc);
            return res.FirstOrDefault().id_incidencia + 1;
        }
        int getAutoIncrementMovProv()
        {
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"SELECT max(autoincremento) as id_incidencia FROM sp_movprov";
            var res = _contextEnkontrol.Select<IncidenciaEmpDTO>(EnkontrolAmbienteEnum.Prod, odbc);
            return res.FirstOrDefault().id_incidencia + 1;
        }
        OdbcConsultaDTO getPolizaToSave(MontoPropPagoDTO obj, DateTime pago, int polizaID, string moneda, decimal total)
        {
            var cargo = moneda.Equals("MN") ? 0 : total;
            var abono = moneda.Equals("MN") ? 0 : -1 * total;
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"INSERT INTO sc_polizas
                                (
                                year ,
                                mes , 
                                poliza ,
                                tp ,
                                fechapol ,
                                cargos ,
                                abonos ,
                                generada ,
                                status ,
                                status_lock ,
                                fec_hora_movto ,
                                fecha_hora_crea ,
                                usuario_crea ,
                                status_carga_pol,
                                concepto    
                                )
                VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
            odbc.parametros = new List<OdbcParameterDTO>() 
            {
                new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Int, valor = pago.Year },
                new OdbcParameterDTO() { nombre = "mes", tipo = OdbcType.Int, valor = pago.Month },
                new OdbcParameterDTO() { nombre = "poliza", tipo = OdbcType.Int, valor = polizaID },
                new OdbcParameterDTO() { nombre = "tp", tipo = OdbcType.VarChar, valor = "03" },
                new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.DateTime, valor = pago },
                new OdbcParameterDTO() { nombre = "cargos", tipo = OdbcType.Decimal, valor = cargo },
                new OdbcParameterDTO() { nombre = "abonos", tipo = OdbcType.Decimal, valor = abono },
                new OdbcParameterDTO() { nombre = "generada", tipo = OdbcType.VarChar, valor = "C" },
                new OdbcParameterDTO() { nombre = "status", tipo = OdbcType.VarChar, valor = "A"},
                new OdbcParameterDTO() { nombre = "status_lock", tipo = OdbcType.VarChar, valor = "N" },
                new OdbcParameterDTO() { nombre = "fec_hora_movto", tipo = OdbcType.DateTime, valor = DateTime.Now },
                new OdbcParameterDTO() { nombre = "fecha_hora_crea", tipo = OdbcType.DateTime, valor = DateTime.Now },
                new OdbcParameterDTO() { nombre = "usuario_crea", tipo = OdbcType.VarChar, valor = "1" },
                new OdbcParameterDTO() { nombre = "status_carga_pol", tipo = OdbcType.VarChar, valor = "" },
                new OdbcParameterDTO() { nombre = "concepto", tipo = OdbcType.VarChar, valor = "Poliza de DIARIO" }
            };
            return odbc;
        }
        OdbcConsultaDTO getPoliza_Det_ToSave(MontoPropPagoDTO obj, DateTime pago, int polizaID, int tipo, int linea)
        {
            int cta = 0;
            int scta = 0;
            if (obj.moneda == "MN")
            {
                if (tipo == 1)
                {
                    cta = 2105;
                    scta = 1;
                }
                else if (tipo == 2)
                {
                    cta = 4901;
                    scta = 2;
                }
            }
            else
            {
                if (tipo == 1)
                {
                    cta = 2105;
                    scta = 2;
                }
                else if (tipo == 2)
                {
                    cta = 2105;
                    scta = 200;
                }
                else if (tipo == 3)
                {
                    cta = 4901;
                    scta = 2;
                }
            }
            var odbc = new OdbcConsultaDTO();
            if (obj.moneda == "MN")
            {
                if (tipo == 1)
                {
                    odbc.consulta = @"INSERT INTO sc_movpol
                                (
                                year ,
                                mes , 
                                poliza ,
                                tp ,
                                linea ,
                                cta ,
                                scta ,
                                sscta ,
                                digito ,
                                tm ,
                                referencia ,
                                cc ,
                                concepto ,
                                monto ,
                                iclave ,
                                itm ,
                                st_par ,
                                orden_compra ,
                                numpro,
                                cfd_ruta_pdf ,
                                cfd_ruta_xml
                                )
                VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
                    odbc.parametros = new List<OdbcParameterDTO>() 
                    {
                        new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Int, valor = pago.Year },
                        new OdbcParameterDTO() { nombre = "mes", tipo = OdbcType.Int, valor = pago.Month },
                        new OdbcParameterDTO() { nombre = "poliza", tipo = OdbcType.Int, valor = polizaID },
                        new OdbcParameterDTO() { nombre = "tp", tipo = OdbcType.VarChar, valor = "03" },
                        new OdbcParameterDTO() { nombre = "linea", tipo = OdbcType.Int, valor = linea },
                        new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Int, valor = cta },
                        new OdbcParameterDTO() { nombre = "scta", tipo = OdbcType.Int, valor = scta },
                        new OdbcParameterDTO() { nombre = "sscta", tipo = OdbcType.Int, valor = 0 },
                        new OdbcParameterDTO() { nombre = "digito", tipo = OdbcType.Int, valor = 0 },
                        new OdbcParameterDTO() { nombre = "tm", tipo = OdbcType.Int, valor = 4 },
                        new OdbcParameterDTO() { nombre = "referencia", tipo = OdbcType.VarChar, valor = obj.factura },
                        new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = obj.cc },
                        new OdbcParameterDTO() { nombre = "concepto", tipo = OdbcType.VarChar, valor = ("PD-"+polizaID+" AJUSTE DE SALDOS") },
                        new OdbcParameterDTO() { nombre = "monto", tipo = OdbcType.Decimal, valor = obj.total },
                        new OdbcParameterDTO() { nombre = "iclave", tipo = OdbcType.Int, valor = 0 },
                        new OdbcParameterDTO() { nombre = "itm", tipo = OdbcType.Int, valor = 51 },
                        new OdbcParameterDTO() { nombre = "st_par", tipo = OdbcType.VarChar, valor = "" },
                        new OdbcParameterDTO() { nombre = "orden_compra", tipo = OdbcType.Int, valor = int.Parse(obj.referenciaoc) },
                        new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.Int, valor = obj.numpro },
                        new OdbcParameterDTO() { nombre = "cfd_ruta_pdf", tipo = OdbcType.VarChar, valor = "" },
                        new OdbcParameterDTO() { nombre = "cfd_ruta_xml", tipo = OdbcType.VarChar, valor = "" }
                    };
                }
                else if (tipo == 2)
                {
                    odbc.consulta = @"INSERT INTO sc_movpol
                                (
                                year ,
                                mes , 
                                poliza ,
                                tp ,
                                linea ,
                                cta ,
                                scta ,
                                sscta ,
                                digito ,
                                tm ,
                                referencia ,
                                cc ,
                                concepto ,
                                monto ,
                                iclave ,
                                itm ,
                                st_par ,
                                orden_compra ,
                                cfd_ruta_pdf ,
                                cfd_ruta_xml
                                )
                VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
                    odbc.parametros = new List<OdbcParameterDTO>() 
                    {
                        new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Int, valor = pago.Year },
                        new OdbcParameterDTO() { nombre = "mes", tipo = OdbcType.Int, valor = pago.Month },
                        new OdbcParameterDTO() { nombre = "poliza", tipo = OdbcType.Int, valor = polizaID },
                        new OdbcParameterDTO() { nombre = "tp", tipo = OdbcType.VarChar, valor = "03" },
                        new OdbcParameterDTO() { nombre = "linea", tipo = OdbcType.Int, valor = linea },
                        new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Int, valor = cta },
                        new OdbcParameterDTO() { nombre = "scta", tipo = OdbcType.Int, valor = scta },
                        new OdbcParameterDTO() { nombre = "sscta", tipo = OdbcType.Int, valor = 0 },
                        new OdbcParameterDTO() { nombre = "digito", tipo = OdbcType.Int, valor = 0 },
                        new OdbcParameterDTO() { nombre = "tm", tipo = OdbcType.Int, valor = 2 },
                        new OdbcParameterDTO() { nombre = "referencia", tipo = OdbcType.VarChar, valor = obj.factura },
                        new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = obj.cc },
                        new OdbcParameterDTO() { nombre = "concepto", tipo = OdbcType.VarChar, valor = ("PD-"+polizaID+" AJUSTE DE SALDOS") },
                        new OdbcParameterDTO() { nombre = "monto", tipo = OdbcType.Decimal, valor = (-1*obj.total) },
                        new OdbcParameterDTO() { nombre = "iclave", tipo = OdbcType.Int, valor = 0 },
                        new OdbcParameterDTO() { nombre = "itm", tipo = OdbcType.Int, valor = 0 },
                        new OdbcParameterDTO() { nombre = "st_par", tipo = OdbcType.VarChar, valor = "" },
                        new OdbcParameterDTO() { nombre = "orden_compra", tipo = OdbcType.Int, valor = 0 },
                        new OdbcParameterDTO() { nombre = "cfd_ruta_pdf", tipo = OdbcType.VarChar, valor = "" },
                        new OdbcParameterDTO() { nombre = "cfd_ruta_xml", tipo = OdbcType.VarChar, valor = "" }
                    };
                }
            }
            else
            {
                if (tipo == 1)
                {
                    odbc.consulta = @"INSERT INTO sc_movpol
                                (
                                year ,
                                mes , 
                                poliza ,
                                tp ,
                                linea ,
                                cta ,
                                scta ,
                                sscta ,
                                digito ,
                                tm ,
                                referencia ,
                                cc ,
                                concepto ,
                                monto ,
                                iclave ,
                                itm ,
                                st_par ,
                                numpro,
                                cfd_ruta_pdf ,
                                cfd_ruta_xml
                                )
                VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
                    odbc.parametros = new List<OdbcParameterDTO>() 
                    {
                        new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Int, valor = pago.Year },
                        new OdbcParameterDTO() { nombre = "mes", tipo = OdbcType.Int, valor = pago.Month },
                        new OdbcParameterDTO() { nombre = "poliza", tipo = OdbcType.Int, valor = polizaID },
                        new OdbcParameterDTO() { nombre = "tp", tipo = OdbcType.VarChar, valor = "03" },
                        new OdbcParameterDTO() { nombre = "linea", tipo = OdbcType.Int, valor = linea },
                        new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Int, valor = cta },
                        new OdbcParameterDTO() { nombre = "scta", tipo = OdbcType.Int, valor = scta },
                        new OdbcParameterDTO() { nombre = "sscta", tipo = OdbcType.Int, valor = 0 },
                        new OdbcParameterDTO() { nombre = "digito", tipo = OdbcType.Int, valor = 0 },
                        new OdbcParameterDTO() { nombre = "tm", tipo = OdbcType.Int, valor = 1 },
                        new OdbcParameterDTO() { nombre = "referencia", tipo = OdbcType.VarChar, valor = obj.factura },
                        new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = obj.cc },
                        new OdbcParameterDTO() { nombre = "concepto", tipo = OdbcType.VarChar, valor = ("PD-"+polizaID+" AJUSTE DE SALDOS") },
                        new OdbcParameterDTO() { nombre = "monto", tipo = OdbcType.Decimal, valor = obj.total },
                        new OdbcParameterDTO() { nombre = "iclave", tipo = OdbcType.Int, valor = 0 },
                        new OdbcParameterDTO() { nombre = "itm", tipo = OdbcType.Int, valor = 51 },
                        new OdbcParameterDTO() { nombre = "st_par", tipo = OdbcType.VarChar, valor = "" },
                        new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.Int, valor = obj.numpro },
                        new OdbcParameterDTO() { nombre = "cfd_ruta_pdf", tipo = OdbcType.VarChar, valor = "" },
                        new OdbcParameterDTO() { nombre = "cfd_ruta_xml", tipo = OdbcType.VarChar, valor = "" }
                    };
                }
                else if (tipo == 2)
                {
                    odbc.consulta = @"INSERT INTO sc_movpol
                                (
                                year ,
                                mes , 
                                poliza ,
                                tp ,
                                linea ,
                                cta ,
                                scta ,
                                sscta ,
                                digito ,
                                tm ,
                                referencia ,
                                cc ,
                                concepto ,
                                monto ,
                                iclave ,
                                itm ,
                                st_par ,
                                numpro,
                                cfd_ruta_pdf ,
                                cfd_ruta_xml
                                )
                VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
                    odbc.parametros = new List<OdbcParameterDTO>() 
                    {
                        new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Int, valor = pago.Year },
                        new OdbcParameterDTO() { nombre = "mes", tipo = OdbcType.Int, valor = pago.Month },
                        new OdbcParameterDTO() { nombre = "poliza", tipo = OdbcType.Int, valor = polizaID },
                        new OdbcParameterDTO() { nombre = "tp", tipo = OdbcType.VarChar, valor = "03" },
                        new OdbcParameterDTO() { nombre = "linea", tipo = OdbcType.Int, valor = linea },
                        new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Int, valor = cta },
                        new OdbcParameterDTO() { nombre = "scta", tipo = OdbcType.Int, valor = scta },
                        new OdbcParameterDTO() { nombre = "sscta", tipo = OdbcType.Int, valor = 0 },
                        new OdbcParameterDTO() { nombre = "digito", tipo = OdbcType.Int, valor = 0 },
                        new OdbcParameterDTO() { nombre = "tm", tipo = OdbcType.Int, valor = 1 },
                        new OdbcParameterDTO() { nombre = "referencia", tipo = OdbcType.VarChar, valor = obj.factura },
                        new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = obj.cc },
                        new OdbcParameterDTO() { nombre = "concepto", tipo = OdbcType.VarChar, valor = ("PD-"+polizaID+" AJUSTE DE SALDOS") },
                        new OdbcParameterDTO() { nombre = "monto", tipo = OdbcType.Decimal, valor = obj.totalMN },
                        new OdbcParameterDTO() { nombre = "iclave", tipo = OdbcType.Int, valor = 0 },
                        new OdbcParameterDTO() { nombre = "itm", tipo = OdbcType.Int, valor = 51 },
                        new OdbcParameterDTO() { nombre = "st_par", tipo = OdbcType.VarChar, valor = "" },
                        new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.Int, valor = obj.numpro },
                        new OdbcParameterDTO() { nombre = "cfd_ruta_pdf", tipo = OdbcType.VarChar, valor = "" },
                        new OdbcParameterDTO() { nombre = "cfd_ruta_xml", tipo = OdbcType.VarChar, valor = "" }
                    };
                }
                else if (tipo == 3)
                {
                    odbc.consulta = @"INSERT INTO sc_movpol
                                (
                                year ,
                                mes , 
                                poliza ,
                                tp ,
                                linea ,
                                cta ,
                                scta ,
                                sscta ,
                                digito ,
                                tm ,
                                referencia ,
                                cc ,
                                concepto ,
                                monto ,
                                iclave ,
                                itm ,
                                st_par ,
                                orden_compra ,
                                numpro,
                                cfd_ruta_pdf ,
                                cfd_ruta_xml
                                )
                VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
                    odbc.parametros = new List<OdbcParameterDTO>() 
                    {
                        new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Int, valor = pago.Year },
                        new OdbcParameterDTO() { nombre = "mes", tipo = OdbcType.Int, valor = pago.Month },
                        new OdbcParameterDTO() { nombre = "poliza", tipo = OdbcType.Int, valor = polizaID },
                        new OdbcParameterDTO() { nombre = "tp", tipo = OdbcType.VarChar, valor = "03" },
                        new OdbcParameterDTO() { nombre = "linea", tipo = OdbcType.Int, valor = linea },
                        new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Int, valor = cta },
                        new OdbcParameterDTO() { nombre = "scta", tipo = OdbcType.Int, valor = scta },
                        new OdbcParameterDTO() { nombre = "sscta", tipo = OdbcType.Int, valor = 0 },
                        new OdbcParameterDTO() { nombre = "digito", tipo = OdbcType.Int, valor = 0 },
                        new OdbcParameterDTO() { nombre = "tm", tipo = OdbcType.Int, valor = 2 },
                        new OdbcParameterDTO() { nombre = "referencia", tipo = OdbcType.VarChar, valor = obj.factura },
                        new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = obj.cc },
                        new OdbcParameterDTO() { nombre = "concepto", tipo = OdbcType.VarChar, valor = ("PD-"+polizaID+" AJUSTE DE SALDOS") },
                        new OdbcParameterDTO() { nombre = "monto", tipo = OdbcType.Decimal, valor = (-1*(obj.total + obj.totalMN)) },
                        new OdbcParameterDTO() { nombre = "iclave", tipo = OdbcType.Int, valor = 0 },
                        new OdbcParameterDTO() { nombre = "itm", tipo = OdbcType.Int, valor = 0 },
                        new OdbcParameterDTO() { nombre = "st_par", tipo = OdbcType.VarChar, valor = "" },
                        new OdbcParameterDTO() { nombre = "orden_compra", tipo = OdbcType.Int, valor = 0 },
                        new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.Int, valor = 0 },
                        new OdbcParameterDTO() { nombre = "cfd_ruta_pdf", tipo = OdbcType.VarChar, valor = "" },
                        new OdbcParameterDTO() { nombre = "cfd_ruta_xml", tipo = OdbcType.VarChar, valor = "" }
                    };
                }
            }
            return odbc;
        }
        OdbcConsultaDTO getMovProv_ToSave(MontoPropPagoDTO obj, DateTime pago, int polizaID)
        {
            var autoInc = getAutoIncrementMovProv();
            var odbc = new OdbcConsultaDTO();
            var tc = obj.moneda.Equals("MN") ? obj.tipocambio : obj.totalMN / obj.total;
            odbc.consulta = @"INSERT INTO sp_movprov
                                (
                                numpro ,
                                factura ,
                                fecha ,
                                tm ,
                                fechavenc ,
                                concepto ,
                                cc ,
                                referenciaoc ,
                                monto ,
                                tipocambio ,
                                iva ,
                                year ,
                                mes , 
                                poliza ,
                                tp ,
                                linea ,
                                generado ,
                                es_factura ,
                                moneda ,
                                autorizapago ,
                                total ,
                                autoincremento ,
                                tipocambio_oc ,
                                bit_factoraje ,
                                bit_autoriza ,
                                bit_transferida ,
                                bit_pagada ,
                                cfd_serie ,
                                cfd_folio ,
                                cfd_certificado ,
                                ruta_rec_xml ,
                                ruta_rec_pdf ,
                                afectacompra ,
                                val_ref ,
                                suma_o_resta ,
                                pide_iva ,
                                valida_recibido ,
                                valida_almacen ,
                                valida_recibido_autorizar ,
                                empleado_autorizo ,
                                empleado_vobo ,
                                tipo_factoraje ,
                                UUID
                                )
                VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
            odbc.parametros = new List<OdbcParameterDTO>() 
            {
                new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.Int, valor = obj.numpro },
                new OdbcParameterDTO() { nombre = "factura", tipo = OdbcType.Int, valor = obj.factura },
                new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.DateTime, valor = pago },
                new OdbcParameterDTO() { nombre = "tm", tipo = OdbcType.Int, valor = 26 },
                new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.DateTime, valor = pago },
                new OdbcParameterDTO() { nombre = "concepto", tipo = OdbcType.VarChar, valor = ("PD-"+polizaID+" AJUSTE DE SALDOS") },
                new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = obj.cc },
                new OdbcParameterDTO() { nombre = "referenciaoc", tipo = OdbcType.VarChar, valor = obj.referenciaoc },
                new OdbcParameterDTO() { nombre = "monto", tipo = OdbcType.Decimal, valor = (-1 * (obj.saldo - (obj.saldo * (decimal)0.16))) },
                new OdbcParameterDTO() { nombre = "tipocambio", tipo = OdbcType.Decimal, valor = tc},
                new OdbcParameterDTO() { nombre = "iva", tipo = OdbcType.Decimal, valor = (-1 * (obj.saldo * (decimal)0.16)) },
                new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Int, valor = pago.Year },
                new OdbcParameterDTO() { nombre = "mes ", tipo = OdbcType.Int, valor = pago.Month },
                new OdbcParameterDTO() { nombre = "poliza", tipo = OdbcType.Int, valor = polizaID },
                new OdbcParameterDTO() { nombre = "tp", tipo = OdbcType.VarChar, valor = "03" },
                new OdbcParameterDTO() { nombre = "linea", tipo = OdbcType.Int, valor = 1 },
                new OdbcParameterDTO() { nombre = "generado", tipo = OdbcType.VarChar, valor = "C" },
                new OdbcParameterDTO() { nombre = "es_factura", tipo = OdbcType.VarChar, valor = "N" },
                new OdbcParameterDTO() { nombre = "moneda", tipo = OdbcType.VarChar, valor = (obj.moneda=="MN" ?1:2) },
                new OdbcParameterDTO() { nombre = "autorizapago", tipo = OdbcType.VarChar, valor = "" },
                new OdbcParameterDTO() { nombre = "total", tipo = OdbcType.Decimal, valor = (-1*obj.saldo) },
                new OdbcParameterDTO() { nombre = "autoincremento", tipo = OdbcType.Int, valor = autoInc },
                new OdbcParameterDTO() { nombre = "tipocambio_oc", tipo = OdbcType.Decimal, valor = 1},
                new OdbcParameterDTO() { nombre = "bit_factoraje", tipo = OdbcType.VarChar, valor = "N" },
                new OdbcParameterDTO() { nombre = "bit_autoriza", tipo = OdbcType.VarChar, valor = "N" },
                new OdbcParameterDTO() { nombre = "bit_transferida", tipo = OdbcType.VarChar, valor = "N" },
                new OdbcParameterDTO() { nombre = "bit_pagada", tipo = OdbcType.VarChar, valor = "N" },
                new OdbcParameterDTO() { nombre = "cfd_serie", tipo = OdbcType.VarChar, valor = "" },
                new OdbcParameterDTO() { nombre = "cfd_folio", tipo = OdbcType.Int, valor = obj.factura },
                new OdbcParameterDTO() { nombre = "cfd_certificado", tipo = OdbcType.VarChar, valor = "" },
                new OdbcParameterDTO() { nombre = "ruta_rec_xml", tipo = OdbcType.VarChar, valor = "" },
                new OdbcParameterDTO() { nombre = "ruta_rec_pdf", tipo = OdbcType.VarChar, valor = "" },
                new OdbcParameterDTO() { nombre = "afectacompra", tipo = OdbcType.VarChar, valor = "S" },
                new OdbcParameterDTO() { nombre = "val_ref", tipo = OdbcType.VarChar, valor = "O" },
                new OdbcParameterDTO() { nombre = "suma_o_resta", tipo = OdbcType.Int, valor = 0 },
                new OdbcParameterDTO() { nombre = "pide_iva", tipo = OdbcType.VarChar, valor = "S" },
                new OdbcParameterDTO() { nombre = "valida_recibido", tipo = OdbcType.VarChar, valor = "R" },
                new OdbcParameterDTO() { nombre = "valida_almacen", tipo = OdbcType.VarChar, valor = "S" },
                new OdbcParameterDTO() { nombre = "valida_recibido_autorizar", tipo = OdbcType.VarChar, valor = "S" },
                new OdbcParameterDTO() { nombre = "empleado_autorizo", tipo = OdbcType.Int, valor = 0 },
                new OdbcParameterDTO() { nombre = "empleado_vobo", tipo = OdbcType.Int, valor = 0 },
                new OdbcParameterDTO() { nombre = "tipo_factoraje", tipo = OdbcType.VarChar, valor = "N" },
                new OdbcParameterDTO() { nombre = "UUID", tipo = OdbcType.VarChar, valor = "" }
            };
            return odbc;
        }
        OdbcConsultaDTO getMovProv_ToUpdate(MontoPropPagoDTO obj)
        {
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"update " + vSesiones.sesionEmpresaDBPregijo + @"sp_movprov set tipocambio = ? where numpro = ? and factura = ? ";
            odbc.parametros = new List<OdbcParameterDTO>() 
            {
                new OdbcParameterDTO() { nombre = "tipocambio", tipo = OdbcType.Decimal, valor = obj.tipocambio},
                new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.Int, valor = obj.numpro },
                new OdbcParameterDTO() { nombre = "factura", tipo = OdbcType.Int, valor = int.Parse(obj.factura) }
            };
            return odbc;
        }

        #endregion
        List<tblC_sp_gastos_prov> getLstGenGastosPorId(List<MontoPropPagoDTO> lst)
        {
            var odbc = new OdbcConsultaDTO()
            {
                consulta = string.Format("SELECT * FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_gastos_prov WHERE id IN {0}", lst.Select(s => s.id.ToString()).ToList().ToParamInValue())
            };
            odbc.parametros.AddRange(lst.Select(s => new OdbcParameterDTO() { nombre = "id", tipo = OdbcType.Numeric, valor = s.id }).ToList());
            var lstEk = _contextEnkontrol.Select<tblC_sp_gastos_prov>(EnkontrolAmbienteEnum.Prod, odbc);
            return lstEk;
        }
        int saveGeneraMovprov(MontoPropPagoDTO obj, DateTime pago)
        {
            var guardado = 0;
            var objEk = new sp_genera_movprovDTO();
            var odbc = new OdbcConsultaDTO()
            {
                consulta = "SELECT * FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_genera_movprov WHERE numpro = ? AND factura = ? AND cc = ?",
                parametros = new List<OdbcParameterDTO>()
            };
            odbc.parametros.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.Numeric, valor = obj.proveedorID });
            odbc.parametros.Add(new OdbcParameterDTO() { nombre = "factura", tipo = OdbcType.Numeric, valor = obj.factura });
            odbc.parametros.Add(new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = obj.cc });
            var lstEk = _contextEnkontrol.Select<sp_genera_movprovDTO>(EnkontrolAmbienteEnum.Prod, odbc);
            if (lstEk.Count == 0)
            {
                objEk = new sp_genera_movprovDTO()
                {
                    numpro = obj.proveedorID,
                    cc = obj.cc,
                    factura = obj.factura,
                    tm = obj.tm,
                    monto_plan = obj.monto,
                    oc = obj.oc,
                    status = "P",
                    fecha_movto = pago,
                    fecha = obj.vence,
                    concepto = obj.concepto,
                    id = obj.id,
                    monto = obj.monto,
                    ruta_rec_pdf = obj.pdf,
                    ruta_rec_xml = obj.xml,
                    tm_bancario = obj.tmb,
                    tm_prov = obj.tmp,
                    //autorizapago = obj.au
                };
                guardado = saveGeneraMovprov(objEk);
            }
            else
            {
                objEk = lstEk.FirstOrDefault();
                guardado = updateGeneraMovprov(objEk);
            }
            return guardado;
        }
        int saveGeneraMovprov(sp_genera_movprovDTO save)
        {
            var i = 0;
            var odbc = new OdbcConsultaDTO()
            {
                consulta = @"INSERT INTO " + vSesiones.sesionEmpresaDBPregijo + @"sp_genera_movprov (
                        numpro
                       ,factura
                       ,fecha
                       ,tm
                       ,monto
                       ,tm_bancario
                       ,tm_prov
                       ,fecha_movto
                       ,cc
                       ,oc
                       ,monto_plan
                       ,status 

            ) VALUES (?,?,?,?,?,?,?,?,?,?,?,?)",
                parametros = setParamGenMovProv(save)
            };
            i = _contextEnkontrol.Save(EnkontrolAmbienteEnum.Prod, odbc);
            try
            {
                var obj = _context.tblC_sp_gastos_prov.FirstOrDefault(x => x.numpro == save.numpro && x.factura.Equals(save.factura.ToString()));
                obj.fechaAutorizacion = DateTime.Now;
                obj.autorizada = true;
                obj.monto_plan = save.monto_plan;
                obj.autorizo = vSesiones.sesionUsuarioDTO.nombre;
                _context.SaveChanges();
            }
            catch (Exception)
            {


            }
            return i;
        }
        int updateGeneraMovprov(sp_genera_movprovDTO save)
        {
            var i = 0;
//            var odbc = new OdbcConsultaDTO()
//            {
//                consulta = @"UPDATE " + vSesiones.sesionEmpresaDBPregijo + @"sp_genera_movprov set
//                        numpro = ?
//                       ,factura = ?
//                       ,fecha = ?
//                       ,tm = ? 
//                       ,monto = ?
//                       ,tm_bancario = ?
//                       ,tm_prov  = ?
//                       ,fecha_movto  = ?
//                       ,cc = ?
//                       ,oc = ?
//                       ,monto_plan = ?
//                       ,status = ?
//                       ,clave_sub_tm = ?
//                       ,id_cta_dep = ?
//                       ,id_batch = ?
//                        WHERE numpro = ? AND factura = ? AND cc = ?",
//                parametros = setParamGenMovProv(save)
//            };
            var odbc = new OdbcConsultaDTO()
            {
                consulta = @"UPDATE " + vSesiones.sesionEmpresaDBPregijo + @"sp_genera_movprov set
                        numpro = ?
                       ,factura = ?
                       ,fecha = ?
                       ,tm = ? 
                       ,monto = ?
                       ,tm_bancario = ?
                       ,tm_prov  = ?
                       ,fecha_movto  = ?
                       ,cc = ?
                       ,oc = ?
                       ,monto_plan = ?
                       ,status = ?
                        WHERE numpro = ? AND factura = ? AND cc = ?",
                parametros = setParamGenMovProv(save)
            };
            odbc.parametros.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.Numeric, valor = save.numpro });
            odbc.parametros.Add(new OdbcParameterDTO() { nombre = "factura", tipo = OdbcType.Numeric, valor = save.factura });
            odbc.parametros.Add(new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = save.cc });
            i = _contextEnkontrol.Save(EnkontrolAmbienteEnum.Prod, odbc);
            return i;
        }
        List<OdbcParameterDTO> setParamGenMovProv(sp_genera_movprovDTO save)
        {
            var lst = new List<OdbcParameterDTO>();
            lst.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.Numeric, valor = save.numpro });
            lst.Add(new OdbcParameterDTO() { nombre = "factura", tipo = OdbcType.Numeric, valor = save.factura });
            lst.Add(new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.Date, valor = save.fecha });
            lst.Add(new OdbcParameterDTO() { nombre = "tm", tipo = OdbcType.Int, valor = save.tm });
            lst.Add(new OdbcParameterDTO() { nombre = "monto", tipo = OdbcType.Decimal, valor = save.monto });
            lst.Add(new OdbcParameterDTO() { nombre = "tm_bancario", tipo = OdbcType.Int, valor = save.tm_bancario });
            lst.Add(new OdbcParameterDTO() { nombre = "tm_prov", tipo = OdbcType.Int, valor = save.tm_prov });
            lst.Add(new OdbcParameterDTO() { nombre = "fecha_movto", tipo = OdbcType.Date, valor = save.fecha_movto });
            lst.Add(new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = save.cc });
            lst.Add(new OdbcParameterDTO() { nombre = "oc", tipo = OdbcType.Int, valor = save.oc });
            lst.Add(new OdbcParameterDTO() { nombre = "monto_plan", tipo = OdbcType.Decimal, valor = save.monto_plan });
            lst.Add(new OdbcParameterDTO() { nombre = "status", tipo = OdbcType.VarChar, valor = save.status });
            //lst.Add(new OdbcParameterDTO() { nombre = "clave_sub_tm", tipo = OdbcType.Numeric, valor = save.clave_sub_tm });
            //lst.Add(new OdbcParameterDTO() { nombre = "id_cta_dep", tipo = OdbcType.Int, valor = save.id_cta_dep });
            //lst.Add(new OdbcParameterDTO() { nombre = "id_batch", tipo = OdbcType.Int, valor = save.id_batch });
            return lst;
        }
        public List<string> getLimitNoProveedores()
        {
            List<ProveedoresDTO> lstProveedores = new List<ProveedoresDTO>();
            try
            {
                var lst = new List<string>();
                if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Peru) 
                {
                    var resultado = _contextEnkontrol.Select<ProveedoresDTO>(EnkontrolAmbienteEnum.Prod, "SELECT numpro AS noProveedor, nombre AS nomProveedor FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_proveedores ");
                    lst = new List<string>() {
                        resultado.Min(m => m.noProveedor).ToString(),
                        resultado.Max(m => m.noProveedor).ToString()
                    };
                }
                else
                {
                    using (var ctx = new MainContextPeruStarSoft003BDCOMUN())
                    { 
                        var listaProveedoresPeru = ctx.MAEPROV.Select(x => new Core.DTO.Principal.Generales.ComboDTO
                        {
                            Value = x.PRVCCODIGO,
                            Text = x.PRVCNOMBRE,
                            Prefijo = x.PRVCRUC
                        }).ToList();
                        lst = new List<string>() {
                            listaProveedoresPeru.Min(m => m.Value),
                            listaProveedoresPeru.Max(m => m.Value)
                        };
                    }

                }
                return lst;
            }
            catch
            {
                return new List<string>(); ;
            }
        }
        /// <summary>
        /// Consulta de Programacion de Pagos
        /// </summary>
        /// <returns>Lista Gastos</returns>
        public List<ProgrPagoDTO> GetListProgrPagos(string min, string max, List<string> cc, DateTime fecha)
        {
//            string query = @"
//                SELECT 
//                    * 
//                FROM (
//                    SELECT 
//                        MAX(gas.id) AS id, 
//                        mov.numpro, 
//                        mov.factura, 
//                        mov.cc, 
//                        MIN(mov.tm) AS tm, 
//                        MAX(tm.tm_pago) AS tm_prov, 
//                        MAX(tm.tm_banco) AS tm_bancario, 
//                        MIN(mov.fechavenc) AS fecha, 
//                        MAX(mov.referenciaoc) AS oc, 
//                        MAX(mov.concepto) AS concepto, 
//                        SUM(mov.total) AS monto, 
//                        gas.tipocambio AS tipocambio, 
//                        MAX(gas.estatus) AS status, 
//                        MAX(gas.ruta_rec_pdf) AS ruta_rec_pdf, 
//                        MAX(gas.ruta_rec_xml) AS ruta_rec_xml, 
//                        gen.status AS genEstado, 
//                        gen.monto_plan AS genMonto";

//            if (vSesiones.sesionEmpresaActual == 2)
//            {
//                query += @", 
//                        (
//                            SELECT 
//                                TOP 1 STRING(area, '-', cuenta) AS ac 
//                            FROM " + vSesiones.sesionEmpresaDBPregijo + @"si_area_cuenta 
//                            WHERE centro_costo = mov.cc
//                        ) AS ac, 
//                        (
//                            SELECT 
//                                TOP 1 descripcion 
//                            FROM " + vSesiones.sesionEmpresaDBPregijo + @"si_area_cuenta 
//                            WHERE centro_costo = mov.cc
//                        ) AS acDesc";
//            }

//            query += @" 
//                    FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_movprov mov
//                        INNER JOIN " + vSesiones.sesionEmpresaDBPregijo + @"sp_gastos_prov gas ON mov.numpro = gas.numpro AND mov.cc = gas.cc AND mov.factura = gas.factura 
//                        INNER JOIN " + vSesiones.sesionEmpresaDBPregijo + @"sp_tm tm ON tm.tm = mov.tm 
//                        LEFT JOIN " + vSesiones.sesionEmpresaDBPregijo + @"sp_genera_movprov gen ON gen.numpro = gas.numpro AND gen.cc = gas.cc AND gen.factura = gas.factura 
//                    WHERE 
//                        gas.cerrado = 1 AND 
//                        (
//                            (mov.autorizapago IN ('A','E','C') AND mov.es_factura='S' AND mov.fechavenc <= ?) OR 
//                            (mov.es_factura='N' and mov.autorizapago not in ('A','E','B'))
//                        ) AND 
//                        mov.numpro BETWEEN ? AND ? ";

//            if (vSesiones.sesionEmpresaActual != 2)
//            {
//                query += @" AND mov.cc IN {0} ";
//            }

//            query += @"AND mov.factura NOT IN (
//                    SELECT 
//                        factura 
//                    FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_movprov 
//                    WHERE 
//                        (tm = 51 OR tm = 26) AND 
//                        numpro = mov.numpro AND 
//                        cc = mov.cc 
//                    GROUP BY factura HAVING SUM(abs(total)) = mov.total) 
//                    --AND bit_factoraje='N' 
//                GROUP BY mov.numpro, mov.factura, mov.cc, gas.tipocambio, gen.status, gen.monto_plan 
//                ORDER BY mov.numpro, mov.factura 
//            ) x WHERE x.monto > 1";

//            var odbc = new OdbcConsultaDTO();

//            if (vSesiones.sesionEmpresaActual != 2)
//            {
//                odbc = new OdbcConsultaDTO()
//                {

//                    consulta = string.Format(query, cc.ToParamInValue()),
//                    parametros = new List<OdbcParameterDTO>() { new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.Date, valor = fecha } }

//                };
//                odbc.parametros.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.VarChar, valor = min });
//                odbc.parametros.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.VarChar, valor = max });
//                odbc.parametros.AddRange(cc.Select(s => new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = s }));
//            }
//            else
//            {
//                odbc = new OdbcConsultaDTO()
//                {

//                    consulta = string.Format(query),
//                    parametros = new List<OdbcParameterDTO>() { new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.Date, valor = fecha } }

//                };
//                odbc.parametros.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.VarChar, valor = min });
//                odbc.parametros.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.VarChar, valor = max });
//            }

//            var gastosProvTemp = _contextEnkontrol.Select<sp_genera_movprovDTO>(_productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, odbc);

            var dataSIGO = new List<sp_genera_movprovDTO>();

            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                case EmpresaEnum.Construplan:
                case EmpresaEnum.Colombia:
                    {
                        var _min = Convert.ToInt32(min);
                        var _max = Convert.ToInt32(max);

                        dataSIGO = _context.tblC_sp_gastos_prov.Where(x => x.autorizada == false && x.numpro >= _min && x.numpro <= _max).ToList().Select(x => new sp_genera_movprovDTO
                        {
                            id = x.id,
                            numpro = (int)x.numpro,
                            factura = x.factura,
                            fecha = x.fecha,
                            tm = x.tm,
                            monto = x.monto,
                            cc = x.cc,
                            oc = x.referenciaoc,
                            monto_plan = x.monto_plan,
                            tipocambio = x.tipocambio,
                            status = x.estatus,
                            concepto = x.concepto,
                            ruta_rec_pdf = x.ruta_rec_pdf,
                            ruta_rec_xml = x.ruta_rec_xml,
                            programo = x.programo,
                            autorizo = x.autorizo,
                            ac = x.ac,
                            acDesc = x.acDesc,
                            activo_fijo = x.activo_fijo ?? false,
                            areaCuenta = x.ac,
                            iva = x.iva,
                            numproPeru = x.numproPeru,
                            moneda = x.moneda
                        }).ToList();
                    }
                    break;
                case EmpresaEnum.Arrendadora:
                    {
                        var _min = Convert.ToInt32(min);
                        var _max = Convert.ToInt32(max);

                        dataSIGO = _context.tblC_sp_gastos_prov.Where(x => x.autorizada == false && x.numpro >= _min && x.numpro <= _max).ToList().Select(x => new sp_genera_movprovDTO
                        {
                            id = x.id,
                            numpro = (int)x.numpro,
                            factura = x.factura,
                            fecha = x.fecha,
                            tm = x.tm,
                            monto = x.monto,
                            cc = x.cc,
                            oc = x.referenciaoc,
                            monto_plan = x.monto_plan,
                            tipocambio = x.tipocambio,
                            status = x.estatus,
                            concepto = x.concepto,
                            ruta_rec_pdf = x.ruta_rec_pdf,
                            ruta_rec_xml = x.ruta_rec_xml,
                            programo = x.programo,
                            autorizo = x.autorizo,
                            ac = x.ac,
                            acDesc = x.acDesc,
                            activo_fijo = x.activo_fijo ?? false,
                            areaCuenta = x.ac,
                            iva = x.iva,
                            numproPeru = x.numproPeru,
                            moneda = x.moneda
                        }).ToList();
                    }
                    break;
                case EmpresaEnum.Peru:
                    {
                        dataSIGO = _context.tblC_sp_gastos_prov.Where(x => x.autorizada == false).ToList().Select(x => new sp_genera_movprovDTO
                        {
                            id = x.id,
                            numpro = (int)x.numpro,
                            factura = x.factura,
                            fecha = x.fecha,
                            tm = x.tm,
                            monto = x.monto,
                            cc = x.cc,
                            oc = x.referenciaoc,
                            monto_plan = x.monto_plan,
                            tipocambio = x.tipocambio,
                            status = x.estatus,
                            concepto = x.concepto,
                            ruta_rec_pdf = x.ruta_rec_pdf,
                            ruta_rec_xml = x.ruta_rec_xml,
                            programo = x.programo,
                            autorizo = x.autorizo,
                            ac = x.ac,
                            acDesc = x.acDesc,
                            activo_fijo = x.activo_fijo ?? false,
                            areaCuenta = x.ac,
                            iva = x.iva,
                            numproPeru = x.numproPeru,
                            moneda = x.moneda
                        }).ToList();
                    }
                    break;
            }
            //var dataSIGO = _context.tblC_sp_gastos_prov.Where(x => x.autorizada == false).ToList().Select(x => new sp_genera_movprovDTO { 
            //    id = x.id,
            //    numpro = (int)x.numpro,
            //    factura = x.factura,
            //    fecha = x.fecha,
            //    tm = x.tm,
            //    monto = x.monto,
            //    cc = x.cc,
            //    oc =  x.referenciaoc,
            //    monto_plan = x.monto_plan,
            //    tipocambio = x.tipocambio,
            //    status = x.estatus,
            //    concepto = x.concepto,
            //    ruta_rec_pdf = x.ruta_rec_pdf,
            //    ruta_rec_xml = x.ruta_rec_xml,
            //    programo = x.programo,
            //    autorizo = x.autorizo,
            //    ac = x.ac,
            //    acDesc = x.acDesc,
            //    activo_fijo = x.activo_fijo ?? false,
            //    areaCuenta = x.ac,
            //    iva = x.iva,
            //    numproPeru = x.numproPeru,
            //    moneda = x.moneda            
            //}).ToList();
            var gastosProv = new List<sp_genera_movprovDTO>();
            //foreach (var i in gastosProvTemp)
            //{
            //    var e = dataSIGO.FirstOrDefault(x => x.numpro == i.numpro && x.factura.Equals(i.factura.ToString()));
            //    if (e != null)
            //    {
            //        var e2 = _context.tblC_sp_gastos_prov_activofijo.FirstOrDefault(x => x.numpro == i.numpro && x.cc == i.cc && x.factura == i.factura.ToString());
            //        i.genMonto = e.monto_plan;
            //        i.activo_fijo = e2 != null ? true : false;
            //        gastosProv.Add(i);
            //    }
            //}


            if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Peru)
            {
                foreach (var i in dataSIGO)
                {
                    var e = _context.tblC_sp_gastos_prov_activofijo.FirstOrDefault(x => x.numpro == i.numpro && x.cc == i.cc && x.factura == i.factura.ToString());
                    i.genMonto = i.monto_plan;
                    i.activo_fijo = e != null ? true : false;
                    gastosProv.Add(i);
                }
            }
            else
            {
                foreach (var i in dataSIGO)
                {
                    //var e = _context.tblC_sp_gastos_prov_activofijo.FirstOrDefault(x => x.numpro == i.numpro && x.cc == i.cc && x.factura == i.factura.ToString());
                    i.genMonto = i.monto_plan;
                    i.activo_fijo = false;
                    gastosProv.Add(i);
                }
            }

            #region Se agregan los manuales.

            //var listaManuales = _context.tblC_sp_gastos_prov.Where(x =>
            //    x.manual && x.estatus == "A" && x.numpro >= min && x.numpro <= max).ToList().Where(x => /*cc.Contains(x.cc) &&*/ DbFunctions.TruncateTime(x.fecha) <= fecha
            //).ToList();
            var listaTM = _contextEnkontrol.Select<dynamic>(EnkontrolAmbienteEnum.Prod, new OdbcConsultaDTO() { consulta = string.Format(@"SELECT * FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_tm") });
            var listaAreaCuenta = _contextEnkontrol.Select<dynamic>(EnkontrolAmbienteEnum.Prod, new OdbcConsultaDTO() { consulta = string.Format(@"SELECT * FROM " + vSesiones.sesionEmpresaDBPregijo + @"si_area_cuenta") });

            //foreach (var man in listaManuales)
            //{
            //    var tmEK = listaTM.FirstOrDefault(x => (int)x.tm == man.tm);
            //    var ac = "";
            //    var acDesc = "";

            //    if (vSesiones.sesionEmpresaActual == 2)
            //    {
            //        var areaCuentaEK = listaAreaCuenta.FirstOrDefault(x => (string)x.centro_costo == man.cc);

            //        ac = (string)areaCuentaEK.area + "-" + (string)areaCuentaEK.cuenta;
            //        acDesc = (string)areaCuentaEK.descripcion;
            //    }

            //    gastosProv.Add(new sp_genera_movprovDTO
            //    {
            //        id = man.id,
            //        cc = man.cc,
            //        concepto = man.concepto,
            //        factura = Int32.Parse(man.factura),
            //        monto = man.monto,
            //        oc = Int32.Parse(man.referenciaoc),
            //        ruta_rec_pdf = man.ruta_rec_pdf ?? "",
            //        ruta_rec_xml = man.ruta_rec_xml ?? "",
            //        numpro = man.numpro,
            //        tipocambio = man.tipocambio,
            //        tm = man.tm,
            //        tm_bancario = (int)tmEK.tm_banco,
            //        tm_prov = (int)tmEK.tm_pago,
            //        fecha = man.fecha,
            //        genMonto = man.monto, //man.genMonto,
            //        genEstado = "", //man.genEstado,
            //        ac = ac,
            //        acDesc = acDesc
            //    });
            //}
            #endregion

            var ordenesCompra = gastosProv.Select(s => s.oc.ToString()).ToList();
            var lstTmp = _contextEnkontrol.Select<ComboDTO>(EnkontrolAmbienteEnum.Prod, "SELECT tm AS Value, descripcion AS Text FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_tm");
            var lstTmb = _contextEnkontrol.Select<ComboDTO>(EnkontrolAmbienteEnum.Prod, "SELECT clave AS Value, descripcion AS Text FROM " + vSesiones.sesionEmpresaDBPregijo + @"sb_tm");
            var lstOC = new List<OrdenCompraPagoDTO>();

            if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Peru)
            {
                if (vSesiones.sesionEmpresaActual != 2)
                {
                    lstOC = _contextEnkontrol.Select<OrdenCompraPagoDTO>(EnkontrolAmbienteEnum.Prod, string.Format(@"
                    SELECT 
                        * 
                    FROM " + vSesiones.sesionEmpresaDBPregijo + @"so_orden_compra 
                    WHERE numero IN ({0}) AND cc IN ({1})", ordenesCompra.ToLine(","), cc.ToLine(","))
                    );
                }
                else
                {
                    lstOC = _contextEnkontrol.Select<OrdenCompraPagoDTO>(EnkontrolAmbienteEnum.Prod, string.Format(@"SELECT * FROM " + vSesiones.sesionEmpresaDBPregijo + @"so_orden_compra WHERE numero IN ({0}) "
                        , ordenesCompra.ToLine(",")
                        , ""));
                }
            }

            var lstProveedores = getComboProveedores().Where(x => gastosProv.Any(p => p.numpro.ToString().Equals(x.Value))).ToList();
            List<Core.DTO.Principal.Generales.ComboDTO> listaProveedoresPeru = new List<Core.DTO.Principal.Generales.ComboDTO>();

            using (var ctx = new MainContextPeruStarSoft003BDCOMUN())
            { 
                listaProveedoresPeru = ctx.MAEPROV.Select(x => new Core.DTO.Principal.Generales.ComboDTO
                {
                    Value = x.PRVCCODIGO,
                    Text = x.PRVCNOMBRE,
                    Prefijo = x.PRVCRUC
                }).ToList();
            }

            var data = gastosProv.Select(item => {
                var proveedor = (item.numproPeru == null || item.numproPeru == "") ? lstProveedores.FirstOrDefault(p => p.Value.Equals(item.numpro.ToString())) : listaProveedoresPeru.FirstOrDefault(p => p.Value.Trim() == item.numproPeru.Trim());
                return new ProgrPagoDTO()
                {
                    id = item.id,
                    cc = item.cc,
                    concepto = item.concepto,
                    factura = item.factura.ToString(),
                    maxPago = item.monto,
                    monto = item.monto,
                    oc = item.oc.ToString(),
                    pdf = item.ruta_rec_pdf,
                    xml = item.ruta_rec_xml,
                    proveedor = proveedor == null ? ((item.numproPeru == null || item.numproPeru == "") ? item.numpro.ToString() : item.numproPeru) : proveedor.Text,
                    proveedorID = (item.numproPeru == null || item.numproPeru == "") ? item.numpro.ToString() : item.numproPeru,
                    recibido = item.monto,
                    tipocambio = item.tipocambio ?? 1,
                    saldo = lstOC.Where(w => w.cc.Equals(item.cc) && w.numero.Equals(item.oc)).ToList().Sum(s => s.total_fac - s.total_fac),
                    pagado = lstOC.Where(w => w.cc.Equals(item.cc) && w.numero.Equals(item.oc)).ToList().Sum(s => s.total_fac - s.total_fac) - item.monto,
                    solicitado = item.monto,
                    tipoMoneda = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru ? item.moneda :  (item.numpro >= 9000 ? "DLL" : "MN"),
                    tm = item.tm,
                    tmb = item.tm_bancario == 0 ? 53 : item.tm_bancario,
                    tmbDescripcion = lstTmb.FirstOrDefault(tm => tm.Value.Equals((item.tm_bancario == 0 ? 53 : item.tm_bancario))).Text ?? string.Empty,
                    tmp = item.tm_prov == 0 ? 51 : item.tm_prov,
                    tmpDescripcion = lstTmp.FirstOrDefault(tm => tm.Value.Equals((item.tm_prov == 0 ? 51 : item.tm_prov))).Text ?? string.Empty,
                    vence = item.fecha,
                    monto_plan = item.genMonto ?? 0,
                    esPagado = item.genEstado == null ? false : item.genEstado.Equals("P"),
                    ac = item.ac,
                    acDesc = item.acDesc,
                    activo_fijo = item.activo_fijo,
                    iva = item.iva,
                    numproPeru = item.numproPeru
                };
            }).OrderBy(o => o.proveedorID).ToList();

            return data;
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> getComboProveedores()
        {
            try
            {
                var lstProv = (List<Core.DTO.Principal.Generales.ComboDTO>)_contextEnkontrol.Where("SELECT numpro as Value, nombre as Text FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_proveedores").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                return lstProv.OrderBy(x => x.Text).ToList();
            }
            catch (Exception) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        public List<sp_genera_movprovDTO> getLstGenMovProv(BusqGenMovProvDTO busq)
        {
            busq.maxMov = busq.maxMov.AddHours(23).AddMinutes(59).AddSeconds(59);
            var ccs = busq.lstCc;
            if (ccs == null) ccs = new List<string>();
            var lst = new List<sp_genera_movprovDTO>();

            var lstTmp = _contextEnkontrol.Select<ComboDTO>(EnkontrolAmbienteEnum.Prod, "SELECT tm AS Value, descripcion AS Text FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_tm");
            var lstTmb = _contextEnkontrol.Select<ComboDTO>(EnkontrolAmbienteEnum.Prod, "SELECT clave AS Value, descripcion AS Text FROM " + vSesiones.sesionEmpresaDBPregijo + @"sb_tm");
            var lstBancos = _contextEnkontrol.Select<BancosDTO>(EnkontrolAmbienteEnum.Prod, "SELECT a.numpro,a.clabe as cuenta,b.banco,b.descripcion as bancoDesc FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_proveedores_cta_dep a inner join " + vSesiones.sesionEmpresaDBPregijo + @"sb_bancos b on a.banco=b.banco where a.ind_cuenta_activa=1");
            var data = new List<tblC_sp_gastos_prov>();
            if (busq.tipo == 1)//Propuesta
            {
                if (vSesiones.sesionEmpresaActual != 2)
                {
                    data = _context.tblC_sp_gastos_prov.Where(x => (ccs.Count() > 0 ? ccs.Contains(x.cc) : true) && x.fechaPropuesta >= busq.minMov && x.fechaPropuesta <= busq.maxMov && (x.numpro >= busq.minProv && x.numpro <= busq.maxProv)).OrderBy(x => x.numpro).ToList();
                }
                else
                {
                    data = _context.tblC_sp_gastos_prov.Where(x => x.fechaPropuesta >= busq.minMov && x.fechaPropuesta <= busq.maxMov && (x.numpro >= busq.minProv && x.numpro <= busq.maxProv)).OrderBy(x => x.numpro).ToList();

                }
                var dataConverted = sp_gastosEntityToGeneraDTO(data, lstBancos);
                lst.AddRange(dataConverted);
            }
            else if (busq.tipo == 2)//Pendiente autorizar
            {
                if (vSesiones.sesionEmpresaActual != 2)
                {
                    data = _context.tblC_sp_gastos_prov.Where(x => ccs.Contains(x.cc) && x.fechaPropuesta >= busq.minMov && x.fechaPropuesta <= busq.maxMov && x.fechaAutorizacion == null && !x.autorizada && (x.numpro >= busq.minProv && x.numpro <= busq.maxProv)).OrderBy(x => x.numpro).ToList();
                }
                else
                {
                    data = _context.tblC_sp_gastos_prov.Where(x => x.fechaPropuesta >= busq.minMov && x.fechaPropuesta <= busq.maxMov && x.fechaAutorizacion == null && !x.autorizada && (x.numpro >= busq.minProv && x.numpro <= busq.maxProv)).OrderBy(x => x.numpro).ToList();
                }
                var dataConverted = sp_gastosEntityToGeneraDTO(data, lstBancos);
                lst.AddRange(dataConverted);
            }
            else if (busq.tipo == 3)//Autorizados
            {
                if (vSesiones.sesionEmpresaActual != 2)
                {
                    data = _context.tblC_sp_gastos_prov.Where(x => ccs.Contains(x.cc) && x.fechaAutorizacion >= busq.minMov && x.fechaAutorizacion <= busq.maxMov && x.autorizada && (x.numpro >= busq.minProv && x.numpro <= busq.maxProv)).OrderBy(x => x.numpro).ToList();
                }
                else
                {
                    data = _context.tblC_sp_gastos_prov.Where(x => x.fechaAutorizacion >= busq.minMov && x.fechaAutorizacion <= busq.maxMov && x.autorizada && (x.numpro >= busq.minProv && x.numpro <= busq.maxProv)).OrderBy(x => x.numpro).ToList();
                }
                var dataConverted = sp_gastosEntityToGeneraDTO(data, lstBancos);
                lst.AddRange(dataConverted);
            }
            else if (busq.tipo == 4)//Pendiente cheque
            {
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = queryLstGenMovProv(busq),
                    parametros = paramLstGenMovProv(busq)
                };
                lst = _contextEnkontrol.Select<sp_genera_movprovDTO>(EnkontrolAmbienteEnum.Prod, odbc);
            }
            else if (busq.tipo == 5)//Cheque generado
            {
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = queryLstGenMovProv(busq),
                    parametros = paramLstGenMovProv(busq)
                };
                var temp = _contextEnkontrol.Select<sp_genera_movprovDTO>(EnkontrolAmbienteEnum.Prod, odbc);

            }
            var lstProveedores = _contextEnkontrol.Select<ComboDTO>(EnkontrolAmbienteEnum.Prod, string.Format(@"SELECT numpro as Value, nombre as Text FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_proveedores WHERE numpro IN ({0})"
                , lst.Select(s => s.numpro.ToString()).ToList().ToLine(",")));

            if (vSesiones.sesionEmpresaActual == 2)
            {

                var lstGastosProvIDs = data.Select(x => x.id).ToList();
                var odbcGastos = new OdbcConsultaDTO()
                {
                    consulta = queryLstAreasCuentaMovProv(lstGastosProvIDs),
                    parametros = paramLstGenProv(lstGastosProvIDs)
                };
                var lstGastosProvAreaCuenta = _contextEnkontrol.Select<listaGastosProvAreaCuentaDTO>(EnkontrolAmbienteEnum.Prod, odbcGastos);

                foreach (var mov in lst)
                {
                    mov.proveedor = lstProveedores.FirstOrDefault(w => w.Value.Equals(mov.numpro)).Text;
                    mov.descTm = lstTmp.FirstOrDefault(w => w.Value.Equals(mov.tm)).Text;
                    //mov.descTmp = lstTmp.FirstOrDefault(w => w.Value.Equals(mov.tm_prov)).Text;
                    //mov.descTmb = lstTmb.FirstOrDefault(w => w.Value.Equals(mov.tm_bancario)).Text;
                    mov.descStatus = busq.tipo == 4 ? "AUTORIZADO" : mov.genEstado;
                    var auxData = lstGastosProvAreaCuenta.FirstOrDefault(x => x.id == mov.id);
                    mov.areaCuenta = auxData == null ? "N-A" : auxData.area + "-" + auxData.cuenta_oc;
                }

                if (busq.lstCc.Count() > 0)
                {
                    lst = lst.Where(x => busq.lstCc.Contains(x.areaCuenta)).ToList();
                }
            }
            else
            {
                if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Peru)
                {

                    lst.ForEach(mov =>
                    {
                        mov.proveedor = lstProveedores.FirstOrDefault(w => w.Value.Equals(mov.numpro)).Text;
                        mov.descTm = lstTmp.FirstOrDefault(w => w.Value.Equals(mov.tm)).Text;
                        //mov.descTmp = lstTmp.FirstOrDefault(w => w.Value.Equals(mov.tm_prov)).Text;
                        //mov.descTmb = lstTmb.FirstOrDefault(w => w.Value.Equals(mov.tm_bancario)).Text;
                        mov.descStatus = busq.tipo == 4 ? "AUTORIZADO" : mov.genEstado;
                        mov.areaCuenta = "N/A";
                    });
                }
                else
                {
                    lst.ForEach(mov =>
                    {
                        mov.proveedor = mov.numproPeru + " " + mov.proveedor;
                        mov.descTm = mov.descTm;
                        //mov.descTmp = lstTmp.FirstOrDefault(w => w.Value.Equals(mov.tm_prov)).Text;
                        //mov.descTmb = lstTmb.FirstOrDefault(w => w.Value.Equals(mov.tm_bancario)).Text;
                        mov.descStatus = busq.tipo == 4 ? "AUTORIZADO" : mov.genEstado;
                        mov.areaCuenta = "N/A";
                    });
                }
            }
            return lst;
        }

        string queryLstAreasCuentaMovProv(List<int> movProvIDs) 
        {
            return string.Format(@"SELECT 
                A.area, A.cuenta_oc, B.id 
            FROM 
                (SELECT 
                    movpol.area, movpol.cuenta_oc, movprov.factura, movprov.numpro  
                FROM 
                    sc_movpol movpol INNER JOIN sp_movprov movprov 
                ON movpol.year = movprov.year AND movpol.mes = movprov.mes AND movpol.tp = movprov.tp AND movpol.poliza = movprov.poliza AND movpol.linea = movprov.linea
                ) A 
                INNER JOIN sp_gastos_prov B ON A.factura = B.factura AND A.numpro = B.numpro
            WHERE B.id in {0}", movProvIDs.ToParamInValue()
            );
        }

        string queryLstGenMovProv(BusqGenMovProvDTO busq)
        {
            if (vSesiones.sesionEmpresaActual != 2)
            {
                return string.Format(@"SELECT * FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_genera_movprov
                                            WHERE fecha_movto BETWEEN ? AND ?
                                                AND cc IN {0}
                                                AND numpro BETWEEN ? AND ?
                                            ORDER BY numpro ,factura", busq.lstCc.ToParamInValue());
            }
            else
            {
                return string.Format(@"SELECT * FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_genera_movprov
                                            WHERE fecha_movto BETWEEN ? AND ? 
                                                AND numpro BETWEEN ? AND ? 
                                            ORDER BY numpro ,factura");
            }
        }
        List<OdbcParameterDTO> paramLstGenMovProv(BusqGenMovProvDTO busq)
        {
            var lst = new List<OdbcParameterDTO>();
            lst.Add(new OdbcParameterDTO() { nombre = "fecha_movto", tipo = OdbcType.Date, valor = busq.minMov });
            lst.Add(new OdbcParameterDTO() { nombre = "fecha_movto", tipo = OdbcType.Date, valor = busq.maxMov });
            if (vSesiones.sesionEmpresaActual != 2)
            {
                lst.AddRange(busq.lstCc.Select(s => new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = s }).ToList());
            }
            lst.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.Numeric, valor = busq.minProv });
            lst.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.Numeric, valor = busq.maxProv });
            return lst;
        }

        List<OdbcParameterDTO> paramLstGenProv(List <int> lstGastosProvIDs)
        {
            var lst = new List<OdbcParameterDTO>();
            lst.AddRange(lstGastosProvIDs.Select(s => new OdbcParameterDTO() { nombre = "id", tipo = OdbcType.Numeric, valor = s }).ToList());
            return lst;
        }

        public List<tblC_sp_gastos_prov> getReportes(DateTime fechaInicio, DateTime fechaFin, bool autorizada)
        {
            var data = new List<tblC_sp_gastos_prov>();
            data = _context.tblC_sp_gastos_prov.Where(x => (x.autorizada && (x.fechaAutorizacion >= fechaInicio && x.fechaAutorizacion <= fechaFin)) || (!x.autorizada && (x.fechaPropuesta >= fechaInicio && x.fechaPropuesta <= fechaFin))).ToList();
            return data;
        }
        public List<tblC_sp_gastos_provDTO> sp_gastosEntityToDTO(List<tblC_sp_gastos_provDTO> data)
        {
            var converted = new List<tblC_sp_gastos_provDTO>();
            foreach (var i in data)
            {
                var o = new tblC_sp_gastos_provDTO();
                o.id = i.id;
                o.idSigoplan = i.idSigoplan;
                o.numpro = i.numpro;
                o.cfd_serie = i.cfd_serie;
                o.cfd_folio = i.cfd_folio;
                o.referenciaoc = i.referenciaoc;
                o.tipo_prov = i.tipo_prov;
                o.cc = i.cc;
                o.tm = i.tm;
                o.factura = i.factura;
                o.monto = i.monto;
                o.iva = i.iva;
                o.tipocambio = i.tipocambio;
                o.total = i.total;
                o.fecha_timbrado = i.fecha_timbrado;
                o.estatus = i.estatus;
                o.idGiro = i.idGiro;
                o.concepto = i.concepto;
                o.fecha = i.fecha;
                o.moneda = i.moneda;
                o.uuid = i.uuid;
                o.ruta_rec_xml = i.ruta_rec_xml;
                o.ruta_rec_pdf = i.ruta_rec_pdf;
                o.fecha_autoriza_portal = i.fecha_autoriza_portal;
                o.descuento = i.descuento;
                o.validacion = i.validacion;
                o.uuid_original = i.uuid_original;
                o.bit_nc = i.bit_nc;
                o.bit_compu = i.bit_compu;
                o.bit_carga = i.bit_carga;
                o.compuesta = i.compuesta;
                o.email_carga = i.email_carga;
                o.comentario_rechazo = i.comentario_rechazo;
                o.uuid_rechazo = i.uuid_rechazo;
                o.total_xml = i.total_xml;
                o.fecha_autoriza_factura = i.fecha_autoriza_factura;
                o.usuario_autoriza = i.usuario_autoriza;
                o.bit_antnc = i.bit_antnc;
                o.nivel_aut = i.nivel_aut;
                o.cerrado = i.cerrado;
                o.ruta_rec_xml_depura = i.ruta_rec_xml_depura;
                o.ruta_rec_pdf_depura = i.ruta_rec_pdf_depura;
                o.fechaPropuesta = i.fechaPropuesta;
                o.autorizada = i.autorizada;
                o.fechaAutorizacion = i.fechaAutorizacion;
                o.programo = i.programo;
                o.autorizo = i.autorizo;
                converted.Add(o);
            }
            return converted;
        }
        public List<sp_genera_movprovDTO> sp_gastosEntityToGeneraDTO(List<tblC_sp_gastos_provDTO> data)
        {
            var converted = new List<sp_genera_movprovDTO>();
            foreach (var i in data)
            {
                var o = new sp_genera_movprovDTO();
                o.id = i.id;
                o.numpro = i.numpro;
                o.factura = i.factura;
                o.tm = i.tm;
                o.monto = i.total;
                o.cc = i.cc;
                o.monto_plan = 0;
                o.descTm = null;
                o.descTmp = null;
                o.descTmb = null;
                o.proveedor = null;
                o.descStatus = null;
                o.genEstado = null;
                o.genMonto = null;
                o.programo = i.programo;
                o.autorizo = i.autorizo;
                converted.Add(o);
            }
            return converted;
        }
        public List<sp_genera_movprovDTO> sp_gastosEntityToGeneraDTO(List<tblC_sp_gastos_prov> data, List<BancosDTO> bancos)
        {
            var converted = new List<sp_genera_movprovDTO>();
            foreach (var i in data)
            {
                var o = new sp_genera_movprovDTO();
                o.id = i.id;
                o.numpro = (int)i.numpro;
                o.factura = i.factura;
                o.tm = i.tm;
                o.monto = i.total;
                o.cc = i.cc;
                o.monto_plan = i.monto_plan;
                o.descTm = null;
                o.descTmp = null;
                o.descTmb = null;
                o.proveedor = null;
                o.descStatus = null;
                o.genEstado = null;
                o.genMonto = null;
                o.genEstado = i.fechaAutorizacion == null ? "PENDIENTE" : "AUTORIZADO";
                o.programo = i.programo;
                o.autorizo = i.autorizo;
                try
                {
                    var banco = bancos.FirstOrDefault(x => x.numpro == i.numpro);
                    o.cuenta = banco.cuenta;
                    o.banco = banco.bancoDesc;
                }
                catch (Exception e) { }

                converted.Add(o);
            }
            return converted;
        }
        public void updateEstatus()
        {
            var data = _context.tblC_sp_gastos_prov.Where(x => !x.autorizada && x.monto_plan == 0).ToList();
            foreach (var i in data)
            {
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = "select * from " + vSesiones.sesionEmpresaDBPregijo + @"sp_genera_movprov where numpro= ? and factura= ?",
                    parametros = paramTemp(i)
                };
                var temp = _contextEnkontrol.Select<sp_genera_movprovDTO>(EnkontrolAmbienteEnum.Prod, odbc);
                if (temp.Count > 0)
                {
                    i.autorizada = true;
                    i.fechaAutorizacion = temp.First().fecha_movto;
                    i.monto_plan = temp.First().monto_plan;
                }

                var odbc2 = new OdbcConsultaDTO()
                {
                    consulta = "select total from " + vSesiones.sesionEmpresaDBPregijo + @"sp_movprov where tm=51 and numpro= ? and factura= ?",
                    parametros = paramTemp(i)
                };
                var temp2 = _contextEnkontrol.Select<sp_movprovDTO>(EnkontrolAmbienteEnum.Prod, odbc2);
                if (temp2.Count > 0)
                {
                    i.autorizada = true;
                    i.monto_plan = temp2.First().total;
                }
            }
            _context.SaveChanges();

        }
        List<OdbcParameterDTO> paramTemp(tblC_sp_gastos_prov busq)
        {
            var lst = new List<OdbcParameterDTO>();
            lst.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.VarChar, valor = busq.numpro });
            lst.Add(new OdbcParameterDTO() { nombre = "factura", tipo = OdbcType.Numeric, valor = busq.factura });

            return lst;
        }

        #region Facturas duplicadas
        public List<Core.DTO.Enkontrol.Tablas.Poliza.sp_movprovDTO> getFacturasDuplicadas()
        {
            var query_sp_movprov = new OdbcConsultaDTO();

            query_sp_movprov.consulta = string.Format
                (
                    @"SELECT
    CASE
        WHEN TRIM(a.cfd_serie) = '' THEN NULL
        ELSE UPPER(TRIM(a.cfd_serie))
    END AS serie,
    a.*
FROM
    sp_movprov a
WHERE
    a.es_factura = 'S' and
    exists(
        select b.numpro,b.factura,b.cc,b.tm,b.referenciaoc,b.cfd_serie from sp_movprov b
        where b.es_factura='S' and b.numpro = a.numpro and b.factura = a.factura and b.cc = a.cc and b.tm = a.tm and b.referenciaoc = a.referenciaoc and b.cfd_serie = a.cfd_serie
        group by b.numpro,b.factura,b.cc,b.tm,b.referenciaoc,b.cfd_serie
        having count(*)>1
    )
                        "
                );

            var sp_movprov = _contextEnkontrol.Select<Core.DTO.Enkontrol.Tablas.Poliza.sp_movprovDTO>(_productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, query_sp_movprov);

            return sp_movprov;
        }
        #endregion

        #region Peru
        public List<FacturasProvDTO> getFacturasPendientesPeru(List<FacturasProvDTO> facturasPeru)
        {
            using (var dbContext = new MainContext(EmpresaEnum.Peru)) 
            {
                List<FacturasProvDTO> elementosBorrar = new List<FacturasProvDTO>();

                var result = dbContext.tblC_sp_gastos_prov.Where(x => x.numproPeru != null && x.numproPeru != "").ToList();
                foreach (var item in facturasPeru)
                {
                    var itemBorrar = result.FirstOrDefault(x => x.numproPeru == item.numproPeru && x.factura == item.factura);
                    if (itemBorrar != null) elementosBorrar.Add(item);
                }
                foreach (var item in elementosBorrar) facturasPeru.Remove(item);
            }

            return facturasPeru;
        }        

        public List<FacturasProvDTO> getLstFacturasProvPeru(BusqPropEkDTO busq)
        {
            try
            {
                List<FacturasProvDTO> lst = new List<FacturasProvDTO>();
                using (var conexion = new SqlConnection(ConextSigoDapper.conexionStarsoftBancos()))
                {
                    conexion.Open();
                    DynamicParameters lstParametros = new DynamicParameters();
                    lstParametros.Add("vence", busq.fechaCorte, DbType.Date);
                    lstParametros.Add("min", busq.min == null ? "" : busq.min, DbType.String);
                    lstParametros.Add("max", busq.max == null ? "" : busq.max, DbType.String);
                    lst = conexion.Query<FacturasProvDTO>(queryLstFacturasProvPeru(busq), lstParametros, null, true, 300, commandType: CommandType.Text).ToList();
                }
                var obrasPeru = _context.tblC_Cta_RelCC.Where(w => w.esActivo).ToList();
                var obrasMexico = _context.tblP_CC.ToList();
                var relacionItm = _context.tblC_TM_Relitm.Where(x => x.esActivo).ToList();
                foreach (var item in lst) 
                {
                    var _relacionItm = relacionItm.FirstOrDefault(x => x.PaliTmPeru == item.tmDesc && x.PaliTm == 1);

                    var obraCaptura = item.cc;
                    obraCaptura = (obraCaptura.Length > 0 && obraCaptura.Contains('/')) ? obraCaptura.Split('/')[1] : obraCaptura;
                    obraCaptura = obraCaptura.Length > 0 ? obraCaptura.Split(' ')[0] : "";

                    tblC_Cta_RelCC obra = obrasPeru.FirstOrDefault(cc => cc.ccPrincipal == obraCaptura);
                    if (obra == null) obra = obrasPeru.FirstOrDefault(cc => cc.ccSecundario == obraCaptura) ?? new tblC_Cta_RelCC();
                    item.cc = (obra.ccPrincipal == null || obra.ccPrincipal.Trim() == String.Empty) ? "010101" : obra.ccPrincipal;
                    var _ccMexico = obrasMexico.FirstOrDefault(x => x.cc == item.cc);
                    item.centroCostos = _ccMexico == null ? "" : _ccMexico.descripcion;
                    item.vence = DateTime.Parse(item.vence).ToShortDateString();
                    item.tm = _relacionItm == null ? 0 : _relacionItm.SeciTm;
                }

                return lst;
            }
            catch (Exception o_O) { return new List<FacturasProvDTO>(); }
        }
        
//        string queryLstFacturasProvPeru(BusqPropEkDTO busq)
//        {
//            string query = @"
//                SELECT 
//                    *, 
//                    CAST(Y.tm as varchar) + ' ' + tm.descripcion AS tmDesc 
//                FROM (
//                    SELECT
//                        MIN(id) AS id,
//                        numpro AS numpro,
//                        MIN(numproPeru) AS numproPeru,
//                        MIN(proveedor) AS proveedor,
//                        MIN(referenciaoc) AS referenciaoc,
//                        MIN(cc) AS cc,
//                        MIN(centroCostos) AS centroCostos,
//                        MIN(tm) AS tm,
//                        MIN(CASt(vence as date)) AS vence,
//                        factura,
//                        MIN(saldo * (-1)) AS saldo,
//                        MIN(monto_plan) AS monto_plan,
//                        MIN(concepto) AS concepto,
//                        MIN(moneda) AS moneda,
//                        MIN(autorizado) AS autorizado,
//                        MIN(tipocambio) AS tipocambio,
//                        MIN(idGiro) AS idGiro,
//                        MIN(activo_fijo) AS activo_fijo
//                    FROM (
//                        SELECT
//                            0 AS id,
//                            LEFT (cta.descripcion, CHARINDEX(' ', cta.descripcion)) AS numproPeru,
//                            mov.sscta AS numpro,
//                            RIGHT (cta.descripcion, LENGTH(cta.descripcion) - CHARINDEX(' ', cta.descripcion)) AS proveedor,
//                            0 AS referenciaoc,
//                            mov.cc AS cc,
//                            cc.descripcion AS centroCostos,
//                            itm AS tm,
//            
//                            pol.fechapol AS vence,
//                            mov.referencia AS factura,
//                            mov.monto AS saldo,
//                            mov.monto AS monto_plan,
//                            mov.concepto AS concepto,
//                            'MN' AS moneda,
//                            ' ' AS autorizado,
//                            1 AS tipocambio,
//                            0 AS idGiro,
//                            0 AS activo_fijo
//                        FROM 
//                            sc_movpol mov 
//                            LEFT JOIN sc_polizas pol 
//                            ON pol.year = mov.year AND pol.mes = mov.mes AND pol.tp = mov.tp AND pol.poliza = mov.poliza
//                            LEFT JOIN catcta cta
//                            ON mov.cta = cta.cta AND mov.scta = cta.scta AND mov.sscta = cta.sscta
//                            LEFT JOIN cc cc
//                            ON cc.cc = mov.cc
//            
//                        WHERE 
//                            mov.cta = 2105 
//                            AND mov.scta = 500 
//                            AND pol.fechapol <= ?
//                            AND mov.sscta BETWEEN ? AND ?                            
//                    ) X
//                    GROUP BY
//                        X.numpro, 
//                        X.factura
//                ) Y
//                LEFT JOIN 
//                    sb_tm tm
//                ON 
//                    Y.tm = tm.clave
//                WHERE 
//                    Y.saldo <> 0
//                    AND Y.cc IN {0}";
//            return string.Format(query, busq.lstCc.ToParamInValue());
//        }


        string queryLstFacturasProvPeru(BusqPropEkDTO busq)
        {
            var cuentasAplicaSoles = _context.tblC_CP_CuentasProveedores.Where(x => x.moneda == 1 && x.esActivo).Select(x => x.cta).ToList();
            var cuentasAplica = _context.tblC_CP_CuentasProveedores.Where(x => x.esActivo).Select(x => x.cta).ToList();

            string stringCuentasAplicaSoles = "'" + string.Join("','", cuentasAplicaSoles) + "'";
            string stringCuentasAplica = "'" + string.Join("','", cuentasAplica) + "'";
            string stringCC = "";
            if (busq.lstCc != null && busq.lstCc.Count() > 0) 
            {
                stringCC = " AND cc in ('" + string.Join("','", busq.lstCc) + "') ";
            }            

            string query = @"                
                
                SELECT * FROM (
	                SELECT 
		                0 as id,
		                0 as numpro,
		                RIGHT('00000000000'+ RTRIM(ISNULL(mov.DMOV_ANEXO, '0')), 11) as numproPeru,
		                MIN(prov.PRVCNOMBRE) as proveedor,
                        ISNULL(CASE WHEN MIN(c.CCFACGUI) = 'N' THEN 'OS-' + CAST(CAST(MIN(c.CCORDCOM) as int) as varchar) ELSE 'OC-' + CAST(CAST(MIN(d.CCORDCOM) as int) as varchar) END, 0) AS referenciaoc,
		                (SELECT top 1 (temp.DMOV_GLOSA) FROM (
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2022].dbo.DETMOV01 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2022].dbo.DETMOV02 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2022].dbo.DETMOV03 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2022].dbo.DETMOV04 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2022].dbo.DETMOV05 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2022].dbo.DETMOV06 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2022].dbo.DETMOV07 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2022].dbo.DETMOV08 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2022].dbo.DETMOV09 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2022].dbo.DETMOV10 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2022].dbo.DETMOV11 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2022].dbo.DETMOV12 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2023].dbo.DETMOV01 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2023].dbo.DETMOV02 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2023].dbo.DETMOV03 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2023].dbo.DETMOV04 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2023].dbo.DETMOV05 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2023].dbo.DETMOV06 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2023].dbo.DETMOV07 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2023].dbo.DETMOV08 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2023].dbo.DETMOV09 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2023].dbo.DETMOV10 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2023].dbo.DETMOV11 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2023].dbo.DETMOV12 WHERE DMOV_CUENT in ({1}))
						) temp WHERE temp.DMOV_ANEXO = mov.DMOV_ANEXO AND temp.DMOV_DOCUM = mov.DMOV_DOCUM AND temp.DMOV_FECHA = MIN(mov.DMOV_FECHA)) AS cc,
		                MIN(cc.CENCOST_DESCRIPCION) AS centroCostos,
		                LEFT(mov.DMOV_DOCUM, 2) AS tmDesc,            
		                ISNULL(MIN(mov.DMOV_FECVEN), GETDATE())  AS vence,
		                mov.DMOV_DOCUM as factura,
		                --SUM(mov.DMOV_HABER - mov.DMOV_DEBE) AS saldo,
                        CASE WHEN MIN(DMOV_CUENT) in ({0}) THEN SUM(mov.DMOV_HABER - mov.DMOV_DEBE) ELSE SUM(mov.DMOV_HABUS - mov.DMOV_DEBUS) END AS saldo,
		                --SUM(mov.DMOV_HABER - mov.DMOV_DEBE) AS monto_plan,
                        CASE WHEN MIN(DMOV_CUENT) in ({0}) THEN SUM(mov.DMOV_HABER - mov.DMOV_DEBE) ELSE SUM(mov.DMOV_HABUS - mov.DMOV_DEBUS) END AS monto_plan,
		                (SELECT top 1 (temp.DMOV_GLOSA) FROM (
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2022].dbo.DETMOV01 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2022].dbo.DETMOV02 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2022].dbo.DETMOV03 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2022].dbo.DETMOV04 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2022].dbo.DETMOV05 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2022].dbo.DETMOV06 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2022].dbo.DETMOV07 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2022].dbo.DETMOV08 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2022].dbo.DETMOV09 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2022].dbo.DETMOV10 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2022].dbo.DETMOV11 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2022].dbo.DETMOV12 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2023].dbo.DETMOV01 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2023].dbo.DETMOV02 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2023].dbo.DETMOV03 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2023].dbo.DETMOV04 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2023].dbo.DETMOV05 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2023].dbo.DETMOV06 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2023].dbo.DETMOV07 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2023].dbo.DETMOV08 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2023].dbo.DETMOV09 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2023].dbo.DETMOV10 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2023].dbo.DETMOV11 WHERE DMOV_CUENT in ({1})) UNION
							(SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA FROM [003BDCONT2023].dbo.DETMOV12 WHERE DMOV_CUENT in ({1}))
						) temp WHERE temp.DMOV_ANEXO = mov.DMOV_ANEXO AND temp.DMOV_DOCUM = mov.DMOV_DOCUM AND temp.DMOV_FECHA = MIN(mov.DMOV_FECHA)) AS concepto,
		                CASE WHEN MIN(DMOV_CUENT) in ({0}) THEN 'MN' ELSE 'DLL' END AS moneda,
		                ' ' AS autorizado,
		                CASE WHEN MIN(DMOV_CUENT) in ({0}) THEN 1 ELSE (CASE WHEN SUM(mov.DMOV_HABUS - mov.DMOV_DEBUS) > 0 THEN SUM(mov.DMOV_HABER - mov.DMOV_DEBE) / SUM(mov.DMOV_HABUS - mov.DMOV_DEBUS) ELSE 1 END) END AS tipocambio,
		                0 AS idGiro,
		                0 AS activo_fijo,
                        MIN(mov.SUBDIAR_CODIGO) AS tp
	                FROM (
		                (SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA, DMOV_CENCO, SUBDIAR_CODIGO, DMOV_HABUS, DMOV_DEBUS, DMOV_HABER, DMOV_DEBE, DMOV_CUENT, DMOV_FECVEN FROM [003BDCONT2022].dbo.DETMOV01 WHERE DMOV_CUENT in ({1})) UNION
		                (SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA, DMOV_CENCO, SUBDIAR_CODIGO, DMOV_HABUS, DMOV_DEBUS, DMOV_HABER, DMOV_DEBE, DMOV_CUENT, DMOV_FECVEN FROM [003BDCONT2022].dbo.DETMOV02 WHERE DMOV_CUENT in ({1})) UNION
		                (SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA, DMOV_CENCO, SUBDIAR_CODIGO, DMOV_HABUS, DMOV_DEBUS, DMOV_HABER, DMOV_DEBE, DMOV_CUENT, DMOV_FECVEN FROM [003BDCONT2022].dbo.DETMOV03 WHERE DMOV_CUENT in ({1})) UNION
		                (SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA, DMOV_CENCO, SUBDIAR_CODIGO, DMOV_HABUS, DMOV_DEBUS, DMOV_HABER, DMOV_DEBE, DMOV_CUENT, DMOV_FECVEN FROM [003BDCONT2022].dbo.DETMOV04 WHERE DMOV_CUENT in ({1})) UNION
		                (SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA, DMOV_CENCO, SUBDIAR_CODIGO, DMOV_HABUS, DMOV_DEBUS, DMOV_HABER, DMOV_DEBE, DMOV_CUENT, DMOV_FECVEN FROM [003BDCONT2022].dbo.DETMOV05 WHERE DMOV_CUENT in ({1})) UNION
		                (SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA, DMOV_CENCO, SUBDIAR_CODIGO, DMOV_HABUS, DMOV_DEBUS, DMOV_HABER, DMOV_DEBE, DMOV_CUENT, DMOV_FECVEN FROM [003BDCONT2022].dbo.DETMOV06 WHERE DMOV_CUENT in ({1})) UNION
		                (SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA, DMOV_CENCO, SUBDIAR_CODIGO, DMOV_HABUS, DMOV_DEBUS, DMOV_HABER, DMOV_DEBE, DMOV_CUENT, DMOV_FECVEN FROM [003BDCONT2022].dbo.DETMOV07 WHERE DMOV_CUENT in ({1})) UNION
		                (SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA, DMOV_CENCO, SUBDIAR_CODIGO, DMOV_HABUS, DMOV_DEBUS, DMOV_HABER, DMOV_DEBE, DMOV_CUENT, DMOV_FECVEN FROM [003BDCONT2022].dbo.DETMOV08 WHERE DMOV_CUENT in ({1})) UNION
		                (SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA, DMOV_CENCO, SUBDIAR_CODIGO, DMOV_HABUS, DMOV_DEBUS, DMOV_HABER, DMOV_DEBE, DMOV_CUENT, DMOV_FECVEN FROM [003BDCONT2022].dbo.DETMOV09 WHERE DMOV_CUENT in ({1})) UNION
		                (SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA, DMOV_CENCO, SUBDIAR_CODIGO, DMOV_HABUS, DMOV_DEBUS, DMOV_HABER, DMOV_DEBE, DMOV_CUENT, DMOV_FECVEN FROM [003BDCONT2022].dbo.DETMOV10 WHERE DMOV_CUENT in ({1})) UNION
		                (SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA, DMOV_CENCO, SUBDIAR_CODIGO, DMOV_HABUS, DMOV_DEBUS, DMOV_HABER, DMOV_DEBE, DMOV_CUENT, DMOV_FECVEN FROM [003BDCONT2022].dbo.DETMOV11 WHERE DMOV_CUENT in ({1})) UNION
		                (SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA, DMOV_CENCO, SUBDIAR_CODIGO, DMOV_HABUS, DMOV_DEBUS, DMOV_HABER, DMOV_DEBE, DMOV_CUENT, DMOV_FECVEN FROM [003BDCONT2022].dbo.DETMOV12 WHERE DMOV_CUENT in ({1})) UNION
		                (SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA, DMOV_CENCO, SUBDIAR_CODIGO, DMOV_HABUS, DMOV_DEBUS, DMOV_HABER, DMOV_DEBE, DMOV_CUENT, DMOV_FECVEN FROM [003BDCONT2023].dbo.DETMOV01 WHERE DMOV_CUENT in ({1})) UNION
		                (SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA, DMOV_CENCO, SUBDIAR_CODIGO, DMOV_HABUS, DMOV_DEBUS, DMOV_HABER, DMOV_DEBE, DMOV_CUENT, DMOV_FECVEN FROM [003BDCONT2023].dbo.DETMOV02 WHERE DMOV_CUENT in ({1})) UNION
		                (SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA, DMOV_CENCO, SUBDIAR_CODIGO, DMOV_HABUS, DMOV_DEBUS, DMOV_HABER, DMOV_DEBE, DMOV_CUENT, DMOV_FECVEN FROM [003BDCONT2023].dbo.DETMOV03 WHERE DMOV_CUENT in ({1})) UNION
		                (SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA, DMOV_CENCO, SUBDIAR_CODIGO, DMOV_HABUS, DMOV_DEBUS, DMOV_HABER, DMOV_DEBE, DMOV_CUENT, DMOV_FECVEN FROM [003BDCONT2023].dbo.DETMOV04 WHERE DMOV_CUENT in ({1})) UNION
		                (SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA, DMOV_CENCO, SUBDIAR_CODIGO, DMOV_HABUS, DMOV_DEBUS, DMOV_HABER, DMOV_DEBE, DMOV_CUENT, DMOV_FECVEN FROM [003BDCONT2023].dbo.DETMOV05 WHERE DMOV_CUENT in ({1})) UNION
		                (SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA, DMOV_CENCO, SUBDIAR_CODIGO, DMOV_HABUS, DMOV_DEBUS, DMOV_HABER, DMOV_DEBE, DMOV_CUENT, DMOV_FECVEN FROM [003BDCONT2023].dbo.DETMOV06 WHERE DMOV_CUENT in ({1})) UNION
		                (SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA, DMOV_CENCO, SUBDIAR_CODIGO, DMOV_HABUS, DMOV_DEBUS, DMOV_HABER, DMOV_DEBE, DMOV_CUENT, DMOV_FECVEN FROM [003BDCONT2023].dbo.DETMOV07 WHERE DMOV_CUENT in ({1})) UNION
		                (SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA, DMOV_CENCO, SUBDIAR_CODIGO, DMOV_HABUS, DMOV_DEBUS, DMOV_HABER, DMOV_DEBE, DMOV_CUENT, DMOV_FECVEN FROM [003BDCONT2023].dbo.DETMOV08 WHERE DMOV_CUENT in ({1})) UNION
		                (SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA, DMOV_CENCO, SUBDIAR_CODIGO, DMOV_HABUS, DMOV_DEBUS, DMOV_HABER, DMOV_DEBE, DMOV_CUENT, DMOV_FECVEN FROM [003BDCONT2023].dbo.DETMOV09 WHERE DMOV_CUENT in ({1})) UNION
		                (SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA, DMOV_CENCO, SUBDIAR_CODIGO, DMOV_HABUS, DMOV_DEBUS, DMOV_HABER, DMOV_DEBE, DMOV_CUENT, DMOV_FECVEN FROM [003BDCONT2023].dbo.DETMOV10 WHERE DMOV_CUENT in ({1})) UNION
		                (SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA, DMOV_CENCO, SUBDIAR_CODIGO, DMOV_HABUS, DMOV_DEBUS, DMOV_HABER, DMOV_DEBE, DMOV_CUENT, DMOV_FECVEN FROM [003BDCONT2023].dbo.DETMOV11 WHERE DMOV_CUENT in ({1})) UNION
		                (SELECT DMOV_GLOSA, DMOV_ANEXO, DMOV_DOCUM, DMOV_FECHA, DMOV_CENCO, SUBDIAR_CODIGO, DMOV_HABUS, DMOV_DEBUS, DMOV_HABER, DMOV_DEBE, DMOV_CUENT, DMOV_FECVEN FROM [003BDCONT2023].dbo.DETMOV12 WHERE DMOV_CUENT in ({1}))
	                ) mov
	                LEFT JOIN [003BDCOMUN].dbo.MAEPROV prov ON RIGHT('00000000000'+ RTRIM(ISNULL(mov.DMOV_ANEXO, '0')), 11) = prov.PRVCCODIGO
	                LEFT JOIN [003BDCONTABILIDAD].dbo.CENTRO_COSTOS cc ON mov.DMOV_CENCO = cc.CENCOST_CODIGO
					LEFT JOIN [003BDCOMUN].dbo.COMPROBANTECAB comp ON 
						TRIM(LEFT(TRIM(mov.DMOV_DOCUM), 2)) = comp.TIPODOCU_CODIGO 
						AND TRIM(CASE WHEN LEN(TRIM(mov.DMOV_DOCUM)) >= 6 THEN RIGHT(LEFT(TRIM(mov.DMOV_DOCUM), 6), 4) ELSE '' END) = comp.CSERIE 
						AND TRIM(CASE WHEN LEN(TRIM(mov.DMOV_DOCUM)) >= 6 THEN RIGHT(TRIM(mov.DMOV_DOCUM), LEN(TRIM(mov.DMOV_DOCUM)) - 6) ELSE '' END) = comp.CNUMERO
						AND TRIM(CASE WHEN LEN(mov.DMOV_ANEXO) >= 2 THEN  RIGHT(mov.DMOV_ANEXO, LEN(mov.DMOV_ANEXO) - 2) ELSE '' END) = comp.ANEX_CODIGO
					LEFT JOIN [003BDCOMUN].dbo.COMCAB c ON comp.TIPODOCU_CODIGO = c.CCTD And comp.CSERIE = c.CCNUMSER And comp.CNUMERO = c.CCNUMDOC AND comp.ANEX_CODIGO = c.CCCODPRO 
                    LEFT JOIN [003BDCOMUN].dbo.COMGUICAB d ON c.ID_COMCAB = d.CCNROFACTURA 
	                GROUP BY mov.DMOV_DOCUM, mov.DMOV_ANEXO
                ) X
                WHERE X.vence <= @vence AND X.saldo > 0 AND X.numproPeru BETWEEN @min AND @max
                ORDER BY X.moneda DESC, X.numproPeru
                ";
            return string.Format(query, stringCuentasAplicaSoles, stringCuentasAplica, stringCC);
        }
        

        string queryLstFacturasProvPeruTodas()
        {
            string query = @"
                SELECT 
                    *, 
                    CAST(Y.tm as varchar) + ' ' + tm.descripcion AS tmDesc 
                FROM (
                    SELECT
                            0 AS id,
                            LEFT (cta.descripcion, CHARINDEX(' ', cta.descripcion)) AS numproPeru,
                            mov.sscta AS numpro,
                            RIGHT (cta.descripcion, LENGTH(cta.descripcion) - CHARINDEX(' ', cta.descripcion)) AS proveedor,
                            0 AS referenciaoc,
                            mov.cc AS cc,
                            cc.descripcion AS centroCostos,
                            itm AS tm,
    
                            pol.fechapol AS vence,
                            mov.referencia AS factura,
                            mov.monto AS saldo,
                            mov.monto AS monto_plan,
                            mov.concepto AS concepto,
                            'MN' AS moneda,
                            ' ' AS autorizado,
                            1 AS tipocambio,
                            0 AS idGiro,
                            0 AS activo_fijo,
                            mov.SUBDIAR_CODIGO AS tp
                        FROM 
                            sc_movpol mov 
                            LEFT JOIN sc_polizas pol 
                            ON pol.year = mov.year AND pol.mes = mov.mes AND pol.tp = mov.tp AND pol.poliza = mov.poliza
                            LEFT JOIN catcta cta
                            ON mov.cta = cta.cta AND mov.scta = cta.scta AND mov.sscta = cta.sscta
                            LEFT JOIN cc cc
                            ON cc.cc = mov.cc
    
                        WHERE 
                            mov.cta = 2105 
                            AND mov.scta = 500        
                    ) Y
                    LEFT JOIN 
                        sb_tm tm
                    ON 
                        Y.tm = tm.clave
                    WHERE 
                        Y.tm < 20";
            return string.Format(query);
        }
        #endregion

        #region Colombia

        string queryLstFacturasProvColombia(BusqPropEkDTO busq)
        {
            string query = @"
                SELECT
                    x.*,
                    ";
            if (vSesiones.sesionEmpresaActual == 2)
            {
                query += @"(select case when count(*) > 0 then 1 else 0 end from " + vSesiones.sesionEmpresaDBPregijo + @"so_orden_Compra_det comp where comp.cc=x.cc and comp.numero=x.referenciaoc and substring(comp.insumo,0,3) in ('701','702','703')) as activo_fijo";
            }
            else
            {
                query += @"0 as activo_fijo";
            }

            query += @" FROM (
                    SELECT
                        mov.numpro, mov.factura, mov.cc, MIN(mov.tm) AS tm, MIN(mov.fechavenc) AS fecha, MAX(mov.referenciaoc) AS referenciaoc, MAX(mov.concepto) AS concepto,
                        SUM(mov.total) AS total, mov.autorizapago AS estatus, mov.tipocambio, MOV.fecha AS fechaTimbrado, MOV.fecha as fechaValidacion
                    FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_movprov mov ";

            query += @"
                    WHERE (
                        (mov.autorizapago NOT IN ('A','E') AND mov.es_factura = 'S' AND mov.fechavenc <= ?) OR
                        (mov.es_factura='N' AND mov.autorizapago IN ('A','E'))
                    ) AND mov.numpro BETWEEN ? AND ? ";

            //            if (busq.tipo)
            //            {
            //                query += @" AND NOT EXISTS (
            //                    SELECT
            //                        *
            //                    FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_gastos_prov gas
            //                    WHERE (mov.numpro = gas.numpro AND mov.cc = gas.cc AND mov.factura = gas.factura)
            //                ) ";
            //            }

            if (vSesiones.sesionEmpresaActual != 2)
            {
                query += @" AND mov.cc IN {0} ";
            }

            query += @" AND mov.factura not in (select factura FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_movprov where (tm=51 or tm=26) and numpro=mov.numpro and cc=mov.cc group by factura having sum(abs(total))=mov.total)
                                   -- AND bit_factoraje='N'
                                group by mov.numpro ,mov.factura ,mov.cc ,mov.autorizapago, mov.tipocambio,mov.fecha order by mov.numpro,mov.factura) x where (x.total * x.tipocambio)>1";

            if (vSesiones.sesionEmpresaActual != 2)
            {
                return string.Format(query, busq.lstCc.ToParamInValue());
            }
            else
            {
                return string.Format(query);
            }
        }
        #endregion

        #region Autorizacion Facturas
        public List<ProgrPagoDTO> GetFacturasPendientes(string min, string max, List<string> cc, int tipo)
        {
            var dataSIGO = new List<tblC_sp_gastos_provDTO>();
            var _min = Convert.ToInt32(min);
            var _max = Convert.ToInt32(max);

            string consulta = string.Format(@"select * FROM {0} sp_gastos_prov WHERE estatus = ? AND numpro BETWEEN ? AND ? AND cc in {1}", vSesiones.sesionEmpresaDBPregijo, cc.ToParamInValue());
            List<OdbcParameterDTO> parametros = new List<OdbcParameterDTO>();

            parametros.Add(new OdbcParameterDTO() { nombre = "estatus", tipo = OdbcType.VarChar, valor = tipo == 1 ? "C" : "P" });
            parametros.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.VarChar, valor = _min });
            parametros.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.VarChar, valor = _max });
            parametros.AddRange(cc.Select(s => new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = s }).ToList());

            var odbc = new OdbcConsultaDTO()
            {
                consulta = consulta,
                parametros = parametros
            };
            dataSIGO = _contextEnkontrol.Select<tblC_sp_gastos_provDTO>(EnkontrolAmbienteEnum.Prod, odbc);        

            #region Se agregan los manuales.

            var listaTM = _contextEnkontrol.Select<dynamic>(EnkontrolAmbienteEnum.Prod, new OdbcConsultaDTO() { consulta = string.Format(@"SELECT * FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_tm") });
            var listaAreaCuenta = _contextEnkontrol.Select<dynamic>(EnkontrolAmbienteEnum.Prod, new OdbcConsultaDTO() { consulta = string.Format(@"SELECT * FROM " + vSesiones.sesionEmpresaDBPregijo + @"si_area_cuenta") });

            #endregion

            var lstTmp = _contextEnkontrol.Select<ComboDTO>(EnkontrolAmbienteEnum.Prod, "SELECT tm AS Value, descripcion AS Text FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_tm");
            var lstTmb = _contextEnkontrol.Select<ComboDTO>(EnkontrolAmbienteEnum.Prod, "SELECT clave AS Value, descripcion AS Text FROM " + vSesiones.sesionEmpresaDBPregijo + @"sb_tm");


            var lstProveedores = getComboProveedores().Where(x => dataSIGO.Any(p => p.numpro.ToString().Equals(x.Value))).ToList();
            List<Core.DTO.Principal.Generales.ComboDTO> listaProveedoresPeru = new List<Core.DTO.Principal.Generales.ComboDTO>();

            var data = dataSIGO.Select(item =>
            {
                var proveedor = lstProveedores.FirstOrDefault(p => p.Value.Equals(item.numpro.ToString()));
                ProgrPagoDTO _item = new ProgrPagoDTO()
                {
                    id = item.id,
                    cc = item.cc,
                    concepto = item.concepto,
                    factura = item.factura,
                    maxPago = item.monto,
                    monto = item.monto,
                    oc = item.referenciaoc,
                    pdf = item.ruta_rec_pdf,
                    xml = item.ruta_rec_xml,
                    proveedor = proveedor == null ? item.numpro.ToString() : proveedor.Text,
                    proveedorID = item.numpro.ToString(),
                    recibido = item.monto,
                    tipocambio = item.tipocambio ?? 1,
                    saldo = item.monto,
                    pagado = 0,
                    solicitado = item.monto,
                    tipoMoneda = item.moneda,
                    tm = item.tm,
                    tmb = item.tm == 0 ? 53 : item.tm,
                    tmbDescripcion = lstTmp.FirstOrDefault(tm => tm.Value.Equals((item.tm == 0 ? 53 : item.tm))).Text ?? string.Empty,
                    tmp = item.tm == 0 ? 51 : item.tm,
                    tmpDescripcion = lstTmp.FirstOrDefault(tm => tm.Value.Equals((item.tm == 0 ? 51 : item.tm))).Text ?? string.Empty,
                    vence = item.fecha,
                    monto_plan = item.monto,
                    esPagado = false,
                    ac = "",
                    acDesc = "",
                    activo_fijo = false,
                    iva = item.iva,
                    numproPeru = "",
                    estatus = item.estatus
                };
                return _item;
            }).OrderBy(o => o.proveedorID).ToList();

            return data;
        }

        public string GetRutaRequerimiento(string cc, int numero)
        {
            string ruta = "";

            using (var dbContext = new MainContextPortal())
            {
                try
                {
                    var objRequerimiento = dbContext.RequirementPurchaseOrder.Where(e => e.CeCo == cc && e.PurchaseOrderID == numero).OrderByDescending(e => e.ID).FirstOrDefault();

                    ruta = objRequerimiento.RequirementValue;
                }
                catch (Exception e)
                {
                    
                    throw e;
                }
            }

            return ruta;
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

        public Dictionary<string, object> ValidarFactura(List<FiltroValidacionDTO> lstFiltro)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            using (var dbTransac = _context.Database.BeginTransaction())
            {
                using (var con = checkConexionProductivo())
                {
                    using (var trans = con.BeginTransaction())
                    {
                        decimal cambioDls = GetTipoCambioNow();

                        foreach (var item in lstFiltro)
                        {
                            try
                            {
                                if (item.esAuth)
                                {

                                    #region INFO FACTURA / OC

                                    var queryEk = new OdbcConsultaDTO();
                                    queryEk.consulta = "SELECT * FROM sp_gastos_prov WHERE id = ?";
                                    queryEk.parametros.Add(new OdbcParameterDTO
                                    {
                                        nombre = "id",
                                        tipo = OdbcType.Int,
                                        valor = item.id
                                    });

                                    var objGasto = _contextEnkontrol.Select<sp_gastos_prov>(vSesiones.sesionAmbienteEnkontrolAdm, queryEk).FirstOrDefault();

                                    queryEk = new OdbcConsultaDTO();
                                    queryEk.consulta = "SELECT * FROM so_orden_compra WHERE numero = ? AND cc = ?";
                                    queryEk.parametros.Add(new OdbcParameterDTO
                                    {
                                        nombre = "numero",
                                        tipo = OdbcType.Int,
                                        valor = objGasto.referenciaoc
                                    });
                                    queryEk.parametros.Add(new OdbcParameterDTO
                                    {
                                        nombre = "cc",
                                        tipo = OdbcType.NVarChar,
                                        valor = objGasto.cc
                                    });

                                    var objOrdenCompra = _contextEnkontrol.Select<OrdenCompraValidacionDTO>(vSesiones.sesionAmbienteEnkontrolAdm, queryEk).FirstOrDefault();

                                    queryEk = new OdbcConsultaDTO();
                                    queryEk.consulta = "SELECT TOP 1 * FROM so_orden_compra_det WHERE numero = ? AND cc = ?";
                                    queryEk.parametros.Add(new OdbcParameterDTO
                                    {
                                        nombre = "numero",
                                        tipo = OdbcType.Int,
                                        valor = objGasto.referenciaoc
                                    });
                                    queryEk.parametros.Add(new OdbcParameterDTO
                                    {
                                        nombre = "cc",
                                        tipo = OdbcType.NVarChar,
                                        valor = objGasto.cc
                                    });

                                    var objOrdenCompraDet = _contextEnkontrol.Select<dynamic>(vSesiones.sesionAmbienteEnkontrolAdm, queryEk).FirstOrDefault();

                                    queryEk = new OdbcConsultaDTO();
                                    queryEk.consulta = "SELECT condpago FROM sp_proveedores WHERE numpro = ?";
                                    queryEk.parametros.Add(new OdbcParameterDTO
                                    {
                                        nombre = "numpro",
                                        tipo = OdbcType.Int,
                                        valor = objGasto.numpro
                                    });

                                    var condicionPagoProv = _contextEnkontrol.Select<dynamic>(vSesiones.sesionAmbienteEnkontrolAdm, queryEk).FirstOrDefault();

                                    int numCondPago = Convert.ToInt32(condicionPagoProv.condpago);

                                    var fechaVenc = DateTime.Now.AddDays(numCondPago);
                                    #endregion

                                    #region ARCHIVOS Y LECTURA DE XML
                                    var objXml = new FacturaXmlDTO();

                                    string RutaBasePortalXml = @"\\10.1.0.100\Portal\xml\FileCopyPruebas\" + Path.GetFileName(objGasto.ruta_rec_xml);
                                    string RutaBasePortalPdf = @"\\10.1.0.100\Portal\xml\FileCopyPruebas\" + Path.GetFileName(objGasto.ruta_rec_pdf);
#if DEBUG
                                    RutaBasePortalXml = objGasto.ruta_rec_xml;
#endif

                                    #region LEER XML
                                    using (FileStream sr = File.OpenRead(RutaBasePortalXml))
                                    {

                                        XNamespace cfdi = "http://www.sat.gob.mx/cfd/4";
                                        XNamespace tfd = "http://www.sat.gob.mx/TimbreFiscalDigital";
                                        XElement factura = XElement.Load(sr);

                                        //ATRIBUTOS DEL ROOT
                                        List<XAttribute> attRoot =
                                        (from at in factura.Attributes()
                                         select at).ToList();

                                        //ATRIBUTOS DE RECEPTOR
                                        List<XElement> lstReceptores = (
                                            from el in factura.Elements(cfdi + "Receptor")
                                            select el).ToList();

                                        var objReceptor = lstReceptores.FirstOrDefault();

                                        List<XAttribute> attReceptor =
                                        (from at in objReceptor.Attributes()
                                         select at).ToList();

                                        //ATRIBUTOS DE EMISOR
                                        List<XElement> lstEmisor = (
                                            from el in factura.Elements(cfdi + "Emisor")
                                            select el).ToList();

                                        var objEmisor = lstEmisor.FirstOrDefault();

                                        List<XAttribute> attEmisor =
                                        (from at in objEmisor.Attributes()
                                         select at).ToList();

                                        //ATRIBUTOS DE TIMBRE FISCAL
                                        List<XElement> lstPartidas = (
                                            from el in factura.Elements(cfdi + "Conceptos").Elements(cfdi + "Concepto")
                                            select el).ToList();

                                        List<XAttribute> attPartida =
                                            (from at in lstPartidas.FirstOrDefault().Attributes()
                                             select at).ToList();

                                        objXml.Descripcion = attPartida.FirstOrDefault(e => e.Name == "Descripcion").Value.Length > 40 ? attPartida.FirstOrDefault(e => e.Name == "Descripcion").Value.Substring(0,40) : attPartida.FirstOrDefault(e => e.Name == "Descripcion").Value;
                                        objXml.Folio = Convert.ToInt64(attRoot.FirstOrDefault(e => e.Name == "Folio").Value);
                                        objXml.Folio = Convert.ToInt64(attRoot.FirstOrDefault(e => e.Name == "Folio").Value);
                                        objXml.Fecha = Convert.ToDateTime(attRoot.FirstOrDefault(e => e.Name == "Fecha").Value);
                                        objXml.RFCEmisor = attEmisor.FirstOrDefault(e => e.Name == "Rfc").Value;
                                        objXml.Total = Convert.ToDecimal(attRoot.FirstOrDefault(e => e.Name == "Total").Value);
                                        objXml.TipoCambio = attRoot.Any(e => e.Name == "TipoCambio") ? Convert.ToDecimal(attRoot.FirstOrDefault(e => e.Name == "TipoCambio").Value) : 1M;
                                        objXml.Moneda = attRoot.FirstOrDefault(e => e.Name == "Moneda").Value;
                                        objXml.Serie = attRoot.Any(e => e.Name == "Serie") ? attRoot.FirstOrDefault(e => e.Name == "Serie").Value : "";
                                        objXml.Certificado = attRoot.Any(e => e.Name == "NoCertificado") ? attRoot.FirstOrDefault(e => e.Name == "NoCertificado").Value : "";
                                    }
                                    #endregion

                                    #region GUARDAR ARCHIVOS EN EK

                                    //GET RFC DE LA EMPRESA
                                    string rfcEmpresa = "";
                                    switch (vSesiones.sesionEmpresaActual)
                                    {
                                        case 1:
                                            rfcEmpresa = "GCP800324FJ1";
                                            break;

                                        case 2:
                                            rfcEmpresa = "ACO171207CZ7";
                                            break;

                                        default:
                                            rfcEmpresa = "GCP800324FJ1";
                                            break;
                                    }

                                    var lstArchivos = GuardarArchivos(RutaBasePortalXml, RutaBasePortalPdf, rfcEmpresa, objXml.Folio);

                                    string rutaXML = lstArchivos.FirstOrDefault(e => e.tipoArchivo == TipoArchivoEnum.Xml).ruta;
                                    string rutaPdf = lstArchivos.FirstOrDefault(e => e.tipoArchivo == TipoArchivoEnum.Factura).ruta;
                                    #endregion

                                    #endregion

                                    #region POLIZA
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

                                    var objPol = new Core.DTO.Enkontrol.Tablas.Poliza.sc_polizasDTO()
                                    {
                                        year = DateTime.Now.Year,
                                        mes = DateTime.Now.Month,
                                        tp = "07",
                                        fechapol = DateTime.Now,
                                        cargos = objGasto.total,
                                        abonos = -(objGasto.total),
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
                                        concepto = "Póliza de Proveedores",
                                    };

                                    var objMovs = new List<Core.DTO.Enkontrol.Tablas.Poliza.sc_movpolDTO>();

                                    #region DETALLE POL
                                    if (objGasto.moneda == "MXN")
                                    {
                                        #region MN

                                        #region LN 1
                                        objMovs.Add(new Core.DTO.Enkontrol.Tablas.Poliza.sc_movpolDTO
                                        {
                                            year = DateTime.Now.Year,
                                            mes = DateTime.Now.Month,
                                            tp = "07",
                                            linea = 1,
                                            cta = 2105,
                                            scta = 1,
                                            sscta = 0,
                                            digito = 0,
                                            tm = 2,
                                            referencia = item.factura.ToString(),
                                            cc = objGasto.cc,
                                            concepto = "",
                                            monto = -(objGasto.total),
                                            iclave = 0,
                                            itm = item.tm,
                                            st_par = "",
                                            orden_compra = objOrdenCompra.numero,
                                            numpro = objOrdenCompra.proveedor,
                                            socio_inversionista = null,
                                            istm = null,
                                            folio_imp = null,
                                            linea_imp = null,
                                            num_emp = null,
                                            folio_gxc = null,
                                            cfd_ruta_pdf = rutaPdf,
                                            cfd_ruta_xml = rutaXML,
                                            UUID = objXml.UUID, 
                                            cfd_rfc = objXml.RFCEmisor, 
                                            cfd_tipocambio = objXml.TipoCambio, 
                                            cfd_total = objXml.Total,
                                            cfd_moneda = objXml.Moneda,
                                            metodo_pago_sat = null,
                                            ruta_comp_ext = null,
                                            taxid = null,
                                            forma_pago = null,
                                            cfd_fecha_expedicion = null,
                                            cfd_tipocomprobante = null,
                                            cfd_metodo_pago_sat = null,
                                            area = objOrdenCompraDet.area ?? 0,
                                            cuenta_oc = objOrdenCompraDet.cuenta ?? 0,
                                        });
                                        #endregion

                                        #region LN 2
                                        objMovs.Add(new Core.DTO.Enkontrol.Tablas.Poliza.sc_movpolDTO
                                        {
                                            year = DateTime.Now.Year,
                                            mes = DateTime.Now.Month,
                                            tp = "07",
                                            linea = 2,
                                            cta = 1146,
                                            scta = 4,
                                            sscta = 0,
                                            digito = 0,
                                            tm = 1,
                                            referencia = item.factura.ToString(),
                                            cc = objGasto.cc,
                                            concepto = "",
                                            monto = objGasto.iva,
                                            iclave = 0,
                                            itm = item.tm,
                                            st_par = "",
                                            orden_compra = objOrdenCompra.numero,
                                            numpro = objOrdenCompra.proveedor,
                                            socio_inversionista = null,
                                            istm = null,
                                            folio_imp = null,
                                            linea_imp = null,
                                            num_emp = null,
                                            folio_gxc = null,
                                            cfd_ruta_pdf = null,
                                            cfd_ruta_xml = null,
                                            UUID = null,
                                            cfd_rfc = null,
                                            cfd_tipocambio = null,
                                            cfd_total = null,
                                            cfd_moneda = null,
                                            metodo_pago_sat = null,
                                            ruta_comp_ext = null,
                                            taxid = null,
                                            forma_pago = null,
                                            cfd_fecha_expedicion = null,
                                            cfd_tipocomprobante = null,
                                            cfd_metodo_pago_sat = null,
                                            area = objOrdenCompraDet.area ?? 0,
                                            cuenta_oc = objOrdenCompraDet.cuenta ?? 0,
                                        });
                                        #endregion

                                        #region LN 3
                                        objMovs.Add(new Core.DTO.Enkontrol.Tablas.Poliza.sc_movpolDTO
                                        {
                                            year = DateTime.Now.Year,
                                            mes = DateTime.Now.Month,
                                            tp = "07",
                                            linea = 3,
                                            cta = 5000,
                                            scta = 6,
                                            sscta = 2,
                                            digito = 0,
                                            tm = 1,
                                            referencia = item.factura.ToString(),
                                            cc = objGasto.cc,
                                            concepto = "",
                                            monto = objGasto.monto,
                                            iclave = 0,
                                            itm = item.tm,
                                            st_par = "",
                                            orden_compra = objOrdenCompra.numero,
                                            numpro = objOrdenCompra.proveedor,
                                            socio_inversionista = null,
                                            istm = null,
                                            folio_imp = null,
                                            linea_imp = null,
                                            num_emp = null,
                                            folio_gxc = null,
                                            cfd_ruta_pdf = null,
                                            cfd_ruta_xml = null,
                                            UUID = null,
                                            cfd_rfc = null,
                                            cfd_tipocambio = null,
                                            cfd_total = null,
                                            cfd_moneda = null,
                                            metodo_pago_sat = null,
                                            ruta_comp_ext = null,
                                            taxid = null,
                                            forma_pago = null,
                                            cfd_fecha_expedicion = null,
                                            cfd_tipocomprobante = null,
                                            cfd_metodo_pago_sat = null,
                                            area = objOrdenCompraDet.area ?? 0,
                                            cuenta_oc = objOrdenCompraDet.cuenta ?? 0,
                                        });
                                        #endregion
                                        
                                        #endregion
                                    }
                                    else
                                    {
                                        decimal ivaDLStoMN = objGasto.iva * cambioDls;
                                        decimal montoDLStoMN = objGasto.monto * cambioDls;
                                        decimal totalDLStoMN = objGasto.total * cambioDls;
                                        decimal montoComp = totalDLStoMN - objGasto.total;

                                        #region DOLORES
                                        #region LN 1
                                        objMovs.Add(new Core.DTO.Enkontrol.Tablas.Poliza.sc_movpolDTO
                                        {
                                            year = DateTime.Now.Year,
                                            mes = DateTime.Now.Month,
                                            tp = "07",
                                            linea = 1,
                                            cta = 2105,
                                            scta = 2,
                                            sscta = 0,
                                            digito = 0,
                                            tm = 2,
                                            referencia = item.factura.ToString(),
                                            cc = objGasto.cc,
                                            concepto = "",
                                            monto = -(objGasto.total),
                                            iclave = 0,
                                            itm = item.tm,
                                            st_par = "",
                                            orden_compra = objOrdenCompra.numero,
                                            numpro = objOrdenCompra.proveedor,
                                            socio_inversionista = null,
                                            istm = null,
                                            folio_imp = null,
                                            linea_imp = null,
                                            num_emp = null,
                                            folio_gxc = null,
                                            cfd_ruta_pdf = rutaPdf,
                                            cfd_ruta_xml = rutaXML,
                                            UUID = objXml.UUID,
                                            cfd_rfc = objXml.RFCEmisor,
                                            cfd_tipocambio = objXml.TipoCambio,
                                            cfd_total = objXml.Total,
                                            cfd_moneda = objXml.Moneda,
                                            metodo_pago_sat = null,
                                            ruta_comp_ext = null,
                                            taxid = null,
                                            forma_pago = null,
                                            cfd_fecha_expedicion = null,
                                            cfd_tipocomprobante = null,
                                            cfd_metodo_pago_sat = null,
                                            area = objOrdenCompraDet.area ?? 0,
                                            cuenta_oc = objOrdenCompraDet.cuenta ?? 0,
                                        });
                                        #endregion

                                        #region LN 2
                                        objMovs.Add(new Core.DTO.Enkontrol.Tablas.Poliza.sc_movpolDTO
                                        {
                                            year = DateTime.Now.Year,
                                            mes = DateTime.Now.Month,
                                            tp = "07",
                                            linea = 2,
                                            cta = 2105,
                                            scta = 200,
                                            sscta = 4,
                                            digito = 0,
                                            tm = 2,
                                            referencia = item.factura.ToString(),
                                            cc = objGasto.cc,
                                            concepto = "",
                                            monto = -(montoComp),
                                            iclave = 0,
                                            itm = item.tm,
                                            st_par = "",
                                            orden_compra = objOrdenCompra.numero,
                                            numpro = objOrdenCompra.proveedor,
                                            socio_inversionista = null,
                                            istm = null,
                                            folio_imp = null,
                                            linea_imp = null,
                                            num_emp = null,
                                            folio_gxc = null,
                                            cfd_ruta_pdf = null,
                                            cfd_ruta_xml = null,
                                            UUID = objXml.UUID,
                                            cfd_rfc = objXml.RFCEmisor,
                                            cfd_tipocambio = objXml.TipoCambio,
                                            cfd_total = objXml.Total,
                                            cfd_moneda = objXml.Moneda,
                                            metodo_pago_sat = null,
                                            ruta_comp_ext = null,
                                            taxid = null,
                                            forma_pago = null,
                                            cfd_fecha_expedicion = null,
                                            cfd_tipocomprobante = null,
                                            cfd_metodo_pago_sat = null,
                                            area = objOrdenCompraDet.area ?? 0,
                                            cuenta_oc = objOrdenCompraDet.cuenta ?? 0,
                                        });
                                        #endregion

                                        #region LN 3
                                        objMovs.Add(new Core.DTO.Enkontrol.Tablas.Poliza.sc_movpolDTO
                                        {
                                            year = DateTime.Now.Year,
                                            mes = DateTime.Now.Month,
                                            tp = "07",
                                            linea = 3,
                                            cta = 1146,
                                            scta = 4,
                                            sscta = 0,
                                            digito = 0,
                                            tm = 1,
                                            referencia = item.factura.ToString(),
                                            cc = objGasto.cc,
                                            concepto = "",
                                            monto = ivaDLStoMN,
                                            iclave = 0,
                                            itm = item.tm,
                                            st_par = "",
                                            orden_compra = objOrdenCompra.numero,
                                            numpro = objOrdenCompra.proveedor,
                                            socio_inversionista = null,
                                            istm = null,
                                            folio_imp = null,
                                            linea_imp = null,
                                            num_emp = null,
                                            folio_gxc = null,
                                            cfd_ruta_pdf = null,
                                            cfd_ruta_xml = null,
                                            UUID = null,
                                            cfd_rfc = null,
                                            cfd_tipocambio = null,
                                            cfd_total = null,
                                            cfd_moneda = null,
                                            metodo_pago_sat = null,
                                            ruta_comp_ext = null,
                                            taxid = null,
                                            forma_pago = null,
                                            cfd_fecha_expedicion = null,
                                            cfd_tipocomprobante = null,
                                            cfd_metodo_pago_sat = null,
                                            area = objOrdenCompraDet.area ?? 0,
                                            cuenta_oc = objOrdenCompraDet.cuenta ?? 0,
                                        });
                                        #endregion

                                        #region LN 4
                                        objMovs.Add(new Core.DTO.Enkontrol.Tablas.Poliza.sc_movpolDTO
                                        {
                                            year = DateTime.Now.Year,
                                            mes = DateTime.Now.Month,
                                            tp = "07",
                                            linea = 4,
                                            cta = 5000,
                                            scta = 11,
                                            sscta = 0,
                                            digito = 6,
                                            tm = 1,
                                            referencia = item.factura.ToString(),
                                            cc = objGasto.cc,
                                            concepto = "",
                                            monto = montoComp,
                                            iclave = 0,
                                            itm = item.tm,
                                            st_par = "",
                                            orden_compra = objOrdenCompra.numero,
                                            numpro = objOrdenCompra.proveedor,
                                            socio_inversionista = null,
                                            istm = null,
                                            folio_imp = null,
                                            linea_imp = null,
                                            num_emp = null,
                                            folio_gxc = null,
                                            cfd_ruta_pdf = null,
                                            cfd_ruta_xml = null,
                                            UUID = null,
                                            cfd_rfc = null,
                                            cfd_tipocambio = null,
                                            cfd_total = null,
                                            cfd_moneda = null,
                                            metodo_pago_sat = null,
                                            ruta_comp_ext = null,
                                            taxid = null,
                                            forma_pago = null,
                                            cfd_fecha_expedicion = null,
                                            cfd_tipocomprobante = null,
                                            cfd_metodo_pago_sat = null,
                                            area = objOrdenCompraDet.area ?? 0,
                                            cuenta_oc = objOrdenCompraDet.cuenta ?? 0,
                                        });
                                        #endregion
                                        
                                        #endregion
                                    }
                                    #endregion

                                    //GUARDAR POLIZA
                                    _polizaEkFS.SetContext(con);
                                    _polizaEkFS.SetTransaccion(trans);
                                    string numeroPoliza = _polizaEkFS.GuardarPoliza(objPol, objMovs);
                                    //string numeroPoliza = "";

                                    _polizaSPFS.SetContext(_context);
                                    _polizaSPFS.SetTransaccion(dbTransac);
                                    var polizaPoliza = numeroPoliza.Split(new string[] { "-" }, StringSplitOptions.None)[2];
                                    objPol.poliza = Convert.ToInt32(polizaPoliza);
                                    var resultadoSP = _polizaSPFS.GuardarPoliza(objPol, objMovs);
                                    
                                    #endregion

                                    #region MOVPROV
                                    int count = 0;
                                    string insertQuery = @"INSERT INTO sp_movprov 
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
                                                                    ,cfd_serie
                                                                    ,cfd_folio
                                                                    ,cfd_fecha
                                                                    ,cfd_certificado
                                                                    ,ruta_rec_xml
                                                                    ,ruta_rec_pdf
                                                                    ,afectacompra
                                                                    ,val_ref
                                                                    ,pide_iva
                                                                    ,valida_recibido
                                                                    ,valida_almacen
                                                                    ,valida_recibido_autorizar)
                                                            VALUES (?,?,?,?,?,?,?,?,?,?
                                                                    ,?,?,?,?,?,?,?,?,?,?
                                                                    ,?,?,?,?,?,?,?,?,?,?
                                                                    ,?,?,?,?,?,?,?,?,?,?,?,?)";


                                    using (var cmd = new OdbcCommand(insertQuery))
                                    {
                                        OdbcParameterCollection parameters = cmd.Parameters;

                                        parameters.Add("@numpro", OdbcType.Numeric).Value = objGasto.numpro;
                                        parameters.Add("@factura", OdbcType.Numeric).Value = item.factura;
                                        parameters.Add("@fecha", OdbcType.Date).Value = DateTime.Now;
                                        parameters.Add("@tm", OdbcType.Numeric).Value = item.tm; //?
                                        parameters.Add("@fechavenc", OdbcType.Date).Value = fechaVenc; //?
                                        parameters.Add("@concepto", OdbcType.Char).Value = objXml.Descripcion; //? 
                                        parameters.Add("@cc", OdbcType.Char).Value = objGasto.cc;
                                        parameters.Add("@referenciaoc", OdbcType.Char).Value = objGasto.referenciaoc;
                                        parameters.Add("@monto", OdbcType.Numeric).Value = objGasto.monto;
                                        parameters.Add("@tipocambio", OdbcType.Numeric).Value = objGasto.numpro > 9000 ? cambioDls : 1M;
                                        parameters.Add("@iva", OdbcType.Numeric).Value = objGasto.iva;
                                        parameters.Add("@year", OdbcType.Numeric).Value = DateTime.Now.Year;
                                        parameters.Add("@mes", OdbcType.Numeric).Value = DateTime.Now.Month;
                                        parameters.Add("@poliza", OdbcType.Numeric).Value = objPol.poliza;
                                        parameters.Add("@tp", OdbcType.Char).Value = "07";
                                        parameters.Add("@linea", OdbcType.Numeric).Value = 1;
                                        parameters.Add("@generado", OdbcType.Char).Value = objPol.generada;
                                        parameters.Add("@es_factura", OdbcType.Char).Value = "S";
                                        parameters.Add("@moneda", OdbcType.Char).Value = objGasto.numpro > 9000 ? 2 : 1;
                                        parameters.Add("@autorizapago", OdbcType.Char).Value = string.Empty;
                                        parameters.Add("@total", OdbcType.Numeric).Value = objGasto.total;
                                        parameters.Add("@tipocambio_oc", OdbcType.Numeric).Value = 1;
                                        parameters.Add("@empleado_modifica", OdbcType.Numeric).Value = idUsrEnkontrol;
                                        parameters.Add("@fecha_modifica", OdbcType.Date).Value = DateTime.Now;
                                        parameters.Add("@hora_modifica", OdbcType.DateTime).Value = DateTime.Now;
                                        parameters.Add("@bit_factoraje", OdbcType.Char).Value = "N";
                                        parameters.Add("@bit_autoriza", OdbcType.Char).Value = "N";
                                        parameters.Add("@bit_transferida", OdbcType.Char).Value = "N";
                                        parameters.Add("@bit_pagada", OdbcType.Char).Value = "N";
                                        parameters.Add("@empleado", OdbcType.Numeric).Value = idUsrEnkontrol;
                                        parameters.Add("@cfd_serie", OdbcType.Char).Value = objXml.Serie;
                                        parameters.Add("@cfd_folio", OdbcType.Numeric).Value = objXml.Folio;
                                        parameters.Add("@cfd_fecha", OdbcType.Date).Value = objXml.Fecha;
                                        parameters.Add("@cfd_certificado", OdbcType.Char).Value = objXml.Certificado;
                                        parameters.Add("@ruta_rec_xml", OdbcType.Char).Value = rutaXML;
                                        parameters.Add("@ruta_rec_xml", OdbcType.Char).Value = rutaPdf;
                                        parameters.Add("@afectacompra", OdbcType.Char).Value = "S";
                                        parameters.Add("@val_ref", OdbcType.Char).Value = "0";
                                        parameters.Add("@pide_iva", OdbcType.Char).Value = "S";
                                        parameters.Add("@valida_recibido", OdbcType.Char).Value = string.Empty;
                                        parameters.Add("@valida_almacen", OdbcType.Char).Value = "N";
                                        parameters.Add("@valida_recibido_autorizar", OdbcType.Char).Value = "N";

                                        cmd.Connection = trans.Connection;
                                        cmd.Transaction = trans;

                                        count += cmd.ExecuteNonQuery();
                                    }
                                    #endregion

                                    #region ACTUALIZAR GASTO
                                    int countUpdate = 0;
                                    var consultaUpdateOC = @"
                                    UPDATE sp_gastos_prov 
                                    SET estatus = 'P',
                                        factura = ?
                                    WHERE id = ?";

                                    using (var cmd = new OdbcCommand(consultaUpdateOC))
                                    {
                                        OdbcParameterCollection parametrosUpdateOC = cmd.Parameters;

                                        parametrosUpdateOC.Add("@factura", OdbcType.Int).Value = item.factura;
                                        parametrosUpdateOC.Add("@id", OdbcType.Int).Value = item.id;

                                        cmd.Connection = trans.Connection;
                                        cmd.Transaction = trans;

                                        countUpdate += cmd.ExecuteNonQuery();
                                    }

                                    #endregion
                                    
                                }
                                else
                                {
                                    #region RECHAZAR

                                    var queryEk = new OdbcConsultaDTO();
                                    queryEk.consulta = "SELECT total FROM sp_gastos_prov WHERE id = ?";
                                    queryEk.parametros.Add(new OdbcParameterDTO
                                    {
                                        nombre = "id",
                                        tipo = OdbcType.Int,
                                        valor = item.id
                                    });

                                    var objGasto = _contextEnkontrol.Select<dynamic>(vSesiones.sesionAmbienteEnkontrolAdm, queryEk).FirstOrDefault();

                                    #region LIBERAR ORDEN DE COMPRA
                                    int countUpdate = 0;
                                    var consultaUpdateOC = @"
                                    UPDATE so_orden_compra 
                                    SET total_fac = total_fac - ?
                                    WHERE cc = ? AND numero = ?";

                                    using (var cmd = new OdbcCommand(consultaUpdateOC))
                                    {
                                        OdbcParameterCollection parametrosUpdateOC = cmd.Parameters;

                                        parametrosUpdateOC.Add("@total_fac", OdbcType.Decimal).Value = objGasto.total;

                                        parametrosUpdateOC.Add("@cc", OdbcType.NVarChar).Value = item.cc;
                                        parametrosUpdateOC.Add("@numero", OdbcType.Int).Value = item.numero;

                                        cmd.Connection = trans.Connection;
                                        cmd.Transaction = trans;

                                        countUpdate += cmd.ExecuteNonQuery();
                                    }
                                    #endregion

                                    #region ACTUALIZAR GASTO
                                    countUpdate = 0;
                                    var consultaUpdateGasto = @"
                                    UPDATE sp_gastos_prov 
                                    SET estatus = 'R'
                                    WHERE id = ?";

                                    using (var cmd = new OdbcCommand(consultaUpdateGasto))
                                    {
                                        OdbcParameterCollection parametrosUpdateOC = cmd.Parameters;

                                        parametrosUpdateOC.Add("@id", OdbcType.Int).Value = item.id;

                                        cmd.Connection = trans.Connection;
                                        cmd.Transaction = trans;

                                        countUpdate += cmd.ExecuteNonQuery();
                                    }
                                    #endregion
                                    #endregion
                                }

                                SaveBitacora(0, (int)AccionEnum.AGREGAR, item.id, JsonUtils.convertNetObjectToJson(item.esAuth));
                                trans.Commit();
                                resultado.Add(SUCCESS, true);

                            }
                            catch (Exception e)
                            {
                                trans.Rollback();
                                LogError(0, 0, "PropuestaController", "ValidarFactura", e, AccionEnum.AGREGAR, item.id, item.esAuth);

                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, e.Message);
                            }
                        }
                    }
                }
            }
            return resultado;
        }

        public List<ArchivoRutaDTO> GuardarArchivos(string rutaXml, string rutaPdf, string empresa, Int64 folio)
        {
            var lstRutas = new List<ArchivoRutaDTO>();

            try
            {
                //C:\EnkontrolV8\Fiel Construplan\xml\CFDI_KCM951027BN2_0001105308916320716.xml
                string RutaBaseEkXml = @"\\10.1.0.125\EnkontrolV8\Fiel Construplan\xml";
                string RutaBaseEkPdf = @"\\10.1.0.125\EnkontrolV8\Fiel Construplan\pdf";

                string RutaBaseEkProductivoXml = @"C:\EnkontrolV8\Fiel Construplan\xml\";
                string RutaBaseEkProductivoPdf = @"C:\EnkontrolV8\Fiel Construplan\pdf\";
#if DEBUG
                //RutaBasePortalRequisitos = @"C:\Portal\Requisitos";
                //RutaBasePortalRequisitosEnkontrol = @"C:\po\req";
                //RutaBasePortal = @"C:\Portal\Xml\FileCopyPruebas";
                //RutaBaseEnkontrol = @"C:\po\xml";
#endif
                String rutaArchivo = empresa + "_" + folio;

                string filePathEnkontrolXml = "";
                string filePathEnkontrolXmlUnica = "";
                string filePathEnkontrolPdf = "";
                string filePathEnkontrolPdfUnica = "";

                filePathEnkontrolXml = Path.Combine(RutaBaseEkXml, rutaArchivo + ".xml");
                filePathEnkontrolPdf = Path.Combine(RutaBaseEkXml, rutaArchivo + ".pdf");

                filePathEnkontrolXmlUnica = ObtenerRutaArchivo(filePathEnkontrolXml);
                filePathEnkontrolPdfUnica = ObtenerRutaArchivo(filePathEnkontrolPdf);

                string fileNameUnicoXml = Path.GetFileName(filePathEnkontrolXml);
                string fileNameUnicoPdf = Path.GetFileName(filePathEnkontrolPdf);

                lstRutas.Add(new ArchivoRutaDTO { tipoArchivo = TipoArchivoEnum.Xml, ruta = RutaBaseEkProductivoXml + fileNameUnicoXml });
                lstRutas.Add(new ArchivoRutaDTO { tipoArchivo = TipoArchivoEnum.Factura, ruta = RutaBaseEkProductivoPdf + fileNameUnicoPdf });

                File.Copy(rutaXml, filePathEnkontrolXml);
                File.Copy(rutaPdf, filePathEnkontrolPdf);

            }
            catch (Exception e)
            {
                throw e;
            }

            return lstRutas;
        }

        private string ObtenerRutaArchivo(string ruta)
        {
            if (File.Exists(ruta))
            {
                int count = 1;

                string fileNameOnly = Path.GetFileNameWithoutExtension(ruta);
                string extension = Path.GetExtension(ruta);
                string path = Path.GetDirectoryName(ruta);
                string newFullPath = ruta;

                while (File.Exists(newFullPath))
                {
                    string tempFileName = string.Format("{0} ({1})", fileNameOnly, count++);
                    newFullPath = Path.Combine(path, tempFileName + extension);
                }

                ruta = newFullPath;
            }

            return ruta;
        }

        public Dictionary<string, object> AutorizarFactura(List<FiltroValidacionDTO> lstFiltro)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            using (var con = checkConexionProductivo())
            {
                using (var trans = con.BeginTransaction())
                {
                    foreach (var item in lstFiltro)
                    {
                        try
                        {
                            int countUpdate = 0;
                            var consultaUpdateGastoProv = @"
                            UPDATE sp_gastos_prov 
                            SET nivel_aut = 1,
                                cerrado = 1
                            WHERE id = ?";

                            using (var cmd = new OdbcCommand(consultaUpdateGastoProv))
                            {
                                OdbcParameterCollection parametrosUpdateOC = cmd.Parameters;

                                parametrosUpdateOC.Add("@id", OdbcType.Char).Value = item.id;

                                cmd.Connection = trans.Connection;
                                cmd.Transaction = trans;

                                countUpdate += cmd.ExecuteNonQuery();
                            }

                            SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, 0, JsonUtils.convertNetObjectToJson(item));
                            resultado.Add(SUCCESS, true);
                            trans.Commit();
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            LogError(0, 0, "PropuestaController", "AutorizarFactura", e, AccionEnum.AGREGAR, 0, new { item.cc, item.numero });

                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, e.Message);
                        }
                    }
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetDescripcionTM(int tm)
        {
            Dictionary<string, object> resultado = new Dictionary<string,object>();

            try
            {
                var listaTM = _contextEnkontrol.Select<dynamic>(EnkontrolAmbienteEnum.Prod, new OdbcConsultaDTO() { consulta = string.Format(@"SELECT descripcion FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_tm" + " WHERE tm = " + tm) }).FirstOrDefault();

                resultado.Add("desc", listaTM.descripcion);
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

    }
}
