using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CosmeticMenu : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private Vector3 rotationAxis = new Vector3(0, 1, 0); // Default rotation on Y-axis
    [SerializeField] private float rotationSpeed = 50f; // Rotation speed in degrees per second

    [Header("References")]
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject player;
    [SerializeField] private TMP_FontAsset font;
    [SerializeField] private Button removeCosmetic;

    [Header("Cosmetics")]
    public CosmeticScriptableObject[] cosmetics;

    private void Start()
    {
        removeCosmetic.onClick.AddListener(RemoveCosmetic);

        float yOffset = 50f; // The vertical distance between each button
        float initialYPosition = 350f; // The starting Y position for the first button

        // Create the buttons dynamically
        for (int i = 0; i < cosmetics.Length; i++)
        {
            var cosmetic = cosmetics[i];

            GameObject _cosmeticButton = new GameObject("CosmeticButton_" + i, typeof(RectTransform));
            GameObject _cosmeticText = new GameObject("CosmeticText_" + i, typeof(RectTransform));

            _cosmeticButton.transform.SetParent(parent);
            _cosmeticText.transform.SetParent(_cosmeticButton.transform);

            Button _button = _cosmeticButton.AddComponent<Button>();
            TextMeshProUGUI _text = _cosmeticText.AddComponent<TextMeshProUGUI>();

            var colors = _button.colors;
            colors.highlightedColor = new Color32(255, 239, 100, 255);
            _button.colors = colors;
            _button.targetGraphic = _text;

            _text.text = cosmetic.Name;
            _text.font = font;

            // Set position of the button
            RectTransform buttonRectTransform = _cosmeticButton.GetComponent<RectTransform>();
            buttonRectTransform.sizeDelta = new Vector2(275, 40); // Set button size
            buttonRectTransform.anchoredPosition = new Vector2(0, initialYPosition - (yOffset * i)); // Position the button

            // Set position of the text to center within the button
            RectTransform textRectTransform = _cosmeticText.GetComponent<RectTransform>();
            textRectTransform.sizeDelta = buttonRectTransform.sizeDelta;
            textRectTransform.anchoredPosition = Vector2.zero; // Center text in button

            _button.onClick.AddListener(() => AddCosmetic(cosmetic.ID));
        }
    }

    private void Update()
    {
        RotatePlayer();
    }

    public void AddCosmetic(int id)
    {
        if (CheckForCosmetic()) return;
        GameObject _cosmetic = Instantiate(cosmetics[id].Cosmetic);
        _cosmetic.transform.parent = player.transform;
        _cosmetic.transform.localPosition = cosmetics[id].Offset;
        _cosmetic.transform.localEulerAngles = Vector3.zero;
    }

    private bool CheckForCosmetic()
    {
        for (int i = 0; i < player.transform.childCount; i++) 
        {
            if(player.transform.GetChild(i).CompareTag("Cosmetic")) return true;
        }
        return false;
    }

    private void RotatePlayer()
    {
        player.transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }

    private void RemoveCosmetic()
    {
        if(!CheckForCosmetic()) return;

        for (int i = 0; i < player.transform.childCount; i++)
        {
            var child = player.transform.GetChild(i);
            if (child.CompareTag("Cosmetic")) Destroy(child.gameObject);
        }
    }

    private void OnDisable()
    {
        if (player == null) return;

        player.transform.eulerAngles = new Vector3 (0, 180, 0);
    }
}
