namespace HabitDisc.Telas
{
    partial class ConfiguracoesControl
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
            flpConteudo = new FlowLayoutPanel();
            lblTitulo = new Label();
            lblCor = new Label();
            flpPaletas = new FlowLayoutPanel();
            lblFonte = new Label();
            flpFonte = new FlowLayoutPanel();
            rdoPequena = new RadioButton();
            rdoMedia = new RadioButton();
            rdoGrande = new RadioButton();
            lblAparencia = new Label();
            chkModoEscuro = new CheckBox();
            btnRestaurar = new Button();
            flpConteudo.SuspendLayout();
            flpFonte.SuspendLayout();
            SuspendLayout();
            //
            // flpConteudo
            //
            flpConteudo.AutoScroll = true;
            flpConteudo.Controls.Add(lblTitulo);
            flpConteudo.Controls.Add(lblCor);
            flpConteudo.Controls.Add(flpPaletas);
            flpConteudo.Controls.Add(lblFonte);
            flpConteudo.Controls.Add(flpFonte);
            flpConteudo.Controls.Add(lblAparencia);
            flpConteudo.Controls.Add(chkModoEscuro);
            flpConteudo.Controls.Add(btnRestaurar);
            flpConteudo.Dock = DockStyle.Fill;
            flpConteudo.FlowDirection = FlowDirection.TopDown;
            flpConteudo.Location = new Point(32, 28);
            flpConteudo.Name = "flpConteudo";
            flpConteudo.Size = new Size(662, 596);
            flpConteudo.TabIndex = 0;
            flpConteudo.WrapContents = false;
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Newsreader", 28F, FontStyle.Bold);
            lblTitulo.Margin = new Padding(0, 0, 0, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Configurações";
            //
            // lblCor
            //
            lblCor.AutoSize = true;
            lblCor.Font = new Font("Newsreader", 16F, FontStyle.Bold);
            lblCor.Margin = new Padding(0, 12, 0, 8);
            lblCor.Name = "lblCor";
            lblCor.TabIndex = 1;
            lblCor.Text = "Cor do tema";
            //
            // flpPaletas
            //
            flpPaletas.AutoSize = true;
            flpPaletas.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpPaletas.Margin = new Padding(0, 0, 0, 8);
            flpPaletas.Name = "flpPaletas";
            flpPaletas.TabIndex = 2;
            flpPaletas.WrapContents = false;
            //
            // lblFonte
            //
            lblFonte.AutoSize = true;
            lblFonte.Font = new Font("Newsreader", 16F, FontStyle.Bold);
            lblFonte.Margin = new Padding(0, 12, 0, 8);
            lblFonte.Name = "lblFonte";
            lblFonte.TabIndex = 3;
            lblFonte.Text = "Tamanho da fonte";
            //
            // flpFonte
            //
            flpFonte.AutoSize = true;
            flpFonte.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpFonte.Controls.Add(rdoPequena);
            flpFonte.Controls.Add(rdoMedia);
            flpFonte.Controls.Add(rdoGrande);
            flpFonte.Margin = new Padding(0, 0, 0, 8);
            flpFonte.Name = "flpFonte";
            flpFonte.TabIndex = 4;
            flpFonte.WrapContents = false;
            //
            // rdoPequena
            //
            rdoPequena.AutoSize = true;
            rdoPequena.Cursor = Cursors.Hand;
            rdoPequena.Font = new Font("Newsreader", 13F);
            rdoPequena.Margin = new Padding(0, 0, 24, 0);
            rdoPequena.Name = "rdoPequena";
            rdoPequena.TabIndex = 0;
            rdoPequena.Text = "Pequena";
            rdoPequena.CheckedChanged += rdoFonte_CheckedChanged;
            //
            // rdoMedia
            //
            rdoMedia.AutoSize = true;
            rdoMedia.Cursor = Cursors.Hand;
            rdoMedia.Font = new Font("Newsreader", 13F);
            rdoMedia.Margin = new Padding(0, 0, 24, 0);
            rdoMedia.Name = "rdoMedia";
            rdoMedia.TabIndex = 1;
            rdoMedia.Text = "Média";
            rdoMedia.CheckedChanged += rdoFonte_CheckedChanged;
            //
            // rdoGrande
            //
            rdoGrande.AutoSize = true;
            rdoGrande.Cursor = Cursors.Hand;
            rdoGrande.Font = new Font("Newsreader", 13F);
            rdoGrande.Margin = new Padding(0, 0, 24, 0);
            rdoGrande.Name = "rdoGrande";
            rdoGrande.TabIndex = 2;
            rdoGrande.Text = "Grande";
            rdoGrande.CheckedChanged += rdoFonte_CheckedChanged;
            //
            // lblAparencia
            //
            lblAparencia.AutoSize = true;
            lblAparencia.Font = new Font("Newsreader", 16F, FontStyle.Bold);
            lblAparencia.Margin = new Padding(0, 12, 0, 8);
            lblAparencia.Name = "lblAparencia";
            lblAparencia.TabIndex = 5;
            lblAparencia.Text = "Aparência";
            //
            // chkModoEscuro
            //
            chkModoEscuro.AutoSize = true;
            chkModoEscuro.Cursor = Cursors.Hand;
            chkModoEscuro.Font = new Font("Newsreader", 13F);
            chkModoEscuro.Margin = new Padding(0, 0, 0, 8);
            chkModoEscuro.Name = "chkModoEscuro";
            chkModoEscuro.TabIndex = 6;
            chkModoEscuro.Text = "Modo escuro";
            chkModoEscuro.CheckedChanged += chkModoEscuro_CheckedChanged;
            //
            // btnRestaurar
            //
            btnRestaurar.AutoSize = true;
            btnRestaurar.Cursor = Cursors.Hand;
            btnRestaurar.FlatAppearance.BorderSize = 0;
            btnRestaurar.FlatStyle = FlatStyle.Flat;
            btnRestaurar.Font = new Font("Newsreader", 12F, FontStyle.Bold);
            btnRestaurar.Margin = new Padding(0, 24, 0, 0);
            btnRestaurar.Name = "btnRestaurar";
            btnRestaurar.Padding = new Padding(12, 6, 12, 6);
            btnRestaurar.TabIndex = 7;
            btnRestaurar.Text = "Restaurar padrão";
            btnRestaurar.UseVisualStyleBackColor = false;
            btnRestaurar.Click += btnRestaurar_Click;
            //
            // ConfiguracoesControl
            //
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(246, 244, 236);
            Controls.Add(flpConteudo);
            Name = "ConfiguracoesControl";
            Padding = new Padding(32, 28, 32, 24);
            Size = new Size(726, 648);
            VisibleChanged += ConfiguracoesControl_VisibleChanged;
            flpConteudo.ResumeLayout(false);
            flpConteudo.PerformLayout();
            flpFonte.ResumeLayout(false);
            flpFonte.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private FlowLayoutPanel flpConteudo;
        private Label lblTitulo;
        private Label lblCor;
        private FlowLayoutPanel flpPaletas;
        private Label lblFonte;
        private FlowLayoutPanel flpFonte;
        private RadioButton rdoPequena;
        private RadioButton rdoMedia;
        private RadioButton rdoGrande;
        private Label lblAparencia;
        private CheckBox chkModoEscuro;
        private Button btnRestaurar;
    }
}
