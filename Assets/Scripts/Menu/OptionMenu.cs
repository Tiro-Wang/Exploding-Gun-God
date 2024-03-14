using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Slider volumeSlider;
    //
    private void Start()
    {
        _audioSource.volume = volumeSlider.value;
    }
    //设置 render 质量
    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }
    //设置全屏
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen=isFullScreen;//unity中动态设置
    }
}
