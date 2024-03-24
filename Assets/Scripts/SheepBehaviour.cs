using System.Collections;
using UnityEngine;

public class SheepBehaviour : MonoBehaviour
{
    public enum SHEEP_STATE
    {
        IDLE,
        WALKING,
        SCARED,
        HAPPY,
        DEAD
    }

    public SHEEP_STATE SheepState = SHEEP_STATE.WALKING;

    public GameObject HappyParticles;
    public GameObject FearParticles;

    public Animator SheepAnimator;

    private RandomSfx _randomSfx;

    // Start is called before the first frame update
    private void Start()
    {
        _randomSfx = GetComponent<RandomSfx>();
        SheepState = SHEEP_STATE.IDLE;
        HappyParticles.SetActive(false);
        FearParticles.SetActive(false);
    }

    public void PlayRandomSfx()
    {
        _randomSfx.PlayRandomSfx();
    }

    public void SetHappy()
    {
        if (SheepState == SHEEP_STATE.HAPPY) return;
        SheepState = SHEEP_STATE.HAPPY;
        HappyParticles.SetActive(true);
        FearParticles.SetActive(false);
        PlayRandomSfx();
        StartCoroutine(HappyCoroutine());
    }
    
    private IEnumerator HappyCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(1, 5));
        SetIdle();
    }

    public void SetScared()
    {
        if (SheepState == SHEEP_STATE.SCARED) return;
        SheepState = SHEEP_STATE.SCARED;
        HappyParticles.SetActive(false);
        FearParticles.SetActive(true);
        PlayRandomSfx();
    }

    public void SetIdle()
    {
        if (SheepState == SHEEP_STATE.IDLE) return;
        SheepState = SHEEP_STATE.IDLE;
        HappyParticles.SetActive(false);
        FearParticles.SetActive(false);
        //PlayRandomSfx();
    }

    public void SetWalking()
    {
        if (SheepState == SHEEP_STATE.WALKING) return;
        SheepState = SHEEP_STATE.WALKING;
        HappyParticles.SetActive(false);
        FearParticles.SetActive(false);
        PlayRandomSfx();
    }

    public void SetDead()
    {
        if (SheepState == SHEEP_STATE.DEAD) return;
        SheepState = SHEEP_STATE.DEAD;
        HappyParticles.SetActive(false);
        FearParticles.SetActive(false);
    }
}