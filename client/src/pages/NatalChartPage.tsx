import { Box, Grid, Typography, Alert } from '@mui/material';
import { BirthDataForm } from '../components/BirthDataForm';
import { ChartWheel } from '../components/ChartWheel';
import { PlanetTable } from '../components/PlanetTable';
import { AspectGrid } from '../components/AspectGrid';
import { InterpretationPanel } from '../components/InterpretationPanel';
import { useChart } from '../hooks/useChart';
import type { BirthData } from '../types/chart';

export const NatalChartPage: React.FC = () => {
  const {
    calculateChart, chartResult, chartLoading, chartError,
    interpretChart, interpretations, interpretLoading,
  } = useChart();

  const handleSubmit = (data: BirthData) => {
    calculateChart(data, {
      onSuccess: (result) => {
        interpretChart(result);
      },
    });
  };

  return (
    <Box>
      <Typography variant="h4" gutterBottom sx={{ color: 'white', fontWeight: 800, textAlign: 'center', mb: 4 }}>
        Natal Chart
      </Typography>

      <BirthDataForm onSubmit={handleSubmit} loading={chartLoading} />

      {chartError && (
        <Alert severity="error" sx={{ mb: 3, maxWidth: 900, mx: 'auto' }}>
          Failed to calculate chart. Please check your input and try again.
        </Alert>
      )}

      {chartResult && (
        <Grid container spacing={3} sx={{ maxWidth: 1200, mx: 'auto' }}>
          <Grid item xs={12} md={6}>
            <ChartWheel chart={chartResult} />
          </Grid>
          <Grid item xs={12} md={6}>
            <PlanetTable planets={chartResult.planets} />
          </Grid>
          <Grid item xs={12}>
            <AspectGrid aspects={chartResult.aspects} />
          </Grid>
          {interpretations && (
            <Grid item xs={12}>
              <InterpretationPanel interpretations={interpretations} />
            </Grid>
          )}
        </Grid>
      )}
    </Box>
  );
};
