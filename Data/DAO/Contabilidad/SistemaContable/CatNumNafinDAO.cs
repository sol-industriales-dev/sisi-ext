using Core.DAO.Contabilidad.Reportes;
using Core.DTO;
using Core.Entity.Administrativo.Contabilidad;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using Data.Factory.Contabilidad.Reportes;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Contabilidad.Reportes
{
    public class CatNumNafinDAO : GenericDAO<tblC_CatNumNafin>, ICatNumNafinDAO
    {
        CadenaProductivaFactoryServices ProductivaFactory = new CadenaProductivaFactoryServices();
        public void Guardar(tblC_CatNumNafin obj)
        {
            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.NumNafin);
            else
                Update(obj, obj.id, (int)BitacoraEnum.NumNafin);
        }
        /// <summary>
        /// Guarda lst de proveedores desde nafin
        /// </summary>
        /// <param name="lst">proveedores</param>
        /// <returns>bandera de éxito</returns>
        public bool GuardarLstProvNafin(List<tblC_CatNumNafin> lst)
        {
            var moneda = lst.FirstOrDefault().TipoMoneda;
            var lstBd = GetLstNafin(moneda);
            lst.ForEach(nafin =>
            {
                var obj = lstBd.FirstOrDefault(bd => bd.NumProveedor.Equals(nafin.NumProveedor));
                if (obj == null)
                {
                    obj = generarCatNumNafin(nafin);
                }
                obj.NumNafin = nafin.NumNafin ?? string.Empty;
                obj.correo = nafin.correo ?? string.Empty;
                obj.idTipoPropuesta = nafin.idTipoPropuesta;
                Guardar(obj);
            });
            return true;
        }
        /// <summary>
        /// Crea un nuevo tblC_CatNumNafin desde ProveedorDTO de enkontrol
        /// </summary>
        /// <param name="nafin">numprov y moneda</param>
        /// <returns>Nuevo objeto tblC_CatNumNafin</returns>
        tblC_CatNumNafin generarCatNumNafin(tblC_CatNumNafin nafin)
        {
            var prov = ProductivaFactory.getCadenaProductivaService().getProveedores(nafin.NumProveedor.ParseInt(), nafin.TipoMoneda).FirstOrDefault();
            return new tblC_CatNumNafin()
            {
                TipoMoneda = nafin.TipoMoneda,
                NumProveedor = nafin.NumProveedor,
                RazonSocial = prov.RAZONSOCIAL,
                RFC = prov.RFC,
                estatus = true
            };
        }
        public List<tblC_CatNumNafin> GetLstHanilitadosNumNafin()
        {
            return _context.tblC_CatNumNafin
                .Where(x => !x.NumNafin.Equals("0"))
                .ToList();
        }

        public List<object> GetLstNafin()
        {
            var lstBdNafin = _context.tblC_CatNumNafin.ToList();
            var lstProveedor = ProductivaFactory.getCadenaProductivaService().ListaPRoveedores();
            var lstNafin = lstProveedor
                            .GroupJoin(
                                lstBdNafin,
                                    prov => prov.NUMPROVEEDOR,
                                    nafin => nafin.NumProveedor.Replace("\r\n", string.Empty),
                                    (x, y) => new { prov = x, nafin = y })
                                    .SelectMany(
                                        x => x.nafin.DefaultIfEmpty(),
                                        (x, y) => new { prov = x.prov, nafin = y })
                                        .Select(x => new
                                            {
                                                id = x.nafin == null ? 0 : x.nafin.id,
                                                rfc = x.prov.RFC ?? string.Empty,
                                                razonSocial = x.prov.RAZONSOCIAL ?? string.Empty,
                                                numProveedor = x.prov.NUMPROVEEDOR,
                                                numNafin = x.nafin == null ? x.prov.NUMPROVEEDOR : x.nafin.NumNafin,
                                                tipoMoneda = x.prov.MONEDA.Equals("1"),
                                                estatus = x.nafin == null ? false : x.nafin.estatus
                                            }).Cast<object>().ToList();
            return lstNafin;
        }
        /// <summary>
        /// Consulta de proveedores Sigoplan
        /// </summary>
        /// <param name="moneda">tipo de moneda</param>
        /// <returns>Lista de proveedores</returns>
        public List<tblC_CatNumNafin> GetLstNafin(int moneda)
        {
            return _context.tblC_CatNumNafin.ToList()
                .Where(p => p.estatus)
                .Where(p => p.TipoMoneda.Equals(moneda))
                .Where(p => !string.IsNullOrEmpty(p.NumNafin) && !p.NumNafin.Equals("0"))
                .ToList();
        }
        /// <summary>
        /// Carga NumNafin con vacio vación
        /// </summary>
        /// <param name="obj">proveedor a desactivar</param>
        /// <returns>bandera de eliminado</returns>
        public bool eliminarNafinProv(tblC_CatNumNafin obj)
        {
            var bdNafin = _context.tblC_CatNumNafin.FirstOrDefault(n => n.NumProveedor.Equals(obj.NumProveedor));
            bdNafin.NumNafin = string.Empty;
            SaveChanges();
            SaveBitacora(vSesiones.sesionCurrentView, (int)AccionEnum.ELIMINAR, bdNafin.id, JsonUtils.convertNetObjectToJson(bdNafin));
            return bdNafin.id > 0;
        }
    }
}
