using System.Collections;
using UnityEngine;

public sealed class Player : MonoBehaviour
{
    public static Player Instance;
    public static bool IsLoad;

    [SerializeField]
    private float _speed = 3;

    [SerializeField]
    private SpriteRenderer _view;

    [SerializeField]
    private Animator _animator;
    
    [SerializeField]
    private StepsSoundPlayer _stepsSoundPlayer;

    [SerializeField]
    private AudioSource _evilSFX;
    
    private Rigidbody2D _rigidbody;
    private Vector2 _previousPosition;
    private Vector2 _previousInput;
    
    public GameObject Danger;
    public bool IsMove => _animator.GetFloat("Speed") > 0;
    public SpriteRenderer View  => _view;

    private void Awake()
    {
        Instance = this;
        View.enabled = false;
        
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _previousPosition = _rigidbody.position;
    }

    private void OnDisable()
    {
        _animator.SetFloat("Speed", 0);
        _rigidbody.linearVelocity = Vector2.zero;
        _previousInput = Vector2.zero;
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => IsLoad);
        View.enabled = true;
        _stepsSoundPlayer.Init(transform);
    }

    private void Update()
    {
        // if (InputManager.Instance.IsOpenInventoryDown)
        // {
        //     enabled = false;
        //     // _statsWindow.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y);
        //     // _statsWindow.gameObject.SetActive(true);
        //     return;
        // }
        
        var input = new Vector2(InputManager.Instance.Horizontal, InputManager.Instance.Vertical);
        
        if (input.x > 0)
        {
            SetFlex(false);
        }
        else if (input.x < 0)
        {
            SetFlex(true);
        }
        
        _previousInput = input;

        var isRun = InputManager.Instance.IsCancel;
        _animator.SetBool("IsRun", isRun);
        
        if (InputManager.Instance.IsSubmitDown)
        {
            GetNearestUsable()?.Use();
        }

        if (IsMove)
        {
            _stepsSoundPlayer.Upgrade();
            _stepsSoundPlayer.OnIsRunChange(isRun);
        }
    }

    private void FixedUpdate()
    {
        var input = new Vector2(InputManager.Instance.Horizontal, InputManager.Instance.Vertical);
        var speed = InputManager.Instance.IsCancel ? _speed * 2 : _speed;
        _rigidbody.linearVelocity = input * speed;
        var step = _previousPosition - _rigidbody.position;
        var stepDirection = -step.normalized;

        if (step != Vector2.zero)
        {
            _animator.SetFloat("Speed", 1);
            Stats.Instance.Position = transform.position;

#if PLATFORM_ANDROID
            if (Mathf.Abs(input.x) < Mathf.Abs(input.y))
            {
                _animator.SetFloat("Horizontal", 0);
                _animator.SetFloat("Vertical", input.y);
            }
        
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                _animator.SetFloat("Horizontal", input.x);
                _animator.SetFloat("Vertical", 0);
            } 
#endif
            
        }
        else
        {
            _animator.SetFloat("Speed", 0);
        }
        
        _previousPosition = _rigidbody.position;
    }

    public void SetFlex(bool value)
    {
        transform.localScale = transform.localScale.SetX(value ? -1 : 1);
    }

    private Usable GetNearestUsable()
    {
        var collisions = Physics2D.OverlapCircleAll(transform.position, 0.5f);

        Usable closestObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (var collision in collisions)
        {
            if (collision.GetComponent<Usable>() != null)
            {
                float distance = Vector2.Distance(transform.position, collision.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = collision.gameObject.GetComponent<Usable>();
                }
            }
        }

        return closestObject;
    }

    public IEnumerator WaitAttack()
    {
        _view.GetComponent<Animator>().Play("Attack");
        yield return new WaitForSeconds(1);
        _evilSFX.Play();
    }
}
