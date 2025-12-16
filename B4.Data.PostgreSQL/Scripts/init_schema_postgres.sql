------------------------------------------------------------
-- ESQUEMA
------------------------------------------------------------
CREATE SCHEMA IF NOT EXISTS b4;

------------------------------------------------------------
-- TABLAS LK (MAESTRAS) - LkBase
-- createdat, updatedat, isactive
------------------------------------------------------------

-- LK_Ciclos
CREATE TABLE IF NOT EXISTS b4.lk_ciclos (
    id              SERIAL PRIMARY KEY,
    idciclo         INT NOT NULL,
    ciclo           VARCHAR(50) NOT NULL,
    descripcion     VARCHAR(50) NOT NULL,

    createdat       TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat       TIMESTAMP NOT NULL DEFAULT NOW(),
    isactive        BIGINT NOT NULL DEFAULT 1
);

-- LK_Fases
CREATE TABLE IF NOT EXISTS b4.lk_fases (
    idfase      INT PRIMARY KEY,
    fase        VARCHAR(50) NOT NULL,
    fasealias   VARCHAR(10),

    createdat   TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat   TIMESTAMP NOT NULL DEFAULT NOW(),
    isactive    BIGINT NOT NULL DEFAULT 1
);

-- LK_Plant_Country
CREATE TABLE IF NOT EXISTS b4.lk_plant_country (
    idcountry   INT PRIMARY KEY,
    country     VARCHAR(100) NOT NULL,

    createdat   TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat   TIMESTAMP NOT NULL DEFAULT NOW(),
    isactive    BIGINT NOT NULL DEFAULT 1
);

-- LK_Plant_Subdivision
CREATE TABLE IF NOT EXISTS b4.lk_plant_subdivision (
    idsubdivision INT PRIMARY KEY,
    subdivision   VARCHAR(100) NOT NULL,

    createdat     TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat     TIMESTAMP NOT NULL DEFAULT NOW(),
    isactive      BIGINT NOT NULL DEFAULT 1
);

-- LK_Plant_Division_Company
CREATE TABLE IF NOT EXISTS b4.lk_plant_division_company (
    iddivisioncompany INT PRIMARY KEY,
    divisioncompany   VARCHAR(100),

    createdat         TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat         TIMESTAMP NOT NULL DEFAULT NOW(),
    isactive          BIGINT NOT NULL DEFAULT 1
);

-- LK_Plant_Division
CREATE TABLE IF NOT EXISTS b4.lk_plant_division (
    iddivision INT PRIMARY KEY,
    division   VARCHAR(100) NOT NULL,

    createdat  TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat  TIMESTAMP NOT NULL DEFAULT NOW(),
    isactive   BIGINT NOT NULL DEFAULT 1
);

-- LK_Plant_Currency
CREATE TABLE IF NOT EXISTS b4.lk_plant_currency (
    idcurrency      INT PRIMARY KEY,
    currency        VARCHAR(50) NOT NULL,
    currencyalias   VARCHAR(10) NOT NULL,

    createdat       TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat       TIMESTAMP NOT NULL DEFAULT NOW(),
    isactive        BIGINT NOT NULL DEFAULT 1
);

-- LK_Plant_Tree
CREATE TABLE IF NOT EXISTS b4.lk_plant_tree (
    idtree              INT PRIMARY KEY,
    iddivision          INT NOT NULL,
    iddivisioncompany   INT NOT NULL,
    idsubdivision       INT NOT NULL,
    idcountry           INT NOT NULL,

    createdat           TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat           TIMESTAMP NOT NULL DEFAULT NOW(),
    isactive            BIGINT NOT NULL DEFAULT 1
);

-- LK_Plant_Company
CREATE TABLE IF NOT EXISTS b4.lk_plant_company (
    idcompany               INT PRIMARY KEY,
    companycode             VARCHAR(50) NOT NULL,
    managementcompany       VARCHAR(100) NOT NULL,
    idcurrency              INT NOT NULL,
    company                 VARCHAR(10) NOT NULL,
    active                  BOOLEAN,
    iddivision              INT NOT NULL,
    iddivisioncompany       INT NOT NULL,
    idsubdivision           INT NOT NULL,
    idcountry               INT NOT NULL,
    location                VARCHAR(150) NOT NULL,
    obs                     VARCHAR(4000),
    regionalvalidatorpwd    VARCHAR(50),
    region                  VARCHAR(150),
    regionalvalidator       VARCHAR(150),
    regionalvalidatoremail  VARCHAR(250),
    contributorpwd          VARCHAR(50),
    managementcompanybackup VARCHAR(100),
    regionbackup            VARCHAR(50),
    countrybackup           VARCHAR(100),
    idregvalidator          INT,

    createdat               TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat               TIMESTAMP NOT NULL DEFAULT NOW(),
    isactive                BIGINT NOT NULL DEFAULT 1
);

-- LK_Epigrafe
CREATE TABLE IF NOT EXISTS b4.lk_epigrafe (
    idepigrafe     INT PRIMARY KEY,
    idplantilla    INT NOT NULL,
    idhoja         INT NOT NULL,
    preepigrafe    VARCHAR(255),
    epigrafe       VARCHAR(255) NOT NULL,
    epigrafefull   VARCHAR(255) NOT NULL,

    createdat      TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat      TIMESTAMP NOT NULL DEFAULT NOW(),
    isactive       BIGINT NOT NULL DEFAULT 1
);

-- LK_Plant_Controllers
CREATE TABLE IF NOT EXISTS b4.lk_plant_controllers
(
    idcompanycontroller INT PRIMARY KEY,
    idcompany           INT NOT NULL,
    controller          VARCHAR(200) NOT NULL,
    email               VARCHAR(200) NOT NULL,

    createdat           TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat           TIMESTAMP NOT NULL DEFAULT NOW(),
    isactive            BIGINT NOT NULL DEFAULT 1
);

-- LK_Plantillas_Botones_Pasos_Tipos
CREATE TABLE IF NOT EXISTS b4.lk_plantillas_botones_pasos_tipos
(
    idpasotipo      INT PRIMARY KEY,
    pasotipo        VARCHAR(50) NOT NULL,
    descripcion     VARCHAR(500),

    createdat       TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat       TIMESTAMP NOT NULL DEFAULT NOW(),
    isactive        BIGINT NOT NULL DEFAULT 1
);

------------------------------------------------------------
-- TABLAS DATA FINANCIERAS (DataBaseFinanciero + DataBase)
-- data_actuals, data_budget, data_forecast
------------------------------------------------------------

CREATE TABLE IF NOT EXISTS b4.data_actuals (
    id              SERIAL PRIMARY KEY,
    idapicarga      INT NOT NULL,
    guidcarga       UUID NOT NULL,
    fechaultmodif   TIMESTAMP NOT NULL DEFAULT NOW(),
    idcompany       INT NOT NULL,
    ejercicio       INT NOT NULL,
    idciclo         INT NOT NULL,
    idfase          INT NOT NULL,
    idcurrency      INT NOT NULL,
    idepigrafe      INT NOT NULL,

    mes00 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes01 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes02 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes03 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes04 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes05 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes06 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes07 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes08 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes09 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes10 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes11 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes12 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes13 NUMERIC(18,5) NOT NULL DEFAULT 0,

    idcarga         INT,
    idcargastgbw    INT,
    idhoja          INT,

    createdat       TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat       TIMESTAMP NOT NULL DEFAULT NOW(),
    version         INT NOT NULL DEFAULT 1,
    checksum        INT,
    iszero          INT NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS b4.data_budget (
    id              SERIAL PRIMARY KEY,
    idapicarga      INT NOT NULL,
    guidcarga       UUID NOT NULL,
    fechaultmodif   TIMESTAMP NOT NULL DEFAULT NOW(),
    idcompany       INT NOT NULL,
    ejercicio       INT NOT NULL,
    idciclo         INT NOT NULL,
    idfase          INT NOT NULL,
    idcurrency      INT NOT NULL,
    idepigrafe      INT NOT NULL,

    mes00 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes01 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes02 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes03 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes04 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes05 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes06 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes07 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes08 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes09 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes10 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes11 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes12 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes13 NUMERIC(18,5) NOT NULL DEFAULT 0,

    idcarga         INT,
    idcargastgbw    INT,
    idhoja          INT,

    createdat       TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat       TIMESTAMP NOT NULL DEFAULT NOW(),
    version         INT NOT NULL DEFAULT 1,
    checksum        INT,
    iszero          INT NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS b4.data_forecast (
    id              SERIAL PRIMARY KEY,
    idapicarga      INT NOT NULL,
    guidcarga       UUID NOT NULL,
    fechaultmodif   TIMESTAMP NOT NULL DEFAULT NOW(),
    idcompany       INT NOT NULL,
    ejercicio       INT NOT NULL,
    idciclo         INT NOT NULL,
    idfase          INT NOT NULL,
    idcurrency      INT NOT NULL,
    idepigrafe      INT NOT NULL,

    mes00 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes01 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes02 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes03 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes04 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes05 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes06 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes07 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes08 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes09 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes10 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes11 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes12 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes13 NUMERIC(18,5) NOT NULL DEFAULT 0,

    idcarga         INT,
    idcargastgbw    INT,
    idhoja          INT,

    createdat       TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat       TIMESTAMP NOT NULL DEFAULT NOW(),
    version         INT NOT NULL DEFAULT 1,
    checksum        INT,
    iszero          INT NOT NULL DEFAULT 0
);

------------------------------------------------------------
-- TABLAS DATA FINANCIERAS BW
-- data_actuals_bw, data_budget_bw, data_forecast_bw
------------------------------------------------------------

CREATE TABLE IF NOT EXISTS b4.data_actuals_bw (
    id              SERIAL PRIMARY KEY,
    idapicarga      INT NOT NULL,
    guidcarga       UUID NOT NULL,
    fechaultmodif   TIMESTAMP NOT NULL DEFAULT NOW(),
    idcompany       INT NOT NULL,
    ejercicio       INT NOT NULL,
    idciclo         INT NOT NULL,
    idfase          INT NOT NULL,
    idcurrency      INT NOT NULL,
    idepigrafe      INT NOT NULL,

    mes00 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes01 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes02 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes03 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes04 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes05 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes06 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes07 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes08 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes09 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes10 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes11 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes12 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes13 NUMERIC(18,5) NOT NULL DEFAULT 0,

    idcarga         INT,
    idcargastgbw    INT,
    idhoja          INT,

    createdat       TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat       TIMESTAMP NOT NULL DEFAULT NOW(),
    version         INT NOT NULL DEFAULT 1,
    checksum        INT,
    iszero          INT NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS b4.data_budget_bw (
    id              SERIAL PRIMARY KEY,
    idapicarga      INT NOT NULL,
    guidcarga       UUID NOT NULL,
    fechaultmodif   TIMESTAMP NOT NULL DEFAULT NOW(),
    idcompany       INT NOT NULL,
    ejercicio       INT NOT NULL,
    idciclo         INT NOT NULL,
    idfase          INT NOT NULL,
    idcurrency      INT NOT NULL,
    idepigrafe      INT NOT NULL,

    mes00 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes01 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes02 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes03 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes04 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes05 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes06 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes07 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes08 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes09 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes10 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes11 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes12 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes13 NUMERIC(18,5) NOT NULL DEFAULT 0,

    idcarga         INT,
    idcargastgbw    INT,
    idhoja          INT,

    createdat       TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat       TIMESTAMP NOT NULL DEFAULT NOW(),
    version         INT NOT NULL DEFAULT 1,
    checksum        INT,
    iszero          INT NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS b4.data_forecast_bw (
    id              SERIAL PRIMARY KEY,
    idapicarga      INT NOT NULL,
    guidcarga       UUID NOT NULL,
    fechaultmodif   TIMESTAMP NOT NULL DEFAULT NOW(),
    idcompany       INT NOT NULL,
    ejercicio       INT NOT NULL,
    idciclo         INT NOT NULL,
    idfase          INT NOT NULL,
    idcurrency      INT NOT NULL,
    idepigrafe      INT NOT NULL,

    mes00 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes01 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes02 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes03 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes04 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes05 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes06 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes07 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes08 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes09 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes10 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes11 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes12 NUMERIC(18,5) NOT NULL DEFAULT 0,
    mes13 NUMERIC(18,5) NOT NULL DEFAULT 0,

    idcarga         INT,
    idcargastgbw    INT,
    idhoja          INT,

    createdat       TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat       TIMESTAMP NOT NULL DEFAULT NOW(),
    version         INT NOT NULL DEFAULT 1,
    checksum        INT,
    iszero          INT NOT NULL DEFAULT 0
);

------------------------------------------------------------
-- DATA_Comentarios
------------------------------------------------------------

CREATE TABLE IF NOT EXISTS b4.data_comentarios (
    id              SERIAL PRIMARY KEY,
    idapicarga      INT NOT NULL,
    guidcarga       UUID NOT NULL,
    fechaultmodif   TIMESTAMP,
    idcompany       INT NOT NULL,
    ejercicio       INT NOT NULL,
    idciclo         INT NOT NULL,
    idfase          INT NOT NULL,
    idepigrafe      INT NOT NULL,
    etiqueta        VARCHAR(50) NOT NULL,
    comentario      TEXT,

    createdat       TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat       TIMESTAMP NOT NULL DEFAULT NOW(),
    version         INT NOT NULL DEFAULT 1,
    checksum        INT,
    iszero          INT NOT NULL DEFAULT 0
);

------------------------------------------------------------
-- DATA_Tipo_Cambio
------------------------------------------------------------

CREATE TABLE IF NOT EXISTS b4.data_tipo_cambio (
    id              SERIAL PRIMARY KEY,
    idapicarga      INT NOT NULL,
    guidcarga       UUID NOT NULL,
    fechaultmodif   TIMESTAMP NOT NULL,
    ejercicio       INT NOT NULL,
    idcurrency      INT NOT NULL,
    calendarday     DATE NOT NULL,
    mes             INT NOT NULL,
    p               NUMERIC(18,5),
    fc              NUMERIC(18,5),
    fb              NUMERIC(18,5),

    idcarga         INT,
    idcargastgbw    INT,
    idhoja          INT,

    createdat       TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat       TIMESTAMP NOT NULL DEFAULT NOW(),
    version         INT NOT NULL DEFAULT 1,
    checksum        INT,
    iszero          INT NOT NULL DEFAULT 0
);

------------------------------------------------------------
-- DATA_BRIDGES FY / FY_BW / FY_BW_EUR
-- Basado en tu script MySQL y repos actuales
------------------------------------------------------------

CREATE TABLE IF NOT EXISTS b4.data_bridges_fy (
    id                  SERIAL PRIMARY KEY,
    idapicarga          INT NOT NULL,
    guidcarga           UUID,
    fechaultmodif       TIMESTAMP NOT NULL,
    idcompany           INT NOT NULL,
    ejercicio           INT NOT NULL,
    idciclo             INT NOT NULL,
    idfase              INT NOT NULL,
    idcurrency          INT NOT NULL,
    idepigrafe          INT NOT NULL,

    fiscalyear          NUMERIC(18,5) NOT NULL DEFAULT 0,
    percentage          NUMERIC(18,5) NOT NULL DEFAULT 0,
    zero                NUMERIC(18,5) NOT NULL DEFAULT 0,
    zeropercentage      NUMERIC(18,5) NOT NULL DEFAULT 0,
    absolute            NUMERIC(18,5) NOT NULL DEFAULT 0,
    absolutepercentage  NUMERIC(18,5) NOT NULL DEFAULT 0,
    vmixnew             NUMERIC(18,5) NOT NULL DEFAULT 0,
    rawmaterial         NUMERIC(18,5) NOT NULL DEFAULT 0,
    scrap               NUMERIC(18,5) NOT NULL DEFAULT 0,
    economics           NUMERIC(18,5) NOT NULL DEFAULT 0,
    currencymix         NUMERIC(18,5) NOT NULL DEFAULT 0,
    performance         NUMERIC(18,5) NOT NULL DEFAULT 0,
    prototool           NUMERIC(18,5) NOT NULL DEFAULT 0,
    others              NUMERIC(18,5) NOT NULL DEFAULT 0,
    comments            VARCHAR(4000),

    createdat           TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat           TIMESTAMP NOT NULL DEFAULT NOW(),
    version             INT NOT NULL DEFAULT 1,
    checksum            INT,
    iszero              INT NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS b4.data_bridges_fy_bw (
    id                  SERIAL PRIMARY KEY,
    idapicarga          INT NOT NULL,
    guidcarga           UUID,
    fechaultmodif       TIMESTAMP NOT NULL,
    idcompany           INT NOT NULL,
    ejercicio           INT NOT NULL,
    idciclo             INT NOT NULL,
    idfase              INT NOT NULL,
    idcurrency          INT NOT NULL,
    idepigrafe          INT NOT NULL,

    fiscalyear          NUMERIC(18,5) NOT NULL DEFAULT 0,
    percentage          NUMERIC(18,5) NOT NULL DEFAULT 0,
    zero                NUMERIC(18,5) NOT NULL DEFAULT 0,
    zeropercentage      NUMERIC(18,5) NOT NULL DEFAULT 0,
    absolute            NUMERIC(18,5) NOT NULL DEFAULT 0,
    absolutepercentage  NUMERIC(18,5) NOT NULL DEFAULT 0,
    vmixnew             NUMERIC(18,5) NOT NULL DEFAULT 0,
    rawmaterial         NUMERIC(18,5) NOT NULL DEFAULT 0,
    scrap               NUMERIC(18,5) NOT NULL DEFAULT 0,
    economics           NUMERIC(18,5) NOT NULL DEFAULT 0,
    currencymix         NUMERIC(18,5) NOT NULL DEFAULT 0,
    performance         NUMERIC(18,5) NOT NULL DEFAULT 0,
    prototool           NUMERIC(18,5) NOT NULL DEFAULT 0,
    others              NUMERIC(18,5) NOT NULL DEFAULT 0,
    comments            VARCHAR(4000),

    idcarga             INT,
    idcargastgbw        INT,
    idhoja              INT,

    createdat           TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat           TIMESTAMP NOT NULL DEFAULT NOW(),
    version             INT NOT NULL DEFAULT 1,
    checksum            INT,
    iszero              INT NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS b4.data_bridges_fy_bw_eur (
    id                  SERIAL PRIMARY KEY,
    idapicarga          INT NOT NULL,
    guidcarga           UUID,
    fechaultmodif       TIMESTAMP NOT NULL,
    idcompany           INT NOT NULL,
    ejercicio           INT NOT NULL,
    idciclo             INT NOT NULL,
    idfase              INT NOT NULL,
    idcurrency          INT NOT NULL,
    idepigrafe          INT NOT NULL,

    actuals             NUMERIC(18,5) NOT NULL DEFAULT 0,
    pctactuals          NUMERIC(18,5) NOT NULL DEFAULT 0,
    budget              NUMERIC(18,5) NOT NULL DEFAULT 0,
    pctbudget           NUMERIC(18,5) NOT NULL DEFAULT 0,
    variance            NUMERIC(18,5) NOT NULL DEFAULT 0,
    volume              NUMERIC(18,5) NOT NULL DEFAULT 0,
    inventorychange     NUMERIC(18,5) NOT NULL DEFAULT 0,
    mix                 NUMERIC(18,5) NOT NULL DEFAULT 0,
    new                 NUMERIC(18,5) NOT NULL DEFAULT 0,
    economics           NUMERIC(18,5) NOT NULL DEFAULT 0,
    quicksavings        NUMERIC(18,5) NOT NULL DEFAULT 0,
    currencymix         NUMERIC(18,5) NOT NULL DEFAULT 0,
    exchangerate        NUMERIC(18,5) NOT NULL DEFAULT 0,
    rawmaterial         NUMERIC(18,5) NOT NULL DEFAULT 0,
    scrap               NUMERIC(18,5) NOT NULL DEFAULT 0,
    industrialperformance NUMERIC(18,5) NOT NULL DEFAULT 0,
    prototooling        NUMERIC(18,5) NOT NULL DEFAULT 0,
    other               NUMERIC(18,5) NOT NULL DEFAULT 0,
    "check"             NUMERIC(18,5) NOT NULL DEFAULT 0,
    comments            VARCHAR(4000),

    idcarga             INT,
    idcargastgbw        INT,
    idhoja              INT,

    createdat           TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat           TIMESTAMP NOT NULL DEFAULT NOW(),
    version             INT NOT NULL DEFAULT 1,
    checksum            INT,
    iszero              INT NOT NULL DEFAULT 0
);

------------------------------------------------------------
-- DATA_BRIDGES MONTH / MONTH_BW (estructura larga MySQL)
------------------------------------------------------------

CREATE TABLE IF NOT EXISTS b4.data_bridges_month (
    id                      SERIAL PRIMARY KEY,
    idapicarga              INT NOT NULL,
    guidcarga               UUID,
    fechaultmodif           TIMESTAMP NOT NULL,
    idcompany               INT NOT NULL,
    ejercicio               INT NOT NULL,
    idciclo                 INT NOT NULL,
    idfase                  INT NOT NULL,
    idcurrency              INT NOT NULL,
    idepigrafe              INT NOT NULL,

    actuals                 NUMERIC(18,5) NOT NULL DEFAULT 0,
    pctactuals              NUMERIC(18,5) NOT NULL DEFAULT 0,
    budget                  NUMERIC(18,5) NOT NULL DEFAULT 0,
    pctbudget               NUMERIC(18,5) NOT NULL DEFAULT 0,
    variance                NUMERIC(18,5) NOT NULL DEFAULT 0,
    volume                  NUMERIC(18,5) NOT NULL DEFAULT 0,
    inventorychange         NUMERIC(18,5) NOT NULL DEFAULT 0,
    mix                     NUMERIC(18,5) NOT NULL DEFAULT 0,
    new                     NUMERIC(18,5) NOT NULL DEFAULT 0,
    economicsinflation      NUMERIC(18,5) NOT NULL DEFAULT 0,
    economicsinflationclaim NUMERIC(18,5) NOT NULL DEFAULT 0,
    economicspricingissues  NUMERIC(18,5) NOT NULL DEFAULT 0,
    economicsltas           NUMERIC(18,5) NOT NULL DEFAULT 0,
    economicsbps            NUMERIC(18,5) NOT NULL DEFAULT 0,
    economicscomponenteffect NUMERIC(18,5) NOT NULL DEFAULT 0,
    stockvaluation          NUMERIC(18,5) NOT NULL DEFAULT 0,
    quicksavings            NUMERIC(18,5) NOT NULL DEFAULT 0,
    currencymix             NUMERIC(18,5) NOT NULL DEFAULT 0,
    exchangerate            NUMERIC(18,5) NOT NULL DEFAULT 0,
    rawmaterial             NUMERIC(18,5) NOT NULL DEFAULT 0,
    scrap                   NUMERIC(18,5) NOT NULL DEFAULT 0,
    performancegsd          NUMERIC(18,5) NOT NULL DEFAULT 0,
    performancequality      NUMERIC(18,5) NOT NULL DEFAULT 0,
    performanceother        NUMERIC(18,5) NOT NULL DEFAULT 0,
    performancelaunchingcost NUMERIC(18,5) NOT NULL DEFAULT 0,
    prototooling            NUMERIC(18,5) NOT NULL DEFAULT 0,
    accruralothers          NUMERIC(18,5) NOT NULL DEFAULT 0,
    onetimeeffectothers     NUMERIC(18,5) NOT NULL DEFAULT 0,
    other                   NUMERIC(18,5) NOT NULL DEFAULT 0,
    comments                VARCHAR(4000),

    createdat               TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat               TIMESTAMP NOT NULL DEFAULT NOW(),
    version                 INT NOT NULL DEFAULT 1,
    checksum                INT,
    iszero                  INT NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS b4.data_bridges_month_bw (
    id                      SERIAL PRIMARY KEY,
    idapicarga              INT NOT NULL,
    guidcarga               UUID,
    fechaultmodif           TIMESTAMP NOT NULL,
    idcompany               INT NOT NULL,
    ejercicio               INT NOT NULL,
    idciclo                 INT NOT NULL,
    idfase                  INT NOT NULL,
    idcurrency              INT NOT NULL,
    idepigrafe              INT NOT NULL,

    volume                  NUMERIC(18,5) NOT NULL DEFAULT 0,
    inventorychange         NUMERIC(18,5) NOT NULL DEFAULT 0,
    mix                     NUMERIC(18,5) NOT NULL DEFAULT 0,
    new                     NUMERIC(18,5) NOT NULL DEFAULT 0,
    economics               NUMERIC(18,5) NOT NULL DEFAULT 0,
    quicksavings            NUMERIC(18,5) NOT NULL DEFAULT 0,
    currencymix             NUMERIC(18,5) NOT NULL DEFAULT 0,
    exchangerate            NUMERIC(18,5) NOT NULL DEFAULT 0,
    rawmaterial             NUMERIC(18,5) NOT NULL DEFAULT 0,
    scrap                   NUMERIC(18,5) NOT NULL DEFAULT 0,
    industrialperformance   NUMERIC(18,5) NOT NULL DEFAULT 0,
    prototooling            NUMERIC(18,5) NOT NULL DEFAULT 0,
    other                   NUMERIC(18,5) NOT NULL DEFAULT 0,
    "check"                 NUMERIC(18,5) NOT NULL DEFAULT 0,
    comments                VARCHAR(4000),

    idcarga                 INT,
    idcargastgbw            INT,
    idhoja                  INT,

    createdat               TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat               TIMESTAMP NOT NULL DEFAULT NOW(),
    version                 INT NOT NULL DEFAULT 1,
    checksum                INT,
    iszero                  INT NOT NULL DEFAULT 0
);

------------------------------------------------------------
-- TABLAS STG (simplificadas, sin campos de auditoría extra)
------------------------------------------------------------

CREATE TABLE IF NOT EXISTS b4.stg_data_actuals (
    id              SERIAL PRIMARY KEY,
    idapicarga      INT NOT NULL,
    guidcarga       UUID NOT NULL,
    fechaultmodif   TIMESTAMP NOT NULL,
    idcompany       INT NOT NULL,
    ejercicio       INT NOT NULL,
    idciclo         INT NOT NULL,
    idfase          INT NOT NULL,
    idcurrency      INT NOT NULL,
    idepigrafe      INT NOT NULL,
    mes00 NUMERIC(18,5) NOT NULL,
    mes01 NUMERIC(18,5) NOT NULL,
    mes02 NUMERIC(18,5) NOT NULL,
    mes03 NUMERIC(18,5) NOT NULL,
    mes04 NUMERIC(18,5) NOT NULL,
    mes05 NUMERIC(18,5) NOT NULL,
    mes06 NUMERIC(18,5) NOT NULL,
    mes07 NUMERIC(18,5) NOT NULL,
    mes08 NUMERIC(18,5) NOT NULL,
    mes09 NUMERIC(18,5) NOT NULL,
    mes10 NUMERIC(18,5) NOT NULL,
    mes11 NUMERIC(18,5) NOT NULL,
    mes12 NUMERIC(18,5) NOT NULL,
    mes13 NUMERIC(18,5) NOT NULL,

    createdat       TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat       TIMESTAMP NOT NULL DEFAULT NOW(),
    version         INT NOT NULL DEFAULT 1,
    checksum        INT,
    iszero          INT NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS b4.stg_data_budget (
    id              SERIAL PRIMARY KEY,
    idapicarga      INT NOT NULL,
    guidcarga       UUID NOT NULL,
    fechaultmodif   TIMESTAMP NOT NULL,
    idcompany       INT NOT NULL,
    ejercicio       INT NOT NULL,
    idciclo         INT NOT NULL,
    idfase          INT NOT NULL,
    idcurrency      INT NOT NULL,
    idepigrafe      INT NOT NULL,
    mes00 NUMERIC(18,5) NOT NULL,
    mes01 NUMERIC(18,5) NOT NULL,
    mes02 NUMERIC(18,5) NOT NULL,
    mes03 NUMERIC(18,5) NOT NULL,
    mes04 NUMERIC(18,5) NOT NULL,
    mes05 NUMERIC(18,5) NOT NULL,
    mes06 NUMERIC(18,5) NOT NULL,
    mes07 NUMERIC(18,5) NOT NULL,
    mes08 NUMERIC(18,5) NOT NULL,
    mes09 NUMERIC(18,5) NOT NULL,
    mes10 NUMERIC(18,5) NOT NULL,
    mes11 NUMERIC(18,5) NOT NULL,
    mes12 NUMERIC(18,5) NOT NULL,
    mes13 NUMERIC(18,5) NOT NULL,

    createdat       TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat       TIMESTAMP NOT NULL DEFAULT NOW(),
    version         INT NOT NULL DEFAULT 1,
    checksum        INT,
    iszero          INT NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS b4.stg_data_forecast (
    id              SERIAL PRIMARY KEY,
    idapicarga      INT NOT NULL,
    guidcarga       UUID NOT NULL,
    fechaultmodif   TIMESTAMP,
    idcompany       INT NOT NULL,
    ejercicio       INT NOT NULL,
    idciclo         INT NOT NULL,
    idfase          INT NOT NULL,
    idcurrency      INT NOT NULL,
    idepigrafe      INT NOT NULL,
    mes00 NUMERIC(18,5) NOT NULL,
    mes01 NUMERIC(18,5) NOT NULL,
    mes02 NUMERIC(18,5) NOT NULL,
    mes03 NUMERIC(18,5) NOT NULL,
    mes04 NUMERIC(18,5) NOT NULL,
    mes05 NUMERIC(18,5) NOT NULL,
    mes06 NUMERIC(18,5) NOT NULL,
    mes07 NUMERIC(18,5) NOT NULL,
    mes08 NUMERIC(18,5) NOT NULL,
    mes09 NUMERIC(18,5) NOT NULL,
    mes10 NUMERIC(18,5) NOT NULL,
    mes11 NUMERIC(18,5) NOT NULL,
    mes12 NUMERIC(18,5) NOT NULL,
    mes13 NUMERIC(18,5) NOT NULL,

    createdat       TIMESTAMP NOT NULL DEFAULT NOW(),
    updatedat       TIMESTAMP NOT NULL DEFAULT NOW(),
    version         INT NOT NULL DEFAULT 1,
    checksum        INT,
    iszero          INT NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS b4.stg_data_comentarios (
    id              SERIAL PRIMARY KEY,
    idapicarga      INT NOT NULL,
    guidcarga       UUID NOT NULL,
    fechaultmodif   TIMESTAMP,
    idcompany       INT NOT NULL,
    ejercicio       INT NOT NULL,
    idciclo         INT NOT NULL,
    idfase          INT NOT NULL,
    idepigrafe      INT NOT NULL,
    etiqueta        VARCHAR(50) NOT NULL,
    comentario      TEXT
);

CREATE TABLE IF NOT EXISTS b4.stg_data_bridges_fy (
    id                  SERIAL PRIMARY KEY,
    idapicarga          INT NOT NULL,
    guidcarga           UUID,
    fechaultmodif       TIMESTAMP NOT NULL,
    idcompany           INT NOT NULL,
    ejercicio           INT NOT NULL,
    idciclo             INT NOT NULL,
    idfase              INT NOT NULL,
    idcurrency          INT NOT NULL,
    idepigrafe          INT NOT NULL,
    fiscalyear          NUMERIC(18,5) NOT NULL,
    percentage          NUMERIC(18,5) NOT NULL,
    zero                NUMERIC(18,5) NOT NULL,
    zeropercentage      NUMERIC(18,5) NOT NULL,
    absolute            NUMERIC(18,5) NOT NULL,
    absolutepercentage  NUMERIC(18,5) NOT NULL,
    vmixnew             NUMERIC(18,5) NOT NULL,
    rawmaterial         NUMERIC(18,5) NOT NULL,
    scrap               NUMERIC(18,5) NOT NULL,
    economics           NUMERIC(18,5) NOT NULL,
    currencymix         NUMERIC(18,5) NOT NULL,
    performance         NUMERIC(18,5) NOT NULL,
    prototool           NUMERIC(18,5) NOT NULL,
    others              NUMERIC(18,5) NOT NULL,
    comments            VARCHAR(4000)
);

CREATE TABLE IF NOT EXISTS b4.stg_data_bridges_month (
    id                      SERIAL PRIMARY KEY,
    idapicarga              INT NOT NULL,
    guidcarga               UUID,
    fechaultmodif           TIMESTAMP NOT NULL,
    idcompany               INT NOT NULL,
    ejercicio               INT NOT NULL,
    idciclo                 INT NOT NULL,
    idfase                  INT NOT NULL,
    idcurrency              INT NOT NULL,
    idepigrafe              INT NOT NULL,
    actuals                 NUMERIC(18,5) NOT NULL,
    pctactuals              NUMERIC(18,5) NOT NULL,
    budget                  NUMERIC(18,5) NOT NULL,
    pctbudget               NUMERIC(18,5) NOT NULL,
    variance                NUMERIC(18,5) NOT NULL,
    volume                  NUMERIC(18,5) NOT NULL,
    inventorychange         NUMERIC(18,5) NOT NULL,
    mix                     NUMERIC(18,5) NOT NULL,
    new                     NUMERIC(18,5) NOT NULL,
    economicsinflation      NUMERIC(18,5) NOT NULL,
    economicsinflationclaim NUMERIC(18,5) NOT NULL,
    economicspricingissues  NUMERIC(18,5) NOT NULL,
    economicsltas           NUMERIC(18,5) NOT NULL,
    economicsbps            NUMERIC(18,5) NOT NULL,
    economicscomponenteffect NUMERIC(18,5) NOT NULL,
    stockvaluation          NUMERIC(18,5) NOT NULL,
    quicksavings            NUMERIC(18,5) NOT NULL,
    currencymix             NUMERIC(18,5) NOT NULL,
    exchangerate            NUMERIC(18,5) NOT NULL,
    rawmaterial             NUMERIC(18,5) NOT NULL,
    scrap                   NUMERIC(18,5) NOT NULL,
    performancegsd          NUMERIC(18,5) NOT NULL,
    performancequality      NUMERIC(18,5) NOT NULL,
    performanceother        NUMERIC(18,5) NOT NULL,
    performancelaunchingcost NUMERIC(18,5) NOT NULL,
    prototooling            NUMERIC(18,5) NOT NULL,
    accruralothers          NUMERIC(18,5) NOT NULL,
    onetimeeffectothers     NUMERIC(18,5) NOT NULL,
    other                   NUMERIC(18,5) NOT NULL,
    comments                VARCHAR(4000)
);

------------------------------------------------------------
-- FIN
------------------------------------------------------------
