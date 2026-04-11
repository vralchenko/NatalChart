import { Grid, Typography, Box } from '@mui/material';
import { PlanetTable } from './PlanetTable';
import { AspectGrid } from './AspectGrid';
import { useLang } from '../context/LangContext';
import type { TransitResult } from '../types/chart';

interface Props {
  result: TransitResult;
}

export const TransitView: React.FC<Props> = ({ result }) => {
  const { t } = useLang();

  return (
    <Box>
      <Grid container spacing={3}>
        <Grid size={{ xs: 12, md: 6 }}>
          <Typography variant="h6" sx={{ color: '#a855f7', fontWeight: 700, mb: 2 }}>
            {t.natalPlanets}
          </Typography>
          <PlanetTable planets={result.natalChart.planets} />
        </Grid>
        <Grid size={{ xs: 12, md: 6 }}>
          <Typography variant="h6" sx={{ color: '#22c55e', fontWeight: 700, mb: 2 }}>
            {t.transitPlanetsLabel}
          </Typography>
          <PlanetTable planets={result.transitPlanets} />
        </Grid>
        <Grid size={12}>
          <AspectGrid aspects={result.transitAspects} title={t.transitAspects} />
        </Grid>
      </Grid>
    </Box>
  );
};
