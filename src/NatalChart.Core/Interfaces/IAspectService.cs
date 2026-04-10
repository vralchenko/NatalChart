using NatalChart.Core.Models;

namespace NatalChart.Core.Interfaces;

public interface IAspectService
{
    List<Aspect> CalculateAspects(List<PlanetPosition> planets);
    List<Aspect> CalculateInterAspects(List<PlanetPosition> planets1, List<PlanetPosition> planets2);
}
