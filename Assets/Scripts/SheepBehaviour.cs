using System.Collections;
using System.Collections.Generic;
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
    void Start()
    {
        _randomSfx = GetComponent<RandomSfx>();
        SetIdle();
    }

    public void PlayRandomSfx()
    {
        _randomSfx.PlayRandomSfx();
    }

    public void SetHappy()
    {
        SheepState = SHEEP_STATE.HAPPY;
        HappyParticles.SetActive(true);
        FearParticles.SetActive(false);
    }

    public void SetScared()
    {
        SheepState = SHEEP_STATE.SCARED;
        HappyParticles.SetActive(false);
        FearParticles.SetActive(true);
    }

    public void SetIdle()
    {
        SheepState = SHEEP_STATE.IDLE;
        HappyParticles.SetActive(false);
        FearParticles.SetActive(false);
    }

    public void SetWalking()
    {
        SheepState = SHEEP_STATE.WALKING;
        HappyParticles.SetActive(false);
        FearParticles.SetActive(false);
    }

    public void SetDead()
    {
        SheepState = SHEEP_STATE.DEAD;
        HappyParticles.SetActive(false);
        FearParticles.SetActive(false);
    }
}
