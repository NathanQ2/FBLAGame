using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public const float RealTimeScaleToGameTimeScale = 1 / (0.0833333333f / 60.0f); 
    public static float GameTimeScale = 2.0f;

    public PlayerManager PlayerManager;
    public UIManager UIManager;
    
    public float PenaltyChance = 0.0f;
    public int PesticidesUsed = 0;

    public int CurrentDay => Mathf.FloorToInt(CurrentTimeHours / 24);
    public float CurrentTimeHours { get; private set; }

    private int m_previousDay = -1;

    public Dictionary<Penalties.PenaltyCause, bool> EnabledPenalties = new Dictionary<Penalties.PenaltyCause, bool>
    {
        [Penalties.PenaltyCause.Pesticide] = true
    };

    public void Start()
    {
    }

    public void Update()
    {
        CurrentTimeHours += (RealTimeScaleToGameTimeScale * GameTimeScale * Time.deltaTime) / 60 / 60;

        if (m_previousDay != CurrentDay)
        {
            OnNewDay();
        }
        
        // print($"CurrentTimeHours: {CurrentTimeHours} CurrentDay: {CurrentDay}");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        
        m_previousDay = CurrentDay;
    }
    
    /**
     * Called when a new day happens
     */
    private void OnNewDay()
    {
        if (Random.value < PenaltyChance)
            LaunchPenalty();
    }

    private void LaunchPenalty()
    {
        Penalties.IPenalty[] availablePenalties = Penalties.GetAllForCauses(EnabledPenalties
            .Where(pen => pen.Value)
            .Select(pen => pen.Key)
            .ToArray()
        );

        if (availablePenalties.Length == 0)
            return;
        Penalties.IPenalty penalty = availablePenalties[Mathf.RoundToInt(Random.value * (availablePenalties.Length - 1))];
        int cost = Mathf.FloorToInt(penalty.Cost * (PenaltyChance - 1));

        UIManager.InstantiatePenaltyNotification(penalty, cost);

        PlayerManager.RemoveMoney(cost);
    }
}
