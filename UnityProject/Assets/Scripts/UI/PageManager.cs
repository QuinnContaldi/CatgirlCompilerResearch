using UnityEngine;
using UnityEngine.UI;

namespace Meowra.UI
{
    /// <summary>Controls panel visibility only; page content and study flow live elsewhere.</summary>
    public sealed class PageManager : MonoBehaviour
    {
        [Tooltip("Sibling page panels, in navigation order. Keep this component outside the panels.")]
        [SerializeField] private GameObject[] pages;
        [SerializeField] private Button previousButton;
        [SerializeField] private Button nextButton;

        public int CurrentPageIndex { get; private set; } = -1;

        private void Awake()
        {
            if (pages == null || pages.Length == 0)
            {
                Debug.LogError("PageManager needs at least one page.", this);
                enabled = false;
                return;
            }

            for (int i = 0; i < pages.Length; i++)
            {
                if (pages[i] == null)
                {
                    Debug.LogError($"PageManager page {i} is not assigned.", this);
                    enabled = false;
                    return;
                }
            }

            ShowPage(0);
        }

        public void NextPage()
        {
            ShowPage(CurrentPageIndex + 1);
        }

        public void PreviousPage()
        {
            ShowPage(CurrentPageIndex - 1);
        }

        public void ShowPage(GameObject page)
        {
            if (pages != null)
                ShowPage(System.Array.IndexOf(pages, page));
        }

        // A future experiment controller can select a page without owning UI details.
        public void ShowPage(int index)
        {
            if (!enabled || pages == null || index < 0 || index >= pages.Length)
                return;

            CurrentPageIndex = index;
            for (int i = 0; i < pages.Length; i++)
                pages[i].SetActive(i == index);

            if (previousButton != null)
                previousButton.interactable = index > 0;

            if (nextButton != null)
                nextButton.interactable = index < pages.Length - 1;
        }
    }
}
