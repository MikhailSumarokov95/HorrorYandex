using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public bool IsGameOver { get { return _level.IsGameOver; } }

    //public bool SetMove { get; set; }

    //public bool IsMonsterVisible { get; private set; }

    public float VisibilityDistance { get { return visibilityDistance; } set { visibilityDistance = value; } }
    //[SerializeField] private float angleVisibility = 5f;
    [SerializeField] private float visibilityDistance = 10f;
    //[SerializeField] private float distanceSearchPointRandomMove = 4f;
    [SerializeField] private float distanceToPlayerGameOver = 0.5f;
    [SerializeField] private float factorSpeedIsVisibilityPlayer = 1.5f;
    private float _speedDefault = 1f;
    private NavMeshAgent _monsterNMA;
    //private CapsuleCollider _monsterCollider;
    private Level _level;
    private Transform _playerTr;
    
    private void Start()
    {
        //_monsterCollider = GetComponent<CapsuleCollider>();
        _monsterNMA = GetComponent<NavMeshAgent>();
        _speedDefault = _monsterNMA.speed;
        _level = FindObjectOfType<Level>();
        _playerTr = Camera.main.gameObject.transform;
    }

    private void FixedUpdate()
    {
        if (IsGameOver) return;
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

    //private void RandomMove()
    //{
    //    if (_monsterNMA.remainingDistance > 3) return;
    //    var targetX = transform.position.x + Random.Range(-distanceSearchPointRandomMove, distanceSearchPointRandomMove);
    //    var targetZ = transform.position.z + Random.Range(-distanceSearchPointRandomMove, distanceSearchPointRandomMove);
    //    NavMeshHit hit;
    //    NavMesh.SamplePosition(new Vector3(targetX, 0, targetZ), out hit, 3, NavMesh.AllAreas);
    //    _monsterNMA.destination = hit.position;
    //}

    //private void DetermineVisibility()
    //{
    //    var cameraPosition = Camera.main.gameObject.transform.position;
    //    var indent = 0.1f;
    //    var vectorBetweenPlayerAndMonster = transform.position - cameraPosition;
    //    var vectorViewportParallelAxis = (Quaternion.Euler(0, 90, 0) * vectorBetweenPlayerAndMonster).normalized;

    //    var topSideMonster = _monsterCollider.transform.up * _monsterCollider.height + transform.position;
    //    var botSideMonster = transform.position;
    //    var leftSideMonster = - vectorViewportParallelAxis * _monsterCollider.radius + transform.position + _monsterCollider.transform.up * _monsterCollider.height;
    //    var rightSideMonster = vectorViewportParallelAxis * _monsterCollider.radius + transform.position + _monsterCollider.transform.up * _monsterCollider.height;

    //    var topSideMonsterOnViewport = Camera.main.WorldToViewportPoint(topSideMonster);
    //    var botSideMonsterOnViewport = Camera.main.WorldToViewportPoint(botSideMonster);
    //    var leftSideMonsterOnViewport = Camera.main.WorldToViewportPoint(leftSideMonster);
    //    var rightSideMonsterOnViewport = Camera.main.WorldToViewportPoint(rightSideMonster);

    //    var positionMonsterOnViewport = Camera.main.WorldToViewportPoint(transform.position);

    //    if (rightSideMonsterOnViewport.x + indent < 0 || leftSideMonsterOnViewport.x - indent > 1 ||
    //        topSideMonsterOnViewport.y + indent < 0 || botSideMonsterOnViewport.y - indent > 1 ||
    //        positionMonsterOnViewport.z < 0) IsMonsterVisible = false;
    //    else
    //    {
    //        var indentFromSide = 0.95f;
    //        var sidesMonster = new Vector3[4];
    //        sidesMonster[0] = _monsterCollider.transform.up * indentFromSide * _monsterCollider.height + transform.position;
    //        sidesMonster[1] = _monsterCollider.transform.up * (1 - indentFromSide) + transform.position;
    //        sidesMonster[2] = - vectorViewportParallelAxis * indentFromSide * _monsterCollider.radius + transform.position +
    //            _monsterCollider.transform.up * indentFromSide * _monsterCollider.height / 2;
    //        sidesMonster[3] = vectorViewportParallelAxis * indentFromSide * _monsterCollider.radius + transform.position + 
    //            _monsterCollider.transform.up * indentFromSide * _monsterCollider.height / 2;
    //        for (var i = 0; i < sidesMonster.Length; i++)
    //        {
    //            Debug.DrawRay(cameraPosition, sidesMonster[i] - cameraPosition, Color.red);
    //            RaycastHit hit;
    //            Physics.Raycast(cameraPosition, sidesMonster[i] - cameraPosition, out hit);
    //            if (hit.collider.gameObject == gameObject)
    //            {
    //                IsMonsterVisible = true;
    //                return;
    //            }
    //            else IsMonsterVisible = false;
    //        }
    //    }
    //}

    private void NeckTwist()
    {
        transform.LookAt(_playerTr.position);
        FindObjectOfType<Level>().LossLevel();
        GetComponent<MonsterSound>().PlayNeckTwist();
    }
}