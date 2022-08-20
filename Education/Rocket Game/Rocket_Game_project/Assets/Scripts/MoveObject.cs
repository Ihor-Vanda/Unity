using UnityEngine;

[DisallowMultipleComponent]
public class MoveObject : MonoBehaviour {

    private Vector3 startPosition;
    [SerializeField] Vector3 movePosition;
    [SerializeField][Range(0, 1)] float moveProgress;

    [SerializeField] float moveSpeed;

    // Start is called before the first frame update
    void Start() {

        startPosition = transform.position;

    }

    // Update is called once per frame
    void Update() {

        moveProgress = Mathf.PingPong(Time.time * moveSpeed, 1);

        Vector3 offset = (movePosition - startPosition) * moveProgress;
        
        transform.position = startPosition + offset;

    }
}
