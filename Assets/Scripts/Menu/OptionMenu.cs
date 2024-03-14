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
    //���� render ����
    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }
    //����ȫ��
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen=isFullScreen;//unity�ж�̬����
    }
}
