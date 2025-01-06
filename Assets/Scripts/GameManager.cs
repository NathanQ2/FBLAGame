using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public const float RealTimeScaleToGameTimeScale = 1 / (1 / 60.0f); // 1 game hour / (1 real min / 60)
    public static float GameTimeScale = 1.0f;
    
    public float penaltyChance = 0.0f;

    public int CurrentDay => Mathf.FloorToInt(CurrentTimeHours / 24);
    public float CurrentTimeHours { get; private set; }

    private int m_previousDay = -1;

    // Update is called once per frame
    void Update()
    {
        CurrentTimeHours += (RealTimeScaleToGameTimeScale * GameTimeScale * Time.deltaTime) / 60 / 60;

        if (m_previousDay != CurrentDay)
        {
            OnNewDay();
        }
        
        print($"CurrentTimeHours: {CurrentTimeHours} CurrentDay: {CurrentDay}");

        m_previousDay = CurrentDay;
    }
    
    /**
     * Called when a new day happens
     */
    private void OnNewDay()
    {
        if (Random.value < penaltyChance)
            LaunchPenalty();
    }

    private void LaunchPenalty()
    {
        print("PENALTY!!");
    }
}
