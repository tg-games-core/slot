using System.Linq;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core
{
    public static class VContainerExtension
    {
        public static void InjectInHierarchy<T>(this IObjectResolver container, Transform parent)
            where T : MonoBehaviour
        {
            var components = parent.GetComponentsInChildren<T>(true).Select(t => t.gameObject).ToArray();

            container.InjectInHierarchy(components);
        }

        public static void InjectInHierarchy(this IObjectResolver container, GameObject[] components)
        {
            foreach (var comp in components)
            {
                container.InjectGameObject(comp.gameObject);
            }
        }
    }
}