using Core.DAO.Contabilidad.Reportes;
using Core.DTO;
using Core.DTO.Contabilidad;
using Core.DTO.Contabilidad.FlujoEfectivo;
using Core.DTO.Contabilidad.Propuesta;
using Core.DTO.Principal.Generales;
using Core.DTO.Subcontratistas.Bloqueo;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contabilidad;
using Core.Entity.Administrativo.Contabilidad.FlujoEfectivo;
using Core.Enum.Administracion.CadenaProductiva;
using Core.Enum.Administracion.FlujoEfectivo;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Core.Enum.Subcontratistas;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Contabilidad.Reportes;
using Data.Factory.Principal.Menus;
using Infrastructure.DTO;
using Infrastructure.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Contabilidad.Reportes
{
    public class CadenaProductivaDAO : GenericDAO<tblC_CadenaProductiva>, ICadenaProductivaDAO
    {
        CatNumNafinFactoryServices nafinFactoryServices = new CatNumNafinFactoryServices();
        CadenaPrincipalFactoryServices cadenaPrincipalFactoryServices = new CadenaPrincipalFactoryServices();
        MenuFactoryServices mfs = new MenuFactoryServices();
        public ICollection<ProveedorDTO> ListaPRoveedores()
        {
            try
            {
                
                string consulta = "SELECT numpro AS NUMPROVEEDOR,rfc AS RFC,a_paterno AS PATERNO1, a_materno AS MATERNO1, a_nombre AS NOMBRE1,nombre as RAZONSOCIAL,direccion AS CALLE,'' AS COLONIA, '' AS PAIS, ciudad AS ESTADO, '' AS MUNICIPIO, " +
                             "cp AS CP,telefono1 AS TEL1,fax AS FAX1,email AS EMAIL1, telefono2,'' AS TEL2, '' AS FAX2, '' AS EMAIL2," +
                             "'1' AS TIPO,'1' AS SUSCEPTIBLE,'' AS CLAVE,persona_fisica AS ESFISICA, moneda AS MONEDA " +
                             "from DBA.sp_proveedores ;";

                if(vSesiones.sesionEmpresaActual==3)
                {
                     consulta = "SELECT numpro AS NUMPROVEEDOR,rfc AS RFC,a_paterno AS PATERNO1, a_materno AS MATERNO1, a_nombre AS NOMBRE1,nombre as RAZONSOCIAL,direccion AS CALLE,'' AS COLONIA, '' AS PAIS, ciudad AS ESTADO, '' AS MUNICIPIO, " +
                             "cp AS CP,telefono1 AS TEL1,fax AS FAX1,email AS EMAIL1, telefono2,'' AS TEL2, '' AS FAX2, '' AS EMAIL2," +
                             "'1' AS TIPO,'1' AS SUSCEPTIBLE,'' AS CLAVE,persona_fisica AS ESFISICA, moneda AS MONEDA " +
                             "from DBA.sp_proveedores ;";
                }

                if ((MainContextEnum)vSesiones.sesionEmpresaActual == MainContextEnum.PERU)
                {
                    using (var dbStartSoft = new MainContextPeruStarSoft003BDCOMUN())
                    {
                        var lstProveedoresPeru = dbStartSoft.MAEPROV.Where(e => e.PRVCESTADO == "V").Select(e => new ProveedorDTO
                        {
                            NUMPROVEEDOR = e.PRVCCODIGO,
                            RAZONSOCIAL = e.PRVCNOMBRE
                        }).ToList();

                        return lstProveedoresPeru;

                    }
                }
                var res1 = (ICollection<ProveedorDTO>)_contextEnkontrol.Where(consulta).ToObject<ICollection<ProveedorDTO>>();
                return res1.ToList();
            }
            catch (Exception)
            {

                return null;
            }
        }
        /// <summary>
        /// Consulta las facturas de proveedores que no están saldadas
        /// </summary>
        /// <param name="nomprov"></param>
        /// <param name="tipoFactura"></param>
        /// <returns>Facturas del proveedor no saldadas sin ser empaquetada en alguna cadena</returns>
        public ICollection<VencimientoDTO> getInforVencimiento(string nomprov, int tipoFactura)
        {
            try
            {
                var empresa = vSesiones.sesionEmpresaActual;
                var esColombia = empresa == (int)EmpresaEnum.Colombia;
                var esArrendadora = empresa == (int)EmpresaEnum.Arrendadora;
                string query = @"
                    SELECT  
                        a.numpro AS idProveedor ,
                        b.nombre AS Proveedor ,
                        a.factura AS factura ,
                        a.fecha As fecha ,
                        a.fechavenc AS fechaVencimiento ,
                        a.cc AS centro_costos ,
                        a.tipocambio AS tipoCambio ,
                        a.concepto as concepto ,
                        c.descripcion as nombCC ,
                        b.moneda as tipoMoneda ,
                        a.monto ,
                        a.iva ,
                        a.referenciaoc AS orden_compra,
                        {1} AS area_cuenta,
                        (select SUM(g.total) from sp_movprov as g where g.numpro = ? and g.factura=a.factura) as total,
                        (select TOP 1 cast(GP.fecha_timbrado as date) from sp_gastos_prov GP where GP.numpro = A.numpro and GP.factura = A.factura) as fechaTimbrado
                    FROM sp_movprov A 
                    INNER JOIN sp_proveedores B ON A.numpro = B.numpro  
                    INNER JOIN CC  C ON c.cc = a.cc ";
                    //query += @" INNER JOIN sp_gastos_prov D on (A.numpro = D.numpro AND A.cc = D.cc AND A.factura = D.factura AND D.cerrado = 1)";
                        query += @" WHERE a.numpro = ? AND a.es_factura = 'S' AND a.factura in (select factura from sp_movprov where numpro=a.numpro and factura=a.factura and cc=a.cc group by factura,cc having  sum   (total) >  0) 
                        {0}
                        ORDER BY a.fechavenc asc;";
                var odbc = new OdbcConsultaDTO()
                {

                    consulta = string.Format(query
                    , tipoFactura == 1 ? " AND getdate() > a.fechavenc " : string.Empty
                    , esColombia ? "''" : "(SELECT (CAST(m.area as varchar(10))+'-'+CAST(m.cuenta_oc as varchar(10))) FROM sc_movpol M WHERE m.year = a.year AND m.mes = a.mes AND m.tp = a.tp AND m.poliza = a.poliza AND m.linea = a.linea)"),
                };
                odbc.parametros.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.NVarChar, valor = nomprov });
                odbc.parametros.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.NVarChar, valor = nomprov });
                var res1 = _contextEnkontrol.Select<VencimientoDTO>(EnkontrolAmbienteEnum.Prod, odbc);
                res1.ToList().ForEach(e =>
                {
                    var lstCP = _context.tblC_CadenaProductiva.Where(sg => sg.numProveedor.Equals(nomprov) && sg.factura.Equals(e.factura.ToString())).ToList();
                    if (Math.Abs(lstCP.Sum(s => s.saldoFactura)) == Math.Abs(e.monto + e.IVA))
                    {
                        res1.Remove(e);
                    }
                    else
                    {
                        var lstParcial = _context.tblC_FacturaParcial.Where(sg => sg.numProv.Equals(nomprov) && sg.factura.Equals(e.factura.ToString())).ToList();
                        if (Math.Abs(lstParcial.Sum(s => s.abonado)) == Math.Abs(e.monto + e.IVA))
                            res1.Remove(e);
                    }
                });

                #region Verificar bloqueo del proveedor
                if (_context.tblP_ReglasSubcontratistasBloqueo.Any(x => x.estatus && x.aplicar && x.id == 2))
                {
                    var subcontratistaDB = _context.sp_Select<SubcontratistaBloqueadoDTO>(new StoreProcedureDTO
                    {
                        nombre = "spSUBCONTRATISTAS_PROVEEDOR_POR_NUMERO",
                        parametros = new List<OdbcParameterDTO> { new OdbcParameterDTO { nombre = "numeroProveedor", tipoSql = SqlDbType.Int, valor = nomprov } }
                    });

                    if (subcontratistaDB.Count() > 0)
                    {
                        if (subcontratistaDB[0].tipoBloqueoID != (int)TipoBloqueoEnum.SIN_BLOQUEO)
                        {
                            foreach (var item in res1.ToList())
                            {
                                item.bloqueado = true;
                                item.descripcionBloqueo = subcontratistaDB[0].descripcionTipoBloqueo;
                            }
                        }
                    }
                }
                #endregion

                return res1.ToList();
            }
            catch (Exception o_O)
            {
                return new List<VencimientoDTO>();
            }
        }
        public string getCCVencimiento(string numpro, string factura)
        {
            try
            {
                string consulta = "SELECT cc AS centro_costos FROM sp_movprov Where numpro = " + numpro + " and factura = " + factura;
                var res1 = (ICollection<VencimientoDTO>)_contextEnkontrol.Where(consulta).ToObject<ICollection<VencimientoDTO>>();
                return res1.FirstOrDefault().centro_costos;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Consulta proveedor desde enkontrol
        /// </summary>
        /// <param name="idProveedor">numpro</param>
        /// <param name="moneda">Tipo de moneda</param>
        /// <returns></returns>
        public ICollection<ProveedorDTO> getProveedores(int idProveedor, int moneda)
        {
            try
            {
                string consulta = "SELECT numpro AS NUMPROVEEDOR,rfc AS RFC,a_paterno AS PATERNO1, a_materno AS MATERNO1, a_nombre AS NOMBRE1,nombre as RAZONSOCIAL,direccion AS CALLE,'' AS COLONIA, '' AS PAIS, ciudad AS ESTADO, '' AS MUNICIPIO, " +
                             "cp AS CP,telefono1 AS TEL1,fax AS FAX1,email AS EMAIL1, telefono2,'' AS TEL2, '' AS FAX2, '' AS EMAIL2," +
                             "'1' AS TIPO,'1' AS SUSCEPTIBLE,'' AS CLAVE,persona_fisica AS ESFISICA " +
                             "from sp_proveedores " +
                             "where numpro = '" + idProveedor + "' and moneda ='" + moneda + "';";

                if(vSesiones.sesionEmpresaActual==3)
                {
                    consulta = "SELECT numpro AS NUMPROVEEDOR,rfc AS RFC,a_paterno AS PATERNO1, a_materno AS MATERNO1, a_nombre AS NOMBRE1,nombre as RAZONSOCIAL,direccion AS CALLE,'' AS COLONIA, '' AS PAIS, ciudad AS ESTADO, '' AS MUNICIPIO, " +
                             "cp AS CP,telefono1 AS TEL1,fax AS FAX1,email AS EMAIL1, telefono2,'' AS TEL2, '' AS FAX2, '' AS EMAIL2," +
                             "'1' AS TIPO,'1' AS SUSCEPTIBLE,'' AS CLAVE,persona_fisica AS ESFISICA " +
                             "from DBA.sp_proveedores " +
                             "where numpro = '" + idProveedor + "' and moneda ='" + moneda + "';";
                }

                var res1 = (ICollection<ProveedorDTO>)_contextEnkontrol.Where(consulta).ToObject<ICollection<ProveedorDTO>>();
                return res1.ToList();
            }
            catch (Exception)
            {

                return null;
            }

        }
        public bool enviarCorreo()
        {
            var cadenasPorAutorizar = _context.tblC_CadenaPrincipal.Where(x => x.estatus && x.estadoAutorizacion == EstadoAutorizacionCadenaEnum.VoBo).ToList();

            if (cadenasPorAutorizar.Count > 0)
            {
                List<string> correos = new List<string>();
                correos.Add("carla.velasco@construplan.com.mx");
                correos.Add("g.reina@construplan.com.mx");
                correos.Add("y.olivas@construplan.com.mx");


                string AsuntoCorreo = "";

                AsuntoCorreo = "Se ha dado Vobo de una o mas cadenas productivas y se encuentran pendientes de autorización <br/> Favor de entrar a Admin y Finanzas/ Tesoreria / Autorización cadenas para proceder con la autorización. ";

                GlobalUtils.sendEmail(string.Format("{0}: Cadenas pendientes de autorización", PersonalUtilities.GetNombreEmpresa()), AsuntoCorreo, correos);
                return true;
            }
            else
                return false;
        }
        public bool enviarCorreoPropuesta()
        {
            List<string> correos = new List<string>();
            correos.Add("carla.velasco@construplan.com.mx");
            correos.Add("g.reina@construplan.com.mx");
            correos.Add("y.olivas@construplan.com.mx");
            string AsuntoCorreo = "";

            AsuntoCorreo = "Se han guardado facturas para propuesta de pago y estan pendientes de autorización <br/> Favor de entrar a Admin y Finanzas/ Tesoreria / Pagos / Programación de pagos. ";

            GlobalUtils.sendEmail(string.Format("{0}: Facturas de propuesta pendientes de autorización", PersonalUtilities.GetNombreEmpresa()), AsuntoCorreo, correos);
            return true;
        }
        public ICollection<DocumentoNegociableDTO> GetDocumentoNegociable(int factura, int numProveedor)
        {
            try
            {

                string consulta = @"SELECT B.nombre AS proveedor,A.numpro AS noProveedor, A.factura AS noDocumento, A.fecha AS fechaEmision, A.fechavenc AS fechaVencimiento,B.moneda,(select SUM(g.total) from sp_movprov as g where g.numpro='" + numProveedor + @"' and g.factura=A.factura) as monto
                                    FROM sp_movprov A
                                    INNER JOIN sp_proveedores B ON A.numpro = B.numpro
                                    WHERE A.Factura = '" + factura + "' ANd A.numpro = '" + numProveedor + "' and es_factura = 'S' ;";


                var res1 = (ICollection<DocumentoNegociableDTO>)_contextEnkontrol.Where(consulta).ToObject<ICollection<DocumentoNegociableDTO>>();
                return res1.ToList();
            }
            catch (Exception)
            {

                return null;
            }
        }
        public decimal GetAbono(string factura, string numProv, decimal saldo, out decimal diff)
        {
            var lst = _context.tblC_FacturaParcial.Where(w => w.factura.Equals(factura) && w.numProv.Equals(numProv) && w.abonado != w.total).ToList();
            if (lst.Count > 0)
            {
                var sum = lst.Sum(s => s.abonado);
                diff = lst.FirstOrDefault().total - sum;
                return sum;
            }
            else
            {
                diff = 0;
                return 0;
            }
        }
        public void updateEstatus(bool estatus, int id)
        {
            var _op = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblC_Anticipo>();
            var save = _op.FirstOrDefault(w => w.id == id);
            save.estatus = estatus;
            _context.SaveChanges();
        }
        public void Guardar(tblC_Anticipo obj)
        {
            obj.proveedor = obj.proveedor.Substring(obj.proveedor.IndexOf('-') + 1);
            obj.nombCC = obj.nombCC.Substring(obj.nombCC.IndexOf('-') + 1);
            var _op = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblC_Anticipo>();
            var save = _op.FirstOrDefault(w => w.id == obj.id);
            if (save == null)
            {
                if (obj.tipoMoneda == 2)
                    obj.tipoCambio = getDolarDelDia(obj.fecha);
                _op.AddObject(obj);
            }
            _context.SaveChanges();
        }
        public void Guardar(tblC_Linea obj)
        {
            var _op = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblC_Linea>();
            var save = _op.FirstOrDefault(w => w.banco.Equals(obj.banco) && w.factoraje.Equals(obj.factoraje) && w.moneda == obj.moneda);
            if (save != null)
            {
                save.linea = obj.linea;
                save.fecha = obj.fecha;
            }
            else
                _op.AddObject(obj);
            _context.SaveChanges();
        }
        public void Guardar(tblC_FacturaParcial obj)
        {
            var _op = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblC_FacturaParcial>();
            var cp = _context.tblC_CadenaProductiva.Where(w => w.factura.Equals(obj.factura) && w.numProveedor.Equals(obj.numProv)).ToList();
            obj.idCadena = cp.LastOrDefault().id;
            obj.idPrincipal = cp.LastOrDefault().idPrincipal;
            _op.AddObject(obj);
            cp.LastOrDefault().saldoFactura = obj.abonado;
            var _cPal = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblC_CadenaPrincipal>();
            var oPAl = _cPal.FirstOrDefault(w => w.id == obj.idPrincipal);
            //oPAl.total = cp.Sum(s => s.saldoFactura);
            //Update(cp, cp.LastOrDefault().id, (int)BitacoraEnum.CADENAPRODUCTIVA);
            _context.SaveChanges();
        }
        public void Guardar(tblC_CadenaProductiva obj)
        {
            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.CADENAPRODUCTIVA);
            else
                Update(obj, obj.id, (int)BitacoraEnum.CADENAPRODUCTIVA);

        }
        public List<tblC_Anticipo> getLstAnticipo(string numProveedor)
        {
            return _context.tblC_Anticipo.Where(x => string.IsNullOrEmpty(numProveedor) ? true : (x.numProveedor.Equals(numProveedor) && !x.estatus)).ToList();
        }
        public List<tblC_Anticipo> getLstAnticipo(int moneda)
        {
            return _context.tblC_Anticipo.Where(x => !x.estatus && x.tipoMoneda.Equals(moneda)).ToList();
        }
        public tblC_Anticipo getObjAnticipo(int id)
        {
            return _context.tblC_Anticipo.FirstOrDefault(x => id == 0 ? false : x.id == id);
        }
        public List<tblC_FacturaParcial> GetParcialPorPrincipal(int idPrincial)
        {
            return _context.tblC_FacturaParcial.Where(x => x.idPrincipal == idPrincial).ToList();
        }
        #region Propuesta
        /// <summary>
        /// Consulta de anticipos desde concentrado
        /// </summary>
        /// <param name="busq">busqueda de concentrado</param>
        /// <returns>Anticipos</returns>
        public List<tblC_Anticipo> getLstAnticipo(BusqConcentradoDTO busq)
        {
            return _context.tblC_Anticipo
                .ToList()
                .Where(x => !x.estatus)
                .Where(x => x.fechaVencimiento.Year.Equals(busq.min.Year))
                .Where(x => x.fechaVencimiento.noSemana().Equals(busq.min.noSemana()))
                .ToList();
        }
        /// <summary>
        /// Consulta de anticipos desde centro de costos
        /// </summary>
        /// <param name="busq">centro de costos</param>
        /// <returns>Anticipos</returns>
        public List<tblC_Anticipo> getLstAnticipo(List<string> lstCc)
        {
            return _context.tblC_Anticipo
                .ToList()
                .Where(x => !x.estatus)
                .Where(x => lstCc.Any(c => c.Equals(x.centro_costos)))
                .ToList();
        }
        #endregion
        public List<tblC_CadenaProductiva> GetDocumentoPorPrincipal(int idPrincial)
        {
            return _context.tblC_CadenaProductiva.Where(x => x.idPrincipal == idPrincial).ToList();
        }
        /// <summary>
        /// Consulta las cadenas productivas pagadas
        /// </summary>
        /// <param name="busq">Busqueda de concentradp</param>
        /// <returns>Cadenas productivas</returns>
        public List<tblC_CadenaProductiva> getLstCadenasPagadas(BusqConcentradoDTO busq)
        {
            return _context.tblC_CadenaProductiva.ToList()
                .Where(x => x.pagado)
                .Where(x => x.fechaVencimiento.Year.Equals(busq.min.Year))
                .Where(x => x.fechaVencimiento >= busq.min)
                .Where(x => x.fechaVencimiento <= busq.max)
                .Where(x => busq.lstCC.Any(c => c.Equals(x.centro_costos)))
                .ToList();
        }
        /// <summary>
        /// Consulta las cadenas productivas pagadas
        /// </summary>
        /// <param name="lstCC">centro de costos filtrados</param>
        /// <returns>Cadenas productivas</returns>
        public List<tblC_CadenaProductiva> getLstCadenasPagadas(List<string> lstCC)
        {
            return _context.tblC_CadenaProductiva.ToList()
                .Where(x => x.pagado)
                .Where(x => lstCC.Any(c => c.Equals(x.centro_costos)))
                .ToList();
        }
        public List<tblC_CadenaProductiva> GetAllDocumentos()
        {
            var lst = _context.tblC_CadenaProductiva.ToList();
            lst.AddRange(_context.tblC_Anticipo.Where(w => !w.estatus).ToList().Select(x => new tblC_CadenaProductiva
            {
                id = 0,
                idPrincipal = 0,
                banco = x.banco,
                centro_costos = x.centro_costos,
                cif = x.cif,
                concepto = x.concepto,
                estatus = x.estatus,
                factoraje = x.factoraje,
                factura = string.Empty,
                fecha = x.fecha,
                fechaVencimiento = x.fechaVencimiento,
                IVA = x.IVA,
                monto = x.monto,
                nombCC = x.nombCC,
                numNafin = x.numNafin,
                numProveedor = x.numProveedor,
                pagado = true,
                proveedor = x.proveedor,
                reasignado = false,
                saldoFactura = x.anticipo,
                tipoCambio = x.tipoCambio,
                tipoMoneda = x.tipoMoneda
            }));
            lst.ForEach(x =>
            {
                x.banco = string.IsNullOrWhiteSpace(x.banco) ? "BANORTE" : x.banco;
                x.factoraje = string.IsNullOrWhiteSpace(x.factoraje) ? "N" : x.factoraje;
                x.tipoMoneda = x.numProveedor.ParseInt() < 9000 ? 1 : 2;
                x.tipoCambio = x.numProveedor.ParseInt() < 9000 ? 1 : x.tipoCambio;
            });
            return lst;
        }

        public List<tblC_CadenaProductiva> GetDocumentosGuardados()
        {
            return _context.tblC_CadenaProductiva.Where(x => x.estatus == true).ToList();
        }
        public List<tblC_CadenaProductiva> GetDocumentosGuardados(int idPrincial)
        {
            return _context.tblC_CadenaProductiva.Where(x => x.estatus == true && x.idPrincipal == idPrincial).ToList();
        }
        public List<tblC_CadenaProductiva> GetDocumentosAplicados()
        {
            return _context.tblC_CadenaProductiva.Where(x => x.estatus == false).ToList();
        }
        public List<tblC_CadenaProductiva> GetDocumentosAplicados(int idPrincial)
        {
            return _context.tblC_CadenaProductiva.Where(x => x.estatus == false && x.idPrincipal == idPrincial).ToList();
        }
        public decimal GetTipoCambioRegistro(string factura, string numProv)
        {
            if (numProv.ParseInt() < 9000)
                return 1;
            else
            {
                try
                {
                    string consulta = string.Format("SELECT tipocambio FROM sp_movprov where tp = 07 and factura = {0} and numpro = {1}", factura, numProv);
                    var res1 = (List<VencimientoDTO>)_contextEnkontrol.Where(consulta).ToObject<List<VencimientoDTO>>();
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
        public List<tblC_CadenaProductiva> GetDocumentosAplicados(DateTime inicio, DateTime fin)
        {
            return _context.tblC_CadenaProductiva
                .Where(x => x.estatus == false &&
                    x.fechaVencimiento >= inicio &&
                    x.fechaVencimiento <= fin
                ).ToList();
        }
        public List<tblC_CadenaProductiva> getDocumentoGuardado(int id)
        {
            return _context.tblC_CadenaProductiva.Where(x => x.idPrincipal == id).ToList();

        }
        public void setDocumentoGuardado(int id, string Factoraje, DateTime FechaEmision, DateTime FechaVencimiento, int? IF, string Banco)
        {
            var obj = _context.tblC_CadenaPrincipal.FirstOrDefault(x => x.id == id);
            obj.factoraje = Factoraje;
            obj.fecha = FechaEmision;
            obj.fechaVencimiento = FechaVencimiento;
            obj.banco = Factoraje.Equals("V") ? Banco : "BANORTE";
            obj.estatus = false;
            obj.firmaValidado = GlobalUtils.CrearFirmaDigital(id, DocumentosEnum.Reporte_CadenaProductiva, vSesiones.sesionUsuarioDTO.id, TipoFirmaEnum.Autorizacion);

            var temp = _context.tblC_CadenaProductiva.Where(x => x.idPrincipal == id).ToList();
            temp.ForEach(x =>
            {
                x.banco = obj.banco;
                x.cif = IF.ToString();
                x.concepto = x.concepto;
                x.estatus = obj.estatus;
                x.factoraje = obj.factoraje;
                x.reasignado = x.factoraje.Equals("V");
                x.fechaVencimiento = FechaVencimiento;
                x.fecha = FechaEmision;
            });
            _context.SaveChanges();
        }
        public void SetPago()
        {
            var lstCockiePal = new List<int>().ToList();
            var swBanco = new SwitchClass<string>
            {
                {1179, () => "BANORTE"},
                {940,  () => "BANAMEX"},
                {4392, () => "MONEX"},
                {3965, () => "SCOTIABANK"},
                {9676, () => "BANORTE"},
                {9157, () => "BANAMEX"},
            };
            var swCif = new SwitchClass<string>
                {
                    {"BANORTE", () => "3217" },
                    {"BANAMEX", () => "6544" },
                    {"SCOTIABANK", () => "32046" },
                    {"MONEX", () => "1097745" },
                };
            var lstPal = _context.tblC_CadenaPrincipal.Where(w => !w.estatus && !w.pagado).ToList();
            lstPal.ForEach(pal =>
            {
                pal.pagado = true;
                var lstBDCP = _context.tblC_CadenaProductiva.Where(w => w.idPrincipal == pal.id).ToList();
                lstBDCP.ForEach(x =>
                {
                    var banco = x.banco;
                    var thisCif = x.cif;
                    var factura = x.factura;
                    var cc = x.centro_costos;
                    var monto = x.monto;
                    var prov = x.numProveedor;
                    try
                    {
                        var consulta = string.Empty;
                        if (factura.Equals("0") || string.IsNullOrEmpty(factura))
                            consulta = string.Format("SELECT * FROM sc_movpol where tp = '04' and numpro = '{0}' and cc = '{1}' and monto = -{2}", banco, cc, monto);
                        else
                            consulta = string.Format("SELECT * FROM sp_movprov where tp = '04' and numpro = '{0}' and factura = {1} and monto = -{2}", prov, factura, monto);
                        var lstProv = (List<Core.DTO.Contabilidad.Poliza.MovProDTO>)_contextEnkontrol.Where(consulta).ToObject<List<Core.DTO.Contabilidad.Poliza.MovProDTO>>();
                        if (lstProv.Count == 0)
                        {
                            consulta = string.Format("SELECT * FROM sp_movprov where tp = '04' and numpro = '{0}' and factura = {1}", prov, factura);
                            lstProv = (List<Core.DTO.Contabilidad.Poliza.MovProDTO>)_contextEnkontrol.Where(consulta).ToObject<List<Core.DTO.Contabilidad.Poliza.MovProDTO>>();
                        }
                        var polprov = lstProv.LastOrDefault();
                        consulta = string.Format("SELECT * FROM sc_movpol where tp = '04' and  year = {0} and  mes = {1} and poliza = {2}", polprov.year, polprov.mes, polprov.poliza);
                        var lstEkCP = (List<Core.DTO.Contabilidad.Poliza.MovpolDTO>)_contextEnkontrol.Where(consulta).ToObject<List<Core.DTO.Contabilidad.Poliza.MovpolDTO>>();
                        x.pagado = lstEkCP == null ? false : true;
                        x.banco = swBanco.Execute(lstEkCP.FirstOrDefault(w => w.cc.Equals(x.centro_costos) && w.tm == 2 && w.numpro != null).numpro) ?? banco;
                    }
                    catch (Exception)
                    {
                        x.banco = banco;
                        x.pagado = false;
                    }
                    x.cif = swCif.Execute(x.banco ?? banco);
                    Guardar(x);
                    if (!x.pagado)
                        pal.pagado = false;
                    if (!lstCockiePal.Exists(w => w == x.idPrincipal))
                        lstPal.Where(w => w.id == x.idPrincipal).ToList().ForEach(p =>
                        {
                            p.banco = x.banco;
                            cadenaPrincipalFactoryServices.getCadenaPrincipalService().Guardar(p);
                            lstCockiePal.Add(p.id);
                        });

                });
            });
            var _op = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblC_FechaPago>();
            _op.AddObject(new tblC_FechaPago()
            {
                id = 0,
                fecha = DateTime.Now
            });
            _context.SaveChanges();
        }
        public bool GetPago(int factura, int proveedor, DateTime fecha)
        {
            try
            {
                var consulta = string.Format("SELECT * FROM sp_movprov WHERE factura = {0}  AND numpro = {1}", factura, proveedor);
                var res1 = (ICollection<VencimientoDTO>)_contextEnkontrol.Where(consulta).ToObject<ICollection<VencimientoDTO>>();
                if (res1.Count > 1 && res1.Sum(s => s.total) == 0)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public void ReasignaBanco()
        {
            var lstCockiePal = new List<int>().ToList();
            var swBanco = new SwitchClass<string>
            {
                {1179, () => "BANORTE"},
                {940,  () => "BANAMEX"},
                {4392, () => "MONEX"},
                {3965, () => "SCOTIABANK"},
                {9676, () => "BANORTE"},
                {9157, () => "BANAMEX"},
            };
            var swCif = new SwitchClass<string>
                {
                    {"BANORTE", () => "3217" },
                    {"BANAMEX", () => "6544" },
                    {"SCOTIABANK", () => "32046" },
                    {"MONEX", () => "1097745" },
                };
            var lstBDCPal = _context.tblC_CadenaPrincipal.Where(w => w.fechaVencimiento.Year == 2018 && w.banco.Equals("BANORTE") && w.factoraje.Equals("N")).ToList();
            lstBDCPal.ForEach(pal =>
            {
                var lstBDCP = _context.tblC_CadenaProductiva.Where(w => w.idPrincipal == pal.id).ToList();
                lstBDCP.ForEach(x =>
                {
                    var thisBanco = x.banco;
                    var thisCif = x.cif;
                    try
                    {
                        var consulta = string.Format(@"SELECT pol.* FROM sp_movprov mp 
                                                        INNER JOIN sc_movpol pol ON pol.poliza = mp.poliza
                                                                                AND pol.tp = mp.tp
                                                                                AND pol.year = mp.year
                                                                                AND pol.mes = mp.mes
                                                                                AND pol.cc = mp.cc
                                                                                AND mp.generado = 'C'
                                                        WHERE mp.numpro = {0} AND mp.factura = {1}", x.numProveedor, x.factura);
                        var lstEkCP = (List<Core.DTO.Contabilidad.Poliza.MovpolDTO>)_contextEnkontrol.Where(consulta).ToObject<List<Core.DTO.Contabilidad.Poliza.MovpolDTO>>();
                        x.banco = swBanco.Execute(lstEkCP.FirstOrDefault(w => w.cc.Equals(x.centro_costos) && w.tm == 2 && w.numpro != null).numpro) ?? thisBanco;
                    }
                    catch (Exception)
                    {
                        x.banco = thisBanco;
                    }
                    x.cif = swCif.Execute(x.banco ?? thisBanco);
                    Guardar(x);
                    if (!lstCockiePal.Exists(w => w == x.idPrincipal))
                        lstBDCPal.Where(w => w.id == x.idPrincipal).ToList().ForEach(p =>
                        {
                            p.banco = x.banco;
                            cadenaPrincipalFactoryServices.getCadenaPrincipalService().Guardar(p);
                            lstCockiePal.Add(p.id);
                        });

                });
            });
        }
        public tblC_FechaPago getUltimaFechaPago()
        {
            return _context.tblC_FechaPago.ToList().LastOrDefault();
        }
        #region ResumenSemanal
        public List<tblC_CadenaProductiva> lstCompletaCadenaProductiva()
        {
            return _context.tblC_CadenaProductiva.ToList();
        }
        public List<tblC_Linea> lstLinea()
        {
            return _context.tblC_Linea.ToList();
        }
        #endregion
        #region Intereses Nafin
        /// <summary>
        /// Consulta de intereses bancarios de las cadenas productivas vencida
        /// </summary>
        /// <param name="fecha">Fecha de la semana</param>
        /// <returns>Lista de intereses</returns>
        public List<tblC_InteresesNafin> getlstInteresesNafin(DateTime fecha)
        {
            return _context.tblC_InteresesNafin.ToList()
                .Where(i => i.esActivo)
                .Where(i => i.fecha.Year.Equals(fecha.Year))
                .Where(i => i.fecha.noSemana().Equals(fecha.noSemana()))
                .ToList();
        }
        /// <summary>
        /// Consulta de los detalles de intereses bancarios de las cadenas productivas vencida
        /// </summary>
        /// <param name="busq">Fecha de la semana</param>
        /// <returns>Lista de detalles de intereses</returns>
        public List<tblC_InteresesNafinDetalle> getlstInteresesNafinDetalle(BusqConcentradoDTO busq)
        {
            return _context.tblC_InteresesNafinDetalle.ToList()
                .Where(i => i.esActivo)
                .Where(i => i.fecha >= busq.min)
                .Where(i => i.fecha <= busq.max)
                .Where(i => busq.lstCC.Any(c => c.Equals(i.cc)))
                .ToList();
        }
        /// <summary>
        /// Consulta de los detalles de intereses bancarios de las cadenas productivas vencida
        /// </summary>
        /// <param name="fecha">Fecha de la semana</param>
        /// <returns>Lista de detalles de intereses</returns>
        public List<tblC_InteresesNafinDetalle> getlstInteresesNafinDetalle(List<string> lstCc)
        {
            return _context.tblC_InteresesNafinDetalle.ToList()
                .Where(i => i.esActivo)
                .Where(i => lstCc.Any(c => c.Equals(i.cc)))
                .ToList();
        }
        /// <summary>
        /// Guarda o actualiza los intereses de nafin
        /// </summary>
        /// <param name="lst">Intereses a guardar o actualizar </param>
        /// <returns>guardado realizado</returns>
        public bool guardarInteresesNafin(List<tblC_InteresesNafin> lst)
        {
            try
            {
                var ahora = DateTime.Now;
                var fecha = lst.FirstOrDefault().fecha;
                var lstIntereses = _context.tblC_InteresesNafin.ToList()
                    .Where(w => w.esActivo)
                    .Where(w => w.fecha.noSemana().Equals(fecha.noSemana()))
                    .ToList();
                var primero = lstIntereses.FirstOrDefault();
                var lstCadena = _context.tblC_CadenaProductiva.ToList()
                    .Where(c => c.fechaVencimiento.Year.Equals(fecha.Year))
                    .Where(c => c.fechaVencimiento.noSemana().Equals(fecha.noSemana()))
                    .Where(c => c.pagado)
                    .Where(c => c.factoraje.Equals("V"))
                    .GroupBy(g => new { g.centro_costos, g.banco, g.tipoMoneda })
                    .Select(c => new
                    {
                        cc = c.Key.centro_costos,
                        banco = c.Key.banco,
                        divisa = c.Key.tipoMoneda,
                        saldo = c.Sum(s => s.saldoFactura),
                        count = c.Count()
                    }).ToList();
                lst.ForEach(i =>
                {
                    var guardar = lstIntereses.FirstOrDefault(b => b.banco.Equals(i.banco) && b.divisa.Equals(i.divisa));
                    if (guardar == null)
                        guardar = new tblC_InteresesNafin()
                        {
                            banco = i.banco,
                            fecha = i.fecha
                        };
                    guardar.totalCadenas = i.totalCadenas;
                    guardar.totalBanco = i.totalBanco;
                    guardar.interes = i.interes;
                    guardar.tipoCambio = i.tipoCambio;
                    guardar.divisa = i.divisa;
                    guardar.esActivo = true;
                    guardar.fechaCaptura = ahora;
                    _context.tblC_InteresesNafin.AddOrUpdate(guardar);
                    SaveChanges();
                    i.id = guardar.id;
                    #region Detalles
                    var diff = guardar.totalBanco - guardar.totalCadenas;
                    var tCC = guardar.totalCadenas * guardar.interes;
                    var diffCC = diff - tCC;
                    var lstCC = lstCadena
                        .Where(c => c.banco.Equals(guardar.banco))
                        .Where(c => c.divisa.Equals(guardar.divisa))
                        .ToList();
                    lstCC.ForEach(c =>
                    {
                        var prolateo = diffCC / c.count;
                        var intCC = c.saldo * guardar.interes + prolateo;
                        intCC *= guardar.tipoCambio;
                        var det = _context.tblC_InteresesNafinDetalle.ToList()
                            .Where(d => guardar.fecha.Year.Equals(d.fecha.Year))
                            .Where(d => guardar.fecha.noSemana().Equals(d.fecha))
                            .Where(d => d.idInteres.Equals(guardar.id))
                            .Where(d => d.esActivo)
                            .Where(d => d.divisa.Equals(c.divisa))
                            .FirstOrDefault(d => d.cc.Equals(c.cc));
                        if (det == null)
                            det = new tblC_InteresesNafinDetalle()
                            {
                                idInteres = guardar.id,
                                cc = c.cc,
                                divisa = c.divisa,
                            };
                        det.esActivo = true;
                        det.fecha = guardar.fecha;
                        det.fechaCaptura = ahora;
                        det.interesFactoraje = intCC;
                        _context.tblC_InteresesNafinDetalle.AddOrUpdate(det);
                        SaveChanges();
                    });
                    #endregion
                });
                if (mfs.getMenuService().isLiberado(vSesiones.sesionCurrentView))
                {
                    SaveBitacora((int)BitacoraEnum.InteresesNafin, (int)AccionEnum.AGREGAR, lst.Min(i => i.id), JsonUtils.convertNetObjectToJson(primero));
                    SaveChanges();
                }
                return lst.Any(i => i.id > 0);
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        public void Eliminar(tblC_CadenaProductiva obj)
        {
            try
            {
                Delete(obj, (int)BitacoraEnum.CADENAPRODUCTIVA);
            }
            catch (Exception) { }
        }
        public void Eliminar(tblC_FacturaParcial entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                _context.Set<tblC_FacturaParcial>().Attach(entity);
                _context.Set<tblC_FacturaParcial>().Remove(entity);
                _context.SaveChanges();
                SaveBitacora((int)BitacoraEnum.FacturaParcial, (int)AccionEnum.ELIMINAR, (int)entity.id, JsonUtils.convertNetObjectToJson(entity));
            }
            catch (Exception) { }
        }
        public tblC_CadenaProductiva getMonto(string factura, string numProveedor)
        {
            try
            {
                string consulta = string.Format(@"SELECT a.factura, b.nombre AS Proveedor, a.numpro AS numProveedor, total as saldoFactura, a.tipocambio AS tipoCambio, a.concepto as concepto, a.es_factura AS cif,                                    a.tipo_factoraje AS factoraje, a.fecha As fecha, a.fechavenc AS fechaVencimiento, b.moneda as tipoMoneda, a.cc AS centro_costos, c.descripcion as nombCC, a.monto, a.iva,
                                            (Select top 1 d.descripcion from sp_proveedores_cta_dep E INNER JOIN sb_bancos D ON D.banco = E.banco where E.numpro = A.numpro) AS banco
                                                FROM sp_movprov A
                                                INNER JOIN sp_proveedores B ON A.numpro = B.numpro
                                                INNER JOIN CC  C ON c.cc = a.cc
                                                WHERE a.factura = {0} and a.numpro = {1}
                                                order by a.fechavenc;", factura, numProveedor);
                var lstEkCP = (List<tblC_CadenaProductiva>)_contextEnkontrol.Where(consulta).ToObject<List<tblC_CadenaProductiva>>();
                return lstEkCP.FirstOrDefault();
            }
            catch (Exception) { return new tblC_CadenaProductiva(); }
        }
        /// <summary>
        /// Consulta de dolar al día anterior
        /// </summary>
        /// <param name="dia">fecha de consulta</param>
        /// <returns>tipo de cambio del dolar</returns>
        public decimal getDolarDelDia(DateTime dia)
        {
            try
            {
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = "SELECT tipo_cambio FROM tipo_cambio WHERE fecha = ?"
                };
                odbc.parametros.Add(new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.Date, valor = dia });
                var dolar = _contextEnkontrol.Select<TipoCambioDllDTO>(EnkontrolEnum.CplanProd, odbc);
                return dolar.FirstOrDefault().tipo_cambio;
            }
            catch (Exception) { return 0; }
        }
        public void asignaDolar()
        {
            try
            {
                var lstBDCP = _context.tblC_CadenaProductiva.Where(w => w.tipoMoneda == 2 && w.tipoCambio < 5).ToList();
                lstBDCP.ForEach(x =>
                {
                    var o = getMonto(x.factura, x.numProveedor);
                    x.IVA = o.IVA;
                    x.monto = o.monto;
                    x.concepto = o.concepto ?? string.Empty;
                    x.tipoCambio = o.tipoCambio < 5 ? o.tipoCambio : getDolarDelDia(o.fecha);
                    Guardar(x);
                });
            }
            catch (Exception)
            { }
        }
        public List<string> lstObraCerradas()
        {
            try
            {
                var lst = _contextEnkontrol.Select<CcDTO>(EnkontrolEnum.CplanProd, "SELECT cc FROM cc where st_ppto = 'T'");
                return lst.Select(s => s.cc).ToList();
            }
            catch (Exception)
            { return new List<string>(); }
        }
        public List<CcDTO> lstObra(int division)
        {
            try
            {
                var lst = _contextEnkontrol.Select<CcDTO>(EnkontrolEnum.CplanProd, "SELECT * FROM cc ORDER BY cc");
                var lstDiv = _context.tblC_CCDivision.OrderBy(o => o.cc).ToList();
                lst.ForEach(e =>
                {
                    e.bit_area = e.bit_area ?? string.Empty;
                    e.st_ppto = e.st_ppto ?? string.Empty;
                    e.bit_area = lstDiv.Any(d => d.cc.Equals(e.cc)) ? lstDiv.FirstOrDefault(d => d.cc.Equals(e.cc)).division.ToString() : e.bit_area;
                });
                return lst;
            }
            catch (Exception)
            { return new List<CcDTO>(); }
        }
        public List<CcDTO> lstObra()
        {
            try
            {
                var lst = new List<CcDTO>();
                if (vSesiones.sesionEmpresaActual != 6)
                {
                    lst = _contextEnkontrol.Select<CcDTO>(EnkontrolEnum.CplanProd, "SELECT * FROM cc ORDER BY cc");
                }
                else 
                {
                    lst = _context.tblP_CC.Select(x => new CcDTO
                    {
                        cc = x.cc,
                        descripcion = x.descripcion,
                        bit_area = "",
                        st_ppto = "",
                        fecha_registro = x.fechaArranque,
                        ordernFlujoEfectivo = x.ordernFlujoEfectivo,
                        inicioObra = x.fechaArranque ?? DateTime.Now
                    }).ToList();
                }
                //var lstCCSIGOPLAN = _context.tblP_CC.ToList();
                //foreach (var item in lst)
                //{
                //    var auxLstSIGOPLAN = lstCCSIGOPLAN.FirstOrDefault(x => x.cc == item.cc);
                //    if (auxLstSIGOPLAN != null) item.cc = auxLstSIGOPLAN.areaCuenta;
                //}
                var lstDiv = _context.tblC_CCDivision.OrderBy(o => o.cc).ToList();
                lst.ForEach(e =>
                {
                    e.bit_area = e.bit_area ?? "0";
                    e.st_ppto = e.st_ppto ?? string.Empty;
                    e.bit_area = lstDiv.Exists(d => d.cc.Replace(System.Environment.NewLine, string.Empty).Equals(e.cc)) ? lstDiv.FirstOrDefault(d => d.cc.Replace(System.Environment.NewLine, string.Empty).Equals(e.cc)).division.ToString() : e.bit_area;
                });
                return lst;
            }
            catch (Exception)
            { return new List<CcDTO>(); }
        }
        public List<CcDTO> lstObraAC()
        {
            try
            {
                var lst = _contextEnkontrol.Select<CcDTO>(EnkontrolEnum.CplanProd, "SELECT * FROM cc ORDER BY cc");
                var lstCCSIGOPLAN = _context.tblP_CC.ToList();
                foreach (var item in lst) 
                {
                    var auxLstSIGOPLAN = lstCCSIGOPLAN.FirstOrDefault(x => x.cc == item.cc);
                    if (auxLstSIGOPLAN != null) item.cc = auxLstSIGOPLAN.areaCuenta;
                }

                var lstDiv = _context.tblC_CCDivision.OrderBy(o => o.cc).ToList();
                lst.ForEach(e =>
                {
                    e.bit_area = e.bit_area ?? "0";
                    e.st_ppto = e.st_ppto ?? string.Empty;
                    e.bit_area = lstDiv.Exists(d => d.cc.Replace(System.Environment.NewLine, string.Empty).Equals(e.cc)) ? lstDiv.FirstOrDefault(d => d.cc.Replace(System.Environment.NewLine, string.Empty).Equals(e.cc)).division.ToString() : e.bit_area;
                });
                return lst;
            }
            catch (Exception)
            { return new List<CcDTO>(); }
        }
        public List<CcDTO> lstObraActivas()
        {
            try
            {
                var lst = _contextEnkontrol.Select<CcDTO>(EnkontrolEnum.CplanProd, "SELECT * FROM cc WHERE st_ppto <> 'T' ORDER BY cc");
                var lstDiv = _context.tblC_CCDivision.OrderBy(o => o.cc).ToList();
                lst.ForEach(e =>
                {
                    e.bit_area = e.bit_area ?? "0";
                    e.st_ppto = e.st_ppto ?? string.Empty;
                    e.bit_area = lstDiv.Exists(d => d.cc.Replace(System.Environment.NewLine, string.Empty).Equals(e.cc)) ? lstDiv.FirstOrDefault(d => d.cc.Replace(System.Environment.NewLine, string.Empty).Equals(e.cc)).division.ToString() : e.bit_area;
                });
                return lst;
            }
            catch (Exception)
            { return new List<CcDTO>(); }
        }
        string AsignaNumNafin(string numProveedor)
        {
            try
            {
                var lstNafin = nafinFactoryServices.getNafinService().GetLstHanilitadosNumNafin();
                var objNafin = lstNafin.Where(x => x.NumProveedor.Equals(numProveedor)).FirstOrDefault();
                return objNafin.NumNafin.Replace(System.Environment.NewLine, string.Empty);
            }
            catch (Exception)
            {

                return numProveedor.ToString();
            }
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboProv()
        {
            try
            {
                var lstNafin = nafinFactoryServices.getNafinService().GetLstHanilitadosNumNafin();
                var lst = (List<Core.DTO.Principal.Generales.ComboDTO>)_contextEnkontrol.Where("SELECT numpro AS Value, (Convert(char, numpro) + ' - ' + nombre) AS Text FROM sp_proveedores order by numpro").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                lst.ForEach(e =>
                {
                    if (lstNafin.Exists(x => x.NumProveedor.Equals(e.Value)))
                        e.Prefijo = lstNafin.Where(x => x.NumProveedor.Equals(e.Value)).FirstOrDefault().NumNafin.Replace(System.Environment.NewLine, string.Empty);
                    else
                        e.Prefijo = e.Value;
                });
                return lst.ToList();
            }
            catch (Exception)
            { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        string asignarBanco(string banco)
        {
            switch (banco)
            {
                case "BANAMEX S.A.": return "BANAMEX";
                case "BBVA BANCOMER SA": return "BANCOMER";
                case "SCOTIABANK INVERLAT SA": return "SCOTIABANK";
                default: return banco;
            }
        }
        string asignarCif(string banco)
        {
            switch (banco)
            {
                case "BANAMEX": return "6544";
                case "BANCOMER": return string.Empty;
                case "BANORTE": return "3217";
                case "SCOTIABANK": return "32046";
                case "MONEX": return "1097745";
                default: return string.Empty;
            }
        }
        bool setAreaCuentaParaCadenasProductivas()
        {
            using (var _ctx = new MainContext())
            using (var _trans = _ctx.Database.BeginTransaction())
                try
                {
                    var lst = _ctx.tblC_CadenaProductiva.ToList().Where(w => w.area_cuenta == null || w.area_cuenta == "-").ToList();
                    lst.ForEach(cadena =>
                    {
                        var odbc = new OdbcConsultaDTO()
                        {
                            consulta = @"SELECT CAST(pol.area as varchar(10))+'-'+CAST(pol.cuenta_oc as varchar(10)) AS area_cuenta
                                        FROM DBA.sp_movprov prov
                                        INNER JOIN DBA.sc_movpol pol ON pol.numpro = prov.numpro AND pol.referencia = prov.factura AND pol.year = prov.year AND pol.mes = prov.mes AND pol.tp = prov.tp AND      pol.poliza  =         prov.poliza    AND     pol.linea = prov.linea
                                        WHERE prov.es_factura = 'S' AND prov.numpro = ? AND factura = ?"
                        };
                        odbc.parametros.Add(new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.Numeric, valor = cadena.numProveedor });
                        odbc.parametros.Add(new OdbcParameterDTO() { nombre = "factura", tipo = OdbcType.Numeric, valor = cadena.factura });
                        var res = _contextEnkontrol.Select<VencimientoDTO>(EnkontrolAmbienteEnum.Prod, odbc);
                        cadena.area_cuenta = res.Count == 0 ? "0-0" : res.FirstOrDefault().area_cuenta;
                        _ctx.tblC_CadenaProductiva.AddOrUpdate(cadena);
                        _ctx.SaveChanges();
                    });
                    _trans.Commit();
                    return true;
                }
                catch (Exception o_O) { return false; }
        }
        #region Proyección de cierre
        public List<tblC_FED_DetProyeccionCierre> getLstCadenaProductiva(List<string> lstCC)
        {
            var lstCadena = _context.tblC_CadenaProductiva.ToList()
                .Where(w => !w.pagado)
                .Where(w => lstCC.Contains(w.centro_costos)).ToList();
            var lst = lstCadena.Select(s => new tblC_FED_DetProyeccionCierre() 
            {
                id = 0,
                idConceptoDir = 26,
                tipo = tipoProyeccionCierreEnum.CadenaProductiva,
                factura = s.factura.ParseInt(),
                numpro = s.numProveedor.ParseInt(),
                descripcion = s.proveedor,
                cc = s.centro_costos,
                monto = s.monto,
                fechaFactura = s.fecha,
                esActivo = true,                
            }).ToList();
            return lst;
        }
        public List<tblC_FED_DetProyeccionCierre> getLstMoviminetoProveedor(List<string> lstCC)
        {
            var lstProv = _context.tblC_FED_CatProvedorArrendadora.Where(w => w.esActivo).ToList();
            var odbc = new OdbcConsultaDTO()
            {
                consulta = queryMoviminetoProveedor(lstCC, lstProv),
                parametros = paramMoviminetoProveedor(lstCC, lstProv)
            };
            var lst = _contextEnkontrol.Select<tblC_FED_DetProyeccionCierre>(EnkontrolAmbienteEnum.Prod, odbc);
            return lst;
        }
        public List<tblC_FED_DetProyeccionCierre> getLstMoviminetoArrendadora(List<string> lstCC)
        {
            var lstProv = _context.tblC_FED_CatProvedorArrendadora.Where(w => w.esActivo).ToList();
            var odbc = new OdbcConsultaDTO()
            {
                consulta = queryMoviminetoArrendadora(lstCC, lstProv),
                parametros = paramMoviminetoProveedor(lstCC, lstProv)
            };
            var lst = _contextEnkontrol.Select<tblC_FED_DetProyeccionCierre>(EnkontrolAmbienteEnum.Prod, odbc);
            return lst;
        }
        string queryMoviminetoProveedor(List<string> lstCC, List<tblC_FED_CatProvedorArrendadora> lstProv)
        {
            //WHERE (total NOT BETWEEN -1 AND 1) AND (monto NOT BETWEEN -1 AND 1)"
            return string.Format(@"SELECT 
                    mov.numpro, 
                    mov.cc, 
                    mov.factura, 
                    mov.fecha AS fechaFactura, 
                    prov.nombre descripcion, 
                    mov.monto, 
                    1 AS esActivo, 
                    3 AS tipo, 
                    25 AS idConceptoDIr,
                    mov.total 
                FROM
                (SELECT * FROM (
                    SELECT 
                        numpro, 
                        factura, 
                        MAX(CASE WHEN tm < 26 THEN cc ELSE '' END) as cc,
                        MAX(fecha) as fecha,
                        -SUM((monto + iva) * tipocambio) AS monto,
                        SUM(total) as total
                    FROM (SELECT * FROM sp_movprov WHERE cc in {0} AND numpro NOT IN {1}) Y
                    GROUP BY numpro, factura) X 
                WHERE ABS(X.total) > 1) mov
                INNER JOIN sp_proveedores prov ON prov.numpro = mov.numpro"
                , lstCC.ToParamInValue()
                , lstProv.Select(s => s.numrpo.ToString()).ToParamInValue());
        }
        string queryMoviminetoArrendadora(List<string> lstCC, List<tblC_FED_CatProvedorArrendadora> lstProv)
        {
            //WHERE (total NOT BETWEEN -1 AND 1) AND (monto NOT BETWEEN -1 AND 1)"
            return string.Format(@"SELECT * FROM (SELECT mov.numpro, mov.cc, mov.factura, MAX(mov.fecha) AS fechaFactura, MAX(prov.nombre) AS descripcion, -SUM((mov.monto + mov.iva) * mov.tipocambio) AS monto, 1 AS esActivo, 6 AS tipo, 31 AS idConceptoDIr ,SUM(mov.total) AS total
                                    FROM sp_movprov mov
                                    INNER JOIN sp_proveedores prov ON prov.numpro = mov.numpro
                                    WHERE mov.cc IN {0}
                                        AND mov.numpro IN {1}
                                    GROUP BY mov.numpro, mov.cc, mov.factura) x
                                WHERE (monto NOT BETWEEN -1 AND 1)"
                , lstCC.ToParamInValue()
                , lstProv.Select(s => s.numrpo.ToString()).ToParamInValue());
        }
        List<OdbcParameterDTO> paramMoviminetoProveedor(List<string> lstCC, List<tblC_FED_CatProvedorArrendadora> lstProv)
        {
            var lst = lstCC.Select(cc => new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.NVarChar, valor = cc }).ToList();
            lst.AddRange(lstProv.Select(prov => new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.NVarChar, valor = prov.numrpo }));
            return lst;
        }
        #endregion
        #region Proyección de cierre Arrendadora
        public List<tblC_FED_DetProyeccionCierre> getLstMoviminetoProveedorArrendadora(BusqProyeccionCierreDTO busq)
        {
            var lstProv = _context.tblC_FED_CatProvedorArrendadora.Where(w => w.esActivo).ToList();
            var odbc = new OdbcConsultaDTO()
            {
                consulta = queryMoviminetoProveedorArrendadora(lstProv),
                parametros = paramMoviminetoProveedorArrendadora(lstProv)
            };
            var lst = _contextEnkontrol.Select<tblC_FED_DetProyeccionCierre>(EnkontrolAmbienteEnum.Prod, odbc);
            if(busq.esCC)
            {
                lst = lst.Where(w => busq.lstCC.Contains(w.cc)).ToList();
            }
            else
            {
                lst = lst.Where(w => busq.lstAC.Contains(w.ac)).ToList();
            }
            return lst;
        }
        public List<tblC_FED_DetProyeccionCierre> getLstMoviminetoArrendadora(BusqProyeccionCierreDTO busq)
        {
            var lstProv = _context.tblC_FED_CatProvedorArrendadora.Where(w => w.esActivo).ToList();
            var odbc = new OdbcConsultaDTO()
            {
                consulta = queryMoviminetoArrendadoraArrendadora(lstProv),
                parametros = paramMoviminetoProveedorArrendadora(lstProv)
            };
            var lst = _contextEnkontrol.Select<tblC_FED_DetProyeccionCierre>(EnkontrolAmbienteEnum.Prod, odbc);
            if(busq.esCC)
            {
                lst = lst.Where(w => busq.lstCC.Contains(w.cc)).ToList();
            }
            else
            {
                lst = lst.Where(w => busq.lstAC.Contains(w.ac)).ToList();
            }
            return lst;
        }
        string queryMoviminetoProveedorArrendadora(List<tblC_FED_CatProvedorArrendadora> lstProv)
        {
            return string.Format(@"SELECT * FROM (SELECT mov.numpro, mov.cc, mov.factura, MAX(mov.fecha) AS fechaFactura, MAX(prov.nombre) AS descripcion, -SUM((mov.monto + mov.iva) * mov.tipocambio) AS monto, 1 AS esActivo, 3 AS tipo, 25 AS idConceptoDIr ,SUM(mov.total) AS total
,CAST(MAX(CASE WHEN pol.area IS NULL THEN 0 ELSE pol.area END) AS varchar(10)) +'-'+CAST(MAX(CASE WHEN pol.cuenta_oc IS NULL THEN 0 ELSE pol.cuenta_oc END)AS varchar(10)) AS ac
                                    FROM sp_movprov mov
                                    INNER JOIN sc_movpol pol ON pol.year = mov.year AND pol.mes = mov.mes AND pol.tp = mov.tp AND pol.poliza = mov.poliza AND pol.linea = mov.linea
                                    INNER JOIN sp_proveedores prov ON prov.numpro = mov.numpro
                                    WHERE  mov.numpro NOT IN {0}
                                    GROUP BY mov.numpro, mov.cc, mov.factura) x
                                WHERE (total NOT BETWEEN -1 AND 1) AND (monto NOT BETWEEN -1 AND 1)"
                , lstProv.Select(s => s.numrpo.ToString()).ToParamInValue());
        }
        string queryMoviminetoArrendadoraArrendadora(List<tblC_FED_CatProvedorArrendadora> lstProv)
        {
            return string.Format(@"SELECT * FROM (SELECT mov.numpro, mov.cc, mov.factura, MAX(mov.fecha) AS fechaFactura, MAX(prov.nombre) AS descripcion, -SUM((mov.monto + mov.iva) * mov.tipocambio) AS monto, 1 AS esActivo, 6 AS tipo, 31 AS idConceptoDIr ,SUM(mov.total) AS total
    ,CAST(MAX(CASE WHEN pol.area IS NULL THEN 0 ELSE pol.area END) AS varchar(10)) +'-'+CAST(MAX(CASE WHEN pol.cuenta_oc IS NULL THEN 0 ELSE pol.cuenta_oc END)AS varchar(10)) AS ac
                                    FROM sp_movprov mov
                                    INNER JOIN sc_movpol pol ON pol.year = mov.year AND pol.mes = mov.mes AND pol.tp = mov.tp AND pol.poliza = mov.poliza AND pol.linea = mov.linea
                                    INNER JOIN sp_proveedores prov ON prov.numpro = mov.numpro
                                    WHERE mov.numpro IN {0}
                                    GROUP BY mov.numpro, mov.cc, mov.factura) x
                                WHERE (total NOT BETWEEN -1 AND 1) AND (monto NOT BETWEEN -1 AND 1)"
                , lstProv.Select(s => s.numrpo.ToString()).ToParamInValue());
        }
        List<OdbcParameterDTO> paramMoviminetoProveedorArrendadora(List<tblC_FED_CatProvedorArrendadora> lstProv)
        {
            var lst = lstProv.Select(prov => new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.NVarChar, valor = prov.numrpo }).ToList();
            return lst;
        }
        #endregion
    }
}
