using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [SerializeField] private float distanceToPlayerGameOver = 0.5f;
    [SerializeField] private float factorSpeedIsVisibilityPlayer = 1.5f;
    private float _speedDefault = 1f;
    private NavMeshAgent _monsterNMA;
    private Transform _playerTr;
    private bool _isLocked;
    
    private void Start()
    {
        _monsterNMA = GetComponent<NavMeshAgent>();
        _speedDefault = _monsterNMA.speed;
        _playerTr = Camera.main.gameObject.transform;
    }

    private void FixedUpdate()
    {
        if (_isLocked) return;
        if (GameManager.IsPause) return;
        Move();
        _monsterNMA.speed = IsTargetVisibility() ? _speedDefault * factorSpeedIsVisibilityPlayer : _speedDefault; 
        if (IsMonsterHasCaughtUpWithPlayer()) NeckTwist();
    }

    private bool IsMonsterHasCaughtUpWithPlayer() => Vector3.Distance(transform.position, _playerTr.position) < distanceToPlayerGameOver;

    private void Move()
    {
        _monsterNMA.isStopped = false;
        MoveToPlayer();
    }

    private void MoveToPlayer() => _monsterNMA.destination = _playerTr.position;

    private bool IsTargetVisibility()
    {
        RaycastHit hit;
        Physics.Raycast(_playerTr.position, transform.position - _playerTr.position, out hit);
        if (hit.transform.gameObject.CompareTag("Player")) return true;
        else return false;
    }

    private void NeckTwist()
    {
        _isLocked = true;
        _monsterNMA.isStopped = true;
        transform.LookAt(_playerTr.position);
        FindObjectOfType<Level>().LossLevel();
        GetComponent<MonsterSound>().PlayNeckTwist();
        GetComponent<Animator>().SetTrigger("Stop");
    }
}