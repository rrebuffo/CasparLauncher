namespace CasparLauncher.CasparCG.Configuration.Enums;

public enum ScreenStretch
{
    [Display(Description = "ScreenStretch_None", ResourceType = typeof(L))]
    _none,
    [Display(Description = "ScreenStretch_Fill", ResourceType = typeof(L))]
    _fill,
    [Display(Description = "ScreenStretch_Uniform", ResourceType = typeof(L))]
    _uniform,
    [Display(Description = "ScreenStretch_UniformToFill", ResourceType = typeof(L))]
    _uniform_to_fill
}