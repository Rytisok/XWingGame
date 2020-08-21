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
        localShip.transform.position = rightHandAnchor.position;
        localShip.transform.rotation = rightHandAnchor.rotation;

        if (globalShip != null)
        {
            globalShip.transform.position = rightHandAnchor.position;
            globalShip.transform.rotation = rightHandAnchor.rotation;
        }
    }

    void MoveEditor(Vector3 targetPos)
    {
        localShip.transform.position = editorAnchor.position;
        localShip.transform.rotation = editorAnchor.rotation;

        if (globalShip != null)
        {
            globalShip.transform.position = editorAnchor.position;
            globalShip.transform.rotation = editorAnchor.rotation;
        }
    }

    void ResetRotations()
    {
        localShip.transform.localRotation = Quaternion.Euler(Vector3.zero);
        if (globalShip != null)
            globalShip.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }


}
