using ReflectionSamples.Exemplos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReflectionSamples.Solved
{
    class Solved
    {
        static Territorio territorio;
        static Solved()
        {
            territorio = GeradorDados.ObterTerritorioExemplo();
        }
        //Acessar propriedades
        public static void obterPropriedades()
        {
            //Para obter as propriedades, primeiro, pega-se o tipo, seja com typeof ou GetType (a diferença é que um precisa de uma instância e outro não)

            //Lembre-se, o reflection tem uma lógica de nomenclatura para seus métodos. Ele permite acessar um ou vários membros com Get + o tipo do membro.
            //  Se for plural, ele vai retornar uma list de ___Info, onde ___ é o tipo de membro que vc quer acessar, se é propriedade, método

            //IMPORTANTE: toda vez que você vai procurar propriedades públicas ou privadas, use o operador de bit | para incluir na busca a binding flag instance,
            //              para buscar membros que surgem à partir de instâncias, ou static, para membros que estão estaticamente definidos num tipo
            var propriedades = typeof(Territorio).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            //é possível obter o tipo pai da reflexão apenas voltando ao ReflectedType
            Console.WriteLine($"Tipo refletido:{propriedades[0].ReflectedType.Name}");

            //Aí navega-se pela array de PropertyInfo e tudo o que ele tiver, ele vai mostrar
            foreach (var propriedade in propriedades)
            {
                //Nome...
                Console.WriteLine($"Nome: {propriedade.Name}");
                //Tipo...
                Console.WriteLine($"Tipo: {propriedade.PropertyType.Name}");
                //Até é possível obter o valor da propriedade de um objeto já instanciado, ele confere com o metadado da property com a do objeto e consegue obter o valor
                Console.WriteLine($"Valor: {propriedade.GetValue(territorio)}");
                Console.WriteLine(new string('-', 10));
            }
        }

        //Acessar métodos
        static void obterMetodos()
        {

        }

        //Quebrando regras: Interfaces
        static void criarObjetoPorContrato()
        {

        }

        //Quebrando regras: Encapsulamento
        static void obterStringAgentes()
        {

        }

        //Carregar um Assembly
        static void carregarAssembly()
        {

        }

        //Mini mapper
        static void chamarExemploMiniMapper()
        {

        }
        //Serializador/Deserializador de xml 
        static void chamarExemploSerializacaoComProtocoloDuvidoso()
        {

        }
        static void chamarExemploDeserializacaoComProtocoloDuvidoso()
        {

        }

        //Criando expressões para combar filtragens usando LINQ
        static void chamarExemploComboFiltro()
        {

        }
    }
}
