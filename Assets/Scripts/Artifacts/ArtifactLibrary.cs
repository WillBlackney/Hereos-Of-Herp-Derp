using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactLibrary : Singleton<ArtifactLibrary>
{
    public List<ArtifactDataSO> allArtifacts;
    public ArtifactDataSO stinkyPooArtifact;

    public ArtifactDataSO GetRandomArtifact()
    {
        int randomIndex = Random.Range(0, allArtifacts.Count);
        return allArtifacts[randomIndex];
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
