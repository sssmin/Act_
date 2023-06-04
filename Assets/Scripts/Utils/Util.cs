using System.Collections.Generic;
using UnityEngine;

public class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
		if (component == null)
            component = go.AddComponent<T>();
        return component;
	}

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;
        
        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
		}
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static Vector3 GetMouseWorldPos()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.x = Mathf.Clamp(mouseScreenPos.x, 0f, Screen.width);
        mouseScreenPos.y = Mathf.Clamp(mouseScreenPos.y, 0f, Screen.height);

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        worldPos.z = 0f;

        return worldPos;
    }

    public static List<Transform> GetMonstersInScreen()
    {
        List<Transform> monsters = new List<Transform>();
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 cameraBotLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, cameraPos.z));
        Vector3 cameraTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, cameraPos.z));

        Collider2D[] cols = Physics2D.OverlapAreaAll(cameraBotLeft, cameraTopRight, LayerMask.GetMask("Monster"));

        foreach (Collider2D col in cols)
        {
            monsters.Add(col.transform);
        }
        
        return monsters;
    }

    public static Vector3 GetCenterWorldPos()
    {
        Vector3 viewportCenter = new Vector3(0.5f, 0.5f, 0);
        Vector3 worldCenter = Camera.main.ViewportToWorldPoint(viewportCenter);
        worldCenter.z = 0f;
        return worldCenter;
    }
    
    
}
