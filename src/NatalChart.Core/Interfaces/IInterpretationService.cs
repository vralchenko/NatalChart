using NatalChart.Core.Models;

namespace NatalChart.Core.Interfaces;

public interface IInterpretationService
{
    InterpretationResult GetInterpretations(NatalChartResult chart, string lang = "en");
}
