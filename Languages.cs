namespace CasparLauncher;

public enum Languages
{
    [Display(Description = "LanguagesDefault", ResourceType = typeof(L))]
    none,

    [Display(Description = "LanguagesEnglish", ResourceType = typeof(L))]
    en,

    [Display(Description = "LanguagesEspañol", ResourceType = typeof(L))]
    es,

    [Display(Description = "Languages简体中文", ResourceType = typeof(L))]
    zh_Hans
}