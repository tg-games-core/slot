using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.UI
{
    public class StickButton : Button
    {
        [SerializeField]
        private GameObject _selectedGroup;

        protected override void Start()
        {
            base.Start();
            
            _selectedGroup.SetActive(ReferenceEquals(EventSystem.current.currentSelectedGameObject, gameObject));
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            _selectedGroup.SetActive(true);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            
            _selectedGroup.SetActive(false);
        }
    }
}