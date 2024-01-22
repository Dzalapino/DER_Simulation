using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    private GameObject _selectedIndicator;
    private List<Hub> _hubs = new();

    // Main buttons
    private Button _buttonDisplayInfo;
    private Button _buttonCreateCity;

    // Display info popup elements
    private VisualElement _displayInfoPopup;
    private ListView _listViewEnergyStructures;
    private VisualTreeAsset _houseItemTemplate;
    private VisualTreeAsset _clusterItemTemplate;

    // Hub creation popup elements
    private VisualElement _createHubPopup;
    private SliderInt _sliderNumberOfHouses;
    private FloatField _floatFieldTargetEnergyConsumption;
    private FloatField _floatFieldTargetSolarProduction;
    private FloatField _floatFieldTargetWindProduction;
    private Button _buttonCancelCreateCity;
    private Button _buttonConfirmCreateCity;

    private void Start()
    {
        // Initialize the indicator prefab
        var prefab = Resources.Load<GameObject>("Prefabs/Indicator");
        _selectedIndicator = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        _selectedIndicator.SetActive(false);
        
        // Get the root of the UI document
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        // Init main buttons
        _buttonDisplayInfo = root.Q<Button>("DisplayInfo");
        _buttonCreateCity = root.Q<Button>("CreateHub");
        //_buttonCreateEnergyResource = root.Q<Button>("CreateEnergyResource");

        // Init display info popup elements
        _displayInfoPopup = root.Q<VisualElement>("DisplayInfoPopup");
        _listViewEnergyStructures = root.Q<ListView>("ListViewEnergyStructures");
        _displayInfoPopup.style.display = DisplayStyle.None;

        // Init city popup elements
        _createHubPopup = root.Q<VisualElement>("CreateHubPopup");
        _sliderNumberOfHouses = root.Q<SliderInt>("SliderNumberOfHouses");
        _sliderNumberOfHouses.label = $"Number of houses ({_sliderNumberOfHouses.value})";
        _floatFieldTargetEnergyConsumption = root.Q<FloatField>("TargetEnergyConsumption");
        _floatFieldTargetSolarProduction = root.Q<FloatField>("TargetSolarProduction");
        _floatFieldTargetWindProduction = root.Q<FloatField>("TargetWindProduction");
        _buttonCancelCreateCity = root.Q<Button>("CancelCreateHub");
        _buttonConfirmCreateCity = root.Q<Button>("ConfirmCreateHub");
        _createHubPopup.style.display = DisplayStyle.None;

        // Add callback for slider
        _sliderNumberOfHouses.RegisterValueChangedCallback(evt =>
        {
            _sliderNumberOfHouses.label = $"Number of houses ({evt.newValue})";
        });

        // Add desired events for buttons
        _buttonDisplayInfo.clicked += OnButtonDisplayInfoClicked;
        _buttonCreateCity.clicked += OnButtonCreateHub;
        _buttonCancelCreateCity.clicked += OnButtonCancelCreateHub;
        _buttonConfirmCreateCity.clicked += OnButtonConfirmCreateHub;
    }

    private bool IsPopupDisplayed(VisualElement popup)
    {
        return popup.style.display == DisplayStyle.Flex;
    }

    private void DisableAllPopups()
    {
        _selectedIndicator.SetActive(false);
        DisablePopup(_displayInfoPopup);
        DisablePopup(_createHubPopup);
    }
    
    private void DisablePopup(VisualElement popup)
    {
        popup.style.display = DisplayStyle.None;
    }
    
    private void EnablePopup(VisualElement popup)
    {
        DisableAllPopups();
        popup.style.display = DisplayStyle.Flex;
    }

    private void OnButtonDisplayInfoClicked()
    {
        if (IsPopupDisplayed(_displayInfoPopup))
        {
            _selectedIndicator.SetActive(false);
            DisablePopup(_displayInfoPopup);
        }
        else
        {
            EnablePopup(_displayInfoPopup);
            PopulateListView();
        }
    }

    private void OnButtonCreateHub()
    {
        if (IsPopupDisplayed(_createHubPopup))
        {
            DisablePopup(_createHubPopup);
        }
        else
        {
            EnablePopup(_createHubPopup);
        }
    }
    
    private void OnButtonCancelCreateHub()
    {
        DisablePopup(_createHubPopup);
    }
    
    private void OnButtonConfirmCreateHub()
    {
        _hubs.Add(
            new Hub(
                _sliderNumberOfHouses.value,
                _floatFieldTargetEnergyConsumption.value,
                _floatFieldTargetSolarProduction.value,
                _floatFieldTargetWindProduction.value
            )
        );
        DisablePopup(_createHubPopup);
    }
    
    private void PopulateListView()
    {
        // Clear existing items in the ListView
        _listViewEnergyStructures.Clear();
    
        // Create a list to store the data for the ListView
        List<string> dataList = new List<string>();
    
        // Populate the data list with house information
        foreach (var hub in _hubs)
        {
            if (hub.GetType() != typeof(City)) continue;

            foreach (var energyStructure in hub.City.EnergyStructures)
            {
                // var city = (City) energyStructureCluster;
                var house = energyStructure as House;
                
                // Customize this line to format the house information
                string houseInfo = $"{house.GetType().Name}\n" +
                                   $"Position: {house.Position}\n";
                dataList.Add(houseInfo);
            }
        }
    
        // Set up the ListView
        _listViewEnergyStructures.selectionChanged += OnSelectionChange;
        _listViewEnergyStructures.makeItem = MakeItem;
        _listViewEnergyStructures.bindItem = BindItem;
        _listViewEnergyStructures.fixedItemHeight = 60;
        _listViewEnergyStructures.itemsSource = dataList;
        _listViewEnergyStructures.Rebuild();
    }
    
    VisualElement MakeItem()
    {
        // Create a VisualElement representing your list item
        var listItem = new VisualElement();
        listItem.AddToClassList("text");

        // Add a label to represent house information
        var label = new Label();
        label.name = "HouseInfoLabel";
        listItem.Add(label);

        // Add the click event callback
        listItem.RegisterCallback<ClickEvent>(e => OnItemClick(listItem));

        return listItem;
    }
    
    void BindItem(VisualElement listItem, int index)
    {
        // Bind the data to the item
        string houseInfo = (string)_listViewEnergyStructures.itemsSource[index];
        listItem.Q<Label>().text = houseInfo;

        // Add additional logic to bind data to UI elements in the item
    }
    
    void OnItemClick(VisualElement listItem)
    {
        int selectedIndex = _listViewEnergyStructures.selectedIndex;
        if (selectedIndex >= 0 && selectedIndex < _hubs.First().City.EnergyStructures.Count)
        {
            House selectedHouse = _hubs.First().City.EnergyStructures[selectedIndex] as House;
            
            // Highlight the selected house's model (Assuming there is a Highlight method in your House class)
            if (selectedHouse != null)
            {
                _selectedIndicator.SetActive(true);
                _selectedIndicator.transform.position = selectedHouse.Position + Vector3.up * 3.5f;
            }

            // Add your custom logic here to handle the selected house
        }
    }

    void OnSelectionChange(IEnumerable<object> selectedItems)
    {
        // Handle selection change if needed
    }
}
