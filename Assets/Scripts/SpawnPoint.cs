using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {
    public ParticleSystem Particles;
    public AudioSource SpawnSound;

    public void Spawn(Action callback) {
        StartCoroutine(nameof(SpawnCoroutine), callback);
    }
    protected IEnumerator SpawnCoroutine(Action callback) {
        yield return new WaitForSeconds(0.01f);
        SpawnSound.Play();
        Particles.Play();
        yield return new WaitForSeconds(1);
        callback();
    }
}
