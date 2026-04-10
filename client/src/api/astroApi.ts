import axios from 'axios';
import type {
  BirthData,
  NatalChartResult,
  InterpretationResult,
  SynastryResult,
  TransitResult,
  GeocodingResult,
  SynastryRequest,
  TransitRequest,
} from '../types/chart';

const api = axios.create({
  baseURL: 'http://localhost:5102/api',
});

export const astroApi = {
  calculateChart: (data: BirthData) =>
    api.post<NatalChartResult>('/chart/calculate', data).then(r => r.data),

  interpretChart: (chart: NatalChartResult) =>
    api.post<InterpretationResult>('/chart/interpret', chart).then(r => r.data),

  calculateSynastry: (data: SynastryRequest) =>
    api.post<SynastryResult>('/synastry/calculate', data).then(r => r.data),

  calculateTransits: (data: TransitRequest) =>
    api.post<TransitResult>('/transit/calculate', data).then(r => r.data),

  searchLocation: (query: string) =>
    api.get<GeocodingResult[]>('/geocoding/search', { params: { query } }).then(r => r.data),
};
