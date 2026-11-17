using System;
using System.Windows.Forms;
using HabitDisc.Helpers;

namespace HabitDisc.Telas
{
    // Classe "mãe" de todas as telas do painel de conteúdo.
    //
    // Faz, uma única vez, a parte que toda tela repetia para seguir o tema:
    // escutar o evento Tema.Alterado e parar de escutar quando a tela é descartada.
    // Cada tela "filha" só escreve o seu AplicarTema() (override) e o chama
    // no construtor, depois do InitializeComponent().
    //
    // Não é "abstract" de propósito: o designer do Visual Studio precisa criar
    // a classe mãe para desenhar a tela filha, e não consegue com classe abstrata.
    public class TelaBase : UserControl
    {
        public TelaBase()
        {
            Tema.Alterado += Tema_Alterado;

            // O Tema é static e vive o app inteiro: sem isto, ele continuaria
            // guardando referência a uma tela já descartada.
            Disposed += delegate { Tema.Alterado -= Tema_Alterado; };
        }

        private void Tema_Alterado(object? sender, EventArgs e)
        {
            AplicarTema();
        }

        // Pinta a tela com as cores e fontes atuais do Tema.
        // Cada tela filha escreve a sua versão com "protected override".
        protected virtual void AplicarTema()
        {
        }
    }
}
