using UnityEngine;

public class ShipsController : MonoBehaviour
{
    public GameObject localShip;
    public GameObject globalShip;
    public ScoreManager scoreManager;
    public Transform rightHandAnchor;
    public Transform editorAnchor;

    public Fly fly;
    void Awake()
    {
        localShip.GetComponent<Ship>().Initialize(fly, scoreManager);
        Move(fly.transform.position);

        if (Application.isEditor)
        {
            transform.position = editorAnchor.position;
            fly.onChangePos += MoveEditor;
        }
        else
            fly.onChangePos += Move;
    }

    public void AddGlobalShip(GameObject ship)
    {
        globalShip = ship;
        ResetRotations();
    }

    void Move(Vector3 targetPos)
    {
        transform.position = rightHandAnchor.position;
        transform.rotation = rightHandAnchor.rotation;
    }

    void MoveEditor(Vector3 targetPos)
    {
        transform.position = editorAnchor.position; 
        transform.rotation = editorAnchor.rotation;
    }

    void FixedUpdate()
    {
        ResetRotations();
    }
    void ResetRotations()
    {
        localShip.transform.localRotation = Quaternion.Euler(Vector3.zero);
        if (globalShip != null)
            globalShip.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }


}
