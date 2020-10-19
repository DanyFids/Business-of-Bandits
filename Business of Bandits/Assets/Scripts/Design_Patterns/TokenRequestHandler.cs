using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenRequestHandler : MonoBehaviour
{

    public void ReceiveToken(Callback_Interface token)
    {
        token.Execute();
    }
}
