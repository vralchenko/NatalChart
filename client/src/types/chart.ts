export enum CelestialBody {
  Sun = 'Sun',
  Moon = 'Moon',
  Mercury = 'Mercury',
  Venus = 'Venus',
  Mars = 'Mars',
  Jupiter = 'Jupiter',
  Saturn = 'Saturn',
  Uranus = 'Uranus',
  Neptune = 'Neptune',
  Pluto = 'Pluto',
  NorthNode = 'NorthNode',
  Chiron = 'Chiron',
  Ascendant = 'Ascendant',
  Midheaven = 'Midheaven',
}

export enum ZodiacSign {
  Aries = 'Aries',
  Taurus = 'Taurus',
  Gemini = 'Gemini',
  Cancer = 'Cancer',
  Leo = 'Leo',
  Virgo = 'Virgo',
  Libra = 'Libra',
  Scorpio = 'Scorpio',
  Sagittarius = 'Sagittarius',
  Capricorn = 'Capricorn',
  Aquarius = 'Aquarius',
  Pisces = 'Pisces',
}

export enum AspectType {
  Conjunction = 'Conjunction',
  Opposition = 'Opposition',
  Trine = 'Trine',
  Square = 'Square',
  Sextile = 'Sextile',
  Quincunx = 'Quincunx',
}

export enum HouseSystem {
  Placidus = 'Placidus',
  Koch = 'Koch',
  Equal = 'Equal',
  WholeSign = 'WholeSign',
  Campanus = 'Campanus',
  Regiomontanus = 'Regiomontanus',
}

export interface BirthData {
  birthDate: string;
  birthTime: string;
  latitude: number;
  longitude: number;
  timeZoneId?: string;
  houseSystem: number;
}

export interface PlanetPosition {
  body: CelestialBody;
  longitude: number;
  latitude: number;
  speed: number;
  isRetrograde: boolean;
  sign: ZodiacSign;
  signDegree: number;
  signMinute: number;
  signSecond: number;
  house: number;
}

export interface HouseCusp {
  houseNumber: number;
  longitude: number;
  sign: ZodiacSign;
  degree: number;
  minute: number;
}

export interface Aspect {
  body1: CelestialBody;
  body2: CelestialBody;
  type: AspectType;
  angle: number;
  orb: number;
  isApplying: boolean;
}

export interface NatalChartResult {
  planets: PlanetPosition[];
  houses: HouseCusp[];
  aspects: Aspect[];
}

export interface InterpretationEntry {
  key: string;
  title: string;
  text: string;
}

export interface InterpretationResult {
  planetInSign: InterpretationEntry[];
  planetInHouse: InterpretationEntry[];
  aspects: InterpretationEntry[];
}

export interface SynastryResult {
  chart1: NatalChartResult;
  chart2: NatalChartResult;
  interAspects: Aspect[];
}

export interface TransitResult {
  natalChart: NatalChartResult;
  transitPlanets: PlanetPosition[];
  transitAspects: Aspect[];
}

export interface GeocodingResult {
  displayName: string;
  latitude: number;
  longitude: number;
}

export interface SynastryRequest {
  person1: BirthData;
  person2: BirthData;
}

export interface TransitRequest {
  natalData: BirthData;
  transitDate?: string;
}
