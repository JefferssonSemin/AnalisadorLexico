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
        public bool VerificarTexto([FromBody]Model modelo)
        {
            bool valido = true;
            modelo.Texto = modelo.Texto.ToUpper();
            string[] stringSeparators = new string[] { "\r\n" };
            string[] lines = modelo.Texto.Split(stringSeparators, StringSplitOptions.None);

            foreach (var item in lines)
            {
                var linha = item.Trim();
                if (!string.IsNullOrEmpty(linha))
                {
                    if (VerificaSeTemCondicao(linha))
                    {
                        if (!VerificaSeECondicional(linha))
                        {
                            return false;
                        };
                    }
                    else if (VerificaSeTemRepeticao(linha))
                    {
                        if (!VerificaSeERepeticao(linha))
                        {
                            return false;
                        }
                    }
                    else if (VerificaSeEVariavel(linha))
                    {
                        if (!VerificaSintaxeEPegaVariavel(linha))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return valido;
        }

        private bool VerificaSeTemCondicao(string linha)
        {
            Regex rx = new Regex(@"^(" + string.Join("|", Db.EstruturaCondicao) + ")+(.*.)");
            return rx.Matches(linha).Count() > 0;
        }

        private bool VerificaSintaxeEPegaVariavel(string linha)
        {
            Regex rx = new Regex(@"^" + string.Join("|", Db.Tipos) + "\\s+(.*.)");
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
            Regex rx = new Regex(@"^(" + string.Join("|", Db.EstruturaCondicao) + ")+\\(+(" + string.Join("|", Db.Variaveis) + ")+(" + string.Join("|", Db.CharsComparacao) + ")+(" + string.Join("|", Db.Variaveis) + ")+\\)+{$");
            return rx.Matches(linha).Count() > 0;
        }

        private bool VerificaSeTemRepeticao(string linha)
        {
            Regex rx = new Regex(@"^(" + string.Join("|", Db.EstruturasRepeticao) + ")");
            return rx.Matches(linha).Count() > 0;
        }

        private bool VerificaSeERepeticao(string linha)
        {
            Regex rx = new Regex(@"^(" + string.Join("|", Db.EstruturasRepeticao) + ")+\\(+(" + string.Join("|", Db.Variaveis) + ")+(" + string.Join("|", Db.CharsRepeticao) + ")+(" + string.Join("|", Db.Variaveis) + ")+\\)+{$");
            return rx.Matches(linha).Count() > 0;
        }

        private bool VerificaSeEVariavel(string linha)
        {
            Regex rx = new Regex(@"^" + string.Join("|", Db.Tipos) + "");
            return rx.Matches(linha).Count() > 0;
        }

        private bool VerificaSeTemDigitoFinalizador(string linha)
        {
            Regex rx = new Regex(@"[" + Db.CharFimLinha + "]$");
            return rx.Matches(linha).Count() > 0;
        }
    }
}