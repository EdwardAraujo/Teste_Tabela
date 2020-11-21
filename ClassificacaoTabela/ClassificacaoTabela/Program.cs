using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassificacaoTabela.Request;
using ClassificacaoTabela.Result;
using System.Windows.Forms;


namespace ClassificacaoTabela
{
    class Program
    {
        static void Main(string[] args)
        {
            ResultClass Resultado = new ResultClass();
            Classificacao Classificacao = new Classificacao();

            Resultado = Classificacao.GerarTabela();

            if (Resultado.ProcessOK == true)
            {
                MessageBox.Show("Planilha gerada com sucesso!");

            }
            else
            {
                MessageBox.Show("Falha em gerar planilha!");

            }
        }
    }
}
