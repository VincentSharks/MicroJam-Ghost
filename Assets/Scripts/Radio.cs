using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Radio : MonoBehaviour
{
    [SerializeField] private GameObject _audioObj;

    public void ToggleAudioObj()
    {
        StartCoroutine(Toggle());
    }

    private IEnumerator Toggle()
    {
        _audioObj.SetActive(!_audioObj.activeInHierarchy);

        yield return new WaitForSeconds(0.1f);

        _audioObj.SetActive(!_audioObj.activeInHierarchy);
    }
}
