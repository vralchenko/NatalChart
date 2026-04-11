import { useState, useEffect } from 'react';
import { Autocomplete, TextField, CircularProgress } from '@mui/material';
import { astroApi } from '../api/astroApi';
import { useLang } from '../context/LangContext';
import type { GeocodingResult } from '../types/chart';

interface Props {
  onSelect: (result: GeocodingResult | null) => void;
}

export const LocationAutocomplete: React.FC<Props> = ({ onSelect }) => {
  const { t, lang } = useLang();
  const [open, setOpen] = useState(false);
  const [options, setOptions] = useState<GeocodingResult[]>([]);
  const [inputValue, setInputValue] = useState('');
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    let active = true;
    if (inputValue.length < 2) {
      setOptions([]);
      return;
    }

    const timer = setTimeout(async () => {
      setLoading(true);
      try {
        const results = await astroApi.searchLocation(inputValue, lang);
        if (active) setOptions(results);
      } catch {
        if (active) setOptions([]);
      } finally {
        if (active) setLoading(false);
      }
    }, 500);

    return () => {
      active = false;
      clearTimeout(timer);
    };
  }, [inputValue]);

  return (
    <Autocomplete
      open={open}
      onOpen={() => setOpen(true)}
      onClose={() => setOpen(false)}
      options={options}
      loading={loading}
      filterOptions={(x) => x}
      getOptionLabel={(opt) => opt.displayName}
      isOptionEqualToValue={(opt, val) =>
        opt.latitude === val.latitude && opt.longitude === val.longitude
      }
      onChange={(_, value) => onSelect(value)}
      onInputChange={(_, value) => setInputValue(value)}
      renderInput={(params) => (
        <TextField
          id={params.id}
          disabled={params.disabled}
          fullWidth={params.fullWidth}
          size={params.size}
          label={t.placeOfBirth}
          placeholder={t.startTypingCity}
          slotProps={{
            inputLabel: params.slotProps.inputLabel,
            input: {
              ...params.slotProps.input,
              endAdornment: (
                <>
                  {loading && <CircularProgress color="inherit" size={20} />}
                  {params.slotProps.input.endAdornment}
                </>
              ),
            },
            htmlInput: params.slotProps.htmlInput,
          }}
        />
      )}
    />
  );
};
