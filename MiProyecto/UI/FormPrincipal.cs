using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ProyectoEpidemiologiaIPC2.Modelo;
using ProyectoEpidemiologiaIPC2.Logica;
using ProyectoEpidemiologiaIPC2.Persistencia;
using System.Drawing;
using System.Xml.Linq;
using System.Linq;

namespace ProyectoEpidemiologiaIPC2.UI
{
    public partial class FormPrincipal : Form
    {
        private List<Paciente> pacientes;
        private RejillaOptimizada rejillaActual;
        private int periodoActual = 0;
        private Dictionary<string, int> estadosConPeriodo = new Dictionary<string, int>();
        private System.Windows.Forms.Timer timer;

        private string resultadoFinal = "No determinado";
        private const int MAX_PERIODOS = 500;

        public FormPrincipal()
        {
            InitializeComponent();

            pacientes = GestorXML.Cargar("entrada.xml");
            comboPacientes.DataSource = pacientes;

            if (pacientes.Count > 0)
                comboPacientes.SelectedIndex = 0;

            btnPaso.Click += BtnPaso_Click;
            btnAuto.Click += BtnAuto_Click;
            btnGenerarXML.Click += BtnGenerarXML_Click;
            comboPacientes.SelectedIndexChanged += ComboPacientes_SelectedIndexChanged;
            panelRejilla.Paint += PanelRejilla_Paint;

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 200;
            timer.Tick += Timer_Tick;

            panelRejilla.DoubleBuffered(true);
        }

        private void DibujarRejilla()
        {
            panelRejilla.Invalidate();
        }

        private void BtnPaso_Click(object sender, EventArgs e)
        {
            if (rejillaActual == null) return;

            if (periodoActual >= MAX_PERIODOS)
            {
                resultadoFinal = "Límite alcanzado";
                MessageBox.Show("Se alcanzó el límite máximo.");
                return;
            }

            string estadoActual = SerializarEstado(rejillaActual);

            // 🔥 Corrección: usar estadosConPeriodo y ContainsKey
            if (estadosConPeriodo.ContainsKey(estadoActual))
            {
                int periodoAnterior = estadosConPeriodo[estadoActual];
                int N1 = periodoActual - periodoAnterior;

                if (N1 == 1)
                {
                    resultadoFinal = "Mortal";
                    MessageBox.Show("Resultado: Mortal (ciclo de longitud 1)");
                }
                else
                {
                    resultadoFinal = "Grave";
                    MessageBox.Show("Resultado: Grave (ciclo de longitud " + N1 + ")");
                }

                return;
            }

            // Guardar estado actual
            estadosConPeriodo[estadoActual] = periodoActual;

            // Avanzar simulación
            rejillaActual = SimuladorOptimizado.SiguienteEstado(rejillaActual);
            periodoActual++;

            lblPeriodo.Text = "Periodo: " + periodoActual;
            lblVivas.Text = "Células vivas: " + rejillaActual.Vivas.Count;

            DibujarRejilla();
        }
        private void ComboPacientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboPacientes.SelectedItem == null) return;

            timer.Stop(); // 🔥 detener automático si estaba activo
            resultadoFinal = "No determinado";

            Paciente p = (Paciente)comboPacientes.SelectedItem;

            rejillaActual = new RejillaOptimizada(p.M);

            NodoCelda temp = p.CeldasIniciales.ObtenerCabeza();

            while (temp != null)
            {
                rejillaActual.Vivas.Add((temp.Fila - 1, temp.Columna - 1));
                temp = temp.Siguiente;
            }

            periodoActual = 0;
            estadosConPeriodo.Clear();


            string estadoInicial = SerializarEstado(rejillaActual);
            estadosConPeriodo[estadoInicial] = 0;

            lblPeriodo.Text = "Periodo: 0";
            lblVivas.Text = "Células vivas: " + rejillaActual.Vivas.Count;

            DibujarRejilla();
        }

        private void PanelRejilla_Paint(object sender, PaintEventArgs e)
        {
            if (rejillaActual == null) return;

            e.Graphics.Clear(Color.White);

            int tamañoCelda = panelRejilla.Width / rejillaActual.M;
            if (tamañoCelda < 1) tamañoCelda = 1;

            foreach (var celda in rejillaActual.Vivas)
            {
                e.Graphics.FillRectangle(
                    Brushes.Red,
                    celda.Item2 * tamañoCelda,
                    celda.Item1 * tamañoCelda,
                    tamañoCelda,
                    tamañoCelda);
            }
        }

        private string SerializarEstado(RejillaOptimizada r)
        {
            return string.Join(";",
                r.Vivas
                 .OrderBy(x => x.Item1)
                 .ThenBy(x => x.Item2)
                 .Select(x => $"{x.Item1},{x.Item2}")
            );
        }

        private void GenerarXMLSalida(string ruta)
        {
            if (rejillaActual == null) return;

            Paciente p = (Paciente)comboPacientes.SelectedItem;

            XElement paciente =
                new XElement("paciente",
                    new XElement("datospersonales",
                        new XElement("nombre", p.Nombre),
                        new XElement("edad", p.Edad)
                    ),
                    new XElement("periodos", periodoActual),
                    new XElement("resultado", resultadoFinal), // ✅ ahora sí se guarda
                    new XElement("m", rejillaActual.M),
                    new XElement("rejilla",
                        rejillaActual.Vivas.Select(c =>
                            new XElement("celda",
                                new XAttribute("f", c.Item1 + 1),
                                new XAttribute("c", c.Item2 + 1)
                            )
                        )
                    )
                );

            XDocument documento =
                new XDocument(
                    new XDeclaration("1.0", "UTF-8", null),
                    new XElement("pacientes", paciente)
                );

            documento.Save(ruta);
        }

        private void BtnGenerarXML_Click(object sender, EventArgs e)
        {
            if (rejillaActual == null) return;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Archivo XML (*.xml)|*.xml";
            sfd.FileName = "salida.xml";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                GenerarXMLSalida(sfd.FileName);
                MessageBox.Show("XML generado correctamente.");
            }
        }

        private void BtnAuto_Click(object sender, EventArgs e)
        {
            if (rejillaActual == null) return;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (rejillaActual == null)
            {
                timer.Stop();
                return;
            }

            if (periodoActual >= MAX_PERIODOS)
            {
                timer.Stop();
                resultadoFinal = "Límite alcanzado";
                MessageBox.Show("Se alcanzó el límite máximo.");
                return;
            }

            // Avanzar simulación
            rejillaActual = SimuladorOptimizado.SiguienteEstado(rejillaActual);
            periodoActual++;

            lblPeriodo.Text = "Periodo: " + periodoActual;
            lblVivas.Text = "Células vivas: " + rejillaActual.Vivas.Count;

            DibujarRejilla();

            string estadoActual = SerializarEstado(rejillaActual);

            if (estadosConPeriodo.ContainsKey(estadoActual))
            {
                int periodoAnterior = estadosConPeriodo[estadoActual];
                int N1 = periodoActual - periodoAnterior;

                timer.Stop();

                if (N1 == 1)
                {
                    resultadoFinal = "Mortal";
                    MessageBox.Show("Resultado: Mortal (ciclo de longitud 1)");
                }
                else
                {
                    resultadoFinal = "Grave";
                    MessageBox.Show("Resultado: Grave (ciclo de longitud " + N1 + ")");
                }

                return;
            }

            estadosConPeriodo[estadoActual] = periodoActual;
        }

    }
}

public static class ControlExtensions
{
    public static void DoubleBuffered(this Control control, bool enable)
    {
        var prop = control.GetType().GetProperty(
            "DoubleBuffered",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance);

        prop?.SetValue(control, enable, null);
    }
}