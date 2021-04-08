using System.Collections.Generic;
using UnityEngine;

public class MachineController : MonoBehaviour
{
    public List<GameObject> balls = new List<GameObject>();
    public List<string> ballsPLS = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        for(int cpt = 6; cpt < 19; cpt++)
        {
            ballsPLS.Add("Ball"+cpt);
        }
    }

    public string GetPrefabName (string gOName)
    {
        for(int cpt = 0; cpt < ballsPLS.Count; cpt++)
        {
            if (gOName.Contains(ballsPLS[cpt]))
            {
                return ballsPLS[cpt];
            }
        }
        return null;
    }

    public void RespawnBall (GameObject ball)
    {
        string prefabName = GetPrefabName(ball.name);
        if (int.Parse(prefabName.Remove(0, 4)) % 2 == 0)
        {
            balls.Add(Instantiate(Resources.Load<GameObject>("Prefabs/" + prefabName), new Vector3(-0.985f, 0.636f, -14.035f), Quaternion.identity));
        }
        else
        {
            balls.Add(Instantiate(Resources.Load<GameObject>("Prefabs/" + prefabName), new Vector3(-2.929f, 0.636f, -14.035f), Quaternion.identity));
        }
        Destroy(ball);
    }

    private void OnApplicationQuit()
    {
        for (int cpt = 0; cpt > balls.Count; cpt++)
        {
            if (balls[cpt] != null)
            {
                Destroy(balls[cpt]);
            }
        }
    }
}
