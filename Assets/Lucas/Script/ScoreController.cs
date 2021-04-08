using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public List<Transform> nbQuille = new List<Transform>();
    public List<Transform> nbQuilleEmptyTransfrom = new List<Transform>();
    public List<Score> gestionScore = new List<Score>();

    public int tour;
    public int roll;
    public int cRoll;
    public int scorefinal;
    public int touche;
    public int[] frame; //On part sur 2 coups au début

    // Start is called before the first frame update
    void Start()
    {
        roll = 0;
        scorefinal = 0;
        touche = 0;
        tour = 1;

        for (int cpt = 0; cpt < nbQuille.Count; cpt++)
        {
            nbQuille[cpt] = Instantiate(Resources.Load<GameObject>("Prefabs/PinPref"), nbQuilleEmptyTransfrom[cpt].gameObject.transform.position, Quaternion.identity).transform;
            nbQuille[cpt].name = "PinPref" + (cpt + 1);
            nbQuille[cpt].transform.SetParent(nbQuilleEmptyTransfrom[cpt]);
        }

        cRoll = 1;
    }

    public bool TourSupplementaire()
    {
        bool toursupp = false;
        if (tour == 10 && roll <= 2 && frame[0]+frame[1] >=10) //Gestion du troisième tour
        {
            toursupp = true;
        }
        return toursupp;
    }

    public void CalculResultat()
    {
        for (int index = 0; index <gestionScore.Count; index ++)
        {
            if(index != 0)
            {
                if (gestionScore[index - 1].Strike(gestionScore[index-1].tour, 0)) //Vérification Strike
                {
                    if (gestionScore[index].Strike(gestionScore[index].tour, 0) && gestionScore[index].tour != 10)
                    {
                        gestionScore[index - 1].SetTotal(gestionScore[index - 1].GetScoreFrame() + gestionScore[index].GetScoreFrame() + gestionScore[index + 1].frame[0]); //10 + les deux boules d'après
                        gestionScore[index].SetTotal(gestionScore[index].GetScoreFrame());
                    }
                    else
                    {
                        gestionScore[index - 1].SetTotal(10 + gestionScore[index].frame[0]+ gestionScore[index].frame[1]); //10 + les deux boules d'après
                        gestionScore[index].SetTotal(gestionScore[index].GetScoreFrame());
                    }                   
                }

                if (gestionScore[index - 1].Spare()) //Vérification Spare
                {
                    gestionScore[index - 1].SetTotal(10 + gestionScore[index].GetRoll(0)); //10 + les points de la boule d'après
                    gestionScore[index].SetTotal(gestionScore[index].GetScoreFrame());
                }

                if (gestionScore[index].Spare() == false || gestionScore[index].Strike(gestionScore[index].tour, 0) == false)
                {
                    gestionScore[index].SetTotal(gestionScore[index].GetScoreFrame()); //Obtention du score pour un frame

                }
            }
            else
            {
                //Lors du premier tour, l'index ne doit pas être négative, sinon nous avons une erreur. Donc, le premier tour va juste entrer un total
                gestionScore[index].SetTotal(gestionScore[index].GetScoreFrame()); //Obtention du score pour un frame
            }
        }
    }

    public void ResultatFinal() //Pour l'affichage du score finale
    {
        int resultat = 0;
        for(int index = 0; index < gestionScore.Count; index++)
        {
            resultat += gestionScore[index].total;
        }
        GameObject.Find("Score10").GetComponent<Text>().text = resultat.ToString();
    }

    public async void NewRoll()
    {
        for (int cpt = 0; cpt < nbQuille.Count; cpt++)
        {
            if (nbQuille[cpt] != null)
            {
                if (nbQuille[cpt].GetComponent<QuilleControlleur>().estTombee && tour == 10 && roll != 2)
                {
                    Destroy(nbQuille[cpt].gameObject);
                }
                else if ((tour == 10 && roll >= 2) && TourSupplementaire() == false)
                {
                    Destroy(nbQuille[cpt].gameObject);
                    nbQuille[cpt] = Instantiate(Resources.Load<GameObject>("Prefabs/PinPref"), nbQuilleEmptyTransfrom[cpt].gameObject.transform.position, Quaternion.identity).transform;
                    nbQuille[cpt].name = "PinPref" + (cpt + 1);
                    nbQuille[cpt].transform.SetParent(nbQuilleEmptyTransfrom[cpt]);
                }
                else
                {
                    if (nbQuille[cpt].GetComponent<QuilleControlleur>().estTombee)
                    {
                        Destroy(nbQuille[cpt].gameObject);
                    }
                    nbQuille[cpt].position = nbQuilleEmptyTransfrom[cpt].position;
                    nbQuille[cpt].rotation = Quaternion.identity;
                    nbQuille[cpt].gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
                }
            }
        }

        if (tour == 10)
        {
            if ((gestionScore[tour-1].Strike(tour, roll - 1) || gestionScore[tour - 1].Spare()) && TourSupplementaire())
            {
                for (int cpt = 0; cpt < nbQuille.Count; cpt++)
                {
                    if (nbQuille[cpt] != null)
                    {
                        Destroy(nbQuille[cpt].gameObject);
                    }
                    nbQuille[cpt] = Instantiate(Resources.Load<GameObject>("Prefabs/PinPref"), nbQuilleEmptyTransfrom[cpt].gameObject.transform.position, Quaternion.identity).transform;
                    nbQuille[cpt].name = "PinPref" + (cpt + 1);
                    nbQuille[cpt].transform.SetParent(nbQuilleEmptyTransfrom[cpt]);
                }
            }
        }

        await Task.Delay(700);

        for (int cpt = 0; cpt < nbQuille.Count; cpt++)
        {
            if (nbQuille[cpt] != null)
            {
                nbQuille[cpt].gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
        }
    }

    public void NewFrame()
    {
        if (tour == 11) //Le 11 ème tour étant la fin
        {
            ResultatFinal();
        }
        else
        {
            tour++;
            for (int cpt = 0; cpt < nbQuille.Count; cpt++)
            {
                if (nbQuille[cpt] != null)
                {
                    Destroy(nbQuille[cpt].gameObject);
                    nbQuille[cpt] = null;
                }

                nbQuille[cpt] = Instantiate(Resources.Load<GameObject>("Prefabs/PinPref"), nbQuilleEmptyTransfrom[cpt].gameObject.transform.position, Quaternion.identity).transform;
                nbQuille[cpt].name = "PinPref" + (cpt + 1);
                nbQuille[cpt].transform.SetParent(nbQuilleEmptyTransfrom[cpt]);
            }
        }
    }
    public async void ScoreList()
    {
        await Task.Delay(77);
        if (roll == 0)
        {
            frame = new int[3]; //Nous devons déclarer notre tableau ici pour éviter les conflits. Le programme recréra un nous tableau à chaque fois.
        }

        frame[roll] = Verification(); //On prend le nombre de quille qui sont tombées
        roll++; //On change de coup

        if(roll-1 == 0)
        { 
            gestionScore.Add(new Score(tour, frame)); //On ajoute le score dans la liste des tours
        }
        else
        {
            gestionScore[tour-1].frame = frame;    
        }
        
        if ((roll > 1 || frame[roll-1] == 10) && TourSupplementaire()==false) //Pour le moment, on regarde si c'est le troisième coup
        {
            //Si tel est le cas
            roll = 0; //On réinitialise les coups
            NewFrame();
        }
        else
        {
            NewRoll();
        }

        cRoll = GameObject.Find("Canvas").GetComponent<HUD>().UpdateHUD(cRoll, touche);
        cRoll++;

        if (tour == 11)
        {
            CalculResultat();//On calcule le résultat pour cette frame
            ResultatFinal();
            gameObject.SetActive(false);
        }
    }

    public int Verification()
    {
        touche = 0;
        for (int index = 0; index < nbQuille.Count; index++)
        {
            if (nbQuille[index] != null)
            {
                if (nbQuille[index].eulerAngles.x > 29   && nbQuille[index].eulerAngles.x < 331  ||
                    nbQuille[index].eulerAngles.x < -29  && nbQuille[index].eulerAngles.x > -331 ||
                    nbQuille[index].eulerAngles.x < 331  && nbQuille[index].eulerAngles.x > 389  ||
                    nbQuille[index].eulerAngles.x > -331 && nbQuille[index].eulerAngles.x < -389 ||

                    nbQuille[index].eulerAngles.z > 29   && nbQuille[index].eulerAngles.z < 331  ||
                    nbQuille[index].eulerAngles.z < -29  && nbQuille[index].eulerAngles.z > -331 ||
                    nbQuille[index].eulerAngles.z < 331  && nbQuille[index].eulerAngles.z > 389  ||
                    nbQuille[index].eulerAngles.z > -331 && nbQuille[index].eulerAngles.z < -389 ||

                    nbQuille[index].GetComponent<QuilleControlleur>().ejected == true) //Vérification de l'état de la quille
                {
                    nbQuille[index].GetComponent<QuilleControlleur>().estTombee = true;
                    touche++;
                }
            }
        }
        return touche;
    }

    public async void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Boule"))
        {
            await Task.Delay(2500);
            ScoreList();
            GameObject.Find("Machine_with_rail1").GetComponent<MachineController>().RespawnBall(other.gameObject);
        }
    }
}
