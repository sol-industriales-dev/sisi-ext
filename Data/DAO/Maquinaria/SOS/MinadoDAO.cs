using Core.DAO.Maquinaria.SOS;
using Core.DTO.Maquinaria.SOS;
using Core.Entity.Maquinaria.SOS;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.SOS
{
    public class MinadoDAO : GenericDAO<MinadoEntity>, IMinadoDAO
    {
        public void Guardar(List<MinadoEntity> obj)
        {
            if (true)
            {
                //AddRows(obj);
                saveEntitys(obj, (int)BitacoraEnum.CAPTURACOMBUSTIBLE);
            }
            else
            {
                throw new Exception("");
            }
        }

        public List<CCMuestrasDTO> cboFiltroLugar()
        {

            List<CCMuestrasDTO> result = _context.MinadoEntity
            .GroupBy(p => new { p.Name })
            .Select(g => new CCMuestrasDTO
            {
                descripcion = g.FirstOrDefault().Name.Replace("GRUPO CONSTRUCCIONES PLANIFICADAS / ", ""),
                folio = g.FirstOrDefault().Signum_Account_No
            }).ToList();

            return result;
        }
        public List<CCMuestrasDTO> cboFiltroModelo(string lugar)
        {

            List<CCMuestrasDTO> result = _context.MinadoEntity.Where(x => x.Name.Replace("GRUPO CONSTRUCCIONES PLANIFICADAS / ", "") == lugar)
            .GroupBy(p => new { p.Name })
            .Select(g => new CCMuestrasDTO
            {
                descripcion = g.FirstOrDefault().Model,
                folio = g.FirstOrDefault().Model
            }).ToList();

            return result;
        }
        public List<CCMuestrasDTO> cboComponente()
        {

            List<CCMuestrasDTO> result = _context.MinadoEntity
            .GroupBy(p => new { p.Description })
            .Select(g => new CCMuestrasDTO
            {
                descripcion = g.FirstOrDefault().Description,
                folio = g.FirstOrDefault().Description
            }).ToList();

            return result;
        }

        public List<MaquinaDTO> cboFiltroMaquinaria(string lugar)
        {

            var result = (_context.MinadoEntity.Where(x => x.Name.Replace("GRUPO CONSTRUCCIONES PLANIFICADAS / ", "") == lugar)
            .GroupBy(p => new { p.Unit_ID, })
            .Select(g => new MaquinaDTO
            {
                economico = g.FirstOrDefault().Unit_ID,
                descripcion = g.FirstOrDefault().Unit_ID
            }
            )).ToList();

            return result;
        }

        public List<MaquinaDTO> cboFiltroMaquinariaXlista(List<string> lugar)
        {

            var result = (_context.MinadoEntity.Where(x => lugar.Contains(x.Signum_Account_No))
            .GroupBy(p => new { p.Unit_ID, })
            .Select(g => new MaquinaDTO
            {
                economico = g.FirstOrDefault().Unit_ID,
                descripcion = g.FirstOrDefault().Unit_ID
            }
            )).ToList();

            return result;
        }



        public List<MuestrasGeneralesDTO> muestrasGenerales(string lugar, DateTime fechaini, DateTime fechafin)
        {
            var rest = _context.MinadoEntity.ToList().Where(m => m.Date_Sampled.Date >= fechaini &&
                                                            m.Date_Sampled.Date <= fechafin &&
                                                            m.Name.Replace("GRUPO CONSTRUCCIONES PLANIFICADAS / ", "") == lugar).
                                                            Select(m => new MuestrasElementosDTO { al = m.Al_Aluminum, cu = m.Cu_Copper, fe = m.Fe_Iron, si = m.Si_Silicon });

            var alertaAl = rest.Where(x => Convert.ToInt32(x.al) >= 7).Count();
            var precaucionAl = rest.Where(x => Convert.ToInt32(x.al) >= 3.5 && Convert.ToInt32(x.al) < 7).Count();
            var normalAl = rest.Where(x => Convert.ToInt32(x.al) < 3.5).Count();

            var alertacu = rest.Where(x => Convert.ToInt32(x.cu) >= 11).Count();
            var precaucioncu = rest.Where(x => Convert.ToInt32(x.cu) < 11 && Convert.ToInt32(x.cu) >= 7.5).Count();
            var normalcu = rest.Where(x => Convert.ToInt32(x.cu) < 7.5).Count();

            var alertafe = rest.Where(x => Convert.ToInt32(x.fe) >= 40).Count();
            var precaucionfe = rest.Where(x => Convert.ToInt32(x.fe) < 40 && Convert.ToInt32(x.fe) >= 25).Count();
            var normalfe = rest.Where(x => Convert.ToInt32(x.fe) < 25).Count();

            var alertasi = rest.Where(x => Convert.ToInt32(x.si) >= 10).Count();
            var precaucionsi = rest.Where(x => Convert.ToInt32(x.si) < 10 && Convert.ToInt32(x.si) >= 4).Count();
            var normalsi = rest.Where(x => Convert.ToInt32(x.si) < 4).Count();

            List<MuestrasGeneralesDTO> muestrasGeneralesDTO = new List<MuestrasGeneralesDTO>();
            int totalAleta = alertaAl + alertacu + alertafe + alertasi;
            int totalPrecaucion = precaucionAl + precaucioncu + precaucionfe + precaucionsi;
            int totalNormal = normalAl + normalcu + normalfe + normalsi;
            if (totalAleta != 0 && totalPrecaucion != 0 && totalNormal != 0)
            {
                muestrasGeneralesDTO.Add(new MuestrasGeneralesDTO
                {
                    Descripcion = "Alerta",
                    total = totalAleta
                });
                muestrasGeneralesDTO.Add(new MuestrasGeneralesDTO
                {
                    Descripcion = "Precaucion",
                    total = totalPrecaucion
                });
                muestrasGeneralesDTO.Add(new MuestrasGeneralesDTO
                {
                    Descripcion = "Normal",
                    total = totalNormal
                });
            }
            return muestrasGeneralesDTO;
        }


        public List<MuestrasGeneralesDTO> muestrasGeneralesLists(List<string> lugar, DateTime fechaini, DateTime fechafin)
        {
            var rest = _context.MinadoEntity.ToList().Where(m => m.Date_Sampled.Date >= fechaini &&
                                                            m.Date_Sampled.Date <= fechafin &&
                                                            lugar.Contains(m.Signum_Account_No)).
                                                            Select(m => new MuestrasElementosDTO { al = m.Al_Aluminum, cu = m.Cu_Copper, fe = m.Fe_Iron, si = m.Si_Silicon });

            var alertaAl = rest.Where(x => Convert.ToInt32(x.al) >= 7).Count();
            var precaucionAl = rest.Where(x => Convert.ToInt32(x.al) >= 3.5 && Convert.ToInt32(x.al) < 7).Count();
            var normalAl = rest.Where(x => Convert.ToInt32(x.al) < 3.5).Count();

            var alertacu = rest.Where(x => Convert.ToInt32(x.cu) >= 11).Count();
            var precaucioncu = rest.Where(x => Convert.ToInt32(x.cu) < 11 && Convert.ToInt32(x.cu) >= 7.5).Count();
            var normalcu = rest.Where(x => Convert.ToInt32(x.cu) < 7.5).Count();

            var alertafe = rest.Where(x => Convert.ToInt32(x.fe) >= 40).Count();
            var precaucionfe = rest.Where(x => Convert.ToInt32(x.fe) < 40 && Convert.ToInt32(x.fe) >= 25).Count();
            var normalfe = rest.Where(x => Convert.ToInt32(x.fe) < 25).Count();

            var alertasi = rest.Where(x => Convert.ToInt32(x.si) >= 10).Count();
            var precaucionsi = rest.Where(x => Convert.ToInt32(x.si) < 10 && Convert.ToInt32(x.si) >= 4).Count();
            var normalsi = rest.Where(x => Convert.ToInt32(x.si) < 4).Count();

            List<MuestrasGeneralesDTO> muestrasGeneralesDTO = new List<MuestrasGeneralesDTO>();
            int totalAleta = alertaAl + alertacu + alertafe + alertasi;
            int totalPrecaucion = precaucionAl + precaucioncu + precaucionfe + precaucionsi;
            int totalNormal = normalAl + normalcu + normalfe + normalsi;
            if (totalAleta != 0 && totalPrecaucion != 0 && totalNormal != 0)
            {
                muestrasGeneralesDTO.Add(new MuestrasGeneralesDTO
                {
                    Descripcion = "Alerta",
                    total = totalAleta
                });
                muestrasGeneralesDTO.Add(new MuestrasGeneralesDTO
                {
                    Descripcion = "Precaucion",
                    total = totalPrecaucion
                });
                muestrasGeneralesDTO.Add(new MuestrasGeneralesDTO
                {
                    Descripcion = "Normal",
                    total = totalNormal
                });
            }
            return muestrasGeneralesDTO;
        }


        public List<MuestrasElementosDTO> detalleGeneralMuestras(string lugar, DateTime fechaini, DateTime fechafin, string indicador)
        {
            var rest = _context.MinadoEntity.ToList().Where(m => m.Date_Sampled.Date >= fechaini &&
                                                          m.Date_Sampled.Date <= fechafin &&
                                                          m.Name.Replace("GRUPO CONSTRUCCIONES PLANIFICADAS / ", "") == lugar).
                                                          Select(m => new MuestrasElementosDTO { name = m.Unit_ID, al = m.Al_Aluminum, cu = m.Cu_Copper, fe = m.Fe_Iron, si = m.Si_Silicon, description = m.Description });

            return rest.ToList();
        }

        public List<MuestrasElementosDTO> detalleGeneralMuestrasList(List<string> lugar, DateTime fechaini, DateTime fechafin, string indicador)
        {
            var rest = _context.MinadoEntity.ToList().Where(m => m.Date_Sampled.Date >= fechaini &&
                                                          m.Date_Sampled.Date <= fechafin &&
                                                          lugar.Contains(m.Signum_Account_No)).
                                                          Select(m => new MuestrasElementosDTO { name = m.Unit_ID, al = m.Al_Aluminum, cu = m.Cu_Copper, fe = m.Fe_Iron, si = m.Si_Silicon, description = m.Description });

            return rest.ToList();
        }



        //private List<indicaroresMuestraGeneralDTO> (string indicador, List<MuestrasElementosDTO> lista)
        //{

        //    if (indicador == "Normal")
        //    {

        //    }
        //    if (indicador == "alerta")
        //    {
        //        var al = lista.Where(x => Convert.ToInt16(x.al) > getTipoIndicador("AL", true)
        //                             ).Select(x => new indicaroresMuestraGeneralDTO { elemento = x.al, maquina = x.name }).ToList();
        //        var cu = lista.Where(x => Convert.ToInt16(x.cu) > getTipoIndicador("CU", true)
        //                            ).Select(x => new indicaroresMuestraGeneralDTO { elemento = x.cu, maquina = x.name }).ToList();
        //        var fe = lista.Where(x => Convert.ToInt16(x.fe) > getTipoIndicador("FE", true)
        //                            ).Select(x => new indicaroresMuestraGeneralDTO { elemento = x.fe, maquina = x.name }).ToList();
        //        var si = lista.Where(x => Convert.ToInt16(x.si) > getTipoIndicador("SI", true)
        //                            ).Select(x => new indicaroresMuestraGeneralDTO { elemento = x.si, maquina = x.name }).ToList();
        //    }
        //    if (indicador == "alerta")
        //    {
        //        var al = lista.Where(x => Convert.ToInt16(x.al) > getTipoIndicador("AL", true)
        //                             ).Select(x => new indicaroresMuestraGeneralDTO { elemento = x.al, maquina = x.name }).ToList();
        //        var cu = lista.Where(x => Convert.ToInt16(x.cu) > getTipoIndicador("CU", true)
        //                            ).Select(x => new indicaroresMuestraGeneralDTO { elemento = x.cu, maquina = x.name }).ToList();
        //        var fe = lista.Where(x => Convert.ToInt16(x.fe) > getTipoIndicador("FE", true)
        //                            ).Select(x => new indicaroresMuestraGeneralDTO { elemento = x.fe, maquina = x.name }).ToList();
        //        var si = lista.Where(x => Convert.ToInt16(x.si) > getTipoIndicador("SI", true)
        //                            ).Select(x => new indicaroresMuestraGeneralDTO { elemento = x.si, maquina = x.name }).ToList();
        //    }
        //    return null;
        //}
        public List<indicaroresDTO> detallesMuestras(List<MuestrasElementosDTO> resultado, string elemento)
        {
            double alerta = getTipoIndicador(elemento, true);
            double preacucion = getTipoIndicador(elemento, false);

            switch (elemento)
            {
                case "AL":
                    return resultado.Select(x => new indicaroresDTO { indicador = Convert.ToInt32(x.al), alerta = alerta, precaucion = preacucion, hora_Aceite = x.hora_Aceite, hora_equipo = x.hora_equipo, name = x.name }).ToList();
                case "CU":
                    return resultado.Select(x => new indicaroresDTO { indicador = Convert.ToInt32(x.cu), alerta = alerta, precaucion = preacucion, hora_Aceite = x.hora_Aceite, hora_equipo = x.hora_equipo, name = x.name }).ToList();
                case "FE":
                    return resultado.Select(x => new indicaroresDTO { indicador = Convert.ToInt32(x.fe), alerta = alerta, precaucion = preacucion, hora_Aceite = x.hora_Aceite, hora_equipo = x.hora_equipo, name = x.name }).ToList();
                case "SI":
                    return resultado.Select(x => new indicaroresDTO { indicador = Convert.ToInt32(x.si), alerta = alerta, precaucion = preacucion, hora_Aceite = x.hora_Aceite, hora_equipo = x.hora_equipo, name = x.name }).ToList();
                default:
                    break;
            }
            return null;
        }

        public List<MuestrasElementosDTO> detalleCompleto(string lugar, string componente, string unitid, string modelo, string elemento, DateTime fechaini, DateTime fechafin)
        {
            var rest = _context.MinadoEntity.ToList().
                                        Where(m => m.Date_Sampled.Date >= fechaini &&
                                        m.Date_Sampled.Date <= fechafin &&
                                        m.Name.Replace("GRUPO CONSTRUCCIONES PLANIFICADAS / ", "") == lugar &&
                                        (string.IsNullOrEmpty(componente) == true ? m.Description == m.Description : m.Description.Contains(componente)) &&
                                        (string.IsNullOrEmpty(unitid) == true ? m.Unit_ID == m.Unit_ID : m.Unit_ID.Contains(unitid)) &&
                                        (string.IsNullOrEmpty(modelo) == true ? m.Model == m.Model : m.Model.Contains(modelo))
                                        ).
                                        Select(m => new MuestrasElementosDTO
                                        {
                                            description = m.Description,
                                            al = m.Al_Aluminum,
                                            cu = m.Cu_Copper,
                                            fe = m.Fe_Iron,
                                            si = m.Si_Silicon,
                                            name = m.Unit_ID.ToString(),
                                            hora_Aceite = m.Oil_Age,
                                            hora_equipo = m.Equipment_Age,
                                            fecha = m.Date_Sampled
                                        });
            return rest.ToList();
        }
        public List<MuestrasElementosDTO> detalleCompletoLista(List<string> lugar, string componente, string unitid, string modelo, string elemento, DateTime fechaini, DateTime fechafin)
        {
            var rest = _context.MinadoEntity.ToList().
                                        Where(m => m.Date_Sampled.Date >= fechaini &&
                                        m.Date_Sampled.Date <= fechafin &&
                                        lugar.Contains(m.Signum_Account_No) &&
                                        (string.IsNullOrEmpty(componente) == true ? m.Description == m.Description : m.Description.Contains(componente)) &&
                                        (string.IsNullOrEmpty(unitid) == true ? m.Unit_ID == m.Unit_ID : m.Unit_ID.Contains(unitid)) &&
                                        (string.IsNullOrEmpty(modelo) == true ? m.Model == m.Model : m.Model.Contains(modelo))
                                        ).
                                        Select(m => new MuestrasElementosDTO
                                        {
                                            description = m.Description,
                                            al = m.Al_Aluminum,
                                            cu = m.Cu_Copper,
                                            fe = m.Fe_Iron,
                                            si = m.Si_Silicon,
                                            name = m.Unit_ID.ToString(),
                                            hora_Aceite = m.Oil_Age,
                                            hora_equipo = m.Equipment_Age,
                                            fecha = m.Date_Sampled
                                        });
            return rest.ToList();
        }

        private double getTipoIndicador(string elemento, bool tipo)
        {
            double alerta = 0;
            switch (elemento)
            {
                case "AL":
                    if (tipo)
                    {
                        return 7;
                    }
                    else
                    {
                        return 3.5;
                    }

                case "CU":
                    if (tipo)
                    {
                        return 11;
                    }
                    else
                    {
                        return 7.5;
                    }

                case "FE":
                    if (tipo)
                    {
                        return 40;
                    }
                    else
                    {
                        return 25;
                    }

                case "SI":
                    if (tipo)
                    {
                        return 10;
                    }
                    else
                    {
                        return 4;
                    }
                default:
                    break;
            }
            return alerta;
        }
    }
}
