namespace Util
{
    public static class Conversions
    {
        public static float RealMinutesToGameMinutes(float realMins) => GameManager.RealTimeScaleToGameTimeScale * realMins;
        public static float GameMinutesToRealMinutes(float gameMins) => (1 / GameManager.RealTimeScaleToGameTimeScale) * gameMins;
        public static float RealMinutesToGameHours(float realMins) => MinutesToHours(RealMinutesToGameMinutes(realMins));
        public static float MinutesToHours(float mins) => mins / 60;
    }
}
