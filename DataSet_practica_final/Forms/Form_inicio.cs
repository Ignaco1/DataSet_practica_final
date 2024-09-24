using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataSet_practica_final
{
    public partial class Form1 : Form
    {
        string vari;
        
        public Form1()
        {
            InitializeComponent();

            ModoVista();
            txt_edad.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargaDatos();
        }

        private void ModoCarga()
        {
            groupBox1.Enabled = false;
            groupBox2.Enabled = true;
        }

        private void Limpiar()
        {
            txt_nombre.Text = "";
            txt_correo.Text = "";
            txt_edad.Text = "";
            fecha_picker.Text = "";
        }

        private void ModoVista()
        {
            groupBox1.Enabled = true;
            groupBox2.Enabled = false;
        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CargaDatos()
        {
            DsCrudTableAdapters.UsuariosTableAdapter ta = new DsCrudTableAdapters.UsuariosTableAdapter();
            DsCrud.UsuariosDataTable dt = ta.GetData();

            dataGridView1.DataSource = dt;
        }

        private int GetEdad()
        {
            int edad = DateTime.Now.Year - fecha_picker.Value.Year;

            if (DateTime.Now.Month < fecha_picker.Value.Month)
            {
                edad -= 1;
            }
            else if (DateTime.Now.Month == fecha_picker.Value.Month && DateTime.Now.Day < fecha_picker.Value.Day)
            {
                edad -= 1;
            }

            return edad;
        }

        private int GetId()
        {
            try
            {
                return Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar obtener el ID: " + ex.Message); 
            }
        }

        private void btn_agregar_Click(object sender, EventArgs e)
        {
            vari = "A";

            ModoCarga();
        }

        private void btn_mod_Click(object sender, EventArgs e)
        {
            vari = "M";

            int id = GetId();

            DsCrudTableAdapters.UsuariosTableAdapter ta = new DsCrudTableAdapters.UsuariosTableAdapter();
            DsCrud.UsuariosDataTable dt = ta.GetPersona(id);
            DsCrud.UsuariosRow row = (DsCrud.UsuariosRow) dt.Rows[0];
            
            txt_nombre.Text = row.Nombre;
            txt_correo.Text = row.Correo;
            fecha_picker.Value = row.Fecha_de_nacimiento;
            txt_edad.Text = Convert.ToString(row.Edad);

            ModoCarga();
        }

        private void btn_eliminar_Click(object sender, EventArgs e)
        {
            int id = GetId();
            int edad = GetEdad();
           

            DsCrudTableAdapters.UsuariosTableAdapter ta = new DsCrudTableAdapters.UsuariosTableAdapter();
            DsCrud.UsuariosDataTable dt = ta.GetPersona(id);
            DsCrud.UsuariosRow row = (DsCrud.UsuariosRow)dt.Rows[0];

            string fecha = row.Fecha_de_nacimiento.ToString("dd/MM/yyyy");

            DialogResult respuesta = MessageBox.Show($"Esta seguro que desea eliminar al usuario:\n\nNombre: {row.Nombre}\n\nCorreo: {row.Correo}\n\nFecha de nacimiento: {fecha}\n\nEdad: {row.Edad}", "AVISO", MessageBoxButtons.YesNo);
            
            if (respuesta == DialogResult.Yes)
            {
                ta.Eliminar(id);

                CargaDatos();
            }
        }

        private void btn_guardar_Click(object sender, EventArgs e)
        {
            int edad = GetEdad();
            int id = GetId();

            if (vari == "A")
            {               
                DsCrudTableAdapters.UsuariosTableAdapter ta = new DsCrudTableAdapters.UsuariosTableAdapter();
                ta.Agregar(txt_nombre.Text, txt_correo.Text, fecha_picker.Value.Date, edad);
            }
            else if (vari == "M")
            {                
                DsCrudTableAdapters.UsuariosTableAdapter ta = new DsCrudTableAdapters.UsuariosTableAdapter();
                ta.Modificar(txt_nombre.Text, txt_correo.Text, fecha_picker.Value.Date, edad, id);
            }

            CargaDatos();
            Limpiar();
            ModoVista();
        }

        private void btn_cancelar_Click(object sender, EventArgs e)
        {
            ModoVista();
        }
    }
}
