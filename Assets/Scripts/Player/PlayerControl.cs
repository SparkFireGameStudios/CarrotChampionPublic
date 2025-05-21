using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    #region Animator Hash
    private static readonly int X = Animator.StringToHash("x");
    private static readonly int IdleDown = Animator.StringToHash("IdleDown");
    private static readonly int IdleUp = Animator.StringToHash("IdleUp");
    private static readonly int IdleLeft = Animator.StringToHash("IdleLeft");
    private static readonly int IdleRight = Animator.StringToHash("IdleRight");
    #endregion

    public float m_MoveSpeed;
    [SerializeField] private Animator _animator;

    public enum PlayerState { Alive, Dead }
    
    public PlayerState playerState = PlayerState.Alive;
    
    float dashCooldown = 0f;
    
    private Vector2 _startPosition;
    private Vector2 _destination;
    
    [SerializeField]
    private bool _isJumping;
    
    private Rigidbody2D _rb;
    
    private bool _gameover;
    
    [SerializeField]
    private bool _canControl = true;
    
    [SerializeField]
    private int jumpScore = 1; // 小跳跃得分
    [SerializeField]
    private int _currentScore; // 当前得分
    
    private float _jumpTime = 1f; // 跳跃时间
    private float _currentJumpTime = 0f; // 当前时间
    
    private BoxCollider2D _boxCollider2D;
    private void OnEnable()
    {
        EventHandler.JumpEvent += OnJumpStart;
    }

    private void OnDisable()
    {
        EventHandler.JumpEvent -= OnJumpStart;
    }

    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        
        _gameover = false;
        _currentScore = 0;
        
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
            transform.position = Vector2.Lerp(_startPosition, _destination, _currentJumpTime);
            if (_currentJumpTime >= _jumpTime)
            {
                _isJumping = false;
                _currentJumpTime = 0f;
                transform.position = _destination;
                _destination = transform.position;
                _startPosition = transform.position;
                _boxCollider2D.enabled = true;
                FinishJumpAnimationEvent();
            }
        }
    }

    void Update () 
    {
        if (_canControl == false)
            return;
        if (_gameover)
            return;
        if (_isJumping)
            return;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _destination = new Vector2(this.transform.position.x,this.transform.position.y+4);
            _isJumping = true;
            _animator.SetTrigger(IdleUp);
            _boxCollider2D.enabled = false;
            
            EventHandler.CallJumpEvent();
            AudioManager.Instance?.PlayJumpFx();
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _destination = new Vector2(this.transform.position.x-4,this.transform.position.y);
            _isJumping = true;
            _animator.SetTrigger(IdleLeft);
            _boxCollider2D.enabled = false;
            
            EventHandler.CallJumpEvent();
            AudioManager.Instance?.PlayJumpFx();
        }
        
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _destination = new Vector2(this.transform.position.x+4,this.transform.position.y);
            _isJumping = true;
            _animator.SetTrigger(IdleRight);
            _boxCollider2D.enabled = false;
            
            EventHandler.CallJumpEvent();
            AudioManager.Instance?.PlayJumpFx();
        }
        // if (Input.GetKey(KeyCode.DownArrow))
        // {
        //     _destination = new Vector2(this.transform.position.x,this.transform.position.y-4);
        //     _isJumping = true;
        //     _animator.SetTrigger(IdleDown);
        //     
        //     EventHandler.CallJumpEvent();
        //     AudioManager.Instance?.PlayJumpFx();
        // }
    }

    public void LevelComplete() {
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
    
    // FinishJumpAnimationEvent 因为角色动画和位移无法匹配不在animation中调用
    private void FinishJumpAnimationEvent()
    {
        _currentScore += jumpScore;
        EventHandler.CallGetPointEvent(_currentScore);
    }

    private void OnJumpStart()
    {
        MapManager.Instance.CheckPosition();
    }
}
