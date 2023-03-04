using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float swingCooldown = 1f;
    [SerializeField] private float swingRadius = 1f;
    [SerializeField] private int damage = 1;

    private Rigidbody2D _body;
    private Animator _animator;
    private float _moveSpeed;
    private bool _swingCooldown;
    private bool _swingDisabled = false;
    private LevelManager _levelManager;

    private void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _moveSpeed = movementSpeed;
        _levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        _levelManager.endDay += EndDay;
        _levelManager.startDay += StartDay;
        _levelManager.pauseDay += MovementDisabled;
    }

    public void OnMove(InputValue value)
    {
        Vector2 movement = value.Get<Vector2>();

        movement = movement.normalized;

        _body.velocity = movement * _moveSpeed;

        bool walking = movement.magnitude > 0 ? true : false;

        _animator.SetBool("IsWalking", walking);

        if(movement.x != 0 && _moveSpeed != 0)
        {
            bool left = movement.x < 0 ? true : false;
            _animator.SetBool("IsLeft", left);
        }

    }

    public void OnPause()
    {
        _levelManager.PauseDay();
    }

    public void EndDay()
    {
        MovementDisabled(true);
    }

    public void StartDay(int day)
    {
        MovementDisabled(false);
    }

    public void OnFire(InputValue value)
    {
        if (!_swingCooldown && !_swingDisabled)
            StartCoroutine(Swing());
    }

    private IEnumerator Swing()
    {
        _body.velocity = Vector2.zero;
        _moveSpeed = 0;
        _swingCooldown = true;
        _animator.SetBool("IsSwinging", true);
        yield return new WaitForSeconds(0.25f);
        _animator.SetBool("IsSwinging", false);
        _moveSpeed = movementSpeed;
        yield return new WaitForSeconds(swingCooldown);
        _swingCooldown = false;
    }

    public void SwingDisabled(bool state)
    {
        _swingDisabled = state;
    }

    public void MovementDisabled(bool state)
    {
        _body.velocity = Vector2.zero;
        _moveSpeed = state ? 0 : movementSpeed;
        _swingCooldown = state;
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = state;
    }

    public void CheckSwing()
    {
        float xPos = transform.localScale.x > 0 ? 1f : -1f;
        Collider2D[] hits = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + xPos, transform.position.y + swingRadius), swingRadius);

        foreach(Collider2D hit in hits)
        {
            if (hit)
            {
                MonoBehaviour[] scripts = hit.gameObject.GetComponents<MonoBehaviour>();
                foreach (MonoBehaviour script in scripts)
                {
                    if (script is IHitable)
                    {
                        IHitable hitable = (IHitable)script;
                        hitable.Hit(damage);
                    }
                }
            }
        }
    }
}
