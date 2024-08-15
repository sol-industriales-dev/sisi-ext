using Core.DAO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Proyecciones
{
    public class CapCifrasPrincipalesDAO : GenericDAO<tblPro_CapCifrasPrincipales>, ICapCifrasPrincipalesDAO
    {
        public void Guardar(tblPro_CapCifrasPrincipales obj)
        {
            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.ALTACIFRASPRINCIPALES);
            else
                Update(obj, obj.id, (int)BitacoraEnum.ALTACIFRASPRINCIPALES);
        }

        public void Borrar(int id)
        {

            try
            {
                tblPro_CapCifrasPrincipales entidad = _context.tblPro_CapCifrasPrincipales.FirstOrDefault(x => x.id.Equals(id));
                Delete(entidad, (int)BitacoraEnum.ALTACIFRASPRINCIPALES);
            }
            catch (Exception)
            {
                throw new Exception("Error al eliminar el registro");
            }

        }

        public void BorrarEscenarios(int mes, int anio, string escenarios)
        {
            if (escenarios != "")
            {
                List<string> listaescenarios = new List<string>();
                listaescenarios.AddRange(escenarios.Split(','));
                var Res = _context.tblPro_CapCifrasPrincipales.Where(x => x.ejercicioAnio == anio && x.MesInicio == mes).ToList();
                foreach (var item in Res) 
                {
                    var flag = false;
                    var aux = item.escenarios.Split(',').ToList();
                    var cadenaActualiza = "";
                    foreach (var item2 in aux) 
                    {
                        var indice = listaescenarios.IndexOf(item2);

                        if (indice < 0) { cadenaActualiza += item2 + ","; }
                        else { flag = true;}
                        
                    }
                    if (cadenaActualiza != "")
                    {
                        cadenaActualiza = cadenaActualiza.Substring(0, cadenaActualiza.Length - 1);
                        if (cadenaActualiza != item.escenarios)
                        {
                            item.escenarios = cadenaActualiza;
                            Update(item, item.id, (int)BitacoraEnum.ALTACIFRASPRINCIPALES);
                        }
                    }
                    else 
                    {
                        if (flag) 
                        {
                            Borrar(item.id);
                        }
                    }
                }
            }
        }

        public tblPro_CapCifrasPrincipales getOBJCifrasPrincipales(int mes, int anio, string escenario, int tipo)
        {
            
            tblPro_CapCifrasPrincipales resultado = new tblPro_CapCifrasPrincipales();
        
            resultado = _context.tblPro_CapCifrasPrincipales.FirstOrDefault(x => x.ejercicioAnio == anio && x.MesInicio == mes && x.escenarios == escenario);
            if (resultado == null && tipo == 0)
            {
                resultado = _context.tblPro_CapCifrasPrincipales.FirstOrDefault(x => x.ejercicioAnio == anio && x.MesInicio == mes && (x.escenarios.Contains("," + escenario) || x.escenarios.Contains(escenario + ",")));
            }
            //resultado.escenarios = resultado.escenarios.Substring(0, resultado.escenarios.Length - 1);
            //if (resultado != null)
            //{
            //    return resultado;
            //}
            //else
            //{
            //    if (mes >= 1)
            //    {
            //        mes = mes - 1;
            //    }
            //    else
            //    {
            //        mes = 0;
            //    }

            //    resultado = _context.tblPro_CapCifrasPrincipales.FirstOrDefault(x => x.ejercicioAnio == anio && x.MesInicio == mes);

            //    if (resultado!=null)
            //    {
            //        resultado.id = 0;
            //    }
            return resultado;
            //}
        }
        
        public List<string> getEscenariosConfiguraciones(int mes, int anio)
        {
            var resultado = _context.tblPro_CapCifrasPrincipales.Where(x => x.ejercicioAnio == anio && x.MesInicio == mes);
            if (resultado != null)
            {
                return resultado.Select(x => x.escenarios).ToList();
            }
            else
            {
                if (mes >= 1)
                {
                    mes = mes - 1;
                }
                else
                {
                    mes = 0;
                }
                resultado = _context.tblPro_CapCifrasPrincipales.Where(x => x.ejercicioAnio == anio && x.MesInicio == mes);
                return resultado.Select(x => x.escenarios).ToList();
            }
        }
    }
}
