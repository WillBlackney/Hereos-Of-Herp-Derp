using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : Singleton<ScoreManager>
{
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
        if(epicItemsCollected > 0)
        {
            totalScore += epicItemsCollected * 5;
            CreateScoreElement("Well Armed", epicItemsCollected, epicItemsCollected * 5, "Collect an epic item");
        }        

        // artifacts collected
        if(artifactsCollected > 0)
        {
            totalScore += artifactsCollected * 10;
            CreateScoreElement("Artifact Collector", artifactsCollected, artifactsCollected * 10, "Collect an artifact");
        }        

        // states collected
        if(statesCollected > 0)
        {
            totalScore += statesCollected * 5;
            CreateScoreElement("State Collector", statesCollected, statesCollected * 5, "Gain a state");
        }            

        // killed basic, no damage taken
        if(killedBasicNoDamageTaken > 0)
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
        if(killedBossNoDamageTaken > 0)
        {
            totalScore += killedBossNoDamageTaken * 50;
            CreateScoreElement("Unstoppable", killedBossNoDamageTaken, killedBossNoDamageTaken * 50, "Defeat an boss enemy encounter without taking damage");
        }
        
        foreach(ScoreElement scoreElement in scoreElements)
        {
            StartCoroutine(scoreElement.FadeInPanel());
            yield return new WaitForSeconds(0.3f);
        }
    }
    public void CreateScoreElement(string name, int amount, int finalScoreValue, string description)
    {
        GameObject newElement = Instantiate(scoreElementPrefab, scoreScreenContentParent.transform);
        ScoreElement scoreElement = newElement.GetComponent<ScoreElement>();
        scoreElement.InitializeSetup(name, amount, finalScoreValue, description);

    }

}
