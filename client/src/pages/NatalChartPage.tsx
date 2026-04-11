import { Box, Grid, Typography, Alert } from '@mui/material';
import { BirthDataForm } from '../components/BirthDataForm';
import { ChartWheel } from '../components/ChartWheel';
import { PlanetTable } from '../components/PlanetTable';
import { AspectGrid } from '../components/AspectGrid';
import { InterpretationPanel } from '../components/InterpretationPanel';
import { useChart } from '../hooks/useChart';
import { useLang } from '../context/LangContext';
import type { BirthData } from '../types/chart';

export const NatalChartPage: React.FC = () => {
  const { t, lang } = useLang();
  const {
    calculateChart, chartResult, chartLoading, chartError,
    interpretChart, interpretations,
  } = useChart();

  const handleSubmit = (data: BirthData) => {
    calculateChart(data, {
      onSuccess: (result) => {
        interpretChart({ chart: result, lang });
      },
    });
  };

  return (
    <Box>
      <Typography variant="h4" gutterBottom sx={{ color: 'white', fontWeight: 800, textAlign: 'center', mb: 4 }}>
        {t.natalChart}
      </Typography>

      <BirthDataForm onSubmit={handleSubmit} loading={chartLoading} />

      {chartError && (
        <Alert severity="error" sx={{ mb: 3, maxWidth: 900, mx: 'auto' }}>
          {t.errorChart}
        </Alert>
      )}

      {chartResult && (
        <Grid container spacing={3} sx={{ maxWidth: 1200, mx: 'auto' }}>
          <Grid size={{ xs: 12, md: 6 }}>
            <ChartWheel chart={chartResult} />
          </Grid>
          <Grid size={{ xs: 12, md: 6 }}>
            <PlanetTable planets={chartResult.planets} />
          </Grid>
          <Grid size={12}>
            <AspectGrid aspects={chartResult.aspects} />
          </Grid>
          {interpretations && (
            <Grid size={12}>
              <InterpretationPanel interpretations={interpretations} chart={chartResult} />
            </Grid>
          )}
        </Grid>
      )}
    </Box>
  );
};
