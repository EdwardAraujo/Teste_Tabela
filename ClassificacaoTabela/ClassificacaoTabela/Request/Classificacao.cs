using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ClassificacaoTabela.Class;
using ClassificacaoTabela.Extensions;
using ClassificacaoTabela.Result;
using System.IO;


namespace ClassificacaoTabela.Request
{
	class Classificacao : RequestBase
	{
		public string Destino { get; set; }

		public ResultClass GerarTabela()
		{
			//_session = new CookieContainer();
			HttpWebResponse response;
			var result = new ResultClass();

			Destino = @"C:\Edward_Teste_Classificacao\";

			try
			{
				//GERAR EXCEL
				var wb = new XLWorkbook();
				var ws = wb.Worksheets.Add("Tabela Classificacao");

				//CABEÇALHO 
				ws.Cell("A1").Value = "Classificação";
				ws.Cell("B1").Value = "Time";
				ws.Cell("C1").Value = "Partidas";
				ws.Cell("D1").Value = "Historico de jogos";
				ws.Cell("E1").Value = "Gols marcados";

				var Linha = 2;

				if (Passo1(out response))
				{
					var html = ReadResponse(response).ToHtmlDocument();

					string Selecao = html.Text.Substring(html.Text.IndexOf("const classificacao")).Replace("regulamento", "\n");
					String[] y = Regex.Split(Selecao, "\n"); Selecao = y[0];
					var Selecao2 = Selecao.Substring(Selecao.IndexOf(',')).Replace("faixa_classificacao_cor", "\n"); String[] x = Regex.Split(Selecao2, "\n");

					foreach (var item in x)
					{
						if (item.Contains("ordem"))
						{
							var teste = item.Replace(":", "\n"); String[] QuebraLinhas = Regex.Split(teste, "\n");

							var GolsPro = QuebraLinhas[3].Replace(",", "\n"); String[] a = Regex.Split(GolsPro, "\n"); GolsPro = a[0];
							var Nometime = QuebraLinhas[5].Replace("\"", "").Replace(",", "\n"); String[] b = Regex.Split(Nometime, "\n"); Nometime = b[0];

							if (Nometime.Contains(@"Atl\u00e9tico de Madrid")) Nometime = "Atletico de Madri";
							if (Nometime.Contains(@"C\u00e1diz")) Nometime = "Cádiz";
							if (Nometime.Contains(@"Alav\u00e9s")) Nometime = "Alavés";

							var Classificacao = QuebraLinhas[6].Replace(",", "\n"); String[] c = Regex.Split(Classificacao, "\n"); Classificacao = c[0];
							var Partidas = QuebraLinhas[7].Replace(",", "\n"); String[] d = Regex.Split(Partidas, "\n"); Partidas = d[0];
							var Historico = QuebraLinhas[10].Replace(",", "\n").Replace("\"", "").Replace("[", "").Replace("]", ""); String[] e = Regex.Split(Historico, "\n");
							Historico = e[0] + e[1] + e[2] + e[3] + e[4];

							ws.Cells("A" + Linha.ToString()).Value = Classificacao;
							ws.Cell("B" + Linha.ToString()).Value = Nometime;
							ws.Cell("C" + Linha.ToString()).Value = Partidas;
							ws.Cell("D" + Linha.ToString()).Value = Historico;
							ws.Cell("E" + Linha.ToString()).Value = GolsPro;
							Linha++;

						}


					}

					//CRIAR PASTA
					if (!Directory.Exists(Destino))
						Directory.CreateDirectory(Destino);
					//CRIAR TABELA					
					var range = ws.Range("A1:E" + Linha.ToString());
					range.CreateTable();
					//AJUSTE DA COLUNA COM O CONTEUDO
					ws.Columns("1-5").AdjustToContents();
					//SALVAR ARQUIVO      
					wb.SaveAs($@"{Destino}Classificacao.xlsx");


				}
				else
				{
					result.ProcessOK = false;
					return result;
				}

				result.ProcessOK = true;
				return result;
			}
			catch (Exception Erro)
			{
				Console.WriteLine(Erro);
			}

			return result;

		}

		private bool Passo1(out HttpWebResponse response)
		{
			response = null;

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://globoesporte.globo.com/futebol/futebol-internacional/futebol-espanhol/");

				request.KeepAlive = true;
				request.Headers.Add("Upgrade-Insecure-Requests", @"1");
				request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.66 Safari/537.36";
				request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
				request.Headers.Add("Sec-Fetch-Site", @"none");
				request.Headers.Add("Sec-Fetch-Mode", @"navigate");
				request.Headers.Add("Sec-Fetch-Dest", @"document");
				request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
				request.Headers.Set(HttpRequestHeader.AcceptLanguage, "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");
				request.Headers.Set(HttpRequestHeader.Cookie, @"glb_uid=""nAwTREZ-YYDQNpS3fBe6wlhqBXGIJImnGsNUktVgkUI=""; cookie-banner-consent-accepted=false; kppid=15451795960737726464; _cb_ls=1; _cb=JrOuUCasXHoBOFJNF; _fbp=fb.1.1595434124449.1902310433; _gcl_au=1.1.809725430.1595434125; tt.u=0100007F6D56175FC006202A025E7D11; __tbc=%7Bjzx%7D1l0RdXZwIVvWOvSrDy7MfEIzOIvin5sTlvr_pkWzsj-sHWIMPaePcetbR6lVE4vQABoQyWz5IfW75TtOTxTi2XRSMq7TI4z0LZUPjtfICayTTU2YGjpwTie24L27-pZkQ-UizgODsiaMqyMAgVNRuA; _ga=GA1.2.1d0aaa4c-9cc2-c580-0876-4da04be21407; _hjid=809de82a-bbd6-4a55-a97c-044c032ba829; _hzt.host=horizon-track.globo.com; __pat=-7200000; _gid=GA1.2.1324094772.1605528958; __gads=ID=9dc6710188b9c9c4:T=1595428901:R:S=ALNI_MaocBNnXvEAmRo2utrrS-46w57PWA; nav46169=bef9a9f166a33c9fa2e2e7c2709|2_325; __pvi=%7B%22id%22%3A%22v-2020-11-19-08-49-49-662-FpBQQRWvkogChygB-6935d42125214ad43babc51d93fd4f5b%22%2C%22domain%22%3A%22.globo.com%22%2C%22time%22%3A1605786590039%7D; xbc=%7Bjzx%7DkriXWMUx3OJG5d3jEWQW_Fw68Tq9DODiVixhsTxMuSGYzefJt7t3JdWxFkTJfzUoqtQltflrKE58__QoQWHBuEucoSI1-PQ7PfcNSteflmAnampfMDCfF-RrwdYUyPWKUvr2ut7YSdDTn5oena7lL8_gUI9m2GeIcnWQwNePBXynV8q-ufR4XyhcyP4yjezEEZOJWxXJLL6GHduNN1DJA6G5UHPwNyYKgUA7vwJSedpEFHfWzxYZX8wuodrHH2rCAqH8pihm-2YVel5WCS0hTi5xcEsJ4NGzTW8MzMIwETuU-QUkOFgZWJVVssieI_P6FCv8Jvk9GUDpxyjQgkI3PAxJZtHrElhHwduM_XZmlY3golkqDStVgOIlMFny63compyhR4BMYsUjaV76dp5zB0M9V3K76l5Ow85-ywefaiL5ZQVoJWsYgBbd-VkiVQTi; smDataLocalStorageCart={""onsiteview_77878db1038f4d1aba8c94d25f5fe3fb"":""1"",""smcid"":""a7d2a7d058e54adcbaafc05ff0a0c3b0"",""smtp"":""[]"",""smpid_3"":""c22dc790d68f4f0ca5f894c2aea48b06""}; smDataCookieCart={}; nav13574=bef9a9f16879f752610aad64809|2_325_12:4:1:11:14:5:15:2_222281-111814-111783-103916-168049-113382-111587-222279-222749-103666:2:1:42-41:130-124-61-32-64-66-3-4-8-24:1:1736-183-122-19-3-137-71-39-66-31:3; tt.nprf=59,64,72,24,34,44; utag_main=v_id:01752e0b7864001f31825def929101072001606a00bd0$_sn:51$_se:8$_ss:0$_st:1605838017810$ses_id:1605834483328%3Bexp-session$_pn:8%3Bexp-session; _chartbeat2=.1595434123867.1605836243448.0010000000000001.CcT7qz6QeEjCysvGEDbdTzIBOl_ET.2; FCCDCF=[[""AKsRol-52m9T5ZJ5JRUJ6h65qH-WnF53HScLufDFub7HwxydlVSHb5UdiZ1b2q8dKfigRfV9ZvwalPvXnohWpmHt7DgMxtDqRSuGnj_aygIAlPkvelB9gqhj4YQPLRxZN6iOmCH-UkapFqJDU_PBxHrabx57DdSokA==""],null,[""[[],[],[],[],null,null,true]"",1605836255511]]; _ttuu.s=1605836274433");

				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException e)
			{
				if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
				else return false;
			}
			catch (Exception)
			{
				if (response != null) response.Close();
				return false;
			}

			return true;
		}


	}

}


