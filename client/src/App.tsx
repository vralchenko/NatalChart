import { BrowserRouter, Routes, Route, NavLink } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ThemeProvider, createTheme, CssBaseline, AppBar, Toolbar, Typography, Container, Box, Button, ToggleButtonGroup, ToggleButton } from '@mui/material';
import { NatalChartPage } from './pages/NatalChartPage';
import { SynastryPage } from './pages/SynastryPage';
import { TransitsPage } from './pages/TransitsPage';
import { LangProvider, useLang } from './context/LangContext';
import type { Lang } from './i18n';

const darkTheme = createTheme({
  palette: {
    mode: 'dark',
    primary: { main: '#a855f7' },
    secondary: { main: '#6366f1' },
    background: { default: '#0a0520', paper: '#1a1040' },
  },
  typography: {
    fontFamily: '"Inter", "Roboto", "Helvetica", "Arial", sans-serif',
  },
  components: {
    MuiTableCell: {
      styleOverrides: {
        root: {
          borderColor: 'rgba(255,255,255,0.1)',
        },
      },
    },
  },
});

const queryClient = new QueryClient();

function NavButton({ to, children }: { to: string; children: React.ReactNode }) {
  return (
    <NavLink to={to} style={{ textDecoration: 'none' }}>
      {({ isActive }) => (
        <Button sx={{
          textTransform: 'none',
          color: isActive ? '#a855f7' : '#9ca3af',
          fontWeight: isActive ? 700 : 400,
        }}>
          {children}
        </Button>
      )}
    </NavLink>
  );
}

function AppContent() {
  const { lang, setLang, t } = useLang();

  return (
    <BrowserRouter>
      <AppBar position="static" sx={{ background: 'rgba(10,5,32,0.9)', backdropFilter: 'blur(10px)' }}>
        <Toolbar>
          <Typography variant="h6" sx={{ fontWeight: 800, mr: 4, color: '#a855f7' }}>
            NatalChart
          </Typography>
          <Box sx={{ display: 'flex', gap: 2, flexGrow: 1 }}>
            <NavButton to="/">{t.natalChart}</NavButton>
            <NavButton to="/synastry">{t.synastry}</NavButton>
            <NavButton to="/transits">{t.transits}</NavButton>
          </Box>
          <ToggleButtonGroup
            value={lang}
            exclusive
            onChange={(_, v) => v && setLang(v as Lang)}
            size="small"
            sx={{
              '& .MuiToggleButton-root': {
                color: '#9ca3af',
                borderColor: 'rgba(255,255,255,0.15)',
                px: 1.5,
                py: 0.3,
                fontSize: '0.8rem',
                '&.Mui-selected': {
                  color: '#a855f7',
                  bgcolor: 'rgba(168,85,247,0.15)',
                },
              },
            }}
          >
            <ToggleButton value="en">EN</ToggleButton>
            <ToggleButton value="ru">RU</ToggleButton>
          </ToggleButtonGroup>
        </Toolbar>
      </AppBar>

      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Routes>
          <Route path="/" element={<NatalChartPage />} />
          <Route path="/synastry" element={<SynastryPage />} />
          <Route path="/transits" element={<TransitsPage />} />
        </Routes>
      </Container>

      <Box component="footer" sx={{
        mt: 'auto', py: 3,
        borderTop: '1px solid rgba(255,255,255,0.08)',
        textAlign: 'center',
      }}>
        <Typography sx={{ color: 'rgba(255,255,255,0.35)', fontSize: '0.85rem', mb: 0.5 }}>
          &copy; {new Date().getFullYear()} Viktor Ralchenko
        </Typography>
        <Box sx={{ display: 'flex', justifyContent: 'center', gap: 3 }}>
          <Typography component="a" href="mailto:vralchenko@gmail.com" sx={{ color: 'rgba(255,255,255,0.3)', fontSize: '0.8rem', textDecoration: 'none', '&:hover': { color: '#a855f7' } }}>
            vralchenko@gmail.com
          </Typography>
          <Typography component="a" href="https://www.linkedin.com/in/vralchenko" target="_blank" rel="noopener" sx={{ color: 'rgba(255,255,255,0.3)', fontSize: '0.8rem', textDecoration: 'none', '&:hover': { color: '#a855f7' } }}>
            LinkedIn
          </Typography>
          <Typography component="a" href="https://github.com/vralchenko" target="_blank" rel="noopener" sx={{ color: 'rgba(255,255,255,0.3)', fontSize: '0.8rem', textDecoration: 'none', '&:hover': { color: '#a855f7' } }}>
            GitHub
          </Typography>
        </Box>
      </Box>
    </BrowserRouter>
  );
}

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <ThemeProvider theme={darkTheme}>
        <CssBaseline />
        <LangProvider>
          <AppContent />
        </LangProvider>
      </ThemeProvider>
    </QueryClientProvider>
  );
}

export default App;
