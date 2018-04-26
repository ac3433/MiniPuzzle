using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    #region Singleton
    private static GameController _instance;
    //Used only once to ensure when one thread have access to create the instance
    private static readonly object _Lock = new object();

    public static GameController Instance
    {
        get
        {
            //thread safe!
            lock (_Lock)
            {
                if (_instance != null)
                    return _instance;
                GameController[] instances = FindObjectsOfType<GameController>();
                //see if there are any already more instance of this
                if (instances.Length > 0)
                {
                    //yay only 1 instance so give it back
                    if (instances.Length == 1)
                        return _instance = instances[0];

                    //remove all other instance of it other than the 1st one
                    for (int i = 1; i < instances.Length; i++)
                        Destroy(instances[i]);
                    return _instance = instances[0];
                }

                GameObject manage = new GameObject("GameManager");
                manage.AddComponent<GameController>();

                return _instance = manage.GetComponent<GameController>();
            }
        }
    }
    #endregion

    private GameObject[,] circleNode;

    private void Start()
    {
        byte size = GridLevelManager.Instance.GridSize();
        circleNode = new GameObject[size, size];
    }


}
