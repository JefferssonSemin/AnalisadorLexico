using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AnalisadorLexico.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnalisadorLexico.Controllers
{
    [Produces("application/json")]
    [Route("api/Default")]
    public class DefaultController : Controller
    {
        public StaticDB Db { get; set; }

        public DefaultController()
        {
            Db = new StaticDB();
        }

        [HttpPost]
        [Route("VerificarTexto")]
        public string VerificarTexto([FromBody]Model modelo)
        {
            bool valido = true;
            modelo.Texto = modelo.Texto.ToUpper();
            if ((modelo.Texto.Split("{").Length - 1) != (modelo.Texto.Split("}").Length - 1))
            {
                return "Erro: quantidade de { diferente de }";
            }
            modelo.Texto = modelo.Texto.Replace("{", "");
            modelo.Texto = modelo.Texto.Replace("}", "");
            string[] stringSeparators = new string[] { "\r\n" };
            string[] lines = modelo.Texto.Split(stringSeparators, StringSplitOptions.None);

            var count = 0;
            foreach (var item in lines)
            {
                count++;
                var linha = item.Trim();

                if (!string.IsNullOrEmpty(linha))
                {
                    if (VerificaSeTemCondicao(linha))
                    {
                        if (!VerificaSeECondicional(linha))
                        {
                            return "Erro: Estrutura condicional na linha " + count.ToString() + Environment.NewLine + "command: " + linha;
                        };
                    }
                    else if (VerificaSeTemRepeticao(linha))
                    {
                        if (!VerificaSeERepeticao(linha))
                        {
                            return "Erro: Estrutura de repetição na linha " + count.ToString() + Environment.NewLine + "command: " + linha;
                        }
                    }
                    else if (VerificaSeEVariavel(linha))
                    {
                        if (!VerificaSintaxeEPegaVariavel(linha))
                        {
                            return "Erro: Erro na declaração de var na linha " + count.ToString() + Environment.NewLine + "command: " + linha;
                        }
                    }
                    else
                    {
                        return "Erro:Comando não identificado na linha " + count.ToString() + Environment.NewLine + "command: " + linha;
                    }
                }
            }
            return "Sucesso";
        }

        private bool VerificaSeTemCondicao(string linha)
        {
            Regex rx = new Regex(@"^\s*(" + string.Join("|", Db.EstruturaCondicao) + ")+(.*.)");
            return rx.Matches(linha).Count() > 0;
        }

        private bool VerificaSintaxeEPegaVariavel(string linha)
        {
            Regex rx = new Regex(@"^\s*(" + string.Join("|", Db.Tipos) + ")\\s+(.*.)");
            if (rx.Matches(linha).Count() == 0)
            {
                return false;
            }
            if (!VerificaSeTemDigitoFinalizador(linha))
            {
                return false;
            }
            var palavras = linha.Split(" ");
            Db.Variaveis.Add(palavras[1].Replace(";", ""));
            return true;
        }

        private bool VerificaSeECondicional(string linha)
        {
            Regex rx = new Regex(@"^\s*(" + string.Join("|", Db.EstruturaCondicao) + ")\\s*\\(\\s*(" + string.Join("|", Db.Variaveis) + ")\\s*(" + string.Join("|", Db.CharsComparacao) + ")\\s*(" + string.Join("|", Db.Variaveis) + ")\\s*\\)\\s*$");
            return rx.Matches(linha).Count() > 0;
        }

        private bool VerificaSeTemRepeticao(string linha)
        {
            Regex rx = new Regex(@"^(" + string.Join("|", Db.EstruturasRepeticao) + ")");
            return rx.Matches(linha).Count() > 0;
        }

        private bool VerificaSeERepeticao(string linha)
        {
            Regex rx = new Regex(@"^\s*(" + string.Join("|", Db.EstruturasRepeticao) + ")\\s*\\(\\s*(" + string.Join("|", Db.Variaveis) + ")\\s*(" + string.Join("|", Db.CharsRepeticao) + ")\\s*(" + string.Join("|", Db.Variaveis) + ")\\s*\\)\\s*$");
            return rx.Matches(linha).Count() > 0;
        }

        private bool VerificaSeEVariavel(string linha)
        {
            Regex rx = new Regex(@"^\s*(" + string.Join("|", Db.Tipos) + ")");
            return rx.Matches(linha).Count() > 0;
        }

        private bool VerificaSeTemDigitoFinalizador(string linha)
        {
            Regex rx = new Regex(@"[" + Db.CharFimLinha + "]$");
            return rx.Matches(linha).Count() > 0;
        }
    }
}