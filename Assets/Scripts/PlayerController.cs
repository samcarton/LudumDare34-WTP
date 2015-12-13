using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
        
    Vector2 _heading;
    float _energy;
    float _maxEnergy = 1;
    Transform _indicator;
    Transform _fill;
    float _rotateRate;
    float _launchEnergy;
    bool _firing;
    float _indicatorOffset;

    Rigidbody2D _rigidBody;
    
    public float BaseIndicatorOffset = 1.43f;
    // The amount of additional offset that is given by 1.0 unit of energy
    public float UnitEnergyIndicatorOffset = 1.5f;
    public float RotateRate = 90;
    public float BoostedRotateRateMultiplier = 2;
    public float FiringRotateRateMultiplier = 1/3.0f;

    public float RefillRate = 0.4f;
    public float StartingEnergy = 0;
    public float LaunchEnergyDrainRate = 0.6f;
    public float LaunchEnergyToForceMultiplier = 1;
    public float EatFoodMultiplier = 1.2f;

    public bool PlayerEnabled = true;
    public int PlayerNumber;
    const string _launchButtonFormat = "P{0}Launch";
    const string _boostButtonFormat = "P{0}Boost";

    public AudioClip PowerupSound;
    public AudioClip LaunchSound;
    public AudioClip EatFoodSound;
    public AudioClip PlayerHitSound;
    AudioSource _audioSource;

    public void EatFood(int foodValue = 1)
    {
        transform.localScale *= EatFoodMultiplier;
        _rigidBody.mass *= EatFoodMultiplier;
        LaunchEnergyToForceMultiplier *= EatFoodMultiplier;
        _indicatorOffset *= (EatFoodMultiplier * 1f);
        _maxEnergy *= EatFoodMultiplier;
        RefillRate *= EatFoodMultiplier;
        LaunchEnergyDrainRate *= EatFoodMultiplier;
        _audioSource.PlayOneShot(EatFoodSound);
    }

    // Use this for initialization
    void Start () {
        _heading = Vector2.up;
        _indicator = transform.Find("indicator");
        _fill = transform.Find("CircleFill");
        _rotateRate = RotateRate;
        _energy = StartingEnergy;
        _firing = false;
        _indicatorOffset = BaseIndicatorOffset;
        _launchEnergy = 0;
        _rigidBody = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();        
    }
    
    // Update is called once per frame
    void Update () {
        if (PlayerEnabled)
        {
            HandleInput();
            UpdateEnergy();
        }

        UpdateIndicatorRotation();        
        UpdateFill();
    }       

    void HandleInput()
    {
        if(Input.GetButtonDown(LaunchButton))
        {
            _audioSource.clip = PowerupSound;            
            _audioSource.Play();
        }
        if (Input.GetButtonUp(LaunchButton))
        {
            _firing = false;
            // launch
            Launch();
            _launchEnergy = 0;            
            _audioSource.PlayOneShot(LaunchSound);
        }
        if(Input.GetButton(LaunchButton))
        {
            _rotateRate = FiringRotateRateMultiplier * RotateRate;
            _firing = true;
            return;
        }

        if (Input.GetButton(BoostButton))
        {
            _rotateRate = BoostedRotateRateMultiplier * RotateRate;
        }
        else
        {
            _rotateRate = RotateRate;
        }
    }

    void Launch()
    {
        if(_launchEnergy < 0.0001f)
        {
            return;
        }

        var forceToApply = _heading * _launchEnergy * LaunchEnergyToForceMultiplier;

        _rigidBody.AddForce(forceToApply);        
    }

    void UpdateIndicatorRotation()
    {
        var degreesToRotate = _rotateRate * Time.deltaTime;
        _heading = Quaternion.Euler(0, 0, degreesToRotate) * _heading;

        _indicator.position = transform.position + (Vector3)(GetIndicatorOffsetForEnergy() * _heading) + new Vector3(0,0,-1);
        _indicator.up = (Vector3)_heading;
    }

    float GetIndicatorOffsetForEnergy()
    {
        return _indicatorOffset + _launchEnergy * UnitEnergyIndicatorOffset;
    }
    void UpdateEnergy()
    {
        if(_firing)
        {
            float deltaEnergy = LaunchEnergyDrainRate * Time.deltaTime;
            if(_energy < deltaEnergy)
            {
                deltaEnergy = _energy;
            }

            _energy -= deltaEnergy;
            _launchEnergy += deltaEnergy;

            return;
        }

        _energy += RefillRate * Time.deltaTime;
        if (_energy > _maxEnergy)
        {
            _energy = _maxEnergy;
        }
    }

    void UpdateFill()
    {
        var fillRatio = _energy / _maxEnergy;
        _fill.localScale = GetFillScale(fillRatio);
    }

    static Vector3 GetFillScale(float fillRatio)
    {
        return new Vector3(fillRatio, fillRatio, 1);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(PlayerHitSound == null)
        {
            return;
        }

        if (collision.IsWithPlayer())
        {
            _audioSource.PlayOneShot(PlayerHitSound);
        }
    }

    string LaunchButton
    { get { return string.Format(_launchButtonFormat, PlayerNumber); } }

    string BoostButton
    { get { return string.Format(_boostButtonFormat, PlayerNumber); } }    
}
