using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterSound : MonoBehaviour
{
    [SerializeField] private AudioSource theMonsterSeesThePlayerSound;
    [SerializeField] private AudioSource neckTwistSound;
    [SerializeField] private float delayChangeMusic = 0.3f; 
    private BackRoundMusic _backRoundMusic;
    private Transform _cameraTr;
    private bool _isLocked;
    private Coroutine _coroutineSearchPlayer;

    private void OnEnable()
    {
        _cameraTr = Camera.main.transform;
        _backRoundMusic = FindObjectOfType<BackRoundMusic>();
        SetPlayingMonsterSeesThePlayerSound(false);
        _coroutineSearchPlayer = StartCoroutine(SearchPlayer());
    }

    private void OnDisable()
    {
        StopCoroutine(_coroutineSearchPlayer);
    }

    private void FixedUpdate()
    {
        if (_isLocked) return;
        if (GameManager.IsPause) return;
        SearchPlayer();
        
    }

    public void PlayNeckTwist()
    {
        _isLocked = true;
        SetPlayingMonsterSeesThePlayerSound(false);
        var neckTwist = Instantiate(neckTwistSound.gameObject, transform);
        neckTwist.GetComponent<AudioSource>().Play();
        Destroy(neckTwist, neckTwist.GetComponent<AudioSource>().clip.length);
        Destroy(this);
    }

    private IEnumerator SearchPlayer()
    {
        while (true)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position,
                _cameraTr.position - _cameraTr.up * 0.5f - transform.position, out hit)) 
            {
                if (hit.collider.CompareTag("Player"))
                {
                    yield return new WaitForSeconds(delayChangeMusic);
                    SetPlayingMonsterSeesThePlayerSound(true);
                }
                else
                {
                    yield return new WaitForSeconds(delayChangeMusic);
                    SetPlayingMonsterSeesThePlayerSound(false);
                }
            }
            else SetPlayingMonsterSeesThePlayerSound(false);
            yield return new WaitForFixedUpdate();
        }
    }

    private void SetPlayingMonsterSeesThePlayerSound(bool value)
    {
        theMonsterSeesThePlayerSound.gameObject.SetActive(value);
        _backRoundMusic.IsPause = value;
    }
}
