using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class Radio : MonoBehaviour
{
    [SerializeField] private GameObject _audioObj;
    [SerializeField] private Image _speakerImg;
    [SerializeField] private List<Sprite> _speakerSprites;
    [SerializeField] private Animation _radioClicked;

    private void Start()
    {
        ToggleAudioObj();
    }

    public void RadioClicked()
    {
        _radioClicked.Play();
    }

    public void ToggleAudioObj()
    {
        StartCoroutine(Toggle());
    }

    public void PlayRadioAfterIntercom(StudioEventEmitter emitter)
    {
        StartCoroutine(PlayRadioWithDelay(emitter));
    }

    private IEnumerator PlayRadioWithDelay(StudioEventEmitter emitter)
    {
        StopRadio();
        _speakerImg.sprite = _speakerSprites[0];

        yield return new WaitForSeconds(46);

        StartRadio();
        _speakerImg.sprite = _speakerSprites[1];
    }

    public void StopRadio()
    {
        _audioObj.SetActive(false);
    }

    public void StartRadio()
    {
        _audioObj.SetActive(true);
    }

    private IEnumerator Toggle()
    {
        _audioObj.SetActive(!_audioObj.activeInHierarchy);

        yield return new WaitForSeconds(0.1f);

        _audioObj.SetActive(!_audioObj.activeInHierarchy);
    }
}
