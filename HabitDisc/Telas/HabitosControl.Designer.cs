namespace HabitDisc.Telas
{
    partial class HabitosControl
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

        #region Código gerado pelo Designer de Componentes

        private void InitializeComponent()
        {
            pnlTopo = new Panel();
            btnNovo = new FontAwesome.Sharp.IconButton();
            lblTitulo = new Label();
            flpLista = new FlowLayoutPanel();
            lblVazio = new Label();
            pnlTopo.SuspendLayout();
            SuspendLayout();
            //
            // pnlTopo
            //
            pnlTopo.Controls.Add(btnNovo);
            pnlTopo.Controls.Add(lblTitulo);
            pnlTopo.Dock = DockStyle.Top;
            pnlTopo.Location = new Point(32, 28);
            pnlTopo.Name = "pnlTopo";
            pnlTopo.Size = new Size(662, 70);
            pnlTopo.TabIndex = 0;
            //
            // btnNovo
            //
            btnNovo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNovo.IconChar = FontAwesome.Sharp.IconChar.Plus;
            btnNovo.IconColor = Color.White;
            btnNovo.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnNovo.IconSize = 20;
            btnNovo.ImageAlign = ContentAlignment.MiddleLeft;
            btnNovo.Location = new Point(502, 14);
            btnNovo.Name = "btnNovo";
            btnNovo.Padding = new Padding(10, 4, 12, 4);
            btnNovo.Size = new Size(160, 40);
            btnNovo.TabIndex = 1;
            btnNovo.Text = "Novo hábito";
            btnNovo.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnNovo.Click += btnNovo_Click;
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Newsreader", 28F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Hábitos";
            //
            // flpLista
            //
            flpLista.AutoScroll = true;
            flpLista.Dock = DockStyle.Fill;
            flpLista.FlowDirection = FlowDirection.TopDown;
            flpLista.Location = new Point(32, 98);
            flpLista.Name = "flpLista";
            flpLista.Padding = new Padding(0, 10, 0, 0);
            flpLista.Size = new Size(662, 526);
            flpLista.TabIndex = 1;
            flpLista.WrapContents = false;
            //
            // lblVazio
            //
            lblVazio.Dock = DockStyle.Fill;
            lblVazio.Location = new Point(32, 98);
            lblVazio.Name = "lblVazio";
            lblVazio.Size = new Size(662, 526);
            lblVazio.TabIndex = 2;
            lblVazio.Text = "Você ainda não tem hábitos.\r\nClique em \"Novo hábito\" para criar o primeiro.";
            lblVazio.TextAlign = ContentAlignment.MiddleCenter;
            lblVazio.Visible = false;
            //
            // HabitosControl
            //
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(246, 244, 236);
            Controls.Add(lblVazio);
            Controls.Add(flpLista);
            Controls.Add(pnlTopo);
            Name = "HabitosControl";
            Padding = new Padding(32, 28, 32, 24);
            Size = new Size(726, 648);
            VisibleChanged += HabitosControl_VisibleChanged;
            pnlTopo.ResumeLayout(false);
            pnlTopo.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private Panel pnlTopo;
        private FontAwesome.Sharp.IconButton btnNovo;
        private Label lblTitulo;
        private FlowLayoutPanel flpLista;
        private Label lblVazio;
    }
}
