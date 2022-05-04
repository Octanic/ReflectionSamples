using System.Collections.Generic;
using System.Text;

namespace ReflectionSamples.Exemplos
{
    public class Territorio:TerrenoAbstract
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Nome { get; set; }
        public IList<Agente> Agentes { get; set; }
        public double AreaInSquareMeters { get; set; }

        public override string ExibirTipoTerreno()
        {
            return "Território";
        }

        public string GenerateReport()
        {
            return $"Território {Nome}\nLocalização {Latitude},{Longitude}\nArea: {AreaInSquareMeters}m²\n{getAgentDescription()}";
        }

        private string getAgentDescription()
        {
            if (Agentes == null || Agentes.Count == 0) return "Nenhum agente relacionado.";
            StringBuilder sb = new StringBuilder();

            foreach (var agt in Agentes)
            {
                sb.Append($"\tNome do agente: {agt.Nome}\n\tCPF/CNPJ:{agt.CpfCnpj}\n\n");

            }

            return sb.ToString();
        }

    }
}
