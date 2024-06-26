﻿using CarTuningConfigurator.Contorller;
using CarTuningConfigurator.DatabaseConnection;
using CarTuningConfigurator.Model;
using CarTuningConfigurator.View;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CarTuningConfigurator.View
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {

        public int horsePowerChangeTotal;
        public double brakePowerChangeTotal;
        public double weightChangeTotal;
        public double tractionChangeTotal;
        public int highspeedChangeTotal;
        public double accelerationChangeTotal;
        public double priceChangeTotal;

        TunningPart result = null;
        TunningPart brakesResult = null;
        TunningPart engineResult = null;
        TunningPart TurboResult = null;
        TunningPart WingResult = null;
        TunningPart TiresResult = null;
        TunningPart weightResult = null;
        TunningPart transmissionResult = null;
        TunningPart bodyResult = null;
        TunningPart chipResult = null;


        HomePageController controller = new HomePageController();
        UserModel UserModel = new UserModel();
        Car currentCar;
        List<Car> cars = new List<Car>();
        List<TunningPart> tunningParts = new List<TunningPart>();
        User theUser = null;
        List<TunningPart> tunnings = new List<TunningPart>();
        List<TunningPart> currentTuningPartsOfCurrentModdedCar = new List<TunningPart>();
        string currentUpgradePanel;
        string currentPanel;
        
        public Home(string username)
        {
            InitializeComponent();
            currentCar = new Car();  
            theUser = UserModel.searchUser(username);
            
            //
            // This Part renders Cars into the StandartCars View
            cars = controller.GetAllCars();
            tunningParts = controller.GetAllTunningParts();


            foreach (Car car in cars)
            {

                if (car.Modified == false)
                {
                    var binding = new Binding("LittleImage");
                    binding.Source = car;
                    var img = new Image();
                    img.HorizontalAlignment = HorizontalAlignment.Center;
                    img.VerticalAlignment = VerticalAlignment.Center;

                    var btn = new Button();
                    btn.Content = img;
                    btn.Height = 150;
                    btn.Width = 150;
                    btn.HorizontalAlignment = HorizontalAlignment.Center;
                    btn.VerticalAlignment = VerticalAlignment.Center;
                    btn.Margin = new Thickness(10);
                    btn.Click += (s, e) =>
                    { 
                        currentCar = car;
                        
                        int zIndex1 = Panel.GetZIndex(StandartCarsPanel);
                        int zIndex2 = Panel.GetZIndex(DetailviewOfCar);
                        Panel.SetZIndex(StandartCarsPanel, zIndex2);
                        Panel.SetZIndex(DetailviewOfCar, zIndex1);

                        var backgroundImagebinding = new Binding("Image");
                        backgroundImagebinding.Source = car;
                        DetailViewBackgroundImage.SetBinding(Image.SourceProperty, backgroundImagebinding);

                        var brandBinding = new Binding("Brand");
                        brandBinding.Source = car;
                        BrandLabel.SetBinding(Label.ContentProperty, brandBinding);

                        var modelBinding = new Binding("Model");
                        modelBinding.Source = car;
                        ModelLabel.SetBinding(Label.ContentProperty, modelBinding);

                        var hpBinding = new Binding("Horsepower");
                        hpBinding.Source = car;
                        var text = "Hallo";
                        HorsePowerLabel.SetBinding(Label.ContentProperty, hpBinding);
                        HorsePowerLabel.Content = "Horsepower: \n" + HorsePowerLabel.Content + "HP";

                        var bpBinding = new Binding("Breakpower");
                        bpBinding.Source = car;
                        BrakePowerLabel.SetBinding(Label.ContentProperty, bpBinding);
                        BrakePowerLabel.Content = "Breakpower: \n" + BrakePowerLabel.Content;

                        var weightBinding = new Binding("Weight");
                        weightBinding.Source = car;
                        WightLabel.SetBinding(Label.ContentProperty,  weightBinding );
                        WightLabel.Content = "Weight: \n" + WightLabel.Content + " Kg";

                        var tractionBinding = new Binding("Traction");
                        tractionBinding.Source = car;
                        TractionLabel.SetBinding(Label.ContentProperty,  tractionBinding);
                        TractionLabel.Content = "Traction: \n" + TractionLabel.Content;

                        var highspeedBinding = new Binding("Highspeed");
                        highspeedBinding.Source = car;
                        HighspeedLabel.SetBinding(Label.ContentProperty, highspeedBinding);
                        HighspeedLabel.Content = "Highspeed: \n" + HighspeedLabel.Content + " KM/H";

                        var accelerationBinding = new Binding("Acceleration");
                        accelerationBinding.Source = car;
                        AccelerationLabel.SetBinding(Label.ContentProperty,  accelerationBinding);
                        AccelerationLabel.Content = "Acceleration (0-100): \n" + AccelerationLabel.Content + "Sek";

                        var priceBinding = new Binding("Price");
                        priceBinding.Source = car;
                        PriceLabel.SetBinding(Label.ContentProperty, priceBinding);
                        PriceLabel.Content = "Price: \n" + PriceLabel.Content + "CHF";

                        saveCarBtn.Content = "SAVE";

                    };

                    img.SetBinding(Image.SourceProperty, binding);
                    img.Stretch = Stretch.UniformToFill;
                    HorizontalAlignment = HorizontalAlignment.Center;
                    VerticalAlignment = VerticalAlignment.Center;

                    WrapPanelForStandartCarsImages.Children.Add(btn);
                }
            }
            // End of Render
            //
            // This Part renders Cars into the TunedCars View
            if (theUser != null)
            {
                foreach (Car car in theUser.cars)
                {
                    if (car.Modified == true)
                    {
                        var binding = new Binding("LittleImage");
                        binding.Source = car;
                        var img = new Image();
                        img.HorizontalAlignment = HorizontalAlignment.Center;
                        img.VerticalAlignment = VerticalAlignment.Center;


                        currentTuningPartsOfCurrentModdedCar = car.tunningParts;
                        

                        var btn = new Button();
                        btn.Content = img;
                        btn.Height = 150;
                        btn.Width = 150;
                        btn.HorizontalAlignment = HorizontalAlignment.Center;
                        btn.VerticalAlignment = VerticalAlignment.Center;
                        btn.Margin = new Thickness(10);
                        btn.Click += (s, e) =>
                        {
                            currentCar = car;
                            RenderTuningParts(currentTuningPartsOfCurrentModdedCar);

                            int zIndex1 = Panel.GetZIndex(TunedCarsPanel);
                            int zIndex2 = Panel.GetZIndex(DetailviewOfCar);
                            Panel.SetZIndex(TunedCarsPanel, zIndex2);
                            Panel.SetZIndex(DetailviewOfCar, zIndex1);

                            var backgroundImagebinding = new Binding("Image");
                            backgroundImagebinding.Source = car;
                            DetailViewBackgroundImage.SetBinding(Image.SourceProperty, backgroundImagebinding);

                            var brandBinding = new Binding("Brand");
                            brandBinding.Source = car;
                            BrandLabel.SetBinding(Label.ContentProperty, brandBinding);

                            var modelBinding = new Binding("Model");
                            modelBinding.Source = car;
                            ModelLabel.SetBinding(Label.ContentProperty, modelBinding);

                            var hpBinding = new Binding("Horsepower");
                            hpBinding.Source = car;
                            HorsePowerLabel.SetBinding(Label.ContentProperty, "HorsePower: \n" + hpBinding);

                            var bpBinding = new Binding("Breakpower");
                            bpBinding.Source = car;
                            BrakePowerLabel.SetBinding(Label.ContentProperty, "Brake Power: \n" + bpBinding);

                            var weightBinding = new Binding("Weight");
                            weightBinding.Source = car;
                            WightLabel.SetBinding(Label.ContentProperty, "Weight: \n" + weightBinding);

                            var tractionBinding = new Binding("Traction");
                            tractionBinding.Source = car;
                            TractionLabel.SetBinding(Label.ContentProperty, "Traction: \n" + tractionBinding);

                            var highspeedBinding = new Binding("Highspeed");
                            highspeedBinding.Source = car;
                            HighspeedLabel.SetBinding(Label.ContentProperty, "Highspeed: \n" + highspeedBinding);

                            var accelerationBinding = new Binding("Acceleration");
                            accelerationBinding.Source = car;
                            AccelerationLabel.SetBinding(Label.ContentProperty, "Acceleration (0-100): \n" + accelerationBinding + " sek");

                            var priceBinding = new Binding("Price");
                            priceBinding.Source = car;
                            PriceLabel.SetBinding(Label.ContentProperty, "Price: \n" + priceBinding);

                            saveCarBtn.Content = "UPDATE";

                        };

                        img.SetBinding(Image.SourceProperty, binding);
                        img.Stretch = Stretch.UniformToFill;
                        HorizontalAlignment = HorizontalAlignment.Center;
                        VerticalAlignment = VerticalAlignment.Center;

                        WrapPanelForTunedCarsImages.Children.Add(btn);
                    }
                }
            }
            // End of Render           
        }

        private void RenderTuningParts(List<TunningPart> currentTuningPartsOfCurrentModdedCar)
        {
            foreach (var tuning in currentTuningPartsOfCurrentModdedCar) 
            {
                var category = tuning.Category;

                switch (category)
                {
                    case "Breaks":
                        brakesResult = tuning;
                        break;
                    case "Wing":
                        WingResult = tuning;
                        break ;
                    default: break;
                }

            }
        }

        private void ToStandartCars(object sender, RoutedEventArgs e)
        {
            int zIndex1 = Panel.GetZIndex(HomePanel);
            int zIndex2 = Panel.GetZIndex(StandartCarsPanel);
            Panel.SetZIndex(HomePanel, zIndex2);
            Panel.SetZIndex(StandartCarsPanel, zIndex1);

            currentPanel = "StandartCars";

        }

        

        private void StandartCarsToMainMenu(object sender, RoutedEventArgs e)
        {
            int zIndex1 = Panel.GetZIndex(StandartCarsPanel);
            int zIndex2 = Panel.GetZIndex(HomePanel);
            Panel.SetZIndex(StandartCarsPanel, zIndex2);
            Panel.SetZIndex(HomePanel, zIndex1);
        }

        private void ToTunedCars(object sender, RoutedEventArgs e)
        {
            int zIndex1 = Panel.GetZIndex(HomePanel);
            int zIndex2 = Panel.GetZIndex(TunedCarsPanel);
            Panel.SetZIndex(HomePanel, zIndex2);
            Panel.SetZIndex(TunedCarsPanel, zIndex1);

            currentPanel = "TunedCars";

        }

        private void TunedCarsToMainMenu(object sender, RoutedEventArgs e)
        {
            int zIndex1 = Panel.GetZIndex(TunedCarsPanel);
            int zIndex2 = Panel.GetZIndex(HomePanel);
            Panel.SetZIndex(TunedCarsPanel, zIndex2);
            Panel.SetZIndex(HomePanel, zIndex1);
        }

        private void ToDetailView(object sender, RoutedEventArgs e)
        {
            int zIndex1 = Panel.GetZIndex(DetailviewOfCar);
            int zIndex2 = Panel.GetZIndex(HomePanel);
            Panel.SetZIndex(TunedCarsPanel, zIndex2);
            Panel.SetZIndex(HomePanel, zIndex1);
        }

        private void ToSelectionOfCars(object sender, RoutedEventArgs e)
        {

            int zIndex1 = Panel.GetZIndex(DetailviewOfCar);
            int zIndex2 = Panel.GetZIndex(StandartCarsPanel);
            int zIndex3 = Panel.GetZIndex(TunedCarsPanel);


            if (currentPanel == "StandartCars")
            {
                Panel.SetZIndex(DetailviewOfCar, zIndex2);
                Panel.SetZIndex(StandartCarsPanel, zIndex1);

                result = null;
                brakesResult = null;
                engineResult = null;
                TurboResult = null;
                WingResult = null;
                TiresResult = null;
                weightResult = null;
                transmissionResult = null;
                bodyResult = null;
                chipResult = null;

            }
            else if (currentPanel == "TunedCars")
            {
                Panel.SetZIndex(DetailviewOfCar, zIndex2);
                Panel.SetZIndex(TunedCarsPanel, zIndex1);

            }
        }

        private void UpgradeBrakes(object sender, RoutedEventArgs e)
        {
            UpgradeBrake.Visibility = Visibility.Collapsed;
            UpgradesPanel.Visibility = Visibility.Visible;
            int zIndex1 = Panel.GetZIndex(UpgradeBrake);
            int zIndex2 = Panel.GetZIndex(UpgradesPanel);
            Panel.SetZIndex(UpgradesPanel, zIndex2);
            Panel.SetZIndex(UpgradeBrake, zIndex1);

            currentUpgradePanel = "Brakes";
            List<TunningPart> brakes = GetTunningParts("Breaks");
            LoadPanel(brakes);

        }

        private void UpgradeEngine(object sender, RoutedEventArgs e)
        {
            UpgradeBrake.Visibility = Visibility.Collapsed;
            UpgradesPanel.Visibility = Visibility.Visible;
            int zIndex1 = Panel.GetZIndex(UpgradeBrake);
            int zIndex2 = Panel.GetZIndex(UpgradesPanel);
            Panel.SetZIndex(UpgradesPanel, zIndex2);
            Panel.SetZIndex(UpgradeBrake, zIndex1);

            currentUpgradePanel = "Engine";
            List<TunningPart> engines = GetTunningParts("Engine");
            LoadPanel(engines);

        }

        private void UpgradeTurbo(object sender, RoutedEventArgs e)
        {
            UpgradeBrake.Visibility = Visibility.Collapsed;
            UpgradesPanel.Visibility = Visibility.Visible;
            int zIndex1 = Panel.GetZIndex(UpgradeBrake);
            int zIndex2 = Panel.GetZIndex(UpgradesPanel);
            Panel.SetZIndex(UpgradesPanel, zIndex2);
            Panel.SetZIndex(UpgradeBrake, zIndex1);

            currentUpgradePanel = "Turbo";
            List<TunningPart> turbos = GetTunningParts("Turbo");
            LoadPanel(turbos);

        }

        private void UpgradeTires(object sender, RoutedEventArgs e)
        {
            UpgradeBrake.Visibility = Visibility.Collapsed;
            UpgradesPanel.Visibility = Visibility.Visible;
            int zIndex1 = Panel.GetZIndex(UpgradeBrake);
            int zIndex2 = Panel.GetZIndex(UpgradesPanel);
            Panel.SetZIndex(UpgradesPanel, zIndex2);
            Panel.SetZIndex(UpgradeBrake, zIndex1);

            currentUpgradePanel = "Tires";
            List<TunningPart> tires = GetTunningParts("Tires");
            LoadPanel(tires);

        }

        private void UpgradeWing(object sender, RoutedEventArgs e)
        {
            UpgradeBrake.Visibility = Visibility.Collapsed;
            UpgradesPanel.Visibility = Visibility.Visible;
            int zIndex1 = Panel.GetZIndex(UpgradeBrake);
            int zIndex2 = Panel.GetZIndex(UpgradesPanel);
            Panel.SetZIndex(UpgradesPanel, zIndex2);
            Panel.SetZIndex(UpgradeBrake, zIndex1);

            currentUpgradePanel = "Wing";
            List<TunningPart> wings = GetTunningParts("Wing");
            LoadPanel(wings);

        }

        private void UpgradeTransmission(object sender, RoutedEventArgs e)
        {
            UpgradeBrake.Visibility = Visibility.Collapsed;
            UpgradesPanel.Visibility = Visibility.Visible;
            int zIndex1 = Panel.GetZIndex(UpgradeBrake);
            int zIndex2 = Panel.GetZIndex(UpgradesPanel);
            Panel.SetZIndex(UpgradesPanel, zIndex2);
            Panel.SetZIndex(UpgradeBrake, zIndex1);

            currentUpgradePanel = "Transmission";
            List<TunningPart> wings = GetTunningParts("Transmission");
            LoadPanel(wings);

        }

        private void UpgradeChip(object sender, RoutedEventArgs e)
        {
            UpgradeBrake.Visibility = Visibility.Collapsed;
            UpgradesPanel.Visibility = Visibility.Visible;
            int zIndex1 = Panel.GetZIndex(UpgradeBrake);
            int zIndex2 = Panel.GetZIndex(UpgradesPanel);
            Panel.SetZIndex(UpgradesPanel, zIndex2);
            Panel.SetZIndex(UpgradeBrake, zIndex1);

            currentUpgradePanel = "Chiptuning";
            List<TunningPart> wings = GetTunningParts("Chiptuning");
            LoadPanel(wings);

        }

        private void UpgradeBody(object sender, RoutedEventArgs e)
        {
            UpgradeBrake.Visibility = Visibility.Collapsed;
            UpgradesPanel.Visibility = Visibility.Visible;
            int zIndex1 = Panel.GetZIndex(UpgradeBrake);
            int zIndex2 = Panel.GetZIndex(UpgradesPanel);
            Panel.SetZIndex(UpgradesPanel, zIndex2);
            Panel.SetZIndex(UpgradeBrake, zIndex1);

            currentUpgradePanel = "Bodywork";
            List<TunningPart> wings = GetTunningParts("Bodywork");
            LoadPanel(wings);

        }

        private void WeightReduction(object sender, RoutedEventArgs e)
        {
            UpgradeBrake.Visibility = Visibility.Collapsed;
            UpgradesPanel.Visibility = Visibility.Visible;
            int zIndex1 = Panel.GetZIndex(UpgradeBrake);
            int zIndex2 = Panel.GetZIndex(UpgradesPanel);
            Panel.SetZIndex(UpgradesPanel, zIndex2);
            Panel.SetZIndex(UpgradeBrake, zIndex1);

            currentUpgradePanel = "WeightReduction";
            List<TunningPart> wings = GetTunningParts("WeightReduction");
            LoadPanel(wings);

        }

        private void GoBackToUpgradeSelection(object sender, RoutedEventArgs e)
        {
            UpgradesPanel.Visibility = Visibility.Collapsed;
            UpgradeBrake.Visibility = Visibility.Visible;
            int zIndex1 = Panel.GetZIndex(UpgradesPanel);
            int zIndex2 = Panel.GetZIndex(UpgradeBrake);
            Panel.SetZIndex(UpgradeBrake, zIndex2);
            Panel.SetZIndex(UpgradesPanel, zIndex1);

        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private List<TunningPart> GetTunningParts(string Category) 
        {
            List<TunningPart> parts = new List<TunningPart>();

            foreach (var part in tunningParts) 
            {
                if (part.Category == Category) 
                {
                    parts.Add(part);
                }
            }

            return parts;
        }

        private void LoadPanel(List<TunningPart> parts)
        {
            warppanelForTuningStages.Children.Clear();
            var value = BrakePowerLabel.Content;
            RadioButton rdbtn;
            

            foreach (TunningPart part in parts)
            {
                rdbtn = new RadioButton();
                var testRdbtn = new RadioButton();
                
                var breaksBinding = new Binding();
                breaksBinding.Source = part;
                rdbtn.Content = part.Name;
                result = checkSelectedPanel(currentUpgradePanel);
                if (result != null && rdbtn.Content.Equals(result.Name)) 
                {
                    rdbtn.IsChecked = true;
                }
                else
                {
                    rdbtn.IsChecked = false;
                } 
                rdbtn.Checked += (sender, e) =>
                {
                    result = part;
                    if (result.Category == "Wing" && WingResult != null)
                    {
                        tunnings.Remove(WingResult);
                    }
                    if (result.Category == "Engine" && engineResult != null)
                    {
                        tunnings.Remove(engineResult);
                    }
                    if (result.Category == "Breaks" && brakesResult != null)
                    {
                        tunnings.Remove(brakesResult);
                    }
                    if (result.Category == "Turbo" && TurboResult != null)
                    {
                        tunnings.Remove(TurboResult);
                    }
                    if (result.Category == "Tires" && TiresResult != null)
                    {
                        tunnings.Remove(TiresResult);
                    }
                    if (result.Category == "Transmission"  && transmissionResult != null) 
                    {
                        tunnings.Remove(transmissionResult);
                    }
                    if (result.Category == "Chiptuning" && chipResult != null)
                    {
                        tunnings.Remove(chipResult);
                    }
                    if (result.Category == "Bodywork" && bodyResult != null)
                    {
                        tunnings.Remove(bodyResult);
                    }
                    if (result.Category == "WeightReduction" && weightResult != null)
                    {
                        tunnings.Remove(weightResult);
                    }


                    setRightResult(result);

                    tunnings.Add(result);
                    controller.updateTunningPartfromCar(currentCar, tunnings);
                    horsePowerChangeTotal = 0;
                    brakePowerChangeTotal = 0;
                    tractionChangeTotal = 0;
                    weightChangeTotal = 0;
                    highspeedChangeTotal = 0;
                    accelerationChangeTotal = 0;
                    priceChangeTotal = 0;

                        foreach (var part in tunnings)
                        {      
                            horsePowerChangeTotal = horsePowerChangeTotal + part.ChangeOfHorsePower;
                            brakePowerChangeTotal = brakePowerChangeTotal + part.ChangeOfBrakeForce;
                            tractionChangeTotal = tractionChangeTotal + part.ChangeOfTraction;
                            weightChangeTotal = weightChangeTotal + part.ChangeOfWeight;
                            highspeedChangeTotal = highspeedChangeTotal + part.ChangeOfHighspeed;
                            accelerationChangeTotal = accelerationChangeTotal + part.ChangeOfAcceleration;
                            priceChangeTotal = priceChangeTotal + part.ChangeOfPrice;
                        }

                    if (currentPanel == "StandartCars")
                    {
                        HorsePowerLabel.Content = "HorsePower: \n" + currentCar.Horsepower + " + " + horsePowerChangeTotal + " = " + (currentCar.Horsepower + horsePowerChangeTotal);
                        BrakePowerLabel.Content = "Brake Force: \n" + currentCar.Brakeforce + " + " + brakePowerChangeTotal + " = " + (currentCar.Brakeforce + brakePowerChangeTotal);
                        TractionLabel.Content = "Traction: \n" + currentCar.Traction + " + " + tractionChangeTotal + " = " + (currentCar.Traction + tractionChangeTotal); 
                        WightLabel.Content = "Weight: \n" + currentCar.Weight + " + " + weightChangeTotal + " = " + (currentCar.Weight + weightChangeTotal); 
                        AccelerationLabel.Content = "Acceleration: \n" + currentCar.Acceleration + " + " + accelerationChangeTotal + " = " + (currentCar.Acceleration + accelerationChangeTotal); 
                        HighspeedLabel.Content = "Highspeed: \n" + currentCar.Highspeed + " + " + highspeedChangeTotal + " = " + (currentCar.Highspeed + highspeedChangeTotal);
                        PriceLabel.Content = "Price: \n" + currentCar.Price + " + " + priceChangeTotal + " = " + (currentCar.Price + priceChangeTotal);
                    } else if (currentPanel == "TunedCars") 
                    {
                        HorsePowerLabel.Content = "HorsePower: \n" + currentCar.Horsepower + horsePowerChangeTotal;
                        BrakePowerLabel.Content = "Brake Force: \n" + currentCar.Brakeforce  + brakePowerChangeTotal;
                        TractionLabel.Content = "Traction: \n" + currentCar.Traction  + tractionChangeTotal;
                        WightLabel.Content = "Weight: \n" + currentCar.Weight  + weightChangeTotal;
                        AccelerationLabel.Content = "Acceleration: \n" + currentCar.Acceleration  + accelerationChangeTotal;
                        HighspeedLabel.Content = "Highspeed: \n" + currentCar.Highspeed  + highspeedChangeTotal;
                        PriceLabel.Content = "Price: \n" + currentCar.Price  + priceChangeTotal;
                    }

                    

                };
                rdbtn.Unchecked += (sender, e) =>
                {
                    BrakePowerLabel.Content = value;
                };
                
                
                warppanelForTuningStages.Children.Add(rdbtn);
            }
        }

        private void setRightResult(TunningPart result)
        {
            switch(result.Category) 
            {
                case "Breaks":
                    brakesResult = result;
                    break;
                case "Wing":
                    WingResult = result;
                    break;
                case "Engine":
                    engineResult = result;
                    break;
                case "Turbo":
                    TurboResult = result;
                    break;
                case "Tires":
                    TiresResult = result;
                    break;
                case "Transmission":
                    transmissionResult = result;
                    break;
                case "WeightReuction":
                    weightResult = result;
                    break;
                case "Bodywork":
                    bodyResult = result;
                    break;
                case "chiptuning":
                    chipResult = result;
                    break;
                default:
                    MessageBox.Show("funktionagled doch nd");
                    break;
            }
        }

        private TunningPart checkSelectedPanel(string currentUpgradePanel)
        {

            switch (currentUpgradePanel)
            {
                case "Brakes":
                    result = brakesResult;
                    break;
                case "Wing":
                    result = WingResult;
                    break;
                case "Engine":
                    result = engineResult;
                    break;
                case "Turbo":
                    result = TurboResult;
                    break;
                case "Tires":
                    result = TiresResult;
                    break;
                case "Transmission":
                    result = transmissionResult;
                    break;
                case "WeightReuction":
                    result = weightResult ;
                    break;
                case "Bodywork":
                     result = bodyResult;
                    break;
                case "chiptuning":
                     result = chipResult;
                    break;
                default:
                    MessageBox.Show("Irgendeine Kategorie ist nicht richt eingeschrieben werden");
                    return null;
                    break;
            }

            return result;
        }

        private void Save_Car(object sender, RoutedEventArgs e)
        {
            if(currentCar.tunningParts != null)
            {
                if (currentCar.Modified == false)
                {
                    currentCar.Modified = true;

                    controller.saveCar(theUser, currentCar);
                    MessageBox.Show("Auto wurde gesaved");
                }
                else
                {
                    foreach (var user in controller.GetAllUsers()) 
                    { 
                        if (user.Username == theUser.Username) 
                        {
                            theUser = user;
                        }
                    }

                    controller.saveTunnedCar(theUser, currentCar);
                    MessageBox.Show("Auto wurde geupdated");
                }
            }
            else
            {
                MessageBox.Show("Das Auto muss ein Tunning Part enthalten");
            }

        }

    }

}