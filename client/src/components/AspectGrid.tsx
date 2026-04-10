import {
  Table, TableBody, TableCell, TableContainer, TableHead, TableRow,
  Paper, Chip, Typography,
} from '@mui/material';
import type { Aspect } from '../types/chart';

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

interface Props {
  aspects: Aspect[];
  title?: string;
}

export const AspectGrid: React.FC<Props> = ({ aspects, title = 'Aspects' }) => (
  <TableContainer component={Paper} sx={{
    background: 'rgba(255,255,255,0.05)',
    backdropFilter: 'blur(20px)',
    border: '1px solid rgba(255,255,255,0.1)',
    borderRadius: 4,
  }}>
    <Typography variant="h6" sx={{ color: 'white', fontWeight: 700, p: 2, pb: 0 }}>
      {title}
    </Typography>
    <Table size="small">
      <TableHead>
        <TableRow>
          <TableCell sx={{ color: '#a855f7', fontWeight: 700 }}>Planet 1</TableCell>
          <TableCell sx={{ color: '#a855f7', fontWeight: 700 }}>Aspect</TableCell>
          <TableCell sx={{ color: '#a855f7', fontWeight: 700 }}>Planet 2</TableCell>
          <TableCell sx={{ color: '#a855f7', fontWeight: 700 }}>Orb</TableCell>
          <TableCell sx={{ color: '#a855f7', fontWeight: 700 }}>Status</TableCell>
        </TableRow>
      </TableHead>
      <TableBody>
        {aspects.map((a, i) => (
          <TableRow key={i}>
            <TableCell sx={{ color: 'white' }}>{a.body1}</TableCell>
            <TableCell>
              <Chip
                label={`${ASPECT_SYMBOLS[a.type] || ''} ${a.type}`}
                size="small"
                sx={{ bgcolor: ASPECT_COLORS[a.type] + '33', color: ASPECT_COLORS[a.type], fontWeight: 600 }}
              />
            </TableCell>
            <TableCell sx={{ color: 'white' }}>{a.body2}</TableCell>
            <TableCell sx={{ color: 'white' }}>{a.orb.toFixed(2)}\u00B0</TableCell>
            <TableCell>
              <Chip
                label={a.isApplying ? 'Applying' : 'Separating'}
                size="small"
                color={a.isApplying ? 'success' : 'default'}
                variant="outlined"
              />
            </TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  </TableContainer>
);
