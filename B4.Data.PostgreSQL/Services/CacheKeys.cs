namespace B4.Data.PostgreSQL.Services;

public static class CacheKeys
{
    // Por convención: 2 versiones por tabla -> all / active
    public const string LkCiclosAll = "lk:ciclos:all";
    public const string LkCiclosActive = "lk:ciclos:active";

    public const string LkFasesAll = "lk:fases:all";
    public const string LkFasesActive = "lk:fases:active";

    public const string LkEpigrafesAll = "lk:epigrafes:all";
    public const string LkEpigrafesActive = "lk:epigrafes:active";

    public const string LkPlantCountryAll = "lk:plant_country:all";
    public const string LkPlantCountryActive = "lk:plant_country:active";

    public const string LkPlantCurrencyAll = "lk:plant_currency:all";
    public const string LkPlantCurrencyActive = "lk:plant_currency:active";

    public const string LkPlantDivisionAll = "lk:plant_division:all";
    public const string LkPlantDivisionActive = "lk:plant_division:active";

    public const string LkPlantDivisionCompanyAll = "lk:plant_division_company:all";
    public const string LkPlantDivisionCompanyActive = "lk:plant_division_company:active";

    public const string LkPlantSubdivisionAll = "lk:plant_subdivision:all";
    public const string LkPlantSubdivisionActive = "lk:plant_subdivision:active";

    public const string LkPlantTreeAll = "lk:plant_tree:all";
    public const string LkPlantTreeActive = "lk:plant_tree:active";

    public const string LkPlantCompanyAll = "lk:plant_company:all";
    public const string LkPlantCompanyActive = "lk:plant_company:active";

    public const string LkPlantControllersAll = "lk:plant_controllers:all";
    public const string LkPlantControllersActive = "lk:plant_controllers:active";

    public const string LkPlantillasBotonesPasosTiposAll = "lk:plantillas_botones_pasos_tipos:all";
    public const string LkPlantillasBotonesPasosTiposActive = "lk:plantillas_botones_pasos_tipos:active";

    // Lista única de todas las keys (para invalidar en bloque)
    public static readonly string[] AllKeys =
    {
        LkCiclosAll, LkCiclosActive,
        LkFasesAll, LkFasesActive,
        LkEpigrafesAll, LkEpigrafesActive,
        LkPlantCountryAll, LkPlantCountryActive,
        LkPlantCurrencyAll, LkPlantCurrencyActive,
        LkPlantDivisionAll, LkPlantDivisionActive,
        LkPlantDivisionCompanyAll, LkPlantDivisionCompanyActive,
        LkPlantSubdivisionAll, LkPlantSubdivisionActive,
        LkPlantTreeAll, LkPlantTreeActive,
        LkPlantCompanyAll, LkPlantCompanyActive,
        LkPlantControllersAll, LkPlantControllersActive,
        LkPlantillasBotonesPasosTiposAll, LkPlantillasBotonesPasosTiposActive
    };
}
