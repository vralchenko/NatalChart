import {
  Accordion, AccordionSummary, AccordionDetails, Typography, Box,
} from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import type { InterpretationResult } from '../types/chart';

interface Props {
  interpretations: InterpretationResult;
}

export const InterpretationPanel: React.FC<Props> = ({ interpretations }) => {
  const sections = [
    { title: 'Planets in Signs', entries: interpretations.planetInSign, color: '#a855f7' },
    { title: 'Planets in Houses', entries: interpretations.planetInHouse, color: '#6366f1' },
    { title: 'Aspects', entries: interpretations.aspects, color: '#22c55e' },
  ];

  return (
    <Box>
      <Typography variant="h6" gutterBottom sx={{ color: 'white', fontWeight: 700 }}>
        Interpretations
      </Typography>
      {sections.map((section) => (
        <Box key={section.title} sx={{ mb: 2 }}>
          <Typography variant="subtitle1" sx={{ color: section.color, fontWeight: 600, mb: 1 }}>
            {section.title}
          </Typography>
          {section.entries.map((entry) => (
            <Accordion
              key={entry.key}
              sx={{
                background: 'rgba(255,255,255,0.05)',
                border: '1px solid rgba(255,255,255,0.1)',
                '&:before': { display: 'none' },
                mb: 1,
                borderRadius: '8px !important',
              }}
            >
              <AccordionSummary expandIcon={<ExpandMoreIcon sx={{ color: 'white' }} />}>
                <Typography sx={{ color: 'white', fontWeight: 600 }}>{entry.title}</Typography>
              </AccordionSummary>
              <AccordionDetails>
                <Typography sx={{ color: '#d1d5db' }}>{entry.text}</Typography>
              </AccordionDetails>
            </Accordion>
          ))}
          {section.entries.length === 0 && (
            <Typography sx={{ color: '#6b7280', fontStyle: 'italic' }}>
              No interpretations available for this section.
            </Typography>
          )}
        </Box>
      ))}
    </Box>
  );
};
