using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.EntityFramework.Generic;//generic
using Core.Entity.RecursosHumanos.Captura;//tabla
using Core.DAO.RecursosHumanos.Captura;// interfaz
using Core.Enum.Principal.Bitacoras;
using Core.DTO;

namespace Data.DAO.RecursosHumanos.Captura
{
    public class AutorizacionAditivaDeductivaDAO : GenericDAO<tblRH_AutorizacionAditivaDeductiva>, IAutorizacionAditivaDeductivaDAO
    {
       public tblRH_AutorizacionAditivaDeductiva GuardarAutorizacion(tblRH_AutorizacionAditivaDeductiva objAutorizacion)
        {
            try
            {
                if (objAutorizacion.id == 0)
                {
                    objAutorizacion.fechafirma = null;
                    SaveEntity(objAutorizacion, (int)BitacoraEnum.AutorizacionAditivaPersonal);
                }
                else
                {
                    Update(objAutorizacion, objAutorizacion.id, (int)BitacoraEnum.AutorizacionAditivaPersonal);
                }

            }
            catch (Exception e)
            {
                return new tblRH_AutorizacionAditivaDeductiva();
            }

            return objAutorizacion;
        }
       public List<tblRH_AutorizacionAditivaDeductiva> getAutorizacion(int idAditiva)
        {

            var dto = new List<tblRH_AutorizacionAditivaDeductiva>();

            try
            {
                var result = _context.tblRH_AutorizacionAditivaDeductiva.Where(x => x.id_AditivaDeductiva == idAditiva).OrderBy(x => x.orden).ToList();

                foreach (var i in result)
                {
                    dto.Add(i);
                }

            }
            catch (Exception e)
            {
                return new List<tblRH_AutorizacionAditivaDeductiva>();
            }

            return dto;
        }
       //cambios autorizacion aditiva
       public tblRH_AutorizacionAditivaDeductiva SaveChangesAutorizacionCambios(tblRH_AutorizacionAditivaDeductiva objAutorizacion)
        {
            try
            {
                if (objAutorizacion.id == 0)
                {
                    SaveEntity(objAutorizacion, (int)BitacoraEnum.AutorizacionAditivaPersonal);
                }
                else
                {
                    if (objAutorizacion.rechazado == true)
                    {
                        objAutorizacion.firma = "RECHAZADO";
                        objAutorizacion.fechafirma = (DateTime.Now).ToString();
                    }
                    Update(objAutorizacion, objAutorizacion.id, (int)BitacoraEnum.AutorizacionAditivaPersonal);
                }

            }
            catch (Exception e)
            {
                return new tblRH_AutorizacionAditivaDeductiva();
            }

            return objAutorizacion;
        }
       //extrar firma autorizacion   12/12/17
       public  tblRH_AutorizacionAditivaDeductiva getAutorizacionIndividual(int idFirma)
        {

            try
            {
                //var result = _context.tblRH_AutorizacionAditivaDeductiva.Where(x => x.id_AditivaDeductiva == idAditiva).OrderBy(x => x.orden).ToList();
                var result = _context.tblRH_AutorizacionAditivaDeductiva.FirstOrDefault(x => x.id == idFirma);
                return result;
               
            }
            catch (Exception e)
            {
                return new tblRH_AutorizacionAditivaDeductiva();
            }

          
        }
       public void EliminarAutorizador(tblRH_AutorizacionAditivaDeductiva objAutorizacion)
       {
           try
           {
               tblRH_AutorizacionAditivaDeductiva entidad = _context.tblRH_AutorizacionAditivaDeductiva.FirstOrDefault(x => x.id.Equals(objAutorizacion.id));
               Delete(entidad, (int)BitacoraEnum.AditivaPersonal);
           }
           catch (Exception)
           {
               throw new Exception("Error al eliminar el registro");
           }
       }
       public bool EsUsuarioMismoCC(int usuarioCapturoID)
       {
           var usuarioActualID = vSesiones.sesionUsuarioDTO.id;

           if (usuarioCapturoID == usuarioActualID)
           {
               return true;
           }

           var ccsUsuarioCapturo = _context.tblP_CC_Usuario
               .Where(x => x.usuarioID == usuarioCapturoID)
               .Select(x => x.cc)
               .ToList();

           var ccsUsuarioLogueado = _context.tblP_CC_Usuario
               .Where(x => x.usuarioID == usuarioActualID)
               .Select(x => x.cc)
               .ToList();

           if (ccsUsuarioCapturo.Count == 0 || ccsUsuarioLogueado.Count == 0)
           {
               return false;
           }

           return ccsUsuarioCapturo.Intersect(ccsUsuarioLogueado).Count() > 0;
       }
    }
}
