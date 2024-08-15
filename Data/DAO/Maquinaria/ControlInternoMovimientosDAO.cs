using Core.DAO.Maquinaria.Inventario;
using Core.DTO;
using Core.DTO.Captura;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria
{
    public class ControlInternoMovimientosDAO : GenericDAO<tblM_ControMovimientoInterno>, IControlInternoMovimientosDAO
    {
        public void GuardarActualizar(tblM_ControMovimientoInterno obj)
        {
            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.CONTROLMOVIMIENTOINTERNO);
            else
                Update(obj, obj.id, (int)BitacoraEnum.CONTROLMOVIMIENTOINTERNO);
        }

        public List<tblM_CatMaquina> FillCboEconomicos(string cc)
        {
            return _context.tblM_CatMaquina.Where(x => x.centro_costos == cc).OrderBy(x => x.noEconomico).ToList();
        }

        public tblM_CatMaquina GetDataEconomicoID(int id)
        {
            return _context.tblM_CatMaquina.FirstOrDefault(x => x.id.Equals(id));
        }
        public string LoadFolio()
        {
            var res = _context.tblM_ControInterno.ToList().Count;
            string folio = "";
            folio = folio.PadLeft(5, '0') + (res + 1);
            return folio;
        }

        public List<tblM_ControMovimientoInterno> GetControlesRealizados(int filtro)
        {
            var resultado = _context.tblM_ControInterno.Where(x => x.Estatus == filtro );
            return resultado.ToList();
        }

        public List<ComboDTO> FillCboEconomicosUsuarioID(int usuario)
        {
            var resultado = from a in _context.tblP_Autoriza
                            join pa in _context.tblP_PerfilAutoriza on a.perfilAutorizaID equals pa.id
                            join ccU in _context.tblP_CC_Usuario on a.cc_usuario_ID equals ccU.id
                            where a.perfilAutorizaID == 6 && ccU.usuarioID == usuario
                            select ccU;

            List<ComboDTO> cboData = new List<ComboDTO>();

            foreach (var item in resultado)
            {
                var elemento = _context.tblM_CatMaquina.Where(x => x.centro_costos == item.cc).Select(X => new ComboDTO { Text = X.noEconomico, Value = X.id.ToString() });

                cboData.AddRange(elemento);
            }

            return cboData.ToList();
        }

        public List<ComboDTO> getCentrosCostos(int usuario)
        {
            var res = _context.tblM_CatRelacionMovimientoInterno.Where(x => x.Estatus == true).Select(x => x.Origen).Distinct().ToList();
            var resultado = from a in _context.tblP_Autoriza
                            join pa in _context.tblP_PerfilAutoriza on a.perfilAutorizaID equals pa.id
                            join ccU in _context.tblP_CC_Usuario on a.cc_usuario_ID equals ccU.id
                            where a.perfilAutorizaID == 6 && ccU.usuarioID == usuario && res.Contains(ccU.cc)
                            select new ComboDTO { Text = ccU.cc, Value = ccU.cc };

            
            return resultado.ToList();
        }

        public List<ComboDTO> getCentrosCostosRecepcion(string CentroCostos)
        {
            List<ComboDTO> cboData = new List<ComboDTO>();
            var res = _context.tblM_CatRelacionMovimientoInterno.Where(x => x.Origen.Equals(CentroCostos) && x.Estatus == true).ToList();

            foreach (var item in res)
            {
                ComboDTO ComboDTOObj = new ComboDTO();

                ComboDTOObj.Value = item.Destino;

                var cc = _context.tblP_CC.FirstOrDefault(x=>x.areaCuenta.Equals(item.Destino));
                ComboDTOObj.Text = item.Destino + "-" + cc.descripcion;


                cboData.Add(ComboDTOObj);
            }

            return cboData;
        }

        public string getCentroCostos(string centroCostos)
        {
            try
            {
                string centro_costos = "";
                if (vSesiones.sesionEmpresaActual == 1)
                {
                    centro_costos = "SELECT descripcion FROM cc WHERE cc = '" + centroCostos + "';";
                    var resultado = (IList<economicoDTO>)_contextEnkontrol.Where(centro_costos).ToObject<IList<economicoDTO>>();

                    var nombre_CC = resultado.FirstOrDefault();

                    if (nombre_CC != null)
                    {
                        return nombre_CC.descripcion;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    var resultado = _context.tblP_CC.Where(x => x.areaCuenta == centroCostos);
                    var nombre_CC = resultado.FirstOrDefault();

                    if (nombre_CC != null)
                    {
                        return nombre_CC.descripcion;
                    }
                    else
                    {
                        return "";
                    }
                }
                // string centro_costos = "SELECT descripcion FROM cc WHERE cc = '" + centroCostos + "';";

                //  string 

                ///  


            }
            catch (Exception)
            {
                return "";
            }
        }

        public int GetUsuarioAutoriza(string centroCostos)
        {
            var resultado = (from a in _context.tblP_Autoriza
                             join pa in _context.tblP_PerfilAutoriza on a.perfilAutorizaID equals pa.id
                             join ccU in _context.tblP_CC_Usuario on a.cc_usuario_ID equals ccU.id
                             where a.perfilAutorizaID == 7 && centroCostos == ccU.cc
                             select a.usuarioID).FirstOrDefault();

            return resultado;
        }

    }
}
