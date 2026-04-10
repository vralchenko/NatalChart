import { useMutation } from '@tanstack/react-query';
import { astroApi } from '../api/astroApi';
import type { TransitRequest } from '../types/chart';

export function useTransits() {
  const mutation = useMutation({
    mutationFn: (data: TransitRequest) => astroApi.calculateTransits(data),
  });

  return {
    calculateTransits: mutation.mutate,
    transitResult: mutation.data,
    transitLoading: mutation.isPending,
    transitError: mutation.error,
  };
}
