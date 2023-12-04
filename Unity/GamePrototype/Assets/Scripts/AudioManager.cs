using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    private int attackingCreaturesCount = 0;
    public AudioSource audioSource;
    public AudioClip battleClip;
    public AudioClip natureClip;

    public void StartAttack()
    {
        attackingCreaturesCount++;
        if (attackingCreaturesCount == 1)
        {
            PlayBattleMusic();
        }
    }

    public void StopAttack()
    {
        if (attackingCreaturesCount > 0)
        {
            attackingCreaturesCount--;
        }

        if (attackingCreaturesCount == 0)
        {
            PlayNatureMusic();
        }
    }

    private void PlayBattleMusic()
    {
        audioSource.clip = battleClip;
        audioSource.Play();
    }

    private void PlayNatureMusic()
    {
        audioSource.clip = natureClip;
        audioSource.Play();
    }
}
