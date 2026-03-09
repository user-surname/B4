using B4.Api.Dto.PostDto;
using B4.Api.Dto.GetDto;
using WinFormsApp1.Services;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly CiclosApiService api = new CiclosApiService();

        public Form1()
        {
            InitializeComponent();

            // Evento de selección de fila
            dgvCiclos.CellContentClick += dgvCiclos_CellContentClick;
        }

        // ------------------ GET ALL ------------------
        private async void btnGetAll_Click_1(object sender, EventArgs e)
        {
            try
            {
                var ciclos = await api.GetAll();
                dgvCiclos.DataSource = ciclos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener todos los ciclos: " + ex.Message);
            }
        }

        // ------------------ GET BY ID ------------------
        private async void btnGetById_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtId.Text, out int id))
                {
                    MessageBox.Show("ID inválido");
                    return;
                }

                var ciclo = await api.GetById(id);

                if (ciclo != null)
                {

                    // Mostrar en el DataGridView
                    dgvCiclos.DataSource = new List<CiclosGetDto> { ciclo };

                }
                else
                {
                    MessageBox.Show("No se encontró el ciclo.");

                    // Limpiar DataGridView si no hay resultados
                    dgvCiclos.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el ciclo: " + ex.Message);
            }
        }

        // ------------------ INSERT ------------------
        private async void btnInsert_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtIdCiclo.Text, out int idCiclo))
                {
                    MessageBox.Show("IdCiclo inválido");
                    return;
                }

                var ciclo = new CiclosPostDto
                {
                    IdCiclo = idCiclo,
                    Ciclo = txtCiclo.Text,
                    Descripcion = txtDescripcion.Text
                };

                await api.Insert(ciclo);
                MessageBox.Show("Insertado correctamente");

                // Refrescar DataGridView
                btnGetAll_Click_1(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar el ciclo: " + ex.Message);
            }
        }

        // ------------------ DELETE ------------------
        private async void btnDelete_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtId.Text, out int id))
                {
                    MessageBox.Show("ID inválido");
                    return;
                }

                await api.Delete(id);
                MessageBox.Show("Eliminado correctamente");

                // Refrescar DataGridView
                btnGetAll_Click_1(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el ciclo: " + ex.Message);
            }
        }

        // ------------------ AUTOCOMPLETAR TEXTBOX DESDE DATAGRID ------------------
        private void dgvCiclos_CellContentClick(object sender, EventArgs e)
        {
            if (dgvCiclos.CurrentRow != null && dgvCiclos.CurrentRow.DataBoundItem is CiclosGetDto ciclo)
            {
                txtIdCiclo.Text = ciclo.IdCiclo.ToString();
                txtCiclo.Text = ciclo.Ciclo;
                txtDescripcion.Text = ciclo.Descripcion;
            }
        }

        private void txtId_TextChanged(object sender, EventArgs e)
        {
            // Limpiar campos al cambiar el ID
            txtIdCiclo.Clear();
            txtCiclo.Clear();
            txtDescripcion.Clear();
        }

        private void txtDescripcion_TextChanged(object sender, EventArgs e)
        {
            // No se necesita lógica aquí, pero el evento está registrado
        }

        private void txtIdCiclo_TextChanged(object sender, EventArgs e)
        {
            // No se necesita lógica aquí, pero el evento está registrado
        }

        private void txtCiclo_TextChanged(object sender, EventArgs e)
        {
            // No se necesita lógica aquí, pero el evento está registrado
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var auth = new AuthService();
                var token = await auth.Login(txtEmail.Text, txtPassword.Text);

                if (!string.IsNullOrEmpty(token))
                {
                    api.SetToken(token); // Asignar token al servicio CiclosApiService
                    MessageBox.Show("Login correcto!");
                }

                api.SetToken(token); // Asignar token al servicio CiclosApiService
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al hacer login: " + ex.Message);
            }
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }

}