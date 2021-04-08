using UnityEngine;

public class Score : MonoBehaviour //pas score mais ResultatFrame
{
    public int tour;
    public int[] frame; //roll tab
    public int total;

    public Score(int tourSai, int[] frameSai)
    {
        tour = tourSai;
        frame = frameSai;
        total = 0;
    }

    public int GetRoll(int numRoll)
    {
        return frame[numRoll];
    }

    public int GetScoreFrame()
    {
        return frame[0] + frame[1] + frame[2];
    }

    public void SetTotal(int newtotal)
    {
        total = newtotal;
    }

    public bool Spare()
    {
        if (frame[0] + frame[1] == 10 && frame[0] != 10 || frame[1] + frame[2] == 10 && frame[1] != 10)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Strike(int tour, int roll)
    {
        if (tour <10 && roll==0 && frame[0]==10 || tour==10 && roll==0 && frame[0] == 10 || tour==10 && roll>0 && frame[roll] == 10)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
