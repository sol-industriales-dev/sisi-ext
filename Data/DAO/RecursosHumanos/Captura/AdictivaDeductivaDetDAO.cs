using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.EntityFramework.Generic;//generic
using Core.Entity.RecursosHumanos.Captura;//tabla
using Core.DAO.RecursosHumanos.Captura;// interfaz
using Core.Enum.Principal.Bitacoras;

namespace Data.DAO.RecursosHumanos.Captura
{
    public class AdictivaDeductivaDetDAO : GenericDAO<tblRH_AditivaDeductivaDet>, IAditivaDeductivaDetDAO
    {
        public tblRH_AditivaDeductivaDet GuardarAditivaDeducDet(tblRH_AditivaDeductivaDet objDetalle)
            {
                try
                {
                    if (objDetalle.id == 0)
                    {
                        SaveEntity(objDetalle, (int)BitacoraEnum.Curso);
                    }
                    else
                    {
                        Update(objDetalle, objDetalle.id, (int)BitacoraEnum.Curso);
                    }

                }
                catch (Exception e)
                {
                    return new tblRH_AditivaDeductivaDet();
                }

                return objDetalle;
            }
        //obtener  listado de detalles de la aditiva deductiva
        public List<tblRH_AditivaDeductivaDet> getAditivaDeductivaDet(int idAditiva)
            {

                var dto = new List<tblRH_AditivaDeductivaDet>();

                try
                {
                    var result = _context.tblRH_AditivaDeductivaDet.Where(x => x.id_AditivaDeductiva == idAditiva && x.estado!=false).OrderBy(x => x.categoria).ToList();

                    foreach (var i in result)
                    {
                        dto.Add(i);
                    }

                }
                catch (Exception e)
                {
                    return new List<tblRH_AditivaDeductivaDet>();
                }

                return dto;
            }
        //eliminar raguilar
        public void eliminarDetalle(int formatoDetalleID)
        {
            var DetalleAditiva = _context.tblRH_AditivaDeductivaDet.Where(x => x.id == formatoDetalleID).First();
            DetalleAditiva.estado = false;
            Update(DetalleAditiva, DetalleAditiva.id, (int)BitacoraEnum.Curso);
            //_context.tblRH_AditivaDeductivaDet.Remove(DetalleAditiva);
            _context.SaveChanges();
        }
    }
    }
