using Core.DAO.Facturacion.Enkontrol;
using Core.DAO.Facturacion.Prefacturacion;
using Core.DTO;
using Core.DTO.Facturacion;
using Core.DTO.Facturacion.Prefactura.Insumos;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contabilidad.Facturas;
using Core.Entity.Facturacion.Prefacturacion;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Facturacion.Enkontrol;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Facturacion.Prefacturacion
{
    public class PrefacturacionDAO : GenericDAO<tblF_CapPrefactura>, IPrefacturacionDAO
    {
        IFacturasSPDAO facturasSPInterfaz = new FacturasSPFactoryService().getFacturasSPFactoryService();
        Dictionary<string, object> resultado = new Dictionary<string, object>();

        public tblF_CapPrefactura savePrefactura(tblF_CapPrefactura obj)
        {
            try
            {
                if (obj.id == 0)
                {
                    SaveEntity(obj, (int)BitacoraEnum.PREFACTURA);
                }
                else
                {
                    Update(obj, obj.id, (int)BitacoraEnum.PREFACTURA);
                }

            }
            catch (Exception e)
            {
                return new tblF_CapPrefactura();
            }
            return obj;
        }

        public List<tblF_CapPrefactura> getPrefactura(int id)
        {
            var lstResutl = _context.tblF_CapPrefactura
                .Where(x => x.idRepPrefactura == id);
            return lstResutl.ToList();
        }

        public List<tblF_RepPrefactura> getPrefactura(DateTime inicio, DateTime fin, string cc)
        {
            var lstResutl = _context.tblF_RepPrefactura
                .Where(x => x.Fecha >= inicio && 
                    x.Fecha <= fin &&
                    string.IsNullOrEmpty(cc) ? true : x.CC.Equals(cc)).ToList();
            return lstResutl;
        }

        public List<tipoInsumoDTO> getlstTipoInsumo()
        {
            string consulta = "SELECT tipo_insumo, descripcion, cve_tipo FROM \"DBA\".\"tipos_insumo\"";
            var res = (List<tipoInsumoDTO>)_contextEnkontrol.Where(consulta).ToObject<List<tipoInsumoDTO>>();
            return res.ToList();
        }

        public List<tblF_CatImporte> CboConceptoImporte()
        {
            return _context.tblF_CatImporte.ToList();
        }

        public InsumosDTO getObjInsumo(DateTime fecha, string cc, int insumo)
        {
            string consulta = "SELECT TOP 1 i.insumo, i.descripcion, i.unidad, i.tipo, i.grupo, p.PRECIO_INSUMO, p.FE_PRECIO, p.OBRA "
                              +" FROM \"DBA\".\"px_bit_SU_PRECIOS_INSUMO\" p "
                              +" INNER JOIN \"DBA\".\"px_bit_insumos\" i ON  i.insumo = p.INSUMO "
                              +" where i.insumo = " + insumo + " and p.FE_PRECIO = '" + fecha.ToString("yyyyMMdd") + "' and p.OBRA = '" + cc + "'";
            var res = (List<InsumosDTO>)_contextEnkontrol.Where(consulta).ToObject<List<InsumosDTO>>();
            return res.FirstOrDefault();
        }

        public List<OrdenCompraDTO> getlstOrdenCompra(DateTime inicio, DateTime fin, string cc)
        {
            string consulta = "SELECT numero, partida, insumo, fecha_entrega, cantidad, precio, importe, (select top 1 descripcion from px_bit_insumos where insumo = A.insumo )  FROM px_bit_so_orden_compra_det A "
                               + "WHERE fecha_entrega >= '" + inicio.ToString("yyyyMMdd") + "' AND fecha_entrega <= '" + fin.ToString("yyyyMMdd") + "' AND cc = '" + cc + "' group by numero, partida, insumo, fecha_entrega, cantidad, precio, importe ,A.insumo;";
            try
            {
                var res = (List<OrdenCompraDTO>)_contextEnkontrol.Where(consulta).ToObject<List<OrdenCompraDTO>>();
                return res.ToList();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<ComboDTO> FillComboClienteNombre(string term)
        {
            var lstCliente = new List<ComboDTO>();
            try
            {
                var getCatEmpleado = "SELECT TOP 10 numcte AS Value, nombre AS Text FROM sx_clientes WHERE nombre LIKE '%" + term.Replace(" ", "") + "%'";
                var resultado = (IList<ComboDTO>)ContextEnKontrolNomina.Where(getCatEmpleado).ToObject<IList<ComboDTO>>();
                lstCliente.AddRange(resultado);
            }
            catch
            {
                return lstCliente;
            }
            return lstCliente;
        }
        public List<ComboDTO> FillComboUsocfdi()
        {
            var lst = _contextEnkontrol.Select<usocfdiDTO>(EnkontrolAmbienteEnum.Prod, string.Format("SELECT * FROM {0}\"Stf_usocfdi\"", "\"DBA\"."));
            return lst.Select(s => new ComboDTO()
            {
                Text = string.Format("{0} - {1}", s.UsoCFDI_sat, s.Descripcion),
                Value = s.UsoCFDI_sat,
                Prefijo = JsonUtils.Json(s)
            }).ToList();
        }

        public List<tblF_CapImporte> getTotales(int id)
        {

            return _context.tblF_CapImporte.Where(x=>x.idReporte == id).ToList();
        }

        #region FACTURAS EK
        public Dictionary<string,object> FillComboMetodoPagoSat()
        {
            resultado.Clear();

            try
            {
                var result = _context.tblF_EK_MetodoPagoSat.Where(e => e.esActivo == true).Select(e => new ComboDTO 
                { 
                    Value = e.id.ToString(),
                    Text = e.clave + " - " + e.descripcion 
                }).ToList();

                resultado.Add(ITEMS, result);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string,object> FillComboRegimenFiscal()
        {
            resultado.Clear();

            try
            {
                var result = _context.tblF_EK_RegimenFiscal.Where(e => e.esActivo == true).Select(e => new ComboDTO 
                { 
                    Value = e.id.ToString(),
                    Text = e.descripcion,

                }).ToList();

                resultado.Add(ITEMS, result);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }


            return resultado;
        }

        public Dictionary<string, object> FillComboTM()
        {
            resultado.Clear();

            try
            {
                var result = _context.tblF_EK_TM.Where(e => e.esActivo == true).Select(e => new ComboDTO
                {
                    Value = e.id.ToString(),
                    Text = e.descripcion,

                }).ToList();

                resultado.Add(ITEMS, result);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }


            return resultado;
        }

        public Dictionary<string, object> FillComboTipoFlete()
        {
            resultado.Clear();

            try
            {
                var result = _context.tblF_EK_TipoFlete.Where(e => e.esActivo == true).Select(e => new ComboDTO
                {
                    Value = e.clave.ToString(),
                    Text = e.descripcion,

                }).ToList();

                resultado.Add(ITEMS, result);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }


            return resultado;
        }
        public Dictionary<string, object> FillComboCondEntrega()
        {
            resultado.Clear();

            try
            {
                var result = _context.tblF_EK_CondEntrega.Where(e => e.esActivo == true).Select(e => new ComboDTO
                {
                    Value = e.clave.ToString(),
                    Text = e.descripcion,

                }).ToList();

                resultado.Add(ITEMS, result);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }


            return resultado;
        }

        public Dictionary<string, object> FillComboTipoPedido()
        {
            resultado.Clear();

            try
            {
                var result = _context.tblF_EK_TipoPedido.Where(e => e.esActivo == true).Select(e => new ComboDTO
                {
                    Value = e.clave.ToString(),
                    Text = e.descripcion,

                }).ToList();

                resultado.Add(ITEMS, result);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }


            return resultado;
        }

        public Dictionary<string,object> GetFormaPagoSat(string claveSat)
        {
            resultado.Clear();

            try
            {
                var result = _context.tblF_EK_FormaPagoSat.FirstOrDefault(e => e.clave_sat == claveSat);

                resultado.Add(ITEMS, result);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e )
            {

                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> FillComboTipoFactura()
        {
            resultado.Clear();

            try
            {
                var result = _context.tblF_EK_TipoFactura.Where(e => e.esActivo).Select(e => new ComboDTO
                {
                    Value = e.id.ToString(),
                    Text = e.descripcion,

                }).ToList();

                resultado.Add(ITEMS, result);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {

                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> FillComboSerie()
        {
            resultado.Clear();

            try
            {
                var result = _context.tblF_EK_Serie.Where(e => e.esActivo).Select(e => new ComboDTO
                {
                    Value = e.serie.ToString(),
                    Text = e.serie,

                }).ToList();

                resultado.Add(ITEMS, result);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {

                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string,object> FIllComboInsumos()
        {
            resultado.Clear();

            try
            {

                var result = _context.tblF_EK_Insumos.Where(e => e.esActivo).Select(e => new ComboDTO
                {
                    Value = e.insumo.ToString(),
                    Text = e.descripcion,

                }).ToList();

                resultado.Add(ITEMS, result);
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

        #region CAT INSUMOS

        public Dictionary<string, object> GetInsumosEK(string idInsumoSAT)
        {
            resultado.Clear();

            try
            {
                var lstRel = _context.tblF_EK_InsumosRel.Where(e => e.esActivo && e.insumoSAT == idInsumoSAT).Select(e => e.insumoEK).ToList();
                var lstInsumos = _context.tblF_EK_Insumos.Where(e => e.esActivo && lstRel.Contains(e.insumo)).ToList();

                resultado.Add(ITEMS, lstInsumos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {

                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
                
            }

            return resultado;
        }

        public Dictionary<string, object> GetInsumosSAT(tblF_EK_InsumosSAT objFiltro)
        {
            resultado.Clear();

            try
            {
                var lstInsumos = _context.tblF_EK_InsumosSAT.Where(e => e.esActivo).ToList();

                resultado.Add(ITEMS, lstInsumos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {

                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);

            }

            return resultado;
        }

        public Dictionary<string, object> CrearEditarInsumos(InsumosSATDAO objInsumo, List<string> lstRel)
        {
            resultado.Clear();

            using (var dbTransac = _context.Database.BeginTransaction())
            {
                try
                {
                    var lstInsumosEK = new List<string>();

                    if (objInsumo.id > 0)
                    {
                        #region EDITAR
                        var objCEInsumo = _context.tblF_EK_InsumosSAT.FirstOrDefault(e => e.esActivo && e.clave == objInsumo.clave);

                        objCEInsumo.clave = objInsumo.clave;
                        objCEInsumo.descripcion = objInsumo.descripcion;
                        objCEInsumo.unidad = objInsumo.unidad;

                        _context.SaveChanges();

                        lstInsumosEK = _context.tblF_EK_InsumosRel.Where(e => e.insumoSAT == objCEInsumo.clave).Select(e => e.insumoEK).ToList();

                        
                        #endregion
                    }
                    else
                    {
                        #region CREAR
                        _context.tblF_EK_InsumosSAT.Add(new tblF_EK_InsumosSAT 
                        { 
                            clave = objInsumo.clave,
                            descripcion = objInsumo.descripcion,
                            unidad = objInsumo.unidad,
                            fechaCreacion = DateTime.Now,
                            fechaModificacion = DateTime.Now,
                            idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id,
                            idUsuarioModificacion= vSesiones.sesionUsuarioDTO.id,
                            esActivo = true,
                        });

                        _context.SaveChanges();

                        #endregion
                    }

                    foreach (var item in lstRel)
                    {
                        if (!lstInsumosEK.Contains(item))
                        {
                            _context.tblF_EK_InsumosRel.Add(new tblF_EK_InsumosRel
                            {
                                insumoEK = item,
                                insumoSAT = objInsumo.clave,
                                idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id,
                                idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id,
                                fechaCreacion = DateTime.Now,
                                fechaModificacion = DateTime.Now,
                                esActivo = true,
                            });

                            _context.SaveChanges();
                        }
                    }

                    dbTransac.Commit();

                    resultado.Add(ITEMS, null);
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbTransac.Rollback();

                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarInsumo(int idInsumo)
        {
            resultado.Clear();

            using (var dbTransac = _context.Database.BeginTransaction())
            {
                try
                {
                    var objInsumo = _context.tblF_EK_InsumosSAT.FirstOrDefault(e => e.esActivo && e.id == idInsumo);

                    objInsumo.esActivo = false;

                    _context.SaveChanges();

                    dbTransac.Commit();

                    resultado.Add(ITEMS, null);
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbTransac.Rollback();

                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarRelInsumo(string idInsumoSAT, string idInsumoEK)
        {
            resultado.Clear();

            using (var dbTransac = _context.Database.BeginTransaction())
            {
                try
                {
                    var objRelInsumo = _context.tblF_EK_InsumosRel.FirstOrDefault(e => e.esActivo && e.insumoSAT == idInsumoSAT && e.insumoEK == idInsumoEK);

                    objRelInsumo.esActivo = false;

                    _context.SaveChanges();

                    dbTransac.Commit();

                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbTransac.Rollback();

                    resultado.Add(SUCCESS, true);
                }
            }
            
            return resultado;
        }

        public List<InsumosSATDAO> GetAutoCompleteInsumosDesc(string term)
        {
            var lstInusmos = new List<InsumosSATDAO>();
            try
            {
                lstInusmos = _context.Select<tblF_EK_Insumos>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT TOP 10 * FROM tblF_EK_Insumos as t1 
                                    WHERE t1.esActivo = 1 AND t1.insumo IN (SELECT insumoEk FROM tblF_EK_InsumosRel as t2 WHERE t2.esActivo = 1) AND t1.descripcion LIKE @termCompletar",
                    parametros = new { termCompletar = "%"+term+"%" }
                }).Select(e => new InsumosSATDAO
                {
                    value = e.insumo + " - " + e.descripcion,
                    descripcion = e.descripcion,
                    unidad = e.unidad,
                    clave = e.insumo,
                }).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }

            return lstInusmos;
        }

        public List<InsumosSATDAO> GetAutoCompleteInsumos(string term)
        {
            var lstInusmos = new List<InsumosSATDAO>();
            try
            {
                lstInusmos = _context.Select<tblF_EK_Insumos>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT TOP 10 * FROM tblF_EK_Insumos as t1 
                                    WHERE t1.esActivo = 1 AND t1.insumo IN (SELECT insumoEk FROM tblF_EK_InsumosRel as t2 WHERE t2.esActivo = 1) AND t1.insumo LIKE @termCompletar",
                    parametros = new { termCompletar = "%" + term + "%" }
                }).Select(e => new InsumosSATDAO
                {
                    value = e.insumo + " - " + e.descripcion,
                    descripcion = e.descripcion,
                    unidad = e.unidad,
                    clave = e.insumo,
                }).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }

            return lstInusmos;
        }
        #endregion
    }
}
