using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClassificacaoTabela.Class
{
    public class RequestBase
    {
        internal CookieContainer _session { get; set; }

        internal string ReadResponse(HttpWebResponse response)
        {
            using (Stream responseStream = response.GetResponseStream())
            {
                Stream streamToRead = responseStream;
                if (response.ContentEncoding.ToLower().Contains("gzip"))
                {
                    if (streamToRead != null) streamToRead = new GZipStream(streamToRead, CompressionMode.Decompress);
                }
                else if (response.ContentEncoding.ToLower().Contains("deflate"))
                {
                    if (streamToRead != null) streamToRead = new DeflateStream(streamToRead, CompressionMode.Decompress);
                }

                if (streamToRead != null)
                    using (StreamReader streamReader = new StreamReader(streamToRead, Encoding.UTF8))
                    {
                        return streamReader.ReadToEnd();
                    }
            }
            return null;
        }

        public RequestBase()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

            //Teste
            System.Net.ServicePointManager.ServerCertificateValidationCallback = AcceptCertificate;
        }

        private bool AcceptCertificate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate cert, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors err) { return true; }


    }
}
