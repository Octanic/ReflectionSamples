using ReflectionSamples.Exemplos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReflectionSamples.Solved
{
    public static class GeradorDados
    {
        public static Territorio ObterTerritorioExemplo()
        {
            return new Territorio()
            {
                Agentes = new List<Agente>() {
                    new Agente() {  Nome= "José Josias Josino", CpfCnpj= "887.102.130-40" },
                    new Agente() {  Nome= "Madeireira XYZ", CpfCnpj= "43.749.680/0001-53" }
                },
                Nome = "Madeireira Xyz",
                AreaInSquareMeters = 12000,
                Latitude = -19.472620,
                Longitude = -49.205339
            };
        }
    }
}
