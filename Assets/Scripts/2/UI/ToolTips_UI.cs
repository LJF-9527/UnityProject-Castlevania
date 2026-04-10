using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolTips_UI : MonoBehaviour
{
    [SerializeField] private float xLimit = 960;
    [SerializeField] private float yLimit = 540;

    [SerializeField] private float xOffset = 250;
    [SerializeField] private float yOffset = 250;
    private float defaultSize=36;
    public virtual void AdjustPosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        float newXoffset = 0;
        float newYoffset = 0;
        if (mousePosition.x > xLimit) newXoffset=-xOffset;
        else newXoffset = xOffset;
        if (mousePosition.y > yLimit) newYoffset = -yOffset;
        else newYoffset = yOffset;

        transform.position = new Vector2(mousePosition.x + newXoffset, mousePosition.y + newYoffset);
    }
    public void AdjustFontSize(TextMeshProUGUI _text)
    {
        if (_text.text.Length > 11)
        { 
            defaultSize = _text.fontSize;
            _text.fontSize = _text.fontSize * .8f;
        }
    }
    public void SetDefaultSize(TextMeshProUGUI _text)
    {
        _text.fontSize = defaultSize;
    }
}
