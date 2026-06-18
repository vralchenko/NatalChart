import {
  Accordion, AccordionSummary, AccordionDetails, Typography, Box, Paper,
} from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { useLang } from '../context/LangContext';
import { getPlanetName, getSignName, getAspectName, getSignLocative, getPlanetGenitive, signTraits, elementDescriptions } from '../i18n';
import type { Lang } from '../i18n';
import type { InterpretationResult, InterpretationEntry, NatalChartResult } from '../types/chart';

interface Props {
  interpretations: InterpretationResult;
  chart?: NatalChartResult;
}

const ELEMENTS_EN: Record<string, string[]> = {
  Fire: ['Aries', 'Leo', 'Sagittarius'],
  Earth: ['Taurus', 'Virgo', 'Capricorn'],
  Air: ['Gemini', 'Libra', 'Aquarius'],
  Water: ['Cancer', 'Scorpio', 'Pisces'],
};

const ELEMENT_NAMES: Record<string, Record<string, string>> = {
  en: { Fire: 'Fire', Earth: 'Earth', Air: 'Air', Water: 'Water' },
  ru: { Fire: 'Огонь', Earth: 'Земля', Air: 'Воздух', Water: 'Вода' },
  de: { Fire: 'Feuer', Earth: 'Erde', Air: 'Luft', Water: 'Wasser' },
  uk: { Fire: 'Вогонь', Earth: 'Земля', Air: 'Повітря', Water: 'Вода' },
};

function getElement(sign: string): string {
  for (const [el, signs] of Object.entries(ELEMENTS_EN)) {
    if (signs.includes(sign)) return el;
  }
  return 'Unknown';
}

function translateTitle(entry: InterpretationEntry, lang: Lang, inSign: string, inHouse: string): string {
  const parts = entry.title.split(' ');

  if (parts.length === 3 && parts[1] === 'in') {
    const planet = getPlanetName(lang, parts[0]);
    // Inflected languages (uk, ru) use the locative case ("Місяць у Раку")
    const locative = getSignLocative(lang, parts[2]);
    if (locative) return `${planet} ${locative}`;
    return `${planet} ${inSign} ${getSignName(lang, parts[2])}`;
  }

  if (parts.length === 4 && parts[1] === 'in' && parts[2] === 'House') {
    const planet = getPlanetName(lang, parts[0]);
    if (lang === 'uk') return `${planet} у ${parts[3]}-му домі`;
    if (lang === 'ru') return `${planet} в ${parts[3]}-м доме`;
    return `${planet} ${inHouse} ${parts[3]}`;
  }

  if (parts.length === 3) {
    const aspect = getAspectName(lang, parts[1]);
    // Inflected languages mirror the body text: "Тригон Місяця і Юпітера"
    if (lang === 'uk' || lang === 'ru') {
      const conj = lang === 'uk' ? 'і' : 'и';
      return `${aspect} ${getPlanetGenitive(lang, parts[0])} ${conj} ${getPlanetGenitive(lang, parts[2])}`;
    }
    return `${getPlanetName(lang, parts[0])} — ${aspect} — ${getPlanetName(lang, parts[2])}`;
  }

  return entry.title;
}

export const InterpretationPanel: React.FC<Props> = ({ interpretations, chart }) => {
  const { t, lang } = useLang();

  const sections = [
    { title: t.planetsInSigns, entries: interpretations.planetInSign, color: '#a855f7' },
    { title: t.planetsInHouses, entries: interpretations.planetInHouse, color: '#6366f1' },
    { title: t.aspects, entries: interpretations.aspects, color: '#22c55e' },
  ];

  let summaryText = '';
  if (chart) {
    const sun = chart.planets.find(p => p.body === 'Sun');
    const moon = chart.planets.find(p => p.body === 'Moon');
    const asc = chart.planets.find(p => p.body === 'Ascendant');

    const parts: string[] = [];

    if (sun) {
      const trait = signTraits[lang as Lang]?.[sun.sign] || '';
      parts.push(`${t.sunIn} ${getSignName(lang, sun.sign)} (${t.house} ${sun.house}) — ${trait}`);
    }
    if (moon) {
      const trait = signTraits[lang as Lang]?.[moon.sign] || '';
      parts.push(`${t.moonIn} ${getSignName(lang, moon.sign)} (${t.house} ${moon.house}) — ${trait}`);
    }
    if (asc) {
      const trait = signTraits[lang as Lang]?.[asc.sign] || '';
      parts.push(`${t.ascendantIn} ${getSignName(lang, asc.sign)} — ${trait}`);
    }

    // Count elements
    const elCount: Record<string, number> = { Fire: 0, Earth: 0, Air: 0, Water: 0 };
    for (const p of chart.planets) {
      if (p.body === 'Ascendant' || p.body === 'Midheaven') continue;
      elCount[getElement(p.sign)]++;
    }
    const dominant = Object.entries(elCount).sort((a, b) => b[1] - a[1])[0];
    const elName = ELEMENT_NAMES[lang]?.[dominant[0]] || dominant[0];
    const elDesc = elementDescriptions[lang as Lang]?.[dominant[0]] || '';

    // Count harmonious vs challenging aspects
    const harmonious = chart.aspects.filter(a => a.type === 'Trine' || a.type === 'Sextile').length;
    const challenging = chart.aspects.filter(a => a.type === 'Square' || a.type === 'Opposition').length;

    summaryText = `${t.summaryIntro}\n\n${parts.join('. ')}.\n\n${t.dominantElement}: ${elName} (${dominant[1]}) — ${elDesc}.\n\n${t.aspectBalance}: ${t.harmonious.toLowerCase()} ${harmonious}, ${t.challenging.toLowerCase()} ${challenging}.`;
  }

  return (
    <Box>
      <Typography variant="h6" gutterBottom sx={{ color: 'white', fontWeight: 700 }}>
        {t.interpretations}
      </Typography>

      {summaryText && (
        <Paper sx={{
          p: 3, mb: 3,
          background: 'linear-gradient(135deg, rgba(168,85,247,0.15), rgba(99,102,241,0.15))',
          border: '1px solid rgba(168,85,247,0.3)',
          borderRadius: 3,
        }}>
          <Typography variant="subtitle1" sx={{ color: '#a855f7', fontWeight: 700, mb: 1 }}>
            {t.summary}
          </Typography>
          <Typography sx={{ color: '#e2e8f0', lineHeight: 1.8, whiteSpace: 'pre-line' }}>{summaryText}</Typography>
        </Paper>
      )}

      {sections.map((section) => (
        <Box key={section.title} sx={{ mb: 2 }}>
          <Typography variant="subtitle1" sx={{ color: section.color, fontWeight: 600, mb: 1 }}>
            {section.title}
          </Typography>
          {section.entries.map((entry) => (
            <Accordion
              key={entry.key}
              sx={{
                background: 'rgba(255,255,255,0.05)',
                border: '1px solid rgba(255,255,255,0.1)',
                '&:before': { display: 'none' },
                mb: 1, borderRadius: '8px !important',
              }}
            >
              <AccordionSummary expandIcon={<ExpandMoreIcon sx={{ color: 'white' }} />}>
                <Typography sx={{ color: 'white', fontWeight: 600 }}>
                  {translateTitle(entry, lang, t.inSign, t.inHouse)}
                </Typography>
              </AccordionSummary>
              <AccordionDetails>
                <Typography sx={{ color: '#d1d5db' }}>{entry.text}</Typography>
              </AccordionDetails>
            </Accordion>
          ))}
          {section.entries.length === 0 && (
            <Typography sx={{ color: '#6b7280', fontStyle: 'italic' }}>
              {t.noInterpretations}
            </Typography>
          )}
        </Box>
      ))}
    </Box>
  );
};
