using System;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;
using HabitDisc.Helpers;
using HabitDisc.Telas;

namespace HabitDisc.Forms
{
    public partial class MainForm : Form
    {
        // Telas criadas uma única vez e reaproveitadas
        private readonly HomeControl _home = new HomeControl();
        // TODO: trocar cada placeholder pela tela real quando ela existir
        private readonly UserControl _habitos = new TelaPlaceholderControl("Hábitos");
        private readonly UserControl _estatisticas = new TelaPlaceholderControl("Estatísticas");
        private readonly UserControl _humor = new TelaPlaceholderControl("Humor");
        private readonly UserControl _categorias = new TelaPlaceholderControl("Categorias");
        private readonly ConfiguracoesControl _configuracoes = new ConfiguracoesControl();

        // Qual tela está sendo exibida agora (null até a primeira troca)
        private UserControl? _ucSelecionado;
        private Button? _botaoSelecionado;

        // Todos os botões do menu, para pintar de uma vez ao trocar o tema
        private readonly IconButton[] _botoesMenu;

        public MainForm()
        {
            InitializeComponent();

            _botoesMenu = new IconButton[]
            {
                btnHome, btnHabitos, btnEstatisticas, btnHumor, btnCategorias, btnConfiguracoes
            };

            RegistrarTela(_home);
            RegistrarTela(_habitos);
            RegistrarTela(_estatisticas);
            RegistrarTela(_humor);
            RegistrarTela(_categorias);
            RegistrarTela(_configuracoes);

            AplicarTema();
            Tema.Alterado += Tema_Alterado;
            Disposed += delegate { Tema.Alterado -= Tema_Alterado; };
        }

        // ---------- Eventos ----------

        // Usei Shown (e não Load) porque aqui o form já está na tela,
        // então o VisibleChanged da tela dispara com segurança.
        private void MainForm_Shown(object sender, EventArgs e)
        {
            MostrarTela(_home, btnHome);
        }

        private void Tema_Alterado(object? sender, EventArgs e)
        {
            AplicarTema();
        }

        // A barra de título só pode ser alterada depois que a janela existe no Windows
        // (o AplicarTema do construtor roda antes disso), então aplica aqui também.
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            BarraDeTitulo.AplicarModoEscuro(this, Tema.Atual.ModoEscuro);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            MostrarTela(_home, btnHome);
        }

        private void btnHabitos_Click(object sender, EventArgs e)
        {
            MostrarTela(_habitos, btnHabitos);
        }

        private void btnEstatisticas_Click(object sender, EventArgs e)
        {
            MostrarTela(_estatisticas, btnEstatisticas);
        }

        private void btnHumor_Click(object sender, EventArgs e)
        {
            MostrarTela(_humor, btnHumor);
        }

        private void btnCategorias_Click(object sender, EventArgs e)
        {
            MostrarTela(_categorias, btnCategorias);
        }

        private void btnConfiguracoes_Click(object sender, EventArgs e)
        {
            MostrarTela(_configuracoes, btnConfiguracoes);
        }

        // ---------- Navegação ----------

        // Prepara a tela uma única vez: ocupa o painel todo e começa escondida.
        // Como ficam dentro do painel, o WinForms as descarta sozinho ao fechar o form.
        private void RegistrarTela(UserControl tela)
        {
            tela.Dock = DockStyle.Fill;
            tela.Visible = false;
            pnlConteudo.Controls.Add(tela);
        }

        // Aceita qualquer tela, pois todas são UserControl (polimorfismo)
        private void MostrarTela(UserControl tela, Button botao)
        {
            // Clicou na tela que já está aberta: nada a fazer
            if (tela == _ucSelecionado)
            {
                return;
            }

            // 1. Esconde a atual (na primeira vez ainda não existe: null)
            if (_ucSelecionado != null)
            {
                _ucSelecionado.Visible = false;
            }

            // 2. Mostra a nova (isso dispara o VisibleChanged, que atualiza os dados dela)
            tela.Visible = true;
            tela.BringToFront();

            // 3. Guarda quem é a tela atual e destaca o botão
            _ucSelecionado = tela;
            DestacarBotao(botao);
        }

        private void DestacarBotao(Button botao)
        {
            if (_botaoSelecionado != null)
            {
                _botaoSelecionado.BackColor = Tema.Menu;
            }

            botao.BackColor = Tema.MenuSelecionado;
            _botaoSelecionado = botao;
        }

        // ---------- Tema ----------

        private void AplicarTema()
        {
            // Barra de título (controlBox) clara/escura junto com o resto
            BarraDeTitulo.AplicarModoEscuro(this, Tema.Atual.ModoEscuro);

            BackColor = Tema.Fundo;
            pnlConteudo.BackColor = Tema.Fundo;
            pnlNavegacao.BackColor = Tema.Menu;
            // A barra lateral cresce junto com a fonte, para "Configurações" caber
            pnlNavegacao.Width = Tema.Escalar(244);

            lblLogo.Font = Tema.Fonte(20f, FontStyle.Bold, Tema.FonteMenu);
            lblLogo.ForeColor = Tema.TextoSobreMenu;

            foreach (IconButton botao in _botoesMenu)
            {
                botao.BackColor = botao == _botaoSelecionado ? Tema.MenuSelecionado : Tema.Menu;
                botao.FlatAppearance.MouseOverBackColor = Tema.MenuSelecionado;
                botao.ForeColor = Tema.TextoSobreMenu;
                botao.IconColor = Tema.TextoSobreMenu;
                botao.Font = Tema.Fonte(13f, FontStyle.Bold, Tema.FonteMenu);
                botao.Height = Tema.Escalar(48);
                botao.IconSize = Tema.Escalar(28);
            }
        }
    }
}
