using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class MultiPortraitCharacter : Character {

    public List<PortraitEntry> portraitMap;
    public Stage stage;

    [System.NonSerialized]
    public string key = "";
    private string prevFrameKey = "";

    private void Update() {
        if (!Application.isPlaying || prevFrameKey.Equals(key)) {
            return;
        }
        prevFrameKey = key;

        foreach (PortraitEntry entry in portraitMap) {
            if (entry.key.Equals(key)) {
                portraits = entry.portraits;
                break;
            }
        }

        RefreshStageCharacter();
    }

    public void RefreshStageCharacter() {
        if (stage == null) {
            return;
        }

        if (State.onScreen) {
            stage.ShowPortrait(this, State.portrait.name);
            if (State.dimmed) {
                State.portraitImage.color = stage.DimColor;
            }
        }
    }

    [System.Serializable]
    public struct PortraitEntry {
        public string key;
        public List<Sprite> portraits;
    }
}

