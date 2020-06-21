using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RayBoxPlacer
{
    public const string FLOOR_TAG = "Floor";
    public const string CEIL_TAG = "Ceil";
    public const string WALL_TAG = "Wall";


    public delegate void ToDrawData(BoxCollider box, Vector3 realPosition, Vector3 hitPosition, bool canPlace);

    public event ToDrawData ToDrawListeners;

    public delegate void PutListener();

    public event PutListener PutListeners;


    public BoxCollider Box { get; set; }

    public readonly string OnPlaceTag = "Untagged";

    public readonly LayerMask RayLayer;

    private bool canPlace;

    public RayBoxPlacer(LayerMask rayLayer, string onPlaceTag)
    {
        RayLayer = rayLayer;
        OnPlaceTag = onPlaceTag;
    } 

    public void CastRay(Camera camera)
    {
        if (Box != null)
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, RayLayer))
            {
                int[] pivotIndices;

                var saveForward = Box.transform.forward; //для визуализаций когда объект для пола пытаются натянуть на стену

                if (hit.transform.CompareTag(FLOOR_TAG))
                {
                    pivotIndices = new int[] { BoxVertex.BACK_LEFT_TOP, BoxVertex.BACK_RIGHT_TOP, BoxVertex.FRONT_LEFT_TOP, BoxVertex.FRONT_RIGHT_TOP };
                }
                else if (hit.transform.CompareTag(CEIL_TAG))
                {
                    pivotIndices = new int[] { BoxVertex.BACK_LEFT_BOT, BoxVertex.BACK_RIGHT_BOT, BoxVertex.FRONT_LEFT_BOT, BoxVertex.FRONT_RIGHT_BOT };
                }
                else if(hit.transform.CompareTag(WALL_TAG))
                {
                    if (!hit.transform.CompareTag(Box.tag))
                    {
                        saveForward = Box.transform.forward;
                    }
                    Box.transform.forward = -hit.normal;
                    pivotIndices = new int[] { BoxVertex.BACK_LEFT_BOT, BoxVertex.BACK_RIGHT_BOT, BoxVertex.BACK_LEFT_TOP, BoxVertex.BACK_RIGHT_TOP };
                }
                else
                {
                    return;
                }

                var vertices = BoxHelper.GetBoxWorldVerices(Box);

                var pivot = pivotIndices.Aggregate(Vector3.zero, (total, i) => total += vertices[i]) / pivotIndices.Length;

                var boxCenter = Box.transform.TransformPoint(Box.center * 2.0f);

                var boxPoint = hit.point - boxCenter + pivot;

                var savePosition = Box.transform.position;

                if (hit.transform.CompareTag(Box.tag))
                {
                    Box.transform.position = boxPoint;
                }
                else
                {
                    Box.transform.forward = saveForward;
                    canPlace = false;
                    ToDrawListeners.Invoke(Box, Box.transform.position, boxPoint, canPlace);
                    return;
                }

                if (BoxHelper.CubeCast(Box))
                {
                    canPlace = false;
                    Box.transform.position = savePosition;
                }
                else
                {
                    canPlace = true;
                }

                ToDrawListeners.Invoke(Box, Box.transform.position, boxPoint, canPlace);
            }
        }
    }

    public bool PutBlock()
    {
        if (canPlace)
        {
            Box.isTrigger = false;
            Box.gameObject.tag = OnPlaceTag;
            Box = null;
        }
        return canPlace;
    }
}
