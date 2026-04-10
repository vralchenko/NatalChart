import { Grid, Typography, Box } from '@mui/material';
import { PlanetTable } from './PlanetTable';
import { AspectGrid } from './AspectGrid';
import type { TransitResult } from '../types/chart';

interface Props {
  result: TransitResult;
}

export const TransitView: React.FC<Props> = ({ result }) => (
  <Box>
    <Grid container spacing={3}>
      <Grid item xs={12} md={6}>
        <Typography variant="h6" sx={{ color: '#a855f7', fontWeight: 700, mb: 2 }}>
          Natal Planets
        </Typography>
        <PlanetTable planets={result.natalChart.planets} />
      </Grid>
      <Grid item xs={12} md={6}>
        <Typography variant="h6" sx={{ color: '#22c55e', fontWeight: 700, mb: 2 }}>
          Transit Planets
        </Typography>
        <PlanetTable planets={result.transitPlanets} />
      </Grid>
      <Grid item xs={12}>
        <AspectGrid aspects={result.transitAspects} title="Transit Aspects" />
      </Grid>
    </Grid>
  </Box>
);
