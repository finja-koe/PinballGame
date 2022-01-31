using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;


//Comunication with mat
//----------------------------------------------------------------------
public class SerialScript : MonoBehaviour
{
    SerialPort sp;

    // Start is called before the first frame update
    void Start()
    {
        string the_com="";
        foreach (string mysps in SerialPort.GetPortNames())
        {
            print(mysps);
            if (mysps != "COM1") { the_com = mysps; break; }
        }
        sp = new SerialPort("\\\\.\\" + the_com, 9600);
        if (!sp.IsOpen)
        {
            print("Opening " + the_com + ", baud 9600");
            sp.Open();
            sp.ReadTimeout = 100;
            sp.Handshake = Handshake.None;
            if (sp.IsOpen) { print("Open"); }
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if (!sp.IsOpen)
        {
            sp.Open();
            Debug.Log("opened sp");
        } 
    }

    public void sendState(int state)
    {
        if (!sp.IsOpen)
        {
            sp.Open();
            Debug.Log("opened sp");
        }
        else
        {
            Debug.Log("Send: " + state);
            sp.Write((state.ToString()));
            
        } 
    }

    public void closePort()
    {
        sp.Write("0");
        if(!sp.IsOpen)
        {
            sp.Close();
        }   
    }
}
