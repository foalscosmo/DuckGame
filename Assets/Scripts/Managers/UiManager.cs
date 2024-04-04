using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private List<Image> progressBarSlots = new();
        [SerializeField] private Sprite completeSlot;

        public void FillProgressBar(int index)
        {
            progressBarSlots[index].sprite = completeSlot;
        }
    }
}