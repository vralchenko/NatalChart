using NatalChart.Core.Models;

namespace NatalChart.Core.Interfaces;

public interface INumerologyService
{
    NumerologyResult Calculate(DateTime birthDate, string lang = "en");
}
