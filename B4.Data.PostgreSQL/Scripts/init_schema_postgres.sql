-- Desactivamos chequeos temporales (para importar más rápido)
SET client_min_messages TO WARNING;

-- ==================================================
-- TABLA CONTROL
-- ==================================================
CREATE TABLE control (
    idcontrol SERIAL PRIMARY KEY,
    anyo INT NOT NULL,
    idciclo INT NOT NULL,
    idfasecontrol INT NOT NULL,
    inicio TIMESTAMP NOT NULL,
    final TIMESTAMP NOT NULL,
    activo BOOLEAN DEFAULT FALSE,
    addinfo VARCHAR(4000),
    sendautemail BOOLEAN DEFAULT FALSE,
    bwreportsmandatory BOOLEAN DEFAULT TRUE
);

-- ==================================================
-- TABLA CONTROL_PLANTA
-- ==================================================
CREATE TABLE control_planta (
    idcontrolplanta SERIAL PRIMARY KEY,
    idcontrol INT NOT NULL,
    idcompany INT NOT NULL
);

-- ==================================================
-- TABLA LK_CICLOS
-- ==================================================
CREATE TABLE lk_ciclos (
    id SERIAL PRIMARY KEY,
    idciclo INT NOT NULL,
    ciclo VARCHAR(50) NOT NULL,
    descripcion VARCHAR(50) NOT NULL
);

-- ==================================================
-- TABLA LK_FASES
-- ==================================================
CREATE TABLE lk_fases (
    idfase INT PRIMARY KEY,
    fase VARCHAR(50) NOT NULL,
    fasealias VARCHAR(10)
);

-- ==================================================
-- TABLA LK_PLANT_CURRENCY
-- ==================================================
CREATE TABLE lk_plant_currency (
    idcurrency INT PRIMARY KEY,
    currency VARCHAR(50) NOT NULL,
    currencyalias VARCHAR(10) NOT NULL
);

-- ==================================================
-- TABLA LK_PLANT_DIVISION
-- ==================================================
CREATE TABLE lk_plant_division (
    iddivision INT PRIMARY KEY,
    division VARCHAR(100) NOT NULL
);

-- ==================================================
-- TABLA LK_PLANT_DIVISION_COMPANY
-- ==================================================
CREATE TABLE lk_plant_division_company (
    iddivisioncompany INT PRIMARY KEY,
    divisioncompany VARCHAR(100)
);

-- ==================================================
-- TABLA LK_PLANT_SUBDIVISION
-- ==================================================
CREATE TABLE lk_plant_subdivision (
    idsubdivision INT PRIMARY KEY,
    subdivision VARCHAR(100) NOT NULL
);

-- ==================================================
-- TABLA LK_PLANT_COUNTRY
-- ==================================================
CREATE TABLE lk_plant_country (
    idcountry INT PRIMARY KEY,
    country VARCHAR(100) NOT NULL
);

-- ==================================================
-- TABLA LK_PLANT_TREE
-- ==================================================
CREATE TABLE lk_plant_tree (
    idtree INT PRIMARY KEY,
    iddivision INT NOT NULL,
    iddivisioncompany INT NOT NULL,
    idsubdivision INT NOT NULL,
    idcountry INT NOT NULL
);

-- ==================================================
-- TABLA LK_PLANT_COMPANY
-- ==================================================
CREATE TABLE lk_plant_company (
    idcompany INT PRIMARY KEY,
    companycode VARCHAR(50) NOT NULL,
    managementcompany VARCHAR(100) NOT NULL,
    idcurrency INT NOT NULL,
    company VARCHAR(10) NOT NULL,
    active BOOLEAN DEFAULT TRUE,
    iddivision INT NOT NULL,
    iddivisioncompany INT NOT NULL,
    idsubdivision INT NOT NULL,
    idcountry INT NOT NULL,
    location VARCHAR(150) NOT NULL,
    obs VARCHAR(4000),
    regionalvalidatorpwd VARCHAR(50),
    region VARCHAR(150),
    regionalvalidator VARCHAR(150),
    regionalvalidatoremail VARCHAR(250),
    contributorpwd VARCHAR(50),
    managementcompanybackup VARCHAR(100),
    regionbackup VARCHAR(50),
    countrybackup VARCHAR(100),
    idregvalidator INT
);

-- ==================================================
-- TABLA LK_EPIGRAFES
-- ==================================================
CREATE TABLE lk_epigrafes (
    idepigrafe INT PRIMARY KEY,
    idplantilla INT NOT NULL,
    idhoja INT NOT NULL,
    preepigrafe VARCHAR(255),
    epigrafe VARCHAR(255) NOT NULL,
    epigrafefull VARCHAR(255) NOT NULL
);

-- ==================================================
-- TABLA PLANTILLAS
-- ==================================================
CREATE TABLE plantillas (
    idplantilla INT PRIMARY KEY,
    plantilla VARCHAR(50) NOT NULL
);

-- ==================================================
-- TABLA PLANTILLAS_VERSIONES
-- ==================================================
CREATE TABLE plantillas_versiones (
    idversion INT PRIMARY KEY,
    version VARCHAR(50) NOT NULL,
    idplantilla INT NOT NULL,
    activa BOOLEAN DEFAULT FALSE,
    modificada TIMESTAMP,
    fechaactivacion TIMESTAMP,
    api_version VARCHAR(50),
    adminweb_version VARCHAR(50),
    ejercicio INT
);

-- ==================================================
-- TABLA EMAILS_AUT
-- ==================================================
CREATE TABLE emails_aut (
    idemail INT PRIMARY KEY,
    mail_when VARCHAR(1000),
    send_to VARCHAR(1000),
    send_cc VARCHAR(1000),
    send_bcc VARCHAR(1000),
    subject VARCHAR(500),
    body VARCHAR(4000),
    linktosendautemail BOOLEAN DEFAULT FALSE,
    test VARCHAR(100)
);

-- ==================================================
-- TABLA LOG_ACTIVIDAD
-- ==================================================
CREATE TABLE log_actividad (
    id SERIAL PRIMARY KEY,
    timestamp TIMESTAMP DEFAULT NOW(),
    nivel VARCHAR(20),
    modulo VARCHAR(100),
    entrada VARCHAR(2000),
    idficheroexcel INT,
    nombreficheroexcel VARCHAR(255),
    idcarga INT DEFAULT 0,
    dcr_id_job INT DEFAULT 0
);

-- ==================================================
-- VISTA V_API_CONTROL
-- ==================================================
CREATE OR REPLACE VIEW v_api_control AS
SELECT
    c.idcontrol,
    c.anyo,
    c.idciclo,
    lkc.ciclo AS ciclo,
    lkc.descripcion AS descciclo,
    c.inicio,
    c.final,
    c.idfasecontrol AS idfase,
    lf.fasealias AS codfase,
    lf.fase AS descfase,
    c.activo
FROM control c
LEFT JOIN lk_ciclos lkc ON c.idciclo = lkc.idciclo
LEFT JOIN lk_fases lf ON c.idfasecontrol = lf.idfase;

-- ==================================================
-- TABLAS DE DATOS PRINCIPALES (DATA_*)
-- ==================================================

-- DATA_Actuals
CREATE TABLE data_actuals (
    id SERIAL PRIMARY KEY,
    idapicarga INT NOT NULL,
    guidcarga UUID NOT NULL,
    fechaultmodif TIMESTAMP NOT NULL DEFAULT NOW(),
    idcompany INT NOT NULL,
    ejercicio INT NOT NULL,
    idciclo INT NOT NULL,
    idfase INT NOT NULL,
    idcurrency INT NOT NULL,
    idepigrafe INT NOT NULL,
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
    mes13 NUMERIC(18,5) NOT NULL DEFAULT 0
);

-- DATA_Budget
CREATE TABLE data_budget (
    id SERIAL PRIMARY KEY,
    idapicarga INT NOT NULL,
    guidcarga UUID NOT NULL,
    fechaultmodif TIMESTAMP NOT NULL DEFAULT NOW(),
    idcompany INT NOT NULL,
    ejercicio INT NOT NULL,
    idciclo INT NOT NULL,
    idfase INT NOT NULL,
    idcurrency INT NOT NULL,
    idepigrafe INT NOT NULL,
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
    mes13 NUMERIC(18,5) NOT NULL DEFAULT 0
);

-- DATA_Forecast
CREATE TABLE data_forecast (
    id SERIAL PRIMARY KEY,
    idapicarga INT NOT NULL,
    guidcarga UUID NOT NULL,
    fechaultmodif TIMESTAMP NOT NULL DEFAULT NOW(),
    idcompany INT NOT NULL,
    ejercicio INT NOT NULL,
    idciclo INT NOT NULL,
    idfase INT NOT NULL,
    idcurrency INT NOT NULL,
    idepigrafe INT NOT NULL,
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
    mes13 NUMERIC(18,5) NOT NULL DEFAULT 0
);

-- DATA_Comentarios
CREATE TABLE data_comentarios (
    id SERIAL PRIMARY KEY,
    idapicarga INT NOT NULL,
    guidcarga UUID NOT NULL,
    fechaultmodif TIMESTAMP DEFAULT NOW(),
    idcompany INT NOT NULL,
    ejercicio INT NOT NULL,
    idciclo INT NOT NULL,
    idfase INT NOT NULL,
    idepigrafe INT NOT NULL,
    etiqueta VARCHAR(50) NOT NULL,
    comentario TEXT
);

-- DATA_Tipo_Cambio
CREATE TABLE data_tipo_cambio (
    id SERIAL PRIMARY KEY,
    idapicarga INT NOT NULL,
    guidcarga UUID NOT NULL,
    fechaultmodif TIMESTAMP DEFAULT NOW(),
    ejercicio INT NOT NULL,
    idcurrency INT NOT NULL,
    calendarday DATE NOT NULL,
    mes INT NOT NULL,
    p NUMERIC(18,5),
    fc NUMERIC(18,5),
    fb NUMERIC(18,5)
);

-- ==================================================
-- TABLAS DE STAGING (STG_DATA_*)
-- ==================================================

-- STG_DATA_Actuals
CREATE TABLE stg_data_actuals (
    id SERIAL PRIMARY KEY,
    idapicarga INT NOT NULL,
    guidcarga UUID NOT NULL,
    fechaultmodif TIMESTAMP DEFAULT NOW(),
    idcompany INT NOT NULL,
    ejercicio INT NOT NULL,
    idciclo INT NOT NULL,
    idfase INT NOT NULL,
    idcurrency INT NOT NULL,
    idepigrafe INT NOT NULL,
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
    mes13 NUMERIC(18,5) NOT NULL DEFAULT 0
);

-- STG_DATA_Budget
CREATE TABLE stg_data_budget (
    id SERIAL PRIMARY KEY,
    idapicarga INT NOT NULL,
    guidcarga UUID NOT NULL,
    fechaultmodif TIMESTAMP DEFAULT NOW(),
    idcompany INT NOT NULL,
    ejercicio INT NOT NULL,
    idciclo INT NOT NULL,
    idfase INT NOT NULL,
    idcurrency INT NOT NULL,
    idepigrafe INT NOT NULL,
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
    mes13 NUMERIC(18,5) NOT NULL DEFAULT 0
);

-- STG_DATA_Forecast
CREATE TABLE stg_data_forecast (
    id SERIAL PRIMARY KEY,
    idapicarga INT NOT NULL,
    guidcarga UUID NOT NULL,
    fechaultmodif TIMESTAMP DEFAULT NOW(),
    idcompany INT NOT NULL,
    ejercicio INT NOT NULL,
    idciclo INT NOT NULL,
    idfase INT NOT NULL,
    idcurrency INT NOT NULL,
    idepigrafe INT NOT NULL,
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
    mes13 NUMERIC(18,5) NOT NULL DEFAULT 0
);

-- STG_DATA_Comentarios
CREATE TABLE stg_data_comentarios (
    id SERIAL PRIMARY KEY,
    idapicarga INT NOT NULL,
    guidcarga UUID NOT NULL,
    fechaultmodif TIMESTAMP DEFAULT NOW(),
    idcompany INT NOT NULL,
    ejercicio INT NOT NULL,
    idciclo INT NOT NULL,
    idfase INT NOT NULL,
    idepigrafe INT NOT NULL,
    etiqueta VARCHAR(50) NOT NULL,
    comentario TEXT
);

-- ==================================================
-- FUNCIONES UPSERT ENTRE STG_DATA_* Y DATA_*
-- ==================================================
-- Autor: Víctor
-- Fecha: 11/11/2025
-- Descripción: Adaptación PL/pgSQL de los procedimientos MSSQL
-- ==================================================

-- ==========================================
-- FUNCION: fn_upsert_data_actuals
-- ==========================================
CREATE OR REPLACE FUNCTION fn_upsert_data_actuals(p_guid UUID)
RETURNS VOID AS $$
DECLARE
    v_idapicarga INT;
    v_idcompany INT;
    v_ejercicio INT;
    v_idciclo INT;
    v_idfase INT;
    v_deleted INT;
    v_inserted INT;
BEGIN
    -- Seleccionamos la combinación principal
    SELECT idapicarga, idcompany, ejercicio, idciclo, idfase
    INTO v_idapicarga, v_idcompany, v_ejercicio, v_idciclo, v_idfase
    FROM stg_data_actuals
    WHERE guidcarga = p_guid
    LIMIT 1;

    -- Borramos datos existentes de la combinación
    DELETE FROM data_actuals
    WHERE idcompany = v_idcompany
      AND ejercicio = v_ejercicio
      AND idciclo = v_idciclo
      AND idfase = v_idfase
      AND idepigrafe IN (
          SELECT idepigrafe FROM stg_data_actuals WHERE guidcarga = p_guid
      );
    GET DIAGNOSTICS v_deleted = ROW_COUNT;

    -- Insertamos los nuevos datos
    INSERT INTO data_actuals (
        idapicarga, guidcarga, fechaultmodif,
        idcompany, ejercicio, idciclo, idfase,
        idcurrency, idepigrafe,
        mes00, mes01, mes02, mes03, mes04, mes05, mes06,
        mes07, mes08, mes09, mes10, mes11, mes12, mes13
    )
    SELECT
        v_idapicarga, p_guid, NOW(),
        v_idcompany, v_ejercicio, v_idciclo, v_idfase,
        idcurrency, idepigrafe,
        mes00, mes01, mes02, mes03, mes04, mes05, mes06,
        mes07, mes08, mes09, mes10, mes11, mes12, mes13
    FROM stg_data_actuals
    WHERE guidcarga = p_guid;
    GET DIAGNOSTICS v_inserted = ROW_COUNT;

    -- Borramos staging
    DELETE FROM stg_data_actuals WHERE guidcarga = p_guid;

    RAISE NOTICE 'Actuals → Filas eliminadas: %, Filas insertadas: %', v_deleted, v_inserted;
END;
$$ LANGUAGE plpgsql;

-- ==========================================
-- FUNCION: fn_upsert_data_budget
-- ==========================================
CREATE OR REPLACE FUNCTION fn_upsert_data_budget(p_guid UUID)
RETURNS VOID AS $$
DECLARE
    v_idapicarga INT;
    v_idcompany INT;
    v_ejercicio INT;
    v_idciclo INT;
    v_idfase INT;
    v_deleted INT;
    v_inserted INT;
BEGIN
    SELECT idapicarga, idcompany, ejercicio, idciclo, idfase
    INTO v_idapicarga, v_idcompany, v_ejercicio, v_idciclo, v_idfase
    FROM stg_data_budget
    WHERE guidcarga = p_guid
    LIMIT 1;

    DELETE FROM data_budget
    WHERE idcompany = v_idcompany
      AND ejercicio = v_ejercicio
      AND idciclo = v_idciclo
      AND idfase = v_idfase
      AND idepigrafe IN (
          SELECT idepigrafe FROM stg_data_budget WHERE guidcarga = p_guid
      );
    GET DIAGNOSTICS v_deleted = ROW_COUNT;

    INSERT INTO data_budget (
        idapicarga, guidcarga, fechaultmodif,
        idcompany, ejercicio, idciclo, idfase,
        idcurrency, idepigrafe,
        mes00, mes01, mes02, mes03, mes04, mes05, mes06,
        mes07, mes08, mes09, mes10, mes11, mes12, mes13
    )
    SELECT
        v_idapicarga, p_guid, NOW(),
        v_idcompany, v_ejercicio, v_idciclo, v_idfase,
        idcurrency, idepigrafe,
        mes00, mes01, mes02, mes03, mes04, mes05, mes06,
        mes07, mes08, mes09, mes10, mes11, mes12, mes13
    FROM stg_data_budget
    WHERE guidcarga = p_guid;
    GET DIAGNOSTICS v_inserted = ROW_COUNT;

    DELETE FROM stg_data_budget WHERE guidcarga = p_guid;

    RAISE NOTICE 'Budget → Filas eliminadas: %, Filas insertadas: %', v_deleted, v_inserted;
END;
$$ LANGUAGE plpgsql;

-- ==========================================
-- FUNCION: fn_upsert_data_forecast
-- ==========================================
CREATE OR REPLACE FUNCTION fn_upsert_data_forecast(p_guid UUID)
RETURNS VOID AS $$
DECLARE
    v_idapicarga INT;
    v_idcompany INT;
    v_ejercicio INT;
    v_idciclo INT;
    v_idfase INT;
    v_deleted INT;
    v_inserted INT;
BEGIN
    SELECT idapicarga, idcompany, ejercicio, idciclo, idfase
    INTO v_idapicarga, v_idcompany, v_ejercicio, v_idciclo, v_idfase
    FROM stg_data_forecast
    WHERE guidcarga = p_guid
    LIMIT 1;

    DELETE FROM data_forecast
    WHERE idcompany = v_idcompany
      AND ejercicio = v_ejercicio
      AND idciclo = v_idciclo
      AND idfase = v_idfase
      AND idepigrafe IN (
          SELECT idepigrafe FROM stg_data_forecast WHERE guidcarga = p_guid
      );
    GET DIAGNOSTICS v_deleted = ROW_COUNT;

    INSERT INTO data_forecast (
        idapicarga, guidcarga, fechaultmodif,
        idcompany, ejercicio, idciclo, idfase,
        idcurrency, idepigrafe,
        mes00, mes01, mes02, mes03, mes04, mes05, mes06,
        mes07, mes08, mes09, mes10, mes11, mes12, mes13
    )
    SELECT
        v_idapicarga, p_guid, NOW(),
        v_idcompany, v_ejercicio, v_idciclo, v_idfase,
        idcurrency, idepigrafe,
        mes00, mes01, mes02, mes03, mes04, mes05, mes06,
        mes07, mes08, mes09, mes10, mes11, mes12, mes13
    FROM stg_data_forecast
    WHERE guidcarga = p_guid;
    GET DIAGNOSTICS v_inserted = ROW_COUNT;

    DELETE FROM stg_data_forecast WHERE guidcarga = p_guid;

    RAISE NOTICE 'Forecast → Filas eliminadas: %, Filas insertadas: %', v_deleted, v_inserted;
END;
$$ LANGUAGE plpgsql;

-- ==========================================
-- FUNCION: fn_upsert_data_comentarios
-- ==========================================
CREATE OR REPLACE FUNCTION fn_upsert_data_comentarios(p_guid UUID)
RETURNS VOID AS $$
DECLARE
    v_idapicarga INT;
    v_idcompany INT;
    v_ejercicio INT;
    v_idciclo INT;
    v_idfase INT;
    v_deleted INT;
    v_inserted INT;
BEGIN
    SELECT idapicarga, idcompany, ejercicio, idciclo, idfase
    INTO v_idapicarga, v_idcompany, v_ejercicio, v_idciclo, v_idfase
    FROM stg_data_comentarios
    WHERE guidcarga = p_guid
    LIMIT 1;

    DELETE FROM data_comentarios
    WHERE idcompany = v_idcompany
      AND ejercicio = v_ejercicio
      AND idciclo = v_idciclo
      AND idfase = v_idfase
      AND etiqueta IN (
          SELECT etiqueta FROM stg_data_comentarios WHERE guidcarga = p_guid
      );
    GET DIAGNOSTICS v_deleted = ROW_COUNT;

    INSERT INTO data_comentarios (
        idapicarga, guidcarga, fechaultmodif,
        idcompany, ejercicio, idciclo, idfase, idepigrafe,
        etiqueta, comentario
    )
    SELECT
        v_idapicarga, p_guid, NOW(),
        v_idcompany, v_ejercicio, v_idciclo, v_idfase,
        idepigrafe, etiqueta, comentario
    FROM stg_data_comentarios
    WHERE guidcarga = p_guid;
    GET DIAGNOSTICS v_inserted = ROW_COUNT;

    DELETE FROM stg_data_comentarios WHERE guidcarga = p_guid;

    RAISE NOTICE 'Comentarios → Filas eliminadas: %, Filas insertadas: %', v_deleted, v_inserted;
END;
$$ LANGUAGE plpgsql;

