using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class ColorManager : MonoBehaviour
    {
        // List of sprite renderers for ducks
        [SerializeField] public List<SpriteRenderer> ducksRenderer = new(); // Ducks' sprite renderers.
        
        // List of sprite renderers for buckets
        [SerializeField] public List<SpriteRenderer> bucketsRenderer = new(); // Buckets' sprite renderers.
        
        // Container for colors
        [SerializeField] private List<Color> colorContainer = new(); // Container for colors used for ducks and buckets.

        // Start is called before the first frame update
        private void Awake()
        {
            // Set color to objects when the scene starts
            SetColorToObjects();
        }

        // Function to set color to ducks and buckets
        public void SetColorToObjects()
        {
            // Shuffle the color container
            Shuffle(colorContainer);
            
            // Shuffle the ducks' and buckets' renderers
            Shuffle(ducksRenderer);
            Shuffle(bucketsRenderer);

            // Dictionary to map colors to layer index
            var colorToLayerMap = new Dictionary<Color, int>();

            // Loop through the renderers
            for (var i = 0; i < ducksRenderer.Count; i++)
            {
                // Set the color of duck's renderer
                ducksRenderer[i].material.color = colorContainer[i];
                
                // Set the color of bucket's renderer
                bucketsRenderer[i].material.color = colorContainer[i];

                // If color not in dictionary, assign next layer index
                if (!colorToLayerMap.ContainsKey(colorContainer[i]))
                {
                    // Assign next layer index
                    var nextLayerIndex = colorToLayerMap.Count + 6; 
                    colorToLayerMap[colorContainer[i]] = nextLayerIndex;
                }

                // Assign layer to duck's renderer
                ducksRenderer[i].gameObject.layer = colorToLayerMap[colorContainer[i]];
                
                // Assign layer to bucket's renderer
                bucketsRenderer[i].gameObject.layer = colorToLayerMap[colorContainer[i]];
            }
        }

        // Function to shuffle a list
        private static void Shuffle<T>(IList<T> list)
        {
            // Loop through the list
            for (var i = list.Count - 1; i > 0; i--)
            {
                // Generate random index
                var j = Random.Range(0, i + 1);
                
                // Swap elements at i and j indices
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
