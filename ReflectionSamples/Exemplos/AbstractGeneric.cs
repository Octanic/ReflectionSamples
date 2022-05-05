using System;
using System.Reflection;

namespace ReflectionSamples.Exemplos
{
    class AbstractGeneric
    {

        static void Run()
        {
            Type myAbstractGenericType = typeof(AbstractGeneric<>);
            Type[] types = { typeof(int) };
            Type constructed = myAbstractGenericType.MakeGenericType(types);
            FieldInfo[] fieldInfos = constructed.GetFields(
                BindingFlags.NonPublic |
                BindingFlags.Public |
                BindingFlags.Static);
            foreach (FieldInfo fi in fieldInfos)
            {
                Console.WriteLine("Name: {0}  Value: {1}", fi.Name, fi.GetValue(null));
            }
            Console.ReadLine();
        }

    }
    public abstract class AbstractGeneric<T>
    {
        private static int anInt = 12345;
        private static String aString = "XXYYZZ";
        public abstract T WhatEver { get; set; }
    }
}
