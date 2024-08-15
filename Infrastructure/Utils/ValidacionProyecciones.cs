using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utils
{
    public class ValidacionProyecciones
    {

        public int GetData<T>(List<T> res, int mes, int currentMes)
        {
            if (mes == currentMes)
            {
                return 1;
            }
            else if (currentMes < mes)
            {
                var diff = mes - currentMes;
                if (diff == 1)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else if (currentMes > mes)
            {
                return 2;
            }

            return 0;
        }
    }
}
