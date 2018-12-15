using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
	[SerializeField] float rcsThrust = 100f;
	[SerializeField] float mainThrust = 100f;

	Rigidbody rigidBody;
	AudioSource audioSource;

	enum State
	{
		Alive,
		Dying,
		Transcending
	}

	private State state = State.Alive;

	// Use this for initialization
	void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();

		rigidBody.mass = 0.07f;
	}

	// Update is called once per frame
	void Update()
	{
		if (State.Alive == state)
		{
			Thrust();
			Rotate();
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (state != State.Alive) {
			return;
		}

		switch (collision.gameObject.tag)
		{
			case "Friendly":
				break;
			case "Finish":
				state = State.Transcending;
				Invoke("LoadNextLevel", 1f);
				break;
			default:
				state = State.Dying;
				Invoke("LoadFirstLevel", 1f);
				break;
		}
	}

	private void LoadNextLevel()
	{
		SceneManager.LoadScene(1);
	}

	private void LoadFirstLevel()
	{
		SceneManager.LoadScene(0);
	}

	private void Thrust()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			float thrustSpeed = mainThrust * Time.deltaTime;

			rigidBody.AddRelativeForce(Vector3.up * thrustSpeed);
			if (!audioSource.isPlaying)
			{
				audioSource.Play();
			}
		}
		else
		{
			audioSource.Stop();
		}
	}

	private void Rotate()
	{
		rigidBody.freezeRotation = true; // take manual control of rotation


		float rotationSpeed = rcsThrust * Time.deltaTime;

		if (Input.GetKey(KeyCode.A))
		{
			transform.Rotate(Vector3.forward * rotationSpeed);
		}
		else if (Input.GetKey(KeyCode.D))
		{
			transform.Rotate(-Vector3.forward * rotationSpeed);
		}

		rigidBody.freezeRotation = false; // resume physics control of rotation
	}
}
