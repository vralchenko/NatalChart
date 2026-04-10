import { useMutation } from '@tanstack/react-query';
import { astroApi } from '../api/astroApi';
import type { SynastryRequest } from '../types/chart';

export function useSynastry() {
  const mutation = useMutation({
    mutationFn: (data: SynastryRequest) => astroApi.calculateSynastry(data),
  });

  return {
    calculateSynastry: mutation.mutate,
    synastryResult: mutation.data,
    synastryLoading: mutation.isPending,
    synastryError: mutation.error,
  };
}
