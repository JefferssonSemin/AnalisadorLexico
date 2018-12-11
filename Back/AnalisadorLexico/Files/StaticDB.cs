using System;
using System.Collections.Generic;

namespace AnalisadorLexico.Files
{
    public class StaticDB
    {
        public List<string> EstruturasRepeticao { get; set; }
        public List<string> EstruturaCondicao { get; set; }
        public List<string> CharsComparacao { get; set; }
        public List<string> CharsRepeticao { get; set; }
        public List<string> CharsAtribuicao { get; set; }
        public List<string> Tipos { get; set; }
        public List<string> Variaveis { get; set; }
        public string CharFimLinha { get; set; }
        public string CharInicioCondicao { get; set; }
        public string CharFimCondicao { get; set; }
        public string CharInicioBloco { get; set; }
        public string CharFimBloco { get; set; }

        public StaticDB()
        {
            CharFimLinha = ";";
            CharInicioCondicao = "(";
            CharFimCondicao = ")";
            CharInicioBloco = "{";
            CharFimBloco = "}";
            EstruturasRepeticao = new List<string>() {
                "FOREACH"
            };
            EstruturaCondicao = new List<string>() {
                "IF","ELSE"
            };
            CharsComparacao = new List<string>() {
                "==","!=",">=","<="
            };
            CharsAtribuicao = new List<string>() {
                "=","-=","+="
            };
            Tipos = new List<string>() {
                "INT","STRING","BOOLEAN"
            };
            CharsRepeticao = new List<string>() {
                "IN"
            };
            Variaveis = new List<string>();
        }
    }

    /*

    ^(IF|ELSE)+(.*.)+{$

    if(a==b){
    if(a==b){
    }
    }

    */
}