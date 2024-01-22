namespace CasparLauncher.CasparCG.Configuration.Enums;

public enum BluefishKeyer
{
    [Display(Description = "BluefishKeyer_External", ResourceType = typeof(L))]
    _external,
    [Display(Description = "BluefishKeyer_Internal", ResourceType = typeof(L))]
    _internal,
    [Display(Description = "BluefishKeyer_Disabled", ResourceType = typeof(L))]
    _disabled
}