namespace HabitDisc.Telas
{
    partial class EstatisticasControl
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
            lblTitulo = new Label();
            flpFiltros = new FlowLayoutPanel();
            cmbHabito = new ComboBox();
            rdoSemana = new RadioButton();
            rdoMes = new RadioButton();
            btnAnterior = new FontAwesome.Sharp.IconButton();
            lblPeriodo = new Label();
            btnProximo = new FontAwesome.Sharp.IconButton();
            pnlResultado = new Panel();
            flpCartoes = new FlowLayoutPanel();
            lblGrafico = new Label();
            grafico = new HabitDisc.Telas.Componentes.GraficoBarras();
            lblSemDados = new Label();
            flpFiltros.SuspendLayout();
            pnlResultado.SuspendLayout();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Newsreader", 28F, FontStyle.Bold);
            lblTitulo.Location = new Point(32, 28);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(662, 60);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Estatísticas";
            //
            // flpFiltros
            //
            flpFiltros.AutoSize = true;
            flpFiltros.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpFiltros.Controls.Add(cmbHabito);
            flpFiltros.Controls.Add(rdoSemana);
            flpFiltros.Controls.Add(rdoMes);
            flpFiltros.Controls.Add(btnAnterior);
            flpFiltros.Controls.Add(lblPeriodo);
            flpFiltros.Controls.Add(btnProximo);
            flpFiltros.Dock = DockStyle.Top;
            flpFiltros.Location = new Point(32, 88);
            flpFiltros.Name = "flpFiltros";
            flpFiltros.Padding = new Padding(0, 0, 0, 12);
            flpFiltros.Size = new Size(662, 50);
            flpFiltros.TabIndex = 1;
            //
            // cmbHabito
            //
            cmbHabito.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbHabito.Name = "cmbHabito";
            cmbHabito.Size = new Size(240, 28);
            cmbHabito.TabIndex = 0;
            cmbHabito.SelectedIndexChanged += Filtro_Alterado;
            //
            // rdoSemana
            //
            rdoSemana.AutoSize = true;
            rdoSemana.Checked = true;
            rdoSemana.Cursor = Cursors.Hand;
            rdoSemana.Name = "rdoSemana";
            rdoSemana.TabIndex = 1;
            rdoSemana.TabStop = true;
            rdoSemana.Text = "Semana";
            rdoSemana.CheckedChanged += Filtro_Alterado;
            //
            // rdoMes
            //
            rdoMes.AutoSize = true;
            rdoMes.Cursor = Cursors.Hand;
            rdoMes.Name = "rdoMes";
            rdoMes.TabIndex = 2;
            rdoMes.Text = "Mês";
            rdoMes.CheckedChanged += Filtro_Alterado;
            //
            // btnAnterior
            //
            btnAnterior.IconChar = FontAwesome.Sharp.IconChar.ChevronLeft;
            btnAnterior.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnAnterior.IconSize = 18;
            btnAnterior.Name = "btnAnterior";
            btnAnterior.Size = new Size(34, 34);
            btnAnterior.TabIndex = 3;
            btnAnterior.Click += btnAnterior_Click;
            //
            // lblPeriodo
            //
            lblPeriodo.AutoSize = true;
            lblPeriodo.Name = "lblPeriodo";
            lblPeriodo.TabIndex = 4;
            lblPeriodo.Text = "Período";
            //
            // btnProximo
            //
            btnProximo.IconChar = FontAwesome.Sharp.IconChar.ChevronRight;
            btnProximo.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnProximo.IconSize = 18;
            btnProximo.Name = "btnProximo";
            btnProximo.Size = new Size(34, 34);
            btnProximo.TabIndex = 5;
            btnProximo.Click += btnProximo_Click;
            //
            // pnlResultado (cartões + título do gráfico + gráfico; some quando não há dados)
            //
            pnlResultado.Controls.Add(grafico);
            pnlResultado.Controls.Add(lblGrafico);
            pnlResultado.Controls.Add(flpCartoes);
            pnlResultado.Dock = DockStyle.Fill;
            pnlResultado.Location = new Point(32, 138);
            pnlResultado.Name = "pnlResultado";
            pnlResultado.Size = new Size(662, 486);
            pnlResultado.TabIndex = 2;
            //
            // flpCartoes
            //
            flpCartoes.AutoSize = true;
            flpCartoes.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpCartoes.Dock = DockStyle.Top;
            flpCartoes.Location = new Point(0, 0);
            flpCartoes.Name = "flpCartoes";
            flpCartoes.Size = new Size(662, 10);
            flpCartoes.TabIndex = 0;
            //
            // lblGrafico
            //
            lblGrafico.Dock = DockStyle.Top;
            lblGrafico.Location = new Point(0, 10);
            lblGrafico.Name = "lblGrafico";
            lblGrafico.Size = new Size(662, 50);
            lblGrafico.TabIndex = 1;
            lblGrafico.Text = "Conclusão por dia";
            lblGrafico.TextAlign = ContentAlignment.BottomLeft;
            //
            // grafico
            //
            grafico.Dock = DockStyle.Fill;
            grafico.Location = new Point(0, 60);
            grafico.Name = "grafico";
            grafico.Size = new Size(662, 426);
            grafico.TabIndex = 2;
            //
            // lblSemDados
            //
            lblSemDados.Dock = DockStyle.Fill;
            lblSemDados.Location = new Point(32, 138);
            lblSemDados.Name = "lblSemDados";
            lblSemDados.Size = new Size(662, 486);
            lblSemDados.TabIndex = 3;
            lblSemDados.Text = "Ainda não há dados suficientes.\r\nRegistre seus hábitos por pelo menos 7 dias para ver as estatísticas.";
            lblSemDados.TextAlign = ContentAlignment.MiddleCenter;
            lblSemDados.Visible = false;
            //
            // EstatisticasControl
            //
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(246, 244, 236);
            Controls.Add(lblSemDados);
            Controls.Add(pnlResultado);
            Controls.Add(flpFiltros);
            Controls.Add(lblTitulo);
            Name = "EstatisticasControl";
            Padding = new Padding(32, 28, 32, 24);
            Size = new Size(726, 648);
            VisibleChanged += EstatisticasControl_VisibleChanged;
            flpFiltros.ResumeLayout(false);
            flpFiltros.PerformLayout();
            pnlResultado.ResumeLayout(false);
            pnlResultado.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private Label lblTitulo;
        private FlowLayoutPanel flpFiltros;
        private ComboBox cmbHabito;
        private RadioButton rdoSemana;
        private RadioButton rdoMes;
        private FontAwesome.Sharp.IconButton btnAnterior;
        private Label lblPeriodo;
        private FontAwesome.Sharp.IconButton btnProximo;
        private Panel pnlResultado;
        private FlowLayoutPanel flpCartoes;
        private Label lblGrafico;
        private HabitDisc.Telas.Componentes.GraficoBarras grafico;
        private Label lblSemDados;
    }
}
