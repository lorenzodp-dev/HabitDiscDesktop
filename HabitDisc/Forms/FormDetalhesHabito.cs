using System;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;
using HabitDisc.Helpers;
using HabitDisc.Services;
using HabitDisc.Telas.Componentes;

namespace HabitDisc.Forms
{
    // Detalhes de um hábito (UC08): dados, streak atual e melhor recorde (UC03)
    // e os últimos 30 dias. Daqui o usuário também edita ou exclui o hábito (UC07).
    public class FormDetalhesHabito : FormDialogoBase
    {
        private const int DiasNoHistorico = 30;
        private const int QuadradosPorLinha = 15;

        private readonly HabitoService _habitos;
        private readonly CategoriaService _categorias;
        private readonly long _idHabito;
        private readonly ToolTip _dicas = new ToolTip();

        private HabitoDetalhes? _detalhes;

        public FormDetalhesHabito(HabitoService habitos, CategoriaService categorias, long idHabito)
            : base("Detalhes do hábito")
        {
            _habitos = habitos;
            _categorias = categorias;
            _idHabito = idHabito;

            AdicionarBotao("Excluir", principal: false, delegate { Excluir(); });
            AdicionarBotao("Editar", principal: false, delegate { Editar(); });
            AdicionarBotaoFechar("Fechar", principal: true);

            Carregar();
        }

        // Busca os dados e (re)monta o conteúdo. Chamado de novo depois de editar.
        private void Carregar()
        {
            _detalhes = _habitos.ObterDetalhes(_idHabito);

            while (Conteudo.Controls.Count > 0)
            {
                Control c = Conteudo.Controls[0];
                Conteudo.Controls.RemoveAt(0);
                c.Dispose();
            }

            Conteudo.Controls.Add(CriarCabecalho(_detalhes));
            Conteudo.Controls.Add(CriarCartoes(_detalhes));
            Conteudo.Controls.Add(CriarRotulo("Últimos " + DiasNoHistorico + " dias"));
            Conteudo.Controls.Add(CriarHistorico(_detalhes));
            Conteudo.Controls.Add(CriarLegenda());
        }

        // ---------- Ações ----------

        private void Editar()
        {
            if (_detalhes == null)
            {
                return;
            }

            using FormHabito form = new FormHabito(_habitos, _categorias, _detalhes);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                Carregar();
            }
        }

        private void Excluir()
        {
            if (_detalhes == null)
            {
                return;
            }

            bool confirmou = Mensagem.Confirmar(
                "Excluir o hábito \"" + _detalhes.Nome + "\"?\n\n" +
                "O histórico de check-ins e os lembretes dele também serão apagados. " +
                "Essa ação não pode ser desfeita.");

            // Fluxo alternativo do UC07: cancelou, volta ao hábito sem alterações
            if (!confirmou)
            {
                return;
            }

            _habitos.Excluir(_idHabito);
            DialogResult = DialogResult.OK;   // fecha a janela
        }

        // ---------- Montagem ----------

        private Control CriarCabecalho(HabitoDetalhes d)
        {
            Color cor = CorHelper.HexParaCor(d.Cor);

            FlowLayoutPanel cabecalho = CriarFluxo(FlowDirection.LeftToRight);

            IconPictureBox icone = new IconPictureBox();
            icone.IconChar = IconeHelper.ParaIconChar(d.Icone);
            icone.IconColor = cor;
            icone.IconFont = IconFont.Auto;
            icone.IconSize = Tema.Escalar(44);
            icone.Size = new Size(Tema.Escalar(44), Tema.Escalar(44));
            icone.BackColor = Tema.Fundo;
            icone.Margin = new Padding(0, 0, Tema.Escalar(12), 0);

            FlowLayoutPanel textos = CriarFluxo(FlowDirection.TopDown);

            Label nome = new Label();
            nome.AutoSize = true;
            nome.MaximumSize = new Size(Tema.Escalar(400), 0);
            nome.Text = d.Nome;
            nome.Font = Tema.Fonte(18f, FontStyle.Bold);
            nome.ForeColor = Tema.Titulo;

            Label info = new Label();
            info.AutoSize = true;
            info.Text = d.Categoria + "  ·  criado em " + Textos.DataCurta(d.CriadoEm);
            info.Font = Tema.Fonte(11f);
            info.ForeColor = Tema.Texto;

            textos.Controls.Add(nome);
            textos.Controls.Add(info);
            cabecalho.Controls.Add(icone);
            cabecalho.Controls.Add(textos);
            return cabecalho;
        }

        private Control CriarCartoes(HabitoDetalhes d)
        {
            FlowLayoutPanel cartoes = CriarFluxo(FlowDirection.LeftToRight);
            cartoes.Margin = new Padding(0, Tema.Escalar(16), 0, 0);

            cartoes.Controls.Add(CriarCartao(Textos.Dias(d.StreakAtual), "Sequência atual",
                d.StreakAtual == 0 ? "faça hoje para começar" : "dias seguidos"));
            cartoes.Controls.Add(CriarCartao(Textos.Dias(d.Recorde), "Melhor sequência", "recorde do hábito"));
            cartoes.Controls.Add(CriarCartao(d.TotalConcluidos.ToString(), "Check-ins", "no total"));
            return cartoes;
        }

        private CartaoNumero CriarCartao(string valor, string rotulo, string detalhe)
        {
            CartaoNumero cartao = new CartaoNumero(195);
            cartao.Valor = valor;
            cartao.Rotulo = rotulo;
            cartao.Detalhe = detalhe;
            cartao.Margin = new Padding(0, 0, Tema.Escalar(10), 0);
            return cartao;
        }

        // Um quadrado por dia, do mais antigo (esquerda) ao de hoje.
        // Colorido = concluído naquele dia.
        private Control CriarHistorico(HabitoDetalhes d)
        {
            Color cor = CorHelper.HexParaCor(d.Cor);
            DateOnly hoje = DateOnly.FromDateTime(DateTime.Today);
            DateOnly criadoEm = DateOnly.FromDateTime(d.CriadoEm);

            int tamanho = Tema.Escalar(20);
            int espaco = Tema.Escalar(5);

            FlowLayoutPanel grade = CriarFluxo(FlowDirection.LeftToRight);
            grade.WrapContents = true;
            grade.MaximumSize = new Size(QuadradosPorLinha * (tamanho + espaco), 0);

            for (int i = DiasNoHistorico - 1; i >= 0; i--)
            {
                DateOnly dia = hoje.AddDays(-i);
                bool feito = d.DiasConcluidos.Contains(dia);

                Panel quadrado = new Panel();
                quadrado.Size = new Size(tamanho, tamanho);
                quadrado.Margin = new Padding(0, 0, espaco, espaco);
                quadrado.BackColor = feito ? cor : Tema.Superficie;

                string situacao = feito ? "concluído"
                                : dia < criadoEm ? "o hábito ainda não existia"
                                : "não concluído";
                _dicas.SetToolTip(quadrado, Textos.DataPorExtenso(dia) + ": " + situacao);

                grade.Controls.Add(quadrado);
            }
            return grade;
        }

        private Label CriarLegenda()
        {
            Label legenda = new Label();
            legenda.AutoSize = true;
            legenda.Text = "Cada quadrado é um dia (o último é hoje). Colorido = concluído.";
            legenda.Font = Tema.Fonte(10f);
            legenda.ForeColor = Tema.TextoSuave;
            return legenda;
        }

        private Label CriarRotulo(string texto)
        {
            Label rotulo = new Label();
            rotulo.AutoSize = true;
            rotulo.Text = texto;
            rotulo.Font = Tema.Fonte(12f, FontStyle.Bold);
            rotulo.ForeColor = Tema.Titulo;
            rotulo.Margin = new Padding(0, Tema.Escalar(18), 0, Tema.Escalar(6));
            return rotulo;
        }

        private static FlowLayoutPanel CriarFluxo(FlowDirection direcao)
        {
            FlowLayoutPanel fluxo = new FlowLayoutPanel();
            fluxo.FlowDirection = direcao;
            fluxo.WrapContents = false;
            fluxo.AutoSize = true;
            fluxo.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            fluxo.Margin = Padding.Empty;
            return fluxo;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dicas.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
