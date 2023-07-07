using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;

public class Wires {
    public Wires(wireTesting _module, GameObject _self, int _number) { module = _module; self = _self; number = _number; }
    public int number { get; set; }
    GameObject self { get; set; }
    wireTesting module { get; set; }
    string[] actions = new string[] { "Be Normal", "Not Cut", "False Strike", "False Solve", "Be Pressed", "bip" };
    public Texture Color { get; set; }
    public int ColorNumber { get; set; }
    public string Action { get; set; }
    public int ActionNumber { get; set; }
    bool cut;

    public bool Valid { get; set; }

    public void Startup() {
        Valid = false;
        ColorNumber = Rnd.Range(0, module.WireColors.Length);
        Color = module.WireColors[ColorNumber];
        ActionNumber = Rnd.Range(0, actions.Length);
        Action = actions[ActionNumber];
        self.transform.Find("uncutwire").GetComponent<MeshRenderer>().material.mainTexture = Color;
        self.transform.Find("uncutwire").GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(7, 1);
        self.transform.Find("cutwire").transform.Find("WireCasing").GetComponent<MeshRenderer>().material.mainTexture = Color;
        self.transform.Find("cutwire").transform.Find("WireCasing").GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(7, 1);
        self.transform.Find("uncutwire").gameObject.SetActive(true);
        self.transform.Find("cutwire").gameObject.SetActive(false);
        cut = false;
        self.transform.localPosition = new Vector3(0.485f, 0, -0.2f);
    }

    public void wirecut() {
        if(cut) { return; }
        cut = true;
        module.Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.WireSnip, self.transform);
        module.wireSelectables[number].AddInteractionPunch();
        switch(Action) {
            case "Be Normal":
                self.transform.Find("uncutwire").gameObject.SetActive(false);
                self.transform.Find("cutwire").gameObject.SetActive(true);
                break;
            case "Not Cut":
                break;
            case "False Strike":
                self.transform.Find("uncutwire").gameObject.SetActive(false);
                self.transform.Find("cutwire").gameObject.SetActive(true);
                module.FakeStrike();
                break;
            case "False Solve":
                self.transform.Find("uncutwire").gameObject.SetActive(false);
                self.transform.Find("cutwire").gameObject.SetActive(true);
                module.FakeSolve();
                break;
            case "Be Pressed":
                module.WirePress(self);
                break;
            case "bip":
                self.transform.Find("uncutwire").gameObject.SetActive(false);
                self.transform.Find("cutwire").gameObject.SetActive(true);
                module.bip();
                break;
        }
    }
}