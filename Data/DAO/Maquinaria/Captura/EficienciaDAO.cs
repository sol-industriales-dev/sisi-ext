using Core.DAO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Captura;
using Core.Entity.Maquinaria.Captura;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using Data.Factory.Maquinaria.Captura;
using Data.Factory.Maquinaria.Catalogos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Captura
{
    public class EficienciaDAO : GenericDAO<tblM_Eficiencia>, IEficienciaDAO
    {
        private EficienciaFactoryService EficienciaFactory = new EficienciaFactoryService();
        private CentroCostosFactoryServices centroCostosFactoryServices = new CentroCostosFactoryServices();

        public tblM_Eficiencia GuardaEficiencia(tblM_Eficiencia obj)
        {
            try
            {
                if (obj.id == 0)
                {
                    SaveEntity(obj, (int)BitacoraEnum.EFICIENCIA);
                }
                else
                {
                    Update(obj, obj.id, (int)BitacoraEnum.EFICIENCIA);
                }

            }
            catch (Exception e)
            {
                return new tblM_Eficiencia();
            }

            return obj;
        }
        public List<tblM_Eficiencia> getTablaEficiencia(DateTime FechaInicioFiltro, DateTime FechaUltimoFiltro, string cc)
        {
            List<tblM_Eficiencia> objResult = new List<tblM_Eficiencia>();
            objResult = _context.tblM_Eficiencia.Where(x => x.Fecha > FechaInicioFiltro && x.Fecha < FechaUltimoFiltro).ToList();
            return objResult;
        }

        public List<tblM_Eficiencia> getTablaEficiencia(DateTime FechaInicioFiltro, DateTime FechaUltimoFiltro)
        {
            List<tblM_Eficiencia> objResult = new List<tblM_Eficiencia>();
            objResult = _context.tblM_Eficiencia.Where(x => x.Fecha > FechaInicioFiltro && x.Fecha < FechaUltimoFiltro).ToList();
            return objResult;
        }
        public List<tblM_Eficiencia> getEficienciaObraInfo(DateTime FechaInicioFiltro, DateTime FechaUltimoFiltro, string cc)
        {
            List<tblM_Eficiencia> objResult = new List<tblM_Eficiencia>();
            objResult = getTablaEficiencia(FechaInicioFiltro, FechaUltimoFiltro, cc);
            var centroCosto = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(cc);
            List<tblM_Eficiencia> lstCalculo = new List<tblM_Eficiencia>();
            foreach (var item in objResult)
            {
                tblM_Eficiencia objCalculo = new tblM_Eficiencia();
                objCalculo.Comentarios = centroCosto;
                objCalculo.IdObra = item.IdObra;
                objCalculo.IdGrupo = item.IdGrupo;
                objCalculo.Semana = item.Semana;
                objCalculo.Economico = item.Economico;
                objCalculo.HrsTrabajada += item.HrsTrabajada;
                objCalculo.FaltaOperador += item.FaltaOperador;
                objCalculo.Paro += item.Paro;
                objCalculo.HrsTotalReparacion += item.HrsTotalReparacion;
                objCalculo.HrsTotal = objCalculo.HrsTrabajada + objCalculo.FaltaOperador + objCalculo.Paro + objCalculo.HrsTotalReparacion;
                objCalculo.HrsBase = objCalculo.HrsTotal.Equals(0) ? 0 : objCalculo.HrsTrabajada * (1 / objCalculo.HrsTotal);


                lstCalculo.Add(objCalculo);
            }

            for (int i = 0; i < objResult.Count; i++)
            {
                lstCalculo[i].HrsDiferencia = lstCalculo.Where(x => x.IdGrupo.Equals(lstCalculo[i].IdGrupo)).Sum(x => x.HrsTrabajada) * (1 / lstCalculo.Where(x => x.IdGrupo.Equals(lstCalculo[i].IdGrupo)).Sum(x => x.HrsTotal));
            }

            return lstCalculo;
        }

        public List<RepEficienciaGeneralDTO> getEficienciaGeneralInfo(DateTime FechaInicioFiltro, DateTime FechaUltimoFiltro)
        {
            List<tblM_Eficiencia> objResult = new List<tblM_Eficiencia>();
            objResult = getTablaEficiencia(FechaInicioFiltro, FechaUltimoFiltro);
            var lstGrupo = objResult.Select(x => new
            {
                idGrupo = x.IdGrupo,
                idObra = x.IdObra
            });
            List<RepEficienciaGeneralDTO> lstPromedio = new List<RepEficienciaGeneralDTO>();
            decimal[,] numbers = new decimal[lstGrupo.ToArray().Length, 15];
            for (int i = 0; i < lstGrupo.ToArray().Length; i++)
            {
                RepEficienciaGeneralDTO objPromedio = new RepEficienciaGeneralDTO();
                objPromedio.Frente = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(lstGrupo.ToArray()[i].idObra).ToUpper();
                objPromedio.IdGrupo = objResult[i].IdGrupo;
                var objCalculo = getEficienciaObraInfo(FechaInicioFiltro, FechaUltimoFiltro, lstGrupo.ToArray()[i].idObra);
                decimal suma = 0;
                for (int j = 0; j < lstGrupo.ToArray().Length; j++)
                {
                    try
                    {
                        decimal dato = objCalculo.Where(x => x.IdObra.Equals(lstGrupo.ToArray()[j].idObra) && x.IdGrupo.Equals(lstGrupo.ToArray()[i].idGrupo)).FirstOrDefault().HrsDiferencia;
                        numbers[i, j] = Math.Round(dato, 2);
                        suma += numbers[i, j];
                    }
                    catch (Exception)
                    {
                        numbers[i, j] = 0;
                    }

                }
                objPromedio.Prom1 = numbers[i, 0];
                objPromedio.Prom2 = numbers[i, 1];
                objPromedio.Prom3 = numbers[i, 2];
                objPromedio.Prom4 = numbers[i, 3];
                objPromedio.Prom5 = numbers[i, 4];
                objPromedio.Prom6 = numbers[i, 5];
                objPromedio.Prom7 = numbers[i, 6];
                objPromedio.Prom8 = numbers[i, 7];
                objPromedio.Prom9 = numbers[i, 8];
                objPromedio.Prom10 = numbers[i, 9];
                objPromedio.Prom11 = numbers[i, 10];
                objPromedio.Prom12 = numbers[i, 11];
                objPromedio.Prom13 = numbers[i, 12];
                objPromedio.Prom14 = numbers[i, 13];
                objPromedio.Prom15 = numbers[i, 14];
                lstPromedio.Add(objPromedio);
            }
            foreach (var item in lstPromedio)
            {
                int contador = 0;
                contador += item.Prom1 > decimal.Zero ? 1 : 0;
                contador += item.Prom2 > decimal.Zero ? 1 : 0;
                contador += item.Prom3 > decimal.Zero ? 1 : 0;
                contador += item.Prom4 > decimal.Zero ? 1 : 0;
                contador += item.Prom5 > decimal.Zero ? 1 : 0;
                contador += item.Prom6 > decimal.Zero ? 1 : 0;
                contador += item.Prom7 > decimal.Zero ? 1 : 0;
                contador += item.Prom8 > decimal.Zero ? 1 : 0;
                contador += item.Prom9 > decimal.Zero ? 1 : 0;
                contador += item.Prom10 > decimal.Zero ? 1 : 0;
                contador += item.Prom11 > decimal.Zero ? 1 : 0;
                contador += item.Prom12 > decimal.Zero ? 1 : 0;
                contador += item.Prom13 > decimal.Zero ? 1 : 0;
                contador += item.Prom14 > decimal.Zero ? 1 : 0;
                contador += item.Prom15 > decimal.Zero ? 1 : 0;
                decimal suma = item.Prom1 + item.Prom2 + item.Prom3 + item.Prom4 + item.Prom5 + item.Prom6 + item.Prom7 + item.Prom8 + item.Prom9 + item.Prom10 + item.Prom11 + item.Prom12 + item.Prom13 + item.Prom14 + item.Prom15;
                item.Eficiencia = suma / contador;
            }
            return lstPromedio;
        }
    }
}
