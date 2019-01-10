using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class DataManager : MonoBehaviour
{
    Profile myProfile;
    public InputField nameField;
    public InputField scoreField;

    // Use this for initialization
    void Start()
    {
        myProfile = new Profile();
        Debug.Log(myProfile.GetName());
    }

    public void ChangeName(string name)
    {
        myProfile.SetName(name);
        Debug.Log(myProfile.GetName());
    }
    public void ChangeScore(string score)
    {
        myProfile.SetScore(int.Parse(score));
    }
    public void SaveData()
    {
        Stream stream = File.Open("Data.xml", FileMode.Create);
        XmlSerializer serializer = new XmlSerializer(typeof(Profile));
        serializer.Serialize(stream, myProfile);
        stream.Close();
    }
    public void LoadData()
    {
        if (File.Exists("Data.xml"))
        {
            Stream stream = File.Open("Data.xml", FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(Profile));
            myProfile = serializer.Deserialize(stream) as Profile;
            stream.Close();
            nameField.text = myProfile.GetName();
            scoreField.text = myProfile.GetScore().ToString();
            Debug.Log(myProfile.GetName());
        }        
    }
}