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

        [HttpGet]
        [Route("VerificarTexto")]
        public bool VerificarTexto(string texto)
        {
            texto.Trim();
            bool Valor = true;
            Valor = VerificaSeTemDigitoFinalizador(texto);
            return Valor;
        }

        public bool VerificaSeTemDigitoFinalizador(string texto)
        {
            Regex rx = new Regex(@"[" + Db.CharFimLinha + "]$");
            return rx.Matches(texto).Count() > 0;
        }
    }
}