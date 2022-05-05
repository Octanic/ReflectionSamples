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
                PropertyInfo propriedadeDestino = destino.GetType().GetProperties().Where(p=> p.Name.Equals(propriedadeOrigem.Name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                if (propriedadeDestino != null)
                {
                    propriedadeDestino.SetValue(destino, propriedadeOrigem.GetValue(origem));
                }
            }

            return destino;
        }
    }
}
