namespace HabitDisc.Telas
{
    partial class CategoriasControl
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
            btnNova = new FontAwesome.Sharp.IconButton();
            lblTitulo = new Label();
            lblDica = new Label();
            flpLista = new FlowLayoutPanel();
            pnlTopo.SuspendLayout();
            SuspendLayout();
            //
            // pnlTopo
            //
            pnlTopo.Controls.Add(btnNova);
            pnlTopo.Controls.Add(lblTitulo);
            pnlTopo.Dock = DockStyle.Top;
            pnlTopo.Location = new Point(32, 28);
            pnlTopo.Name = "pnlTopo";
            pnlTopo.Size = new Size(662, 70);
            pnlTopo.TabIndex = 0;
            //
            // btnNova
            //
            btnNova.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNova.IconChar = FontAwesome.Sharp.IconChar.Plus;
            btnNova.IconColor = Color.White;
            btnNova.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnNova.IconSize = 20;
            btnNova.ImageAlign = ContentAlignment.MiddleLeft;
            btnNova.Location = new Point(482, 14);
            btnNova.Name = "btnNova";
            btnNova.Padding = new Padding(10, 4, 12, 4);
            btnNova.Size = new Size(180, 40);
            btnNova.TabIndex = 1;
            btnNova.Text = "Nova categoria";
            btnNova.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnNova.Click += btnNova_Click;
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Newsreader", 28F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Categorias";
            //
            // lblDica
            //
            lblDica.AutoEllipsis = true;
            lblDica.Dock = DockStyle.Top;
            lblDica.Location = new Point(32, 98);
            lblDica.Name = "lblDica";
            lblDica.Size = new Size(662, 30);
            lblDica.TabIndex = 1;
            lblDica.Text = "Predefinidas não podem ser editadas nem excluídas.";
            lblDica.TextAlign = ContentAlignment.TopLeft;
            //
            // flpLista
            //
            flpLista.AutoScroll = true;
            flpLista.Dock = DockStyle.Fill;
            flpLista.FlowDirection = FlowDirection.TopDown;
            flpLista.Location = new Point(32, 128);
            flpLista.Name = "flpLista";
            flpLista.Size = new Size(662, 496);
            flpLista.TabIndex = 2;
            flpLista.WrapContents = false;
            //
            // CategoriasControl
            //
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(246, 244, 236);
            Controls.Add(flpLista);
            Controls.Add(lblDica);
            Controls.Add(pnlTopo);
            Name = "CategoriasControl";
            Padding = new Padding(32, 28, 32, 24);
            Size = new Size(726, 648);
            VisibleChanged += CategoriasControl_VisibleChanged;
            pnlTopo.ResumeLayout(false);
            pnlTopo.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private Panel pnlTopo;
        private FontAwesome.Sharp.IconButton btnNova;
        private Label lblTitulo;
        private Label lblDica;
        private FlowLayoutPanel flpLista;
    }
}
