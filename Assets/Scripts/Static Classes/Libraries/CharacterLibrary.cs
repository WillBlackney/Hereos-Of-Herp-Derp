using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLibrary : Singleton<CharacterLibrary>
{
    [Header("Warrior Data")]
    public Sprite warriorSprite;
    public string warriorClassName;

    [Header("Mage Data")]
    public Sprite mageSprite;
    public string mageClassName;

    [Header("Ranger Data")]
    public Sprite rangerSprite;
    public string rangerClassName;

    [Header("Priest Data")]
    public Sprite priestSprite;
    public string priestClassName;

    [Header("Rogue Data")]
    public Sprite rogueSprite;
    public string rogueClassName;

    [Header("Shaman Data")]
    public Sprite shamanSprite;
    public string shamanClassName;

}
