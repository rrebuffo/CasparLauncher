namespace CasparLauncher.CasparCG.Configuration.Enums;

public enum SystemAudioChannelLayout
{
    [Display(Description = "SystemAudioChannelLayout_Mono", ResourceType = typeof(L))]
    _mono,
    [Display(Description = "SystemAudioChannelLayout_Stereo", ResourceType = typeof(L))]
    _stereo,
    [Display(Description = "SystemAudioChannelLayout_Matrix", ResourceType = typeof(L))]
    _matrix
}