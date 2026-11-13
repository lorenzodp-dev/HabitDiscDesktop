using System;
using System.Drawing;
using System.Windows.Forms;
using HabitDisc.Telas;

namespace HabitDisc.Forms
{
    public partial class MainForm : Form
    {
        private static readonly Color CorBotaoNormal = Color.FromArgb(35, 77, 51);
        private static readonly Color CorBotaoSelecionado = Color.FromArgb(47, 107, 69);

        // Telas criadas uma única vez e reaproveitadas
        private readonly HomeControl _home = new HomeControl();
        // TODO: trocar cada placeholder pela tela real quando ela existir
        private readonly UserControl _habitos = new TelaPlaceholderControl("Hábitos");
        private readonly UserControl _estatisticas = new TelaPlaceholderControl("Estatísticas");
        private readonly UserControl _humor = new TelaPlaceholderControl("Humor");
        private readonly UserControl _categorias = new TelaPlaceholderControl("Categorias");

        // Qual tela está sendo exibida agora (null até a primeira troca)
        private UserControl _ucSelecionado;
        private Button _botaoSelecionado;

        public MainForm()
        {
            InitializeComponent();

            RegistrarTela(_home);
            RegistrarTela(_habitos);
            RegistrarTela(_estatisticas);
            RegistrarTela(_humor);
            RegistrarTela(_categorias);
        }

        // ---------- Eventos ----------

        // Usei Shown (e não Load) porque aqui o form já está na tela,
        // então o VisibleChanged da tela dispara com segurança.
        private void MainForm_Shown(object sender, EventArgs e)
        {
            MostrarTela(_home, btnHome);
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
                _botaoSelecionado.BackColor = CorBotaoNormal;
            }

            botao.BackColor = CorBotaoSelecionado;
            _botaoSelecionado = botao;
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {

        }
    }
}