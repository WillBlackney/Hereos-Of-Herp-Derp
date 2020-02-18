using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactLibrary : MonoBehaviour
{
    [Header("Properties")]
    public List<ArtifactDataSO> allArtifacts;
    public ArtifactDataSO stinkyPooArtifact;

    public static ArtifactLibrary Instance;
    private void Awake()
    {
        Instance = this;
    }

    public ArtifactDataSO GetRandomViableArtifact()
    {
        List<ArtifactDataSO> viableArtifacts = new List<ArtifactDataSO>();
        ArtifactDataSO artifactReturned = null;
        int randomIndex;

        foreach (ArtifactDataSO artifact in allArtifacts)
        {
            if(ArtifactManager.Instance.artifactsObtained.Contains(artifact) == false)
            {
                viableArtifacts.Add(artifact);
            }
        }

        Debug.Log("GetRandomViableArtifact() found " + viableArtifacts.Count.ToString() + " viable artifacts...");

        randomIndex = Random.Range(0, viableArtifacts.Count);

        if (viableArtifacts.Count == 0)
        {
            artifactReturned = stinkyPooArtifact;
        }
        else
        {
            artifactReturned = viableArtifacts[randomIndex];
        }

        return artifactReturned;
    }
}
