using System;
using System.Linq;
using System.Reflection;

namespace ReflectionSamples.Solved
{
    public static class MiniMapper
    {
        public static TDestino Map<TOrigem, TDestino>(TOrigem origem, TDestino destino)
        {
            foreach (var propriedadeOrigem in origem.GetType().GetProperties())
            {
                PropertyInfo propriedadeDestino = destino.GetType().GetProperties().Where(p => p.Name.Equals(propriedadeOrigem.Name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                if (propriedadeDestino != null)
                {
                    propriedadeDestino.SetValue(destino, propriedadeOrigem.GetValue(origem));
                }
            }

            return destino;
        }

        public static TDestino Map<TOrigem, TDestino>(TOrigem origem) where TDestino : class
        {
            TDestino destino = (TDestino)Activator.CreateInstance(typeof(TDestino));

            foreach (var propriedadeOrigem in origem.GetType().GetProperties())
            {
                var propriedadeDestino = destino.GetType().GetProperties()
                                            .FirstOrDefault(x => x.Name.Equals(propriedadeOrigem.Name, StringComparison.InvariantCultureIgnoreCase)
                                                            && x.PropertyType == propriedadeOrigem.PropertyType);

                if (propriedadeDestino != null)
                {
                    propriedadeDestino.SetValue(destino, propriedadeOrigem.GetValue(origem));
                }
            }

            return destino;
        }
    }
}
