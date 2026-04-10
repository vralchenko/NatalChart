import { useMutation } from '@tanstack/react-query';
import { astroApi } from '../api/astroApi';
import type { BirthData, NatalChartResult } from '../types/chart';

export function useChart() {
  const chartMutation = useMutation({
    mutationFn: (data: BirthData) => astroApi.calculateChart(data),
  });

  const interpretMutation = useMutation({
    mutationFn: (chart: NatalChartResult) => astroApi.interpretChart(chart),
  });

  return {
    calculateChart: chartMutation.mutate,
    chartResult: chartMutation.data,
    chartLoading: chartMutation.isPending,
    chartError: chartMutation.error,

    interpretChart: interpretMutation.mutate,
    interpretations: interpretMutation.data,
    interpretLoading: interpretMutation.isPending,
  };
}
