using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FontAwesome.Sharp;
using HabitDisc.Forms;
using HabitDisc.Helpers;
using HabitDisc.Services;
using HabitDisc.Telas.Componentes;

namespace HabitDisc.Telas
{
    // Tela Hábitos: lista todos os hábitos, cria novos (UC01) e abre os detalhes
    // de cada um (UC08), de onde se vê o streak (UC03) e se edita ou exclui (UC07).
    public partial class HabitosControl : TelaBase
    {
        private readonly HabitoService _habitos = new HabitoService();
        private readonly CategoriaService _categorias = new CategoriaService();

        // Lista exibida agora (null até a primeira carga). Fica guardada para
        // remontar as linhas quando o tema muda, sem ir ao banco.
        private List<HabitoResumo>? _lista;

        public HabitosControl()
        {
            InitializeComponent();
            ListaVertical.Configurar(flpLista);
            AplicarTema();
        }

        // ---------- Eventos ----------

        private void HabitosControl_VisibleChanged(object? sender, EventArgs e)
        {
            if (Visible && !DesignMode)
            {
                Carregar();
            }
        }

        private void btnNovo_Click(object? sender, EventArgs e)
        {
            using FormHabito form = new FormHabito(_habitos, _categorias, null);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                Carregar();
            }
        }

        // ---------- Carregamento ----------

        private void Carregar()
        {
            _lista = _habitos.Listar();
            MontarLinhas();
        }

        private void MontarLinhas()
        {
            List<HabitoResumo> habitos = _lista ?? new List<HabitoResumo>();

            ListaVertical.Limpar(flpLista);

            flpLista.SuspendLayout();
            foreach (HabitoResumo h in habitos)
            {
                LinhaItemControl linha = new LinhaItemControl(
                    CorHelper.HexParaCor(h.Cor),
                    IconeHelper.ParaIconChar(h.Icone),
                    h.Nome,
                    h.Categoria + "  ·  " + Textos.Streak(h.Streak));

                long id = h.Id;
                linha.PermitirClique(delegate { AbrirDetalhes(id); });
                linha.AdicionarAcao(IconChar.ChevronRight, "Ver detalhes", delegate { AbrirDetalhes(id); });

                flpLista.Controls.Add(linha);
            }
            flpLista.ResumeLayout();

            bool vazio = habitos.Count == 0;
            flpLista.Visible = !vazio;
            lblVazio.Visible = vazio;
        }

        private void AbrirDetalhes(long idHabito)
        {
            using FormDetalhesHabito form = new FormDetalhesHabito(_habitos, _categorias, idHabito);
            form.ShowDialog(this);

            // Pode ter editado ou excluído: recarrega de qualquer jeito
            Carregar();
        }

        // ---------- Tema ----------

        protected override void AplicarTema()
        {
            BackColor = Tema.Fundo;
            Estilo.Cabecalho(pnlTopo, lblTitulo, btnNovo);

            lblVazio.Font = Tema.Fonte(13f);
            lblVazio.ForeColor = Tema.TextoSuave;

            if (_lista != null)
            {
                MontarLinhas();
            }
        }
    }
}
