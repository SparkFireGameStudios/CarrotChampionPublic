using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private static readonly int ToCenter = Animator.StringToHash("ToCenter");
    private static readonly int ToLeft = Animator.StringToHash("ToLeft");
    private static readonly int ToRight = Animator.StringToHash("ToRight");

    private Vector3 _targetPosition;
    private Vector3 _currentPosition;
    private Vector3 _startPosition;

    [Header("自动获取参数")]
    [SerializeField]
    private int _currentJumpPhase = 0;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField] 
    private bool _isJumping;
    private Transform _playerTransform;
    
    private float _elapsedTime = 0f;
    
    private void Start()
    {
        
        _animator = GetComponentInChildren<Animator>();
        
        _playerTransform = transform.parent;
        _targetPosition = _playerTransform.position;
        _isJumping = false;
        _currentPosition = _playerTransform.position;
        _startPosition = _playerTransform.position;
        
        // 初始化玩家状态
        // _currentJumpPhase = 0;
        // ActionState();
    }

    private void ActionState()
    {
        switch (_currentJumpPhase)
        {
            case 0:
                _animator.SetTrigger(ToCenter);
                break;
            case 1:
                _animator.SetTrigger(ToLeft);
                _targetPosition = new Vector3(-2, _targetPosition.y, 0); // 向左跳2单位
                _isJumping = true;
                break;
            case 2:
                _animator.SetTrigger(ToRight);
                _targetPosition = new Vector3(0, _targetPosition.y, 0); // 向右跳2单位
                _isJumping = true;
                break;
            case 3:
                _animator.SetTrigger(ToRight);
                _targetPosition = new Vector3(2, _targetPosition.y, 0); // 向右跳2单位
                _isJumping = true;
                break;
            case 4:
                _animator.SetTrigger(ToLeft);
                _targetPosition = new Vector3(0, _targetPosition.y, 0); // 向右跳2单位
                _isJumping = true;
                break;
        }
    }
    
    public void UpdatePhase()
    {
        // 更新跳跃阶段
        _currentJumpPhase++;
        if (_currentJumpPhase > 4) // 完成4次跳跃后重置
        {
            _currentJumpPhase = 0;
        }
    }

    public void OnCelebrateEnd()
    {
        UpdatePhase();
        ActionState();
    }

    private void Update()
    {
        if(_isJumping)
        {
            _elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(_elapsedTime); // 总时间1秒
            //float t = _elapsedTime; // 总时间2秒
            
            _currentPosition = Vector2.Lerp(_startPosition, _targetPosition, t);
            _playerTransform.position = _currentPosition;
            if (t >= 1)
            {
                _playerTransform.position = _targetPosition;
                _startPosition = _targetPosition;
                _currentPosition = _targetPosition;
                _isJumping = false;
                _elapsedTime = 0;
                UpdatePhase();
                ActionState();
            }
        }
    }
}