import { BrowserRouter, Routes, Route, NavLink } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ThemeProvider, createTheme, CssBaseline, AppBar, Toolbar, Typography, Container, Box, Button } from '@mui/material';
import { NatalChartPage } from './pages/NatalChartPage';
import { SynastryPage } from './pages/SynastryPage';
import { TransitsPage } from './pages/TransitsPage';

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

const navLinkStyle = ({ isActive }: { isActive: boolean }) => ({
  color: isActive ? '#a855f7' : '#9ca3af',
  fontWeight: isActive ? 700 : 400,
  textDecoration: 'none',
});

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <ThemeProvider theme={darkTheme}>
        <CssBaseline />
        <BrowserRouter>
          <AppBar position="static" sx={{ background: 'rgba(10,5,32,0.9)', backdropFilter: 'blur(10px)' }}>
            <Toolbar>
              <Typography variant="h6" sx={{ fontWeight: 800, mr: 4, color: '#a855f7' }}>
                NatalChart
              </Typography>
              <Box sx={{ display: 'flex', gap: 2 }}>
                <Button component={NavLink} to="/" style={navLinkStyle} sx={{ textTransform: 'none' }}>
                  Natal Chart
                </Button>
                <Button component={NavLink} to="/synastry" style={navLinkStyle} sx={{ textTransform: 'none' }}>
                  Synastry
                </Button>
                <Button component={NavLink} to="/transits" style={navLinkStyle} sx={{ textTransform: 'none' }}>
                  Transits
                </Button>
              </Box>
            </Toolbar>
          </AppBar>

          <Container maxWidth="lg" sx={{ py: 4 }}>
            <Routes>
              <Route path="/" element={<NatalChartPage />} />
              <Route path="/synastry" element={<SynastryPage />} />
              <Route path="/transits" element={<TransitsPage />} />
            </Routes>
          </Container>
        </BrowserRouter>
      </ThemeProvider>
    </QueryClientProvider>
  );
}

export default App;
