using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System;

public class streamAudio : MonoBehaviour {
	AudioSource aud;
	// Use this for initialization
	void Start () {
		beginListening ();
	}

	public void beginListening(){
		aud = GetComponent<AudioSource> ();
		aud.clip = Microphone.Start (Microphone.devices [0], true, 999, 44100);
		while (!(Microphone.GetPosition(Microphone.devices [0]) > 0)){}
		aud.mute = true;
		aud.Play ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey("escape"))
			Application.Quit();

		saveFile();
		GameObject.Find ("volumeText").GetComponent<Text> ().text = GameObject.Find ("Slider").GetComponent<Slider> ().value.ToString ();
	}

	void saveFile()
	{
		float[] samples = new float[4096];
		//aud.GetOutputData (samples, 0);
		aud.clip.GetData(samples, Microphone.GetPosition(Microphone.devices[0])-4096);
		float maxAmp = (float)samples.Max () * 2;


		if (maxAmp > 0.001f) {
			LomontFFT fftMe = new LomontFFT ();
			double[] sampleFFT = Array.ConvertAll (samples, x => (double)x);
			fftMe.RealFFT (sampleFFT, true);

			float[] frequences = new float[5];
			frequences[0] = (float)sampleFFT.Take (25).Max ();
			frequences[1] = (float)sampleFFT.Skip (25).Take (50).Max ();
			frequences[2] = (float)sampleFFT.Skip (75).Take (75).Max ();
			frequences[3] = (float)sampleFFT.Skip (150).Take (100).Max ();
			frequences[4] = (float)sampleFFT.Skip (250).Take (125).Max ();
			float maxFreq = frequences.Max ();
			int maxFreqIndex = frequences.ToList ().IndexOf (maxFreq);


			if (maxFreqIndex == 0) {
				//float noise = (UnityEngine.Random.value - 0.5f) / 5f;
				float noise = 0f;
				makeOrb(maxFreq,new Vector3(22 + noise,-5,20));
				makeOrb(maxFreq,new Vector3(-22 + noise,-5,20));
			}
			if (maxFreqIndex == 1) {
				//float noise = (UnityEngine.Random.value - 0.5f) / 5f;
				float noise = 0f;
				makeOrb (maxFreq, new Vector3 (20+ noise, -2, 20));
				makeOrb (maxFreq, new Vector3 (-20+ noise, -2, 20));
				//makeOrb (maxFreq, new Vector3 (-35+ noise, -1, -15));
				//makeOrb (maxFreq, new Vector3 (35+ noise, -1, -15));
			}
			if (maxFreqIndex == 2) {
				//float noise = (UnityEngine.Random.value - 0.5f) / 5f;
				float noise = 0f;
				makeOrb(maxFreq,new Vector3(15+ noise,3,20));
				makeOrb(maxFreq,new Vector3(-15+ noise,3,20));
				//makeOrb(maxFreq,new Vector3(-25+ noise,0,-25));
				//makeOrb(maxFreq,new Vector3(25+ noise,0,-25));
			}
			if (maxFreqIndex == 3) {
				//float noise = (UnityEngine.Random.value - 0.5f) / 5f;
				float noise = 0f;
				makeOrb (maxFreq, new Vector3 (10+ noise, 7, 25));
				makeOrb (maxFreq, new Vector3 (-10+ noise, 7, 25));
				//makeOrb (maxFreq, new Vector3 (-15+ noise, 1, -35));
				//makeOrb (maxFreq, new Vector3 (15+ noise, 1, -35));
			}
			if (maxFreqIndex == 4) {
				//float noise = (UnityEngine.Random.value - 0.5f) / 5f;
				float noise = 0f;
				makeOrb (maxFreq, new Vector3 (0+ noise, 10, 30));
				//makeOrb (maxFreq, new Vector3 (0+ noise, 2, -45));
			}
		}

	}

	public void makeOrb(float freq, Vector3 myPosition){
		GameObject sliderOb = GameObject.Find ("Slider");
		float circleSize = sliderOb.GetComponent<Slider> ().value / 2f;
		GameObject goMed = (GameObject)Instantiate (Resources.Load ("objectSphere")); 
		goMed.transform.localScale = new Vector3 (freq * circleSize, freq * circleSize, freq * circleSize);
		goMed.GetComponent<Rigidbody> ().mass = freq * circleSize;
		goMed.transform.position = myPosition;
		goMed.GetComponent<Renderer> ().material.color = new Color (UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);
	}
}
