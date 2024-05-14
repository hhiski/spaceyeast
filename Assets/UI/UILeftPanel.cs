using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CelestialBody;

public class UILeftPanel : MonoBehaviour
{

    TextComponents textComponents = new TextComponents();

    public void UpdatePlanetConditionData(Planet planet)
    {
        UpdateGasPanel(planet);
        UpdateSurfacePanel(planet);
        UpdateGeneralDescPanel(planet);
        UpdateGasPanelPopText(planet);
    }

    public void UpdateSurfacePanel(Planet planet)
    {

        string surfaceText = planet.Lithosphere.GetSurfaceType().GetName();
        Text UISurfaceComposition = this.transform.Find("UISurfacePanel/UISurface/UISurfaceComposition").GetComponent<UnityEngine.UI.Text>();
        UISurfaceComposition.text = surfaceText;

        int metalLevel = planet.Lithosphere.GetSurfaceType().GetMetalLevel();
        int fissilesLevel = planet.Lithosphere.GetSurfaceType().GetRadioactivesLevel();
        int organicsLevel = planet.Lithosphere.GetSurfaceType().GetOrganicsLevel();
        int waterLevel = planet.Hydrosphere.GetWaterLevel();

        float metalPercentage = (metalLevel+1) * 0.1f;
        float fissilesPercentage = (fissilesLevel + 1) * 0.1f;
        float organicsPercentage = (organicsLevel + 1) * 0.1f;
        float waterPercentage = (waterLevel + 1) * 0.1f;

        Image UIWaterBar = this.transform.Find("UISurfacePanel/UISurface/UIWater/UIBar").gameObject.GetComponent<Image>();
        Image UIMetalBar = this.transform.Find("UISurfacePanel/UISurface/UIMetals/UIBar").gameObject.GetComponent<Image>();
        Image UIFissilessBar = this.transform.Find("UISurfacePanel/UISurface/UIFissiles/UIBar").gameObject.GetComponent<Image>();
        Image UIOrganicsBar = this.transform.Find("UISurfacePanel/UISurface/UIOrganics/UIBar").gameObject.GetComponent<Image>();

        UIWaterBar.fillAmount = waterPercentage;
        UIMetalBar.fillAmount = metalPercentage;
        UIFissilessBar.fillAmount = fissilesPercentage;
        UIOrganicsBar.fillAmount = organicsPercentage;

    }



    public void UpdateGeneralDescPanel(Planet planet)
    {
        TextComponent primaryTextComponent = textComponents.GetPrimaryGeneralDescription(planet); ;
        TextComponent secondaryTextComponent = textComponents.GetSecondaryGeneralDescription(planet); ;

        Text uiGeneralElementPrimary = this.transform.Find("UIGeneralDescPanel/UIGeneralPrimaryText/UIText").GetComponent<UnityEngine.UI.Text>();
        Text uiGeneralElementSecondary = this.transform.Find("UIGeneralDescPanel/UIGeneralSecundaryText/UIText").GetComponent<UnityEngine.UI.Text>();

        UIPopUp uiDescriptionPrimaryPanel = this.transform.Find("UIGeneralDescPanel/UIGeneralPrimaryText").GetComponent<UIPopUp>();
        UIPopUp uiDescriptionSecundaryPanel = this.transform.Find("UIGeneralDescPanel/UIGeneralSecundaryText").GetComponent<UIPopUp>();

        uiGeneralElementPrimary.text = "+ " + primaryTextComponent.Title;
        uiGeneralElementSecondary.text = "+ " + secondaryTextComponent.Title;

        uiDescriptionPrimaryPanel.Text = primaryTextComponent.Text;
        uiDescriptionSecundaryPanel.Text = secondaryTextComponent.Text;
    }

    void UpdateGasPanel(Planet planet)
    {
        Text UIGasCompositionText = this.transform.Find("UIGasPanel/UIGas/UIGasComposition").GetComponent<UnityEngine.UI.Text>();
        Text UIGasReactionText = this.transform.Find("UIGasPanel/UIGas/UIGasReaction").GetComponent<UnityEngine.UI.Text>();

        Debug.Log("PLANET  DATA:" + planet.Name);
        Debug.Log("PLANET  GAS :" + planet.Atm.PrimaryGas.GetGasName());

        string gasList = planet.Atm.PrimaryGas.GetGasName().ToString();
        string gasListParsed = gasList.Replace("-", "\n");

        string gasReaction = planet.Atm.PrimaryGas.GetGasReaction().ToString();

        if (planet.Atm.Temperature > 800 && planet.Atm.Pressure > 0.1f)
        {
            gasReaction = "Pyroclastic";
        }

        UIGasCompositionText.text = gasListParsed;
        UIGasReactionText.text = gasReaction;

        UpdateGasPanelNumbers(planet);
        UpdateGasPanelBars(planet);
    }

    void UpdateGasPanelNumbers(Planet planet)
    {
        Text UITemperature = this.transform.Find("UIGasPanel/UITemperature/UINum").GetComponent<UnityEngine.UI.Text>();
        Text UIPressure = this.transform.Find("UIGasPanel/UIPressure/UINum").GetComponent<UnityEngine.UI.Text>();
        Text UIRadiation = this.transform.Find("UIGasPanel/UIRadiation/UINum").GetComponent<UnityEngine.UI.Text>();

        string temperatureUnit = " °C";
        string pressureUnit = " bar";
        string radiationUnit = " Sv/y";

        float temperature = planet.Atm.TemperatureB + planet.Atm.TemperatureG;
        UITemperature.text = temperature.ToString("F0") + temperatureUnit;

        float pressure = planet.Atm.Pressure;

        if (pressure < 0.1f)
        {
            UIPressure.text = pressure.ToString("F2") + pressureUnit;
        }
        else if (pressure > 10f)
        {
            UIPressure.text = pressure.ToString("F0") + pressureUnit;
        }
        else
        {
            UIPressure.text = pressure.ToString("F1") + pressureUnit;
        }

        float radiation = planet.Atm.Radiation;
        if (radiation < 0.1f)
        {
            radiation *= 1000;
            radiationUnit = " mSv/y";
        }
        UIRadiation.text = radiation.ToString("F1") + radiationUnit;
    }

    void UpdateGasPanelBars(Planet planet)
    {
        float temperature = planet.Atm.TemperatureG + planet.Atm.TemperatureB;
        float temperatureBarLevel = PlanetConditions.GetTempIndicatorLevel(temperature);

        float pressure = planet.Atm.Pressure;
        float pressureBarLevel = PlanetConditions.GetPresIndicatorLevel(pressure);

        float radiation = planet.Atm.Radiation;
        float radiationBarLevel = PlanetConditions.GetRadIndicatorLevel(radiation);

        Image UITemperatureLevel = this.transform.Find("UIGasPanel/UITemperature/UIBar").gameObject.GetComponent<Image>();
        Image UIPressureLevel = this.transform.Find("UIGasPanel/UIPressure/UIBar").gameObject.GetComponent<Image>();
        Image UIRadiationLevel = this.transform.Find("UIGasPanel/UIRadiation/UIBar").gameObject.GetComponent<Image>();

        UITemperatureLevel.fillAmount = temperatureBarLevel;
        UIPressureLevel.fillAmount = pressureBarLevel;
        UIRadiationLevel.fillAmount = radiationBarLevel;


    }

    void UpdateGasPanelPopText(Planet planet)
    {
        float temperature = planet.Atm.TemperatureG + planet.Atm.TemperatureB;
        string temperatureText = PlanetConditions.GetTempLevelText(temperature);

        float pressure = planet.Atm.Pressure;
        string pressureText = PlanetConditions.GetPressLevelText(pressure);

        float radiation = planet.Atm.Radiation;
     //   string radiationText = PlanetConditions.GetRadLevelText(radiation);

        transform.Find("UIGasPanel/UITemperature").gameObject.GetComponent<UIPopUp>().Text = temperatureText;
        transform.Find("UIGasPanel/UIPressure").gameObject.GetComponent<UIPopUp>().Text = pressureText;
    }
    

}
