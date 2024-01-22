namespace CasparLauncher.CasparCG.Configuration.Enums;

public enum FfmpegDeinterlace
{
    [Display(Description = "FfmpegDeinterlace_None", ResourceType = typeof(L))]
    _none,
    [Display(Description = "FfmpegDeinterlace_Interlaced", ResourceType = typeof(L))]
    _interlaced,
    [Display(Description = "FfmpegDeinterlace_All", ResourceType = typeof(L))]
    _all
}