import { useMutation } from '@tanstack/react-query';
import { astroApi } from '../api/astroApi';
import type { BirthData, NatalChartResult } from '../types/chart';

export function useChart() {
  const chartMutation = useMutation({
    mutationFn: (data: BirthData) => astroApi.calculateChart(data),
  });

  const interpretMutation = useMutation({
    mutationFn: ({ chart, lang }: { chart: NatalChartResult; lang: string }) =>
      astroApi.interpretChart(chart, lang),
  });

  const numerologyMutation = useMutation({
    mutationFn: ({ birthDate, lang }: { birthDate: string; lang: string }) =>
      astroApi.calculateNumerology(birthDate, lang),
  });

  return {
    calculateChart: chartMutation.mutate,
    chartResult: chartMutation.data,
    chartLoading: chartMutation.isPending,
    chartError: chartMutation.error,

    interpretChart: interpretMutation.mutate,
    interpretations: interpretMutation.data,
    interpretLoading: interpretMutation.isPending,

    calculateNumerology: numerologyMutation.mutate,
    numerologyResult: numerologyMutation.data,
  };
}
