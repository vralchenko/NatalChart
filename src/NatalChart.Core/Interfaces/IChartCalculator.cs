using NatalChart.Core.Models;

namespace NatalChart.Core.Interfaces;

public interface IChartCalculator
{
    NatalChartResult Calculate(BirthData birthData);
}
