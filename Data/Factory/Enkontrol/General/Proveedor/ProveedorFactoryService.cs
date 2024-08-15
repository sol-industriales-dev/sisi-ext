using Core.DAO.Enkontrol.General.Proveedor;
using Core.Service.Enkontrol.General.Proveedor;
using Data.DAO.Enkontrol.General.Proveedor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Enkontrol.General.Proveedor
{
    public class ProveedorFactoryService
    {
        public IProveedorDAO getProveedorService()
        {
            return new ProveedorService(new ProveedorDAO());
        }
    }
}
