import { useEffect, useRef } from 'react';
import { Card, CardContent, Typography, Box } from '@mui/material';
import type { NatalChartResult } from '../types/chart';

// Zodiac sign symbols for display
const SIGN_SYMBOLS: Record<string, string> = {
  Aries: '\u2648', Taurus: '\u2649', Gemini: '\u264A', Cancer: '\u264B',
  Leo: '\u264C', Virgo: '\u264D', Libra: '\u264E', Scorpio: '\u264F',
  Sagittarius: '\u2650', Capricorn: '\u2651', Aquarius: '\u2652', Pisces: '\u2653',
};

const PLANET_SYMBOLS: Record<string, string> = {
  Sun: '\u2609', Moon: '\u263D', Mercury: '\u263F', Venus: '\u2640',
  Mars: '\u2642', Jupiter: '\u2643', Saturn: '\u2644', Uranus: '\u2645',
  Neptune: '\u2646', Pluto: '\u2647', NorthNode: '\u260A', Chiron: '\u26B7',
  Ascendant: 'ASC', Midheaven: 'MC',
};

interface Props {
  chart: NatalChartResult;
}

export const ChartWheel: React.FC<Props> = ({ chart }) => {
  const containerRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (!containerRef.current || !chart.planets.length) return;

    const container = containerRef.current;
    container.innerHTML = '';

    // Create SVG manually for the zodiac wheel
    const size = 500;
    const center = size / 2;
    const outerR = 230;
    const signR = 200;
    const innerR = 170;
    const houseR = 80;

    const svg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
    svg.setAttribute('width', '100%');
    svg.setAttribute('height', '100%');
    svg.setAttribute('viewBox', `0 0 ${size} ${size}`);

    // ASC longitude for rotation (House 1 cusp)
    const ascLon = chart.houses.length > 0 ? chart.houses[0].longitude : 0;

    const toAngle = (lon: number) => {
      // Standard: Aries = 0° at 9 o'clock, counterclockwise
      // Rotated so ASC is at 9 o'clock (left)
      return -(lon - ascLon) * (Math.PI / 180) + Math.PI;
    };

    const polarToXY = (angle: number, r: number) => ({
      x: center + r * Math.cos(angle),
      y: center - r * Math.sin(angle),
    });

    // Background circle
    const bgCircle = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
    bgCircle.setAttribute('cx', String(center));
    bgCircle.setAttribute('cy', String(center));
    bgCircle.setAttribute('r', String(outerR));
    bgCircle.setAttribute('fill', '#0f0a2e');
    bgCircle.setAttribute('stroke', '#6366f1');
    bgCircle.setAttribute('stroke-width', '2');
    svg.appendChild(bgCircle);

    // Sign ring
    const innerCircle = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
    innerCircle.setAttribute('cx', String(center));
    innerCircle.setAttribute('cy', String(center));
    innerCircle.setAttribute('r', String(innerR));
    innerCircle.setAttribute('fill', 'none');
    innerCircle.setAttribute('stroke', '#6366f1');
    innerCircle.setAttribute('stroke-width', '1');
    svg.appendChild(innerCircle);

    // House inner circle
    const houseCircle = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
    houseCircle.setAttribute('cx', String(center));
    houseCircle.setAttribute('cy', String(center));
    houseCircle.setAttribute('r', String(houseR));
    houseCircle.setAttribute('fill', '#0a0520');
    houseCircle.setAttribute('stroke', '#6366f1');
    houseCircle.setAttribute('stroke-width', '1');
    svg.appendChild(houseCircle);

    // Sign dividers + symbols
    const signs = ['Aries','Taurus','Gemini','Cancer','Leo','Virgo','Libra','Scorpio','Sagittarius','Capricorn','Aquarius','Pisces'];
    const signColors = ['#ef4444','#22c55e','#eab308','#3b82f6','#ef4444','#22c55e','#eab308','#3b82f6','#ef4444','#22c55e','#eab308','#3b82f6'];

    for (let i = 0; i < 12; i++) {
      const signStartLon = i * 30;
      const angle = toAngle(signStartLon);
      const p1 = polarToXY(angle, innerR);
      const p2 = polarToXY(angle, outerR);

      const line = document.createElementNS('http://www.w3.org/2000/svg', 'line');
      line.setAttribute('x1', String(p1.x));
      line.setAttribute('y1', String(p1.y));
      line.setAttribute('x2', String(p2.x));
      line.setAttribute('y2', String(p2.y));
      line.setAttribute('stroke', '#4b5563');
      line.setAttribute('stroke-width', '0.5');
      svg.appendChild(line);

      // Sign symbol at midpoint
      const midAngle = toAngle(signStartLon + 15);
      const symPos = polarToXY(midAngle, (innerR + outerR) / 2);
      const text = document.createElementNS('http://www.w3.org/2000/svg', 'text');
      text.setAttribute('x', String(symPos.x));
      text.setAttribute('y', String(symPos.y));
      text.setAttribute('text-anchor', 'middle');
      text.setAttribute('dominant-baseline', 'central');
      text.setAttribute('fill', signColors[i]);
      text.setAttribute('font-size', '16');
      text.textContent = SIGN_SYMBOLS[signs[i]] || signs[i][0];
      svg.appendChild(text);
    }

    // House cusps
    chart.houses.forEach((house, idx) => {
      const angle = toAngle(house.longitude);
      const p1 = polarToXY(angle, houseR);
      const p2 = polarToXY(angle, innerR);

      const isAngle = idx === 0 || idx === 3 || idx === 6 || idx === 9; // ASC, IC, DSC, MC
      const line = document.createElementNS('http://www.w3.org/2000/svg', 'line');
      line.setAttribute('x1', String(p1.x));
      line.setAttribute('y1', String(p1.y));
      line.setAttribute('x2', String(p2.x));
      line.setAttribute('y2', String(p2.y));
      line.setAttribute('stroke', isAngle ? '#a855f7' : '#4b5563');
      line.setAttribute('stroke-width', isAngle ? '2' : '0.5');
      svg.appendChild(line);

      // House number
      const nextHouse = chart.houses[(idx + 1) % 12];
      let midLon = (house.longitude + nextHouse.longitude) / 2;
      if (nextHouse.longitude < house.longitude) midLon = ((house.longitude + nextHouse.longitude + 360) / 2) % 360;
      const numAngle = toAngle(midLon);
      const numPos = polarToXY(numAngle, (houseR + innerR) / 2);
      const numText = document.createElementNS('http://www.w3.org/2000/svg', 'text');
      numText.setAttribute('x', String(numPos.x));
      numText.setAttribute('y', String(numPos.y));
      numText.setAttribute('text-anchor', 'middle');
      numText.setAttribute('dominant-baseline', 'central');
      numText.setAttribute('fill', '#9ca3af');
      numText.setAttribute('font-size', '10');
      numText.textContent = String(house.houseNumber);
      svg.appendChild(numText);
    });

    // Planet positions
    const planetR = (innerR + houseR) / 2 + 20;
    chart.planets.forEach((planet) => {
      const angle = toAngle(planet.longitude);
      const pos = polarToXY(angle, planetR);

      // Tick mark from inner circle
      const tickInner = polarToXY(angle, innerR);
      const tickOuter = polarToXY(angle, innerR - 5);
      const tick = document.createElementNS('http://www.w3.org/2000/svg', 'line');
      tick.setAttribute('x1', String(tickInner.x));
      tick.setAttribute('y1', String(tickInner.y));
      tick.setAttribute('x2', String(tickOuter.x));
      tick.setAttribute('y2', String(tickOuter.y));
      tick.setAttribute('stroke', '#a855f7');
      tick.setAttribute('stroke-width', '1');
      svg.appendChild(tick);

      // Planet symbol
      const text = document.createElementNS('http://www.w3.org/2000/svg', 'text');
      text.setAttribute('x', String(pos.x));
      text.setAttribute('y', String(pos.y));
      text.setAttribute('text-anchor', 'middle');
      text.setAttribute('dominant-baseline', 'central');
      text.setAttribute('fill', planet.isRetrograde ? '#f87171' : '#e2e8f0');
      text.setAttribute('font-size', '13');
      text.textContent = PLANET_SYMBOLS[planet.body] || planet.body[0];
      svg.appendChild(text);
    });

    // Aspect lines (only major aspects between planets inside the inner circle)
    const aspectColors: Record<string, string> = {
      Conjunction: '#fbbf24',
      Opposition: '#ef4444',
      Trine: '#22c55e',
      Square: '#ef4444',
      Sextile: '#3b82f6',
      Quincunx: '#9ca3af',
    };

    chart.aspects.forEach((aspect) => {
      const p1 = chart.planets.find(p => p.body === aspect.body1);
      const p2 = chart.planets.find(p => p.body === aspect.body2);
      if (!p1 || !p2) return;

      const a1 = toAngle(p1.longitude);
      const a2 = toAngle(p2.longitude);
      const pos1 = polarToXY(a1, houseR);
      const pos2 = polarToXY(a2, houseR);

      const line = document.createElementNS('http://www.w3.org/2000/svg', 'line');
      line.setAttribute('x1', String(pos1.x));
      line.setAttribute('y1', String(pos1.y));
      line.setAttribute('x2', String(pos2.x));
      line.setAttribute('y2', String(pos2.y));
      line.setAttribute('stroke', aspectColors[aspect.type] || '#6b7280');
      line.setAttribute('stroke-width', '0.8');
      line.setAttribute('opacity', '0.6');
      if (aspect.type === 'Square' || aspect.type === 'Opposition') {
        line.setAttribute('stroke-dasharray', '4 2');
      }
      svg.appendChild(line);
    });

    container.appendChild(svg);
  }, [chart]);

  return (
    <Card sx={{
      background: 'rgba(255,255,255,0.05)',
      backdropFilter: 'blur(20px)',
      border: '1px solid rgba(255,255,255,0.1)',
      borderRadius: 4,
    }}>
      <CardContent>
        <Typography variant="h6" gutterBottom sx={{ color: 'white', fontWeight: 700 }}>
          Natal Chart
        </Typography>
        <Box
          ref={containerRef}
          sx={{
            width: '100%',
            maxWidth: 500,
            aspectRatio: '1',
            mx: 'auto',
          }}
        />
      </CardContent>
    </Card>
  );
};
