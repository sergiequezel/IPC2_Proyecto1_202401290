using System.Windows.Forms;
using System.Drawing;

namespace ProyectoEpidemiologiaIPC2.UI
{
    partial class FormPrincipal
    {
        private ComboBox comboPacientes;
        private Button btnPaso;
        private Button btnAuto;
        private Label lblPeriodo;
        private Label lblVivas;
        private Panel panelRejilla;
        private Button btnGenerarXML;

        private void InitializeComponent()
        {
            this.comboPacientes = new ComboBox();
            this.btnPaso = new Button();
            this.btnAuto = new Button();
            this.btnGenerarXML = new Button();
            this.lblPeriodo = new Label();
            this.lblVivas = new Label();
            this.panelRejilla = new Panel();


            this.SuspendLayout();

            // ComboBox
            comboPacientes.Location = new Point(20, 20);
            comboPacientes.Size = new Size(200, 25);

            // Botón Paso
            btnPaso.Text = "Paso";
            btnPaso.Location = new Point(250, 20);
            btnPaso.Size = new Size(100, 30);

            // Botón Auto
            btnAuto.Text = "Automático";
            btnAuto.Location = new Point(370, 20);
            btnAuto.Size = new Size(120, 30);

            // Label Periodo
            lblPeriodo.Location = new Point(20, 60);
            lblPeriodo.Size = new Size(200, 25);
            lblPeriodo.Text = "Periodo: 0";

            // Botón Generar XML
            btnGenerarXML.Text = "Generar XML";
            btnGenerarXML.Location = new Point(510, 20);
            btnGenerarXML.Size = new Size(130, 30);

            // Label Vivas
            lblVivas.Location = new Point(250, 60);
            lblVivas.Size = new Size(200, 25);
            lblVivas.Text = "Células vivas: 0";

            // Panel Rejilla
            panelRejilla.Location = new Point(20, 100);
            panelRejilla.Size = new Size(600, 600);
            panelRejilla.BorderStyle = BorderStyle.FixedSingle;

            // Form
            this.ClientSize = new Size(800, 750);
            this.Controls.Add(comboPacientes);
            this.Controls.Add(btnPaso);
            this.Controls.Add(btnAuto);
            this.Controls.Add(btnGenerarXML);
            this.Controls.Add(lblPeriodo);
            this.Controls.Add(lblVivas);
            this.Controls.Add(panelRejilla);
            this.Text = "Simulador Epidemiológico";

            this.ResumeLayout(false);
        }
    }
}