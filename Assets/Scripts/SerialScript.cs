using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System.IO;
using System.Text;


//Comunication with mat
//----------------------------------------------------------------------
public class SerialScript : MonoBehaviour
{
    SerialPort sp;
    bool serialAvailable=false;
    float time;
    float period=1;
    int currentState=0;

    // Start is called before the first frame update
    void Start()
    {
        try{
            serialAvailable=true;
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
        catch(IOException e)
        {
            serialAvailable=false;
            Debug.Log("No serial at start");
        }
            
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!serialAvailable)
        {
            time+= Time.deltaTime;
            if(time>period)
            {
                time=0;
                try
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
                    
                    serialAvailable=true;
                    Debug.Log("Serial is connected");
                    sp.Write("1");
                }
                
                catch(IOException e) {
                        Debug.Log("no serial connected (inUpdate)");
                        serialAvailable=false; 
                }
            }
        }

        if(serialAvailable)
        {
            if (!sp.IsOpen)
            {
                sp.Open();
                Debug.Log("opened sp");
            } 
        }
        
    }

    public void sendState(int state)
    {
        currentState=state;
        if(serialAvailable)
        {
            try{
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
            catch (IOException e){
                serialAvailable=false;
                Debug.Log("serial disconnected (in send)");
            }
        }
    }

    public void closePort()
    {
        if(serialAvailable)
        {
            try{
                sp.Write("0");
                if(!sp.IsOpen)
                {
                    sp.Close();
                }  
            }
            catch (IOException e)
            {
                serialAvailable=false;
                Debug.Log("serial disconnected (in close)");
            }

        }
    }
}
