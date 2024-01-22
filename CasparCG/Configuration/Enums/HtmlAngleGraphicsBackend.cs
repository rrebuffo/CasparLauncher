namespace CasparLauncher.CasparCG.Configuration.Enums;

public enum HtmlAngleGraphicsBackend
{
    [Display(Description = "HtmlAngleGraphicsBackend_OpenGL", ResourceType = typeof(L))]
    _gl,
    [Display(Description = "HtmlAngleGraphicsBackend_D3D11", ResourceType = typeof(L))]
    _d3d11,
    [Display(Description = "HtmlAngleGraphicsBackend_D3D9", ResourceType = typeof(L))]
    _d3d9
}
