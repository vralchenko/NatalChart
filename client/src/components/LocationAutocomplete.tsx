import { useState, useEffect } from 'react';
import { Autocomplete, TextField, CircularProgress } from '@mui/material';
import { astroApi } from '../api/astroApi';
import type { GeocodingResult } from '../types/chart';

interface Props {
  onSelect: (result: GeocodingResult | null) => void;
}

export const LocationAutocomplete: React.FC<Props> = ({ onSelect }) => {
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
        const results = await astroApi.searchLocation(inputValue);
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
      getOptionLabel={(opt) => opt.displayName}
      isOptionEqualToValue={(opt, val) =>
        opt.latitude === val.latitude && opt.longitude === val.longitude
      }
      onChange={(_, value) => onSelect(value)}
      onInputChange={(_, value) => setInputValue(value)}
      renderInput={(params) => (
        <TextField
          {...params}
          label="Place of Birth"
          placeholder="Start typing a city..."
          InputProps={{
            ...params.InputProps,
            endAdornment: (
              <>
                {loading && <CircularProgress color="inherit" size={20} />}
                {params.InputProps.endAdornment}
              </>
            ),
          }}
        />
      )}
    />
  );
};
