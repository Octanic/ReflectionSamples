using System.Collections.Generic;
using System.Text;

namespace ReflectionSamples.Exemplos
{
    public class Territorio
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Nome { get; set; }
        public IList<Agente> Agentes { get; set; }
        public double AreaInSquareMeters { get; set; }

        public string GenerateReport()
        {
            return $"Território {Nome}\n Localização {Latitude},{Longitude}\nArea: {AreaInSquareMeters}m²\n{getAgentDescription()}";
        }

        private string getAgentDescription()
        {
            if (Agentes == null || Agentes.Count == 0) return "Nenhum agente relacionado.";
            StringBuilder sb = new StringBuilder();

            foreach (var agt in Agentes)
            {
                sb.Append($"\tNome do agente: {agt.Nome}\n\tCPF/CNPJ:{agt.CpfCnpj}\n");

            }

            return sb.ToString();
        }

    }
}
