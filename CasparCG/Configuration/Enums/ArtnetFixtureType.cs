namespace CasparLauncher.CasparCG.Configuration.Enums;

public enum ArtnetFixtureType
{
    [Display(Description = "ArtnetFixtureType_DIMMER", ResourceType = typeof(L))]
    _DIMMER,
    [Display(Description = "ArtnetFixtureType_RGB", ResourceType = typeof(L))]
    _RGB,
    [Display(Description = "ArtnetFixtureType_RGBW", ResourceType = typeof(L))]
    _RGBW
}
