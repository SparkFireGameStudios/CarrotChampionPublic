using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;

public class PlayerControl : MonoBehaviour
{
    #region Animator Hash

    private static readonly int X = Animator.StringToHash("x");
    private static readonly int IdleDown = Animator.StringToHash("IdleDown");
    private static readonly int IdleUp = Animator.StringToHash("IdleUp");
    private static readonly int IdleLeft = Animator.StringToHash("IdleLeft");
    private static readonly int IdleRight = Animator.StringToHash("IdleRight");

    #endregion

    private Animator _animator;

    public enum PlayerState
    {
        Alive,
        Dead
    }

    public PlayerState playerState = PlayerState.Alive;

    private Vector2 _startPosition;
    private Vector2 _destination;

    private bool _isJumping = false;

    private Rigidbody2D _rb;

    private bool _gameover;

    private bool _canControl = false;

    [SerializeField] private int _jumpScore = 1; // 小跳跃得分
    [SerializeField] private int _currentScore; // 当前得分

    private float _jumpTime = 1f; // 跳跃时间
    private float _currentJumpTime = 0f; // 当前时间

    private BoxCollider2D _boxCollider2D;

    private Vector2 _touchPosition; // 触摸位置

    private bool _canJump = false;

    private enum Direction
    {
        Idle,
        Up,
        Right,
        Left,
    }

    private Direction _dir = Direction.Idle;

    private void OnEnable()
    {
        EventHandler.JumpEvent += OnJumpStart;
    }

    private void OnDisable()
    {
        EventHandler.JumpEvent -= OnJumpStart;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        _gameover = false;
        _currentScore = 0;
        _canJump = false;
        _canControl = true;

        _startPosition = transform.position;
        _destination = transform.position;
    }

    private void FixedUpdate()
    {
        if (_canControl == false)
            return;
        if (_gameover)
            return;
        if (_isJumping)
        {
            _currentJumpTime += Time.fixedDeltaTime;
            _rb.position = Vector2.Lerp(_startPosition, _destination, _currentJumpTime);
            // if (_currentJumpTime >= _jumpTime)
            // {
            //     _isJumping = false;
            //     _currentJumpTime = 0f;
            //     _rb.position = _destination;
            //     _destination = transform.position;
            //     _startPosition = transform.position;
            //     _boxCollider2D.enabled = true;
            //     FinishJumpAnimationEvent();
            // }
        }
    }

    void Update()
    {
        if (_canControl == false)
            return;
        if (_gameover)
            return;
        if (_isJumping)
            return;
        if (_canJump)
        {
            TriggerJump();
            _canJump = false;
        }
    }

    public void LevelComplete()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        // Tree
        if (other.gameObject.CompareTag("Tree"))
        {
            Debug.Log("[LOG] 碰撞到 tree");
            EventHandler.CallGameOverEvent();
            _gameover = true;
            return;
        }

        // Stone
        if (other.gameObject.CompareTag("Stone"))
        {
            Debug.Log("[LOG] 碰撞到 stone");
            EventHandler.CallGameOverEvent();
            _gameover = true;
            return;
        }

        // Fences
        if (other.gameObject.CompareTag("Fences"))
        {
            Debug.Log("[LOG] 碰撞到 fences");
            EventHandler.CallGameOverEvent();
            _gameover = true;
            return;
        }

        // Plants
        if (other.gameObject.CompareTag("Plants"))
        {
            Debug.Log("[LOG] 碰撞到 plants");
            EventHandler.CallGameOverEvent();
            _gameover = true;
            return;
        }

        // Border
        if (other.gameObject.CompareTag("Border"))
        {
            Debug.Log("[LOG] 碰撞到 border");
            EventHandler.CallGameOverEvent();
            _gameover = true;
            return;
        }
    }

    private void OnJumpStart()
    {
        MapManager.Instance.CheckPosition();
    }

    #region Input Actions

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_isJumping)
            {
                Core.LogWarning("当前正在跳跃中，无法再次跳跃");
                return;
            }

            if (!_canControl)
            {
                Core.LogWarning("当前无法控制角色，无法跳跃");
                return;
            }

            Core.Log("执行跳跃动作");
            _canJump = true;
        }
    }

    public void GetTouchPosition(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Core.Log("执行跳跃位置: " + context.ReadValue<Vector2>().ToString());

            // 把点击区域的屏幕坐标转换为世界坐标
            _touchPosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
            // 计算点击区域与主角的插值 并向量化
            var offset = ((Vector3)_touchPosition - transform.position).normalized;

            // 判断点击区域的方向
            if (Mathf.Abs(offset.x) <= 0.2f)
            {
                if (Mathf.Abs(offset.y) <= 0.2f)
                {
                    return;
                }

                _dir = Direction.Up;
            }
            else if (offset.x > 0.2f)
            {
                _dir = Direction.Right;
            }
            else if (offset.x < -0.2f)
            {
                _dir = Direction.Left;
            }
        }
    }

    #endregion

    /// <summary>
    /// 触发执行跳跃动作
    /// </summary>
    public void TriggerJump()
    {
        _canJump = false;
        switch (_dir)
        {
            case Direction.Up:
                _destination = new Vector2(this.transform.position.x, this.transform.position.y + 4);
                _animator.SetTrigger(IdleUp);
                _boxCollider2D.enabled = false;
                EventHandler.CallJumpEvent();
                break;
            case Direction.Right:
                _destination = new Vector2(this.transform.position.x + 4, this.transform.position.y);
                _isJumping = true;
                _animator.SetTrigger(IdleRight);
                _boxCollider2D.enabled = false;
                EventHandler.CallJumpEvent();
                break;
            case Direction.Left:
                _destination = new Vector2(this.transform.position.x - 4, this.transform.position.y);
                _isJumping = true;
                _animator.SetTrigger(IdleLeft);
                _boxCollider2D.enabled = false;
                EventHandler.CallJumpEvent();
                break;
            default:
                Core.LogError("[LOG] 触发跳跃失败");
                break;
        }
    }

    #region Animation Events

    public void JumpAnimationEvent()
    {
        _isJumping = true;
    }

    // FinishJumpAnimationEvent 
    public void FinishJumpAnimationEvent()
    {
        _isJumping = false;
        _currentJumpTime = 0f;
        _rb.position = _destination;
        _destination = transform.position;
        _startPosition = transform.position;
        _boxCollider2D.enabled = true;
        
        _currentScore += _jumpScore;
        EventHandler.CallGetPointEvent(_currentScore);
    }

    #endregion
}