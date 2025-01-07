using System;
using System.Collections;
using System.Linq;

public static class Penalties
{
    public static IPenalty[] All => new IPenalty[] { WaterContamination, PollinatorPopulationDamage, ChemicalExposure };
    public static PenaltyWaterContamination WaterContamination => new();
    public static PenaltyPollinatorPopulationDamage PollinatorPopulationDamage => new();
    public static PenaltyChemicalExposure ChemicalExposure => new();

    public static IPenalty[] GetAllForCause(PenaltyCause cause) => All.Where(penalty => penalty.Cause == cause).ToArray();
    public static IPenalty[] GetAllForCauses(PenaltyCause[] causes) => All.Where(penalty => causes.Contains(penalty.Cause)).ToArray();
    
    public class PenaltyCause
    {
        public static readonly PenaltyCause Pesticide = new PenaltyCause("Pesticide");

        private readonly string m_cause;

        private PenaltyCause(string cause)
        {
            m_cause = cause;
        }

        public override string ToString() => m_cause;
    }

    public interface IPenalty
    {
        public PenaltyCause Cause { get; }
        public string Description { get; }
        public int Cost { get; }
    }

    public class PenaltyWaterContamination : IPenalty
    {
        public PenaltyCause Cause => PenaltyCause.Pesticide;
        public string Description => "Nearby water sources have been contaminated!";
        public int Cost => 5000;

        internal PenaltyWaterContamination()
        { }
    }

    public class PenaltyPollinatorPopulationDamage : IPenalty
    {
        public PenaltyCause Cause => PenaltyCause.Pesticide;
        public string Description => "Nearby water sources have been contaminated!";
        public int Cost => 3000;

        public PenaltyPollinatorPopulationDamage()
        { }
    }

    public class PenaltyChemicalExposure : IPenalty
    {
        public PenaltyCause Cause => PenaltyCause.Pesticide;
        public string Description => "Nearby wildlife has been exposed to dangerous chemicals!";
        public int Cost => 10000;

        public PenaltyChemicalExposure()
        { }
    }
}