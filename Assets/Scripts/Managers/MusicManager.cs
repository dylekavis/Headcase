using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip[] songClips;

    [SerializeField] float timeBetweenSongs = 15f;

    void Start()
    {
        StartCoroutine(MusicLoop());
    }

    IEnumerator MusicLoop()
    {
        while (true)
        {
            if (!audioSource.isPlaying)
            {
                int randSong = Random.Range(0, songClips.Length);

                for(int i = 0; i < songClips.Length; i++)
                {
                    if (randSong == i)
                    {
                        audioSource.clip = songClips[i];
                        audioSource.Play();
                    }
                }
            }
            else
            {
                yield return new WaitForSeconds(timeBetweenSongs);    
            }
            
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
    }

}
