
    using System.Collections.Generic;
    using SinqiaParibas.Models;

    namespace SinqiaParibas.ViewModels
    {
        public class MovimentoManualViewModel
        {
            // Lista de movimentos para o grid
            public IEnumerable<MovimentoManual> Movimentos { get; set; }

            // Objeto para o formulário de criação/edição
            public MovimentoManual MovimentoAtual { get; set; } = new MovimentoManual();
        }
    }

