using Unity.VisualScripting;
using UnityEngine;
using thread = System.Threading.Tasks;

public class StaticMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private Transform head;
    [SerializeField]private float spawnDistance = 2;


    private void Update() 
    {
        // el menu siempre mira al usuario.
        menu.transform.LookAt(new Vector3(head.position.x, menu.transform.position.y, head.position.z));
        menu.transform.forward *= -1;   
    }
}
