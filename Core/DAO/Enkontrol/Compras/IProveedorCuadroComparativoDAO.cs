using Core.DTO.Enkontrol.OrdenCompra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.Enkontrol.Compras
{
    public interface IProveedorCuadroComparativoDAO
    {
        Dictionary<string, object> VerificarProveedorRelHash(string hash);

        Dictionary<string, object> GetDatosProveedor(string hash);

        Dictionary<string, object> GuardarCuadroComparativo(CuadroComparativoDTO cuadro, HttpPostedFileBase archivo);
    }
}