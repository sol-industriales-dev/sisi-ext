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
    public class AltaProveedorColombiaDAO : GenericDAO<tblCom_sp_proveedoresColombia>, IAltaProveedorDAO
    {
        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        private readonly string RutaBase = @"\\REPOSITORIO\Proyecto\SIGOPLAN\COMPRAS\PROVEEDORESCOLOMBIA";
        private const string RutaLocal = @"C:\Proyecto\SIGOPLAN\COMPRAS\PROVEEDORESCOLOMBIA";
        private string NombreControlador = "AltaProveedorController";
        private bool productivo = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["enkontrolProductivo"]) == "1";
        bool esEdicion = false;
        public Dictionary<string, object> getProveedores()
        {
            try
            {
                List<tblCom_sp_proveedoresConsultaDTO> listaProveedoresCol = new List<tblCom_sp_proveedoresConsultaDTO>();
                List<tblCom_sp_proveedoresConsultaDTO> listaUnidasProveedores = new List<tblCom_sp_proveedoresConsultaDTO>();
                tblCom_sp_proveedoresConsultaDTO objListaProveedoresCol = new tblCom_sp_proveedoresConsultaDTO();
                tblCom_sp_proveedoresConsultaDTO objListaProveedores = new tblCom_sp_proveedoresConsultaDTO();
                List<tblCom_sp_proveedoresColombia> listaProveedores = _context.tblCom_sp_proveedoresColombia.Where(x => x.registroActivo && x.statusAutorizacion == false).ToList();
                var puedeDarVobo = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.idUsuario == vSesiones.sesionUsuarioDTO.id && e.PuedeVobo == true).FirstOrDefault();
                var puedeAutorizar = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.idUsuario == vSesiones.sesionUsuarioDTO.id && e.PuedeAutorizar == true).FirstOrDefault();


                bool estatusVobo = false;
                bool estatusAutorizar = false;
                if (puedeDarVobo != null)
                {
                    estatusVobo = true;
                }
                if (puedeAutorizar != null)
                {
                    estatusAutorizar = true;
                }



                foreach (var prov in listaProveedores)
                {
                    var TieneVobo = _context.tblCom_sp_proveedoresColombia.Where(x => x.registroActivo && x.id == prov.id && x.vobo == true).FirstOrDefault();
                    var TieneAutorizacion = _context.tblCom_sp_proveedoresColombia.Where(x => x.registroActivo && x.id == prov.id && x.Autorizado == true).FirstOrDefault();
                    if (TieneVobo != null)
                    {
                        estatusAutorizar = true;
                        estatusVobo = false;
                    }
                    if (puedeAutorizar != null && TieneVobo != null)
                    {
                        estatusAutorizar = true;
                    }
                    else
                    {
                        if (puedeDarVobo != null && TieneVobo == null)
                        {
                            estatusVobo = true;
                        }

                        estatusAutorizar = false;
                    }
                    if (TieneVobo != null && TieneAutorizacion != null)
                    {
                        estatusAutorizar = false;
                    }
                    objListaProveedoresCol = new tblCom_sp_proveedoresConsultaDTO();
                    objListaProveedoresCol.id = prov.id;
                    objListaProveedoresCol.numpro = prov.numpro;
                    objListaProveedoresCol.nombre = !string.IsNullOrEmpty(prov.persona_fisica) && prov.persona_fisica.Contains("S") ? PersonalUtilities.NombreCompletoMayusculas(prov.a_nombre, prov.a_paterno, prov.a_materno) : prov.nombre;
                    objListaProveedoresCol.direccion = prov.direccion;
                    objListaProveedoresCol.ciudad = prov.ciudad;
                    objListaProveedoresCol.rfc = prov.rfc;
                    objListaProveedoresCol.vobo = prov.vobo;
                    objListaProveedoresCol.Autorizado = prov.Autorizado;
                    objListaProveedoresCol.puedeDarVobo = estatusVobo;
                    objListaProveedoresCol.puedeAutorizar = estatusAutorizar;
                    objListaProveedoresCol.statusAutorizacion = prov.statusAutorizacion;
                    objListaProveedoresCol.esEnKontrol = false;
                    listaProveedoresCol.Add(objListaProveedoresCol);

                    string descEstatus = "";

                    if (!prov.vobo)
                    {
                        descEstatus = "Pendiente VoBo";
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

                    objListaProveedoresCol.descEstatus = descEstatus;
                }

                var conexion = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(EmpresaEnum.Colombia);

                var odbc = new OdbcConsultaDTO();
                odbc.consulta = string.Format(@"select numpro,nombre,direccion,ciudad,rfc,persona_fisica,a_nombre,a_paterno,a_materno
                                    from dba.sp_proveedores WHERE cancelado = 'A'");


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
                listaUnidasProveedores.AddRange(listaProveedoresCol);
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
                List<tblCom_sp_proveedoresColombiaDTO> lstProveedores = new List<tblCom_sp_proveedoresColombiaDTO>();
                tblCom_sp_proveedoresColombiaDTO datosProveedores = new tblCom_sp_proveedoresColombiaDTO();
                var socioProveedor = "";
                var infoProveedor = _context.tblCom_sp_proveedoresColombia.Where(x => x.registroActivo && x.id == id).FirstOrDefault();
                var lstArchivosEK = _context.tblCom_ArchivosAdjuntosProveedores.Where(x => x.registroActivo && x.FK_numpro == numpro).ToList();
                //var infoProveedorSocioEK = _context.tblCom_ProveedoresSocios.Where(e => e.registroActivo && e.FK_numpro == numpro).FirstOrDefault();
                //var infoCuentasProv = listaCuentasProveedores.Where(x => x.registroActivo && x.numpro == numpro).ToList();
                //if (infoProveedorSocioEK!=null)
                //{
                //    socioProveedor = infoProveedorSocioEK.socios;
                //}
                var conexion = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(EmpresaEnum.Colombia);

                var odbc = new OdbcConsultaDTO();
                odbc.consulta = string.Format(@"SELECT numpro,nomcorto,nombre,direccion,ciudad,cp,responsable,telefono1,telefono2,fax,
                                                        email,rfc,limcred,tmbase,descuento,condpago,moneda,cta_bancaria,tipo_prov,cancelado,
                                                        tipo_pago,inst_factoraje,cta_cheque,cve_banco,plaza_banco,tipo_cta,fecha_modifica_plazo_pago,usuario_modifica_plazo_pago,prov_exterior,filial,
                                                        tipo_tercero,tipo_operacion,curp,bit_factoraje,id_fiscal,nacionalidad,persona_fisica,a_paterno,a_materno,a_nombre,
                                                        cncdirid,pyme,calle,tipo_soc,colonia,deleg,activo,tranfer_banco,cve_medio_confir,transfer_sant,
                                                        lada,nombre_archivo,id_codigo_cat,num_empleado,num_usuario,num_nomina,cat_nomina,beneficiario,ciiu,base_iva,
                                                        id_doc_identidad,camara_comercio,obliga_cfd,bit_autoretenedor,id_regimen,categoria_empleado,sincroniza_adm,bit_corp,convenio_nomina FROM dba.sp_proveedores WHERE numpro = {0}", numpro);

                var ProvEkontrol = _contextEnkontrol.Select<tblCom_sp_proveedoresColombiaDTO>(conexion, odbc);

                var archivoEK = new List<tblCom_ArchivosAdjuntosProveedores>();
                archivoEK = lstArchivosEK;
                if (ProvEkontrol.Count > 0)
                {
                    esEdicion = true;
                    foreach (var item in ProvEkontrol)
                    {

                        var objListaProveedores = new tblCom_sp_proveedoresColombiaDTO();
                        objListaProveedores.numpro = item.numpro;
                        objListaProveedores.nomcorto = item.nomcorto.Trim();
                        objListaProveedores.nombre = item.nombre.Trim();
                        objListaProveedores.direccion = item.direccion.Trim();
                        objListaProveedores.ciudad = item.ciudad;
                        objListaProveedores.cp = item.cp;
                        objListaProveedores.responsable = item.responsable.Trim();
                        objListaProveedores.telefono1 = item.telefono1;
                        objListaProveedores.telefono2 = item.telefono2;
                        objListaProveedores.email = item.email;
                        objListaProveedores.fax = item.fax;
                        objListaProveedores.rfc = item.rfc;
                        objListaProveedores.limcred = item.limcred;
                        objListaProveedores.tmbase = item.tmbase;
                        objListaProveedores.condpago = item.condpago;
                        objListaProveedores.moneda = item.moneda.Trim();
                        objListaProveedores.cta_bancaria = item.cta_bancaria;
                        objListaProveedores.tipo_prov = item.tipo_prov;
                        objListaProveedores.cancelado = item.cancelado;
                        objListaProveedores.tipo_pago = item.tipo_pago;
                        objListaProveedores.cta_cheque = item.cta_cheque;
                        objListaProveedores.cve_banco = item.cve_banco;
                        objListaProveedores.plaza_banco = item.plaza_banco;
                        objListaProveedores.tipo_cta = item.tipo_cta;
                        objListaProveedores.fecha_modifica_plazo_pago = item.fecha_modifica_plazo_pago;
                        objListaProveedores.usuario_modifica_plazo_pago = item.usuario_modifica_plazo_pago;
                        objListaProveedores.prov_exterior = item.prov_exterior;
                        objListaProveedores.filial = item.filial.Trim();
                        objListaProveedores.tipo_tercero = item.tipo_tercero;
                        objListaProveedores.tipo_operacion = item.tipo_operacion;
                        objListaProveedores.id_doc_identidad = item.id_doc_identidad;
                        objListaProveedores.id_regimen = item.id_regimen;
                        objListaProveedores.curp = item.curp;
                        objListaProveedores.id_fiscal = item.id_fiscal;
                        objListaProveedores.nacionalidad = item.nacionalidad;
                        objListaProveedores.persona_fisica = item.persona_fisica.Trim();
                        objListaProveedores.a_paterno = item.a_paterno;
                        objListaProveedores.a_materno = item.a_materno;
                        objListaProveedores.a_nombre = item.a_nombre;
                        objListaProveedores.bit_factoraje = item.bit_factoraje.Trim();
                        objListaProveedores.calle = item.calle;
                        objListaProveedores.deleg = item.deleg;
                        objListaProveedores.colonia = item.colonia;
                        objListaProveedores.base_iva = item.base_iva;
                        objListaProveedores.transfer_sant = item.transfer_sant;
                        objListaProveedores.bit_autoretenedor = item.bit_autoretenedor.Trim();
                        objListaProveedores.categoria_empleado = item.categoria_empleado;
                        objListaProveedores.sincroniza_adm = item.sincroniza_adm;
                        objListaProveedores.convenio_nomina = item.convenio_nomina.Trim();
                        //objListaProveedores.socios = socioProveedor;
                        objListaProveedores.esEdicion = esEdicion;
                        objListaProveedores.lstArchivos = archivoEK;

                        resultado.Add(ITEMS, objListaProveedores);
                    }

                }
                else
                {


                    var infoProveedorArchivo = _context.tblCom_ArchivosAdjuntosProveedores.Where(e => e.registroActivo && e.FK_idProv == infoProveedor.id).FirstOrDefault();
                    //string infoProveedorSocio = _context.tblCom_ProveedoresSocios.Where(e => e.registroActivo && e.FK_idProv == infoProveedor.id).FirstOrDefault().socios;
                    var lstArchivos = _context.tblCom_ArchivosAdjuntosProveedores.Where(x => x.registroActivo && x.FK_idProv == infoProveedor.id).ToList();


                    var archivo = new List<tblCom_ArchivosAdjuntosProveedores>();
                    archivo = lstArchivos;


                    var InformacionProvedores = new Core.DTO.Enkontrol.OrdenCompra.tblCom_sp_proveedoresColombiaDTO();
                    InformacionProvedores.id = infoProveedor.id;
                    InformacionProvedores.numpro = infoProveedor.numpro;
                    InformacionProvedores.nomcorto = infoProveedor.nomcorto.Trim();
                    InformacionProvedores.nombre = infoProveedor.nombre.Trim();
                    InformacionProvedores.direccion = infoProveedor.direccion.Trim();
                    InformacionProvedores.ciudad = infoProveedor.ciudad;
                    InformacionProvedores.cp = infoProveedor.cp;
                    InformacionProvedores.responsable = infoProveedor.responsable;
                    InformacionProvedores.telefono1 = infoProveedor.telefono1;
                    InformacionProvedores.telefono2 = infoProveedor.telefono2;
                    InformacionProvedores.email = infoProveedor.email.Trim();
                    InformacionProvedores.fax = infoProveedor.fax;
                    InformacionProvedores.rfc = infoProveedor.rfc;
                    InformacionProvedores.limcred = infoProveedor.limcred;
                    InformacionProvedores.tmbase = infoProveedor.tmbase;
                    InformacionProvedores.condpago = infoProveedor.condpago;
                    InformacionProvedores.moneda = infoProveedor.moneda.Trim();
                    InformacionProvedores.cta_bancaria = infoProveedor.cta_bancaria;
                    InformacionProvedores.tipo_prov = infoProveedor.tipo_prov;
                    InformacionProvedores.cancelado = infoProveedor.cancelado;
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
                    InformacionProvedores.curp = infoProveedor.curp;
                    InformacionProvedores.id_fiscal = infoProveedor.id_fiscal;
                    InformacionProvedores.nacionalidad = infoProveedor.nacionalidad.Trim();
                    InformacionProvedores.persona_fisica = infoProveedor.persona_fisica.Trim();
                    InformacionProvedores.a_paterno = infoProveedor.a_paterno.Trim();
                    InformacionProvedores.a_materno = infoProveedor.a_materno.Trim();
                    InformacionProvedores.a_nombre = infoProveedor.a_nombre.Trim();
                    InformacionProvedores.bit_factoraje = infoProveedor.bit_factoraje.Trim();
                    InformacionProvedores.calle = infoProveedor.calle;
                    InformacionProvedores.deleg = infoProveedor.deleg;
                    InformacionProvedores.id_doc_identidad = infoProveedor.id_doc_identidad;
                    InformacionProvedores.id_regimen = infoProveedor.id_regimen;
                    InformacionProvedores.colonia = infoProveedor.colonia;
                    InformacionProvedores.base_iva = infoProveedor.base_iva;
                    InformacionProvedores.transfer_sant = infoProveedor.transfer_sant;
                    InformacionProvedores.bit_autoretenedor = infoProveedor.bit_autoretenedor.Trim();
                    InformacionProvedores.categoria_empleado = infoProveedor.categoria_empleado;
                    InformacionProvedores.sincroniza_adm = infoProveedor.sincroniza_adm;
                    InformacionProvedores.convenio_nomina = infoProveedor.convenio_nomina.Trim();
                    //InformacionProvedores.socios = socioProveedor;
                    InformacionProvedores.lstArchivos = archivo;
                    InformacionProvedores.esEdicion = esEdicion;

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
        public Dictionary<string, object> GuardarProveedor(tblCom_sp_proveedoresDTO objProveedor, HttpPostedFileBase objFile)
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
        public Dictionary<string, object> GuardarEditarProveedorColombia(tblCom_sp_proveedoresColombiaDTO objProveedor, HttpPostedFileBase objFile)
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
                            if (objProveedor.limcred <= 0) { throw new Exception("El limite de credito es requerido"); }
                            if (objProveedor.tmbase <= 0) { throw new Exception("Tipo de movimiento requerido"); }
                            if (objProveedor.condpago < 0) { throw new Exception("Condición de pago invalido"); }
                            //if (objProveedor.persona_fisica == "N" && objProveedor.socios == "") { throw new Exception("Socio requerido"); }


                            #endregion

                            List<tblCom_sp_proveedoresColombia> lstProveedores = new List<tblCom_sp_proveedoresColombia>();
                            tblCom_sp_proveedoresColombia datosProveedores = new tblCom_sp_proveedoresColombia();

                            if (objProveedor.esNuevo == 0)
                            {
                                var ExisteEnKontrol = consultaCheckProductivo(
                                    string.Format(@"SELECT * FROM sp_proveedores WHERE (numpro = '{0}' OR rfc = '{1}') AND moneda = {2}", objProveedor.numpro, objProveedor.rfc, objProveedor.moneda)
                                );

                                if (ExisteEnKontrol == null)
                                {
                                    #region SE REGISTRA EN SQLSERV MEXICO
                                    var infoProveedor = _context.tblCom_sp_proveedoresColombia.Where(e => e.registroActivo && (e.numpro == objProveedor.numpro || e.rfc == objProveedor.rfc)).FirstOrDefault();
                                    if (infoProveedor == null)
                                    {
                                        datosProveedores.numpro = objProveedor.numpro;
                                        datosProveedores.nomcorto = objProveedor.nomcorto.Trim();
                                        datosProveedores.nombre = objProveedor.nombre.Trim();
                                        datosProveedores.direccion = objProveedor.direccion.Trim();
                                        datosProveedores.ciudad = objProveedor.ciudad;
                                        datosProveedores.cp = objProveedor.cp;
                                        datosProveedores.responsable = objProveedor.responsable.Trim();
                                        datosProveedores.telefono1 = objProveedor.telefono1;
                                        datosProveedores.telefono2 = objProveedor.telefono2;
                                        datosProveedores.email = objProveedor.email.Trim();
                                        datosProveedores.fax = objProveedor.fax;
                                        datosProveedores.rfc = objProveedor.rfc;
                                        datosProveedores.limcred = objProveedor.limcred;
                                        datosProveedores.tmbase = objProveedor.tmbase;
                                        datosProveedores.condpago = objProveedor.condpago;
                                        datosProveedores.moneda = objProveedor.moneda.Trim();
                                        datosProveedores.cta_bancaria = objProveedor.cta_bancaria;
                                        datosProveedores.tipo_prov = objProveedor.tipo_prov;
                                        datosProveedores.tipo_pago = objProveedor.tipo_pago;
                                        datosProveedores.cta_cheque = objProveedor.cta_cheque;
                                        datosProveedores.cve_banco = objProveedor.cve_banco;
                                        datosProveedores.plaza_banco = objProveedor.plaza_banco;
                                        datosProveedores.tipo_cta = objProveedor.tipo_cta;
                                        datosProveedores.prov_exterior = objProveedor.prov_exterior;
                                        datosProveedores.filial = objProveedor.filial.Trim();
                                        datosProveedores.tipo_tercero = objProveedor.tipo_tercero;
                                        datosProveedores.tipo_operacion = objProveedor.tipo_operacion;
                                        //datosProveedores.curp = objProveedor.curp.Trim();
                                        datosProveedores.curp = "";
                                        datosProveedores.nacionalidad = objProveedor.nacionalidad.Trim();
                                        datosProveedores.persona_fisica = objProveedor.persona_fisica;
                                        datosProveedores.a_paterno = objProveedor.a_paterno.Trim();
                                        datosProveedores.a_materno = objProveedor.a_materno.Trim();
                                        datosProveedores.a_nombre = objProveedor.a_nombre.Trim();
                                        datosProveedores.calle = objProveedor.calle;
                                        datosProveedores.deleg = objProveedor.deleg;
                                        datosProveedores.colonia = objProveedor.colonia;
                                        datosProveedores.transfer_banco = string.Empty;
                                        datosProveedores.obliga_cfd = objProveedor.obliga_cfd;
                                        datosProveedores.bit_factoraje = objProveedor.bit_factoraje;
                                        datosProveedores.cancelado = "C";
                                        datosProveedores.id_regimen = objProveedor.id_regimen;
                                        datosProveedores.descuento = objProveedor.descuento;
                                        datosProveedores.base_iva = objProveedor.base_iva;
                                        datosProveedores.id_doc_identidad = objProveedor.id_doc_identidad;
                                        //datosProveedores.transfer_sant = "N";
                                        //datosProveedores.categoria_empleado = "N";
                                        //datosProveedores.sincroniza_adm = "S";
                                        datosProveedores.bit_autoretenedor = objProveedor.bit_autoretenedor;
                                        datosProveedores.convenio_nomina = objProveedor.convenio_nomina;
                                        datosProveedores.id_usuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                        datosProveedores.fechaCreacion = DateTime.Now;
                                        datosProveedores.registroActivo = true;

                                        _context.tblCom_sp_proveedoresColombia.Add(datosProveedores);
                                        _context.SaveChanges();

                                        var objVobo = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.PuedeVobo == true).ToList();
                                        var infoVobo = objVobo.FirstOrDefault();
                                        int numprov = Convert.ToInt32(objProveedor.numpro);
                                        foreach (var item in objVobo)
                                        {

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
                                            objNuevaAlerta.url = "/Enkontrol/AltaProveedor/AltaProveedorColombia?numpro=" + objProveedor.numpro;
                                            objNuevaAlerta.objID = numprov != null ? numprov : 0;
                                            objNuevaAlerta.obj = "AltaProveedor";
                                            objNuevaAlerta.msj = "Alta Proveedor - " + objProveedor.numpro;
                                            objNuevaAlerta.documentoID = 0;
                                            objNuevaAlerta.moduloID = 0;
                                            _context.tblP_Alerta.Add(objNuevaAlerta);
                                            _context.SaveChanges();
                                            #endregion //ALERTA SIGPLAN

                                        }
                                        //int idProve = _context.tblCom_sp_proveedoresColombia.OrderByDescending(x => x.id).First().id;
                                        if (objFile != null)
                                        {
                                            #region SE REGISTRA EL ARCHIVO ADJUNTO
                                            //decimal FK_numpro = _context.tblCom_sp_proveedoresColombia.OrderByDescending(x => x.id).First().numpro;


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
                                        var infoProveedorSocio = _context.tblCom_ProveedoresSocios.Where(e => e.registroActivo && e.FK_idProv == datosProveedores.id).FirstOrDefault();
                                        if (infoProveedorSocio != null)
                                        {
                                            infoSocios.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                            infoSocios.fecha_modificacion = DateTime.Now;

                                        }
                                        else
                                        {
                                            infoSocios.FK_idProv = datosProveedores.id;
                                            infoSocios.FK_numpro = objProveedor.numpro;
                                            infoSocios.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                            infoSocios.fecha_creacion = DateTime.Now;
                                            infoSocios.registroActivo = true;


                                            //var usuarioQueAplicoVobo = _context.tblP_Usuario.FirstOrDefault(x => x.id == infoVobo.idUsuario);

                                            #region Correo de envio de informacion y documentos a toño
                                            List<string> lstCorreos = new List<string>();

                                            foreach (var item in objVobo)
                                            {
                                                lstCorreos.Add(_context.tblP_Usuario.FirstOrDefault(x => x.id == item.idUsuario).correo);
                                            }
                                            
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
                                            lstCorreos.Add("alexandra.gomez@construplan.com");
                                            lstCorreos.Add("aura.canas@construplan.com");
#if DEBUG
                                            lstCorreos = new List<string>();
                                            lstCorreos.Add("miguel.buzani@construplan.com.mx");
                                            //lstCorreos.Add("martin.zayas@construplan.com.mx");
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
                                            string body = string.Format(@"<H2>Envío documentación para la revisión del proveedor dado de alta en la empresa Construplan Colombia con el núm. {0}</H2>
                                                       <br>
                                                       {1}
                                                      <br>
                                                      <H2>
                                                      {2}</H2>
                                                      <br><br>Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>Construplan> Compras > Proveedores.<br>
                                                       Se informa que este es un correo autogenerado por el sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx</a>).
                                                       No es necesario dar una respuesta. Gracias.",
                                                         objProveedor.numpro,
                                                         htmlCorreo(datosProveedores.id),
                                                         PersonaFisicaMoral
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
                                            //_context.tblCom_ProveedoresSocios.Add(infoSocios);
                                            //_context.SaveChanges();
                                        }
                                            #endregion



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
                                if (objProveedor.esEdicionEnKontrol != true)
                                {
                                    #region edicion mexico

                                    var infoProveedor = _context.tblCom_sp_proveedoresColombia.Where(e => e.registroActivo && e.id == objProveedor.id).FirstOrDefault();
                                    infoProveedor.numpro = objProveedor.numpro;
                                    infoProveedor.nomcorto = objProveedor.nomcorto;
                                    infoProveedor.nombre = objProveedor.nombre;
                                    infoProveedor.direccion = objProveedor.direccion;
                                    infoProveedor.ciudad = objProveedor.ciudad;
                                    infoProveedor.cp = objProveedor.cp;
                                    infoProveedor.responsable = objProveedor.responsable;
                                    infoProveedor.telefono1 = objProveedor.telefono1;
                                    infoProveedor.telefono2 = objProveedor.telefono2;
                                    infoProveedor.email = objProveedor.email;
                                    infoProveedor.fax = objProveedor.fax;
                                    infoProveedor.rfc = objProveedor.rfc;
                                    infoProveedor.limcred = objProveedor.limcred;
                                    infoProveedor.tmbase = objProveedor.tmbase;
                                    infoProveedor.condpago = objProveedor.condpago;
                                    infoProveedor.moneda = objProveedor.moneda;
                                    infoProveedor.cta_bancaria = objProveedor.cta_bancaria;
                                    infoProveedor.tipo_prov = objProveedor.tipo_prov;
                                    infoProveedor.tipo_pago = objProveedor.tipo_pago;
                                    infoProveedor.cta_cheque = objProveedor.cta_cheque;
                                    infoProveedor.cve_banco = objProveedor.cve_banco;
                                    infoProveedor.plaza_banco = objProveedor.plaza_banco;
                                    infoProveedor.tipo_cta = objProveedor.tipo_cta;
                                    infoProveedor.prov_exterior = objProveedor.prov_exterior;
                                    infoProveedor.filial = objProveedor.filial;
                                    infoProveedor.tipo_tercero = objProveedor.tipo_tercero;
                                    infoProveedor.tipo_operacion = objProveedor.tipo_operacion;
                                    //infoProveedor.curp = objProveedor.curp;
                                    infoProveedor.nacionalidad = objProveedor.nacionalidad;
                                    infoProveedor.persona_fisica = objProveedor.persona_fisica;
                                    infoProveedor.a_paterno = objProveedor.a_paterno;
                                    infoProveedor.a_materno = objProveedor.a_materno;
                                    infoProveedor.a_nombre = objProveedor.a_nombre;
                                    infoProveedor.calle = objProveedor.calle;
                                    infoProveedor.deleg = objProveedor.deleg;
                                    infoProveedor.colonia = objProveedor.colonia;
                                    infoProveedor.transfer_banco = string.Empty;
                                    infoProveedor.obliga_cfd = objProveedor.obliga_cfd;
                                    infoProveedor.bit_factoraje = objProveedor.bit_factoraje;
                                    infoProveedor.cancelado = objProveedor.cancelado;
                                    infoProveedor.descuento = objProveedor.descuento;
                                    infoProveedor.base_iva = objProveedor.base_iva;
                                    infoProveedor.id_regimen = objProveedor.id_regimen;
                                    //datosProveedores.transfer_sant = "N";
                                    //datosProveedores.categoria_empleado = "N";
                                    //datosProveedores.sincroniza_adm = "S";
                                    infoProveedor.bit_autoretenedor = objProveedor.bit_autoretenedor;
                                    infoProveedor.convenio_nomina = objProveedor.convenio_nomina;
                                    infoProveedor.id_usuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                    infoProveedor.fechaModificacion = DateTime.Now;
                                    _context.SaveChanges();


                                    //var infoProveedorSocioEditar = _context.tblCom_ProveedoresSocios.Where(e => e.registroActivo && e.FK_idProv == infoProveedor.id).FirstOrDefault();

                                    //infoProveedorSocioEditar.socios = objProveedor.socios;
                                    //infoProveedorSocioEditar.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                    //infoProveedorSocioEditar.fecha_modificacion = DateTime.Now;
                                    //_context.SaveChanges();

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
                                    #region Edicion eKontrol

                                    var updateProv = @"
                                                   UPDATE dba.sp_proveedores 
                                                    SET numpro=?
                                                         ,nomcorto=?
                                                         ,nombre=?
                                                         ,direccion=?
                                                         ,ciudad=?
                                                         ,cp=?
                                                         ,responsable=?
                                                         ,telefono1=?
                                                         ,telefono2=?
                                                         ,fax=?
                                                         ,email=?
                                                         ,rfc=?
                                                        ,limcred=?
                                                        ,tmbase=?
                                                        ,descuento=?
                                                        ,condpago=?
                                                        ,moneda=?
                                                        ,cta_bancaria=?
                                                        ,tipo_prov=?
                                                        ,cancelado=?
                                                        ,tipo_pago=?
                                                        ,inst_factoraje=?
                                                        ,cta_cheque=?
                                                        ,cve_banco=?
                                                        ,plaza_banco=?
                                                        ,tipo_cta=?
                                                        ,fecha_modifica_plazo_pago=?
                                                        ,usuario_modifica_plazo_pago=?
                                                        ,prov_exterior=?
                                                        ,filial=?
                                                        ,tipo_tercero=?
                                                        ,tipo_operacion=?
                                                        ,curp=?
                                                        ,bit_factoraje=?
                                                        ,id_fiscal=?
                                                        ,nacionalidad=?
                                                        ,persona_fisica=?
                                                        ,a_paterno=?
                                                        ,a_materno=?
                                                        ,a_nombre=?
                                                        ,cncdirid=?
                                                        ,pyme=?
                                                        ,calle=?
                                                        ,tipo_soc=?
                                                        ,colonia=?
                                                        ,deleg=?
                                                        ,activo=?
                                                        ,tranfer_banco=?
                                                        ,cve_medio_confir=?
                                                        ,transfer_sant=?
                                                        ,lada=?
                                                        ,nombre_archivo=?
                                                        ,id_codigo_cat=?
                                                        ,num_empleado=?
                                                        ,num_usuario=?
                                                        ,num_nomina=?
                                                        ,cat_nomina=?
                                                        ,beneficiario=?
                                                        ,ciiu=?
                                                        ,base_iva=?
                                                        ,id_doc_identidad=?
                                                        ,camara_comercio=?
                                                        ,obliga_cfd=?
                                                        ,bit_autoretenedor=?
                                                        ,id_regimen=?
                                                        ,categoria_empleado=?
                                                        ,sincroniza_adm=?
                                                        ,bit_corp=?
                                                        ,convenio_nomina =?
                                                      WHERE numpro = ?";

                                    using (var cmd = new OdbcCommand(updateProv))
                                    {
                                        //numpro, nomcorto, nombre, direccion, ciudad, cp, responsable, telefono1, telefono2, fax,
                                        cmd.Parameters.Add("@numpro", OdbcType.Numeric).Value = objProveedor.numpro;
                                        cmd.Parameters.Add("@nomcorto", OdbcType.Char).Value = objProveedor.nomcorto;
                                        cmd.Parameters.Add("@nombre", OdbcType.Char).Value = objProveedor.nombre;
                                        cmd.Parameters.Add("@direccion", OdbcType.Char).Value = objProveedor.direccion;
                                        cmd.Parameters.Add("@ciudad", OdbcType.Char).Value = objProveedor.ciudad;
                                        cmd.Parameters.Add("@cp", OdbcType.Char).Value = objProveedor.cp;
                                        cmd.Parameters.Add("@responsable", OdbcType.Char).Value = objProveedor.responsable;
                                        cmd.Parameters.Add("@telefono1", OdbcType.Char).Value = objProveedor.telefono1;
                                        cmd.Parameters.Add("@telefono2", OdbcType.Char).Value = objProveedor.telefono2;
                                        cmd.Parameters.Add("@fax", OdbcType.Char).Value = objProveedor.fax;

                                        //  email, rfc, limcred, tmbase, descuento, condpago, moneda, cta_bancaria, tipo_prov, cancelado,
                                        cmd.Parameters.Add("@email", OdbcType.VarChar).Value = objProveedor.email;
                                        cmd.Parameters.Add("@rfc", OdbcType.Char).Value = objProveedor.rfc;
                                        cmd.Parameters.Add("@limcred", OdbcType.Numeric).Value = objProveedor.limcred;
                                        cmd.Parameters.Add("@tmbase", OdbcType.Numeric).Value = objProveedor.tmbase;
                                        cmd.Parameters.Add("@descuento", OdbcType.Numeric).Value = objProveedor.tmbase;
                                        cmd.Parameters.Add("@condpago", OdbcType.Numeric).Value = objProveedor.condpago;
                                        cmd.Parameters.Add("@moneda", OdbcType.Char).Value = objProveedor.moneda.Trim();
                                        cmd.Parameters.Add("@cta_bancaria", OdbcType.Char).Value = objProveedor.cta_bancaria;
                                        cmd.Parameters.Add("@tipo_prov", OdbcType.Numeric).Value = objProveedor.tipo_prov;
                                        cmd.Parameters.Add("@cancelado", OdbcType.Char).Value = objProveedor.cancelado;

                                        //tipo_pago, inst_factoraje, cta_cheque, cve_banco, plaza_banco, tipo_cta, fecha_modifica_plazo_pago, usuario_modifica_plazo_pago, prov_exterior, filial,
                                        cmd.Parameters.Add("@tipo_pago", OdbcType.Char).Value = string.Empty;
                                        cmd.Parameters.Add("@inst_factoraje", OdbcType.Numeric).Value = 0;
                                        cmd.Parameters.Add("@cta_cheque", OdbcType.Char).Value = objProveedor.cta_cheque;
                                        cmd.Parameters.Add("@cve_banco", OdbcType.Char).Value = objProveedor.cve_banco;
                                        cmd.Parameters.Add("@plaza_banco", OdbcType.Char).Value = objProveedor.plaza_banco;
                                        cmd.Parameters.Add("@tipo_cta", OdbcType.Char).Value = objProveedor.tipo_cta;
                                        cmd.Parameters.Add("@fecha_modifica_plazo_pago", OdbcType.DateTime).Value = objProveedor.fecha_modifica_plazo_pago;
                                        cmd.Parameters.Add("@usuario_modifica_plazo_pago", OdbcType.Numeric).Value = objProveedor.usuario_modifica_plazo_pago;
                                        cmd.Parameters.Add("@prov_exterior", OdbcType.Char).Value = string.Empty;
                                        cmd.Parameters.Add("@filial", OdbcType.Char).Value = objProveedor.filial;

                                        //tipo_tercero, tipo_operacion, curp, bit_factoraje, id_fiscal, nacionalidad, persona_fisica, a_paterno, a_materno, a_nombre,
                                        cmd.Parameters.Add("@tipo_tercero", OdbcType.Numeric).Value = objProveedor.tipo_tercero;
                                        cmd.Parameters.Add("@tipo_operacion", OdbcType.Numeric).Value = objProveedor.tipo_operacion;
                                        cmd.Parameters.Add("@curp", OdbcType.Char).Value = "";
                                        cmd.Parameters.Add("@bit_factoraje", OdbcType.Char).Value = objProveedor.bit_factoraje;
                                        cmd.Parameters.Add("@id_fiscal", OdbcType.Char).Value = objProveedor.id_fiscal;
                                        cmd.Parameters.Add("@nacionalidad", OdbcType.Char).Value = objProveedor.nacionalidad;
                                        cmd.Parameters.Add("@persona_fisica", OdbcType.Char).Value = objProveedor.persona_fisica;
                                        cmd.Parameters.Add("@a_paterno", OdbcType.VarChar).Value = objProveedor.a_paterno;
                                        cmd.Parameters.Add("@a_materno", OdbcType.VarChar).Value = objProveedor.a_materno;
                                        cmd.Parameters.Add("@a_nombre", OdbcType.VarChar).Value = objProveedor.a_nombre;

                                        //cncdirid,pyme,calle,tipo_soc,colonia,deleg,activo,tranfer_banco,cve_medio_confir,transfer_sant,
                                        cmd.Parameters.Add("@cncdirid", OdbcType.Numeric).Value = DBNull.Value;
                                        cmd.Parameters.Add("@pyme", OdbcType.Char).Value = DBNull.Value;
                                        cmd.Parameters.Add("@calle", OdbcType.VarChar).Value = objProveedor.calle;
                                        cmd.Parameters.Add("@tipo_soc", OdbcType.VarChar).Value = string.Empty;
                                        cmd.Parameters.Add("@colonia", OdbcType.VarChar).Value = objProveedor.colonia;
                                        cmd.Parameters.Add("@deleg", OdbcType.VarChar).Value = objProveedor.deleg;
                                        cmd.Parameters.Add("@activo", OdbcType.Numeric).Value = 0;
                                        cmd.Parameters.Add("@tranfer_banco", OdbcType.Numeric).Value = DBNull.Value;
                                        cmd.Parameters.Add("@cve_medio_confir", OdbcType.Numeric).Value = "0";
                                        cmd.Parameters.Add("@transfer_sant", OdbcType.Char).Value = "N";

                                        //lada,nombre_archivo,id_codigo_cat,num_empleado,num_usuario,num_nomina,cat_nomina,beneficiario,ciiu,base_iva,
                                        cmd.Parameters.Add("@lada", OdbcType.VarChar).Value = DBNull.Value;
                                        cmd.Parameters.Add("@nombre_archivo", OdbcType.VarChar).Value = "S";
                                        cmd.Parameters.Add("@id_codigo_cat", OdbcType.Char).Value = false;
                                        cmd.Parameters.Add("@num_empleado", OdbcType.Numeric).Value = 0;
                                        cmd.Parameters.Add("@num_usuario", OdbcType.Numeric).Value = DBNull.Value;
                                        cmd.Parameters.Add("@num_nomina", OdbcType.Char).Value = DBNull.Value;
                                        cmd.Parameters.Add("@cat_nomina", OdbcType.Numeric).Value = DBNull.Value;
                                        cmd.Parameters.Add("@beneficiario", OdbcType.Char).Value = DBNull.Value;
                                        cmd.Parameters.Add("@ciiu", OdbcType.Numeric).Value = DBNull.Value;
                                        cmd.Parameters.Add("@base_iva", OdbcType.Numeric).Value = DBNull.Value;

                                        //id_doc_identidad,camara_comercio,obliga_cfd,bit_autoretenedor,id_regimen,categoria_empleado,sincroniza_adm,bit_corp,convenio_nomina
                                        cmd.Parameters.Add("@id_doc_identidad", OdbcType.Numeric).Value = DBNull.Value;
                                        cmd.Parameters.Add("@camara_comercio", OdbcType.VarChar).Value = DBNull.Value;
                                        cmd.Parameters.Add("@obliga_cfd", OdbcType.Numeric).Value = objProveedor.obliga_cfd;
                                        cmd.Parameters.Add("@bit_autoretenedor", OdbcType.Char).Value = "N";
                                        cmd.Parameters.Add("@id_regimen", OdbcType.Numeric).Value = 0;
                                        cmd.Parameters.Add("@categoria_empleado", OdbcType.Char).Value = "N";
                                        cmd.Parameters.Add("@sincroniza_adm", OdbcType.Char).Value = "S";
                                        cmd.Parameters.Add("@bit_corp", OdbcType.Char).Value = "N";
                                        cmd.Parameters.Add("@convenio_nomina", OdbcType.Char).Value = "N";
                                        cmd.Parameters.Add("@numpro", OdbcType.Numeric).Value = objProveedor.numpro;
                                        cmd.Connection = transactionEk.Connection;
                                        cmd.Transaction = transactionEk;
                                        cmd.ExecuteNonQuery();
                                    }


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
                                            FK_idProv = objProveedor.id,
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

                                    //var infoProveedorSocioEditar = _context.tblCom_ProveedoresSocios.Where(e => e.registroActivo && e.FK_idProv == objProveedor.id).FirstOrDefault();
                                    //if (infoProveedorSocioEditar == null)
                                    //{
                                    //    tblCom_ProveedoresSocios infoSocios = new tblCom_ProveedoresSocios();
                                    //    infoSocios.FK_numpro = objProveedor.numpro;
                                    //    infoSocios.socios = objProveedor.socios;
                                    //    infoSocios.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                    //    infoSocios.fecha_creacion = DateTime.Now;
                                    //    infoSocios.registroActivo = true;

                                    //    _context.tblCom_ProveedoresSocios.Add(infoSocios);
                                    //    _context.SaveChanges();

                                    //}
                                    //else
                                    //{
                                    //    infoProveedorSocioEditar.socios = objProveedor.socios;
                                    //    infoProveedorSocioEditar.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                    //    infoProveedorSocioEditar.fecha_modificacion = DateTime.Now;

                                    //    _context.SaveChanges();
                                    //}
                                    #endregion
                                }
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

        private OdbcConnection ConexionEnkontrol()
        {
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
                        try
                        {
                            #region VALIDACIONES
                            if (id <= 0) { throw new Exception("Ocurrió un error al autorizar el alta de proveedor."); }
                            #endregion
                            var objAutorizador = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.idUsuario == vSesiones.sesionUsuarioDTO.id && e.PuedeAutorizar == true).ToList();
                            var objVobo = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.idUsuario == vSesiones.sesionUsuarioDTO.id && e.PuedeVobo == true).ToList();
                            var objProveedor = _context.tblCom_sp_proveedoresColombia.Where(e => e.registroActivo && e.id == id).FirstOrDefault();
                            var infoProveedorSocio = _context.tblCom_ProveedoresSocios.Where(e => e.registroActivo && e.FK_numpro == objProveedor.numpro).FirstOrDefault();

                            var quitarAlertas = _context.tblP_Alerta.Where(x => x.tipoAlerta == 2 && x.sistemaID == 4 && !x.visto && x.objID == (int)objProveedor.numpro && (x.obj == "AutorizacionProveedor" || x.obj == "AltaProveedor") && x.msj.Contains("Proveedor") && x.userRecibeID == vSesiones.sesionUsuarioDTO.id).ToList();
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

                            #region SE ENVIA CORREO AL AUTORIZANTE
                            List<string> lstCorreos = new List<string>();
                            var usuarioQueAplicoVobo = _context.tblP_Usuario.FirstOrDefault(x => x.id == objProveedor.id_usuarioVobo);
                            var usuarioQueAplicoAutorizacion = _context.tblP_Usuario.FirstOrDefault(x => x.id == objProveedor.id_usuarioAutorizo);
                            var usuarioCreoProveedor = _context.tblP_Usuario.FirstOrDefault(x => x.id == objProveedor.id_usuarioCreacion);

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
#if DEBUG
                            lstCorreos = new List<string>();
                            lstCorreos.Add("aaron.gracia@construplan.com.mx");
                            //lstCorreos.Add("martin.zayas@construplan.com.mx");
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
                                Buen dia ,<br>Se informa que el usuario {0} {1} {2} Autorizó el alta en la empresa Construplan Colombia del siguiente proveedor {3} {4} <br>
                                {5}
                                <br>
                                <H2>
                                {6}</H2>
                                <H2>
                                {7}</H2>
                                <br><br>Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>Construplan Colombia> Compras > Proveedores.<br>
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
                            GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, lstCorreos);
                            #endregion

                            #region SE REGISTRA EN SYBASE EKONTROL MEXICO

                            #region SP_PROVEEDORES
                            
                            var insertProv = @"
                                                INSERT INTO
                                                    dba.sp_proveedores
                                                        (
                                                        numpro,nomcorto,nombre,direccion,ciudad,cp,responsable,telefono1,telefono2,fax,
                                                        email,rfc,limcred,tmbase,descuento,condpago,moneda,cta_bancaria,tipo_prov,cancelado,
                                                        tipo_pago,inst_factoraje,cta_cheque,cve_banco,plaza_banco,tipo_cta,fecha_modifica_plazo_pago,usuario_modifica_plazo_pago,prov_exterior,filial,
                                                        tipo_tercero,tipo_operacion,curp,bit_factoraje,id_fiscal,nacionalidad,persona_fisica,a_paterno,a_materno,a_nombre,
                                                        cncdirid,pyme,calle,tipo_soc,colonia,deleg,activo,tranfer_banco,cve_medio_confir,transfer_sant,
                                                        lada,nombre_archivo,id_codigo_cat,num_empleado,num_usuario,num_nomina,cat_nomina,beneficiario,ciiu,base_iva,
                                                        id_doc_identidad,camara_comercio,obliga_cfd,bit_autoretenedor,id_regimen,categoria_empleado,sincroniza_adm,bit_corp,convenio_nomina
                                                        )
                                                    VALUES
                                                        (
                                                        ?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?
                                                        )
                                                ";

                            using (var cmd = new OdbcCommand(insertProv))
                            {
                                OdbcParameterCollection parameters = cmd.Parameters;
                                parameters.Clear();

                                //numpro, nomcorto, nombre, direccion, ciudad, cp, responsable, telefono1, telefono2, fax,
                                parameters.Add("@numpro", OdbcType.Numeric).Value = objProveedor.numpro;
                                parameters.Add("@nomcorto", OdbcType.Char).Value = objProveedor.nomcorto;
                                parameters.Add("@nombre", OdbcType.Char).Value = objProveedor.nombre;
                                parameters.Add("@direccion", OdbcType.Char).Value = objProveedor.direccion;
                                parameters.Add("@ciudad", OdbcType.Char).Value = objProveedor.ciudad;
                                parameters.Add("@cp", OdbcType.Char).Value = objProveedor.cp;
                                parameters.Add("@responsable", OdbcType.Char).Value = objProveedor.responsable;
                                parameters.Add("@telefono1", OdbcType.Char).Value = objProveedor.telefono1;
                                parameters.Add("@telefono2", OdbcType.Char).Value = objProveedor.telefono2;
                                parameters.Add("@fax", OdbcType.Char).Value = objProveedor.fax;

                                //  email, rfc, limcred, tmbase, descuento, condpago, moneda, cta_bancaria, tipo_prov, cancelado,
                                parameters.Add("@email", OdbcType.VarChar).Value = objProveedor.email;
                                parameters.Add("@rfc", OdbcType.Char).Value = objProveedor.rfc;
                                parameters.Add("@limcred", OdbcType.Numeric).Value = objProveedor.limcred;
                                parameters.Add("@tmbase", OdbcType.Numeric).Value = objProveedor.tmbase;
                                parameters.Add("@descuento", OdbcType.Numeric).Value = objProveedor.tmbase;
                                parameters.Add("@condpago", OdbcType.Numeric).Value = objProveedor.condpago;
                                parameters.Add("@moneda", OdbcType.Char).Value = objProveedor.moneda.Trim();
                                parameters.Add("@cta_bancaria", OdbcType.Char).Value = objProveedor.cta_bancaria;
                                parameters.Add("@tipo_prov", OdbcType.Numeric).Value = objProveedor.tipo_prov;
                                parameters.Add("@cancelado", OdbcType.Char).Value = objProveedor.cancelado;

                                //tipo_pago, inst_factoraje, cta_cheque, cve_banco, plaza_banco, tipo_cta, fecha_modifica_plazo_pago, usuario_modifica_plazo_pago, prov_exterior, filial,
                                parameters.Add("@tipo_pago", OdbcType.Char).Value = string.Empty;
                                parameters.Add("@inst_factoraje", OdbcType.Numeric).Value = 0;
                                parameters.Add("@cta_cheque", OdbcType.Char).Value = objProveedor.cta_cheque;
                                parameters.Add("@cve_banco", OdbcType.Char).Value = objProveedor.cve_banco;
                                parameters.Add("@plaza_banco", OdbcType.Char).Value = objProveedor.plaza_banco;
                                parameters.Add("@tipo_cta", OdbcType.Char).Value = objProveedor.tipo_cta;
                                parameters.Add("@fecha_modifica_plazo_pago", OdbcType.DateTime).Value = objProveedor.fecha_modifica_plazo_pago;
                                parameters.Add("@usuario_modifica_plazo_pago", OdbcType.Numeric).Value = objProveedor.usuario_modifica_plazo_pago;
                                parameters.Add("@prov_exterior", OdbcType.Char).Value = string.Empty;
                                parameters.Add("@filial", OdbcType.Char).Value = objProveedor.filial;

                                //tipo_tercero, tipo_operacion, curp, bit_factoraje, id_fiscal, nacionalidad, persona_fisica, a_paterno, a_materno, a_nombre,
                                parameters.Add("@tipo_tercero", OdbcType.Numeric).Value = objProveedor.tipo_tercero;
                                parameters.Add("@tipo_operacion", OdbcType.Numeric).Value = objProveedor.tipo_operacion;
                                parameters.Add("@curp", OdbcType.Char).Value = "";
                                parameters.Add("@bit_factoraje", OdbcType.Char).Value = objProveedor.bit_factoraje;
                                parameters.Add("@id_fiscal", OdbcType.Char).Value = objProveedor.id_fiscal;
                                parameters.Add("@nacionalidad", OdbcType.Char).Value = objProveedor.nacionalidad;
                                parameters.Add("@persona_fisica", OdbcType.Char).Value = objProveedor.persona_fisica;
                                parameters.Add("@a_paterno", OdbcType.VarChar).Value = objProveedor.a_paterno;
                                parameters.Add("@a_materno", OdbcType.VarChar).Value = objProveedor.a_materno;
                                parameters.Add("@a_nombre", OdbcType.VarChar).Value = objProveedor.a_nombre;

                                //cncdirid,pyme,calle,tipo_soc,colonia,deleg,activo,tranfer_banco,cve_medio_confir,transfer_sant,
                                parameters.Add("@cncdirid", OdbcType.Numeric).Value = DBNull.Value;
                                parameters.Add("@pyme", OdbcType.Char).Value = DBNull.Value;
                                parameters.Add("@calle", OdbcType.VarChar).Value = objProveedor.calle;
                                parameters.Add("@tipo_soc", OdbcType.VarChar).Value = string.Empty;
                                parameters.Add("@colonia", OdbcType.VarChar).Value = objProveedor.colonia;
                                parameters.Add("@deleg", OdbcType.VarChar).Value = objProveedor.deleg;
                                parameters.Add("@activo", OdbcType.Numeric).Value = 0;
                                parameters.Add("@tranfer_banco", OdbcType.Numeric).Value = DBNull.Value;
                                parameters.Add("@cve_medio_confir", OdbcType.Numeric).Value = "0";
                                parameters.Add("@transfer_sant", OdbcType.Char).Value = "N";

                                //lada,nombre_archivo,id_codigo_cat,num_empleado,num_usuario,num_nomina,cat_nomina,beneficiario,ciiu,base_iva,
                                parameters.Add("@lada", OdbcType.VarChar).Value = DBNull.Value;
                                parameters.Add("@nombre_archivo", OdbcType.VarChar).Value = "S";
                                parameters.Add("@id_codigo_cat", OdbcType.Char).Value = false;
                                parameters.Add("@num_empleado", OdbcType.Numeric).Value = 0;
                                parameters.Add("@num_usuario", OdbcType.Numeric).Value = DBNull.Value;
                                parameters.Add("@num_nomina", OdbcType.Char).Value = DBNull.Value;
                                parameters.Add("@cat_nomina", OdbcType.Numeric).Value = DBNull.Value;
                                parameters.Add("@beneficiario", OdbcType.Char).Value = DBNull.Value;
                                parameters.Add("@ciiu", OdbcType.Numeric).Value = DBNull.Value;
                                parameters.Add("@base_iva", OdbcType.Numeric).Value = DBNull.Value;

                                //id_doc_identidad,camara_comercio,obliga_cfd,bit_autoretenedor,id_regimen,categoria_empleado,sincroniza_adm,bit_corp,convenio_nomina
                                parameters.Add("@id_doc_identidad", OdbcType.Numeric).Value = 31;
                                parameters.Add("@camara_comercio", OdbcType.VarChar).Value = DBNull.Value;
                                parameters.Add("@obliga_cfd", OdbcType.Numeric).Value = objProveedor.obliga_cfd;
                                parameters.Add("@bit_autoretenedor", OdbcType.Char).Value = "N";
                                parameters.Add("@id_regimen", OdbcType.Numeric).Value = 0;
                                parameters.Add("@categoria_empleado", OdbcType.Char).Value = "N";
                                parameters.Add("@sincroniza_adm", OdbcType.Char).Value = "S";
                                parameters.Add("@bit_corp", OdbcType.Char).Value = "N";
                                parameters.Add("@convenio_nomina", OdbcType.Char).Value = "N";

                                cmd.Connection = transactionEk.Connection;
                                cmd.Transaction = transactionEk;
                                cmd.ExecuteNonQuery();
                            }
                            #endregion

                            #region SP_PROVEEDORES_TIPOPROV
                            var insertProvTipoProv = @"
                                                INSERT INTO dba.sp_provedores_tipoprov (numpro, tipo_prov)
                                                VALUES (?, ?)";

                            using (var cmd = new OdbcCommand(insertProvTipoProv))
                            {
                                OdbcParameterCollection parameters = cmd.Parameters;
                                parameters.Clear();

                                //numpro, nomcorto, nombre, direccion, ciudad, cp, responsable, telefono1, telefono2, fax,
                                parameters.Add("@numpro", OdbcType.Numeric).Value = objProveedor.numpro;
                                parameters.Add("@tipo_prov", OdbcType.Char).Value = objProveedor.tipo_prov;

                                cmd.Connection = transactionEk.Connection;
                                cmd.Transaction = transactionEk;
                                cmd.ExecuteNonQuery();
                            }
                            #endregion

                            #region SP_TM_NUMPRO
                            var insertProvTM = @"
                                                INSERT INTO dba.sp_tm_numpro (numpro, tm)
                                                VALUES (?, ?)";

                            using (var cmd = new OdbcCommand(insertProvTM))
                            {
                                OdbcParameterCollection parameters = cmd.Parameters;
                                parameters.Clear();

                                //numpro, nomcorto, nombre, direccion, ciudad, cp, responsable, telefono1, telefono2, fax,
                                parameters.Add("@numpro", OdbcType.Numeric).Value = objProveedor.numpro;
                                parameters.Add("@tm", OdbcType.Char).Value = objProveedor.tmbase;

                                cmd.Connection = transactionEk.Connection;
                                cmd.Transaction = transactionEk;
                                cmd.ExecuteNonQuery();
                            }
                            #endregion

                            #endregion

                            transactionEk.Commit();
                            dbContextTransaction.Commit();
                            resultado.Add(SUCCESS, true);
                        }
                        catch (Exception e)
                        {

                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, e.Message);
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

                    #region SE INDICA QUE EL ALTA DE PROVEEDOR SE ENCUENTRA EN ESTATUS NOTIFICADO
                    var objProveedor = _context.tblCom_sp_proveedoresColombia.Where(e => e.registroActivo && e.id == id && !e.vobo).FirstOrDefault();
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
                        var alertasRelacionadasAlVoBo = _context.tblP_Alerta.Where(x => x.tipoAlerta == 2 && x.sistemaID == 4 && x.objID == objProveedor.numpro && (x.obj == "AutorizacionProveedor" || x.obj == "AltaProveedor") && x.msj.Contains("Proveedor") && !x.visto).ToList();
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
#if DEBUG
                            lstCorreos = new List<string>();
                            //lstCorreos.Add("aaron.gracia@construplan.com.mx");
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
                                Buen dia ,<br>Se informa que el usuario {0} {1} {2} confirmó el alta en la empresa Construplan Colombia del siguiente proveedor {3} {4} y se encuentra listo para ser autorizado <br>
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
                            //GlobalUtils.sendEmail(subject, body, lstCorreos);
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
                            objNuevaAlerta.url = "/Enkontrol/AltaProveedor/AltaProveedorColombia?numpro=" +objProveedor.numpro;
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
            var lstProveedores = _context.tblCom_sp_proveedoresColombia.Where(e => e.registroActivo && e.id == id).ToList();



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
            foreach (var item in lstProveedores)
            {

                var objAutorizador = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.idUsuario == item.id_usuarioAutorizo).FirstOrDefault();
                var objVobo = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.idUsuario == item.id_usuarioVobo).FirstOrDefault();
                
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
            //html += "</div>";

            return html;
        }
        public Dictionary<string, object> eliminarProveedor(int id)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var objProveedor = _context.tblCom_sp_proveedoresColombia.FirstOrDefault(x => x.registroActivo && x.id == id);

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
                var lstArchivos = _context.tblCom_ArchivosAdjuntosProveedores.Where(x => x.registroActivo && x.FK_numpro == idArchivo).ToList();

                resultado.Add("data", lstArchivos);
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
                tblCom_ArchivosAdjuntosProveedores objArchivoAdjunto = _context.tblCom_ArchivosAdjuntosProveedores.Where(w => w.FK_numpro == idArchivo && w.registroActivo).FirstOrDefault();
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
        public Dictionary<string, object> EliminarArchivoAdjunto(int idArchivo)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (idArchivo <= 0) { throw new Exception("Ocurrió un error al eliminar el archivo."); }
                #endregion

                #region SE ELIMINA EL ARCHIVO
                tblCom_ArchivosAdjuntosProveedores objEliminar = _context.tblCom_ArchivosAdjuntosProveedores.Where(w => w.id == idArchivo && w.registroActivo).FirstOrDefault();
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

                var conexion = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(EmpresaEnum.Colombia);

                var odbc = new OdbcConsultaDTO();
                odbc.consulta = "SELECT numpro FROM DBA.sp_proveedores ORDER BY numpro";

                var ProvEkontrol = _contextEnkontrol.Select<NumProDTO>(conexion, odbc);

                List<int> lstObjsProveedores = ProvEkontrol.OrderBy(e => e.numpro).Select(e => e.numpro).ToList();
                numProvEKCP = findMissing(lstObjsProveedores);

                if (numProvEKCP == -1)
                {
                    throw new Exception("Ocurrio algo mal obteniendo el ultimo proveedor, favor de comunicarse con el departamento de TI");
                }

                #endregion

                #region SP
                int valSPCP = 0;
                using (var ctx = new MainContext(EmpresaEnum.Colombia))
                {
                    var objUltimoProvSP = ctx.tblCom_sp_proveedoresColombia
                        .Where(e => e.registroActivo)
                        .ToList().Select(e => Convert.ToInt32(e.numpro)).OrderBy(e => e).ToList();

                    int numProvSP = findMissing(objUltimoProvSP);

                    if (objUltimoProvSP.Count() > 0)
                    {
                        valSPCP = numProvSP == -1 ? (objUltimoProvSP.Max() + 1) : numProvSP;
                        lstObjsProveedores.Add(objUltimoProvSP.Max());
                        lstObjsProveedores = lstObjsProveedores.Distinct().OrderBy(e => e).ToList();
                    }
                }
                #endregion

                #endregion

                if (valSPCP > numProvEKCP)
                {
                    sigNumProveedor = valSPCP;
                    
                }
                else
                {
                    numProvEKCP = findMissing(lstObjsProveedores);
                    sigNumProveedor = numProvEKCP.Value;

                }
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
                    .Where("SELECT ciudad as Value, desc_ciudad as Text FROM dba.ciudades").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
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
                    .Where("SELECT tipo_prov as Value, descripcion as Text FROM dba.sp_tipo_prov").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
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
                    .Where("SELECT id_tercero as Value, desc_tercero as Text FROM dba.sp_tercero").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
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
                    .Where("SELECT id_operacion as Value, desc_operacion as Text FROM dba.sp_operacion").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                lst.AsParallel().Where(w => string.IsNullOrEmpty(w.Prefijo)).ForAll(x => x.Prefijo = string.Empty);
                return lst.ToList();
            }
            catch (Exception) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoPagoTerceroTrans()
        {
            try
            {
                var lst = (List<Core.DTO.Principal.Generales.ComboDTO>)_contextEnkontrol
                    .Where("SELECT numero as Value, descripcion as Text FROM dba.so_libre_abordo").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                lst.AsParallel().Where(w => string.IsNullOrEmpty(w.Prefijo)).ForAll(x => x.Prefijo = string.Empty);
                return lst.ToList();
            }
            catch (Exception) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoMovBase()
        {
            try
            {
                var lst = (List<Core.DTO.Principal.Generales.ComboDTO>)_contextEnkontrol
                    .Where("SELECT tm as Value, descripcion as Text FROM dba.sp_tm").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
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
                    .Where("SELECT clave as Value, moneda as Text FROM dba.moneda").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
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
                     .Where("SELECT banco as Value, descripcion as Text FROM dba.sb_bancos").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
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
