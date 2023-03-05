using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardV2 : MonoBehaviour
{
    [SerializeField] List<Vector3> patrolPos = new List<Vector3>();
    [SerializeField] float distanceCheck = 0.5f;
    [SerializeField] float speed;
    [SerializeField] float detectRadius;
    [SerializeField] float caughtTimer = 1f;
    [SerializeField] private GameObject alert;

#if UNITY_EDITOR
    [SerializeField] bool showDetectRadius = false;
#endif

    private int _currentDest = 0;
    private Rigidbody2D _body;
    private float _moveSpeed;
    private Animator _animator;
    private LevelManager _levelManager;

    private void OnEnable()
    {
        _body = GetComponent<Rigidbody2D>();
        _moveSpeed = speed;
        _animator = GetComponent<Animator>();
        _levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        _levelManager.pauseDay += OnPause;
    }

    private void Update()
    {
        GoToDest();
        SetAnim();
        CheckDetection();
    }

    private void GoToDest()
    {
        float distance = Vector2.Distance(transform.position, patrolPos[_currentDest]);

        if (distance > distanceCheck)
        {
            Vector2 dir = patrolPos[_currentDest] - transform.position;
            dir = dir.normalized;
            _body.velocity = dir * _moveSpeed;
        }
        else
        {
            _body.velocity = Vector2.zero;

            GetNextDest();
        }
    }

    private void GetNextDest()
    {
        int last = patrolPos.Count - 1;

        if(_currentDest == last)
        {
            patrolPos.Reverse();
            _currentDest = 0;
        }
        else
        {
            _currentDest++;
        }
    }

    private void OnPause(bool state)
    {
        _moveSpeed = state ? 0f : speed;
    }

    private void SetAnim()
    {
        bool walking = _body.velocity.magnitude > 0.01f ? true : false;
        _animator.SetBool("IsWalking", walking);

        bool left = _body.velocity.x < 0 ? true : false;
        _animator.SetBool("IsLeft", left);
    }

    private void CheckDetection()
    {
        float xPos = transform.localScale.x > 0 ? 1f : -1f;
        Collider2D[] hits = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + xPos * detectRadius, transform.position.y + detectRadius/2), detectRadius);

        foreach (Collider2D hit in hits)
        {
            if (hit && hit.transform.name == "Player")
            {
                StartCoroutine(Caught(hit.gameObject));
            }
        }
    }

    private IEnumerator Caught(GameObject player)
    {
        PlayerController controller = player.GetComponent<PlayerController>();
        controller.MovementDisabled(true);
        alert.SetActive(true);
        yield return new WaitForSeconds(caughtTimer);
        alert.SetActive(false);
        _levelManager.StartDay();
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        foreach(Vector2 pos in patrolPos)
        {
            Gizmos.DrawSphere(new Vector3(pos.x, pos.y, 0f), 0.3f);
        }

        if (showDetectRadius)
        {
            float xPos = transform.localScale.x > 0 ? 1f : -1f;
            Gizmos.DrawSphere(new Vector2(transform.position.x + xPos * detectRadius, transform.position.y + detectRadius / 2), detectRadius);
        }
    }
#endif
}
