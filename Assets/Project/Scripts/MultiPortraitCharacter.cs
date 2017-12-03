using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class MultiPortraitCharacter : Character {

    public List<PortraitEntry> portraitMap;

    [System.NonSerialized]
    public string key = "";
    private string prevFrameKey = "";

    private void Update() {
        if (prevFrameKey.Equals(key)) {
            return;
        }
        prevFrameKey = key;

        foreach (PortraitEntry entry in portraitMap) {
            if (entry.key.Equals(key)) {
                portraits = entry.portraits;
                break;
            }
        }
    }

    [System.Serializable]
    public struct PortraitEntry {
        public string key;
        public List<Sprite> portraits;
    }
}

