using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, InteractionObserver
{
    private Transform interactCamTr;
    private BoxCollider detectCollider;

    public Transform InteractCamPos
    {
        get => interactCamTr;
    }

    private void Awake()
    {
        interactCamTr = gameObject.transform.Find("InteractCamTr");

        detectCollider = gameObject.AddComponent<BoxCollider>();
        detectCollider.size = new Vector3(3f, 0, 3f);
        detectCollider.isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 tmp = other.transform.position - gameObject.transform.position;
            tmp.y = 0f;
            transform.forward = Vector3.Lerp(transform.forward, tmp.normalized, 5f * Time.deltaTime);

            int layerMask = 1 << LayerMask.NameToLayer("NPC");
            if (Physics.Raycast(other.transform.position + Vector3.up * 1f, other.transform.forward, 5f, layerMask))
            {
                PlayManager.instance.Player.AddInteractionObserver(this);
                PlayManager.instance.AddInteractionNPC(this);
                UIManager.Instance.PlayerUI.ActiveInteractionUI(true);
                PlayManager.instance.Player.IsInteractable = true;
            }
            else
            {
                PlayManager.instance.Player.RemoveInteractionObserver(this);
                PlayManager.instance.RemoveInteractionNPC(this);
                UIManager.Instance.PlayerUI.ActiveInteractionUI(false);
                PlayManager.instance.Player.IsInteractable = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayManager.instance.Player.RemoveInteractionObserver(this);
            PlayManager.instance.RemoveInteractionNPC(this);
            UIManager.Instance.PlayerUI.ActiveInteractionUI(false);
            PlayManager.instance.Player.IsInteractable = false;
            UIManager.Instance.NPCUI.gameObject.SetActive(false);
        }
    }

    public void ActiveInteraction()
    {
        UIManager.Instance.FadeIn(2f);
        UIManager.Instance.OpenNPCUI();
    }
}
