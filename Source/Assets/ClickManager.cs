using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{

    private GameObject CurrentTouch = null;
    private GameObject SelectedLight = null;
    private bool isSelected = false;

    public gameManager m_gameManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out hit);

                if (hit.collider != null)
                {
                    CurrentTouch = hit.transform.gameObject;
                    Debug.Log(CurrentTouch.name);
                    if (CurrentTouch.name == "Tile")
                    {
                        if(CurrentTouch.GetComponent<tileManager>().isBuild == false)
                        {
                            ClickedTile();
                            isSelected = true;
                            m_gameManager.InitSelectPhase(CurrentTouch);
                            CurrentTouch.GetComponent<tileManager>().isBuild = true;

                        }
                        else
                        {
                            Debug.Log("이미 터렛이 지어졌습니다.");
                        }
                    }
                }
            }
        }
    }

    void ClickedTile()
    {
        if (SelectedLight != null)
        {
            SelectedLight.SetActive(false);
        }
        SelectedLight = CurrentTouch.transform.GetChild(0).gameObject;
        SelectedLight.SetActive(true);
    }


    public void ResetIsSelected()
    {
        isSelected = false;
    }
}
