namespace ProyectoEpidemiologiaIPC2.Modelo
{
    public class ListaCeldas
    {
        public NodoCelda Cabeza;

        public void Agregar(int f, int c)
        {
            NodoCelda nuevo = new NodoCelda(f, c);
            nuevo.Siguiente = Cabeza;
            Cabeza = nuevo;
        }

        public NodoCelda ObtenerCabeza()
        {
            return Cabeza;
        }
    }
}

// Codio ultimo ... 