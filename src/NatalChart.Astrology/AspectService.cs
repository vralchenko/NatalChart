using NatalChart.Core.Enums;
using NatalChart.Core.Interfaces;
using NatalChart.Core.Models;
using NatalChart.Astrology.Constants;

namespace NatalChart.Astrology;

public class AspectService : IAspectService
{
    public List<Aspect> CalculateAspects(List<PlanetPosition> planets)
    {
        var aspects = new List<Aspect>();
        var calculable = planets.Where(p =>
            p.Body != CelestialBody.Ascendant && p.Body != CelestialBody.Midheaven).ToList();

        for (int i = 0; i < calculable.Count; i++)
        {
            for (int j = i + 1; j < calculable.Count; j++)
            {
                var aspect = CheckAspect(calculable[i], calculable[j]);
                if (aspect != null)
                    aspects.Add(aspect);
            }
        }

        return aspects;
    }

    public List<Aspect> CalculateInterAspects(List<PlanetPosition> planets1, List<PlanetPosition> planets2)
    {
        var aspects = new List<Aspect>();
        var calc1 = planets1.Where(p =>
            p.Body != CelestialBody.Ascendant && p.Body != CelestialBody.Midheaven).ToList();
        var calc2 = planets2.Where(p =>
            p.Body != CelestialBody.Ascendant && p.Body != CelestialBody.Midheaven).ToList();

        foreach (var p1 in calc1)
        {
            foreach (var p2 in calc2)
            {
                var aspect = CheckAspect(p1, p2);
                if (aspect != null)
                    aspects.Add(aspect);
            }
        }

        return aspects;
    }

    private static Aspect? CheckAspect(PlanetPosition p1, PlanetPosition p2)
    {
        var angle = CalculateAngle(p1.Longitude, p2.Longitude);

        foreach (var (aspectType, exactAngle) in SwissEphConstants.AspectAngles)
        {
            var orb = GetOrb(aspectType, p1.Body, p2.Body);
            var diff = Math.Abs(angle - exactAngle);
            if (diff > 180) diff = 360 - diff;

            if (diff <= orb)
            {
                // Determine if applying: the angle is getting closer to exact
                var isApplying = DetermineApplying(p1, p2, exactAngle);

                return new Aspect
                {
                    Body1 = p1.Body,
                    Body2 = p2.Body,
                    Type = aspectType,
                    Angle = angle,
                    Orb = Math.Round(diff, 2),
                    IsApplying = isApplying
                };
            }
        }

        return null;
    }

    private static double CalculateAngle(double lon1, double lon2)
    {
        var diff = Math.Abs(lon1 - lon2);
        if (diff > 180) diff = 360 - diff;
        return diff;
    }

    private static double GetOrb(AspectType type, CelestialBody body1, CelestialBody body2)
    {
        var orbData = SwissEphConstants.Orbs[type];
        // If either planet is a luminary, use luminary orb
        if (SwissEphConstants.IsLuminary(body1) || SwissEphConstants.IsLuminary(body2))
            return orbData.LuminaryOrb;
        return orbData.OtherOrb;
    }

    private static bool DetermineApplying(PlanetPosition p1, PlanetPosition p2, double aspectAngle)
    {
        // Simplified: if faster planet is moving toward the aspect angle, it's applying
        var fasterPlanet = Math.Abs(p1.Speed) > Math.Abs(p2.Speed) ? p1 : p2;
        var slowerPlanet = fasterPlanet == p1 ? p2 : p1;

        var currentAngle = CalculateAngle(fasterPlanet.Longitude, slowerPlanet.Longitude);
        var diff = Math.Abs(currentAngle - aspectAngle);

        // Project forward slightly
        var futureFastLon = fasterPlanet.Longitude + fasterPlanet.Speed;
        var futureSlowLon = slowerPlanet.Longitude + slowerPlanet.Speed;
        var futureAngle = CalculateAngle(futureFastLon, futureSlowLon);
        var futureDiff = Math.Abs(futureAngle - aspectAngle);

        return futureDiff < diff;
    }
}
