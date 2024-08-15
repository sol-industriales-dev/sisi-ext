using Core.DAO.Facturacion.Enkontrol;
using Core.DAO.Facturacion.Prefacturacion;
using Core.DTO.Facturacion;
using Core.Entity.Facturacion.Prefacturacion;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Facturacion.Enkontrol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Facturacion.Prefacturacion
{
    public class RepFacturacionDAO : GenericDAO<tblF_RepPrefactura>, IRepPrefacturacionDAO
    {
        #region FS FACTURAS SP/EK
        IFacturasSPDAO facturasSPInterfaz = new FacturasSPFactoryService().getFacturasSPFactoryService();
        IFacturasSPDAO facturasEKInterfaz = new FacturasSPFactoryService().getFacturasEKFactoryService();

        #endregion

        public tblF_RepPrefactura saveRepPrefactura(tblF_RepPrefactura obj)
        {
            try
            {
                if (obj.id == 0)
                {
                    SaveEntity(obj, (int)BitacoraEnum.REPPREFACTURA);
                }
                else
                {
                    Update(obj, obj.id, (int)BitacoraEnum.REPPREFACTURA);
                }

            }
            catch (Exception e)
            {
                return new tblF_RepPrefactura();
            }
            return obj;
        }

        public tblF_RepPrefactura ActualizaEstatus(int id, int estatus)
        {
            var obj = _context.tblF_RepPrefactura
                .Where(x => x.id == id)
                .FirstOrDefault();
            try
            {
                obj.Estado = estatus;
                Update(obj, id, (int)BitacoraEnum.REPPREFACTURA);
            }
            catch (Exception)
            {
                return new tblF_RepPrefactura();
            }
            return obj;
        }
        public tblF_RepPrefactura ActualizaEstatusCLONE(int id, int estatus)
        {
            var obj = _context.tblF_RepPrefactura
                .Where(x => x.id == id)
                .FirstOrDefault();
            try
            {
                //SI EL ESTATUS ES IGUAL ACEPTADO (2) GUARDAR EN ENKONTROL
                if (estatus == 2)
                {
                    List<tblF_CapPrefactura> lstCapPrefactura = _context.tblF_CapPrefactura.Where(e => e.idRepPrefactura == id).ToList();
                    List<tblF_CapImporte> lstCapImporte = _context.tblF_CapImporte.Where(e => e.idReporte == id).ToList();

                    facturasEKInterfaz.GuardarPrefactura(obj, lstCapPrefactura, lstCapImporte);
                }

                obj.Estado = estatus;
                Update(obj, id, (int)BitacoraEnum.REPPREFACTURA);

            }
            catch (Exception)
            {
                return new tblF_RepPrefactura();
            }
            return obj;
        }

        public List<tblF_RepPrefactura> getPrefactura(DateTime inicio, DateTime fin, string cc)
        {
            var lstResutl = _context.tblF_RepPrefactura
                .Where(x => x.Fecha >= inicio && x.Fecha <= fin && x.CC.Equals(cc));
            return lstResutl.ToList();
        }

        public List<tblF_RepPrefactura> getPrefactura(int id)
        {
            var lstResutl = _context.tblF_RepPrefactura
                .Where(x => x.id == id);
            return lstResutl.ToList();
        }

        public List<tblF_RepPrefactura> getPrefactura()
        {
            var lstResutl = _context.tblF_RepPrefactura;
            return lstResutl.ToList();
        }

        public tblF_RepPrefactura getUltimaPrefacturaCliente(string nombre, string moneda, string cc)
        {
            var lstResutl = _context.tblF_RepPrefactura.OrderByDescending(x => x.Fecha).Where(x => x.Nombre.ToUpper().Equals(nombre.ToUpper()) && x.TipoMoneda == moneda && x.CC.Equals(cc));
            return lstResutl.FirstOrDefault();
        }

        public List<ComboDTO> FillComboClienteNombre(string term)
        {
            var lstCliente = new List<ComboDTO>();

            try
            {
                var getCatEmpleado = "SELECT TOP 10 numcte AS Value, (STR(numcte) + ' - ' + nombre) AS Text FROM sx_clientes WHERE Text LIKE '%" + term.Replace(" ", "") + "%'";
                var resultado = (IList<ComboDTO>)_contextEnkontrol.Where(getCatEmpleado).ToObject<IList<ComboDTO>>();
                foreach (var item in resultado)
                {
                    lstCliente.Add(item);

                }
            }
            catch
            {
                return lstCliente;
            }

            return lstCliente;
        }

        public ClienteDTO objCliente(int cliente)
        {
            string consulta = "SELECT * FROM sx_clientes where numcte = " + cliente;
            var res = (List<ClienteDTO>)_contextEnkontrol.Where(consulta).ToObject<List<ClienteDTO>>();
            return res.FirstOrDefault();
        }
    }
}
