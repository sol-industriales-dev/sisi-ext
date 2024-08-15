using Core.DAO.Enkontrol.General.Proveedor;
using Core.DTO.Enkontrol.Tablas.Proveedor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Enkontrol.General.Proveedor
{
    public class ProveedorService : IProveedorDAO
    {
        private IProveedorDAO _proveedorDAO;

        public IProveedorDAO ProveedorDAO
        {
            get { return _proveedorDAO; }
            set { _proveedorDAO = value; }
        }

        public ProveedorService(IProveedorDAO proveedor)
        {
            this.ProveedorDAO = proveedor;
        }

        public List<sp_proveedoresDTO> GetProveedores()
        {
            return this.ProveedorDAO.GetProveedores();
        }
    }
}
