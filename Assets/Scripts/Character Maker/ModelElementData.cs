using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ModelElementData 
{
    public string elementName;
    public UniversalCharacterModelElement.BodyPartType bodyPartType;
    public ModelElementData(UniversalCharacterModelElement data)
    {
        elementName = data.gameObject.name;
        bodyPartType = data.bodyPartType;
    }
    public ModelElementData(string _elementName)
    {
        elementName = _elementName;
    }
}
