using ReflectionSamples.Classes;
using System.Collections.Generic;

namespace ReflectionSamples.DTO
{
    public class TerritorioDTO
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string nome { get; set; }
        public IList<Agente> agentes { get; set; }
        public double areaInSquareMeters { get; set; }
    }
}
