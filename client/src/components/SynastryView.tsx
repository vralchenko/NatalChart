import { Grid, Typography, Box } from '@mui/material';
import { PlanetTable } from './PlanetTable';
import { AspectGrid } from './AspectGrid';
import type { SynastryResult } from '../types/chart';

interface Props {
  result: SynastryResult;
}

export const SynastryView: React.FC<Props> = ({ result }) => (
  <Box>
    <Grid container spacing={3}>
      <Grid item xs={12} md={6}>
        <Typography variant="h6" sx={{ color: '#a855f7', fontWeight: 700, mb: 2 }}>
          Person 1
        </Typography>
        <PlanetTable planets={result.chart1.planets} />
      </Grid>
      <Grid item xs={12} md={6}>
        <Typography variant="h6" sx={{ color: '#6366f1', fontWeight: 700, mb: 2 }}>
          Person 2
        </Typography>
        <PlanetTable planets={result.chart2.planets} />
      </Grid>
      <Grid item xs={12}>
        <AspectGrid aspects={result.interAspects} title="Inter-Aspects (Synastry)" />
      </Grid>
    </Grid>
  </Box>
);
