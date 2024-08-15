using Core.DAO.Administracion.Contratistas;
using Core.DTO;
using Core.DTO.Administracion.Cotnratistas;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Contratistas;
using Core.Enum.Administracion.Seguridad;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Administracion.Contratistas
{
    public class EmpresasDAO : GenericDAO<tblS_IncidentesEmpresasContratistas>, IEmpresasDAO
    {
        string Controller = "EmpresasController";
        private Dictionary<string, object> resultado;
        public List<ComboDTO> ObtenerEmpresasCombo()
        {
            //List<ComboDTO> data = new List<ComboDTO>();
            //data = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.esActivo).Select(x => new ComboDTO //TODO
            //{
            //    Value = x.id.ToString(),
            //    Text = x.nombreEmpresa
            //}).ToList();
            //return data;

            bool esContratista = vSesiones.sesionUsuarioDTO.idPerfil == 4 ? true : false;
            int idContratista = esContratista ? vSesiones.sesionUsuarioDTO.id : 0;

            List<int> lstRelEmpleadoEmpresa = _context.tblS_IncidentesRelEmpresaContratistas.Where(x => x.idContratista == idContratista && x.esActivo).Select(x => x.idEmpresa).ToList();
            List<tblS_IncidentesEmpresasContratistas> lstEmpresas = _context.tblS_IncidentesEmpresasContratistas.Where(x => lstRelEmpleadoEmpresa.Count() > 0 ? lstRelEmpleadoEmpresa.Contains(x.id) : true && x.esActivo).ToList();
            var lstContratistasCboDTO = lstEmpresas.Select(x => new Core.DTO.Principal.Generales.ComboDTO
            {
                Text = x.nombreEmpresa,
                Value = x.id.ToString(),
                Prefijo = ((int)GruposSeguridadEnum.CONTRATISTA).ToString()
            }).ToList();
            return lstContratistasCboDTO;
        }

        public List<EmpresasDTO> ObtenerEmpresas(int nombreEmpresa, bool esActivo)
        {
            List<EmpresasDTO> data;
            if (nombreEmpresa != 0)
            {
                data = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.id == nombreEmpresa && x.esActivo == esActivo).Select(x => new EmpresasDTO
                {
                    id = x.id,
                    nombreEmpresa = x.nombreEmpresa,
                    esActivo = x.esActivo
                }).ToList();
            }
            else
            {
                data = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.esActivo == esActivo).Select(x => new EmpresasDTO
                {
                    id = x.id,
                    nombreEmpresa = x.nombreEmpresa,
                    esActivo = x.esActivo
                }).ToList();
            }

            return data;
        }

        public EmpresasDTO AgregarEmpresa(EmpresasDTO objEmpresa)
        {
            try
            {
                EmpresasDTO objRespuesta = new EmpresasDTO();
                tblS_IncidentesEmpresasContratistas objAddEmpresas = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.nombreEmpresa == objEmpresa.nombreEmpresa && x.esActivo == true).FirstOrDefault();
                if (objAddEmpresas == null)
                {
                    tblS_IncidentesEmpresasContratistas obj = new tblS_IncidentesEmpresasContratistas();
                    obj.nombreEmpresa = objEmpresa.nombreEmpresa;
                    obj.esActivo = true;

                    _context.tblS_IncidentesEmpresasContratistas.Add(obj);
                    _context.SaveChanges();
                    objRespuesta.msjExito = "Guardado Con Exito";
                    objRespuesta.statusExito = 1;
                }
                else
                {
                    objRespuesta.msjExito = "ya existe una empresa favor de asignar otra";
                    objRespuesta.statusExito = 2;
                }
                return objRespuesta;
            }
            catch (Exception e)
            {
                LogError(2, 0, Controller, "AtblC_EmpresasContratistas", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public EmpresasDTO EditarEmpresa(EmpresasDTO objEmpresa)
        {
            try
            {

                tblS_IncidentesEmpresasContratistas objEditEmpr = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.nombreEmpresa == objEmpresa.nombreEmpresa).FirstOrDefault();
                tblS_IncidentesEmpresasContratistas objEditEmpresas = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.id == objEmpresa.id && x.esActivo == true).FirstOrDefault();
                if (objEditEmpr == null)
                {

                    if (objEditEmpresas != null)
                    {
                        objEditEmpresas.nombreEmpresa = objEmpresa.nombreEmpresa;
                        objEditEmpresas.esActivo = true;

                        _context.SaveChanges();
                        objEmpresa.msjExito = "Guardado Con Exito";
                        objEmpresa.statusExito = 1;
                    }
                    else
                    {
                        objEmpresa.msjExito = "Ocurrio algun problema al editar";
                        objEmpresa.statusExito = 2;
                    }
                }
                else
                {
                    objEmpresa.msjExito = "¡Ya existe una empresa con este nombre!";
                    objEmpresa.statusExito = 2;

                }
                return objEmpresa;
            }
            catch (Exception e)
            {
                LogError(2, 0, Controller, "AtblC_EmpresasContratistas", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }


        public EmpresasDTO ActivarDesactivarEmpresa(int idEmpresa, bool esActivo)
        {
            try
            {
                EmpresasDTO objEmpresa = new EmpresasDTO();
                tblS_IncidentesEmpresasContratistas objActDescEmpresas = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.id == idEmpresa).FirstOrDefault();
                List<tblS_IncidentesEmpleadoContratistas> lstEmpleado = _context.tblS_IncidentesEmpleadoContratistas.Where(x => x.idEmpresaContratista == idEmpresa).ToList(); 
                if (objActDescEmpresas != null)
                {
                    objActDescEmpresas.esActivo = esActivo;
                    _context.SaveChanges();

                    foreach(var item in lstEmpleado)
                    {
                        tblS_IncidentesEmpleadoContratistas objEmpleado = _context.tblS_IncidentesEmpleadoContratistas.Where(x => x.id == item.id).FirstOrDefault();
                        objEmpleado.esActivo = esActivo;
                        _context.SaveChanges();
                    }

                    objEmpresa.msjExito = "Guardado Con Exito";
                    objEmpresa.statusExito = 1;
                }
                else
                {
                    objEmpresa.msjExito = "No se pudo Activar o Desactivar";
                    objEmpresa.statusExito = 2;
                }
                return objEmpresa;
            }
            catch (Exception e)
            {
                LogError(2, 0, Controller, "AtblC_EmpresasContratistas", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

    }
}
