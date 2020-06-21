using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MainController : MonoBehaviour
{
    private BoxCollider box = null;

    [SerializeField]
    private Vector3 ItemSpawnPoint = new Vector3(12213,12321,12312);

    [SerializeField]
    private Material raysMaterial = null;

    [SerializeField]
    private string onPlaceTag = "Untagged";

    [SerializeField]
    private LayerMask rayLayer = 1;

    [SerializeField]
    private float zoomSensetivity = 0.25f;

    [SerializeField]
    public BoxCollider[] furnitures;

    private RayBoxPlacer rayBoxPlacer;

    private BoxObserver boxObserver;

    private bool placeMode = false;

    private void Awake()
    {
        boxObserver = new BoxObserver();
        rayBoxPlacer = new RayBoxPlacer(rayLayer, onPlaceTag);
        rayBoxPlacer.ToDrawListeners += OnDrawDataRecieved;
    }

    private void OnValidate()
    { 
        rayBoxPlacer = new RayBoxPlacer(rayLayer, onPlaceTag);
        rayBoxPlacer.ToDrawListeners += OnDrawDataRecieved;
    }

    private void SelectNewBox(BoxCollider newBox)
    {
        if(box != null)
        {
            Destroy(box.gameObject);
        }
        box = newBox;
        rayBoxPlacer.Box = box;
        boxObserver.Box = box;
    }

    void Update()
    {
        if (placeMode)
        {
            rayBoxPlacer.CastRay(Camera.main);
        }
        else
        {
            boxObserver.PutBoxFront();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            placeMode = !placeMode;
        }

        if (Input.GetMouseButtonDown(0))
        {
            var put = rayBoxPlacer.PutBlock();
            if (put)
            {
                box = null;
                boxObserver.Box = null;
            }
        }

        if (box != null)
        {
            if (!placeMode)
            {
                if (Input.GetKey(KeyCode.Z))
                {
                    boxObserver.RotateX();
                }
                if (Input.GetKey(KeyCode.X))
                {
                    boxObserver.RotateY();
                }
                if (Input.GetKey(KeyCode.C))
                {
                    boxObserver.RotateZ();
                }
                if (Input.GetKey(KeyCode.C))
                {
                    boxObserver.RotateZ();
                }
                if (Input.GetKey(KeyCode.R))
                {
                    boxObserver.ResetRotation();
                }
                boxObserver.Zoom += Input.mouseScrollDelta.y * zoomSensetivity;
                if (boxObserver.Zoom < 0)
                {
                    boxObserver.Zoom = 0;
                }
            }
        }

        for (int i = 0; i < Mathf.Min(furnitures.Length, KeyCode.Alpha9 - KeyCode.Alpha1); ++i)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectNewBox(Instantiate(furnitures[i], ItemSpawnPoint, Quaternion.identity));
            }
        }
    }

    private bool canPlace = false;
    private Vector3 boxRealAndNotDelta;

    private void OnDrawDataRecieved(BoxCollider box, Vector3 realPosition, Vector3 hitPosition, bool canPlace)
    {
        boxRealAndNotDelta = hitPosition - box.transform.position;
        this.canPlace = canPlace;
    }

    private void OnRenderObject()
    {
        if (box != null && placeMode)
        {
            GL.PushMatrix();

            raysMaterial.SetPass(0);

            GL.Begin(GL.LINES);

            GL.Color(canPlace ? Color.green : Color.red);

            var vertices = BoxHelper.GetBoxWorldVerices(box).Select(x => x += boxRealAndNotDelta).ToArray();

            BoxHelper.GlDrawBox(vertices);

            GL.End();
            GL.PopMatrix();
        }
    }
}
