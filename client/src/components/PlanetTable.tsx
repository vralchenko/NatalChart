import {
  Table, TableBody, TableCell, TableContainer, TableHead, TableRow,
  Paper, Chip, Typography, Tooltip,
} from '@mui/material';
import { useLang } from '../context/LangContext';
import { getPlanetName, getSignName, signTraits } from '../i18n';
import type { Lang } from '../i18n';
import type { PlanetPosition } from '../types/chart';

const SIGN_SYMBOLS: Record<string, string> = {
  Aries: '\u2648', Taurus: '\u2649', Gemini: '\u264A', Cancer: '\u264B',
  Leo: '\u264C', Virgo: '\u264D', Libra: '\u264E', Scorpio: '\u264F',
  Sagittarius: '\u2650', Capricorn: '\u2651', Aquarius: '\u2652', Pisces: '\u2653',
};

const PLANET_SYMBOLS: Record<string, string> = {
  Sun: '\u2609', Moon: '\u263D', Mercury: '\u263F', Venus: '\u2640',
  Mars: '\u2642', Jupiter: '\u2643', Saturn: '\u2644', Uranus: '\u2645',
  Neptune: '\u2646', Pluto: '\u2647', NorthNode: '\u260A', Chiron: '\u26B7',
  Ascendant: 'ASC', Midheaven: 'MC',
};

interface Props {
  planets: PlanetPosition[];
}

export const PlanetTable: React.FC<Props> = ({ planets }) => {
  const { t, lang } = useLang();

  return (
    <TableContainer component={Paper} sx={{
      background: 'rgba(255,255,255,0.05)', backdropFilter: 'blur(20px)',
      border: '1px solid rgba(255,255,255,0.1)', borderRadius: 4,
    }}>
      <Table size="small">
        <TableHead>
          <TableRow>
            <TableCell sx={{ color: '#a855f7', fontWeight: 700 }}>{t.planet}</TableCell>
            <TableCell sx={{ color: '#a855f7', fontWeight: 700 }}>{t.sign}</TableCell>
            <TableCell sx={{ color: '#a855f7', fontWeight: 700 }}>{t.position}</TableCell>
            <TableCell sx={{ color: '#a855f7', fontWeight: 700 }}>
              <Tooltip title={t.houseTooltip} arrow>
                <span>{t.house}</span>
              </Tooltip>
            </TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {planets.map((p) => (
            <>
              <TableRow key={p.body}>
                <TableCell sx={{ color: 'white' }}>
                  <Typography component="span" sx={{ mr: 1 }}>
                    {PLANET_SYMBOLS[p.body] || ''}
                  </Typography>
                  {getPlanetName(lang, p.body)}
                </TableCell>
                <TableCell sx={{ color: 'white' }}>
                  {SIGN_SYMBOLS[p.sign] || ''} {getSignName(lang, p.sign)}
                </TableCell>
                <TableCell sx={{ color: 'white' }}>
                  {p.signDegree}{'\u00B0'} {String(p.signMinute).padStart(2, '0')}' {String(p.signSecond).padStart(2, '0')}"
                </TableCell>
                <TableCell sx={{ color: 'white' }}>
                  {p.house > 0 ? p.house : '-'}
                  {p.isRetrograde && (
                    <Tooltip title={t.retrogradeTooltip} arrow>
                      <Chip
                        label={`\u211E ${t.retrogradeLabel}`}
                        size="small"
                        color="error"
                        sx={{ ml: 1 }}
                      />
                    </Tooltip>
                  )}
                </TableCell>
              </TableRow>
              {p.body !== 'Ascendant' && p.body !== 'Midheaven' && signTraits[lang as Lang]?.[p.sign] && (
                <TableRow key={`${p.body}-trait`}>
                  <TableCell colSpan={4} sx={{ borderBottom: '1px solid rgba(255,255,255,0.05)', py: 0.5, pl: 4 }}>
                    <Typography sx={{ color: 'rgba(255,255,255,0.45)', fontSize: '0.8rem', fontStyle: 'italic' }}>
                      {PLANET_SYMBOLS[p.body] || ''} {getPlanetName(lang, p.body)} {t.inSign} {getSignName(lang, p.sign)} — {signTraits[lang as Lang][p.sign]}
                    </Typography>
                  </TableCell>
                </TableRow>
              )}
            </>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};
