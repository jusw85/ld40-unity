using UnityEngine;
using UnityStandardAssets.ImageEffects;
using UnityEngine.PostProcessing;
using Fungus;
using DG.Tweening;

public class DrunkManager : MonoBehaviour {

    public GameObject mainCamera;
    public Flowchart flowchart;

    private int prevFrameDrunkLvl;
    private BlurOptimized blur;
    private Tween blurTween;
    private Tween vignetteTween;

    private ChromaticAberrationModel chromaticAberration;
    private VignetteModel vignette;

    private void Start() {
        prevFrameDrunkLvl = flowchart.GetIntegerVariable("drunkLvl");
        blur = mainCamera.GetComponent<BlurOptimized>();

        var postProcessingBehaviour = mainCamera.GetComponent<PostProcessingBehaviour>();
        chromaticAberration = postProcessingBehaviour.profile.chromaticAberration;
        chromaticAberration.enabled = false;
        SetChromaticAberrationIntensity(0);

        vignette = postProcessingBehaviour.profile.vignette;
        vignette.enabled = false;
        var s = vignette.settings;
        s.intensity = 0f;
        s.smoothness = 0.8f;
        s.roundness = 1.0f;
    }

    private void Update() {
        int drunkLvl = flowchart.GetIntegerVariable("drunkLvl");
        if (prevFrameDrunkLvl == drunkLvl)
            return;
        prevFrameDrunkLvl = drunkLvl;

        switch (drunkLvl) {
            case 0:
                if (blurTween != null && blurTween.IsPlaying()) {
                    blurTween.Kill();
                }
                blur.enabled = false;
                blur.blurSize = 0;
                blur.blurIterations = 0;

                chromaticAberration.enabled = false;
                SetChromaticAberrationIntensity(0);

                if (vignetteTween != null && vignetteTween.IsPlaying()) {
                    vignetteTween.Kill();
                }
                vignette.enabled = false;
                SetVignetteIntensity(0f);
                break;
            case 1:
                if (blurTween != null && blurTween.IsPlaying()) {
                    blurTween.Kill();
                }
                blur.enabled = true;
                blur.blurSize = 0;
                blur.blurIterations = 0;
                blurTween = DOTween.To(() => blur.blurSize, x => blur.blurSize = x, 1, 2).SetLoops(-1, LoopType.Yoyo).Play();

                chromaticAberration.enabled = true;
                SetChromaticAberrationIntensity(0.4f);

                if (vignetteTween != null && vignetteTween.IsPlaying()) {
                    vignetteTween.Kill();
                }
                vignette.enabled = true;
                SetVignetteIntensity(0f);
                vignetteTween = DOTween.To(() => vignette.settings.intensity, x => SetVignetteIntensity(x), 0.1f, 2).SetLoops(-1, LoopType.Yoyo).Play();
                break;
            case 2:
                if (blurTween != null && blurTween.IsPlaying()) {
                    blurTween.Kill();
                }
                blur.enabled = true;
                blur.blurSize = 0;
                blur.blurIterations = 1;
                blurTween = DOTween.To(() => blur.blurSize, x => blur.blurSize = x, 2, 2).SetLoops(-1, LoopType.Yoyo).Play();

                chromaticAberration.enabled = true;
                SetChromaticAberrationIntensity(0.8f);

                if (vignetteTween != null && vignetteTween.IsPlaying()) {
                    vignetteTween.Kill();
                }
                vignette.enabled = true;
                SetVignetteIntensity(0f);
                vignetteTween = DOTween.To(() => vignette.settings.intensity, x => SetVignetteIntensity(x), 0.4f, 2).SetLoops(-1, LoopType.Yoyo).Play();
                break;
            case 3:
            default:
                blur.enabled = true;
                blur.blurSize = 0;
                blur.blurIterations = 2;
                if (blurTween != null && blurTween.IsPlaying()) {
                    blurTween.Kill();
                }
                blurTween = DOTween.To(() => blur.blurSize, x => blur.blurSize = x, 3, 2).SetLoops(-1, LoopType.Yoyo).Play();

                chromaticAberration.enabled = true;
                SetChromaticAberrationIntensity(1);

                if (vignetteTween != null && vignetteTween.IsPlaying()) {
                    vignetteTween.Kill();
                }
                vignette.enabled = true;
                SetVignetteIntensity(0f);
                vignetteTween = DOTween.To(() => vignette.settings.intensity, x => SetVignetteIntensity(x), 0.6f, 2).SetLoops(-1, LoopType.Yoyo).Play();
                break;
        }
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
}
