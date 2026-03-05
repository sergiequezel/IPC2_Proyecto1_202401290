namespace ProyectoEpidemiologiaIPC2.Modelo
{
    public class NodoCelda
    {
        public int Fila { get; private set; }
        public int Columna { get; private set; }
        public NodoCelda Siguiente { get; set; }

        public NodoCelda(int fila, int columna)
        {
            Fila = fila;
            Columna = columna;
            Siguiente = null;
        }
    }
}
