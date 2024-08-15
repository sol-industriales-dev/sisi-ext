using Core.DAO.Enkontrol.Compras;
using Core.Service.Enkontrol.Compras;
using Data.DAO.Enkontrol.Compras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Enkontrol.Compras
{
    public class GestionProveedorFactoryService
    {
        public IGestionProveedorDAO GetGestionProveedorFS()
        {
            return new GestionProveedorService(new GestionProveedorDAO());
        }
    }
}
