namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtId = new TextBox();
            txtDescripcion = new TextBox();
            txtIdCiclo = new TextBox();
            txtCiclo = new TextBox();
            btnGetAll = new Button();
            btnGetById = new Button();
            btnInsert = new Button();
            btnDelete = new Button();
            dgvCiclos = new DataGridView();
            Id = new Label();
            label1 = new Label();
            txtEmail = new TextBox();
            txtPassword = new TextBox();
            btnLogin = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvCiclos).BeginInit();
            SuspendLayout();
            // 
            // txtId
            // 
            txtId.Location = new Point(77, 121);
            txtId.Name = "txtId";
            txtId.PlaceholderText = "Id";
            txtId.Size = new Size(100, 23);
            txtId.TabIndex = 0;
            txtId.TextChanged += txtId_TextChanged;
            // 
            // txtDescripcion
            // 
            txtDescripcion.Location = new Point(395, 179);
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.PlaceholderText = "Descripcion";
            txtDescripcion.Size = new Size(100, 23);
            txtDescripcion.TabIndex = 1;
            txtDescripcion.TextChanged += txtDescripcion_TextChanged;
            // 
            // txtIdCiclo
            // 
            txtIdCiclo.Location = new Point(395, 121);
            txtIdCiclo.Name = "txtIdCiclo";
            txtIdCiclo.PlaceholderText = "IdCiclo";
            txtIdCiclo.Size = new Size(100, 23);
            txtIdCiclo.TabIndex = 2;
            txtIdCiclo.TextChanged += txtIdCiclo_TextChanged;
            // 
            // txtCiclo
            // 
            txtCiclo.Location = new Point(395, 150);
            txtCiclo.Name = "txtCiclo";
            txtCiclo.PlaceholderText = "Ciclo";
            txtCiclo.Size = new Size(100, 23);
            txtCiclo.TabIndex = 3;
            txtCiclo.TextChanged += txtCiclo_TextChanged;
            // 
            // btnGetAll
            // 
            btnGetAll.Location = new Point(77, 208);
            btnGetAll.Name = "btnGetAll";
            btnGetAll.Size = new Size(75, 23);
            btnGetAll.TabIndex = 4;
            btnGetAll.Text = "Get all";
            btnGetAll.UseVisualStyleBackColor = true;
            btnGetAll.Click += btnGetAll_Click_1;
            // 
            // btnGetById
            // 
            btnGetById.Location = new Point(183, 121);
            btnGetById.Name = "btnGetById";
            btnGetById.Size = new Size(75, 23);
            btnGetById.TabIndex = 5;
            btnGetById.Text = "Get by id";
            btnGetById.UseVisualStyleBackColor = true;
            btnGetById.Click += btnGetById_Click_1;
            // 
            // btnInsert
            // 
            btnInsert.Location = new Point(501, 121);
            btnInsert.Name = "btnInsert";
            btnInsert.Size = new Size(75, 23);
            btnInsert.TabIndex = 6;
            btnInsert.Text = "Insert";
            btnInsert.UseVisualStyleBackColor = true;
            btnInsert.Click += btnInsert_Click_1;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(264, 121);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 23);
            btnDelete.TabIndex = 7;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click_1;
            // 
            // dgvCiclos
            // 
            dgvCiclos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCiclos.Location = new Point(77, 237);
            dgvCiclos.Name = "dgvCiclos";
            dgvCiclos.Size = new Size(696, 165);
            dgvCiclos.TabIndex = 8;
            dgvCiclos.CellContentClick += dgvCiclos_CellContentClick;
            // 
            // Id
            // 
            Id.Location = new Point(0, 0);
            Id.Name = "Id";
            Id.Size = new Size(100, 23);
            Id.TabIndex = 1;
            // 
            // label1
            // 
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(100, 23);
            label1.TabIndex = 0;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(77, 26);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "Email";
            txtEmail.Size = new Size(206, 23);
            txtEmail.TabIndex = 9;
            txtEmail.TextChanged += txtEmail_TextChanged;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(77, 55);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Password";
            txtPassword.Size = new Size(206, 23);
            txtPassword.TabIndex = 10;
            txtPassword.TextChanged += txtPassword_TextChanged;
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(289, 26);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(75, 23);
            btnLogin.TabIndex = 11;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(799, 425);
            Controls.Add(btnLogin);
            Controls.Add(txtPassword);
            Controls.Add(txtEmail);
            Controls.Add(label1);
            Controls.Add(Id);
            Controls.Add(dgvCiclos);
            Controls.Add(btnDelete);
            Controls.Add(btnInsert);
            Controls.Add(btnGetById);
            Controls.Add(btnGetAll);
            Controls.Add(txtCiclo);
            Controls.Add(txtIdCiclo);
            Controls.Add(txtDescripcion);
            Controls.Add(txtId);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dgvCiclos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtId;
        private TextBox txtDescripcion;
        private TextBox txtIdCiclo;
        private TextBox txtCiclo;
        private Button btnGetAll;
        private Button btnGetById;
        private Button btnInsert;
        private Button btnDelete;
        private DataGridView dgvCiclos;
        private Label Id;
        private Label label1;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private Button btnLogin;
    }
}
