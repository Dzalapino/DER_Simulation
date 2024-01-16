using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    private GameObject _selectedIndicator;
    private List<EnergyStructureCluster> _energyStructureClusters = new();

    // Main buttons
    private Button _buttonDisplayInfo;
    private Button _buttonCreateCity;
    private Button _buttonCreateEnergyResource;

    // Display info popup elements
    private VisualElement _displayInfoPopup;
    private ListView _listViewEnergyStructures;
    private VisualTreeAsset _houseItemTemplate;
    private VisualTreeAsset _clusterItemTemplate;

    // City popup elements
    private VisualElement _createCityPopup;
    private SliderInt _sliderNumberOfHouses;
    private FloatField _floatFieldMinEnergyConsumption;
    private FloatField _floatFieldMaxEnergyConsumption;
    private Button _buttonCancelCreateCity;
    private Button _buttonConfirmCreateCity;
    
    // Energy source popup elements

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
        _buttonCreateCity = root.Q<Button>("CreateCity");
        _buttonCreateEnergyResource = root.Q<Button>("CreateEnergyResource");

        // Init display info popup elements
        _displayInfoPopup = root.Q<VisualElement>("DisplayInfoPopup");
        _listViewEnergyStructures = root.Q<ListView>("ListViewEnergyStructures");
        _displayInfoPopup.style.display = DisplayStyle.None;

        // Init city popup elements
        _createCityPopup = root.Q<VisualElement>("CreateCityPopup");
        _sliderNumberOfHouses = root.Q<SliderInt>("SliderNumberOfHouses");
        _sliderNumberOfHouses.label = $"Number of houses ({_sliderNumberOfHouses.value})";
        _floatFieldMinEnergyConsumption = root.Q<FloatField>("MinEnergyConsumption");
        _floatFieldMaxEnergyConsumption = root.Q<FloatField>("MaxEnergyConsumption");
        _buttonCancelCreateCity = root.Q<Button>("CancelCreateCity");
        _buttonConfirmCreateCity = root.Q<Button>("ConfirmCreateCity");
        _createCityPopup.style.display = DisplayStyle.None;

        // Add callback for slider
        _sliderNumberOfHouses.RegisterValueChangedCallback(evt =>
        {
            _sliderNumberOfHouses.label = $"Number of houses ({evt.newValue})";
        });

        // Add desired events for buttons
        _buttonDisplayInfo.clicked += OnButtonDisplayInfoClicked;
        _buttonCreateCity.clicked += OnButtonCreateCity;
        _buttonCancelCreateCity.clicked += OnButtonCancelCreateCity;
        _buttonConfirmCreateCity.clicked += OnButtonConfirmCreateCity;
    }

    private bool IsPopupDisplayed(VisualElement popup)
    {
        return popup.style.display == DisplayStyle.Flex;
    }

    private void DisableAllPopups()
    {
        _selectedIndicator.SetActive(false);
        DisablePopup(_displayInfoPopup);
        DisablePopup(_createCityPopup);
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

    private void OnButtonCreateCity()
    {
        if (IsPopupDisplayed(_createCityPopup))
        {
            DisablePopup(_createCityPopup);
        }
        else
        {
            EnablePopup(_createCityPopup);
        }
    }
    
    private void OnButtonCancelCreateCity()
    {
        DisablePopup(_createCityPopup);
    }
    
    private void OnButtonConfirmCreateCity()
    {
        _energyStructureClusters.Add(
            new City(
                new Vector3(0f, Constants.InitialHousePosition.y, 0f),
                _sliderNumberOfHouses.value,
                _floatFieldMinEnergyConsumption.value,
                _floatFieldMaxEnergyConsumption.value)
        );
        DisablePopup(_createCityPopup);
    }
    
    private void PopulateListView()
    {
        // Clear existing items in the ListView
        _listViewEnergyStructures.Clear();
    
        // Create a list to store the data for the ListView
        List<string> dataList = new List<string>();
    
        // Populate the data list with house information
        foreach (var energyStructureCluster in _energyStructureClusters)
        {
            if (energyStructureCluster.GetType() != typeof(City)) continue;

            foreach (var energyStructure in energyStructureCluster.EnergyStructures)
            {
                // var city = (City) energyStructureCluster;
                var house = energyStructure as House;
                
                // Customize this line to format the house information
                string houseInfo = $"{house.GetType().Name}\n" +
                                   $"Position: {house.Position}\n" +
                                   $"Consumption: {house.ConsumptionAmount}";
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
        if (selectedIndex >= 0 && selectedIndex < _energyStructureClusters.First().EnergyStructures.Count)
        {
            House selectedHouse = _energyStructureClusters.First().EnergyStructures[selectedIndex] as House;
            
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
