using System.Collections.Generic;

namespace ProyectoEpidemiologiaIPC2.Logica
{
    public class RejillaOptimizada
    {
        public HashSet<(int, int)> Vivas { get; set; }
        public int M { get; set; }

        public RejillaOptimizada(int m)
        {
            M = m;
            Vivas = new HashSet<(int, int)>();
        }
    }
}