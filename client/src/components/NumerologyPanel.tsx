import { Paper, Typography, Grid, Box } from '@mui/material';
import { useLang } from '../context/LangContext';
import type { NumerologyResult } from '../types/chart';

interface Props {
  numerology: NumerologyResult;
}

export const NumerologyPanel: React.FC<Props> = ({ numerology }) => {
  const { t } = useLang();

  return (
    <Paper sx={{
      p: 3,
      background: 'linear-gradient(135deg, rgba(168,85,247,0.12), rgba(99,102,241,0.12))',
      border: '1px solid rgba(168,85,247,0.25)',
      borderRadius: 3,
      backdropFilter: 'blur(20px)',
    }}>
      <Typography variant="h6" gutterBottom sx={{ color: 'white', fontWeight: 700 }}>
        {t.numerologyTitle}
      </Typography>

      <Grid container spacing={3}>
        <Grid size={{ xs: 12, md: 6 }}>
          <Box sx={{
            p: 2.5,
            background: 'rgba(255,255,255,0.05)',
            border: '1px solid rgba(168,85,247,0.2)',
            borderRadius: 2,
            height: '100%',
          }}>
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, mb: 1.5 }}>
              <Box sx={{
                width: 48, height: 48, borderRadius: '50%',
                background: 'linear-gradient(135deg, #a855f7, #7c3aed)',
                display: 'flex', alignItems: 'center', justifyContent: 'center',
                fontSize: '1.4rem', fontWeight: 800, color: 'white',
              }}>
                {numerology.lifePathNumber}
              </Box>
              <Typography sx={{ color: '#a855f7', fontWeight: 700, fontSize: '1rem' }}>
                {t.lifePathNumber}
              </Typography>
            </Box>
            <Typography sx={{ color: '#d1d5db', lineHeight: 1.7, fontSize: '0.9rem' }}>
              {numerology.lifePathDescription}
            </Typography>
          </Box>
        </Grid>

        <Grid size={{ xs: 12, md: 6 }}>
          <Box sx={{
            p: 2.5,
            background: 'rgba(255,255,255,0.05)',
            border: '1px solid rgba(99,102,241,0.2)',
            borderRadius: 2,
            height: '100%',
          }}>
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, mb: 1.5 }}>
              <Box sx={{
                width: 48, height: 48, borderRadius: '50%',
                background: 'linear-gradient(135deg, #6366f1, #4338ca)',
                display: 'flex', alignItems: 'center', justifyContent: 'center',
                fontSize: '1.4rem', fontWeight: 800, color: 'white',
              }}>
                {numerology.birthdayNumber}
              </Box>
              <Typography sx={{ color: '#6366f1', fontWeight: 700, fontSize: '1rem' }}>
                {t.birthdayNumber}
              </Typography>
            </Box>
            <Typography sx={{ color: '#d1d5db', lineHeight: 1.7, fontSize: '0.9rem' }}>
              {numerology.birthdayDescription}
            </Typography>
          </Box>
        </Grid>
      </Grid>
    </Paper>
  );
};
