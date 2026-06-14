using UnityEngine;
using UnityEditor;
using System.IO;

public class VignetteGenerator : MonoBehaviour
{
    [MenuItem("Tools/Создать виньетку (Vignette)")]
    public static void GenerateVignette()
    {
        int size = 512; // Размер картинки
        Texture2D tex = new Texture2D(size, size, TextureFormat.ARGB32, false);
        
        Color transparent = new Color(0, 0, 0, 0); // Прозрачный центр
        Color black = new Color(0, 0, 0, 1f);      // Черные края
        
        float radius = size / 2f;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                // Считаем расстояние от центра картинки
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(radius, radius));
                
                // Преобразуем расстояние в значение от 0 до 1. 
                // 0.4f означает, что затемнение начнется на 40% от центра
                float t = Mathf.InverseLerp(radius * 0.4f, radius, dist);
                
                // Смешиваем прозрачный и черный цвета
                Color c = Color.Lerp(transparent, black, t);
                tex.SetPixel(x, y, c);
            }
        }
        
        tex.Apply();

        // Сохраняем картинку в проект
        byte[] bytes = tex.EncodeToPNG();
        string path = Application.dataPath + "/Vignette.png";
        File.WriteAllBytes(path, bytes);
        
        AssetDatabase.Refresh();
        Debug.Log("Виньетка создана: Assets/Vignette.png");
    }
}