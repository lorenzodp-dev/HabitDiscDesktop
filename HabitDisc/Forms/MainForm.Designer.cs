namespace HabitDisc.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        private void InitializeComponent()
        {
            pnlNavegacao = new Panel();
            btnConfiguracoes = new FontAwesome.Sharp.IconButton();
            btnCategorias = new FontAwesome.Sharp.IconButton();
            btnHumor = new FontAwesome.Sharp.IconButton();
            btnEstatisticas = new FontAwesome.Sharp.IconButton();
            btnHabitos = new FontAwesome.Sharp.IconButton();
            btnHome = new FontAwesome.Sharp.IconButton();
            lblLogo = new Label();
            pnlConteudo = new Panel();
            pnlNavegacao.SuspendLayout();
            SuspendLayout();
            //
            // pnlNavegacao
            //
            pnlNavegacao.BackColor = Color.FromArgb(35, 77, 51);
            pnlNavegacao.Controls.Add(btnConfiguracoes);
            pnlNavegacao.Controls.Add(btnCategorias);
            pnlNavegacao.Controls.Add(btnHumor);
            pnlNavegacao.Controls.Add(btnEstatisticas);
            pnlNavegacao.Controls.Add(btnHabitos);
            pnlNavegacao.Controls.Add(btnHome);
            pnlNavegacao.Controls.Add(lblLogo);
            pnlNavegacao.Dock = DockStyle.Left;
            pnlNavegacao.Location = new Point(0, 0);
            pnlNavegacao.Name = "pnlNavegacao";
            pnlNavegacao.Size = new Size(244, 648);
            pnlNavegacao.TabIndex = 1;
            //
            // btnConfiguracoes
            //
            btnConfiguracoes.BackColor = Color.FromArgb(35, 77, 51);
            btnConfiguracoes.Cursor = Cursors.Hand;
            btnConfiguracoes.Dock = DockStyle.Bottom;
            btnConfiguracoes.FlatAppearance.BorderSize = 0;
            btnConfiguracoes.FlatAppearance.MouseOverBackColor = Color.FromArgb(47, 107, 69);
            btnConfiguracoes.FlatStyle = FlatStyle.Flat;
            btnConfiguracoes.Font = new Font("Microsoft Sans Serif", 13F, FontStyle.Bold);
            btnConfiguracoes.ForeColor = Color.White;
            btnConfiguracoes.IconChar = FontAwesome.Sharp.IconChar.Gear;
            btnConfiguracoes.IconColor = Color.White;
            btnConfiguracoes.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnConfiguracoes.IconSize = 28;
            btnConfiguracoes.ImageAlign = ContentAlignment.MiddleLeft;
            btnConfiguracoes.Location = new Point(0, 600);
            btnConfiguracoes.Name = "btnConfiguracoes";
            btnConfiguracoes.Padding = new Padding(22, 0, 0, 0);
            btnConfiguracoes.Size = new Size(244, 48);
            btnConfiguracoes.TabIndex = 6;
            btnConfiguracoes.Text = "Configurações";
            btnConfiguracoes.TextAlign = ContentAlignment.MiddleLeft;
            btnConfiguracoes.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnConfiguracoes.UseVisualStyleBackColor = false;
            btnConfiguracoes.Click += btnConfiguracoes_Click;
            //
            // btnCategorias
            //
            btnCategorias.BackColor = Color.FromArgb(35, 77, 51);
            btnCategorias.Cursor = Cursors.Hand;
            btnCategorias.Dock = DockStyle.Top;
            btnCategorias.FlatAppearance.BorderSize = 0;
            btnCategorias.FlatAppearance.MouseOverBackColor = Color.FromArgb(47, 107, 69);
            btnCategorias.FlatStyle = FlatStyle.Flat;
            btnCategorias.Font = new Font("Microsoft Sans Serif", 13F, FontStyle.Bold);
            btnCategorias.ForeColor = Color.White;
            btnCategorias.IconChar = FontAwesome.Sharp.IconChar.List12;
            btnCategorias.IconColor = Color.White;
            btnCategorias.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnCategorias.IconSize = 28;
            btnCategorias.ImageAlign = ContentAlignment.MiddleLeft;
            btnCategorias.Location = new Point(0, 312);
            btnCategorias.Name = "btnCategorias";
            btnCategorias.Padding = new Padding(22, 0, 0, 0);
            btnCategorias.Size = new Size(244, 48);
            btnCategorias.TabIndex = 5;
            btnCategorias.Text = "Categorias";
            btnCategorias.TextAlign = ContentAlignment.MiddleLeft;
            btnCategorias.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCategorias.UseVisualStyleBackColor = false;
            btnCategorias.Click += btnCategorias_Click;
            //
            // btnHumor
            //
            btnHumor.BackColor = Color.FromArgb(35, 77, 51);
            btnHumor.Cursor = Cursors.Hand;
            btnHumor.Dock = DockStyle.Top;
            btnHumor.FlatAppearance.BorderSize = 0;
            btnHumor.FlatAppearance.MouseOverBackColor = Color.FromArgb(47, 107, 69);
            btnHumor.FlatStyle = FlatStyle.Flat;
            btnHumor.Font = new Font("Microsoft Sans Serif", 13F, FontStyle.Bold);
            btnHumor.ForeColor = Color.White;
            btnHumor.IconChar = FontAwesome.Sharp.IconChar.FaceSmile;
            btnHumor.IconColor = Color.White;
            btnHumor.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnHumor.IconSize = 28;
            btnHumor.ImageAlign = ContentAlignment.MiddleLeft;
            btnHumor.Location = new Point(0, 264);
            btnHumor.Name = "btnHumor";
            btnHumor.Padding = new Padding(22, 0, 0, 0);
            btnHumor.Size = new Size(244, 48);
            btnHumor.TabIndex = 4;
            btnHumor.Text = "Humor";
            btnHumor.TextAlign = ContentAlignment.MiddleLeft;
            btnHumor.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnHumor.UseVisualStyleBackColor = false;
            btnHumor.Click += btnHumor_Click;
            //
            // btnEstatisticas
            //
            btnEstatisticas.BackColor = Color.FromArgb(35, 77, 51);
            btnEstatisticas.Cursor = Cursors.Hand;
            btnEstatisticas.Dock = DockStyle.Top;
            btnEstatisticas.FlatAppearance.BorderSize = 0;
            btnEstatisticas.FlatAppearance.MouseOverBackColor = Color.FromArgb(47, 107, 69);
            btnEstatisticas.FlatStyle = FlatStyle.Flat;
            btnEstatisticas.Font = new Font("Microsoft Sans Serif", 13F, FontStyle.Bold);
            btnEstatisticas.ForeColor = Color.White;
            btnEstatisticas.IconChar = FontAwesome.Sharp.IconChar.ChartLine;
            btnEstatisticas.IconColor = Color.White;
            btnEstatisticas.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnEstatisticas.IconSize = 28;
            btnEstatisticas.ImageAlign = ContentAlignment.MiddleLeft;
            btnEstatisticas.Location = new Point(0, 216);
            btnEstatisticas.Name = "btnEstatisticas";
            btnEstatisticas.Padding = new Padding(22, 0, 0, 0);
            btnEstatisticas.Size = new Size(244, 48);
            btnEstatisticas.TabIndex = 3;
            btnEstatisticas.Text = "Estatísticas";
            btnEstatisticas.TextAlign = ContentAlignment.MiddleLeft;
            btnEstatisticas.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnEstatisticas.UseVisualStyleBackColor = false;
            btnEstatisticas.Click += btnEstatisticas_Click;
            //
            // btnHabitos
            //
            btnHabitos.BackColor = Color.FromArgb(35, 77, 51);
            btnHabitos.Cursor = Cursors.Hand;
            btnHabitos.Dock = DockStyle.Top;
            btnHabitos.FlatAppearance.BorderSize = 0;
            btnHabitos.FlatAppearance.MouseOverBackColor = Color.FromArgb(47, 107, 69);
            btnHabitos.FlatStyle = FlatStyle.Flat;
            btnHabitos.Font = new Font("Microsoft Sans Serif", 13F, FontStyle.Bold);
            btnHabitos.ForeColor = Color.White;
            btnHabitos.IconChar = FontAwesome.Sharp.IconChar.Fire;
            btnHabitos.IconColor = Color.White;
            btnHabitos.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnHabitos.IconSize = 28;
            btnHabitos.ImageAlign = ContentAlignment.MiddleLeft;
            btnHabitos.Location = new Point(0, 168);
            btnHabitos.Name = "btnHabitos";
            btnHabitos.Padding = new Padding(22, 0, 0, 0);
            btnHabitos.Size = new Size(244, 48);
            btnHabitos.TabIndex = 2;
            btnHabitos.Text = "Hábitos";
            btnHabitos.TextAlign = ContentAlignment.MiddleLeft;
            btnHabitos.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnHabitos.UseVisualStyleBackColor = false;
            btnHabitos.Click += btnHabitos_Click;
            //
            // btnHome
            //
            btnHome.BackColor = Color.FromArgb(35, 77, 51);
            btnHome.Cursor = Cursors.Hand;
            btnHome.Dock = DockStyle.Top;
            btnHome.FlatAppearance.BorderSize = 0;
            btnHome.FlatAppearance.MouseOverBackColor = Color.FromArgb(47, 107, 69);
            btnHome.FlatStyle = FlatStyle.Flat;
            btnHome.Font = new Font("Microsoft Sans Serif", 13F, FontStyle.Bold);
            btnHome.ForeColor = Color.White;
            btnHome.IconChar = FontAwesome.Sharp.IconChar.House;
            btnHome.IconColor = Color.White;
            btnHome.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnHome.IconSize = 28;
            btnHome.ImageAlign = ContentAlignment.MiddleLeft;
            btnHome.Location = new Point(0, 120);
            btnHome.Name = "btnHome";
            btnHome.Padding = new Padding(22, 0, 0, 0);
            btnHome.Size = new Size(244, 48);
            btnHome.TabIndex = 1;
            btnHome.Text = "Home";
            btnHome.TextAlign = ContentAlignment.MiddleLeft;
            btnHome.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnHome.UseVisualStyleBackColor = false;
            btnHome.Click += btnHome_Click;
            //
            // lblLogo
            //
            lblLogo.Dock = DockStyle.Top;
            lblLogo.Font = new Font("Microsoft Sans Serif", 20F, FontStyle.Bold);
            lblLogo.ForeColor = Color.White;
            lblLogo.Location = new Point(0, 0);
            lblLogo.Name = "lblLogo";
            lblLogo.Size = new Size(244, 120);
            lblLogo.TabIndex = 0;
            lblLogo.Text = "HabitDisc";
            lblLogo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // pnlConteudo
            //
            pnlConteudo.BackColor = Color.FromArgb(246, 244, 236);
            pnlConteudo.Dock = DockStyle.Fill;
            pnlConteudo.Location = new Point(244, 0);
            pnlConteudo.Name = "pnlConteudo";
            pnlConteudo.Size = new Size(668, 648);
            pnlConteudo.TabIndex = 0;
            //
            // MainForm
            //
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(912, 648);
            Controls.Add(pnlConteudo);
            Controls.Add(pnlNavegacao);
            MinimumSize = new Size(800, 560);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "HabitDisc";
            Shown += MainForm_Shown;
            pnlNavegacao.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlNavegacao;
        private System.Windows.Forms.Label lblLogo;
        private FontAwesome.Sharp.IconButton btnHome;
        private FontAwesome.Sharp.IconButton btnHabitos;
        private FontAwesome.Sharp.IconButton btnEstatisticas;
        private FontAwesome.Sharp.IconButton btnHumor;
        private FontAwesome.Sharp.IconButton btnCategorias;
        private FontAwesome.Sharp.IconButton btnConfiguracoes;
        private System.Windows.Forms.Panel pnlConteudo;
    }
}
