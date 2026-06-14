using UnityEngine;

namespace UI
{
    // Абстрактный класс - мы не сможем повесить его напрямую, только его наследников
    public abstract class UIPanel : MonoBehaviour
    {
        // Виртуальные методы, которые наследники могут переопределить
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}