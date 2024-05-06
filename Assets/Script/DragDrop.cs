using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class DragDrop : MonoBehaviour
{
    
    [SerializeField] GameObject[] item; // Array item yang bisa dijatuhkan
    [SerializeField] Transform[] itemDrop; // Target drop untuk setiap item
    [SerializeField] int jarak; // value untuk menentukan jarak antara item dan item drop
    [SerializeField] GameObject panelWin; // Panel yang menunjukkan kemenangan
    public Vector2[] itemPos; // menyimpan posisi awal objek
    bool[] isCorrectPosition; // Untuk melacak posisi yang benar
    

    // Start is called before the first frame update
    void Start()
    {
        // Inisialisasi isCorrectPosition berdasarkan jumlah item
        isCorrectPosition = new bool[item.Length];
        
        // Simpan posisi awal setiap item
        for (int i = 0; i < itemPos.Length; i++)
        {
            itemPos[i] = item[i].transform.localPosition;
        }

        // Mencari game object yang memiliki komponen Music
        Music sliderMusic = FindObjectOfType<Music>();
        if (sliderMusic != null)
        {
             GetComponentInChildren<Music>();
            // Lakukan sesuatu dengan sliderMusic...
        }
        else
        {
            Debug.LogWarning("Music script not found!");
        }
    }
    public void ItemDrag(int number)
    {
        item[number].transform.position = Input.mousePosition;
        
    }
    public void ItemDrop(int number)
    {
        float distance = Vector3.Distance(item[number].transform.localPosition, itemDrop[number].transform.localPosition);
         
        
        if (distance < jarak)
        {
            item[number].transform.localPosition = itemDrop[number].transform.localPosition;
            isCorrectPosition[number] = true; // Item ini sudah diletakkan dengan benar
            
        }
        else
        {
            item[number].transform.localPosition = itemPos[number];
            isCorrectPosition[number] = false; // Item ini tidak diletakkan dengan benar
        }

        CheckWinCondition();
    }
    void CheckWinCondition()
    {
        // Periksa apakah semua item sudah diletakkan dengan benar
        foreach (bool status in isCorrectPosition)
        {
            if (!status) return; // Jika ada satu item yang belum benar, keluar
        }
        // Jika kode mencapai sini, berarti semua item sudah benar
        panelWin.SetActive(true);
    }
    //musik
    public void OnVolumeChange(float volume)
    {
        if (Music.instance != null)
        {
            Music.instance.SetVolume(volume);
        }
        
    }
    //perpindahan schene
    public void LoadToScene(string sceneName)
    {
        // Di mana pun Anda memuat scene baru, pastikan "MainMenu" dimuat juga
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene(sceneName);
    }
}
