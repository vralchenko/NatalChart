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
  NumerologyResult,
} from '../types/chart';

const api = axios.create({
  baseURL: (import.meta.env.VITE_API_URL || 'http://localhost:5102') + '/api',
});

export const astroApi = {
  calculateChart: (data: BirthData) =>
    api.post<NatalChartResult>('/chart/calculate', data).then(r => r.data),

  interpretChart: (chart: NatalChartResult, lang: string = 'en') =>
    api.post<InterpretationResult>('/chart/interpret', chart, { params: { lang } }).then(r => r.data),

  calculateSynastry: (data: SynastryRequest) =>
    api.post<SynastryResult>('/synastry/calculate', data).then(r => r.data),

  calculateTransits: (data: TransitRequest) =>
    api.post<TransitResult>('/transit/calculate', data).then(r => r.data),

  searchLocation: (query: string, lang: string = 'en') =>
    api.get<GeocodingResult[]>('/geocoding/search', { params: { query, lang } }).then(r => r.data),

  calculateNumerology: (birthDate: string, lang: string = 'en') =>
    api.post<NumerologyResult>('/chart/numerology', { birthDate }, { params: { lang } }).then(r => r.data),

  exportPdf: (birthData: BirthData, lang: string, locationName: string) =>
    api.post('/export/pdf', { birthData, lang, locationName }, { responseType: 'blob' }).then(r => r.data),
};
