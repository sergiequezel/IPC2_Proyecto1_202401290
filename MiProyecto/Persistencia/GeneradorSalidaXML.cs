using System.Collections.Generic;
using System.Xml.Linq;
using ProyectoEpidemiologiaIPC2.Modelo;

namespace ProyectoEpidemiologiaIPC2.Persistencia
{
    public static class GeneradorSalidaXML
    {
        public static void Generar(List<ResultadoPaciente> resultados)
        {
            XElement root = new XElement("pacientes");

            foreach (var r in resultados)
            {
                XElement paciente =
                    new XElement("paciente",
                        new XElement("datospersonales",
                            new XElement("nombre", r.Nombre),
                            new XElement("edad", r.Edad)
                        ),

                        new XElement("periodos", r.Periodos),

                        new XElement("resultado", r.Resultado),

                        new XElement("m", r.M),

                        new XElement("n", r.N),

                        // solo agregar N1 si existe
                        r.N1 > 0 ? new XElement("n1", r.N1) : null
                    );

                root.Add(paciente);
            }

            XDocument documento = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                root
            );

            documento.Save("salida.xml");
        }
    }
}