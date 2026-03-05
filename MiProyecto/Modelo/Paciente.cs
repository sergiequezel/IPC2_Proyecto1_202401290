namespace ProyectoEpidemiologiaIPC2.Modelo
{
    public class Paciente
    {
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public int Periodos { get; set; }
        public int M { get; set; }

        public ListaCeldas CeldasIniciales { get; set; }

        public Paciente()
        {
            CeldasIniciales = new ListaCeldas();
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}