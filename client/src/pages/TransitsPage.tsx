import { Box, Typography, Alert } from '@mui/material';
import { BirthDataForm } from '../components/BirthDataForm';
import { TransitView } from '../components/TransitView';
import { useTransits } from '../hooks/useTransits';
import type { BirthData } from '../types/chart';

export const TransitsPage: React.FC = () => {
  const { calculateTransits, transitResult, transitLoading, transitError } = useTransits();

  const handleSubmit = (data: BirthData) => {
    calculateTransits({
      natalData: data,
      transitDate: new Date().toISOString(),
    });
  };

  return (
    <Box>
      <Typography variant="h4" gutterBottom sx={{ color: 'white', fontWeight: 800, textAlign: 'center', mb: 4 }}>
        Current Transits
      </Typography>

      <BirthDataForm onSubmit={handleSubmit} loading={transitLoading} />

      {transitError && (
        <Alert severity="error" sx={{ mb: 3, maxWidth: 900, mx: 'auto' }}>
          Failed to calculate transits.
        </Alert>
      )}

      {transitResult && <TransitView result={transitResult} />}
    </Box>
  );
};
