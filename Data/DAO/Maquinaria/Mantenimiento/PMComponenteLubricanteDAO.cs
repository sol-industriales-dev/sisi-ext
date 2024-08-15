using Core.DAO.Maquinaria.Mantenimiento;
using Core.DTO.Maquinaria.Mantenimiento.DTO2._0;
using Core.Entity.Maquinaria.Mantenimiento;
using Core.Entity.Maquinaria.Mantenimiento2;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Mantenimiento
{
    public class PMComponenteLubricanteDAO : GenericDAO<tblM_PMComponenteLubricante>, IPMComponenteLubricanteDAO
    {

        public bool SaveOrUpdateComponenteLubricante(tblM_PMComponenteLubricante obj)
        {
            try
            {
                if (obj.id == 0)
                    SaveEntity(obj, (int)BitacoraEnum.ComponenteLubricanteAgrupacion);
                else
                    Update(obj, obj.id, (int)BitacoraEnum.ComponenteLubricanteAgrupacion);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteComponenteLubricante(tblM_PMComponenteLubricante obj)
        {
            try
            {
                if (obj.id != 0)
                {
                    tblM_PMComponenteLubricante entidad = _context.tblM_PMComponenteLubricante.FirstOrDefault(x => x.id.Equals(obj.id));
                    Delete(entidad, (int)BitacoraEnum.ComponenteModeloAgrupacion);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<tblM_PMComponenteLubricante> getTblComponentesLubricantes(int modeloID)
        {
            return _context.tblM_PMComponenteLubricante.Where(x => x.modeloID == modeloID && x.estatus).ToList();
        }

        public List<tblM_CatSuministros> FillCboCatLubricantes()
        {
            return _context.tblM_CatSuministros.OrderBy(x => x.nomeclatura).ToList();
        }
        public List<setLubricantesAlta> tblComponenteLubricante(int modeloID)
        {
            var componentesLubricantes = _context.tblM_PMComponenteLubricante.Where(x => x.modeloID == modeloID && x.estatus).ToList();
            var lubricantesDTO = _context.tblM_CatSuministros.ToList();
            var iconos = _context.tblM_IconMantenimiento.ToList();
            var infoData = from a in _context.tblM_PMComponenteModelo.ToList()
                           where a.modeloID == modeloID
                           select a;
            List<setLubricantesAlta> data = new List<setLubricantesAlta>();

            foreach (var a in infoData.ToList())
            {
                var lubricantesObj = componentesLubricantes.Where(x => x.componenteID == a.componenteID).Select(l => new cboLubricantesDTO
                            {
                                descripcion = lubricantesDTO.FirstOrDefault(r => r.id == l.lubricanteID).nomeclatura,
                                lubricanteID = l.lubricanteID,
                                edadAceite = l.vidaLubricante

                            }).ToList();

                if (lubricantesObj.Count > 0)
                {
                    var dataObj = new setLubricantesAlta
                    {
                        componenteID = a.componenteID,
                        componenteDesc = a.Componente.Descripcion,
                        lubricantes = lubricantesObj,
                        vidaLubricante = lubricantesObj.FirstOrDefault().edadAceite,
                        vidaRest = lubricantesObj.FirstOrDefault().edadAceite - 0,
                        horServ = 0,
                        actID = 0,
                        pruebaLub = false,
                        estatus = false,
                        vidaActual = 0
                    };

                    data.Add(dataObj);
                }

            }
            return data;
        }

    }
}
