import { useState } from 'react';
import {
  Card, CardContent, Grid, Button, TextField, MenuItem, CircularProgress, Typography,
} from '@mui/material';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { TimePicker } from '@mui/x-date-pickers/TimePicker';
import { Dayjs } from 'dayjs';
import { LocationAutocomplete } from './LocationAutocomplete';
import { useLang } from '../context/LangContext';
import type { BirthData, GeocodingResult } from '../types/chart';

interface Props {
  onSubmit: (data: BirthData, locationName?: string) => void;
  loading?: boolean;
  title?: string;
}

export const BirthDataForm: React.FC<Props> = ({ onSubmit, loading, title }) => {
  const { t } = useLang();
  const [date, setDate] = useState<Dayjs | null>(null);
  const [time, setTime] = useState<Dayjs | null>(null);
  const [location, setLocation] = useState<GeocodingResult | null>(null);
  const [houseSystem, setHouseSystem] = useState(0);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!date || !location) return;

    onSubmit({
      birthDate: date.format('YYYY-MM-DD'),
      birthTime: time?.isValid() ? time.format('HH:mm') : '12:00',
      latitude: location.latitude,
      longitude: location.longitude,
      houseSystem,
    }, location.displayName);
  };

  const isValid = date?.isValid() && location;

  return (
    <LocalizationProvider dateAdapter={AdapterDayjs}>
      <Card sx={{
        maxWidth: 900, mx: 'auto', mb: 3,
        background: 'rgba(255,255,255,0.05)', backdropFilter: 'blur(20px)',
        border: '1px solid rgba(255,255,255,0.1)',
        boxShadow: '0 15px 35px rgba(0,0,0,0.3)', borderRadius: 4,
      }}>
        <CardContent sx={{ p: { xs: 2, md: 4 } }}>
          <Typography variant="h6" gutterBottom sx={{ color: 'white', fontWeight: 700 }}>
            {title || t.birthData}
          </Typography>
          <form onSubmit={handleSubmit}>
            <Grid container spacing={3}>
              <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                <DatePicker
                  label={t.dateOfBirth}
                  value={date}
                  onChange={setDate}
                  disableFuture
                  format="DD.MM.YYYY"
                  slotProps={{ textField: { fullWidth: true } }}
                />
              </Grid>
              <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                <TimePicker
                  label={t.timeOfBirth}
                  value={time}
                  onChange={setTime}
                  ampm={false}
                  slotProps={{ textField: { fullWidth: true, title: t.timeOptionalHint } }}
                />
              </Grid>
              <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                <LocationAutocomplete onSelect={setLocation} />
              </Grid>
              <Grid size={{ xs: 12, sm: 6, md: 2 }}>
                <TextField
                  select fullWidth
                  label={t.houses}
                  value={houseSystem}
                  onChange={(e) => setHouseSystem(Number(e.target.value))}
                >
                  <MenuItem value={0}>Placidus</MenuItem>
                  <MenuItem value={1}>Koch</MenuItem>
                  <MenuItem value={2}>Equal</MenuItem>
                  <MenuItem value={3}>Whole Sign</MenuItem>
                </TextField>
              </Grid>
              <Grid size={12}>
                <Button
                  fullWidth variant="contained" type="submit"
                  disabled={!isValid || loading} size="large"
                  sx={{
                    py: 1.5, borderRadius: 3, fontSize: '1.1rem', fontWeight: 700,
                    background: 'linear-gradient(45deg, #a855f7 30%, #6366f1 90%)',
                    boxShadow: '0 4px 20px rgba(168,85,247,0.3)',
                    '&:hover': { background: 'linear-gradient(45deg, #9333ea 30%, #4f46e5 90%)' },
                  }}
                >
                  {loading ? <CircularProgress size={24} sx={{ color: 'white' }} /> : t.calculateChart}
                </Button>
              </Grid>
            </Grid>
          </form>
        </CardContent>
      </Card>
    </LocalizationProvider>
  );
};
