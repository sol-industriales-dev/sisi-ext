using Core.DAO.Enkontrol.Compras;
using Core.DTO;
using Core.DTO.Enkontrol.OrdenCompra;
using Core.DTO.Enkontrol.OrdenCompra.Proveedores;
using Core.DTO.Utils;
using Core.DTO.Utils.Data;
using Core.Entity.Enkontrol.Compras;
using Core.Entity.Principal.Alertas;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Enkontrol.Compras.Proveedores;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Data.DAO.Enkontrol.Compras
{
    public class AltaProveedorArrendadoraDAO : GenericDAO<tblCom_sp_proveedores>, IAltaProveedorDAO
    {       
        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        private readonly string RutaBase = @"\\REPOSITORIO\Proyecto\SIGOPLAN\COMPRAS\PROVEEDORESARRENDADORA";
        private const string RutaLocal = @"C:\Proyecto\SIGOPLAN\COMPRAS\PROVEEDORESARRENDADORA";
        private string NombreControlador = "AltaProveedorController";
        private bool productivo = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["enkontrolProductivo"]) == "1";
        bool esEdicion = false;
        public Dictionary<string, object> getProveedores()
        {
            try
            {
                List<tblCom_sp_proveedoresConsultaDTO> listaProveedoresMex = new List<tblCom_sp_proveedoresConsultaDTO>();
                List<tblCom_sp_proveedoresConsultaDTO> listaUnidasProveedores = new List<tblCom_sp_proveedoresConsultaDTO>();
                tblCom_sp_proveedoresConsultaDTO objListaProveedoresMex = new tblCom_sp_proveedoresConsultaDTO();
                tblCom_sp_proveedoresConsultaDTO objListaProveedores = new tblCom_sp_proveedoresConsultaDTO();
                List<tblCom_sp_proveedores> listaProveedores = _context.tblCom_sp_proveedores.Where(x => x.registroActivo && x.statusAutorizacion == false).ToList();
                var puedeDarVobo = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.idUsuario == vSesiones.sesionUsuarioDTO.id && e.PuedeVobo == true).FirstOrDefault();
                var puedeAutorizar = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.idUsuario == vSesiones.sesionUsuarioDTO.id && e.PuedeAutorizar == true).FirstOrDefault();
                var primerVobo = _context.tblCom_AutorizarProveedor.Where(x => x.registroActivo && x.PrimerVobo).Select(x => x.idUsuario).ToList();

                //bool estatusVobo = false;
                //bool estatusAutorizar = false;
                //if (puedeDarVobo != null)
                //{
                //    estatusVobo = true;
                //}
                //if (puedeAutorizar != null)
                //{
                //    estatusAutorizar = true;
                //}



                foreach (var prov in listaProveedores)
                {
                    //var TieneVobo = _context.tblCom_sp_proveedores.Where(x => x.registroActivo && x.id == prov.id && x.vobo == true).FirstOrDefault();
                    //var TieneAutorizacion = _context.tblCom_sp_proveedores.Where(x => x.registroActivo && x.id == prov.id && x.Autorizado == true).FirstOrDefault();
                    //if (TieneVobo != null)
                    //{
                    //    estatusAutorizar = true;
                    //    estatusVobo = false;
                    //}
                    //if (puedeAutorizar != null && TieneVobo != null)
                    //{
                    //    estatusAutorizar = true;
                    //}
                    //else
                    //{
                    //    estatusAutorizar = false;
                    //}
                    //if (TieneVobo != null && TieneAutorizacion != null)
                    //{
                    //    estatusAutorizar = false;
                    //}
                    objListaProveedoresMex = new tblCom_sp_proveedoresConsultaDTO();
                    objListaProveedoresMex.id = prov.id;
                    objListaProveedoresMex.numpro = prov.numpro;
                    objListaProveedoresMex.nombre = !string.IsNullOrEmpty(prov.persona_fisica) && prov.persona_fisica.Contains("S") ? PersonalUtilities.NombreCompletoMayusculas(prov.a_nombre, prov.a_paterno, prov.a_materno) : prov.nombre;
                    objListaProveedoresMex.direccion = prov.direccion;
                    objListaProveedoresMex.ciudad = prov.ciudad;
                    objListaProveedoresMex.rfc = prov.rfc;
                    objListaProveedoresMex.vobo = prov.vobo;
                    objListaProveedoresMex.Autorizado = prov.Autorizado;
                    objListaProveedoresMex.puedeDarPrimerVobo = !prov.primerVobo && primerVobo.Contains(vSesiones.sesionUsuarioDTO.id);
                    objListaProveedoresMex.puedeDarVobo = prov.primerVobo && !prov.Autorizado && puedeDarVobo != null && !prov.vobo;
                    objListaProveedoresMex.puedeAutorizar = prov.primerVobo && prov.vobo && puedeAutorizar != null && !prov.Autorizado;
                    objListaProveedoresMex.statusAutorizacion = prov.statusAutorizacion;
                    objListaProveedoresMex.esEnKontrol = false;

                    string descEstatus = "";

                    if (!prov.primerVobo)
                    {
                        descEstatus = "Pendiente VoBo1";
                    }
                    else
                    {
                        if (!prov.vobo)
                        {
                            descEstatus = "Pendiente VoBo2";
                        }
                        else
                        {
                            if (!prov.statusAutorizacion)
                            {
                                descEstatus = "Pendiente autorización";
                            }
                            else
                            {
                                descEstatus = "Autorizado";
                            }
                        }
                    }

                    objListaProveedoresMex.descEstatus = descEstatus;

                    listaProveedoresMex.Add(objListaProveedoresMex);
                }

                var conexion = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(EmpresaEnum.Arrendadora);

                var odbc = new OdbcConsultaDTO();
                odbc.consulta = string.Format(@"SELECT numpro,nombre,direccion,ciudad,rfc,persona_fisica,a_nombre,a_paterno,a_materno FROM sp_proveedores WHERE cancelado = 'A'");


                var ProvEkontrol = _contextEnkontrol.Select<tblCom_sp_proveedoresConsultaDTO>(conexion, odbc);

                foreach (var item in ProvEkontrol)
                {
                    objListaProveedores = new tblCom_sp_proveedoresConsultaDTO();
                    objListaProveedores.id = 0;
                    objListaProveedores.numpro = item.numpro;
                    objListaProveedores.nombre = !string.IsNullOrEmpty(item.persona_fisica) && item.persona_fisica.Contains("S") ? PersonalUtilities.NombreCompletoMayusculas(item.a_nombre, item.a_paterno, item.a_materno) : item.nombre;
                    objListaProveedores.direccion = item.direccion;
                    objListaProveedores.ciudad = item.ciudad;
                    objListaProveedores.rfc = item.rfc;
                    objListaProveedores.vobo = true;
                    objListaProveedores.Autorizado = false;
                    objListaProveedores.statusAutorizacion = true;
                    objListaProveedores.esEnKontrol = true;
                    objListaProveedores.descEstatus = "Autorizado";

                    listaUnidasProveedores.Add(objListaProveedores);
                }
                listaUnidasProveedores.AddRange(listaProveedoresMex);
                resultado.Add(ITEMS, listaUnidasProveedores);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }
        public Dictionary<string, object> obtenerDatosProveedores(int id, int numpro)
        {
            try
            {
                List<tblCom_sp_proveedoresDTO> lstProveedores = new List<tblCom_sp_proveedoresDTO>();
                List<tblCom_sp_proveedoresDTO> ListaProveedoresEK = new List<tblCom_sp_proveedoresDTO>();
                tblCom_sp_proveedoresDTO datosProveedores = new tblCom_sp_proveedoresDTO();
                var socioProveedor = "";
                var infoProveedor = _context.tblCom_sp_proveedores.Where(x => x.registroActivo && x.id == id).FirstOrDefault();
                var lstArchivosEK = _context.tblCom_ArchivosAdjuntosProveedores.Where(x => x.registroActivo && x.FK_numpro == numpro).ToList();
                var infoProveedorSocioEK = _context.tblCom_ProveedoresSocios.Where(e => e.registroActivo && e.FK_numpro == numpro).FirstOrDefault();
                if (infoProveedorSocioEK != null)
                {
                    socioProveedor = infoProveedorSocioEK.socios;
                }
                var conexion = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(EmpresaEnum.Arrendadora);

                var odbc = new OdbcConsultaDTO();
                odbc.consulta = string.Format(@"SELECT numpro, nomcorto, nombre, direccion, ciudad, cp, responsable, telefono1, telefono2, fax,
                                                        email, rfc, limcred, tmbase, descuento, condpago, moneda, cta_bancaria, tipo_prov, cancelado,
                                                        tipo_pago, inst_factoraje, cta_cheque, cve_banco, plaza_banco, tipo_cta, fecha_modifica_plazo_pago, usuario_modifica_plazo_pago, prov_exterior, filial,
                                                        tipo_tercero, tipo_operacion, curp, bit_factoraje, id_fiscal, nacionalidad, persona_fisica, a_paterno, a_materno, a_nombre,
                                                        tipo_pago_transferencia, bit_estrategico, swift_code, account_number, routing, numpro_factoraje, num_empleado, obliga_cfd, clave_id, numregidtrib,
                                                        fecha_mod, pp, ekuserweb, st_aut, st_vobo FROM sp_proveedores WHERE numpro = {0}", numpro);

                var ProvEkontrol = _contextEnkontrol.Select<tblCom_sp_proveedoresDTO>(conexion, odbc);

                odbc.consulta = string.Format(@"SELECT cuentas.numpro,cuentas.id_cta_dep,ban.banco as banco,ban.descripcion as descBanco,mon.clave as moneda,mon.moneda as descMoneda,cuentas.cuenta,cuentas.sucursal,cuentas.tipo_cta,cuentas.plaza,clabe,(SELECT CASE cuentas.tipo_cta
                                                            When 1 Then 'Cheque'
                                                            When 2 Then 'Plástico'
                                                            When 3 Then 'Transferencia'
                                                            END)as descCuenta,cuentas.plastico FROM sp_proveedores_cta_dep as cuentas
                                                         inner join moneda as mon on cuentas.moneda = mon.clave 
                                                         inner join sb_bancos as ban on cuentas.banco = ban.banco where cuentas.numpro= {0}", numpro);
                var ProvCuentasEkontrol = _contextEnkontrol.Select<tblCom_sp_proveedores_cta_depDTO>(conexion, odbc);

                var ListInformacionCuentasEK = new List<tblCom_sp_proveedores_cta_depDTO>();
                foreach (var item in ProvCuentasEkontrol)
                {
                    var InformacionCuentas = new tblCom_sp_proveedores_cta_depDTO();
                    InformacionCuentas.id = item.id;
                    InformacionCuentas.numpro = item.numpro;
                    InformacionCuentas.id_cta_dep = item.id_cta_dep;
                    InformacionCuentas.banco = item.banco;
                    InformacionCuentas.descBanco = item.descBanco;
                    InformacionCuentas.moneda = item.moneda;
                    InformacionCuentas.descMoneda = item.descMoneda;
                    InformacionCuentas.cuenta = item.cuenta;
                    InformacionCuentas.sucursal = item.sucursal;
                    InformacionCuentas.clabe = item.clabe;
                    InformacionCuentas.plaza = item.plaza;
                    InformacionCuentas.tipo_cta = item.tipo_cta;
                    InformacionCuentas.descCuenta = item.descCuenta;
                    InformacionCuentas.plastico = item.plastico;
                    InformacionCuentas.isKontrol = false;
                    InformacionCuentas.esEnKontrol = true;
                    ListInformacionCuentasEK.Add(InformacionCuentas);
                }
                var infoProveedorCuentas = _context.tblCom_sp_proveedores_cta_dep.Where(e => e.registroActivo && e.FK_idProv == id).ToList();
                var ListInformacionCuentas = new List<tblCom_sp_proveedores_cta_depDTO>();
                foreach (var item in infoProveedorCuentas)
                {
                    var InformacionCuentas = new tblCom_sp_proveedores_cta_depDTO();
                    InformacionCuentas.id = item.id;
                    InformacionCuentas.FK_idProv = item.FK_idProv;
                    InformacionCuentas.numpro = item.numpro;
                    InformacionCuentas.id_cta_dep = item.id_cta_dep;
                    InformacionCuentas.banco = item.banco;
                    InformacionCuentas.descBanco = item.descBanco;
                    InformacionCuentas.moneda = item.moneda;
                    InformacionCuentas.descMoneda = item.descMoneda;
                    InformacionCuentas.cuenta = item.cuenta;
                    InformacionCuentas.sucursal = item.sucursal;
                    InformacionCuentas.clabe = item.clabe;
                    InformacionCuentas.plaza = item.plaza;
                    InformacionCuentas.tipo_cta = item.tipo_cta;
                    InformacionCuentas.descCuenta = item.descCuenta;
                    InformacionCuentas.plastico = item.plastico;
                    InformacionCuentas.isKontrol = false;
                    InformacionCuentas.esEnKontrol = false;
                    ListInformacionCuentas.Add(InformacionCuentas);
                }
                ListInformacionCuentasEK.AddRange(ListInformacionCuentas);

                var archivoEK = new List<tblCom_ArchivosAdjuntosProveedores>();
                archivoEK = lstArchivosEK;
                if (ProvEkontrol.Count > 0)
                {
                    esEdicion = true;
                    foreach (var item in ProvEkontrol)
                    {
                        var objListaProveedores = new tblCom_sp_proveedoresDTO();
                        objListaProveedores.numpro = item.numpro;
                        objListaProveedores.nomcorto = item.nomcorto.Trim();
                        objListaProveedores.nombre = item.nombre.Trim();
                        objListaProveedores.direccion = item.direccion.Trim();
                        objListaProveedores.ciudad = item.ciudad;
                        objListaProveedores.cp = item.cp.Trim();
                        objListaProveedores.fax = item.fax;
                        objListaProveedores.responsable = item.responsable.Trim();
                        objListaProveedores.telefono1 = item.telefono1;
                        objListaProveedores.telefono2 = item.telefono2;
                        objListaProveedores.email = (item.email ?? "").Trim();
                        objListaProveedores.rfc = (item.rfc ?? "").Trim();
                        objListaProveedores.limcred = item.limcred;
                        objListaProveedores.tmbase = item.tmbase;
                        objListaProveedores.condpago = item.condpago;
                        objListaProveedores.moneda = item.moneda.Trim();
                        objListaProveedores.cta_bancaria = item.cta_bancaria;
                        objListaProveedores.tipo_prov = item.tipo_prov;
                        objListaProveedores.cancelado = item.cancelado.Trim();
                        objListaProveedores.tipo_pago = item.tipo_pago;
                        objListaProveedores.cta_cheque = item.cta_cheque;
                        objListaProveedores.cve_banco = item.cve_banco;
                        objListaProveedores.plaza_banco = item.plaza_banco;
                        objListaProveedores.tipo_cta = item.tipo_cta;
                        objListaProveedores.fecha_modifica_plazo_pago = item.fecha_modifica_plazo_pago;
                        objListaProveedores.usuario_modifica_plazo_pago = item.usuario_modifica_plazo_pago;
                        objListaProveedores.prov_exterior = (item.prov_exterior ?? "").Trim();
                        objListaProveedores.filial = (item.filial ?? "N").Trim();
                        objListaProveedores.tipo_tercero = item.tipo_tercero;
                        objListaProveedores.tipo_operacion = item.tipo_operacion;
                        objListaProveedores.curp = (item.curp ?? "").Trim();
                        objListaProveedores.id_fiscal = (item.id_fiscal ?? "").Trim();
                        objListaProveedores.nacionalidad = (item.nacionalidad ?? "").Trim();
                        objListaProveedores.persona_fisica = (item.persona_fisica ?? "N").Trim();
                        objListaProveedores.a_paterno = (item.a_paterno ?? "").Trim();
                        objListaProveedores.a_materno = (item.a_materno ?? "").Trim();
                        objListaProveedores.a_nombre = (item.a_nombre ?? "").Trim();
                        objListaProveedores.tipo_pago_transferencia = item.tipo_pago_transferencia;
                        objListaProveedores.bit_estrategico = item.bit_estrategico;
                        objListaProveedores.swift_code = item.swift_code;
                        objListaProveedores.account_number = item.account_number;
                        objListaProveedores.routing = item.routing;
                        objListaProveedores.numpro_factoraje = item.numpro_factoraje;
                        objListaProveedores.num_empleado = item.num_empleado;
                        objListaProveedores.obliga_cfd = item.obliga_cfd;
                        objListaProveedores.bit_factoraje = (item.bit_factoraje ?? "N").Trim();
                        objListaProveedores.socios = socioProveedor;
                        objListaProveedores.esEdicion = esEdicion;
                        objListaProveedores.lstCuentas = ListInformacionCuentasEK;
                        objListaProveedores.lstArchivos = archivoEK;

                        resultado.Add(ITEMS, objListaProveedores);
                    }
                }
                else
                {
                    var infoProveedorCuentasMX = _context.tblCom_sp_proveedores_cta_dep.Where(e => e.registroActivo && e.FK_idProv == infoProveedor.id).ToList();
                    var infoProveedorSocio = _context.tblCom_ProveedoresSocios.Where(e => e.registroActivo && e.FK_idProv == infoProveedor.id).FirstOrDefault();
                    var lstArchivos = _context.tblCom_ArchivosAdjuntosProveedores.Where(x => x.registroActivo && x.FK_idProv == infoProveedor.id).ToList();
                    var ListInformacionCuentasMX = new List<tblCom_sp_proveedores_cta_depDTO>();
                    foreach (var item in infoProveedorCuentasMX)
                    {
                        var InformacionCuentas = new tblCom_sp_proveedores_cta_depDTO();
                        InformacionCuentas.id = item.id;
                        InformacionCuentas.FK_idProv = item.FK_idProv;
                        InformacionCuentas.numpro = item.numpro;
                        InformacionCuentas.id_cta_dep = item.id_cta_dep;
                        InformacionCuentas.banco = item.banco;
                        InformacionCuentas.descBanco = item.descBanco;
                        InformacionCuentas.moneda = item.moneda;
                        InformacionCuentas.descMoneda = item.descMoneda;
                        InformacionCuentas.cuenta = item.cuenta;
                        InformacionCuentas.sucursal = item.sucursal;
                        InformacionCuentas.clabe = item.clabe;
                        InformacionCuentas.plaza = item.plaza;
                        InformacionCuentas.tipo_cta = item.tipo_cta;
                        InformacionCuentas.descCuenta = item.descCuenta;
                        InformacionCuentas.plastico = item.plastico;
                        InformacionCuentas.isKontrol = false;
                        InformacionCuentas.esEnKontrol = false;
                        ListInformacionCuentasMX.Add(InformacionCuentas);
                    }
                    var archivo = new List<tblCom_ArchivosAdjuntosProveedores>();
                    archivo = lstArchivos;


                    var InformacionProvedores = new tblCom_sp_proveedoresDTO();
                    InformacionProvedores.id = infoProveedor.id;
                    InformacionProvedores.numpro = infoProveedor.numpro;
                    InformacionProvedores.nomcorto = infoProveedor.nomcorto.Trim();
                    InformacionProvedores.nombre = infoProveedor.nombre.Trim();
                    InformacionProvedores.direccion = infoProveedor.direccion.Trim();
                    InformacionProvedores.ciudad = infoProveedor.ciudad;
                    InformacionProvedores.cp = infoProveedor.cp.Trim();
                    InformacionProvedores.fax = infoProveedor.fax.Trim();
                    InformacionProvedores.responsable = infoProveedor.responsable.Trim();
                    InformacionProvedores.telefono1 = infoProveedor.telefono1.Trim();
                    InformacionProvedores.telefono2 = infoProveedor.telefono2.Trim();
                    InformacionProvedores.email = infoProveedor.email.Trim();
                    InformacionProvedores.rfc = infoProveedor.rfc.Trim();
                    InformacionProvedores.limcred = infoProveedor.limcred;
                    InformacionProvedores.tmbase = infoProveedor.tmbase;
                    InformacionProvedores.condpago = infoProveedor.condpago;
                    InformacionProvedores.moneda = infoProveedor.moneda.Trim();
                    InformacionProvedores.cta_bancaria = infoProveedor.cta_bancaria.Trim();
                    InformacionProvedores.tipo_prov = infoProveedor.tipo_prov;
                    InformacionProvedores.cancelado = infoProveedor.cancelado.Trim();
                    InformacionProvedores.tipo_pago = infoProveedor.tipo_pago;
                    InformacionProvedores.cta_cheque = infoProveedor.cta_cheque;
                    InformacionProvedores.cve_banco = infoProveedor.cve_banco;
                    InformacionProvedores.plaza_banco = infoProveedor.plaza_banco;
                    InformacionProvedores.tipo_cta = infoProveedor.tipo_cta;
                    InformacionProvedores.fecha_modifica_plazo_pago = infoProveedor.fecha_modifica_plazo_pago;
                    InformacionProvedores.usuario_modifica_plazo_pago = infoProveedor.usuario_modifica_plazo_pago;
                    InformacionProvedores.prov_exterior = infoProveedor.prov_exterior;
                    InformacionProvedores.filial = infoProveedor.filial.Trim();
                    InformacionProvedores.tipo_tercero = infoProveedor.tipo_tercero;
                    InformacionProvedores.tipo_operacion = infoProveedor.tipo_operacion;
                    InformacionProvedores.curp = infoProveedor.curp.Trim();
                    InformacionProvedores.id_fiscal = infoProveedor.id_fiscal;
                    InformacionProvedores.nacionalidad = infoProveedor.nacionalidad.Trim();
                    InformacionProvedores.persona_fisica = infoProveedor.persona_fisica;
                    InformacionProvedores.a_paterno = infoProveedor.a_paterno.Trim();
                    InformacionProvedores.a_materno = infoProveedor.a_materno.Trim();
                    InformacionProvedores.a_nombre = infoProveedor.a_nombre.Trim();
                    InformacionProvedores.tipo_pago_transferencia = infoProveedor.tipo_pago_transferencia;
                    InformacionProvedores.bit_estrategico = infoProveedor.bit_estrategico;
                    InformacionProvedores.swift_code = infoProveedor.swift_code;
                    InformacionProvedores.account_number = infoProveedor.account_number;
                    InformacionProvedores.routing = infoProveedor.routing;
                    InformacionProvedores.numpro_factoraje = infoProveedor.numpro_factoraje;
                    InformacionProvedores.num_empleado = infoProveedor.num_empleado;
                    InformacionProvedores.obliga_cfd = infoProveedor.obliga_cfd;
                    InformacionProvedores.bit_factoraje = infoProveedor.bit_factoraje.Trim();
                    InformacionProvedores.esEdicion = esEdicion;
                    InformacionProvedores.socios = socioProveedor;
                    InformacionProvedores.lstCuentas = ListInformacionCuentas;
                    InformacionProvedores.lstArchivos = archivo;


                    resultado.Add(ITEMS, InformacionProvedores);
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
        private string SetNombreArchivo(string nombreBase, string fileName)
        {
            return String.Format("{0}{1}{2}", nombreBase, fileName.Split('.')[0], Path.GetExtension(fileName));
        }
        private dynamic consultaCheckProductivo(string consulta)
        {
            if (productivo)
            {
                return _contextEnkontrol.WhereComprasOrigen(consulta);
            }
            else
            {
                return _contextEnkontrolPrueba.Where(consulta);
            }
        }
        public Dictionary<string, object> GuardarProveedor(tblCom_sp_proveedoresDTO objProveedor, HttpPostedFileBase objFile)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                using (var connEk = ConexionEnkontrol())
                {
                    using (var transactionEk = connEk.BeginTransaction())
                    {
                        try
                        {
                            #region Validaciones
                            if (objProveedor.numpro <= 0) { throw new Exception("El numero de proveedor requerido"); }
                            if (objProveedor.nomcorto == "") { throw new Exception("Nombre corto de proveedor requerido"); }
                            if (objProveedor.nombre == "") { throw new Exception("nombre de proveedor requerido"); }
                            if (objProveedor.direccion == "") { throw new Exception("Dirección requerido"); }
                            if (objProveedor.responsable == "") { throw new Exception("Nombre del responsable requerido"); }
                            if (objProveedor.rfc == "") { throw new Exception("RFC requerido"); }
                            if (objProveedor.limcred <= 0) { throw new Exception("Limite de credito requerido"); }
                            if (objProveedor.tmbase <= 0) { throw new Exception("Tipo de movimiento requerido"); }
                            if (objProveedor.condpago < 0) { throw new Exception("Condición de pago invalido"); }
                            if (objProveedor.persona_fisica == "N" && objProveedor.socios == "") { throw new Exception("Socio requerido"); }

                            #endregion

                            List<tblCom_sp_proveedores> lstProveedores = new List<tblCom_sp_proveedores>();
                            tblCom_sp_proveedores datosProveedores = new tblCom_sp_proveedores();

                            if (objProveedor.esNuevo == 0)
                            {
                                var ExisteEnKontrol = consultaCheckProductivo(
                                    string.Format(@"SELECT * FROM sp_proveedores WHERE (numpro = '{0}' OR rfc = '{1}') AND moneda = {2}", objProveedor.numpro, objProveedor.rfc, objProveedor.moneda)
                                );

                                if (ExisteEnKontrol == null)
                                {
                                    #region SE REGISTRA EN SQLSERV MEXICO
                                    //CHECAR SI YA EXISTE EL PROVEEDORE EN ARRENDADORA
                                    var infoProveedor = _context.tblCom_sp_proveedores.Where(e => e.registroActivo && (e.numpro == objProveedor.numpro || e.rfc == objProveedor.rfc) && e.moneda == objProveedor.moneda).FirstOrDefault();

                                    //CHECAR SI YA EXISTE EL PROVEEDORE EN CONSTRUPLAN
                                    var objProveedorArr = new tblCom_sp_proveedores();
                                    using (var ctxARR = new MainContext(EmpresaEnum.Construplan))
                                    {
                                        objProveedorArr = ctxARR.tblCom_sp_proveedores.FirstOrDefault(e => e.registroActivo && (e.numpro == objProveedor.numpro || e.rfc == objProveedor.rfc) && e.moneda == objProveedor.moneda);
                                    }

                                    if (infoProveedor == null && objProveedorArr == null)
                                    {
                                        datosProveedores.numpro = objProveedor.numpro;
                                        datosProveedores.nomcorto = objProveedor.nomcorto;
                                        datosProveedores.nombre = objProveedor.nombre;
                                        datosProveedores.direccion = objProveedor.direccion;
                                        datosProveedores.ciudad = objProveedor.ciudad;
                                        datosProveedores.cp = objProveedor.cp;
                                        datosProveedores.responsable = objProveedor.responsable;
                                        datosProveedores.telefono1 = objProveedor.telefono1.Trim();
                                        datosProveedores.telefono2 = objProveedor.telefono2.Trim();
                                        datosProveedores.email = objProveedor.email;
                                        datosProveedores.fax = objProveedor.fax;
                                        datosProveedores.rfc = objProveedor.rfc;
                                        datosProveedores.limcred = objProveedor.limcred;
                                        datosProveedores.tmbase = objProveedor.tmbase;
                                        datosProveedores.condpago = objProveedor.condpago;
                                        datosProveedores.moneda = objProveedor.moneda;
                                        datosProveedores.cta_bancaria = objProveedor.cta_bancaria;
                                        datosProveedores.tipo_prov = objProveedor.tipo_prov;
                                        datosProveedores.cancelado = objProveedor.cancelado;
                                        datosProveedores.tipo_pago = objProveedor.tipo_pago;
                                        datosProveedores.cta_cheque = objProveedor.cta_cheque;
                                        datosProveedores.cve_banco = objProveedor.cve_banco;
                                        datosProveedores.plaza_banco = objProveedor.plaza_banco;
                                        datosProveedores.tipo_cta = objProveedor.tipo_cta;
                                        datosProveedores.fecha_modifica_plazo_pago = objProveedor.fecha_modifica_plazo_pago;
                                        datosProveedores.usuario_modifica_plazo_pago = objProveedor.usuario_modifica_plazo_pago;
                                        datosProveedores.prov_exterior = objProveedor.prov_exterior;
                                        datosProveedores.filial = objProveedor.filial;
                                        datosProveedores.tipo_tercero = objProveedor.tipo_tercero;
                                        datosProveedores.tipo_operacion = objProveedor.tipo_operacion;
                                        datosProveedores.curp = objProveedor.curp;
                                        datosProveedores.id_fiscal = objProveedor.id_fiscal;
                                        datosProveedores.nacionalidad = objProveedor.nacionalidad;
                                        datosProveedores.persona_fisica = objProveedor.persona_fisica;
                                        datosProveedores.a_paterno = objProveedor.a_paterno;
                                        datosProveedores.a_materno = objProveedor.a_materno;
                                        datosProveedores.a_nombre = objProveedor.a_nombre;
                                        datosProveedores.tipo_pago_transferencia = objProveedor.tipo_pago_transferencia;
                                        datosProveedores.bit_estrategico = objProveedor.bit_estrategico;
                                        datosProveedores.bit_factoraje = objProveedor.bit_factoraje;
                                        datosProveedores.cancelado = "C";
                                        datosProveedores.id_usuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                        datosProveedores.fechaCreacion = DateTime.Now;
                                        datosProveedores.registroActivo = true;


                                        _context.tblCom_sp_proveedores.Add(datosProveedores);
                                        _context.SaveChanges();

                                        var objPrimerVobo = _context.tblCom_AutorizarProveedor.Where(x => x.registroActivo && x.PrimerVobo).Select(x => x.idUsuario).ToList();
                                        //var objVobo = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.PuedeVobo == true).ToList();
                                        //var infoVobo = objVobo.FirstOrDefault();
                                        int numprov = Convert.ToInt32(objProveedor.numpro);
                                        foreach (var item in objPrimerVobo)
                                        {
                                            #region Alerta SIGOPLAN
                                            tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                                            objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                                            objNuevaAlerta.userRecibeID = item;
#if DEBUG
                                            //objNuevaAlerta.userRecibeID = 79626; //USUARIO ID:Aaron.
                                            objNuevaAlerta.userRecibeID = 13; //USUARIO ID:Administrador.
#endif
                                            objNuevaAlerta.tipoAlerta = 2;
                                            objNuevaAlerta.sistemaID = 4;
                                            objNuevaAlerta.visto = false;
                                            objNuevaAlerta.url = "/Enkontrol/AltaProveedor/AltaProveedor?numpro=" + objProveedor.numpro;
                                            objNuevaAlerta.objID = numprov != null ? numprov : 0;
                                            objNuevaAlerta.obj = "AltaProveedor";
                                            objNuevaAlerta.msj = "Alta Proveedor - " + objProveedor.numpro;
                                            objNuevaAlerta.documentoID = 0;
                                            objNuevaAlerta.moduloID = 0;
                                            _context.tblP_Alerta.Add(objNuevaAlerta);
                                            _context.SaveChanges();

                                            #endregion //ALERTA SIGPLAN
                                        }
                                        //int idProve = _context.tblCom_sp_proveedores.OrderByDescending(x => x.id).First().id;
                                        if (objFile != null)
                                        {
                                            #region SE REGISTRA EL ARCHIVO ADJUNTO
                                            //decimal FK_numpro = _context.tblCom_sp_proveedores.OrderByDescending(x => x.id).First().numpro;



                                            var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
#if DEBUG
                                            var CarpetaNueva = Path.Combine(RutaLocal, datosProveedores.numpro.ToString());
#else
                                            var CarpetaNueva = Path.Combine(RutaBase, datosProveedores.numpro.ToString());
#endif
                                            // Verifica si existe la carpeta y si no, la crea.
                                            if (GlobalUtils.VerificarExisteCarpeta(CarpetaNueva, true) == false)
                                            {
                                                dbContextTransaction.Rollback();
                                                resultado.Add(SUCCESS, false);
                                                resultado.Add(MESSAGE, "No se pudo crear la carpeta en el servidor.");
                                                return resultado;
                                            }


                                            string nombreArchivo = SetNombreArchivo("EvidenciaProveedor", objFile.FileName);
                                            string rutaArchivo = Path.Combine(CarpetaNueva, nombreArchivo);
                                            listaRutaArchivos.Add(Tuple.Create(objFile, rutaArchivo));

                                            // GUARDAR TABLA ARCHIVOS
                                            tblCom_ArchivosAdjuntosProveedores objEvidencia = new tblCom_ArchivosAdjuntosProveedores()
                                            {
                                                FK_idProv = datosProveedores.id,
                                                FK_numpro = datosProveedores.numpro,
                                                nombreArchivo = nombreArchivo,
                                                rutaArchivo = rutaArchivo,
                                                FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id,
                                                fechaCreacion = DateTime.Now,
                                                registroActivo = true
                                            };

                                            _context.tblCom_ArchivosAdjuntosProveedores.Add(objEvidencia);
                                            _context.SaveChanges();

                                            if (GlobalUtils.SaveHTTPPostedFile(objFile, rutaArchivo) == false)
                                            {
                                                dbContextTransaction.Rollback();
                                                resultado.Clear();
                                                resultado.Add(SUCCESS, false);
                                                resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                                                return resultado;
                                            }

                                            //resultado.Add(MESSAGE, "Se han cargado las evidencias con éxito, Favor de consultar en en apartado de Consultas.");
                                            //resultado.Add(SUCCESS, true);
                                            #endregion
                                        }
                                        else
                                        {
                                            throw new Exception("Favor de cargar al menos un archivo");
                                        }

                                        #region Guardado y Correo tabla socios
                                        tblCom_ProveedoresSocios infoSocios = new tblCom_ProveedoresSocios();
                                        var infoProveedorSocio = _context.tblCom_ProveedoresSocios.Where(e => e.registroActivo && e.FK_numpro == objProveedor.numpro).FirstOrDefault();
                                        infoSocios.FK_idProv = datosProveedores.id;
                                        infoSocios.FK_numpro = objProveedor.numpro;
                                        infoSocios.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                        infoSocios.fecha_creacion = DateTime.Now;
                                        infoSocios.registroActivo = true;

                                        //var usuarioQueAplicoVobo = _context.tblP_Usuario.FirstOrDefault(x => x.id == infoVobo.idUsuario);

                                        #region Correo de envio de informacion y documentos a toño
                                        List<string> lstCorreos = new List<string>();
                                        string PersonaFisicaMoral = "";
                                        string PersonaFisica = "";

                                        if (objProveedor.persona_fisica == "S")
                                        {
                                            PersonaFisicaMoral = "PERSONA FISICA";
                                            PersonaFisica = objProveedor.a_nombre + " " + objProveedor.a_paterno + "" + objProveedor.a_materno;
                                            infoSocios.socios = PersonaFisica;

                                        }
                                        else
                                        {
                                            PersonaFisicaMoral = "NOMBRES EN EL ACTA";
                                            infoSocios.socios = objProveedor.socios;
                                        }

                                        //lstCorreos.Add(usuarioQueAplicoVobo.correo);
                                        var correosUsuarios = _context.tblP_Usuario.Where(x => objPrimerVobo.Contains(x.id) && x.estatus).Select(x => x.correo).ToList();
                                        lstCorreos.AddRange(correosUsuarios);
                                        if (vSesiones.sesionUsuarioDTO.correo != null)
                                        {
                                            lstCorreos.Add(vSesiones.sesionUsuarioDTO.correo);
                                        }
#if DEBUG
                                        lstCorreos = new List<string>();
                                        lstCorreos.Add("martin.zayas@construplan.com.mx");
#endif
                                        string subject = "ALTA DE PROVEEDOR";
                                        //string body = string.Format("Buen día,<br> se informa " + "que el usuario " + objUsuarioAutorizoAlta.nombre + " " + objUsuarioAutorizoAlta.apellidoPaterno + " " + objUsuarioAutorizoAlta.apellidoMaterno + " confirmó el alta y el proveedor " + objProveedor.PRVCCODIGO + " " + objProveedor.PRVCNOMBRE + " se encuentra listo para ser autorizado <br>" +
                                        //                            "<br><br>Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>Construplan Peru > Compras > Proveedores.<br>" +
                                        //                            "Se informa que este es un correo autogenerado por el sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx</a>)." +
                                        //                            "No es necesario dar una respuesta. Gracias.");

                                        string sociosTemp = "";

                                        if (objProveedor != null && !string.IsNullOrEmpty(objProveedor.socios))
                                        {
                                            foreach (var item in objProveedor.socios)
                                            {
                                                switch (item)
                                                {
                                                    case ',':
                                                        sociosTemp += "<br/> ";
                                                        break;
                                                    case '\n':
                                                        sociosTemp += "<br/> ";
                                                        break;
                                                    default:
                                                        sociosTemp += item;
                                                        break;
                                                }
                                            }
                                        }

                                        string body = string.Format(@"<H2>Envío documentación para la revisión del proveedor dado de alta en la empresa Construplan Arrendadora con el núm. {0}</H2>
                                                      <br>
                                                       {1}
                                                      <br>
                                                      <H2>
                                                      {2}</H2>
                                                       <H2>
                                                      {3}</H2>
                                                      <br><br>Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>Construplan Arrendadora> Compras > Proveedores.<br>
                                                       Se informa que este es un correo autogenerado por el sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx</a>).
                                                       No es necesario dar una respuesta. Gracias.",
                                                               objProveedor.numpro,
                                                               htmlCorreo(datosProveedores.id),
                                                               PersonaFisicaMoral,
                                                               sociosTemp
                                                               );
                                        var lstArchives = new List<adjuntoCorreoDTO>();
                                        List<tblCom_ArchivosAdjuntosProveedores> lstArchivos = _context.tblCom_ArchivosAdjuntosProveedores.Where(w => w.FK_idProv == datosProveedores.id && w.registroActivo).ToList();

                                        if (lstArchivos != null)
                                        {
                                            foreach (var item in lstArchivos)
                                            {
                                                var fileStream = GlobalUtils.GetFileAsStream(item.rutaArchivo);
                                                using (var streamReader = new MemoryStream())
                                                {
                                                    fileStream.CopyTo(streamReader);
                                                    //downloadFiles.Add(streamReader.ToArray());

                                                    lstArchives.Add(new adjuntoCorreoDTO
                                                    {
                                                        archivo = streamReader.ToArray(),
                                                        nombreArchivo = Path.GetFileNameWithoutExtension(item.rutaArchivo),
                                                        extArchivo = Path.GetExtension(item.rutaArchivo)
                                                    });
                                                }
                                            }

                                            GlobalUtils.sendMailWithFilesReclutamientos(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject),
                                                  body, lstCorreos, lstArchives);

                                        }
                                        else
                                        {
                                            GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, lstCorreos);
                                        }
                                        #endregion

                                        _context.tblCom_ProveedoresSocios.Add(infoSocios);
                                        _context.SaveChanges();
                                        #endregion

                                        //int ultimoIdProv = _context.tblCom_sp_proveedores.OrderByDescending(x => x.id).FirstOrDefault().id;

                                        #region Guardado informacion de cuentas bancarias
                                        int consCuenta = 0;
                                        if (objProveedor.lstCuentas != null)
                                        {
                                            //#region Validaciones
                                            //if (objProveedor.lstCuentas[0].banco <= 0) { throw new Exception("Banco requerido"); }
                                            //if (objProveedor.lstCuentas[0].cuenta == "") { throw new Exception("Cuenta requerido"); }
                                            //if (objProveedor.lstCuentas[0].moneda == "") { throw new Exception("Moneda requerido"); }
                                            //if (objProveedor.lstCuentas[0].tipo_cta == "") { throw new Exception("Tipo de requerido"); }
                                            //if (objProveedor.lstCuentas[0].sucursal == "") { throw new Exception("Sucursal requerido"); }
                                            //#endregion
                                            foreach (var item in objProveedor.lstCuentas)
                                            {
                                                tblCom_sp_proveedores_cta_dep infoCuentas = new tblCom_sp_proveedores_cta_dep();

                                                consCuenta++;
                                                infoCuentas.FK_idProv = datosProveedores.id;
                                                infoCuentas.numpro = datosProveedores.numpro;
                                                infoCuentas.id_cta_dep = consCuenta;
                                                infoCuentas.banco = item.banco;
                                                infoCuentas.descBanco = item.descBanco;
                                                infoCuentas.moneda = item.moneda;
                                                infoCuentas.descMoneda = item.descMoneda;
                                                infoCuentas.cuenta = item.cuenta;
                                                infoCuentas.sucursal = item.sucursal;
                                                infoCuentas.clabe = item.clabe;
                                                infoCuentas.plaza = item.plaza;
                                                infoCuentas.tipo_cta = item.tipo_cta.ToString();
                                                infoCuentas.descCuenta = item.descCuenta;
                                                infoCuentas.plastico = item.plastico;
                                                infoCuentas.ind_cuenta_activa = 1;
                                                infoCuentas.ind_cuenta_def = 1;
                                                infoCuentas.bit_valida_captura = 1;
                                                infoCuentas.id_usuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                                infoCuentas.fechaCreacion = DateTime.Now;
                                                infoCuentas.registroActivo = true;

                                                _context.tblCom_sp_proveedores_cta_dep.Add(infoCuentas);
                                                _context.SaveChanges();
                                            }


                                        }
                                        #endregion
                                    }
                                    else { throw new Exception("El proveedor que esta intentando crear se encuentra en proceso de autorizacion"); }
                                    #endregion
                                }
                                else
                                {
                                    throw new Exception("El proveedor que esta intentando crear ya se encuentra dado de alta.");
                                }
                            }
                            else
                            {
                                #region edicion mexico y  ekontrol
                                if (objProveedor.esEdicionEnKontrol != true)
                                {
                                    var infoProveedor = _context.tblCom_sp_proveedores.Where(e => e.registroActivo && e.id == objProveedor.id).FirstOrDefault();
                                    #region edicion mexico
                                    infoProveedor.numpro = objProveedor.numpro;
                                    infoProveedor.nomcorto = objProveedor.nomcorto;
                                    infoProveedor.nombre = objProveedor.nombre;
                                    infoProveedor.direccion = objProveedor.direccion;
                                    infoProveedor.ciudad = objProveedor.ciudad;
                                    infoProveedor.cp = objProveedor.cp;
                                    infoProveedor.fax = objProveedor.fax;
                                    infoProveedor.responsable = objProveedor.responsable;
                                    infoProveedor.telefono1 = objProveedor.telefono1;
                                    infoProveedor.telefono2 = objProveedor.telefono2;
                                    infoProveedor.email = objProveedor.email;
                                    infoProveedor.rfc = objProveedor.rfc;
                                    infoProveedor.limcred = objProveedor.limcred;
                                    infoProveedor.tmbase = objProveedor.tmbase;
                                    infoProveedor.condpago = objProveedor.condpago;
                                    infoProveedor.moneda = objProveedor.moneda.Trim();
                                    infoProveedor.cta_bancaria = objProveedor.cta_bancaria;
                                    infoProveedor.tipo_prov = objProveedor.tipo_prov;
                                    infoProveedor.cancelado = objProveedor.cancelado;
                                    infoProveedor.tipo_pago = objProveedor.tipo_pago;
                                    infoProveedor.cta_cheque = objProveedor.cta_cheque;
                                    infoProveedor.cve_banco = objProveedor.cve_banco;
                                    infoProveedor.plaza_banco = objProveedor.plaza_banco;
                                    infoProveedor.tipo_cta = objProveedor.tipo_cta;
                                    infoProveedor.fecha_modifica_plazo_pago = objProveedor.fecha_modifica_plazo_pago;
                                    infoProveedor.usuario_modifica_plazo_pago = objProveedor.usuario_modifica_plazo_pago;
                                    infoProveedor.prov_exterior = objProveedor.prov_exterior;
                                    infoProveedor.filial = objProveedor.filial.Trim();
                                    infoProveedor.tipo_tercero = objProveedor.tipo_tercero;
                                    infoProveedor.tipo_operacion = objProveedor.tipo_operacion;
                                    infoProveedor.curp = objProveedor.curp;
                                    infoProveedor.id_fiscal = objProveedor.id_fiscal;
                                    infoProveedor.nacionalidad = objProveedor.nacionalidad;
                                    infoProveedor.persona_fisica = objProveedor.persona_fisica.Trim();
                                    infoProveedor.a_paterno = objProveedor.a_paterno;
                                    infoProveedor.a_materno = objProveedor.a_materno;
                                    infoProveedor.a_nombre = objProveedor.a_nombre;
                                    infoProveedor.tipo_pago_transferencia = objProveedor.tipo_pago_transferencia;
                                    infoProveedor.bit_estrategico = objProveedor.bit_estrategico;
                                    infoProveedor.swift_code = objProveedor.swift_code;
                                    infoProveedor.account_number = objProveedor.account_number;
                                    infoProveedor.routing = objProveedor.routing;
                                    infoProveedor.numpro_factoraje = objProveedor.numpro_factoraje;
                                    infoProveedor.num_empleado = objProveedor.num_empleado;
                                    infoProveedor.obliga_cfd = objProveedor.obliga_cfd;
                                    infoProveedor.id_usuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                    infoProveedor.fechaModificacion = DateTime.Now;
                                    _context.SaveChanges();


                                    var infoProveedorSocioEditar = _context.tblCom_ProveedoresSocios.Where(e => e.registroActivo && e.FK_idProv == infoProveedor.id).FirstOrDefault();

                                    infoProveedorSocioEditar.socios = objProveedor.socios;
                                    infoProveedorSocioEditar.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                    infoProveedorSocioEditar.fecha_modificacion = DateTime.Now;

                                    _context.SaveChanges();

                                    #region cuentas
                                    if (objProveedor.lstCuentasAeliminar != null)
                                    {
                                        //#region Validaciones
                                        //if (objProveedor.lstCuentas[0].banco <= 0) { throw new Exception("Banco requerido"); }
                                        //if (objProveedor.lstCuentas[0].cuenta == "") { throw new Exception("Cuenta requerido"); }
                                        //if (objProveedor.lstCuentas[0].tipo_cta == "") { throw new Exception("Tipo de requerido"); }
                                        //if (objProveedor.lstCuentas[0].sucursal == "") { throw new Exception("Sucursal requerido"); }
                                        //#endregion
                                        foreach (var item in objProveedor.lstCuentasAeliminar)
                                        {
                                            var infoProveedorCuentasEditar = _context.tblCom_sp_proveedores_cta_dep.Where(e => e.registroActivo && e.id == item.id).FirstOrDefault();

                                            infoProveedorCuentasEditar.id_usuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                            infoProveedorCuentasEditar.fechaModificacion = DateTime.Now;
                                            infoProveedorCuentasEditar.registroActivo = false;

                                            _context.SaveChanges();
                                        }
                                        int consCuenta = 0;
                                        int idProveedor = 0;
                                        foreach (var item in objProveedor.lstCuentas)
                                        {
                                            tblCom_sp_proveedores_cta_dep infoCuentas = new tblCom_sp_proveedores_cta_dep();
                                            var infoProveedorCuentas = _context.tblCom_sp_proveedores_cta_dep.Where(e => e.registroActivo && e.id == item.id).FirstOrDefault();

                                            if (infoProveedorCuentas == null)
                                            {
                                                consCuenta++;
                                                infoCuentas.FK_idProv = infoProveedor.id;
                                                infoCuentas.numpro = item.numpro;
                                                infoCuentas.id_cta_dep = consCuenta;
                                                infoCuentas.banco = item.banco;
                                                infoCuentas.descBanco = item.descBanco;
                                                infoCuentas.moneda = item.moneda;
                                                infoCuentas.descMoneda = item.descMoneda;
                                                infoCuentas.cuenta = item.cuenta;
                                                infoCuentas.sucursal = item.sucursal;
                                                infoCuentas.clabe = item.clabe;
                                                infoCuentas.plaza = item.plaza;
                                                infoCuentas.tipo_cta = item.tipo_cta.ToString();
                                                infoCuentas.descCuenta = item.descCuenta;
                                                infoCuentas.plastico = item.plastico;
                                                infoCuentas.ind_cuenta_activa = 1;
                                                infoCuentas.ind_cuenta_def = 1;
                                                infoCuentas.bit_valida_captura = 1;
                                                infoCuentas.id_usuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                                infoCuentas.fechaCreacion = DateTime.Now;
                                                infoCuentas.registroActivo = true;
                                                _context.tblCom_sp_proveedores_cta_dep.Add(infoCuentas);
                                                _context.SaveChanges();
                                            }
                                            else
                                            {
                                                idProveedor = infoProveedorCuentas.FK_idProv;
                                                consCuenta = infoProveedorCuentas.id_cta_dep;
                                            }
                                        }
                                    }

                                    #endregion

                                    #region ver si hay archivos

                                    if (objProveedor.lstArchivosAeliminar != null)
                                    {

                                        foreach (var item in objProveedor.lstArchivosAeliminar)
                                        {
                                            var infoProveedorArchivo = _context.tblCom_ArchivosAdjuntosProveedores.Where(e => e.registroActivo && e.id == item).FirstOrDefault();

                                            infoProveedorArchivo.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                            infoProveedorArchivo.fechaModificacion = DateTime.Now;
                                            infoProveedorArchivo.registroActivo = false;

                                            _context.SaveChanges();
                                        }

                                    }

                                    if (objFile == null && !_context.tblCom_ArchivosAdjuntosProveedores.Any(x => x.registroActivo && x.FK_idProv == infoProveedor.id))
                                    {
                                        throw new Exception("Favor de agregar al menos un archivo");
                                    }

                                    if (objFile != null)
                                    {
                                        #region SE REGISTRA EL ARCHIVO ADJUNTO

                                        var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
#if DEBUG
                                        var CarpetaNueva = Path.Combine(RutaLocal, infoProveedor.numpro.ToString());
#else
                                        var CarpetaNueva = Path.Combine(RutaBase, infoProveedor.numpro.ToString());
#endif
                                        //// Verifica si existe la carpeta y si no, la crea.
                                        //if (GlobalUtils.VerificarExisteCarpeta(CarpetaNueva, true) == false)
                                        //{
                                        //    dbContextTransaction.Rollback();
                                        //    resultado.Add(SUCCESS, false);
                                        //    resultado.Add(MESSAGE, "No se pudo crear la carpeta en el servidor.");
                                        //    return resultado;
                                        //}


                                        string nombreArchivo = SetNombreArchivo("EvidenciaProveedor", objFile.FileName);
                                        string rutaArchivo = Path.Combine(CarpetaNueva, nombreArchivo);
                                        listaRutaArchivos.Add(Tuple.Create(objFile, rutaArchivo));

                                        // GUARDAR TABLA ARCHIVOS
                                        tblCom_ArchivosAdjuntosProveedores objEvidencia = new tblCom_ArchivosAdjuntosProveedores()
                                        {
                                            FK_idProv = infoProveedor.id,
                                            FK_numpro = infoProveedor.numpro,
                                            nombreArchivo = nombreArchivo,
                                            rutaArchivo = rutaArchivo,
                                            FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id,
                                            fechaCreacion = DateTime.Now,
                                            registroActivo = true
                                        };

                                        _context.tblCom_ArchivosAdjuntosProveedores.Add(objEvidencia);
                                        _context.SaveChanges();

                                        if (GlobalUtils.SaveHTTPPostedFile(objFile, rutaArchivo) == false)
                                        {
                                            dbContextTransaction.Rollback();
                                            resultado.Clear();
                                            resultado.Add(SUCCESS, false);
                                            resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                                            return resultado;
                                        }

                                        //resultado.Add(MESSAGE, "Se han cargado las evidencias con éxito, Favor de consultar en en apartado de Consultas.");
                                        //resultado.Add(SUCCESS, true);
                                        #endregion

                                    }
                                    #endregion
                                    #endregion
                                }
                                else
                                {
                                    #region Edicion en Kontrol
                                    var updateProv = @"
                                                   UPDATE sp_proveedores 
                                                    SET numpro = ?
                                                        ,nomcorto = ?
                                                        ,nombre = ?
                                                        ,direccion = ?
                                                        ,ciudad = ?
                                                        ,cp = ?
                                                        ,responsable = ?
                                                        ,telefono1 = ?
                                                        ,telefono2 = ?
                                                        ,fax = ?
                                                        ,email = ?
                                                        ,rfc = ?
                                                        ,limcred = ?
                                                        ,tmbase = ?
                                                        ,descuento = ?
                                                        ,condpago = ?
                                                        ,moneda = ?
                                                        ,cta_bancaria = ?
                                                        ,tipo_prov = ?
                                                        ,cancelado = ?
                                                        ,tipo_pago = ?
                                                        ,inst_factoraje = ?
                                                        ,cta_cheque = ?
                                                        ,cve_banco = ?
                                                        ,plaza_banco = ?
                                                        ,tipo_cta = ?
                                                        ,fecha_modifica_plazo_pago = ?
                                                        ,usuario_modifica_plazo_pago = ?
                                                        ,prov_exterior = ?
                                                        ,filial = ?
                                                        ,tipo_tercero = ?
                                                        ,tipo_operacion = ?
                                                        ,curp = ?
                                                        ,bit_factoraje = ?
                                                        ,id_fiscal = ?
                                                        ,nacionalidad = ?
                                                        ,persona_fisica = ?
                                                        ,a_paterno = ?
                                                        ,a_materno = ?
                                                        ,a_nombre = ?
                                                        ,tipo_pago_transferencia = ?
                                                        ,bit_estrategico = ?
                                                        ,swift_code = ?
                                                        ,account_number = ?
                                                        ,routing = ?
                                                        ,numpro_factoraje = ?
                                                        ,num_empleado = ?
                                                        ,obliga_cfd = ?
                                                        ,clave_id = ?
                                                        ,numregidtrib = ?
                                                        ,fecha_mod = ?
                                                        ,pp = ?
                                                        ,ekuserweb = ?
                                                        ,st_aut = ?
                                                        ,st_vobo = ?
                                                      WHERE numpro = ?";

                                    using (var cmd = new OdbcCommand(updateProv))
                                    {
                                        //numpro, nomcorto, nombre, direccion, ciudad, cp, responsable, telefono1, telefono2, fax,
                                        cmd.Parameters.Add("@numpro", OdbcType.Numeric).Value = objProveedor.numpro;
                                        cmd.Parameters.Add("@nomcorto", OdbcType.Char).Value = (objProveedor.nomcorto ?? "").Trim();
                                        cmd.Parameters.Add("@nombre", OdbcType.Char).Value = (objProveedor.nombre ?? "").Trim();
                                        cmd.Parameters.Add("@direccion", OdbcType.Char).Value = objProveedor.direccion;
                                        cmd.Parameters.Add("@ciudad", OdbcType.Char).Value = (objProveedor.ciudad ?? "");
                                        cmd.Parameters.Add("@cp", OdbcType.Char).Value = (objProveedor.cp ?? "").Trim();
                                        cmd.Parameters.Add("@responsable", OdbcType.Char).Value = (objProveedor.responsable ?? "").Trim();
                                        cmd.Parameters.Add("@telefono1", OdbcType.Char).Value = (objProveedor.telefono1 ?? "").Trim();
                                        cmd.Parameters.Add("@telefono2", OdbcType.Char).Value = (objProveedor.telefono2 ?? "").Trim();
                                        cmd.Parameters.Add("@fax", OdbcType.Char).Value = (objProveedor.fax ?? "").Trim();

                                        //  email, rfc, limcred, tmbase, condpago, moneda, cta_bancaria, tipo_prov, cancelado,
                                        cmd.Parameters.Add("@email", OdbcType.VarChar).Value = (objProveedor.email ?? "").Trim();
                                        cmd.Parameters.Add("@rfc", OdbcType.Char).Value = (objProveedor.rfc ?? "").Trim();
                                        cmd.Parameters.Add("@limcred", OdbcType.Numeric).Value = objProveedor.limcred;
                                        cmd.Parameters.Add("@tmbase", OdbcType.Numeric).Value = objProveedor.tmbase;
                                        cmd.Parameters.Add("@descuento", OdbcType.Numeric).Value = 0; //falta agregar
                                        cmd.Parameters.Add("@condpago", OdbcType.Numeric).Value = objProveedor.condpago;
                                        cmd.Parameters.Add("@moneda", OdbcType.Char).Value = (objProveedor.moneda ?? "").Trim();
                                        cmd.Parameters.Add("@cta_bancaria", OdbcType.Char).Value = (objProveedor.cta_bancaria ?? "").Trim();
                                        cmd.Parameters.Add("@tipo_prov", OdbcType.Numeric).Value = objProveedor.tipo_prov;
                                        cmd.Parameters.Add("@cancelado", OdbcType.Char).Value = (objProveedor.cancelado ?? "").Trim();

                                        //tipo_pago, inst_factoraje, cta_cheque, cve_banco, plaza_banco, tipo_cta, fecha_modifica_plazo_pago, usuario_modifica_plazo_pago, prov_exterior, filial,
                                        cmd.Parameters.Add("@tipo_pago", OdbcType.Char).Value = "";
                                        cmd.Parameters.Add("@inst_factoraje", OdbcType.Numeric).Value = 0;
                                        cmd.Parameters.Add("@cta_cheque", OdbcType.Char).Value = (objProveedor.cta_cheque ?? "").Trim();
                                        cmd.Parameters.Add("@cve_banco", OdbcType.Char).Value = (objProveedor.cve_banco ?? "").Trim();
                                        cmd.Parameters.Add("@plaza_banco", OdbcType.Char).Value = (objProveedor.plaza_banco ?? "").Trim();
                                        cmd.Parameters.Add("@tipo_cta", OdbcType.Char).Value = (objProveedor.tipo_cta ?? "").Trim();
                                        cmd.Parameters.Add("@fecha_modifica_plazo_pago", OdbcType.DateTime).Value = objProveedor.fecha_modifica_plazo_pago ?? DateTime.Now;
                                        cmd.Parameters.Add("@usuario_modifica_plazo_pago", OdbcType.Numeric).Value = objProveedor.usuario_modifica_plazo_pago;
                                        cmd.Parameters.Add("@prov_exterior", OdbcType.Char).Value = "";
                                        cmd.Parameters.Add("@filial", OdbcType.Char).Value = (objProveedor.filial ?? "").Trim();

                                        //tipo_tercero, tipo_operacion, curp, bit_factoraje, id_fiscal, nacionalidad, persona_fisica, a_paterno, a_materno, a_nombre,
                                        cmd.Parameters.Add("@tipo_tercero", OdbcType.Numeric).Value = objProveedor.tipo_tercero;
                                        cmd.Parameters.Add("@tipo_operacion", OdbcType.Numeric).Value = objProveedor.tipo_operacion;
                                        cmd.Parameters.Add("@curp", OdbcType.Char).Value = (objProveedor.curp ?? "").Trim();
                                        cmd.Parameters.Add("@bit_factoraje", OdbcType.Char).Value = (objProveedor.bit_factoraje ?? "").Trim();
                                        cmd.Parameters.Add("@id_fiscal", OdbcType.Char).Value = (objProveedor.id_fiscal ?? "").Trim();
                                        cmd.Parameters.Add("@nacionalidad", OdbcType.Char).Value = (objProveedor.nacionalidad ?? "").Trim();
                                        cmd.Parameters.Add("@persona_fisica", OdbcType.Char).Value = (objProveedor.persona_fisica ?? "N").Trim();
                                        cmd.Parameters.Add("@a_paterno", OdbcType.VarChar).Value = (objProveedor.a_paterno ?? "").Trim();
                                        cmd.Parameters.Add("@a_materno", OdbcType.VarChar).Value = (objProveedor.a_materno ?? "").Trim();
                                        cmd.Parameters.Add("@a_nombre", OdbcType.VarChar).Value = (objProveedor.a_nombre ?? "").Trim();

                                        //tipo_pago_transferencia, bit_estrategico, swift_code, account_number, routing, numpro_factoraje, num_empleado, obliga_cfd, clave_id, numregidtrib,
                                        cmd.Parameters.Add("@tipo_pago_transferencia", OdbcType.Numeric).Value = objProveedor.tipo_pago_transferencia;
                                        cmd.Parameters.Add("@bit_estrategico", OdbcType.Numeric).Value = objProveedor.bit_estrategico;
                                        cmd.Parameters.Add("@swift_code", OdbcType.VarChar).Value = "";
                                        cmd.Parameters.Add("@account_number", OdbcType.VarChar).Value = "";
                                        cmd.Parameters.Add("@routing", OdbcType.VarChar).Value = "";
                                        cmd.Parameters.Add("@numpro_factoraje", OdbcType.VarChar).Value = "";
                                        cmd.Parameters.Add("@num_empleado", OdbcType.Numeric).Value = 0;
                                        cmd.Parameters.Add("@obliga_cfd", OdbcType.Numeric).Value = objProveedor.obliga_cfd;
                                        cmd.Parameters.Add("@clave_id", OdbcType.Char).Value = "";
                                        cmd.Parameters.Add("@numregidtrib", OdbcType.VarChar).Value = "";

                                        //fecha_mod, pp, ekuserweb, st_aut, st_vobo
                                        cmd.Parameters.Add("@fecha_mod", OdbcType.Date).Value = DateTime.Now.Date;
                                        cmd.Parameters.Add("@pp", OdbcType.VarChar).Value = "S";
                                        cmd.Parameters.Add("@ekuserweb", OdbcType.Int).Value = 0;
                                        cmd.Parameters.Add("@st_aut", OdbcType.Bit).Value = false;
                                        cmd.Parameters.Add("@st_vobo", OdbcType.Bit).Value = false;
                                        cmd.Parameters.Add("@numpro", OdbcType.Numeric).Value = objProveedor.numpro;
                                        cmd.Connection = transactionEk.Connection;
                                        cmd.Transaction = transactionEk;
                                        cmd.ExecuteNonQuery();
                                    }

                                    var infoProveedorSocioEditar = _context.tblCom_ProveedoresSocios.Where(e => e.registroActivo && e.FK_numpro == objProveedor.numpro).FirstOrDefault();
                                    if (infoProveedorSocioEditar != null)
                                    {
                                        infoProveedorSocioEditar.socios = objProveedor.socios;
                                        infoProveedorSocioEditar.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                        infoProveedorSocioEditar.fecha_modificacion = DateTime.Now;

                                        _context.SaveChanges();

                                    }
                                    else
                                    {
                                        tblCom_ProveedoresSocios infoSocios = new tblCom_ProveedoresSocios();
                                        infoSocios.FK_numpro = objProveedor.numpro;
                                        infoSocios.socios = objProveedor.socios;
                                        infoSocios.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                        infoSocios.fecha_creacion = DateTime.Now;
                                        infoSocios.registroActivo = true;

                                        _context.tblCom_ProveedoresSocios.Add(infoSocios);
                                        _context.SaveChanges();
                                    }


                                    #region cuentas
                                    if (objProveedor.lstCuentasAeliminar != null)
                                    {
                                        foreach (var item in objProveedor.lstCuentasAeliminar)
                                        {
                                            var infoProveedorCuentasEditar = _context.tblCom_sp_proveedores_cta_dep.Where(e => e.registroActivo && e.id == item.id).FirstOrDefault();
                                            if (infoProveedorCuentasEditar != null)
                                            {
                                                infoProveedorCuentasEditar.id_usuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                                infoProveedorCuentasEditar.fechaModificacion = DateTime.Now;
                                                infoProveedorCuentasEditar.registroActivo = false;
                                                _context.SaveChanges();
                                            }


                                            var listcuentaEkontrol = consultaCheckProductivo(string.Format(@"SELECT numpro,id_cta_dep FROM sp_proveedores_cta_dep WHERE numpro = {0} AND id_cta_dep={1} ", item.numpro, item.id_cta_dep));
                                            var lisObjCuentaEkontrol = (List<lstCuentasAEliminarProveedoresDTO>)listcuentaEkontrol.ToObject<List<lstCuentasAEliminarProveedoresDTO>>();
                                            if (listcuentaEkontrol != null)
                                            {
                                                foreach (var cuenta in lisObjCuentaEkontrol)
                                                {
                                                    var consulta = @"DELETE from sp_proveedores_cta_dep WHERE numpro = ? AND id_cta_dep = ?";

                                                    using (var cmd = new OdbcCommand(consulta))
                                                    {

                                                        cmd.Parameters.Add("@numpro", OdbcType.Numeric).Value = cuenta.numpro;
                                                        cmd.Parameters.Add("@id_cta_dep", OdbcType.Int).Value = cuenta.id_cta_dep;

                                                        cmd.Connection = transactionEk.Connection;
                                                        cmd.Transaction = transactionEk;
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    int idProveedor = 0;
                                    int consCuenta = 0;

                                    var consecutivoCuenta = consultaCheckProductivo(string.Format(@"SELECT id_cta_dep FROM sp_proveedores_cta_dep WHERE numpro = {0} order by id_cta_dep desc ", objProveedor.numpro));

                                    if (consecutivoCuenta != null)
                                    {
                                        var ultimaCuenta = (List<tblCom_sp_proveedores_cta_depDTO>)consecutivoCuenta.ToObject<List<tblCom_sp_proveedores_cta_depDTO>>();
                                        consCuenta = ultimaCuenta[0].id_cta_dep;
                                    }
                                    //List<tblCom_sp_proveedores_cta_dep> ListInfoCuentasEK = new List<tblCom_sp_proveedores_cta_dep>();
                                    if (objProveedor.esEdicionEnKontrol)
                                    {
                                        foreach (var item in objProveedor.lstCuentasNuevas)
                                        {
                                            tblCom_sp_proveedores_cta_dep infoCuentasDelKontrol = new tblCom_sp_proveedores_cta_dep();
                                            var infoProveedorCuentasDelKontrol = _context.tblCom_sp_proveedores_cta_dep.Where(e => e.registroActivo && e.numpro == item.numpro).ToList().OrderByDescending(x => x.id_cta_dep).FirstOrDefault();

                                            if (infoProveedorCuentasDelKontrol == null)
                                            {
                                                consCuenta++;
                                                infoCuentasDelKontrol.FK_idProv = (int)objProveedor.numpro;
                                                infoCuentasDelKontrol.numpro = item.numpro;
                                                infoCuentasDelKontrol.id_cta_dep = consCuenta;
                                                infoCuentasDelKontrol.banco = item.banco;
                                                infoCuentasDelKontrol.descBanco = item.descBanco;
                                                infoCuentasDelKontrol.moneda = item.moneda;
                                                infoCuentasDelKontrol.descMoneda = item.descMoneda;
                                                infoCuentasDelKontrol.cuenta = item.cuenta;
                                                infoCuentasDelKontrol.sucursal = item.sucursal;
                                                infoCuentasDelKontrol.clabe = item.clabe;
                                                infoCuentasDelKontrol.plaza = item.plaza;
                                                infoCuentasDelKontrol.tipo_cta = item.tipo_cta.ToString();
                                                infoCuentasDelKontrol.descCuenta = item.descCuenta;
                                                infoCuentasDelKontrol.plastico = item.plastico;
                                                infoCuentasDelKontrol.ind_cuenta_activa = 1;
                                                infoCuentasDelKontrol.ind_cuenta_def = 1;
                                                infoCuentasDelKontrol.bit_valida_captura = 1;
                                                infoCuentasDelKontrol.id_usuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                                infoCuentasDelKontrol.fechaCreacion = DateTime.Now;
                                                infoCuentasDelKontrol.registroActivo = true;
                                                _context.tblCom_sp_proveedores_cta_dep.Add(infoCuentasDelKontrol);
                                                _context.SaveChanges();

                                            }
                                            else
                                            {
                                                consCuenta = infoProveedorCuentasDelKontrol.id_cta_dep;
                                            }
                                        }
                                        var proveedorCuentaExistenteEK = consultaCheckProductivo(string.Format(@"SELECT * FROM sp_proveedores_cta_dep WHERE numpro = '{0}' AND id_cta_dep = '{1}'", objProveedor.numpro, consCuenta)
                                                                  );
                                        if (proveedorCuentaExistenteEK == null)
                                        {
                                            foreach (var item in objProveedor.lstCuentasNuevas)
                                            {
                                                var insertCta = @"INSERT INTO
                                                    sp_proveedores_cta_dep 
                                                        (
                                                        numpro,id_cta_dep,banco,moneda,cuenta,sucursal,plaza,clabe,tipo_cta,plastico,ind_cuenta_def,ind_cuenta_activa,bit_valida_captura
                                                        )
                                                    VALUES
                                                        (
                                                         ?,?,?,?,?,?,?,?,?,?,?,?,?
                                                        )
                                                ";

                                                using (var cmd = new OdbcCommand(insertCta))
                                                {
                                                    cmd.Parameters.Add("@numpro", OdbcType.Numeric).Value = item.numpro;
                                                    cmd.Parameters.Add("@id_cta_dep", OdbcType.Int).Value = consCuenta;
                                                    cmd.Parameters.Add("@banco", OdbcType.Numeric).Value = item.banco;
                                                    cmd.Parameters.Add("@moneda", OdbcType.Char).Value = item.moneda;
                                                    cmd.Parameters.Add("@cuenta", OdbcType.Char).Value = item.cuenta;
                                                    cmd.Parameters.Add("@sucursal", OdbcType.Char).Value = item.sucursal;
                                                    cmd.Parameters.Add("@plaza", OdbcType.Numeric).Value = item.plaza;
                                                    cmd.Parameters.Add("@clabe", OdbcType.Char).Value = item.clabe;
                                                    cmd.Parameters.Add("@tipo_cta", OdbcType.Char).Value = item.tipo_cta;
                                                    cmd.Parameters.Add("@plastico", OdbcType.Char).Value = item.plastico;
                                                    cmd.Parameters.Add("@ind_cuenta_def", OdbcType.Numeric).Value = 1;
                                                    cmd.Parameters.Add("@ind_cuenta_activa", OdbcType.Numeric).Value = 1;
                                                    cmd.Parameters.Add("@bit_valida_captura", OdbcType.Numeric).Value = 1;
                                                    cmd.Connection = transactionEk.Connection;
                                                    cmd.Transaction = transactionEk;
                                                    cmd.ExecuteNonQuery();

                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (var item in objProveedor.lstCuentasNuevas)
                                        {
                                            tblCom_sp_proveedores_cta_dep infoCuentas = new tblCom_sp_proveedores_cta_dep();
                                            var infoProveedorCuentas = _context.tblCom_sp_proveedores_cta_dep.Where(e => e.registroActivo && e.FK_idProv == item.FK_idProv).ToList().OrderByDescending(x => x.id_cta_dep).FirstOrDefault();

                                            if (infoProveedorCuentas == null)
                                            {
                                                consCuenta++;
                                                infoCuentas.FK_idProv = (int)objProveedor.numpro;
                                                infoCuentas.numpro = item.numpro;
                                                infoCuentas.id_cta_dep = consCuenta;
                                                infoCuentas.banco = item.banco;
                                                infoCuentas.descBanco = item.descBanco;
                                                infoCuentas.moneda = item.moneda;
                                                infoCuentas.descMoneda = item.descMoneda;
                                                infoCuentas.cuenta = item.cuenta;
                                                infoCuentas.sucursal = item.sucursal;
                                                infoCuentas.clabe = item.clabe;
                                                infoCuentas.plaza = item.plaza;
                                                infoCuentas.tipo_cta = item.tipo_cta.ToString();
                                                infoCuentas.descCuenta = item.descCuenta;
                                                infoCuentas.plastico = item.plastico;
                                                infoCuentas.ind_cuenta_activa = 1;
                                                infoCuentas.ind_cuenta_def = 1;
                                                infoCuentas.bit_valida_captura = 1;
                                                infoCuentas.id_usuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                                infoCuentas.fechaCreacion = DateTime.Now;
                                                infoCuentas.registroActivo = true;
                                                _context.tblCom_sp_proveedores_cta_dep.Add(infoCuentas);
                                                _context.SaveChanges();

                                            }
                                            else
                                            {
                                                consCuenta = infoProveedorCuentas.id_cta_dep;
                                            }
                                        }
                                    }


                                    #endregion

                                    #region ver si hay archivos

                                    if (objProveedor.lstArchivosAeliminar != null)
                                    {

                                        foreach (var item in objProveedor.lstArchivosAeliminar)
                                        {
                                            var infoProveedorArchivo = _context.tblCom_ArchivosAdjuntosProveedores.Where(e => e.registroActivo && e.id == item).FirstOrDefault();
                                            if (infoProveedorArchivo != null)
                                            {
                                                infoProveedorArchivo.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                                infoProveedorArchivo.fechaModificacion = DateTime.Now;
                                                infoProveedorArchivo.registroActivo = false;

                                                _context.SaveChanges();
                                            }
                                        }

                                    }
                                    if (objFile != null)
                                    {
                                        #region SE REGISTRA EL ARCHIVO ADJUNTO

                                        var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
#if DEBUG
                                        var CarpetaNueva = Path.Combine(RutaLocal, objProveedor.numpro.ToString());
#else
                                var CarpetaNueva = Path.Combine(RutaBase, objProveedor.numpro.ToString());
#endif
                                        //// Verifica si existe la carpeta y si no, la crea.
                                        if (GlobalUtils.VerificarExisteCarpeta(CarpetaNueva, true) == false)
                                        {
                                            dbContextTransaction.Rollback();
                                            resultado.Add(SUCCESS, false);
                                            resultado.Add(MESSAGE, "No se pudo crear la carpeta en el servidor.");
                                            return resultado;
                                        }


                                        string nombreArchivo = SetNombreArchivo("EvidenciaProveedor ", objFile.FileName);
                                        string rutaArchivo = Path.Combine(CarpetaNueva, nombreArchivo);
                                        listaRutaArchivos.Add(Tuple.Create(objFile, rutaArchivo));

                                        // GUARDAR TABLA ARCHIVOS
                                        tblCom_ArchivosAdjuntosProveedores objEvidencia = new tblCom_ArchivosAdjuntosProveedores()
                                        {
                                            //FK_idProv = infoProveedor.id,
                                            FK_numpro = objProveedor.numpro,
                                            nombreArchivo = nombreArchivo,
                                            rutaArchivo = rutaArchivo,
                                            FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id,
                                            fechaCreacion = DateTime.Now,
                                            registroActivo = true
                                        };

                                        _context.tblCom_ArchivosAdjuntosProveedores.Add(objEvidencia);
                                        _context.SaveChanges();

                                        if (GlobalUtils.SaveHTTPPostedFile(objFile, rutaArchivo) == false)
                                        {
                                            dbContextTransaction.Rollback();
                                            resultado.Clear();
                                            resultado.Add(SUCCESS, false);
                                            resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                                            return resultado;
                                        }

                                        //resultado.Add(MESSAGE, "Se han cargado las evidencias con éxito, Favor de consultar en en apartado de Consultas.");
                                        //resultado.Add(SUCCESS, true);
                                        #endregion

                                    }
                                    #endregion
                                    #endregion
                                }
                                #endregion

                            }

                            _context.SaveChanges();
                            transactionEk.Commit();
                            dbContextTransaction.Commit();
                            resultado.Add(SUCCESS, true);
                        }
                        catch (Exception e)
                        {
                            transactionEk.Rollback();
                            dbContextTransaction.Rollback();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, e.Message);
                        }
                    }
                }
            }
            return resultado;
        }
        public Dictionary<string, object> GuardarEditarProveedorColombia(tblCom_sp_proveedoresColombiaDTO objProveedor, HttpPostedFileBase objFile)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {


                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        private OdbcConnection ConexionEnkontrol()
        {
            //return new Conexion().ConnectArrendaroraPrueba();
            //return new Conexion().ConnectPrueba();
            return new Conexion().Connect();
        }
        public Dictionary<string, object> AutorizarProveedor(int id)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                using (var connEk = ConexionEnkontrol())
                {
                    using (var transactionEk = connEk.BeginTransaction())
                    {
                        using (var ctxCP = new MainContext(EmpresaEnum.Construplan))
                        {
                            using(var dbTransacCP = ctxCP.Database.BeginTransaction())
                            {
                                using (var dbEKConstruplan = new Conexion().Connect(1))
                                {
                                    using (var dbTransacEKConstrplan = dbEKConstruplan.BeginTransaction())
                                    {

                                        try
                                        {
                                            var objAutorizador = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.idUsuario == vSesiones.sesionUsuarioDTO.id && e.PuedeAutorizar == true).ToList();
                                            var objVobo = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.idUsuario == vSesiones.sesionUsuarioDTO.id && e.PuedeVobo == true).ToList();
                                            var objProveedor = _context.tblCom_sp_proveedores.Where(e => e.registroActivo && e.id == id).FirstOrDefault();
                                            var infoProveedorCuentas = _context.tblCom_sp_proveedores_cta_dep.Where(e => e.registroActivo && e.FK_idProv == objProveedor.id).ToList();
                                            var usuariosPrimerVobo = _context.tblCom_AutorizarProveedor.Where(x => x.registroActivo && x.PrimerVobo).Select(x => x.idUsuario).ToList();
                                            var quitarAlertas = _context.tblP_Alerta.Where(x => x.tipoAlerta == 2 && x.sistemaID == 4 && !x.visto && x.objID == (int)objProveedor.numpro && (x.obj == "AutorizacionProveedor" || x.obj == "AltaProveedor") && x.msj.Contains("Proveedor") && x.userRecibeID == vSesiones.sesionUsuarioDTO.id).ToList();
                                            var infoProveedorSocio = _context.tblCom_ProveedoresSocios.Where(e => e.registroActivo && e.FK_numpro == objProveedor.numpro).FirstOrDefault();

                                            foreach (var item in quitarAlertas)
                                            {
                                                item.visto = true;
                                            }
                                            _context.SaveChanges();

                                            if (objProveedor.vobo == true)
                                            {
                                                if (objAutorizador.Count() > 0)
                                                {
                                                    objProveedor.Autorizado = true;
                                                    objProveedor.id_usuarioAutorizo = vSesiones.sesionUsuarioDTO.id;
                                                    objProveedor.fechaAutorizo = DateTime.Now;
                                                    objProveedor.statusAutorizacion = true;

                                                }
                                            }
                                            else
                                            {
                                                throw new Exception("El proveedor que esta intentando autorizar no tiene V.o.b.o.");
                                            }
                                            _context.SaveChanges();

                                            #region AGREGAR REGISTRO SP CONSTRUPLAN


                                            var objProvCP = ctxCP.tblCom_sp_proveedores.FirstOrDefault(e => e.registroActivo && e.numpro == objProveedor.numpro);

                                            if (objProvCP == null)
                                            {
                                                var objNewProv = new tblCom_sp_proveedores()
                                                {
                                                    #region DATOS

                                                    numpro = objProveedor.numpro,
                                                    nomcorto = objProveedor.nomcorto,
                                                    nombre = objProveedor.nombre,
                                                    direccion = objProveedor.direccion,
                                                    ciudad = objProveedor.ciudad,
                                                    cp = objProveedor.cp,
                                                    responsable = objProveedor.responsable,
                                                    telefono1 = objProveedor.telefono1,
                                                    telefono2 = objProveedor.telefono2,
                                                    fax = objProveedor.fax,
                                                    email = objProveedor.email,
                                                    rfc = objProveedor.rfc,
                                                    limcred = objProveedor.limcred,
                                                    tmbase = objProveedor.tmbase,
                                                    condpago = objProveedor.condpago,
                                                    moneda = objProveedor.moneda,
                                                    cta_bancaria = objProveedor.cta_bancaria,
                                                    tipo_prov = objProveedor.tipo_prov,
                                                    cancelado = objProveedor.cancelado,
                                                    tipo_pago = objProveedor.tipo_pago,
                                                    cta_cheque = objProveedor.cta_cheque,
                                                    cve_banco = objProveedor.cve_banco,
                                                    plaza_banco = objProveedor.plaza_banco,
                                                    tipo_cta = objProveedor.tipo_cta,
                                                    fecha_modifica_plazo_pago = objProveedor.fecha_modifica_plazo_pago,
                                                    usuario_modifica_plazo_pago = objProveedor.usuario_modifica_plazo_pago,
                                                    prov_exterior = objProveedor.prov_exterior,
                                                    filial = objProveedor.filial,
                                                    tipo_tercero = objProveedor.tipo_tercero,
                                                    tipo_operacion = objProveedor.tipo_operacion,
                                                    curp = objProveedor.curp,
                                                    id_fiscal = objProveedor.id_fiscal,
                                                    nacionalidad = objProveedor.nacionalidad,
                                                    persona_fisica = objProveedor.persona_fisica,
                                                    a_paterno = objProveedor.a_paterno,
                                                    a_materno = objProveedor.a_materno,
                                                    a_nombre = objProveedor.a_nombre,
                                                    tipo_pago_transferencia = objProveedor.tipo_pago_transferencia,
                                                    bit_estrategico = objProveedor.bit_estrategico,
                                                    swift_code = objProveedor.swift_code,
                                                    account_number = objProveedor.account_number,
                                                    routing = objProveedor.routing,
                                                    numpro_factoraje = objProveedor.numpro_factoraje,
                                                    num_empleado = objProveedor.num_empleado,
                                                    obliga_cfd = objProveedor.obliga_cfd,
                                                    bit_factoraje = objProveedor.bit_factoraje,
                                                    id_usuarioCreacion = objProveedor.id_usuarioCreacion,
                                                    fechaCreacion = objProveedor.fechaCreacion,
                                                    id_usuarioModificacion = objProveedor.id_usuarioModificacion,
                                                    fechaModificacion = objProveedor.fechaModificacion,
                                                    statusAutorizacion = objProveedor.statusAutorizacion,
                                                    primerVobo = objProveedor.primerVobo,
                                                    id_usuarioPrimerVobo = objProveedor.id_usuarioPrimerVobo,
                                                    fechaPrimerVobo = objProveedor.fechaPrimerVobo,
                                                    vobo = objProveedor.vobo,
                                                    id_usuarioVobo = objProveedor.id_usuarioVobo,
                                                    fechaVobo = objProveedor.fechaVobo,
                                                    Autorizado = objProveedor.Autorizado,
                                                    id_usuarioAutorizo = objProveedor.id_usuarioAutorizo,
                                                    fechaAutorizo = objProveedor.fechaAutorizo,
                                                    statusNotificacion = objProveedor.statusNotificacion,
                                                    registroActivo = objProveedor.registroActivo,
                                                    #endregion
                                                };

                                                ctxCP.tblCom_sp_proveedores.Add(objNewProv);
                                                ctxCP.SaveChanges();

                                                #region Guardado informacion de cuentas bancarias
                                                int consCuenta = 0;
                                                if (infoProveedorCuentas != null && infoProveedorCuentas.Count() > 0)
                                                {
                                                    foreach (var item in infoProveedorCuentas)
                                                    {
                                                        tblCom_sp_proveedores_cta_dep infoCuentas = new tblCom_sp_proveedores_cta_dep();

                                                        consCuenta++;
                                                        infoCuentas.FK_idProv = objNewProv.id;
                                                        infoCuentas.numpro = item.numpro;
                                                        infoCuentas.id_cta_dep = consCuenta;
                                                        infoCuentas.banco = item.banco;
                                                        infoCuentas.descBanco = item.descBanco;
                                                        infoCuentas.moneda = item.moneda;
                                                        infoCuentas.descMoneda = item.descMoneda;
                                                        infoCuentas.cuenta = item.cuenta;
                                                        infoCuentas.sucursal = item.sucursal;
                                                        infoCuentas.clabe = item.clabe;
                                                        infoCuentas.plaza = item.plaza;
                                                        infoCuentas.tipo_cta = item.tipo_cta.ToString();
                                                        infoCuentas.descCuenta = item.descCuenta;
                                                        infoCuentas.plastico = item.plastico;
                                                        infoCuentas.ind_cuenta_activa = 1;
                                                        infoCuentas.ind_cuenta_def = 1;
                                                        infoCuentas.bit_valida_captura = 1;
                                                        infoCuentas.id_usuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                                        infoCuentas.fechaCreacion = DateTime.Now;
                                                        infoCuentas.registroActivo = true;

                                                        ctxCP.tblCom_sp_proveedores_cta_dep.Add(infoCuentas);
                                                        ctxCP.SaveChanges();
                                                    }
                                                }
                                                #endregion


                                                var objEvidenciaProv = _context.tblCom_ArchivosAdjuntosProveedores.FirstOrDefault(e => e.registroActivo && e.FK_idProv == id);

                                                if (objEvidenciaProv != null)
                                                {
                                                    var objNewEvidenciaProv = new tblCom_ArchivosAdjuntosProveedores() 
                                                    {
                                                        FK_idProv = objNewProv.id,
                                                        FK_numpro = objEvidenciaProv.FK_numpro,
                                                        nombreArchivo = objEvidenciaProv.nombreArchivo,
                                                        rutaArchivo = objEvidenciaProv.rutaArchivo,
                                                        FK_UsuarioCreacion = objEvidenciaProv.FK_UsuarioCreacion,
                                                        FK_UsuarioModificacion = objEvidenciaProv.FK_UsuarioModificacion,
                                                        fechaCreacion = objEvidenciaProv.fechaCreacion,
                                                        fechaModificacion = objEvidenciaProv.fechaModificacion,
                                                        registroActivo = objEvidenciaProv.registroActivo,
                                                    };

                                                    ctxCP.tblCom_ArchivosAdjuntosProveedores.Add(objNewEvidenciaProv);
                                                    ctxCP.SaveChanges();
                                                }
                                            }

                                            
                                            #endregion

                                            #region SE ENVIA CORREO AL AUTORIZANTE
                                            List<string> lstCorreos = new List<string>();
                                            var usuarioQueAplicoVobo = _context.tblP_Usuario.FirstOrDefault(x => x.id == objProveedor.id_usuarioVobo);
                                            var usuarioQueAplicoAutorizacion = _context.tblP_Usuario.FirstOrDefault(x => x.id == objProveedor.id_usuarioAutorizo);
                                            var usuarioCreoProveedor = _context.tblP_Usuario.FirstOrDefault(x => x.id == objProveedor.id_usuarioCreacion);
                                            var correosPrimerVobo = _context.tblP_Usuario.Where(x => usuariosPrimerVobo.Contains(x.id)).Select(x => x.correo).ToList();

                                            if (usuarioQueAplicoVobo != null && !string.IsNullOrEmpty(usuarioQueAplicoVobo.correo))
                                            {
                                                lstCorreos.Add(usuarioQueAplicoVobo.correo);
                                            }
                                            if (usuarioQueAplicoAutorizacion != null && !string.IsNullOrEmpty(usuarioQueAplicoAutorizacion.correo))
                                            {
                                                lstCorreos.Add(usuarioQueAplicoAutorizacion.correo);
                                            }
                                            if (usuarioCreoProveedor != null && !string.IsNullOrEmpty(usuarioCreoProveedor.correo))
                                            {
                                                lstCorreos.Add(usuarioCreoProveedor.correo);
                                            }

                                            lstCorreos.AddRange(correosPrimerVobo);

                #if DEBUG
                                            lstCorreos = new List<string>();
                                            lstCorreos.Add("martin.zayas@construplan.com.mx");
                #endif

                                            string Persona = "";

                                            if (objProveedor.persona_fisica == "S")
                                            {
                                                Persona = "PERSONA FISICA<br/>";
                                                Persona += objProveedor.a_nombre + " " + objProveedor.a_paterno + "" + objProveedor.a_materno;

                                            }
                                            else
                                            {
                                                Persona = "NOMBRES EN EL ACTA";
                                            }

                                            string sociosTemp = "";

                                            if (objProveedor != null && infoProveedorSocio != null && !string.IsNullOrEmpty(infoProveedorSocio.socios))
                                            {
                                                foreach (var item in infoProveedorSocio.socios)
                                                {
                                                    switch (item)
                                                    {
                                                        case ',':
                                                            sociosTemp += "<br/> ";
                                                            break;
                                                        case '\n':
                                                            sociosTemp += "<br/> ";
                                                            break;
                                                        default:
                                                            sociosTemp += item;
                                                            break;
                                                    }
                                                }
                                            }

                                            string subject = "Alta de proveedor";
                                            //string body = string.Format("Buen día,<br> se informa " + "que el usuario " + objUsuarioAutorizoAlta.nombre + " " + objUsuarioAutorizoAlta.apellidoPaterno + " " + objUsuarioAutorizoAlta.apellidoMaterno + " confirmó el alta y el proveedor " + objProveedor.PRVCCODIGO + " " + objProveedor.PRVCNOMBRE + " se encuentra listo para ser autorizado <br>" +
                                            //                            "<br><br>Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>Construplan Peru > Compras > Proveedores.<br>" +
                                            //                            "Se informa que este es un correo autogenerado por el sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx</a>)." +
                                            //                            "No es necesario dar una respuesta. Gracias.");


                                            string body = string.Format(@"
                                                Buen dia ,<br>Se informa que el usuario {0} {1} {2} Autorizó el alta del siguiente proveedor {3} {4} en la empresa Construplan Arrendadora. <br>
                                                {5}
                                                <br>
                                                <H2>
                                                {6}</H2>
                                                <H2>
                                                {7}</H2>
                                                <br><br>Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>Construplan> Compras > Proveedores.<br>
                                                Se informa que este es un correo autogenerado por el sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx</a>).
                                                No es necesario dar una respuesta. Gracias.",
                                            usuarioQueAplicoAutorizacion.nombre,
                                            usuarioQueAplicoAutorizacion.apellidoPaterno,
                                            usuarioQueAplicoAutorizacion.apellidoMaterno,
                                            objProveedor.numpro,
                                            objProveedor.nombre,
                                            htmlCorreo(id),
                                            Persona,
                                            sociosTemp);
                           
                                            #endregion

                                            #region SE REGISTRA EN SYBASE EKONTROL MEXICO

                                            var insertProv = string.Format(@"
                                                                  INSERT INTO
                                                                    sp_proveedores
                                                                        (
                                                                                  numpro, nomcorto, nombre, direccion, ciudad, cp, responsable, telefono1, telefono2, fax,
                                                                        email, rfc, limcred, tmbase, descuento, condpago, moneda, cta_bancaria, tipo_prov, cancelado,
                                                                        tipo_pago, inst_factoraje, cta_cheque, cve_banco, plaza_banco, tipo_cta, fecha_modifica_plazo_pago, usuario_modifica_plazo_pago, prov_exterior, filial,
                                                                        tipo_tercero, tipo_operacion, curp, bit_factoraje, id_fiscal, nacionalidad, persona_fisica, a_paterno, a_materno, a_nombre,
                                                                        tipo_pago_transferencia, bit_estrategico, swift_code, account_number, routing, numpro_factoraje, num_empleado, obliga_cfd, clave_id, numregidtrib,
                                                                        fecha_mod, pp, ekuserweb, st_aut, st_vobo
                                                                        )
                                                                    VALUES
                                                                        (
                                                                             ?, ?, ?, ?, ?, ?, ?, ?, ?, ?,
                                                                             ?, ?, ?, ?, ?, ?, ?, ?, ?, ?,
                                                                             ?, ?, ?, ?, ?, ?, ?, ?, ?, ?,
                                                                             ?, ?, ?, ?, ?, ?, ?, ?, ?, ?,
                                                                             ?, ?, ?, ?, ?, ?, ?, ?, ?, ?,
                                                                             ?, ?, ?, ?, ?
                                                                        )
                                                                ");

                                            using (var cmd = new OdbcCommand(insertProv))
                                            {
                                                //numpro, nomcorto, nombre, direccion, ciudad, cp, responsable, telefono1, telefono2, fax,
                                                cmd.Parameters.Add("@numpro", OdbcType.Numeric).Value = objProveedor.numpro;
                                                cmd.Parameters.Add("@nomcorto", OdbcType.Char).Value = (objProveedor.nomcorto ?? "").Trim();
                                                cmd.Parameters.Add("@nombre", OdbcType.Char).Value = (objProveedor.nombre ?? "").Trim();
                                                cmd.Parameters.Add("@direccion", OdbcType.Char).Value = (objProveedor.direccion ?? "").Trim();
                                                cmd.Parameters.Add("@ciudad", OdbcType.Char).Value = (objProveedor.ciudad ?? "");
                                                cmd.Parameters.Add("@cp", OdbcType.Char).Value = (objProveedor.cp ?? "").Trim();
                                                cmd.Parameters.Add("@responsable", OdbcType.Char).Value = (objProveedor.responsable ?? "").Trim();
                                                cmd.Parameters.Add("@telefono1", OdbcType.Char).Value = (objProveedor.telefono1 ?? "").Trim();
                                                cmd.Parameters.Add("@telefono2", OdbcType.Char).Value = (objProveedor.telefono2 ?? "").Trim();
                                                cmd.Parameters.Add("@fax", OdbcType.Char).Value = (objProveedor.fax ?? "").Trim();

                                                //  email, rfc, limcred, tmbase, condpago, moneda, cta_bancaria, tipo_prov, cancelado,
                                                cmd.Parameters.Add("@email", OdbcType.VarChar).Value = (objProveedor.email ?? "").Trim();
                                                cmd.Parameters.Add("@rfc", OdbcType.Char).Value = (objProveedor.rfc ?? "").Trim();
                                                cmd.Parameters.Add("@limcred", OdbcType.Numeric).Value = objProveedor.limcred;
                                                cmd.Parameters.Add("@tmbase", OdbcType.Numeric).Value = objProveedor.tmbase;
                                                cmd.Parameters.Add("@descuento", OdbcType.Numeric).Value = 0; //falta agregar
                                                cmd.Parameters.Add("@condpago", OdbcType.Numeric).Value = objProveedor.condpago;
                                                cmd.Parameters.Add("@moneda", OdbcType.Char).Value = (objProveedor.moneda ?? "").Trim();
                                                cmd.Parameters.Add("@cta_bancaria", OdbcType.Char).Value = (objProveedor.cta_bancaria ?? "").Trim();
                                                cmd.Parameters.Add("@tipo_prov", OdbcType.Numeric).Value = objProveedor.tipo_prov;
                                                cmd.Parameters.Add("@cancelado", OdbcType.Char).Value = (objProveedor.cancelado ?? "").Trim();

                                                //tipo_pago, inst_factoraje, cta_cheque, cve_banco, plaza_banco, tipo_cta, fecha_modifica_plazo_pago, usuario_modifica_plazo_pago, prov_exterior, filial,
                                                cmd.Parameters.Add("@tipo_pago", OdbcType.Char).Value = "";
                                                cmd.Parameters.Add("@inst_factoraje", OdbcType.Numeric).Value = 0;
                                                cmd.Parameters.Add("@cta_cheque", OdbcType.Char).Value = (objProveedor.cta_cheque ?? "").Trim();
                                                cmd.Parameters.Add("@cve_banco", OdbcType.Char).Value = (objProveedor.cve_banco ?? "").Trim();
                                                cmd.Parameters.Add("@plaza_banco", OdbcType.Char).Value = (objProveedor.plaza_banco ?? "").Trim();
                                                cmd.Parameters.Add("@tipo_cta", OdbcType.Char).Value = (objProveedor.tipo_cta ?? "").Trim();
                                                cmd.Parameters.Add("@fecha_modifica_plazo_pago", OdbcType.DateTime).Value = objProveedor.fecha_modifica_plazo_pago ?? DateTime.Now;
                                                cmd.Parameters.Add("@usuario_modifica_plazo_pago", OdbcType.Numeric).Value = objProveedor.usuario_modifica_plazo_pago;
                                                cmd.Parameters.Add("@prov_exterior", OdbcType.Char).Value = "";
                                                cmd.Parameters.Add("@filial", OdbcType.Char).Value = (objProveedor.filial ?? "").Trim();

                                                //tipo_tercero, tipo_operacion, curp, bit_factoraje, id_fiscal, nacionalidad, persona_fisica, a_paterno, a_materno, a_nombre,
                                                cmd.Parameters.Add("@tipo_tercero", OdbcType.Numeric).Value = objProveedor.tipo_tercero;
                                                cmd.Parameters.Add("@tipo_operacion", OdbcType.Numeric).Value = objProveedor.tipo_operacion;
                                                cmd.Parameters.Add("@curp", OdbcType.Char).Value = (objProveedor.curp ?? "").Trim();
                                                cmd.Parameters.Add("@bit_factoraje", OdbcType.Char).Value = (objProveedor.bit_factoraje ?? "").Trim();
                                                cmd.Parameters.Add("@id_fiscal", OdbcType.Char).Value = (objProveedor.id_fiscal ?? "").Trim();
                                                cmd.Parameters.Add("@nacionalidad", OdbcType.Char).Value = (objProveedor.nacionalidad ?? "").Trim();
                                                cmd.Parameters.Add("@persona_fisica", OdbcType.Char).Value = (objProveedor.persona_fisica ?? "N").Trim();
                                                cmd.Parameters.Add("@a_paterno", OdbcType.VarChar).Value = (objProveedor.a_paterno ?? "").Trim();
                                                cmd.Parameters.Add("@a_materno", OdbcType.VarChar).Value = (objProveedor.a_materno ?? "").Trim();
                                                cmd.Parameters.Add("@a_nombre", OdbcType.VarChar).Value = (objProveedor.a_nombre ?? "").Trim();

                                                //tipo_pago_transferencia, bit_estrategico, swift_code, account_number, routing, numpro_factoraje, num_empleado, obliga_cfd, clave_id, numregidtrib,
                                                cmd.Parameters.Add("@tipo_pago_transferencia", OdbcType.Numeric).Value = objProveedor.tipo_pago_transferencia;
                                                cmd.Parameters.Add("@bit_estrategico", OdbcType.Numeric).Value = objProveedor.bit_estrategico;
                                                cmd.Parameters.Add("@swift_code", OdbcType.VarChar).Value = "";
                                                cmd.Parameters.Add("@account_number", OdbcType.VarChar).Value = "";
                                                cmd.Parameters.Add("@routing", OdbcType.VarChar).Value = "";
                                                cmd.Parameters.Add("@numpro_factoraje", OdbcType.VarChar).Value = "";
                                                cmd.Parameters.Add("@num_empleado", OdbcType.Numeric).Value = 0;
                                                cmd.Parameters.Add("@obliga_cfd", OdbcType.Numeric).Value = objProveedor.obliga_cfd;
                                                cmd.Parameters.Add("@clave_id", OdbcType.Char).Value = "";
                                                cmd.Parameters.Add("@numregidtrib", OdbcType.VarChar).Value = "";

                                                //fecha_mod, pp, ekuserweb, st_aut, st_vobo
                                                cmd.Parameters.Add("@fecha_mod", OdbcType.Date).Value = DateTime.Now.Date;
                                                cmd.Parameters.Add("@pp", OdbcType.VarChar).Value = "S";
                                                cmd.Parameters.Add("@ekuserweb", OdbcType.Int).Value = 0;
                                                cmd.Parameters.Add("@st_aut", OdbcType.Bit).Value = false;
                                                cmd.Parameters.Add("@st_vobo", OdbcType.Bit).Value = false;
                                                cmd.Connection = transactionEk.Connection;
                                                cmd.Transaction = transactionEk;
                                                cmd.ExecuteNonQuery();
                                            }

                                            foreach (var item in infoProveedorCuentas)
                                            {
                                                var insertCta = @"INSERT INTO
                                                                    sp_proveedores_cta_dep 
                                                                        (
                                                                        numpro,id_cta_dep,banco,moneda,cuenta,sucursal,plaza,clabe,tipo_cta,plastico,ind_cuenta_def,ind_cuenta_activa,bit_valida_captura
                                                                        )
                                                                    VALUES
                                                                        (
                                                                         ?,?,?,?,?,?,?,?,?,?,?,?,?
                                                                        )
                                                                ";

                                                using (var cmd = new OdbcCommand(insertCta))
                                                {
                                                    cmd.Parameters.Add("@numpro", OdbcType.Numeric).Value = item.numpro;
                                                    cmd.Parameters.Add("@id_cta_dep", OdbcType.Int).Value = item.id_cta_dep;
                                                    cmd.Parameters.Add("@banco", OdbcType.Numeric).Value = item.banco;
                                                    cmd.Parameters.Add("@moneda", OdbcType.Char).Value = item.moneda;
                                                    cmd.Parameters.Add("@cuenta", OdbcType.Char).Value = item.cuenta;
                                                    cmd.Parameters.Add("@sucursal", OdbcType.Char).Value = item.sucursal;
                                                    cmd.Parameters.Add("@plaza", OdbcType.Numeric).Value = item.plaza;
                                                    cmd.Parameters.Add("@clabe", OdbcType.Char).Value = item.clabe;
                                                    cmd.Parameters.Add("@tipo_cta", OdbcType.Char).Value = item.tipo_cta;
                                                    cmd.Parameters.Add("@plastico", OdbcType.Char).Value = item.plastico;
                                                    cmd.Parameters.Add("@ind_cuenta_def", OdbcType.Numeric).Value = item.ind_cuenta_def;
                                                    cmd.Parameters.Add("@ind_cuenta_activa", OdbcType.Numeric).Value = item.ind_cuenta_activa;
                                                    cmd.Parameters.Add("@bit_valida_captura", OdbcType.Numeric).Value = item.bit_valida_captura;
                                                    cmd.Connection = transactionEk.Connection;
                                                    cmd.Transaction = transactionEk;
                                                    cmd.ExecuteNonQuery();

                                                }
                                            }

                                            #endregion

                                            #region CONSTRUPLAN EK

                                            var insertProvArr = string.Format(@"
                                                            INSERT INTO
                                                            sp_proveedores
                                                                (
                                                                            numpro, nomcorto, nombre, direccion, ciudad, cp, responsable, telefono1, telefono2, fax,
                                                                email, rfc, limcred, tmbase, descuento, condpago, moneda, cta_bancaria, tipo_prov, cancelado,
                                                                tipo_pago, inst_factoraje, cta_cheque, cve_banco, plaza_banco, tipo_cta, fecha_modifica_plazo_pago, usuario_modifica_plazo_pago, prov_exterior, filial,
                                                                tipo_tercero, tipo_operacion, curp, bit_factoraje, id_fiscal, nacionalidad, persona_fisica, a_paterno, a_materno, a_nombre,
                                                                tipo_pago_transferencia, bit_estrategico, swift_code, account_number, routing, numpro_factoraje, num_empleado, obliga_cfd, clave_id, numregidtrib,
                                                                fecha_mod, pp, ekuserweb, st_aut, st_vobo
                                                                )
                                                            VALUES
                                                                (
                                                                        ?, ?, ?, ?, ?, ?, ?, ?, ?, ?,
                                                                        ?, ?, ?, ?, ?, ?, ?, ?, ?, ?,
                                                                        ?, ?, ?, ?, ?, ?, ?, ?, ?, ?,
                                                                        ?, ?, ?, ?, ?, ?, ?, ?, ?, ?,
                                                                        ?, ?, ?, ?, ?, ?, ?, ?, ?, ?,
                                                                        ?, ?, ?, ?, ?
                                                                )
                                                        ");

                                            using (var cmd = new OdbcCommand(insertProvArr))
                                            {
                                                //numpro, nomcorto, nombre, direccion, ciudad, cp, responsable, telefono1, telefono2, fax,
                                                cmd.Parameters.Add("@numpro", OdbcType.Numeric).Value = objProveedor.numpro;
                                                cmd.Parameters.Add("@nomcorto", OdbcType.Char).Value = (objProveedor.nomcorto ?? "").Trim();
                                                cmd.Parameters.Add("@nombre", OdbcType.Char).Value = (objProveedor.nombre ?? "").Trim();
                                                cmd.Parameters.Add("@direccion", OdbcType.Char).Value = (objProveedor.direccion ?? "").Trim();
                                                cmd.Parameters.Add("@ciudad", OdbcType.Char).Value = (objProveedor.ciudad ?? "");
                                                cmd.Parameters.Add("@cp", OdbcType.Char).Value = (objProveedor.cp ?? "").Trim();
                                                cmd.Parameters.Add("@responsable", OdbcType.Char).Value = (objProveedor.responsable ?? "").Trim();
                                                cmd.Parameters.Add("@telefono1", OdbcType.Char).Value = (objProveedor.telefono1 ?? "").Trim();
                                                cmd.Parameters.Add("@telefono2", OdbcType.Char).Value = (objProveedor.telefono2 ?? "").Trim();
                                                cmd.Parameters.Add("@fax", OdbcType.Char).Value = (objProveedor.fax ?? "").Trim();

                                                //  email, rfc, limcred, tmbase, condpago, moneda, cta_bancaria, tipo_prov, cancelado,
                                                cmd.Parameters.Add("@email", OdbcType.VarChar).Value = (objProveedor.email ?? "").Trim();
                                                cmd.Parameters.Add("@rfc", OdbcType.Char).Value = (objProveedor.rfc ?? "").Trim();
                                                cmd.Parameters.Add("@limcred", OdbcType.Numeric).Value = objProveedor.limcred;
                                                cmd.Parameters.Add("@tmbase", OdbcType.Numeric).Value = objProveedor.tmbase;
                                                cmd.Parameters.Add("@descuento", OdbcType.Numeric).Value = 0; //falta agregar
                                                cmd.Parameters.Add("@condpago", OdbcType.Numeric).Value = objProveedor.condpago;
                                                cmd.Parameters.Add("@moneda", OdbcType.Char).Value = (objProveedor.moneda ?? "").Trim();
                                                cmd.Parameters.Add("@cta_bancaria", OdbcType.Char).Value = (objProveedor.cta_bancaria ?? "").Trim();
                                                cmd.Parameters.Add("@tipo_prov", OdbcType.Numeric).Value = objProveedor.tipo_prov;
                                                cmd.Parameters.Add("@cancelado", OdbcType.Char).Value = (objProveedor.cancelado ?? "").Trim();

                                                //tipo_pago, inst_factoraje, cta_cheque, cve_banco, plaza_banco, tipo_cta, fecha_modifica_plazo_pago, usuario_modifica_plazo_pago, prov_exterior, filial,
                                                cmd.Parameters.Add("@tipo_pago", OdbcType.Char).Value = "";
                                                cmd.Parameters.Add("@inst_factoraje", OdbcType.Numeric).Value = 0;
                                                cmd.Parameters.Add("@cta_cheque", OdbcType.Char).Value = (objProveedor.cta_cheque ?? "").Trim();
                                                cmd.Parameters.Add("@cve_banco", OdbcType.Char).Value = (objProveedor.cve_banco ?? "").Trim();
                                                cmd.Parameters.Add("@plaza_banco", OdbcType.Char).Value = (objProveedor.plaza_banco ?? "").Trim();
                                                cmd.Parameters.Add("@tipo_cta", OdbcType.Char).Value = (objProveedor.tipo_cta ?? "").Trim();
                                                cmd.Parameters.Add("@fecha_modifica_plazo_pago", OdbcType.DateTime).Value = objProveedor.fecha_modifica_plazo_pago ?? DateTime.Now;
                                                cmd.Parameters.Add("@usuario_modifica_plazo_pago", OdbcType.Numeric).Value = objProveedor.usuario_modifica_plazo_pago;
                                                cmd.Parameters.Add("@prov_exterior", OdbcType.Char).Value = "";
                                                cmd.Parameters.Add("@filial", OdbcType.Char).Value = (objProveedor.filial ?? "").Trim();

                                                //tipo_tercero, tipo_operacion, curp, bit_factoraje, id_fiscal, nacionalidad, persona_fisica, a_paterno, a_materno, a_nombre,
                                                cmd.Parameters.Add("@tipo_tercero", OdbcType.Numeric).Value = objProveedor.tipo_tercero;
                                                cmd.Parameters.Add("@tipo_operacion", OdbcType.Numeric).Value = objProveedor.tipo_operacion;
                                                cmd.Parameters.Add("@curp", OdbcType.Char).Value = (objProveedor.curp ?? "").Trim();
                                                cmd.Parameters.Add("@bit_factoraje", OdbcType.Char).Value = (objProveedor.bit_factoraje ?? "").Trim();
                                                cmd.Parameters.Add("@id_fiscal", OdbcType.Char).Value = (objProveedor.id_fiscal ?? "").Trim();
                                                cmd.Parameters.Add("@nacionalidad", OdbcType.Char).Value = (objProveedor.nacionalidad ?? "").Trim();
                                                cmd.Parameters.Add("@persona_fisica", OdbcType.Char).Value = (objProveedor.persona_fisica ?? "N").Trim();
                                                cmd.Parameters.Add("@a_paterno", OdbcType.VarChar).Value = (objProveedor.a_paterno ?? "").Trim();
                                                cmd.Parameters.Add("@a_materno", OdbcType.VarChar).Value = (objProveedor.a_materno ?? "").Trim();
                                                cmd.Parameters.Add("@a_nombre", OdbcType.VarChar).Value = (objProveedor.a_nombre ?? "").Trim();

                                                //tipo_pago_transferencia, bit_estrategico, swift_code, account_number, routing, numpro_factoraje, num_empleado, obliga_cfd, clave_id, numregidtrib,
                                                cmd.Parameters.Add("@tipo_pago_transferencia", OdbcType.Numeric).Value = objProveedor.tipo_pago_transferencia;
                                                cmd.Parameters.Add("@bit_estrategico", OdbcType.Numeric).Value = objProveedor.bit_estrategico;
                                                cmd.Parameters.Add("@swift_code", OdbcType.VarChar).Value = "";
                                                cmd.Parameters.Add("@account_number", OdbcType.VarChar).Value = "";
                                                cmd.Parameters.Add("@routing", OdbcType.VarChar).Value = "";
                                                cmd.Parameters.Add("@numpro_factoraje", OdbcType.VarChar).Value = "";
                                                cmd.Parameters.Add("@num_empleado", OdbcType.Numeric).Value = 0;
                                                cmd.Parameters.Add("@obliga_cfd", OdbcType.Numeric).Value = objProveedor.obliga_cfd;
                                                cmd.Parameters.Add("@clave_id", OdbcType.Char).Value = "";
                                                cmd.Parameters.Add("@numregidtrib", OdbcType.VarChar).Value = "";

                                                //fecha_mod, pp, ekuserweb, st_aut, st_vobo
                                                cmd.Parameters.Add("@fecha_mod", OdbcType.Date).Value = DateTime.Now.Date;
                                                cmd.Parameters.Add("@pp", OdbcType.VarChar).Value = "S";
                                                cmd.Parameters.Add("@ekuserweb", OdbcType.Int).Value = 0;
                                                cmd.Parameters.Add("@st_aut", OdbcType.Bit).Value = false;
                                                cmd.Parameters.Add("@st_vobo", OdbcType.Bit).Value = false;
                                                cmd.Connection = dbTransacEKConstrplan.Connection;
                                                cmd.Transaction = dbTransacEKConstrplan;
                                                cmd.ExecuteNonQuery();
                                            }

                                            foreach (var item in infoProveedorCuentas)
                                            {
                                                var insertCta = @"INSERT INTO
                                                                    sp_proveedores_cta_dep 
                                                                        (
                                                                        numpro,id_cta_dep,banco,moneda,cuenta,sucursal,plaza,clabe,tipo_cta,plastico,ind_cuenta_def,ind_cuenta_activa,bit_valida_captura
                                                                        )
                                                                    VALUES
                                                                        (
                                                                            ?,?,?,?,?,?,?,?,?,?,?,?,?
                                                                        )
                                                                ";

                                                using (var cmd = new OdbcCommand(insertCta))
                                                {
                                                    cmd.Parameters.Add("@numpro", OdbcType.Numeric).Value = item.numpro;
                                                    cmd.Parameters.Add("@id_cta_dep", OdbcType.Int).Value = item.id_cta_dep;
                                                    cmd.Parameters.Add("@banco", OdbcType.Numeric).Value = item.banco;
                                                    cmd.Parameters.Add("@moneda", OdbcType.Char).Value = item.moneda;
                                                    cmd.Parameters.Add("@cuenta", OdbcType.Char).Value = item.cuenta;
                                                    cmd.Parameters.Add("@sucursal", OdbcType.Char).Value = item.sucursal;
                                                    cmd.Parameters.Add("@plaza", OdbcType.Numeric).Value = item.plaza;
                                                    cmd.Parameters.Add("@clabe", OdbcType.Char).Value = item.clabe;
                                                    cmd.Parameters.Add("@tipo_cta", OdbcType.Char).Value = item.tipo_cta;
                                                    cmd.Parameters.Add("@plastico", OdbcType.Char).Value = item.plastico;
                                                    cmd.Parameters.Add("@ind_cuenta_def", OdbcType.Numeric).Value = item.ind_cuenta_def;
                                                    cmd.Parameters.Add("@ind_cuenta_activa", OdbcType.Numeric).Value = item.ind_cuenta_activa;
                                                    cmd.Parameters.Add("@bit_valida_captura", OdbcType.Numeric).Value = item.bit_valida_captura;
                                                    cmd.Connection = dbTransacEKConstrplan.Connection;
                                                    cmd.Transaction = dbTransacEKConstrplan;
                                                    cmd.ExecuteNonQuery();

                                                }
                                            }

                                            #endregion

                                            GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, lstCorreos.Distinct().ToList());

                                            transactionEk.Commit();
                                            dbContextTransaction.Commit();
                                            dbTransacCP.Commit();
                                            dbTransacEKConstrplan.Commit();

                                            resultado.Add(SUCCESS, true);
                                        }
                                        catch (Exception e)
                                        {

                                            transactionEk.Rollback();
                                            dbContextTransaction.Rollback();
                                            dbTransacCP.Rollback();
                                            dbTransacEKConstrplan.Rollback();

                                            resultado.Add(SUCCESS, false);
                                            resultado.Add(MESSAGE, e.Message);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }        
            }
            return resultado;
        }
        public Dictionary<string, object> NotificarAltaProveedor(int id)
        {
            using (var dbContextTransactionNotificacion = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (id <= 0) { throw new Exception("Ocurrió un error al notificar el alta de proveedor."); }
                    #endregion


                    if (_context.tblCom_sp_proveedores.Any(x => x.id == id && x.registroActivo && !x.primerVobo))
                    {
                        PrimerVobo(id);
                    }
                    else
                    {
                        #region SE INDICA QUE EL ALTA DE PROVEEDOR SE ENCUENTRA EN ESTATUS NOTIFICADO
                        var objProveedor = _context.tblCom_sp_proveedores.Where(e => e.registroActivo && e.id == id && !e.vobo).FirstOrDefault();
                        //var objProveedor = _context.tblCom_MAEPROV.Where(e => e.registroActivo && e.id == id).FirstOrDefault(); // para pruebas
                        if (objProveedor == null)
                            throw new Exception("Ocurrió un error al notificar.");

                        #endregion

                        #region SE OBTIENE AL AUTORIZADOR

                        var objAutorizador = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.PuedeAutorizar == true).ToList();
                        var objVobo = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.idUsuario == vSesiones.sesionUsuarioDTO.id && e.PuedeVobo == true).FirstOrDefault();
                        var infoProveedorSocio = _context.tblCom_ProveedoresSocios.Where(e => e.registroActivo && e.FK_numpro == objProveedor.numpro).FirstOrDefault();

                        //var objVobo = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.PuedeVobo == true).ToList(); // para pruebas
                        // SE OBTIENE EL CORREO DEL CREADOR DEL CREADOR DEL ALTA
                        if (objVobo != null)
                        {
                            var alertasRelacionadasAlVoBo = _context.tblP_Alerta.Where(x => x.tipoAlerta == 2 && x.sistemaID == 4 && x.objID == (int)objProveedor.numpro && (x.obj == "AutorizacionProveedor" || x.obj == "AltaProveedor") && x.msj.Contains("Proveedor") && !x.visto).ToList();
                            foreach (var item in alertasRelacionadasAlVoBo)
                            {
                                item.visto = true;
                            }
                            _context.SaveChanges();
                            objProveedor.cancelado = "A";
                            objProveedor.vobo = true;
                            objProveedor.id_usuarioVobo = vSesiones.sesionUsuarioDTO.id;
                            objProveedor.fechaVobo = DateTime.Now;

                            _context.SaveChanges();

                            foreach (var item in objAutorizador)
                            {
                                tblP_Usuario objUsuarioAutorizoAlta = _context.tblP_Usuario.Where(w => w.id == item.idUsuario).FirstOrDefault();
                                tblP_Usuario objUsuarioVobo = _context.tblP_Usuario.Where(w => w.id == objVobo.idUsuario).FirstOrDefault();
                                if (objUsuarioAutorizoAlta == null)
                                    throw new Exception("No se encuentra el correo del creador del alta.");

                                if (string.IsNullOrEmpty(objUsuarioAutorizoAlta.correo))
                                    throw new Exception("No se encuentra el correo al autorizante.");

                                #region SE ENVIA CORREO AL AUTORIZANTE
                                List<string> lstCorreos = new List<string>();
                                lstCorreos.Add(objUsuarioAutorizoAlta.correo);

                                var usuarioRegistroProveedor = _context.tblP_Usuario.FirstOrDefault(x => x.id == objProveedor.id_usuarioCreacion);
                                if (usuarioRegistroProveedor != null)
                                {
                                    lstCorreos.Add(usuarioRegistroProveedor.correo);
                                }

                                var puedeDarPrimerVobo = _context.tblCom_AutorizarProveedor.Where(x => x.registroActivo && x.PrimerVobo).Select(x => x.idUsuario).ToList();
                                var correosPrimerVoBo = _context.tblP_Usuario.Where(x => puedeDarPrimerVobo.Contains(x.id) && x.estatus).Select(x => x.correo).ToList();
                                lstCorreos.AddRange(correosPrimerVoBo);
#if DEBUG
                                lstCorreos = new List<string>();
                                lstCorreos.Add("martin.zayas@construplan.com.mx");
#endif
                                string Persona = "";

                                if (objProveedor.persona_fisica == "S")
                                {
                                    Persona = "PERSONA FISICA<br/>";
                                    Persona += objProveedor.a_nombre + " " + objProveedor.a_paterno + "" + objProveedor.a_materno;

                                }
                                else
                                {
                                    Persona = "NOMBRES EN EL ACTA";
                                }

                                string sociosTemp = "";

                                if (objProveedor != null && infoProveedorSocio != null && !string.IsNullOrEmpty(infoProveedorSocio.socios))
                                {
                                    foreach (var itemS in infoProveedorSocio.socios)
                                    {
                                        switch (itemS)
                                        {
                                            case ',':
                                                sociosTemp += "<br/> ";
                                                break;
                                            case '\n':
                                                sociosTemp += "<br/> ";
                                                break;
                                            default:
                                                sociosTemp += itemS;
                                                break;
                                        }
                                    }
                                }
                                string subject = "Alta de proveedor";
                                //string body = string.Format("Buen día,<br> se informa " + "que el usuario " + objUsuarioAutorizoAlta.nombre + " " + objUsuarioAutorizoAlta.apellidoPaterno + " " + objUsuarioAutorizoAlta.apellidoMaterno + " confirmó el alta y el proveedor " + objProveedor.PRVCCODIGO + " " + objProveedor.PRVCNOMBRE + " se encuentra listo para ser autorizado <br>" +
                                //                            "<br><br>Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>Construplan Peru > Compras > Proveedores.<br>" +
                                //                            "Se informa que este es un correo autogenerado por el sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx</a>)." +
                                //                            "No es necesario dar una respuesta. Gracias.");


                                string body = string.Format(@"
                                    Buen dia ,<br>Se informa que el usuario {0} {1} {2} confirmó el alta en la empresa Construplan Arrendadora del siguiente proveedor {3} {4} y se encuentra listo para ser autorizado <br>
                                    {5}
                                    <br>
                                    <H2>
                                    {6}</H2>
                                    <H2>
                                    {7}</H2>
                                    <br><br>Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>Construplan> Compras > Proveedores.<br>
                                    Se informa que este es un correo autogenerado por el sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx</a>).
                                    No es necesario dar una respuesta. Gracias.",
                                objUsuarioVobo.nombre,
                                objUsuarioVobo.apellidoPaterno,
                                objUsuarioVobo.apellidoMaterno,
                                objProveedor.numpro,
                                objProveedor.nombre,
                                htmlCorreo(id),
                                Persona,
                                sociosTemp);

                                var lstArchives = new List<adjuntoCorreoDTO>();
                                List<tblCom_ArchivosAdjuntosProveedores> lstArchivos = _context.tblCom_ArchivosAdjuntosProveedores.Where(w => w.FK_idProv == objProveedor.id && w.registroActivo).ToList();

                                if (lstArchivos != null)
                                {
                                    foreach (var itemArchivo in lstArchivos)
                                    {
                                        var fileStream = GlobalUtils.GetFileAsStream(itemArchivo.rutaArchivo);
                                        using (var streamReader = new MemoryStream())
                                        {
                                            fileStream.CopyTo(streamReader);
                                            //downloadFiles.Add(streamReader.ToArray());

                                            lstArchives.Add(new adjuntoCorreoDTO
                                            {
                                                archivo = streamReader.ToArray(),
                                                nombreArchivo = Path.GetFileNameWithoutExtension(itemArchivo.rutaArchivo),
                                                extArchivo = Path.GetExtension(itemArchivo.rutaArchivo)
                                            });
                                        }
                                    }

                                    GlobalUtils.sendMailWithFilesReclutamientos(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, lstCorreos.Distinct().ToList(), lstArchives);
                                }
                                else
                                {
                                    GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, lstCorreos.Distinct().ToList());
                                }
                                #endregion

                                #region Alerta SIGOPLAN
                                tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                                objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                                objNuevaAlerta.userRecibeID = item.idUsuario;
#if DEBUG
                                //objNuevaAlerta.userRecibeID = 79626; //USUARIO ID:Aaron.
                                objNuevaAlerta.userRecibeID = 13; //USUARIO ID:Administrador.
#endif
                                objNuevaAlerta.tipoAlerta = 2;
                                objNuevaAlerta.sistemaID = 4;
                                objNuevaAlerta.visto = false;
                                objNuevaAlerta.url = "/Enkontrol/AltaProveedor/AltaProveedor?&numpro=" + objProveedor.numpro;
                                objNuevaAlerta.objID = (int)objProveedor.numpro != null ? (int)objProveedor.numpro : 0;
                                objNuevaAlerta.obj = "AutorizacionProveedor";
                                objNuevaAlerta.msj = "Autorización Proveedor - " + objProveedor.numpro;
                                objNuevaAlerta.documentoID = 0;
                                objNuevaAlerta.moduloID = 0;
                                _context.tblP_Alerta.Add(objNuevaAlerta);
                                _context.SaveChanges();
                                #endregion //ALERTA SIGPLAN

                        #endregion
                                objProveedor.statusNotificacion = true;
                            }
                        }
                        else
                        {
                            throw new Exception("No tiene permiso para aplicar VoBo");
                        }
                    }

                    _context.SaveChanges();
                    dbContextTransactionNotificacion.Commit();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha notificado con éxito");
                }
                catch (Exception e)
                {
                    dbContextTransactionNotificacion.Rollback();
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }
        public string htmlCorreo(int id)
        {
            List<InfoNotificacionAutorizadorProvDTO> lstInfoAutorizadores = new List<InfoNotificacionAutorizadorProvDTO>();

            string html = "";
            contextSigoplan db = new contextSigoplan();
            //html += "<style>h3 {text-align: center;}table.dataTable tbody tr td, table thead tr th, table.dataTable, .dataTables_scrollBody {";
            //html += "border: 1px solid}table.dataTable thead {font-size: 18px;background-color:  white;color: black;}";
            //html += "</style>";
            var lstProveedores = _context.tblCom_sp_proveedores.Where(e => e.registroActivo && e.id == id).ToList();



            html += "<style>";
            html += "table, th, td {";
            html += "border: 1px solid grey;";
            html += "border-collapse: collapse;";
            html += "}";
            html += "th {";
            html += "text-align: center;";
            html += "}";
            html += "td {";
            html += "text-align: center;";
            html += "}";
            html += "</style>";


            html += "<br><table style='width:100%'>";
            html += "<thead>";
            html += "<tr>";

            html += "<th>Nombre Autorizador</th>";
            html += "<th>Tipo</th>";
            html += "<th>Autorizó</th>";

            html += "</tr>";
            html += "</thead>";
            html += "<tbody>";
            var objAutorizadorPuede = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.PuedeAutorizar).ToList();
            var objVoboPuede = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.PuedeVobo).ToList();
            var objPrimerVoboPuede = _context.tblCom_AutorizarProveedor.Where(x => x.registroActivo && x.PrimerVobo).ToList();

            foreach (var item in lstProveedores)
            {

                var objAutorizador = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.idUsuario == item.id_usuarioAutorizo).FirstOrDefault();
                var objVobo = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.idUsuario == item.id_usuarioVobo).FirstOrDefault();
                var objPrimerVobo = _context.tblCom_AutorizarProveedor.Where(x => x.registroActivo && x.idUsuario == item.id_usuarioPrimerVobo).ToList();
                //if (objAutorizador != null)

                if (item.primerVobo)
                {
                    foreach (var pVobo in objPrimerVobo)
                    {
                        InfoNotificacionAutorizadorProvDTO InfoAutorizadores = new InfoNotificacionAutorizadorProvDTO();
                        InfoAutorizadores.idUsuario = item.id_usuarioPrimerVobo;
                        InfoAutorizadores.autorizo = item.id_usuarioPrimerVobo == pVobo.idUsuario ? "AUTORIZADO" : "N/A";
                        InfoAutorizadores.tipo = "PRIMER VOBO";
                        InfoAutorizadores.color = "#82e0aa";
                        lstInfoAutorizadores.Add(InfoAutorizadores);
                    }
                }
                else
                {
                    foreach (var pVobo in objPrimerVoboPuede)
                    {
                        InfoNotificacionAutorizadorProvDTO InfoAutorizadores = new InfoNotificacionAutorizadorProvDTO();
                        InfoAutorizadores.idUsuario = pVobo.idUsuario;
                        InfoAutorizadores.autorizo = "PENDIENTE";
                        InfoAutorizadores.tipo = "PRIMER VOBO";
                        InfoAutorizadores.color = "#f08024";
                        lstInfoAutorizadores.Add(InfoAutorizadores);
                    }
                }

                if (item.vobo)
                {
                    InfoNotificacionAutorizadorProvDTO InfoAutorizadores = new InfoNotificacionAutorizadorProvDTO();

                    InfoAutorizadores.idUsuario = objVobo.idUsuario;
                    InfoAutorizadores.autorizo = "AUTORIZADO";
                    InfoAutorizadores.tipo = "VOBO";
                    InfoAutorizadores.color = "#82e0aa";

                    lstInfoAutorizadores.Add(InfoAutorizadores);
                }
                else
                {
                    foreach (var item4 in objVoboPuede)
                    {
                        InfoNotificacionAutorizadorProvDTO InfoAutorizadores = new InfoNotificacionAutorizadorProvDTO();

                        InfoAutorizadores.idUsuario = item4.idUsuario;
                        InfoAutorizadores.autorizo = "PENDIENTE";
                        InfoAutorizadores.tipo = "VOBO";
                        InfoAutorizadores.color = "#f08024";
                        lstInfoAutorizadores.Add(InfoAutorizadores);
                    }
                }
                if (item.Autorizado)
                {
                    InfoNotificacionAutorizadorProvDTO InfoAutorizadores = new InfoNotificacionAutorizadorProvDTO();

                    InfoAutorizadores.idUsuario = objAutorizador.idUsuario;
                    InfoAutorizadores.autorizo = "AUTORIZADO";
                    InfoAutorizadores.tipo = "AUTORIZANTE";
                    InfoAutorizadores.color = "#82e0aa";

                    lstInfoAutorizadores.Add(InfoAutorizadores);
                }
                else
                {
                    foreach (var item3 in objAutorizadorPuede)
                    {
                        InfoNotificacionAutorizadorProvDTO InfoAutorizadores = new InfoNotificacionAutorizadorProvDTO();
                        InfoAutorizadores.idUsuario = item3.idUsuario;
                        InfoAutorizadores.autorizo = "PENDIENTE";
                        InfoAutorizadores.tipo = "AUTORIZADOR";
                        InfoAutorizadores.color = "#f08024";
                        lstInfoAutorizadores.Add(InfoAutorizadores);
                    }
                }   
            }



            foreach (var item in lstInfoAutorizadores)
            {

                int autorizante = item.idUsuario;

                html += "<tr>";
                html += "<td>" + _context.tblP_Usuario.Where(r => r.id == autorizante).Select(y => y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno).FirstOrDefault() + "</td>";
                html += "<td>" + item.tipo + "</td>";
                html += "<td style='background-color:" + item.color + ";'>" + item.autorizo + "</td>";
                html += "</tr>";

            }

            html += "</tbody>";
            html += "</table>";

            return html;
        }
        public Dictionary<string, object> eliminarProveedor(int id)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var objProveedor = _context.tblCom_sp_proveedores.FirstOrDefault(x => x.id == id);

                    var quitarAlertas = _context.tblP_Alerta.Where(x => x.tipoAlerta == 2 && x.sistemaID == 4 && !x.visto && x.objID == (int)objProveedor.numpro && (x.obj == "AutorizacionProveedor" || x.obj == "AltaProveedor") && x.msj.Contains("Proveedor")).ToList();
                    foreach (var item in quitarAlertas)
                    {
                        item.visto = true;
                    }
                    _context.SaveChanges();

                    objProveedor.registroActivo = false;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }
        public Dictionary<string, object> GetArchivosAdjuntos(int idArchivo)
        {
            try
            {
                var lstArchivos = _context.tblCom_ArchivosAdjuntosProveedores.Where(x => x.registroActivo && x.id == idArchivo).ToList();

                resultado.Add(ITEMS, lstArchivos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }
        public Dictionary<string, object> VisualizarArchivoAdjunto(int idArchivo)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                tblCom_ArchivosAdjuntosProveedores objArchivoAdjunto = _context.tblCom_ArchivosAdjuntosProveedores.Where(w => w.FK_idProv == idArchivo && w.registroActivo).FirstOrDefault();
                if (objArchivoAdjunto == null)
                    throw new Exception("Ocurrió un error al visualizar el archivo.");

                resultado.Add("ruta", objArchivoAdjunto.rutaArchivo);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "VisualizarArchivoAdjunto", e, AccionEnum.CONSULTA, idArchivo, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public Tuple<Stream, string> descargarArchivo(int idArchivo)
        {
            try
            {
                var objArchivoAdjunto = _context.tblCom_ArchivosAdjuntosProveedores.Where(w => w.id == idArchivo && w.registroActivo).FirstOrDefault();

                var fileStream = GlobalUtils.GetFileAsStream(objArchivoAdjunto.rutaArchivo);
                string name = Path.GetFileName(objArchivoAdjunto.rutaArchivo);

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public Dictionary<string, object> EliminarArchivoAdjunto(int idArchivo)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (idArchivo <= 0) { throw new Exception("Ocurrió un error al eliminar el archivo."); }
                #endregion

                #region SE ELIMINA EL ARCHIVO
                tblCom_ArchivosAdjuntosProveedores objEliminar = _context.tblCom_ArchivosAdjuntosProveedores.Where(w => w.FK_numpro == idArchivo && w.registroActivo).FirstOrDefault();
                if (objEliminar == null)
                    throw new Exception("Ocurrió un error al eliminar el archivo.");

                objEliminar.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                objEliminar.fechaModificacion = DateTime.Now;
                objEliminar.registroActivo = false;
                _context.SaveChanges();
                #endregion

                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha eliminado con éxito.");
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "EliminarArchivoAdjunto", e, AccionEnum.ELIMINAR, idArchivo, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        private void PrimerVobo(int id)
        {
            var proveedor = _context.tblCom_sp_proveedores.FirstOrDefault(x => x.id == id);
            if (proveedor != null)
            {
                proveedor.primerVobo = true;
                proveedor.id_usuarioPrimerVobo = vSesiones.sesionUsuarioDTO.id;
                proveedor.fechaPrimerVobo = DateTime.Now;
                _context.SaveChanges();

                var alertas = _context.tblP_Alerta.Where(x => x.sistemaID == 4 && !x.visto && x.objID == proveedor.numpro && x.obj == "AltaProveedor").ToList();
                foreach (var item in alertas)
                {
                    item.visto = true;
                    _context.SaveChanges();
                }

                var vobo = _context.tblCom_AutorizarProveedor.FirstOrDefault(x => x.PuedeVobo && x.registroActivo);
                if (vobo != null)
                {
                    var alerta = new tblP_Alerta();
                    alerta.userEnviaID = vSesiones.sesionUsuarioDTO.id;
                    alerta.userRecibeID = vobo.idUsuario;
#if DEBUG
                    alerta.userRecibeID = 13;
#endif
                    alerta.tipoAlerta = 2;
                    alerta.sistemaID = 4;
                    alerta.visto = false;
                    alerta.url = "/Enkontrol/AltaProveedor/AltaProveedor?numpro=" + proveedor.numpro;
                    alerta.objID = (int)proveedor.numpro;
                    alerta.obj = "AltaProveedor";
                    alerta.msj = "Alta Proveedor - " + proveedor.numpro;
                    alerta.documentoID = 0;
                    alerta.moduloID = 0;
                    _context.tblP_Alerta.Add(alerta);
                    _context.SaveChanges();

                    var socios = _context.tblCom_ProveedoresSocios.FirstOrDefault(x => x.FK_idProv == proveedor.id && x.registroActivo);

                    string persona = "";
                    string fisica = "";
                    if (proveedor.persona_fisica == "S")
                    {
                        persona = "PERSONA FISICA";
                        fisica = PersonalUtilities.NombreCompletoMayusculas(proveedor.a_nombre, proveedor.a_paterno, proveedor.a_materno);
                    }
                    else
                    {
                        persona = "NOMBRES EN EL ACTA";
                    }

                    var correos = new List<string>();
                    var correoVoBo = _context.tblP_Usuario.FirstOrDefault(x => x.id == vobo.idUsuario && x.estatus);
                    if (correoVoBo != null)
                    {
                        correos.Add(correoVoBo.correo);
                    }
                    var idUsuarioAlertas = alertas.Select(x => x.userRecibeID).ToList();
                    correos.AddRange(_context.tblP_Usuario.Where(x => idUsuarioAlertas.Contains(x.id) && x.estatus).Select(x => x.correo).ToList());

                    var usuarioRegistro = _context.tblP_Usuario.FirstOrDefault(x => x.estatus && x.id == proveedor.id_usuarioCreacion);
                    if (usuarioRegistro != null && !string.IsNullOrEmpty(usuarioRegistro.correo))
                    {
                        correos.Add(usuarioRegistro.correo);
                    }

#if DEBUG
                    correos = new List<string>();
                    correos.Add("martin.zayas@construplan.com.mx");
#endif

                    string subject = "ALTA DE PROVEEDOR";

                    string sociosTemp = "";

                    if (socios != null && !string.IsNullOrEmpty(socios.socios))
                    {
                        foreach (var item in socios.socios)
                        {
                            switch (item)
                            {
                                case ',':
                                    sociosTemp += "<br/> ";
                                    break;
                                case '\n':
                                    sociosTemp += "<br/> ";
                                    break;
                                default:
                                    sociosTemp += item;
                                    break;
                            }
                        }
                    }

                    string body = string.Format(@"<H2>Envío documentación para la revisión del proveedor dado de alta en sistema Construplan con el núm. {0}</H2>
                                                      <br>
                                                       {1}
                                                      <br>
                                                      <H2>
                                                      {2}</H2>
                                                       <H2>
                                                      {3}</H2>
                                                      <br><br>Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>Construplan> Compras > Proveedores.<br>
                                                       Se informa que este es un correo autogenerado por el sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx</a>).
                                                       No es necesario dar una respuesta. Gracias.",
                                                       proveedor.numpro,
                                                       htmlCorreo(proveedor.id),
                                                       persona,
                                                       sociosTemp
                                                       );

                    var lstArchives = new List<adjuntoCorreoDTO>();
                    List<tblCom_ArchivosAdjuntosProveedores> lstArchivos = _context.tblCom_ArchivosAdjuntosProveedores.Where(w => w.FK_idProv == proveedor.id && w.registroActivo).ToList();

                    if (lstArchivos != null)
                    {
                        foreach (var item in lstArchivos)
                        {
                            var fileStream = GlobalUtils.GetFileAsStream(item.rutaArchivo);
                            using (var streamReader = new MemoryStream())
                            {
                                fileStream.CopyTo(streamReader);
                                //downloadFiles.Add(streamReader.ToArray());

                                lstArchives.Add(new adjuntoCorreoDTO
                                {
                                    archivo = streamReader.ToArray(),
                                    nombreArchivo = Path.GetFileNameWithoutExtension(item.rutaArchivo),
                                    extArchivo = Path.GetExtension(item.rutaArchivo)
                                });
                            }
                        }

                        GlobalUtils.sendMailWithFilesReclutamientos(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject),
                              body, correos, lstArchives);

                    }
                    else
                    {
                        GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, correos);
                    }
                }
                else
                {
                    throw new Exception("No se encontró información del VoBo");
                }
            }
            else
            {
                throw new Exception("No se encontró al proveedor");
            }
        }

        #region GENERALES
        public int GetLastProveedor(int tipoProveedor)
        {
            int sigNumProveedor = 0;

            try
            {
                var lstIds = new List<int?>();

                #region CONSTRPLAN

                #region EK

                int? numProvEKCP = 0;
                var lstObjsProveedores = new List<int>();

                if (tipoProveedor == (int)TipoProveedorEnum.provNacional)
                {
                    var conexion = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(EmpresaEnum.Construplan);

                    var odbc = new OdbcConsultaDTO();
                    odbc.consulta = "SELECT numpro FROM DBA.sp_proveedores WHERE numpro < 9000 ORDER BY numpro";

                    var ProvEkontrol = _contextEnkontrol.Select<NumProDTO>(conexion, odbc);

                    lstObjsProveedores = ProvEkontrol.OrderBy(e => e.numpro).Select(e => e.numpro).ToList();
                    numProvEKCP = findMissing(lstObjsProveedores);

                    if (numProvEKCP == -1)
                    {
                        throw new Exception("Ocurrio algo mal obteniendo el ultimo proveedor, favor de comunicarse con el departamento de TI");
                    }
                }
                else
                {
                    var conexion = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(EmpresaEnum.Construplan);

                    var odbc = new OdbcConsultaDTO();
                    odbc.consulta = "SELECT numpro FROM DBA.sp_proveedores WHERE numpro >= 9000 ORDER BY numpro";

                    var ProvEkontrol = _contextEnkontrol.Select<NumProDTO>(conexion, odbc);

                    lstObjsProveedores = ProvEkontrol.OrderBy(e => e.numpro).Select(e => e.numpro).ToList();
                    numProvEKCP = findMissing(lstObjsProveedores);

                    if (numProvEKCP == -1)
                    {
                        throw new Exception("Ocurrio algo mal obteniendo el ultimo proveedor, favor de comunicarse con el departamento de TI");
                    }
                }

                lstIds.Add(numProvEKCP ?? 0);
                #endregion

                #region SP

                int valSPCP = 0;
                var lstProvsSPCP = new List<int>();

                using (var ctx = new MainContext(EmpresaEnum.Construplan))
                {
                    var objUltimoProvSP = ctx.tblCom_sp_proveedores
                        .Where(e => e.registroActivo && (tipoProveedor == (int)TipoProveedorEnum.provNacional ? e.numpro < 9000 : (e.numpro < 9999 && e.numpro >= 9000)))
                        .ToList().Select(e => Convert.ToInt32(e.numpro)).OrderBy(e => e).ToList();

                    int numProvSP = findMissing(objUltimoProvSP);

                    if (objUltimoProvSP.Count() > 0)
                    {
                        valSPCP = numProvSP == -1 ? (objUltimoProvSP.Max() + 1) : numProvSP;
                    }
                }
                #endregion

                #endregion

                #region ARRENDADORA

                #region EK

                int? numProvEKARR = 0;
                List<int> lstObjsProveedoresARR = new List<int>();

                if (tipoProveedor == (int)TipoProveedorEnum.provNacional)
                {
                    var conexion = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(EmpresaEnum.Arrendadora);

                    var odbc = new OdbcConsultaDTO();
                    odbc.consulta = "SELECT numpro FROM DBA.sp_proveedores WHERE numpro < 9000 ORDER BY numpro";

                    var ProvEkontrol = _contextEnkontrol.Select<NumProDTO>(conexion, odbc);

                    lstObjsProveedoresARR = ProvEkontrol.OrderBy(e => e.numpro).Select(e => e.numpro).ToList();
                    numProvEKARR = findMissing(lstObjsProveedoresARR);

                    if (numProvEKARR == -1)
                    {
                        throw new Exception("Ocurrio algo mal obteniendo el ultimo proveedor, favor de comunicarse con el departamento de TI");
                    }
                }
                else
                {
                    var conexion = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(EmpresaEnum.Arrendadora);

                    var odbc = new OdbcConsultaDTO();
                    odbc.consulta = "SELECT numpro FROM DBA.sp_proveedores WHERE numpro >= 9000 ORDER BY numpro";

                    var ProvEkontrol = _contextEnkontrol.Select<NumProDTO>(conexion, odbc);

                    lstObjsProveedoresARR = ProvEkontrol.OrderBy(e => e.numpro).Select(e => e.numpro).ToList();
                    numProvEKARR = findMissing(lstObjsProveedoresARR);

                    if (numProvEKARR == -1)
                    {
                        throw new Exception("Ocurrio algo mal obteniendo el ultimo proveedor, favor de comunicarse con el departamento de TI");
                    }
                }

                lstIds.Add(numProvEKARR ?? 0);
                #endregion

                #region SP
                int valSPARR = 0;
                var lstProvsSPARR = new List<int>();

                using (var ctx = new MainContext(EmpresaEnum.Arrendadora))
                {
                    lstProvsSPARR = ctx.tblCom_sp_proveedores
                        .Where(e => e.registroActivo && (tipoProveedor == (int)TipoProveedorEnum.provNacional ? e.numpro < 9000 : (e.numpro < 9999 && e.numpro >= 9000)))
                        .ToList().Select(e => Convert.ToInt32(e.numpro)).OrderBy(e => e).ToList();

                    int numProvSP = findMissing(lstProvsSPARR);

                    if (lstProvsSPARR.Count() > 0)
                    {
                        valSPARR = numProvSP == -1 ? (lstProvsSPARR.Max() + 1) : numProvSP;
                    }
                }

                #endregion

                #endregion

                if (lstProvsSPCP.Count() > 0 && lstProvsSPCP.Contains(numProvEKCP.Value))
                {
                    sigNumProveedor = numProvEKCP.Value;
                }
                else
                {
                    if (valSPCP > numProvEKCP)
                    {
                        if (lstProvsSPARR.Count() > 0)
                        {
                            if (lstProvsSPARR.Contains(valSPCP) || lstProvsSPARR.Contains(numProvEKCP.Value))
                            {
                                sigNumProveedor = valSPARR;

                            }
                            else
                            {
                                sigNumProveedor = valSPCP;

                            }
                        }
                        else
                        {
                            sigNumProveedor = valSPCP;

                        }
                    }
                    else
                    {
                        sigNumProveedor = numProvEKCP.Value;

                    }
                }

                lstObjsProveedores.Add(numProvEKCP.Value);
                lstObjsProveedores = lstObjsProveedores.OrderBy(e => e).ToList();

                sigNumProveedor = findMissing(lstObjsProveedores);
            }
            catch (Exception e)
            {
                throw e;
            }

            return sigNumProveedor;
        }

        private int findMissing(List<int> lstNums)
        {
            int temp = lstNums.FirstOrDefault();

            foreach (var item in lstNums)
            {

                if (item != temp)
                {
                    return temp;
                }

                temp += 1;
                //temp += item;
            }

            return -1;

        }
        #endregion

        #region FilCombos
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboCiudad()
        {
            try
            {
                var lst = (List<Core.DTO.Principal.Generales.ComboDTO>)_contextEnkontrol
                .Where("SELECT ciudad as Value, desc_ciudad as Text FROM ciudades").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                lst.AsParallel().Where(w => string.IsNullOrEmpty(w.Prefijo)).ForAll(x => x.Prefijo = string.Empty);
                return lst.ToList();

            }
            catch (Exception) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoProveedor()
        {
            try
            {
                var lst = (List<Core.DTO.Principal.Generales.ComboDTO>)_contextEnkontrol
                    .Where("SELECT tipo_prov as Value, descripcion as Text FROM sp_tipo_prov").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                lst.AsParallel().Where(w => string.IsNullOrEmpty(w.Prefijo)).ForAll(x => x.Prefijo = string.Empty);
                return lst.ToList();
            }
            catch (Exception) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoTercero()
        {
            try
            {
                var lst = (List<Core.DTO.Principal.Generales.ComboDTO>)_contextEnkontrol
                    .Where("SELECT id_tercero as Value, desc_tercero as Text FROM sp_tercero").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                lst.AsParallel().Where(w => string.IsNullOrEmpty(w.Prefijo)).ForAll(x => x.Prefijo = string.Empty);
                return lst.ToList();
            }
            catch (Exception) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoOperacion()
        {
            try
            {
                var lst = (List<Core.DTO.Principal.Generales.ComboDTO>)_contextEnkontrol
                    .Where("SELECT id_operacion as Value, desc_operacion as Text FROM sp_operacion").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                lst.AsParallel().Where(w => string.IsNullOrEmpty(w.Prefijo)).ForAll(x => x.Prefijo = string.Empty);
                return lst.ToList();
            }
            catch (Exception) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoPagoTerceroTrans()
        {
            try
            {
                //var lst = (List<Core.DTO.Principal.Generales.ComboDTO>)_contextEnkontrol
                //    .Where("SELECT numero as Value, descripcion as Text FROM so_libre_abordo").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                //lst.AsParallel().Where(w => string.IsNullOrEmpty(w.Prefijo)).ForAll(x => x.Prefijo = string.Empty);
                //return lst.ToList();
                return new List<Core.DTO.Principal.Generales.ComboDTO>();
            }
            catch (Exception) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoMovBase()
        {
            try
            {
                var lst = (List<Core.DTO.Principal.Generales.ComboDTO>)_contextEnkontrol
                    .Where("SELECT tm as Value, descripcion as Text FROM sp_tm").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                lst.AsParallel().Where(w => string.IsNullOrEmpty(w.Prefijo)).ForAll(x => x.Prefijo = string.Empty);
                return lst.ToList();
            }
            catch (Exception) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoMoneda()
        {
            try
            {
                var lst = (List<Core.DTO.Principal.Generales.ComboDTO>)_contextEnkontrol
                    .Where("SELECT clave as Value, moneda as Text FROM moneda").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                lst.AsParallel().Where(w => string.IsNullOrEmpty(w.Prefijo)).ForAll(x => x.Prefijo = string.Empty);
                return lst.ToList();
            }
            catch (Exception) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboBancos()
        {
            try
            {
                var lst = (List<Core.DTO.Principal.Generales.ComboDTO>)_contextEnkontrol
                      .Where("SELECT banco as Value, descripcion as Text FROM sb_bancos").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                lst.AsParallel().Where(w => string.IsNullOrEmpty(w.Prefijo)).ForAll(x => x.Prefijo = string.Empty);
                return lst.ToList();
            }
            catch (Exception) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoRegimen()
        {
            try
            {
                var lst = (List<Core.DTO.Principal.Generales.ComboDTO>)_contextEnkontrol
                     .Where("SELECT id_regimen as Value, desc_regimen as Text from dba.sr_regimen").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                lst.AsParallel().Where(w => string.IsNullOrEmpty(w.Prefijo)).ForAll(x => x.Prefijo = string.Empty);
                return lst.ToList();
            }
            catch (Exception) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        #endregion

    }
}

