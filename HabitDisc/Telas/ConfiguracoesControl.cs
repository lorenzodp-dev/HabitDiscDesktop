using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using HabitDisc.Helpers;
using HabitDisc.Services;

namespace HabitDisc.Telas
{
    // Configurações de aparência: cor do tema, tamanho da fonte e modo escuro.
    // Cada mudança vale na hora (todas as telas se repintam pelo evento Tema.Alterado)
    // e é salva em %AppData%\HabitDisc\preferencias.json pelo PreferenciasService.
    public partial class ConfiguracoesControl : UserControl
    {
        private readonly PreferenciasService _service = new PreferenciasService();

        // true enquanto os controles são preenchidos por código:
        // evita que o CheckedChanged salve algo que o usuário não mexeu
        private bool _carregando;

        public ConfiguracoesControl()
        {
            InitializeComponent();

            CriarBotoesPaleta();

            AplicarTema();
            Tema.Alterado += Tema_Alterado;
            Disposed += delegate { Tema.Alterado -= Tema_Alterado; };
        }

        // ---------- Eventos ----------

        private void ConfiguracoesControl_VisibleChanged(object? sender, EventArgs e)
        {
            if (Visible && !DesignMode)
            {
                MostrarValoresAtuais();
            }
        }

        private void Tema_Alterado(object? sender, EventArgs e)
        {
            AplicarTema();
        }

        private void BotaoPaleta_Click(object? sender, EventArgs e)
        {
            if (sender is Button botao && botao.Tag is string nome)
            {
                Preferencias p = Tema.Atual.Copiar();
                p.Paleta = nome;
                SalvarEAplicar(p);
            }
        }

        // Os três RadioButton usam este mesmo evento
        private void rdoFonte_CheckedChanged(object? sender, EventArgs e)
        {
            // O evento dispara também no botão que foi DESmarcado: só interessa o marcado
            if (_carregando || sender is not RadioButton rdo || !rdo.Checked)
            {
                return;
            }

            Preferencias p = Tema.Atual.Copiar();

            if (rdo == rdoPequena) p.TamanhoFonte = TamanhoFonte.Pequena;
            else if (rdo == rdoGrande) p.TamanhoFonte = TamanhoFonte.Grande;
            else p.TamanhoFonte = TamanhoFonte.Media;

            SalvarEAplicar(p);
        }

        private void chkModoEscuro_CheckedChanged(object? sender, EventArgs e)
        {
            if (_carregando)
            {
                return;
            }

            Preferencias p = Tema.Atual.Copiar();
            p.ModoEscuro = chkModoEscuro.Checked;
            SalvarEAplicar(p);
        }

        private void btnRestaurar_Click(object? sender, EventArgs e)
        {
            SalvarEAplicar(new Preferencias());
            MostrarValoresAtuais();
        }

        // ---------- Lógica ----------

        private void SalvarEAplicar(Preferencias p)
        {
            try
            {
                _service.Salvar(p);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                MessageBox.Show(
                    "Não foi possível salvar as preferências:\n" + ex.Message +
                    "\n\nA mudança vale só até fechar o aplicativo.",
                    "Configurações", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Aplica mesmo se não salvou: o usuário vê a mudança nesta sessão
            Tema.Aplicar(p);
        }

        // Marca nos controles o que está em uso agora
        private void MostrarValoresAtuais()
        {
            _carregando = true;

            Preferencias p = Tema.Atual;
            rdoPequena.Checked = p.TamanhoFonte == TamanhoFonte.Pequena;
            rdoMedia.Checked = p.TamanhoFonte == TamanhoFonte.Media;
            rdoGrande.Checked = p.TamanhoFonte == TamanhoFonte.Grande;
            chkModoEscuro.Checked = p.ModoEscuro;

            _carregando = false;
        }

        // Um botão por paleta, criado a partir da lista do Tema:
        // para ter uma cor nova, basta acrescentá-la em Tema.Paletas
        private void CriarBotoesPaleta()
        {
            foreach (Paleta paleta in Tema.Paletas)
            {
                Button botao = new Button();
                botao.Tag = paleta.Nome;
                botao.FlatStyle = FlatStyle.Flat;
                botao.Cursor = Cursors.Hand;
                botao.Margin = new Padding(0, 0, 10, 0);
                botao.UseVisualStyleBackColor = false;
                botao.Click += BotaoPaleta_Click;

                flpPaletas.Controls.Add(botao);
            }
        }

        // ---------- Tema ----------

        private void AplicarTema()
        {
            BackColor = Tema.Fundo;

            lblTitulo.Font = Tema.Fonte(28f, FontStyle.Bold);
            lblTitulo.ForeColor = Tema.Titulo;

            foreach (Label secao in new[] { lblCor, lblFonte, lblAparencia })
            {
                secao.Font = Tema.Fonte(16f, FontStyle.Bold);
                secao.ForeColor = Tema.Titulo;
            }

            foreach (ButtonBase opcao in new ButtonBase[] { rdoPequena, rdoMedia, rdoGrande, chkModoEscuro })
            {
                opcao.Font = Tema.Fonte(13f);
                opcao.ForeColor = Tema.Texto;
            }

            btnRestaurar.Font = Tema.Fonte(12f, FontStyle.Bold);
            btnRestaurar.BackColor = Tema.Menu;
            btnRestaurar.ForeColor = Tema.TextoSobreMenu;
            btnRestaurar.FlatAppearance.MouseOverBackColor = Tema.MenuSelecionado;

            // Cada botão mostra a própria paleta; o selecionado ganha borda e um ✓
            foreach (Control c in flpPaletas.Controls)
            {
                if (c is not Button botao || botao.Tag is not string nome)
                {
                    continue;
                }

                Paleta paleta = Tema.BuscarPaleta(nome);
                bool selecionada = nome == Tema.Atual.Paleta;

                botao.Text = selecionada ? "✓ " + nome : nome;
                botao.Font = Tema.Fonte(12f, FontStyle.Bold);
                botao.Size = new Size(Tema.Escalar(110), Tema.Escalar(44));
                botao.BackColor = paleta.Menu;
                botao.ForeColor = Color.White;
                botao.FlatAppearance.MouseOverBackColor = paleta.MenuSelecionado;
                botao.FlatAppearance.BorderColor = paleta.Destaque;
                botao.FlatAppearance.BorderSize = selecionada ? 3 : 0;
            }
        }
    }
}
