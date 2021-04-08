using UnityEngine;

public class QuilleControlleur : MonoBehaviour
{
    private AudioManager audio; // Pour utiliser la classe audio
    public Transform game;      //Pour récupérer l'objet

    public bool estTombee = false;
    public bool ejected = false;

    // Start is called before the first frame update
    void Start()
    {
        game = GetComponent<Transform>();
        audio = GameObject.Find("AudioManager").GetComponent<AudioManager>(); //On récupère l'audiomanager de l'objet
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boule") || collision.gameObject.CompareTag("Quille")) //Vérification de si c'est une boule ou une quille
        {
            audio.Play(game); //On va jouer le son approprié
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("VerifQuille"))
        {
            ejected = true;
        }
    }
}
