export type Lang = 'en' | 'ru';

const planetNames: Record<Lang, Record<string, string>> = {
  en: {
    Sun: 'Sun', Moon: 'Moon', Mercury: 'Mercury', Venus: 'Venus',
    Mars: 'Mars', Jupiter: 'Jupiter', Saturn: 'Saturn', Uranus: 'Uranus',
    Neptune: 'Neptune', Pluto: 'Pluto', NorthNode: 'North Node', Chiron: 'Chiron',
    Ascendant: 'Ascendant', Midheaven: 'Midheaven',
  },
  ru: {
    Sun: 'Солнце', Moon: 'Луна', Mercury: 'Меркурий', Venus: 'Венера',
    Mars: 'Марс', Jupiter: 'Юпитер', Saturn: 'Сатурн', Uranus: 'Уран',
    Neptune: 'Нептун', Pluto: 'Плутон', NorthNode: 'Сев. Узел', Chiron: 'Хирон',
    Ascendant: 'Асцендент', Midheaven: 'Середина Неба',
  },
};

const signNames: Record<Lang, Record<string, string>> = {
  en: {
    Aries: 'Aries', Taurus: 'Taurus', Gemini: 'Gemini', Cancer: 'Cancer',
    Leo: 'Leo', Virgo: 'Virgo', Libra: 'Libra', Scorpio: 'Scorpio',
    Sagittarius: 'Sagittarius', Capricorn: 'Capricorn', Aquarius: 'Aquarius', Pisces: 'Pisces',
  },
  ru: {
    Aries: 'Овен', Taurus: 'Телец', Gemini: 'Близнецы', Cancer: 'Рак',
    Leo: 'Лев', Virgo: 'Дева', Libra: 'Весы', Scorpio: 'Скорпион',
    Sagittarius: 'Стрелец', Capricorn: 'Козерог', Aquarius: 'Водолей', Pisces: 'Рыбы',
  },
};

const aspectNames: Record<Lang, Record<string, string>> = {
  en: {
    Conjunction: 'Conjunction', Opposition: 'Opposition', Trine: 'Trine',
    Square: 'Square', Sextile: 'Sextile', Quincunx: 'Quincunx',
  },
  ru: {
    Conjunction: 'Соединение', Opposition: 'Оппозиция', Trine: 'Трин',
    Square: 'Квадратура', Sextile: 'Секстиль', Quincunx: 'Квиконс',
  },
};

export const getPlanetName = (lang: Lang, key: string) => planetNames[lang][key] || key;
export const getSignName = (lang: Lang, key: string) => signNames[lang][key] || key;
export const getAspectName = (lang: Lang, key: string) => aspectNames[lang][key] || key;

export const translations = {
  en: {
    appName: 'NatalChart',
    natalChart: 'Natal Chart',
    synastry: 'Synastry',
    transits: 'Transits',
    birthData: 'Birth Data',
    dateOfBirth: 'Date of Birth',
    timeOfBirth: 'Time of Birth',
    placeOfBirth: 'Place of Birth',
    startTypingCity: 'Start typing a city...',
    houses: 'Houses',
    calculateChart: 'Calculate Chart',
    timeOptionalHint: 'Optional. Without time, houses and Ascendant will be approximate.',
    person1: 'Person 1',
    person1Saved: 'Person 1 (saved)',
    person2: 'Person 2',
    synastryTitle: 'Synastry (Compatibility)',
    transitsTitle: 'Current Transits',
    interpretations: 'Interpretations',
    planetsInSigns: 'Planets in Signs',
    planetsInHouses: 'Planets in Houses',
    aspects: 'Aspects',
    interAspects: 'Inter-Aspects (Synastry)',
    transitAspects: 'Transit Aspects',
    natalPlanets: 'Natal Planets',
    transitPlanetsLabel: 'Transit Planets',
    planet: 'Planet',
    sign: 'Sign',
    position: 'Position',
    house: 'House',
    speed: 'Speed',
    planet1: 'Planet 1',
    aspect: 'Aspect',
    planet2: 'Planet 2',
    orb: 'Orb',
    status: 'Status',
    applying: 'Applying',
    separating: 'Separating',
    inSign: 'in',
    inHouse: 'in House',
    errorChart: 'Failed to calculate chart. Please check your input and try again.',
    errorSynastry: 'Failed to calculate synastry.',
    errorTransits: 'Failed to calculate transits.',
    noInterpretations: 'No interpretations available for this section.',
  },
  ru: {
    appName: 'НатальнаяКарта',
    natalChart: 'Натальная карта',
    synastry: 'Синастрия',
    transits: 'Транзиты',
    birthData: 'Данные рождения',
    dateOfBirth: 'Дата рождения',
    timeOfBirth: 'Время рождения',
    placeOfBirth: 'Место рождения',
    startTypingCity: 'Начните вводить город...',
    houses: 'Дома',
    calculateChart: 'Рассчитать карту',
    timeOptionalHint: 'Необязательно. Без времени дома и Асцендент будут приблизительными.',
    person1: 'Персона 1',
    person1Saved: 'Персона 1 (сохранена)',
    person2: 'Персона 2',
    synastryTitle: 'Синастрия (совместимость)',
    transitsTitle: 'Текущие транзиты',
    interpretations: 'Интерпретации',
    planetsInSigns: 'Планеты в знаках',
    planetsInHouses: 'Планеты в домах',
    aspects: 'Аспекты',
    interAspects: 'Межаспекты (синастрия)',
    transitAspects: 'Транзитные аспекты',
    natalPlanets: 'Натальные планеты',
    transitPlanetsLabel: 'Транзитные планеты',
    planet: 'Планета',
    sign: 'Знак',
    position: 'Позиция',
    house: 'Дом',
    speed: 'Скорость',
    planet1: 'Планета 1',
    aspect: 'Аспект',
    planet2: 'Планета 2',
    orb: 'Орб',
    status: 'Статус',
    applying: 'Сходящийся',
    separating: 'Расходящийся',
    inSign: 'в',
    inHouse: 'в доме',
    errorChart: 'Не удалось рассчитать карту. Проверьте введённые данные.',
    errorSynastry: 'Не удалось рассчитать синастрию.',
    errorTransits: 'Не удалось рассчитать транзиты.',
    noInterpretations: 'Нет интерпретаций для этого раздела.',
  },
} as const;

export type Translations = typeof translations[Lang];
