using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPopUp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    internal string Text = "";

    public GameObject popUpPanel;
    Text popUpText;


    public void OnAwake()
    {

        popUpPanel.SetActive(false);

    }

    //Activates popup object and displays the text. 
    //Finds out how much height the text takes and 
    //scales text background object accordingly
    public void OnPointerEnter(PointerEventData eventData)
    {
        float preferredTextHeight = 0;

        popUpPanel.SetActive(true);
        popUpText = popUpPanel.transform.Find("UIText").GetComponent<UnityEngine.UI.Text>();
        RectTransform popUpBgPanelRectTransform = popUpPanel.transform.Find("UIBlock").GetComponent<RectTransform>();

        popUpText.text = Text;

        

        preferredTextHeight = popUpText.preferredHeight;

        popUpBgPanelRectTransform.sizeDelta = new Vector2(popUpBgPanelRectTransform.sizeDelta.x, preferredTextHeight);


    }

    public void OnPointerExit(PointerEventData eventData)
    {
        popUpPanel.SetActive(false);
       
    }
 



}
