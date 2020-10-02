using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    /// <summary>
    /// target - obiekt za ktorym bedzie poruszac sie kamera
    /// smoothing parametr wygladzajacy ruch kamery
    /// player - instancja gracza
    /// offset - odleglosc kamery od gracza
    /// </summary>
    public Transform target;
    public float smoothing = 5;
    Player player;

    Vector3 offset;
    //Start jest wywoływany przed pierwszym Update. Inicjuje zmienną offset i przyporządkowuje zmiennej player obiekt gracza
    void Start()
    {
        offset = transform.position - target.position;
        player = FindObjectOfType<Player>();
    }

    //Wywoływana co każdą klatkę. Jeżeli gra nie jest w stanie pauzy oblicza docelową pozycję kamery na podstawie pozycji gracza i początkowego przesunięcia między graczem i kamerą.
    //Następnie przemieszcza kamerę do pozycji docelowej korzystając z liniowej interpolacji w celu wygładzenia ruchu kamery.
    private void Update()
    {
        if (player.GetCurrentState() == 1)
        {
            Vector3 targetCamPos = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
    }
}
