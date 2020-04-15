using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("Component References")]
    public GameObject scoreElementPrefab;
    public GameObject scoreScreenContentParent;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI gameOverScreenText;

    [Header("Properties")]
    public int totalScore;
    public List<ScoreElement> scoreElements;

    [Header("Scoring Properties")]
    public int itemsCollected;
    public int epicItemsCollected;
    public int artifactsCollected;
    public int statesCollected;
    public int encountersCompleted;
    public int levelsGained;
    public int goldGained;
    public int bossesDefeated;
    public int elitesDefeated;
    public int basicsDefeated;
    public int killedBasicNoDamageTaken;
    public int killedEliteNoDamageTaken;
    public int killedBossNoDamageTaken;

    #endregion

    // Singleton Set up
    #region
    public static ScoreManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        // FOR TESTING: REMOVE LATER
        itemsCollected++;
        epicItemsCollected++;
        statesCollected++;
        encountersCompleted = 8;
        levelsGained = 8;
        goldGained = 300;
        bossesDefeated = 2;
        elitesDefeated = 2;
        basicsDefeated = 2;
        killedBasicNoDamageTaken = 2;
        killedEliteNoDamageTaken = 2;
        killedBossNoDamageTaken = 2;
    }
    #endregion

    // Calculate Score
    #region
    public Action CalculateFinalScore()
    {
        Action action = new Action();
        StartCoroutine(CalculateFinalScoreCoroutine(action));
        return action;
    }
    public IEnumerator CalculateFinalScoreCoroutine(Action action)
    {
        // encounters completed
        if (encountersCompleted > 0)
        {
            totalScore += encountersCompleted * 5;
            CreateScoreElement("Encounters Completed", encountersCompleted, encountersCompleted * 5, "Complete an encounter");
        }

        // basic enemys defeated
        if (basicsDefeated > 0)
        {
            totalScore += basicsDefeated * 5;
            CreateScoreElement("Monster Slayer", basicsDefeated, basicsDefeated * 5, "Defeat a basic enemy encounter");
        }

        // elites defeated
        if (elitesDefeated > 0)
        {
            totalScore += elitesDefeated * 20;
            CreateScoreElement("Elite Slayer", elitesDefeated, elitesDefeated * 20, "Defeat an elite enemy encounter");
        }

        // boss defeated
        if (bossesDefeated > 0)
        {
            totalScore += bossesDefeated * 50;
            CreateScoreElement("Boss Slayer", bossesDefeated, bossesDefeated * 50, "Defeat a boss enemy encounter");
        }

        // levels gained
        if (levelsGained > 0)
        {
            totalScore += levelsGained * 5;
            CreateScoreElement("Levels Gained", levelsGained, levelsGained * 5, "Total levels gained by all characters");
        }

        // gold gained
        if (goldGained > 100)
        {
            totalScore += ((goldGained / 100) * 10);
            CreateScoreElement("Wealthy", (goldGained / 100), ((goldGained / 100) * 10), "Gain 100 gold");
        }

        // items collected
        if (itemsCollected > 0)
        {
            totalScore += itemsCollected * 2;
            CreateScoreElement("Item Collector", itemsCollected, itemsCollected * 2, "Collect an encounter");
        }

        // epic items collected
        if (epicItemsCollected > 0)
        {
            totalScore += epicItemsCollected * 5;
            CreateScoreElement("Well Armed", epicItemsCollected, epicItemsCollected * 5, "Collect an epic item");
        }

        // states collected
        if (statesCollected > 0)
        {
            totalScore += statesCollected * 5;
            CreateScoreElement("State Collector", statesCollected, statesCollected * 5, "Gain a state");
        }

        // killed basic, no damage taken
        if (killedBasicNoDamageTaken > 0)
        {
            totalScore += killedBasicNoDamageTaken * 10;
            CreateScoreElement("Rabble Wrecker", killedBasicNoDamageTaken, killedBasicNoDamageTaken * 10, "Defeat a basic enemy encounter without taking damage");
        }

        // killed elite, no damage taken
        if (killedEliteNoDamageTaken > 0)
        {
            totalScore += killedEliteNoDamageTaken * 20;
            CreateScoreElement("Champion", killedEliteNoDamageTaken, killedEliteNoDamageTaken * 20, "Defeat an elite enemy encounter without taking damage");

        }

        // killed basic, no damage taken
        if (killedBossNoDamageTaken > 0)
        {
            totalScore += killedBossNoDamageTaken * 50;
            CreateScoreElement("Unstoppable", killedBossNoDamageTaken, killedBossNoDamageTaken * 50, "Defeat an boss enemy encounter without taking damage");
        }

        // Set final score text
        finalScoreText.text = totalScore.ToString();

        // fade in each score element 1 by 1
        foreach (ScoreElement scoreElement in scoreElements)
        {
            StartCoroutine(scoreElement.FadeInPanel());
            yield return new WaitForSeconds(0.3f);
        }        

        action.actionResolved = true;
    }
    public void CreateScoreElement(string name, int amount, int finalScoreValue, string description)
    {
        GameObject newElement = Instantiate(scoreElementPrefab, scoreScreenContentParent.transform);
        ScoreElement scoreElement = newElement.GetComponent<ScoreElement>();
        scoreElements.Add(scoreElement);
        scoreElement.InitializeSetup(name, amount, finalScoreValue, description);

    }
    #endregion

    // Modify Score Elements
    public void HandlePostCombatScoring()
    {
        if (EventManager.Instance.currentEncounterType == WorldEncounter.EncounterType.BasicEnemy)
        {
            basicsDefeated++;
            if (EventManager.Instance.damageTakenThisEncounter == false)
            {
                killedBasicNoDamageTaken++;
            }
        }
        else if (EventManager.Instance.currentEncounterType == WorldEncounter.EncounterType.EliteEnemy)
        {
            elitesDefeated++;
            if (EventManager.Instance.damageTakenThisEncounter == false)
            {
                killedEliteNoDamageTaken++;
            }
        }
        else if (EventManager.Instance.currentEncounterType == WorldEncounter.EncounterType.Boss)
        {
            bossesDefeated++;
            if (EventManager.Instance.damageTakenThisEncounter == false)
            {
                killedBossNoDamageTaken++;
            }
        }

        encountersCompleted++;

    }
}
