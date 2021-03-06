﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Volcano : MonoBehaviour {
	public Text ScoreText;
	public float WaitBetween = 0.6f;
	public float WaitPerSecond = 0.02f;
	public Transform SpawnPoint;
	public Transform JumpPoint;
	public ParticleSystem Particles;
	private static int _score;
	private float _waitTimer;
	public GameObject GameOverObject;
	public AudioSource SplatAudio;
	public AudioSource TimeSound;

	// Use this for initialization
	void Start () {
		AddScore(0);
	}
	
	// Update is called once per frame
	void Update () {
		if (GameOverObject.activeSelf) {
			if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) {
				_score = 0;
				SceneManager.LoadScene(0);
			}
			return;
		}

		_waitTimer += Time.deltaTime;
		if (Level.TimeLeft > 1) {
			while (_waitTimer > WaitPerSecond) {
				_waitTimer -= WaitPerSecond;
				Level.TimeLeft -= 1;
				AddScore(1);
				TimeSound.Play();
			}
		} else {
			if (Level.KilledVictims.Count > 0) {
				if (_waitTimer > WaitBetween) {
					_waitTimer -= WaitBetween;
					var killedType = Level.KilledVictims[Level.KilledVictims.Count - 1];
					Level.KilledVictims.RemoveAt(Level.KilledVictims.Count - 1);
					var victim = Victim.InstantiateType(killedType, SpawnPoint.position);
					victim.Sacrifice(JumpPoint.position);
				}
			} else if (_waitTimer > 5) {
				if (string.IsNullOrEmpty(Level.NextScene)) {
					GameOverObject.SetActive(true);
				} else {
					SceneManager.LoadScene(Level.NextScene);
				}
			}
		}
	}

	private void MakeParticles() {
		Particles.Play();
		SplatAudio.Play();
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.layer == LayerHelper.LayerVictim) {
			MakeParticles();
			Destroy(other.gameObject);
			AddScore(other.gameObject.GetComponent<Victim>().Score);
		}
	}

	private void AddScore(int score) {
		if (Level.VictimsLeft == 0) {
			score *= 2;
		}
		_score += score;
		ScoreText.text = MathHelper.CountdownSeconds(Level.TimeLeft) + " left, Score: " + _score;
	}
}

