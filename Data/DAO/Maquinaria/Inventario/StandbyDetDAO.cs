using Core.DAO.Maquinaria.Inventario;
using Core.DTO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Inventario
{
    public class StandbyDetDAO : GenericDAO<tblM_DetStandby>, IStandByDetDAO
    {
        public void GuardarStandByDet(List<standByDetDTO> obj, standByDTO standbyCliente, int StandByID)
        {
            List<tblM_DetStandby> listobjDet = new List<tblM_DetStandby>();
            foreach (var item in obj)
            {
                tblM_DetStandby objDet = new tblM_DetStandby();

                if (item.id == 0)
                {
                    objDet.id = 0;
                }
                else
                {
                    objDet.id = item.id;
                }

                objDet.DiaParo = item.FechaStandBy;
                objDet.FechaCaptura = DateTime.Now;
                objDet.noEconomicoID = item.noEconomicoID;
                objDet.StandByID = StandByID;
                objDet.TipoConsideracion = item.TipoConsideracion;
                objDet.estatus = true;
                objDet.FechaFinStandby = standbyCliente.fechaFin;


                var horometro = _context.tblM_CapHorometro.Where(x => x.Economico == item.Economico && x.Fecha >= standbyCliente.fechaInicio.Date && x.Fecha <= standbyCliente.fechaFin);
                if (horometro.Count() > 0)
                {
                    objDet.HorometroInicial = horometro.FirstOrDefault().Horometro - horometro.FirstOrDefault().HorasTrabajo;
                    objDet.HorometroFinal = horometro.OrderByDescending(x => x.id).FirstOrDefault().Horometro;
                }
                else
                {
                    var horometroAux = _context.tblM_CapHorometro.Where(x => x.Economico == item.Economico).OrderByDescending(x => x.id).FirstOrDefault();

                    if (horometroAux != null)
                    {
                        objDet.HorometroInicial = horometroAux.Horometro;
                        objDet.HorometroFinal = horometroAux.Horometro;

                    }
                    else
                    {
                        objDet.HorometroInicial = 0;
                        objDet.HorometroFinal = 0;
                    }
                }



                if (true)
                {
                    if (objDet.id == 0)
                        SaveEntity(objDet, (int)BitacoraEnum.STANDBYDETALLE);

                    else
                        Update(objDet, objDet.id, (int)BitacoraEnum.STANDBYDETALLE);
                }
                else
                {
                    throw new Exception("Error ocurrio un error al insertar un registro");
                }


            }

        }



        public List<tblM_DetStandby> getListaDetStandBy(int StandByID)
        {

            var result = _context.tblM_DetStandby.Where(x => x.StandByID == StandByID).ToList();

            return result;
        }
        public void DeleteRow(tblM_DetStandby objDetSingle)
        {
            try
            {
                tblM_DetStandby entidad = _context.tblM_DetStandby.FirstOrDefault(x => x.id == objDetSingle.id);

                Delete(entidad, (int)BitacoraEnum.STANDBYDETALLE);
            }
            catch (Exception)
            {
                throw new Exception("Error al eliminar el registro");
            }
        }

    }
}
