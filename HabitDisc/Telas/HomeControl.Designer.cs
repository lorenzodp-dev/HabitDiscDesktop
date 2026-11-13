namespace HabitDisc.Telas
{
    partial class HomeControl
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
            this.pnlTopo = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblData = new System.Windows.Forms.Label();
            this.pnlProgresso = new System.Windows.Forms.Panel();
            this.lblProgresso = new System.Windows.Forms.Label();
            this.lblPercentual = new System.Windows.Forms.Label();
            this.pnlBarraFundo = new System.Windows.Forms.Panel();
            this.pnlBarraPreenchida = new System.Windows.Forms.Panel();
            this.lblHabitosHoje = new System.Windows.Forms.Label();
            this.flpHabitos = new System.Windows.Forms.FlowLayoutPanel();
            this.lblVazio = new System.Windows.Forms.Label();
            this.pnlTopo.SuspendLayout();
            this.pnlProgresso.SuspendLayout();
            this.pnlBarraFundo.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTopo
            // 
            this.pnlTopo.BackColor = System.Drawing.Color.Transparent;
            this.pnlTopo.Controls.Add(this.lblTitulo);
            this.pnlTopo.Controls.Add(this.lblData);
            this.pnlTopo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopo.Location = new System.Drawing.Point(32, 28);
            this.pnlTopo.Name = "pnlTopo";
            this.pnlTopo.Size = new System.Drawing.Size(662, 84);
            this.pnlTopo.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.Font = new System.Drawing.Font("Newsreader", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(35, 77, 51);
            this.lblTitulo.Location = new System.Drawing.Point(0, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(400, 46);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Hoje";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblData
            // 
            this.lblData.Font = new System.Drawing.Font("Newsreader", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblData.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.lblData.Location = new System.Drawing.Point(2, 48);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(500, 26);
            this.lblData.TabIndex = 1;
            this.lblData.Text = "Data";
            this.lblData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlProgresso
            // 
            this.pnlProgresso.BackColor = System.Drawing.Color.FromArgb(35, 77, 51);
            this.pnlProgresso.Controls.Add(this.lblProgresso);
            this.pnlProgresso.Controls.Add(this.lblPercentual);
            this.pnlProgresso.Controls.Add(this.pnlBarraFundo);
            this.pnlProgresso.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlProgresso.Location = new System.Drawing.Point(32, 112);
            this.pnlProgresso.Name = "pnlProgresso";
            this.pnlProgresso.Size = new System.Drawing.Size(662, 100);
            this.pnlProgresso.TabIndex = 1;
            // 
            // lblProgresso
            // 
            this.lblProgresso.BackColor = System.Drawing.Color.Transparent;
            this.lblProgresso.Font = new System.Drawing.Font("Newsreader", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblProgresso.ForeColor = System.Drawing.Color.White;
            this.lblProgresso.Location = new System.Drawing.Point(22, 16);
            this.lblProgresso.Name = "lblProgresso";
            this.lblProgresso.Size = new System.Drawing.Size(440, 30);
            this.lblProgresso.TabIndex = 0;
            this.lblProgresso.Text = "0 de 0 hábitos concluídos";
            this.lblProgresso.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPercentual
            // 
            this.lblPercentual.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPercentual.BackColor = System.Drawing.Color.Transparent;
            this.lblPercentual.Font = new System.Drawing.Font("Newsreader", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblPercentual.ForeColor = System.Drawing.Color.FromArgb(111, 196, 116);
            this.lblPercentual.Location = new System.Drawing.Point(532, 12);
            this.lblPercentual.Name = "lblPercentual";
            this.lblPercentual.Size = new System.Drawing.Size(108, 38);
            this.lblPercentual.TabIndex = 1;
            this.lblPercentual.Text = "0%";
            this.lblPercentual.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlBarraFundo
            // 
            this.pnlBarraFundo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBarraFundo.BackColor = System.Drawing.Color.FromArgb(47, 107, 69);
            this.pnlBarraFundo.Controls.Add(this.pnlBarraPreenchida);
            this.pnlBarraFundo.Location = new System.Drawing.Point(22, 64);
            this.pnlBarraFundo.Name = "pnlBarraFundo";
            this.pnlBarraFundo.Size = new System.Drawing.Size(618, 14);
            this.pnlBarraFundo.TabIndex = 2;
            this.pnlBarraFundo.Resize += new System.EventHandler(this.pnlBarraFundo_Resize);
            // 
            // pnlBarraPreenchida
            // 
            this.pnlBarraPreenchida.BackColor = System.Drawing.Color.FromArgb(111, 196, 116);
            this.pnlBarraPreenchida.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlBarraPreenchida.Location = new System.Drawing.Point(0, 0);
            this.pnlBarraPreenchida.Name = "pnlBarraPreenchida";
            this.pnlBarraPreenchida.Size = new System.Drawing.Size(0, 14);
            this.pnlBarraPreenchida.TabIndex = 0;
            // 
            // lblHabitosHoje
            // 
            this.lblHabitosHoje.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHabitosHoje.Font = new System.Drawing.Font("Newsreader", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblHabitosHoje.ForeColor = System.Drawing.Color.FromArgb(35, 77, 51);
            this.lblHabitosHoje.Location = new System.Drawing.Point(32, 212);
            this.lblHabitosHoje.Name = "lblHabitosHoje";
            this.lblHabitosHoje.Size = new System.Drawing.Size(662, 60);
            this.lblHabitosHoje.TabIndex = 2;
            this.lblHabitosHoje.Text = "Hábitos de hoje";
            this.lblHabitosHoje.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // flpHabitos
            // 
            this.flpHabitos.AutoScroll = true;
            this.flpHabitos.BackColor = System.Drawing.Color.Transparent;
            this.flpHabitos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpHabitos.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpHabitos.Location = new System.Drawing.Point(32, 272);
            this.flpHabitos.Name = "flpHabitos";
            this.flpHabitos.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.flpHabitos.Size = new System.Drawing.Size(662, 352);
            this.flpHabitos.TabIndex = 3;
            this.flpHabitos.WrapContents = false;
            this.flpHabitos.ClientSizeChanged += new System.EventHandler(this.flpHabitos_ClientSizeChanged);
            // 
            // lblVazio
            // 
            this.lblVazio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVazio.Font = new System.Drawing.Font("Newsreader", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblVazio.ForeColor = System.Drawing.Color.FromArgb(120, 120, 120);
            this.lblVazio.Location = new System.Drawing.Point(32, 272);
            this.lblVazio.Name = "lblVazio";
            this.lblVazio.Size = new System.Drawing.Size(662, 352);
            this.lblVazio.TabIndex = 4;
            this.lblVazio.Text = "Você ainda não tem hábitos cadastrados.\r\nCrie o primeiro na tela Hábitos.";
            this.lblVazio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblVazio.Visible = false;
            // 
            // HomeControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(246, 244, 236);
            this.Controls.Add(this.lblVazio);
            this.Controls.Add(this.flpHabitos);
            this.Controls.Add(this.lblHabitosHoje);
            this.Controls.Add(this.pnlProgresso);
            this.Controls.Add(this.pnlTopo);
            this.Name = "HomeControl";
            this.Padding = new System.Windows.Forms.Padding(32, 28, 32, 24);
            this.Size = new System.Drawing.Size(726, 648);
            this.VisibleChanged += new System.EventHandler(this.HomeControl_VisibleChanged);
            this.pnlTopo.ResumeLayout(false);
            this.pnlProgresso.ResumeLayout(false);
            this.pnlBarraFundo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTopo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblData;
        private System.Windows.Forms.Panel pnlProgresso;
        private System.Windows.Forms.Label lblProgresso;
        private System.Windows.Forms.Label lblPercentual;
        private System.Windows.Forms.Panel pnlBarraFundo;
        private System.Windows.Forms.Panel pnlBarraPreenchida;
        private System.Windows.Forms.Label lblHabitosHoje;
        private System.Windows.Forms.FlowLayoutPanel flpHabitos;
        private System.Windows.Forms.Label lblVazio;
    }
}