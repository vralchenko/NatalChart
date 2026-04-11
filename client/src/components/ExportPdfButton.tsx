import { useState } from 'react';
import { Button, CircularProgress } from '@mui/material';
import PictureAsPdfIcon from '@mui/icons-material/PictureAsPdf';
import { astroApi } from '../api/astroApi';
import { useLang } from '../context/LangContext';
import type { BirthData } from '../types/chart';

interface Props {
  birthData: BirthData;
  locationName: string;
}

export const ExportPdfButton: React.FC<Props> = ({ birthData, locationName }) => {
  const { t, lang } = useLang();
  const [loading, setLoading] = useState(false);

  const handleExport = async () => {
    setLoading(true);
    try {
      const blob = await astroApi.exportPdf(birthData, lang, locationName);
      const url = URL.createObjectURL(new Blob([blob], { type: 'application/pdf' }));
      const a = document.createElement('a');
      a.href = url;
      a.download = `natal-chart-${birthData.birthDate}.pdf`;
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
      URL.revokeObjectURL(url);
    } catch (e) {
      console.error('PDF export failed:', e);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Button
      variant="outlined"
      startIcon={loading ? <CircularProgress size={18} sx={{ color: 'white' }} /> : <PictureAsPdfIcon />}
      onClick={handleExport}
      disabled={loading}
      sx={{
        textTransform: 'none',
        borderColor: 'rgba(168,85,247,0.5)',
        color: '#a855f7',
        fontWeight: 600,
        '&:hover': {
          borderColor: '#a855f7',
          bgcolor: 'rgba(168,85,247,0.1)',
        },
      }}
    >
      {loading ? t.exportingPdf : t.downloadPdf}
    </Button>
  );
};
