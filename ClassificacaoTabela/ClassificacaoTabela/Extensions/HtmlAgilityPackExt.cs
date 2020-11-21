using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificacaoTabela.Extensions
{
    public static class HtmlAgilityPackExt
    {
        public static HtmlDocument ToHtmlDocument(this string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            return doc;
        }

    }
}
