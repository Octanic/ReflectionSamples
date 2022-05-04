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

            //IMPORTANTE: toda vez que você vai procurar propriedades públicas ou privadas, use o operador "OU binário" para incluir na busca a binding flag instance,
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
        public static void obterMetodos()
        {
            //Obter métodos é tão fácil quanto obter propriedades.
            //Neste caso, não estou passando BindingFlags.
            var metodos = territorio.GetType().GetMethods();

            foreach (var metodo in metodos)
            {
                Console.WriteLine($"Método descoberto: {metodo.Name}");
                Console.WriteLine("----");
            }

            //Veja que aqui, ele vai retornar um monte de métodos. Especialmente Getters e Setters das propriedades (que nós normalmente não preenchemos o que ele faz, mas são métodos sim)

            //Podemos também executar um método específico.
            var metodoReport = territorio.GetType().GetMethod("GenerateReport");
            object report = metodoReport.Invoke(territorio, null);

            Console.WriteLine(report);

            //e sim.... se você pensou... é possível então chamar um método privado dessa forma... 

        }

        //Quebrando regras: Encapsulamento
        public static void obterStringAgentes()
        {
            //é possível sim!
            //O método abaixo já conhecemos... porém, 
            var metodos = territorio.GetType().GetMethods( System.Reflection.BindingFlags.DeclaredOnly | //Apenas membro desta classe, não quero as derivadas
                                                            System.Reflection.BindingFlags.Instance | // Todos os membros vindos de instância
                                                            System.Reflection.BindingFlags.NonPublic); // Todos os membros não públicos.
            foreach (var metodo in metodos)
            {
                Console.WriteLine($"Método descoberto: {metodo.Name}");
                Console.WriteLine("----");

                object retornoMetodo = metodo.Invoke(territorio, null);
                Console.WriteLine(retornoMetodo);

                //Se houvessem mais métodos nessa classe, ele iria invocar todos aqui, sem passar parâmetro. Isso quebra o invoke? Sim.
            }

            //Parabéns, vc acaba de quebrar a ideia de encapsulamento. - btw, tem regra do SonarQube que impede isso. RSPEC-3011

            //Único cenário onde isso pode vir a ser uma utilidade: teste automatizado em código legado.

            //Dá para fazer coisas piores também
        }

        //Quebrando regras: Classes abstratas
        public static void criarObjetoPorContrato()
        {

        }

        //Carregar um Assembly
        public static void carregarAssembly()
        {

        }

        //Mini mapper
        public static void chamarExemploMiniMapper()
        {

        }
        //Serializador/Deserializador de xml 
        public static void chamarExemploSerializacaoComProtocoloDuvidoso()
        {

        }
        public static void chamarExemploDeserializacaoComProtocoloDuvidoso()
        {

        }

        //Criando expressões para combar filtragens usando LINQ
        public static void chamarExemploComboFiltro()
        {

        }
    }
}
