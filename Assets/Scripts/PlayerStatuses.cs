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

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (IsInLight)
        {
            _animator.SetBool("IsAfraid", false);
        }
        else
        {
            _animator.SetBool("IsAfraid", true);
        }

        //if (!IsInLight && !DarknessFearAnimator.enabled)
        //{
        //    DarknessFearAnimator.enabled = true;
        //}
        //else if (IsInLight && DarknessFearAnimator.enabled)
        //{
        //    DarknessFearAnimator.enabled = false;
        //}

        if (IsInLight && PlayerAmbientLight.intensity > 0)
        {
            print("Player is in light");
            PlayerAmbientLight.intensity = Mathf.Lerp(PlayerAmbientLight.intensity, 0, AmbientLightFadeOutSpeed * Time.deltaTime);
        }
        else if (!IsInLight && PlayerAmbientLight.intensity < AmbientLightIntensity)
        {
            print("Player is in darkness");
            PlayerAmbientLight.intensity = Mathf.Lerp(PlayerAmbientLight.intensity, AmbientLightIntensity, AmbientLightFadeInSpeed * Time.deltaTime);
        }
    }
}
