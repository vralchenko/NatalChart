import {
  Table, TableBody, TableCell, TableContainer, TableHead, TableRow,
  Paper, Chip, Typography, Box,
} from '@mui/material';
import { useLang } from '../context/LangContext';
import { getPlanetName, getAspectName } from '../i18n';
import type { Aspect, InterpretationEntry } from '../types/chart';

const ASPECT_SYMBOLS: Record<string, string> = {
  Conjunction: '\u260C',
  Opposition: '\u260D',
  Trine: '\u25B3',
  Square: '\u25A1',
  Sextile: '\u26B9',
  Quincunx: '\u26BB',
};

const ASPECT_COLORS: Record<string, string> = {
  Conjunction: '#fbbf24',
  Opposition: '#ef4444',
  Trine: '#22c55e',
  Square: '#f97316',
  Sextile: '#3b82f6',
  Quincunx: '#9ca3af',
};

type AspectCategory = 'harmonious' | 'challenging' | 'neutral' | 'other';

const ASPECT_CATEGORY: Record<string, AspectCategory> = {
  Trine: 'harmonious',
  Sextile: 'harmonious',
  Square: 'challenging',
  Opposition: 'challenging',
  Conjunction: 'neutral',
  Quincunx: 'other',
};

const ROW_BG: Record<AspectCategory, string> = {
  harmonious: 'rgba(34, 197, 94, 0.08)',
  challenging: 'rgba(239, 68, 68, 0.08)',
  neutral: 'rgba(251, 191, 36, 0.08)',
  other: 'rgba(156, 163, 175, 0.08)',
};

interface Props {
  aspects: Aspect[];
  title?: string;
  interpretations?: InterpretationEntry[];
}

export const AspectGrid: React.FC<Props> = ({ aspects, title, interpretations }) => {
  const { t, lang } = useLang();

  const findInterpretation = (a: Aspect): string | undefined => {
    if (!interpretations) return undefined;
    const key = `${a.body1}_${a.type}_${a.body2}`;
    const reverseKey = `${a.body2}_${a.type}_${a.body1}`;
    const entry = interpretations.find(e => e.key === key || e.key === reverseKey);
    if (!entry?.text) return undefined;
    // Return first sentence
    const dot = entry.text.indexOf('. ');
    return dot > 0 ? entry.text.substring(0, dot + 1) : entry.text;
  };

  return (
    <TableContainer component={Paper} sx={{
      background: 'rgba(255,255,255,0.05)', backdropFilter: 'blur(20px)',
      border: '1px solid rgba(255,255,255,0.1)', borderRadius: 4,
    }}>
      <Typography variant="h6" sx={{ color: 'white', fontWeight: 700, p: 2, pb: 0 }}>
        {title || t.aspects}
      </Typography>

      {/* Legend */}
      <Box sx={{ display: 'flex', gap: 2, px: 2, py: 1, flexWrap: 'wrap' }}>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
          <Box sx={{ width: 10, height: 10, borderRadius: '50%', bgcolor: '#22c55e' }} />
          <Typography sx={{ color: '#9ca3af', fontSize: '0.75rem' }}>{t.harmonious}</Typography>
        </Box>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
          <Box sx={{ width: 10, height: 10, borderRadius: '50%', bgcolor: '#ef4444' }} />
          <Typography sx={{ color: '#9ca3af', fontSize: '0.75rem' }}>{t.challenging}</Typography>
        </Box>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
          <Box sx={{ width: 10, height: 10, borderRadius: '50%', bgcolor: '#fbbf24' }} />
          <Typography sx={{ color: '#9ca3af', fontSize: '0.75rem' }}>{t.neutral}</Typography>
        </Box>
      </Box>

      <Table size="small">
        <TableHead>
          <TableRow>
            <TableCell sx={{ color: '#a855f7', fontWeight: 700 }}>{t.planet1}</TableCell>
            <TableCell sx={{ color: '#a855f7', fontWeight: 700 }}>{t.aspect}</TableCell>
            <TableCell sx={{ color: '#a855f7', fontWeight: 700 }}>{t.planet2}</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {aspects.map((a, i) => {
            const category = ASPECT_CATEGORY[a.type] || 'other';
            const brief = findInterpretation(a);
            return (
              <>
                <TableRow key={i} sx={{ bgcolor: ROW_BG[category] }}>
                  <TableCell sx={{ color: 'white' }}>{getPlanetName(lang, a.body1)}</TableCell>
                  <TableCell>
                    <Chip
                      label={`${ASPECT_SYMBOLS[a.type] || ''} ${getAspectName(lang, a.type)}`}
                      size="small"
                      sx={{ bgcolor: ASPECT_COLORS[a.type] + '33', color: ASPECT_COLORS[a.type], fontWeight: 600 }}
                    />
                  </TableCell>
                  <TableCell sx={{ color: 'white' }}>{getPlanetName(lang, a.body2)}</TableCell>
                </TableRow>
                {brief && (
                  <TableRow key={`${i}-desc`} sx={{ bgcolor: ROW_BG[category] }}>
                    <TableCell colSpan={3} sx={{ borderBottom: '1px solid rgba(255,255,255,0.05)', py: 0.3, pl: 4 }}>
                      <Typography sx={{ color: 'rgba(255,255,255,0.45)', fontSize: '0.78rem', fontStyle: 'italic' }}>
                        {brief}
                      </Typography>
                    </TableCell>
                  </TableRow>
                )}
              </>
            );
          })}
        </TableBody>
      </Table>
    </TableContainer>
  );
};
