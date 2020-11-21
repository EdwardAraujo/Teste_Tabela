using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificacaoTabela.Class
{
    public class ResultBase
    {
        /// <summary>
        /// Define se o Robo fêz oque deveria, ou seja se não ocorreu nenhuma exceção de código o resultado deverá ser TRUE.
        /// </summary>   
        public bool ProcessOK { get; set; }

    }
}
