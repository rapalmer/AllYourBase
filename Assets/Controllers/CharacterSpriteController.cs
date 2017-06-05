﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpriteController : MonoBehaviour {

    Dictionary<Character, GameObject> characterGameObjectMap;

    Dictionary<string, Sprite> characterSprites;

    World world
    {
        get { return WorldController.Instance.world; }
    }

    void Start () {
        LoadSprites();

        // Instantiate our dictionary that tracks which GameObject is rendering which Tile data.
        characterGameObjectMap = new Dictionary<Character, GameObject>();

        // Register our callback so that our GameObject gets updated whenever
        // the tile's type changes.
        world.RegisterCharacterCreated(OnCharacterCreated);

        Character c = world.CreateCharacter(world.GetTileAt(world.Width/2, world.Height/2));
        //c.SetDestination(world.GetTileAt(world.Width / 2 + 5, world.Height / 2));
        
    }

    void LoadSprites()
    {
        characterSprites = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Images/Characters/");

        Debug.Log("LOADED RESOURCE:");
        foreach (Sprite s in sprites)
        {
            Debug.Log(s);
            characterSprites[s.name] = s;
        }
    }

    public void OnCharacterCreated(Character c)
    {
        //Debug.Log("OnCharacterCreated");
        // Create a visual GameObject linked to this data.

        // FIXME: Does not consider multi-tile objects nor rotated objects

        // This creates a new GameObject and adds it to our scene.
        GameObject char_go = new GameObject();

        // Add our tile/GO pair to the dictionary.
        characterGameObjectMap.Add(c, char_go);

        char_go.name = "Character Bill";
        char_go.transform.position = new Vector3(c.X, c.Y, 0);
        char_go.transform.SetParent(this.transform, true);

        SpriteRenderer sr = char_go.AddComponent<SpriteRenderer>();
        sr.sprite = characterSprites["p1_front"];
        sr.sortingLayerName = "Characters";

        c.RegisterOnChangedCallback(OnCharacterChanged);

    }

    void OnCharacterChanged(Character c)
    {
        if (characterGameObjectMap.ContainsKey(c) == false)
        {
            Debug.LogError("OnCharacterChanged -- trying to change visuals for a character not in the map");
            return;
        }
        GameObject char_go = characterGameObjectMap[c];
        //char_go.GetComponent<SpriteRenderer>().sprite =
        char_go.transform.position = new Vector3(c.X, c.Y, 0);
    }

}
