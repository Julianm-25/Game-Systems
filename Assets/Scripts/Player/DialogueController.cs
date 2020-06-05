using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public Dialogue reference;
    public void OptionButton(bool status)
    {
        if(reference)
        {
            if(status)
            {
                reference.PositiveOption();
            }
            else
            {
                reference.NegativeOption();
            }
        }
    }
}
