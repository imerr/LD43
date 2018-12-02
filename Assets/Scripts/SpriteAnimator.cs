using System;
using UnityEngine;

public enum EntityState {
	None,
	Idle,
	Walking,
	Running,
	Dead,
}
public class SpriteAnimator : MonoBehaviour {
	[Serializable]
	public class StateSprites {
		public EntityState State;
		public float FlipTime;
		public Sprite[] Sprites;
		public bool Loop;
		public EntityState Return = EntityState.Idle;
	}

	private SpriteRenderer _sprite;
	public StateSprites[] States;
	private StateSprites _currentState;
	private EntityState _state;
	private float _spriteTime;
	private int _currentSprite;
	public Action<EntityState> OnStateChanged;

	public EntityState State {
		get { return _state; }
		set { ChangeState(value); }
	}

	public bool FlipX {
		get { return _sprite.flipX; }
		set { _sprite.flipX = value; }
	}
	public bool FlipY {
		get { return _sprite.flipY; }
		set { _sprite.flipY = value; }
	}
	
	private void ChangeState(EntityState state) {
		if (state == _state) {
			return;
		}
		OnStateChanged?.Invoke(state);
		var s = GetSprites(state);
		if (s == null) {
			_state = state;
			_currentState = null;
			return;
		}

		_currentState = s;
		_state = state;
		_spriteTime = s.FlipTime;
		_currentSprite = -1;
	}

	public StateSprites GetSprites(EntityState state) {
		StateSprites idle = null;
		foreach (var s in States) {
			if (s.State == state) {
				return s;
			}

			if (s.State == EntityState.Idle) {
				idle = s;
			}
		}

		return idle;
	}

	private void Awake() {
		_sprite = GetComponent<SpriteRenderer>();
	}

	private void Start() {
		ChangeState(EntityState.Idle);
	}

	private void Update() {
		if (_currentState != null) {
			_spriteTime += Time.deltaTime;
			while (_spriteTime >= _currentState.FlipTime) {
				if (Mathf.Approximately(_currentState.FlipTime, 0) || _currentState.FlipTime < 0) {
					_currentState.FlipTime = 0.01f;
				}
				_spriteTime -= _currentState.FlipTime; 
				Flip();
			}
		}
	}

	public void Flip() {
		_currentSprite++;
		if (_currentSprite >= _currentState.Sprites.Length) {
			if (_currentState.Loop) {
				_currentSprite = 0;
			} else {
				ChangeState(_currentState.Return);
				return;
			}
		}

		_sprite.sprite = _currentState.Sprites[_currentSprite];
	}
}
