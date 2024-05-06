using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour
{
    public AudioSource music;
    public Slider sliderMusic;
    public static Music instance;

    public const string keyVolume = "Volume";
    private void Awake()
    {
        // Singleton pattern: Pastikan hanya ada satu instance Music yang hidup
        // Jika instance belum ada, set ini sebagai instance dan jangan hancurkan saat load scene baru
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Jangan hancurkan sliderMusic saat berpindah scene
            DontDestroyOnLoad(sliderMusic.gameObject);
        }
        // Jika instance sudah ada dan bukan ini, hancurkan ini
        else 
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Start()
    {
        // Mainkan musik jika belum diputar
        if (!music.isPlaying)
        {
            music.Play();
        }

        // Gunakan nilai default 0.5f jika tidak ada nilai yang disimpan
        float lastVolume = PlayerPrefs.GetFloat(keyVolume, 0.5f);
        SetVolume(lastVolume);
        sliderMusic.value = lastVolume;

        // Tambahkan listener ke slider untuk memperbarui volume secara dinamis
        sliderMusic.onValueChanged.AddListener(delegate { SetVolume(sliderMusic.value); });
    }
    private void Update()
    {
        
    }
    public void SetVolume(float volume)
    {
        music.volume = volume;

        PlayerPrefs.SetFloat(keyVolume, volume);
    }
}
