using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ClassificacaoTabela.Class;

namespace ClassificacaoTabela.Result
{
    public class ResultClass : ResultBase
    {
        public CookieContainer Session { get; set; }
    }
}
