using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {
    public ParticleSystem Particles;

    public void Spawn(Action callback) {
        StartCoroutine(nameof(SpawnCoroutine), callback);
    }
    protected IEnumerator SpawnCoroutine(Action callback) {
        Particles.Play();
        yield return new WaitForSeconds(0.8f);
        callback();
    }
}
