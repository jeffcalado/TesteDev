using System.ComponentModel.DataAnnotations;

namespace SinqiaParibas.Models
{
    public class MovimentoManual
    {
        
        [Required(ErrorMessage = "O campo Mês é obrigatório.")]
        [Range(1, 12, ErrorMessage = "O Mês deve estar entre 1 e 12.")]
        public int DAT_MES { get; set; } // PK composta

        [Required(ErrorMessage = "O campo Ano é obrigatório.")]
        [Range(1900, 2050, ErrorMessage = "O Ano deve estar entre 1900 e 2050.")]
        public int DAT_ANO { get; set; } // PK composta
        
        public int NUM_LANCAMENTO { get; set; } // PK composta
        public string COD_PRODUTO { get; set; } // FK para ProdutoCosif
        public string COD_COSIF { get; set; } // FK para ProdutoCosif
        public string DES_DESCRICAO { get; set; }
        public DateTime DAT_MOVIMENTO { get; set; }
        public string COD_USUARIO { get; set ; } 
        
        [Required(ErrorMessage = "O campo Valor é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O Valor deve ser maior que zero.")] 
        public decimal VAL_VALOR { get; set; }

        // Relacionamento
        public ProdutoCosif ProdutoCosif { get; set; }
    }
}
