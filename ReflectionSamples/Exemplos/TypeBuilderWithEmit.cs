using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ReflectionSamples.Exemplos
{
    /// <summary>
    /// Este exemplo é a criação de um tipo dinamicamente... 
    /// Não sei de uma aplicação sobre isso, mas mostra um pouco do real poder do que o reflection pode fazer.
    /// </summary>
    /// <remarks>
    /// Código obtido da documentação do .NET sobre como usar o type builder
    /// https://docs.microsoft.com/pt-br/dotnet/api/system.reflection.emit.typebuilder?view=net-6.0
    /// </remarks>
    public class TypeBuilderWithEmit
    {
        public static void GenerateTypeDynamically()
        {
            // Este código cria um assembly que tem um tipo, chamado de 
            // "MyDynamicType", que tem um tipo privado, uma propriedade que 
            // obtém ou define um campo privado, construtores que inicializam
            // inicializam o campo privado, e um método que multiplica um 
            // número suprido pelo campo privado e retorna um resultado.
            // Em C#, o tipo seria mais ou menos assim:
            /*
            public class MyDynamicType
            {
                private int m_number;

                public MyDynamicType() : this(42) {}
                public MyDynamicType(int initNumber)
                {
                    m_number = initNumber;
                }

                public int Number
                {
                    get { return m_number; }
                    set { m_number = value; }
                }

                public int MyMethod(int multiplier)
                {
                    return m_number * multiplier;
                }
            }
            */

            AssemblyName aName = new AssemblyName("DynamicAssemblyExample");
            AssemblyBuilder ab = AssemblyBuilder.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);

            // O nome do módulo é normalmente o mesmo do nome do assembly
            ModuleBuilder mb = ab.DefineDynamicModule(aName.Name);

            TypeBuilder tb = mb.DefineType("MyDynamicType", TypeAttributes.Public);

            // adiciona um campo privado do tipo int (Int32).
            FieldBuilder fbNumber = tb.DefineField("m_number", typeof(int), FieldAttributes.Private);

            // Define um construtor que pega um argumento int e armazena-o em um campo privado
            Type[] parameterTypes = { typeof(int) };
            ConstructorBuilder ctor1 = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, parameterTypes);

            ILGenerator ctor1IL = ctor1.GetILGenerator();
            // Para um construtor, argumento 0 é a referência para a nova
            // instância. Empurre-a para a stack antes de chamar o construtor da classe base.
            // Especifique o construtor padrão da classe base (System.Object)
            // passando uma array vazia de tipos (Type.EmptyTypes) para o GetConstructor.
            ctor1IL.Emit(OpCodes.Ldarg_0);
            ctor1IL.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));

            // Empurre a instância na stack antes de mandar o argumento
            // isso estará para ser definido ao campo privado m_number.
            ctor1IL.Emit(OpCodes.Ldarg_0);
            ctor1IL.Emit(OpCodes.Ldarg_1);
            ctor1IL.Emit(OpCodes.Stfld, fbNumber);
            ctor1IL.Emit(OpCodes.Ret);

            // Define um construtor padrão que fornece um valor padrão ao campo privado
            // Para tipos de parâmetro, passe um array de tipos vazios ou null.
            ConstructorBuilder ctor0 = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);

            ILGenerator ctor0IL = ctor0.GetILGenerator();
            // Para o construtor, argumento zero é a referência para a nova instância.
            // Empurre-a para a stack antes de empurrar o valor padrão para a stack
            // e então chame o construtor ctor1.
            ctor0IL.Emit(OpCodes.Ldarg_0);
            ctor0IL.Emit(OpCodes.Ldc_I4_S, 42);
            ctor0IL.Emit(OpCodes.Call, ctor1);
            ctor0IL.Emit(OpCodes.Ret);

            // Define uma propriedade chamada Number que obtém e define o campo privado
            //
            // O último argumento da DefineProperty é null, porque a
            // propriedade não tem parâmetros. (Se vc não especificar null, vc precisa
            // especificar uma array de objetos Type. Para uma propriedade sem parâmetros, 
            // use a array sem elementos que já existe: Type.EmptyTypes)
            PropertyBuilder pbNumber = tb.DefineProperty("Number", PropertyAttributes.HasDefault, typeof(int), null);

            // Os métodos da propriedade "set" e a propriedade "get" necessitam um conjunto especial de atributos
            MethodAttributes getSetAttr = MethodAttributes.Public |
                MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            // Define o método de acesso "get" para Number.
            // O método retorna um inteiro e não tem argumento
            // (Note que null pode ser usado ao invés de Types.EmptyTypes)
            MethodBuilder mbNumberGetAccessor = tb.DefineMethod("get_Number", getSetAttr, typeof(int), Type.EmptyTypes);

            ILGenerator numberGetIL = mbNumberGetAccessor.GetILGenerator();
            // Para uma propriedade de instância, argumento zero é a instância. Carregue a instância primeiro,
            // então carregue o campo privado e retorne, deixando o valor do campo na pilha.
            numberGetIL.Emit(OpCodes.Ldarg_0);
            numberGetIL.Emit(OpCodes.Ldfld, fbNumber);
            numberGetIL.Emit(OpCodes.Ret);

            // Define o método de acesso "set" para Number, que não tem tipo de retorno
            // e toma um argumento do tipo int (Int32).
            MethodBuilder mbNumberSetAccessor = tb.DefineMethod("set_Number", getSetAttr, null, new Type[] { typeof(int) });

            ILGenerator numberSetIL = mbNumberSetAccessor.GetILGenerator();
            // Carrega a instância e então o argumento numérico, e o armazena no campo.
            numberSetIL.Emit(OpCodes.Ldarg_0);
            numberSetIL.Emit(OpCodes.Ldarg_1);
            numberSetIL.Emit(OpCodes.Stfld, fbNumber);
            numberSetIL.Emit(OpCodes.Ret);

            // Por fim, mapeia os métodos de acesso get e set para o 
            // PropertyBuilder. A propriedade está completa.
            pbNumber.SetGetMethod(mbNumberGetAccessor);
            pbNumber.SetSetMethod(mbNumberSetAccessor);

            // Define um método que aceita um argumento inteiro e retorna 
            // o produto daquele inteiro com o campo privado m_number. 
            // Dessa vez, a array de tipos de parâmetro é criada ali na hora.
            MethodBuilder meth = tb.DefineMethod("MyMethod", MethodAttributes.Public, typeof(int), new Type[] { typeof(int) });

            ILGenerator methIL = meth.GetILGenerator();
            // Para obter o campo da instância privada, carregue a instância a qual ele pertence,
            // Após carregar o campo, carregue o argumento 1, e então multiplique.
            // Retorne do método com o valor do retorno (produto de dois números) na pilha de execução.
            methIL.Emit(OpCodes.Ldarg_0);
            methIL.Emit(OpCodes.Ldfld, fbNumber);
            methIL.Emit(OpCodes.Ldarg_1);
            methIL.Emit(OpCodes.Mul);
            methIL.Emit(OpCodes.Ret);

            // Finalize o tipo.
            Type t = tb.CreateType();

            // Porque o AssemblyBuilderAccess inclui o método Run, o código pode ser
            // executado imediatamente. Comece obtendo os objetos de reflexão para 
            // o método e a propriedade.
            MethodInfo mi = t.GetMethod("MyMethod");
            PropertyInfo pi = t.GetProperty("Number");

            // Crie uma instância de MyDynamicType usando o construtor padrão.
            object o1 = Activator.CreateInstance(t);

            // Mostre o valor da propriedade e então altere-a para 127 
            // e mostre-a novamente. Use null para indicar que a propriedade não tem index.
            Console.WriteLine("O valor padrão na classe é 42.\no1.Number: {0}", pi.GetValue(o1, null));
            pi.SetValue(o1, 127, null);
            Console.WriteLine("depois de definir o número acima para 127:\no1.Number: {0}", pi.GetValue(o1, null));

            // Chame MyMethod, passando 22, e mostre o valor de retorno, 22 * 127.
            // Argumentos devem ser passados como array, mesmo sendo apenas 1.
            object[] arguments = { 22 };
            Console.WriteLine("multiplica o número definido anteriormente por 22:\no1.MyMethod(22): {0}", mi.Invoke(o1, arguments));

            // Cria uma instância de MyDynamicType usando o construtor que especifica 
            // m_Number. O construtor é identificado por bater os tipos na array de argumentos.
            // Neste caso, a array de argumentos é criada ali na hora. Daí, mostra o valor da propriedade.
            object o2 = Activator.CreateInstance(t, new object[] { 5280 });
            Console.WriteLine("cria nova instância com o número {0}\no2.Number: {0}", pi.GetValue(o2, null));
        }
    }

}
