using Core.DAO.Facturacion.Prefacturacion;
using Core.DTO.Facturacion;
using Core.Entity.Facturacion.Prefacturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.Facturacion.Prefacturacion
{
    public class RepPrefacturacionService : IRepPrefacturacionDAO
    {
        private IRepPrefacturacionDAO f_IRepPrefacturacionDAO;

        public IRepPrefacturacionDAO Prefactura
        {
            get { return f_IRepPrefacturacionDAO; }
            set { f_IRepPrefacturacionDAO = value; }
        }

        public RepPrefacturacionService(IRepPrefacturacionDAO prefactura)
        {
            this.f_IRepPrefacturacionDAO = prefactura;
        }

        public tblF_RepPrefactura saveRepPrefactura(tblF_RepPrefactura obj)
        {
            return Prefactura.saveRepPrefactura(obj);
        }

        public List<tblF_RepPrefactura> getPrefactura(DateTime inicio, DateTime fin, string cc)
        {
            return Prefactura.getPrefactura(inicio, fin, cc);
        }
        public tblF_RepPrefactura ActualizaEstatus(int id, int estatus)
        {
            return Prefactura.ActualizaEstatus(id, estatus);
        }

        public tblF_RepPrefactura ActualizaEstatusCLONE(int id, int estatus)
        {
            return Prefactura.ActualizaEstatusCLONE(id, estatus);
        }

        public List<tblF_RepPrefactura> getPrefactura(int id)
        {
            return Prefactura.getPrefactura(id);
        }

        public List<tblF_RepPrefactura> getPrefactura()
        {
            return Prefactura.getPrefactura();
        }

        public tblF_RepPrefactura getUltimaPrefacturaCliente(string nombre, string moneda, string cc)
        {
            return Prefactura.getUltimaPrefacturaCliente(nombre, moneda, cc);
        }
        public List<ComboDTO> FillComboClienteNombre(string term)
        {
            return Prefactura.FillComboClienteNombre(term);
        }
        public ClienteDTO objCliente(int cliente)
        {
            return Prefactura.objCliente(cliente);
        }
    }
}
