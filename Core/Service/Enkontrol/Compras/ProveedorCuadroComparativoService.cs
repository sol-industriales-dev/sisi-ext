using Core.DAO.Enkontrol.Compras;
using Core.DTO.Enkontrol.OrdenCompra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.Enkontrol.Compras
{
    public class ProveedorCuadroComparativoService : IProveedorCuadroComparativoDAO
    {
        #region CONSTRUCTOR
        public IProveedorCuadroComparativoDAO _iProveedorCPDAO;
        public IProveedorCuadroComparativoDAO iProveedorCPDAO
        {
            get { return _iProveedorCPDAO; }
            set { _iProveedorCPDAO = value; }
        }
        public ProveedorCuadroComparativoService(IProveedorCuadroComparativoDAO _iProveedorCPDAO)
        {
            this.iProveedorCPDAO = _iProveedorCPDAO;
        }
        #endregion

        public Dictionary<string, object> VerificarProveedorRelHash(string hash)
        {
            return _iProveedorCPDAO.VerificarProveedorRelHash(hash);
        }

        public Dictionary<string, object> GetDatosProveedor(string hash)
        {
            return _iProveedorCPDAO.GetDatosProveedor(hash);
        }

        public Dictionary<string, object> GuardarCuadroComparativo(CuadroComparativoDTO cuadro, HttpPostedFileBase archivo)
        {
            return _iProveedorCPDAO.GuardarCuadroComparativo(cuadro, archivo);
        }
    }
}