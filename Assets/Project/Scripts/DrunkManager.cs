using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using UnityEngine.PostProcessing;
using Fungus;
using DG.Tweening;

public class DrunkManager : MonoBehaviour {

    public GameObject mainCamera;
    public Flowchart flowchart;
    public MultiPortraitCharacter girl;

    public string drunkVariable = "drunkLvl";
    public float defaultVignetteSmoothness = 0.8f;
    public float defaultVignetteRoundness = 1.0f;
    public float effectLoopDuration = 2.0f;

    private int prevFrameDrunkLvl;

    private BlurOptimized blur;
    private ChromaticAberrationModel chromaticAberration;
    private VignetteModel vignette;

    private Tween blurTween;
    private Tween vignetteTween;

    private void Start() {
        prevFrameDrunkLvl = drunkValToDrunkLvl(flowchart.GetIntegerVariable("drunkLvl"));
        blur = mainCamera.GetComponent<BlurOptimized>();

        var postProcessingBehaviour = mainCamera.GetComponent<PostProcessingBehaviour>();

        chromaticAberration = postProcessingBehaviour.profile.chromaticAberration;
        chromaticAberration.enabled = false;
        SetChromaticAberrationIntensity(0f);

        vignette = postProcessingBehaviour.profile.vignette;
        vignette.enabled = false;
        SetVignetteIntensity(0f);
        var s = vignette.settings;
        s.smoothness = defaultVignetteSmoothness;
        s.roundness = defaultVignetteRoundness;
        vignette.settings = s;

        if (girl != null)
            girl.key = "plump";
    }

    private void Update() {
        int drunkLvl = drunkValToDrunkLvl(flowchart.GetIntegerVariable(drunkVariable));
        if (prevFrameDrunkLvl == drunkLvl)
            return;
        prevFrameDrunkLvl = drunkLvl;

        DrunkLevelSetting s = drunkLevelSettings[drunkLvl];

        if (blurTween != null && blurTween.IsPlaying()) {
            blurTween.Kill();
        }
        blur.enabled = s.blurEnabled;
        blur.blurSize = 0;
        blur.blurIterations = s.blurIterations;
        if (blur.enabled) {
            blurTween = DOTween.To(() => blur.blurSize, x => blur.blurSize = x, s.blurSize, effectLoopDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .Play();
        }

        chromaticAberration.enabled = s.chromaticAberrationEnabled;
        SetChromaticAberrationIntensity(s.chromaticAberrationIntensity);

        if (vignetteTween != null && vignetteTween.IsPlaying()) {
            vignetteTween.Kill();
        }
        vignette.enabled = s.vignetteEnabled;
        SetVignetteIntensity(0f);
        if (vignette.enabled) {
            vignetteTween = DOTween.To(() => vignette.settings.intensity, x => SetVignetteIntensity(x), s.vignetteIntensity, effectLoopDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .Play();
        }
        //switch (drunkVal) {
        //    case 0:

        //        if (girl != null)
        //            girl.key = "plump";
        //        break;
        //    case 1:

        //        if (girl != null)
        //            girl.key = "normal";
        //        break;
        //    case 2:

        //        if (girl != null)
        //            girl.key = "sexy";
        //        break;
        //    case 3:
        //    default:

        //        if (girl != null)
        //            girl.key = "sexy";
        //        break;
        //}
    }

    private void SetChromaticAberrationIntensity(float intensity) {
        var settings = chromaticAberration.settings;
        settings.intensity = intensity;
        chromaticAberration.settings = settings;
    }

    private void SetVignetteIntensity(float intensity) {
        var settings = vignette.settings;
        settings.intensity = intensity;
        vignette.settings = settings;
    }

    private int drunkValToDrunkLvl(int drunkVal) {
        if (drunkVal < drunkLevelSettings[0].minValue) {
            return 0;
        }
        for (int i = 0; i < drunkLevelSettings.Length; i++) {
            DrunkLevelSetting s = drunkLevelSettings[i];
            if (drunkVal >= s.minValue && drunkVal <= s.maxValue) {
                return i;
            }
        }
        return drunkLevelSettings.Length - 1;
    }

    public DrunkLevelSetting[] drunkLevelSettings;

    [System.Serializable]
    public class DrunkLevelSetting {
        public int minValue;
        public int maxValue;

        public bool blurEnabled = false;
        public int blurSize = 0;
        public int blurIterations = 0;

        public bool chromaticAberrationEnabled = false;
        public float chromaticAberrationIntensity = 0;

        public bool vignetteEnabled = false;
        public float vignetteIntensity = 0;
    }
}
