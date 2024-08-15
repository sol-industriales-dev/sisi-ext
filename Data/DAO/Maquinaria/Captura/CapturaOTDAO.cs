using Core.DAO.Maquinaria.Captura;
using Core.DTO;
using Core.DTO.Utils.Data;
using Core.Entity.Maquinaria.BackLogs;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.RecursosHumanos.Catalogo;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Captura
{
    public class CapturaOTDAO : GenericDAO<tblM_CapOrdenTrabajo>, ICapturaOTDAO
    {
        public void Guardar(tblM_CapOrdenTrabajo obj, int idBL)
        {
            if (obj.id == 0)
            {
                #region SE CREA LA OT
                SaveEntity(obj, (int)BitacoraEnum.CAPTURAOT);
                #endregion

                #region SE CREA RELACION DE LA OT CON EL BL, EN CASO QUE LA CREACIÓN DE LA OT VENGA DE BACKLOGS.
                List<tblM_CapOrdenTrabajo> lstOT = _context.tblM_CapOrdenTrabajo.Where(x => x.EconomicoID == obj.EconomicoID && x.CC == obj.CC && x.horometro == obj.horometro).ToList();
                int idOT = lstOT.Count() > 0 ? lstOT.OrderByDescending(x => x.id).FirstOrDefault().id : 0;

                tblBL_OT objOT = new tblBL_OT();
                objOT.idOT = idOT;
                objOT.idBL = idBL;
                objOT.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                objOT.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                objOT.fechaCreacion = DateTime.Now;
                objOT.fechaModificacion = DateTime.Now;
                objOT.esActivo = true;
                _context.tblBL_OT.Add(objOT);
                _context.SaveChanges();
                #endregion
            }
            else
                Update(obj, obj.id, (int)BitacoraEnum.CAPTURAOT);
        }
        public List<tblRH_CatEmpleados> getCatEmpleados(string term, List<string> CentroCostos)
        {

            List<string> CentroCostos2 = new List<string>();

            //  CentroCostos2 = _context.tblP_CC.Where(x => CentroCostos.Contains(x.areaCuenta)).Select(y => y.cc).ToList();

            List<tblRH_CatEmpleados> lstCatEmpleado = new List<tblRH_CatEmpleados>();

            var getCatEmpleado = @"SELECT top 10 clave_empleado, (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Nombre, puesto, cc_contable as CC FROM DBA.sn_empleados WHERE replace((nombre+' '+ape_paterno+' '+ape_materno), ' ', '') LIKE '%" + term.Replace(" ", "") + "%' " +
                //"AND CC_Contable in (" + GetListaCCString(CentroCostos) + ")";
            "AND estatus_empleado = 'A'";
            try
            {
                //var empresas = _context.tblP_Empresas.Where(x => x.estatus).Select(x => x.id).OrderByDescending(x => x).First();

                int ii = 1;
                var empresas = 8;

                //if (vSesiones.sesionEmpresaActual == 6)
                //{
                //    ii = 6;
                //    empresas = 6;
                //}
                //if (vSesiones.sesionEmpresaActual == 3)
                //{
                //    ii = 3;
                //    empresas = 3;
                //}

                //if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 2)
                //{
                //    ii = 1;
                //    empresas = 2;
                //}

                for (int i = ii; i <= empresas; i++)
                {
                    var obtenerEmpleados = false;
                    switch ((MainContextEnum)i)
                    {
                        case MainContextEnum.Construplan:
                            obtenerEmpleados = true;
                            break;
                        case MainContextEnum.GCPLAN:
                            obtenerEmpleados = true;
                            break;
                        case MainContextEnum.Arrendadora:
                            obtenerEmpleados = true;
                            break;
                        case MainContextEnum.Colombia:
                            obtenerEmpleados = true;
                            break;
                        case MainContextEnum.PERU:
                            obtenerEmpleados = true;
                            break;
                        default:
                            obtenerEmpleados = false;
                            break;
                    }

                    if (obtenerEmpleados)
                    {
                        var resultado = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                        {
                            baseDatos = (MainContextEnum)i,
                            consulta = @"SELECT top 10 clave_empleado, (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Nombre, puesto, cc_contable as CC 
                                    FROM tblRH_EK_Empleados 
                                    WHERE estatus_empleado='A' and replace((nombre+' '+ape_paterno+' '+ape_materno), ' ', '') LIKE @parametro",
                            parametros = new { parametro = "%" + term.Replace(" ", "") + "%" }
                        });

                        foreach (var item in resultado)
                        {
                            lstCatEmpleado.Add(item);
                        }
                    }
                }

                var lstTemp = new List<tblRH_CatEmpleados>();
                foreach (var item in lstCatEmpleado.GroupBy(x => x.clave_empleado))
                {
                    lstTemp.Add(item.First());
                }

                lstCatEmpleado = lstTemp;              
            }
            catch
            {
                return lstCatEmpleado;
            }

            return lstCatEmpleado;
        }
        private string GetListaCCString(List<string> CentroCostos)
        {
            string cadenaString = "";

            foreach (string CC in CentroCostos)
            {
                cadenaString += "'" + CC + "',";
            }

            return cadenaString.TrimEnd(',');
        }

        public List<tblM_CapOrdenTrabajo> getListaOT(string cc, List<string> listcc)
        {
            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                case EmpresaEnum.Peru:
                    {
                        var areaCuenta = _context.tblP_CC.FirstOrDefault(x => x.cc == cc);
                        cc = areaCuenta != null ? areaCuenta.areaCuenta : cc;
                    }
                    break;
            }

            return _context.tblM_CapOrdenTrabajo
               .Where(x => !string.IsNullOrEmpty(cc) ? x.CC.Equals(cc) : true)
               .ToList();

        }
        public List<tblM_DetOrdenTrabajo> getListaOTDet(string cc, List<string> listcc,DateTime fechaInicio,DateTime fechaFin)
        {
            List<tblM_DetOrdenTrabajo> lista = new List<tblM_DetOrdenTrabajo>();
            lista.AddRange(_context.tblM_DetOrdenTrabajo
               .Where(x => (!string.IsNullOrEmpty(cc) ? x.OrdenTrabajo.CC.Equals(cc) : true) && x.OrdenTrabajo.FechaEntrada >= fechaInicio && x.OrdenTrabajo.FechaEntrada <= fechaFin).OrderBy(x => x.HoraInicio)
               .ToList());
            return lista;

        }
        public tblM_CapOrdenTrabajo GetOTbyID(int id)
        {

            return _context.tblM_CapOrdenTrabajo.FirstOrDefault(x => x.id.Equals(id));
        }

        public tblM_CapOrdenTrabajo GetOTByEconomico(int idEconomico)
        {

            var DataOrden = _context.tblM_CapOrdenTrabajo.Where(x => x.EconomicoID == idEconomico).OrderByDescending(x => x.id).ToList();

            return DataOrden.FirstOrDefault();
        }

    }
}
