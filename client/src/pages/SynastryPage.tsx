import { useState } from 'react';
import { Box, Typography, Alert } from '@mui/material';
import { BirthDataForm } from '../components/BirthDataForm';
import { SynastryView } from '../components/SynastryView';
import { useSynastry } from '../hooks/useSynastry';
import { useLang } from '../context/LangContext';
import type { BirthData } from '../types/chart';

export const SynastryPage: React.FC = () => {
  const { t } = useLang();
  const { calculateSynastry, synastryResult, synastryLoading, synastryError } = useSynastry();
  const [person1, setPerson1] = useState<BirthData | null>(null);

  const handlePerson1 = (data: BirthData) => {
    setPerson1(data);
  };

  const handlePerson2 = (data: BirthData) => {
    if (!person1) return;
    calculateSynastry({ person1, person2: data });
  };

  return (
    <Box>
      <Typography variant="h4" gutterBottom sx={{ color: 'white', fontWeight: 800, textAlign: 'center', mb: 4 }}>
        {t.synastryTitle}
      </Typography>

      <BirthDataForm
        onSubmit={handlePerson1}
        loading={false}
        title={person1 ? t.person1Saved : t.person1}
      />

      {person1 && (
        <BirthDataForm
          onSubmit={handlePerson2}
          loading={synastryLoading}
          title={t.person2}
        />
      )}

      {synastryError && (
        <Alert severity="error" sx={{ mb: 3, maxWidth: 900, mx: 'auto' }}>
          {t.errorSynastry}
        </Alert>
      )}

      {synastryResult && <SynastryView result={synastryResult} />}
    </Box>
  );
};
