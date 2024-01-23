using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    public Camera mainCamera;
    
    private List<Hub> _hubs = new();
    private Hub _selectedHub = null;

    // Main buttons
    private Button _buttonSimulateDailyCycle;
    private Button _buttonDisplayInfo;
    private Button _buttonCreateCity;
    private Button _buttonReset;

    // Display info popup elements
    private VisualElement _displayInfoPopup;
    private ListView _listView;
    
    // Daily info popup elements
    private VisualElement _dailyInfoPopup;
    private ListView _dailyInfoListView;

    // Hub creation popup elements
    private VisualElement _createHubPopup;
    private SliderInt _sliderNumberOfHouses;
    private FloatField _floatFieldTargetEnergyConsumption;
    private FloatField _floatFieldTargetSolarProduction;
    private FloatField _floatFieldTargetWindProduction;
    private FloatField _floatFieldStorageSize;
    private Button _buttonCancelCreateCity;
    private Button _buttonConfirmCreateCity;
    
    // Move hub popup elements
    private VisualElement _moveHubPopup;
    private Button _buttonMoveLeft;
    private Button _buttonMoveRight;
    private Button _buttonMoveUp;
    private Button _buttonMoveDown;
    private Button _buttonConfirmMoveHub;

    private void Start()
    {
        mainCamera = mainCamera == null ? Camera.main : mainCamera;
        
        // Get the root of the UI document
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        // Init main buttons
        _buttonSimulateDailyCycle = root.Q<Button>("SimulateDailyCycle");
        _buttonDisplayInfo = root.Q<Button>("DisplayInfo");
        _buttonCreateCity = root.Q<Button>("CreateHub");
        _buttonReset = root.Q<Button>("Reset");
        _buttonSimulateDailyCycle.style.display = DisplayStyle.None;

        // Create hub popup elements
        _createHubPopup = root.Q<VisualElement>("CreateHubPopup");
        _sliderNumberOfHouses = root.Q<SliderInt>("SliderNumberOfHouses");
        _sliderNumberOfHouses.label = $"Number of houses ({_sliderNumberOfHouses.value})";
        _floatFieldTargetEnergyConsumption = root.Q<FloatField>("TargetEnergyConsumption");
        _floatFieldTargetSolarProduction = root.Q<FloatField>("TargetSolarProduction");
        _floatFieldTargetWindProduction = root.Q<FloatField>("TargetWindProduction");
        _floatFieldStorageSize = root.Q<FloatField>("StorageSize");
        _buttonCancelCreateCity = root.Q<Button>("CancelCreateHub");
        _buttonConfirmCreateCity = root.Q<Button>("ConfirmCreateHub");

        // Init move hub popup elements
        _moveHubPopup = root.Q<VisualElement>("MoveHubPopup");
        _buttonMoveLeft = root.Q<Button>("MoveLeft");
        _buttonMoveRight = root.Q<Button>("MoveRight");
        _buttonMoveUp = root.Q<Button>("MoveUp");
        _buttonMoveDown = root.Q<Button>("MoveDown");
        _buttonConfirmMoveHub = root.Q<Button>("ConfirmMoving");
        
        // Init display info popup elements
        _displayInfoPopup = root.Q<VisualElement>("DisplayInfoPopup");
        _listView = root.Q<ListView>("ListViewEnergyStructures");
        
        // Init daily info popup elements
        _dailyInfoPopup = root.Q<VisualElement>("DailyInfoPopup");
        _dailyInfoListView = root.Q<ListView>("ListViewDailyInfo");
        
        DisableAllPopups();

        // Add callback for slider
        _sliderNumberOfHouses.RegisterValueChangedCallback(evt =>
        {
            _sliderNumberOfHouses.label = $"Number of houses ({evt.newValue})";
        });

        // Add desired events for buttons
        _buttonSimulateDailyCycle.clicked += OnButtonSimulateDailyCycleClicked;
        _buttonDisplayInfo.clicked += OnButtonDisplayInfoClicked;
        _buttonCreateCity.clicked += OnButtonCreateHub;
        _buttonReset.clicked += OnButtonReset;
        _buttonCancelCreateCity.clicked += OnButtonCancelCreateHub;
        _buttonConfirmCreateCity.clicked += OnButtonConfirmCreateHub;
        _buttonMoveLeft.clicked += OnButtonMoveLeft;
        _buttonMoveRight.clicked += OnButtonMoveRight;
        _buttonMoveUp.clicked += OnButtonMoveUp;
        _buttonMoveDown.clicked += OnButtonMoveDown;
        _buttonConfirmMoveHub.clicked += OnButtonConfirmMoveHub;
    }

    private bool IsPopupDisplayed(VisualElement popup)
    {
        return popup.style.display == DisplayStyle.Flex;
    }

    private void DisableAllPopups()
    {
        DisablePopup(_displayInfoPopup);
        DisablePopup(_createHubPopup);
        DisablePopup(_moveHubPopup);
        DisablePopup(_dailyInfoPopup);
        _buttonSimulateDailyCycle.style.display = DisplayStyle.None;
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
    
    private void OnButtonSimulateDailyCycleClicked()
    {
        _buttonSimulateDailyCycle.style.display = DisplayStyle.None;
        PopulateDailyInfoListView();
        EnablePopup(_dailyInfoPopup);
    }

    private void OnButtonDisplayInfoClicked()
    {
        if (IsPopupDisplayed(_displayInfoPopup))
        {
            DisablePopup(_displayInfoPopup);
        }
        else
        {
            EnablePopup(_displayInfoPopup);
            PopulateHubInfoListView();
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
            _floatFieldTargetEnergyConsumption.SetValueWithoutNotify(0f);
            _floatFieldTargetSolarProduction.SetValueWithoutNotify(0f);
            _floatFieldTargetWindProduction.SetValueWithoutNotify(0f);
            _floatFieldStorageSize.SetValueWithoutNotify(0f);
        }
    }

    private void OnButtonReset()
    {
        DisableAllPopups();
        _hubs.ForEach(hub => hub.DestroyHub());
        _hubs.Clear();
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
                _floatFieldTargetWindProduction.value,
                _floatFieldStorageSize.value
            )
        );
        EnablePopup(_moveHubPopup);
    }

    private void OnButtonMoveLeft()
    {
        _hubs.Last().MoveHub(Vector3.left * Constants.HouseSeparation);
    }
    
    private void OnButtonMoveRight()
    {
        _hubs.Last().MoveHub(Vector3.right * Constants.HouseSeparation);
    }
    
    private void OnButtonMoveDown()
    {
        _hubs.Last().MoveHub(Vector3.back * Constants.HouseSeparation);
    }
    
    private void OnButtonMoveUp()
    {
        _hubs.Last().MoveHub(Vector3.forward * Constants.HouseSeparation);
    }
    
    private void OnButtonConfirmMoveHub()
    {
        DisablePopup(_moveHubPopup);
    }
    
    private void PopulateHubInfoListView()
    {
        // Clear existing items in the ListView
        _listView.Clear();
    
        // Create a list to store the data for the ListView
        List<string> dataList = new List<string>();
    
        // Populate the data list with house information
        int i = 0;
        foreach (var hub in _hubs)
        {
            string hubInfo = $"Hub {i++} on position {hub.Position}\n" +
                             $"City consumption: {hub.City.TargetEnergyConsumption} MWh\n" +
                             $"Solar production: {hub.SolarFarm.TargetEnergyProduction} MWh\n" +
                             $"Wind production: {hub.WindFarm.TargetEnergyProduction} MWh\n" +
                             $"Storage size: {hub.EnergyStorageSize} MWh\n" +
                             $"Storage filled to {hub.CurrentEnergyStoragePercentage}%";
            dataList.Add(hubInfo);
        }
    
        // Set up the ListView
        _listView.makeItem = MakeHubInfoListItem;
        _listView.bindItem = BindHubInfoListItem;
        _listView.itemsSource = dataList;
        _listView.Rebuild();
    }
    
    private void PopulateDailyInfoListView()
    {
        // Clear existing items in the ListView
        _dailyInfoListView.Clear();
        // Initialize the data list
        var dataList = new List<string> { _selectedHub.GenerateDailyInfo() };
        
        // Bind the data list to the ListView
        _dailyInfoListView.itemsSource = dataList;
        
        // Optionally, you can define a template for each item in the ListView
        _dailyInfoListView.makeItem = () =>
        {
             var label = new Label();
             label.AddToClassList("text");
             return label;
        };
        _dailyInfoListView.bindItem = (element, index) => (element as Label).text = dataList[index];
        _dailyInfoListView.Rebuild();
    }
    
    VisualElement MakeHubInfoListItem()
    {
        // Create a VisualElement representing list item
        var listItem = new VisualElement();
        listItem.AddToClassList("text");

        // Add a label to represent house information
        var label = new Label();
        label.name = "HubInfoLabel";
        listItem.Add(label);
        
        // Add the click event callback
        listItem.RegisterCallback<ClickEvent>(e => OnHubInfoItemClick(listItem));

        return listItem;
    }
    
    void BindHubInfoListItem(VisualElement listItem, int index)
    {
        // Bind the data to the item
        string itemInfo = (string)_listView.itemsSource[index];
        listItem.Q<Label>().text = itemInfo;

        // Add additional logic to bind data to UI elements in the item
    }
    
    void OnHubInfoItemClick(VisualElement listItem)
    {
        int selectedIndex = _listView.selectedIndex;
        if (selectedIndex >= 0 && selectedIndex < _hubs.Count)
        {
            Hub selectedHub = _hubs[selectedIndex];
            
            // Highlight the selected house's model (Assuming there is a Highlight method in your House class)
            if (selectedHub != null)
            {
                _selectedHub = selectedHub;
                _buttonSimulateDailyCycle.style.display = DisplayStyle.Flex;
                
                // Set the camera position to the selected house
                mainCamera.transform.position = new Vector3(selectedHub.Position.x, mainCamera.transform.position.y, selectedHub.Position.z);
            }
        }
    }
}
