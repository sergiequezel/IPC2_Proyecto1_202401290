using System.Collections.Generic;
using System.Xml.Linq;
using ProyectoEpidemiologiaIPC2.Modelo;

namespace ProyectoEpidemiologiaIPC2.Persistencia
{
    public static class GestorXML
    {
        public static List<Paciente> Cargar(string ruta)
        {
            var lista = new List<Paciente>();
            XDocument doc = XDocument.Load(ruta);

            foreach (var p in doc.Descendants("paciente"))
            {
                Paciente paciente = new Paciente
                {
                    Nombre = p.Element("datospersonales").Element("nombre").Value,
                    Edad = int.Parse(p.Element("datospersonales").Element("edad").Value),
                    Periodos = int.Parse(p.Element("periodos").Value),
                    M = int.Parse(p.Element("m").Value)
                };

                foreach (var celda in p.Element("rejilla").Elements("celda"))
                {
                    int f = int.Parse(celda.Attribute("f").Value);
                    int c = int.Parse(celda.Attribute("c").Value);
                    paciente.CeldasIniciales.Agregar(f, c);
                }

                lista.Add(paciente);
            }

            return lista;
        }
    }
}