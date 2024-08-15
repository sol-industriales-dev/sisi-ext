using Core.DAO.Enkontrol.General.Proveedor;
using Core.DTO;
using Core.DTO.Enkontrol.Tablas.Proveedor;
using Core.DTO.Utils.Data;
using Core.Enum.Multiempresa;
using Data.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Enkontrol.General.Proveedor
{
    public class ProveedorDAO : IProveedorDAO
    {
        public List<sp_proveedoresDTO> GetProveedores()
        {
            var query_sp_proveedores = new OdbcConsultaDTO();

            query_sp_proveedores.consulta =
                @"SELECT
                    P.numpro,
                    P.nomCorto,
                    P.nombre,
                    P.moneda,
                    M.moneda AS descripcionMoneda
                FROM
                    sp_proveedores AS P
                INNER JOIN
                    moneda AS M
                    ON
                        M.clave = P.moneda";

            return _contextEnkontrol.Select<sp_proveedoresDTO>(vSesiones.sesionAmbienteEnkontrolAdm, query_sp_proveedores);
        }
    }
}
