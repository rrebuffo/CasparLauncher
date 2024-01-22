namespace CasparLauncher.CasparCG.Configuration.Enums;

public enum DecklinkKeyer
{
    [Display(Description = "DecklinkKeyer_External", ResourceType = typeof(L))]
    _external,
    [Display(Description = "DecklinkKeyer_ExternalSeparateDevice", ResourceType = typeof(L))]
    _external_separate_device,
    [Display(Description = "DecklinkKeyer_Internal", ResourceType = typeof(L))]
    _internal,
    [Display(Description = "DecklinkKeyer_Default", ResourceType = typeof(L))]
    _default
}