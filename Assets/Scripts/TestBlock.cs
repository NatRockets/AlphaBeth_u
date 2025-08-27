using DG.Tweening;
using UnityEngine;

public class TestBlock : MonoBehaviour
{
    [SerializeField] private GameObject targetPanel;

    private Transform panelTransform;

    public void BindBlock()
    {
        panelTransform = targetPanel.transform;
    }
    
    public void Show(bool sliding = false, bool vert = false, System.Action callback = null)
    {
        targetPanel.SetActive(true);
        
        if (sliding)
        {
            if (vert)
            {
                panelTransform.DOScaleY(1f, 0.2f)
                    .OnKill(() =>
                    {
                        panelTransform.localScale = new Vector3(1f, 1f, 1f);
                        callback?.Invoke();
                    });
            }
            else
            {
                panelTransform.DOScaleX(1f, 0.2f)
                    .OnKill(() =>
                    {
                        panelTransform.localScale = new Vector3(1f, 1f, 1f);
                        callback?.Invoke();
                    });
            }

            return;
        }
        
        panelTransform.DOScale(1f, 0.4f)
            .OnKill(() =>
            {
                panelTransform.localScale = new Vector3(1f, 1f, 1f);
                callback?.Invoke();
            });
    }

    public void Hide(bool sliding = false, bool vert = false, System.Action callback = null)
    {
        if (sliding)
        {
            if (vert)
            {
                panelTransform.DOScaleY(0f, 0.2f)
                    .OnKill(() =>
                    {
                        targetPanel.SetActive(false);
                        callback?.Invoke();
                    });
            }
            else
            {
                panelTransform.DOScaleX(0f, 0.2f)
                    .OnKill(() =>
                    {
                        targetPanel.SetActive(false);
                        callback?.Invoke();
                    });
            }
            return;
        }
        
        panelTransform.DOScale(0f, 0.4f)
            .OnKill(() =>
            {
                panelTransform.localScale = Vector3.zero;
                targetPanel.SetActive(false);
                callback?.Invoke();
            });
    }

    public void HideForce()
    {
        panelTransform.localScale = Vector3.zero;
        targetPanel.SetActive(false);
    }
}
