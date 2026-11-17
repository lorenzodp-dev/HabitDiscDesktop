namespace HabitDisc.Telas
{
    partial class HumorControl
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
            lblPergunta = new Label();
            flpOpcoes = new FlowLayoutPanel();
            txtObservacao = new TextBox();
            pnlAcoes = new Panel();
            lblStatus = new Label();
            btnSalvar = new Button();
            lblHistorico = new Label();
            flpHistorico = new FlowLayoutPanel();
            lblHistoricoVazio = new Label();
            pnlAcoes.SuspendLayout();
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
            lblTitulo.Text = "Humor";
            lblTitulo.TextAlign = ContentAlignment.TopLeft;
            //
            // lblPergunta
            //
            lblPergunta.Dock = DockStyle.Top;
            lblPergunta.Location = new Point(32, 88);
            lblPergunta.Name = "lblPergunta";
            lblPergunta.Size = new Size(662, 40);
            lblPergunta.TabIndex = 1;
            lblPergunta.Text = "Como você está hoje?";
            lblPergunta.TextAlign = ContentAlignment.TopLeft;
            //
            // flpOpcoes
            //
            flpOpcoes.AutoSize = true;
            flpOpcoes.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpOpcoes.Dock = DockStyle.Top;
            flpOpcoes.Location = new Point(32, 128);
            flpOpcoes.Name = "flpOpcoes";
            flpOpcoes.Padding = new Padding(0, 0, 0, 12);
            flpOpcoes.Size = new Size(662, 12);
            flpOpcoes.TabIndex = 2;
            //
            // txtObservacao
            //
            txtObservacao.Dock = DockStyle.Top;
            txtObservacao.Location = new Point(32, 140);
            txtObservacao.Multiline = true;
            txtObservacao.Name = "txtObservacao";
            txtObservacao.PlaceholderText = "Observação (opcional): como foi o seu dia?";
            txtObservacao.ScrollBars = ScrollBars.Vertical;
            txtObservacao.Size = new Size(662, 70);
            txtObservacao.TabIndex = 3;
            //
            // pnlAcoes
            //
            pnlAcoes.Controls.Add(lblStatus);
            pnlAcoes.Controls.Add(btnSalvar);
            pnlAcoes.Dock = DockStyle.Top;
            pnlAcoes.Location = new Point(32, 210);
            pnlAcoes.Name = "pnlAcoes";
            pnlAcoes.Size = new Size(662, 64);
            pnlAcoes.TabIndex = 4;
            //
            // lblStatus
            //
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(200, 24);
            lblStatus.Name = "lblStatus";
            lblStatus.TabIndex = 1;
            //
            // btnSalvar
            //
            btnSalvar.AutoSize = true;
            btnSalvar.Location = new Point(0, 12);
            btnSalvar.Name = "btnSalvar";
            btnSalvar.Padding = new Padding(10, 4, 10, 4);
            btnSalvar.TabIndex = 0;
            btnSalvar.Text = "Registrar humor";
            btnSalvar.Click += btnSalvar_Click;
            //
            // lblHistorico
            //
            lblHistorico.Dock = DockStyle.Top;
            lblHistorico.Location = new Point(32, 274);
            lblHistorico.Name = "lblHistorico";
            lblHistorico.Size = new Size(662, 44);
            lblHistorico.TabIndex = 5;
            lblHistorico.Text = "Histórico";
            lblHistorico.TextAlign = ContentAlignment.BottomLeft;
            //
            // flpHistorico
            //
            flpHistorico.AutoScroll = true;
            flpHistorico.Dock = DockStyle.Fill;
            flpHistorico.FlowDirection = FlowDirection.TopDown;
            flpHistorico.Location = new Point(32, 318);
            flpHistorico.Name = "flpHistorico";
            flpHistorico.Padding = new Padding(0, 10, 0, 0);
            flpHistorico.Size = new Size(662, 306);
            flpHistorico.TabIndex = 6;
            flpHistorico.WrapContents = false;
            //
            // lblHistoricoVazio
            //
            lblHistoricoVazio.Dock = DockStyle.Fill;
            lblHistoricoVazio.Location = new Point(32, 318);
            lblHistoricoVazio.Name = "lblHistoricoVazio";
            lblHistoricoVazio.Size = new Size(662, 306);
            lblHistoricoVazio.TabIndex = 7;
            lblHistoricoVazio.Text = "Nenhum registro ainda.";
            lblHistoricoVazio.TextAlign = ContentAlignment.MiddleCenter;
            lblHistoricoVazio.Visible = false;
            //
            // HumorControl
            //
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(246, 244, 236);
            Controls.Add(lblHistoricoVazio);
            Controls.Add(flpHistorico);
            Controls.Add(lblHistorico);
            Controls.Add(pnlAcoes);
            Controls.Add(txtObservacao);
            Controls.Add(flpOpcoes);
            Controls.Add(lblPergunta);
            Controls.Add(lblTitulo);
            Name = "HumorControl";
            Padding = new Padding(32, 28, 32, 24);
            Size = new Size(726, 648);
            VisibleChanged += HumorControl_VisibleChanged;
            pnlAcoes.ResumeLayout(false);
            pnlAcoes.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private Label lblTitulo;
        private Label lblPergunta;
        private FlowLayoutPanel flpOpcoes;
        private TextBox txtObservacao;
        private Panel pnlAcoes;
        private Label lblStatus;
        private Button btnSalvar;
        private Label lblHistorico;
        private FlowLayoutPanel flpHistorico;
        private Label lblHistoricoVazio;
    }
}
