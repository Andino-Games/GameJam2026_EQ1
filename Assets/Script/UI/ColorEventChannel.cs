using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Game Architecture/Color Event Channel")]
public class ColorEventChannel : ScriptableObject
{
    public GameColor CurrentColor { get; private set; }
    public event Action<GameColor> OnColorChanged;

    public void RaiseColorChanged(GameColor newColor)
    {
        if (CurrentColor == newColor) return;
        CurrentColor = newColor;
        OnColorChanged?.Invoke(newColor);
    }
    
    public void ResetState()
    {
        CurrentColor = GameColor.None;
    }
}