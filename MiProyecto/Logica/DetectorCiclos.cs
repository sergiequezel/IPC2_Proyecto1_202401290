namespace ProyectoEpidemiologiaIPC2.Logica
{
    public static class DetectorCiclos
    {
        public static (int mu, int lambda)? Detectar(RejillaOptimizada inicial, int limite)
        {
            var tortuga = SimuladorOptimizado.SiguienteEstado(inicial);
            var liebre = SimuladorOptimizado.SiguienteEstado(
                         SimuladorOptimizado.SiguienteEstado(inicial));

            int pasos = 0;

            while (!Iguales(tortuga, liebre))
            {
                if (pasos++ > limite) return null;

                tortuga = SimuladorOptimizado.SiguienteEstado(tortuga);
                liebre = SimuladorOptimizado.SiguienteEstado(
                         SimuladorOptimizado.SiguienteEstado(liebre));
            }

            int mu = 0;
            tortuga = inicial;

            while (!Iguales(tortuga, liebre))
            {
                tortuga = SimuladorOptimizado.SiguienteEstado(tortuga);
                liebre = SimuladorOptimizado.SiguienteEstado(liebre);
                mu++;
            }

            int lambda = 1;
            liebre = SimuladorOptimizado.SiguienteEstado(tortuga);

            while (!Iguales(tortuga, liebre))
            {
                liebre = SimuladorOptimizado.SiguienteEstado(liebre);
                lambda++;
            }

            return (mu, lambda);
        }

        private static bool Iguales(RejillaOptimizada a, RejillaOptimizada b)
        {
            return a.Vivas.SetEquals(b.Vivas);
        }
    }
}