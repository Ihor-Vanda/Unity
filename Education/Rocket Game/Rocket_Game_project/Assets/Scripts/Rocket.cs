using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Rocket : MonoBehaviour {
    private Rigidbody rigidBody;
    private AudioSource audioSource;

    private enum State {
        Playing, Lose, Win
    };

    private State state;

    private bool godMode = false;

    [SerializeField] float rotSpeed = 40f;
    [SerializeField] float lSpeed = 100f;

    [SerializeField] AudioClip flySound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip finishSound;

    [SerializeField] ParticleSystem flyParticle;
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem finishParticle;

    [SerializeField] int energy = 100;
    [SerializeField] int energyApplyPerSecond;
    [SerializeField] int energyFromFuel;

    [SerializeField] Text EnergyText;

    // Start is called before the first frame update
    void Start() {
        state = State.Playing;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        EnergyText.text = Convert.ToString(energy);
    }

    // Update is called once per frame
    void Update() {
        if (state == State.Playing) {
            Launch();
            Rotation();
            if (Debug.isDebugBuild) {
                Cheats();
            }
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (state != State.Playing) return;
        switch (collision.gameObject.tag) {
            case "Friendly":
                Debug.Log("Collision with friendly object");

                break;
            case "Battery":
                Debug.Log("Collision with battery");

                PlusEnergy(energyFromFuel, collision.gameObject);
                break;
            case "Finish":
                Debug.Log("Collision with finish platform");

                Win();
                Invoke(nameof(LoadNextLevel), 2f);

                break;
            default:

                if (!godMode) {
                    Debug.Log("Collision with not friendly object");

                    Lose();
                    Invoke(nameof(LoadFirstLevel), 2f);
                }

                break;
        }
    }

    void PlusEnergy(int energyFromBattery, GameObject battery) {
        battery.GetComponent<SphereCollider>().enabled = false;
        energy += energyFromBattery;
        EnergyText.text = Convert.ToString(energy);

        Destroy(battery);
    }

    /*void LoadScenes(State state) {
        switch (state) {
            case State.Lose:
                SceneManager.LoadScene(0);
                break;
            case State.Win:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
        }
    }*/

    void LoadNextLevel() {
        int index = SceneManager.GetActiveScene().buildIndex;

        if (SceneManager.sceneCountInBuildSettings > index + 1) {
            SceneManager.LoadScene(index + 1);
        } else {
            SceneManager.LoadScene(0);
        }
    }

    void Win() {
        state = State.Win;

        audioSource.Stop();
        audioSource.PlayOneShot(finishSound);

        flyParticle.Stop();
        finishParticle.Play();
    }

    void LoadFirstLevel() {
        SceneManager.LoadScene(0);
    }

    void Lose() {
        state = State.Lose;

        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);

        flyParticle.Stop();
        deathParticle.Play();
    }

    void Cheats() {
        if (Input.GetKeyDown(KeyCode.C)) {
            godMode = !godMode;
            Debug.Log("God mode is " + godMode);
        }
    }

    void Rotation() {
        float rotationSpeed = rotSpeed * Time.deltaTime;

        rigidBody.freezeRotation = true;

        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward * rotationSpeed);

            if (Debug.isDebugBuild) Debug.Log("Pressed 'A'");

        } else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(-Vector3.forward * rotationSpeed);

            if (Debug.isDebugBuild) Debug.Log("Pressed 'D'");
      
        } else {
            rigidBody.freezeRotation = false;
        }
    }

    void Launch() {
        if (Input.GetKey(KeyCode.Space)) {

            if (Debug.isDebugBuild) Debug.Log("Pressed 'Space'");

            if (energy > 0) {

                float launchSpeed = lSpeed * Time.deltaTime;

                rigidBody.AddRelativeForce(Vector3.up * launchSpeed);

                if (!audioSource.isPlaying) audioSource.PlayOneShot(flySound);

                if (!flyParticle.isPlaying) flyParticle.Play();

                energy -= Convert.ToInt32(energyApplyPerSecond * Time.deltaTime);

            } else {
                energy = 0;

                audioSource.Pause();
                flyParticle.Stop();
            }

            EnergyText.text = Convert.ToString(energy);
        } else {
            
            audioSource.Pause();
            flyParticle.Stop();
        }
    }
}
