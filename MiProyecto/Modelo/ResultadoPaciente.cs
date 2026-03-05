namespace ProyectoEpidemiologiaIPC2.Modelo
{
    public class ResultadoPaciente
    {
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public int Periodos { get; set; }
        public int M { get; set; }

        public string Resultado { get; set; }  // leve, grave, mortal
        public int N { get; set; }
        public int N1 { get; set; }
    }
}