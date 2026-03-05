using System.Collections.Generic;

namespace ProyectoEpidemiologiaIPC2.Logica
{
    public static class SimuladorOptimizado
    {
        public static RejillaOptimizada SiguienteEstado(RejillaOptimizada actual)
        {
            var nueva = new RejillaOptimizada(actual.M);
            var conteo = new Dictionary<(int, int), int>();

            foreach (var celda in actual.Vivas)
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue;

                        var vecino = (celda.Item1 + i, celda.Item2 + j);

                        if (!conteo.ContainsKey(vecino))
                            conteo[vecino] = 0;

                        conteo[vecino]++;
                    }
                }
            }

            foreach (var item in conteo)
            {
                bool viva = actual.Vivas.Contains(item.Key);

                if (item.Value == 3 || (viva && item.Value == 2))
                    nueva.Vivas.Add(item.Key);
            }

            return nueva;
        }
    }
}