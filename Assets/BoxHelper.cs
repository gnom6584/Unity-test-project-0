using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public static class BoxVertex
{
    public const int BACK_LEFT_BOT = 0;
    public const int BACK_RIGHT_BOT = 1;
    public const int BACK_LEFT_TOP = 2;
    public const int BACK_RIGHT_TOP = 3;
    public const int FRONT_LEFT_BOT = 4;
    public const int FRONT_RIGHT_BOT = 5;
    public const int FRONT_LEFT_TOP = 6;
    public const int FRONT_RIGHT_TOP = 7;
};

public static class BoxHelper
{
 

    public static Vector3[] GetBoxWorldVerices(BoxCollider box)
    {
        var transform = box.transform;
        var min = box.center - box.size * 0.5f;
        var max = box.center + box.size * 0.5f;

        var vertices = new Vector3[8];

        vertices[BoxVertex.BACK_LEFT_BOT] = transform.TransformPoint(new Vector3(min.x, min.y, min.z));
        vertices[BoxVertex.FRONT_LEFT_BOT] = transform.TransformPoint(new Vector3(min.x, min.y, max.z));
        vertices[BoxVertex.BACK_LEFT_TOP] = transform.TransformPoint(new Vector3(min.x, max.y, min.z));
        vertices[BoxVertex.FRONT_LEFT_TOP] = transform.TransformPoint(new Vector3(min.x, max.y, max.z));
        vertices[BoxVertex.BACK_RIGHT_BOT] = transform.TransformPoint(new Vector3(max.x, min.y, min.z));
        vertices[BoxVertex.FRONT_RIGHT_BOT] = transform.TransformPoint(new Vector3(max.x, min.y, max.z));
        vertices[BoxVertex.BACK_RIGHT_TOP] = transform.TransformPoint(new Vector3(max.x, max.y, min.z));
        vertices[BoxVertex.FRONT_RIGHT_TOP] = transform.TransformPoint(new Vector3(max.x, max.y, max.z));

        return vertices;
    }

    public static bool CubeCast(BoxCollider box)
    {
        var vertices = GetBoxWorldVerices(box).Select(x => x += (box.transform.TransformPoint(box.center) - x).normalized * box.contactOffset).ToArray();


        if (Physics.Linecast(vertices[BoxVertex.BACK_LEFT_BOT], vertices[BoxVertex.BACK_RIGHT_BOT]))
        {
            return true;
        }
        else if (Physics.Linecast(vertices[BoxVertex.FRONT_LEFT_BOT], vertices[BoxVertex.FRONT_RIGHT_BOT]))
        {
            return true;
        }
        else if (Physics.Linecast(vertices[BoxVertex.BACK_LEFT_TOP], vertices[BoxVertex.BACK_RIGHT_TOP]))
        {
            return true;
        }
        else if (Physics.Linecast(vertices[BoxVertex.FRONT_LEFT_TOP], vertices[BoxVertex.FRONT_RIGHT_TOP]))
        {
            return true;
        }
        else if (Physics.Linecast(vertices[BoxVertex.FRONT_LEFT_BOT], vertices[BoxVertex.BACK_LEFT_BOT]))
        {
            return true;
        }
        else if (Physics.Linecast(vertices[BoxVertex.FRONT_LEFT_TOP], vertices[BoxVertex.FRONT_RIGHT_TOP]))
        {
            return true;
        }
        else if (Physics.Linecast(vertices[BoxVertex.BACK_LEFT_BOT], vertices[BoxVertex.BACK_LEFT_TOP]))
        {
            return true;
        }
        else if (Physics.Linecast(vertices[BoxVertex.FRONT_LEFT_BOT], vertices[BoxVertex.FRONT_LEFT_TOP]))
        {
            return true;
        }
        else if (Physics.Linecast(vertices[BoxVertex.FRONT_RIGHT_BOT], vertices[BoxVertex.BACK_RIGHT_BOT]))
        {
            return true;
        }
        else if (Physics.Linecast(vertices[BoxVertex.FRONT_RIGHT_TOP], vertices[BoxVertex.BACK_RIGHT_TOP]))
        {
            return true;
        }
        else if (Physics.Linecast(vertices[BoxVertex.BACK_RIGHT_BOT], vertices[BoxVertex.BACK_RIGHT_TOP]))
        {
            return true;
        }
        else if (Physics.Linecast(vertices[BoxVertex.FRONT_RIGHT_BOT], vertices[BoxVertex.FRONT_RIGHT_TOP]))
        {
            return true;
        }
        else if (Physics.Linecast(vertices[BoxVertex.BACK_LEFT_BOT], vertices[BoxVertex.BACK_RIGHT_TOP]))
        {
            return true;
        }
        else if (Physics.Linecast(vertices[BoxVertex.FRONT_LEFT_BOT], vertices[BoxVertex.FRONT_RIGHT_TOP]))
        {
            return true;
        }
        else if (Physics.Linecast(vertices[BoxVertex.BACK_LEFT_BOT], vertices[BoxVertex.FRONT_LEFT_TOP]))
        {
            return true;
        }
        else if (Physics.Linecast(vertices[BoxVertex.BACK_RIGHT_BOT], vertices[BoxVertex.FRONT_RIGHT_TOP]))
        {
            return true;
        }
        else if (Physics.Linecast(vertices[BoxVertex.BACK_RIGHT_TOP], vertices[BoxVertex.FRONT_LEFT_TOP]))
        {
            return true;
        }
        else if (Physics.Linecast(vertices[BoxVertex.BACK_RIGHT_BOT], vertices[BoxVertex.FRONT_LEFT_BOT]))
        {
            return true;
        }
        return false;
    }

    public static void GlDrawBox(Vector3[] vertices)
    {
        GL.Vertex(vertices[BoxVertex.BACK_LEFT_BOT]);
        GL.Vertex(vertices[BoxVertex.BACK_RIGHT_BOT]);

        GL.Vertex(vertices[BoxVertex.FRONT_LEFT_BOT]);
        GL.Vertex(vertices[BoxVertex.FRONT_RIGHT_BOT]);

        GL.Vertex(vertices[BoxVertex.BACK_LEFT_TOP]);
        GL.Vertex(vertices[BoxVertex.BACK_RIGHT_TOP]);

        GL.Vertex(vertices[BoxVertex.FRONT_LEFT_TOP]);
        GL.Vertex(vertices[BoxVertex.FRONT_RIGHT_TOP]);
        //
        GL.Vertex(vertices[BoxVertex.FRONT_LEFT_BOT]);
        GL.Vertex(vertices[BoxVertex.BACK_LEFT_BOT]);

        GL.Vertex(vertices[BoxVertex.FRONT_LEFT_TOP]);
        GL.Vertex(vertices[BoxVertex.BACK_LEFT_TOP]);

        GL.Vertex(vertices[BoxVertex.BACK_LEFT_BOT]);
        GL.Vertex(vertices[BoxVertex.BACK_LEFT_TOP]);

        GL.Vertex(vertices[BoxVertex.FRONT_LEFT_BOT]);
        GL.Vertex(vertices[BoxVertex.FRONT_LEFT_TOP]);
        //
        GL.Vertex(vertices[BoxVertex.FRONT_RIGHT_BOT]);
        GL.Vertex(vertices[BoxVertex.BACK_RIGHT_BOT]);

        GL.Vertex(vertices[BoxVertex.FRONT_RIGHT_TOP]);
        GL.Vertex(vertices[BoxVertex.BACK_RIGHT_TOP]);

        GL.Vertex(vertices[BoxVertex.BACK_RIGHT_BOT]);
        GL.Vertex(vertices[BoxVertex.BACK_RIGHT_TOP]);

        GL.Vertex(vertices[BoxVertex.FRONT_RIGHT_BOT]);
        GL.Vertex(vertices[BoxVertex.FRONT_RIGHT_TOP]);
    }

}
