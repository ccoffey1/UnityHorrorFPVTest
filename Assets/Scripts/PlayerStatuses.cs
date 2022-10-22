using UnityEngine;

public class PlayerStatuses : MonoBehaviour
{
    public float LanternIntensity;
    public Light PlayerAmbientLight;
    public float AmbientLightIntensity;
    public float AmbientLightFadeInSpeed = 0.5f;
    public float AmbientLightFadeOutSpeed = 3f;
    public bool IsInLight;
    public Transform CameraParentPosition;

    private Animator _animator;
    private PlayerLantern _lantern;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _lantern = GetComponent<PlayerLantern>();
    }

    void Update()
    {
        // override player light detection sources if lantern is active
        IsInLight = _lantern.LanternOn || IsInLight;

        CameraFearAnimation();
        PlayerNightVision();
    }

    private void CameraFearAnimation()
    {
        if (IsInLight)
        {
            _animator.SetBool("IsAfraid", false);
        }
        else
        {
            _animator.SetBool("IsAfraid", true);
        }
    }
 
    private void PlayerNightVision()
    {
        if (IsInLight && PlayerAmbientLight.intensity > 0)
        {
            PlayerAmbientLight.intensity = Mathf.Lerp(PlayerAmbientLight.intensity, 0, AmbientLightFadeOutSpeed * Time.deltaTime);
        }
        else if (!IsInLight && PlayerAmbientLight.intensity < AmbientLightIntensity)
        {
            PlayerAmbientLight.intensity = Mathf.Lerp(PlayerAmbientLight.intensity, AmbientLightIntensity, AmbientLightFadeInSpeed * Time.deltaTime);
        }
    }
}
