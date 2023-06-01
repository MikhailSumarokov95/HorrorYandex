using UnityEngine;

public class MonsterSound : MonoBehaviour
{
    [SerializeField] private AudioSource theMonsterSeesThePlayerSound;
    [SerializeField] private AudioSource neckTwistSound;
    private BackRoundMusic _backRoundMusic;
    private Transform _cameraTr;

    private void OnEnable()
    {
        _cameraTr = Camera.main.transform;
        _backRoundMusic = FindObjectOfType<BackRoundMusic>();
        SetPlayingMonsterSeesThePlayerSound(false);
    }

    private void FixedUpdate()
    {
        if (GameManager.IsPause) return;
        SearchPlayer();
    }

    public void PlayNeckTwist()
    {
        theMonsterSeesThePlayerSound.gameObject.SetActive(false);
        var neckTwist = Instantiate(neckTwistSound.gameObject, transform);
        neckTwist.GetComponent<AudioSource>().Play();
        Destroy(neckTwist, neckTwist.GetComponent<AudioSource>().clip.length);
    }

    private void SearchPlayer()
    { 
        RaycastHit hit;
        if (Physics.Raycast(transform.position,
            _cameraTr.position - _cameraTr.up * 0.9f - transform.position, out hit))
        {
            if (hit.collider.CompareTag("Player")) SetPlayingMonsterSeesThePlayerSound(true);
            else SetPlayingMonsterSeesThePlayerSound(false);
        }
        else SetPlayingMonsterSeesThePlayerSound(false);
    }

    private void SetPlayingMonsterSeesThePlayerSound(bool value)
    {
        theMonsterSeesThePlayerSound.gameObject.SetActive(value);
        _backRoundMusic.IsPause = value;
    }
}
