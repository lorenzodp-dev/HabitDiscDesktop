using System.Windows.Forms;
using HabitDisc.Helpers;
using HabitDisc.Models;
using HabitDisc.Services;
using HabitDisc.Telas.Componentes;

namespace HabitDisc.Forms
{
    // Criar ou editar uma categoria personalizada (UC09).
    // Recebe o CategoriaService de quem a abriu, em vez de criar um próprio:
    // a janela só cuida da tela, e a regra fica no Service.
    public class FormCategoria : FormDialogoBase
    {
        private readonly CategoriaService _service;
        private readonly Categoria? _categoria;

        private readonly TextBox _txtNome = new TextBox();
        private readonly SeletorIcone _seletorIcone = new SeletorIcone();
        private readonly SeletorCor _seletorCor = new SeletorCor();

        // categoria == null: nova categoria; senão, edição dela
        public FormCategoria(CategoriaService service, Categoria? categoria)
            : base(categoria == null ? "Nova categoria" : "Editar categoria")
        {
            _service = service;
            _categoria = categoria;

            _txtNome.Width = Tema.Escalar(440);
            _txtNome.MaxLength = CategoriaService.TamanhoMaximoNome;
            Estilo.CaixaTexto(_txtNome);

            AdicionarCampo("Nome", _txtNome);
            AdicionarCampo("Ícone", _seletorIcone);
            AdicionarCampo("Cor", _seletorCor);

            if (categoria != null)
            {
                _txtNome.Text = categoria.Nome;
                _seletorIcone.IconeSelecionado = categoria.Icone;
                _seletorCor.CorSelecionada = categoria.Cor;
            }

            AdicionarBotaoFechar("Cancelar", principal: false);
            AcceptButton = AdicionarBotao("Salvar", principal: true, delegate { Salvar(); });
        }

        private void Salvar()
        {
            try
            {
                if (_categoria == null)
                {
                    _service.Criar(_txtNome.Text, _seletorIcone.IconeSelecionado, _seletorCor.CorSelecionada);
                }
                else
                {
                    _service.Atualizar(_categoria.IdCategoria, _txtNome.Text,
                        _seletorIcone.IconeSelecionado, _seletorCor.CorSelecionada);
                }

                DialogResult = DialogResult.OK;   // fecha a janela
            }
            catch (RegraNegocioException ex)
            {
                // Ex.: nome vazio ou repetido. A janela continua aberta para corrigir.
                Mensagem.Aviso(ex.Message);
                _txtNome.Focus();
            }
        }
    }
}
